using System;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class KeepRotation : MonoBehaviour
{
	// Token: 0x060003E4 RID: 996 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x00006285 File Offset: 0x00004485
	private void Update()
	{
		base.transform.rotation = Quaternion.Euler(this.rot);
	}

	// Token: 0x04000447 RID: 1095
	public Vector3 rot;
}
