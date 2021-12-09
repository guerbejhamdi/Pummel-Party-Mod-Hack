using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x02000219 RID: 537
public class RhythmController : MinigameController
{
	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06000FAB RID: 4011 RVA: 0x0000D692 File Offset: 0x0000B892
	public float CurrentSongTime
	{
		get
		{
			if (this.m_source != null)
			{
				return this.m_source.time;
			}
			return 0f;
		}
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x0000D6B3 File Offset: 0x0000B8B3
	public void Awake()
	{
		this.m_source = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x0007CA48 File Offset: 0x0007AC48
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.rand = new System.Random(UnityEngine.Random.Range(0, int.MaxValue));
		this.m_panel = UnityEngine.Object.FindObjectOfType<RhythmUIPanel>();
		this.m_panel.Init(this);
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("RhythmPlayer", null);
		}
		this.SetupSong(0);
	}

	// Token: 0x06000FAE RID: 4014 RVA: 0x0000D6C1 File Offset: 0x0000B8C1
	protected override void OnSpawnPlayers()
	{
		base.OnSpawnPlayers();
		this.m_panel.Show();
	}

	// Token: 0x06000FAF RID: 4015 RVA: 0x0000398C File Offset: 0x00001B8C
	public void OnDestroy()
	{
	}

	// Token: 0x06000FB0 RID: 4016 RVA: 0x0007CAA4 File Offset: 0x0007ACA4
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.Top, new Vector2(145f, 45f), 68f, true);
		if (this.ui_score != null)
		{
			for (int i = 0; i < GameManager.GetPlayerCount(); i++)
			{
				if (this.ui_score[i] != null)
				{
					base.SetScoreUIParent(i, this.m_panel.GetScoreUIParent(i));
				}
			}
		}
		float start_time = this.round_length;
		if (this.m_curSong != null)
		{
			if (this.m_source == null)
			{
				this.m_source = base.GetComponent<AudioSource>();
			}
			if (this.m_source != null)
			{
				this.m_source.clip = this.m_curSong.m_songClip;
				this.m_source.volume = Settings.MasterVolume;
			}
			start_time = this.m_curSong.m_songClip.length + 2.25f;
		}
		else
		{
			Debug.LogError("song was not set when minigame started");
		}
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), start_time);
		if (NetSystem.IsServer)
		{
			this.StartSongWithDelay(NetSystem.NetTime.GameTime + 1f);
		}
		UnityEngine.Object.FindObjectOfType<RhythmCamera>().Init();
		base.StartMinigame();
	}

	// Token: 0x06000FB1 RID: 4017 RVA: 0x0007CBCC File Offset: 0x0007ADCC
	private void SetupSong(int index)
	{
		this.m_curSong = this.m_songs[index];
		base.Spawn(this.m_curSong.m_scenePfb, Vector3.zero, Quaternion.identity);
		base.Spawn(this.m_curSong.m_platformPfb, Vector3.zero, Quaternion.identity);
		RhythmMarkupFile rhythmMarkupFile = new RhythmMarkupFile();
		rhythmMarkupFile.LoadFile(this.m_curSong.m_songMarkupFile);
		this.m_hits = rhythmMarkupFile.GetHitList();
	}

	// Token: 0x06000FB2 RID: 4018 RVA: 0x0000D6D4 File Offset: 0x0000B8D4
	private void StartSongWithDelay(float time)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCStartSongWithDelay", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				time
			});
		}
		this.m_startSongAt = time;
	}

	// Token: 0x06000FB3 RID: 4019 RVA: 0x0000D6FF File Offset: 0x0000B8FF
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCStartSongWithDelay(NetPlayer sender, float time)
	{
		this.StartSongWithDelay(time);
	}

	// Token: 0x06000FB4 RID: 4020 RVA: 0x0000D708 File Offset: 0x0000B908
	private void StartSong()
	{
		this.m_songStarted = true;
		this.m_curSongTime = 0f;
		this.m_nextSpawn = Time.time + 0.25f;
		this.m_source.Play();
	}

	// Token: 0x06000FB5 RID: 4021 RVA: 0x0007CC44 File Offset: 0x0007AE44
	public void UpdateSong()
	{
		if (!this.m_songStarted)
		{
			return;
		}
		if (false)
		{
			if (Time.time > this.m_nextSpawn)
			{
				RhythmHit hit = new RhythmHit(this.CurrentSongTime + RhythmController.LeadTime, this.CurrentSongTime + RhythmController.LeadTime, RhythmHitType.Hit, (RhythmHitButton)UnityEngine.Random.Range(0, 7));
				this.m_panel.SpawnButton(hit);
				this.m_nextSpawn = Time.time + 1f;
			}
		}
		else
		{
			for (int i = this.m_hits.Count - 1; i >= 0; i--)
			{
				RhythmHit rhythmHit = this.m_hits[i];
				if (this.CurrentSongTime + RhythmController.LeadTime > rhythmHit.startTime)
				{
					this.m_panel.SpawnButton(rhythmHit);
					this.m_hits.RemoveAt(i);
				}
			}
		}
		this.m_panel.UpdatePanel();
	}

	// Token: 0x06000FB6 RID: 4022 RVA: 0x0007CD10 File Offset: 0x0007AF10
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			if (this.ui_timer.time_test < 1f && !this.finalized && NetSystem.IsServer)
			{
				foreach (CharacterBase characterBase in this.players)
				{
					RhythmPlayer rhythmPlayer = (RhythmPlayer)characterBase;
					int num = (int)(rhythmPlayer.LargestCombo * 10);
					rhythmPlayer.Score += (short)num;
					rhythmPlayer.AddCombo(num);
				}
				this.finalized = true;
			}
			if (NetSystem.IsServer && this.ui_timer.time_test <= 0f)
			{
				base.EndRound(3f, 1f, false);
			}
			if (!this.m_songStarted && NetSystem.NetTime.GameTime > this.m_startSongAt)
			{
				this.StartSong();
			}
			this.UpdateSong();
		}
	}

	// Token: 0x06000FB7 RID: 4023 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000FB8 RID: 4024 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000FB9 RID: 4025 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000FBA RID: 4026 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000FBB RID: 4027 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000FBC RID: 4028 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x04000FCE RID: 4046
	private System.Random rand;

	// Token: 0x04000FCF RID: 4047
	[Header("Songs")]
	public SongInfo[] m_songs;

	// Token: 0x04000FD0 RID: 4048
	private SongInfo m_curSong;

	// Token: 0x04000FD1 RID: 4049
	private float m_curSongTime;

	// Token: 0x04000FD2 RID: 4050
	private bool m_songStarted;

	// Token: 0x04000FD3 RID: 4051
	private float m_startSongAt = float.PositiveInfinity;

	// Token: 0x04000FD4 RID: 4052
	private RhythmUIPanel m_panel;

	// Token: 0x04000FD5 RID: 4053
	private float m_nextSpawn = float.PositiveInfinity;

	// Token: 0x04000FD6 RID: 4054
	private List<RhythmHit> m_hits;

	// Token: 0x04000FD7 RID: 4055
	public static float LeadTime = 3f;

	// Token: 0x04000FD8 RID: 4056
	public static float PerfectZonePercent = 0.0375f;

	// Token: 0x04000FD9 RID: 4057
	public static float GreatZonePercent = RhythmController.PerfectZonePercent + 0.015f;

	// Token: 0x04000FDA RID: 4058
	public static float GoodZonePercent = RhythmController.GreatZonePercent + 0.015f;

	// Token: 0x04000FDB RID: 4059
	private AudioSource m_source;

	// Token: 0x04000FDC RID: 4060
	private bool finalized;
}
