using System;
using System.Collections.Generic;

namespace ZP.Net
{
	// Token: 0x0200060E RID: 1550
	public class NetTypeDefinition
	{
		// Token: 0x06002883 RID: 10371 RVA: 0x0001C79A File Offset: 0x0001A99A
		public NetTypeDefinition()
		{
			this.net_type_id = 0;
			this.is_behaviour = false;
			this.fields = null;
			this.rpc_id_table = null;
			this.rpc_string_table = new Dictionary<string, NetRPCDefinition>();
		}

		// Token: 0x04002B30 RID: 11056
		public ushort net_type_id;

		// Token: 0x04002B31 RID: 11057
		public Type object_type;

		// Token: 0x04002B32 RID: 11058
		public bool is_behaviour;

		// Token: 0x04002B33 RID: 11059
		public NetFieldDefinition[] fields;

		// Token: 0x04002B34 RID: 11060
		public NetRPCDefinition[] rpc_id_table;

		// Token: 0x04002B35 RID: 11061
		public Dictionary<string, NetRPCDefinition> rpc_string_table;
	}
}
