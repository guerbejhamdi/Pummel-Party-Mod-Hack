using System;
using System.Collections;
using MalbersAnimations.Utilities;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200071F RID: 1823
	public class CameraWallStop : MonoBehaviour
	{
		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06003542 RID: 13634 RVA: 0x00024272 File Offset: 0x00022472
		// (set) Token: 0x06003543 RID: 13635 RVA: 0x0002427A File Offset: 0x0002247A
		public bool protecting { get; private set; }

		// Token: 0x06003544 RID: 13636 RVA: 0x00113618 File Offset: 0x00111818
		private void Start()
		{
			this.m_Cam = base.GetComponentInChildren<Camera>().transform;
			this.m_Pivot = this.m_Cam.parent;
			this.m_OriginalDist = this.m_Cam.localPosition.magnitude;
			this.m_CurrentDist = this.m_OriginalDist;
			this.m_RayHitComparer = new CameraWallStop.RayHitComparer();
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x00113678 File Offset: 0x00111878
		private void LateUpdate()
		{
			float num = this.m_OriginalDist;
			this.m_Ray.origin = this.m_Pivot.position + this.m_Pivot.forward * this.sphereCastRadius;
			this.m_Ray.direction = -this.m_Pivot.forward;
			Collider[] array = Physics.OverlapSphere(this.m_Ray.origin, this.sphereCastRadius);
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].isTrigger && !MalbersTools.CollidersLayer(array[i], this.dontClip))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.m_Ray.origin = this.m_Ray.origin + this.m_Pivot.forward * this.sphereCastRadius;
				this.hits = Physics.RaycastAll(this.m_Ray, this.m_OriginalDist - this.sphereCastRadius);
			}
			else
			{
				this.hits = Physics.SphereCastAll(this.m_Ray, this.sphereCastRadius, this.m_OriginalDist + this.sphereCastRadius);
			}
			Array.Sort(this.hits, this.m_RayHitComparer);
			float num2 = float.PositiveInfinity;
			for (int j = 0; j < this.hits.Length; j++)
			{
				if (this.hits[j].distance < num2 && !this.hits[j].collider.isTrigger && !MalbersTools.CollidersLayer(this.hits[j].collider, this.dontClip))
				{
					num2 = this.hits[j].distance;
					num = -this.m_Pivot.InverseTransformPoint(this.hits[j].point).z;
					flag2 = true;
				}
			}
			if (flag2)
			{
				Debug.DrawRay(this.m_Ray.origin, -this.m_Pivot.forward * (num + this.sphereCastRadius), Color.red);
			}
			this.protecting = flag2;
			this.m_CurrentDist = Mathf.SmoothDamp(this.m_CurrentDist, num, ref this.m_MoveVelocity, (this.m_CurrentDist > num) ? this.clipMoveTime : this.returnTime);
			this.m_CurrentDist = Mathf.Clamp(this.m_CurrentDist, this.closestDistance, this.m_OriginalDist);
			this.m_Cam.localPosition = -Vector3.forward * this.m_CurrentDist;
		}

		// Token: 0x04003462 RID: 13410
		public float clipMoveTime = 0.05f;

		// Token: 0x04003463 RID: 13411
		public float returnTime = 0.4f;

		// Token: 0x04003464 RID: 13412
		public float sphereCastRadius = 0.15f;

		// Token: 0x04003465 RID: 13413
		public bool visualiseInEditor;

		// Token: 0x04003466 RID: 13414
		public float closestDistance = 0.5f;

		// Token: 0x04003468 RID: 13416
		public LayerMask dontClip = 1048576;

		// Token: 0x04003469 RID: 13417
		private Transform m_Cam;

		// Token: 0x0400346A RID: 13418
		private Transform m_Pivot;

		// Token: 0x0400346B RID: 13419
		private float m_OriginalDist;

		// Token: 0x0400346C RID: 13420
		private float m_MoveVelocity;

		// Token: 0x0400346D RID: 13421
		private float m_CurrentDist;

		// Token: 0x0400346E RID: 13422
		private Ray m_Ray;

		// Token: 0x0400346F RID: 13423
		private RaycastHit[] hits;

		// Token: 0x04003470 RID: 13424
		private CameraWallStop.RayHitComparer m_RayHitComparer;

		// Token: 0x02000720 RID: 1824
		public class RayHitComparer : IComparer
		{
			// Token: 0x06003547 RID: 13639 RVA: 0x00113950 File Offset: 0x00111B50
			public int Compare(object x, object y)
			{
				return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
			}
		}
	}
}
