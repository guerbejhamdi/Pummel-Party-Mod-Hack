using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000822 RID: 2082
	public abstract class ILocalizeTarget : ScriptableObject
	{
		// Token: 0x06003B28 RID: 15144
		public abstract bool IsValid(Localize cmp);

		// Token: 0x06003B29 RID: 15145
		public abstract void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm);

		// Token: 0x06003B2A RID: 15146
		public abstract void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation);

		// Token: 0x06003B2B RID: 15147
		public abstract bool CanUseSecondaryTerm();

		// Token: 0x06003B2C RID: 15148
		public abstract bool AllowMainTermToBeRTL();

		// Token: 0x06003B2D RID: 15149
		public abstract bool AllowSecondTermToBeRTL();

		// Token: 0x06003B2E RID: 15150
		public abstract eTermType GetPrimaryTermType(Localize cmp);

		// Token: 0x06003B2F RID: 15151
		public abstract eTermType GetSecondaryTermType(Localize cmp);
	}
}
