using System;
using UnityEngine;
using ZP.Net;

// Token: 0x02000118 RID: 280
public class LightningController : MinigameController
{
	// Token: 0x06000854 RID: 2132 RVA: 0x0004DCA0 File Offset: 0x0004BEA0
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			int num = UnityEngine.Random.Range(0, GameManager.GetPlayerCount());
			base.FindSpawnPoints();
			int num2 = 0;
			Vector3 position = base.SpawnPoints[0].position;
			Vector3 position2 = base.SpawnPoints[2].position;
			float num3 = 0f;
			float num4 = Vector3.Distance(position, position2);
			if (GameManager.GetPlayerCount() == 2)
			{
				num3 = 0.5f;
				num4 = 0f;
			}
			else if (GameManager.GetPlayerCount() > 3)
			{
				num4 = num4 / (float)(GameManager.GetPlayerCount() - 2) / num4;
			}
			for (int i = 0; i < GameManager.GetPlayerCount(); i++)
			{
				if (i == num)
				{
					base.NetSpawn("LightningPlayer", base.SpawnPoints[3].position, base.SpawnPoints[3].rotation, (ushort)i, GameManager.GetPlayerAt(i).NetOwner).GetComponent<LightningPlayer>().LightningHolderRecieve(true);
				}
				else
				{
					base.NetSpawn("LightningPlayer", Vector3.Lerp(position, position2, num3), base.SpawnPoints[0].rotation, (ushort)i, GameManager.GetPlayerAt(i).NetOwner);
					num2++;
					num3 += num4;
				}
			}
		}
		this.cameraShake = base.Root.GetComponentInChildren<CameraShake>();
		this.tiles = base.Root.GetComponentsInChildren<LightningFloorTile>();
		this.lightningParticle = UnityEngine.Object.FindObjectOfType<LightningParticleController>();
		this.lightningFlash = UnityEngine.Object.FindObjectOfType<Effects_Lightning>();
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x00009BEC File Offset: 0x00007DEC
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x0004DE08 File Offset: 0x0004C008
	public override void RoundEnded()
	{
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < this.players.Count; i++)
			{
				LightningPlayer lightningPlayer = (LightningPlayer)this.players[i];
				if (!lightningPlayer.IsDead && !lightningPlayer.lightningHolder.Value)
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
		bool flag = false;
		bool flag2 = false;
		for (int j = 0; j < this.players.Count; j++)
		{
			if (((LightningPlayer)this.players[j]).lightningHolder.Value)
			{
				if (this.players[j].IsOwner && !this.players[j].GamePlayer.IsAI)
				{
					flag = true;
				}
			}
			else if (!this.players[j].IsDead)
			{
				flag2 = true;
			}
		}
		if (flag && !flag2)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_THUNDEROUS_TRENCH");
		}
		base.RoundEnded();
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x0004DF6C File Offset: 0x0004C16C
	private void Update()
	{
		if (base.State != MinigameControllerState.Playing)
		{
			MinigameControllerState state = base.State;
		}
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer && (this.ui_timer.time_test <= 0f || this.players_alive <= 1))
		{
			base.EndRound(1f, 3f, false);
		}
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x0004DFC8 File Offset: 0x0004C1C8
	public void PlayerDied(LightningPlayer player)
	{
		if (NetSystem.IsServer)
		{
			player.RoundScore = (short)(this.players.Count - this.players_alive);
			LightningPlayer lightningPlayer = this.lightningUser;
			lightningPlayer.Score += 25;
		}
		this.players_alive--;
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x0004E018 File Offset: 0x0004C218
	public void DestroyTile(Vector3 pos)
	{
		float num = float.MaxValue;
		int num2 = -1;
		for (int i = 0; i < this.tiles.Length; i++)
		{
			if (!this.tiles[i].IsDestroyed)
			{
				Vector3 vector = pos - this.tiles[i].transform.position;
				float sqrMagnitude = vector.sqrMagnitude;
				if (sqrMagnitude < num && Mathf.Abs(vector.x) <= 6f && Mathf.Abs(vector.z) <= 6f)
				{
					num = sqrMagnitude;
					num2 = i;
				}
			}
		}
		if (num2 != -1)
		{
			base.SendRPC("DestroyTileRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				(byte)num2
			});
			this.DestroyTile(num2);
		}
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x0004E0C8 File Offset: 0x0004C2C8
	private void DestroyTile(int id)
	{
		this.lightningParticle.Fire(this.tiles[id].transform.position + new Vector3(1.25f, 0f, 1.25f));
		this.lightningFlash.ShootLightning();
		this.cameraShake.AddShake(0.3f);
		this.tiles[id].Destroy();
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x00009C54 File Offset: 0x00007E54
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	private void DestroyTileRPC(NetPlayer sender, byte id)
	{
		this.DestroyTile((int)id);
	}

	// Token: 0x040006B4 RID: 1716
	[HideInInspector]
	public LightningPlayer lightningUser;

	// Token: 0x040006B5 RID: 1717
	[HideInInspector]
	public LightningFloorTile[] tiles;

	// Token: 0x040006B6 RID: 1718
	private CameraShake cameraShake;

	// Token: 0x040006B7 RID: 1719
	private LightningParticleController lightningParticle;

	// Token: 0x040006B8 RID: 1720
	private Effects_Lightning lightningFlash;
}
