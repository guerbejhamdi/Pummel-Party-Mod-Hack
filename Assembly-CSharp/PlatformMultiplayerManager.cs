using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002E1 RID: 737
public class PlatformMultiplayerManager
{
	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06001498 RID: 5272 RVA: 0x0000FF3C File Offset: 0x0000E13C
	public static PlatformMultiplayerManager Instance
	{
		get
		{
			PlatformMultiplayerManager instance = PlatformMultiplayerManager.m_instance;
			return PlatformMultiplayerManager.m_instance;
		}
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void CheckMultiplayerAvailability(IPlatformUser user, MultiplayerAvailabilityResult callback)
	{
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Initialize()
	{
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x0000539F File Offset: 0x0000359F
	public virtual bool IsConnectedToMultiplayerService()
	{
		return false;
	}

	// Token: 0x0600149C RID: 5276 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void CreateMultiplayerServiceSession(IPlatformUser user, CreateSessionResult callback)
	{
	}

	// Token: 0x0600149D RID: 5277 RVA: 0x0000539F File Offset: 0x0000359F
	public virtual bool ShowSystemInviteGUI()
	{
		return false;
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x0000539F File Offset: 0x0000359F
	public virtual bool HasQueuedGameInvites()
	{
		return false;
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x000053AE File Offset: 0x000035AE
	public virtual IPlatformGameInvite GetLatestGameInviteAndClearQueue()
	{
		return null;
	}

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x060014A0 RID: 5280 RVA: 0x00098DC8 File Offset: 0x00096FC8
	// (remove) Token: 0x060014A1 RID: 5281 RVA: 0x00098E00 File Offset: 0x00097000
	public event LobbyListUpdated OnLobbyListUpdated;

	// Token: 0x060014A2 RID: 5282 RVA: 0x0000FF49 File Offset: 0x0000E149
	protected void DoOnLobbyListUpdated(List<PlatformMultiplayerLobby> lobbies)
	{
		if (this.OnLobbyListUpdated != null)
		{
			this.OnLobbyListUpdated(lobbies);
			return;
		}
		Debug.LogError("OnLobbyListUpdated is null");
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x060014A3 RID: 5283 RVA: 0x0000FF6A File Offset: 0x0000E16A
	// (set) Token: 0x060014A4 RID: 5284 RVA: 0x0000FF72 File Offset: 0x0000E172
	public bool WasLobbyDataChanged { get; set; }

	// Token: 0x060014A5 RID: 5285 RVA: 0x00098E38 File Offset: 0x00097038
	public void SetLobbyDataString(string key, string value)
	{
		Debug.Log("SetLobbyDataString : " + key + " = " + value);
		if (this.m_stringLobbyData.ContainsKey(key))
		{
			this.WasLobbyDataChanged = (this.m_stringLobbyData[key] != value);
			this.m_stringLobbyData[key] = value;
			return;
		}
		this.m_stringLobbyData.Add(key, value);
		this.WasLobbyDataChanged = true;
	}

	// Token: 0x060014A6 RID: 5286 RVA: 0x00098EA4 File Offset: 0x000970A4
	public void SetLobbyDataNumber(string key, double value)
	{
		Debug.Log("SetLobbyDataNumber : " + key + " = " + value.ToString());
		if (this.m_numberLobbyData.ContainsKey(key))
		{
			this.WasLobbyDataChanged = (this.m_numberLobbyData[key] != value);
			this.m_numberLobbyData[key] = value;
			return;
		}
		this.m_numberLobbyData.Add(key, value);
		this.WasLobbyDataChanged = true;
	}

	// Token: 0x060014A7 RID: 5287 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void CreateLobby(IPlatformUser user, MultiplayerLobbyVisibility visibility, CreateLobbyResult callback)
	{
	}

	// Token: 0x060014A8 RID: 5288 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void JoinGameLobby(IPlatformUser user, object handle, JoinLobbyResult callback)
	{
	}

	// Token: 0x060014A9 RID: 5289 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void LeaveLobby(IPlatformUser user)
	{
	}

	// Token: 0x060014AA RID: 5290 RVA: 0x0000539F File Offset: 0x0000359F
	public virtual bool ShouldApplyLobbyData()
	{
		return false;
	}

	// Token: 0x060014AB RID: 5291 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void ApplyLobbyData(IPlatformUser user)
	{
	}

	// Token: 0x060014AC RID: 5292 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void UpdateLobbyList(IPlatformUser user)
	{
	}

	// Token: 0x060014AD RID: 5293 RVA: 0x0000539F File Offset: 0x0000359F
	public virtual bool IsSecureConnectionRequired()
	{
		return false;
	}

	// Token: 0x060014AE RID: 5294 RVA: 0x0000539F File Offset: 0x0000359F
	public virtual bool IsNatIntroductionRequired()
	{
		return false;
	}

	// Token: 0x060014AF RID: 5295 RVA: 0x0000FF7B File Offset: 0x0000E17B
	public virtual ulong GetUniqueIdentifierForNatIntroduction()
	{
		return 0UL;
	}

	// Token: 0x060014B0 RID: 5296 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void CreateSecureConnectionToHost(CreateSecureConnectionResult callback)
	{
	}

	// Token: 0x060014B1 RID: 5297 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void ConnectToHostFromActiveLobby(ConnectToHostResult callback)
	{
	}

	// Token: 0x060014B2 RID: 5298 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void RunQueuedTitleActivationEvents()
	{
	}

	// Token: 0x060014B3 RID: 5299 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void ResetGame()
	{
	}

	// Token: 0x040015CD RID: 5581
	protected static PlatformMultiplayerManager m_instance;

	// Token: 0x040015D0 RID: 5584
	protected Dictionary<string, string> m_stringLobbyData = new Dictionary<string, string>();

	// Token: 0x040015D1 RID: 5585
	protected Dictionary<string, double> m_numberLobbyData = new Dictionary<string, double>();
}
