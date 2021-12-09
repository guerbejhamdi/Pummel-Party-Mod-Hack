using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200075F RID: 1887
	public class UseTransform : MonoBehaviour
	{
		// Token: 0x06003667 RID: 13927 RVA: 0x00024EEA File Offset: 0x000230EA
		private void Update()
		{
			if (this.updateMode == UseTransform.UpdateMode.Update)
			{
				this.SetTransformReference();
			}
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x00024EFA File Offset: 0x000230FA
		private void LateUpdate()
		{
			if (this.updateMode == UseTransform.UpdateMode.LateUpdate)
			{
				this.SetTransformReference();
			}
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x00024F0B File Offset: 0x0002310B
		private void FixedUpdate()
		{
			if (this.updateMode == UseTransform.UpdateMode.FixedUpdate)
			{
				this.SetTransformReference();
			}
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x00024F1C File Offset: 0x0002311C
		private void SetTransformReference()
		{
			if (!this.Reference)
			{
				return;
			}
			base.transform.position = this.Reference.position;
			base.transform.rotation = this.Reference.rotation;
		}

		// Token: 0x040035C5 RID: 13765
		public Transform Reference;

		// Token: 0x040035C6 RID: 13766
		public UseTransform.UpdateMode updateMode = UseTransform.UpdateMode.LateUpdate;

		// Token: 0x02000760 RID: 1888
		public enum UpdateMode
		{
			// Token: 0x040035C8 RID: 13768
			Update,
			// Token: 0x040035C9 RID: 13769
			LateUpdate,
			// Token: 0x040035CA RID: 13770
			FixedUpdate
		}
	}
}
