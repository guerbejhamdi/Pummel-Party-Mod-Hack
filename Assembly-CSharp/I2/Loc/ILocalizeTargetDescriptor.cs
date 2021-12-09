using System;

namespace I2.Loc
{
	// Token: 0x02000824 RID: 2084
	public abstract class ILocalizeTargetDescriptor
	{
		// Token: 0x06003B33 RID: 15155
		public abstract bool CanLocalize(Localize cmp);

		// Token: 0x06003B34 RID: 15156
		public abstract ILocalizeTarget CreateTarget(Localize cmp);

		// Token: 0x06003B35 RID: 15157
		public abstract Type GetTargetType();

		// Token: 0x040038FC RID: 14588
		public string Name;

		// Token: 0x040038FD RID: 14589
		public int Priority;
	}
}
