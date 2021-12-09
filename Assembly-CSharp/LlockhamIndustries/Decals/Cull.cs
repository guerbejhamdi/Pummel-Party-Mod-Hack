using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x0200088C RID: 2188
	[RequireComponent(typeof(ProjectionRenderer))]
	public class Cull : Modifier
	{
		// Token: 0x06003E59 RID: 15961 RVA: 0x00029F17 File Offset: 0x00028117
		private void Awake()
		{
			this.projection = base.GetComponent<ProjectionRenderer>();
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x00029F25 File Offset: 0x00028125
		protected override void Begin()
		{
			this.timeElapsed = 0f;
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x00133218 File Offset: 0x00131418
		public override void Perform()
		{
			if (this.timeElapsed < this.cullTime)
			{
				this.timeElapsed += base.UpdateRate;
				if (this.projection.Renderer.isVisible)
				{
					this.timeElapsed = 0f;
				}
				return;
			}
			this.projection.Destroy();
		}

		// Token: 0x04003A83 RID: 14979
		public float cullTime = 4f;

		// Token: 0x04003A84 RID: 14980
		private ProjectionRenderer projection;

		// Token: 0x04003A85 RID: 14981
		private float timeElapsed;
	}
}
