using System;
using UnityEngine;

// Token: 0x0200032B RID: 811
public class Scaler : MonoBehaviour
{
	// Token: 0x06001619 RID: 5657 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x0600161A RID: 5658 RVA: 0x00010AA4 File Offset: 0x0000ECA4
	private void Update()
	{
		base.transform.localScale = Vector3.one * this.curve.Evaluate(Time.realtimeSinceStartup % this.pulseRate / this.pulseRate);
	}

	// Token: 0x04001749 RID: 5961
	public AnimationCurve curve;

	// Token: 0x0400174A RID: 5962
	public float pulseRate = 1f;
}
