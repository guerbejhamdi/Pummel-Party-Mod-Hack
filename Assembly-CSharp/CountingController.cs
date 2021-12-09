using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200019A RID: 410
public class CountingController : MinigameController
{
	// Token: 0x06000BC7 RID: 3015 RVA: 0x00063C74 File Offset: 0x00061E74
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("CountingPlayer", null);
		}
		this.textObject = UnityEngine.Object.Instantiate<GameObject>(this.textPrefab);
		this.text = this.textObject.GetComponent<Text>();
		this.text.color = new Color(0f, 0f, 0f, 0f);
		this.textObject.transform.SetParent(GameManager.UIController.MinigameUIRoot, false);
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x00063CFC File Offset: 0x00061EFC
	public override void StartMinigame()
	{
		for (int i = 0; i < 4; i++)
		{
			MeshRenderer component = base.Root.transform.Find("World/Podiums/Podium" + (i + 1).ToString() + "/ScoreScreen").gameObject.GetComponent<MeshRenderer>();
			this.mats[i] = new Material(component.sharedMaterial);
			component.sharedMaterial = this.mats[i];
		}
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x00063D90 File Offset: 0x00061F90
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer && this.curState == CountingController.CountingMinigameState.FinishedRound)
		{
			if (this.curRound == 3)
			{
				base.EndRound(2.5f, 2f, false);
				return;
			}
			byte group = (byte)UnityEngine.Random.Range(0, this.groups.Length);
			int seed = UnityEngine.Random.Range(0, int.MaxValue);
			if (this.roundCoroutine != null)
			{
				base.StopCoroutine(this.roundCoroutine);
			}
			this.roundCoroutine = base.StartCoroutine(this.DoRound(group, seed));
		}
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x0000B666 File Offset: 0x00009866
	private IEnumerator Fade(bool fadeIn, float fadeTime)
	{
		Color color = new Color(1f, 1f, 1f, 0f);
		Color color2 = new Color(1f, 1f, 1f, 1f);
		Color startColor = fadeIn ? color : color2;
		Color endColor = fadeIn ? color2 : color;
		float startTime = Time.time;
		yield return new WaitUntil(delegate()
		{
			float num = Time.time - startTime;
			if (num >= fadeTime)
			{
				if (this.text != null)
				{
					this.text.color = endColor;
				}
				return true;
			}
			if (this.text != null)
			{
				this.text.color = Color.Lerp(startColor, endColor, num / fadeTime);
			}
			return false;
		});
		yield break;
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x0000B683 File Offset: 0x00009883
	private IEnumerator DoRound(byte group, int seed)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCStartRound", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				group,
				seed
			});
		}
		this.curRound += 1;
		System.Random rand = new System.Random(seed);
		if (this.text != null)
		{
			this.text.text = "Count " + this.strings[this.groups[(int)group][0]];
		}
		int num = 0;
		List<byte> spawnList = new List<byte>();
		List<float> spawnTimes = new List<float>();
		byte[] counts = new byte[this.groups[(int)group].Length];
		for (int j = 0; j < counts.Length; j++)
		{
			counts[j] = (byte)rand.Next(this.groupSpawnMin[(int)group][j], this.groupSpawnMax[(int)group][j]);
			num += (int)counts[j];
			for (int k = 0; k < (int)counts[j]; k++)
			{
				spawnList.Add((byte)j);
				spawnTimes.Add(ZPMath.RandomFloat(rand, 0f, this.spawnTime));
			}
		}
		this.curCorrectCount = (int)counts[0];
		this.curState = CountingController.CountingMinigameState.DoingRound;
		yield return base.StartCoroutine(this.Fade(true, 1f));
		float startTime = Time.time;
		while (spawnList.Count > 0)
		{
			int l = 0;
			while (l < spawnList.Count)
			{
				if (Time.time - startTime > spawnTimes[l])
				{
					float x = ZPMath.RandomFloat(rand, this.minSpawnX, this.maxSpawnX);
					GameObject prefab = this.countingObjects[this.groups[(int)group][(int)spawnList[l]]];
					CountingObjectMover component = base.Spawn(prefab, new Vector3(x, this.spawnY, this.spawnZ), Quaternion.Euler(0f, 180f, 0f)).GetComponent<CountingObjectMover>();
					component.curSpeed = ZPMath.RandomFloat(rand, component.minSpeed, component.maxSpeed);
					spawnList.RemoveAt(l);
					spawnTimes.RemoveAt(l);
				}
				else
				{
					l++;
				}
			}
			yield return null;
		}
		yield return base.StartCoroutine(this.Fade(false, 1f));
		yield return new WaitForSeconds(3f);
		this.curState = CountingController.CountingMinigameState.ShowingScores;
		int[] placements = new int[GameManager.GetPlayerCount()];
		List<CountingPlayer> list = new List<CountingPlayer>();
		for (int m = 0; m < GameManager.GetPlayerCount(); m++)
		{
			list.Add((CountingPlayer)base.GetPlayerInSlot((short)m));
		}
		List<CountingPlayer> list2 = new List<CountingPlayer>();
		for (int n = 0; n < GameManager.GetPlayerCount(); n += list2.Count)
		{
			int num2 = int.MaxValue;
			list2.Clear();
			for (int num3 = 0; num3 < list.Count; num3++)
			{
				int num4 = Mathf.Abs((int)list[num3].guessCount.Value - this.curCorrectCount);
				if (num4 < num2)
				{
					list2.Clear();
					list2.Add(list[num3]);
					num2 = num4;
				}
				else if (num4 == num2)
				{
					list2.Add(list[num3]);
				}
			}
			for (int num5 = 0; num5 < list2.Count; num5++)
			{
				placements[(int)list2[num5].OwnerSlot] = n;
				list.Remove(list2[num5]);
			}
		}
		int num6;
		for (int i = 0; i < GameManager.GetPlayerCount(); i = num6)
		{
			for (int a = 0; a <= (int)((CountingPlayer)base.GetPlayer(i)).guessCount.Value; a = num6)
			{
				this.mats[i].SetInt("_AtlasIndex", a);
				this.mats[i].color = ((a == (int)counts[0]) ? this.correctColor : this.wrongColor);
				CountingPlayer countingPlayer = (CountingPlayer)base.GetPlayerInSlot((short)i);
				if (a == (int)countingPlayer.guessCount.Value)
				{
					if (a == (int)counts[0])
					{
						AudioSystem.PlayOneShot(this.correct, 1f, 0f, 1f);
					}
					else
					{
						AudioSystem.PlayOneShot(this.incorrect, 1f, 0f, 1f);
					}
					if (NetSystem.IsServer)
					{
						CountingPlayer countingPlayer2 = countingPlayer;
						countingPlayer2.Score += (short)(this.placementScores[placements[i]] + ((a == (int)counts[0]) ? 5 : 0));
					}
				}
				AudioSystem.PlayOneShot(this.countUp, 2f, 0f, 1f);
				yield return new WaitForSeconds(0.05f);
				num6 = a + 1;
			}
			yield return new WaitForSeconds(0.25f);
			num6 = i + 1;
		}
		yield return new WaitForSeconds(2f);
		for (int num7 = 0; num7 < GameManager.GetPlayerCount(); num7++)
		{
			if (this.mats[num7] != null)
			{
				this.mats[num7].SetInt("_AtlasIndex", 0);
				this.mats[num7].color = this.wrongColor;
			}
			this.ResetStats();
		}
		yield return null;
		this.curState = CountingController.CountingMinigameState.FinishedRound;
		yield break;
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x0000B6A0 File Offset: 0x000098A0
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCStartRound(NetPlayer sender, byte group, int seed)
	{
		if (this.roundCoroutine != null)
		{
			base.StopCoroutine(this.roundCoroutine);
		}
		this.roundCoroutine = base.StartCoroutine(this.DoRound(group, seed));
		this.ResetStats();
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x00063E14 File Offset: 0x00062014
	private void ResetStats()
	{
		for (int i = 0; i < GameManager.GetPlayerCount(); i++)
		{
			if (this.mats[i] != null)
			{
				this.mats[i].SetInt("_AtlasIndex", 0);
				this.mats[i].color = this.wrongColor;
			}
			CountingPlayer countingPlayer = (CountingPlayer)base.GetPlayerInSlot((short)i);
			if (countingPlayer.IsOwner)
			{
				countingPlayer.guessCount.Value = 0;
			}
		}
	}

	// Token: 0x04000AEB RID: 2795
	[Header("Counting Variables")]
	public GameObject[] countingObjects;

	// Token: 0x04000AEC RID: 2796
	public string[] strings;

	// Token: 0x04000AED RID: 2797
	public AudioClip countUp;

	// Token: 0x04000AEE RID: 2798
	public AudioClip correct;

	// Token: 0x04000AEF RID: 2799
	public AudioClip incorrect;

	// Token: 0x04000AF0 RID: 2800
	public GameObject textPrefab;

	// Token: 0x04000AF1 RID: 2801
	public Color wrongColor;

	// Token: 0x04000AF2 RID: 2802
	public Color correctColor;

	// Token: 0x04000AF3 RID: 2803
	private int[][] groups = new int[][]
	{
		new int[]
		{
			10,
			0,
			9
		},
		new int[]
		{
			11,
			5,
			3
		},
		new int[]
		{
			12,
			0,
			8
		},
		new int[]
		{
			13,
			1,
			6
		},
		new int[]
		{
			14,
			7,
			4
		},
		new int[]
		{
			15,
			3,
			4
		},
		new int[]
		{
			16,
			3,
			1
		},
		new int[]
		{
			17,
			9,
			6
		},
		new int[]
		{
			18,
			0,
			4
		}
	};

	// Token: 0x04000AF4 RID: 2804
	private int[][] groupSpawnMin = new int[][]
	{
		new int[]
		{
			5,
			10,
			3
		},
		new int[]
		{
			5,
			10,
			3
		},
		new int[]
		{
			5,
			10,
			3
		},
		new int[]
		{
			5,
			10,
			3
		},
		new int[]
		{
			5,
			10,
			3
		},
		new int[]
		{
			5,
			10,
			3
		},
		new int[]
		{
			5,
			10,
			3
		},
		new int[]
		{
			5,
			10,
			3
		},
		new int[]
		{
			5,
			10,
			3
		}
	};

	// Token: 0x04000AF5 RID: 2805
	private int[][] groupSpawnMax = new int[][]
	{
		new int[]
		{
			10,
			20,
			6
		},
		new int[]
		{
			10,
			20,
			6
		},
		new int[]
		{
			10,
			20,
			6
		},
		new int[]
		{
			10,
			20,
			6
		},
		new int[]
		{
			10,
			20,
			6
		},
		new int[]
		{
			10,
			20,
			6
		},
		new int[]
		{
			10,
			20,
			6
		},
		new int[]
		{
			10,
			20,
			6
		},
		new int[]
		{
			10,
			20,
			6
		}
	};

	// Token: 0x04000AF6 RID: 2806
	public byte curRound;

	// Token: 0x04000AF7 RID: 2807
	public CountingController.CountingMinigameState curState;

	// Token: 0x04000AF8 RID: 2808
	private Coroutine roundCoroutine;

	// Token: 0x04000AF9 RID: 2809
	private Material[] mats = new Material[4];

	// Token: 0x04000AFA RID: 2810
	private GameObject textObject;

	// Token: 0x04000AFB RID: 2811
	private Text text;

	// Token: 0x04000AFC RID: 2812
	private float spawnTime = 10f;

	// Token: 0x04000AFD RID: 2813
	private float minSpawnX = 3.5f;

	// Token: 0x04000AFE RID: 2814
	private float maxSpawnX = 8.5f;

	// Token: 0x04000AFF RID: 2815
	private float spawnY = 1f;

	// Token: 0x04000B00 RID: 2816
	private float spawnZ = 17.5f;

	// Token: 0x04000B01 RID: 2817
	public int curCorrectCount;

	// Token: 0x04000B02 RID: 2818
	private int[] placementScores = new int[]
	{
		10,
		5,
		2,
		0
	};

	// Token: 0x0200019B RID: 411
	public enum CountingMinigameState
	{
		// Token: 0x04000B04 RID: 2820
		FinishedRound,
		// Token: 0x04000B05 RID: 2821
		DoingRound,
		// Token: 0x04000B06 RID: 2822
		ShowingScores
	}
}
