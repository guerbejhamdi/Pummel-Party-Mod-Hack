using System;
using UnityEngine;

// Token: 0x02000316 RID: 790
public class FX_AnimationNoise : MonoBehaviour
{
	// Token: 0x060015B3 RID: 5555 RVA: 0x0009C3A4 File Offset: 0x0009A5A4
	private void LateUpdate()
	{
		if (Time.time > this.m_nextUpdate)
		{
			this.m_lastNoiseVec = this.m_noiseVec;
			this.m_noiseVec = UnityEngine.Random.insideUnitSphere * this.m_strength;
			this.m_noiseVec.Scale(this.m_axisMask);
			this.m_nextUpdate = Time.time + this.m_updateRate;
			this.m_timePassed = 0f;
			if (this.m_scaleNoise)
			{
				this.m_scale = 1f + UnityEngine.Random.value * this.m_scaleNoiseStrength;
			}
		}
		float t = Mathf.Clamp01(this.m_timePassed / this.m_updateRate);
		if (this.m_strength > 0f)
		{
			base.transform.position += Vector3.Lerp(this.m_lastNoiseVec, this.m_noiseVec, t);
		}
		if (this.m_scaleNoise)
		{
			Vector3 localScale = base.transform.localScale;
			float num = Mathf.Lerp(this.m_lastScale, this.m_scale, t);
			localScale.Scale(new Vector3(num, num, 1f));
			base.transform.localScale = localScale;
		}
		this.m_timePassed += Time.deltaTime;
	}

	// Token: 0x040016B3 RID: 5811
	[Header("Position")]
	[SerializeField]
	private float m_strength;

	// Token: 0x040016B4 RID: 5812
	[SerializeField]
	private float m_updateRate;

	// Token: 0x040016B5 RID: 5813
	[SerializeField]
	private Vector3 m_axisMask = Vector3.one;

	// Token: 0x040016B6 RID: 5814
	[Header("Scale")]
	[SerializeField]
	private bool m_scaleNoise;

	// Token: 0x040016B7 RID: 5815
	[SerializeField]
	private float m_scaleNoiseStrength = 1f;

	// Token: 0x040016B8 RID: 5816
	private float m_nextUpdate;

	// Token: 0x040016B9 RID: 5817
	private Vector3 m_noiseVec;

	// Token: 0x040016BA RID: 5818
	private Vector3 m_lastNoiseVec;

	// Token: 0x040016BB RID: 5819
	private float m_timePassed;

	// Token: 0x040016BC RID: 5820
	private float m_scale = 1f;

	// Token: 0x040016BD RID: 5821
	private float m_lastScale = 1f;
}
