using System;

namespace ZP.Net
{
	// Token: 0x02000621 RID: 1569
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class NetSend : Attribute
	{
		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x060028E8 RID: 10472 RVA: 0x0001CAED File Offset: 0x0001ACED
		public short Bits
		{
			get
			{
				return this.bits;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x060028E9 RID: 10473 RVA: 0x0001CAF5 File Offset: 0x0001ACF5
		public NetSendFlags Flags
		{
			get
			{
				return this.flags;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x060028EA RID: 10474 RVA: 0x0001CAFD File Offset: 0x0001ACFD
		public NetSendOwner Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x060028EB RID: 10475 RVA: 0x0001CB05 File Offset: 0x0001AD05
		public NetSend(short _bits = -1, NetSendOwner _owner = NetSendOwner.SERVER, NetSendFlags _flags = NetSendFlags.NONE)
		{
			this.bits = _bits;
			this.flags = _flags;
			this.owner = _owner;
		}

		// Token: 0x04002BC9 RID: 11209
		private short bits;

		// Token: 0x04002BCA RID: 11210
		private NetSendOwner owner;

		// Token: 0x04002BCB RID: 11211
		private NetSendFlags flags;
	}
}
