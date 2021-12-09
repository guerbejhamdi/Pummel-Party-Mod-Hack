using System;

namespace MalbersAnimations
{
	// Token: 0x02000747 RID: 1863
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
	public sealed class LineAttribute : Attribute
	{
		// Token: 0x06003626 RID: 13862 RVA: 0x00024BCB File Offset: 0x00022DCB
		public LineAttribute()
		{
			this.height = 8f;
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x00024BDE File Offset: 0x00022DDE
		public LineAttribute(float height)
		{
			this.height = height;
		}

		// Token: 0x04003533 RID: 13619
		public readonly float height;
	}
}
