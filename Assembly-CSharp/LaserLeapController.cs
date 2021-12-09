using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001C2 RID: 450
public class LaserLeapController : MinigameController
{
	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000CFC RID: 3324 RVA: 0x0000BF77 File Offset: 0x0000A177
	// (set) Token: 0x06000CFD RID: 3325 RVA: 0x0000BF7F File Offset: 0x0000A17F
	public int DeadPlayerCount { get; set; }

	// Token: 0x06000CFE RID: 3326 RVA: 0x0006B870 File Offset: 0x00069A70
	public Vector3 GetRandomNavMeshPoint()
	{
		if (this.binaryTree == null && this.triangulation.vertices.Length != 0)
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

	// Token: 0x06000CFF RID: 3327 RVA: 0x0006BA20 File Offset: 0x00069C20
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.triangulation = NavMesh.CalculateTriangulation();
		this.rand = new System.Random(UnityEngine.Random.Range(0, int.MaxValue));
		this.m_laserSpawner = UnityEngine.Object.FindObjectOfType<LaserSpawner>();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("LaserLeapPlayer", null);
		}
	}

	// Token: 0x06000D00 RID: 3328 RVA: 0x0000ACF2 File Offset: 0x00008EF2
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.StartMinigame();
	}

	// Token: 0x06000D01 RID: 3329 RVA: 0x0000BF88 File Offset: 0x0000A188
	public override void RoundEnded()
	{
		this.m_laserSpawner.Clear();
		base.RoundEnded();
	}

	// Token: 0x06000D02 RID: 3330 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000D03 RID: 3331 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000D04 RID: 3332 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000D05 RID: 3333 RVA: 0x0006BA74 File Offset: 0x00069C74
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			int num = 0;
			for (int i = 0; i < this.players.Count; i++)
			{
				if (((LaserLeapPlayer)this.players[i]).IsDead)
				{
					num++;
				}
			}
			if (num >= this.players.Count - 1 || this.ui_timer.time_test <= 0f)
			{
				base.EndRound(3f, 1f, false);
			}
			if (this.m_curSequence != null && Time.time - this.m_lastSequenceStart > this.m_curSequence.sequenceLength + 1f)
			{
				this.m_curSequence = null;
			}
			if (this.m_curSequence == null)
			{
				int level = 0;
				if (this.ui_timer.time_test <= 70f)
				{
					level = 1;
				}
				if (this.ui_timer.time_test <= 50f)
				{
					level = 2;
				}
				if (this.ui_timer.time_test <= 20f)
				{
					level = 3;
				}
				this.m_curSequence = this.m_laserSpawner.GetRandomSequence(level);
				this.m_lastSequenceStart = Time.time;
				this.SpawnLaserSequence(this.m_laserSpawner.GetSequenceIndex(this.m_curSequence));
			}
		}
	}

	// Token: 0x06000D06 RID: 3334 RVA: 0x0000BF9B File Offset: 0x0000A19B
	private void SpawnLaserSequence(int index)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSpawnLaserSequence", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				index
			});
		}
		this.m_laserSpawner.SpawnSequence(index);
	}

	// Token: 0x06000D07 RID: 3335 RVA: 0x0000BFCB File Offset: 0x0000A1CB
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCSpawnLaserSequence(NetPlayer sender, int index)
	{
		this.SpawnLaserSequence(index);
	}

	// Token: 0x06000D08 RID: 3336 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000D09 RID: 3337 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000D0A RID: 3338 RVA: 0x0006BBA8 File Offset: 0x00069DA8
	public override void BuildResults()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			int placement = ((LaserLeapPlayer)this.players[i]).Placement;
			int lives = ((LaserLeapPlayer)this.players[i]).Lives;
			this.players[i].Score = (short)(placement * 10 + lives);
		}
		base.BuildResults();
	}

	// Token: 0x04000C53 RID: 3155
	private System.Random rand;

	// Token: 0x04000C54 RID: 3156
	private NavMeshTriangulation triangulation;

	// Token: 0x04000C55 RID: 3157
	private BinaryTree binaryTree;

	// Token: 0x04000C56 RID: 3158
	private float totalArea;

	// Token: 0x04000C58 RID: 3160
	private LaserSpawner m_laserSpawner;

	// Token: 0x04000C59 RID: 3161
	private float m_lastSequenceStart;

	// Token: 0x04000C5A RID: 3162
	private LaserLeapSequence m_curSequence;

	// Token: 0x04000C5B RID: 3163
	private int count;
}
