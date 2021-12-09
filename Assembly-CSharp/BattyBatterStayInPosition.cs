using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class BattyBatterStayInPosition : MonoBehaviour
{
	// Token: 0x06000045 RID: 69 RVA: 0x00003BDA File Offset: 0x00001DDA
	private void Start()
	{
		this.pos = base.transform.position;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00003BED File Offset: 0x00001DED
	private void Update()
	{
		base.transform.position = this.pos;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00003BED File Offset: 0x00001DED
	private void LateUpdate()
	{
		base.transform.position = this.pos;
	}

	// Token: 0x0400003A RID: 58
	private Vector3 pos;
}
