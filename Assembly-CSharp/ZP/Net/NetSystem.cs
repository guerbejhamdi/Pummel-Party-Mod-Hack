using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using I2.Loc;
using Lidgren.Network;
using UnityEngine;
using ZP.Utility;

namespace ZP.Net
{
	// Token: 0x02000630 RID: 1584
	public class NetSystem
	{
		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x0600291B RID: 10523 RVA: 0x0001CB3C File Offset: 0x0001AD3C
		public static float NatConnectTimeRemaining
		{
			get
			{
				return 6f - (Time.time - NetSystem.connect_start_time);
			}
		}

		// Token: 0x0600291C RID: 10524 RVA: 0x000F4FDC File Offset: 0x000F31DC
		static NetSystem()
		{
			NetSystem.MASTER_SERVERS = new List<IPEndPoint>();
			foreach (string ipOrHost in NetSystem.MASTER_SERVER_IP_LIST)
			{
				NetSystem.MASTER_SERVERS.Add(new IPEndPoint(NetUtility.Resolve(ipOrHost), 58117));
			}
			NetSystem.Initialize();
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x0600291D RID: 10525 RVA: 0x0001CB4F File Offset: 0x0001AD4F
		// (set) Token: 0x0600291E RID: 10526 RVA: 0x0001CB56 File Offset: 0x0001AD56
		public static string AppID
		{
			get
			{
				return NetSystem.app_id;
			}
			set
			{
				NetSystem.app_id = value;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x0600291F RID: 10527 RVA: 0x0001CB5E File Offset: 0x0001AD5E
		// (set) Token: 0x06002920 RID: 10528 RVA: 0x0001CB65 File Offset: 0x0001AD65
		public static bool IsInitialized
		{
			get
			{
				return NetSystem.is_initialized;
			}
			set
			{
				NetSystem.is_initialized = value;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06002921 RID: 10529 RVA: 0x0001CB6D File Offset: 0x0001AD6D
		public static bool IsConnected
		{
			get
			{
				return NetSystem.status == NetSystemStatus.Connected || NetSystem.status == NetSystemStatus.Hosting;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06002922 RID: 10530 RVA: 0x0001CB81 File Offset: 0x0001AD81
		public static bool IsConnecting
		{
			get
			{
				return NetSystem.status == NetSystemStatus.Connecting;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06002923 RID: 10531 RVA: 0x0001CB8B File Offset: 0x0001AD8B
		public static bool IsServer
		{
			get
			{
				return NetSystem.status == NetSystemStatus.Hosting;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06002924 RID: 10532 RVA: 0x0001CB95 File Offset: 0x0001AD95
		public static int PlayerCount
		{
			get
			{
				return NetSystem.player_count;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06002925 RID: 10533 RVA: 0x0001CB9C File Offset: 0x0001AD9C
		public static int ObserverCount
		{
			get
			{
				return NetSystem.observer_count;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06002926 RID: 10534 RVA: 0x0001CBA3 File Offset: 0x0001ADA3
		// (set) Token: 0x06002927 RID: 10535 RVA: 0x0001CBAA File Offset: 0x0001ADAA
		public static NetPlayer MyPlayer
		{
			get
			{
				return NetSystem.my_player;
			}
			set
			{
				NetSystem.my_player = value;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06002928 RID: 10536 RVA: 0x0001CBB2 File Offset: 0x0001ADB2
		// (set) Token: 0x06002929 RID: 10537 RVA: 0x0001CBB9 File Offset: 0x0001ADB9
		public static int MaxPlayers
		{
			get
			{
				return NetSystem.max_players;
			}
			set
			{
				NetSystem.max_players = value;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x0600292A RID: 10538 RVA: 0x0001CBC1 File Offset: 0x0001ADC1
		// (set) Token: 0x0600292B RID: 10539 RVA: 0x0001CBC8 File Offset: 0x0001ADC8
		public static int MaxObservers
		{
			get
			{
				return NetSystem.max_observers;
			}
			set
			{
				NetSystem.max_observers = value;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x0600292C RID: 10540 RVA: 0x0001CBD0 File Offset: 0x0001ADD0
		// (set) Token: 0x0600292D RID: 10541 RVA: 0x0001CBD7 File Offset: 0x0001ADD7
		public static int MaxPossibleConnections
		{
			get
			{
				return NetSystem.max_possible_connections;
			}
			set
			{
				NetSystem.max_possible_connections = value;
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x0600292E RID: 10542 RVA: 0x0001CBDF File Offset: 0x0001ADDF
		public static int UserCount
		{
			get
			{
				return NetSystem.player_list.Count;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x0600292F RID: 10543 RVA: 0x0001CBEB File Offset: 0x0001ADEB
		public static int CurrentTick
		{
			get
			{
				return NetSystem.current_tick;
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06002930 RID: 10544 RVA: 0x0001CBF2 File Offset: 0x0001ADF2
		public static int GameTimeTickRate
		{
			get
			{
				return NetSystem.gametime_update_tick_rate;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06002931 RID: 10545 RVA: 0x0001CBF9 File Offset: 0x0001ADF9
		public static ZPNetTime NetTime
		{
			get
			{
				return NetSystem.net_time;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06002932 RID: 10546 RVA: 0x0001CC00 File Offset: 0x0001AE00
		public static int NetEntityCount
		{
			get
			{
				if (NetSystem.IsServer)
				{
					return NetGameServer.NetEntityCount;
				}
				return NetGameClient.NetEntityCount;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06002933 RID: 10547 RVA: 0x0001CC14 File Offset: 0x0001AE14
		public static string LastConnectionError
		{
			get
			{
				return NetSystem.last_connection_error;
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06002934 RID: 10548 RVA: 0x000F50F0 File Offset: 0x000F32F0
		// (remove) Token: 0x06002935 RID: 10549 RVA: 0x000F5124 File Offset: 0x000F3324
		public static event ConnectLobbyEventHandler ConnectedToLobby;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06002936 RID: 10550 RVA: 0x000F5158 File Offset: 0x000F3358
		// (remove) Token: 0x06002937 RID: 10551 RVA: 0x000F518C File Offset: 0x000F338C
		public static event DisconnectEventHandler Disconnected;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06002938 RID: 10552 RVA: 0x000F51C0 File Offset: 0x000F33C0
		// (remove) Token: 0x06002939 RID: 10553 RVA: 0x000F51F4 File Offset: 0x000F33F4
		public static event ConnectFailedEventHandler ConnectFailed;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600293A RID: 10554 RVA: 0x000F5228 File Offset: 0x000F3428
		// (remove) Token: 0x0600293B RID: 10555 RVA: 0x000F525C File Offset: 0x000F345C
		public static event ChatEventHandler ChatMessageRecieved;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600293C RID: 10556 RVA: 0x000F5290 File Offset: 0x000F3490
		// (remove) Token: 0x0600293D RID: 10557 RVA: 0x000F52C4 File Offset: 0x000F34C4
		public static event HostConnectedEventHandler HostConnected;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600293E RID: 10558 RVA: 0x000F52F8 File Offset: 0x000F34F8
		// (remove) Token: 0x0600293F RID: 10559 RVA: 0x000F532C File Offset: 0x000F352C
		public static event PlayerConnectedEventHandler PlayerConnected;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06002940 RID: 10560 RVA: 0x000F5360 File Offset: 0x000F3560
		// (remove) Token: 0x06002941 RID: 10561 RVA: 0x000F5394 File Offset: 0x000F3594
		public static event PlayerDisconnectedEventHandler PlayerDisconnected;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06002942 RID: 10562 RVA: 0x000F53C8 File Offset: 0x000F35C8
		// (remove) Token: 0x06002943 RID: 10563 RVA: 0x000F53FC File Offset: 0x000F35FC
		public static event PlayerLoadedEventHandler PlayerLoaded;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06002944 RID: 10564 RVA: 0x000F5430 File Offset: 0x000F3630
		// (remove) Token: 0x06002945 RID: 10565 RVA: 0x000F5464 File Offset: 0x000F3664
		public static event HostLoadedEventHandler HostLoaded;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06002946 RID: 10566 RVA: 0x000F5498 File Offset: 0x000F3698
		// (remove) Token: 0x06002947 RID: 10567 RVA: 0x000F54CC File Offset: 0x000F36CC
		public static event LobbyHostedEventHandler LobbyHosted;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06002948 RID: 10568 RVA: 0x000F5500 File Offset: 0x000F3700
		// (remove) Token: 0x06002949 RID: 10569 RVA: 0x000F5534 File Offset: 0x000F3734
		public static event SlotsChangedEventHandler SlotsChanged;

		// Token: 0x0600294A RID: 10570 RVA: 0x0001CC1B File Offset: 0x0001AE1B
		public static void OnConnectedToLobby()
		{
			if (NetSystem.ConnectedToLobby != null)
			{
				NetSystem.ConnectedToLobby();
			}
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x0001CC2E File Offset: 0x0001AE2E
		public static void OnDisconnect(string reason)
		{
			NetSystem.Destroy();
			NetSystem.SetStatus(NetSystemStatus.Disconnected);
			if (NetSystem.Disconnected != null)
			{
				NetSystem.Disconnected(reason);
			}
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x0001CC4D File Offset: 0x0001AE4D
		public static void OnConnectFailed(string msg)
		{
			NetSystem.SetLastConnectionError(msg);
			if (NetSystem.ConnectFailed != null)
			{
				NetSystem.ConnectFailed(msg);
			}
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x0001CC67 File Offset: 0x0001AE67
		public static void OnChatMessageRecieved(NetPlayer player, string msg)
		{
			if (NetSystem.ChatMessageRecieved != null)
			{
				NetSystem.ChatMessageRecieved(player, msg);
			}
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x0001CC7C File Offset: 0x0001AE7C
		public static void OnHostConnected(NetPlayer host)
		{
			if (NetSystem.HostConnected != null)
			{
				NetSystem.HostConnected(host);
			}
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x0001CC90 File Offset: 0x0001AE90
		public static void OnPlayerConnected(NetPlayer player)
		{
			if (NetSystem.PlayerConnected != null)
			{
				NetSystem.PlayerConnected(player);
			}
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x000F5568 File Offset: 0x000F3768
		public static void OnPlayerDisconnected(NetPlayer player)
		{
			Debug.LogWarning("PLAYER DISCONNECTED = " + player.UserID.ToString());
			if (NetSystem.PlayerDisconnected != null)
			{
				NetSystem.PlayerDisconnected(player);
				return;
			}
			Debug.Log("PlayerDisonnected delegate is null!");
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x0001CCA4 File Offset: 0x0001AEA4
		public static void OnPlayerLoaded(NetPlayer player)
		{
			if (NetSystem.PlayerLoaded != null)
			{
				NetSystem.PlayerLoaded(player);
			}
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x0001CCB8 File Offset: 0x0001AEB8
		public static void OnHostLoaded()
		{
			if (NetSystem.HostLoaded != null)
			{
				NetSystem.HostLoaded();
			}
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x0001CCCB File Offset: 0x0001AECB
		public static void OnLobbyHosted()
		{
			if (NetSystem.LobbyHosted != null)
			{
				NetSystem.LobbyHosted();
			}
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x0001CCDE File Offset: 0x0001AEDE
		public static void OnSlotsChanged()
		{
			if (NetSystem.SlotsChanged != null)
			{
				NetSystem.SlotsChanged();
			}
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x0001CCF1 File Offset: 0x0001AEF1
		public static void SetMaxConnections(int value)
		{
			NetSystem.max_connections = value;
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x000F55B0 File Offset: 0x000F37B0
		public static List<IPAddress> GetAllLocalIPAddresses(bool ipV4Only = false)
		{
			List<IPAddress> list = new List<IPAddress>();
			foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
			{
				if ((networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet3Megabit || networkInterface.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet || networkInterface.NetworkInterfaceType == NetworkInterfaceType.FastEthernetFx || networkInterface.NetworkInterfaceType == NetworkInterfaceType.FastEthernetT) && networkInterface.OperationalStatus == OperationalStatus.Up)
				{
					foreach (UnicastIPAddressInformation unicastIPAddressInformation in networkInterface.GetIPProperties().UnicastAddresses)
					{
						if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork || (!ipV4Only && unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetworkV6))
						{
							list.Add(unicastIPAddressInformation.Address);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x0001CCF9 File Offset: 0x0001AEF9
		public static void OnLidgrenLog(string msg)
		{
			Debug.LogError("Lidgren : " + msg);
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x000F569C File Offset: 0x000F389C
		public static void Initialize()
		{
			if (NetSystem.initiated)
			{
				return;
			}
			NetSystem.net_time = new ZPNetTime();
			IPEndPoint endpoint = new IPEndPoint(0L, 0);
			NetSystem.my_player = new NetPlayer(PlatformUtility.GetUsername(), 0, false, 0, null, endpoint);
			NetSystem.SetGameMode(0);
			NetGameClient.Initialize();
			NetGameServer.Initialize();
			for (int i = 0; i < NetSystem.MAX_STREAM_COUNT; i++)
			{
				NetSystem.write_streams[i] = new ZPBitStream();
				NetSystem.read_streams[i] = new ZPBitStream();
				NetSystem.write_streams[i].Reserve(32);
				NetSystem.read_streams[i].Reserve(32);
			}
			NetSystem.initiated = true;
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x000F5734 File Offset: 0x000F3934
		public static void Create(bool server)
		{
			NetSystem.player_count = 0;
			NetSystem.observer_count = 0;
			NetSystem.player_list = new List<NetPlayer>();
			NetSystem.player_slots = new NetPlayer[NetSystem.max_players];
			NetSystem.observer_slots = new NetPlayer[NetSystem.max_observers];
			NetSystem.net_cur_time = 0f;
			if (server)
			{
				NetSystem.player_ip_map = new Dictionary<IPEndPoint, NetPlayer>();
				NetSystem.player_uid_map = new Dictionary<ushort, NetPlayer>();
				NetSystem.player_id_status = new bool[NetSystem.max_possible_connections];
				for (int i = 0; i < NetSystem.max_possible_connections; i++)
				{
					NetSystem.player_id_status[i] = false;
				}
				NetSystem.player_id_status[0] = true;
				IPEndPoint ipendPoint = new IPEndPoint(0L, 0);
				NetSystem.my_player = new NetPlayer(NetSystem.my_player.Name, 0, false, 0, null, ipendPoint);
				NetSystem.player_ip_map.Add(ipendPoint, NetSystem.my_player);
				NetSystem.player_uid_map.Add(0, NetSystem.my_player);
				NetSystem.player_list.Add(NetSystem.my_player);
				NetSystem.player_slots[0] = NetSystem.my_player;
				NetSystem.player_count = 1;
				return;
			}
			NetSystem.player_uid_map = new Dictionary<ushort, NetPlayer>();
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x0001CD0B File Offset: 0x0001AF0B
		public static void SetStatus(NetSystemStatus new_status)
		{
			NetSystem.status = new_status;
			if (NetSystem.status == NetSystemStatus.Connecting)
			{
				NetSystem.connect_start_time = Time.time;
			}
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x000F5838 File Offset: 0x000F3A38
		public static void Destroy()
		{
			if (!NetSystem.IsConnected && !NetSystem.is_initialized)
			{
				return;
			}
			if (NetSystem.IsServer)
			{
				NetGameServer.Destroy();
			}
			else
			{
				NetGameClient.Stop("Client Disconnect");
			}
			NetSystem.initiated = false;
			NetSystem.connect_start_time = 0f;
			NetSystem.player_count = 0;
			NetSystem.observer_count = 0;
			NetSystem.my_player = null;
			NetSystem.player_id_status = null;
			NetSystem.player_ip_map = null;
			NetSystem.player_uid_map = null;
			NetSystem.player_list = null;
			NetSystem.player_slots = null;
			NetSystem.observer_slots = null;
			NetSystem.game_mode = 0;
			NetSystem.current_tick = 0;
			NetSystem.net_cur_time = 0f;
			NetSystem.net_time = null;
			NetSystem.last_connection_error = "";
			NetSystem.is_initialized = false;
			NetSystem.Initialize();
			NetSystem.SetStatus(NetSystemStatus.Disconnected);
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x0001CD25 File Offset: 0x0001AF25
		public static void IncrementTime()
		{
			if (!NetSystem.IsConnected)
			{
				return;
			}
			NetSystem.net_time.UpdateTime();
			NetSystem.net_cur_time += Time.deltaTime;
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x000F58EC File Offset: 0x000F3AEC
		public static void Update()
		{
			if (!NetSystem.IsConnected)
			{
				if (NetSystem.status == NetSystemStatus.Connecting && Time.time - NetSystem.connect_start_time >= 6f)
				{
					bool waitingForNatIntroductionSuccess = NetGameClient.WaitingForNatIntroductionSuccess;
					NetSystem.Destroy();
					NetSystem.SetStatus(NetSystemStatus.Disconnected);
					NetSystem.OnConnectFailed(waitingForNatIntroductionSuccess ? "Nat Intro Failed" : LocalizationManager.GetTranslation("NoResponse", true, 0, true, false, null, null, true));
				}
				return;
			}
			if (NetSystem.IsServer)
			{
				NetGameServer.Update();
				return;
			}
			NetGameClient.Update();
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x0001CD49 File Offset: 0x0001AF49
		public static void Send()
		{
			if (!NetSystem.IsConnected)
			{
				return;
			}
			if (NetSystem.IsServer)
			{
				NetGameServer.Send();
				return;
			}
			NetGameClient.Send();
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x0001CD65 File Offset: 0x0001AF65
		public static void Recieve()
		{
			if (NetSystem.IsServer)
			{
				if (!NetSystem.IsConnected)
				{
					return;
				}
				NetGameServer.Recieve();
				return;
			}
			else
			{
				if (!NetSystem.is_initialized || (!NetSystem.IsConnected && !NetSystem.IsConnecting))
				{
					return;
				}
				NetGameClient.Recieve();
				return;
			}
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x0001CD97 File Offset: 0x0001AF97
		public static void IncrementTick()
		{
			NetSystem.current_tick++;
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x0001CDA5 File Offset: 0x0001AFA5
		public static void SetSendRate(int server_rate, int client_rate)
		{
			if (NetSystem.IsServer)
			{
				NetGameServer.SendRate = server_rate;
				NetSystem.gametime_update_tick_rate = server_rate / 60;
				return;
			}
			NetGameClient.SendRate = client_rate;
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x0001CDC4 File Offset: 0x0001AFC4
		public void WriteStream(int index, bool bit)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(bit);
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x0001CDEA File Offset: 0x0001AFEA
		public void WriteStream(int index, byte val)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val);
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x0001CE10 File Offset: 0x0001B010
		public void WriteStream(int index, char val)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val);
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x0001CE36 File Offset: 0x0001B036
		public void WriteStream(int index, short val)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val);
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x0001CE5C File Offset: 0x0001B05C
		public void WriteStream(int index, ushort val)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val);
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x0001CE82 File Offset: 0x0001B082
		public void WriteStream(int index, int val)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val);
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x0001CEA8 File Offset: 0x0001B0A8
		public void WriteStream(int index, uint val)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val);
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x0001CECE File Offset: 0x0001B0CE
		public void WriteStream(int index, long val)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val);
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x0001CEF4 File Offset: 0x0001B0F4
		public void WriteStream(int index, ulong val)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val);
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x0001CF1A File Offset: 0x0001B11A
		public void WriteStream(int index, float val)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val);
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x0001CF40 File Offset: 0x0001B140
		public void WriteStream(int index, double val)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val);
		}

		// Token: 0x0600296D RID: 10605 RVA: 0x0001CF66 File Offset: 0x0001B166
		public void WriteStream(int index, byte val, int bits)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val, bits);
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x0001CF8D File Offset: 0x0001B18D
		public void WriteStream(int index, char val, int bits)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val, bits);
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x0001CFB4 File Offset: 0x0001B1B4
		public void WriteStream(int index, short val, int bits)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val, bits);
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x0001CFDB File Offset: 0x0001B1DB
		public void WriteStream(int index, ushort val, int bits)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val, bits);
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x0001D002 File Offset: 0x0001B202
		public void WriteStream(int index, int val, int bits)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val, bits);
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x0001D029 File Offset: 0x0001B229
		public void WriteStream(int index, uint val, int bits)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val, bits);
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x0001D050 File Offset: 0x0001B250
		public void WriteStream(int index, long val, int bits)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val, bits);
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x0001D077 File Offset: 0x0001B277
		public void WriteStream(int index, ulong val, int bits)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val, bits);
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x0001D09E File Offset: 0x0001B29E
		public void WriteStream(int index, float val, int bits)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val, bits);
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x0001D0C5 File Offset: 0x0001B2C5
		public void WriteStream(int index, double val, int bits)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot write to stream, stream index out of bounds");
				return;
			}
			NetSystem.write_streams[index].Write(val, bits);
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x000F595C File Offset: 0x000F3B5C
		public static void FlushStream(int index, NetDeliveryMethod method)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot flush stream, stream index out of bounds");
				return;
			}
			if (NetSystem.IsServer)
			{
				NetGameServer.WriteStream(index, method, NetSystem.write_streams[index].GetDataCopy());
			}
			else
			{
				NetGameClient.WriteStream(index, method, NetSystem.write_streams[index].GetDataCopy());
			}
			NetSystem.write_streams[index].Reset();
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x0001D0EC File Offset: 0x0001B2EC
		public static void ConfigureStream(int index, NetSystem.RecieveStream callback, bool relay)
		{
			if (index <= 0 || index >= NetSystem.MAX_STREAM_COUNT)
			{
				Debug.LogError("Cannot configure stream, stream index out of bounds");
				return;
			}
			NetSystem.recieve_callbacks[index] = callback;
			NetSystem.stream_relay[index] = relay;
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x000F59BC File Offset: 0x000F3BBC
		public static void ReadStream(int index, NetIncomingMessage inc)
		{
			byte[] array = new byte[inc.LengthBytes - 1];
			inc.ReadBytes(array, 0, inc.LengthBytes - 1);
			if (NetSystem.IsServer && NetSystem.stream_relay[index])
			{
				NetGameServer.WriteStream(index, NetDeliveryMethod.ReliableOrdered, array);
			}
			if (NetSystem.recieve_callbacks[index] != null)
			{
				NetSystem.read_streams[index].Reset();
				NetSystem.read_streams[index].SetData(array, array.Length * 8);
				NetSystem.recieve_callbacks[index](NetSystem.GetPlayer(inc.SenderEndPoint), NetSystem.read_streams[index]);
			}
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x0001D115 File Offset: 0x0001B315
		public static void Spawn(INetComponent obj)
		{
			if (!NetSystem.IsConnected)
			{
				Debug.LogWarning("Must be connected to or hosting a server to spawn Network Objects!");
				return;
			}
			Debug.Log("Spawned Entity!");
			if (NetSystem.IsServer)
			{
				NetGameServer.Spawn(obj, null);
			}
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x0001D141 File Offset: 0x0001B341
		public static GameObject Spawn(string prefab_name, ushort ownerSlot, NetPlayer owner)
		{
			if (!NetSystem.IsConnected)
			{
				Debug.LogWarning("Must be connected to or hosting a server to spawn Network Objects!");
				return null;
			}
			if (NetSystem.IsServer)
			{
				return NetGameServer.Spawn(prefab_name, ownerSlot, owner);
			}
			return null;
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x0001D167 File Offset: 0x0001B367
		public static NetPrefab GetPrefab(string prefabName)
		{
			return NetGameServer.GetPrefab(prefabName);
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x0001D16F File Offset: 0x0001B36F
		public static GameObject Spawn(string prefab_name, Vector3 pos, ushort ownerSlot, NetPlayer owner)
		{
			return NetSystem.Spawn(prefab_name, pos, Quaternion.identity, ownerSlot, owner);
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x0001D17F File Offset: 0x0001B37F
		public static GameObject Spawn(string prefab_name, Vector3 pos, Quaternion rotation, ushort ownerSlot, NetPlayer owner = null)
		{
			if (!NetSystem.IsConnected)
			{
				Debug.LogWarning("Must be connected to or hosting a server to spawn Network Objects!");
				return null;
			}
			if (NetSystem.IsServer)
			{
				return NetGameServer.Spawn(prefab_name, pos, rotation, ownerSlot, owner);
			}
			return null;
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x0001D1A8 File Offset: 0x0001B3A8
		public static void Spawn(string prefab, Vector3 position, Quaternion rotation)
		{
			if (!NetSystem.initiated)
			{
				NetSystem.Initialize();
			}
			UnityEngine.Object.Instantiate(null);
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x0001D1BD File Offset: 0x0001B3BD
		public static void Kill(INetComponent obj)
		{
			if (!NetSystem.initiated)
			{
				NetSystem.Initialize();
			}
			if (NetSystem.IsServer)
			{
				NetGameServer.Kill(obj);
			}
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x000F5A48 File Offset: 0x000F3C48
		public static void Reset()
		{
			if (NetSystem.ConnectedToLobby != null)
			{
				Delegate[] invocationList = NetSystem.ConnectedToLobby.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					NetSystem.ConnectedToLobby -= (ConnectLobbyEventHandler)invocationList[i];
				}
			}
			if (NetSystem.PlayerDisconnected != null)
			{
				Delegate[] invocationList = NetSystem.PlayerDisconnected.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					NetSystem.PlayerDisconnected -= (PlayerDisconnectedEventHandler)invocationList[i];
				}
			}
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x000F5AB0 File Offset: 0x000F3CB0
		public static void FinishedLoading()
		{
			Debug.Log("Finishing Loading: " + NetSystem.status.ToString());
			if (NetSystem.IsServer)
			{
				NetGameServer.FinishedLoading();
				if (NetSystem.my_player.IsLoading)
				{
					NetSystem.my_player.IsLoading = false;
					Debug.Log("Host Loaded");
					NetSystem.OnHostLoaded();
					return;
				}
			}
			else
			{
				NetGameClient.FinishedLoading();
			}
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x0001D1D8 File Offset: 0x0001B3D8
		public static void SendRPC(INetComponent entity, string method_name, NetDeliveryMethod delivery, params object[] parameters)
		{
			if (NetSystem.IsServer)
			{
				NetSystem.SendRPC_Internal(entity, method_name, delivery, NetGameServer.GetConnections(), parameters);
				return;
			}
			NetSystem.SendRPC_Internal(entity, method_name, delivery, null, parameters);
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x000F5B14 File Offset: 0x000F3D14
		public static void SendRPCPlayers(INetComponent entity, string method_name, NetDeliveryMethod delivery, List<NetPlayer> players, params object[] parameters)
		{
			if (!NetSystem.IsServer)
			{
				Debug.LogError("SendRPCPlayer can only be called on the server");
				return;
			}
			List<NetConnection> list = new List<NetConnection>();
			for (int i = 0; i < players.Count; i++)
			{
				list.Add(players[i].Connection);
			}
			NetSystem.SendRPC_Internal(entity, method_name, delivery, list, parameters);
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x0001D1FA File Offset: 0x0001B3FA
		public static void SendRPC_Internal(INetComponent entity, string method_name, NetDeliveryMethod delivery, List<NetConnection> connections, params object[] parameters)
		{
			if (!NetSystem.IsConnected)
			{
				Debug.LogWarning("Cannot send rpc when not connected to or hosting a server!");
				return;
			}
			if (NetSystem.IsServer)
			{
				NetGameServer.SendRPC(entity, method_name, delivery, connections, null, parameters);
				return;
			}
			NetGameClient.SendRPC(entity, method_name, delivery, parameters);
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x0001D22C File Offset: 0x0001B42C
		public static void SendLobbySlotChange(ushort slot, bool observer)
		{
			if (NetSystem.IsServer)
			{
				if (NetSystem.ChangePlayerSlot(slot, observer, NetSystem.my_player))
				{
					NetGameServer.SetSlotsChanged(true);
					NetSystem.OnSlotsChanged();
					return;
				}
			}
			else
			{
				NetGameClient.SendLobbySlotChange(slot, observer);
			}
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x0001D256 File Offset: 0x0001B456
		public static void SendLobbyChatMessage(string msg)
		{
			if (NetSystem.IsConnected)
			{
				if (!NetSystem.IsServer)
				{
					NetGameClient.SendLobbyChatMessage(msg);
					return;
				}
				NetGameServer.RelayLobbyChatMessage(msg, NetSystem.my_player);
			}
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x0001D278 File Offset: 0x0001B478
		public static bool PlayerExists(ref IPEndPoint end_point)
		{
			return NetSystem.player_ip_map.ContainsKey(end_point);
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x000F5B68 File Offset: 0x000F3D68
		public static void AddPlayer(string name, ushort uid, byte slot, bool observer)
		{
			if (NetSystem.IsServer)
			{
				Debug.LogError("Cannot use AddPlayer on server, use CreatePlayer instead!");
				return;
			}
			NetPlayer netPlayer = new NetPlayer(name, (int)slot, observer, uid, null, null);
			if (NetSystem.player_uid_map.ContainsKey(netPlayer.UserID))
			{
				NetPlayer netPlayer2 = NetSystem.player_uid_map[netPlayer.UserID];
				Debug.LogError(string.Concat(new string[]
				{
					"cannot add player ",
					netPlayer.Name,
					" with uid ",
					netPlayer.UserID.ToString(),
					" the uid is already in use by ",
					netPlayer2.Name,
					"!"
				}));
				return;
			}
			NetSystem.player_uid_map.Add(uid, netPlayer);
			NetSystem.player_list.Add(netPlayer);
			if (observer)
			{
				NetSystem.observer_count++;
				NetSystem.observer_slots[(int)slot] = netPlayer;
			}
			else
			{
				NetSystem.player_count++;
				NetSystem.player_slots[(int)slot] = netPlayer;
			}
			NetSystem.OnPlayerConnected(netPlayer);
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x000F5C54 File Offset: 0x000F3E54
		public static void SetPlayer(NetPlayer new_player)
		{
			if (new_player == null)
			{
				Debug.Log("SetPlayer, player was null!");
				return;
			}
			if (NetSystem.IsServer)
			{
				Debug.LogError("Cannot use SetPlayer on server, use CreatePlayer instead!");
				return;
			}
			if (NetSystem.player_uid_map.ContainsKey(new_player.UserID))
			{
				NetPlayer netPlayer = NetSystem.player_uid_map[new_player.UserID];
				Debug.LogError(string.Concat(new string[]
				{
					"cannot add player ",
					new_player.Name,
					" with uid ",
					new_player.UserID.ToString(),
					" the uid is already in use by ",
					netPlayer.Name,
					"!"
				}));
				return;
			}
			NetSystem.player_uid_map.Add(new_player.UserID, new_player);
			NetSystem.player_list.Add(new_player);
			if (new_player.Observer)
			{
				NetSystem.observer_count++;
				NetSystem.observer_slots[new_player.Slot] = new_player;
				return;
			}
			NetSystem.player_count++;
			NetSystem.player_slots[new_player.Slot] = new_player;
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x000F5D50 File Offset: 0x000F3F50
		public static void SetPlayerSlot(NetPlayer player, int slot, bool observer)
		{
			if (player.Observer)
			{
				NetSystem.observer_count--;
				NetSystem.observer_slots[player.Slot] = null;
			}
			else
			{
				NetSystem.player_count--;
				NetSystem.player_slots[player.Slot] = null;
			}
			player.Slot = slot;
			player.Observer = observer;
			if (!observer)
			{
				NetSystem.player_count++;
				NetSystem.player_slots[slot] = player;
				return;
			}
			NetSystem.observer_count++;
			NetSystem.observer_slots[slot] = player;
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x000F5DD4 File Offset: 0x000F3FD4
		public static NetPlayer CreatePlayer(string name, NetConnection connection, IPEndPoint sender_endpoint)
		{
			ushort num = 0;
			bool flag = false;
			ushort freePlayerID = NetSystem.GetFreePlayerID();
			bool freeSlot = NetSystem.GetFreeSlot(ref num, ref flag);
			if (freePlayerID == 65535 || !freeSlot)
			{
				Debug.LogWarning("No free user id found, maximum connections reached.");
				return null;
			}
			NetPlayer netPlayer = new NetPlayer(name, (int)num, flag, freePlayerID, connection, sender_endpoint);
			NetSystem.player_ip_map.Add(sender_endpoint, netPlayer);
			NetSystem.player_uid_map.Add(freePlayerID, netPlayer);
			NetSystem.player_list.Add(netPlayer);
			if (flag)
			{
				NetSystem.observer_slots[(int)num] = netPlayer;
				NetSystem.observer_count++;
			}
			else
			{
				NetSystem.player_slots[(int)num] = netPlayer;
				NetSystem.player_count++;
			}
			Debug.Log("player_count = " + NetSystem.player_count.ToString());
			Debug.Log("player created and given uid of " + freePlayerID.ToString());
			return netPlayer;
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x000F5EA4 File Offset: 0x000F40A4
		public static void RemovePlayer(ushort player_id)
		{
			NetPlayer player = null;
			if (NetSystem.player_uid_map.TryGetValue(player_id, out player))
			{
				NetSystem.RemovePlayer(player);
				return;
			}
			Debug.LogWarning("RemovePlayer(uid), player was not found with uid!");
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x000F5ED4 File Offset: 0x000F40D4
		public static void RemovePlayer(NetPlayer player)
		{
			if (player == null)
			{
				Debug.LogWarning("RemovePlayer, player was null!");
				return;
			}
			if (player.Observer)
			{
				NetSystem.observer_slots[player.Slot] = null;
				NetSystem.observer_count--;
			}
			else
			{
				NetSystem.player_slots[player.Slot] = null;
				NetSystem.player_count--;
			}
			NetPlayer netPlayer = NetSystem.player_uid_map[0];
			if (NetSystem.IsServer)
			{
				NetSystem.player_ip_map.Remove(player.EndPoint);
				NetSystem.player_id_status[(int)player.UserID] = false;
				NetGameServer.NetObjectManager.ChangeOwner(player, NetSystem.MyPlayer);
			}
			else
			{
				NetGameServer.NetObjectManager.ChangeOwner(player, netPlayer);
			}
			for (int i = 0; i < GameManager.GetPlayerCount(); i++)
			{
				GamePlayer playerAt = GameManager.GetPlayerAt(i);
				if (playerAt.NetOwner == player)
				{
					playerAt.NetOwner = netPlayer;
					playerAt.IsAI = true;
					playerAt.IsLocalPlayer = (netPlayer == NetSystem.MyPlayer);
				}
			}
			NetSystem.player_uid_map.Remove(player.UserID);
			NetSystem.player_list.Remove(player);
			NetSystem.OnPlayerDisconnected(player);
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x000F5FDC File Offset: 0x000F41DC
		public static NetPlayer GetPlayer(ushort uid)
		{
			if (!NetSystem.IsConnected)
			{
				Debug.LogError("Must be connected to get player by id.");
				return null;
			}
			NetPlayer result = null;
			if (NetSystem.player_uid_map.TryGetValue(uid, out result))
			{
				return result;
			}
			Debug.LogWarning("Could not get player with uid " + uid.ToString());
			return null;
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x000F6028 File Offset: 0x000F4228
		public static NetPlayer GetPlayer(IPEndPoint endpoint)
		{
			if (!NetSystem.IsServer || !NetSystem.IsConnected)
			{
				Debug.LogError("Must be connected and server to get players by ip.");
				return null;
			}
			NetPlayer result = null;
			if (NetSystem.player_ip_map.TryGetValue(endpoint, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x0001D286 File Offset: 0x0001B486
		public static NetPlayer GetPlayerAtIndex(int index)
		{
			if (!NetSystem.IsConnected)
			{
				Debug.LogError("Must be connected to get player!");
				return null;
			}
			if (index >= 0 && index < NetSystem.player_list.Count)
			{
				return NetSystem.player_list[index];
			}
			return null;
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x0001D2B9 File Offset: 0x0001B4B9
		public static NetPlayer GetLobbySlot(int slot, bool observer)
		{
			if (!observer)
			{
				if (slot < 0 || slot >= NetSystem.max_players)
				{
					return null;
				}
				return NetSystem.player_slots[slot];
			}
			else
			{
				if (slot < 0 || slot >= NetSystem.max_observers)
				{
					return null;
				}
				return NetSystem.observer_slots[slot];
			}
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x000F6064 File Offset: 0x000F4264
		public static bool ChangePlayerSlot(ushort slot, bool observer, NetPlayer player)
		{
			bool result = false;
			if (observer)
			{
				for (int i = 0; i < NetSystem.max_observers; i++)
				{
					if (NetSystem.observer_slots[i] == null)
					{
						if (player.Observer)
						{
							NetSystem.observer_slots[player.Slot] = null;
						}
						else
						{
							NetSystem.player_slots[player.Slot] = null;
							NetSystem.observer_count++;
							NetSystem.player_count--;
						}
						player.Slot = i;
						player.Observer = observer;
						NetSystem.observer_slots[i] = player;
						result = true;
					}
				}
			}
			else if (NetSystem.player_slots[(int)slot] == null)
			{
				if (player.Observer)
				{
					NetSystem.observer_slots[player.Slot] = null;
					NetSystem.observer_count--;
					NetSystem.player_count++;
				}
				else
				{
					NetSystem.player_slots[player.Slot] = null;
				}
				player.Slot = (int)slot;
				player.Observer = observer;
				NetSystem.player_slots[(int)slot] = player;
				result = true;
			}
			return result;
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x000F6144 File Offset: 0x000F4344
		public static void SetGameMode(int new_game_mode)
		{
			NetSystem.game_mode = new_game_mode;
			NetSystem.max_players = 8;
			NetSystem.max_observers = 4;
			if (!NetSystem.IsServer)
			{
				NetSystem.player_slots = new NetPlayer[NetSystem.max_players];
				NetSystem.observer_slots = new NetPlayer[NetSystem.max_observers];
				return;
			}
			NetSystem.player_count = 1;
			NetSystem.observer_count = 0;
			if (NetSystem.player_slots != null && NetSystem.observer_slots != null)
			{
				NetPlayer[] array = new NetPlayer[NetSystem.max_players];
				NetPlayer[] array2 = new NetPlayer[NetSystem.max_observers];
				for (int i = 0; i < NetSystem.player_slots.Length; i++)
				{
					if (NetSystem.player_slots[i] != null)
					{
						if (NetSystem.max_players < i && array[i] == null)
						{
							NetSystem.player_slots[i].Observer = false;
							NetSystem.player_slots[i].Slot = i;
							array[i] = NetSystem.player_slots[i];
							NetSystem.player_count++;
						}
						else
						{
							for (int j = 0; j < NetSystem.max_players; j++)
							{
								if (array[j] == null)
								{
									NetSystem.player_slots[i].Observer = false;
									NetSystem.player_slots[i].Slot = j;
									array[j] = NetSystem.player_slots[i];
									NetSystem.player_count++;
								}
							}
							for (int k = 0; k < NetSystem.max_observers; k++)
							{
								if (array2[k] == null)
								{
									NetSystem.player_slots[i].Observer = true;
									NetSystem.player_slots[i].Slot = k;
									array2[k] = NetSystem.player_slots[i];
									NetSystem.observer_count++;
								}
							}
						}
					}
				}
				for (int l = 0; l < NetSystem.observer_slots.Length; l++)
				{
					if (NetSystem.observer_slots[l] != null)
					{
						for (int m = 0; m < NetSystem.max_observers; m++)
						{
							if (array2[m] == null)
							{
								NetSystem.observer_slots[l].Observer = true;
								NetSystem.observer_slots[l].Slot = m;
								array2[m] = NetSystem.observer_slots[l];
								NetSystem.observer_count++;
							}
						}
					}
				}
				NetSystem.player_slots = array;
				NetSystem.observer_slots = array2;
				return;
			}
			NetSystem.player_slots = new NetPlayer[NetSystem.max_players];
			NetSystem.observer_slots = new NetPlayer[NetSystem.max_observers];
			NetSystem.player_slots[0] = NetSystem.my_player;
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x0001D2E9 File Offset: 0x0001B4E9
		public static int GetGameMode()
		{
			return NetSystem.game_mode;
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x0001D2F0 File Offset: 0x0001B4F0
		public static void SetPlayerName(string name)
		{
			NetSystem.my_player.Name = name;
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x0001D2FD File Offset: 0x0001B4FD
		public static string GetPlayerName(ushort user_id)
		{
			if (NetSystem.player_uid_map.ContainsKey(user_id))
			{
				return NetSystem.player_list[(int)user_id].Name;
			}
			return "NAME_MISSING";
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x0001D322 File Offset: 0x0001B522
		public static bool IsServerFull()
		{
			return NetSystem.player_count >= NetSystem.max_players && NetSystem.observer_count >= NetSystem.max_observers;
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x0001D341 File Offset: 0x0001B541
		public static void SetLastConnectionError(string error)
		{
			NetSystem.last_connection_error = error;
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x000F6360 File Offset: 0x000F4560
		private static ushort GetFreePlayerID()
		{
			for (int i = 1; i < NetSystem.max_connections; i++)
			{
				if (!NetSystem.player_id_status[i])
				{
					NetSystem.player_id_status[i] = true;
					return (ushort)i;
				}
			}
			Debug.LogError("MAXIMUM CONNECTIONS RECHED : REACHED NO FREE USER ID'S EXIST!");
			return ushort.MaxValue;
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x000F63A0 File Offset: 0x000F45A0
		private static bool GetFreeSlot(ref ushort slot, ref bool observer)
		{
			for (int i = 0; i < NetSystem.max_players; i++)
			{
				if (NetSystem.player_slots[i] == null)
				{
					slot = (ushort)i;
					observer = false;
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002BDC RID: 11228
		public const int DEFAULT_SEND_RATE = 60;

		// Token: 0x04002BDD RID: 11229
		public const int STORE_SNAPSHOT_COUNT = 180;

		// Token: 0x04002BDE RID: 11230
		public const int SERVER_USER_ID = 0;

		// Token: 0x04002BDF RID: 11231
		public const int GAMETIME_UPDATE_RATE = 60;

		// Token: 0x04002BE0 RID: 11232
		public const float MAX_CONNECT_WAIT_TIME = 6f;

		// Token: 0x04002BE1 RID: 11233
		public const int MAXIMUM_TRANSMISSION_UNIT = 1190;

		// Token: 0x04002BE2 RID: 11234
		public static string[] MASTER_SERVER_IP_LIST = new string[]
		{
			"159.203.99.90",
			"178.128.184.210"
		};

		// Token: 0x04002BE3 RID: 11235
		public static List<IPEndPoint> MASTER_SERVERS = new List<IPEndPoint>();

		// Token: 0x04002BE4 RID: 11236
		public const int MASTER_SERVER_PORT = 58117;

		// Token: 0x04002BE5 RID: 11237
		public const bool NET_SIM = true;

		// Token: 0x04002BE6 RID: 11238
		public const float NET_SIM_LATENCY_MIN = 0.25f;

		// Token: 0x04002BE7 RID: 11239
		public const float NET_SIM_LATENCY_VARIATION = 0.1f;

		// Token: 0x04002BE8 RID: 11240
		public const float NET_SIM_LOSS = 0.2f;

		// Token: 0x04002BE9 RID: 11241
		public const float NET_SIM_DUPLICATES = 0.2f;

		// Token: 0x04002BEA RID: 11242
		private static string app_id = "SimpleGames";

		// Token: 0x04002BEB RID: 11243
		private static int max_players = 8;

		// Token: 0x04002BEC RID: 11244
		private static int max_observers = 4;

		// Token: 0x04002BED RID: 11245
		private static int max_possible_connections = 8;

		// Token: 0x04002BEE RID: 11246
		private static int max_connections = 4;

		// Token: 0x04002BEF RID: 11247
		private static int gametime_update_tick_rate = 1;

		// Token: 0x04002BF0 RID: 11248
		private static bool initiated = false;

		// Token: 0x04002BF1 RID: 11249
		private static bool is_initialized = false;

		// Token: 0x04002BF2 RID: 11250
		private static NetSystemStatus status = NetSystemStatus.Disconnected;

		// Token: 0x04002BF3 RID: 11251
		private static float connect_start_time;

		// Token: 0x04002BF4 RID: 11252
		private static int player_count = 0;

		// Token: 0x04002BF5 RID: 11253
		private static int observer_count = 0;

		// Token: 0x04002BF6 RID: 11254
		private static NetPlayer my_player;

		// Token: 0x04002BF7 RID: 11255
		private static bool[] player_id_status;

		// Token: 0x04002BF8 RID: 11256
		private static Dictionary<IPEndPoint, NetPlayer> player_ip_map;

		// Token: 0x04002BF9 RID: 11257
		private static Dictionary<ushort, NetPlayer> player_uid_map;

		// Token: 0x04002BFA RID: 11258
		private static List<NetPlayer> player_list;

		// Token: 0x04002BFB RID: 11259
		private static NetPlayer[] player_slots;

		// Token: 0x04002BFC RID: 11260
		private static NetPlayer[] observer_slots;

		// Token: 0x04002BFD RID: 11261
		private static int game_mode = 0;

		// Token: 0x04002BFE RID: 11262
		private static int current_tick = 0;

		// Token: 0x04002BFF RID: 11263
		private static float net_cur_time = 0f;

		// Token: 0x04002C00 RID: 11264
		private static ZPNetTime net_time;

		// Token: 0x04002C01 RID: 11265
		private static string last_connection_error;

		// Token: 0x04002C0D RID: 11277
		private static int MAX_STREAM_COUNT = 4;

		// Token: 0x04002C0E RID: 11278
		private static ZPBitStream[] write_streams = new ZPBitStream[NetSystem.MAX_STREAM_COUNT];

		// Token: 0x04002C0F RID: 11279
		private static ZPBitStream[] read_streams = new ZPBitStream[NetSystem.MAX_STREAM_COUNT];

		// Token: 0x04002C10 RID: 11280
		private static NetSystem.RecieveStream[] recieve_callbacks = new NetSystem.RecieveStream[NetSystem.MAX_STREAM_COUNT];

		// Token: 0x04002C11 RID: 11281
		private static bool[] stream_relay = new bool[NetSystem.MAX_STREAM_COUNT];

		// Token: 0x02000631 RID: 1585
		// (Invoke) Token: 0x0600299E RID: 10654
		public delegate void RecieveStream(NetPlayer sender, ZPBitStream stream);
	}
}
