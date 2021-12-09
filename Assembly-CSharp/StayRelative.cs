using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004C0 RID: 1216
public class StayRelative : MonoBehaviour
{
	// Token: 0x06002050 RID: 8272 RVA: 0x000179CA File Offset: 0x00015BCA
	private void Start()
	{
		this.TargetReset();
	}

	// Token: 0x06002051 RID: 8273 RVA: 0x000CA8C4 File Offset: 0x000C8AC4
	private void Update()
	{
		if (this.lastPosition != this.target.position)
		{
			base.transform.position = this.target.TransformPoint(this.relativeOffset);
			this.lastPosition = this.target.position;
		}
	}

	// Token: 0x06002052 RID: 8274 RVA: 0x000179D2 File Offset: 0x00015BD2
	public void TargetReset()
	{
		this.relativeOffset = this.target.InverseTransformPoint(base.transform.position);
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x000179F0 File Offset: 0x00015BF0
	private IEnumerator Delay()
	{
		yield return new WaitForSeconds(2.1f);
		this.relativeOffset = this.target.InverseTransformPoint(base.transform.position);
		yield break;
	}

	// Token: 0x04002319 RID: 8985
	public Transform target;

	// Token: 0x0400231A RID: 8986
	private Vector3 relativeOffset;

	// Token: 0x0400231B RID: 8987
	private Vector3 lastPosition = Vector3.zero;
}
