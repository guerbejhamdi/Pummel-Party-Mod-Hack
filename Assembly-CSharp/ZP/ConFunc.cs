using System;

namespace ZP
{
	// Token: 0x020005E7 RID: 1511
	public class ConFunc
	{
		// Token: 0x060026AC RID: 9900 RVA: 0x0001B940 File Offset: 0x00019B40
		public ConFunc(string _name, ConFuncDelegate _func)
		{
			this.name = _name;
			this.func = _func;
		}

		// Token: 0x04002A68 RID: 10856
		public string name;

		// Token: 0x04002A69 RID: 10857
		public ConFuncDelegate func;
	}
}
