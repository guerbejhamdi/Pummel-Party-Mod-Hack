using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x02000202 RID: 514
public class PongController : MinigameController
{
	// Token: 0x06000F20 RID: 3872 RVA: 0x0000D17B File Offset: 0x0000B37B
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.m_ballPfb = Resources.Load<GameObject>("PongBall");
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("PongPlayer", null);
		}
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x0000BCDB File Offset: 0x00009EDB
	public override void OnPlayersReady()
	{
		base.OnPlayersReady();
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x00078AEC File Offset: 0x00076CEC
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		this.m_nextBallSpawn = this.ui_timer.time_test - 5f;
		if (NetSystem.IsServer)
		{
			this.SpawnBall();
		}
		base.StartMinigame();
	}

	// Token: 0x06000F23 RID: 3875 RVA: 0x00078B5C File Offset: 0x00076D5C
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			if (this.ui_timer.time_test <= 0f)
			{
				base.EndRound(1f, 1f, false);
			}
			if (this.ui_timer.time_test <= this.m_nextBallSpawn)
			{
				this.SpawnBall();
				this.m_nextBallSpawn = this.ui_timer.time_test - 15f;
			}
		}
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x00078BCC File Offset: 0x00076DCC
	private void SpawnBall()
	{
		PongBall component = base.NetSpawn("PongBall", new Vector3(0f, 0.5f, 0f), Quaternion.identity, 0, null).GetComponent<PongBall>();
		Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
		onUnitSphere.y = 0f;
		component.Init(onUnitSphere.normalized * 7f);
		this.m_activeBalls.Add(component);
	}

	// Token: 0x06000F25 RID: 3877 RVA: 0x0000D1A6 File Offset: 0x0000B3A6
	public List<PongBall> GetActiveBalls()
	{
		return this.m_activeBalls;
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x0000D1AE File Offset: 0x0000B3AE
	public void AddBall(PongBall b)
	{
		this.m_activeBalls.Add(b);
	}

	// Token: 0x06000F27 RID: 3879 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000F28 RID: 3880 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x06000F29 RID: 3881 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000F2A RID: 3882 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x04000EEA RID: 3818
	private GameObject m_ballPfb;

	// Token: 0x04000EEB RID: 3819
	private float m_nextBallSpawn;

	// Token: 0x04000EEC RID: 3820
	private List<PongBall> m_activeBalls = new List<PongBall>();
}
