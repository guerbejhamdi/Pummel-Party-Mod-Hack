using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001F4 RID: 500
public class PassTheBombController : MinigameController
{
	// Token: 0x06000E88 RID: 3720 RVA: 0x00073F80 File Offset: 0x00072180
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

	// Token: 0x06000E89 RID: 3721 RVA: 0x00074138 File Offset: 0x00072338
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.rand = new System.Random(UnityEngine.Random.Range(0, int.MaxValue));
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("PassTheBombPlayer", null);
		}
		this.triangulation = NavMesh.CalculateTriangulation();
		this.SetBombTimers(new float[]
		{
			ZPMath.RandomFloat(this.rand, this.bombExplodeIntervalMin, this.bombExplodeIntervalMax),
			ZPMath.RandomFloat(this.rand, this.bombExplodeIntervalMin, this.bombExplodeIntervalMax),
			ZPMath.RandomFloat(this.rand, this.bombExplodeIntervalMin, this.bombExplodeIntervalMax),
			ZPMath.RandomFloat(this.rand, this.bombExplodeIntervalMin, this.bombExplodeIntervalMax),
			ZPMath.RandomFloat(this.rand, this.bombExplodeIntervalMin, this.bombExplodeIntervalMax),
			ZPMath.RandomFloat(this.rand, this.bombExplodeIntervalMin, this.bombExplodeIntervalMax),
			ZPMath.RandomFloat(this.rand, this.bombExplodeIntervalMin, this.bombExplodeIntervalMax),
			ZPMath.RandomFloat(this.rand, this.bombExplodeIntervalMin, this.bombExplodeIntervalMax)
		});
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x0000C5CB File Offset: 0x0000A7CB
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x0007425C File Offset: 0x0007245C
	public override void RoundEnded()
	{
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < this.players.Count; i++)
			{
				if (!((PassTheBombPlayer)this.players[i]).IsDead)
				{
					CharacterBase characterBase = this.players[i];
					characterBase.Score += (short)((this.players.Count - 1) * 25);
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

	// Token: 0x06000E8C RID: 3724 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x0000CC9F File Offset: 0x0000AE9F
	public override void StartNewRound()
	{
		this.lastBombExplodeTime = Time.time;
		if (NetSystem.IsServer)
		{
			((PassTheBombPlayer)this.players[UnityEngine.Random.Range(1, GameManager.GetPlayerCount())]).HoldingBomb = true;
		}
		base.StartNewRound();
	}

	// Token: 0x06000E8F RID: 3727 RVA: 0x0007431C File Offset: 0x0007251C
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			if (Time.time - this.lastBombExplodeTime > this.bombTimers[this.intervalIndex])
			{
				if (NetSystem.IsServer)
				{
					int i = 0;
					while (i < this.players.Count)
					{
						PassTheBombPlayer passTheBombPlayer = (PassTheBombPlayer)this.players[i];
						if (!passTheBombPlayer.IsDead && passTheBombPlayer.HoldingBomb)
						{
							passTheBombPlayer.KillPlayer(true);
							if (this.players_alive >= 2)
							{
								float num = float.MaxValue;
								int index = 0;
								for (int j = 1; j < this.players.Count; j++)
								{
									PassTheBombPlayer passTheBombPlayer2 = (PassTheBombPlayer)this.players[j];
									if (j != i && !passTheBombPlayer2.IsDead)
									{
										float num2 = Vector3.Distance(passTheBombPlayer2.transform.position, passTheBombPlayer.transform.position);
										if (num2 < num)
										{
											num = num2;
											index = j;
										}
									}
								}
								((PassTheBombPlayer)this.players[index]).HoldingBomb = true;
								break;
							}
							break;
						}
						else
						{
							i++;
						}
					}
				}
				this.intervalIndex++;
				this.lastBombExplodeTime = Time.time;
			}
			if (Time.time - this.last_beep_time > this.beep_interval)
			{
				AudioSystem.PlayOneShot(this.bomb_beep, 1f, 0f, 1f);
				this.beep_interval = (30f - (Time.time - this.lastBombExplodeTime)) / 30f * 2f;
				this.beep_interval = Mathf.Clamp(this.beep_interval, 0.1f, float.MaxValue);
				this.last_beep_time = Time.time;
			}
			if (NetSystem.IsServer && base.State == MinigameControllerState.Playing && this.players_alive <= 1)
			{
				Debug.Log("Ending Round");
				base.EndRound(2.5f, 2f, false);
			}
		}
	}

	// Token: 0x06000E90 RID: 3728 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000E91 RID: 3729 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x0000A7A0 File Offset: 0x000089A0
	public void PlayerDied(PassTheBombPlayer player)
	{
		if (NetSystem.IsServer)
		{
			player.RoundScore = (short)(this.players.Count - this.players_alive);
		}
		this.players_alive--;
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x0000CCDA File Offset: 0x0000AEDA
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSetBombTimers(NetPlayer sender, float[] times)
	{
		this.SetBombTimers(times);
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x0000CCE3 File Offset: 0x0000AEE3
	public void SetBombTimers(float[] times)
	{
		this.bombTimers = times;
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSetBombTimers", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				times
			});
		}
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x00074500 File Offset: 0x00072700
	public int GetBombTimer()
	{
		float num = Time.time - this.lastBombExplodeTime;
		if (num > this.bombTimers[this.intervalIndex])
		{
			num = 0f;
		}
		return Mathf.RoundToInt(num);
	}

	// Token: 0x06000E96 RID: 3734 RVA: 0x00074538 File Offset: 0x00072738
	public PassTheBombPlayer GetBombPlayer()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			PassTheBombPlayer passTheBombPlayer = (PassTheBombPlayer)this.players[i];
			if (passTheBombPlayer.HoldingBomb)
			{
				return passTheBombPlayer;
			}
		}
		return (PassTheBombPlayer)this.players[0];
	}

	// Token: 0x04000E16 RID: 3606
	[Header("Minigame specific attributes")]
	public AudioClip bomb_beep;

	// Token: 0x04000E17 RID: 3607
	private System.Random rand;

	// Token: 0x04000E18 RID: 3608
	private NavMeshTriangulation triangulation;

	// Token: 0x04000E19 RID: 3609
	private BinaryTree binaryTree;

	// Token: 0x04000E1A RID: 3610
	private float totalArea;

	// Token: 0x04000E1B RID: 3611
	private float lastBombExplodeTime;

	// Token: 0x04000E1C RID: 3612
	private int intervalIndex;

	// Token: 0x04000E1D RID: 3613
	private float[] bombTimers;

	// Token: 0x04000E1E RID: 3614
	private float bombExplodeIntervalMin = 27f;

	// Token: 0x04000E1F RID: 3615
	private float bombExplodeIntervalMax = 33f;

	// Token: 0x04000E20 RID: 3616
	private float last_beep_time;

	// Token: 0x04000E21 RID: 3617
	private float beep_interval = 2f;
}
