using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using ZP.Net;

// Token: 0x0200006C RID: 108
public class AwardSceneManager : NetBehaviour
{
	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000200 RID: 512 RVA: 0x00004E0A File Offset: 0x0000300A
	public Transform Root
	{
		get
		{
			return this.root;
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x06000201 RID: 513 RVA: 0x00004E12 File Offset: 0x00003012
	// (set) Token: 0x06000202 RID: 514 RVA: 0x00004E1A File Offset: 0x0000301A
	public AwardSceneManager.AwardSceneState CurAwardSceneState { get; set; }

	// Token: 0x06000203 RID: 515 RVA: 0x00004E23 File Offset: 0x00003023
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		GameManager.Board.awardSceneManager = this;
		base.StartCoroutine(this.Spawnplayers());
	}

	// Token: 0x06000204 RID: 516 RVA: 0x00004E43 File Offset: 0x00003043
	private IEnumerator Spawnplayers()
	{
		GameObject g = null;
		while (g == null)
		{
			g = GameObject.Find("GobletChallengeAwardsParent");
			yield return null;
		}
		this.root = GameObject.Find("GobletChallengeAwardsParent").transform;
		if (NetSystem.IsServer)
		{
			Transform transform = this.root.Find("CharacterSpawnPoint");
			for (int i = 0; i < GameManager.GetPlayerCount(); i++)
			{
				GamePlayer playerAt = GameManager.GetPlayerAt(i);
				NetSystem.Spawn("AwardScenePlayer", transform.position, transform.rotation, (ushort)i, playerAt.NetOwner);
			}
		}
		yield break;
	}

	// Token: 0x06000205 RID: 517 RVA: 0x00004E52 File Offset: 0x00003052
	public void Awake()
	{
		this.m_awardsController = UnityEngine.Object.FindObjectOfType<GobletChallengeAwards>();
	}

	// Token: 0x06000206 RID: 518 RVA: 0x00034368 File Offset: 0x00032568
	public void Cleanup()
	{
		if (NetSystem.IsServer)
		{
			foreach (AwardScenePlayer obj in this.players)
			{
				NetSystem.Kill(obj);
			}
			NetSystem.Kill(this);
		}
	}

	// Token: 0x06000207 RID: 519 RVA: 0x00004E5F File Offset: 0x0000305F
	public void StartIntro(bool doIntroduction, StatChallengeBoardEvent ev, bool isGameFinished)
	{
		base.StartCoroutine(this.DoAwardsShow(doIntroduction, ev, isGameFinished));
	}

	// Token: 0x06000208 RID: 520 RVA: 0x00004E71 File Offset: 0x00003071
	public IEnumerator DoAwardsShow(bool doIntroduction, StatChallengeBoardEvent ev, bool isGameFinished)
	{
		while (this.m_awardsController == null)
		{
			this.m_awardsController = UnityEngine.Object.FindObjectOfType<GobletChallengeAwards>();
			yield return new WaitForSeconds(0.25f);
		}
		this.m_awardsController.ShowIntro();
		yield return new WaitForSeconds(4.5f);
		string header = LocalizationManager.GetTranslation("AwardCeremony_Header", true, 0, true, false, null, null, true);
		string translation = LocalizationManager.GetTranslation("AwardCeremony_Intro_1_Line1", true, 0, true, false, null, null, true);
		string translation2 = LocalizationManager.GetTranslation("AwardCeremony_Intro_1_Line2", true, 0, true, false, null, null, true);
		string intro2_line = LocalizationManager.GetTranslation("AwardCeremony_Intro_2_Line1", true, 0, true, false, null, null, true);
		string intro2_line2 = LocalizationManager.GetTranslation("AwardCeremony_Intro_2_Line2", true, 0, true, false, null, null, true);
		if (doIntroduction)
		{
			AwardNote.ShowNote(header, translation + "\n" + translation2, this.m_awardsController.GetNoteSpawn(), 5f);
			yield return new WaitForSeconds(5.5f);
			AwardNote.ShowNote(header, intro2_line + "\n" + intro2_line2, this.m_awardsController.GetNoteSpawn(), 5f);
			yield return new WaitForSeconds(5.5f);
		}
		if (NetSystem.IsServer)
		{
			List<GobletChallenge> challenges = GameManager.RulesetManager.ActiveRuleset.GobletChallenges.GetChallenges(ev);
			if (challenges != null && challenges.Count > 0)
			{
				foreach (GobletChallenge gobletChallenge in challenges)
				{
					StatSortType sortType = (gobletChallenge.Extent == StatChallengeExtent.Most) ? StatSortType.Descending : StatSortType.Ascending;
					List<PlayerStats> playerStats = StatTracker.GetPlayerStats(gobletChallenge.NumPlayers, GameManager.GetPlayerCount(), gobletChallenge.Stat, true, sortType);
					List<GamePlayer> list = new List<GamePlayer>();
					foreach (PlayerStats playerStats2 in playerStats)
					{
						GamePlayer playerWithID = GameManager.GetPlayerWithID(playerStats2.PlayerID);
						if (playerWithID != null)
						{
							list.Add(playerWithID);
						}
					}
					yield return this.ShowAwardCoroutine(list, gobletChallenge.Extent, gobletChallenge.Stat);
				}
				List<GobletChallenge>.Enumerator enumerator = default(List<GobletChallenge>.Enumerator);
			}
			if (ev == StatChallengeBoardEvent.EndGame)
			{
				yield return this.DoWinnerEvent();
			}
		}
		else
		{
			while (!this.m_awardsComplete || this.m_awardQueue.Count > 0)
			{
				if (this.m_awardQueue.Count > 0)
				{
					QueuedAward queuedAward = this.m_awardQueue.Dequeue();
					if (queuedAward.winner)
					{
						yield return this.ShowWinnerCoroutine(queuedAward.players[0]);
					}
					else
					{
						yield return this.ShowAwardCoroutine(queuedAward.players, queuedAward.extent, queuedAward.stat);
					}
				}
				yield return new WaitForSeconds(0.01f);
			}
		}
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCAwardsComplete", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
			GameManager.Board.QueueAction(new ActionEndPummelAwards(isGameFinished), true, true);
		}
		yield break;
		yield break;
	}

