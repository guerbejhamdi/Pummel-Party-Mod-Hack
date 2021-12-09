using System;
using UnityEngine;

// Token: 0x02000241 RID: 577
[Serializable]
public class LeafShadowAnimator
{
	// Token: 0x060010B3 RID: 4275 RVA: 0x0000DEAE File Offset: 0x0000C0AE
	public float GetTime()
	{
		return Mathf.Sin(Time.realtimeSinceStartup * this.curFrequency) * this.curAmplitude;
	}

	// Token: 0x060010B4 RID: 4276 RVA: 0x00082D58 File Offset: 0x00080F58
	public void Update(float deltaTime)
	{
		if (Time.realtimeSinceStartup >= this.nextChangeTime)
		{
			this.targetAmpltidue = UnityEngine.Random.Range(this.minAmplitude, this.maxAmplitude);
			this.curChangeLength = UnityEngine.Random.Range(this.minChangeTime, this.maxChangeTime);
			this.nextChangeTime = Time.realtimeSinceStartup + this.curChangeLength;
		}
		this.curAmplitude = Mathf.SmoothDamp(this.curAmplitude, this.targetAmpltidue, ref this.curAmplitudeVelocity, this.curChangeLength);
	}

	// Token: 0x04001127 RID: 4391
	public float minAmplitude;

	// Token: 0x04001128 RID: 4392
	public float maxAmplitude = 1f;

	// Token: 0x04001129 RID: 4393
	public float minFrequency = 1f;

	// Token: 0x0400112A RID: 4394
	public float maxFrequency = 2f;

	// Token: 0x0400112B RID: 4395
	public float minChangeTime = 1f;

	// Token: 0x0400112C RID: 4396
	public float maxChangeTime = 3f;

	// Token: 0x0400112D RID: 4397
	private float curAmplitude = 1f;

	// Token: 0x0400112E RID: 4398
	private float curAmplitudeVelocity;

	// Token: 0x0400112F RID: 4399
	private float targetAmpltidue = 1f;

	// Token: 0x04001130 RID: 4400
	public float curFrequency = 1f;

	// Token: 0x04001131 RID: 4401
	private float curChangeLength = 1f;

	// Token: 0x04001132 RID: 4402
	private float nextChangeTime;
}
