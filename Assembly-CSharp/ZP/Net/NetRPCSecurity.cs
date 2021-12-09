using System;

namespace ZP.Net
{
	// Token: 0x0200061C RID: 1564
	[Flags]
	public enum NetRPCSecurity
	{
		// Token: 0x04002B9F RID: 11167
		PROXY = 1,
		// Token: 0x04002BA0 RID: 11168
		OWNER = 2,
		// Token: 0x04002BA1 RID: 11169
		SERVER = 4,
		// Token: 0x04002BA2 RID: 11170
		ALL = 8
	}
}
