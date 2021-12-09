using System;

namespace ZP.Net
{
	// Token: 0x02000617 RID: 1559
	public class NetRPC : Attribute
	{
		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x060028D1 RID: 10449 RVA: 0x0001CA0F File Offset: 0x0001AC0F
		public bool Relay
		{
			get
			{
				return this.relay;
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x060028D2 RID: 10450 RVA: 0x0001CA17 File Offset: 0x0001AC17
		public NetRPCSecurity Send
		{
			get
			{
				return this.send;
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x060028D3 RID: 10451 RVA: 0x0001CA1F File Offset: 0x0001AC1F
		public NetRPCSecurity Recieve
		{
			get
			{
				return this.recieve;
			}
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x0001CA27 File Offset: 0x0001AC27
		public NetRPC(bool _relay = true, NetRPCSecurity _send = NetRPCSecurity.ALL, NetRPCSecurity _recieve = NetRPCSecurity.ALL)
		{
			this.relay = _relay;
			this.send = _send;
			this.recieve = _recieve;
		}

		// Token: 0x04002B86 RID: 11142
		private bool relay = true;

		// Token: 0x04002B87 RID: 11143
		private NetRPCSecurity send = NetRPCSecurity.ALL;

		// Token: 0x04002B88 RID: 11144
		private NetRPCSecurity recieve = NetRPCSecurity.ALL;
	}
}
