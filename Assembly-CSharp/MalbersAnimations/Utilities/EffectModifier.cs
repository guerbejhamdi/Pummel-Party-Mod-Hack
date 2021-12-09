using System;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x02000789 RID: 1929
	public class EffectModifier : ScriptableObject
	{
		// Token: 0x06003702 RID: 14082 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void AwakeEffect(Effect effect)
		{
		}

		// Token: 0x06003703 RID: 14083 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void StartEffect(Effect effect)
		{
		}

		// Token: 0x06003704 RID: 14084 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void StopEffect(Effect effect)
		{
		}

		// Token: 0x04003635 RID: 13877
		[TextArea]
		public string Description = string.Empty;
	}
}
