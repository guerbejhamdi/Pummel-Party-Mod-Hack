using System;
using UnityEngine;
using ZP.Net;

// Token: 0x0200022A RID: 554
public class RunnerController : MinigameController
{
	// Token: 0x0600101A RID: 4122 RVA: 0x0007F07C File Offset: 0x0007D27C
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("RunnerPlayer", null);
			this.seed = UnityEngine.Random.Range(0, int.MaxValue);
			this.SetupPath(this.seed);
		}
		this.m_movement = base.Root.GetComponentInChildren<RunnerMovement>();
	}

	// Token: 0x0600101B RID: 4123 RVA: 0x0007F0D0 File Offset: 0x0007D2D0
	private void SetupPath(int seed)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSetupPath", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				seed
			});
		}
		if (!this.pathSetup)
		{
			RunnerPath[] array = UnityEngine.Object.FindObjectsOfType<RunnerPath>();
			if (array != null && array.Length >= 2)
			{
				RunnerPath[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].SetupPath(seed);
				}
				this.pathSetup = true;
			}
		}
	}

	// Token: 0x0600101C RID: 4124 RVA: 0x0000DAC1 File Offset: 0x0000BCC1
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSetupPath(NetPlayer sender, int seed)
	{
		this.SetupPath(seed);
	}

	// Token: 0x0600101D RID: 4125 RVA: 0x0000BCDB File Offset: 0x00009EDB
	public override void OnPlayersReady()
	{
		base.OnPlayersReady();
	}

	// Token: 0x0600101E RID: 4126 RVA: 0x0007F138 File Offset: 0x0007D338
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		this.m_movement.StartMovement();
		if (NetSystem.IsServer)
		{
			this.SetupPath(this.seed);
		}
		base.StartMinigame();
	}

	// Token: 0x0600101F RID: 4127 RVA: 0x0007F1A4 File Offset: 0x0007D3A4
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			if (this.ui_timer.time_test <= 30f && !this.gotAchievement)
			{
				bool flag = false;
				for (int i = 0; i < this.players.Count; i++)
				{
					if (this.players[i].GamePlayer.IsLocalPlayer && !this.players[i].IsDead && !this.players[i].GamePlayer.IsAI)
					{
						flag = true;
					}
				}
				if (flag)
				{
					PlatformAchievementManager.Instance.TriggerAchievement("ACH_SORCERERS_SPRINT");
					this.gotAchievement = true;
				}
			}
			if (NetSystem.IsServer)
			{
				if (this.ui_timer.time_test <= 0f)
				{
					base.EndRound(1f, 1f, false);
				}
				if (this.players_alive <= 0)
				{
					base.EndRound(1f, 1f, false);
				}
			}
		}
	}

	// Token: 0x06001020 RID: 4128 RVA: 0x0000AD54 File Offset: 0x00008F54
	public void PlayerDied(RunnerPlayer player)
	{
		this.players_alive--;
	}

	// Token: 0x06001021 RID: 4129 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06001022 RID: 4130 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x04001066 RID: 4198
	private RunnerMovement m_movement;

	// Token: 0x04001067 RID: 4199
	private bool gotAchievement;

	// Token: 0x04001068 RID: 4200
	private bool pathSetup;

	// Token: 0x04001069 RID: 4201
	private int seed;
}
