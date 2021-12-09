using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Lidgren.Network;
using Steamworks;
using UnityEngine;
using ZP.Net;

// Token: 0x02000483 RID: 1155
public class NetSteamRelay : MonoBehaviour, INetRelay
{
	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x06001EFF RID: 7935 RVA: 0x00016D11 File Offset: 0x00014F11
	public SteamRelayConnectionStatus Status
	{
		get
		{
			return this.m_status;
		}
	}

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x06001F00 RID: 7936 RVA: 0x00016D19 File Offset: 0x00014F19
	public static NetSteamRelay Instance
	{
		get
		{
			return NetSteamRelay.m_instance;
		}
	}

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06001F01 RID: 7937 RVA: 0x000C693C File Offset: 0x000C4B3C
	// (remove) Token: 0x06001F02 RID: 7938 RVA: 0x000C6974 File Offset: 0x000C4B74
	public event P2PSessionEstablishedEventHandler P2PSessionEstablished;

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06001F03 RID: 7939 RVA: 0x000C69AC File Offset: 0x000C4BAC
	// (remove) Token: 0x06001F04 RID: 7940 RVA: 0x000C69E4 File Offset: 0x000C4BE4
	public event P2PSessionFailureEventHandler P2PSessionFailure;

	// Token: 0x06001F05 RID: 7941 RVA: 0x00016D20 File Offset: 0x00014F20
	private void OnP2PSessionEstablished()
	{
		if (this.P2PSessionEstablished != null)
		{
			this.P2PSessionEstablished();
		}
	}

	// Token: 0x06001F06 RID: 7942 RVA: 0x00016D35 File Offset: 0x00014F35
	private void OnP2PSessionFailure(string result)
	{
		if (this.P2PSessionFailure != null)
		{
			this.P2PSessionFailure(result);
		}
	}

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x06001F07 RID: 7943 RVA: 0x000C6A1C File Offset: 0x000C4C1C
	// (remove) Token: 0x06001F08 RID: 7944 RVA: 0x000C6A54 File Offset: 0x000C4C54
	public event RelayConnectSucceededEventHandler RelayConnectSucceeded;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06001F09 RID: 7945 RVA: 0x000C6A8C File Offset: 0x000C4C8C
	// (remove) Token: 0x06001F0A RID: 7946 RVA: 0x000C6AC4 File Offset: 0x000C4CC4
	public event RelayConnectFailedEventHandler RelayConnectFailed;

