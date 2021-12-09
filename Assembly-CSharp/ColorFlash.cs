using System;
using UnityEngine;

// Token: 0x0200033B RID: 827
public class ColorFlash : MonoBehaviour
{
	// Token: 0x06001677 RID: 5751 RVA: 0x00010EF3 File Offset: 0x0000F0F3
	private void Start()
	{
		this.baseMat = base.GetComponent<MeshRenderer>().materials[0];
		this.baseMat.EnableKeyword("_EMISSION");
		this.spawnTime = Time.time;
	}

	// Token: 0x06001678 RID: 5752 RVA: 0x0009F5D0 File Offset: 0x0009D7D0
	private void Update()
	{
		this.baseMat.SetColor("_EmissionColor", Color.Lerp(this.baseColor, this.flashColor, Mathf.PingPong(this.t, 1f)) * this.emissionIntensity);
		float num = Time.time - this.spawnTime;
		this.t += this.speed * (num * num / this.flashLength) * Time.deltaTime;
	}

	// Token: 0x040017A1 RID: 6049
	public Color baseColor = Color.white;

	// Token: 0x040017A2 RID: 6050
	public Color flashColor = Color.red;

	// Token: 0x040017A3 RID: 6051
	public float flashLength = 10f;

	// Token: 0x040017A4 RID: 6052
	public float speed = 0.03f;

	// Token: 0x040017A5 RID: 6053
	public float emissionIntensity = 1.5f;

	// Token: 0x040017A6 RID: 6054
	private Material baseMat;

	// Token: 0x040017A7 RID: 6055
	private float spawnTime;

	// Token: 0x040017A8 RID: 6056
	private float t;
}
