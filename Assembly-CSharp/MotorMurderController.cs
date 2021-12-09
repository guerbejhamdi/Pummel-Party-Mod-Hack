using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x020001E0 RID: 480
public class MotorMurderController : MinigameController
{
	// Token: 0x06000DEA RID: 3562 RVA: 0x0007016C File Offset: 0x0006E36C
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("MotorMurderPlayer", null);
		}
		Transform transform = base.Root.transform.Find("BuffSpawnPointsRoot");
		this.buffSpawnPoints = new Transform[transform.childCount];
		for (int i = 0; i < this.buffSpawnPoints.Length; i++)
		{
			this.buffSpawnPoints[i] = transform.GetChild(i);
		}
	}

	// Token: 0x06000DEB RID: 3563 RVA: 0x000701DC File Offset: 0x0006E3DC
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		this.statusBars = new MotorMurderStatusBar[GameManager.GetPlayerCount()];
		for (int i = 0; i < this.statusBars.Length; i++)
		{
			this.statusBars[i] = UnityEngine.Object.Instantiate<GameObject>(this.statusUIBarPrefab).GetComponent<MotorMurderStatusBar>();
			this.statusBars[i].transform.SetParent(GameManager.UIController.MinigameUIRoot, false);
			this.statusBars[i].minigameController = this;
			this.statusBars[i].player = (MotorMurderPlayer)base.GetPlayer(i);
		}
		base.StartMinigame();
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x000702A8 File Offset: 0x0006E4A8
	public override void ResetRound()
	{
		for (int i = 0; i < this.activeBuffs.Count; i++)
		{
			if (this.activeBuffs[i] != null && !this.activeBuffs[i].Despawning)
			{
				this.activeBuffs[i].Despawn();
			}
		}
		base.ResetRound();
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000DF1 RID: 3569 RVA: 0x0007030C File Offset: 0x0006E50C
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			if (this.buffSpawnTimer.Elapsed(true))
			{
				int num = 1000;
				int i = 0;
				byte b = 0;
				while (i < num)
				{
					b = (byte)UnityEngine.Random.Range(0, this.buffSpawnPoints.Length);
					bool flag = false;
					for (int j = 0; j < this.activeBuffs.Count; j++)
					{
						if (this.activeBuffs[j] != null && !this.activeBuffs[j].Despawning && this.activeBuffs[j].SpawnPointID == b)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
					i++;
				}
				this.SpawnBuff((byte)UnityEngine.Random.Range(1, 4), (byte)this.activeBuffs.Count, b);
			}
			if (this.ui_timer.time_test <= 0f)
			{
				base.EndRound(1f, 1f, false);
			}
		}
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x0000C7DE File Offset: 0x0000A9DE
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawnBuff(NetPlayer sender, byte type, byte id, byte spawnPoint)
	{
		this.SpawnBuff(type, id, spawnPoint);
	}

	// Token: 0x06000DF3 RID: 3571 RVA: 0x00070404 File Offset: 0x0006E604
	private void SpawnBuff(byte type, byte id, byte spawnPoint)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSpawnBuff", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				type,
				id,
				spawnPoint
			});
		}
		MotorMurderBuff component = base.Spawn(this.buffPrefab, this.buffSpawnPoints[(int)spawnPoint].position, Quaternion.identity).GetComponent<MotorMurderBuff>();
		component.Type = type;
		component.ID = id;
		component.SpawnPointID = spawnPoint;
		this.activeBuffs.Add(component);
	}

	// Token: 0x06000DF4 RID: 3572 RVA: 0x0000C7EA File Offset: 0x0000A9EA
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void RPCDespawnBuff(NetPlayer sender, byte id)
	{
		this.DespawnBuff(id, false);
	}

	// Token: 0x06000DF5 RID: 3573 RVA: 0x0007048C File Offset: 0x0006E68C
	public void DespawnBuff(byte id, bool sendRPC = false)
	{
		if (sendRPC)
		{
			base.SendRPC("RPCDespawnBuff", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				id
			});
		}
		if (this.activeBuffs[(int)id] != null)
		{
			this.activeBuffs[(int)id].Despawn();
		}
	}

	// Token: 0x06000DF6 RID: 3574 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000DF7 RID: 3575 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000DF8 RID: 3576 RVA: 0x0000C7F4 File Offset: 0x0000A9F4
	public void PlayerDied(MotorMurderPlayer player)
	{
		bool isServer = NetSystem.IsServer;
		this.players_alive--;
	}

	// Token: 0x04000D54 RID: 3412
	public GameObject statusUIBarPrefab;

	// Token: 0x04000D55 RID: 3413
	public GameObject buffPrefab;

	// Token: 0x04000D56 RID: 3414
	public MotorMurderStatusBar[] statusBars;

	// Token: 0x04000D57 RID: 3415
	public Transform[] buffSpawnPoints;

	// Token: 0x04000D58 RID: 3416
	private List<MotorMurderBuff> activeBuffs = new List<MotorMurderBuff>();

	// Token: 0x04000D59 RID: 3417
	private ActionTimer buffSpawnTimer = new ActionTimer(4f, 8f);
}
