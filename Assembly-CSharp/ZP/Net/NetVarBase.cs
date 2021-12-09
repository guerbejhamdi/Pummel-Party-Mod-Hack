using System;

namespace ZP.Net
{
	// Token: 0x02000634 RID: 1588
	public class NetVarBase : INetVar
	{
		// Token: 0x060029AB RID: 10667 RVA: 0x0001D3A5 File Offset: 0x0001B5A5
		public NetVarBase()
		{
			this.flags = NetSendFlags.NONE;
			this.bits = -1;
			this.changed = false;
			this.last_change_tick = -1;
			this.last_size_change_tick = -1;
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x060029AC RID: 10668 RVA: 0x0001D3D0 File Offset: 0x0001B5D0
		// (set) Token: 0x060029AD RID: 10669 RVA: 0x0001D3D8 File Offset: 0x0001B5D8
		public RecieveProxy Recieve
		{
			get
			{
				return this.recieve_method;
			}
			set
			{
				this.recieve_method = value;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x060029AE RID: 10670 RVA: 0x0001D3E1 File Offset: 0x0001B5E1
		// (set) Token: 0x060029AF RID: 10671 RVA: 0x0001D3E9 File Offset: 0x0001B5E9
		public bool DidRecieve
		{
			get
			{
				return this.did_recieve;
			}
			set
			{
				this.did_recieve = value;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x060029B0 RID: 10672 RVA: 0x0001D3F2 File Offset: 0x0001B5F2
		// (set) Token: 0x060029B1 RID: 10673 RVA: 0x0001D3FA File Offset: 0x0001B5FA
		public SendProxy Send
		{
			get
			{
				return this.send_method;
			}
			set
			{
				this.send_method = value;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x060029B2 RID: 10674 RVA: 0x0001D403 File Offset: 0x0001B603
		// (set) Token: 0x060029B3 RID: 10675 RVA: 0x0001D40B File Offset: 0x0001B60B
		public ArrayRecieveProxy ArrayRecieve
		{
			get
			{
				return this.array_recieve_method;
			}
			set
			{
				this.array_recieve_method = value;
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x0001D414 File Offset: 0x0001B614
		// (set) Token: 0x060029B5 RID: 10677 RVA: 0x0001D41C File Offset: 0x0001B61C
		public ArrayResizeProxy ArrayResizeRecieve
		{
			get
			{
				return this.array_resize_method;
			}
			set
			{
				this.array_resize_method = value;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x060029B6 RID: 10678 RVA: 0x0001D425 File Offset: 0x0001B625
		// (set) Token: 0x060029B7 RID: 10679 RVA: 0x0001D42D File Offset: 0x0001B62D
		public NetSendFlags Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				this.flags = value;
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x060029B8 RID: 10680 RVA: 0x0001D436 File Offset: 0x0001B636
		// (set) Token: 0x060029B9 RID: 10681 RVA: 0x0001D43E File Offset: 0x0001B63E
		public int Bits
		{
			get
			{
				return this.bits;
			}
			set
			{
				this.bits = value;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x060029BA RID: 10682 RVA: 0x000053AE File Offset: 0x000035AE
		public virtual object Object
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x060029BB RID: 10683 RVA: 0x0000539F File Offset: 0x0000359F
		public virtual int Length
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x060029BC RID: 10684 RVA: 0x0000539F File Offset: 0x0000359F
		public virtual bool SizeChanged
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x060029BD RID: 10685 RVA: 0x0000539F File Offset: 0x0000359F
		public virtual bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x060029BE RID: 10686 RVA: 0x0001D447 File Offset: 0x0001B647
		public bool HasChanged
		{
			get
			{
				return this.changed;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x060029BF RID: 10687 RVA: 0x0001D44F File Offset: 0x0001B64F
		public NetVarType VarType
		{
			get
			{
				return this.netvar_type;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x060029C0 RID: 10688 RVA: 0x0001D457 File Offset: 0x0001B657
		// (set) Token: 0x060029C1 RID: 10689 RVA: 0x0001D45F File Offset: 0x0001B65F
		public int LastChangeTick
		{
			get
			{
				return this.last_change_tick;
			}
			set
			{
				this.last_change_tick = value;
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x060029C2 RID: 10690 RVA: 0x0001D468 File Offset: 0x0001B668
		// (set) Token: 0x060029C3 RID: 10691 RVA: 0x0001D470 File Offset: 0x0001B670
		public int LastSizeChangeTick
		{
			get
			{
				return this.last_size_change_tick;
			}
			set
			{
				this.last_size_change_tick = value;
			}
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x0001D479 File Offset: 0x0001B679
		public virtual void ResetDelta()
		{
			this.changed = false;
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void SnapshotSet(object _val)
		{
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void SnapshotResize(int _size)
		{
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void AlwaysSendSet(object _val)
		{
		}

		// Token: 0x060029C8 RID: 10696 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void Resize(int size)
		{
		}

		// Token: 0x04002C1A RID: 11290
		protected RecieveProxy recieve_method;

		// Token: 0x04002C1B RID: 11291
		protected SendProxy send_method;

		// Token: 0x04002C1C RID: 11292
		protected ArrayRecieveProxy array_recieve_method;

		// Token: 0x04002C1D RID: 11293
		protected ArrayResizeProxy array_resize_method;

		// Token: 0x04002C1E RID: 11294
		protected NetSendFlags flags;

		// Token: 0x04002C1F RID: 11295
		protected int bits;

		// Token: 0x04002C20 RID: 11296
		protected bool changed;

		// Token: 0x04002C21 RID: 11297
		protected int last_change_tick;

		// Token: 0x04002C22 RID: 11298
		protected int last_size_change_tick;

		// Token: 0x04002C23 RID: 11299
		protected bool did_recieve;

		// Token: 0x04002C24 RID: 11300
		protected NetVarType netvar_type;
	}
}
