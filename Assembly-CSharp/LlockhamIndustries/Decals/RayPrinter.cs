using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x020008A2 RID: 2210
	public class RayPrinter : Printer
	{
		// Token: 0x06003EA1 RID: 16033 RVA: 0x00134AF4 File Offset: 0x00132CF4
		public void PrintOnRay(Ray Ray, float RayLength, Vector3 DecalUp = default(Vector3))
		{
			if (DecalUp == Vector3.zero)
			{
				DecalUp = Vector3.up;
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(Ray, out raycastHit, RayLength, this.layers.value))
			{
				base.Print(raycastHit.point, Quaternion.LookRotation(-raycastHit.normal, DecalUp), raycastHit.transform, raycastHit.collider.gameObject.layer);
			}
		}

		// Token: 0x04003AF0 RID: 15088
		public LayerMask layers;
	}
}
