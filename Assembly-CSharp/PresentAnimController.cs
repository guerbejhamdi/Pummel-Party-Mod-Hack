using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002FD RID: 765
public class PresentAnimController : MonoBehaviour
{
	// Token: 0x0600153E RID: 5438 RVA: 0x00010311 File Offset: 0x0000E511
	public IEnumerator Open()
	{
		this.anim.Play("OpenPresentItem2");
		yield return new WaitForSeconds(2f);
		yield break;
	}

	// Token: 0x0600153F RID: 5439 RVA: 0x00010320 File Offset: 0x0000E520
	public void OnEnable()
	{
		if (this.swallowed)
		{
			this.anim.Play("PresentIdle2");
		}
	}

	// Token: 0x06001540 RID: 5440 RVA: 0x0009AAB4 File Offset: 0x00098CB4
	public void OnThrowLid()
	{
		this.lidCollider.enabled = true;
		this.lidRigidbody = this.lidCollider.gameObject.AddComponent<Rigidbody>();
		this.lidRigidbody.AddForceAtPosition(new Vector3(-2.5f, 5f, -2.5f) * 100f, this.lidCollider.transform.position + new Vector3(0.4f, -0.15f, 0.4f));
		if (this.presentAnim.IsGoodPresent)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.confettiParticlesPrefab, base.transform.position, Quaternion.Euler(-90f, 0f, 0f));
			AudioSystem.PlayOneShot("ConfettiPop_01_CC0_Rudmer_Rotteveel", 0.5f, 0.01f);
			return;
		}
		AudioSystem.PlayOneShot(this.badPresentClip, 0.5f, 0.01f, 1f);
	}

	// Token: 0x06001541 RID: 5441 RVA: 0x0001033B File Offset: 0x0000E53B
	public void OnSwallowMaximum()
	{
		this.item.OnSwallowMaximum();
		this.swallowed = true;
	}

	// Token: 0x06001542 RID: 5442 RVA: 0x0001034F File Offset: 0x0000E54F
	public void OnWarpIn()
	{
		AudioSystem.PlayOneShot(this.warpInClip, 1f, 0f, 1f);
	}

	// Token: 0x06001543 RID: 5443 RVA: 0x0001036B File Offset: 0x0000E56B
	public void OnWarpOut()
	{
		AudioSystem.PlayOneShot(this.warpOutClip, 1f, 0f, 1f);
	}

	// Token: 0x06001544 RID: 5444 RVA: 0x00010387 File Offset: 0x0000E587
	public IEnumerator DoDestroyPresentAnimation()
	{
		float destroyTime = 0.5f;
		float startTime = Time.time;
		UnityEngine.Object.Destroy(this.lidRigidbody);
		yield return null;
		this.lidCollider.enabled = false;
		this.lidCollider.transform.parent = null;
		UnityEngine.Object.Destroy(this.lidCollider.gameObject, 3f);
		this.anim.enabled = false;
		while (Time.time - startTime < destroyTime)
		{
			float num = Mathf.Clamp01((Time.time - startTime) / destroyTime);
			Vector3 localScale = Vector3.one * Easing.BackEaseOut(1f - num);
			this.lidCollider.transform.localScale = localScale;
			base.transform.localScale = localScale;
			yield return null;
		}
		this.lidCollider.transform.localScale = Vector3.zero;
		base.transform.localScale = Vector3.zero;
		yield return null;
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400164D RID: 5709
	public Animation anim;

	// Token: 0x0400164E RID: 5710
	public BoxCollider lidCollider;

	// Token: 0x0400164F RID: 5711
	public MeshRenderer quadRenderer;

	// Token: 0x04001650 RID: 5712
	public PresentItem item;

	// Token: 0x04001651 RID: 5713
	public GameObject confettiParticlesPrefab;

	// Token: 0x04001652 RID: 5714
	public PresentAnim presentAnim;

	// Token: 0x04001653 RID: 5715
	public AudioClip badPresentClip;

	// Token: 0x04001654 RID: 5716
	public AudioClip warpInClip;

	// Token: 0x04001655 RID: 5717
	public AudioClip warpOutClip;

	// Token: 0x04001656 RID: 5718
	private Rigidbody lidRigidbody;

	// Token: 0x04001657 RID: 5719
	private bool swallowed;
}
