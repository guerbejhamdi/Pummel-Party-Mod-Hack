using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using I2.Loc;
using Rewired;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x02000520 RID: 1312
public class MultiplayerLobbyController : NetBehaviour
{
	// Token: 0x0600222F RID: 8751 RVA: 0x00018C4F File Offset: 0x00016E4F
	public bool SlotStatus(int i)
	{
		return this.slot_status[i];
	}

	// Token: 0x06002230 RID: 8752 RVA: 0x00018C5D File Offset: 0x00016E5D
	public ushort SlotOwner(int i)
	{
		return this.slot_owner[i];
	}

	// Token: 0x06002231 RID: 8753 RVA: 0x00018C6B File Offset: 0x00016E6B
	public short SlotLocalID(int i)
	{
		return this.slot_local_id[i];
	}

	// Token: 0x06002232 RID: 8754 RVA: 0x00018C79 File Offset: 0x00016E79
	public string SlotName(int i)
	{
		return this.slot_names[i];
	}

	// Token: 0x06002233 RID: 8755 RVA: 0x00018C87 File Offset: 0x00016E87
	public ushort SlotColor(int i)
	{
		return this.slot_color[i];
	}

	// Token: 0x06002234 RID: 8756 RVA: 0x00018C95 File Offset: 0x00016E95
	public ushort SlotSkin(int i)
	{
		return this.slot_skin[i];
	}

	// Token: 0x06002235 RID: 8757 RVA: 0x00018CA3 File Offset: 0x00016EA3
	public byte SlotCape(int i)
	{
		return this.slot_cape[i];
	}

	// Token: 0x06002236 RID: 8758 RVA: 0x00018CB1 File Offset: 0x00016EB1
	public bool SlotIsAI(int i)
	{
		return this.slot_is_ai[i];
	}

	// Token: 0x06002237 RID: 8759 RVA: 0x00018CBF File Offset: 0x00016EBF
	public BotDifficulty BotDifficulty(int i)
	{
		return (BotDifficulty)this.slot_bot_difficulty[i];
	}

	// Token: 0x06002238 RID: 8760 RVA: 0x00018CCD File Offset: 0x00016ECD
	public bool ColorStatus(int i)
	{
		return this.color_status[i];
	}

	// Token: 0x06002239 RID: 8761 RVA: 0x00018CD7 File Offset: 0x00016ED7
	public bool HatStatus(int i)
	{
		return this.hat_status[i];
	}

	// Token: 0x0600223A RID: 8762 RVA: 0x00018CE1 File Offset: 0x00016EE1
	public bool SkinStatus(int i)
	{
		return this.skin_status[i];
	}

	// Token: 0x0600223B RID: 8763 RVA: 0x00018CEB File Offset: 0x00016EEB
	public bool IsLocalPlayerInSlot(int playerID)
	{
		return this.localPlayerInSlot[playerID];
	}

	// Token: 0x0600223C RID: 8764 RVA: 0x000D1A90 File Offset: 0x000CFC90
	public int GetLocalPlayerSlot(int playerID)
	{
		if (!this.localPlayerInSlot[playerID])
		{
			return -1;
		}
		for (int i = 0; i < 8; i++)
		{
			if (this.slot_owner[i] == NetSystem.MyPlayer.UserID && (int)this.slot_local_id[i] == playerID)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x0600223D RID: 8765 RVA: 0x00018CF5 File Offset: 0x00016EF5
	public List<Name> Names
	{
		get
		{
			return this.names;
		}
	}

	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x0600223E RID: 8766 RVA: 0x00018CFD File Offset: 0x00016EFD
	public UIController UIController
	{
		get
		{
			return this.ui_controller;
		}
	}

	// Token: 0x0600223F RID: 8767 RVA: 0x000D1AE0 File Offset: 0x000CFCE0
	public override void OnNetInitialize()
	{
		GameManager.LobbyController = this;
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < 7; i++)
			{
				NetArray<byte> netArray = this.lobby_options;
				int index = i;
				string str = "LobbySettings_";
				LobbyOption lobbyOption = (LobbyOption)i;
				netArray[index] = (byte)RBPrefs.GetInt(str + lobbyOption.ToString(), GameManager.LobbyOptionsDefaults[i]);
			}
		}
		this.slot_status.ArrayRecieve = new ArrayRecieveProxy(this.SlotStatusRecieve);
		this.slot_names.ArrayRecieve = new ArrayRecieveProxy(this.SlotNameRecieve);
		this.slot_owner.ArrayRecieve = new ArrayRecieveProxy(this.SlotOwnerRecieve);
		this.slot_local_id.ArrayRecieve = new ArrayRecieveProxy(this.SlotLocalIDRecieve);
		this.slot_color.ArrayRecieve = new ArrayRecieveProxy(this.SlotColorRecieve);
		this.slot_skin.ArrayRecieve = new ArrayRecieveProxy(this.SlotSkinRecieve);
		this.slot_hat.ArrayRecieve = new ArrayRecieveProxy(this.SlotHatRecieve);
		this.slot_cape.ArrayRecieve = new ArrayRecieveProxy(this.SlotCapeRecieve);
		this.lobby_options.ArrayRecieve = new ArrayRecieveProxy(this.LobbyOptionsRecieve);
		this.slot_ready.ArrayRecieve = new ArrayRecieveProxy(this.SlotReadyRecieve);
		this.slot_is_ai.ArrayRecieve = new ArrayRecieveProxy(this.SlotIsAIRecieve);
		this.slot_bot_difficulty.ArrayRecieve = new ArrayRecieveProxy(this.SlotBotDifficulty);
		this.ruleset_data.ArrayRecieve = new ArrayRecieveProxy(this.OnRecieveRulesetData);
		if (NetSystem.IsServer)
		{
			this.WriteRulesetData();
		}
		else
		{
			this.ReadRulesetData();
		}
		this.ui_controller = GameManager.MainMenuUIController;
		this.scene = GameManager.MultiplayerLobbyScene;
		this.scene.Show();
		this.player_connected_event = new PlayerConnectedEventHandler(this.PlayerConnected);
		this.host_connected_event = new HostConnectedEventHandler(this.HostConnected);
		this.player_disconnected_event = new PlayerDisconnectedEventHandler(this.PlayerDisconnected);
		NetSystem.PlayerConnected += this.player_connected_event;
		NetSystem.HostConnected += this.host_connected_event;
		NetSystem.PlayerDisconnected += this.player_disconnected_event;
		this.color_status = new bool[GameManager.GetPlayerColorCount()];
		this.skin_status = new bool[GameManager.GetPlayerSkinCount()];
		this.hat_status = new bool[GameManager.GetPlayerHatCount()];
		this.names = new List<Name>();
		for (int j = 0; j < 8; j++)
		{
			string text = NetSystem.MyPlayer.Name;
			if (j > 0)
			{
				text = text + "(" + (j + 1).ToString() + ")";
			}
			this.names.Add(new Name(text, NameStatus.Free, false));
		}
		for (int k = 0; k < Settings.Names.Count; k++)
		{
			this.names.Add(new Name(Settings.Names[k], NameStatus.Free, true));
		}
		base.StartCoroutine(this.UpdateAllLobbyOptions());
	}

