using System;
using UnityEngine;

// Token: 0x0200011B RID: 283
public class LimitRotation : MonoBehaviour
{
	// Token: 0x06000878 RID: 2168 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x0004EF70 File Offset: 0x0004D170
	private void Update()
	{
		if (base.transform.localRotation.eulerAngles.z > this.maxZ)
		{
			base.transform.localRotation = Quaternion.Euler(base.transform.localRotation.x, base.transform.localRotation.y, this.maxZ);
		}
		if (base.transform.localRotation.eulerAngles.z < this.minZ)
		{
			base.transform.localRotation = Quaternion.Euler(base.transform.localRotation.x, base.transform.localRotation.y, this.minZ);
		}
	}

	// Token: 0x040006D0 RID: 1744
	public float minZ;

	// Token: 0x040006D1 RID: 1745
	public float maxZ = 360f;
}
