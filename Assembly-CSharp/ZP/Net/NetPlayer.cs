using System;
using System.Net;
using Lidgren.Network;

namespace ZP.Net
{
	// Token: 0x02000616 RID: 1558
	public class NetPlayer
	{
		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060028B2 RID: 10418 RVA: 0x0001C900 File Offset: 0x0001AB00
		// (set) Token: 0x060028B3 RID: 10419 RVA: 0x0001C908 File Offset: 0x0001AB08
		public bool SentFullSnapshot
		{
			get
			{
				return this.sentFullSnapshot;
			}
			set
			{
				this.sentFullSnapshot = value;
			}
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x000F48BC File Offset: 0x000F2ABC
		public NetPlayer(string _name, int _slot, bool _observer, ushort _user_id, NetConnection _net_connection, IPEndPoint _endpoint)
		{
			this.name = _name;
			this.slot = _slot;
			this.observer = _observer;
			this.user_id = _user_id;
			this.net_connection = _net_connection;
			this.endpoint = _endpoint;
			this.last_ack = -1;
			this.last_sent_snapshot = int.MinValue;
			this.last_full_snapshot = -1;
			this.last_snapshot_tick = -1;
			this.approved = false;
			this.is_loading = false;
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060028B5 RID: 10421 RVA: 0x0001C911 File Offset: 0x0001AB11
		// (set) Token: 0x060028B6 RID: 10422 RVA: 0x0001C919 File Offset: 0x0001AB19
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060028B7 RID: 10423 RVA: 0x0001C922 File Offset: 0x0001AB22
		// (set) Token: 0x060028B8 RID: 10424 RVA: 0x0001C92A File Offset: 0x0001AB2A
		public int Slot
		{
			get
			{
				return this.slot;
			}
			set
			{
				this.slot = value;
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060028B9 RID: 10425 RVA: 0x0001C933 File Offset: 0x0001AB33
		// (set) Token: 0x060028BA RID: 10426 RVA: 0x0001C93B File Offset: 0x0001AB3B
		public bool Observer
		{
			get
			{
				return this.observer;
			}
			set
			{
				this.observer = value;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060028BB RID: 10427 RVA: 0x0001C944 File Offset: 0x0001AB44
		// (set) Token: 0x060028BC RID: 10428 RVA: 0x0001C94C File Offset: 0x0001AB4C
		public ushort UserID
		{
			get
			{
				return this.user_id;
			}
			set
			{
				this.user_id = value;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060028BD RID: 10429 RVA: 0x0001C955 File Offset: 0x0001AB55
		// (set) Token: 0x060028BE RID: 10430 RVA: 0x0001C95D File Offset: 0x0001AB5D
		public NetConnection Connection
		{
			get
			{
				return this.net_connection;
			}
			set
			{
				this.net_connection = value;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060028BF RID: 10431 RVA: 0x0001C966 File Offset: 0x0001AB66
		// (set) Token: 0x060028C0 RID: 10432 RVA: 0x0001C96E File Offset: 0x0001AB6E
		public IPEndPoint EndPoint
		{
			get
			{
				return this.endpoint;
			}
			set
			{
				this.endpoint = value;
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060028C1 RID: 10433 RVA: 0x0001C977 File Offset: 0x0001AB77
		// (set) Token: 0x060028C2 RID: 10434 RVA: 0x0001C97F File Offset: 0x0001AB7F
		public int LastAck
		{
			get
			{
				return this.last_ack;
			}
			set
			{
				this.last_ack = value;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060028C3 RID: 10435 RVA: 0x0001C988 File Offset: 0x0001AB88
		// (set) Token: 0x060028C4 RID: 10436 RVA: 0x0001C990 File Offset: 0x0001AB90
		public int LastSentSnapshot
		{
			get
			{
				return this.last_sent_snapshot;
			}
			set
			{
				this.last_sent_snapshot = value;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060028C5 RID: 10437 RVA: 0x0001C999 File Offset: 0x0001AB99
		// (set) Token: 0x060028C6 RID: 10438 RVA: 0x0001C9A1 File Offset: 0x0001ABA1
		public int LastFullSnapshot
		{
			get
			{
				return this.last_full_snapshot;
			}
			set
			{
				this.last_full_snapshot = value;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060028C7 RID: 10439 RVA: 0x0001C9AA File Offset: 0x0001ABAA
		// (set) Token: 0x060028C8 RID: 10440 RVA: 0x0001C9B2 File Offset: 0x0001ABB2
		public int LastSnapshotTick
		{
			get
			{
				return this.last_snapshot_tick;
			}
			set
			{
				this.last_snapshot_tick = value;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060028C9 RID: 10441 RVA: 0x0001C9BB File Offset: 0x0001ABBB
		// (set) Token: 0x060028CA RID: 10442 RVA: 0x0001C9C3 File Offset: 0x0001ABC3
		public int LastClientSnapshot
		{
			get
			{
				return this.last_client_snapshot;
			}
			set
			{
				if (value != this.last_client_snapshot)
				{
					this.last_client_snapshot_changed = true;
					this.last_client_snapshot = value;
				}
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060028CB RID: 10443 RVA: 0x0001C9DC File Offset: 0x0001ABDC
		// (set) Token: 0x060028CC RID: 10444 RVA: 0x0001C9E4 File Offset: 0x0001ABE4
		public bool LastClientSnapshotChanged
		{
			get
			{
				return this.last_client_snapshot_changed;
			}
			set
			{
				this.last_client_snapshot_changed = value;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060028CD RID: 10445 RVA: 0x0001C9ED File Offset: 0x0001ABED
		// (set) Token: 0x060028CE RID: 10446 RVA: 0x0001C9F5 File Offset: 0x0001ABF5
		public bool IsLoading
		{
			get
			{
				return this.is_loading;
			}
			set
			{
				this.is_loading = value;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x060028CF RID: 10447 RVA: 0x0001C9FE File Offset: 0x0001ABFE
		// (set) Token: 0x060028D0 RID: 10448 RVA: 0x0001CA06 File Offset: 0x0001AC06
		public bool Approved
		{
			get
			{
				return this.approved;
			}
			set
			{
				this.approved = value;
			}
		}

		// Token: 0x04002B77 RID: 11127
		private string name;

		// Token: 0x04002B78 RID: 11128
		private int slot;

		// Token: 0x04002B79 RID: 11129
		private bool observer;

		// Token: 0x04002B7A RID: 11130
		private ushort user_id;

		// Token: 0x04002B7B RID: 11131
		private NetConnection net_connection;

		// Token: 0x04002B7C RID: 11132
		private int last_ack;

		// Token: 0x04002B7D RID: 11133
		private int last_sent_snapshot;

		// Token: 0x04002B7E RID: 11134
		private int last_full_snapshot;

		// Token: 0x04002B7F RID: 11135
		private int last_snapshot_tick;

		// Token: 0x04002B80 RID: 11136
		private int last_client_snapshot;

		// Token: 0x04002B81 RID: 11137
		private bool last_client_snapshot_changed;

		// Token: 0x04002B82 RID: 11138
		private bool approved;

		// Token: 0x04002B83 RID: 11139
		private bool is_loading;

		// Token: 0x04002B84 RID: 11140
		private bool sentFullSnapshot;

		// Token: 0x04002B85 RID: 11141
		private IPEndPoint endpoint;
	}
}
