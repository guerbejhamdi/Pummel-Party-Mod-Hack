using System;
using ZP.Net;

// Token: 0x020001A1 RID: 417
public class DefaultController : MinigameController
{
	// Token: 0x06000BE7 RID: 3047 RVA: 0x0000B7DF File Offset: 0x000099DF
	public override void InitializeMinigame()
	{
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("DefaultPlayer", null);
		}
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x0000B7F4 File Offset: 0x000099F4
	public override void StartMinigame()
	{
		base.StartMinigame();
	}

	// Token: 0x06000BE9 RID: 3049 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}
}
