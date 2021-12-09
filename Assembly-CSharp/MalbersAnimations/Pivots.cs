using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000748 RID: 1864
	public class Pivots : MonoBehaviour
	{
		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06003628 RID: 13864 RVA: 0x00012D07 File Offset: 0x00010F07
		public Vector3 GetPivot
		{
			get
			{
				return base.transform.position;
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06003629 RID: 13865 RVA: 0x00024BED File Offset: 0x00022DED
		public float Y
		{
			get
			{
				return base.transform.position.y;
			}
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x0011676C File Offset: 0x0011496C
		private void OnDrawGizmos()
		{
			if (this.debug)
			{
				Gizmos.color = this.DebugColor;
				Gizmos.DrawWireSphere(this.GetPivot, this.debugSize);
				if (this.drawRay)
				{
					Gizmos.DrawRay(this.GetPivot, -base.transform.up * this.multiplier * base.transform.root.localScale.y);
				}
			}
		}

		// Token: 0x04003534 RID: 13620
		public float multiplier = 1f;

		// Token: 0x04003535 RID: 13621
		public bool debug = true;

		// Token: 0x04003536 RID: 13622
		public float debugSize = 0.03f;

		// Token: 0x04003537 RID: 13623
		public Color DebugColor = Color.blue;

		// Token: 0x04003538 RID: 13624
		public bool drawRay = true;
	}
}
