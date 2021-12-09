using System;
using UnityEngine;
using ZP.Net;

// Token: 0x020001FB RID: 507
public class PlanesController : MinigameController
{
	// Token: 0x06000EDA RID: 3802 RVA: 0x0000CEF6 File Offset: 0x0000B0F6
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("PlanesPlayer", null);
		}
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x00009BEC File Offset: 0x00007DEC
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x0000AB1C File Offset: 0x00008D1C
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer && this.ui_timer.time_test <= 0f)
		{
			base.EndRound(1f, 3f, false);
		}
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x00076F30 File Offset: 0x00075130
	public void TryDamagePlayer(PlanesPlayer sourcePlayer, PlanesPlayer hitPlayer, Vector3 worldPos)
	{
		if (!NetSystem.IsServer)
		{
			Vector3 vector = hitPlayer.transform.InverseTransformPoint(worldPos);
			base.SendRPC("RPCTryDamagePlayer", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				sourcePlayer.GamePlayer.GlobalID,
				hitPlayer.GamePlayer.GlobalID,
				vector
			});
			return;
		}
		if (hitPlayer.IsDead)
		{
			return;
		}
		if (hitPlayer.Health > 0)
		{
			hitPlayer.Health -= 1;
		}
		if (hitPlayer.Health == 0)
		{
			this.OnPlayerDamaged(hitPlayer, true, worldPos);
			hitPlayer.Health = 5;
			sourcePlayer.Score += 1;
			return;
		}
		this.OnPlayerDamaged(hitPlayer, false, worldPos);
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x00076FE8 File Offset: 0x000751E8
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCTryDamagePlayer(NetPlayer sender, short sourcePlayerId, short hitPlayerId, Vector3 localPos)
	{
		PlanesPlayer planesPlayer = null;
		PlanesPlayer planesPlayer2 = null;
		foreach (CharacterBase characterBase in this.players)
		{
			PlanesPlayer planesPlayer3 = (PlanesPlayer)characterBase;
			if (planesPlayer3.GamePlayer.GlobalID == sourcePlayerId)
			{
				planesPlayer = planesPlayer3;
			}
			else if (planesPlayer3.GamePlayer.GlobalID == hitPlayerId)
			{
				planesPlayer2 = planesPlayer3;
			}
		}
		if (planesPlayer2 != null && planesPlayer != null)
		{
			Vector3 worldPos = planesPlayer2.transform.TransformPoint(localPos);
			this.TryDamagePlayer(planesPlayer, planesPlayer2, worldPos);
			return;
		}
		Debug.LogError("Could not find player to damage with id = " + hitPlayerId.ToString());
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x000770A4 File Offset: 0x000752A4
	private void OnPlayerDamaged(PlanesPlayer player, bool dead, Vector3 worldPos)
	{
		if (NetSystem.IsServer)
		{
			Vector3 vector = player.transform.InverseTransformPoint(worldPos);
			base.SendRPC("RPCOnPlayerDamaged", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				player.GamePlayer.GlobalID,
				dead,
				vector
			});
		}
		player.OnPlayerDamaged(worldPos);
		if (dead)
		{
			player.KillPlayer(false);
		}
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x00077110 File Offset: 0x00075310
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCOnPlayerDamaged(NetPlayer sender, short globalID, bool dead, Vector3 localPos)
	{
		foreach (CharacterBase characterBase in this.players)
		{
			PlanesPlayer planesPlayer = (PlanesPlayer)characterBase;
			if (planesPlayer.GamePlayer.GlobalID == globalID)
			{
				Vector3 worldPos = planesPlayer.transform.TransformPoint(localPos);
				this.OnPlayerDamaged(planesPlayer, dead, worldPos);
				return;
			}
		}
		Debug.LogError("Could not find player to damage with id = " + globalID.ToString());
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x0000A3B0 File Offset: 0x000085B0
	public override void ReleaseMinigame()
	{
		GameManager.CaptureInput = false;
		base.ReleaseMinigame();
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x0000A3A2 File Offset: 0x000085A2
	public override void OnPlayersReady()
	{
		base.OnPlayersReady();
		GameManager.CaptureInput = true;
	}
}
