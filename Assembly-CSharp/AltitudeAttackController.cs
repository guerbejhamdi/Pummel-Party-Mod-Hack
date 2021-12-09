using System;
using UnityEngine;
using ZP.Net;

// Token: 0x0200013D RID: 317
public class AltitudeAttackController : MinigameController
{
	// Token: 0x06000913 RID: 2323 RVA: 0x0000A194 File Offset: 0x00008394
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("AltitudeAttackPlayer", null);
		}
		this.canvas = base.Root.GetComponentInChildren<Canvas>(true);
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x0000A1C1 File Offset: 0x000083C1
	public override void OnPlayersReady()
	{
		base.OnPlayersReady();
		this.canvas.enabled = true;
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x0000A1D5 File Offset: 0x000083D5
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.StartMinigame();
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x0000A1F9 File Offset: 0x000083F9
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer && this.ui_timer.time_test <= 0f)
		{
			base.EndRound(1f, 1f, false);
		}
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x04000786 RID: 1926
	private Canvas canvas;
}
