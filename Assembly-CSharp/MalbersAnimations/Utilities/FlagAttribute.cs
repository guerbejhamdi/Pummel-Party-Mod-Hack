using System;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x02000798 RID: 1944
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class FlagAttribute : PropertyAttribute
	{
		// Token: 0x0600374B RID: 14155 RVA: 0x00025A2A File Offset: 0x00023C2A
		public FlagAttribute()
		{
		}

		// Token: 0x0600374C RID: 14156 RVA: 0x00025A32 File Offset: 0x00023C32
		public FlagAttribute(string name)
		{
			this.enumName = name;
		}

		// Token: 0x0400365E RID: 13918
		public string enumName;
	}
}
