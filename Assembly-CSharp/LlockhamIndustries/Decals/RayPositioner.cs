using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000896 RID: 2198
	public class RayPositioner : Positioner
	{
		// Token: 0x06003E89 RID: 16009 RVA: 0x00133F38 File Offset: 0x00132138
		private void LateUpdate()
		{
			Transform transform = (this.rayTransform != null) ? this.rayTransform : base.transform;
			Quaternion rotation = transform.rotation * Quaternion.Euler(this.rotationOffset);
			Vector3 origin = transform.position + rotation * this.positionOffset;
			Ray ray = new Ray(origin, rotation * Vector3.forward);
			base.Reproject(ray, this.castLength, rotation * Vector3.up);
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x00133FBC File Offset: 0x001321BC
		private void OnDrawGizmosSelected()
		{
			Transform transform = (this.rayTransform != null) ? this.rayTransform : base.transform;
			Quaternion rotation = transform.rotation * Quaternion.Euler(this.rotationOffset);
			Vector3 from = transform.position + rotation * this.positionOffset;
			Gizmos.color = Color.black;
			Gizmos.DrawRay(from, rotation * Vector3.up * 0.4f);
			Gizmos.color = Color.white;
			Gizmos.DrawRay(from, rotation * Vector3.forward * this.castLength);
		}

		// Token: 0x04003AAD RID: 15021
		public Transform rayTransform;

		// Token: 0x04003AAE RID: 15022
		public Vector3 positionOffset;

		// Token: 0x04003AAF RID: 15023
		public Vector3 rotationOffset;

		// Token: 0x04003AB0 RID: 15024
		public float castLength = 100f;
	}
}
