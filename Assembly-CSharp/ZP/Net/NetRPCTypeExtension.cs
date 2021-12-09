using System;

namespace ZP.Net
{
	// Token: 0x0200061E RID: 1566
	internal static class NetRPCTypeExtension
	{
		// Token: 0x060028E7 RID: 10471 RVA: 0x000F4D3C File Offset: 0x000F2F3C
		public static int GetByteSize(this NetRPCType type)
		{
			switch (type)
			{
			case NetRPCType.UNKNOWN:
				return 0;
			case NetRPCType.BOOL:
				return 1;
			case NetRPCType.BYTE:
				return 1;
			case NetRPCType.CHAR:
				return 2;
			case NetRPCType.SHORT:
				return 2;
			case NetRPCType.USHORT:
				return 2;
			case NetRPCType.INT:
				return 4;
			case NetRPCType.UINT:
				return 4;
			case NetRPCType.LONG:
				return 8;
			case NetRPCType.ULONG:
				return 8;
			case NetRPCType.FLOAT:
				return 4;
			case NetRPCType.DOUBLE:
				return 8;
			case NetRPCType.STRING:
				return 4;
			case NetRPCType.VECTOR2:
				return 8;
			case NetRPCType.VECTOR3:
				return 12;
			case NetRPCType.ARRAY_BOOL:
				return 3;
			case NetRPCType.ARRAY_BYTE:
				return 3;
			case NetRPCType.ARRAY_CHAR:
				return 4;
			case NetRPCType.ARRAY_SHORT:
				return 4;
			case NetRPCType.ARRAY_USHORT:
				return 4;
			case NetRPCType.ARRAY_INT:
				return 6;
			case NetRPCType.ARRAY_UINT:
				return 6;
			case NetRPCType.ARRAY_LONG:
				return 10;
			case NetRPCType.ARRAY_ULONG:
				return 10;
			case NetRPCType.ARRAY_FLOAT:
				return 6;
			case NetRPCType.ARRAY_DOUBLE:
				return 10;
			case NetRPCType.ARRAY_STRING:
				return 6;
			case NetRPCType.ARRAY_VECTOR2:
				return 10;
			case NetRPCType.ARRAY_VECTOR3:
				return 14;
			default:
				return 0;
			}
		}
	}
}
