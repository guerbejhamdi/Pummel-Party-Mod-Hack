using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000838 RID: 2104
	public class TermsPopup : PropertyAttribute
	{
		// Token: 0x06003BCB RID: 15307 RVA: 0x00028183 File Offset: 0x00026383
		public TermsPopup(string filter = "")
		{
			this.Filter = filter;
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06003BCC RID: 15308 RVA: 0x00028192 File Offset: 0x00026392
		// (set) Token: 0x06003BCD RID: 15309 RVA: 0x0002819A File Offset: 0x0002639A
		public string Filter { get; private set; }
	}
}
