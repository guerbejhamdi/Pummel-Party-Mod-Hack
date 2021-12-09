using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class BannerRotation : MonoBehaviour
{
	// Token: 0x0600002D RID: 45 RVA: 0x0002B218 File Offset: 0x00029418
	private void Update()
	{
		float y = this.m_curve.Evaluate(this.m_speed * Time.time) * this.m_magnitude;
		base.transform.localRotation = Quaternion.Euler(0f, y, 0f);
	}

	// Token: 0x04000028 RID: 40
	[SerializeField]
	protected AnimationCurve m_curve;

	// Token: 0x04000029 RID: 41
	[SerializeField]
	protected float m_speed;

	// Token: 0x0400002A RID: 42
	[SerializeField]
	protected float m_magnitude;
}
