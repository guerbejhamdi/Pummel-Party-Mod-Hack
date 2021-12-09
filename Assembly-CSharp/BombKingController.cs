using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000154 RID: 340
public class BombKingController : MinigameController
{
	// Token: 0x060009C0 RID: 2496 RVA: 0x000573F4 File Offset: 0x000555F4
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

	// Token: 0x060009C1 RID: 2497 RVA: 0x0000A734 File Offset: 0x00008934
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.rand = new System.Random(UnityEngine.Random.Range(0, int.MaxValue));
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("BombKingPlayer", null);
		}
		this.triangulation = NavMesh.CalculateTriangulation();
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x000575AC File Offset: 0x000557AC
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		for (int i = 0; i < this.ui_score.Length; i++)
		{
			if (!(this.ui_score[i] == null))
			{
				this.ui_score[i].scoreUpdateSpeed = 100;
				this.ui_score[i].minChangeText = 25;
			}
		}
		base.StartMinigame();
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x0000A770 File Offset: 0x00008970
	public override void StartNewRound()
	{
		if (NetSystem.IsServer)
		{
			((BombKingPlayer)this.players[UnityEngine.Random.Range(1, GameManager.GetPlayerCount())]).HoldingCrown = true;
		}
		base.StartNewRound();
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x0000A1F9 File Offset: 0x000083F9
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer && this.ui_timer.time_test <= 0f)
		{
			base.EndRound(1f, 1f, false);
		}
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x0000A7A0 File Offset: 0x000089A0
	public void PlayerDied(BombKingPlayer player)
	{
		if (NetSystem.IsServer)
		{
			player.RoundScore = (short)(this.players.Count - this.players_alive);
		}
		this.players_alive--;
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x00057638 File Offset: 0x00055838
	public BombKingPlayer GetBombPlayer()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			BombKingPlayer bombKingPlayer = (BombKingPlayer)this.players[i];
			if (bombKingPlayer.HoldingCrown)
			{
				return bombKingPlayer;
			}
		}
		return (BombKingPlayer)this.players[0];
	}

	// Token: 0x0400088C RID: 2188
	[Header("Minigame specific attributes")]
	private System.Random rand;

	// Token: 0x0400088D RID: 2189
	private NavMeshTriangulation triangulation;

	// Token: 0x0400088E RID: 2190
	private BinaryTree binaryTree;

	// Token: 0x0400088F RID: 2191
	private float totalArea;
}
