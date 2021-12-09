using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x02000234 RID: 564
public class SelfishStrideController : MinigameController
{
	// Token: 0x06001055 RID: 4181 RVA: 0x00080414 File Offset: 0x0007E614
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("SelfishStridePlayer", null);
		}
		int playerCount = GameManager.GetPlayerCount();
		int num = this.bridgeRotations[playerCount].Length;
		for (int i = 0; i < num; i++)
		{
			this.bridges.Add(base.Spawn(this.bridgePrefab, Vector3.zero, Quaternion.Euler(0f, this.bridgeRotations[playerCount][i], 0f)).GetComponent<SelfishStrideBridge>());
		}
	}

	// Token: 0x06001056 RID: 4182 RVA: 0x0000C5CB File Offset: 0x0000A7CB
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x06001057 RID: 4183 RVA: 0x00080494 File Offset: 0x0007E694
	public override void ResetRound()
	{
		base.ResetRound();
		for (int i = 0; i < this.bridges.Count; i++)
		{
			this.bridges[i].FixBridge();
		}
		for (int j = 0; j < this.keys.Count; j++)
		{
			if (this.keys[j] != null)
			{
				UnityEngine.Object.Destroy(this.keys[j]);
			}
		}
		this.keys.Clear();
	}

	// Token: 0x06001058 RID: 4184 RVA: 0x00080514 File Offset: 0x0007E714
	public override void StartNewRound()
	{
		base.StartNewRound();
		if (this.roundCoroutine != null)
		{
			base.StopCoroutine(this.roundCoroutine);
		}
		this.roundCoroutine = base.StartCoroutine(this.DoRound());
		if (NetSystem.IsServer)
		{
			this.SpawnKeys(GameManager.rand.Next());
		}
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x0000DC19 File Offset: 0x0000BE19
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawnKeys(NetPlayer sender, int seed)
	{
		this.SpawnKeys(seed);
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x00080564 File Offset: 0x0007E764
	private void SpawnKeys(int seed)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSpawnKeys", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				seed
			});
		}
		System.Random random = new System.Random(seed);
		List<int> list = new List<int>();
		for (int i = 0; i < this.bridges.Count; i++)
		{
			list.Add(i);
		}
		int num = 0;
		while (list.Count != 0)
		{
			int index = random.Next(0, list.Count);
			if (num == 0)
			{
				this.bridges[list[index]].KeyCount = 5;
			}
			else if (num == 1 || num == 2)
			{
				this.bridges[list[index]].KeyCount = 3;
			}
			else
			{
				this.bridges[list[index]].KeyCount = 1;
			}
			list.RemoveAt(index);
			num++;
		}
		for (int j = 0; j < this.bridges.Count; j++)
		{
			switch (this.bridges[j].KeyCount)
			{
			case 1:
			{
				GameObject gameObject = base.Spawn(this.keyPrefab, this.bridges[j].oneKeySpawnPosition.position, Quaternion.identity);
				gameObject.GetComponent<SelfishStrideKey>().bronze.SetActive(true);
				this.keys.Add(gameObject);
				break;
			}
			case 3:
				for (int k = 0; k < 3; k++)
				{
					GameObject gameObject2 = base.Spawn(this.keyPrefab, this.bridges[j].threeKeySpawnPositions[k].position, Quaternion.identity);
					gameObject2.GetComponent<SelfishStrideKey>().silver.SetActive(true);
					this.keys.Add(gameObject2);
				}
				break;
			case 5:
				for (int l = 0; l < 5; l++)
				{
					GameObject gameObject3 = base.Spawn(this.keyPrefab, this.bridges[j].fiveKeySpawnPositions[l].position, Quaternion.identity);
					gameObject3.GetComponent<SelfishStrideKey>().gold.SetActive(true);
					this.keys.Add(gameObject3);
				}
				break;
			}
		}
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x0000DC22 File Offset: 0x0000BE22
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			SelfishStrideController.SelfishStrideState selfishStrideState = this.curState;
		}
	}

	// Token: 0x0600105C RID: 4188 RVA: 0x0000DC3B File Offset: 0x0000BE3B
	private IEnumerator DoRound()
	{
		this.curState = SelfishStrideController.SelfishStrideState.GettingDirectionChoice;
		int num;
		for (int i = 10; i > 0; i = num)
		{
			GameManager.UIController.ShowLargeText(i.ToString(), LargeTextType.PlayerWins, 0.9f, true, false);
			AudioSystem.PlayOneShot(this.clockTick, 1f, 0f, 1f);
			yield return new WaitForSeconds(1f);
			num = i - 1;
		}
		GameManager.UIController.ShowLargeText("0", LargeTextType.PlayerWins, 0.9f, true, false);
		AudioSystem.PlayOneShot(this.clockTick, 1f, 0f, 1f);
		this.curState = SelfishStrideController.SelfishStrideState.DelayBeforeMovement;
		yield return new WaitForSeconds(1f);
		int[] counts = new int[this.bridges.Count];
		for (int j = 0; j < GameManager.GetPlayerCount(); j++)
		{
			SelfishStridePlayer selfishStridePlayer = (SelfishStridePlayer)base.GetPlayerInSlot((short)j);
			selfishStridePlayer.Subtarget = counts[(int)selfishStridePlayer.target.Value];
			counts[(int)selfishStridePlayer.target.Value]++;
		}
		for (int k = 0; k < GameManager.GetPlayerCount(); k++)
		{
			SelfishStridePlayer selfishStridePlayer2 = (SelfishStridePlayer)base.GetPlayerInSlot((short)k);
			selfishStridePlayer2.IsAlone = (counts[(int)selfishStridePlayer2.target.Value] == 1);
		}
		this.curState = SelfishStrideController.SelfishStrideState.MovingToBridge;
		yield return new WaitForSeconds(2.25f);
		this.curState = SelfishStrideController.SelfishStrideState.CrossingBridge;
		yield return new WaitForSeconds(0.25f);
		for (int l = 0; l < this.bridges.Count; l++)
		{
			if (counts[l] > 1)
			{
				this.bridges[l].BreakBridge();
			}
		}
		yield return new WaitForSeconds(2.5f);
		if (NetSystem.IsServer)
		{
			base.EndRound(0f, 1f, false);
		}
		yield break;
	}

	// Token: 0x040010B3 RID: 4275
	[Header("Selfish Stride Variables")]
	public AudioClip clockTick;

	// Token: 0x040010B4 RID: 4276
	public GameObject bridgePrefab;

	// Token: 0x040010B5 RID: 4277
	public GameObject keyPrefab;

	// Token: 0x040010B6 RID: 4278
	public List<SelfishStrideBridge> bridges = new List<SelfishStrideBridge>();

	// Token: 0x040010B7 RID: 4279
	public SelfishStrideController.SelfishStrideState curState;

	// Token: 0x040010B8 RID: 4280
	private int curRound;

	// Token: 0x040010B9 RID: 4281
	private Coroutine roundCoroutine;

	// Token: 0x040010BA RID: 4282
	private List<GameObject> keys = new List<GameObject>();

	// Token: 0x040010BB RID: 4283
	private float[][] bridgeRotations = new float[][]
	{
		new float[0],
		new float[0],
		new float[]
		{
			0f,
			90f,
			180f
		},
		new float[]
		{
			0f,
			90f,
			180f,
			270f
		},
		new float[]
		{
			45f,
			90f,
			135f,
			225f,
			315f
		},
		new float[]
		{
			0f,
			45f,
			90f,
			135f,
			180f,
			270f
		},
		new float[]
		{
			0f,
			45f,
			90f,
			135f,
			180f,
			225f,
			270f
		},
		new float[]
		{
			0f,
			45f,
			90f,
			135f,
			180f,
			225f,
			270f,
			315f
		},
		new float[]
		{
			0f,
			45f,
			90f,
			135f,
			180f,
			225f,
			270f,
			315f
		}
	};

	// Token: 0x02000235 RID: 565
	public enum SelfishStrideState
	{
		// Token: 0x040010BD RID: 4285
		FinishedRound,
		// Token: 0x040010BE RID: 4286
		GettingDirectionChoice,
		// Token: 0x040010BF RID: 4287
		DelayBeforeMovement,
		// Token: 0x040010C0 RID: 4288
		MovingToBridge,
		// Token: 0x040010C1 RID: 4289
		CrossingBridge
	}
}
