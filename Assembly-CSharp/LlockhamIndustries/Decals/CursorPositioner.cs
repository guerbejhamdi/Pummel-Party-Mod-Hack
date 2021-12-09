using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000894 RID: 2196
	public class CursorPositioner : Positioner
	{
		// Token: 0x06003E80 RID: 16000 RVA: 0x0002A091 File Offset: 0x00028291
		protected override void Start()
		{
			if (this.projectionCamera == null)
			{
				this.projectionCamera = Camera.main;
			}
			base.Start();
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x0002A0B2 File Offset: 0x000282B2
		private void LateUpdate()
		{
			base.Reproject(this.projectionCamera.ScreenPointToRay(Input.mousePosition), float.PositiveInfinity, this.projectionCamera.transform.up);
		}

		// Token: 0x04003AA8 RID: 15016
		public Camera projectionCamera;
	}
}
