using System;
using System.Collections;
using System.Net;
using System.Runtime.CompilerServices;
using I2.Loc;
using Rewired;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x02000535 RID: 1333
public class UIController : MonoBehaviour
{
	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x06002307 RID: 8967 RVA: 0x000194BB File Offset: 0x000176BB
	public MainMenuWindow MultiplayerLobbyWindow
	{
		get
		{
			return this.multiplayer_lobby_window;
		}
	}

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x06002308 RID: 8968 RVA: 0x000194C3 File Offset: 0x000176C3
	// (set) Token: 0x06002309 RID: 8969 RVA: 0x000194CB File Offset: 0x000176CB
	public MainMenuWindow gameStartCountdownWnd { get; set; }

	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x0600230A RID: 8970 RVA: 0x000194D4 File Offset: 0x000176D4
	public bool OnSplashScreen
	{
		get
		{
			return this.on_splash;
		}
	}

	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x0600230B RID: 8971 RVA: 0x000194DC File Offset: 0x000176DC
	public bool SignedOut
	{
		get
		{
			return this.m_signedOut;
		}
	}

	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x0600230C RID: 8972 RVA: 0x000194E4 File Offset: 0x000176E4
	// (set) Token: 0x0600230D RID: 8973 RVA: 0x000194EC File Offset: 0x000176EC
	public bool InviteWaitingForUserLoad { get; set; }

	// Token: 0x0600230E RID: 8974 RVA: 0x000194F5 File Offset: 0x000176F5
	public void Awake()
	{
		GameManager.MainMenuUIController = this;
	}

	// Token: 0x0600230F RID: 8975 RVA: 0x000194FD File Offset: 0x000176FD
	private IEnumerator RefreshLobby()
	{
		yield return new WaitForSeconds(5f);
		for (;;)
		{
			this.RefreshList();
			yield return new WaitForSeconds(2f);
		}
		yield break;
	}

