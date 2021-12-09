using System;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x02000797 RID: 1943
	[Serializable]
	public struct DeltaTransform
	{
		// Token: 0x06003748 RID: 14152 RVA: 0x0011899C File Offset: 0x00116B9C
		public void StoreTransform(Transform transform)
		{
			if (transform == null)
			{
				return;
			}
			this.Position = transform.position;
			this.LocalPosition = transform.localPosition;
			this.EulerAngles = transform.eulerAngles;
			this.Rotation = transform.rotation;
			this.LocalEulerAngles = transform.localEulerAngles;
			this.LocalRotation = transform.localRotation;
			this.lossyScale = transform.lossyScale;
			this.LocalScale = transform.localScale;
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x000259DE File Offset: 0x00023BDE
		public void RestoreTransform(Transform transform)
		{
			transform.position = this.Position;
			transform.rotation = this.Rotation;
			transform.localScale = this.LocalScale;
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x00025A04 File Offset: 0x00023C04
		public void RestoreLocalTransform(Transform transform)
		{
			transform.localPosition = this.LocalPosition;
			transform.localRotation = this.LocalRotation;
			transform.localScale = this.LocalScale;
		}

		// Token: 0x04003656 RID: 13910
		public Vector3 LocalPosition;

		// Token: 0x04003657 RID: 13911
		public Vector3 LocalEulerAngles;

		// Token: 0x04003658 RID: 13912
		public Vector3 Position;

		// Token: 0x04003659 RID: 13913
		public Vector3 EulerAngles;

		// Token: 0x0400365A RID: 13914
		public Quaternion Rotation;

		// Token: 0x0400365B RID: 13915
		public Quaternion LocalRotation;

		// Token: 0x0400365C RID: 13916
		public Vector3 lossyScale;

		// Token: 0x0400365D RID: 13917
		public Vector3 LocalScale;
	}
}
