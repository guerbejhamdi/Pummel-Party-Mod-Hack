using System;

namespace ZP.Net
{
	// Token: 0x02000636 RID: 1590
	internal static class NetVarTypeExtension
	{
		// Token: 0x060029C9 RID: 10697 RVA: 0x000F6594 File Offset: 0x000F4794
		public static int GetBitSize(this NetVarType type)
		{
			switch (type)
			{
			case NetVarType.BOOL:
				return 1;
			case NetVarType.BYTE:
				return 8;
			case NetVarType.CHAR:
				return 16;
			case NetVarType.SHORT:
				return 16;
			case NetVarType.USHORT:
				return 16;
			case NetVarType.INT:
				return 32;
			case NetVarType.UINT:
				return 32;
			case NetVarType.LONG:
				return 64;
			case NetVarType.ULONG:
				return 64;
			case NetVarType.FLOAT:
				return 32;
			case NetVarType.DOUBLE:
				return 64;
			case NetVarType.STRING:
				return 16;
			case NetVarType.VEC2:
				return 32;
			case NetVarType.VEC3:
				return 32;
			case NetVarType.HALFVEC2:
				return 16;
			case NetVarType.HALFVEC3:
				return 16;
			case NetVarType.ARRAY_BOOL:
				return 1;
			case NetVarType.ARRAY_BYTE:
				return 8;
			case NetVarType.ARRAY_CHAR:
				return 16;
			case NetVarType.ARRAY_SHORT:
				return 16;
			case NetVarType.ARRAY_USHORT:
				return 16;
			case NetVarType.ARRAY_INT:
				return 32;
			case NetVarType.ARRAY_UINT:
				return 32;
			case NetVarType.ARRAY_LONG:
				return 64;
			case NetVarType.ARRAY_ULONG:
				return 64;
			case NetVarType.ARRAY_FLOAT:
				return 32;
			case NetVarType.ARRAY_DOUBLE:
				return 64;
			case NetVarType.ARRAY_STRING:
				return 16;
			case NetVarType.ARRAY_VEC2:
				return 32;
			case NetVarType.ARRAY_VEC3:
				return 32;
			default:
				return 0;
			}
		}
	}
}
