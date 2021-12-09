using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000868 RID: 2152
	[Serializable]
	public struct ProjectionLayer
	{
		// Token: 0x06003CFC RID: 15612 RVA: 0x00028B16 File Offset: 0x00026D16
		public ProjectionLayer(string Name)
		{
			this.name = Name;
			this.layers = 0;
		}

		// Token: 0x06003CFD RID: 15613 RVA: 0x00028B2B File Offset: 0x00026D2B
		public ProjectionLayer(string Name, int Layer)
		{
			this.name = Name;
			this.layers = 1 << Layer;
		}

		// Token: 0x06003CFE RID: 15614 RVA: 0x00028B45 File Offset: 0x00026D45
		public ProjectionLayer(string Name, LayerMask Layers)
		{
			this.name = Name;
			this.layers = Layers;
		}

		// Token: 0x040039DE RID: 14814
		public string name;

		// Token: 0x040039DF RID: 14815
		public LayerMask layers;
	}
}
