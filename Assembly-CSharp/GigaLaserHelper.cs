using System;
using UnityEngine;

// Token: 0x0200031C RID: 796
public class GigaLaserHelper : MonoBehaviour
{
	// Token: 0x060015C4 RID: 5572 RVA: 0x00010764 File Offset: 0x0000E964
	public void Awake()
	{
		this.m_anim = base.GetComponentInChildren<Animator>();
		this.m_anim.SetBool("Visible", true);
		this.m_visible = true;
	}

	// Token: 0x060015C5 RID: 5573 RVA: 0x0001078A File Offset: 0x0000E98A
	public void LateUpdate()
	{
		if (this.m_targetRot != Quaternion.identity && this.m_targetSet)
		{
			this.m_turret.transform.rotation *= this.m_targetRot;
		}
	}

	// Token: 0x060015C6 RID: 5574 RVA: 0x0009C87C File Offset: 0x0009AA7C
	public void SetTargetPosition(Vector3 p)
	{
		this.m_targetRot = Quaternion.LookRotation((p - this.m_turret.transform.position).normalized);
		this.m_targetSet = true;
	}

	// Token: 0x060015C7 RID: 5575 RVA: 0x000107C7 File Offset: 0x0000E9C7
	public void Spawn()
	{
		this.m_anim.SetBool("Visible", true);
		this.m_visible = true;
	}

	// Token: 0x060015C8 RID: 5576 RVA: 0x000107E1 File Offset: 0x0000E9E1
	public void Despawn()
	{
		this.m_anim.SetBool("Visible", false);
		this.m_visible = false;
	}

	// Token: 0x060015C9 RID: 5577 RVA: 0x000107FB File Offset: 0x0000E9FB
	public void Fire()
	{
		this.m_anim.SetTrigger("Fire");
	}

	// Token: 0x060015CA RID: 5578 RVA: 0x0001080D File Offset: 0x0000EA0D
	public void HitGround()
	{
		AudioSystem.PlayOneShot(this.m_baseLandClip, 1f, 0f, 1f);
	}

	// Token: 0x060015CB RID: 5579 RVA: 0x00010829 File Offset: 0x0000EA29
	public void TurretLand()
	{
		AudioSystem.PlayOneShot(this.m_turretLandClip, 1f, 0f, 1f);
	}

	// Token: 0x040016EC RID: 5868
	[SerializeField]
	private Transform m_turret;

	// Token: 0x040016ED RID: 5869
	[SerializeField]
	private bool m_allowTurretRotation = true;

	// Token: 0x040016EE RID: 5870
	[SerializeField]
	private AudioClip m_baseLandClip;

	// Token: 0x040016EF RID: 5871
	[SerializeField]
	private AudioClip m_turretLandClip;

	// Token: 0x040016F0 RID: 5872
	private Animator m_anim;

	// Token: 0x040016F1 RID: 5873
	private Quaternion m_targetRot = Quaternion.identity;

	// Token: 0x040016F2 RID: 5874
	private bool m_targetSet;

	// Token: 0x040016F3 RID: 5875
	private bool m_visible;
}
