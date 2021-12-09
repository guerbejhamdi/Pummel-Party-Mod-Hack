using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x020008A1 RID: 2209
	internal struct CollisionData
	{
		// Token: 0x06003EA0 RID: 16032 RVA: 0x0002A2BF File Offset: 0x000284BF
		public CollisionData(Vector3 Position, Quaternion Rotation, Transform Surface, int Layer)
		{
			this.position = Position;
			this.rotation = Rotation;
			this.surface = Surface;
			this.layer = Layer;
		}

		// Token: 0x04003AEC RID: 15084
		public Vector3 position;

		// Token: 0x04003AED RID: 15085
		public Quaternion rotation;

		// Token: 0x04003AEE RID: 15086
		public Transform surface;

		// Token: 0x04003AEF RID: 15087
		public int layer;
	}
}
