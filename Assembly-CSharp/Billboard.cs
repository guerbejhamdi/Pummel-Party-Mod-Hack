using System;
using UnityEngine;

// Token: 0x02000337 RID: 823
public class Billboard : MonoBehaviour
{
	// Token: 0x0600165E RID: 5726 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x0600165F RID: 5727 RVA: 0x0009F1CC File Offset: 0x0009D3CC
	private void Update()
	{
		Vector3 vector = base.transform.position - Camera.main.transform.position;
		base.transform.rotation = Quaternion.AngleAxis(this.curRot % 360f, vector.normalized) * Quaternion.LookRotation(vector.normalized);
		this.curRot += this.rotSpeed * Time.deltaTime;
	}

	// Token: 0x04001794 RID: 6036
	private float curRot;

	// Token: 0x04001795 RID: 6037
	public float rotSpeed = 10f;
}
