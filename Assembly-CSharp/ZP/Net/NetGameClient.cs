using System;
using System.Collections.Generic;
using System.Net;
using I2.Loc;
using Lidgren.Network;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZP.Utility;

namespace ZP.Net
{
	// Token: 0x02000603 RID: 1539
	public class NetGameClient
	{
		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06002823 RID: 10275 RVA: 0x0001C427 File Offset: 0x0001A627
		public static int ServerCount
		{
			get
			{
				return NetGameClient.server_list.Count;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06002824 RID: 10276 RVA: 0x0001C433 File Offset: 0x0001A633
		public static int NetEntityCount
		{
			get
			{
				return NetGameClient.net_object_manager.NetEntityCount;
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06002825 RID: 10277 RVA: 0x0001C43F File Offset: 0x0001A63F
		// (set) Token: 0x06002826 RID: 10278 RVA: 0x0001C446 File Offset: 0x0001A646
		public static int SendRate
		{
			get
			{
				return NetGameClient.send_rate;
			}
			set
			{
				NetGameClient.send_rate = value;
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06002827 RID: 10279 RVA: 0x0001C44E File Offset: 0x0001A64E
		public static bool WaitingForNatIntroductionSuccess
		{
			get
			{
				return NetGameClient.waitingForNatIntroSuccess;
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06002828 RID: 10280 RVA: 0x0001C455 File Offset: 0x0001A655
		public static NetObjectManager NetObjectManager
		{
			get
			{
				return NetGameClient.net_object_manager;
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06002829 RID: 10281 RVA: 0x000ED15C File Offset: 0x000EB35C
		// (remove) Token: 0x0600282A RID: 10282 RVA: 0x000ED190 File Offset: 0x000EB390
		public static event ServerListChangedEventHandler ServerListChanged;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600282B RID: 10283 RVA: 0x000ED1C4 File Offset: 0x000EB3C4
		// (remove) Token: 0x0600282C RID: 10284 RVA: 0x000ED1F8 File Offset: 0x000EB3F8
		public static event NatIntroductionSuccessEventHandler NatIntroductionSuccess;

		// Token: 0x0600282D RID: 10285 RVA: 0x0001C45C File Offset: 0x0001A65C
		public static void OnServerListChanged()
		{
			if (NetGameClient.ServerListChanged != null)
			{
				NetGameClient.ServerListChanged();
			}
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x0001C46F File Offset: 0x0001A66F
		public static void OnNatIntroductionSuccess(IPEndPoint endPoint)
		{
			if (NetGameClient.NatIntroductionSuccess != null)
			{
				NetGameClient.NatIntroductionSuccess(endPoint);
				return;
			}
			Debug.LogWarning("Nat introduction has no listeners");
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x000ED22C File Offset: 0x000EB42C
		public static void Initialize()
		{
			if (NetGameClient.initialized)
			{
				NetGameClient.Reset();
				return;
			}
			NetGameClient.initialized = true;
			NetGameClient.net_object_manager = new NetObjectManager();
			NetGameClient.net_object_manager.Initialize();
			NetGameClient.net_object_manager.InitiateNetSendObjects();
			NetGameClient.server = new NetPlayer(PlatformUtility.GetUsername(), 0, false, 0, null, null);
			NetGameClient.server.LastAck = -1;
			NetGameClient.server.LastSentSnapshot = -1;
			NetGameClient.is_loading = false;
			NetGameClient.bs = new ZPBitStream();
			NetGameClient.bs.Reserve(8096);
			NetSystem.SetSendRate(NetGameServer.SendRate, NetGameClient.send_rate);
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x000ED2C4 File Offset: 0x000EB4C4
		public static void Reset()
		{
			if (NetGameClient.net_object_manager != null)
			{
				NetGameClient.net_object_manager.Reset();
			}
			NetGameClient.server = new NetPlayer(PlatformUtility.GetUsername(), 0, false, 0, null, null);
			NetGameClient.server.LastAck = -1;
			NetGameClient.server.LastSentSnapshot = -1;
			if (NetGameClient.bs != null)
			{
				NetGameClient.bs.Clear();
			}
			NetSystem.SetSendRate(NetGameServer.SendRate, NetGameClient.send_rate);
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x0001C48E File Offset: 0x0001A68E
		public static void SetRelayProvider(INetRelay relay)
		{
			if (NetGameClient.net_client == null)
			{
				Debug.LogWarning("did not set relay provider net client is null");
				return;
			}
			NetGameClient.net_client.SetRelayProvider(relay);
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x000ED32C File Offset: 0x000EB52C
		public static string Start(int port)
		{
			Debug.LogError("NetGameClient.Start called " + Time.time.ToString());
			port = 14300;
			if (NetSystem.IsConnected || NetSystem.IsInitialized)
			{
				Debug.LogError("Destroying existing connection " + Time.time.ToString());
				NetSystem.Destroy();
			}
			if (!NetSystem.IsInitialized)
			{
				NetGameClient.server_list = new List<ServerData>();
				NetGameClient.server_map = new Dictionary<IPEndPoint, ServerData>();
				NetSystem.Create(false);
				try
				{
					NetPeerConfiguration configuration = NetGameClient.GetConfiguration(port);
					if (configuration == null)
					{
						return "Failed to setup NetPeerConfiguration";
					}
					configuration.MaximumTransmissionUnit = 1190;
					configuration.AutoExpandMTU = false;
					NetGameClient.net_client = new NetClient(configuration);
					NetGameClient.net_client.Start();
				}
				catch (Exception ex)
				{
					int num = 1024;
					for (int i = 1; i < num; i++)
					{
						NetPeerConfiguration configuration2 = NetGameClient.GetConfiguration(port + i);
						if (configuration2 == null)
						{
							return "Failed to setup NetPeerConfiguration";
						}
						configuration2.MaximumTransmissionUnit = 1190;
						configuration2.AutoExpandMTU = false;
						try
						{
							Debug.LogError("Failed to connect on port " + port.ToString() + " : " + ex.Message);
							NetGameClient.net_client = new NetClient(configuration2);
							NetGameClient.net_client.Start();
							break;
						}
						catch (Exception)
						{
							Debug.LogError("Client failed to start on port " + i.ToString() + " attempting next port");
						}
					}
				}
				Debug.LogError("Setting NetSystem.IsInitialized to true " + Time.time.ToString());
				NetSystem.IsInitialized = true;
			}
			return "";
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x000ED4DC File Offset: 0x000EB6DC
		private static NetPeerConfiguration GetConfiguration(int port)
		{
			NetPeerConfiguration netPeerConfiguration = null;
			try
			{
				netPeerConfiguration = new NetPeerConfiguration(NetSystem.AppID);
			}
			catch (Exception)
			{
				return null;
			}
			netPeerConfiguration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
			netPeerConfiguration.EnableMessageType(NetIncomingMessageType.Error);
			netPeerConfiguration.EnableMessageType(NetIncomingMessageType.ErrorMessage);
			netPeerConfiguration.EnableMessageType(NetIncomingMessageType.WarningMessage);
			netPeerConfiguration.EnableMessageType(NetIncomingMessageType.UnconnectedData);
			netPeerConfiguration.EnableMessageType(NetIncomingMessageType.NatIntroductionSuccess);
			netPeerConfiguration.DisableMessageType(NetIncomingMessageType.VerboseDebugMessage);
			netPeerConfiguration.DisableMessageType(NetIncomingMessageType.DebugMessage);
			netPeerConfiguration.SuppressUnreliableUnorderedAcks = false;
			netPeerConfiguration.AutoFlushSendQueue = false;
			netPeerConfiguration.Port = port;
			netPeerConfiguration.ConnectionTimeout = 61f;
			return netPeerConfiguration;
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x000ED580 File Offset: 0x000EB780
		public static void Stop(string msg)
		{
			if (NetSystem.IsConnected && !NetSystem.IsServer)
			{
				NetGameClient.Disconnect(msg);
			}
			if (NetSystem.IsInitialized)
			{
				NetGameClient.send_rate = 30;
				NetGameClient.server_list.Clear();
				NetGameClient.server_map.Clear();
				NetGameClient.net_client.Shutdown("sd");
				NetGameClient.net_client = null;
				NetGameClient.net_object_manager.Destroy();
				NetGameClient.time_passed = 0f;
				NetGameClient.last_send_time = 0f;
				NetGameClient.server = null;
				NetGameClient.is_loading = false;
				NetGameClient.waitingForNatIntroSuccess = false;
				NetGameClient.next_snapshot_send = NetTime.Now;
				NetGameClient.snapshot_sent_timer = 0f;
				NetGameClient.snapshot_sent_count = 0;
				NetGameClient.byte_count = 0;
				NetGameClient.byte_timer = 0f;
				NetGameClient.bufferedMessages.Clear();
				NetGameClient.snapshot_count = 0;
				NetGameClient.snapshot_timer = Time.time;
				NetSystem.IsInitialized = false;
			}
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x0001C4AD File Offset: 0x0001A6AD
		public static void Update()
		{
			NetGameClient.net_object_manager.UpdateTick();
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x0001C4B9 File Offset: 0x0001A6B9
		public static bool Connect(string host, int port)
		{
			return NetGameClient.Connect(NetUtility.Resolve(host, port));
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x000ED658 File Offset: 0x000EB858
		public static bool Connect(IPEndPoint endPoint)
		{
			Debug.LogError("NetGameClient.Connect" + Time.time.ToString());
			NetSystem.SetLastConnectionError("EMPTY");
			if (NetSystem.IsServer)
			{
				Debug.LogError("Cannot connect with client while server is running! " + Time.time.ToString());
				NetSystem.OnConnectFailed(LocalizationManager.GetTranslation("AlreadyHosting", true, 0, true, false, null, null, true));
				return false;
			}
			if (!NetSystem.IsInitialized)
			{
				Debug.LogError("Must call Start on NetGameClient before connecting! " + Time.time.ToString());
				NetSystem.OnConnectFailed(LocalizationManager.GetTranslation("NetworkNotStarted", true, 0, true, false, null, null, true));
				return false;
			}
			Debug.LogWarning("ATTEMPTING CONNECTION! " + Time.time.ToString() + " : " + endPoint.ToString());
			try
			{
				NetOutgoingMessage netOutgoingMessage = NetGameClient.net_client.CreateMessage();
				netOutgoingMessage.Write(GameManager.VERSION);
				netOutgoingMessage.Write(NetSystem.MyPlayer.Name);
				NetSystem.MyPlayer.Connection = NetGameClient.net_client.Connect(endPoint, netOutgoingMessage);
			}
			catch (Exception ex)
			{
				NetSystem.OnConnectFailed(LocalizationManager.GetTranslation("UnableToConnect", true, 0, true, false, null, null, true) + " : " + ex.Message);
				return false;
			}
			if (NetSystem.MyPlayer.Connection != null)
			{
				NetSystem.SetStatus(NetSystemStatus.Connecting);
				return true;
			}
			NetSystem.OnConnectFailed(LocalizationManager.GetTranslation("UnableToConnect", true, 0, true, false, null, null, true));
			return false;
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x0001C4C7 File Offset: 0x0001A6C7
		public static void Disconnect(string msg)
		{
			if (NetSystem.IsConnected && !NetSystem.IsServer)
			{
				NetGameClient.net_object_manager.Destroy();
			}
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x0001C4E1 File Offset: 0x0001A6E1
		public static void RequestNatIntroduction(string address, int port, bool first)
		{
			NetGameClient.RequestNatIntroduction(true, address, port, first, 0UL);
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x0001C4EE File Offset: 0x0001A6EE
		public static void RequestNatIntroduction(ulong uniqueIdentifier, bool first)
		{
			NetGameClient.RequestNatIntroduction(false, "", NetGameClient.net_client.Port, first, uniqueIdentifier);
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x000ED7D0 File Offset: 0x000EB9D0
		private static void RequestNatIntroduction(bool directConnect, string address, int port, bool first, ulong uniqueIdentifier = 0UL)
		{
			if (first)
			{
				NetSystem.SetStatus(NetSystemStatus.Connecting);
			}
			if (NetGameClient.net_client == null || !NetSystem.IsInitialized)
			{
				Debug.LogError("Nat Introduction Error : net system not initialized has NetGameClient.Start been called?");
				return;
			}
			if (NetSystem.MASTER_SERVERS == null)
			{
				throw new Exception("unable to complete nat introduction, master server is null");
			}
			Debug.Log("Unique Idenitifer: " + uniqueIdentifier.ToString());
			Debug.Log("intro client port = " + NetGameClient.net_client.Port.ToString());
			if (first)
			{
				NetGameClient.waitingForNatIntroSuccess = true;
			}
			foreach (IPEndPoint recipient in NetSystem.MASTER_SERVERS)
			{
				NetOutgoingMessage netOutgoingMessage = NetGameClient.net_client.CreateMessage();
				netOutgoingMessage.Write(0);
				List<IPAddress> allLocalIPAddresses = NetSystem.GetAllLocalIPAddresses(true);
				netOutgoingMessage.Write((ushort)allLocalIPAddresses.Count);
				foreach (IPAddress ipaddress in allLocalIPAddresses)
				{
					Debug.Log("Sending local IP [" + ipaddress.ToString() + "] in nat introduction request.");
					netOutgoingMessage.Write(new IPEndPoint(ipaddress, NetGameClient.net_client.Port));
				}
				netOutgoingMessage.Write(directConnect);
				if (directConnect)
				{
					netOutgoingMessage.Write(NetUtility.Resolve(address, port));
				}
				else
				{
					netOutgoingMessage.Write(uniqueIdentifier);
				}
				netOutgoingMessage.Write("mytoken");
				NetGameClient.net_client.SendUnconnectedMessage(netOutgoingMessage, recipient);
			}
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x000ED960 File Offset: 0x000EBB60
		public static void FinishedLoading()
		{
			if (NetGameClient.is_loading && NetSystem.IsConnected)
			{
				NetGameClient.is_loading = false;
				NetOutgoingMessage netOutgoingMessage = NetGameClient.net_client.CreateMessage();
				NetGameClient.WriteMessageType(netOutgoingMessage, ClientMessageType.FINISHED_LOADING);
				NetGameClient.net_client.SendMessage(netOutgoingMessage, NetDeliveryMethod.ReliableOrdered);
			}
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x000ED9A4 File Offset: 0x000EBBA4
		public static void SendRPC(INetComponent net_entity, string method_name, NetDeliveryMethod delivery_method, object[] parameters)
		{
			NetOutgoingMessage msg = NetGameClient.net_client.CreateMessage();
			NetRPCDefinition rpcdefinition = NetGameClient.net_object_manager.GetRPCDefinition(net_entity, method_name);
			if (NetGameClient.net_object_manager.CreateRPCPacket(ref msg, rpcdefinition, net_entity, parameters))
			{
				NetGameClient.net_client.SendMessage(msg, delivery_method);
			}
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x000ED9E8 File Offset: 0x000EBBE8
		public static void WriteStream(int stream_index, NetDeliveryMethod delivery, byte[] data)
		{
			NetOutgoingMessage netOutgoingMessage = NetGameClient.net_client.CreateMessage();
			switch (stream_index)
			{
			case 0:
				netOutgoingMessage.Write(14);
				break;
			case 1:
				netOutgoingMessage.Write(16);
				break;
			case 2:
				netOutgoingMessage.Write(18);
				break;
			case 3:
				netOutgoingMessage.Write(20);
				break;
			}
			netOutgoingMessage.Write(data);
			NetGameClient.net_client.SendMessage(netOutgoingMessage, delivery);
		}

		// Token: 0x0600283F RID: 10303 RVA: 0x000EDA54 File Offset: 0x000EBC54
		public static void SendLobbyChatMessage(string msg)
		{
			NetOutgoingMessage netOutgoingMessage = NetGameClient.net_client.CreateMessage(8);
			NetGameClient.WriteMessageType(netOutgoingMessage, ClientMessageType.LOBBY_CHAT_MSG);
			netOutgoingMessage.Write(msg);
			NetGameClient.net_client.SendMessage(netOutgoingMessage, NetDeliveryMethod.ReliableOrdered);
		}

		// Token: 0x06002840 RID: 10304 RVA: 0x000EDA8C File Offset: 0x000EBC8C
		public static void SendLobbySlotChange(ushort slot, bool observer)
		{
			NetOutgoingMessage netOutgoingMessage = NetGameClient.net_client.CreateMessage(4);
			NetGameClient.WriteMessageType(netOutgoingMessage, ClientMessageType.REQUEST_SLOT_CHANGE);
			netOutgoingMessage.Write(slot);
			netOutgoingMessage.Write(observer);
			NetGameClient.net_client.SendMessage(netOutgoingMessage, NetDeliveryMethod.ReliableOrdered);
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x0001C507 File Offset: 0x0001A707
		public static ServerData GetServer(int index)
		{
			return NetGameClient.server_list[index];
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x000EDACC File Offset: 0x000EBCCC
		private static bool IsLocalAddress(string ipAddress)
		{
			if (ipAddress == "")
			{
				Debug.LogError("IsLocalAddress - address is empty");
				return false;
			}
			string[] array = ipAddress.Split(new string[]
			{
				"."
			}, StringSplitOptions.RemoveEmptyEntries);
			List<int> list = new List<int>();
			foreach (string s in array)
			{
				list.Add(int.Parse(s));
			}
			return list[0] == 10 || (list[0] == 192 && list[1] == 168) || (list[0] == 172 && list[1] >= 16 && list[1] <= 31);
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x000EDB7C File Offset: 0x000EBD7C
		public static void Send()
		{
			NetGameClient.time_passed += Time.deltaTime;
			if (NetTime.Now >= NetGameClient.next_snapshot_send)
			{
				NetGameClient.next_snapshot_send += 1.0 / (double)NetGameClient.send_rate;
				if (NetTime.Now >= NetGameClient.next_snapshot_send)
				{
					NetGameClient.next_snapshot_send = NetTime.Now;
				}
				NetGameClient.bs.Reset();
				bool flag = true;
				bool flag2;
				if ((NetSystem.CurrentTick - NetGameClient.server.LastAck >= NetGameClient.send_rate * 2 && NetGameClient.server.LastAck < NetGameClient.server.LastSentSnapshot) || NetGameClient.server.LastAck == -1)
				{
					NetGameClient.net_object_manager.CreateFullSnapshotClient(NetGameClient.server, ref NetGameClient.bs);
					flag2 = true;
					NetGameClient.server.LastAck = NetSystem.CurrentTick;
				}
				else
				{
					flag = NetGameClient.net_object_manager.CreateDeltaSnapshot(NetGameClient.server, ref NetGameClient.bs);
					flag2 = false;
				}
				if (NetGameClient.bs.GetByteLength() >= 1 && flag)
				{
					NetOutgoingMessage netOutgoingMessage = NetGameClient.net_client.CreateMessage(NetGameClient.bs.GetByteLength() + 5);
					if (flag2)
					{
						NetGameClient.WriteMessageType(netOutgoingMessage, ClientMessageType.SNAPSHOT_FULL);
					}
					else
					{
						NetGameClient.WriteMessageType(netOutgoingMessage, ClientMessageType.SNAPSHOT_DELTA);
					}
					netOutgoingMessage.Write(NetSystem.CurrentTick, 24);
					netOutgoingMessage.Write(NetGameClient.bs.GetBuffer(), 0, NetGameClient.bs.GetByteLength());
					NetGameClient.net_client.SendMessage(netOutgoingMessage, NetDeliveryMethod.Unreliable);
					NetGameClient.snapshot_sent_count++;
					NetGameClient.server.LastSentSnapshot = NetSystem.CurrentTick;
				}
				NetSystem.IncrementTick();
			}
			NetGameClient.snapshot_sent_timer += Time.deltaTime;
			if (NetGameClient.snapshot_sent_timer >= 1f)
			{
				DebugUtility.SetText("SnapshotsSent", "snapshots_sent : " + NetGameClient.snapshot_sent_count.ToString(), new Vector2(10f, 45f));
				NetGameClient.snapshot_sent_count = 0;
				NetGameClient.snapshot_sent_timer -= 1f;
			}
			NetGameClient.net_client.FlushSendQueue();
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x000EDD60 File Offset: 0x000EBF60
		public static void Recieve()
		{
			if (NetGameClient.net_client == null)
			{
				return;
			}
			NetGameClient.snapshot_timer += Time.deltaTime;
			if (NetGameClient.snapshot_timer >= 1f)
			{
				DebugUtility.SetText("Snapshots", "snapshots_recieved : " + NetGameClient.snapshot_count.ToString(), new Vector2(10f, 65f));
				NetGameClient.snapshot_timer -= 1f;
				NetGameClient.snapshot_count = 0;
			}
			NetGameClient.byte_timer += Time.deltaTime;
			if (NetGameClient.byte_timer >= 1f)
			{
				DebugUtility.SetText("kbs", "b/s : " + NetGameClient.byte_count.ToString(), new Vector2(10f, 85f));
				NetGameClient.byte_timer -= 1f;
				NetGameClient.byte_count = 0;
			}
			NetGameClient.net_client.ReadMessages(NetGameClient.bufferedMessages);
			for (int i = 0; i < NetGameClient.bufferedMessages.Count; i++)
			{
				NetIncomingMessage netIncomingMessage = NetGameClient.bufferedMessages[i];
				if (NetSystem.IsConnected || NetSystem.IsConnecting || netIncomingMessage.MessageType == NetIncomingMessageType.UnconnectedData || netIncomingMessage.MessageType == NetIncomingMessageType.NatIntroductionSuccess || netIncomingMessage.MessageType == NetIncomingMessageType.StatusChanged)
				{
					NetGameClient.byte_count += netIncomingMessage.LengthBytes;
					NetIncomingMessageType messageType = netIncomingMessage.MessageType;
					if (messageType <= NetIncomingMessageType.DiscoveryResponse)
					{
						if (messageType <= NetIncomingMessageType.UnconnectedData)
						{
							if (messageType != NetIncomingMessageType.StatusChanged)
							{
								if (messageType != NetIncomingMessageType.UnconnectedData)
								{
									goto IL_405;
								}
							}
							else
							{
								NetConnectionStatus netConnectionStatus = (NetConnectionStatus)netIncomingMessage.ReadByte();
								Debug.Log("Status Change = " + netConnectionStatus.ToString());
								switch (netConnectionStatus)
								{
								case NetConnectionStatus.Connected:
									NetSystem.OnConnectedToLobby();
									NetSystem.SetStatus(NetSystemStatus.Connected);
									break;
								case NetConnectionStatus.Disconnected:
								{
									string text = netIncomingMessage.ReadString();
									string text2;
									if (LocalizationManager.TryGetTranslation(text, out text2, true, 0, true, false, null, null, true))
									{
										text = text2;
									}
									NetSystem.OnDisconnect(text);
									Debug.Log("Disconnected from server - Reason : " + text);
									break;
								}
								}
							}
						}
						else if (messageType != NetIncomingMessageType.Data)
						{
							if (messageType != NetIncomingMessageType.DiscoveryResponse)
							{
								goto IL_405;
							}
							Debug.LogWarning("Recieved discovery response!");
							ServerBroadcastInfo serverBroadcastInfo = new ServerBroadcastInfo("NO_NAME", 0, 0, 0);
							netIncomingMessage.ReadAllFields(serverBroadcastInfo);
							ServerData serverData = new ServerData(serverBroadcastInfo.server_name, (int)serverBroadcastInfo.players, (int)serverBroadcastInfo.observers, (int)serverBroadcastInfo.game_type, netIncomingMessage.SenderEndPoint.Address.ToString());
							if (!NetGameClient.server_map.ContainsKey(netIncomingMessage.SenderEndPoint))
							{
								NetGameClient.server_list.Add(serverData);
								NetGameClient.server_map.Add(netIncomingMessage.SenderEndPoint, serverData);
								NetGameClient.OnServerListChanged();
							}
						}
						else
						{
							NetGameClient.ReadMessage(netIncomingMessage);
						}
					}
					else if (messageType <= NetIncomingMessageType.DebugMessage)
					{
						if (messageType != NetIncomingMessageType.VerboseDebugMessage)
						{
							if (messageType != NetIncomingMessageType.DebugMessage)
							{
								goto IL_405;
							}
							string text3 = netIncomingMessage.ReadString();
							Debug.Log("LidDbg : " + text3);
						}
						else
						{
							string text3 = netIncomingMessage.ReadString();
							Debug.LogWarning("LidVerb : " + text3);
						}
					}
					else if (messageType != NetIncomingMessageType.WarningMessage)
					{
						if (messageType != NetIncomingMessageType.ErrorMessage)
						{
							if (messageType != NetIncomingMessageType.NatIntroductionSuccess)
							{
								goto IL_405;
							}
							string str = netIncomingMessage.ReadString();
							if (NetGameClient.waitingForNatIntroSuccess)
							{
								List<IPAddress> allLocalIPAddresses = NetSystem.GetAllLocalIPAddresses(true);
								bool flag = false;
								Debug.Log(netIncomingMessage.SenderEndPoint.Address);
								foreach (IPAddress ipaddress in allLocalIPAddresses)
								{
									Debug.Log(ipaddress.ToString());
									if (ipaddress.ToString() == netIncomingMessage.SenderEndPoint.Address.ToString())
									{
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									NetGameClient.waitingForNatIntroSuccess = false;
									string str2 = "Nat introduction success to ";
									IPEndPoint senderEndPoint = netIncomingMessage.SenderEndPoint;
									Debug.Log(str2 + ((senderEndPoint != null) ? senderEndPoint.ToString() : null) + " token is: " + str);
									NetGameClient.OnNatIntroductionSuccess(netIncomingMessage.SenderEndPoint);
								}
							}
						}
						else
						{
							string text3 = netIncomingMessage.ReadString();
							Debug.LogError(text3);
						}
					}
					else
					{
						string text3 = netIncomingMessage.ReadString();
					}
					IL_489:
					if (NetGameClient.net_client != null)
					{
						NetGameClient.net_client.Recycle(netIncomingMessage);
						NetGameClient.bufferedMessages[i] = null;
						goto IL_4A7;
					}
					break;
					IL_405:
					Debug.LogWarning(string.Concat(new string[]
					{
						"Unhandled type: ",
						netIncomingMessage.MessageType.ToString(),
						" ",
						netIncomingMessage.LengthBytes.ToString(),
						" bytes ",
						netIncomingMessage.DeliveryMethod.ToString(),
						"|",
						netIncomingMessage.SequenceChannel.ToString()
					}));
					goto IL_489;
				}
				IL_4A7:;
			}
			for (int j = NetGameClient.bufferedMessages.Count - 1; j >= 0; j--)
			{
				if (NetGameClient.bufferedMessages[j] == null)
				{
					NetGameClient.bufferedMessages.RemoveAt(j);
				}
			}
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x0001C514 File Offset: 0x0001A714
		private static void WriteMessageType(NetOutgoingMessage m, ClientMessageType msg_type)
		{
			if (msg_type == ClientMessageType.SNAPSHOT_FULL || msg_type == ClientMessageType.SNAPSHOT_DELTA)
			{
				m.Write(true);
				m.Write(msg_type == ClientMessageType.SNAPSHOT_FULL);
				return;
			}
			m.Write((byte)msg_type);
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x000EE270 File Offset: 0x000EC470
		private static void ReadMessage(NetIncomingMessage inc)
		{
			if ((inc.PeekByte() & 1) > 0)
			{
				NetGameClient.snapshot_count++;
				inc.ReadBoolean();
				bool full_snapshot = inc.ReadBoolean();
				int lastAck = NetGameClient.server.LastAck;
				int source = NetGameClient.net_object_manager.ReadSnapshot(inc, full_snapshot, ref lastAck);
				NetGameClient.server.LastAck = ((lastAck > NetGameClient.server.LastAck) ? lastAck : NetGameClient.server.LastAck);
				NetOutgoingMessage netOutgoingMessage = NetGameClient.net_client.CreateMessage(6);
				NetGameClient.WriteMessageType(netOutgoingMessage, ClientMessageType.SNAPSHOT_ACK);
				netOutgoingMessage.Write(source);
				NetGameClient.net_client.SendMessage(netOutgoingMessage, NetDeliveryMethod.Unreliable);
				return;
			}
			ServerMessageType serverMessageType = (ServerMessageType)inc.ReadByte();
			switch (serverMessageType)
			{
			case ServerMessageType.HEARTBEAT:
			{
				int source2 = inc.ReadInt32(24);
				if (inc.ReadBoolean())
				{
					int num = inc.ReadInt32(24);
					NetGameClient.server.LastAck = ((num > NetGameClient.server.LastAck) ? num : NetGameClient.server.LastAck);
				}
				if (inc.ReadBoolean())
				{
					float remote_time = inc.ReadFloat();
					NetSystem.NetTime.UpdateOffset(remote_time, NetSystem.MyPlayer.Connection.AverageRoundtripTime);
				}
				NetOutgoingMessage netOutgoingMessage2 = NetGameClient.net_client.CreateMessage(5);
				NetGameClient.WriteMessageType(netOutgoingMessage2, ClientMessageType.SNAPSHOT_ACK);
				netOutgoingMessage2.Write(source2);
				NetGameClient.net_client.SendMessage(netOutgoingMessage2, NetDeliveryMethod.Unreliable);
				NetGameClient.snapshot_count++;
				return;
			}
			case (ServerMessageType)3:
			case (ServerMessageType)5:
			case (ServerMessageType)7:
			case (ServerMessageType)9:
			case (ServerMessageType)11:
			case (ServerMessageType)13:
			case (ServerMessageType)15:
			case (ServerMessageType)17:
				break;
			case ServerMessageType.NETENTITY_CHANGES:
				NetGameClient.net_object_manager.ReadNetEntityChangesMessage(inc);
				return;
			case ServerMessageType.SEND_RPC:
			{
				NetPlayer player = NetSystem.GetPlayer(0);
				byte[] array = new byte[inc.LengthBytes - 1];
				inc.ReadBytes(array, 0, inc.LengthBytes - 1);
				NetGameClient.net_object_manager.InvokeRPC(player, array, array.Length * 8, false);
				return;
			}
			case ServerMessageType.LOAD_LEVEL:
			{
				string sceneName = inc.ReadString();
				NetGameClient.is_loading = true;
				SceneManager.LoadScene(sceneName);
				return;
			}
			case ServerMessageType.CHAT_SERVER_MSG:
			{
				ushort uid = inc.ReadUInt16();
				string msg = inc.ReadString();
				NetSystem.OnChatMessageRecieved(NetSystem.GetPlayer(uid), msg);
				return;
			}
			case ServerMessageType.PLAYER_CONNECTED:
			{
				ushort num2 = inc.ReadUInt16();
				byte slot = inc.ReadByte();
				bool observer = inc.ReadBoolean();
				NetSystem.AddPlayer(inc.ReadString(), num2, slot, observer);
				return;
			}
			case ServerMessageType.PLAYER_DISCONNECTED:
			{
				ushort num2 = inc.ReadUInt16();
				NetSystem.RemovePlayer(NetSystem.GetPlayer(num2));
				return;
			}
			case ServerMessageType.CONNECTION_APPROVED:
			{
				inc.ReadByte();
				NetSystem.SetGameMode((int)inc.ReadByte());
				byte slot = inc.ReadByte();
				bool observer = inc.ReadBoolean();
				ushort num2 = inc.ReadUInt16();
				NetSystem.MyPlayer.UserID = num2;
				NetSystem.MyPlayer.Slot = (int)slot;
				NetSystem.MyPlayer.Observer = observer;
				NetSystem.SetPlayer(NetSystem.MyPlayer);
				ushort num3 = inc.ReadUInt16();
				for (int i = 0; i < (int)num3; i++)
				{
					num2 = inc.ReadUInt16();
					slot = inc.ReadByte();
					observer = inc.ReadBoolean();
					NetSystem.SetPlayer(new NetPlayer(inc.ReadString(), (int)slot, observer, num2, null, null));
				}
				NetSystem.OnConnectedToLobby();
				return;
			}
			case ServerMessageType.SLOTS_CHANGED:
			{
				ushort num4 = inc.ReadUInt16();
				for (int j = 0; j < (int)num4; j++)
				{
					ushort num2 = inc.ReadUInt16();
					byte slot = inc.ReadByte();
					NetSystem.SetPlayerSlot(NetSystem.GetPlayer(num2), (int)slot, false);
				}
				ushort num5 = inc.ReadUInt16();
				for (int k = 0; k < (int)num5; k++)
				{
					ushort num2 = inc.ReadUInt16();
					byte slot = inc.ReadByte();
					NetSystem.SetPlayerSlot(NetSystem.GetPlayer(num2), (int)slot, true);
				}
				NetSystem.OnSlotsChanged();
				return;
			}
			default:
				switch (serverMessageType)
				{
				case ServerMessageType.STREAM_0:
					NetSystem.ReadStream(0, inc);
					return;
				case ServerMessageType.STREAM_1:
					NetSystem.ReadStream(1, inc);
					return;
				case ServerMessageType.STREAM_2:
					NetSystem.ReadStream(2, inc);
					return;
				case ServerMessageType.STREAM_3:
					NetSystem.ReadStream(3, inc);
					return;
				}
				break;
			}
			Debug.LogWarning("Unhandled GameMessageType : " + serverMessageType.ToString());
		}

		// Token: 0x04002AD5 RID: 10965
		private static int send_rate = 30;

		// Token: 0x04002AD6 RID: 10966
		private static List<ServerData> server_list;

		// Token: 0x04002AD7 RID: 10967
		private static Dictionary<IPEndPoint, ServerData> server_map;

		// Token: 0x04002AD8 RID: 10968
		private static NetClient net_client;

		// Token: 0x04002AD9 RID: 10969
		private static NetObjectManager net_object_manager;

		// Token: 0x04002ADA RID: 10970
		private static float time_passed;

		// Token: 0x04002ADB RID: 10971
		private static float last_send_time;

		// Token: 0x04002ADC RID: 10972
		private static NetPlayer server;

		// Token: 0x04002ADD RID: 10973
		private static bool is_loading;

		// Token: 0x04002ADE RID: 10974
		private static ZPBitStream bs;

		// Token: 0x04002ADF RID: 10975
		private static bool waitingForNatIntroSuccess = false;

		// Token: 0x04002AE0 RID: 10976
		private static double next_snapshot_send = NetTime.Now;

		// Token: 0x04002AE1 RID: 10977
		private static float snapshot_sent_timer = 0f;

		// Token: 0x04002AE2 RID: 10978
		private static int snapshot_sent_count = 0;

		// Token: 0x04002AE3 RID: 10979
		private static int byte_count = 0;

		// Token: 0x04002AE4 RID: 10980
		private static float byte_timer = 0f;

		// Token: 0x04002AE5 RID: 10981
		private static List<NetIncomingMessage> bufferedMessages = new List<NetIncomingMessage>();

		// Token: 0x04002AE6 RID: 10982
		private static int snapshot_count = 0;

		// Token: 0x04002AE7 RID: 10983
		private static float snapshot_timer = Time.time;

		// Token: 0x04002AEA RID: 10986
		private static bool initialized = false;
	}
}
