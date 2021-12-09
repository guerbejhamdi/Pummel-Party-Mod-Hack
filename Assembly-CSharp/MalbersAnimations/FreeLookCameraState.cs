using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000724 RID: 1828
	[CreateAssetMenu(menuName = "Malbers Animations/Camera/FreeLook Camera State")]
	public class FreeLookCameraState : ScriptableObject
	{
		// Token: 0x0600355C RID: 13660 RVA: 0x00113EB0 File Offset: 0x001120B0
		public FreeLookCameraState()
		{
			this.CamFOV = 45f;
			this.PivotPos = new Vector3(0f, 1f, 0f);
			this.CamPos = new Vector3(0f, 0f, -4.45f);
		}

		// Token: 0x04003487 RID: 13447
		public Vector3 PivotPos;

		// Token: 0x04003488 RID: 13448
		public Vector3 CamPos;

		// Token: 0x04003489 RID: 13449
		public float CamFOV = 45f;
	}
}
