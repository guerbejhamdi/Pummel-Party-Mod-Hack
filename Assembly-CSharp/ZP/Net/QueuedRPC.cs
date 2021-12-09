using System;

namespace ZP.Net
{
	// Token: 0x02000612 RID: 1554
	public class QueuedRPC
	{
		// Token: 0x06002888 RID: 10376 RVA: 0x0001C847 File Offset: 0x0001AA47
		public QueuedRPC(NetPlayer _sender, ushort _entity_id, byte _rpc_id, byte[] _data, int _bit_length)
		{
			this.sender = _sender;
			this.entity_id = _entity_id;
			this.rpc_id = _rpc_id;
			this.data = _data;
			this.bit_length = _bit_length;
			this.wait_time = 0f;
		}

		// Token: 0x04002B50 RID: 11088
		public NetPlayer sender;

		// Token: 0x04002B51 RID: 11089
		public ushort entity_id;

		// Token: 0x04002B52 RID: 11090
		public byte rpc_id;

		// Token: 0x04002B53 RID: 11091
		public byte[] data;

		// Token: 0x04002B54 RID: 11092
		public int bit_length;

		// Token: 0x04002B55 RID: 11093
		public float wait_time;
	}
}
