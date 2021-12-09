using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000752 RID: 1874
	[Serializable]
	public struct RangedFloat
	{
		// Token: 0x0600362C RID: 13868 RVA: 0x00024C36 File Offset: 0x00022E36
		public RangedFloat(float minValue, float maxValue)
		{
			this.minValue = minValue;
			this.maxValue = maxValue;
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x0600362D RID: 13869 RVA: 0x00024C46 File Offset: 0x00022E46
		public float RandomValue
		{
			get
			{
				return UnityEngine.Random.Range(this.minValue, this.maxValue);
			}
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x00024C59 File Offset: 0x00022E59
		public bool IsInRange(float value)
		{
			return value >= this.minValue && value <= this.maxValue;
		}

		// Token: 0x0400357B RID: 13691
		public float minValue;

		// Token: 0x0400357C RID: 13692
		public float maxValue;
	}
}
