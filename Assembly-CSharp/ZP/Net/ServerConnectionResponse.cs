using System;
using System.ComponentModel;

namespace ZP.Net
{
	// Token: 0x02000606 RID: 1542
	public enum ServerConnectionResponse
	{
		// Token: 0x04002B00 RID: 11008
		[Description("Server is full.")]
		SERVER_FULL,
		// Token: 0x04002B01 RID: 11009
		[Description("Maximum connections reached.")]
		MAXIMUM_CONNECTIONS,
		// Token: 0x04002B02 RID: 11010
		[Description("Kicked from server.")]
		KICKED
	}
}
