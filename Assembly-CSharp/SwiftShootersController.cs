using System;
using UnityEngine;
using ZP.Net;

// Token: 0x02000273 RID: 627
public class SwiftShootersController : MinigameController
{
	// Token: 0x06001237 RID: 4663 RVA: 0x0000EBE9 File Offset: 0x0000CDE9
	public void Awake()
	{
		this.FindTargetSpawners();
	}

	// Token: 0x06001238 RID: 4664 RVA: 0x0000EBF1 File Offset: 0x0000CDF1
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("SwiftShootersPlayer", null);
		}
		this.FindTargetSpawners();
	}

	// Token: 0x06001239 RID: 4665 RVA: 0x0000BCDB File Offset: 0x00009EDB
	public override void OnPlayersReady()
	{
		base.OnPlayersReady();
	}

	// Token: 0x0600123A RID: 4666 RVA: 0x0008CBE0 File Offset: 0x0008ADE0
	public override void StartMinigame()
	{
		this.FindTargetSpawners();
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.BottomLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x0600123B RID: 4667 RVA: 0x0000EC12 File Offset: 0x0000CE12
	private void FindTargetSpawners()
	{
		if (this.m_spawners == null || this.m_spawners.Length == 0)
		{
			this.m_spawners = UnityEngine.Object.FindObjectsOfType<SwiftShooterTargetSpawner>();
		}
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x0008CC34 File Offset: 0x0008AE34
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			if (this.m_curSequence != null && Time.time - this.m_sequenceStartTime >= this.m_curSequence.totalActiveTime + 0.5f)
			{
				this.m_curSequence = null;
			}
			if (this.m_curSequence == null)
			{
				int num = UnityEngine.Random.Range(0, this.m_sequences.Length);
				int num2 = 64;
				int num3 = 0;
				while (num3 < num2 && num == this.m_lastSequence)
				{
					num = UnityEngine.Random.Range(0, this.m_sequences.Length);
				}
				this.m_lastSequence = num;
				this.SetSequence(num);
			}
			if (this.ui_timer.time_test <= 0f)
			{
				base.EndRound(1f, 1f, false);
			}
		}
	}

	// Token: 0x0600123D RID: 4669 RVA: 0x0008CCF0 File Offset: 0x0008AEF0
	public void SetSequence(int index)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSetSequence", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				(short)index
			});
		}
		this.FindTargetSpawners();
		this.m_curSequence = this.m_sequences[index];
		this.m_sequenceStartTime = Time.time;
		foreach (SwiftShooterTargetSpawner swiftShooterTargetSpawner in this.m_spawners)
		{
			if (swiftShooterTargetSpawner.PlayerIndex < GameManager.GetPlayerCount())
			{
				swiftShooterTargetSpawner.StartTargetSequence(this.m_curSequence);
			}
		}
	}

	// Token: 0x0600123E RID: 4670 RVA: 0x0000EC30 File Offset: 0x0000CE30
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSetSequence(NetPlayer sender, short index)
	{
		this.SetSequence((int)index);
	}

	// Token: 0x0600123F RID: 4671 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x06001241 RID: 4673 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06001242 RID: 4674 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x0400131F RID: 4895
	[SerializeField]
	private TargetSequence[] m_sequences;

	// Token: 0x04001320 RID: 4896
	private SwiftShooterTargetSpawner[] m_spawners;

	// Token: 0x04001321 RID: 4897
	private TargetSequence m_curSequence;

	// Token: 0x04001322 RID: 4898
	private float m_sequenceStartTime;

	// Token: 0x04001323 RID: 4899
	private bool m_presentsDestroyed;

	// Token: 0x04001324 RID: 4900
	private int m_lastSequence = -1;
}