	// Token: 0x06002310 RID: 8976 RVA: 0x000D55E0 File Offset: 0x000D37E0
	public void Start()
	{
		this.UpdateUI();
		this.backgroundPanel = base.transform.Find("Background").gameObject;
		this.multiplayer_window = base.transform.Find("MultiplayerWnd").gameObject.GetComponent<MainMenuWindow>();
		this.options_window = base.transform.Find("OptionsWnd").gameObject.GetComponent<MainMenuWindow>();
		this.create_game_window = base.transform.Find("CreateGameWnd").gameObject.GetComponent<MainMenuWindow>();
		this.join_game_window = base.transform.Find("JoinLobbyWnd").gameObject.GetComponent<MainMenuWindow>();
		this.quit_window = base.transform.Find("QuitGameWnd").gameObject.GetComponent<MainMenuWindow>();
		this.multiplayer_lobby_window = base.transform.Find("MultiplayerLobbyWnd").gameObject.GetComponent<MainMenuWindow>();
		this.multiplayer_lobby_menu_btm = base.transform.Find("MultiplayerLobbyMenuBtm").gameObject.GetComponent<MainMenuWindow>();
		this.multiplayer_lobby_menu_top = base.transform.Find("MultiplayerLobbyMenuTop").gameObject.GetComponent<MainMenuWindow>();
		this.connecting_wnd = base.transform.Find("ConnectingWnd").gameObject.GetComponent<MainMenuWindow>();
		this.connecting_wnd_text = this.connecting_wnd.transform.Find("Window/Lbl").GetComponent<Text>();
		this.loading_wnd = base.transform.Find("LoadingWnd").gameObject.GetComponent<MainMenuWindow>();
		this.loading_wnd_text = this.loading_wnd.transform.Find("Window/Lbl").GetComponent<Text>();
		this.gameStartCountdownWnd = base.transform.Find("GameStartCountdownWnd").GetComponent<MainMenuWindow>();
		this.direct_connect_window = base.transform.Find("DirectConnectWnd").GetComponent<MainMenuWindow>();
		this.error_wnd = base.transform.Find("ErrorWnd").gameObject.GetComponent<DialogWindow>();
		this.lobbyStartGameBtn = this.multiplayer_lobby_menu_btm.transform.Find("Container/StartGameBtn").GetComponent<BasicButtonBase>();
		this.lobbyAddAIBtn = this.multiplayer_lobby_menu_btm.transform.Find("Container/AddAIBtn").GetComponent<BasicButtonBase>();
		this.lobbyLoadSaveBtn = this.multiplayer_lobby_menu_btm.transform.Find("Container/LoadSaveBtn").GetComponent<BasicButtonBase>();
		this.lobbyCancelLoadBtn = this.multiplayer_lobby_menu_btm.transform.Find("Container/CancelLoadBtn").GetComponent<BasicButtonBase>();
		this.mapNameLocalize = this.multiplayer_lobby_window.transform.Find("Group/LobbySettings/MapTitle/TitleBar/TitleText").GetComponent<Localize>();
		this.mapDescriptionLocalize = this.multiplayer_lobby_window.transform.Find("Group/LobbySettings/MapTitle/DescriptionBar/DescriptionText").GetComponent<Localize>();
		this.mapPreviewImage = this.multiplayer_lobby_window.transform.Find("Group/LobbySettings/MapTitle/MapImage").GetComponent<RawImage>();
		this.lobbyOptionTabs = this.multiplayer_lobby_window.transform.Find("Group/LobbySettings/Options").GetComponentsInChildren<LobbyOptionTab>();
		LobbyOptionTab[] array = new LobbyOptionTab[this.lobbyOptionTabs.Length];
		foreach (LobbyOptionTab lobbyOptionTab in this.lobbyOptionTabs)
		{
			array[(int)lobbyOptionTab.lobbyOption] = lobbyOptionTab;
		}
		this.lobbyOptionTabs = array;
		if (!this.ShouldShowSplashScreen())
		{
			this.menu_border_anim.SetBool("Hidden", false);
			this.on_splash = false;
		}
		if (GameManager.disconnectUserEvent == DisconnectUserEventType.None)
		{
			if (GameManager.disconnected && !GameManager.disconnectUserSignOut)
			{
				this.error_return_wnd = this.main_menu;
				this.error_wnd.SetDialog(GameManager.disconnectReason, this.error_txt_color, DialogType.OK);
				this.error_wnd.SetState(MainMenuWindowState.Visible);
				this.main_menu.SetState(MainMenuWindowState.Hidden);
			}
			else if (this.ShouldShowSplashScreen() || GameManager.disconnectUserSignOut)
			{
				this.m_signedOut = true;
				this.ShowSplashScreen();
				this.main_menu.SetState(MainMenuWindowState.Hidden);
				PlatformMultiplayerManager.Instance.ResetGame();
			}
			else
			{
				this.menu_border_anim.SetBool("Hidden", false);
				this.main_menu.SetState(MainMenuWindowState.Visible);
				this.on_splash = false;
			}
		}
		else if (GameManager.disconnectUserEvent == DisconnectUserEventType.AcceptedInvite)
		{
			this.main_menu.SetState(MainMenuWindowState.Hidden);
			this.menu_border_anim.SetBool("Hidden", false);
			this.on_splash = false;
		}
		else if (GameManager.disconnectUserEvent == DisconnectUserEventType.LeftGame)
		{
			this.main_menu.SetState(MainMenuWindowState.Visible);
			this.menu_border_anim.SetBool("Hidden", false);
			this.on_splash = false;
		}
		this.connectLobbyEventHandler = new ConnectLobbyEventHandler(this.Connected);
		this.disconnectLobbyEventHandler = new DisconnectEventHandler(this.Disconnected);
		NetSystem.ConnectedToLobby += this.connectLobbyEventHandler;
		NetSystem.Disconnected += this.disconnectLobbyEventHandler;
		this.connectFailedEventhandler = new ConnectFailedEventHandler(this.ConnectFailed);
		NetSystem.ConnectFailed += this.connectFailedEventhandler;
		NetGameClient.NatIntroductionSuccess += this.OnNatIntroductionSuccess;
		this.Callback_lobbyCreated = Callback<LobbyCreated_t>.Create(new Callback<LobbyCreated_t>.DispatchDelegate(this.OnLobbyCreated));
		this.Callback_lobbyEnter = Callback<LobbyEnter_t>.Create(new Callback<LobbyEnter_t>.DispatchDelegate(this.OnLobbyEntered));
		this.Callback_lobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(this.OnLobbyJoinRequested));
		GameManager.disconnectUserEvent = DisconnectUserEventType.None;
		GameManager.disconnected = false;
		GameManager.disconnectUserSignOut = false;
		LocalizationManager.OnLocalizeEvent += this.LocalizationManager_OnLocalizeEvent;
	}

	// Token: 0x06002311 RID: 8977 RVA: 0x0001950C File Offset: 0x0001770C
	private void OnStorageLoaded()
	{
		Debug.Log("UIController : OnStorageLoaded");
		this.UpdateUI();
	}

	// Token: 0x06002312 RID: 8978 RVA: 0x000D5B1C File Offset: 0x000D3D1C
	private void UpdateUI()
	{
		NetSystem.MyPlayer.Name = PlatformUtility.GetUsername();
		this.host_name_field.text = NetSystem.MyPlayer.Name;
		this.connect_name_field.text = NetSystem.MyPlayer.Name;
		this.connect_ip_field.text = RBPrefs.GetString("net_connect_ip", "127.0.0.1");
		this.connect_port_field.text = RBPrefs.GetString("net_connect_port", "14242");
		this.host_port_field.text = RBPrefs.GetString("net_host_port", "14242");
		this.upnpToggle.isOn = (RBPrefs.GetInt("net_host_upnp", 1) == 1);
		this.lobbyType.value = RBPrefs.GetInt("net_lobby_type", 0);
	}

	// Token: 0x06002313 RID: 8979 RVA: 0x0000398C File Offset: 0x00001B8C
	private void LocalizationManager_OnLocalizeEvent()
	{
	}

	// Token: 0x06002314 RID: 8980 RVA: 0x0000539F File Offset: 0x0000359F
	private bool ShouldShowSplashScreen()
	{
		return false;
	}

	// Token: 0x06002315 RID: 8981 RVA: 0x000D5BE0 File Offset: 0x000D3DE0
	public void Update()
	{
		if (LocalizationManager.CurrentLanguageCode.Equals("en") && this.UPNPTextFailed.enabled != this.UPNPFailed)
		{
			this.UPNPTextFailed.enabled = this.UPNPFailed;
		}
		if (this.m_showSplashScreen)
		{
			bool flag = this.on_splash;
		}
	}

	// Token: 0x06002316 RID: 8982 RVA: 0x0001951E File Offset: 0x0001771E
	private void OnSplashScreenAccountPickerComplete(bool cancelled, IPlatformUser user)
	{
		if (cancelled)
		{
			return;
		}
		base.StartCoroutine(this.OnSplashScreenSelectMainUser(user));
	}

	// Token: 0x06002317 RID: 8983 RVA: 0x00019532 File Offset: 0x00017732
	public void OnSelectMainUserFromInvite(IPlatformUser user)
	{
		this.menu_border_anim.SetBool("Hidden", false);
		base.StartCoroutine(this.OnSplashScreenSelectMainUser(user));
	}

	// Token: 0x06002318 RID: 8984 RVA: 0x00019553 File Offset: 0x00017753
	private IEnumerator OnSplashScreenSelectMainUser(IPlatformUser user)
	{
		PlatformUserManager.Instance.SetMainUser(user);
		PlatformStorageManager.Instance.LoadAll();
		Joystick joystick = ReInput.controllers.GetJoystick(user.RewiredJoystickID);
		if (joystick != null)
		{
			Debug.Log("OnSplashScreenSelectMainUser - systemId=" + ((joystick.systemId == null) ? "id_null" : joystick.systemId.ToString()));
		}
		else
		{
			Debug.Log("OnSplashScreenSelectMainUser - joystick=null");
		}
		ReInput.controllers.RemoveJoystickFromAllPlayers(joystick, true);
		foreach (Player player in ReInput.players.AllPlayers)
		{
			player.controllers.ClearAllControllers();
		}
		ReInput.players.GetPlayer(0).controllers.AddController(joystick, true);
		this.m_signedOut = false;
		yield return base.StartCoroutine(this.OpenMainMenu());
		yield break;
	}

	// Token: 0x06002319 RID: 8985 RVA: 0x00019569 File Offset: 0x00017769
	private void OnSwitchAccountPickerComplete(bool cancelled, IPlatformUser user)
	{
		if (cancelled)
		{
			return;
		}
		base.StartCoroutine(this.OnSwitchAccount(user));
	}

	// Token: 0x0600231A RID: 8986 RVA: 0x0001957D File Offset: 0x0001777D
	private IEnumerator OnSwitchAccount(IPlatformUser user)
	{
		PlatformUserManager.Instance.SetMainUser(user);
		PlatformStorageManager.Instance.LoadAll();
		yield return base.StartCoroutine(this.LoadUserToMainMenu());
		yield break;
	}

	// Token: 0x0600231B RID: 8987 RVA: 0x00019593 File Offset: 0x00017793
	private void OnMainUserSignedOut()
	{
		Debug.Log("UIController : OnMainUserSignOut");
		this.signOutStarted = true;
		this.signOutCompleted = false;
		base.StartCoroutine(this.LoadUserToSplashScreen(true));
	}

	// Token: 0x0600231C RID: 8988 RVA: 0x000195BB File Offset: 0x000177BB
	private void OnMainUserSignOutCompleted()
	{
		this.signOutStarted = false;
		this.signOutCompleted = true;
	}

	// Token: 0x0600231D RID: 8989 RVA: 0x000195CB File Offset: 0x000177CB
	public void SuspendUserSignedOut()
	{
		Debug.Log("UIController : Suspend User Signed Out");
		base.StartCoroutine(this.LoadUserToSplashScreen(true));
	}

	// Token: 0x0600231E RID: 8990 RVA: 0x000195E5 File Offset: 0x000177E5
	public void ControllerDisconnectedUserChanged()
	{
		Debug.Log("UIController : Controller Disconnected User Changed");
		base.StartCoroutine(this.LoadUserToSplashScreen(false));
	}

	// Token: 0x0600231F RID: 8991 RVA: 0x000195FF File Offset: 0x000177FF
	private IEnumerator OpenMainMenu()
	{
		Debug.Log("UIController : OpenMainMenu");
		this.ExitSplashScreen();
		yield return base.StartCoroutine(this.WaitForProfileLoad());
		if (!this.InviteWaitingForUserLoad)
		{
			if (PlatformUserManager.Instance.MainUser.IsSignedIn)
			{
				this.ShowMainMenu();
			}
			else
			{
				this.menu_border_anim.SetBool("Hidden", false);
				this.backgroundPanel.SetActive(true);
				this.ShowErrorWindow(LocalizationManager.GetTranslation("UIPromptNoSave", true, 0, true, false, null, null, true), DialogType.OK);
			}
		}
		this.InviteWaitingForUserLoad = false;
		yield break;
	}

	// Token: 0x06002320 RID: 8992 RVA: 0x0001960E File Offset: 0x0001780E
	private IEnumerator LoadUserToMainMenu()
	{
		Debug.Log("UIController : LoadUserToMainMenu");
		this.HideAllMainMenuUI();
		this.HideLobbyUI();
		if (NetSystem.IsConnected)
		{
			this.ShutdownLobby();
		}
		yield return base.StartCoroutine(this.OpenMainMenu());
		yield break;
	}

	// Token: 0x06002321 RID: 8993 RVA: 0x0001961D File Offset: 0x0001781D
	private IEnumerator LoadUserToSplashScreen(bool waitForSignout = true)
	{
		Debug.Log("UIController : LoadUserToSplashScreen");
		this.m_signedOut = true;
		ReInput.players.GetPlayer(0).controllers.ClearAllControllers();
		if (NetSystem.IsConnected)
		{
			this.ShutdownLobby();
		}
		this.HideAllMainMenuUI();
		this.HideLobbyUI();
		PlatformMultiplayerManager.Instance.ResetGame();
		if (waitForSignout)
		{
			this.ShowLoadingWindow();
			this.SetLoadingWindowText(LocalizationManager.GetTranslation("UIPromptSigningOut", true, 0, true, false, null, null, true));
			yield return new WaitUntil(() => this.signOutCompleted);
			this.HideLoadingWindow();
		}
		this.ShowSplashScreen();
		this.InviteWaitingForUserLoad = false;
		yield break;
	}

	// Token: 0x06002322 RID: 8994 RVA: 0x00019633 File Offset: 0x00017833
	private IEnumerator WaitForProfileLoad()
	{
		Debug.Log("UIController : WaitForProfileToLoad");
		this.SetLoadingWindowText(LocalizationManager.GetTranslation("Loading", true, 0, true, false, null, null, true));
		this.ShowLoadingWindow();
		float waitStart = Time.time;
		yield return new WaitForSeconds(0.25f);
		while (!PlatformStorageManager.Instance.HasLoaded() && Time.time - waitStart < this.m_maxProfileLoadWaitTime)
		{
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(0.25f);
		this.HideLoadingWindow();
		yield break;
	}

	// Token: 0x06002323 RID: 8995 RVA: 0x00019642 File Offset: 0x00017842
	public void ShowMainMenu()
	{
		this.main_menu.SetState(MainMenuWindowState.Visible);
		this.menu_border_anim.SetBool("Hidden", false);
		this.backgroundPanel.SetActive(true);
	}

	// Token: 0x06002324 RID: 8996 RVA: 0x0001966D File Offset: 0x0001786D
	public void ShowSplashScreen()
	{
		this.backgroundPanel.SetActive(false);
		this.splash_window.gameObject.SetActive(true);
		this.on_splash = true;
	}

	// Token: 0x06002325 RID: 8997 RVA: 0x00019693 File Offset: 0x00017893
	public void ExitSplashScreen()
	{
		if (this.on_splash)
		{
			this.splash_window.gameObject.SetActive(false);
			ReInput.configuration.autoAssignJoysticks = true;
			ReInput.controllers.AutoAssignJoysticks();
			this.on_splash = false;
		}
	}

	// Token: 0x06002326 RID: 8998 RVA: 0x000196CA File Offset: 0x000178CA
	public void SetConnectIP()
	{
		RBPrefs.SetString("net_connect_ip", this.connect_ip_field.text);
	}

	// Token: 0x06002327 RID: 8999 RVA: 0x000196E1 File Offset: 0x000178E1
	public void SetLobbyType()
	{
		RBPrefs.SetInt("net_lobby_type", this.lobbyType.value);
	}

	// Token: 0x06002328 RID: 9000 RVA: 0x000196F8 File Offset: 0x000178F8
	public void SetForwardPortType()
	{
		RBPrefs.SetInt("net_host_upnp", this.upnpToggle.isOn ? 1 : 0);
	}

	// Token: 0x06002329 RID: 9001 RVA: 0x000D5C34 File Offset: 0x000D3E34
	public void HideLobbyUI()
	{
		this.SetLobbyState(MainMenuWindowState.Hidden);
		UIMultiplayerLobbySlot[] array = this.lobby_slots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Reset();
		}
		GameManager.SaveToLoad = null;
	}

	// Token: 0x0600232A RID: 9002 RVA: 0x00019715 File Offset: 0x00017915
	public void ShutdownLobby()
	{
		this.LeaveMultiplayerLobby();
		NetSystem.Destroy();
		this.Callback_lobbyEnter.Unregister();
		this.Callback_lobbyCreated.Unregister();
		this.Callback_lobbyJoinRequested.Unregister();
	}

	// Token: 0x0600232B RID: 9003 RVA: 0x000D5C6C File Offset: 0x000D3E6C
	public void LeaveMultiplayerLobbyUI()
	{
		RBPrefs.Save();
		this.SetLobbyState(MainMenuWindowState.Hidden);
		UIMultiplayerLobbySlot[] array = this.lobby_slots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Reset();
		}
		if (NetSystem.IsServer)
		{
			if (NetGameServer.IsLocal)
			{
				this.main_menu.SetState(MainMenuWindowState.Visible);
				this.SetWindowName("Pummel Party");
			}
			else
			{
				this.create_game_window.SetState(MainMenuWindowState.Visible);
				this.SetWindowName("Multiplayer");
			}
		}
		else
		{
			this.SetWindowName("Multiplayer");
			this.join_game_window.SetState(MainMenuWindowState.Visible);
		}
		this.LeaveMultiplayerLobby();
		GameManager.SaveToLoad = null;
	}

	// Token: 0x0600232C RID: 9004 RVA: 0x00019743 File Offset: 0x00017943
	public void LeaveMultiplayerLobby()
	{
		NetSystem.Destroy();
		if (SteamManager.Initialized)
		{
			SteamMatchmaking.LeaveLobby((CSteamID)GameManager.CurrentLobby);
		}
	}

	// Token: 0x0600232D RID: 9005 RVA: 0x00019760 File Offset: 0x00017960
	public void SetWindowName(string text)
	{
		this.window_text.SetTerm(text);
	}

	// Token: 0x0600232E RID: 9006 RVA: 0x0001976E File Offset: 0x0001796E
	public void SetHoverText(string text)
	{
		this.hover_description.SetTerm(text);
	}

	// Token: 0x0600232F RID: 9007 RVA: 0x0001977C File Offset: 0x0001797C
	private void RelayConnectFailed(string msg)
	{
		Debug.LogError("Relay Connect Failed : " + msg);
		if (!this.m_relayConnectAttemptComplete)
		{
			this.m_relayConnectAttemptComplete = true;
			this.ConnectFailed(msg);
		}
	}

	// Token: 0x06002330 RID: 9008 RVA: 0x000D5D08 File Offset: 0x000D3F08
	private void ConnectFailed(string msg)
	{
		if (!this.m_relayConnectAttemptComplete)
		{
			this.m_relayConnectFailed = true;
			this.m_relayConnectAttemptComplete = true;
			NetSteamRelay.Destroy();
		}
		if (!(msg == "Nat Intro Failed"))
		{
			if (SteamManager.Initialized)
			{
				SteamMatchmaking.LeaveLobby((CSteamID)GameManager.CurrentLobby);
			}
			this.connecting_wnd.SetState(MainMenuWindowState.Hidden);
			this.error_return_wnd = this.join_game_window;
			this.error_wnd.SetDialog(msg, this.error_txt_color, DialogType.OK);
			this.error_wnd.SetState(MainMenuWindowState.Visible);
			return;
		}
		Debug.Log("Nat Intro Failed ... Attempting Steam P2P Connection");
		if (SteamManager.Initialized)
		{
			this.AttemptSteamConnect();
			return;
		}
		Debug.LogError("STEAM NOT INITIALIZED?");
		this.connecting_wnd.SetState(MainMenuWindowState.Hidden);
		this.error_return_wnd = this.join_game_window;
		this.error_wnd.SetDialog("Steam Not Initialized", this.error_txt_color, DialogType.OK);
		this.error_wnd.SetState(MainMenuWindowState.Visible);
	}

	// Token: 0x06002331 RID: 9009 RVA: 0x000197A4 File Offset: 0x000179A4
	private void RelayConnectSuccess()
	{
		Debug.LogError("Relay connect success");
		if (!this.m_relayConnectAttemptComplete)
		{
			this.m_relayConnectAttemptComplete = true;
		}
	}

	// Token: 0x06002332 RID: 9010 RVA: 0x000197BF File Offset: 0x000179BF
	private void Connected()
	{
		this.m_relayConnectAttemptComplete = true;
		this.connecting_wnd.SetState(MainMenuWindowState.Hidden);
		this.SetLobbyState(MainMenuWindowState.Visible);
		this.SetWindowName("GameLobby");
		this.SetLobbyTypeText();
	}

	// Token: 0x06002333 RID: 9011 RVA: 0x000D5DE8 File Offset: 0x000D3FE8
	private void Disconnected(string reason)
	{
		if (GameManager.LoadScreen != null && GameManager.LoadScreen.gameObject.activeSelf)
		{
			GameManager.LoadScreen.Hide();
		}
		this.SetLobbyState(MainMenuWindowState.Hidden);
		this.friendsListDialogManager.Hide();
		this.loadSaveDialogWindow.SetState(MainMenuWindowState.Hidden);
		this.gameStartCountdownWnd.SetState(MainMenuWindowState.Hidden);
		this.gameRulesWindow.SetState(MainMenuWindowState.Hidden);
		UIMultiplayerLobbySlot[] array = this.lobby_slots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Reset();
		}
		this.connecting_wnd.SetState(MainMenuWindowState.Hidden);
		this.error_return_wnd = this.join_game_window;
		this.error_wnd.SetDialog(reason, this.error_txt_color, DialogType.OK);
		this.error_wnd.SetState(MainMenuWindowState.Visible);
		if (SteamManager.Initialized)
		{
			SteamMatchmaking.LeaveLobby((CSteamID)GameManager.CurrentLobby);
		}
		NetSteamRelay.Destroy();
	}

	// Token: 0x06002334 RID: 9012 RVA: 0x000197EC File Offset: 0x000179EC
	public void SetMinigameSettingsButtonState(BasicButtonBase.BasicButtonState state)
	{
		this.minigameSettingsBtn.SetState(state);
	}

	// Token: 0x06002335 RID: 9013 RVA: 0x000197FA File Offset: 0x000179FA
	public void SetStartGameButtonState(BasicButtonBase.BasicButtonState state)
	{
		this.lobbyStartGameBtn.SetState(state);
	}

	// Token: 0x06002336 RID: 9014 RVA: 0x00019808 File Offset: 0x00017A08
	public void SetAddAIButtonState(BasicButtonBase.BasicButtonState state)
	{
		this.lobbyAddAIBtn.SetState(state);
	}

	// Token: 0x06002337 RID: 9015 RVA: 0x00019816 File Offset: 0x00017A16
	public void SetInviteButtonState(BasicButtonBase.BasicButtonState state)
	{
		this.lobbyInviteBtn.SetState(state);
	}

	// Token: 0x06002338 RID: 9016 RVA: 0x00019824 File Offset: 0x00017A24
	public void SetLoadButtonState(BasicButtonBase.BasicButtonState state)
	{
		this.lobbyLoadSaveBtn.SetState(state);
	}

	// Token: 0x06002339 RID: 9017 RVA: 0x000D5EC4 File Offset: 0x000D40C4
	public void SetLobbyOption(LobbyOption type, int id)
	{
		this.lobbyOptionTabs[(int)type].SetOption(id);
		switch (type)
		{
		case LobbyOption.GameMode:
			GameManager.partyGameMode = (PartyGameMode)id;
			if (GameManager.partyGameMode == PartyGameMode.BoardGame)
			{
				if (GameManager.CurMap != null)
				{
					this.mapNameLocalize.SetTerm(GameManager.CurMap.name);
					this.mapDescriptionLocalize.SetTerm(GameManager.CurMap.description);
					this.mapPreviewImage.texture = GameManager.CurMap.previewImage;
					this.mapPreviewImage.color = new Color(1f, 1f, 1f, 1f);
				}
				this.mapTab.SetActive(true);
				this.turnCountTab.SetActive(true);
				this.winningRelicsTab.SetActive(true);
				this.turnLengthsTab.SetActive(true);
				this.minigameCountTab.SetActive(false);
				return;
			}
			if (GameManager.partyGameMode == PartyGameMode.MinigamesOnly)
			{
				this.mapNameLocalize.SetTerm("MinigamesOnly");
				this.mapDescriptionLocalize.SetTerm("MinigamesOnlyDescription");
				this.mapPreviewImage.texture = this.minigamesOnlyPreviewImage;
				this.mapTab.SetActive(false);
				this.turnCountTab.SetActive(false);
				this.winningRelicsTab.SetActive(false);
				this.turnLengthsTab.SetActive(false);
				this.minigameCountTab.SetActive(true);
				return;
			}
			break;
		case LobbyOption.Map:
		{
			MapDetails map = GameManager.GetMap(id);
			GameManager.CurMap = map;
			if (GameManager.partyGameMode == PartyGameMode.BoardGame)
			{
				this.mapNameLocalize.SetTerm(map.name);
				this.mapDescriptionLocalize.SetTerm(map.description);
				this.mapPreviewImage.texture = map.previewImage;
				this.mapPreviewImage.color = new Color(1f, 1f, 1f, 1f);
				return;
			}
			break;
		}
		case LobbyOption.TurnCount:
			GameManager.TurnCount = GameManager.PossibleTurnCounts[id];
			return;
		case LobbyOption.WinningRelics:
			GameManager.WinningRelics = GameManager.PossibleWinningRelics[id];
			return;
		case LobbyOption.MaxTurnLength:
			GameManager.TurnLength = GameManager.PossiblyTurnLengths[id];
			return;
		case LobbyOption.MinigameCount:
			GameManager.MinigameModeCount = GameManager.PossibleMinigameCounts[id];
			return;
		case LobbyOption.MaxPlayers:
		{
			int lobbyMaxPlayers = GameManager.LobbyMaxPlayers;
			GameManager.LobbyMaxPlayers = GameManager.PossibleMaxPlayers[id];
			if (NetSystem.IsServer)
			{
				NetSystem.SetMaxConnections(GameManager.LobbyMaxPlayers);
				if (SteamManager.Initialized && SteamMatchmaking.GetLobbyOwner((CSteamID)GameManager.CurrentLobby) == SteamUser.GetSteamID())
				{
					SteamMatchmaking.SetLobbyMemberLimit((CSteamID)GameManager.CurrentLobby, GameManager.LobbyMaxPlayers);
				}
			}
			foreach (UIMultiplayerLobbySlot uimultiplayerLobbySlot in this.lobby_slots)
			{
				uimultiplayerLobbySlot.Set8PlayerSlots(GameManager.LobbyMaxPlayers > 4);
				uimultiplayerLobbySlot.gameObject.SetActive(uimultiplayerLobbySlot.slot_index < GameManager.LobbyMaxPlayers);
			}
			if (lobbyMaxPlayers > GameManager.LobbyMaxPlayers)
			{
				for (int j = lobbyMaxPlayers - 1; j >= GameManager.LobbyMaxPlayers; j--)
				{
					GameManager.LobbyController.ForceRemovePlayer((short)j);
				}
				GameManager.LobbyController.UpdatePlayers();
			}
			GameManager.LobbyController.UpdateAddAIButton();
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x0600233A RID: 9018 RVA: 0x00019832 File Offset: 0x00017A32
	private void MultiplayerWindowback()
	{
		this.multiplayer_window.SetState(MainMenuWindowState.Hidden);
		this.main_menu.SetState(MainMenuWindowState.Visible);
		this.SetWindowName("Pummel Party");
	}

	// Token: 0x0600233B RID: 9019 RVA: 0x00019857 File Offset: 0x00017A57
	private IEnumerator Failed()
	{
		yield return new WaitForSeconds(20f);
		this.error_return_wnd = this.main_menu;
		this.error_wnd.SetDialog(LocalizationManager.GetTranslation("MultiplayerConnectionTimeout", true, 0, true, false, null, null, true), this.error_txt_color, DialogType.OK);
		this.error_wnd.SetState(MainMenuWindowState.Visible);
		yield break;
	}

	// Token: 0x0600233C RID: 9020 RVA: 0x000D61B4 File Offset: 0x000D43B4
	public void DoButtonEvent(MainMenuButtonEventType btn_event)
	{
		switch (btn_event)
		{
		case MainMenuButtonEventType.GoSingleplayerWindow:
			this.HostGame(true, false);
			return;
		case MainMenuButtonEventType.GoMultiplayerWindow:
			this.main_menu.SetState(MainMenuWindowState.Hidden);
			this.multiplayer_window.SetState(MainMenuWindowState.Visible);
			this.SetWindowName("Multiplayer");
			return;
		case MainMenuButtonEventType.ExitGame:
			Application.Quit();
			return;
		case MainMenuButtonEventType.GoCreateGameWindow:
			this.host_name_field.text = PlatformUtility.GetUsername();
			this.multiplayer_window.SetState(MainMenuWindowState.Hidden);
			this.create_game_window.SetState(MainMenuWindowState.Visible);
			this.SetWindowName("CreateGame");
			return;
		case MainMenuButtonEventType.GoJoinGameWindow:
			this.RefreshList();
			this.multiplayer_window.SetState(MainMenuWindowState.Hidden);
			this.join_game_window.SetState(MainMenuWindowState.Visible);
			this.SetWindowName("Join Game");
			return;
		case MainMenuButtonEventType.MultiplayerWindowBack:
			this.MultiplayerWindowback();
			return;
		case MainMenuButtonEventType.HostGame:
		{
			GameManager.SaveToLoad = null;
			string text = PlatformUtility.GetUsername();
			text = this.host_name_field.text;
			NetSystem.SetPlayerName(text);
			NetGameServer.ServerName = text + "'s Game";
			RBPrefs.SetString("net_player_name", text);
			this.HostGame(false, false);
			return;
		}
		case MainMenuButtonEventType.CreateGameWindowBack:
			NetSystem.Destroy();
			this.create_game_window.SetState(MainMenuWindowState.Hidden);
			this.multiplayer_window.SetState(MainMenuWindowState.Visible);
			this.SetWindowName("Multiplayer");
			return;
		case MainMenuButtonEventType.GoOptionsWindow:
		case MainMenuButtonEventType.OptionsWindowBack:
		case MainMenuButtonEventType.ApplyOptions:
		case MainMenuButtonEventType.OptionsControls:
		case MainMenuButtonEventType.OptionsGraphics:
		case MainMenuButtonEventType.OptionsSound:
		case MainMenuButtonEventType.OptionsTabLeft:
		case MainMenuButtonEventType.OptionsTabRight:
		case MainMenuButtonEventType.OptionsRestoreDefaults:
		case MainMenuButtonEventType.OptionsGeneral:
		case MainMenuButtonEventType.MinigamesOnlySkip:
		case MainMenuButtonEventType.OptionsCredits:
		case MainMenuButtonEventType.CreditsSection:
		case MainMenuButtonEventType.ThanksSection:
			this.optionsWindow.DoButtonEvent(btn_event);
			return;
		case MainMenuButtonEventType.ConnectToGame:
			GameManager.SaveToLoad = null;
			this.JoinGame(this.connect_ip_field.text, int.Parse(this.connect_port_field.text), false, false);
			return;
		case MainMenuButtonEventType.JoinGameWindowBack:
			NetSystem.Destroy();
			this.join_game_window.SetState(MainMenuWindowState.Hidden);
			this.multiplayer_window.SetState(MainMenuWindowState.Visible);
			this.SetWindowName("Multiplayer");
			return;
		case MainMenuButtonEventType.GoQuitGameWindow:
			this.quit_window.SetState(MainMenuWindowState.Visible);
			this.main_menu.SetState(MainMenuWindowState.Hidden);
			this.SetWindowName("Quit?");
			return;
		case MainMenuButtonEventType.CancelQuit:
			this.main_menu.SetState(MainMenuWindowState.Visible);
			this.quit_window.SetState(MainMenuWindowState.Hidden);
			this.SetWindowName("Pummel Party");
			return;
		case MainMenuButtonEventType.LeaveMultiplayerLobby:
			this.LeaveMultiplayerLobbyUI();
			return;
		case MainMenuButtonEventType.StartGame:
			if (!NetSystem.IsServer)
			{
				Debug.Log("Not Server!");
				return;
			}
			for (int i = 0; i < NetSystem.PlayerCount; i++)
			{
				bool flag = false;
				for (int j = 0; j < GameManager.GetPlayerCount(); j++)
				{
					if (GameManager.GetPlayerAt(j).NetOwner == NetSystem.GetPlayerAtIndex(i))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					NetSystem.GetPlayerAtIndex(i).Connection.Disconnect("No player in slot");
				}
			}
			GameManager.LobbyController.BeginCountdown();
			if (GameManager.SaveToLoad != null)
			{
				GameManager.LobbyController.LoadSave();
				return;
			}
			break;
		case MainMenuButtonEventType.ErrorOK:
			this.error_wnd.SetState(MainMenuWindowState.Hidden);
			if (this.error_return_wnd != null)
			{
				this.error_return_wnd.SetState(MainMenuWindowState.Visible);
				this.error_return_wnd = null;
				return;
			}
			this.main_menu.SetState(MainMenuWindowState.Visible);
			return;
		case MainMenuButtonEventType.CancelStartGameCountdown:
			GameManager.LobbyController.CancelCountdown();
			return;
		case MainMenuButtonEventType.None:
		case MainMenuButtonEventType.OptionsLeaveGame:
		case MainMenuButtonEventType.OptionsQuitGame:
		case MainMenuButtonEventType.SelectProfile:
			break;
		case MainMenuButtonEventType.AddAI:
			GameManager.LobbyController.AddAI();
			return;
		case MainMenuButtonEventType.InviteFriends:
			SteamFriends.ActivateGameOverlayInviteDialog((CSteamID)GameManager.CurrentLobby);
			return;
		case MainMenuButtonEventType.ConnecToGameNatPunch:
			this.JoinGame(this.connect_ip_field.text, int.Parse(this.connect_port_field.text), true, false);
			return;
		case MainMenuButtonEventType.RefreshLobbyList:
			this.RefreshList();
			return;
		case MainMenuButtonEventType.GoDirectConnectWindow:
			this.multiplayer_window.SetState(MainMenuWindowState.Hidden);
			this.direct_connect_window.SetState(MainMenuWindowState.Visible);
			this.SetWindowName("Direct Connect");
			return;
		case MainMenuButtonEventType.DirectConnetWindowBack:
			this.direct_connect_window.SetState(MainMenuWindowState.Hidden);
			this.multiplayer_window.SetState(MainMenuWindowState.Visible);
			this.SetWindowName("Multiplayer");
			return;
		case MainMenuButtonEventType.OpenLoadSave:
			this.SetLobbyState(MainMenuWindowState.Hidden);
			this.loadSaveDialogWindow.SetState(MainMenuWindowState.Visible);
			GameManager.LobbyController.scene.Hide();
			return;
		case MainMenuButtonEventType.ExitLoadSave:
			this.SetLobbyState(MainMenuWindowState.Visible);
			this.loadSaveDialogWindow.SetState(MainMenuWindowState.Hidden);
			GameManager.LobbyController.scene.Show();
			return;
		case MainMenuButtonEventType.LoadSave:
			this.SetLobbyState(MainMenuWindowState.Visible);
			this.loadSaveDialogWindow.SetState(MainMenuWindowState.Hidden);
			this.loadSaveDialog.Load();
			this.SetLoadingSaveGame(true);
			GameManager.LobbyController.scene.Show();
			return;
		case MainMenuButtonEventType.CancelLoad:
			this.SetLoadingSaveGame(false);
			GameManager.LobbyController.CancelLoad();
			return;
		case MainMenuButtonEventType.CancelOnScreenKeyboard:
			this.onScreenKeyboardUI.Cancel();
			return;
		case MainMenuButtonEventType.FinishOnScreenKeyboard:
			this.onScreenKeyboardUI.Finish();
			return;
		case MainMenuButtonEventType.OpenFriendListDialog:
			this.friendsListDialogManager.Show();
			this.SetLobbyState(MainMenuWindowState.Hidden);
			GameManager.LobbyController.scene.Hide();
			return;
		case MainMenuButtonEventType.ExitFriendsListDialog:
			this.SetLobbyState(MainMenuWindowState.Visible);
			GameManager.LobbyController.scene.Show();
			this.friendsListDialogManager.Hide();
			return;
		case MainMenuButtonEventType.OpenMinigameItemSettings:
			this.SetLobbyState(MainMenuWindowState.Hidden);
			this.minigameItemSettingsWindow.SetState(MainMenuWindowState.Visible);
			GameManager.LobbyController.scene.Hide();
			return;
		case MainMenuButtonEventType.ExitMinigameItemSettings:
			GameManager.UpdatePsuedoRandomMinigameList();
			this.SetLobbyState(MainMenuWindowState.Visible);
			this.minigameItemSettingsWindow.SetState(MainMenuWindowState.Hidden);
			GameManager.LobbyController.scene.Show();
			return;
		case MainMenuButtonEventType.OpenGameRulesets:
			this.SetLobbyState(MainMenuWindowState.Hidden);
			this.gameRulesWindow.SetState(MainMenuWindowState.Visible);
			GameManager.LobbyController.scene.Hide();
			this.m_rulesetUIWindow.Reset();
			return;
		case MainMenuButtonEventType.ExitGameRulesets:
			this.SetLobbyState(MainMenuWindowState.Visible);
			this.gameRulesWindow.SetState(MainMenuWindowState.Hidden);
			GameManager.LobbyController.scene.Show();
			GameManager.RulesetManager.Save();
			GameManager.UpdatePsuedoRandomMinigameList();
			return;
		case MainMenuButtonEventType.GoUnlockWindow:
			this.main_menu.SetState(MainMenuWindowState.Hidden);
			this.unlockWindowManager.Show();
			this.SetWindowName("Unlocks");
			return;
		case MainMenuButtonEventType.LeaveUnlockWindow:
			this.unlockWindowManager.Hide();
			this.main_menu.SetState(MainMenuWindowState.Visible);
			this.SetWindowName("Pummel Party");
			break;
		default:
			return;
		}
	}

	// Token: 0x0600233D RID: 9021 RVA: 0x000D6784 File Offset: 0x000D4984
	private void SetLoadingSaveGame(bool state)
	{
		if (NetSystem.IsServer)
		{
			this.SetLobbyOptionInteractable(!state);
			this.lobbyCancelLoadBtn.pollAction = state;
			this.lobbyLoadSaveBtn.pollAction = !state;
			Vector3 vector = new Vector3(-498f, 0f, 0f);
			Vector3 vector2 = Vector3.right * 9999f;
			this.lobbyLoadSaveBtn.GetComponent<RectTransform>().anchoredPosition = (state ? vector2 : vector);
			this.lobbyCancelLoadBtn.GetComponent<RectTransform>().anchoredPosition = (state ? vector : vector2);
		}
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x00019866 File Offset: 0x00017A66
	public void SetLobbyState(MainMenuWindowState state)
	{
		base.StartCoroutine(this.ShowLobbyDelayed(state));
	}

	// Token: 0x0600233F RID: 9023 RVA: 0x00019876 File Offset: 0x00017A76
	private IEnumerator ShowLobbyDelayed(MainMenuWindowState state)
	{
		yield return null;
		this.multiplayer_lobby_window.SetState(state);
		yield return new WaitForSeconds(0.05f);
		this.multiplayer_lobby_menu_top.SetState(state);
		yield return new WaitForSeconds(0.05f);
		this.multiplayer_lobby_menu_btm.SetState(state);
		yield break;
	}

	// Token: 0x06002340 RID: 9024 RVA: 0x000D6820 File Offset: 0x000D4A20
	private void SetLobbyOptionInteractable(bool state)
	{
		LobbyOptionTab[] array = this.lobbyOptionTabs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetInteractable(state);
		}
	}

	// Token: 0x06002341 RID: 9025 RVA: 0x000D684C File Offset: 0x000D4A4C
	private void CreateServerCallback(string outcome)
	{
		Debug.Log("CreateServerCallback : " + outcome);
		if (outcome == "")
		{
			this.SetLobbyState(MainMenuWindowState.Visible);
			this.SetLoadingSaveGame(false);
			this.SetWindowName("Game Lobby");
			this.SetHoverText("EmptyString");
			if (!this.local && SteamManager.Initialized)
			{
				NetSteamRelay.Create(true);
				NetGameServer.SetRelayProvider(NetSteamRelay.Instance);
			}
			if (SteamManager.Initialized && !this.local)
			{
				SteamMatchmaking.CreateLobby((ELobbyType)this.lobbyType.value, 4);
			}
			else
			{
				this.SetLobbyTypeText();
			}
			NetSystem.Spawn("MultiplayerLobbyController", 0, NetSystem.MyPlayer);
			if (this.net_test)
			{
				this.DoNetTestLogic();
				return;
			}
		}
		else
		{
			this.error_return_wnd = this.create_game_window;
			this.error_wnd.SetDialog(outcome, this.error_txt_color, DialogType.OK);
			this.error_wnd.SetState(MainMenuWindowState.Visible);
		}
	}

	// Token: 0x06002342 RID: 9026 RVA: 0x000D6930 File Offset: 0x000D4B30
	private void HostGame(bool local, bool net_test = false)
	{
		Debug.Log("HostGame");
		this.local = local;
		this.net_test = net_test;
		this.lobbyInviteBtn.SetState(local ? BasicButtonBase.BasicButtonState.Disabled : BasicButtonBase.BasicButtonState.Enabled);
		if (net_test || local)
		{
			this.main_menu.SetState(MainMenuWindowState.Hidden);
		}
		else
		{
			this.create_game_window.SetState(MainMenuWindowState.Hidden);
		}
		NetGameServer.CreateServer(int.Parse(this.host_port_field.text), this.upnpToggle.isOn, new UnityAction<string>(this.CreateServerCallback), local, 0);
		GameManager.UpdatePsuedoRandomMinigameList();
	}

	// Token: 0x06002343 RID: 9027 RVA: 0x000D69BC File Offset: 0x000D4BBC
	public void JoinGame(string ip, int port, bool natIntro, bool net_test = false)
	{
		this.OnJoinUI(net_test, natIntro);
		string text = NetGameClient.Start(14300);
		if (!(text == ""))
		{
			NetSystem.OnConnectFailed(text);
			return;
		}
		if (natIntro)
		{
			NetGameClient.RequestNatIntroduction(ip, port, true);
			return;
		}
		NetGameClient.Connect(ip, port);
	}

	// Token: 0x06002344 RID: 9028 RVA: 0x000D6A08 File Offset: 0x000D4C08
	private void OnLobbyEntered(LobbyEnter_t result)
	{
		if (result.m_EChatRoomEnterResponse == 1U && SteamManager.Initialized && !NetSystem.IsServer)
		{
			Debug.Log("Joined Lobby: " + result.m_ulSteamIDLobby.ToString());
			GameManager.CurrentLobby = result.m_ulSteamIDLobby;
			this.SetLobbyTypeText();
			this.AttemptNatConnect();
		}
		else if (!NetSystem.IsServer)
		{
			Debug.Log("Failed to join Lobby");
			if (this.lastAttemptedLobbyEntry != null)
			{
				this.lastAttemptedLobbyEntry.Failed();
			}
		}
		this.SetLobbyTypeText();
	}

	// Token: 0x06002345 RID: 9029 RVA: 0x000D6A90 File Offset: 0x000D4C90
	private void AttemptNatConnect()
	{
		this.OnJoinUI(false, true);
		string text = NetGameClient.Start(14300);
		Debug.Log("NetGameClient Creation Outcome: " + text);
		if (text == "")
		{
			base.StartCoroutine(this.RequestNatIntroduction());
			return;
		}
		NetSystem.OnConnectFailed(text);
	}

	// Token: 0x06002346 RID: 9030 RVA: 0x000D6AE4 File Offset: 0x000D4CE4
	private void AttemptSteamConnect()
	{
		NetSteamRelay.Create(false);
		NetSteamRelay.Instance.RelayConnectFailed += this.RelayConnectFailed;
		NetSteamRelay.Instance.RelayConnectSucceeded += this.RelayConnectSuccess;
		this.OnJoinUI(false, true);
		if (this.m_relayConnectUICoroutine != null)
		{
			base.StopCoroutine(this.m_relayConnectUICoroutine);
			this.m_relayConnectUICoroutine = null;
		}
		if (this.m_relayConnectCoroutine != null)
		{
			base.StopCoroutine(this.m_relayConnectCoroutine);
			this.m_relayConnectCoroutine = null;
		}
		this.m_relayConnectUICoroutine = base.StartCoroutine(this.StartRelayConnect((CSteamID)GameManager.CurrentLobby));
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x0001988C File Offset: 0x00017A8C
	private IEnumerator StartRelayConnect(CSteamID lobbyId)
	{
		yield return new WaitForSeconds(0.1f);
		this.m_relayConnectFailed = false;
		this.m_relayConnectAttemptComplete = false;
		this.m_relayConnectCoroutine = base.StartCoroutine(NetSteamRelay.Instance.ConnectToLobby(lobbyId));
		float startTime = Time.time;
		while (Time.time - startTime < 30f && !this.m_relayConnectAttemptComplete)
		{
			this.connecting_wnd_text.text = NetSteamRelay.Instance.GetStatusString();
			yield return null;
		}
		if (!this.m_relayConnectAttemptComplete)
		{
			NetSystem.OnConnectFailed(LocalizationManager.GetTranslation("NetworkTimeout", true, 0, true, false, null, null, true));
		}
		yield break;
	}

	// Token: 0x06002348 RID: 9032 RVA: 0x000198A2 File Offset: 0x00017AA2
	private IEnumerator RequestNatIntroduction()
	{
		bool first = true;
		do
		{
			NetGameClient.RequestNatIntroduction(GameManager.CurrentLobby, first);
			first = false;
			int num = (int)NetSystem.NatConnectTimeRemaining;
			this.connecting_wnd_text.text = "Attempting Nat Introduction Please Wait ... " + num.ToString();
			yield return new WaitForSeconds(0.25f);
		}
		while (NetGameClient.WaitingForNatIntroductionSuccess);
		yield break;
	}

	// Token: 0x06002349 RID: 9033 RVA: 0x000D6B80 File Offset: 0x000D4D80
	private void OnLobbyCreated(LobbyCreated_t result)
	{
		if (result.m_eResult == EResult.k_EResultOK && SteamManager.Initialized)
		{
			GameManager.CurrentLobby = result.m_ulSteamIDLobby;
			Debug.Log("Steam Lobby Created: " + result.m_ulSteamIDLobby.ToString());
			string personaName = SteamFriends.GetPersonaName();
			SteamMatchmaking.SetLobbyData((CSteamID)GameManager.CurrentLobby, "name", personaName);
			SteamMatchmaking.SetLobbyData((CSteamID)GameManager.CurrentLobby, "version", GameManager.VERSION);
			SteamMatchmaking.SetLobbyData((CSteamID)GameManager.CurrentLobby, "Port", this.host_port_field.text);
			string pchValue = SteamUtils.GetServerRealTime().ToString();
			SteamMatchmaking.SetLobbyData((CSteamID)GameManager.CurrentLobby, "time", pchValue);
			GameManager.LobbyController.StartCoroutine(GameManager.LobbyController.UpdateAllSteamLobbyData());
			if (NetGameServer.NetServer.UPnP != null)
			{
				Debug.Log("UPNP Status: " + NetGameServer.NetServer.UPnP.Status.ToString());
			}
			if (GameManager.LobbyController != null)
			{
				GameManager.LobbyController.UpdateAllLobbyOptions();
			}
			base.StartCoroutine(this.RegisterHost());
		}
		else
		{
			Debug.Log("Failed to create steam lobby");
		}
		this.SetLobbyTypeText();
	}

	// Token: 0x0600234A RID: 9034 RVA: 0x000198B1 File Offset: 0x00017AB1
	private IEnumerator RegisterHost()
	{
		while (NetGameServer.IsRunning)
		{
			NetGameServer.RegisterHostWithMasterServer(GameManager.CurrentLobby, false);
			yield return new WaitForSeconds(0.5f);
		}
		yield break;
	}

	// Token: 0x0600234B RID: 9035 RVA: 0x000D6CC8 File Offset: 0x000D4EC8
	private void OnLobbyJoinRequested(GameLobbyJoinRequested_t t)
	{
		if (SteamManager.Initialized)
		{
			string str = "Steam lobby join request: ";
			CSteamID steamIDLobby = t.m_steamIDLobby;
			Debug.Log(str + steamIDLobby.ToString());
			SteamMatchmaking.JoinLobby(t.m_steamIDLobby);
		}
	}

	// Token: 0x0600234C RID: 9036 RVA: 0x000D6D0C File Offset: 0x000D4F0C
	public void SetLobbyTypeText()
	{
		if (NetGameServer.IsLocal)
		{
			this.lobbyTypeLocalize.SetTerm("LocalLobby");
			return;
		}
		if (!SteamManager.Initialized)
		{
			this.lobbyTypeLocalize.SetTerm("Unknown");
			return;
		}
		switch (this.lobbyType.value)
		{
		case 0:
			this.lobbyTypeLocalize.SetTerm("InviteOnlyLobby");
			return;
		case 1:
			this.lobbyTypeLocalize.SetTerm("FriendsOnlyLobby");
			return;
		case 2:
			this.lobbyTypeLocalize.SetTerm("PublicLobby");
			return;
		default:
			return;
		}
	}

	// Token: 0x0600234D RID: 9037 RVA: 0x000198B9 File Offset: 0x00017AB9
	public void JoinLobby(int lobbyIndex, LobbyEntry entry)
	{
		this.lastAttemptedLobbyEntry = entry;
		if (SteamManager.Initialized)
		{
			SteamMatchmaking.JoinLobby(SteamMatchmaking.GetLobbyByIndex(lobbyIndex));
		}
	}

	// Token: 0x0600234E RID: 9038 RVA: 0x000D6D9C File Offset: 0x000D4F9C
	public void HideAllMainMenuUI()
	{
		this.main_menu.SetState(MainMenuWindowState.Hidden);
		this.multiplayer_window.SetState(MainMenuWindowState.Hidden);
		this.create_game_window.SetState(MainMenuWindowState.Hidden);
		this.join_game_window.SetState(MainMenuWindowState.Hidden);
		this.options_window.SetState(MainMenuWindowState.Hidden);
		this.quit_window.SetState(MainMenuWindowState.Hidden);
		this.multiplayer_window.SetState(MainMenuWindowState.Hidden);
		this.multiplayer_lobby_menu_btm.SetState(MainMenuWindowState.Hidden);
		this.multiplayer_lobby_menu_top.SetState(MainMenuWindowState.Hidden);
		this.connecting_wnd.SetState(MainMenuWindowState.Hidden);
		this.direct_connect_window.SetState(MainMenuWindowState.Hidden);
		this.loadSaveDialogWindow.SetState(MainMenuWindowState.Hidden);
		this.error_wnd.SetState(MainMenuWindowState.Hidden);
		this.error_return_wnd = null;
		this.unlockWindowManager.Hide();
		this.join_game_window.SetState(MainMenuWindowState.Hidden);
		this.direct_connect_window.SetState(MainMenuWindowState.Hidden);
	}

	// Token: 0x0600234F RID: 9039 RVA: 0x000198D5 File Offset: 0x00017AD5
	public void ReturnToMainMenu()
	{
		this.HideAllMainMenuUI();
		this.main_menu.SetState(MainMenuWindowState.Visible);
	}

	// Token: 0x06002350 RID: 9040 RVA: 0x000198E9 File Offset: 0x00017AE9
	public void DisableMainMenuUI()
	{
		this.lobbyStartGameBtn.SetState(BasicButtonBase.BasicButtonState.Disabled);
		this.lobbyLoadSaveBtn.SetState(BasicButtonBase.BasicButtonState.Disabled);
		this.SetLobbyOptionInteractable(false);
	}

	// Token: 0x06002351 RID: 9041 RVA: 0x0001990A File Offset: 0x00017B0A
	public void ShowErrorWindow(string text, DialogType dialogType)
	{
		this.ShowErrorWindow(text, dialogType, this.main_menu);
	}

	// Token: 0x06002352 RID: 9042 RVA: 0x0001991A File Offset: 0x00017B1A
	public void ShowErrorWindow(string text, DialogType dialogType, MainMenuWindow window)
	{
		this.connecting_wnd.SetState(MainMenuWindowState.Hidden);
		this.error_return_wnd = window;
		this.error_wnd.SetDialog(text, this.error_txt_color, dialogType);
		this.error_wnd.SetState(MainMenuWindowState.Visible);
	}

	// Token: 0x06002353 RID: 9043 RVA: 0x0001994E File Offset: 0x00017B4E
	public void ShowLoadingWindow()
	{
		this.loading_wnd.SetState(MainMenuWindowState.Visible);
	}

	// Token: 0x06002354 RID: 9044 RVA: 0x0001995C File Offset: 0x00017B5C
	public void SetLoadingWindowText(string txt)
	{
		this.loading_wnd_text.text = txt;
	}

	// Token: 0x06002355 RID: 9045 RVA: 0x0001996A File Offset: 0x00017B6A
	public void HideLoadingWindow()
	{
		this.loading_wnd.SetState(MainMenuWindowState.Hidden);
	}

	// Token: 0x06002356 RID: 9046 RVA: 0x00019978 File Offset: 0x00017B78
	public void ShowConnectingWindow()
	{
		this.connecting_wnd.SetState(MainMenuWindowState.Visible);
	}

	// Token: 0x06002357 RID: 9047 RVA: 0x00019986 File Offset: 0x00017B86
	public void SetConnectingWindowText(string txt)
	{
		this.connecting_wnd_text.text = txt;
	}

	// Token: 0x06002358 RID: 9048 RVA: 0x00019994 File Offset: 0x00017B94
	public void HideConnectingWindow()
	{
		this.connecting_wnd.SetState(MainMenuWindowState.Hidden);
	}

	// Token: 0x06002359 RID: 9049 RVA: 0x000D6E70 File Offset: 0x000D5070
	private void OnJoinUI(bool net_test, bool natIntro)
	{
		if (net_test)
		{
			this.main_menu.SetState(MainMenuWindowState.Hidden);
		}
		else
		{
			this.HideAllMainMenuUI();
		}
		this.DisableMainMenuUI();
		this.connecting_wnd_text.text = LocalizationManager.GetTranslation(natIntro ? "MultiplayerIntroduction" : "MultiplayerDirectConnect", true, 0, true, false, null, null, true);
		this.connecting_wnd.SetState(MainMenuWindowState.Visible);
	}

	// Token: 0x0600235A RID: 9050 RVA: 0x000199A2 File Offset: 0x00017BA2
	private void OnNatIntroductionSuccess(IPEndPoint endPoint)
	{
		if (!this.waitConnectActive)
		{
			Debug.Log("UIController.OnNatIntroductionSuccess called waiting then connecting");
			this.waitConnectActive = true;
			base.StartCoroutine(this.WaitAndThenConnect(endPoint));
			return;
		}
		Debug.LogError("WaitConnect is already active for nat intro");
	}

	// Token: 0x0600235B RID: 9051 RVA: 0x000199D6 File Offset: 0x00017BD6
	private void RefreshList()
	{
		if (SteamManager.Initialized)
		{
			SteamMatchmaking.AddRequestLobbyListStringFilter("version", "", ELobbyComparison.k_ELobbyComparisonNotEqual);
			SteamMatchmaking.RequestLobbyList();
		}
	}

	// Token: 0x0600235C RID: 9052 RVA: 0x000199F5 File Offset: 0x00017BF5
	public void OnDestroy()
	{
		NetSystem.ConnectedToLobby -= this.connectLobbyEventHandler;
		NetSystem.Disconnected -= this.disconnectLobbyEventHandler;
		NetSystem.ConnectFailed -= this.connectFailedEventhandler;
		NetGameClient.NatIntroductionSuccess -= this.OnNatIntroductionSuccess;
	}

	// Token: 0x0600235D RID: 9053 RVA: 0x00019A29 File Offset: 0x00017C29
	public IEnumerator WaitAndThenConnect(IPEndPoint endPoint)
	{
		yield return new WaitForSeconds(0.5f);
		Debug.Log("Nat intro success from " + endPoint.ToString() + "! ... attempting connection");
		NetGameClient.Connect(endPoint);
		Debug.Log("Done");
		this.waitConnectActive = false;
		yield break;
	}

	// Token: 0x0600235E RID: 9054 RVA: 0x00019A3F File Offset: 0x00017C3F
	private void DoNetTestLogic()
	{
		GameManager.RequestLobbySlot(0, 0);
		GameManager.LobbyController.AddAI();
		GameManager.LobbyController.AddAI();
		GameManager.LobbyController.AddAI();
		GameManager.LobbyController.BeginCountdown();
	}

	// Token: 0x0600235F RID: 9055 RVA: 0x00019A70 File Offset: 0x00017C70
	public UIController()
	{
		KeyCode[] array = new KeyCode[4];
		RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.A18AED3988AD43E1B62B65E026446F5DBCF3790748C1F1821EC4EAE576C35FA7).FieldHandle);
		this.ps4KeyCodes = array;
		this.m_maxProfileLoadWaitTime = 10f;
		base..ctor();
	}

	// Token: 0x04002606 RID: 9734
	public InputField connect_name_field;

	// Token: 0x04002607 RID: 9735
	public InputField connect_ip_field;

	// Token: 0x04002608 RID: 9736
	public InputField connect_port_field;

	// Token: 0x04002609 RID: 9737
	public InputField host_port_field;

	// Token: 0x0400260A RID: 9738
	public Toggle upnpToggle;

	// Token: 0x0400260B RID: 9739
	public Localize lobbyTypeLocalize;

	// Token: 0x0400260C RID: 9740
	public InputField host_name_field;

	// Token: 0x0400260D RID: 9741
	public Localize window_text;

	// Token: 0x0400260E RID: 9742
	public Localize hover_description;

	// Token: 0x0400260F RID: 9743
	public MainMenuWindow main_menu;

	// Token: 0x04002610 RID: 9744
	public bool on_splash = true;

	// Token: 0x04002611 RID: 9745
	public RectTransform splash_window;

	// Token: 0x04002612 RID: 9746
	public Animator menu_border_anim;

	// Token: 0x04002613 RID: 9747
	public UIMultiplayerLobbySlot[] lobby_slots;

	// Token: 0x04002614 RID: 9748
	public UIConnectionSlot[] connection_slots;

	// Token: 0x04002615 RID: 9749
	public Color error_txt_color;

	// Token: 0x04002616 RID: 9750
	public static GlyphDatabase glyphDatabase;

	// Token: 0x04002617 RID: 9751
	public OptionsWindow optionsWindow;

	// Token: 0x04002618 RID: 9752
	public Toggle lobbyFriendsToggle;

	// Token: 0x04002619 RID: 9753
	public Dropdown lobbyType;

	// Token: 0x0400261A RID: 9754
	public LocalizeDropdown lobbyDropDownLocalize;

	// Token: 0x0400261B RID: 9755
	[Header("LobbyOptions")]
	public GameObject mapTab;

	// Token: 0x0400261C RID: 9756
	public GameObject turnCountTab;

	// Token: 0x0400261D RID: 9757
	public GameObject winningRelicsTab;

	// Token: 0x0400261E RID: 9758
	public GameObject turnLengthsTab;

	// Token: 0x0400261F RID: 9759
	public GameObject minigameCountTab;

	// Token: 0x04002620 RID: 9760
	public MainMenuWindow loadSaveDialogWindow;

	// Token: 0x04002621 RID: 9761
	public LoadSaveDialog loadSaveDialog;

	// Token: 0x04002622 RID: 9762
	public FriendsListDialogManager friendsListDialogManager;

	// Token: 0x04002623 RID: 9763
	public UnlockWindowManager unlockWindowManager;

	// Token: 0x04002624 RID: 9764
	public MainMenuWindow minigameItemSettingsWindow;

	// Token: 0x04002625 RID: 9765
	public MainMenuWindow gameRulesWindow;

	// Token: 0x04002626 RID: 9766
	public Text UPNPTextFailed;

	// Token: 0x04002627 RID: 9767
	public Texture2D minigamesOnlyPreviewImage;

	// Token: 0x04002628 RID: 9768
	private MainMenuWindow multiplayer_window;

	// Token: 0x04002629 RID: 9769
	private MainMenuWindow create_game_window;

	// Token: 0x0400262A RID: 9770
	private MainMenuWindow join_game_window;

	// Token: 0x0400262B RID: 9771
	private MainMenuWindow options_window;

	// Token: 0x0400262C RID: 9772
	private MainMenuWindow quit_window;

	// Token: 0x0400262D RID: 9773
	private MainMenuWindow multiplayer_lobby_window;

	// Token: 0x0400262E RID: 9774
	private MainMenuWindow multiplayer_lobby_menu_btm;

	// Token: 0x0400262F RID: 9775
	private MainMenuWindow multiplayer_lobby_menu_top;

	// Token: 0x04002630 RID: 9776
	private MainMenuWindow connecting_wnd;

	// Token: 0x04002631 RID: 9777
	private Text connecting_wnd_text;

	// Token: 0x04002632 RID: 9778
	private MainMenuWindow loading_wnd;

	// Token: 0x04002633 RID: 9779
	private Text loading_wnd_text;

	// Token: 0x04002634 RID: 9780
	private MainMenuWindow direct_connect_window;

	// Token: 0x04002635 RID: 9781
	private GameObject backgroundPanel;

	// Token: 0x04002636 RID: 9782
	[HideInInspector]
	public DialogWindow error_wnd;

	// Token: 0x04002637 RID: 9783
	public OnScreenKeyboardUI onScreenKeyboardUI;

	// Token: 0x04002638 RID: 9784
	private BasicButtonBase lobbyStartGameBtn;

	// Token: 0x04002639 RID: 9785
	private BasicButtonBase lobbyAddAIBtn;

	// Token: 0x0400263A RID: 9786
	public BasicButtonBase lobbyInviteBtn;

	// Token: 0x0400263B RID: 9787
	public BasicButtonBase minigameSettingsBtn;

	// Token: 0x0400263C RID: 9788
	private BasicButtonBase lobbyLoadSaveBtn;

	// Token: 0x0400263D RID: 9789
	private BasicButtonBase lobbyCancelLoadBtn;

	// Token: 0x0400263E RID: 9790
	public Localize mapNameLocalize;

	// Token: 0x0400263F RID: 9791
	public Localize mapDescriptionLocalize;

	// Token: 0x04002640 RID: 9792
	private RawImage mapPreviewImage;

	// Token: 0x04002641 RID: 9793
	private Text mapDescription;

	// Token: 0x04002642 RID: 9794
	private LobbyOptionTab[] lobbyOptionTabs;

	// Token: 0x04002643 RID: 9795
	private MainMenuWindow error_return_wnd;

	// Token: 0x04002645 RID: 9797
	private bool m_signedOut = true;

	// Token: 0x04002647 RID: 9799
	private ConnectFailedEventHandler connectFailedEventhandler;

	// Token: 0x04002648 RID: 9800
	private ConnectLobbyEventHandler connectLobbyEventHandler;

	// Token: 0x04002649 RID: 9801
	private DisconnectEventHandler disconnectLobbyEventHandler;

	// Token: 0x0400264A RID: 9802
	protected Callback<LobbyCreated_t> Callback_lobbyCreated;

	// Token: 0x0400264B RID: 9803
	protected Callback<GameLobbyJoinRequested_t> Callback_lobbyJoinRequested;

	// Token: 0x0400264C RID: 9804
	protected Callback<LobbyEnter_t> Callback_lobbyEnter;

	// Token: 0x0400264D RID: 9805
	private bool m_relayConnectFailed;

	// Token: 0x0400264E RID: 9806
	private bool m_relayConnectAttemptComplete = true;

	// Token: 0x0400264F RID: 9807
	private Coroutine m_relayConnectUICoroutine;

	// Token: 0x04002650 RID: 9808
	private Coroutine m_relayConnectCoroutine;

	// Token: 0x04002651 RID: 9809
	private bool m_showSplashScreen;

	// Token: 0x04002652 RID: 9810
	[SerializeField]
	private RulesetUIWindow m_rulesetUIWindow;

	// Token: 0x04002653 RID: 9811
	private bool m_lobbyTypesSet;

	// Token: 0x04002654 RID: 9812
	public bool UPNPFailed;

	// Token: 0x04002655 RID: 9813
	private KeyCode[] ps4KeyCodes;

	// Token: 0x04002656 RID: 9814
	private float m_maxProfileLoadWaitTime;

	// Token: 0x04002657 RID: 9815
	private bool signOutStarted;

	// Token: 0x04002658 RID: 9816
	private bool signOutCompleted;

	// Token: 0x04002659 RID: 9817
	private bool local;

	// Token: 0x0400265A RID: 9818
	private bool net_test;

	// Token: 0x0400265B RID: 9819
	private LobbyEntry lastAttemptedLobbyEntry;

	// Token: 0x0400265C RID: 9820
	private bool waitConnectActive;
}
