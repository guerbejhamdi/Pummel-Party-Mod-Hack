using System;
using UnityEngine;

// Token: 0x02000217 RID: 535
public class RhythmCamera : MonoBehaviour
{
	// Token: 0x06000FA6 RID: 4006 RVA: 0x0007C8C8 File Offset: 0x0007AAC8
	public void Init()
	{
		this.m_initialRot = base.transform.localRotation.eulerAngles;
		this.m_curRot = this.m_initialRot;
		this.UpdateTargetRot();
		this.m_isInitialized = true;
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x0007C908 File Offset: 0x0007AB08
	public void Update()
	{
		if (this.m_isInitialized)
		{
			this.m_curRot = Vector3.SmoothDamp(this.m_curRot, this.m_targetRot, ref this.m_rotVelocity, this.m_updateAngleTime);
			base.transform.localRotation = Quaternion.Euler(this.m_curRot);
			this.m_updateTime += Time.deltaTime;
			if (this.m_updateTime > this.m_updateAngleTime)
			{
				this.UpdateTargetRot();
			}
		}
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x0007C97C File Offset: 0x0007AB7C
	private void UpdateTargetRot()
	{
		float num = UnityEngine.Random.Range(this.m_minAngleOffsets.x, this.m_maxAngleOffsets.x);
		float num2 = UnityEngine.Random.Range(this.m_minAngleOffsets.y, this.m_maxAngleOffsets.y);
		float num3 = UnityEngine.Random.Range(this.m_minAngleOffsets.z, this.m_maxAngleOffsets.z);
		if (UnityEngine.Random.value > 0.5f)
		{
			num = -num;
		}
		if (UnityEngine.Random.value > 0.5f)
		{
			num2 = -num2;
		}
		if (UnityEngine.Random.value > 0.5f)
		{
			num3 = -num3;
		}
		this.m_updateTime = 0f;
		this.m_startRot = base.transform.localRotation.eulerAngles;
		this.m_targetRot = this.m_initialRot + new Vector3(num, num2, num3);
	}

	// Token: 0x04000FBF RID: 4031
	[SerializeField]
	protected Vector3 m_maxAngleOffsets;

	// Token: 0x04000FC0 RID: 4032
	[SerializeField]
	protected Vector3 m_minAngleOffsets;

	// Token: 0x04000FC1 RID: 4033
	[SerializeField]
	protected float m_updateAngleTime = 4f;

	// Token: 0x04000FC2 RID: 4034
	private Vector3 m_initialRot;

	// Token: 0x04000FC3 RID: 4035
	private Vector3 m_startRot;

	// Token: 0x04000FC4 RID: 4036
	private Vector3 m_targetRot;

	// Token: 0x04000FC5 RID: 4037
	private Vector3 m_curRot;

	// Token: 0x04000FC6 RID: 4038
	private Vector3 m_rotVelocity;

	// Token: 0x04000FC7 RID: 4039
	private float m_updateTime;

	// Token: 0x04000FC8 RID: 4040
	private float m_nextUpdate;

	// Token: 0x04000FC9 RID: 4041
	private bool m_isInitialized;
}
