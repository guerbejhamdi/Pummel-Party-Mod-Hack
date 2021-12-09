using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000823 RID: 2083
	public abstract class LocalizeTarget<T> : ILocalizeTarget where T : UnityEngine.Object
	{
		// Token: 0x06003B31 RID: 15153 RVA: 0x0012B690 File Offset: 0x00129890
		public override bool IsValid(Localize cmp)
		{
			if (this.mTarget != null)
			{
				Component component = this.mTarget as Component;
				if (component != null && component.gameObject != cmp.gameObject)
				{
					this.mTarget = default(T);
				}
			}
			if (this.mTarget == null)
			{
				this.mTarget = cmp.GetComponent<T>();
			}
			return this.mTarget != null;
		}

		// Token: 0x040038FB RID: 14587
		public T mTarget;
	}
}
