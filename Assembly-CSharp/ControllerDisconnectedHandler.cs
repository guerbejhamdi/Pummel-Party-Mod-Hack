using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020002EF RID: 751
public class ControllerDisconnectedHandler : MonoBehaviour
{
	// Token: 0x060014F5 RID: 5365 RVA: 0x000990B0 File Offset: 0x000972B0
	public void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		PlatformUserManager instance = PlatformUserManager.Instance;
		instance.OnMainUserSignOutStarted = (UserManagerEvent)Delegate.Combine(instance.OnMainUserSignOutStarted, new UserManagerEvent(this.OnMainUserSignOutStarted));
		ReInput.ControllerPreDisconnectEvent += this.OnRewiredControllerPreDisconnect;
		ReInput.ControllerConnectedEvent += this.OnRewiredControllerConnected;
	}

	// Token: 0x060014F6 RID: 5366 RVA: 0x00099110 File Offset: 0x00097310
	public void OnDestroy()
	{
		PlatformUserManager instance = PlatformUserManager.Instance;
		instance.OnMainUserSignOutStarted = (UserManagerEvent)Delegate.Remove(instance.OnMainUserSignOutStarted, new UserManagerEvent(this.OnMainUserSignOutStarted));
		ReInput.ControllerPreDisconnectEvent -= this.OnRewiredControllerPreDisconnect;
		ReInput.ControllerConnectedEvent -= this.OnRewiredControllerConnected;
	}

	// Token: 0x060014F7 RID: 5367 RVA: 0x00099168 File Offset: 0x00097368
	private void OnRewiredControllerPreDisconnect(ControllerStatusChangedEventArgs args)
	{
		if (args.controllerType != ControllerType.Joystick)
		{
			return;
		}
		Joystick joystick = (Joystick)ReInput.controllers.GetController(args.controllerType, args.controllerId);
		if (joystick.systemId != null)
		{
			long value = joystick.systemId.Value;
			foreach (Player player in ReInput.players.AllPlayers)
			{
				if (player != ReInput.players.SystemPlayer && ReInput.controllers.IsControllerAssignedToPlayer(args.controllerType, args.controllerId, player.id))
				{
					this.m_previousControllers[value] = player;
					Debug.Log(string.Concat(new string[]
					{
						"Saving controller that was assigned to player ",
						player.id.ToString(),
						", systemID=",
						value.ToString(),
						", cid=",
						args.controllerId.ToString()
					}));
				}
			}
		}
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x00099298 File Offset: 0x00097498
	private void OnRewiredControllerConnected(ControllerStatusChangedEventArgs args)
	{
		if (args.controllerType != ControllerType.Joystick)
		{
			return;
		}
		Joystick joystick = (Joystick)ReInput.controllers.GetController(args.controllerType, args.controllerId);
		if (ReInput.controllers.IsControllerAssigned(ControllerType.Joystick, joystick))
		{
			return;
		}
		if (joystick.systemId != null)
		{
			long value = joystick.systemId.Value;
			Player player = null;
			if (this.m_previousControllers.TryGetValue(value, out player))
			{
				if (player != null && player.controllers.joystickCount <= 0)
				{
					Debug.Log(string.Concat(new string[]
					{
						"Reconnecting controller that was assigned to player ",
						player.id.ToString(),
						", systemID=",
						value.ToString(),
						", cid=",
						args.controllerId.ToString()
					}));
					player.controllers.AddController(joystick, true);
					if (this.m_curDisconnectedPlayer == player)
					{
						this.SetCurrentDisconnectedPlayer(null);
					}
				}
				this.m_previousControllers.Remove(value);
			}
		}
	}

	// Token: 0x060014F9 RID: 5369 RVA: 0x0001008B File Offset: 0x0000E28B
	private void OnMainUserSignOutStarted()
	{
		if (this.m_curDisconnectedPlayer != null && this.m_curState != ControllerDisconnectedHandlerState.None)
		{
			this.SetCurrentDisconnectedPlayer(null);
		}
	}

	// Token: 0x060014FA RID: 5370 RVA: 0x000993A8 File Offset: 0x000975A8
	public void Update()
	{
		if (SceneManager.GetActiveScene().name == "Main")
		{
			return;
		}
		if (this.m_curDisconnectedPlayer == null && (GameManager.MainMenuUIController == null || !GameManager.MainMenuUIController.SignedOut))
		{
			using (IEnumerator<Player> enumerator = ReInput.players.AllPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Player player = enumerator.Current;
					if (player != ReInput.players.SystemPlayer && this.IsPlayerActive(player) && player.controllers.joystickCount <= 0)
					{
						if (player.id == 0)
						{
							this.SetCurrentDisconnectedPlayer(player);
							break;
						}
						if (this.IsInGame())
						{
							this.SetCurrentDisconnectedPlayer(player);
						}
						else if (GameManager.LobbyController != null)
						{
							GameManager.LobbyController.RemoveLocalPlayerFromSlot(player);
						}
					}
				}
				return;
			}
		}
		this.DisplayReconnectWindow();
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x00099498 File Offset: 0x00097698
	private bool IsPlayerActive(Player player)
	{
		if (player == ReInput.players.SystemPlayer)
		{
			return false;
		}
		if (GameManager.MainMenuUIController != null && GameManager.MainMenuUIController.SignedOut)
		{
			return false;
		}
		if (player.id == 0)
		{
			return true;
		}
		if (this.IsInGame())
		{
			return this.GetGamePlayerFromRewiredPlayer(player) != null;
		}
		return GameManager.LobbyController != null && GameManager.LobbyController.IsLocalPlayerInSlot(player.id);
	}

	// Token: 0x060014FC RID: 5372 RVA: 0x00099510 File Offset: 0x00097710
	private GamePlayer GetGamePlayerFromRewiredPlayer(Player player)
	{
		List<GamePlayer> playerList = GameManager.PlayerList;
		if (playerList != null)
		{
			foreach (GamePlayer gamePlayer in playerList)
			{
				if (gamePlayer.IsLocalPlayer && !gamePlayer.IsAI && gamePlayer.RewiredPlayer == player)
				{
					return gamePlayer;
				}
			}
		}
		return null;
	}

	// Token: 0x060014FD RID: 5373 RVA: 0x00099580 File Offset: 0x00097780
	private bool IsJoystickUsed(int joystickID)
	{
		if (!ReInput.controllers.IsJoystickAssigned(joystickID))
		{
			return false;
		}
		foreach (Player player in ReInput.players.AllPlayers)
		{
			if (ReInput.controllers.IsJoystickAssignedToPlayer(joystickID, player.id))
			{
				if (player.id == 0)
				{
					return true;
				}
				if (this.IsInGame())
				{
					return this.GetGamePlayerFromRewiredPlayer(player) != null;
				}
			}
		}
		return false;
	}

	// Token: 0x060014FE RID: 5374 RVA: 0x00099610 File Offset: 0x00097810
	private void RemovePreviousPlayerWithJoystick(int joystickID)
	{
		if (!ReInput.controllers.IsJoystickAssigned(joystickID))
		{
			return;
		}
		foreach (Player player in ReInput.players.AllPlayers)
		{
			if (ReInput.controllers.IsJoystickAssignedToPlayer(joystickID, player.id))
			{
				if (player.id == 0)
				{
					break;
				}
				if (!this.IsInGame() && GameManager.LobbyController != null && GameManager.LobbyController.IsLocalPlayerInSlot(player.id))
				{
					GameManager.LobbyController.RemoveLocalPlayerFromSlot(player);
					break;
				}
				break;
			}
		}
	}

	// Token: 0x060014FF RID: 5375 RVA: 0x000996BC File Offset: 0x000978BC
	public void DisplayReconnectWindow()
	{
		if (this.m_curDisconnectedPlayer == null)
		{
			return;
		}
		if (this.m_curDisconnectedPlayer.controllers.joystickCount > 0 || (GameManager.MainMenuUIController != null && GameManager.MainMenuUIController.SignedOut))
		{
			this.SetCurrentDisconnectedPlayer(null);
			return;
		}
		foreach (Joystick joystick in ReInput.controllers.Joysticks)
		{
			if (!ReInput.controllers.IsControllerAssigned(ControllerType.Joystick, joystick) && joystick.systemId != null)
			{
				long value = joystick.systemId.Value;
				Player player = null;
				if (this.m_previousControllers.TryGetValue(value, out player) && player != null && player.controllers.joystickCount <= 0 && player == this.m_curDisconnectedPlayer)
				{
					this.SetDisconnectedPlayersJoystick(joystick.id);
					this.SetCurrentDisconnectedPlayer(null);
					this.m_previousControllers[value] = null;
					return;
				}
			}
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		ulong num = 0UL;
		int disconnectedPlayersJoystick = 0;
		if (!flag4)
		{
			if (this.m_curState == ControllerDisconnectedHandlerState.WaitingForContinue)
			{
				if (flag)
				{
					if (this.m_curDisconnectedPlayer.id != 0)
					{
						this.SetDisconnectedPlayersJoystick(disconnectedPlayersJoystick);
						this.SetCurrentDisconnectedPlayer(null);
						return;
					}
					if (PlatformUserManager.Instance.MainUser.ControllerID == num)
					{
						Debug.Log("Setting new joystick, same controller connected");
						this.SetDisconnectedPlayersJoystick(disconnectedPlayersJoystick);
						this.SetCurrentDisconnectedPlayer(null);
						return;
					}
				}
			}
			else if (this.m_curState == ControllerDisconnectedHandlerState.IncorrectProfileResponse)
			{
				if (flag3)
				{
					PlatformUserManager.Instance.ShowAccountPicker(num, false, false, new AccountPickerCompleteEvent(this.OnAccountPickerComplete));
					return;
				}
				if (flag2)
				{
					if (this.IsInGame())
					{
						if (GameManager.gameNetworkManager != null)
						{
							GameManager.gameNetworkManager.OnMainUserSignedOut();
							return;
						}
					}
					else if (GameManager.MainMenuUIController != null)
					{
						this.SetCurrentDisconnectedPlayer(null);
						GameManager.MainMenuUIController.ControllerDisconnectedUserChanged();
						return;
					}
				}
			}
		}
		else if (flag)
		{
			this.m_alreadyAssigned.SetActive(true);
			this.m_anim.SetTrigger("pulse");
			AudioSystem.PlayOneShot(this.m_error, 1f, 0.5f, 1f);
		}
	}

	// Token: 0x06001500 RID: 5376 RVA: 0x000100A4 File Offset: 0x0000E2A4
	private void OnAccountPickerComplete(bool cancelled, IPlatformUser user)
	{
	}

	// Token: 0x06001501 RID: 5377 RVA: 0x000998F0 File Offset: 0x00097AF0
	private void SetDisconnectedPlayersJoystick(int joystickID)
	{
		this.RemovePreviousPlayerWithJoystick(joystickID);
		this.m_curDisconnectedPlayer.controllers.ClearAllControllers();
		Joystick joystick = ReInput.controllers.GetJoystick(joystickID);
		this.m_curDisconnectedPlayer.controllers.AddController(joystick, true);
		if (joystick == null)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Adding joystick for player ",
				this.m_curDisconnectedPlayer.id.ToString(),
				", joystickID = ",
				joystickID.ToString(),
				", joystick=null"
			}));
			return;
		}
		Debug.Log(string.Concat(new string[]
		{
			"Adding joystick for player ",
			this.m_curDisconnectedPlayer.id.ToString(),
			", joystickID = ",
			joystickID.ToString(),
			", joystick=",
			joystick.id.ToString()
		}));
	}

	// Token: 0x06001502 RID: 5378 RVA: 0x000100A8 File Offset: 0x0000E2A8
	private void SwapMainUser(IPlatformUser user)
	{
		PlatformUserManager.Instance.SetMainUser(user);
		this.SetDisconnectedPlayersJoystick(user.RewiredJoystickID);
	}

	// Token: 0x06001503 RID: 5379 RVA: 0x000999D8 File Offset: 0x00097BD8
	private void SetState(ControllerDisconnectedHandlerState newState)
	{
		switch (newState)
		{
		case ControllerDisconnectedHandlerState.None:
			GameManager.UnpauseGame(false);
			this.m_alreadyAssigned.SetActive(false);
			this.m_anim.SetBool("visible", false);
			break;
		case ControllerDisconnectedHandlerState.WaitingForContinue:
			GameManager.PauseGame(false);
			this.m_window.SetActive(true);
			this.m_titleText.text = "Controller Disconnected";
			this.m_descriptionText.text = "Please reconnect your controller to continue";
			this.m_continueButton.SetActive(true);
			this.m_leaveButton.SetActive(false);
			this.m_switchButton.SetActive(false);
			this.m_alreadyAssigned.SetActive(false);
			this.m_anim.SetBool("visible", true);
			break;
		case ControllerDisconnectedHandlerState.IncorrectProfileResponse:
			this.m_titleText.text = "Controller Disconnected - Incorrect Profile";
			this.m_descriptionText.text = "The active profile has changed, switch profiles or you will be returned to the main menu and your current game will be lost.";
			this.m_continueButton.SetActive(false);
			this.m_leaveButton.SetActive(true);
			this.m_switchButton.SetActive(true);
			this.m_alreadyAssigned.SetActive(false);
			this.m_anim.SetTrigger("pulse");
			break;
		}
		this.m_curState = newState;
	}

	// Token: 0x06001504 RID: 5380 RVA: 0x00099B00 File Offset: 0x00097D00
	private void SetCurrentDisconnectedPlayer(Player player)
	{
		if (player == null)
		{
			Debug.Log("Closing disconnect UI");
			this.SetState(ControllerDisconnectedHandlerState.None);
		}
		else
		{
			Debug.Log("Showing disconnect UI for " + player.id.ToString());
			this.SetState(ControllerDisconnectedHandlerState.WaitingForContinue);
			string text = "Player";
			Color white = Color.white;
			this.GetPlayerNameAndColor(player, ref text, ref white);
			this.m_userNameText.text = text;
			this.m_playerIcon.color = white;
		}
		this.m_curDisconnectedPlayer = player;
	}

	// Token: 0x06001505 RID: 5381 RVA: 0x00099B80 File Offset: 0x00097D80
	private void GetPlayerNameAndColor(Player player, ref string name, ref Color col)
	{
		if (this.IsInGame())
		{
			GamePlayer gamePlayerFromRewiredPlayer = this.GetGamePlayerFromRewiredPlayer(player);
			if (gamePlayerFromRewiredPlayer != null)
			{
				name = gamePlayerFromRewiredPlayer.Name;
				col = gamePlayerFromRewiredPlayer.Color.uiColor;
				return;
			}
		}
		else if (player.id == 0)
		{
			if (GameManager.LobbyController != null && GameManager.LobbyController.IsLocalPlayerInSlot(player.id))
			{
				int localPlayerSlot = GameManager.LobbyController.GetLocalPlayerSlot(player.id);
				if (localPlayerSlot != -1)
				{
					name = GameManager.LobbyController.SlotName(localPlayerSlot);
					col = GameManager.GetColorAtIndex((int)GameManager.LobbyController.SlotColor(localPlayerSlot)).skinColor3;
					return;
				}
			}
			else
			{
				name = ((PlatformUserManager.Instance.MainUser == null) ? "Main Player" : PlatformUserManager.Instance.MainUser.GetProfileName());
			}
		}
	}

	// Token: 0x06001506 RID: 5382 RVA: 0x00099C4C File Offset: 0x00097E4C
	private bool IsInGame()
	{
		Scene activeScene = SceneManager.GetActiveScene();
		return !(activeScene.name == "Main") && !(activeScene.name == "MainMenu");
	}

	// Token: 0x040015EF RID: 5615
	[Header("UI")]
	[SerializeField]
	private GameObject m_window;

	// Token: 0x040015F0 RID: 5616
	[SerializeField]
	private Text m_titleText;

	// Token: 0x040015F1 RID: 5617
	[SerializeField]
	private Text m_descriptionText;

	// Token: 0x040015F2 RID: 5618
	[SerializeField]
	private Image m_playerIcon;

	// Token: 0x040015F3 RID: 5619
	[SerializeField]
	private Text m_userNameText;

	// Token: 0x040015F4 RID: 5620
	[SerializeField]
	private GameObject m_userList;

	// Token: 0x040015F5 RID: 5621
	[SerializeField]
	private GameObject m_alreadyAssigned;

	// Token: 0x040015F6 RID: 5622
	[SerializeField]
	private AudioClip m_error;

	// Token: 0x040015F7 RID: 5623
	[SerializeField]
	private Animator m_anim;

	// Token: 0x040015F8 RID: 5624
	[Header("Buttons")]
	[SerializeField]
	private GameObject m_continueButton;

	// Token: 0x040015F9 RID: 5625
	[SerializeField]
	private GameObject m_switchButton;

	// Token: 0x040015FA RID: 5626
	[SerializeField]
	private GameObject m_leaveButton;

	// Token: 0x040015FB RID: 5627
	private Player m_curDisconnectedPlayer;

	// Token: 0x040015FC RID: 5628
	private ControllerDisconnectedHandlerState m_curState;

	// Token: 0x040015FD RID: 5629
	private Dictionary<long, Player> m_previousControllers = new Dictionary<long, Player>();
}
