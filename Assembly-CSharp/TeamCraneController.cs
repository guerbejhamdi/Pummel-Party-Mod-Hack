using System;
using UnityEngine;
using ZP.Net;

// Token: 0x02000279 RID: 633
public class TeamCraneController : MinigameController
{
	// Token: 0x06001277 RID: 4727 RVA: 0x0008F018 File Offset: 0x0008D218
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.rand = new System.Random(UnityEngine.Random.Range(0, int.MaxValue));
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("TeamCranePlayer", null);
		}
		this.m_shake = base.Root.GetComponentInChildren<CameraShake>();
	}

	// Token: 0x06001278 RID: 4728 RVA: 0x0000ACF2 File Offset: 0x00008EF2
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.StartMinigame();
	}

	// Token: 0x06001279 RID: 4729 RVA: 0x0005C444 File Offset: 0x0005A644
	public override void BuildResults()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			this.players[i].Score = (short)(((BoxDropPlayer)this.players[i]).Placement * 10);
		}
		base.BuildResults();
	}

	// Token: 0x0600127A RID: 4730 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x0600127D RID: 4733 RVA: 0x0000EE2B File Offset: 0x0000D02B
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer && this.ui_timer.time_test <= 0f)
		{
			base.EndRound(3f, 1f, false);
		}
	}

	// Token: 0x0600127E RID: 4734 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x0600127F RID: 4735 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x0400138C RID: 5004
	private System.Random rand;

	// Token: 0x0400138D RID: 5005
	private CameraShake m_shake;

	// Token: 0x0400138E RID: 5006
	private int count;

	// Token: 0x0400138F RID: 5007
	private float m_dropTimeElapsed = 5f;
}
