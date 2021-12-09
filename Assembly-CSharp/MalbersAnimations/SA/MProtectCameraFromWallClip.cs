using System;
using System.Collections;
using UnityEngine;

namespace MalbersAnimations.SA
{
	// Token: 0x0200077A RID: 1914
	public class MProtectCameraFromWallClip : MonoBehaviour
	{
		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x060036CB RID: 14027 RVA: 0x0002557C File Offset: 0x0002377C
		// (set) Token: 0x060036CC RID: 14028 RVA: 0x00025584 File Offset: 0x00023784
		public bool protecting { get; private set; }

		// Token: 0x060036CD RID: 14029 RVA: 0x00117340 File Offset: 0x00115540
		private void Start()
		{
			this.m_Cam = base.GetComponentInChildren<Camera>().transform;
			this.m_Pivot = this.m_Cam.parent;
			this.m_OriginalDist = this.m_Cam.localPosition.magnitude;
			this.m_CurrentDist = this.m_OriginalDist;
			this.m_RayHitComparer = new MProtectCameraFromWallClip.RayHitComparer();
		}

		// Token: 0x060036CE RID: 14030 RVA: 0x001173A0 File Offset: 0x001155A0
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
				if (!array[i].isTrigger && (!(array[i].attachedRigidbody != null) || !array[i].attachedRigidbody.CompareTag(this.dontClipTag)))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.m_Ray.origin = this.m_Ray.origin + this.m_Pivot.forward * this.sphereCastRadius;
				this.m_Hits = Physics.RaycastAll(this.m_Ray, this.m_OriginalDist - this.sphereCastRadius);
			}
			else
			{
				this.m_Hits = Physics.SphereCastAll(this.m_Ray, this.sphereCastRadius, this.m_OriginalDist + this.sphereCastRadius);
			}
			Array.Sort(this.m_Hits, this.m_RayHitComparer);
			float num2 = float.PositiveInfinity;
			for (int j = 0; j < this.m_Hits.Length; j++)
			{
				if (this.m_Hits[j].distance < num2 && !this.m_Hits[j].collider.isTrigger && (!(this.m_Hits[j].collider.attachedRigidbody != null) || !this.m_Hits[j].collider.attachedRigidbody.CompareTag(this.dontClipTag)))
				{
					num2 = this.m_Hits[j].distance;
					num = -this.m_Pivot.InverseTransformPoint(this.m_Hits[j].point).z;
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

		// Token: 0x04003601 RID: 13825
		public float clipMoveTime = 0.05f;

		// Token: 0x04003602 RID: 13826
		public float returnTime = 0.4f;

		// Token: 0x04003603 RID: 13827
		public float sphereCastRadius = 0.1f;

		// Token: 0x04003604 RID: 13828
		public bool visualiseInEditor;

		// Token: 0x04003605 RID: 13829
		public float closestDistance = 0.5f;

		// Token: 0x04003607 RID: 13831
		public string dontClipTag = "Player";

		// Token: 0x04003608 RID: 13832
		private Transform m_Cam;

		// Token: 0x04003609 RID: 13833
		private Transform m_Pivot;

		// Token: 0x0400360A RID: 13834
		private float m_OriginalDist;

		// Token: 0x0400360B RID: 13835
		private float m_MoveVelocity;

		// Token: 0x0400360C RID: 13836
		private float m_CurrentDist;

		// Token: 0x0400360D RID: 13837
		private Ray m_Ray;

		// Token: 0x0400360E RID: 13838
		private RaycastHit[] m_Hits;

		// Token: 0x0400360F RID: 13839
		private MProtectCameraFromWallClip.RayHitComparer m_RayHitComparer;

		// Token: 0x0200077B RID: 1915
		public class RayHitComparer : IComparer
		{
			// Token: 0x060036D0 RID: 14032 RVA: 0x00113950 File Offset: 0x00111B50
			public int Compare(object x, object y)
			{
				return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
			}
		}
	}
}
