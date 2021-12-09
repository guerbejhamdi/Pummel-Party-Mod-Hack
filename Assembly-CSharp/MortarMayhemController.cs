using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001D9 RID: 473
public class MortarMayhemController : MinigameController
{
	// Token: 0x06000DAF RID: 3503 RVA: 0x0006ED94 File Offset: 0x0006CF94
	public Vector3 GetRandomNavMeshPoint()
	{
		if (this.binaryTree == null)
		{
			this.triangulation = NavMesh.CalculateTriangulation();
			if (this.triangulation.vertices.Length != 0)
			{
				List<float> list = new List<float>();
				for (int i = 0; i < this.triangulation.indices.Length / 3; i++)
				{
					int num = i * 3;
					Vector3 vector = this.triangulation.vertices[this.triangulation.indices[num]];
					Vector3 vector2 = this.triangulation.vertices[this.triangulation.indices[num + 1]];
					Vector3 vector3 = this.triangulation.vertices[this.triangulation.indices[num + 2]];
					float num2 = Vector3.Distance(vector, vector2);
					float num3 = Vector3.Distance(vector2, vector3);
					float num4 = Vector3.Distance(vector3, vector);
					float num5 = (num2 + num3 + num4) / 2f;
					float num6 = Mathf.Sqrt(num5 * (num5 - num2) * (num5 - num3) * (num5 - num4));
					list.Add(this.totalArea);
					this.totalArea += num6;
				}
				this.binaryTree = new BinaryTree(list.ToArray());
			}
		}
		if (this.binaryTree != null)
		{
			float p = ZPMath.RandomFloat(this.rand, 0f, this.totalArea);
			int num7 = this.binaryTree.FindPoint(p) * 3;
			Vector3[] vertices = this.triangulation.vertices;
			int[] indices = this.triangulation.indices;
			return ZPMath.RandomTrianglePoint(vertices[indices[num7]], vertices[indices[num7 + 1]], vertices[indices[num7 + 2]], this.rand);
		}
		return Vector3.zero;
	}

