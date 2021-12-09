using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001B8 RID: 440
public class IcebergController : MinigameController
{
	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06000CAF RID: 3247 RVA: 0x0000BD53 File Offset: 0x00009F53
	// (set) Token: 0x06000CB0 RID: 3248 RVA: 0x0000BD5B File Offset: 0x00009F5B
	public int FinishedPlayers { get; set; }

	// Token: 0x06000CB1 RID: 3249 RVA: 0x0006A1AC File Offset: 0x000683AC
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

	// Token: 0x06000CB2 RID: 3250 RVA: 0x0006A364 File Offset: 0x00068564
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.rand = new System.Random(UnityEngine.Random.Range(0, int.MaxValue));
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("IcebergPlayer", null);
		}
		this.triangulation = NavMesh.CalculateTriangulation();
		foreach (NavMeshSurface navMeshSurface in base.Root.GetComponentsInChildren<NavMeshSurface>())
		{
			if (!navMeshSurface.CompareTag("StaticNavMesh"))
			{
				this.m_surface = navMeshSurface;
				break;
			}
		}
		this.m_movers = base.Root.GetComponentsInChildren<IcebergMover>();
		this.m_fallingPlatforms = base.Root.GetComponentsInChildren<IcebergFallingPlatform>();
		IcebergFallingPlatform[] fallingPlatforms = this.m_fallingPlatforms;
		for (int i = 0; i < fallingPlatforms.Length; i++)
		{
			fallingPlatforms[i].Init(this);
		}
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x0006A420 File Offset: 0x00068620
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		this.ResetPlatforms();
		base.StartMinigame();
	}

	// Token: 0x06000CB4 RID: 3252 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000CB5 RID: 3253 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000CB6 RID: 3254 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000CB7 RID: 3255 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x0006A474 File Offset: 0x00068674
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			this.count++;
			if (this.m_surface != null && this.count > 4)
			{
				this.m_surface.UpdateNavMesh(this.m_surface.navMeshData);
				this.count = 0;
			}
			int num = 0;
			for (int i = 0; i < this.players.Count; i++)
			{
				if (((IcebergPlayer)this.players[i]).IsFinished)
				{
					num++;
				}
			}
			if (num == this.players.Count || this.ui_timer.time_test <= 0f)
			{
				base.EndRound(3f, 1f, false);
			}
		}
	}

	// Token: 0x06000CB9 RID: 3257 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x0000BD64 File Offset: 0x00009F64
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCResetPlatforms(NetPlayer sender)
	{
		this.ResetPlatforms();
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x0006A540 File Offset: 0x00068740
	protected void ResetPlatforms()
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCResetPlatforms", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		IcebergMover[] movers = this.m_movers;
		for (int i = 0; i < movers.Length; i++)
		{
			movers[i].Reset();
		}
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x0000BD6C File Offset: 0x00009F6C
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCPlatformFallClients(NetPlayer sender, int index)
	{
		this.PlatformFall(index, true);
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x0000BD6C File Offset: 0x00009F6C
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCPlatformFallServer(NetPlayer sender, int index)
	{
		this.PlatformFall(index, true);
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x0006A584 File Offset: 0x00068784
	public void PlatformFall(int index, bool rpc = false)
	{
		if (!rpc)
		{
			if (NetSystem.IsServer)
			{
				base.SendRPC("RPCPlatformFallClients", NetRPCDelivery.RELIABLE_ORDERED, new object[]
				{
					index
				});
			}
			else
			{
				base.SendRPC("RPCPlatformFallServer", NetRPCDelivery.RELIABLE_ORDERED, new object[]
				{
					index
				});
			}
		}
		foreach (IcebergFallingPlatform icebergFallingPlatform in this.m_fallingPlatforms)
		{
			if (icebergFallingPlatform.Index == index)
			{
				icebergFallingPlatform.Fall();
			}
		}
	}

	// Token: 0x04000C0C RID: 3084
	private System.Random rand;

	// Token: 0x04000C0D RID: 3085
	private NavMeshTriangulation triangulation;

	// Token: 0x04000C0E RID: 3086
	private BinaryTree binaryTree;

	// Token: 0x04000C0F RID: 3087
	private float totalArea;

	// Token: 0x04000C10 RID: 3088
	private IcebergMover[] m_movers;

	// Token: 0x04000C11 RID: 3089
	private IcebergFallingPlatform[] m_fallingPlatforms;

	// Token: 0x04000C13 RID: 3091
	private NavMeshSurface m_surface;

	// Token: 0x04000C14 RID: 3092
	private int count;
}