	// Token: 0x06000209 RID: 521 RVA: 0x00004E95 File Offset: 0x00003095
	private IEnumerator DoWinnerEvent()
	{
		GamePlayer player = GameManager.GetPlayerPlacements()[0];
		yield return this.ShowWinnerCoroutine(player);
		yield break;
	}

	// Token: 0x0600020A RID: 522 RVA: 0x00004EA4 File Offset: 0x000030A4
	private IEnumerator ShowWinnerCoroutine(GamePlayer player)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCShowWinner", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				player.GlobalID
			});
		}
		string translation = LocalizationManager.GetTranslation("AwardCeremony_Header", true, 0, true, false, null, null, true);
		string translation2 = LocalizationManager.GetTranslation("AwardCeremony_End_Line1", true, 0, true, false, null, null, true);
		string translation3 = LocalizationManager.GetTranslation("AwardCeremony_End_Line2", true, 0, true, false, null, null, true);
		AwardNote.ShowNote(translation, translation2 + "\n" + translation3, this.m_awardsController.GetNoteSpawn(), 5f);
		yield return new WaitForSeconds(6f);
		AudioSystem.PlayMusic(null, 0.75f, 1f);
		AudioSystem.PlayOneShot(this.m_drumRoll, 0.5f, 0f, 1f);
		yield return new WaitForSeconds(2.4f);
		if (player.IsLocalPlayer && !player.IsAI)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_WIN_FIRST_GAME");
		}
		AudioSystem.PlayOneShot(this.m_finishFanfare, 0.65f, 0f, 1f);
		AwardScenePlayer awardScenePlayer = this.GetAwardScenePlayer(player);
		if (awardScenePlayer != null)
		{
			awardScenePlayer.Spawn(this.m_awardsController.GetSpawnPoint(0, 0).position, true);
		}
		this.m_awardsController.FinalWinner();
		yield return new WaitForSeconds(0.5f);
		string text = string.Concat(new string[]
		{
			"<color=#",
			ColorUtility.ToHtmlStringRGBA(player.Color.uiColor),
			"> ",
			player.Name,
			"</color>"
		});
		string translation4 = LocalizationManager.GetTranslation("Wins", true, 0, true, false, null, null, true);
		text = text + "<color=#4FF051FF> " + translation4 + " </color>";
		GameManager.UIController.ShowLargeText(text, LargeTextType.PlayerWins, 9f, false, false);
		yield return new WaitForSeconds(6f);
		yield break;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x000343C8 File Offset: 0x000325C8
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCShowWinner(NetPlayer sender, short playerID)
	{
		List<GamePlayer> list = new List<GamePlayer>();
		list.Add(GameManager.GetPlayerWithID(playerID));
		this.m_awardQueue.Enqueue(new QueuedAward(list, StatChallengeExtent.Most, StatType.MinigamesWon, true));
	}

	// Token: 0x0600020C RID: 524 RVA: 0x00004EBA File Offset: 0x000030BA
	public void ShowAward(List<GamePlayer> players, StatChallengeExtent extent, StatType stat)
	{
		base.StartCoroutine(this.ShowAwardCoroutine(players, extent, stat));
	}

	// Token: 0x0600020D RID: 525 RVA: 0x00004ECC File Offset: 0x000030CC
	private IEnumerator ShowAwardCoroutine(List<GamePlayer> players, StatChallengeExtent extent, StatType stat)
	{
		if (NetSystem.IsServer)
		{
			short[] array = new short[players.Count];
			for (int i = 0; i < players.Count; i++)
			{
				array[i] = players[i].GlobalID;
			}
			base.SendRPC("RPCShowAward", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				array,
				(byte)extent,
				(byte)stat
			});
		}
		string title;
		if (LocalizationManager.CurrentLanguage == "English")
		{
			title = "Award for the " + extent.ToString() + " " + PlayerStats.statNames[(int)stat];
		}
		else
		{
			string[] names = Enum.GetNames(typeof(StatType));
			string translation = LocalizationManager.GetTranslation("Challenge_Order_" + extent.ToString(), true, 0, true, false, null, null, true);
			string translation2 = LocalizationManager.GetTranslation("Challenge_Statistic_" + names[(int)stat], true, 0, true, false, null, null, true);
			if (LocalizationManager.IsRight2Left)
			{
				title = translation + " " + translation2;
			}
			else
			{
				title = translation2 + " " + translation;
			}
		}
		AwardNote note = AwardNote.ShowNote(title, "", this.m_awardsController.GetNoteSpawn(), 7f);
		yield return new WaitForSeconds(3f);
		string winners = "";
		int idx = 0;
		foreach (GamePlayer gamePlayer in players)
		{
			if (NetSystem.IsServer)
			{
				gamePlayer.BoardObject.GiveTrophy(1, 0, false);
			}
			AwardScenePlayer awardScenePlayer = this.GetAwardScenePlayer(gamePlayer);
			if (awardScenePlayer != null)
			{
				awardScenePlayer.Spawn(this.m_awardsController.GetSpawnPoint(players.Count - 1, idx).position, false);
			}
			Color uiColor = gamePlayer.Color.uiColor;
			uiColor.r *= 0.75f;
			uiColor.g *= 0.75f;
			uiColor.b *= 0.75f;
			string text = ColorUtility.ToHtmlStringRGB(gamePlayer.Color.uiColor);
			if (idx != 0)
			{
				winners += ", ";
			}
			winners = string.Concat(new string[]
			{
				winners,
				"<color=#",
				text,
				">",
				gamePlayer.Name,
				"</color>"
			});
			int num = idx;
			idx = num + 1;
			yield return new WaitForSeconds(0.1f);
		}
		List<GamePlayer>.Enumerator enumerator = default(List<GamePlayer>.Enumerator);
		note.DoWinnersEffect(winners);
		yield return new WaitForSeconds(6f);
		yield break;
		yield break;
	}

	// Token: 0x0600020E RID: 526 RVA: 0x000343FC File Offset: 0x000325FC
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCShowAward(NetPlayer sender, short[] playerIDs, byte extent, byte stat)
	{
		List<GamePlayer> list = new List<GamePlayer>();
		for (int i = 0; i < playerIDs.Length; i++)
		{
			GamePlayer playerWithID = GameManager.GetPlayerWithID(playerIDs[i]);
			if (playerWithID != null)
			{
				list.Add(playerWithID);
			}
		}
		this.m_awardQueue.Enqueue(new QueuedAward(list, (StatChallengeExtent)extent, (StatType)stat, false));
	}

	// Token: 0x0600020F RID: 527 RVA: 0x00004EF0 File Offset: 0x000030F0
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCAwardsComplete(NetPlayer sender)
	{
		this.m_awardsComplete = true;
	}

	// Token: 0x06000210 RID: 528 RVA: 0x00034448 File Offset: 0x00032648
	private AwardScenePlayer GetAwardScenePlayer(GamePlayer player)
	{
		foreach (AwardScenePlayer awardScenePlayer in this.players)
		{
			if (awardScenePlayer.GamePlayer == player)
			{
				return awardScenePlayer;
			}
		}
		return null;
	}

	// Token: 0x06000211 RID: 529 RVA: 0x000344A4 File Offset: 0x000326A4
	public void SetClientLoaded(ushort netUserID, bool sendRPC = false)
	{
		if (sendRPC)
		{
			base.SendRPC("ClientReady", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				netUserID
			});
		}
		List<GamePlayer> playerList = GameManager.PlayerList;
		for (int i = 0; i < playerList.Count; i++)
		{
			if (playerList[i].NetOwner.UserID == netUserID && !this.loadedPlayers.Contains(playerList[i].GlobalID))
			{
				this.loadedPlayers.Add(playerList[i].GlobalID);
			}
		}
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0003452C File Offset: 0x0003272C
	public bool AllClientsLoaded()
	{
		List<GamePlayer> playerList = GameManager.PlayerList;
		for (int i = 0; i < playerList.Count; i++)
		{
			if (!this.loadedPlayers.Contains(playerList[i].GlobalID))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0003456C File Offset: 0x0003276C
	public void Update()
	{
		switch (this.CurAwardSceneState)
		{
		case AwardSceneManager.AwardSceneState.Loading:
		case AwardSceneManager.AwardSceneState.Playing:
			break;
		case AwardSceneManager.AwardSceneState.Intro:
			if (this.stateTimer.Elapsed(false))
			{
				this.CurAwardSceneState = AwardSceneManager.AwardSceneState.Playing;
				if (GameManager.partyGameMode == PartyGameMode.BoardGame)
				{
					GameManager.UIController.ShowScoreBoard();
				}
				for (int i = 0; i < this.players.Count; i++)
				{
					this.players[i].StopPlacementAnim();
				}
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000214 RID: 532 RVA: 0x00004EF9 File Offset: 0x000030F9
	public bool Playable
	{
		get
		{
			return this.CurAwardSceneState == AwardSceneManager.AwardSceneState.Playing;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x06000215 RID: 533 RVA: 0x000345E0 File Offset: 0x000327E0
	public int PlayersAlive
	{
		get
		{
			int num = 0;
			for (int i = 0; i < this.players.Count; i++)
			{
				if (!this.players[i].IsDead)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x06000216 RID: 534 RVA: 0x00004F04 File Offset: 0x00003104
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void ClientReady(NetPlayer sender, ushort playerID)
	{
		this.SetClientLoaded(playerID, false);
	}

	// Token: 0x04000265 RID: 613
	[SerializeField]
	private AudioClip m_finishFanfare;

	// Token: 0x04000266 RID: 614
	[SerializeField]
	private AudioClip m_drumRoll;

	// Token: 0x04000267 RID: 615
	public List<AwardScenePlayer> players = new List<AwardScenePlayer>();

	// Token: 0x04000268 RID: 616
	public List<GamePlayer> placements;

	// Token: 0x04000269 RID: 617
	private List<short> loadedPlayers = new List<short>();

	// Token: 0x0400026A RID: 618
	private Transform root;

	// Token: 0x0400026C RID: 620
	private ActionTimer stateTimer = new ActionTimer(0.1f);

	// Token: 0x0400026D RID: 621
	private GobletChallengeAwards m_awardsController;

	// Token: 0x0400026E RID: 622
	private bool m_awardsComplete;

	// Token: 0x0400026F RID: 623
	private Queue<QueuedAward> m_awardQueue = new Queue<QueuedAward>();

	// Token: 0x0200006D RID: 109
	public enum AwardSceneState
	{
		// Token: 0x04000271 RID: 625
		Loading,
		// Token: 0x04000272 RID: 626
		Intro,
		// Token: 0x04000273 RID: 627
		Playing
	}
}
