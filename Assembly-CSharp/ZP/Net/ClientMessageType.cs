﻿using System;

namespace ZP.Net
{
	// Token: 0x02000600 RID: 1536
	public enum ClientMessageType
	{
		// Token: 0x04002ACA RID: 10954
		SNAPSHOT_FULL,
		// Token: 0x04002ACB RID: 10955
		SNAPSHOT_DELTA,
		// Token: 0x04002ACC RID: 10956
		SEND_RPC,
		// Token: 0x04002ACD RID: 10957
		SNAPSHOT_ACK = 4,
		// Token: 0x04002ACE RID: 10958
		FINISHED_LOADING = 8,
		// Token: 0x04002ACF RID: 10959
		LOBBY_CHAT_MSG = 10,
		// Token: 0x04002AD0 RID: 10960
		REQUEST_SLOT_CHANGE = 12,
		// Token: 0x04002AD1 RID: 10961
		STREAM_0 = 14,
		// Token: 0x04002AD2 RID: 10962
		STREAM_1 = 16,
		// Token: 0x04002AD3 RID: 10963
		STREAM_2 = 18,
		// Token: 0x04002AD4 RID: 10964
		STREAM_3 = 20
	}
}