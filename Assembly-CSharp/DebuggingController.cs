using System;
using System.Collections;
using LlockhamIndustries.Decals;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZP.Net;

// Token: 0x02000041 RID: 65
public class DebuggingController : MonoBehaviour
{
	// Token: 0x17000020 RID: 32
	// (get) Token: 0x0600010C RID: 268 RVA: 0x00004431 File Offset: 0x00002631
	// (set) Token: 0x0600010D RID: 269 RVA: 0x00004439 File Offset: 0x00002639
	public bool Host { get; set; }

	// Token: 0x0600010E RID: 270 RVA: 0x00004442 File Offset: 0x00002642
	public void Awake()
	{
		if (!GameManager.DEBUGGING)
		{
			return;
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		DebuggingController.cur = this;
	}

	// Token: 0x0600010F RID: 271 RVA: 0x0002FFE8 File Offset: 0x0002E1E8
	private void Update()
	{
		if (!GameManager.DEBUGGING)
		{
			return;
		}
		if (this.state != DebuggingState.FirstLoad)
		{
			TimeSpan timeSpan = DateTime.Now.Subtract(GameManager.startTime);
			DebugTextUI.AddLine(string.Concat(new string[]
			{
				"Total Turns: ",
				GameManager.totalTurns.ToString(),
				" Time: ",
				timeSpan.Hours.ToString(),
				"h ",
				timeSpan.Minutes.ToString(),
				"m"
			}), false);
			DebugTextUI.AddLine("Debugging Game Count: " + this.GameCount.ToString(), false);
			DebugTextUI.AddLine("Ending Game In: " + ((this.gameLength.Remaining > 60f) ? (((int)this.gameLength.Remaining / 60).ToString() + "m ") : "") + ((int)(this.gameLength.Remaining % 60f)).ToString() + "s", false);
		}
		if (Input.GetKeyDown(KeyCode.KeypadMinus))
		{
			Time.timeScale = Mathf.Clamp(Time.timeScale - 1f, 0f, 10f);
		}
		if (Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			Time.timeScale = Mathf.Clamp(Time.timeScale + 1f, 0f, 10f);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			DynamicDecals.System.enabled = !DynamicDecals.System.enabled;
		}
		switch (this.state)
		{
		case DebuggingState.FirstLoad:
			if (this.GetButtonHost())
			{
				this.Host = true;
				this.StartDebugging();
				SceneManager.sceneLoaded += this.OnSceneLoaded;
				return;
			}
			if (this.GetButtonClient())
			{
				this.Host = false;
				this.StartDebugging();
				SceneManager.sceneLoaded += this.OnSceneLoaded;
				return;
			}
			break;
		case DebuggingState.Menu:
			break;
		case DebuggingState.Host:
			if (this.gameLength.Elapsed(true) || (this.inWinScreen && this.winScreenLeaveTimer.Elapsed(true)))
			{
				this.StartEndGame(GameManager.rand.NextDouble() > 0.5, GameManager.rand.NextDouble() > 0.5);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000110 RID: 272 RVA: 0x0000445D File Offset: 0x0000265D
	private bool GetButtonHost()
	{
		return Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F11);
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00004477 File Offset: 0x00002677
	private bool GetButtonClient()
	{
		return Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F10);
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000112 RID: 274 RVA: 0x00004491 File Offset: 0x00002691
	// (set) Token: 0x06000113 RID: 275 RVA: 0x00004499 File Offset: 0x00002699
	public int GameCount { get; set; }

	// Token: 0x06000114 RID: 276 RVA: 0x0003023C File Offset: 0x0002E43C
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "MainMenu" && this.state == DebuggingState.Menu)
		{
			int gameCount = this.GameCount;
			this.GameCount = gameCount + 1;
			this.StartDebugging();
			this.inWinScreen = false;
			return;
		}
		if (scene.name == "EndScreen")
		{
			this.winScreenLeaveTimer.SetInterval(15f, 20f, true);
			this.inWinScreen = true;
		}
	}

	// Token: 0x06000115 RID: 277 RVA: 0x000044A2 File Offset: 0x000026A2
	public void StartEndGame(bool clientHost, bool hostDisconnect)
	{
		base.StartCoroutine(this.EndGame(clientHost, hostDisconnect));
	}

	// Token: 0x06000116 RID: 278 RVA: 0x000044B3 File Offset: 0x000026B3
	private IEnumerator EndGame(bool clientHost, bool hostDisconnect)
	{
		this.state = DebuggingState.Menu;
		Debug.LogError("Ending Game: ");
		if (NetSystem.IsServer)
		{
			this.Host = !clientHost;
			GameManager.Board.EndGame(clientHost, hostDisconnect);
			yield return new WaitForSeconds(3f);
			GameManager.PauseGame(true);
			yield return new WaitForSeconds(2f);
			GameManager.UIController.DoButtonEvent(MainMenuButtonEventType.OptionsLeaveGame);
			yield return new WaitForSeconds(2f);
			GameManager.UIController.DoButtonEvent(MainMenuButtonEventType.ExitGame);
		}
		else
		{
			this.Host = clientHost;
			if (!hostDisconnect)
			{
				yield return new WaitForSeconds(this.wTime);
				GameManager.PauseGame(true);
				yield return new WaitForSeconds(2f);
				GameManager.UIController.DoButtonEvent(MainMenuButtonEventType.OptionsLeaveGame);
				yield return new WaitForSeconds(2f);
				GameManager.UIController.DoButtonEvent(MainMenuButtonEventType.ExitGame);
			}
		}
		yield break;
	}

	// Token: 0x06000117 RID: 279 RVA: 0x000044D0 File Offset: 0x000026D0
	private void StartDebugging()
	{
		Debug.LogError("Starting Debugging Join");
		this.testingStartTime = Time.time;
		if (this.Host)
		{
			base.StartCoroutine(this.HostFromMenu());
			return;
		}
		base.StartCoroutine(this.ConnectFromMenu());
	}

	// Token: 0x06000118 RID: 280 RVA: 0x0000450A File Offset: 0x0000270A
	private IEnumerator HostFromMenu()
	{
		yield return new WaitForSeconds(2f);
		if (GameManager.MainMenuUIController.error_wnd.Visible)
		{
			GameManager.MainMenuUIController.DoButtonEvent(MainMenuButtonEventType.ErrorOK);
		}
		yield return new WaitForSeconds(1f);
		GameManager.MainMenuUIController.DoButtonEvent(MainMenuButtonEventType.GoMultiplayerWindow);
		yield return new WaitForSeconds(this.wTime * 6f);
		GameManager.MainMenuUIController.DoButtonEvent(MainMenuButtonEventType.GoCreateGameWindow);
		yield return new WaitForSeconds(this.wTime);
		int attempts = 0;
		int num;
		for (;;)
		{
			if (attempts != 0)
			{
				GameManager.MainMenuUIController.DoButtonEvent(MainMenuButtonEventType.ErrorOK);
				yield return new WaitForSeconds(this.wTime);
			}
			GameManager.MainMenuUIController.DoButtonEvent(MainMenuButtonEventType.HostGame);
			yield return new WaitForSeconds(2f);
			yield return new WaitUntil(() => NetSystem.IsConnected || GameManager.MainMenuUIController.error_wnd.Visible);
			if (NetSystem.IsConnected)
			{
				break;
			}
			num = attempts;
			attempts = num + 1;
		}
		yield return new WaitForSeconds(this.wTime);
		yield return new WaitUntil(() => GameManager.LobbyController != null);
		int num2 = GameManager.rand.Next(0, GameManager.PossibleMaxPlayers.Length);
		int num3 = GameManager.PossibleMaxPlayers[num2];
		GameManager.LobbyController.SetLobbyOption(LobbyOption.MaxPlayers, num2);
		GameManager.LobbyController.SetLobbyOption(LobbyOption.TurnCount, GameManager.rand.Next(0, GameManager.PossibleTurnCounts.Length - 1));
		GameManager.LobbyController.SetLobbyOption(LobbyOption.Map, GameManager.rand.Next(0, GameManager.GetMaps().Length));
		GameManager.LobbyController.SetLobbyOption(LobbyOption.MaxTurnLength, GameManager.rand.Next(0, GameManager.PossiblyTurnLengths.Length));
		GameManager.LobbyController.SetLobbyOption(LobbyOption.MinigameCount, GameManager.rand.Next(0, GameManager.PossibleMinigameCounts.Length - 1));
		GameManager.LobbyController.SetLobbyOption(LobbyOption.WinningRelics, GameManager.rand.Next(0, GameManager.PossibleWinningRelics.Length));
		GameManager.LobbyController.SetLobbyOption(LobbyOption.GameMode, (GameManager.rand.NextDouble() < 0.800000011920929) ? 0 : 1);
		int aiAddCount = (int)Math.Ceiling((double)((float)GameManager.rand.Next(1, num3 + 1) / 2f));
		for (int i = 0; i < aiAddCount; i = num)
		{
			GameManager.MainMenuUIController.DoButtonEvent(MainMenuButtonEventType.AddAI);
			yield return new WaitForSeconds(0.4f);
			num = i + 1;
		}
		yield return new WaitUntil(() => GameManager.LobbyController.SlotsFull() || GameManager.LobbyController.GetActiveCount() == aiAddCount * 2);
		yield return new WaitForSeconds(this.wTime);
		GameManager.MainMenuUIController.DoButtonEvent(MainMenuButtonEventType.StartGame);
		this.gameLength.SetInterval(this.minGameLength, this.maxGameLength, true);
		this.state = DebuggingState.Host;
		yield break;
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00004519 File Offset: 0x00002719
	private IEnumerator ConnectFromMenu()
	{
		yield return new WaitForSeconds(10f);
		if (GameManager.MainMenuUIController.error_wnd.Visible)
		{
			GameManager.MainMenuUIController.DoButtonEvent(MainMenuButtonEventType.ErrorOK);
		}
		yield return new WaitForSeconds(1f);
		int attempts = 0;
		bool waitForGame = false;
		int num;
		for (;;)
		{
			if (!waitForGame)
			{
				if (attempts == 0)
				{
					GameManager.MainMenuUIController.DoButtonEvent(MainMenuButtonEventType.GoMultiplayerWindow);
					yield return new WaitForSeconds(this.wTime * 6f);
				}
				else
				{
					GameManager.MainMenuUIController.DoButtonEvent(MainMenuButtonEventType.ErrorOK);
					yield return new WaitForSeconds(this.wTime);
					GameManager.MainMenuUIController.DoButtonEvent(MainMenuButtonEventType.JoinGameWindowBack);
					yield return new WaitForSeconds(this.wTime);
				}
			}
			GameManager.MainMenuUIController.DoButtonEvent(MainMenuButtonEventType.GoDirectConnectWindow);
			yield return new WaitForSeconds(this.wTime);
			GameManager.MainMenuUIController.DoButtonEvent(MainMenuButtonEventType.ConnectToGame);
			yield return new WaitForSeconds(this.wTime);
			yield return new WaitUntil(() => NetSystem.IsConnected || GameManager.MainMenuUIController.error_wnd.Visible);
			yield return new WaitForSeconds(this.wTime * 3f);
			if (NetSystem.IsConnected)
			{
				break;
			}
			num = attempts;
			attempts = num + 1;
		}
		yield return new WaitUntil(() => GameManager.LobbyController != null);
		yield return new WaitForSeconds(3f);
		int activeCount = GameManager.LobbyController.GetActiveCount();
		int aiAddCount = Mathf.Clamp(activeCount * 2, 0, GameManager.LobbyMaxPlayers) - activeCount;
		for (int i = 0; i < aiAddCount; i = num)
		{
			GameManager.MainMenuUIController.DoButtonEvent(MainMenuButtonEventType.AddAI);
			yield return new WaitForSeconds(this.wTime);
			num = i + 1;
		}
		this.state = DebuggingState.Client;
		yield break;
	}

	// Token: 0x04000166 RID: 358
	public static DebuggingController cur;

	// Token: 0x04000168 RID: 360
	public DebuggingState state;

	// Token: 0x04000169 RID: 361
	private float minGameLength = 300f;

	// Token: 0x0400016A RID: 362
	private float maxGameLength = 7200f;

	// Token: 0x0400016B RID: 363
	private ActionTimer winScreenLeaveTimer = new ActionTimer(0f);

	// Token: 0x0400016C RID: 364
	public ActionTimer gameLength = new ActionTimer(0f);

	// Token: 0x0400016D RID: 365
	private bool inWinScreen;

	// Token: 0x0400016E RID: 366
	private float testingStartTime;

	// Token: 0x04000170 RID: 368
	private float wTime = 1f;
}
