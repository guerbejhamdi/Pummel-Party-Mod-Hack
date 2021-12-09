using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000826 RID: 2086
	public class LocalizeTargetDesc_Type<T, G> : LocalizeTargetDesc<G> where T : UnityEngine.Object where G : LocalizeTarget<T>
	{
		// Token: 0x06003B3A RID: 15162 RVA: 0x00027CFB File Offset: 0x00025EFB
		public override bool CanLocalize(Localize cmp)
		{
			return cmp.GetComponent<T>() != null;
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x0012B71C File Offset: 0x0012991C
		public override ILocalizeTarget CreateTarget(Localize cmp)
		{
			T component = cmp.GetComponent<T>();
			if (component == null)
			{
				return null;
			}
			G g = ScriptableObject.CreateInstance<G>();
			g.mTarget = component;
			return g;
		}
	}
}
