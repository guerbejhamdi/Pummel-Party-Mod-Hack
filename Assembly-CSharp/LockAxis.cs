using System;
using UnityEngine;

// Token: 0x0200012C RID: 300
public class LockAxis : MonoBehaviour
{
	// Token: 0x060008BA RID: 2234 RVA: 0x00050618 File Offset: 0x0004E818
	private void Update()
	{
		Vector3 position = base.transform.position;
		if (this.LockX)
		{
			position.x = 0f;
		}
		if (this.LockY)
		{
			position.y = 0f;
		}
		if (this.LockZ)
		{
			position.z = 0f;
		}
		base.transform.position = position;
	}

	// Token: 0x0400071E RID: 1822
	public bool LockX = true;

	// Token: 0x0400071F RID: 1823
	public bool LockY;

	// Token: 0x04000720 RID: 1824
	public bool LockZ;
}