	// Token: 0x06000DB0 RID: 3504 RVA: 0x0000C58F File Offset: 0x0000A78F
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.rand = new System.Random(UnityEngine.Random.Range(0, int.MaxValue));
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("MortarMayhemPlayer", null);
		}
		this.triangulation = NavMesh.CalculateTriangulation();
	}

	// Token: 0x06000DB1 RID: 3505 RVA: 0x0000C5CB File Offset: 0x0000A7CB
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x06000DB2 RID: 3506 RVA: 0x0006EF4C File Offset: 0x0006D14C
	public override void BuildResults()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
		}
		base.BuildResults();
	}

	// Token: 0x06000DB3 RID: 3507 RVA: 0x0006EF78 File Offset: 0x0006D178
	public override void RoundEnded()
	{
		base.StopCoroutine(this.cycle);
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < this.players.Count; i++)
			{
				if (!((MortarMayhemPlayer)this.players[i]).IsDead)
				{
					CharacterBase characterBase = this.players[i];
					characterBase.Score += (short)(this.players.Count * 25);
				}
				else
				{
					CharacterBase characterBase2 = this.players[i];
					characterBase2.Score += this.players[i].RoundScore * 25;
					this.players[i].RoundScore = 0;
				}
			}
		}
		base.RoundEnded();
	}

	// Token: 0x06000DB4 RID: 3508 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000DB5 RID: 3509 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000DB7 RID: 3511 RVA: 0x0000C5EF File Offset: 0x0000A7EF
	public bool CanMove()
	{
		return this.mortarMayhemState == MortarMayhemController.MortarMayhemState.DoingPattern;
	}

	// Token: 0x06000DB8 RID: 3512 RVA: 0x0006F040 File Offset: 0x0006D240
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			if (this.players_alive <= 1 || (int)this.curRound >= this.roundMovements.Length)
			{
				Debug.Log("Ending Round");
				base.EndRound(2.5f, 2f, false);
				return;
			}
			if (this.mortarMayhemState == MortarMayhemController.MortarMayhemState.FinishedDoingPattern && (int)this.curRound < this.roundMovements.Length)
			{
				byte b = this.roundMovements[(int)this.curRound];
				byte[] array = new byte[(int)(b * 4)];
				int num = 0;
				byte[] array2 = new byte[4];
				byte[] array3 = new byte[4];
				for (int i = 0; i < 4; i++)
				{
					array2[i] = this.curX[i];
					array3[i] = this.curY[i];
				}
				for (int j = 0; j < (int)b; j++)
				{
					for (int k = 0; k < 4; k++)
					{
						byte b2;
						do
						{
							b2 = (byte)this.rand.Next(0, 4);
						}
						while ((b2 != 0 || array3[k] >= this.H - 1) && (b2 != 1 || array2[k] >= this.W - 1) && (b2 != 2 || array3[k] <= 0) && (b2 != 3 || array2[k] <= 0));
						array[num] = b2;
						switch (b2)
						{
						case 0:
						{
							byte[] array4 = array3;
							int num2 = k;
							array4[num2] += 1;
							break;
						}
						case 1:
						{
							byte[] array5 = array2;
							int num3 = k;
							array5[num3] += 1;
							break;
						}
						case 2:
						{
							byte[] array6 = array3;
							int num4 = k;
							array6[num4] -= 1;
							break;
						}
						case 3:
						{
							byte[] array7 = array2;
							int num5 = k;
							array7[num5] -= 1;
							break;
						}
						}
						num++;
					}
				}
				this.cycle = base.StartCoroutine(this.PatternCycle(this.curRound, array));
			}
		}
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x0000C5FA File Offset: 0x0000A7FA
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void PatternCycleRPC(NetPlayer player, byte round, byte[] patterns)
	{
		this.cycle = base.StartCoroutine(this.PatternCycle(round, patterns));
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x0000C610 File Offset: 0x0000A810
	public IEnumerator PatternCycle(byte round, byte[] patterns)
	{
		this.mortarMayhemState = MortarMayhemController.MortarMayhemState.ShowingPattern;
		if (NetSystem.IsServer)
		{
			base.SendRPC("PatternCycleRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				round,
				patterns
			});
		}
		int num2;
		for (int a = 0; a < (int)this.roundMovements[(int)round]; a = num2)
		{
			for (int i = 0; i < GameManager.GetPlayerCount(); i++)
			{
				if (!((MortarMayhemPlayer)base.GetPlayer(i)).IsDead)
				{
					int num = i + a * 4;
					Vector3 b = new Vector3(0f, 0f, 0f);
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.arrowPrefabs[0]);
					gameObject.transform.SetParent(GameManager.UIController.MinigameUIRoot, false);
					gameObject.transform.position = this.minigameCameras[0].WorldToScreenPoint(this.players[i].transform.position + b) + new Vector3(0f, 0f, 0f);
					gameObject.transform.rotation = Quaternion.Euler(0f, 0f, this.rotations[(int)patterns[num]]);
				}
			}
			yield return new WaitForSeconds(this.roundDisplayTimes[(int)this.curRound]);
			num2 = a + 1;
		}
		this.mortarMayhemState = MortarMayhemController.MortarMayhemState.DoingPattern;
		for (int a = 0; a < (int)this.roundMovements[(int)round]; a = num2)
		{
			for (int j = 0; j < GameManager.GetPlayerCount(); j++)
			{
				switch (patterns[j + a * 4])
				{
				case 0:
				{
					byte[] array = this.curY;
					int num3 = j;
					array[num3] += 1;
					break;
				}
				case 1:
				{
					byte[] array2 = this.curX;
					int num4 = j;
					array2[num4] += 1;
					break;
				}
				case 2:
				{
					byte[] array3 = this.curY;
					int num5 = j;
					array3[num5] -= 1;
					break;
				}
				case 3:
				{
					byte[] array4 = this.curX;
					int num6 = j;
					array4[num6] -= 1;
					break;
				}
				}
			}
			yield return new WaitForSeconds(1.25f);
			AudioSystem.PlayOneShot(this.explosion, 0.75f, 0f, 1f);
			for (int y = 0; y < (int)this.W; y = num2)
			{
				for (int k = 0; k < (int)this.H; k++)
				{
					for (int l = 0; l < GameManager.GetPlayerCount(); l++)
					{
						if (!((MortarMayhemPlayer)base.GetPlayer(l)).IsDead && ((int)this.curX[l] != k || (int)this.curY[l] != y))
						{
							Vector3 gridPos = this.GetGridPos(l, k, y);
							UnityEngine.Object.Instantiate<GameObject>(this.explosionPrefab, gridPos, Quaternion.identity);
						}
					}
				}
				yield return null;
				num2 = y + 1;
			}
			for (int m = 0; m < GameManager.GetPlayerCount(); m++)
			{
				MortarMayhemPlayer mortarMayhemPlayer = (MortarMayhemPlayer)base.GetPlayer(m);
				if (mortarMayhemPlayer.IsOwner && !mortarMayhemPlayer.IsDead)
				{
					Vector3 gridPos2 = this.GetGridPos(m, (int)this.curX[m], (int)this.curY[m]);
					if (Mathf.Abs(mortarMayhemPlayer.transform.position.x - gridPos2.x) > this.gridSpaceSize * 0.5f || Mathf.Abs(mortarMayhemPlayer.transform.position.z - gridPos2.z) > this.gridSpaceSize * 0.5f)
					{
						mortarMayhemPlayer.KillPlayer(true);
					}
				}
			}
			num2 = a + 1;
		}
		yield return new WaitForSeconds(0.5f);
		this.mortarMayhemState = MortarMayhemController.MortarMayhemState.FinishedDoingPattern;
		this.curRound += 1;
		yield break;
	}

	// Token: 0x06000DBB RID: 3515 RVA: 0x0006F204 File Offset: 0x0006D404
	public Vector3 GetGridPos(int a, int x, int y)
	{
		return this.basePositions[a] - new Vector3(this.gridSpaceSize * 2f, 0f, this.gridSpaceSize * 2f) + new Vector3((float)x * this.gridSpaceSize, 0f, (float)y * this.gridSpaceSize);
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x0000A7A0 File Offset: 0x000089A0
	public void PlayerDied(MortarMayhemPlayer player)
	{
		if (NetSystem.IsServer)
		{
			player.RoundScore = (short)(this.players.Count - this.players_alive);
		}
		this.players_alive--;
	}

	// Token: 0x06000DBD RID: 3517 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000DBE RID: 3518 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x04000D10 RID: 3344
	public GameObject[] arrowPrefabs;

	// Token: 0x04000D11 RID: 3345
	public AudioClip explosion;

	// Token: 0x04000D12 RID: 3346
	private System.Random rand;

	// Token: 0x04000D13 RID: 3347
	private NavMeshTriangulation triangulation;

	// Token: 0x04000D14 RID: 3348
	private BinaryTree binaryTree;

	// Token: 0x04000D15 RID: 3349
	private float totalArea;

	// Token: 0x04000D16 RID: 3350
	private NavMeshSurface m_surface;

	// Token: 0x04000D17 RID: 3351
	public MortarMayhemController.MortarMayhemState mortarMayhemState = MortarMayhemController.MortarMayhemState.FinishedDoingPattern;

	// Token: 0x04000D18 RID: 3352
	private byte W = 5;

	// Token: 0x04000D19 RID: 3353
	private byte H = 5;

	// Token: 0x04000D1A RID: 3354
	public byte[] curX = new byte[]
	{
		2,
		2,
		2,
		2
	};

	// Token: 0x04000D1B RID: 3355
	public byte[] curY = new byte[]
	{
		2,
		2,
		2,
		2
	};

	// Token: 0x04000D1C RID: 3356
	private byte curRound;

	// Token: 0x04000D1D RID: 3357
	private byte[] roundMovements = new byte[]
	{
		2,
		3,
		4,
		4,
		5,
		5,
		5,
		6,
		7,
		8
	};

	// Token: 0x04000D1E RID: 3358
	private float[] roundDisplayTimes = new float[]
	{
		1f,
		0.95f,
		0.9f,
		0.85f,
		0.8f,
		0.75f,
		0.7f,
		0.65f,
		0.6f,
		0.55f
	};

	// Token: 0x04000D1F RID: 3359
	private Coroutine cycle;

	// Token: 0x04000D20 RID: 3360
	public GameObject explosionPrefab;

	// Token: 0x04000D21 RID: 3361
	private Vector3[] basePositions = new Vector3[]
	{
		new Vector3(-5f, 1f, 5f),
		new Vector3(5f, 1f, 5f),
		new Vector3(-5f, 1f, -5f),
		new Vector3(5f, 1f, -5f)
	};

	// Token: 0x04000D22 RID: 3362
	private float gridSpaceSize = 1.8f;

	// Token: 0x04000D23 RID: 3363
	private float[] rotations = new float[]
	{
		180f,
		90f,
		0f,
		270f
	};

	// Token: 0x020001DA RID: 474
	public enum MortarMayhemState
	{
		// Token: 0x04000D25 RID: 3365
		ShowingPattern,
		// Token: 0x04000D26 RID: 3366
		DoingPattern,
		// Token: 0x04000D27 RID: 3367
		FinishedDoingPattern
	}
}
