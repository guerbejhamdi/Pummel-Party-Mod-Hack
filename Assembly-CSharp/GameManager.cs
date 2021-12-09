using System;
using System.Collections.Generic;
using System.IO;
using Prime31.TransitionKit;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x020003E6 RID: 998
public static class GameManager
{
	// Token: 0x170002FD RID: 765
	// (get) Token: 0x06001B0D RID: 6925 RVA: 0x00013E2F File Offset: 0x0001202F
	// (set) Token: 0x06001B0E RID: 6926 RVA: 0x00013E36 File Offset: 0x00012036
	public static GameState CurState
	{
		get
		{
			return GameManager.cur_game_state;
		}
		set
		{
			GameManager.cur_game_state = value;
		}
	}

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x06001B0F RID: 6927 RVA: 0x00013E3E File Offset: 0x0001203E
	// (set) Token: 0x06001B10 RID: 6928 RVA: 0x00013E45 File Offset: 0x00012045
	public static MultiplayerLobbyScene MultiplayerLobbyScene { get; set; }

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06001B11 RID: 6929 RVA: 0x00013E4D File Offset: 0x0001204D
	public static MinigameDefinition CurrentMinigameDef
	{
		get
		{
			return GameManager.current_minigame_def;
		}
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x06001B12 RID: 6930 RVA: 0x00013E54 File Offset: 0x00012054
	// (set) Token: 0x06001B13 RID: 6931 RVA: 0x00013E5B File Offset: 0x0001205B
	public static ulong CurrentLobby { get; set; }

	// Token: 0x17000301 RID: 769
	// (get) Token: 0x06001B14 RID: 6932 RVA: 0x00013E63 File Offset: 0x00012063
	public static bool IsGamePaused
	{
		get
		{
			return GameManager.is_game_paused;
		}
	}

	// Token: 0x17000302 RID: 770
	// (get) Token: 0x06001B15 RID: 6933 RVA: 0x00013E6A File Offset: 0x0001206A
	public static GameObject BoardRoot
	{
		get
		{
			return GameManager.boardWorldObj;
		}
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x06001B16 RID: 6934 RVA: 0x00013E71 File Offset: 0x00012071
	// (set) Token: 0x06001B17 RID: 6935 RVA: 0x00013E78 File Offset: 0x00012078
	public static bool MainMenuLoaded { get; set; }

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x06001B18 RID: 6936 RVA: 0x00013E80 File Offset: 0x00012080
	public static bool HasFocus
	{
		get
		{
			return Application.isFocused;
		}
	}

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x06001B19 RID: 6937 RVA: 0x00013E87 File Offset: 0x00012087
	// (set) Token: 0x06001B1A RID: 6938 RVA: 0x000B851C File Offset: 0x000B671C
	public static bool CaptureInput
	{
		get
		{
			return GameManager.capture_input;
		}
		set
		{
			GameManager.capture_input = value;
			Debug.Log("Capture Input: " + value.ToString() + " Has Focus: " + GameManager.HasFocus.ToString());
			if (GameManager.capture_input && GameManager.HasFocus)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				return;
			}
			Cursor.lockState = CursorLockMode.None;
			if (ReInput.players != null && ReInput.players.GetPlayer(0) != null && ReInput.players.GetPlayer(0).controllers.GetLastActiveController() != null && ReInput.players.GetPlayer(0).controllers.GetLastActiveController().type != ControllerType.Joystick)
			{
				Cursor.visible = true;
			}
		}
	}

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x06001B1B RID: 6939 RVA: 0x00013E8E File Offset: 0x0001208E
	// (set) Token: 0x06001B1C RID: 6940 RVA: 0x00013E95 File Offset: 0x00012095
	public static bool PollInput
	{
		get
		{
			return GameManager.poll_input;
		}
		set
		{
			GameManager.poll_input = value;
		}
	}

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x06001B1D RID: 6941 RVA: 0x00013E9D File Offset: 0x0001209D
	// (set) Token: 0x06001B1E RID: 6942 RVA: 0x00013EA4 File Offset: 0x000120A4
	public static GameBoardController Board
	{
		get
		{
			return GameManager.game_board;
		}
		set
		{
			GameManager.game_board = value;
		}
	}

	// Token: 0x17000308 RID: 776
	// (get) Token: 0x06001B1F RID: 6943 RVA: 0x00013EAC File Offset: 0x000120AC
	// (set) Token: 0x06001B20 RID: 6944 RVA: 0x00013EB3 File Offset: 0x000120B3
	public static KeyController KeyController
	{
		get
		{
			return GameManager.key_controller;
		}
		set
		{
			GameManager.key_controller = value;
		}
	}

	// Token: 0x17000309 RID: 777
	// (get) Token: 0x06001B21 RID: 6945 RVA: 0x00013EBB File Offset: 0x000120BB
	// (set) Token: 0x06001B22 RID: 6946 RVA: 0x00013EC2 File Offset: 0x000120C2
	public static BoardLoadScreen LoadScreen
	{
		get
		{
			return GameManager.board_load_screen;
		}
		set
		{
			GameManager.board_load_screen = value;
		}
	}

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x06001B23 RID: 6947 RVA: 0x00013ECA File Offset: 0x000120CA
	public static GameUIController UIController
	{
		get
		{
			return GameManager.ui_controller;
		}
	}

	// Token: 0x1700030B RID: 779
	// (get) Token: 0x06001B24 RID: 6948 RVA: 0x00013ED1 File Offset: 0x000120D1
	// (set) Token: 0x06001B25 RID: 6949 RVA: 0x00013ED8 File Offset: 0x000120D8
	public static UIController MainMenuUIController
	{
		get
		{
			return GameManager.mainMenuUIController;
		}
		set
		{
			GameManager.mainMenuUIController = value;
		}
	}

	// Token: 0x1700030C RID: 780
	// (get) Token: 0x06001B26 RID: 6950 RVA: 0x00013EE0 File Offset: 0x000120E0
	// (set) Token: 0x06001B27 RID: 6951 RVA: 0x00013EE7 File Offset: 0x000120E7
	public static MinigameController Minigame
	{
		get
		{
			return GameManager.current_minigame;
		}
		set
		{
			GameManager.current_minigame = value;
		}
	}

	// Token: 0x1700030D RID: 781
	// (get) Token: 0x06001B28 RID: 6952 RVA: 0x00013EEF File Offset: 0x000120EF
	public static GameObject MinigameRoot
	{
		get
		{
			return GameManager.minigame_root;
		}
	}

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x06001B29 RID: 6953 RVA: 0x000B85C8 File Offset: 0x000B67C8
	public static bool UsingInputField
	{
		get
		{
			GameObject currentSelectedGameObject = EventSystem.GetSystem(0).currentSelectedGameObject;
			if (currentSelectedGameObject != null)
			{
				if (currentSelectedGameObject != GameManager.lastGameObject)
				{
					GameManager.lastGameObject = currentSelectedGameObject;
					GameManager.lastVal = EventSystem.GetSystem(0).currentSelectedGameObject.GetComponent<InputField>();
				}
			}
			else
			{
				GameManager.lastVal = false;
			}
			return GameManager.lastVal;
		}
	}

	// Token: 0x1700030F RID: 783
	// (get) Token: 0x06001B2A RID: 6954 RVA: 0x00013EF6 File Offset: 0x000120F6
	// (set) Token: 0x06001B2B RID: 6955 RVA: 0x00013EFD File Offset: 0x000120FD
	public static OptionsWindow OptionsWindow { get; set; }

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x06001B2C RID: 6956 RVA: 0x00013F05 File Offset: 0x00012105
	// (set) Token: 0x06001B2D RID: 6957 RVA: 0x00013F0C File Offset: 0x0001210C
	public static MapDetails CurMap
	{
		get
		{
			return GameManager.curMap;
		}
		set
		{
			GameManager.curMap = value;
		}
	}

	// Token: 0x17000311 RID: 785
	// (get) Token: 0x06001B2E RID: 6958 RVA: 0x00013F14 File Offset: 0x00012114
	public static bool WeaponsCacheEnabled
	{
		get
		{
			return GameManager.RulesetManager.ActiveRuleset.General.WeaponsCacheEnabled && GameManager.ItemList.HasWeaponSpaceItem();
		}
	}

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x06001B2F RID: 6959 RVA: 0x00013F38 File Offset: 0x00012138
	// (set) Token: 0x06001B30 RID: 6960 RVA: 0x00013F3F File Offset: 0x0001213F
	public static int TurnCount { get; set; }

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x06001B31 RID: 6961 RVA: 0x00013F47 File Offset: 0x00012147
	// (set) Token: 0x06001B32 RID: 6962 RVA: 0x00013F4E File Offset: 0x0001214E
	public static int WinningRelics { get; set; }

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x06001B33 RID: 6963 RVA: 0x00013F56 File Offset: 0x00012156
	// (set) Token: 0x06001B34 RID: 6964 RVA: 0x00013F5D File Offset: 0x0001215D
	public static int MinigameModeCount { get; set; }

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x06001B35 RID: 6965 RVA: 0x00013F65 File Offset: 0x00012165
	// (set) Token: 0x06001B36 RID: 6966 RVA: 0x00013F6C File Offset: 0x0001216C
	public static float TurnLength { get; set; }

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x06001B37 RID: 6967 RVA: 0x00013F74 File Offset: 0x00012174
	public static bool TurnLengthLimited
	{
		get
		{
			return GameManager.TurnLength != -1f;
		}
	}

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06001B38 RID: 6968 RVA: 0x00013F85 File Offset: 0x00012185
	// (set) Token: 0x06001B39 RID: 6969 RVA: 0x00013F8C File Offset: 0x0001218C
	public static int LobbyMaxPlayers { get; set; }

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06001B3A RID: 6970 RVA: 0x00013F94 File Offset: 0x00012194
	public static RagdollSettings RagdollSettings
	{
		get
		{
			return GameManager.ragdoll_settings;
		}
	}

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06001B3B RID: 6971 RVA: 0x00013F9B File Offset: 0x0001219B
	// (set) Token: 0x06001B3C RID: 6972 RVA: 0x00013FA2 File Offset: 0x000121A2
	public static MultiplayerLobbyController LobbyController
	{
		get
		{
			return GameManager.lobby_controller;
		}
		set
		{
			GameManager.lobby_controller = value;
		}
	}

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x06001B3D RID: 6973 RVA: 0x00013FAA File Offset: 0x000121AA
	// (set) Token: 0x06001B3E RID: 6974 RVA: 0x00013FB1 File Offset: 0x000121B1
	public static ItemList ItemList
	{
		get
		{
			return GameManager.itemList;
		}
		set
		{
			GameManager.itemList = value;
		}
	}

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x06001B3F RID: 6975 RVA: 0x00013FB9 File Offset: 0x000121B9
	// (set) Token: 0x06001B40 RID: 6976 RVA: 0x00013FC0 File Offset: 0x000121C0
	public static bool IsInitialized { get; set; }

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x06001B41 RID: 6977 RVA: 0x00013FC8 File Offset: 0x000121C8
	// (set) Token: 0x06001B42 RID: 6978 RVA: 0x00013FCF File Offset: 0x000121CF
	public static PlatformManagerHelper PlatformHelper { get; set; }

	// Token: 0x06001B43 RID: 6979 RVA: 0x0000398C File Offset: 0x00001B8C
	public static void TextBoxPause()
	{
	}

	// Token: 0x06001B44 RID: 6980 RVA: 0x00013FD7 File Offset: 0x000121D7
	public static void PauseGame(bool showUI = true)
	{
		if (!GameManager.is_game_paused)
		{
			GameManager.is_game_paused = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			if (GameManager.ui_controller != null && showUI)
			{
				GameManager.ui_controller.ShowPauseScreen();
			}
		}
	}

	// Token: 0x06001B45 RID: 6981 RVA: 0x0001400B File Offset: 0x0001220B
	public static void UnpauseGame(bool showUI = true)
	{
		if (GameManager.is_game_paused)
		{
			GameManager.is_game_paused = false;
			if (GameManager.capture_input)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			if (GameManager.ui_controller != null && showUI)
			{
				GameManager.ui_controller.HidePauseScreen();
			}
		}
	}

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x06001B46 RID: 6982 RVA: 0x00014046 File Offset: 0x00012246
	public static SceneSwitchState SwitchState
	{
		get
		{
			return GameManager.switcher.switch_state;
		}
	}

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x06001B47 RID: 6983 RVA: 0x00014052 File Offset: 0x00012252
	public static GameRulesetManager RulesetManager
	{
		get
		{
			return GameManager.m_rulesetManager;
		}
	}

	// Token: 0x06001B48 RID: 6984 RVA: 0x000B8624 File Offset: 0x000B6824
	static GameManager()
	{
		GameManager.players = new List<GamePlayer>();
		GameManager.disconnected_players = new List<GamePlayer>();
		GameManager.rand = new System.Random(Guid.NewGuid().GetHashCode());
	}

	// Token: 0x06001B49 RID: 6985 RVA: 0x000B8CDC File Offset: 0x000B6EDC
	public static void Reset()
	{
		PlayerRagdoll.DespawnAll();
		GameManager.players.Clear();
		GameManager.disconnected_players.Clear();
		GameManager.ui_controller = null;
		GameManager.capture_input = false;
		GameManager.poll_input = true;
		GameManager.UnpauseGame(true);
		GameManager.CurGameSave = null;
		StatTracker.ResetStats();
		TemporaryEffect.ClearEffects();
	}

	// Token: 0x06001B4A RID: 6986 RVA: 0x00014059 File Offset: 0x00012259
	public static void Awake()
	{
		GlyphDatabase.Instance = (Resources.Load("GlyphDatabase") as GlyphDatabase);
	}

	// Token: 0x06001B4B RID: 6987 RVA: 0x000B8D2C File Offset: 0x000B6F2C
	public static void Initialize(GameState new_state)
	{
		GameManager.cur_game_state = new_state;
		switch (GameManager.cur_game_state)
		{
		case GameState.GameBoard:
			GameManager.ui_controller = GameObject.Find("BoardCanvas").GetComponent<GameUIController>();
			GameManager.ui_controller.Initialize();
			if (GameManager.players.Count <= 0)
			{
				GameManager.AddPlayer("Chris", true, 0, 0, 0, 0, 0, 0, false, BotDifficulty.Normal, NetSystem.MyPlayer);
				GameManager.AddPlayer("Bob", true, 1, 1, 1, 1, 1, 1, false, BotDifficulty.Normal, NetSystem.MyPlayer);
			}
			GameManager.boardWorldObj = GameObject.Find("BoardWorld");
			GameManager.boardSquaresObj = GameObject.Find("BoardSquares");
			GameManager.mapSettings = GameManager.boardWorldObj.GetComponent<MapSettings>();
			break;
		case GameState.MinigamesOnly:
			GameManager.boardWorldObj = GameObject.Find("World");
			break;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/ResultScreenScene")) as GameObject;
		gameObject.name = "ResultScreenScene";
		GameManager.result_screen = gameObject.GetComponent<ResultSceenScene>();
		GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/ScoreUIRenderScene")) as GameObject;
		gameObject2.name = "ScoreUIRenderScene";
		GameManager.scoreUIScene = gameObject2.GetComponent<ScoreUIScene>();
		GameManager.IsInitialized = true;
	}

	// Token: 0x06001B4C RID: 6988 RVA: 0x0001406F File Offset: 0x0001226F
	private static void XInputChange()
	{
		GameManager.p = ReInput.players.GetPlayer(0);
	}

	// Token: 0x06001B4D RID: 6989 RVA: 0x000B8E50 File Offset: 0x000B7050
	public static void Update()
	{
		if (GameManager.ui_controller)
		{
			GameManager.ui_controller.UpdateUI();
		}
		if (!GameManager.gotPlayer && ReInput.isReady)
		{
			GameManager.p = ReInput.players.GetPlayer(0);
			GameManager.gotPlayer = true;
			GameManager.OnXInput = (GameManager.OnXInputEvent)Delegate.Combine(GameManager.OnXInput, new GameManager.OnXInputEvent(GameManager.XInputChange));
		}
		GameManager.cont = null;
		if (GameManager.p != null)
		{
			GameManager.cont = GameManager.p.controllers.GetLastActiveController();
		}
		if (GameManager.capture_input && !GameManager.is_game_paused)
		{
			if (GameManager.HasFocus && (Cursor.lockState != CursorLockMode.Locked || Cursor.visible))
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			else if (!GameManager.HasFocus && (Cursor.lockState != CursorLockMode.None || !Cursor.visible))
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
		else if (GameManager.cont != null && GameManager.cont.type == ControllerType.Joystick)
		{
			Cursor.visible = false;
		}
		if (Time.time > GameManager.m_nextTemporaryEffectUpdate)
		{
			GameManager.m_nextTemporaryEffectUpdate = Time.time + 1f;
			TemporaryEffect.UpdateEffects();
		}
	}

	// Token: 0x06001B4E RID: 6990 RVA: 0x00014081 File Offset: 0x00012281
	public static void SetupRulesets()
	{
		GameManager.m_rulesetManager = new GameRulesetManager();
		GameManager.m_rulesetManager.Load();
	}

	// Token: 0x06001B4F RID: 6991 RVA: 0x000B8F68 File Offset: 0x000B7168
	public static void LoadGameData(MinigameDefinition[] minigames, GameModifierDefinition[] modifiers)
	{
		GameManager.minigame_map = new Dictionary<string, MinigameDefinition>();
		GameManager.minigame_list = new List<MinigameDefinition>();
		for (int i = 0; i < minigames.Length; i++)
		{
			if (!GameManager.minigame_map.ContainsKey(minigames[i].minigameName))
			{
				if (minigames[i].enabled)
				{
					GameManager.WonMinigame.Add(RBPrefs.GetInt(minigames[i].minigameName + "_ACH_WON", 0) == 1);
					GameManager.LostMinigame.Add(RBPrefs.GetInt(minigames[i].minigameName + "_ACH_LOST", 0) == 1);
					GameManager.minigame_map.Add(minigames[i].minigameName, minigames[i]);
					GameManager.minigame_list.Add(minigames[i]);
				}
			}
			else
			{
				Debug.LogError("Two or more minigames share the same name '" + minigames[i].minigameName + "', minigames must have unique names!");
			}
		}
		GameManager.modifier_map = new Dictionary<int, GameModifierDefinition>();
		GameManager.modifier_list = new List<GameModifierDefinition>();
		for (int j = 0; j < modifiers.Length; j++)
		{
			if (modifiers[j].enabled)
			{
				if (modifiers[j].scriptTypeName == "")
				{
					Debug.LogError("Modifier script reference not set : " + modifiers[j].name);
				}
				else
				{
					GameModifier gameModifier = Activator.CreateInstance(Type.GetType(modifiers[j].scriptTypeName + ", Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")) as GameModifier;
					if (gameModifier != null)
					{
						int gameModifierID = gameModifier.GetGameModifierID();
						if (!GameManager.modifier_map.ContainsKey(gameModifierID))
						{
							new Modifier_MirrorMapHorizontal();
							Debug.LogError("Adding modifier " + gameModifierID.ToString() + " : " + modifiers[j].name);
							GameManager.modifier_map[gameModifierID] = modifiers[j];
							GameManager.modifier_list.Add(modifiers[j]);
						}
					}
				}
			}
		}
	}

	// Token: 0x06001B50 RID: 6992 RVA: 0x000B9128 File Offset: 0x000B7328
	public static void SetMinigameWon()
	{
		RBPrefs.SetInt(GameManager.current_minigame_def.minigameName + "_ACH_WON", 1);
		int minigameIndex = GameManager.GetMinigameIndex(GameManager.current_minigame_def);
		if (minigameIndex != -1)
		{
			GameManager.WonMinigame[minigameIndex] = true;
		}
		bool flag = true;
		for (int i = 0; i < GameManager.WonMinigame.Count; i++)
		{
			if (!GameManager.WonMinigame[i])
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_MINIGAME_MASTER");
		}
	}

	// Token: 0x06001B51 RID: 6993 RVA: 0x000B91A4 File Offset: 0x000B73A4
	public static void SetMinigameLost()
	{
		RBPrefs.SetInt(GameManager.current_minigame_def.minigameName + "_ACH_LOST", 1);
		int minigameIndex = GameManager.GetMinigameIndex(GameManager.current_minigame_def);
		if (minigameIndex != -1)
		{
			GameManager.LostMinigame[minigameIndex] = true;
		}
		bool flag = true;
		for (int i = 0; i < GameManager.LostMinigame.Count; i++)
		{
			if (!GameManager.LostMinigame[i])
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_UNLUCKY");
		}
	}

	// Token: 0x06001B52 RID: 6994 RVA: 0x000B9220 File Offset: 0x000B7420
	private static int GetMinigameIndex(MinigameDefinition def)
	{
		for (int i = 0; i < GameManager.minigame_list.Count; i++)
		{
			if (def == GameManager.minigame_list[i])
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06001B53 RID: 6995 RVA: 0x00014097 File Offset: 0x00012297
	public static List<MinigameDefinition> GetMinigameList()
	{
		return GameManager.minigame_list;
	}

	// Token: 0x06001B54 RID: 6996 RVA: 0x0001409E File Offset: 0x0001229E
	public static List<GameModifierDefinition> GetModifierList()
	{
		return GameManager.modifier_list;
	}

	// Token: 0x06001B55 RID: 6997 RVA: 0x000B9258 File Offset: 0x000B7458
	public static ItemDetails GetItemFromEnum(Items item)
	{
		for (int i = 0; i < GameManager.ItemList.items.Length; i++)
		{
			if (GameManager.ItemList.items[i].enumReference == item)
			{
				return GameManager.ItemList.items[i];
			}
		}
		return null;
	}

	// Token: 0x06001B56 RID: 6998 RVA: 0x000B92A0 File Offset: 0x000B74A0
	public static void AddMinigame(MinigameDefinition minigame)
	{
		GameManager.forcedNextMinigame = minigame;
		for (int i = 0; i < GameManager.pseudoRandomList.Count; i++)
		{
			if (GameManager.pseudoRandomList[i] == minigame)
			{
				GameManager.pseudoRandomList.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06001B57 RID: 6999 RVA: 0x000B92E8 File Offset: 0x000B74E8
	public static List<MinigameDefinition> GetActiveMinigameList()
	{
		List<MinigameDefinition> list = new List<MinigameDefinition>();
		int num = 1;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < GameManager.minigame_list.Count; j++)
			{
				if (GameManager.minigame_list[j].GetIsActive())
				{
					list.Add(GameManager.minigame_list[j]);
				}
			}
		}
		return list;
	}

	// Token: 0x06001B58 RID: 7000 RVA: 0x000B9344 File Offset: 0x000B7544
	public static MinigameDefinition GetRandomMinigame()
	{
		if (GameManager.pseudoRandomList.Count == 0)
		{
			GameManager.UpdatePsuedoRandomMinigameList();
		}
		if (GameManager.forcedNextMinigame != null)
		{
			MinigameDefinition result = GameManager.forcedNextMinigame;
			GameManager.forcedNextMinigame = null;
			return result;
		}
		bool flag = false;
		MinigameDefinition minigameDefinition = null;
		int num = 2048;
		int num2 = 0;
		while (!flag && num2 < num)
		{
			if (GameManager.pseudoRandomList.Count == 0)
			{
				GameManager.UpdatePsuedoRandomMinigameList();
			}
			int index = GameManager.rand.Next(0, GameManager.pseudoRandomList.Count);
			minigameDefinition = GameManager.pseudoRandomList[index];
			GameManager.pseudoRandomList.RemoveAt(index);
			if (GameManager.GetPlayerCount() >= minigameDefinition.minPlayers && GameManager.GetPlayerCount() <= minigameDefinition.maxPlayers)
			{
				flag = true;
			}
			num2++;
			if (num2 >= num - 1)
			{
				minigameDefinition = GameManager.FallbackMinigame;
				break;
			}
		}
		return minigameDefinition;
	}

	// Token: 0x06001B59 RID: 7001 RVA: 0x000B9400 File Offset: 0x000B7600
	public static void RemoveIncompatibleMinigames()
	{
		for (int i = GameManager.pseudoRandomList.Count - 1; i >= 0; i--)
		{
			if (GameManager.GetPlayerCount() < GameManager.pseudoRandomList[i].minPlayers || GameManager.GetPlayerCount() > GameManager.pseudoRandomList[i].maxPlayers)
			{
				GameManager.pseudoRandomList.RemoveAt(i);
			}
		}
	}

	// Token: 0x06001B5A RID: 7002 RVA: 0x000B9460 File Offset: 0x000B7660
	public static void UpdatePsuedoRandomMinigameList()
	{
		GameManager.pseudoRandomList.Clear();
		int num = 1;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < GameManager.minigame_list.Count; j++)
			{
				if (GameManager.minigame_list[j].GetIsActive())
				{
					GameManager.pseudoRandomList.Add(GameManager.minigame_list[j]);
				}
			}
		}
	}

	// Token: 0x06001B5B RID: 7003 RVA: 0x000B94C4 File Offset: 0x000B76C4
	public static int GetNumberOfActiveMinigames()
	{
		int num = 0;
		using (List<MinigameDefinition>.Enumerator enumerator = GameManager.minigame_list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetIsActive())
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06001B5C RID: 7004 RVA: 0x000B951C File Offset: 0x000B771C
	public static int GetNumberOfActiveItems()
	{
		int num = 0;
		ItemDetails[] items = GameManager.ItemList.items;
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].GetIsActive())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06001B5D RID: 7005 RVA: 0x000B9554 File Offset: 0x000B7754
	public static MinigameDefinition GetMinigameByName(string name)
	{
		MinigameDefinition result = null;
		if (GameManager.minigame_map.TryGetValue(name, out result))
		{
			return result;
		}
		Debug.LogError("Unable to find minigame with name '" + name + "'!");
		return null;
	}

	// Token: 0x06001B5E RID: 7006 RVA: 0x000B958C File Offset: 0x000B778C
	public static GameModifierDefinition GetModifierByID(int id)
	{
		GameModifierDefinition result = null;
		if (GameManager.modifier_map.TryGetValue(id, out result))
		{
			return result;
		}
		Debug.LogError("Unable to find modifier with id '" + id.ToString() + "'!");
		return null;
	}

	// Token: 0x06001B5F RID: 7007 RVA: 0x000B95C8 File Offset: 0x000B77C8
	public static void LoadMinigame(MinigameDefinition minigame, int alternateIndex)
	{
		if (alternateIndex == 0)
		{
			GameManager.SwapScene(minigame.sceneName);
		}
		else if (minigame.alternates != null && alternateIndex <= minigame.alternates.Length)
		{
			GameManager.current_minigame_alternate = minigame.alternates[alternateIndex - 1];
			GameManager.SwapScene(GameManager.current_minigame_alternate.sceneName);
		}
		else
		{
			GameManager.SwapScene(minigame.sceneName);
		}
		GameManager.current_minigame_def = minigame;
	}

	// Token: 0x06001B60 RID: 7008 RVA: 0x000B962C File Offset: 0x000B782C
	public static void CreateMinigameController()
	{
		GameManager.minigame_root = GameObject.Find("MinigameRoot");
		if (GameManager.minigame_root == null)
		{
			Debug.LogError("Could not find gameobject for minigame root, it should be named 'MinigameRoot'!");
		}
		if (NetSystem.IsServer)
		{
			NetSystem.Spawn((Resources.Load(GameManager.current_minigame_def.minigameControllerPfbPath) as GameObject).name, 0, NetSystem.MyPlayer);
		}
	}

	// Token: 0x06001B61 RID: 7009 RVA: 0x000B968C File Offset: 0x000B788C
	public static bool MinigameWaitForLoad()
	{
		if (GameManager.current_minigame)
		{
			if (GameManager.current_minigame.State == MinigameControllerState.None)
			{
				GameManager.current_minigame.StartWaitForLoad();
			}
			else if (GameManager.current_minigame.State == MinigameControllerState.WaitingForLoad)
			{
				return GameManager.current_minigame.CheckControllerLoaded();
			}
			return false;
		}
		return false;
	}

	// Token: 0x06001B62 RID: 7010 RVA: 0x000140A5 File Offset: 0x000122A5
	public static void InitializeMinigame()
	{
		GameManager.current_minigame.Root = GameManager.minigame_root;
		GameManager.current_minigame.InitializeMinigame();
	}

	// Token: 0x06001B63 RID: 7011 RVA: 0x000140C0 File Offset: 0x000122C0
	public static void SetMinigameResults(MinigameResults new_results)
	{
		GameManager.minigame_results = new_results;
	}

	// Token: 0x06001B64 RID: 7012 RVA: 0x000140C8 File Offset: 0x000122C8
	public static MinigameResults GetMinigameResults()
	{
		return GameManager.minigame_results;
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x000140CF File Offset: 0x000122CF
	public static void ShowResultScreen()
	{
		if (GameManager.current_minigame == null)
		{
			Debug.LogWarning("Cannot show results, no active minigame!");
			return;
		}
		GameManager.current_minigame.ShowResultScreen();
		GameManager.game_board.ShowMinigameResults();
		GameManager.scoreUIScene.State(false);
	}

	// Token: 0x06001B66 RID: 7014 RVA: 0x000B96D8 File Offset: 0x000B78D8
	public static void ReleaseMinigame()
	{
		if (GameManager.current_minigame != null)
		{
			GameManager.current_minigame.ReleaseMinigame();
			GameManager.current_minigame.DestroyObjects();
			MinigameController obj = GameManager.current_minigame;
			GameManager.current_minigame = null;
			if (NetSystem.IsServer)
			{
				NetSystem.Kill(obj);
			}
		}
		if (GameManager.minigame_root != null)
		{
			UnityEngine.Object.Destroy(GameManager.minigame_root);
		}
	}

	// Token: 0x06001B67 RID: 7015 RVA: 0x00013EE7 File Offset: 0x000120E7
	public static void RegisterMinigameController(MinigameController minigame)
	{
		GameManager.current_minigame = minigame;
	}

	// Token: 0x06001B68 RID: 7016 RVA: 0x00014108 File Offset: 0x00012308
	public static void EnableGameBoard()
	{
		GameManager.ui_controller.EnableBoardUI();
		GameManager.boardWorldObj.SetActive(true);
		if (GameManager.partyGameMode == PartyGameMode.BoardGame)
		{
			GameManager.boardSquaresObj.SetActive(true);
		}
		GameManager.game_board.EnableBoard();
	}

	// Token: 0x06001B69 RID: 7017 RVA: 0x0001413B File Offset: 0x0001233B
	public static void DisableGameBoard(bool disableUI = true, bool enablePlayers = false)
	{
		if (GameManager.partyGameMode == PartyGameMode.BoardGame)
		{
			GameManager.boardSquaresObj.SetActive(false);
		}
		if (disableUI)
		{
			GameManager.ui_controller.DisableBoardUI();
		}
		GameManager.boardWorldObj.SetActive(false);
		GameManager.game_board.DisableBoard(enablePlayers);
	}

	// Token: 0x06001B6A RID: 7018 RVA: 0x00014172 File Offset: 0x00012372
	public static void HideResultScreen()
	{
		GameManager.result_screen.Hide();
	}

	// Token: 0x06001B6B RID: 7019 RVA: 0x0000398C File Offset: 0x00001B8C
	public static void FinishedLoading()
	{
	}

	// Token: 0x06001B6C RID: 7020 RVA: 0x000B9738 File Offset: 0x000B7938
	public static void AssignTeams(MinigameDefinition definition)
	{
		for (int i = 0; i < GameManager.players.Count; i++)
		{
			if (i < 2)
			{
				GameManager.players[i].MinigameTeam = 0;
			}
			else
			{
				GameManager.players[i].MinigameTeam = 1;
			}
		}
	}

	// Token: 0x06001B6D RID: 7021 RVA: 0x000B9784 File Offset: 0x000B7984
	public static List<GamePlayer> GetTeam(int team_index)
	{
		List<GamePlayer> list = new List<GamePlayer>();
		for (int i = 0; i < GameManager.players.Count; i++)
		{
			if (GameManager.players[i].MinigameTeam == team_index)
			{
				list.Add(GameManager.players[i]);
			}
		}
		return list;
	}

	// Token: 0x06001B6E RID: 7022 RVA: 0x000B97D4 File Offset: 0x000B79D4
	public static void SetPlayerTeam(short global_id, int team)
	{
		for (int i = GameManager.players.Count - 1; i >= 0; i--)
		{
			if (GameManager.players[i].GlobalID == global_id)
			{
				GameManager.players[i].MinigameTeam = team;
			}
		}
	}

	// Token: 0x06001B6F RID: 7023 RVA: 0x000B981C File Offset: 0x000B7A1C
	public static void AddPlayer(string name, bool local, short local_id, short global_id, ushort color_index, ushort skin_index, byte hat, byte cape, bool is_ai, BotDifficulty difficulty, NetPlayer client_owner)
	{
		GameManager.players.Add(new GamePlayer(name, local, local_id, global_id, color_index, skin_index, hat, cape, is_ai, difficulty, client_owner));
	}

	// Token: 0x06001B70 RID: 7024 RVA: 0x000B984C File Offset: 0x000B7A4C
	public static void RemovePlayer(short global_id)
	{
		for (int i = GameManager.players.Count - 1; i >= 0; i--)
		{
			if (GameManager.players[i].GlobalID == global_id)
			{
				GameManager.players.RemoveAt(i);
				return;
			}
		}
		for (int j = GameManager.disconnected_players.Count - 1; j >= 0; j--)
		{
			if (GameManager.disconnected_players[j].GlobalID == global_id)
			{
				GameManager.disconnected_players.RemoveAt(j);
				return;
			}
		}
		Debug.LogWarning("Could not remove player with global id " + global_id.ToString() + ", player does not exist with this id");
	}

	// Token: 0x06001B71 RID: 7025 RVA: 0x000B98E0 File Offset: 0x000B7AE0
	public static void DisconnectPlayer(short global_id)
	{
		for (int i = GameManager.players.Count - 1; i >= 0; i--)
		{
			if (GameManager.players[i].GlobalID == global_id)
			{
				GameManager.disconnected_players.Add(GameManager.players[i]);
				GameManager.players.RemoveAt(i);
				return;
			}
		}
		Debug.LogWarning("Could not disconnect player with global id " + global_id.ToString() + ", player does not exist with this id");
	}

	// Token: 0x06001B72 RID: 7026 RVA: 0x0001417E File Offset: 0x0001237E
	public static void ClearPlayers()
	{
		GameManager.players.Clear();
		GameManager.disconnected_players.Clear();
	}

	// Token: 0x06001B73 RID: 7027 RVA: 0x00014194 File Offset: 0x00012394
	public static int GetPlayerCount()
	{
		return GameManager.players.Count;
	}

	// Token: 0x06001B74 RID: 7028 RVA: 0x000141A0 File Offset: 0x000123A0
	public static int GetLocalPlayerCount()
	{
		return GameManager.GetLocalPlayers().Count;
	}

	// Token: 0x06001B75 RID: 7029 RVA: 0x000141AC File Offset: 0x000123AC
	public static bool IsPlayerInSlot(int index)
	{
		return index >= 0 && index < GameManager.players.Count && GameManager.players[index] != null;
	}

	// Token: 0x06001B76 RID: 7030 RVA: 0x000141CF File Offset: 0x000123CF
	public static GamePlayer GetPlayerAt(int index)
	{
		if (index >= 0 && index < GameManager.players.Count)
		{
			return GameManager.players[index];
		}
		return null;
	}

	// Token: 0x06001B77 RID: 7031 RVA: 0x000B9954 File Offset: 0x000B7B54
	public static GamePlayer GetPlayerWithID(short global_id)
	{
		for (int i = 0; i < GameManager.players.Count; i++)
		{
			if (GameManager.players[i].GlobalID == global_id)
			{
				return GameManager.players[i];
			}
		}
		Debug.LogWarning("Unable to get player with global id " + global_id.ToString());
		return null;
	}

	// Token: 0x06001B78 RID: 7032 RVA: 0x000B99AC File Offset: 0x000B7BAC
	public static void FixGlobalIDs()
	{
		for (int i = 0; i < GameManager.players.Count; i++)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Changing player ",
				i.ToString(),
				" global id from ",
				GameManager.players[i].GlobalID.ToString(),
				" to ",
				i.ToString()
			}));
			GameManager.players[i].GlobalID = (short)i;
		}
	}

	// Token: 0x1700031F RID: 799
	// (get) Token: 0x06001B79 RID: 7033 RVA: 0x000141EF File Offset: 0x000123EF
	public static List<GamePlayer> PlayerList
	{
		get
		{
			return GameManager.players;
		}
	}

	// Token: 0x06001B7A RID: 7034 RVA: 0x000B9A38 File Offset: 0x000B7C38
	public static List<GamePlayer> GetLocalPlayers()
	{
		List<GamePlayer> list = new List<GamePlayer>();
		for (int i = 0; i < GameManager.players.Count; i++)
		{
			if (GameManager.players[i].IsLocalPlayer)
			{
				list.Add(GameManager.players[i]);
			}
		}
		return list;
	}

	// Token: 0x06001B7B RID: 7035 RVA: 0x000B9A84 File Offset: 0x000B7C84
	public static List<GamePlayer> GetLocalNonAIPlayers()
	{
		List<GamePlayer> list = new List<GamePlayer>();
		for (int i = 0; i < GameManager.players.Count; i++)
		{
			if (GameManager.players[i].IsLocalPlayer && !GameManager.players[i].IsAI)
			{
				list.Add(GameManager.players[i]);
			}
		}
		return list;
	}

	// Token: 0x06001B7C RID: 7036 RVA: 0x000B9AE4 File Offset: 0x000B7CE4
	public static List<GamePlayer> GetLocalAIPlayers()
	{
		List<GamePlayer> list = new List<GamePlayer>();
		for (int i = 0; i < GameManager.players.Count; i++)
		{
			if (GameManager.players[i].IsLocalPlayer && GameManager.players[i].IsAI)
			{
				list.Add(GameManager.players[i]);
			}
		}
		return list;
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x000141F6 File Offset: 0x000123F6
	public static void SwapScene(string scene_name)
	{
		GameManager.switcher.DoTransition(scene_name);
	}

	// Token: 0x06001B7E RID: 7038 RVA: 0x00014203 File Offset: 0x00012403
	public static void SwapScene(TransitionKitDelegate transitionDelegate)
	{
		GameManager.switcher.DoTransition(transitionDelegate);
	}

	// Token: 0x06001B7F RID: 7039 RVA: 0x00014210 File Offset: 0x00012410
	public static void RequestLobbySlot(int local_id, int slot_index = -1)
	{
		if (GameManager.lobby_controller == null)
		{
			return;
		}
		GameManager.lobby_controller.RequestSlot((short)local_id, (short)slot_index);
	}

	// Token: 0x06001B80 RID: 7040 RVA: 0x0001422E File Offset: 0x0001242E
	public static void LeaveLobbySlot(int slot_index)
	{
		if (GameManager.lobby_controller == null)
		{
			return;
		}
		GameManager.lobby_controller.LeaveSlot((short)slot_index);
	}

	// Token: 0x06001B81 RID: 7041 RVA: 0x00005651 File Offset: 0x00003851
	public static GameEventTheme GetCurrentEventTheme()
	{
		return GameEventTheme.Halloween;
	}

	// Token: 0x06001B82 RID: 7042 RVA: 0x0001424A File Offset: 0x0001244A
	public static Camera CurrentCamera()
	{
		if (GameManager.game_board != null && GameManager.game_board.Camera != null)
		{
			return GameManager.game_board.Camera.Cam;
		}
		return null;
	}

	// Token: 0x06001B83 RID: 7043 RVA: 0x0001427C File Offset: 0x0001247C
	public static GameBoardCamera GetCamera()
	{
		if (GameManager.game_board)
		{
			return GameManager.game_board.Camera;
		}
		return null;
	}

	// Token: 0x06001B84 RID: 7044 RVA: 0x00014296 File Offset: 0x00012496
	public static MapDetails[] GetMaps()
	{
		return GameManager.maps;
	}

	// Token: 0x06001B85 RID: 7045 RVA: 0x0001429D File Offset: 0x0001249D
	public static MapDetails GetMap(int i)
	{
		return GameManager.maps[i];
	}

	// Token: 0x06001B86 RID: 7046 RVA: 0x000142A6 File Offset: 0x000124A6
	public static WorldTextData GetWorldTextData(WorldTextType type)
	{
		return GameManager.world_text_curves[(int)type];
	}

	// Token: 0x06001B87 RID: 7047 RVA: 0x000142AF File Offset: 0x000124AF
	public static PlayerColor GetColorAtIndex(int index)
	{
		return GameManager.player_colors[index];
	}

	// Token: 0x06001B88 RID: 7048 RVA: 0x000142BC File Offset: 0x000124BC
	public static int ColorCount()
	{
		return GameManager.player_colors.Length;
	}

	// Token: 0x06001B89 RID: 7049 RVA: 0x000142C5 File Offset: 0x000124C5
	public static PlayerSkin GetSkinAtIndex(int index)
	{
		return GameManager.playerSkins[index].male;
	}

	// Token: 0x06001B8A RID: 7050 RVA: 0x000142D3 File Offset: 0x000124D3
	public static CharacterHat GetHatAtIndex(int index)
	{
		return GameManager.playerHats[index];
	}

	// Token: 0x06001B8B RID: 7051 RVA: 0x000142BC File Offset: 0x000124BC
	public static int GetPlayerColorCount()
	{
		return GameManager.player_colors.Length;
	}

	// Token: 0x06001B8C RID: 7052 RVA: 0x000142DC File Offset: 0x000124DC
	public static int GetPlayerSkinCount()
	{
		return GameManager.playerSkins.Length;
	}

	// Token: 0x06001B8D RID: 7053 RVA: 0x000142E5 File Offset: 0x000124E5
	public static int GetPlayerHatCount()
	{
		return GameManager.playerHats.Length;
	}

	// Token: 0x06001B8E RID: 7054 RVA: 0x000142EE File Offset: 0x000124EE
	public static void SetPlayerColors(PlayerColor[] colors)
	{
		GameManager.player_colors = colors;
	}

	// Token: 0x06001B8F RID: 7055 RVA: 0x000142EE File Offset: 0x000124EE
	public static void SetPlayerMatColors(PlayerColor[] colors)
	{
		GameManager.player_colors = colors;
	}

	// Token: 0x06001B90 RID: 7056 RVA: 0x000142F6 File Offset: 0x000124F6
	public static void SetPlayerSkins(CharacterSkin[] skins)
	{
		GameManager.playerSkins = skins;
	}

	// Token: 0x06001B91 RID: 7057 RVA: 0x000142FE File Offset: 0x000124FE
	public static void SetPlayerHats(CharacterHat[] hats)
	{
		GameManager.playerHats = hats;
	}

	// Token: 0x06001B92 RID: 7058 RVA: 0x00014306 File Offset: 0x00012506
	public static void SetTextData(WorldTextData curves)
	{
		GameManager.world_text_curves[(int)curves.text_type] = curves;
	}

	// Token: 0x06001B93 RID: 7059 RVA: 0x00013EA4 File Offset: 0x000120A4
	public static void SetGameBoard(GameBoardController _game_board)
	{
		GameManager.game_board = _game_board;
	}

	// Token: 0x06001B94 RID: 7060 RVA: 0x00014315 File Offset: 0x00012515
	public static void SetSceneSwitcher(SceneSwitcher _switcher)
	{
		GameManager.switcher = _switcher;
	}

	// Token: 0x06001B95 RID: 7061 RVA: 0x0001431D File Offset: 0x0001251D
	public static void SetMapData(MapDetails[] newMaps)
	{
		GameManager.maps = newMaps;
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x00014325 File Offset: 0x00012525
	public static string GetSavePath()
	{
		return Application.persistentDataPath + "/BoardSaves.pps";
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x000B9B44 File Offset: 0x000B7D44
	public static List<GamePlayer> GetPlayerPlacements()
	{
		List<GamePlayer> playerList = GameManager.PlayerList;
		List<GamePlayer> list = new List<GamePlayer>();
		for (int i = 0; i < playerList.Count; i++)
		{
			if (list.Count == 0)
			{
				list.Add(playerList[i]);
			}
			else
			{
				for (int j = 0; j < playerList.Count; j++)
				{
					bool flag;
					if (GameManager.partyGameMode == PartyGameMode.BoardGame)
					{
						flag = (playerList[i].BoardObject.GoalScore > list[j].BoardObject.GoalScore || (playerList[i].BoardObject.GoalScore == list[j].BoardObject.GoalScore && playerList[i].BoardObject.Gold > list[j].BoardObject.Gold));
					}
					else
					{
						flag = (GameManager.UIController.minigameOnlySceneController.scores[i] > GameManager.UIController.minigameOnlySceneController.scores[(int)list[j].GlobalID]);
					}
					if (flag)
					{
						list.Insert(j, playerList[i]);
						break;
					}
					if (j == list.Count - 1)
					{
						list.Add(playerList[i]);
						break;
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06001B98 RID: 7064 RVA: 0x00014336 File Offset: 0x00012536
	public static void SetGlobalPlayerEmission(float value)
	{
		GameManager.OnGlobalPlayerEmissionChanged.Invoke(value);
		Shader.SetGlobalFloat("_GlobalPlayerEmission", value);
	}

	// Token: 0x06001B99 RID: 7065 RVA: 0x000B9C88 File Offset: 0x000B7E88
	public static void SaveUnlocks()
	{
		try
		{
			if (File.Exists(GameManager.filePath))
			{
				File.Delete(GameManager.filePath);
			}
			using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(GameManager.filePath, FileMode.Create)))
			{
				binaryWriter.Write(GameManager.TrophyCount);
				for (int i = 0; i < GameManager.unlocked.Length; i++)
				{
					binaryWriter.Write(GameManager.unlocked[i]);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Saving unlocks failed: " + ex.ToString());
		}
	}

	// Token: 0x06001B9A RID: 7066 RVA: 0x000B9D28 File Offset: 0x000B7F28
	public static void LoadUnlocks()
	{
		try
		{
			if (File.Exists(GameManager.filePath))
			{
				using (BinaryReader binaryReader = new BinaryReader(File.Open(GameManager.filePath, FileMode.Open)))
				{
					GameManager.TrophyCount = binaryReader.ReadInt32();
					for (int i = 0; i < GameManager.unlocked.Length; i++)
					{
						GameManager.unlocked[i] = binaryReader.ReadBoolean();
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Loading unlocks failed: " + ex.ToString());
		}
	}

	// Token: 0x06001B9B RID: 7067 RVA: 0x0001434E File Offset: 0x0001254E
	public static void DelayedCall(DelayedCall callback, float delay)
	{
		if (GameManager.gameManagerObj != null)
		{
			GameManager.gameManagerObj.DelayedCall(callback, delay);
		}
	}

	// Token: 0x04001D16 RID: 7446
	public const int DefaultLayerMask = 1;

	// Token: 0x04001D17 RID: 7447
	public const int TransparentFXLayerMask = 2;

	// Token: 0x04001D18 RID: 7448
	public const int IgnoreRaycastLayerMask = 4;

	// Token: 0x04001D19 RID: 7449
	public const int BuiltInLayer3Mask = 8;

	// Token: 0x04001D1A RID: 7450
	public const int WaterLayerMask = 16;

	// Token: 0x04001D1B RID: 7451
	public const int UILayerMask = 32;

	// Token: 0x04001D1C RID: 7452
	public const int BuiltInLayer6Mask = 64;

	// Token: 0x04001D1D RID: 7453
	public const int BuiltInLayer7Mask = 128;

	// Token: 0x04001D1E RID: 7454
	public const int PlayersLayerMask = 256;

	// Token: 0x04001D1F RID: 7455
	public const int ProjectilesLayerMask = 512;

	// Token: 0x04001D20 RID: 7456
	public const int WorldGroundLayerMask = 1024;

	// Token: 0x04001D21 RID: 7457
	public const int WorldWallLayerMask = 2048;

	// Token: 0x04001D22 RID: 7458
	public const int EnemyLayerMask = 4096;

	// Token: 0x04001D23 RID: 7459
	public const int WorldCameraIgnoreLayerMask = 8192;

	// Token: 0x04001D24 RID: 7460
	public const int U14LayerMask = 16384;

	// Token: 0x04001D25 RID: 7461
	public const int ClothLayerMask = 32768;

	// Token: 0x04001D26 RID: 7462
	public const int WorldUILayerMask = 65536;

	// Token: 0x04001D27 RID: 7463
	public const int U17LayerMask = 131072;

	// Token: 0x04001D28 RID: 7464
	public const int MinigameUtil1LayerMask = 262144;

	// Token: 0x04001D29 RID: 7465
	public const int MinigameUtil2LayerMask = 524288;

	// Token: 0x04001D2A RID: 7466
	public const int MinigameUtil3LayerMask = 1048576;

	// Token: 0x04001D2B RID: 7467
	public const int MinigameUtil4LayerMask = 2097152;

	// Token: 0x04001D2C RID: 7468
	public const int DebriLayerMask = 4194304;

	// Token: 0x04001D2D RID: 7469
	public const int NotVisibleMask = 1;

	// Token: 0x04001D2E RID: 7470
	public const int DefaultLayer = 0;

	// Token: 0x04001D2F RID: 7471
	public const int TransparentFXLayer = 1;

	// Token: 0x04001D30 RID: 7472
	public const int IgnoreRaycastLayer = 2;

	// Token: 0x04001D31 RID: 7473
	public const int BuiltInLayer3 = 3;

	// Token: 0x04001D32 RID: 7474
	public const int WaterLayer = 4;

	// Token: 0x04001D33 RID: 7475
	public const int UILayer = 5;

	// Token: 0x04001D34 RID: 7476
	public const int BuiltInLayer6 = 6;

	// Token: 0x04001D35 RID: 7477
	public const int BuiltInLayer7 = 7;

	// Token: 0x04001D36 RID: 7478
	public const int PlayersLayer = 8;

	// Token: 0x04001D37 RID: 7479
	public const int ProjectilesLayer = 9;

	// Token: 0x04001D38 RID: 7480
	public const int WorldGroundLayer = 10;

	// Token: 0x04001D39 RID: 7481
	public const int WorldWallLayer = 11;

	// Token: 0x04001D3A RID: 7482
	public const int EnemyLayer = 12;

	// Token: 0x04001D3B RID: 7483
	public const int WorldCameraIgnoreLayer = 13;

	// Token: 0x04001D3C RID: 7484
	public const int U14Layer = 14;

	// Token: 0x04001D3D RID: 7485
	public const int ClothLayer = 15;

	// Token: 0x04001D3E RID: 7486
	public const int WorldUILayer = 16;

	// Token: 0x04001D3F RID: 7487
	public const int U17Layer = 17;

	// Token: 0x04001D40 RID: 7488
	public const int MinigameUtil1Layer = 18;

	// Token: 0x04001D41 RID: 7489
	public const int MinigameUtil2Layer = 19;

	// Token: 0x04001D42 RID: 7490
	public const int MinigameUtil3Layer = 20;

	// Token: 0x04001D43 RID: 7491
	public const int MinigameUtil4Layer = 21;

	// Token: 0x04001D44 RID: 7492
	public const int DebriLayer = 22;

	// Token: 0x04001D45 RID: 7493
	public const int NotVisibleLayer = 31;

	// Token: 0x04001D46 RID: 7494
	public static Color32[] red = new Color32[]
	{
		new Color32(244, 204, 204, byte.MaxValue),
		new Color32(234, 153, 153, byte.MaxValue),
		new Color32(224, 102, 102, byte.MaxValue),
		new Color32(204, 0, 0, byte.MaxValue),
		new Color32(153, 0, 0, byte.MaxValue),
		new Color32(102, 0, 0, byte.MaxValue)
	};

	// Token: 0x04001D47 RID: 7495
	public static Color32[] green = new Color32[]
	{
		new Color32(217, 234, 211, byte.MaxValue),
		new Color32(182, 215, 168, byte.MaxValue),
		new Color32(147, 196, 125, byte.MaxValue),
		new Color32(106, 168, 79, byte.MaxValue),
		new Color32(56, 118, 29, byte.MaxValue),
		new Color32(39, 78, 19, byte.MaxValue)
	};

	// Token: 0x04001D48 RID: 7496
	public static Color32[] teal = new Color32[]
	{
		new Color32(201, 218, 248, byte.MaxValue),
		new Color32(164, 194, 244, byte.MaxValue),
		new Color32(109, 158, 235, byte.MaxValue),
		new Color32(60, 120, 216, byte.MaxValue),
		new Color32(17, 85, 204, byte.MaxValue),
		new Color32(28, 69, 135, byte.MaxValue)
	};

	// Token: 0x04001D49 RID: 7497
	public static Color32[] orange = new Color32[]
	{
		new Color32(252, 229, 20, byte.MaxValue),
		new Color32(249, 203, 156, byte.MaxValue),
		new Color32(246, 178, 107, byte.MaxValue),
		new Color32(230, 145, 56, byte.MaxValue),
		new Color32(180, 95, 6, byte.MaxValue),
		new Color32(120, 63, 4, byte.MaxValue)
	};

	// Token: 0x04001D4A RID: 7498
	public static Color32[] purple = new Color32[]
	{
		new Color32(217, 210, 233, byte.MaxValue),
		new Color32(180, 167, 214, byte.MaxValue),
		new Color32(142, 124, 195, byte.MaxValue),
		new Color32(103, 78, 167, byte.MaxValue),
		new Color32(53, 28, 117, byte.MaxValue),
		new Color32(32, 18, 77, byte.MaxValue)
	};

	// Token: 0x04001D4B RID: 7499
	public const int GOAL_REWARDS = 6;

	// Token: 0x04001D4C RID: 7500
	public static System.Random rand;

	// Token: 0x04001D4D RID: 7501
	public static GameManager.OnQuitEvent OnQuit;

	// Token: 0x04001D4E RID: 7502
	public static GameManager.OnXInputEvent OnXInput;

	// Token: 0x04001D4F RID: 7503
	public static GameManagerObj gameManagerObj;

	// Token: 0x04001D50 RID: 7504
	private static GameState cur_game_state = GameState.MainMenu;

	// Token: 0x04001D51 RID: 7505
	private static bool is_game_paused = false;

	// Token: 0x04001D52 RID: 7506
	private static List<GamePlayer> players;

	// Token: 0x04001D53 RID: 7507
	private static List<GamePlayer> disconnected_players;

	// Token: 0x04001D54 RID: 7508
	private static GameObject board_player_prefab;

	// Token: 0x04001D55 RID: 7509
	private static GameBoardController game_board;

	// Token: 0x04001D56 RID: 7510
	private static KeyController key_controller;

	// Token: 0x04001D57 RID: 7511
	private static GameObject boardWorldObj;

	// Token: 0x04001D58 RID: 7512
	private static GameObject boardSquaresObj;

	// Token: 0x04001D59 RID: 7513
	public static MapSettings mapSettings;

	// Token: 0x04001D5A RID: 7514
	private static GameUIController ui_controller;

	// Token: 0x04001D5B RID: 7515
	private static UIController mainMenuUIController;

	// Token: 0x04001D5C RID: 7516
	private static WorldTextData[] world_text_curves = new WorldTextData[16];

	// Token: 0x04001D5D RID: 7517
	private static BoardLoadScreen board_load_screen;

	// Token: 0x04001D5F RID: 7519
	public static string VERSION = "1.11.2g";

	// Token: 0x04001D60 RID: 7520
	private static ItemList itemList;

	// Token: 0x04001D61 RID: 7521
	private static GameObject minigame_root;

	// Token: 0x04001D62 RID: 7522
	private static Dictionary<string, MinigameDefinition> minigame_map;

	// Token: 0x04001D63 RID: 7523
	private static List<MinigameDefinition> minigame_list;

	// Token: 0x04001D64 RID: 7524
	private static Dictionary<int, GameModifierDefinition> modifier_map;

	// Token: 0x04001D65 RID: 7525
	private static List<GameModifierDefinition> modifier_list;

	// Token: 0x04001D66 RID: 7526
	private static CharacterHat[] playerHats = new CharacterHat[8];

	// Token: 0x04001D67 RID: 7527
	private static CharacterSkin[] playerSkins = new CharacterSkin[8];

	// Token: 0x04001D68 RID: 7528
	private static PlayerColor[] player_colors = new PlayerColor[8];

	// Token: 0x04001D69 RID: 7529
	private static MapDetails[] maps;

	// Token: 0x04001D6A RID: 7530
	public static SceneSwitcher switcher;

	// Token: 0x04001D6B RID: 7531
	private static ResultSceenScene result_screen;

	// Token: 0x04001D6C RID: 7532
	public static ScoreUIScene scoreUIScene;

	// Token: 0x04001D6D RID: 7533
	public static ModifierUI modifierUI;

	// Token: 0x04001D6E RID: 7534
	private static bool capture_input = false;

	// Token: 0x04001D6F RID: 7535
	private static bool poll_input = true;

	// Token: 0x04001D70 RID: 7536
	private static MultiplayerLobbyController lobby_controller;

	// Token: 0x04001D71 RID: 7537
	private static MinigameResults minigame_results = new MinigameResults(8);

	// Token: 0x04001D72 RID: 7538
	private static MapDetails curMap;

	// Token: 0x04001D73 RID: 7539
	private static MinigameController current_minigame;

	// Token: 0x04001D74 RID: 7540
	private static MinigameDefinition current_minigame_def;

	// Token: 0x04001D75 RID: 7541
	private static MinigameAlternate current_minigame_alternate;

	// Token: 0x04001D76 RID: 7542
	private static int cur_minigame = 0;

	// Token: 0x04001D77 RID: 7543
	private static List<MinigameDefinition> pseudoRandomList = new List<MinigameDefinition>();

	// Token: 0x04001D79 RID: 7545
	public static GameNetworkManager gameNetworkManager;

	// Token: 0x04001D7A RID: 7546
	public static List<BoardModifier> BoardModifiers = new List<BoardModifier>();

	// Token: 0x04001D7B RID: 7547
	private static RagdollSettings ragdoll_settings = new RagdollSettings();

	// Token: 0x04001D7D RID: 7549
	public static MinigameDefinition FallbackMinigame;

	// Token: 0x04001D7E RID: 7550
	public static GameObject lastGameObject;

	// Token: 0x04001D7F RID: 7551
	public static bool lastVal;

	// Token: 0x04001D88 RID: 7560
	private static GameRulesetManager m_rulesetManager;

	// Token: 0x04001D89 RID: 7561
	private static Controller cont = null;

	// Token: 0x04001D8A RID: 7562
	private static Player p;

	// Token: 0x04001D8B RID: 7563
	private static bool gotPlayer = false;

	// Token: 0x04001D8C RID: 7564
	private static float m_nextTemporaryEffectUpdate = 0f;

	// Token: 0x04001D8D RID: 7565
	public static List<bool> WonMinigame = new List<bool>();

	// Token: 0x04001D8E RID: 7566
	public static List<bool> LostMinigame = new List<bool>();

	// Token: 0x04001D8F RID: 7567
	private static MinigameDefinition forcedNextMinigame;

	// Token: 0x04001D90 RID: 7568
	public static List<MinigameDefinition> localPlayedMinigameList = new List<MinigameDefinition>();

	// Token: 0x04001D91 RID: 7569
	public static bool DEBUGGING = true;

	// Token: 0x04001D92 RID: 7570
	public const int MIN_PLAYERS = 2;

	// Token: 0x04001D93 RID: 7571
	public const int MAX_PLAYERS = 8;

	// Token: 0x04001D94 RID: 7572
	public static bool disconnected = false;

	// Token: 0x04001D95 RID: 7573
	public static bool disconnectUserSignOut = false;

	// Token: 0x04001D96 RID: 7574
	public static DisconnectUserEventType disconnectUserEvent = DisconnectUserEventType.None;

	// Token: 0x04001D97 RID: 7575
	public static string disconnectReason = "";

	// Token: 0x04001D98 RID: 7576
	public static PartyGameMode partyGameMode = PartyGameMode.BoardGame;

	// Token: 0x04001D99 RID: 7577
	public static readonly int[] LobbyOptionsDefaults = new int[]
	{
		0,
		0,
		3,
		4,
		5,
		2,
		0
	};

	// Token: 0x04001D9A RID: 7578
	public static readonly int[] PossibleTurnCounts = new int[]
	{
		10,
		15,
		20,
		25,
		30,
		9999
	};

	// Token: 0x04001D9B RID: 7579
	public static readonly int[] PossibleWinningRelics = new int[]
	{
		1,
		2,
		3,
		4,
		5,
		6,
		7,
		9999
	};

	// Token: 0x04001D9C RID: 7580
	public static readonly float[] PossiblyTurnLengths = new float[]
	{
		10f,
		15f,
		20f,
		25f,
		30f,
		-1f
	};

	// Token: 0x04001D9D RID: 7581
	public static readonly int[] PossibleMinigameCounts = new int[]
	{
		10,
		15,
		20,
		25,
		30
	};

	// Token: 0x04001D9E RID: 7582
	public static readonly int[] PossibleMaxPlayers = new int[]
	{
		4,
		5,
		6,
		7,
		8
	};

	// Token: 0x04001D9F RID: 7583
	public static readonly string[] GameModeStrings = new string[]
	{
		"Boardgame",
		"Minigames"
	};

	// Token: 0x04001DA0 RID: 7584
	public static readonly string[] TurnCountStrings = new string[]
	{
		"10",
		"15",
		"20",
		"25",
		"30",
		"Unlimited"
	};

	// Token: 0x04001DA1 RID: 7585
	public static readonly string[] WinningRelicsStrings = new string[]
	{
		"1",
		"2",
		"3",
		"4",
		"5",
		"6",
		"7",
		"Unlimited"
	};

	// Token: 0x04001DA2 RID: 7586
	public static readonly string[] MaxTurnLengthStrings = new string[]
	{
		"10",
		"15",
		"20",
		"25",
		"30",
		"Unlimited"
	};

	// Token: 0x04001DA3 RID: 7587
	public static readonly string[] MaxMinigameCountStrings = new string[]
	{
		"10",
		"15",
		"20",
		"25",
		"30"
	};

	// Token: 0x04001DA4 RID: 7588
	public static readonly string[] MaxPlayers = new string[]
	{
		"4",
		"5",
		"6",
		"7",
		"8"
	};

	// Token: 0x04001DA5 RID: 7589
	public static short SaveVersion = 24;

	// Token: 0x04001DA6 RID: 7590
	public static TurnSave SaveToLoad;

	// Token: 0x04001DA7 RID: 7591
	public static GameSave CurGameSave;

	// Token: 0x04001DA8 RID: 7592
	public static List<GameSave> Saves = new List<GameSave>();

	// Token: 0x04001DA9 RID: 7593
	public static int MaxGameSaves = 4;

	// Token: 0x04001DAA RID: 7594
	public static int MaxTurnSaves = 4;

	// Token: 0x04001DAB RID: 7595
	public static byte[] lobbyOptions = new byte[7];

	// Token: 0x04001DAC RID: 7596
	public static int totalTurns = 0;

	// Token: 0x04001DAD RID: 7597
	public static DateTime startTime = DateTime.Now;

	// Token: 0x04001DAE RID: 7598
	public static EmissionChangedEvent OnGlobalPlayerEmissionChanged = new EmissionChangedEvent();

	// Token: 0x04001DAF RID: 7599
	private static string filePath = Application.persistentDataPath + "/Save.pps";

	// Token: 0x04001DB0 RID: 7600
	public static int TrophyCount = 0;

	// Token: 0x04001DB1 RID: 7601
	public static bool[] unlocked = new bool[64];

	// Token: 0x020003E7 RID: 999
	// (Invoke) Token: 0x06001B9D RID: 7069
	public delegate void OnQuitEvent();

	// Token: 0x020003E8 RID: 1000
	// (Invoke) Token: 0x06001BA1 RID: 7073
	public delegate void OnXInputEvent();
}
