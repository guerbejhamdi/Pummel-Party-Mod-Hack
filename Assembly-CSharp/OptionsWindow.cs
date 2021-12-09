using System;
using System.Collections;
using I2.Loc;
using Rewired;
using Rewired.UI.ControlMapper;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x020002BB RID: 699
public class OptionsWindow : MonoBehaviour
{
	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06001427 RID: 5159 RVA: 0x0000FD94 File Offset: 0x0000DF94
	// (set) Token: 0x06001428 RID: 5160 RVA: 0x00097CD0 File Offset: 0x00095ED0
	public bool MainMenu
	{
		get
		{
			return this.mainMenu;
		}
		set
		{
			this.mainMenu = value;
			this.leaveQuitBar.SetActive(!this.mainMenu);
			if (this.quitWindow == null && !this.mainMenu)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.quitWindowPrefab, base.transform.parent);
				this.quitWindow = gameObject.GetComponent<MainMenuWindow>();
				this.quitWindowLocalize = this.quitWindow.transform.Find("Window/Lbl").GetComponent<Localize>();
			}
		}
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x0000FD9C File Offset: 0x0000DF9C
	public void OnStorageLoaded()
	{
		this.UpdateSettingsFields();
	}

	// Token: 0x0600142A RID: 5162 RVA: 0x00097D54 File Offset: 0x00095F54
	private void Start()
	{
		if (this.mainMenu)
		{
			this.uiCanvas = GameObject.Find("Canvas");
			if (this.uiCanvas != null)
			{
				this.uiController = this.uiCanvas.GetComponent<UIController>();
			}
		}
		this.buttons = base.gameObject.GetComponentsInChildren<SettingField>(true);
		base.StartCoroutine(this.ResetControls());
	}

	// Token: 0x0600142B RID: 5163 RVA: 0x0000FDA4 File Offset: 0x0000DFA4
	private IEnumerator ResetControls()
	{
		yield return new WaitForSeconds(3f);
		if (RBPrefs.GetInt("ResetControls", 0) == 0)
		{
			Debug.Log("Reseting Controls");
			RBPrefs.SetInt("ResetControls", 1);
			this.controlMapper.ForceRestoreDefaults();
			this.controlMapper.Toggle();
			yield return new WaitForSeconds(0.2f);
			this.controlMapper.Toggle();
		}
		yield break;
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x00097DB8 File Offset: 0x00095FB8
	public void DoButtonEvent(MainMenuButtonEventType btn_event)
	{
		if (btn_event <= MainMenuButtonEventType.OptionsGeneral)
		{
			if (btn_event != MainMenuButtonEventType.ExitGame)
			{
				switch (btn_event)
				{
				case MainMenuButtonEventType.GoOptionsWindow:
					if (this.mainMenu)
					{
						this.uiController.main_menu.SetState(MainMenuWindowState.Hidden);
						this.uiController.SetWindowName("Options");
					}
					this.optionsMainMenuWindow.SetState(MainMenuWindowState.Visible);
					this.ShowOptionsPane(0);
					this.curPane = 0;
					return;
				case MainMenuButtonEventType.OptionsWindowBack:
					Settings.ApplySettings();
					Settings.SaveSettings();
					this.UpdateSettingsFields();
					this.optionsMainMenuWindow.SetState(MainMenuWindowState.Hidden);
					if (this.mainMenu)
					{
						this.uiController.main_menu.SetState(MainMenuWindowState.Visible);
						this.uiController.SetWindowName("Pummel Party");
						return;
					}
					GameManager.UnpauseGame(true);
					return;
				case MainMenuButtonEventType.ConnectToGame:
				case MainMenuButtonEventType.JoinGameWindowBack:
				case MainMenuButtonEventType.GoQuitGameWindow:
				case MainMenuButtonEventType.LeaveMultiplayerLobby:
				case MainMenuButtonEventType.StartGame:
				case MainMenuButtonEventType.ErrorOK:
				case MainMenuButtonEventType.CancelStartGameCountdown:
				case MainMenuButtonEventType.None:
				case MainMenuButtonEventType.AddAI:
				case MainMenuButtonEventType.InviteFriends:
				case MainMenuButtonEventType.ConnecToGameNatPunch:
				case MainMenuButtonEventType.RefreshLobbyList:
					break;
				case MainMenuButtonEventType.CancelQuit:
					this.optionsMainMenuWindow.SetState(MainMenuWindowState.Visible);
					this.quitWindow.SetState(MainMenuWindowState.Hidden);
					return;
				case MainMenuButtonEventType.ApplyOptions:
					if (!this.optionsControlsPane.Hidden)
					{
						ReInput.userDataStore.Save();
					}
					else if (this.curPane != 4)
					{
						SettingField[] array = null;
						switch (this.curPane)
						{
						case 0:
							array = this.optionsGraphicsPane.gameObject.GetComponentsInChildren<SettingField>();
							break;
						case 2:
							array = this.optionsSoundPane.gameObject.GetComponentsInChildren<SettingField>();
							break;
						case 3:
							array = this.optionsGeneralPane.gameObject.GetComponentsInChildren<SettingField>();
							break;
						}
						SettingField[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							array2[i].OnApply();
						}
						Settings.ApplySettings();
						Settings.SaveSettings();
					}
					this.settingsSavedAnimation.Play("SettingsUI");
					return;
				case MainMenuButtonEventType.OptionsControls:
					this.ShowOptionsPane(1);
					this.curPane = 1;
					return;
				case MainMenuButtonEventType.OptionsGraphics:
					this.ShowOptionsPane(0);
					this.curPane = 0;
					return;
				case MainMenuButtonEventType.OptionsSound:
					this.ShowOptionsPane(2);
					this.curPane = 2;
					return;
				case MainMenuButtonEventType.OptionsTabLeft:
				{
					int min = 0;
					this.curPane = Mathf.Clamp(this.curPane - 1, min, 4);
					this.ShowOptionsPane(this.curPane);
					return;
				}
				case MainMenuButtonEventType.OptionsTabRight:
				{
					int min2 = 0;
					this.curPane = Mathf.Clamp(this.curPane + 1, min2, 4);
					this.ShowOptionsPane(this.curPane);
					return;
				}
				case MainMenuButtonEventType.OptionsRestoreDefaults:
					switch (this.curPane)
					{
					case 0:
						Settings.RestoreGraphicsDefaults();
						this.UpdateSettingsFields();
						return;
					case 1:
						this.controlMapper.OnRestoreDefaults();
						return;
					case 2:
						Settings.RestoreSoundDefaults();
						this.UpdateSettingsFields();
						return;
					case 3:
						Settings.RestoreGeneralDefaults();
						this.UpdateSettingsFields();
						return;
					default:
						return;
					}
					break;
				case MainMenuButtonEventType.OptionsLeaveGame:
					this.quitWindowLocalize.SetTerm("QuestionLeave");
					this.optionsMainMenuWindow.SetState(MainMenuWindowState.Disabled);
					this.quitWindow.SetState(MainMenuWindowState.Visible);
					this.leaving = true;
					return;
				case MainMenuButtonEventType.OptionsQuitGame:
					this.quitWindowLocalize.SetTerm("QuestionQuit");
					this.optionsMainMenuWindow.SetState(MainMenuWindowState.Disabled);
					this.quitWindow.SetState(MainMenuWindowState.Visible);
					this.leaving = false;
					return;
				case MainMenuButtonEventType.OptionsGeneral:
					this.ShowOptionsPane(3);
					this.curPane = 3;
					return;
				default:
					return;
				}
			}
			else
			{
				if (this.leaving)
				{
					GameManager.disconnectUserEvent = DisconnectUserEventType.LeftGame;
					NetSystem.OnDisconnect(LocalizationManager.GetTranslation("DisconnectReasonLeft", true, 0, true, false, null, null, true));
					return;
				}
				Application.Quit();
				return;
			}
		}
		else
		{
			if (btn_event == MainMenuButtonEventType.MinigamesOnlySkip)
			{
				GameManager.UIController.minigameOnlySceneController.Skip();
				return;
			}
			switch (btn_event)
			{
			case MainMenuButtonEventType.OptionsCredits:
				this.ShowOptionsPane(4);
				this.curPane = 4;
				return;
			case MainMenuButtonEventType.CreditsSection:
				this.thanksPane.SetState(MainMenuWindowState.Hidden);
				this.creditsPane.SetState(MainMenuWindowState.Visible);
				break;
			case MainMenuButtonEventType.ThanksSection:
				this.creditsPane.SetState(MainMenuWindowState.Hidden);
				this.thanksPane.SetState(MainMenuWindowState.Visible);
				return;
			default:
				return;
			}
		}
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x00098168 File Offset: 0x00096368
	private void ShowOptionsPane(int id)
	{
		EventSystem.GetSystem(0).SetSelectedGameObject(null);
		if (id != 0)
		{
			this.optionsGraphicsPane.SetState(MainMenuWindowState.Hidden);
		}
		if (id != 1)
		{
			this.optionsControlsPane.SetState(MainMenuWindowState.Hidden);
		}
		if (id != 2)
		{
			this.optionsSoundPane.SetState(MainMenuWindowState.Hidden);
		}
		if (id != 3)
		{
			this.optionsGeneralPane.SetState(MainMenuWindowState.Hidden);
		}
		if (id != 4)
		{
			this.optionsCreditsPane.SetState(MainMenuWindowState.Hidden);
		}
		switch (id)
		{
		case 0:
			this.optionsGraphicsPane.SetState(MainMenuWindowState.Visible);
			break;
		case 1:
			this.optionsControlsPane.SetState(MainMenuWindowState.Visible);
			break;
		case 2:
			this.optionsSoundPane.SetState(MainMenuWindowState.Visible);
			break;
		case 3:
			this.optionsGeneralPane.SetState(MainMenuWindowState.Visible);
			break;
		case 4:
			this.optionsCreditsPane.SetState(MainMenuWindowState.Visible);
			this.thanksPane.SetState(MainMenuWindowState.Visible);
			this.creditsPane.SetState(MainMenuWindowState.Hidden);
			break;
		}
		bool flag = false;
		Controller lastActiveController = ReInput.players.SystemPlayer.controllers.GetLastActiveController();
		if (lastActiveController != null)
		{
			flag = (lastActiveController.type == ControllerType.Joystick);
		}
		this.optionsGraphicsButton.interactable = (id != 0 || flag);
		this.optionsControlsButton.interactable = (id != 1 || flag);
		this.optionsSoundButton.interactable = (id != 2 || flag);
		this.optionsGeneralButton.interactable = (id != 3 || flag);
		this.optionsCreditsButton.interactable = (id != 4 || flag);
		this.UpdateSettingsFields();
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x000982D4 File Offset: 0x000964D4
	private void UpdateSettingsFields()
	{
		for (int i = 0; i < this.buttons.Length; i++)
		{
			this.buttons[i].Setup();
		}
	}

	// Token: 0x04001561 RID: 5473
	public MainMenuWindow optionsMainMenuWindow;

	// Token: 0x04001562 RID: 5474
	public Animation settingsSavedAnimation;

	// Token: 0x04001563 RID: 5475
	public ControlMapper controlMapper;

	// Token: 0x04001564 RID: 5476
	public GameObject leaveQuitBar;

	// Token: 0x04001565 RID: 5477
	public GameObject quitWindowPrefab;

	// Token: 0x04001566 RID: 5478
	public MainMenuWindow optionsGraphicsPane;

	// Token: 0x04001567 RID: 5479
	public MainMenuWindow optionsControlsPane;

	// Token: 0x04001568 RID: 5480
	public MainMenuWindow optionsSoundPane;

	// Token: 0x04001569 RID: 5481
	public MainMenuWindow optionsGeneralPane;

	// Token: 0x0400156A RID: 5482
	public MainMenuWindow optionsCreditsPane;

	// Token: 0x0400156B RID: 5483
	public Button optionsGraphicsButton;

	// Token: 0x0400156C RID: 5484
	public Button optionsControlsButton;

	// Token: 0x0400156D RID: 5485
	public Button optionsSoundButton;

	// Token: 0x0400156E RID: 5486
	public Button optionsGeneralButton;

	// Token: 0x0400156F RID: 5487
	public Button optionsCreditsButton;

	// Token: 0x04001570 RID: 5488
	public BasicButton backButton;

	// Token: 0x04001571 RID: 5489
	public bool mainMenu = true;

	// Token: 0x04001572 RID: 5490
	public MainMenuWindow thanksPane;

	// Token: 0x04001573 RID: 5491
	public MainMenuWindow creditsPane;

	// Token: 0x04001574 RID: 5492
	private GameObject uiCanvas;

	// Token: 0x04001575 RID: 5493
	private UIController uiController;

	// Token: 0x04001576 RID: 5494
	private MainMenuWindow quitWindow;

	// Token: 0x04001577 RID: 5495
	private Localize quitWindowLocalize;

	// Token: 0x04001578 RID: 5496
	private bool leaving;

	// Token: 0x04001579 RID: 5497
	private int curPane;

	// Token: 0x0400157A RID: 5498
	private SettingField[] buttons;
}
