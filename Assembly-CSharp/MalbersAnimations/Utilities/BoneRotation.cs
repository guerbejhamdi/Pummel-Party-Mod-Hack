using System;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x0200079A RID: 1946
	[Serializable]
	public class BoneRotation
	{
		// Token: 0x04003661 RID: 13921
		public Transform bone;

		// Token: 0x04003662 RID: 13922
		public Vector3 offset = new Vector3(0f, -90f, -90f);

		// Token: 0x04003663 RID: 13923
		[Range(0f, 1f)]
		public float weight = 1f;

		// Token: 0x04003664 RID: 13924
		internal Quaternion initialRotation;
	}
}
