using System;

namespace MalbersAnimations
{
	// Token: 0x02000746 RID: 1862
	public class MinMaxRangeAttribute : Attribute
	{
		// Token: 0x06003621 RID: 13857 RVA: 0x00024B93 File Offset: 0x00022D93
		public MinMaxRangeAttribute(float min, float max)
		{
			this.Min = min;
			this.Max = max;
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06003622 RID: 13858 RVA: 0x00024BA9 File Offset: 0x00022DA9
		// (set) Token: 0x06003623 RID: 13859 RVA: 0x00024BB1 File Offset: 0x00022DB1
		public float Min { get; private set; }

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06003624 RID: 13860 RVA: 0x00024BBA File Offset: 0x00022DBA
		// (set) Token: 0x06003625 RID: 13861 RVA: 0x00024BC2 File Offset: 0x00022DC2
		public float Max { get; private set; }
	}
}
