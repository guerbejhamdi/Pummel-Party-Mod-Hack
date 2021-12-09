using System;
using UnityEngine;

// Token: 0x0200025D RID: 605
public class MinigameSpotlight
{
	// Token: 0x060011A7 RID: 4519 RVA: 0x000895B0 File Offset: 0x000877B0
	public MinigameSpotlight(Vector3 pos, float angle, float yRot, float yRotSpeed, float xRotSpeed, GameObject gameObject)
	{
		this.pos = pos;
		this.angle = angle;
		this.yRotSpeed = yRotSpeed;
		this.xRotSpeed = xRotSpeed;
		this.gameObject = gameObject;
		this.yRot = yRot;
		this.xRot = 45f;
		this.light = gameObject.GetComponent<Light>();
		this.light.spotAngle = angle;
	}

	// Token: 0x04001257 RID: 4695
	public Vector3 pos;

	// Token: 0x04001258 RID: 4696
	public float angle;

	// Token: 0x04001259 RID: 4697
	public float yRotSpeed;

	// Token: 0x0400125A RID: 4698
	public float xRotSpeed;

	// Token: 0x0400125B RID: 4699
	public GameObject gameObject;

	// Token: 0x0400125C RID: 4700
	public Light light;

	// Token: 0x0400125D RID: 4701
	public float xRot;

	// Token: 0x0400125E RID: 4702
	public float yRot;
}
