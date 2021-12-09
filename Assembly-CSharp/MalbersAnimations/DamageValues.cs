using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x020006FA RID: 1786
	public class DamageValues
	{
		// Token: 0x060034AC RID: 13484 RVA: 0x00023D2C File Offset: 0x00021F2C
		public DamageValues(Vector3 dir, float amount = 0f)
		{
			this.Direction = dir;
			this.Amount = amount;
		}

		// Token: 0x0400330D RID: 13069
		public Vector3 Direction;

		// Token: 0x0400330E RID: 13070
		public float Amount;
	}
}
