using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000895 RID: 2197
	public abstract class Positioner : MonoBehaviour
	{
		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06003E83 RID: 16003 RVA: 0x0002A0E7 File Offset: 0x000282E7
		public ProjectionRenderer Active
		{
			get
			{
				return this.proj;
			}
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x0002A0EF File Offset: 0x000282EF
		private void OnDisable()
		{
			if (this.proj != null)
			{
				this.proj.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x00133E40 File Offset: 0x00132040
		protected virtual void Start()
		{
			if (this.projection != null)
			{
				this.proj = UnityEngine.Object.Instantiate<GameObject>(this.projection.gameObject, DynamicDecals.System.DefaultPool.Parent).GetComponent<ProjectionRenderer>();
				this.proj.name = "Positioned Projection";
				return;
			}
			Debug.LogWarning("Positioner has no projection to position.");
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x00133EA0 File Offset: 0x001320A0
		protected void Reproject(Ray Ray, float CastLength, Vector3 ReferenceUp)
		{
			if (this.proj != null)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(Ray, out raycastHit, CastLength, this.layers.value))
				{
					this.proj.gameObject.SetActive(true);
					this.proj.transform.rotation = Quaternion.LookRotation(-raycastHit.normal, ReferenceUp);
					this.proj.transform.position = raycastHit.point;
					return;
				}
				if (!this.alwaysVisible)
				{
					this.proj.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x0002A110 File Offset: 0x00028310
		private Vector3 Divide(Vector3 A, Vector3 B)
		{
			return new Vector3(A.x / B.x, A.y / B.y, A.z / B.z);
		}

		// Token: 0x04003AA9 RID: 15017
		public ProjectionRenderer projection;

		// Token: 0x04003AAA RID: 15018
		public LayerMask layers = -1;

		// Token: 0x04003AAB RID: 15019
		public bool alwaysVisible;

		// Token: 0x04003AAC RID: 15020
		private ProjectionRenderer proj;
	}
}