	// Token: 0x06001F0B RID: 7947 RVA: 0x00016D4B File Offset: 0x00014F4B
	private void OnRelayConnectSucceeded()
	{
		Debug.Log("OnRelayConnectSucceeded");
		if (this.RelayConnectSucceeded != null)
		{
			this.RelayConnectSucceeded();
		}
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x00016D6A File Offset: 0x00014F6A
	private void OnRelayConnectFailed(string result)
	{
		Debug.Log("OnRelayConnectFailed");
		if (this.RelayConnectFailed != null)
		{
			this.RelayConnectFailed(result);
		}
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x00016D8A File Offset: 0x00014F8A
	public IEnumerator ConnectToLobby(CSteamID lobbyId)
	{
		Debug.Log("Net Steam Relay : Start Connect To Lobby");
		Debug.Log("===================");
		if (!this.m_connectionInProgress)
		{
			this.m_connectionInProgress = true;
			this.m_status = SteamRelayConnectionStatus.StartingConnection;
			CSteamID hostUserId = SteamMatchmaking.GetLobbyOwner(lobbyId);
			string str = "lobby = ";
			CSteamID csteamID = lobbyId;
			string str2 = csteamID.ToString();
			string str3 = ", host = ";
			csteamID = hostUserId;
			Debug.Log(str + str2 + str3 + csteamID.ToString());
			yield return base.StartCoroutine(this.EstablishP2PSession(hostUserId));
			Debug.Log("EstablishP2PSession Coroutine Completed");
			if (this.m_p2pSessionFailed)
			{
				Debug.Log("EstablishP2PSession : Failed");
				this.OnRelayConnectFailed(this.m_lastError);
				yield break;
			}
			Debug.Log("EstablishP2PSession : Success");
			yield return base.StartCoroutine(this.DoGameConnect(hostUserId));
			hostUserId = default(CSteamID);
		}
		else
		{
			this.OnRelayConnectFailed("Relay connect already in progress");
		}
		Debug.Log("===================");
		yield break;
	}

	// Token: 0x06001F0E RID: 7950 RVA: 0x00016DA0 File Offset: 0x00014FA0
	private IEnumerator EstablishP2PSession(CSteamID hostUserId)
	{
		this.m_status = SteamRelayConnectionStatus.EstablishingSession;
		Debug.Log("Establishing P2P Session");
		this.m_p2pSessionFailed = false;
		if (!this.SendConnectionRequestPacket(hostUserId))
		{
			this.m_p2pSessionFailed = true;
			this.m_lastError = "Failed sending initial p2p session packet";
			this.OnP2PSessionFailure(this.m_lastError);
			yield break;
		}
		this.startTime = Time.time;
		while (Time.time - this.startTime < 21f)
		{
			if (this.m_p2pSessionFailed)
			{
				this.OnP2PSessionFailure("P2P Session Failure : " + this.m_lastError);
				yield break;
			}
			uint num;
			if (SteamNetworking.IsP2PPacketAvailable(out num, 1) && num == 2U)
			{
				byte[] array = new byte[2];
				uint num2 = 0U;
				CSteamID csteamID;
				if (SteamNetworking.ReadP2PPacket(array, 2U, out num2, out csteamID, 1))
				{
					if (csteamID.m_SteamID == hostUserId.m_SteamID && array[0] == 8 && array[1] == this.m_lastConnectRequestID)
					{
						P2PSessionState_t p2PSessionState_t;
						if (SteamNetworking.GetP2PSessionState(hostUserId, out p2PSessionState_t))
						{
							Debug.Log("Steam Networking P2P session established | relay=" + p2PSessionState_t.m_bUsingRelay.ToString());
							this.OnP2PSessionEstablished();
							yield break;
						}
						Debug.LogError("Failed getting session state");
						this.m_p2pSessionFailed = true;
						this.m_lastError = "Failed getting session state";
						this.OnP2PSessionFailure(this.m_lastError);
						yield break;
					}
					else
					{
						Debug.LogError(string.Concat(new string[]
						{
							"NetSteamRelay connect coroutine read bad packet, ",
							csteamID.m_SteamID.ToString(),
							"==",
							hostUserId.m_SteamID.ToString(),
							", ",
							array[0].ToString(),
							"==",
							8.ToString(),
							", ",
							array[1].ToString(),
							"==",
							this.m_lastConnectRequestID.ToString()
						}));
					}
				}
			}
			yield return null;
		}
		Debug.LogError("Session Creation Attempt Timeout");
		this.m_p2pSessionFailed = true;
		this.m_lastError = "Session Creation Attempt Timeout";
		this.OnP2PSessionFailure(this.m_lastError);
		yield break;
	}

	// Token: 0x06001F0F RID: 7951 RVA: 0x000C6AFC File Offset: 0x000C4CFC
	private bool SendConnectionRequestPacket(CSteamID hostUserId)
	{
		this.m_lastConnectRequestID = NetSteamRelay.m_nextClientRequestID;
		byte[] array = new byte[]
		{
			4,
			this.m_lastConnectRequestID
		};
		NetSteamRelay.m_nextClientRequestID += 1;
		return SteamNetworking.SendP2PPacket(hostUserId, array, (uint)array.Length, EP2PSend.k_EP2PSendReliable, 1);
	}

	// Token: 0x06001F10 RID: 7952 RVA: 0x000C6B44 File Offset: 0x000C4D44
	private void OnP2PSessionRequest(P2PSessionRequest_t pCallback)
	{
		if (NetSystem.IsServer && SteamManager.Initialized)
		{
			CSteamID steamIDLobby = (CSteamID)GameManager.CurrentLobby;
			int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(steamIDLobby);
			for (int i = 0; i < numLobbyMembers; i++)
			{
				if (SteamMatchmaking.GetLobbyMemberByIndex(steamIDLobby, i).m_SteamID == pCallback.m_steamIDRemote.m_SteamID)
				{
					SteamNetworking.AcceptP2PSessionWithUser(pCallback.m_steamIDRemote);
					return;
				}
			}
			Debug.LogError("OnP2pSessionRequest : user is not in lobby");
			return;
		}
		Debug.LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		Debug.LogError("NetSteamRelay : Not Server or SteamManager not initialized!!");
	}

	// Token: 0x06001F11 RID: 7953 RVA: 0x00016DB6 File Offset: 0x00014FB6
	private void OnP2PSessionConnectFail(P2PSessionConnectFail_t pCallback)
	{
		this.m_lastError = this.GetP2PErrorString((EP2PSessionError)pCallback.m_eP2PSessionError);
		Debug.LogError("P2PSessionConnectFail : " + this.m_lastError);
		this.m_p2pSessionFailed = true;
	}

	// Token: 0x06001F12 RID: 7954 RVA: 0x000C6BC4 File Offset: 0x000C4DC4
	public void Update()
	{
		if (this.m_destroyQueued)
		{
			NetSteamRelay.Destroy();
			return;
		}
		if (this.m_isInitialized && NetSteamRelay.m_isServer && SteamManager.Initialized)
		{
			uint num = 0U;
			if (SteamNetworking.IsP2PPacketAvailable(out num, 1))
			{
				byte[] array = new byte[Mathf.Max(1, (int)num)];
				uint num2 = 0U;
				Debug.Log("packet available, size = " + num.ToString());
				CSteamID csteamID;
				if (SteamNetworking.ReadP2PPacket(array, num, out num2, out csteamID, 1))
				{
					if (num2 == 2U && array[0] == 4)
					{
						Debug.LogError("Accepting relay connection handshake");
						NetGameServer.SetRelayProvider(this);
						this.AddEndpointMapping(csteamID);
						byte[] array2 = new byte[]
						{
							8,
							array[1]
						};
						if (!SteamNetworking.SendP2PPacket(csteamID, array2, (uint)array2.Length, EP2PSend.k_EP2PSendReliable, 1))
						{
							Debug.LogError("NetSteamRelay :  error sending accept connection packet");
							NetGameServer.SetRelayProvider(null);
							this.RemoveEndpointMapping(csteamID);
						}
					}
					else
					{
						Debug.LogError("Sending session establishment null packet");
					}
				}
			}
		}
		this.DoStatistics();
	}

	// Token: 0x06001F13 RID: 7955 RVA: 0x00016DE6 File Offset: 0x00014FE6
	public IEnumerator DoGameConnect(CSteamID host)
	{
		this.m_status = SteamRelayConnectionStatus.AttemptingConnection;
		string str = "DoGameConnect : ";
		CSteamID csteamID = host;
		Debug.Log(str + csteamID.ToString());
		string text = NetGameClient.Start(14300);
		if (text == "")
		{
			Debug.Log("DoGameConnect Success");
			NetGameClient.SetRelayProvider(this);
			NetGameClient.Connect(this.AddEndpointMapping(host));
		}
		else
		{
			Debug.Log("DoGameConnect Failure : " + text);
			this.OnRelayConnectFailed(text);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001F14 RID: 7956 RVA: 0x00016DFC File Offset: 0x00014FFC
	public void Awake()
	{
		this.InitDebug();
	}

	// Token: 0x06001F15 RID: 7957 RVA: 0x00016E04 File Offset: 0x00015004
	private void OnDestroy()
	{
		this.DestroyDebug();
	}

	// Token: 0x06001F16 RID: 7958 RVA: 0x000C6CB0 File Offset: 0x000C4EB0
	private IPEndPoint AddEndpointMapping(CSteamID id)
	{
		Debug.Log("Adding endpoint mapping id = " + id.m_SteamID.ToString());
		IPEndPoint ipendPoint = new IPEndPoint((long)((id.m_SteamID & this.mask) >> 1), 12345);
		Debug.Log("Generated endpoint = " + ipendPoint.ToString());
		if (!this.m_endPointSteamMapping.ContainsKey(ipendPoint))
		{
			this.m_endPointSteamMapping.Add(ipendPoint, id);
		}
		else
		{
			this.m_endPointSteamMapping[ipendPoint] = id;
		}
		return ipendPoint;
	}

	// Token: 0x06001F17 RID: 7959 RVA: 0x000C6D34 File Offset: 0x000C4F34
	private void RemoveEndpointMapping(CSteamID id)
	{
		Debug.Log("Removing endpoint mapping id = " + id.m_SteamID.ToString());
		IPEndPoint key = new IPEndPoint((long)id.m_SteamID, 12345);
		if (this.m_endPointSteamMapping.ContainsKey(key))
		{
			this.m_endPointSteamMapping.Remove(key);
		}
	}

	// Token: 0x06001F18 RID: 7960 RVA: 0x000C6D88 File Offset: 0x000C4F88
	public static void Create(bool server)
	{
		Debug.Log("Creating NetSteamRelay, server=" + server.ToString());
		if (NetSteamRelay.m_instance != null)
		{
			NetSteamRelay.Destroy();
		}
		GameObject gameObject = new GameObject("NetSteamRelay");
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		NetSteamRelay.m_instance = gameObject.AddComponent<NetSteamRelay>();
		NetSteamRelay.m_instance.InternalCreate(server);
	}

	// Token: 0x06001F19 RID: 7961 RVA: 0x000C6DE4 File Offset: 0x000C4FE4
	private void InternalCreate(bool server)
	{
		NetSteamRelay.m_isServer = server;
		if (!this.m_isInitialized)
		{
			if (SteamManager.Initialized)
			{
				this.m_p2pSessionRequest = Callback<P2PSessionRequest_t>.Create(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.OnP2PSessionRequest));
				this.m_p2pSessionConnectFail = Callback<P2PSessionConnectFail_t>.Create(new Callback<P2PSessionConnectFail_t>.DispatchDelegate(this.OnP2PSessionConnectFail));
			}
			else
			{
				Debug.LogError("NetSteamRelay : SteamManager not initialized");
			}
			this.m_isInitialized = true;
		}
	}

	// Token: 0x06001F1A RID: 7962 RVA: 0x00016E0C File Offset: 0x0001500C
	public static void Destroy()
	{
		if (NetSteamRelay.m_instance != null)
		{
			Debug.Log("NetSteamRelay : Destroy");
			NetSteamRelay.m_instance.InternalDestroy();
		}
	}

	// Token: 0x06001F1B RID: 7963 RVA: 0x000C6E48 File Offset: 0x000C5048
	private void InternalDestroy()
	{
		NetGameClient.SetRelayProvider(null);
		NetGameServer.SetRelayProvider(null);
		foreach (KeyValuePair<IPEndPoint, CSteamID> keyValuePair in this.m_endPointSteamMapping)
		{
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001F1C RID: 7964 RVA: 0x000C6EAC File Offset: 0x000C50AC
	public int SendData(byte[] data, int dataLen, IPEndPoint target, NetRelaySendType type)
	{
		CSteamID steamIDRemote;
		if (!this.m_endPointSteamMapping.TryGetValue(target, out steamIDRemote))
		{
			Debug.LogError("no steam id for net end point exists");
			return 0;
		}
		bool flag = SteamNetworking.SendP2PPacket(steamIDRemote, data, (uint)dataLen, this.GetSendType(type), 0);
		if (flag && this.m_gatherStatistics)
		{
			this.m_perFrame.sentMessages = this.m_perFrame.sentMessages + 1;
			this.m_perFrame.sentBytes = this.m_perFrame.sentBytes + dataLen;
		}
		if (flag)
		{
			return dataLen;
		}
		Debug.LogError("NetSteamRelay : SendData failure");
		return 0;
	}

	// Token: 0x06001F1D RID: 7965 RVA: 0x000C6F20 File Offset: 0x000C5120
	public int IsDataAvailable()
	{
		uint result = 0U;
		SteamNetworking.IsP2PPacketAvailable(out result, 0);
		return (int)result;
	}

	// Token: 0x06001F1E RID: 7966 RVA: 0x000C6F3C File Offset: 0x000C513C
	public int ReadData(byte[] readBuffer, int readBufferSize, ref EndPoint sender)
	{
		uint num = 0U;
		CSteamID csteamID;
		if (SteamNetworking.ReadP2PPacket(readBuffer, (uint)readBufferSize, out num, out csteamID, 0) && this.m_endPointSteamMapping.ContainsValue(csteamID))
		{
			if (this.m_gatherStatistics)
			{
				this.m_perFrame.recievedMessages = this.m_perFrame.recievedMessages + 1;
				this.m_perFrame.recievedBytes = this.m_perFrame.recievedBytes + (int)num;
			}
			foreach (KeyValuePair<IPEndPoint, CSteamID> keyValuePair in this.m_endPointSteamMapping)
			{
				if (keyValuePair.Value == csteamID)
				{
					sender = keyValuePair.Key;
					return (int)num;
				}
			}
			return 0;
		}
		return 0;
	}

	// Token: 0x06001F1F RID: 7967 RVA: 0x00016E2F File Offset: 0x0001502F
	public bool IsPeerUsingRelay(IPEndPoint sender)
	{
		return this.m_endPointSteamMapping.ContainsKey(sender);
	}

	// Token: 0x06001F20 RID: 7968 RVA: 0x00016E3D File Offset: 0x0001503D
	public bool IsClient()
	{
		return !NetSteamRelay.m_isServer;
	}

	// Token: 0x06001F21 RID: 7969 RVA: 0x00016E47 File Offset: 0x00015047
	public void Shutdown()
	{
		this.m_destroyQueued = true;
		Debug.Log("NetSteamRelay shutdown called");
	}

	// Token: 0x06001F22 RID: 7970 RVA: 0x000C6FF4 File Offset: 0x000C51F4
	private void InitDebug()
	{
		if (this.m_gatherStatistics)
		{
			Debug.Log("Creating Debug Relay UI");
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/UIRelayDebug"));
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			this.m_relayDebugUI = gameObject.GetComponent<UIRelayDebug>();
			this.m_nextStatTime = Time.time + 1f;
		}
	}

	// Token: 0x06001F23 RID: 7971 RVA: 0x00016E5A File Offset: 0x0001505A
	private void DestroyDebug()
	{
		if (this.m_relayDebugUI != null)
		{
			UnityEngine.Object.Destroy(this.m_relayDebugUI.gameObject);
		}
	}

	// Token: 0x06001F24 RID: 7972 RVA: 0x00016E7A File Offset: 0x0001507A
	private EP2PSend GetSendType(NetRelaySendType type)
	{
		if (type == NetRelaySendType.Reliable)
		{
			return EP2PSend.k_EP2PSendReliable;
		}
		if (type != NetRelaySendType.Unreliable)
		{
			return EP2PSend.k_EP2PSendUnreliable;
		}
		return EP2PSend.k_EP2PSendUnreliable;
	}

	// Token: 0x06001F25 RID: 7973 RVA: 0x000C7048 File Offset: 0x000C5248
	private string GetP2PErrorString(EP2PSessionError err)
	{
		switch (err)
		{
		case EP2PSessionError.k_EP2PSessionErrorNone:
			return "No Error";
		case EP2PSessionError.k_EP2PSessionErrorNotRunningApp:
			return "The target user is not running the same game.";
		case EP2PSessionError.k_EP2PSessionErrorNoRightsToApp:
			return "R1576";
		case EP2PSessionError.k_EP2PSessionErrorDestinationNotLoggedIn:
			return "Target user isn't connected to Steam.";
		case EP2PSessionError.k_EP2PSessionErrorTimeout:
			return "Connection timeout. Corporate firewalls can block, ensure UDP ports 3478, 4379, and 4380 are open in an outbound direction";
		default:
			return "Unknown";
		}
	}

	// Token: 0x06001F26 RID: 7974 RVA: 0x000C7094 File Offset: 0x000C5294
	public string GetStatusString()
	{
		int num = (int)(21f - (Time.time - this.startTime));
		switch (this.m_status)
		{
		case SteamRelayConnectionStatus.None:
			return "UNKNOWN";
		case SteamRelayConnectionStatus.StartingConnection:
			return "Starting Connection Process";
		case SteamRelayConnectionStatus.EstablishingSession:
			return "Establishing Session Please Wait ... " + num.ToString();
		case SteamRelayConnectionStatus.Handshaking:
			return "Handshaking " + num.ToString();
		case SteamRelayConnectionStatus.AttemptingConnection:
			return "Attempting to Connect";
		default:
			return "UNKNOWN";
		}
	}

	// Token: 0x06001F27 RID: 7975 RVA: 0x000C7114 File Offset: 0x000C5314
	private void DoStatistics()
	{
		if (this.m_gatherStatistics)
		{
			this.m_perSecondTemp.recievedMessages = this.m_perSecondTemp.recievedMessages + this.m_perFrame.recievedMessages;
			this.m_perSecondTemp.recievedBytes = this.m_perSecondTemp.recievedBytes + this.m_perFrame.recievedBytes;
			this.m_perSecondTemp.sentMessages = this.m_perSecondTemp.sentMessages + this.m_perFrame.sentMessages;
			this.m_perSecondTemp.sentBytes = this.m_perSecondTemp.sentBytes + this.m_perFrame.sentBytes;
			if (Time.time > this.m_nextStatTime)
			{
				this.m_totalData.recievedMessages = this.m_totalData.recievedMessages + this.m_perSecondTemp.recievedMessages;
				this.m_totalData.recievedBytes = this.m_totalData.recievedBytes + this.m_perSecondTemp.recievedBytes;
				this.m_totalData.sentMessages = this.m_totalData.sentMessages + this.m_perSecondTemp.sentMessages;
				this.m_totalData.sentBytes = this.m_totalData.sentBytes + this.m_perSecondTemp.sentBytes;
				this.m_maxData.recievedMessages = Mathf.Max(this.m_perSecondTemp.recievedMessages, this.m_maxData.recievedMessages);
				this.m_maxData.recievedBytes = Mathf.Max(this.m_perSecondTemp.recievedBytes, this.m_maxData.recievedBytes);
				this.m_maxData.sentMessages = Mathf.Max(this.m_perSecondTemp.sentMessages, this.m_maxData.sentMessages);
				this.m_maxData.sentBytes = Mathf.Max(this.m_perSecondTemp.sentBytes, this.m_maxData.sentBytes);
				this.m_perSecond.recievedMessages = this.m_perSecondTemp.recievedMessages;
				this.m_perSecond.recievedBytes = this.m_perSecondTemp.recievedBytes;
				this.m_perSecond.sentMessages = this.m_perSecondTemp.sentMessages;
				this.m_perSecond.sentBytes = this.m_perSecondTemp.sentBytes;
				this.m_perSecondTemp.recievedMessages = 0;
				this.m_perSecondTemp.recievedBytes = 0;
				this.m_perSecondTemp.sentMessages = 0;
				this.m_perSecondTemp.sentBytes = 0;
				this.m_nextStatTime = Time.time + 1f;
			}
			if (this.m_relayDebugUI != null)
			{
				this.m_relayDebugUI.SetText(this.GetDebugUIText());
			}
			this.m_perFrame.recievedMessages = 0;
			this.m_perFrame.recievedBytes = 0;
			this.m_perFrame.sentMessages = 0;
			this.m_perFrame.sentBytes = 0;
		}
	}

	// Token: 0x06001F28 RID: 7976 RVA: 0x000C738C File Offset: 0x000C558C
	private string GetDebugUIText()
	{
		return "" + "<color=#60C71AFF>OUT MSG/S\t\t" + this.m_perSecond.sentMessages.ToString() + "\n" + "OUT BYTES/S\t\t" + this.m_perSecond.sentBytes.ToString() + "\n" + "IN MSG/S\t\t" + this.m_perSecond.recievedMessages.ToString() + "\n" + "IN BYTES/S\t\t" + this.m_perSecond.recievedBytes.ToString() + "</color>\n" + "SESSIONS\t\t" + this.m_endPointSteamMapping.Count.ToString() + "\n" + "<color=#1AC7C7FF>OUT BYTES\t\t" + this.m_perFrame.sentBytes.ToString() + "\n" + "IN BYTES\t\t" + this.m_perFrame.recievedBytes.ToString() + "</color>\n";
	}

	// Token: 0x040021FB RID: 8699
	private const int m_dataChannel = 0;

	// Token: 0x040021FC RID: 8700
	private const int m_p2pConnectionChannel = 1;

	// Token: 0x040021FD RID: 8701
	private const float m_connectionAttemptTimeout = 21f;

	// Token: 0x040021FE RID: 8702
	private static NetSteamRelay m_instance;

	// Token: 0x040021FF RID: 8703
	private static bool m_isServer;

	// Token: 0x04002200 RID: 8704
	private static byte m_nextClientRequestID;

	// Token: 0x04002201 RID: 8705
	private bool m_isInitialized;

	// Token: 0x04002202 RID: 8706
	private bool m_connectionInProgress;

	// Token: 0x04002203 RID: 8707
	private bool m_p2pSessionFailed;

	// Token: 0x04002204 RID: 8708
	private bool m_handshakeFailed;

	// Token: 0x04002205 RID: 8709
	private bool m_connectFailed;

	// Token: 0x04002206 RID: 8710
	private string m_lastError = "";

	// Token: 0x04002207 RID: 8711
	private byte m_lastConnectRequestID;

	// Token: 0x04002208 RID: 8712
	private SteamRelayConnectionStatus m_status;

	// Token: 0x04002209 RID: 8713
	private float startTime;

	// Token: 0x0400220A RID: 8714
	private bool m_destroyQueued;

	// Token: 0x0400220B RID: 8715
	private bool m_gatherStatistics;

	// Token: 0x0400220C RID: 8716
	private NetRelayData m_perSecond;

	// Token: 0x0400220D RID: 8717
	private NetRelayData m_perSecondTemp;

	// Token: 0x0400220E RID: 8718
	private NetRelayData m_maxData;

	// Token: 0x0400220F RID: 8719
	private NetRelayData m_totalData;

	// Token: 0x04002210 RID: 8720
	private NetRelayData m_perFrame;

	// Token: 0x04002211 RID: 8721
	private float m_nextStatTime;

	// Token: 0x04002212 RID: 8722
	private UIRelayDebug m_relayDebugUI;

	// Token: 0x04002213 RID: 8723
	private Dictionary<IPEndPoint, CSteamID> m_endPointSteamMapping = new Dictionary<IPEndPoint, CSteamID>();

	// Token: 0x04002218 RID: 8728
	protected Callback<P2PSessionRequest_t> m_p2pSessionRequest;

	// Token: 0x04002219 RID: 8729
	protected Callback<P2PSessionConnectFail_t> m_p2pSessionConnectFail;

	// Token: 0x0400221A RID: 8730
	private readonly ulong mask = (ulong)-2;
}
