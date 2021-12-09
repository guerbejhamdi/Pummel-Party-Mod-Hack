using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A0 RID: 160
public class FX_PrisonerCage : BoardGoalBase
{
	// Token: 0x0600035D RID: 861 RVA: 0x00005CC1 File Offset: 0x00003EC1
	public void OnEnable()
	{
		this.m_anim.SetBool("Spawned", this.spawned);
	}

	// Token: 0x0600035E RID: 862 RVA: 0x00038E40 File Offset: 0x00037040
	public void Awake()
	{
		foreach (object obj in this.m_debriParent)
		{
			Transform transform = (Transform)obj;
			Rigidbody component = transform.GetComponent<Rigidbody>();
			this.m_debri.Add(new PrisonerCageDebri(transform.gameObject, component));
		}
	}

	// Token: 0x0600035F RID: 863 RVA: 0x00005CD9 File Offset: 0x00003ED9
	public override void Spawn()
	{
		this.spawned = true;
		this.Reset();
		base.StartCoroutine(this.SpawnLater());
	}

	// Token: 0x06000360 RID: 864 RVA: 0x00005CF5 File Offset: 0x00003EF5
	private IEnumerator SpawnLater()
	{
		yield return null;
		this.m_anim.SetBool("Spawned", true);
		this.m_spawnParticle.Play();
		AudioSystem.PlayOneShot(this.m_spawnSound, 2f, 0f, 1f);
		AudioSystem.PlayOneShot(this.m_spawnFlashSound, 2f, 0f, 1f);
		yield break;
	}

	// Token: 0x06000361 RID: 865 RVA: 0x00038EB0 File Offset: 0x000370B0
	public override void Open()
	{
		this.m_spawnParticle.Stop();
		this.m_spawnParticle.Play();
		this.m_holyParticle.gameObject.SetActive(true);
		this.m_holyParticle.Stop();
		this.m_holyParticle.Play();
		this.m_holySparkles.gameObject.SetActive(true);
		this.m_holySparkles.Stop();
		this.m_holySparkles.Play();
		this.m_debriParent.gameObject.SetActive(true);
		AudioSystem.PlayOneShot(this.m_spawnFlashSound, 2f, 0f, 1f);
		foreach (PrisonerCageDebri prisonerCageDebri in this.m_debri)
		{
			prisonerCageDebri.body.velocity = UnityEngine.Random.insideUnitSphere * 4f;
			prisonerCageDebri.body.angularVelocity = UnityEngine.Random.insideUnitSphere * 180f - Vector3.one * 90f;
		}
		this.m_anim.SetTrigger("Open");
		this.m_ghostAnim.SetBool("IsAscending", true);
	}

	// Token: 0x06000362 RID: 866 RVA: 0x0000398C File Offset: 0x00001B8C
	public override void Despawn()
	{
	}

	// Token: 0x06000363 RID: 867 RVA: 0x00038FF4 File Offset: 0x000371F4
	public void Reset()
	{
		this.m_anim.SetBool("Spawned", false);
		foreach (object obj in base.transform)
		{
			((Transform)obj).gameObject.SetActive(false);
		}
		this.m_spawnParticle.Stop();
		this.m_debriParent.gameObject.SetActive(false);
		foreach (PrisonerCageDebri prisonerCageDebri in this.m_debri)
		{
			prisonerCageDebri.Reset();
		}
	}

	// Token: 0x06000364 RID: 868 RVA: 0x00005D04 File Offset: 0x00003F04
	public void PlaySound()
	{
		AudioSystem.PlayOneShot(this.holySound, 1f, 0f, 1f);
	}

	// Token: 0x0400036F RID: 879
	[SerializeField]
	protected Animator m_anim;

	// Token: 0x04000370 RID: 880
	[SerializeField]
	protected ParticleSystem m_spawnParticle;

	// Token: 0x04000371 RID: 881
	[SerializeField]
	protected ParticleSystem m_holyParticle;

	// Token: 0x04000372 RID: 882
	[SerializeField]
	protected ParticleSystem m_holySparkles;

	// Token: 0x04000373 RID: 883
	[SerializeField]
	protected AudioClip m_spawnSound;

	// Token: 0x04000374 RID: 884
	[SerializeField]
	protected AudioClip m_spawnFlashSound;

	// Token: 0x04000375 RID: 885
	[SerializeField]
	protected Transform m_debriParent;

	// Token: 0x04000376 RID: 886
	[SerializeField]
	protected Transform m_debriExplosionSource;

	// Token: 0x04000377 RID: 887
	[SerializeField]
	protected Animator m_ghostAnim;

	// Token: 0x04000378 RID: 888
	private List<PrisonerCageDebri> m_debri = new List<PrisonerCageDebri>();

	// Token: 0x04000379 RID: 889
	private bool m_isSpawning;

	// Token: 0x0400037A RID: 890
	private bool spawned;

	// Token: 0x0400037B RID: 891
	public AudioClip holySound;
}
