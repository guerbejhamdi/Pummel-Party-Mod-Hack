using System;
using System.Reflection;

namespace ZP.Net
{
	// Token: 0x0200060F RID: 1551
	public class NetFieldDefinition
	{
		// Token: 0x06002884 RID: 10372 RVA: 0x0001C7C9 File Offset: 0x0001A9C9
		public NetFieldDefinition()
		{
			this.field_type = TypeCode.Object;
			this.owner = NetSendOwner.NONE;
			this.field_info = null;
			this.bits = -1;
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x0001C7ED File Offset: 0x0001A9ED
		public NetFieldDefinition(TypeCode _field_type, NetSendOwner _owner, FieldInfo _field_info, int _bits, NetSendFlags _flags)
		{
			this.field_type = _field_type;
			this.owner = _owner;
			this.field_info = _field_info;
			this.bits = _bits;
			this.flags = _flags;
		}

		// Token: 0x04002B36 RID: 11062
		public TypeCode field_type;

		// Token: 0x04002B37 RID: 11063
		public NetSendOwner owner;

		// Token: 0x04002B38 RID: 11064
		public FieldInfo field_info;

		// Token: 0x04002B39 RID: 11065
		public int bits;

		// Token: 0x04002B3A RID: 11066
		public NetSendFlags flags;
	}
}
