using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;

// Token: 0x02000596 RID: 1430
public class WinterMazeController : MinigameController
{
	// Token: 0x06002522 RID: 9506 RVA: 0x000E0B44 File Offset: 0x000DED44
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("WinterMazePlayer", null);
		}
		this.navMeshSurface = base.Root.GetComponent<NavMeshSurface>();
		base.StartCoroutine(this.WaitTobuild());
		this.hedgeMazeGenerator = base.Root.GetComponentInChildren<HedgeMazeGenerator>();
		this.mazeKeys = new WinterMazeKey[this.hedgeMazeGenerator.width, this.hedgeMazeGenerator.height];
		float num = -((float)this.hedgeMazeGenerator.width / 2f) * this.hedgeMazeGenerator.gridSize;
		float num2 = (float)this.hedgeMazeGenerator.height / 2f * this.hedgeMazeGenerator.gridSize;
		for (int i = 0; i < this.hedgeMazeGenerator.width; i++)
		{
			for (int j = 0; j < this.hedgeMazeGenerator.height; j++)
			{
				Vector3 vector = new Vector3(num + (float)i * this.hedgeMazeGenerator.gridSize, 0f, num2 - (float)j * this.hedgeMazeGenerator.gridSize);
				vector += new Vector3(this.hedgeMazeGenerator.gridSize / 2f, 0f, -this.hedgeMazeGenerator.gridSize / 2f);
				vector.y = 1.25f;
				this.mazeKeys[i, j] = base.Spawn(this.mazeKeyPrefab, vector, Quaternion.Euler(0f, 90f, 0f)).GetComponent<WinterMazeKey>();
			}
		}
	}

	// Token: 0x06002523 RID: 9507 RVA: 0x0001AABB File Offset: 0x00018CBB
	private IEnumerator WaitTobuild()
	{
		yield return new WaitForSeconds(0.5f);
		this.navMeshSurface.BuildNavMesh();
		yield break;
	}

	// Token: 0x06002524 RID: 9508 RVA: 0x0000ACF2 File Offset: 0x00008EF2
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.StartMinigame();
	}

	// Token: 0x06002525 RID: 9509 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06002526 RID: 9510 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06002527 RID: 9511 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06002528 RID: 9512 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06002529 RID: 9513 RVA: 0x000E0CD4 File Offset: 0x000DEED4
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			this.count++;
			if (this.mazeChangeTimer.Elapsed(true))
			{
				int num = UnityEngine.Random.Range(0, int.MaxValue);
				this.BuildMaze(num);
				base.SendRPC("RPCRebuildMaze", NetRPCDelivery.RELIABLE_ORDERED, new object[]
				{
					num
				});
			}
			int num2 = 0;
			for (int i = 0; i < this.players.Count; i++)
			{
				if (((WinterMazePlayer)this.players[i]).IsFinished)
				{
					num2++;
				}
			}
			if (this.ui_timer.time_test <= 0f)
			{
				base.EndRound(1f, 1f, false);
			}
			for (int j = 0; j < this.mazeKeys.GetLength(0); j++)
			{
				for (int k = 0; k < this.mazeKeys.GetLength(1); k++)
				{
					if (!this.mazeKeys[j, k].Collected)
					{
						for (int l = 0; l < this.players.Count; l++)
						{
							if ((this.players[l].transform.position - this.mazeKeys[j, k].transform.position).sqrMagnitude < 3f)
							{
								this.mazeKeys[j, k].Collect(this.players[l]);
								base.SendRPC("RPCCollect", NetRPCDelivery.RELIABLE_ORDERED, new object[]
								{
									(byte)(j + k * this.mazeKeys.GetLength(0))
								});
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x0600252A RID: 9514 RVA: 0x0001AACA File Offset: 0x00018CCA
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCRebuildMaze(NetPlayer sender, int seed)
	{
		this.BuildMaze(seed);
	}

	// Token: 0x0600252B RID: 9515 RVA: 0x0001AAD3 File Offset: 0x00018CD3
	private void BuildMaze(int seed)
	{
		base.StartCoroutine(this.hedgeMazeGenerator.UpdateMaze(seed, false));
		base.StartCoroutine(this.WaitTobuild());
	}

	// Token: 0x0600252C RID: 9516 RVA: 0x000E0EA0 File Offset: 0x000DF0A0
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCCollect(NetPlayer sender, byte index)
	{
		int num = (int)(index % 12);
		int num2 = (int)(index / 12);
		if (this.mazeKeys[num, num2] != null)
		{
			this.mazeKeys[num, num2].Collect(null);
		}
	}

	// Token: 0x0600252D RID: 9517 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x0600252E RID: 9518 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x040028A1 RID: 10401
	public GameObject mazeKeyPrefab;

	// Token: 0x040028A2 RID: 10402
	private NavMeshSurface navMeshSurface;

	// Token: 0x040028A3 RID: 10403
	private HedgeMazeGenerator hedgeMazeGenerator;

	// Token: 0x040028A4 RID: 10404
	private WinterMazeKey[,] mazeKeys;

	// Token: 0x040028A5 RID: 10405
	private ActionTimer mazeChangeTimer = new ActionTimer(6.5f, 9f);

	// Token: 0x040028A6 RID: 10406
	private int count;
}
