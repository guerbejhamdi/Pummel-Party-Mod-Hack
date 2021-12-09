using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000869 RID: 2153
	[Serializable]
	public class ReplacementPass
	{
		// Token: 0x06003CFF RID: 15615 RVA: 0x00028B55 File Offset: 0x00026D55
		public ReplacementPass(LayerMask Mask, Vector4 LayerVector)
		{
			this.vector = LayerVector;
			this.layers = Mask;
		}

		// Token: 0x06003D00 RID: 15616 RVA: 0x00028B6B File Offset: 0x00026D6B
		public ReplacementPass(int LayerIndex, Vector4 LayerVector)
		{
			this.vector = LayerVector;
			this.layers = 1 << LayerIndex;
		}

		// Token: 0x040039E0 RID: 14816
		public Vector4 vector;

		// Token: 0x040039E1 RID: 14817
		public LayerMask layers;
	}
}
