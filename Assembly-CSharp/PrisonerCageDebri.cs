using System;
using UnityEngine;

// Token: 0x0200009F RID: 159
public class PrisonerCageDebri
{
	// Token: 0x0600035B RID: 859 RVA: 0x00005C5B File Offset: 0x00003E5B
	public PrisonerCageDebri(GameObject o, Rigidbody b)
	{
		this.obj = o;
		this.body = b;
		this.startPos = o.transform.localPosition;
		this.startRot = o.transform.localRotation;
	}

	// Token: 0x0600035C RID: 860 RVA: 0x00005C93 File Offset: 0x00003E93
	public void Reset()
	{
		this.obj.transform.localPosition = this.startPos;
		this.obj.transform.localRotation = this.startRot;
	}

	// Token: 0x0400036B RID: 875
	public GameObject obj;

	// Token: 0x0400036C RID: 876
	public Rigidbody body;

	// Token: 0x0400036D RID: 877
	private Vector3 startPos;

	// Token: 0x0400036E RID: 878
	private Quaternion startRot;
}