	// Token: 0x06002240 RID: 8768 RVA: 0x000D1DBC File Offset: 0x000CFFBC
	private void WriteRulesetData()
	{
		byte[] array = GameManager.RulesetManager.ActiveRuleset.Save(false);
		if (array != null)
		{
			this.ruleset_data.Resize(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				this.ruleset_data.Set(i, array[i]);
			}
			Debug.LogWarning("Writing ruleset to netvar " + array.Length.ToString() + " bytes");
			return;
		}
		Debug.LogError("Writing ruleset to netvar failed, data was null");
	}

	// Token: 0x06002241 RID: 8769 RVA: 0x000D1E34 File Offset: 0x000D0034
	private void ReadRulesetData()
	{
		byte[] data = this.ruleset_data.GetData();
		GameManager.RulesetManager.SetHostRuleset(data);
		Debug.LogError("Reading ruleset from netvar " + data.Length.ToString() + " bytes");
	}

	// Token: 0x06002242 RID: 8770 RVA: 0x00018D05 File Offset: 0x00016F05
	public IEnumerator UpdateAllLobbyOptions()
	{
		int num;
		for (int i = 0; i < this.lobby_options.Length; i = num)
		{
			yield return null;
			this.SetLobbyOption((LobbyOption)i, (int)this.lobby_options[i]);
			num = i + 1;
		}
		yield break;
	}

	// Token: 0x06002243 RID: 8771 RVA: 0x00018D14 File Offset: 0x00016F14
	public IEnumerator UpdateAllSteamLobbyData()
	{
		int num;
		for (int i = 0; i < this.lobby_options.Length; i = num)
		{
			yield return null;
			this.SetLobbyOptionSteamData((LobbyOption)i, (int)this.lobby_options[i]);
			num = i + 1;
		}
		SteamMatchmaking.SetLobbyMemberLimit((CSteamID)GameManager.CurrentLobby, GameManager.LobbyMaxPlayers);
		yield break;
	}

	// Token: 0x06002244 RID: 8772 RVA: 0x000D1E78 File Offset: 0x000D0078
	public void SetLobbyOptionSteamData(LobbyOption lobbyOption, int val)
	{
		if (NetSystem.IsServer && SteamManager.Initialized)
		{
			string pchValue = val.ToString();
			SteamMatchmaking.SetLobbyData((CSteamID)GameManager.CurrentLobby, lobbyOption.ToString(), pchValue);
		}
	}

	// Token: 0x06002245 RID: 8773 RVA: 0x0000398C File Offset: 0x00001B8C
	public override void OnNetDestroy()
	{
	}

	// Token: 0x06002246 RID: 8774 RVA: 0x000D1EBC File Offset: 0x000D00BC
	public void OnDestroy()
	{
		if (this.scene != null && this.scene.gameObject != null)
		{
			this.scene.Hide();
		}
		if (SteamManager.Initialized)
		{
			SteamMatchmaking.LeaveLobby((CSteamID)GameManager.CurrentLobby);
		}
		NetSystem.PlayerConnected -= this.player_connected_event;
		NetSystem.HostConnected -= this.host_connected_event;
		NetSystem.PlayerDisconnected -= this.player_disconnected_event;
		Debug.Log("Destroying Lobby Controlller");
		GameManager.LobbyController = null;
		this.SaveLobbySettings();
	}

	// Token: 0x06002247 RID: 8775 RVA: 0x00018D23 File Offset: 0x00016F23
	private void OnDisable()
	{
		this.SaveLobbySettings();
	}

