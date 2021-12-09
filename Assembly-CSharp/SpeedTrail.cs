using System;
using UnityEngine;

// Token: 0x020004B1 RID: 1201
public class SpeedTrail : MonoBehaviour
{
	// Token: 0x06002006 RID: 8198 RVA: 0x0001765D File Offset: 0x0001585D
	private void Start()
	{
		this.m1 = base.GetComponent<MeshRenderer>().materials[0];
		this.startAlpha1 = this.m1.color.a;
		this.startTime = Time.time;
	}

	// Token: 0x06002007 RID: 8199 RVA: 0x000C9A38 File Offset: 0x000C7C38
	private void Update()
	{
		float num = Time.time - this.startTime;
		if (num > this.fadeTime)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		float num2 = 1f - num / this.fadeTime;
		Color color = this.m1.color;
		color.a = num2 * this.startAlpha1;
		this.m1.color = color;
		float d = this.endScale + (this.startScale - this.endScale) * num2;
		base.transform.localScale = Vector3.one * d;
	}

	// Token: 0x040022DB RID: 8923
	public float fadeTime = 0.05f;

	// Token: 0x040022DC RID: 8924
	public float startScale = 1f;

	// Token: 0x040022DD RID: 8925
	public float endScale = 0.5f;

	// Token: 0x040022DE RID: 8926
	private float startAlpha1;

	// Token: 0x040022DF RID: 8927
	private Material m1;

	// Token: 0x040022E0 RID: 8928
	private float startTime;
}
