using System;
using System.Collections;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000753 RID: 1875
	public class ScaleBonesFix : MonoBehaviour, IAnimatorListener
	{
		// Token: 0x0600362F RID: 13871 RVA: 0x00024C72 File Offset: 0x00022E72
		public void FixHeight(bool active)
		{
			base.StartCoroutine(this.SmoothFix(active));
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x0002339D File Offset: 0x0002159D
		public virtual void OnAnimatorBehaviourMessage(string message, object value)
		{
			this.InvokeWithParams(message, value);
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x00024C82 File Offset: 0x00022E82
		public IEnumerator SmoothFix(bool active)
		{
			float t = 0f;
			Vector3 startpos = this.fixGameObject.localPosition;
			Vector3 endpos = startpos + (active ? this.Offset : (-this.Offset));
			while (t < this.duration)
			{
				this.fixGameObject.localPosition = Vector3.Lerp(startpos, endpos, t / this.duration);
				t += Time.deltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x0400357D RID: 13693
		public Transform fixGameObject;

		// Token: 0x0400357E RID: 13694
		public Vector3 Offset;

		// Token: 0x0400357F RID: 13695
		public float duration;
	}
}
