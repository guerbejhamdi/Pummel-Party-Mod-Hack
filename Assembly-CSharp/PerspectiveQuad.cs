using System;
using UnityEngine;

// Token: 0x0200045E RID: 1118
public class PerspectiveQuad : MonoBehaviour
{
	// Token: 0x06001E82 RID: 7810 RVA: 0x000C57E8 File Offset: 0x000C39E8
	private void Update()
	{
		float num = this.cam.nearClipPlane + 0.01f;
		base.transform.position = this.cam.transform.position + this.cam.transform.forward * num;
		base.transform.LookAt(this.cam.transform);
		base.transform.Rotate(90f, 0f, 0f);
		float num2 = Mathf.Tan(this.cam.fieldOfView * 0.017453292f * 0.5f) * num * 2f / 10f;
		base.transform.localScale = new Vector3(num2 * this.cam.aspect, 1f, num2);
	}

	// Token: 0x04002180 RID: 8576
	public Camera cam;
}
