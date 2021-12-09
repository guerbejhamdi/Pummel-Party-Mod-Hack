using System;
using System.Collections;
using I2.Loc;
using Rewired;
using UnityEngine;
using ZP.Net;

// Token: 0x020003F6 RID: 1014
public class GameNetworkManager : MonoBehaviour
{
	// Token: 0x06001BDB RID: 7131 RVA: 0x000BAC84 File Offset: 0x000B8E84
	private void Awake()
	{
		this.disconnectAnimation = (Resources.Load("Prefabs/UI/DisconnectTransitionCanvas") as GameObject);
		this.player_connected_event = new PlayerConnectedEventHandler(this.PlayerConnected);
		this.host_connected_event = new HostConnectedEventHandler(this.HostConnected);
		this.player_disconnected_event = new PlayerDisconnectedEventHandler(this.PlayerDisconnected);
		this.player_loaded_event = new PlayerLoadedEventHandler(this.PlayerLoaded);
		this.host_loaded_event = new HostLoadedEventHandler(this.HostLoaded);
		this.disconnectEventHandler = new DisconnectEventHandler(this.OnDisconnected);
		NetSystem.PlayerConnected += this.player_connected_event;
		NetSystem.HostConnected += this.host_connected_event;
		NetSystem.PlayerDisconnected += this.player_disconnected_event;
		NetSystem.PlayerLoaded += this.player_loaded_event;
		NetSystem.HostLoaded += this.host_loaded_event;
		NetSystem.Disconnected += this.disconnectEventHandler;
		GameManager.gameNetworkManager = this;
	}

	// Token: 0x06001BDC RID: 7132 RVA: 0x00014655 File Offset: 0x00012855
	public void OnMainUserSignedOut()
	{
		base.StartCoroutine(this.UserChanged());
	}

	// Token: 0x06001BDD RID: 7133 RVA: 0x00014655 File Offset: 0x00012855
	public void SuspendUserSignedOut()
	{
		base.StartCoroutine(this.UserChanged());
	}

	// Token: 0x06001BDE RID: 7134 RVA: 0x00014664 File Offset: 0x00012864
	private IEnumerator UserChanged()
	{
		this.waitStart = Time.time;
		GameManager.disconnectUserSignOut = true;
		yield return new WaitForSeconds(0.25f);
		while (!PlatformStorageManager.Instance.HasLoaded() && Time.time - this.waitStart < this.m_maxProfileLoadWaitTime)
		{
			yield return new WaitForSeconds(0.1f);
		}
		NetSystem.OnDisconnect("UserChanged");
		yield break;
	}

	// Token: 0x06001BDF RID: 7135 RVA: 0x00014673 File Offset: 0x00012873
	private void Start()
	{
		Debug.Log("Game network Manager: " + base.name);
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x000BAD5C File Offset: 0x000B8F5C
	private void Update()
	{
		if (GameManager.CurState != GameState.MainMenu && ReInput.isReady && ReInput.players.Players[0] != null && (ReInput.players.Players[0].GetButtonUp(InputActions.UIBack) || (!GameManager.IsGamePaused && ReInput.players.Players[0].GetButtonUp(InputActions.UIConfirm))) && !GameManager.IsGamePaused)
		{
			GameManager.PauseGame(true);
		}
	}

	// Token: 0x06001BE1 RID: 7137 RVA: 0x0000398C File Offset: 0x00001B8C
	public void HostConnected(NetPlayer host)
	{
	}

	// Token: 0x06001BE2 RID: 7138 RVA: 0x0000398C File Offset: 0x00001B8C
	public void PlayerConnected(NetPlayer player)
	{
	}

	// Token: 0x06001BE3 RID: 7139 RVA: 0x000BADD8 File Offset: 0x000B8FD8
	public void OnDisconnected(string reason)
	{
		this.Disconnect(LocalizationManager.GetTranslation("Disconnected", true, 0, true, false, null, null, true) + ": " + reason);
	}

	// Token: 0x06001BE4 RID: 7140 RVA: 0x0001468A File Offset: 0x0001288A
	public void PlayerDisconnected(NetPlayer player)
	{
		if (GameManager.UIController != null)
		{
			GameManager.UIController.playerDisconnectUI.Show(player);
		}
	}

	// Token: 0x06001BE5 RID: 7141 RVA: 0x000146A9 File Offset: 0x000128A9
	public void Disconnect(string reason)
	{
		if (GameManager.disconnected)
		{
			return;
		}
		if (!NetGameServer.IsLocal)
		{
			GameManager.disconnected = true;
			GameManager.disconnectReason = reason;
		}
		UnityEngine.Object.Instantiate<GameObject>(this.disconnectAnimation);
	}

	// Token: 0x06001BE6 RID: 7142 RVA: 0x000BAE08 File Offset: 0x000B9008
	public void PlayerLoaded(NetPlayer player)
	{
		Debug.Log("player loaded : " + player.UserID.ToString());
		this.loadedPlayersCount++;
		if (this.loadedPlayersCount == NetSystem.PlayerCount)
		{
			this.Spawn();
		}
	}

	// Token: 0x06001BE7 RID: 7143 RVA: 0x000146D2 File Offset: 0x000128D2
	public void HostLoaded()
	{
		Debug.Log("Host Loaded");
		this.loadedPlayersCount++;
		if (this.loadedPlayersCount == NetSystem.PlayerCount)
		{
			this.Spawn();
		}
	}

	// Token: 0x06001BE8 RID: 7144 RVA: 0x000146FF File Offset: 0x000128FF
	private void Spawn()
	{
		if (NetSystem.IsServer)
		{
			NetSystem.Spawn("GameBoardController", 0, NetSystem.MyPlayer);
			if (GameManager.partyGameMode == PartyGameMode.BoardGame)
			{
				NetSystem.Spawn("KeyController", 0, NetSystem.MyPlayer);
			}
		}
	}

	// Token: 0x06001BE9 RID: 7145 RVA: 0x000BAE54 File Offset: 0x000B9054
	private void OnDestroy()
	{
		NetSystem.PlayerConnected -= this.player_connected_event;
		NetSystem.HostConnected -= this.host_connected_event;
		NetSystem.PlayerDisconnected -= this.player_disconnected_event;
		NetSystem.PlayerLoaded -= this.player_loaded_event;
		NetSystem.HostLoaded -= this.host_loaded_event;
		NetSystem.Disconnected -= this.disconnectEventHandler;
	}

	// Token: 0x04001E0F RID: 7695
	private GameObject disconnectAnimation;

	// Token: 0x04001E10 RID: 7696
	private PlayerConnectedEventHandler player_connected_event;

	// Token: 0x04001E11 RID: 7697
	private HostConnectedEventHandler host_connected_event;

	// Token: 0x04001E12 RID: 7698
	private PlayerDisconnectedEventHandler player_disconnected_event;

	// Token: 0x04001E13 RID: 7699
	private PlayerLoadedEventHandler player_loaded_event;

	// Token: 0x04001E14 RID: 7700
	private HostLoadedEventHandler host_loaded_event;

	// Token: 0x04001E15 RID: 7701
	private DisconnectEventHandler disconnectEventHandler;

	// Token: 0x04001E16 RID: 7702
	private int loadedPlayersCount;

	// Token: 0x04001E17 RID: 7703
	private float waitStart;

	// Token: 0x04001E18 RID: 7704
	private float m_maxProfileLoadWaitTime = 10f;
}
