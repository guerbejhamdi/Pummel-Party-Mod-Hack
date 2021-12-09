using System;
using UnityEngine;
using ZP.Net;

// Token: 0x0200018F RID: 399
public class CarsController : MinigameController
{
	// Token: 0x06000B65 RID: 2917 RVA: 0x0006177C File Offset: 0x0005F97C
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("CarsPlayer", null);
		}
		Transform transform = this.minigame_root.transform.Find("NavPoints");
		this.navPoints = new Transform[transform.childCount];
		for (int i = 0; i < this.navPoints.Length; i++)
		{
			this.navPoints[i] = transform.GetChild(i);
		}
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x0000ACF2 File Offset: 0x00008EF2
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.StartMinigame();
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x000617EC File Offset: 0x0005F9EC
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			int num = 0;
			for (int i = 0; i < this.players.Count; i++)
			{
				if (((CarsPlayer)this.players[i]).Finished)
				{
					num++;
				}
			}
			if (num >= this.players.Count - 1 || this.ui_timer.time_test <= 0f)
			{
				base.EndRound(1f, 1f, false);
			}
		}
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x00061870 File Offset: 0x0005FA70
	public void ShowWinnerText(GamePlayer player)
	{
		string text = string.Concat(new string[]
		{
			"<color=#",
			ColorUtility.ToHtmlStringRGBA(player.Color.uiColor),
			"> ",
			player.Name,
			"</color><color=#4FF051FF> Wins </color>"
		});
		GameManager.UIController.ShowLargeText(text, LargeTextType.PlayerWins, 4.5f, true, false);
		if (NetSystem.IsServer)
		{
			base.SendRPC("ShowWinnerRPC", NetRPCDelivery.RELIABLE_SEQUENCED, new object[]
			{
				player.GlobalID
			});
		}
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x0000B3D3 File Offset: 0x000095D3
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void ShowWinnerRPC(NetPlayer sender, short globalID)
	{
		this.ShowWinnerText(GameManager.GetPlayerAt((int)globalID));
	}

	// Token: 0x04000A74 RID: 2676
	public Transform[] navPoints;

	// Token: 0x04000A75 RID: 2677
	public int laps = 2;
}
