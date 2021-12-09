using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using ZP.Net;

// Token: 0x02000170 RID: 368
public class BulletDodgeController : MinigameController
{
	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x0000ADE8 File Offset: 0x00008FE8
	public int CurPhase
	{
		get
		{
			return this.cur_phase;
		}
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x0005D108 File Offset: 0x0005B308
	public override void OnNetInitialize()
	{
		this.events.Add(this.phase1);
		this.events.Add(this.phase2);
		this.events.Add(this.phase3);
		this.events.Add(this.phase4);
		base.OnNetInitialize();
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x0000ADF0 File Offset: 0x00008FF0
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("BulletDodgePlayer", null);
		}
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x0005D160 File Offset: 0x0005B360
	public override void StartMinigame()
	{
		float num = 0f;
		for (int i = 0; i < this.phase_lengths.Count; i++)
		{
			num += this.phase_lengths[i];
		}
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), num);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		for (int j = 0; j < this.players.Count; j++)
		{
			this.UpdateUILives(j, 2);
		}
		if (NetSystem.IsServer)
		{
			this.last_phase_shift_time = Time.time;
		}
		GameObject.Find("ScoreUIRenderScene/Camera").GetComponent<PostProcessLayer>().enabled = false;
		base.StartMinigame();
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x0005D218 File Offset: 0x0005B418
	public override void RoundEnded()
	{
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < this.players.Count; i++)
			{
				if (((BulletDodgePlayer)this.players[i]).CurState != BulletDodgePlayer.BulletDodgePlayerState.Permadeath)
				{
					CharacterBase characterBase = this.players[i];
					characterBase.Score += (short)((((BulletDodgePlayer)this.players[i]).lives + 1) * 10);
				}
				else
				{
					CharacterBase characterBase2 = this.players[i];
					characterBase2.Score += this.players[i].RoundScore;
					this.players[i].RoundScore = 0;
				}
			}
		}
		base.RoundEnded();
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x0005D2E0 File Offset: 0x0005B4E0
	private void CheckAchievement()
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
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_BULLET_BARRAGE");
			this.gotAchievement = true;
		}
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x0005D364 File Offset: 0x0005B564
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			if (this.ui_timer.time_test <= 70f && !this.gotAchievement)
			{
				this.CheckAchievement();
			}
			if (NetSystem.IsServer)
			{
				if (Time.time - this.last_phase_shift_time > this.phase_lengths[this.cur_phase])
				{
					this.last_phase_shift_time = Time.time;
					if (this.cur_phase + 1 >= this.events.Count)
					{
						this.CheckAchievement();
						base.EndRound(1f, 1f, false);
						return;
					}
					if (this.events[this.cur_phase + 1].Count > 0)
					{
						this.cur_phase++;
					}
				}
				if (!this.event_active)
				{
					this.StartEvent(NetSystem.NetTime.GameTime, this.cur_phase, UnityEngine.Random.Range(0, this.events[this.cur_phase].Count));
				}
				if (this.players_alive <= 1)
				{
					base.EndRound(1f, 1f, false);
				}
			}
		}
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x0000AE0B File Offset: 0x0000900B
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCStartEvent(NetPlayer sender, float net_time, int phase_id, int event_id)
	{
		this.StartEvent(net_time, phase_id, event_id);
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x0005D47C File Offset: 0x0005B67C
	public void StartEvent(float net_time, int phase_id, int event_id)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCStartEvent", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				net_time,
				phase_id,
				event_id
			});
		}
		base.Spawn(this.events[phase_id][event_id], Vector3.zero, Quaternion.identity).GetComponent<BulletDodgeEvent>().NetTime = net_time;
		this.event_active = true;
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x0000A7A0 File Offset: 0x000089A0
	public void PlayerDied(BulletDodgePlayer player)
	{
		if (NetSystem.IsServer)
		{
			player.RoundScore = (short)(this.players.Count - this.players_alive);
		}
		this.players_alive--;
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0000AE17 File Offset: 0x00009017
	public void UpdateUILives(int player_slot, int lives)
	{
		if (this.ui_score != null && player_slot > 0 && player_slot < this.ui_score.Length && this.ui_score[player_slot] != null)
		{
			this.ui_score[player_slot].SetScore(lives);
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0000ADE8 File Offset: 0x00008FE8
	public int CurrentPhase
	{
		get
		{
			return this.cur_phase;
		}
	}

	// Token: 0x04000988 RID: 2440
	public List<GameObject> phase1 = new List<GameObject>();

	// Token: 0x04000989 RID: 2441
	public List<GameObject> phase2 = new List<GameObject>();

	// Token: 0x0400098A RID: 2442
	public List<GameObject> phase3 = new List<GameObject>();

	// Token: 0x0400098B RID: 2443
	public List<GameObject> phase4 = new List<GameObject>();

	// Token: 0x0400098C RID: 2444
	public List<float> phase_lengths = new List<float>();

	// Token: 0x0400098D RID: 2445
	public bool event_active;

	// Token: 0x0400098E RID: 2446
	private List<List<GameObject>> events = new List<List<GameObject>>();

	// Token: 0x0400098F RID: 2447
	private float last_phase_shift_time;

	// Token: 0x04000990 RID: 2448
	private int cur_phase;

	// Token: 0x04000991 RID: 2449
	private int player_objs_loaded;

	// Token: 0x04000992 RID: 2450
	private bool gotAchievement;
}
