using System;
using UnityEngine;
using ZP.Net;

// Token: 0x020001F0 RID: 496
public class PackAndPileController : MinigameController
{
	// Token: 0x06000E71 RID: 3697 RVA: 0x0000CC1D File Offset: 0x0000AE1D
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("PackAndPilePlayer", null);
		}
	}

	// Token: 0x06000E72 RID: 3698 RVA: 0x0000ACF2 File Offset: 0x00008EF2
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.StartMinigame();
	}

	// Token: 0x06000E73 RID: 3699 RVA: 0x00073654 File Offset: 0x00071854
	public override void RoundEnded()
	{
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < this.players.Count; i++)
			{
				if (!((PackAndPilePlayer)this.players[i]).finished)
				{
					CharacterBase characterBase = this.players[i];
					characterBase.Score += (short)((PackAndPilePlayer)this.players[i]).curY;
				}
			}
		}
		base.RoundEnded();
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x000736CC File Offset: 0x000718CC
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			int num = 0;
			for (int i = 0; i < this.players.Count; i++)
			{
				if (((PackAndPilePlayer)this.players[i]).finished)
				{
					num++;
				}
			}
			if (this.ui_timer.time_test <= 0f || num >= this.players.Count - 1)
			{
				base.EndRound(1f, 3f, false);
			}
		}
	}

	// Token: 0x04000DF5 RID: 3573
	[HideInInspector]
	public int playersFinished;
}
