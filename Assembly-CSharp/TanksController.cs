using System;
using UnityEngine;
using ZP.Net;

// Token: 0x02000276 RID: 630
public class TanksController : MinigameController
{
	// Token: 0x06001255 RID: 4693 RVA: 0x0000ED3B File Offset: 0x0000CF3B
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("TanksPlayer", null);
		}
		this.grid = base.Root.transform.Find("VoxelGrid").GetComponent<VoxelGrid>();
	}

	// Token: 0x06001256 RID: 4694 RVA: 0x0000BCDB File Offset: 0x00009EDB
	public override void OnPlayersReady()
	{
		base.OnPlayersReady();
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x00009BEC File Offset: 0x00007DEC
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x06001258 RID: 4696 RVA: 0x0008D814 File Offset: 0x0008BA14
	public override void RoundEnded()
	{
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < this.players.Count; i++)
			{
				if (!((TanksPlayer)this.players[i]).IsDead)
				{
					CharacterBase characterBase = this.players[i];
					characterBase.Score += (short)(this.players.Count * 25);
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

	// Token: 0x06001259 RID: 4697 RVA: 0x0008D8D0 File Offset: 0x0008BAD0
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			if (this.ui_timer.time_test <= 0f)
			{
				base.EndRound(1f, 1f, false);
			}
			if (this.players_alive <= 1)
			{
				base.EndRound(1f, 1f, false);
			}
		}
	}

	// Token: 0x0600125A RID: 4698 RVA: 0x0000ED76 File Offset: 0x0000CF76
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void EditRPC(NetPlayer sender, Vector3 position)
	{
		this.Edit(position);
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x0008D92C File Offset: 0x0008BB2C
	public void Edit(Vector3 pos)
	{
		bool flag = this.grid.Edit(new Edit(BrushShape.Cylinder, BrushAction.Subtract, pos, 6f), true);
		this.grid.Edit(new Edit(BrushShape.Sphere, BrushAction.Smooth, pos, 9f, 0.4f), false);
		if (NetSystem.IsServer && flag)
		{
			base.SendRPC("EditRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				pos
			});
		}
	}

	// Token: 0x0600125C RID: 4700 RVA: 0x0000A7A0 File Offset: 0x000089A0
	public void PlayerDied(TanksPlayer player)
	{
		if (NetSystem.IsServer)
		{
			player.RoundScore = (short)(this.players.Count - this.players_alive);
		}
		this.players_alive--;
	}

	// Token: 0x0600125D RID: 4701 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x0600125E RID: 4702 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x0600125F RID: 4703 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x04001354 RID: 4948
	private VoxelGrid grid;
}
