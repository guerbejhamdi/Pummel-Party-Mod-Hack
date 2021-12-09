using System;
using UnityEngine;

// Token: 0x02000391 RID: 913
public class PunchingGlove : MonoBehaviour
{
	// Token: 0x06001897 RID: 6295 RVA: 0x000A821C File Offset: 0x000A641C
	private void Awake()
	{
		this.m_body = this.m_glove.GetComponent<Rigidbody>();
		this.m_joint = this.m_glove.GetComponent<SpringJoint>();
		this.m_initialSpring = this.m_joint.spring;
		this.m_punchTimeElapsed = this.m_punchTime;
	}

	// Token: 0x06001898 RID: 6296 RVA: 0x0001232C File Offset: 0x0001052C
	private void Update()
	{
		if (this.m_wasFired)
		{
			this.UpdateSpring();
		}
	}

	// Token: 0x06001899 RID: 6297 RVA: 0x000A8268 File Offset: 0x000A6468
	private void UpdateSpring()
	{
		float time = this.m_punchTimeElapsed / this.m_punchTime;
		this.m_joint.spring = this.m_springCurve.Evaluate(time) * this.m_initialSpring;
		this.m_punchTimeElapsed += Time.deltaTime;
	}

	// Token: 0x0600189A RID: 6298 RVA: 0x000A82B4 File Offset: 0x000A64B4
	public void Fire()
	{
		Vector3 force = base.transform.right * -this.m_forceMultiplier;
		this.m_punchTimeElapsed = 0f;
		this.UpdateSpring();
		this.m_body.AddForce(force, ForceMode.Impulse);
		AudioSystem.PlayOneShot(this.m_pressureRelease, 0.15f, 0f, 1f);
		AudioSystem.PlayOneShot(this.m_woosh, 0.25f, 0f, 1f);
		this.m_steamPressure.Emit(50);
		this.m_wasFired = true;
	}

	// Token: 0x0600189B RID: 6299 RVA: 0x0000398C File Offset: 0x00001B8C
	public void OnGloveImpact(Collision collision)
	{
	}

	// Token: 0x0600189C RID: 6300 RVA: 0x000A8340 File Offset: 0x000A6540
	public void OnHit(Vector3 hitPoint)
	{
		AudioSystem.PlayOneShot(this.m_impact, 0.25f, 0f, 1f);
		UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(this.m_impactEffect, hitPoint, Quaternion.identity), 1f);
		GameManager.Board.boardCamera.AddShake(0.55f);
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x0001233C File Offset: 0x0001053C
	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.m_glove);
	}

	// Token: 0x04001A21 RID: 6689
	[Header("General")]
	[SerializeField]
	protected GameObject m_glove;

	// Token: 0x04001A22 RID: 6690
	[Header("Fire Options")]
	[SerializeField]
	protected float m_forceMultiplier = 75f;

	// Token: 0x04001A23 RID: 6691
	[SerializeField]
	protected float m_punchTime = 1f;

	// Token: 0x04001A24 RID: 6692
	[SerializeField]
	protected AnimationCurve m_springCurve;

	// Token: 0x04001A25 RID: 6693
	[Header("Audio")]
	[SerializeField]
	protected AudioClip m_pressureRelease;

	// Token: 0x04001A26 RID: 6694
	[SerializeField]
	protected AudioClip m_woosh;

	// Token: 0x04001A27 RID: 6695
	[SerializeField]
	protected AudioClip m_impact;

	// Token: 0x04001A28 RID: 6696
	[Header("FX")]
	[SerializeField]
	protected ParticleSystem m_steamPressure;

	// Token: 0x04001A29 RID: 6697
	[SerializeField]
	protected GameObject m_impactEffect;

	// Token: 0x04001A2A RID: 6698
	private float m_punchTimeElapsed;

	// Token: 0x04001A2B RID: 6699
	private float m_initialSpring;

	// Token: 0x04001A2C RID: 6700
	private bool m_wasFired;

	// Token: 0x04001A2D RID: 6701
	private Rigidbody m_body;

	// Token: 0x04001A2E RID: 6702
	private SpringJoint m_joint;
}
