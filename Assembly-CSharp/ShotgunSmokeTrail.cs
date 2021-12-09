using System;
using UnityEngine;

// Token: 0x02000411 RID: 1041
public class ShotgunSmokeTrail : MonoBehaviour
{
	// Token: 0x06001D0A RID: 7434 RVA: 0x000BE6B8 File Offset: 0x000BC8B8
	public void Setup(Vector3 startPos, Vector3 endPos)
	{
		this.lr.startWidth = this.startWidth;
		this.lr.endWidth = this.startWidth;
		this.lr.SetPosition(0, startPos);
		this.lr.SetPosition(1, endPos);
		this.lr.material = new Material(this.lr.material);
	}

	// Token: 0x06001D0B RID: 7435 RVA: 0x000BE71C File Offset: 0x000BC91C
	private void Update()
	{
		this.lr.endWidth += Time.deltaTime * this.smokeWidthSpeed;
		this.lr.startWidth += Time.deltaTime * this.smokeWidthSpeed;
		Color color = this.lr.material.GetColor("_TintColor");
		color.a -= Time.deltaTime * this.smokeAlphaSpeed;
		this.lr.material.SetColor("_TintColor", color);
		for (int i = 0; i < this.lr.positionCount; i++)
		{
			this.lr.SetPosition(i, this.lr.GetPosition(i) + Vector3.up * this.upSpeed * Time.deltaTime);
		}
		if (color.a <= 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04001F70 RID: 8048
	public float startWidth = 0.5f;

	// Token: 0x04001F71 RID: 8049
	public float smokeWidthSpeed = 1.5f;

	// Token: 0x04001F72 RID: 8050
	public float smokeAlphaSpeed = 2.5f;

	// Token: 0x04001F73 RID: 8051
	public float upSpeed = 2f;

	// Token: 0x04001F74 RID: 8052
	public LineRenderer lr;
}
