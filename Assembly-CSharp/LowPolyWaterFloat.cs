using System;
using UnityEngine;

// Token: 0x0200041B RID: 1051
public class LowPolyWaterFloat : MonoBehaviour
{
	// Token: 0x06001D26 RID: 7462 RVA: 0x00015810 File Offset: 0x00013A10
	private void Start()
	{
		this.curInterval = UnityEngine.Random.Range(this.minInterval, this.maxInterval);
	}

	// Token: 0x06001D27 RID: 7463 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x06001D28 RID: 7464 RVA: 0x000BF214 File Offset: 0x000BD414
	private void FixedUpdate()
	{
		this.GetTargets();
		base.transform.position = new Vector3(base.transform.position.x, this.floatHeight + this.targetHeight, base.transform.position.z);
		base.transform.rotation = Quaternion.Euler(new Vector3(this.targetZRot, base.transform.rotation.eulerAngles.y, this.targetXRot));
	}

	// Token: 0x06001D29 RID: 7465 RVA: 0x000BF2A0 File Offset: 0x000BD4A0
	private void GetTargets()
	{
		float num = this.SampleHeight(new Vector2(base.transform.position.x, base.transform.position.z));
		float num2 = this.SampleHeight(new Vector2(base.transform.position.x - this.xDist, base.transform.position.z)) * this.strength;
		float num3 = this.SampleHeight(new Vector2(base.transform.position.x + this.xDist, base.transform.position.z)) * this.strength;
		float num4 = this.SampleHeight(new Vector2(base.transform.position.x, base.transform.position.z - this.zDist)) * this.strength;
		float num5 = this.SampleHeight(new Vector2(base.transform.position.x, base.transform.position.z + this.zDist)) * this.strength;
		this.targetHeight = (this.averageHeight ? ((num + num2 + num3 + num4 + num5) / 5f) : num);
		Vector2 vector = new Vector2(0f, num);
		float num6 = this.Angle(new Vector2(-this.xDist, num2), vector);
		float num7 = this.Angle(vector, new Vector2(this.xDist, num3));
		float num8 = this.Angle(new Vector2(-this.zDist, num4), vector);
		float num9 = this.Angle(vector, new Vector2(this.zDist, num5));
		this.targetZRot = -((num8 + num9) / 2f);
		this.targetXRot = (num6 + num7) / 2f;
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x000BF470 File Offset: 0x000BD670
	private float Angle(Vector2 v1, Vector2 v2)
	{
		float x = v2.x - v1.x;
		return Mathf.Atan2(v2.y - v1.y, x) * 57.295776f;
	}

	// Token: 0x06001D2B RID: 7467 RVA: 0x000BF4A4 File Offset: 0x000BD6A4
	private float GetWave(Vector2 vert, float scale, Vector2 sPos)
	{
		float num = Vector2.Distance(vert * scale, sPos);
		return Mathf.Sin((Time.timeSinceLevelLoad + this.curInterval) * this.waveSpeed + num) * this.waveHeight;
	}

	// Token: 0x06001D2C RID: 7468 RVA: 0x000BF4E0 File Offset: 0x000BD6E0
	private float SampleHeight(Vector2 pos)
	{
		float num = 32f;
		return this.GetWave(pos, this.waveScale.x, new Vector2(0f, -num)) + this.GetWave(pos, this.waveScale.y, new Vector2(-num, num)) + this.GetWave(pos, this.waveScale.z, new Vector2(num, num));
	}

	// Token: 0x04001FA6 RID: 8102
	public float floatHeight = -2f;

	// Token: 0x04001FA7 RID: 8103
	public float waveSpeed = 0.5f;

	// Token: 0x04001FA8 RID: 8104
	public float waveHeight = 0.5f;

	// Token: 0x04001FA9 RID: 8105
	public Vector4 waveScale = Vector4.zero;

	// Token: 0x04001FAA RID: 8106
	public float strength = 1f;

	// Token: 0x04001FAB RID: 8107
	public float xDist = 1f;

	// Token: 0x04001FAC RID: 8108
	public float zDist = 1f;

	// Token: 0x04001FAD RID: 8109
	public bool averageHeight;

	// Token: 0x04001FAE RID: 8110
	private float minInterval = 0.08f;

	// Token: 0x04001FAF RID: 8111
	private float maxInterval = 0.1f;

	// Token: 0x04001FB0 RID: 8112
	private float curInterval;

	// Token: 0x04001FB1 RID: 8113
	private float targetXRot;

	// Token: 0x04001FB2 RID: 8114
	private float targetZRot;

	// Token: 0x04001FB3 RID: 8115
	private float targetHeight;
}
