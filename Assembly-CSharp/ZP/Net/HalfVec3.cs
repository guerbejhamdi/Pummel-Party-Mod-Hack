using System;

namespace ZP.Net
{
	// Token: 0x020005F4 RID: 1524
	public class HalfVec3
	{
		// Token: 0x06002774 RID: 10100 RVA: 0x0001C0A1 File Offset: 0x0001A2A1
		public HalfVec3()
		{
			this.hx = 0;
			this.hy = 0;
			this.hz = 0;
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x0001C0BE File Offset: 0x0001A2BE
		public HalfVec3(ushort _x, ushort _y, ushort _z)
		{
			this.hx = _x;
			this.hy = _y;
			this.hz = _z;
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06002776 RID: 10102 RVA: 0x0001C0DB File Offset: 0x0001A2DB
		// (set) Token: 0x06002777 RID: 10103 RVA: 0x0001C0E3 File Offset: 0x0001A2E3
		public ushort x
		{
			get
			{
				return this.hx;
			}
			set
			{
				this.hx = value;
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06002778 RID: 10104 RVA: 0x0001C0EC File Offset: 0x0001A2EC
		// (set) Token: 0x06002779 RID: 10105 RVA: 0x0001C0F4 File Offset: 0x0001A2F4
		public ushort y
		{
			get
			{
				return this.hy;
			}
			set
			{
				this.hy = value;
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x0600277A RID: 10106 RVA: 0x0001C0FD File Offset: 0x0001A2FD
		// (set) Token: 0x0600277B RID: 10107 RVA: 0x0001C105 File Offset: 0x0001A305
		public ushort z
		{
			get
			{
				return this.hz;
			}
			set
			{
				this.hz = value;
			}
		}

		// Token: 0x04002AA3 RID: 10915
		private ushort hx;

		// Token: 0x04002AA4 RID: 10916
		private ushort hy;

		// Token: 0x04002AA5 RID: 10917
		private ushort hz;
	}
}
