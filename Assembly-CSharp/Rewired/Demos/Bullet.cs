using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020006D7 RID: 1751
	[AddComponentMenu("")]
	public class Bullet : MonoBehaviour
	{
		// Token: 0x06003324 RID: 13092 RVA: 0x00022C7A File Offset: 0x00020E7A
		private void Start()
		{
			if (this.lifeTime > 0f)
			{
				this.deathTime = Time.time + this.lifeTime;
				this.die = true;
			}
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x00022CA2 File Offset: 0x00020EA2
		private void Update()
		{
			if (this.die && Time.time >= this.deathTime)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x04003162 RID: 12642
		public float lifeTime = 3f;

		// Token: 0x04003163 RID: 12643
		private bool die;

		// Token: 0x04003164 RID: 12644
		private float deathTime;
	}
}
