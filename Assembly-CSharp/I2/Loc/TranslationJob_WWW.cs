using System;
using UnityEngine.Networking;

namespace I2.Loc
{
	// Token: 0x020007FE RID: 2046
	public class TranslationJob_WWW : TranslationJob
	{
		// Token: 0x06003A02 RID: 14850 RVA: 0x0002744D File Offset: 0x0002564D
		public override void Dispose()
		{
			if (this.www != null)
			{
				this.www.Dispose();
			}
			this.www = null;
		}

		// Token: 0x04003837 RID: 14391
		public UnityWebRequest www;
	}
}
