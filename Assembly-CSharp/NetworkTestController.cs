using System;
using ZP.Net;

// Token: 0x020001EA RID: 490
public class NetworkTestController : MinigameController
{
	// Token: 0x06000E46 RID: 3654 RVA: 0x0000CADC File Offset: 0x0000ACDC
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("NetworkTestPlayerNew", null);
		}
	}

	// Token: 0x06000E47 RID: 3655 RVA: 0x0000B7F4 File Offset: 0x000099F4
	public override void StartMinigame()
	{
		base.StartMinigame();
	}

	// Token: 0x06000E48 RID: 3656 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000E49 RID: 3657 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000E4A RID: 3658 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000E4B RID: 3659 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}
}
