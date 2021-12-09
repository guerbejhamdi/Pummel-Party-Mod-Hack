using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Lidgren.Network;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using ZP.Utility;

namespace ZP.Net
{
	// Token: 0x02000609 RID: 1545
	public class NetGameServer
	{
		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x0600284B RID: 10315 RVA: 0x0001C589 File Offset: 0x0001A789
		public static bool IsDedicated
		{
			get
			{
				return NetGameServer.is_dedicated;
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x0600284C RID: 10316 RVA: 0x0001C590 File Offset: 0x0001A790
		public static bool IsRunning
		{
			get
			{
				return NetGameServer.is_running;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x0600284D RID: 10317 RVA: 0x0001C597 File Offset: 0x0001A797
		// (set) Token: 0x0600284E RID: 10318 RVA: 0x0001C59E File Offset: 0x0001A79E
		public static string ServerName
		{
			get
			{
				return NetGameServer.server_name;
			}
			set
			{
				NetGameServer.server_name = value;
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x0600284F RID: 10319 RVA: 0x0001C5A6 File Offset: 0x0001A7A6
		// (set) Token: 0x06002850 RID: 10320 RVA: 0x0001C5AD File Offset: 0x0001A7AD
		public static int MaxConnections
		{
			get
			{
				return NetGameServer.max_connections;
			}
			set
			{
				NetGameServer.max_connections = value;
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06002851 RID: 10321 RVA: 0x0001C5B5 File Offset: 0x0001A7B5
		// (set) Token: 0x06002852 RID: 10322 RVA: 0x0001C5BC File Offset: 0x0001A7BC
		public static int SendRate
		{
			get
			{
				return NetGameServer.send_rate;
			}
			set
			{
				NetGameServer.send_rate = value;
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06002853 RID: 10323 RVA: 0x0001C5C4 File Offset: 0x0001A7C4
		public static ServerStatus Status
		{
			get
			{
				return NetGameServer.server_status;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06002854 RID: 10324 RVA: 0x0001C5CB File Offset: 0x0001A7CB
		public static int Port
		{
			get
			{
				return NetGameServer.server_port;
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06002855 RID: 10325 RVA: 0x0001C5D2 File Offset: 0x0001A7D2
		public static int NetEntityCount
		{
			get
			{
				return NetGameServer.net_object_manager.NetEntityCount;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06002856 RID: 10326 RVA: 0x0001C5DE File Offset: 0x0001A7DE
		public static NetServer NetServer
		{
			get
			{
				return NetGameServer.net_server;
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06002857 RID: 10327 RVA: 0x0001C5E5 File Offset: 0x0001A7E5
		public static NetObjectManager NetObjectManager
		{
			get
			{
				return NetGameServer.net_object_manager;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06002858 RID: 10328 RVA: 0x0001C5EC File Offset: 0x0001A7EC
		// (set) Token: 0x06002859 RID: 10329 RVA: 0x0001C5F3 File Offset: 0x0001A7F3
		public static bool IsLocal { get; private set; }

		// Token: 0x0600285A RID: 10330 RVA: 0x0001C5FB File Offset: 0x0001A7FB
		public static NetPeerStatus GetStatus()
		{
			if (NetGameServer.net_server == null)
			{
				return NetPeerStatus.NotRunning;
			}
			return NetGameServer.net_server.Status;
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x0001C610 File Offset: 0x0001A810
		public static List<NetConnection> GetConnections()
		{
			if (NetGameServer.net_server == null)
			{
				return null;
			}
			return NetGameServer.net_server.Connections;
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x000EE69C File Offset: 0x000EC89C
		public static void Initialize()
		{
			if (NetGameServer.initialized)
			{
				NetGameServer.Reset();
				return;
			}
			NetGameServer.initialized = true;
			NetGameServer.net_object_manager = new NetObjectManager();
			NetGameServer.net_object_manager.Initialize();
			NetGameServer.net_object_manager.InitiateNetSendObjects();
			NetGameServer.bs = new ZPBitStream();
			NetGameServer.bs.Reserve(8096);
			NetGameServer.server_name = PlatformUtility.GetUsername() + "'s Game";
			NetSystem.SetSendRate(NetGameServer.send_rate, NetGameClient.SendRate);
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x0001C625 File Offset: 0x0001A825
		public static void Reset()
		{
			if (NetGameServer.net_object_manager != null)
			{
				NetGameServer.net_object_manager.Reset();
			}
			if (NetGameServer.bs != null)
			{
				NetGameServer.bs.Clear();
			}
			NetSystem.SetSendRate(NetGameServer.send_rate, NetGameClient.SendRate);
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x0001C658 File Offset: 0x0001A858
		public static void SetRelayProvider(INetRelay relay)
		{
			if (NetGameServer.net_server == null)
			{
				Debug.LogWarning("Error : could not set relay provider net server is null");
				return;
			}
			NetGameServer.net_server.SetRelayProvider(relay);
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x000EE718 File Offset: 0x000EC918
		public static string CreateServer(int port, bool forwardPort, UnityAction<string> callback, bool local = false, int mtu = 0)
		{
			Debug.Log("CreateServer");
			NetGameServer.finishedServerCreation = false;
			NetGameServer.IsLocal = local;
			new Thread(delegate()
			{
				NetGameServer.outcome = NetGameServer.DoServerStuff(port, forwardPort);
				NetGameServer.finishedServerCreation = true;
			}).Start();
			ZPNetManager.Instance.StartCoroutine(ZPNetManager.Instance.WaitForServerCreation(callback));
			return "";
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x000EE780 File Offset: 0x000EC980
		private static string DoServerStuff(int port, bool forwardPort)
		{
			Debug.Log("DoServerStuff");
			if (NetSystem.IsConnected || NetSystem.IsInitialized)
			{
				NetSystem.Destroy();
			}
			if (!NetGameServer.IsLocal)
			{
				NetPeerConfiguration netPeerConfiguration = null;
				try
				{
					netPeerConfiguration = new NetPeerConfiguration(NetSystem.AppID);
				}
				catch (Exception)
				{
					return "Client network not started.";
				}
				netPeerConfiguration.Port = port;
				netPeerConfiguration.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
				netPeerConfiguration.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
				netPeerConfiguration.EnableMessageType(NetIncomingMessageType.NatIntroductionSuccess);
				netPeerConfiguration.EnableMessageType(NetIncomingMessageType.Error);
				netPeerConfiguration.EnableMessageType(NetIncomingMessageType.ErrorMessage);
				netPeerConfiguration.DisableMessageType(NetIncomingMessageType.VerboseDebugMessage);
				netPeerConfiguration.DisableMessageType(NetIncomingMessageType.DebugMessage);
				netPeerConfiguration.MaximumConnections = NetGameServer.max_connections;
				netPeerConfiguration.SuppressUnreliableUnorderedAcks = false;
				netPeerConfiguration.AutoFlushSendQueue = false;
				netPeerConfiguration.EnableUPnP = forwardPort;
				netPeerConfiguration.MaximumTransmissionUnit = 1190;
				netPeerConfiguration.AutoExpandMTU = false;
				netPeerConfiguration.ConnectionTimeout = 61f;
				try
				{
					Debug.LogError("Creating NetServer");
					NetGameServer.net_server = new NetServer(netPeerConfiguration);
					NetGameServer.net_server.Start();
					if (forwardPort)
					{
						try
						{
							if (NetGameServer.net_server.UPnP != null)
							{
								new Thread(delegate()
								{
									DateTime now = DateTime.Now;
									while (NetGameServer.net_server.UPnP.Status == UPnPStatus.Discovering && DateTime.Now.Subtract(now).Milliseconds < 5000)
									{
										Thread.Sleep(100);
									}
									Debug.Log(NetGameServer.net_server.UPnP.Status.ToString());
									if (NetGameServer.net_server.UPnP.Status != UPnPStatus.Available)
									{
										GameManager.MainMenuUIController.UPNPFailed = true;
										return;
									}
									if (!NetGameServer.net_server.UPnP.DeleteForwardingRule(port))
									{
										Debug.Log("Failed to delete rule");
									}
									else
									{
										Debug.Log("Deleted rule");
									}
									Thread.Sleep(400);
									if (NetGameServer.net_server.UPnP.ForwardPort(port, "Pummel Party"))
									{
										Debug.Log("Succeded forwarding port: " + port.ToString());
										GameManager.MainMenuUIController.UPNPFailed = false;
										return;
									}
									Debug.Log("Failed to forward port");
									GameManager.MainMenuUIController.UPNPFailed = true;
								}).Start();
							}
						}
						catch (Exception ex)
						{
							Debug.LogError("Port Forward Exception: " + ex.ToString());
						}
					}
				}
				catch (Exception ex2)
				{
					return string.Concat(new string[]
					{
						"Could not create server : ",
						ex2.Message,
						" - ",
						ex2.HResult.ToString(),
						" - ",
						ex2.StackTrace
					});
				}
			}
			Debug.Log("Creating NetSystem");
			NetSystem.Create(true);
			NetSystem.SetStatus(NetSystemStatus.Hosting);
			NetGameServer.is_dedicated = false;
			NetGameServer.is_running = true;
			NetGameServer.server_port = port;
			return "";
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x000EE974 File Offset: 0x000ECB74
		public static void RegisterHostWithMasterServer(ulong identifier, bool isPrivate)
		{
			if (NetGameServer.net_server == null)
			{
				Debug.LogError("Unable to register host with master server, net server is null. has NetGameServer.CreateServer not been called?");
				return;
			}
			foreach (IPEndPoint recipient in NetSystem.MASTER_SERVERS)
			{
				NetOutgoingMessage netOutgoingMessage = NetGameServer.net_server.CreateMessage();
				netOutgoingMessage.Write(4);
				List<IPAddress> allLocalIPAddresses = NetSystem.GetAllLocalIPAddresses(true);
				netOutgoingMessage.Write(identifier);
				netOutgoingMessage.Write((ushort)allLocalIPAddresses.Count);
				foreach (IPAddress address in allLocalIPAddresses)
				{
					netOutgoingMessage.Write(new IPEndPoint(address, NetGameServer.server_port));
				}
				NetGameServer.net_server.SendUnconnectedMessage(netOutgoingMessage, recipient);
			}
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x000EEA58 File Offset: 0x000ECC58
		public static void Destroy()
		{
			if (!NetGameServer.is_running)
			{
				return;
			}
			NetGameServer.server_name = "DEFAULT_SERVER_NAME";
			NetGameServer.server_port = 14242;
			NetGameServer.send_rate = 30;
			NetGameServer.max_connections = 8;
			NetGameServer.server_status = ServerStatus.LOBBY;
			NetGameServer.slots_changed = false;
			if (NetGameServer.net_server != null)
			{
				NetGameServer.net_server.Shutdown("Server Closed.");
			}
			NetGameServer.net_server = null;
			NetGameServer.net_object_manager.Destroy();
			NetGameServer.loaded_level = "";
			NetGameServer.level_loading = "";
			NetGameServer.is_loading = false;
			NetGameServer.next_stat_update = Time.time;
			NetGameServer.last_sent_bytes = 0;
			NetGameServer.last_sent_packets = 0;
			NetGameServer.last_sent_messages = 0;
			NetGameServer.next_snapshot_send = NetTime.Now;
			NetGameServer.snapshot_sent_timer = NetTime.Now;
			NetGameServer.snapshot_sent_count = 0;
			NetGameServer.gametime_update_count = 0;
			NetGameServer.snapshot_recieve_count = 0;
			NetGameServer.snapshot_recieved_timer = Time.time;
			NetSystem.SetStatus(NetSystemStatus.Disconnected);
			NetGameServer.is_dedicated = false;
			NetGameServer.is_running = false;
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x000EEB38 File Offset: 0x000ECD38
		public static void Update()
		{
			NetGameServer.net_object_manager.UpdateTick();
			if (NetGameServer.net_server == null)
			{
				return;
			}
			if (Time.time >= NetGameServer.next_stat_update)
			{
				NetGameServer.next_stat_update += 1f;
				DebugUtility.SetText("kbs_out", "kbs_out : " + ((float)(NetGameServer.net_server.Statistics.SentBytes - NetGameServer.last_sent_bytes) / 1000f).ToString(), new Vector2(10f, 85f));
				DebugUtility.SetText("p_out", "p_out : " + (NetGameServer.net_server.Statistics.SentPackets - NetGameServer.last_sent_packets).ToString(), new Vector2(10f, 65f));
				DebugUtility.SetText("msg_out", "msg_out : " + (NetGameServer.net_server.Statistics.SentMessages - NetGameServer.last_sent_messages).ToString(), new Vector2(10f, 125f));
				NetGameServer.last_sent_packets = NetGameServer.net_server.Statistics.SentPackets;
				NetGameServer.last_sent_bytes = NetGameServer.net_server.Statistics.SentBytes;
				NetGameServer.last_sent_messages = NetGameServer.net_server.Statistics.SentMessages;
			}
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x0001C677 File Offset: 0x0001A877
		public static void Spawn(INetComponent net_entity, NetPlayer owner = null)
		{
			Debug.LogError("SPAWN net_entity not implemented!");
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x0001C683 File Offset: 0x0001A883
		public static NetPrefab GetPrefab(string prefabName)
		{
			return NetGameServer.net_object_manager.GetNetPrefab(prefabName, PrefabType.PREFAB_HOST);
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x0001C691 File Offset: 0x0001A891
		public static GameObject Spawn(string prefab_name, ushort owner_slot = 0, NetPlayer owner = null)
		{
			return NetGameServer.InternalSpawn(prefab_name, Vector3.zero, Quaternion.identity, owner, false, owner_slot);
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x0001C6A6 File Offset: 0x0001A8A6
		public static GameObject Spawn(string prefab_name, Vector3 pos, Quaternion rotation, ushort owner_slot = 0, NetPlayer owner = null)
		{
			return NetGameServer.InternalSpawn(prefab_name, pos, rotation, owner, true, owner_slot);
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x000EEC7C File Offset: 0x000ECE7C
		private static GameObject InternalSpawn(string prefab_name, Vector3 pos, Quaternion rotation, NetPlayer owner, bool initial_transform, ushort owner_slot)
		{
			if (!NetSystem.IsConnected)
			{
				Debug.LogWarning("Must be connected to or hosting a server to spawn Network Objects!");
				return null;
			}
			NetPrefab netPrefab = null;
			if (!NetGameServer.is_dedicated)
			{
				if (NetGameServer.net_object_manager == null)
				{
					Debug.LogError("Net Object Manager Null");
				}
				netPrefab = NetGameServer.net_object_manager.GetNetPrefab(prefab_name, PrefabType.PREFAB_HOST);
			}
			else
			{
				netPrefab = NetGameServer.net_object_manager.GetNetPrefab(prefab_name, PrefabType.PREFAB_DEDICATED);
			}
			if (netPrefab == null)
			{
				Debug.LogWarning("Could not load prefab : " + prefab_name + "!");
				return null;
			}
			ushort gameobj_id = 0;
			GameObject gameObject = null;
			if (initial_transform)
			{
				gameObject = NetGameServer.net_object_manager.CreateGameObject(netPrefab.game_object, pos, rotation, out gameobj_id);
			}
			else
			{
				gameObject = NetGameServer.net_object_manager.CreateGameObject(netPrefab.game_object, out gameobj_id);
			}
			NetBehaviour[] components = gameObject.GetComponents<NetBehaviour>();
			if (components == null)
			{
				Debug.LogError("Requested spawn prefab [" + prefab_name + "] does not have any NetBehaviour scripts!");
				return null;
			}
			if (owner == null && NetSystem.IsServer)
			{
				owner = NetSystem.MyPlayer;
			}
			for (int i = 0; i < components.Length; i++)
			{
				NetGameServer.net_object_manager.CreateNetEntity(components[i], gameobj_id, gameObject, netPrefab.prefab_id, owner, owner_slot, -1);
				try
				{
					components[i].OnNetInitialize();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			return gameObject;
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x0001C6B4 File Offset: 0x0001A8B4
		public static void Kill(INetComponent net_entity)
		{
			NetGameServer.net_object_manager.Kill(net_entity);
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x0001C6C1 File Offset: 0x0001A8C1
		public static GameObject GetGameObject(ushort gameobj_id)
		{
			return NetGameServer.net_object_manager.GetGameObject(gameobj_id);
		}

		// Token: 0x0600286B RID: 10347 RVA: 0x0001C6CE File Offset: 0x0001A8CE
		public static void LoadLevel(string name)
		{
			if (!NetGameServer.is_running)
			{
				Debug.LogError("Cannot call LoadLevel on server when server is not running!");
				return;
			}
			Debug.Log("Loading Scene");
			SceneManager.LoadScene(name);
			NetGameServer.loaded_level = name;
			NetGameServer.level_loading = name;
			NetGameServer.is_loading = true;
			NetGameServer.server_status = ServerStatus.INGAME;
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x000EEDA0 File Offset: 0x000ECFA0
		public static void FinishedLoading()
		{
			if (NetGameServer.is_loading)
			{
				Debug.Log("Finishing Loading Server");
				if (NetGameServer.net_server != null)
				{
					NetOutgoingMessage netOutgoingMessage = NetGameServer.net_server.CreateMessage(NetGameServer.level_loading.Length + 4);
					NetGameServer.WriteMessageType(netOutgoingMessage, ServerMessageType.LOAD_LEVEL);
					netOutgoingMessage.Write(NetGameServer.level_loading);
					NetGameServer.net_server.SendToAll(netOutgoingMessage, NetDeliveryMethod.ReliableOrdered);
				}
				for (int i = 0; i < NetSystem.UserCount; i++)
				{
					NetPlayer playerAtIndex = NetSystem.GetPlayerAtIndex(i);
					playerAtIndex.IsLoading = true;
					playerAtIndex.LastAck = -1;
					playerAtIndex.SentFullSnapshot = false;
				}
			}
			NetGameServer.level_loading = "";
			NetGameServer.is_loading = false;
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x000EEE38 File Offset: 0x000ED038
		public static void SendRPC(INetComponent net_entity, string method_name, NetDeliveryMethod delivery_method, List<NetConnection> recievers, NetConnection relay_sender, object[] parameters)
		{
			if (NetGameServer.net_server == null)
			{
				return;
			}
			NetRPCDefinition rpcdefinition = NetGameServer.net_object_manager.GetRPCDefinition(net_entity, method_name);
			int msgSize = rpcdefinition.GetMsgSize(parameters);
			NetOutgoingMessage msg = NetGameServer.net_server.CreateMessage(msgSize + 2);
			if (NetGameServer.net_object_manager.CreateRPCPacket(ref msg, rpcdefinition, net_entity, parameters))
			{
				rpcdefinition.GetRPCRecievers(net_entity, ref recievers);
				if (relay_sender != null)
				{
					recievers.Remove(relay_sender);
				}
				if (recievers.Count >= 1)
				{
					NetGameServer.net_server.SendMessage(msg, recievers, delivery_method, 0);
				}
			}
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x000EEEB0 File Offset: 0x000ED0B0
		public static bool RelayRPC(NetPlayer sender, byte[] data, int bit_length, INetComponent entity, NetRPCDefinition rpc_definition, object[] paramaters)
		{
			if (!rpc_definition.Relay)
			{
				return false;
			}
			if (!rpc_definition.CanSendRPC(sender, entity))
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"User '",
					sender.Name,
					"' attempted to send RPC '",
					rpc_definition.RPCid.ToString(),
					"' when they do not have permission to!"
				}));
				return false;
			}
			List<NetConnection> connections = NetGameServer.net_server.Connections;
			connections.Remove(sender.Connection);
			if (connections.Count < 1)
			{
				return false;
			}
			int msgSize = rpc_definition.GetMsgSize(paramaters);
			NetOutgoingMessage netOutgoingMessage = NetGameServer.net_server.CreateMessage(msgSize - 1);
			NetGameServer.WriteMessageType(netOutgoingMessage, ServerMessageType.SEND_RPC);
			netOutgoingMessage.Write(data, 0, data.Length);
			NetGameServer.net_server.SendMessage(netOutgoingMessage, connections, NetDeliveryMethod.ReliableUnordered, 0);
			return true;
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x000EEF78 File Offset: 0x000ED178
		public static void WriteStream(int stream_index, NetDeliveryMethod delivery, byte[] data)
		{
			NetOutgoingMessage netOutgoingMessage = NetGameServer.net_server.CreateMessage();
			switch (stream_index)
			{
			case 0:
				netOutgoingMessage.Write(22);
				break;
			case 1:
				netOutgoingMessage.Write(24);
				break;
			case 2:
				netOutgoingMessage.Write(26);
				break;
			case 3:
				netOutgoingMessage.Write(28);
				break;
			}
			netOutgoingMessage.Write(data);
			NetGameServer.net_server.SendMessage(netOutgoingMessage, NetGameServer.net_server.Connections, delivery, 0);
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x0001C70A File Offset: 0x0001A90A
		public static void RelayLobbyChatMessage(string msg, NetPlayer player)
		{
			NetOutgoingMessage netOutgoingMessage = NetGameServer.net_server.CreateMessage();
			NetGameServer.WriteMessageType(netOutgoingMessage, ServerMessageType.CHAT_SERVER_MSG);
			netOutgoingMessage.Write(player.UserID);
			netOutgoingMessage.Write(msg);
			NetGameServer.RelayReliableMessage(netOutgoingMessage, player);
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x0001C737 File Offset: 0x0001A937
		public static void SetSlotsChanged(bool val)
		{
			NetGameServer.slots_changed = val;
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000EEFEC File Offset: 0x000ED1EC
		public static void Send()
		{
			if (NetGameServer.net_server == null)
			{
				return;
			}
			if (NetSystem.UserCount > 1)
			{
				NetOutgoingMessage netOutgoingMessage = NetGameServer.net_server.CreateMessage();
				NetGameServer.WriteMessageType(netOutgoingMessage, ServerMessageType.NETENTITY_CHANGES);
				if (NetGameServer.net_object_manager.CreateNetEntityChangesMessage(netOutgoingMessage) && NetGameServer.net_server.Connections.Count > 0)
				{
					NetGameServer.net_server.SendMessage(netOutgoingMessage, NetGameServer.net_server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
				}
				if (NetTime.Now >= NetGameServer.next_snapshot_send)
				{
					NetGameServer.next_snapshot_send += 1.0 / (double)NetGameServer.send_rate;
					if (NetTime.Now >= NetGameServer.next_snapshot_send)
					{
						NetGameServer.next_snapshot_send = NetTime.Now;
					}
					for (int i = 1; i < NetSystem.UserCount; i++)
					{
						NetPlayer playerAtIndex = NetSystem.GetPlayerAtIndex(i);
						if (playerAtIndex.Approved && !playerAtIndex.IsLoading)
						{
							NetGameServer.bs.Reset();
							bool flag = true;
							bool flag2;
							if ((NetSystem.CurrentTick - playerAtIndex.LastAck >= NetGameServer.send_rate * 2 && playerAtIndex.LastAck < playerAtIndex.LastSentSnapshot && NetSystem.CurrentTick - playerAtIndex.LastFullSnapshot > NetGameServer.send_rate / 2) || playerAtIndex.LastAck == -1 || !playerAtIndex.SentFullSnapshot)
							{
								NetGameServer.net_object_manager.CreateFullSnapshot(playerAtIndex, ref NetGameServer.bs);
								playerAtIndex.LastAck = NetSystem.CurrentTick;
								flag2 = true;
								playerAtIndex.SentFullSnapshot = true;
							}
							else
							{
								flag = NetGameServer.net_object_manager.CreateDeltaSnapshot(playerAtIndex, ref NetGameServer.bs);
								flag2 = false;
							}
							NetGameServer.gametime_update_count++;
							if (NetGameServer.bs.GetByteLength() >= 1 && flag)
							{
								NetOutgoingMessage netOutgoingMessage2 = NetGameServer.net_server.CreateMessage(NetGameServer.bs.GetByteLength() + 9);
								netOutgoingMessage2.Write(true);
								netOutgoingMessage2.Write(flag2);
								netOutgoingMessage2.Write(NetSystem.CurrentTick, 24);
								netOutgoingMessage2.Write(playerAtIndex.LastClientSnapshotChanged);
								if (playerAtIndex.LastClientSnapshotChanged)
								{
									netOutgoingMessage2.Write(playerAtIndex.LastClientSnapshot, 24);
									playerAtIndex.LastClientSnapshotChanged = false;
								}
								if (NetGameServer.gametime_update_count >= NetSystem.GameTimeTickRate)
								{
									netOutgoingMessage2.Write(true);
									netOutgoingMessage2.Write(NetSystem.NetTime.GameTime);
									NetGameServer.gametime_update_count = 0;
								}
								else
								{
									netOutgoingMessage2.Write(false);
								}
								netOutgoingMessage2.Write(NetGameServer.bs.GetBuffer(), 0, NetGameServer.bs.GetByteLength());
								NetGameServer.net_server.SendMessage(netOutgoingMessage2, playerAtIndex.Connection, flag2 ? NetDeliveryMethod.ReliableOrdered : NetDeliveryMethod.Unreliable);
								playerAtIndex.LastSentSnapshot = NetSystem.CurrentTick;
								NetGameServer.snapshot_sent_count++;
							}
							else
							{
								NetOutgoingMessage netOutgoingMessage3 = NetGameServer.net_server.CreateMessage(9);
								NetGameServer.WriteMessageType(netOutgoingMessage3, ServerMessageType.HEARTBEAT);
								netOutgoingMessage3.Write(NetSystem.CurrentTick, 24);
								netOutgoingMessage3.Write(playerAtIndex.LastClientSnapshotChanged);
								if (playerAtIndex.LastClientSnapshotChanged)
								{
									netOutgoingMessage3.Write(playerAtIndex.LastClientSnapshot, 24);
									playerAtIndex.LastClientSnapshotChanged = false;
								}
								if (NetGameServer.gametime_update_count >= NetSystem.GameTimeTickRate)
								{
									netOutgoingMessage3.Write(true);
									netOutgoingMessage3.Write(NetSystem.NetTime.GameTime);
									NetGameServer.gametime_update_count = 0;
								}
								else
								{
									netOutgoingMessage3.Write(false);
								}
								NetGameServer.net_server.SendMessage(netOutgoingMessage3, playerAtIndex.Connection, NetDeliveryMethod.Unreliable);
							}
						}
					}
					NetSystem.IncrementTick();
				}
				NetGameServer.snapshot_sent_timer += (double)Time.deltaTime;
				if (NetGameServer.snapshot_sent_timer >= 1.0)
				{
					DebugUtility.SetText("SnapshotsSent", "snapshots_sent : " + NetGameServer.snapshot_sent_count.ToString(), new Vector2(10f, 45f));
					NetGameServer.snapshot_sent_count = 0;
					NetGameServer.snapshot_sent_timer -= 1.0;
				}
				NetGameServer.SendLobbySlots();
			}
			NetGameServer.net_object_manager.ClearEntityChanges();
			NetGameServer.net_server.FlushSendQueue();
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000EF388 File Offset: 0x000ED588
		private static void SendLobbySlots()
		{
			if (NetGameServer.slots_changed)
			{
				NetOutgoingMessage netOutgoingMessage = NetGameServer.net_server.CreateMessage();
				NetGameServer.WriteMessageType(netOutgoingMessage, ServerMessageType.SLOTS_CHANGED);
				netOutgoingMessage.Write((ushort)NetSystem.PlayerCount);
				for (int i = 0; i < NetSystem.MaxPlayers; i++)
				{
					NetPlayer lobbySlot = NetSystem.GetLobbySlot(i, false);
					if (lobbySlot != null)
					{
						netOutgoingMessage.Write(lobbySlot.UserID);
						netOutgoingMessage.Write((byte)lobbySlot.Slot);
					}
				}
				netOutgoingMessage.Write((ushort)NetSystem.ObserverCount);
				for (int j = 0; j < NetSystem.MaxObservers; j++)
				{
					NetPlayer lobbySlot2 = NetSystem.GetLobbySlot(j, true);
					if (lobbySlot2 != null)
					{
						netOutgoingMessage.Write(lobbySlot2.UserID);
						netOutgoingMessage.Write((byte)lobbySlot2.Slot);
					}
				}
				NetGameServer.RelayReliableMessage(netOutgoingMessage, NetSystem.MyPlayer);
				NetSystem.OnSlotsChanged();
				NetGameServer.slots_changed = false;
			}
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x0000398C File Offset: 0x00001B8C
		private static void SendSnapshot()
		{
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x0001C514 File Offset: 0x0001A714
		private static void WriteMessageType(NetOutgoingMessage m, ServerMessageType msg_type)
		{
			if (msg_type == ServerMessageType.SNAPSHOT_FULL || msg_type == ServerMessageType.SNAPSHOT_DELTA)
			{
				m.Write(true);
				m.Write(msg_type == ServerMessageType.SNAPSHOT_FULL);
				return;
			}
			m.Write((byte)msg_type);
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x000EF44C File Offset: 0x000ED64C
		public static void Recieve()
		{
			if (NetGameServer.net_server == null)
			{
				return;
			}
			NetGameServer.snapshot_recieved_timer += Time.deltaTime;
			if (NetGameServer.snapshot_recieved_timer >= 1f)
			{
				DebugUtility.SetText("SnapshotsRecieved", "snapshots_recieved : " + NetGameServer.snapshot_recieve_count.ToString(), new Vector2(10f, 65f));
				NetGameServer.snapshot_recieved_timer -= 1f;
				NetGameServer.snapshot_recieve_count = 0;
			}
			NetIncomingMessage netIncomingMessage;
			while ((netIncomingMessage = NetGameServer.net_server.ReadMessage()) != null)
			{
				NetIncomingMessageType messageType = netIncomingMessage.MessageType;
				if (messageType <= NetIncomingMessageType.DiscoveryRequest)
				{
					if (messageType <= NetIncomingMessageType.ConnectionApproval)
					{
						if (messageType != NetIncomingMessageType.StatusChanged)
						{
							if (messageType != NetIncomingMessageType.ConnectionApproval)
							{
								goto IL_341;
							}
							if (NetSystem.IsServerFull())
							{
								netIncomingMessage.SenderConnection.Deny("NetDenyFull");
							}
							else if (NetGameServer.server_status == ServerStatus.INGAME)
							{
								netIncomingMessage.SenderConnection.Deny("NetDenyInProgress");
							}
							else if (netIncomingMessage.ReadString() != GameManager.VERSION)
							{
								netIncomingMessage.SenderConnection.Deny("NetDenyVersion");
							}
							else
							{
								NetPlayer netPlayer = NetSystem.CreatePlayer(netIncomingMessage.ReadString(), netIncomingMessage.SenderConnection, netIncomingMessage.SenderEndPoint);
								if (NetGameServer.server_status == ServerStatus.INGAME)
								{
									netPlayer.IsLoading = true;
								}
								if (netPlayer != null)
								{
									netIncomingMessage.SenderConnection.Approve();
								}
								else
								{
									netIncomingMessage.SenderConnection.Deny("NetDenyFull");
								}
							}
						}
						else
						{
							NetConnectionStatus netConnectionStatus = (NetConnectionStatus)netIncomingMessage.ReadByte();
							string reason = netIncomingMessage.ReadString();
							NetPlayer player = NetSystem.GetPlayer(netIncomingMessage.SenderEndPoint);
							if (player != null)
							{
								if (netConnectionStatus == NetConnectionStatus.Connected)
								{
									NetGameServer.SendServerInfo(player);
									NetGameServer.SendPlayerConnected(player);
									player.Approved = true;
									if (NetGameServer.server_status == ServerStatus.INGAME)
									{
										NetOutgoingMessage netOutgoingMessage = NetGameServer.net_server.CreateMessage(NetGameServer.loaded_level.Length + 4);
										NetGameServer.WriteMessageType(netOutgoingMessage, ServerMessageType.LOAD_LEVEL);
										netOutgoingMessage.Write(NetGameServer.loaded_level);
										NetGameServer.net_server.SendMessage(netOutgoingMessage, player.Connection, NetDeliveryMethod.ReliableOrdered);
										player.IsLoading = true;
										player.LastAck = -1;
									}
									NetSystem.OnPlayerConnected(player);
								}
								else if (netConnectionStatus == NetConnectionStatus.Disconnected)
								{
									NetGameServer.SendPlayerDisconnected(player, reason);
									NetSystem.RemovePlayer(player);
								}
							}
							else
							{
								Debug.LogWarning("Status of player not in player list changed! : [" + netConnectionStatus.ToString() + "]");
							}
						}
					}
					else if (messageType != NetIncomingMessageType.Data)
					{
						if (messageType != NetIncomingMessageType.DiscoveryRequest)
						{
							goto IL_341;
						}
						Debug.LogWarning("Recieved Discovery Request!");
						NetOutgoingMessage netOutgoingMessage2 = NetGameServer.net_server.CreateMessage();
						netOutgoingMessage2.WriteAllFields(new ServerBroadcastInfo(NetGameServer.server_name, (byte)NetSystem.PlayerCount, (byte)NetSystem.ObserverCount, 1));
						NetGameServer.net_server.SendDiscoveryResponse(netOutgoingMessage2, netIncomingMessage.SenderEndPoint);
					}
					else
					{
						NetGameServer.ReadMessage(netIncomingMessage);
					}
				}
				else if (messageType <= NetIncomingMessageType.DebugMessage)
				{
					if (messageType != NetIncomingMessageType.VerboseDebugMessage)
					{
						if (messageType != NetIncomingMessageType.DebugMessage)
						{
							goto IL_341;
						}
						Debug.Log(netIncomingMessage.ReadString());
					}
				}
				else if (messageType != NetIncomingMessageType.WarningMessage)
				{
					if (messageType != NetIncomingMessageType.ErrorMessage)
					{
						if (messageType != NetIncomingMessageType.NatIntroductionSuccess)
						{
							goto IL_341;
						}
						string text = netIncomingMessage.ReadString();
						Debug.Log(string.Concat(new object[]
						{
							"Nat introduction success to ",
							netIncomingMessage.SenderEndPoint,
							" token is: ",
							text
						}));
					}
					else
					{
						Debug.LogError(netIncomingMessage.ReadString());
					}
				}
				else
				{
					Debug.LogWarning(netIncomingMessage.ReadString());
				}
				IL_3C5:
				NetGameServer.net_server.Recycle(netIncomingMessage);
				continue;
				IL_341:
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
				goto IL_3C5;
			}
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x000EF83C File Offset: 0x000EDA3C
		private static void ReadMessage(NetIncomingMessage inc)
		{
			NetPlayer player = NetSystem.GetPlayer(inc.SenderEndPoint);
			if ((inc.PeekByte() & 1) > 0)
			{
				inc.ReadBoolean();
				bool flag = inc.ReadBoolean();
				int num = 0;
				if (flag)
				{
					int num2 = NetGameServer.net_object_manager.ReadSnapshot(inc, true, ref num);
					if (num2 > player.LastClientSnapshot)
					{
						player.LastClientSnapshot = num2;
					}
					NetGameServer.snapshot_recieve_count++;
					return;
				}
				int num3 = NetGameServer.net_object_manager.ReadSnapshot(inc, false, ref num);
				if (num3 > player.LastClientSnapshot)
				{
					player.LastClientSnapshot = num3;
				}
				NetGameServer.snapshot_recieve_count++;
				return;
			}
			else
			{
				ClientMessageType clientMessageType = (ClientMessageType)inc.ReadByte();
				if (clientMessageType != ClientMessageType.SEND_RPC)
				{
					if (clientMessageType != ClientMessageType.SNAPSHOT_ACK)
					{
						switch (clientMessageType)
						{
						case ClientMessageType.FINISHED_LOADING:
							if (player.IsLoading)
							{
								player.IsLoading = false;
								NetSystem.OnPlayerLoaded(player);
								return;
							}
							return;
						case ClientMessageType.LOBBY_CHAT_MSG:
						{
							string msg = inc.ReadString();
							NetSystem.OnChatMessageRecieved(NetSystem.GetPlayer(inc.SenderEndPoint), msg);
							NetGameServer.RelayLobbyChatMessage(msg, NetSystem.GetPlayer(inc.SenderEndPoint));
							return;
						}
						case ClientMessageType.REQUEST_SLOT_CHANGE:
						{
							ushort slot = inc.ReadUInt16();
							bool observer = inc.ReadBoolean();
							if (NetSystem.ChangePlayerSlot(slot, observer, player))
							{
								NetGameServer.slots_changed = true;
								return;
							}
							return;
						}
						case ClientMessageType.STREAM_0:
							NetSystem.ReadStream(0, inc);
							return;
						case ClientMessageType.STREAM_1:
							NetSystem.ReadStream(1, inc);
							return;
						case ClientMessageType.STREAM_2:
							NetSystem.ReadStream(2, inc);
							return;
						case ClientMessageType.STREAM_3:
							NetSystem.ReadStream(3, inc);
							return;
						}
						string str = "Unhandled ClientMessageType : ";
						int num4 = (int)clientMessageType;
						Debug.LogWarning(str + num4.ToString() + "!");
					}
					else
					{
						int num5 = inc.ReadInt32();
						if (player.LastAck < num5)
						{
							player.LastAck = num5;
							return;
						}
					}
					return;
				}
				byte[] array = new byte[inc.LengthBytes - 1];
				inc.ReadBytes(array, 0, inc.LengthBytes - 1);
				NetGameServer.net_object_manager.InvokeRPC(player, array, array.Length * 8, false);
				return;
			}
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x000EFA18 File Offset: 0x000EDC18
		private static void RelayReliableMessage(NetOutgoingMessage om, NetPlayer player)
		{
			List<NetConnection> connections = NetGameServer.net_server.Connections;
			if (player.UserID != 0)
			{
				connections.Remove(player.Connection);
			}
			if (connections.Count > 0)
			{
				NetGameServer.net_server.SendMessage(om, connections, NetDeliveryMethod.ReliableOrdered, 0);
			}
		}

		// Token: 0x06002879 RID: 10361 RVA: 0x000EFA60 File Offset: 0x000EDC60
		private static void SendServerInfo(NetPlayer player)
		{
			NetOutgoingMessage netOutgoingMessage = NetGameServer.net_server.CreateMessage(32);
			NetGameServer.WriteMessageType(netOutgoingMessage, ServerMessageType.CONNECTION_APPROVED);
			netOutgoingMessage.Write((byte)NetGameServer.server_status);
			netOutgoingMessage.Write((byte)NetSystem.GetGameMode());
			netOutgoingMessage.Write((byte)player.Slot);
			netOutgoingMessage.Write(player.Observer);
			netOutgoingMessage.Write(player.UserID);
			ushort num = (ushort)NetSystem.UserCount;
			netOutgoingMessage.Write(num - 1);
			for (int i = 0; i < (int)num; i++)
			{
				NetPlayer playerAtIndex = NetSystem.GetPlayerAtIndex(i);
				if (playerAtIndex != player)
				{
					netOutgoingMessage.Write(playerAtIndex.UserID);
					netOutgoingMessage.Write((byte)playerAtIndex.Slot);
					netOutgoingMessage.Write(playerAtIndex.Observer);
					netOutgoingMessage.Write(playerAtIndex.Name);
				}
			}
			NetGameServer.net_server.SendMessage(netOutgoingMessage, player.Connection, NetDeliveryMethod.ReliableOrdered);
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x000EFB2C File Offset: 0x000EDD2C
		private static void SendPlayerConnected(NetPlayer player)
		{
			NetOutgoingMessage netOutgoingMessage = NetGameServer.net_server.CreateMessage(16);
			NetGameServer.WriteMessageType(netOutgoingMessage, ServerMessageType.PLAYER_CONNECTED);
			netOutgoingMessage.Write(player.UserID);
			netOutgoingMessage.Write((byte)player.Slot);
			netOutgoingMessage.Write(player.Observer);
			netOutgoingMessage.Write(player.Name);
			NetGameServer.RelayReliableMessage(netOutgoingMessage, player);
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x0001C73F File Offset: 0x0001A93F
		private static void SendPlayerDisconnected(NetPlayer player, string reason)
		{
			NetOutgoingMessage netOutgoingMessage = NetGameServer.net_server.CreateMessage(4);
			NetGameServer.WriteMessageType(netOutgoingMessage, ServerMessageType.PLAYER_DISCONNECTED);
			netOutgoingMessage.Write(player.UserID);
			NetGameServer.RelayReliableMessage(netOutgoingMessage, player);
		}

		// Token: 0x04002B0C RID: 11020
		private static bool is_dedicated = false;

		// Token: 0x04002B0D RID: 11021
		private static bool is_running = false;

		// Token: 0x04002B0E RID: 11022
		private static string server_name = "DEFAULT_SERVER_NAME";

		// Token: 0x04002B0F RID: 11023
		private static int server_port = 14242;

		// Token: 0x04002B10 RID: 11024
		private static int send_rate = 30;

		// Token: 0x04002B11 RID: 11025
		private static int max_connections = 8;

		// Token: 0x04002B12 RID: 11026
		private static ServerStatus server_status = ServerStatus.LOBBY;

		// Token: 0x04002B13 RID: 11027
		private static bool slots_changed = false;

		// Token: 0x04002B14 RID: 11028
		private static NetServer net_server;

		// Token: 0x04002B15 RID: 11029
		private static NetObjectManager net_object_manager;

		// Token: 0x04002B16 RID: 11030
		private static ZPBitStream bs;

		// Token: 0x04002B17 RID: 11031
		private static string loaded_level = "";

		// Token: 0x04002B18 RID: 11032
		private static string level_loading = "";

		// Token: 0x04002B19 RID: 11033
		private static bool is_loading = false;

		// Token: 0x04002B1A RID: 11034
		private static float next_stat_update = Time.time;

		// Token: 0x04002B1B RID: 11035
		private static int last_sent_bytes = 0;

		// Token: 0x04002B1C RID: 11036
		private static int last_sent_packets = 0;

		// Token: 0x04002B1D RID: 11037
		private static int last_sent_messages = 0;

		// Token: 0x04002B1E RID: 11038
		private static double next_snapshot_send = NetTime.Now;

		// Token: 0x04002B1F RID: 11039
		private static double snapshot_sent_timer = NetTime.Now;

		// Token: 0x04002B20 RID: 11040
		private static int snapshot_sent_count = 0;

		// Token: 0x04002B21 RID: 11041
		private static int gametime_update_count = 0;

		// Token: 0x04002B22 RID: 11042
		private static int snapshot_recieve_count = 0;

		// Token: 0x04002B23 RID: 11043
		private static float snapshot_recieved_timer = Time.time;

		// Token: 0x04002B25 RID: 11045
		private static bool initialized = false;

		// Token: 0x04002B26 RID: 11046
		public static bool finishedServerCreation = false;

		// Token: 0x04002B27 RID: 11047
		public static string outcome = "";
	}
}
