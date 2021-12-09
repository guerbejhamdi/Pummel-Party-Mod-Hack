using System;
using UnityEngine;
using ZP.Net;

// Token: 0x0200024A RID: 586
public class SnowySpinController : MinigameController
{
	// Token: 0x060010EA RID: 4330 RVA: 0x0000E036 File Offset: 0x0000C236
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("SnowySpinPlayer", null);
		}
	}

	// Token: 0x060010EB RID: 4331 RVA: 0x00009BEC File Offset: 0x00007DEC
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x060010EC RID: 4332 RVA: 0x00084F1C File Offset: 0x0008311C
	public override void RoundEnded()
	{
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < this.players.Count; i++)
			{
				if (!((SnowySpinPlayer)this.players[i]).IsDead)
				{
					CharacterBase characterBase = this.players[i];
					characterBase.Score += (short)((this.players.Count - 1) * 25);
				}
				else
				{
					CharacterBase characterBase2 = this.players[i];
					characterBase2.Score += this.players[i].RoundScore * 25;
					this.players[i].RoundScore = 0;
				}
			}
		}
		base.RoundEnded();
	}

	// Token: 0x060010ED RID: 4333 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x060010EE RID: 4334 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x060010EF RID: 4335 RVA: 0x0000E051 File Offset: 0x0000C251
	public override void ResetRound()
	{
		this.ui_timer.time_test = this.round_length;
		base.ResetRound();
	}

	// Token: 0x060010F0 RID: 4336 RVA: 0x0000E06A File Offset: 0x0000C26A
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && ((NetSystem.IsServer && this.players_alive <= 1) || this.ui_timer.time_test <= 0f))
		{
			base.EndRound(1f, 1f, false);
		}
	}

	// Token: 0x060010F1 RID: 4337 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x060010F3 RID: 4339 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x0000A7A0 File Offset: 0x000089A0
	public void PlayerDied(SnowySpinPlayer player)
	{
		if (NetSystem.IsServer)
		{
			player.RoundScore = (short)(this.players.Count - this.players_alive);
		}
		this.players_alive--;
	}
}
