using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004DF RID: 1247
public class TreasureChest : BoardGoalBase
{
	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x060020EC RID: 8428 RVA: 0x00017E80 File Offset: 0x00016080
	// (set) Token: 0x060020ED RID: 8429 RVA: 0x00017E88 File Offset: 0x00016088
	public StayRelative StayRelative { get; set; }

	// Token: 0x060020EE RID: 8430 RVA: 0x00017E91 File Offset: 0x00016091
	public override void Open()
	{
		base.StartCoroutine(this.OpenChestEffect());
	}

	// Token: 0x060020EF RID: 8431 RVA: 0x00017EA0 File Offset: 0x000160A0
	private IEnumerator OpenChestEffect()
	{
		AudioSystem.PlayOneShot(this.fanfareClip, 0.75f, 0f, 1f);
		yield return new WaitForSeconds(0.5f);
		AudioSystem.PlayOneShot(this.unlockClip, 0.25f, 0f, 1f);
		yield return new WaitForSeconds(0.25f);
		this.anim.ResetTrigger("ChestFall");
		this.anim.SetTrigger("OpenChest");
		yield return new WaitForSeconds(1f);
		AudioSystem.PlayOneShot(this.confettiPop, 2f, 0f, 1f);
		this.confettiParticles[0].Emit(100);
		yield return new WaitForSeconds(0.4f);
		AudioSystem.PlayOneShot(this.confettiPop, 1f, 0f, 1f);
		this.confettiParticles[1].Emit(100);
		yield break;
	}

	// Token: 0x060020F0 RID: 8432 RVA: 0x00017EAF File Offset: 0x000160AF
	public override void Despawn()
	{
		base.StartCoroutine(this.CloseChestEffect());
	}

	// Token: 0x060020F1 RID: 8433 RVA: 0x00017EBE File Offset: 0x000160BE
	private IEnumerator CloseChestEffect()
	{
		this.anim.ResetTrigger("ChestFall");
		this.anim.SetTrigger("CloseChest");
		yield return new WaitForSeconds(0.15f);
		AudioSystem.PlayOneShot(this.despawnWoosh, 1f, 0f, 1f);
		yield break;
	}

	// Token: 0x060020F2 RID: 8434 RVA: 0x00017ECD File Offset: 0x000160CD
	public override void Spawn()
	{
		this.chestFall = true;
		this.anim.ResetTrigger("ChestFall");
		this.anim.SetTrigger("ChestFall");
	}

	// Token: 0x060020F3 RID: 8435 RVA: 0x00017EF6 File Offset: 0x000160F6
	public void OnDropHitGround()
	{
		if (this.chestFall)
		{
			this.chestFall = false;
		}
	}

	// Token: 0x060020F4 RID: 8436 RVA: 0x00017F07 File Offset: 0x00016107
	public void OnDropHitSOund()
	{
		if (this.chestFall)
		{
			AudioSystem.PlayOneShot(this.hitGroundSound, 0.5f, 0f, 1f);
		}
	}

	// Token: 0x040023AE RID: 9134
	public Animator anim;

	// Token: 0x040023AF RID: 9135
	public AudioClip hitGroundSound;

	// Token: 0x040023B0 RID: 9136
	public AudioClip unlockClip;

	// Token: 0x040023B1 RID: 9137
	public AudioClip fanfareClip;

	// Token: 0x040023B2 RID: 9138
	public ParticleSystem[] confettiParticles;

	// Token: 0x040023B3 RID: 9139
	public AudioClip confettiPop;

	// Token: 0x040023B4 RID: 9140
	public AudioClip despawnWoosh;

	// Token: 0x040023B5 RID: 9141
	public bool chestFall = true;
}
