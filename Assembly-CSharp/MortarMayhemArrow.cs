using System;
using UnityEngine;

// Token: 0x020001D8 RID: 472
public class MortarMayhemArrow : MonoBehaviour
{
	// Token: 0x06000DAC RID: 3500 RVA: 0x0000C54C File Offset: 0x0000A74C
	private void Start()
	{
		UnityEngine.Object.Destroy(base.gameObject, 5f);
	}

	// Token: 0x06000DAD RID: 3501 RVA: 0x0000C55E File Offset: 0x0000A75E
	private void Update()
	{
		base.transform.position += Vector3.up * Time.deltaTime * 150f;
	}
}
