using System;

namespace I2.Loc
{
	// Token: 0x0200082A RID: 2090
	public class LocalizeTargetDesc_Child : LocalizeTargetDesc<LocalizeTarget_UnityStandard_Child>
	{
		// Token: 0x06003B5F RID: 15199 RVA: 0x00027E6E File Offset: 0x0002606E
		public override bool CanLocalize(Localize cmp)
		{
			return cmp.transform.childCount > 1;
		}
	}
}
