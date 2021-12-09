using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000886 RID: 2182
	public class SceneLayers : MonoBehaviour
	{
		// Token: 0x06003E34 RID: 15924 RVA: 0x00029D6C File Offset: 0x00027F6C
		private void OnEnable()
		{
			this.original = DynamicDecals.System.Settings.Layers;
			DynamicDecals.System.Settings.Layers = this.layers;
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x00029D98 File Offset: 0x00027F98
		private void OnDisable()
		{
			DynamicDecals.System.Settings.Layers = this.original;
		}

		// Token: 0x04003A70 RID: 14960
		public ProjectionLayer[] layers;

		// Token: 0x04003A71 RID: 14961
		private ProjectionLayer[] original;
	}
}
