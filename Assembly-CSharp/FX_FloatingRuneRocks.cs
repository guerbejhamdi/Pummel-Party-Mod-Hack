using System;
using UnityEngine;

// Token: 0x020001A6 RID: 422
public class FX_FloatingRuneRocks : MonoBehaviour
{
	// Token: 0x06000C19 RID: 3097 RVA: 0x000658A8 File Offset: 0x00063AA8
	private void Update()
	{
		this.m_time += Time.deltaTime;
		for (int i = 0; i < this.m_rocks.Length; i++)
		{
			float num = this.m_angleOffset * (float)i + this.m_time * this.m_rotateSpeed;
			Vector3 vector = new Vector3(Mathf.Sin(num * 0.017453292f), 0f, Mathf.Cos(num * 0.017453292f));
			vector *= this.m_distance;
			this.m_rocks[i].transform.position = base.transform.position + vector + new Vector3(0f, this.m_bounceCurve.Evaluate(this.m_time * this.m_bounceSpeed) * this.m_bounceStrength, 0f);
			this.m_rocks[i].transform.localRotation = Quaternion.LookRotation((base.transform.position - vector).normalized);
		}
	}

	// Token: 0x04000B4C RID: 2892
	[SerializeField]
	private GameObject[] m_rocks;

	// Token: 0x04000B4D RID: 2893
	[SerializeField]
	private float m_angleOffset = 90f;

	// Token: 0x04000B4E RID: 2894
	[SerializeField]
	private float m_distance = 5f;

	// Token: 0x04000B4F RID: 2895
	[SerializeField]
	private float m_rotateSpeed = 90f;

	// Token: 0x04000B50 RID: 2896
	[SerializeField]
	private AnimationCurve m_bounceCurve;

	// Token: 0x04000B51 RID: 2897
	[SerializeField]
	private float m_bounceSpeed;

	// Token: 0x04000B52 RID: 2898
	[SerializeField]
	private float m_bounceStrength;

	// Token: 0x04000B53 RID: 2899
	private float m_time;
}
