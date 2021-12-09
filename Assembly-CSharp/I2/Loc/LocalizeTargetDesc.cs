using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000825 RID: 2085
	public abstract class LocalizeTargetDesc<T> : ILocalizeTargetDescriptor where T : ILocalizeTarget
	{
		// Token: 0x06003B37 RID: 15159 RVA: 0x00027CDB File Offset: 0x00025EDB
		public override ILocalizeTarget CreateTarget(Localize cmp)
		{
			return ScriptableObject.CreateInstance<T>();
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x00027CE7 File Offset: 0x00025EE7
		public override Type GetTargetType()
		{
			return typeof(T);
		}
	}
}
