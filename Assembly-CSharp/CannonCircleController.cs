using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200018C RID: 396
public class CannonCircleController : MinigameController
{
	// Token: 0x06000B46 RID: 2886 RVA: 0x00060C44 File Offset: 0x0005EE44
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("CannonCirclePlayer", null);
		}
		this.cameraShake = base.Root.GetComponentInChildren<CameraShake>();
		this.cannonController = base.Root.GetComponentInChildren<CannonController>();
		this.cannonController.minigameController = this;
	}

	// Token: 0x06000B47 RID: 2887 RVA: 0x0005B184 File Offset: 0x00059384
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		for (int i = 0; i < this.ui_score.Length; i++)
		{
			if (!(this.ui_score[i] == null))
			{
				this.ui_score[i].scoreUpdateSpeed = 25;
			}
		}
		base.StartMinigame();
	}

	// Token: 0x06000B48 RID: 2888 RVA: 0x00060C98 File Offset: 0x0005EE98
	public override void RoundEnded()
	{
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < this.players.Count; i++)
			{
				if (!((CannonCirclePlayer)this.players[i]).IsDead)
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

	// Token: 0x06000B49 RID: 2889 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000B4A RID: 2890 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x00060D58 File Offset: 0x0005EF58
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			if (this.ui_timer.time_test <= 0f || base.PlayersAlive <= 1)
			{
				base.EndRound(1f, 3f, false);
			}
			if (this.shootTimer.Elapsed(false))
			{
				float time = this.round_length - this.ui_timer.time_test;
				byte[] free = this.cannonController.GetFree(UnityEngine.Random.Range((int)this.minCannons.Evaluate(time), (int)this.maxCannons.Evaluate(time)));
				float[] array = new float[free.Length];
				float[] array2 = new float[free.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = ZPMath.RandomFloat(GameManager.rand, 0f, 360f);
					array2[i] = ZPMath.RandomFloat(GameManager.rand, 0f, 0.8f);
				}
				if (free != null)
				{
					this.DoShots(free, array, array2);
				}
				this.shootTimer.SetInterval(this.roundMinInterval.Evaluate(time), this.roundMaxInterval.Evaluate(time), true);
			}
		}
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x0000B2EC File Offset: 0x000094EC
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCDoShots(NetPlayer sender, byte[] cannons, float[] rots, float[] delays)
	{
		if (this == null || base.State != MinigameControllerState.Playing)
		{
			return;
		}
		this.DoShots(cannons, rots, delays);
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x00060E7C File Offset: 0x0005F07C
	private void DoShots(byte[] cannons, float[] rots, float[] delays)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCDoShots", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				cannons,
				rots,
				delays
			});
		}
		if (this.cannonController == null)
		{
			return;
		}
		for (int i = 0; i < cannons.Length; i++)
		{
			base.StartCoroutine(this.cannonController.FireCannon(cannons[i], rots[i], delays[i]));
		}
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x0000A7A0 File Offset: 0x000089A0
	public void PlayerDied(CannonCirclePlayer player)
	{
		if (NetSystem.IsServer)
		{
			player.RoundScore = (short)(this.players.Count - this.players_alive);
		}
		this.players_alive--;
	}

	// Token: 0x04000A60 RID: 2656
	[Header("Minigame specific attributes")]
	public AnimationCurve minCannons;

	// Token: 0x04000A61 RID: 2657
	public AnimationCurve maxCannons;

	// Token: 0x04000A62 RID: 2658
	public AnimationCurve roundMinInterval;

	// Token: 0x04000A63 RID: 2659
	public AnimationCurve roundMaxInterval;

	// Token: 0x04000A64 RID: 2660
	private CameraShake cameraShake;

	// Token: 0x04000A65 RID: 2661
	private CannonController cannonController;

	// Token: 0x04000A66 RID: 2662
	private ActionTimer shootTimer = new ActionTimer(2f, 5f);

	// Token: 0x04000A67 RID: 2663
	public List<Transform> cannonCircleProjectiles = new List<Transform>();
}
