using System;
using UnityEngine;

// Token: 0x020002B1 RID: 689
public class MoveBox : MonoBehaviour
{
	// Token: 0x060013FB RID: 5115 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x060013FC RID: 5116 RVA: 0x0000FBC0 File Offset: 0x0000DDC0
	private void Update()
	{
		base.transform.position += new Vector3(1f * Time.deltaTime, 0f, 0f);
	}
}
