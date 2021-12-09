using System;
using UnityEngine;

// Token: 0x02000362 RID: 866
public class BeeHiveSwing : MonoBehaviour
{
	// Token: 0x0600174B RID: 5963 RVA: 0x00011633 File Offset: 0x0000F833
	public void Awake()
	{
		this.m_lastPosition = base.transform.position;
	}

	// Token: 0x0600174C RID: 5964 RVA: 0x00011646 File Offset: 0x0000F846
	public void Update()
	{
		if (!this.m_lateUpdate)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x0600174D RID: 5965 RVA: 0x00011656 File Offset: 0x0000F856
	public void LateUpdate()
	{
		if (this.m_lateUpdate)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x0600174E RID: 5966 RVA: 0x000A1CEC File Offset: 0x0009FEEC
	private void DoUpdate()
	{
		Vector3 b = base.transform.position - this.m_lastPosition;
		this.m_wobble += b;
		float num = (Mathf.Sin(this.m_swingWaveTime * this.m_swingFrequency) + 1f) * 0.5f;
		this.m_swingWaveTime += Time.deltaTime;
		this.m_lastPosition = base.transform.position;
		this.m_wobble = Vector3.MoveTowards(this.m_wobble, Vector3.zero, Time.deltaTime / this.m_restTime * Mathf.Max(0.01f, this.m_wobble.magnitude));
		if (!this.m_localRotation)
		{
			base.transform.rotation = Quaternion.Euler(this.m_wobble.z * this.m_maxSwingAngle * num, 0f, -this.m_wobble.x * this.m_maxSwingAngle * num);
			return;
		}
		if (this.m_additive)
		{
			base.transform.localRotation *= Quaternion.Euler(this.m_wobble.z * this.m_maxSwingAngle * num, 0f, -this.m_wobble.x * this.m_maxSwingAngle * num);
			return;
		}
		base.transform.localRotation = Quaternion.Euler(this.m_wobble.z * this.m_maxSwingAngle * num, 0f, -this.m_wobble.x * this.m_maxSwingAngle * num);
	}

	// Token: 0x0400189D RID: 6301
	[SerializeField]
	private bool m_lateUpdate;

	// Token: 0x0400189E RID: 6302
	[SerializeField]
	private bool m_additive;

	// Token: 0x0400189F RID: 6303
	[SerializeField]
	private float m_restTime = 0.75f;

	// Token: 0x040018A0 RID: 6304
	[SerializeField]
	private float m_maxSwingAngle = 20f;

	// Token: 0x040018A1 RID: 6305
	[SerializeField]
	private float m_swingFrequency = 15f;

	// Token: 0x040018A2 RID: 6306
	[SerializeField]
	private bool m_localRotation;

	// Token: 0x040018A3 RID: 6307
	private Vector3 m_wobble;

	// Token: 0x040018A4 RID: 6308
	private Vector3 m_lastPosition;

	// Token: 0x040018A5 RID: 6309
	private float m_swingWaveTime;
}
