using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000745 RID: 1861
	public class LookAtCamera : MonoBehaviour
	{
		// Token: 0x0600361E RID: 13854 RVA: 0x00024B72 File Offset: 0x00022D72
		private void Start()
		{
			this.cam = Camera.main.transform;
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x001166E8 File Offset: 0x001148E8
		private void Update()
		{
			Vector3 forward = this.cam.position - base.transform.position;
			forward.y = 0f;
			Quaternion quaternion = Quaternion.LookRotation(forward);
			base.transform.eulerAngles = new Vector3(this.justY ? 0f : quaternion.eulerAngles.x, quaternion.eulerAngles.y, 0f) + this.Offset;
		}

		// Token: 0x0400352E RID: 13614
		public bool justY = true;

		// Token: 0x0400352F RID: 13615
		public Vector3 Offset;

		// Token: 0x04003530 RID: 13616
		private Transform cam;
	}
}
