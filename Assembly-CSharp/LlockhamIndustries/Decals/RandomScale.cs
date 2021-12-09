using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000892 RID: 2194
	public class RandomScale : MonoBehaviour
	{
		// Token: 0x06003E78 RID: 15992 RVA: 0x00133CE4 File Offset: 0x00131EE4
		private void Awake()
		{
			float num = UnityEngine.Random.Range(this.minSize, this.maxSize);
			base.transform.localScale = new Vector3(num, num, num);
		}

		// Token: 0x04003A9C RID: 15004
		public float minSize = 0.5f;

		// Token: 0x04003A9D RID: 15005
		public float maxSize = 0.8f;
	}
}