	// Token: 0x06002248 RID: 8776 RVA: 0x000D1F44 File Offset: 0x000D0144
	private void SaveLobbySettings()
	{
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < 7; i++)
			{
				string str = "LobbySettings_";
				LobbyOption lobbyOption = (LobbyOption)i;
				RBPrefs.SetInt(str + lobbyOption.ToString(), (int)this.lobby_options[i]);
			}
			RBPrefs.Save();
		}
	}

	// Token: 0x06002249 RID: 8777 RVA: 0x00018D2B File Offset: 0x00016F2B
	public IEnumerator Start()
	{
		yield return null;
		this.UpdateLobbyUI();
		yield break;
	}

	// Token: 0x0600224A RID: 8778 RVA: 0x000D1F94 File Offset: 0x000D0194
	public void Update()
	{
		if (!this.level_load_waiting && !GameManager.IsGamePaused)
		{
			foreach (Player player in ReInput.players.GetPlayers(false))
			{
				if (!this.localPlayerInSlot[player.id] && !GameManager.IsGamePaused && player.GetButtonDown(InputActions.Accept) && this.ui_controller.MultiplayerLobbyWindow.Visible)
				{
					this.RequestSlot((short)player.id, -1);
				}
			}
		}
		if (this.level_load_waiting && Time.time > this.level_load_time)
		{
			Debug.Log("Level Load Wait Time Complete");
			NetGameServer.LoadLevel(this.level_to_load);
			this.level_load_waiting = false;
		}
		if (this.dirty)
		{
			this.UpdatePlayerData();
			this.UpdateLobbyUI();
		}
		for (int i = 0; i < this.slot_owner.Length; i++)
		{
			this.localPlayerInSlot[i] = false;
		}
		for (int j = 0; j < this.slot_owner.Length; j++)
		{
			if (this.slot_status[j] && this.slot_owner[j] == NetSystem.MyPlayer.UserID && !this.slot_is_ai[j])
			{
				this.localPlayerInSlot[(int)this.slot_local_id[j]] = true;
			}
		}
		if (NetSystem.IsServer)
		{
			if (GameManager.RulesetManager.RulesetDataChanged)
			{
				this.WriteRulesetData();
				GameManager.RulesetManager.RulesetDataChanged = false;
			}
		}
		else if (this.m_rulesetDataChanged)
		{
			this.ReadRulesetData();
			this.m_rulesetDataChanged = false;
		}
		if (NetSystem.IsServer && SteamManager.Initialized && Time.time - this.lastUpdate > 2f)
		{
			string pchValue = SteamUtils.GetServerRealTime().ToString();
			SteamMatchmaking.SetLobbyData((CSteamID)GameManager.CurrentLobby, "time", pchValue);
			this.lastUpdate = Time.time;
		}
	}

	// Token: 0x0600224B RID: 8779 RVA: 0x00018D3A File Offset: 0x00016F3A
	public void BeginCountdown()
	{
		base.StartCoroutine("StartCountdown");
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCStartCountdown", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x0600224C RID: 8780 RVA: 0x00018D60 File Offset: 0x00016F60
	private IEnumerator StartCountdown()
	{
		RBPrefs.Save();
		this.ui_controller.SetLobbyState(MainMenuWindowState.Disabled);
		float countdownTime = 5f;
		float countdownStartTime = Time.time;
		float endTime = countdownStartTime + countdownTime;
		this.ui_controller.gameStartCountdownWnd.SetState(MainMenuWindowState.Visible);
		Text text = this.ui_controller.gameStartCountdownWnd.transform.Find("Window/Lbl").GetComponent<Text>();
		this.ui_controller.gameStartCountdownWnd.transform.Find("CancelBtn").gameObject.SetActive(NetSystem.IsServer);
		int secondCount = 0;
		while (Time.time < endTime)
		{
			int num = (int)(countdownTime - (float)secondCount);
			string translation = LocalizationManager.GetTranslation("Game Start", true, 0, true, false, null, null, true);
			string translation2 = LocalizationManager.GetTranslation("Seconds", true, 0, true, false, null, null, true);
			string translation3 = LocalizationManager.GetTranslation("Second", true, 0, true, false, null, null, true);
			text.text = string.Concat(new string[]
			{
				translation,
				" ",
				num.ToString(),
				".. ",
				(num > 1) ? translation2 : translation3
			});
			int num2 = secondCount;
			secondCount = num2 + 1;
			AudioSystem.PlayOneShot("ButtonPress01_SFXR", 1f, 0f);
			yield return new WaitForSeconds(countdownStartTime + (float)secondCount - Time.time);
		}
		if (GameManager.partyGameMode == PartyGameMode.BoardGame)
		{
			AudioSystem.PlayMusic(null, 1f, 1f);
		}
		GameManager.FixGlobalIDs();
		StatTracker.ResetStats();
		if (NetSystem.IsServer)
		{
			if (GameManager.partyGameMode == PartyGameMode.BoardGame)
			{
				GameRuleset activeRuleset = GameManager.RulesetManager.ActiveRuleset;
				if (activeRuleset != null)
				{
					BoardModifier.ActiveModifiers = activeRuleset.Modifiers.GenerateBoardModifiers(GameManager.CurMap.sceneName);
				}
				else
				{
					BoardModifier.ActiveModifiers = new List<BoardModifier>();
				}
				BoardModifier.InitializeModifiers();
				string level = BoardModifier.OnModifyMapScene(GameManager.CurMap.sceneName);
				this.LoadLevel(level);
			}
			else
			{
				this.LoadLevel("MinigamesOnly_Scene");
			}
		}
		yield break;
	}

	// Token: 0x0600224D RID: 8781 RVA: 0x000D2184 File Offset: 0x000D0384
	public void CancelCountdown()
	{
		base.StopCoroutine("StartCountdown");
		this.ui_controller.gameStartCountdownWnd.SetState(MainMenuWindowState.Hidden);
		this.ui_controller.SetLobbyState(MainMenuWindowState.Visible);
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCCancelCountdown", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x0600224E RID: 8782 RVA: 0x000D21D4 File Offset: 0x000D03D4
	private void LoadLevel(string level)
	{
		Debug.Log("Loading Level!");
		if (NetSystem.IsServer)
		{
			this.level_to_load = level;
			this.level_load_time = Time.time + 0.5f;
			this.level_load_waiting = true;
			this.ShowLoadScreen();
			base.SendRPC("RPCShowLoadScreen", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x0600224F RID: 8783 RVA: 0x00018D6F File Offset: 0x00016F6F
	private void ShowLoadScreen()
	{
		this.ui_controller.gameStartCountdownWnd.SetState(MainMenuWindowState.Hidden);
		GameManager.LoadScreen.Show();
	}

	// Token: 0x06002250 RID: 8784 RVA: 0x000D2228 File Offset: 0x000D0428
	private void CreatePlayer(NetPlayer owner, short local_id, string name, bool is_ai, short slot_index = -1)
	{
		if (NetSystem.IsServer)
		{
			if (!is_ai)
			{
				for (int i = 0; i < this.slot_status.Length; i++)
				{
					if (this.slot_status[i] && this.slot_owner[i] == owner.UserID && this.slot_local_id[i] == local_id)
					{
						Debug.LogWarning("Cannot allow local slot player to take more than one slot!");
						return;
					}
				}
			}
			if (is_ai)
			{
				string[] array = new string[]
				{
					"Bot Jack",
					"Bot Sam",
					"Bot Daniel",
					"Bot Chris",
					"Bot George",
					"Bot John",
					"Bot Rod",
					"Bot Eli"
				};
				int num = 0;
				for (int j = 0; j < 8; j++)
				{
					if (this.slot_is_ai[j])
					{
						num++;
					}
				}
				name = array[num];
			}
			if (GameManager.SaveToLoad != null)
			{
				for (int k = 0; k < (int)GameManager.SaveToLoad.playerCount; k++)
				{
					if (GameManager.SaveToLoad.players[k].name == name && !this.slot_status[k])
					{
						slot_index = (short)k;
					}
				}
			}
			if (slot_index == -1)
			{
				for (int l = 0; l < this.slot_status.Length; l++)
				{
					if (!this.slot_status[l])
					{
						slot_index = (short)l;
						break;
					}
				}
			}
			if (slot_index >= 0 && slot_index < 8 && !this.slot_status[(int)slot_index])
			{
				this.slot_status[(int)slot_index] = true;
				this.slot_owner[(int)slot_index] = owner.UserID;
				this.slot_local_id[(int)slot_index] = local_id;
				this.slot_names[(int)slot_index] = name;
				if (GameManager.SaveToLoad != null)
				{
					this.slot_color[(int)slot_index] = GameManager.SaveToLoad.players[(int)slot_index].slotColor;
					this.slot_skin[(int)slot_index] = GameManager.SaveToLoad.players[(int)slot_index].slotSkin;
					this.slot_hat[(int)slot_index] = GameManager.SaveToLoad.players[(int)slot_index].slotHat;
					this.slot_bot_difficulty[(int)slot_index] = GameManager.SaveToLoad.players[(int)slot_index].botDifficulty;
				}
				else if (is_ai)
				{
					List<ushort> list = new List<ushort>();
					for (int m = 0; m < this.color_status.Length; m++)
					{
						if (!this.color_status[m])
						{
							list.Add((ushort)m);
						}
					}
					if (list.Count == 0)
					{
						this.slot_color[(int)slot_index] = (ushort)UnityEngine.Random.Range(0, GameManager.GetPlayerColorCount());
					}
					else
					{
						this.slot_color[(int)slot_index] = list[UnityEngine.Random.Range(0, list.Count)];
					}
					this.slot_bot_difficulty[(int)slot_index] = 0;
					List<ushort> list2 = new List<ushort>();
					for (int n = 0; n < this.skin_status.Length; n++)
					{
						if (!this.skin_status[n])
						{
							list2.Add((ushort)n);
						}
					}
					if (list2.Count == 0)
					{
						this.slot_skin[(int)slot_index] = (ushort)UnityEngine.Random.Range(0, GameManager.GetPlayerSkinCount());
					}
					else
					{
						this.slot_skin[(int)slot_index] = list2[UnityEngine.Random.Range(0, list2.Count)];
					}
					List<byte> list3 = new List<byte>();
					for (int num2 = 0; num2 < this.hat_status.Length; num2++)
					{
						if (!this.hat_status[num2])
						{
							list3.Add((byte)num2);
						}
					}
					if (list3.Count == 0)
					{
						this.slot_hat[(int)slot_index] = (byte)UnityEngine.Random.Range(0, GameManager.GetPlayerHatCount());
					}
					else
					{
						this.slot_hat[(int)slot_index] = list3[UnityEngine.Random.Range(0, list3.Count)];
					}
					if (GameManager.DEBUGGING)
					{
						this.slot_cape[(int)slot_index] = (byte)UnityEngine.Random.Range(0, 10);
					}
					this.hat_status[(int)this.slot_hat[(int)slot_index]] = true;
					this.skin_status[(int)this.slot_skin[(int)slot_index]] = true;
				}
				else
				{
					this.slot_color[(int)slot_index] = (ushort)this.GetFreeColor();
					this.slot_skin[(int)slot_index] = (ushort)this.GetFreeSkin();
					this.slot_hat[(int)slot_index] = (byte)this.GetFreeHat();
					this.slot_bot_difficulty[(int)slot_index] = 0;
				}
				this.slot_is_ai[(int)slot_index] = is_ai;
			}
			this.UpdatePlayerData();
			this.UpdateLobbyUI();
		}
	}

	// Token: 0x06002251 RID: 8785 RVA: 0x000D26A0 File Offset: 0x000D08A0
	private void RemovePlayer(short slot_index, NetPlayer owner)
	{
		if (!NetSystem.IsServer)
		{
			return;
		}
		if (slot_index < 0 || slot_index >= 8)
		{
			Debug.LogWarning("Given slot index '" + slot_index.ToString() + "' is not valid!");
			return;
		}
		if (this.slot_status[(int)slot_index] && this.slot_owner[(int)slot_index] == owner.UserID)
		{
			Debug.LogWarning("Removing player: " + slot_index.ToString());
			this.ClearSlot((int)slot_index);
			this.UpdatePlayerData();
			this.UpdateLobbyUI();
			return;
		}
		Debug.LogWarning("Cannot remove player, slot is either not filled or requesting player is not in the given slot!");
	}

	// Token: 0x06002252 RID: 8786 RVA: 0x000D2730 File Offset: 0x000D0930
	public void ForceRemovePlayer(short slot_index)
	{
		if (!NetSystem.IsServer)
		{
			return;
		}
		if (slot_index < 0 || slot_index >= 8)
		{
			Debug.LogWarning("Given slot index '" + slot_index.ToString() + "' is not valid!");
			return;
		}
		if (this.slot_status[(int)slot_index])
		{
			Debug.LogWarning("Removing player: " + slot_index.ToString());
			this.ClearSlot((int)slot_index);
			return;
		}
		Debug.LogWarning("Cannot remove player, slot not filled ");
	}

	// Token: 0x06002253 RID: 8787 RVA: 0x00018D8C File Offset: 0x00016F8C
	public void UpdatePlayers()
	{
		this.UpdatePlayerData();
		this.UpdateLobbyUI();
	}

	// Token: 0x06002254 RID: 8788 RVA: 0x000D27A0 File Offset: 0x000D09A0
	private void ClearSlot(int slot_index)
	{
		this.slot_status[slot_index] = false;
		this.slot_names[slot_index] = LocalizationManager.GetTranslation("Empty", true, 0, true, false, null, null, true);
		this.slot_owner[slot_index] = 0;
		this.slot_local_id[slot_index] = -1;
		this.slot_color[slot_index] = 0;
		this.slot_skin[slot_index] = 0;
		this.slot_hat[slot_index] = 0;
		this.slot_cape[slot_index] = 0;
		this.slot_ready[slot_index] = false;
		this.slot_is_ai[slot_index] = false;
		this.slot_bot_difficulty[slot_index] = 0;
	}

	// Token: 0x06002255 RID: 8789 RVA: 0x00018D9A File Offset: 0x00016F9A
	private void PlayerConnected(NetPlayer player)
	{
		this.UpdateLobbyUI();
	}

	// Token: 0x06002256 RID: 8790 RVA: 0x000D284C File Offset: 0x000D0A4C
	private void PlayerDisconnected(NetPlayer player)
	{
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < 8; i++)
			{
				if (this.slot_status[i] && this.slot_owner[i] == player.UserID)
				{
					this.ClearSlot(i);
				}
			}
			this.UpdatePlayerData();
		}
		this.UpdateLobbyUI();
	}

	// Token: 0x06002257 RID: 8791 RVA: 0x00018D9A File Offset: 0x00016F9A
	private void HostConnected(NetPlayer player)
	{
		this.UpdateLobbyUI();
	}

	// Token: 0x06002258 RID: 8792 RVA: 0x00018D9A File Offset: 0x00016F9A
	public void UpdateAddAIButton()
	{
		this.UpdateLobbyUI();
	}

	// Token: 0x06002259 RID: 8793 RVA: 0x000D28A4 File Offset: 0x000D0AA4
	private void UpdateLobbyUI()
	{
		for (int i = 0; i < 8; i++)
		{
			this.ui_controller.lobby_slots[i].SetSlotStatus(this.slot_status[i], this.slot_names[i], this.slot_owner[i], this.slot_color[i], this.slot_skin[i], this.slot_hat[i], this.slot_cape[i], this.slot_local_id[i], this.slot_is_ai[i], this.slot_ready[i], (BotDifficulty)this.slot_bot_difficulty[i]);
		}
		for (int j = 0; j < this.ui_controller.connection_slots.Length; j++)
		{
			this.ui_controller.connection_slots[j].SetStatus(false, "");
		}
		for (int k = 0; k < NetSystem.PlayerCount; k++)
		{
			this.ui_controller.connection_slots[k].SetStatus(true, NetSystem.GetPlayerAtIndex(k).Name);
		}
		this.UpdateSlotNameButtons();
		int num = 0;
		int num2 = 0;
		bool flag = false;
		for (int l = 0; l < 8; l++)
		{
			if (this.slot_status[l])
			{
				num++;
				if (this.slot_ready[l] || this.slot_is_ai[l] || (int)this.slot_owner[l] == NetSystem.MyPlayer.Slot)
				{
					num2++;
				}
				if (this.slot_owner[l] == NetSystem.MyPlayer.UserID)
				{
					flag = true;
				}
			}
		}
		if (NetSystem.IsServer)
		{
			int num3 = (GameManager.SaveToLoad == null) ? 2 : Mathf.Max((int)GameManager.SaveToLoad.playerCount, 2);
			bool flag2 = num >= num3 && num2 >= num3 && flag;
			this.ui_controller.SetStartGameButtonState(flag2 ? BasicButtonBase.BasicButtonState.Enabled : BasicButtonBase.BasicButtonState.Disabled);
		}
		bool flag3 = num < GameManager.LobbyMaxPlayers && (NetSystem.IsServer || (SteamManager.Initialized && (SteamUser.GetSteamID().m_SteamID == 76561198031370069UL || SteamUser.GetSteamID().m_SteamID == 76561198031400496UL || SteamUser.GetSteamID().m_SteamID == 76561198096251060UL || SteamUser.GetSteamID().m_SteamID == 76561198866805546UL || SteamUser.GetSteamID().m_SteamID == 76561198835362908UL)));
		this.ui_controller.SetAddAIButtonState(flag3 ? BasicButtonBase.BasicButtonState.Enabled : BasicButtonBase.BasicButtonState.Disabled);
		this.ui_controller.SetMinigameSettingsButtonState(BasicButtonBase.BasicButtonState.Enabled);
		this.ui_controller.SetInviteButtonState((!NetGameServer.IsLocal) ? BasicButtonBase.BasicButtonState.Enabled : BasicButtonBase.BasicButtonState.Disabled);
	}

	// Token: 0x0600225A RID: 8794 RVA: 0x000D2B6C File Offset: 0x000D0D6C
	public int GetActiveCount()
	{
		int num = 0;
		for (int i = 0; i < 8; i++)
		{
			if (this.slot_status[i])
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600225B RID: 8795 RVA: 0x000D2B9C File Offset: 0x000D0D9C
	public bool SlotsFull()
	{
		int num = 0;
		for (int i = 0; i < 8; i++)
		{
			if (this.slot_status[i])
			{
				num++;
			}
		}
		return num == GameManager.LobbyMaxPlayers;
	}

	// Token: 0x0600225C RID: 8796 RVA: 0x000D2BD4 File Offset: 0x000D0DD4
	private void UpdatePlayerData()
	{
		this.dirty = false;
		GameManager.ClearPlayers();
		for (int i = 0; i < this.color_status.Length; i++)
		{
			this.color_status[i] = false;
		}
		for (int j = 0; j < this.skin_status.Length; j++)
		{
			this.skin_status[j] = false;
		}
		for (int k = 0; k < this.hat_status.Length; k++)
		{
			this.hat_status[k] = false;
		}
		for (int l = 0; l < this.slot_status.Length; l++)
		{
			if (this.slot_status[l])
			{
				bool local = this.slot_owner[l] == NetSystem.MyPlayer.UserID;
				NetPlayer player = NetSystem.GetPlayer(this.slot_owner[l]);
				GameManager.AddPlayer(this.slot_names[l], local, this.slot_local_id[l], (short)l, this.slot_color[l], this.slot_skin[l], this.slot_hat[l], this.slot_cape[l], this.slot_is_ai[l], (BotDifficulty)this.slot_bot_difficulty[l], player);
				this.color_status[(int)this.slot_color[l]] = true;
				this.skin_status[(int)this.slot_skin[l]] = true;
				this.hat_status[(int)this.slot_hat[l]] = true;
			}
		}
	}

	// Token: 0x0600225D RID: 8797 RVA: 0x00018DA2 File Offset: 0x00016FA2
	public void AddAI()
	{
		if (NetSystem.IsServer)
		{
			this.CreatePlayer(NetSystem.MyPlayer, -1, "AI", true, -1);
			return;
		}
		base.SendRPC("RPCRequestAISlot", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
	}

	// Token: 0x0600225E RID: 8798 RVA: 0x000D2D44 File Offset: 0x000D0F44
	public void RequestSlot(short local_id, short slot_index = -1)
	{
		string text = "";
		for (int i = 0; i < 8; i++)
		{
			if (this.names[i].status == NameStatus.Free)
			{
				text = this.names[i].name;
				break;
			}
		}
		if (NetSystem.IsServer)
		{
			this.CreatePlayer(NetSystem.MyPlayer, local_id, text, false, slot_index);
			return;
		}
		base.SendRPC("RPCRequestSlot", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			local_id,
			text,
			slot_index
		});
	}

	// Token: 0x0600225F RID: 8799 RVA: 0x000D2DCC File Offset: 0x000D0FCC
	public void RemoveLocalPlayerFromSlot(Player player)
	{
		for (int i = 0; i < this.slot_owner.Length; i++)
		{
			if (this.slot_status[i] && this.slot_owner[i] == NetSystem.MyPlayer.UserID && !this.slot_is_ai[i] && this.localPlayerInSlot[(int)this.slot_local_id[i]] && (int)this.slot_local_id[i] == player.id)
			{
				this.LeaveSlot((short)i);
			}
		}
	}

	// Token: 0x06002260 RID: 8800 RVA: 0x00018DD0 File Offset: 0x00016FD0
	public void LeaveSlot(short slot_index)
	{
		if (NetSystem.IsServer)
		{
			this.RemovePlayer(slot_index, NetSystem.MyPlayer);
			return;
		}
		base.SendRPC("RPCLeaveSlot", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			slot_index
		});
	}

	// Token: 0x06002261 RID: 8801 RVA: 0x000D2E54 File Offset: 0x000D1054
	public void SetSlotColor(short slot_index, ushort color_index)
	{
		if (NetSystem.IsServer)
		{
			if (!this.color_status[(int)color_index])
			{
				this.color_status[(int)this.slot_color[(int)slot_index]] = false;
				this.color_status[(int)color_index] = true;
				this.slot_color[(int)slot_index] = color_index;
				this.dirty = true;
				return;
			}
		}
		else
		{
			base.SendRPC("RPCRequestSlotColorChange", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				slot_index,
				color_index
			});
		}
	}

	// Token: 0x06002262 RID: 8802 RVA: 0x000D2EC8 File Offset: 0x000D10C8
	public void SetSlotBotDifficulty(short slot_index, byte difficulty)
	{
		if (NetSystem.IsServer)
		{
			this.slot_bot_difficulty.Set((int)slot_index, difficulty);
			this.dirty = true;
			return;
		}
		base.SendRPC("RPCRequestSlotBotDifficultyChange", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			slot_index,
			difficulty
		});
	}

	// Token: 0x06002263 RID: 8803 RVA: 0x000D2F18 File Offset: 0x000D1118
	public void SetSlotSkin(short slot_index, ushort skin_index)
	{
		if (NetSystem.IsServer)
		{
			this.skin_status[(int)this.slot_skin[(int)slot_index]] = false;
			this.skin_status[(int)skin_index] = true;
			this.slot_skin.Set((int)slot_index, skin_index);
			this.dirty = true;
			return;
		}
		base.SendRPC("RPCRequestSlotSkinChange", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			slot_index,
			skin_index
		});
	}

	// Token: 0x06002264 RID: 8804 RVA: 0x000D2F84 File Offset: 0x000D1184
	public void SetSlotHat(short slot_index, byte hat_index)
	{
		if (NetSystem.IsServer)
		{
			if (!this.hat_status[(int)hat_index])
			{
				this.hat_status[(int)this.slot_hat[(int)slot_index]] = false;
				this.hat_status[(int)hat_index] = true;
				this.slot_hat.Set((int)slot_index, hat_index);
				this.dirty = true;
				return;
			}
		}
		else
		{
			base.SendRPC("RPCRequestSlotHatChange", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				slot_index,
				hat_index
			});
		}
	}

	// Token: 0x06002265 RID: 8805 RVA: 0x000D2FF8 File Offset: 0x000D11F8
	public void SetSlotCape(short slot_index, byte cape_index)
	{
		if (NetSystem.IsServer)
		{
			this.slot_cape.Set((int)slot_index, cape_index);
			this.dirty = true;
			return;
		}
		base.SendRPC("RPCRequestSlotCapeChange", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			slot_index,
			cape_index
		});
	}

	// Token: 0x06002266 RID: 8806 RVA: 0x00018E01 File Offset: 0x00017001
	public void SetSlotName(short slot_index, string name)
	{
		if (NetSystem.IsServer)
		{
			this.slot_names[(int)slot_index] = name;
			this.UpdateLobbyUI();
			return;
		}
		base.SendRPC("RPCRequestSlotNameChange", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			slot_index,
			name
		});
	}

	// Token: 0x06002267 RID: 8807 RVA: 0x000D3048 File Offset: 0x000D1248
	public void SetSlotReadyState(short slot_index, bool state)
	{
		if (NetSystem.IsServer)
		{
			this.slot_ready[(int)slot_index] = state;
			this.UpdateLobbyUI();
			return;
		}
		base.SendRPC("RPCRequestSlotReadyChange", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			slot_index,
			state
		});
	}

	// Token: 0x06002268 RID: 8808 RVA: 0x000D3094 File Offset: 0x000D1294
	private int GetFreeColor()
	{
		for (int i = 0; i < this.color_status.Length; i++)
		{
			if (!this.color_status[i])
			{
				this.color_status[i] = true;
				return i;
			}
		}
		Debug.Log("returning color 0");
		return 0;
	}

	// Token: 0x06002269 RID: 8809 RVA: 0x000D30D4 File Offset: 0x000D12D4
	private int GetFreeSkin()
	{
		for (int i = 0; i < this.skin_status.Length; i++)
		{
			if (!this.skin_status[i])
			{
				this.skin_status[i] = true;
				return i;
			}
		}
		Debug.Log("returning skin 0");
		return 0;
	}

	// Token: 0x0600226A RID: 8810 RVA: 0x000D3114 File Offset: 0x000D1314
	private int GetFreeHat()
	{
		for (int i = 0; i < this.hat_status.Length; i++)
		{
			if (!this.hat_status[i])
			{
				this.hat_status[i] = true;
				return i;
			}
		}
		Debug.Log("returning hat 0");
		return 0;
	}

	// Token: 0x0600226B RID: 8811 RVA: 0x000D3154 File Offset: 0x000D1354
	public void UpdateSlotNameButtons()
	{
		UIMultiplayerLobbySlot[] lobby_slots = this.ui_controller.lobby_slots;
		for (int i = 0; i < lobby_slots.Length; i++)
		{
			lobby_slots[i].UpdateNameButtonStatus();
		}
		Settings.SetNames(this.names);
	}

	// Token: 0x0600226C RID: 8812 RVA: 0x000D3190 File Offset: 0x000D1390
	public void SetLobbyOption(LobbyOption lobbyOption, int val)
	{
		if (NetSystem.IsServer)
		{
			this.lobby_options[(int)lobbyOption] = (byte)val;
			if (SteamManager.Initialized)
			{
				string pchValue = val.ToString();
				SteamMatchmaking.SetLobbyData((CSteamID)GameManager.CurrentLobby, lobbyOption.ToString(), pchValue);
			}
		}
		this.UIController.SetLobbyOption(lobbyOption, val);
		GameManager.lobbyOptions[(int)lobbyOption] = (byte)val;
	}

	// Token: 0x0600226D RID: 8813 RVA: 0x00018E3D File Offset: 0x0001703D
	public byte GetLobbyOption(LobbyOption lobbyOption)
	{
		return this.lobby_options[(int)lobbyOption];
	}

	// Token: 0x0600226E RID: 8814 RVA: 0x00018E4B File Offset: 0x0001704B
	public void SlotStatusRecieve(int index, object val)
	{
		this.dirty = true;
	}

	// Token: 0x0600226F RID: 8815 RVA: 0x00018E4B File Offset: 0x0001704B
	public void SlotOwnerRecieve(int index, object val)
	{
		this.dirty = true;
	}

	// Token: 0x06002270 RID: 8816 RVA: 0x00018E4B File Offset: 0x0001704B
	public void SlotNameRecieve(int index, object val)
	{
		this.dirty = true;
	}

	// Token: 0x06002271 RID: 8817 RVA: 0x00018E4B File Offset: 0x0001704B
	public void SlotLocalIDRecieve(int index, object val)
	{
		this.dirty = true;
	}

	// Token: 0x06002272 RID: 8818 RVA: 0x00018E4B File Offset: 0x0001704B
	public void SlotColorRecieve(int index, object val)
	{
		this.dirty = true;
	}

	// Token: 0x06002273 RID: 8819 RVA: 0x00018E4B File Offset: 0x0001704B
	public void SlotSkinRecieve(int index, object val)
	{
		this.dirty = true;
	}

	// Token: 0x06002274 RID: 8820 RVA: 0x00018E4B File Offset: 0x0001704B
	public void SlotHatRecieve(int index, object val)
	{
		this.dirty = true;
	}

	// Token: 0x06002275 RID: 8821 RVA: 0x00018E4B File Offset: 0x0001704B
	public void SlotCapeRecieve(int index, object val)
	{
		this.dirty = true;
	}

	// Token: 0x06002276 RID: 8822 RVA: 0x00018E4B File Offset: 0x0001704B
	public void SlotReadyRecieve(int index, object val)
	{
		this.dirty = true;
	}

	// Token: 0x06002277 RID: 8823 RVA: 0x00018E4B File Offset: 0x0001704B
	public void SlotIsAIRecieve(int index, object val)
	{
		this.dirty = true;
	}

	// Token: 0x06002278 RID: 8824 RVA: 0x00018E4B File Offset: 0x0001704B
	public void SlotBotDifficulty(int index, object val)
	{
		this.dirty = true;
	}

	// Token: 0x06002279 RID: 8825 RVA: 0x00018E54 File Offset: 0x00017054
	public void OnRecieveRulesetData(int index, object val)
	{
		this.m_rulesetDataChanged = true;
	}

	// Token: 0x0600227A RID: 8826 RVA: 0x00018E5D File Offset: 0x0001705D
	public void LobbyOptionsRecieve(int index, object val)
	{
		this.SetLobbyOption((LobbyOption)index, (int)((byte)val));
	}

	// Token: 0x0600227B RID: 8827 RVA: 0x00018E6C File Offset: 0x0001706C
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCRequestSlot(NetPlayer sender, short local_id, string name, short slot_index)
	{
		this.CreatePlayer(sender, local_id, name, false, slot_index);
	}

	// Token: 0x0600227C RID: 8828 RVA: 0x00018E7A File Offset: 0x0001707A
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCRequestAISlot(NetPlayer sender)
	{
		this.CreatePlayer(sender, -1, "AI", true, -1);
	}

	// Token: 0x0600227D RID: 8829 RVA: 0x00018E8B File Offset: 0x0001708B
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCLeaveSlot(NetPlayer sender, short slot_index)
	{
		this.RemovePlayer(slot_index, sender);
	}

	// Token: 0x0600227E RID: 8830 RVA: 0x00018E95 File Offset: 0x00017095
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCShowLoadScreen(NetPlayer sender)
	{
		this.ShowLoadScreen();
	}

	// Token: 0x0600227F RID: 8831 RVA: 0x00018E9D File Offset: 0x0001709D
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCStartCountdown(NetPlayer sender)
	{
		this.BeginCountdown();
	}

	// Token: 0x06002280 RID: 8832 RVA: 0x00018EA5 File Offset: 0x000170A5
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCCancelCountdown(NetPlayer sender)
	{
		this.CancelCountdown();
	}

	// Token: 0x06002281 RID: 8833 RVA: 0x00018EAD File Offset: 0x000170AD
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCRequestSlotColorChange(NetPlayer sender, short slot_index, ushort color_index)
	{
		if (sender.UserID == this.slot_owner[(int)slot_index])
		{
			this.SetSlotColor(slot_index, color_index);
		}
	}

	// Token: 0x06002282 RID: 8834 RVA: 0x00018ECB File Offset: 0x000170CB
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCRequestSlotSkinChange(NetPlayer sender, short slot_index, ushort skin_index)
	{
		if (sender.UserID == this.slot_owner[(int)slot_index])
		{
			this.SetSlotSkin(slot_index, skin_index);
		}
	}

	// Token: 0x06002283 RID: 8835 RVA: 0x00018EE9 File Offset: 0x000170E9
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCRequestSlotBotDifficultyChange(NetPlayer sender, short slot_index, byte difficulty)
	{
		if (sender.UserID == this.slot_owner[(int)slot_index])
		{
			this.SetSlotBotDifficulty(slot_index, difficulty);
		}
	}

	// Token: 0x06002284 RID: 8836 RVA: 0x00018F07 File Offset: 0x00017107
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCRequestSlotHatChange(NetPlayer sender, short slot_index, byte hat_index)
	{
		if (sender.UserID == this.slot_owner[(int)slot_index])
		{
			this.SetSlotHat(slot_index, hat_index);
		}
	}

	// Token: 0x06002285 RID: 8837 RVA: 0x00018F25 File Offset: 0x00017125
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCRequestSlotCapeChange(NetPlayer sender, short slot_index, byte cape_index)
	{
		if (sender.UserID == this.slot_owner[(int)slot_index])
		{
			this.SetSlotCape(slot_index, cape_index);
		}
	}

	// Token: 0x06002286 RID: 8838 RVA: 0x00018F43 File Offset: 0x00017143
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCRequestSlotNameChange(NetPlayer sender, short slot_index, string name)
	{
		if (sender.UserID == this.slot_owner[(int)slot_index])
		{
			this.SetSlotName(slot_index, name);
		}
	}

	// Token: 0x06002287 RID: 8839 RVA: 0x00018F61 File Offset: 0x00017161
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCRequestSlotReadyChange(NetPlayer sender, short slot_index, bool state)
	{
		if (sender.UserID == this.slot_owner[(int)slot_index])
		{
			this.SetSlotReadyState(slot_index, state);
		}
	}

	// Token: 0x06002288 RID: 8840 RVA: 0x000D31F8 File Offset: 0x000D13F8
	public void SetLobbySettings(byte[] settings)
	{
		for (int i = 0; i < settings.Length; i++)
		{
			this.SetLobbyOption((LobbyOption)i, (int)settings[i]);
		}
	}

	// Token: 0x06002289 RID: 8841 RVA: 0x00018F7F File Offset: 0x0001717F
	public void CancelLoad()
	{
		GameManager.SaveToLoad = null;
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCCancelLoad", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x0600228A RID: 8842 RVA: 0x00018F9F File Offset: 0x0001719F
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCCancelLoad(NetPlayer sender)
	{
		this.CancelLoad();
	}

	// Token: 0x0600228B RID: 8843 RVA: 0x000D3220 File Offset: 0x000D1420
	public void LoadSave()
	{
		if (NetSystem.IsServer)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					GameManager.SaveToLoad.Serialize(binaryWriter);
					byte[] array = ((MemoryStream)binaryWriter.BaseStream).ToArray();
					base.SendRPC("RPCLoadSave", NetRPCDelivery.RELIABLE_UNORDERED, new object[]
					{
						array
					});
				}
			}
		}
	}

	// Token: 0x0600228C RID: 8844 RVA: 0x000D32A8 File Offset: 0x000D14A8
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCLoadSave(NetPlayer sender, byte[] bytes)
	{
		using (MemoryStream memoryStream = new MemoryStream(bytes))
		{
			using (BinaryReader binaryReader = new BinaryReader(memoryStream))
			{
				TurnSave turnSave = new TurnSave();
				turnSave.Serialize(binaryReader);
				GameManager.SaveToLoad = turnSave;
			}
		}
	}

	// Token: 0x04002551 RID: 9553
	private const int SLOT_COUNT = 8;

	// Token: 0x04002552 RID: 9554
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetArray<bool> slot_status = new NetArray<bool>(8);

	// Token: 0x04002553 RID: 9555
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetArray<ushort> slot_owner = new NetArray<ushort>(8);

	// Token: 0x04002554 RID: 9556
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetArray<bool> slot_ready = new NetArray<bool>(8);

	// Token: 0x04002555 RID: 9557
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetArray<short> slot_local_id = new NetArray<short>(8);

	// Token: 0x04002556 RID: 9558
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetArray<string> slot_names = new NetArray<string>(8);

	// Token: 0x04002557 RID: 9559
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetArray<ushort> slot_color = new NetArray<ushort>(8);

	// Token: 0x04002558 RID: 9560
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetArray<ushort> slot_skin = new NetArray<ushort>(8);

	// Token: 0x04002559 RID: 9561
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetArray<byte> slot_hat = new NetArray<byte>(8);

	// Token: 0x0400255A RID: 9562
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetArray<byte> slot_cape = new NetArray<byte>(8);

	// Token: 0x0400255B RID: 9563
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetArray<bool> slot_is_ai = new NetArray<bool>(8);

	// Token: 0x0400255C RID: 9564
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetArray<byte> slot_bot_difficulty = new NetArray<byte>(8);

	// Token: 0x0400255D RID: 9565
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetArray<byte> lobby_options = new NetArray<byte>(7);

	// Token: 0x0400255E RID: 9566
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetArray<byte> ruleset_data = new NetArray<byte>(125);

	// Token: 0x0400255F RID: 9567
	private UIController ui_controller;

	// Token: 0x04002560 RID: 9568
	[HideInInspector]
	public MultiplayerLobbyScene scene;

	// Token: 0x04002561 RID: 9569
	private string level_to_load = "";

	// Token: 0x04002562 RID: 9570
	private bool level_load_waiting;

	// Token: 0x04002563 RID: 9571
	private float level_load_time;

	// Token: 0x04002564 RID: 9572
	private bool[] color_status;

	// Token: 0x04002565 RID: 9573
	private bool[] skin_status;

	// Token: 0x04002566 RID: 9574
	private bool[] hat_status;

	// Token: 0x04002567 RID: 9575
	private bool dirty;

	// Token: 0x04002568 RID: 9576
	private bool[] localPlayerInSlot = new bool[8];

	// Token: 0x04002569 RID: 9577
	private List<Name> names;

	// Token: 0x0400256A RID: 9578
	private PlayerConnectedEventHandler player_connected_event;

	// Token: 0x0400256B RID: 9579
	private HostConnectedEventHandler host_connected_event;

	// Token: 0x0400256C RID: 9580
	private PlayerDisconnectedEventHandler player_disconnected_event;

	// Token: 0x0400256D RID: 9581
	private bool m_rulesetDataChanged;

	// Token: 0x0400256E RID: 9582
	private float lastUpdate;
}
