using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZP
{
	// Token: 0x020005E9 RID: 1513
	public static class ZPSystem
	{
		// Token: 0x060026AF RID: 9903 RVA: 0x0001B95D File Offset: 0x00019B5D
		public static void Initialize()
		{
			if (!ZPSystem.created)
			{
				ZPSystem.create_confuncs();
				ZPSystem.create_convars();
				ZPSystem.read_convars(ZPSystem.con_var_file);
				ZPSystem.created = true;
			}
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x0001B981 File Offset: 0x00019B81
		public static void shutdown()
		{
			ZPSystem.write_convars(ZPSystem.con_var_file);
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x0000398C File Offset: 0x00001B8C
		public static void console_input(string input)
		{
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x0001B98E File Offset: 0x00019B8E
		private static void create_confuncs()
		{
			ZPSystem.con_funcs = new List<ConFunc>();
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x000053AE File Offset: 0x000035AE
		public static ConVar get_convar(string name)
		{
			return null;
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x0000398C File Offset: 0x00001B8C
		public static void set_convar(string name, UnityEngine.Object value)
		{
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x00005651 File Offset: 0x00003851
		public static bool write_convars(string file)
		{
			return true;
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x00005651 File Offset: 0x00003851
		public static bool read_convars(string file)
		{
			return true;
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x0001B99A File Offset: 0x00019B9A
		public static void create_convars()
		{
			ZPSystem.con_vars = new List<ConVar>();
		}

		// Token: 0x04002A6D RID: 10861
		private static bool created = false;

		// Token: 0x04002A6E RID: 10862
		private static List<ConFunc> con_funcs;

		// Token: 0x04002A6F RID: 10863
		private static List<ConVar> con_vars;

		// Token: 0x04002A70 RID: 10864
		private static string con_var_file = "";
	}
}
