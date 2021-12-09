using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class LightSwordsPortal : MonoBehaviour
{
	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x0600084A RID: 2122 RVA: 0x00009BBE File Offset: 0x00007DBE
	public bool Finished
	{
		get
		{
			return this.finished;
		}
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x0004D7DC File Offset: 0x0004B9DC
	public void Setup(LightSwordsItem.SwordHit swordHit)
	{
		this.swordHit = swordHit;
		this.mat = new Material(this.swordRenderer.material);
		this.swordRenderer.material = this.mat;
		this.mat.SetVector("_PlaneNormal", -base.transform.forward);
		this.mat.SetVector("_StartPoint", base.transform.position);
		this.heightDif = Vector3.Distance(this.swordRenderer.bounds.center, this.sword.transform.position);
		this.sword.transform.localPosition = new Vector3(0f, 0f, -this.swordRenderer.bounds.extents.z - this.heightDif - 0.5f);
		this.swordHit.endPoint = this.swordHit.endPoint - (swordHit.endPoint - swordHit.startPoint).normalized * this.swordRenderer.bounds.extents.z;
		base.StartCoroutine(this.DoEvent());
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x00009BC6 File Offset: 0x00007DC6
	private IEnumerator DoEvent()
	{
		float speed = 45f;
		AudioSystem.PlayOneShot(this.portalSpawnSound, 0.5f, 0f, 1f);
		while (this.sword.transform.localPosition.z < 0f)
		{
			this.sword.transform.localPosition += Vector3.forward * Time.deltaTime * 0.5f;
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		AudioSystem.PlayOneShot(this.swordShootSound, 0.25f, 0f, 1f);
		if (this.swordHit.hit)
		{
			while (this.sword.transform.position != this.swordHit.hitPoint)
			{
				this.sword.transform.position = this.sword.transform.position.ZPMoveTowards(this.swordHit.hitPoint, Time.deltaTime * speed);
				yield return null;
			}
			bool isLastHit = this.swordHit.isLastHit;
			DamageInstance d = new DamageInstance
			{
				damage = 1,
				origin = this.swordHit.startPoint,
				blood = true,
				ragdoll = this.swordHit.isLastHit,
				ragdollVel = 10f,
				bloodVel = 5f,
				bloodAmount = 0.3f,
				details = "Light Sword",
				removeKeys = true
			};
			GameManager.GetPlayerAt((int)this.swordHit.hitPlayerSlot).BoardObject.ApplyDamage(d);
			AudioSystem.PlayOneShot(this.hitSound, 0.25f, 0f, 1f);
			while (this.sword.transform.position != this.swordHit.endPoint)
			{
				this.sword.transform.position = this.sword.transform.position.ZPMoveTowards(this.swordHit.endPoint, Time.deltaTime * speed);
				yield return null;
			}
		}
		else
		{
			while (this.sword.transform.position != this.swordHit.endPoint)
			{
				this.sword.transform.position = this.sword.transform.position.ZPMoveTowards(this.swordHit.endPoint, Time.deltaTime * speed);
				yield return null;
			}
		}
		base.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
		yield return new WaitForSeconds(2f);
		this.finished = true;
		yield break;
	}

	// Token: 0x040006A7 RID: 1703
	public MeshRenderer swordRenderer;

	// Token: 0x040006A8 RID: 1704
	public GameObject sword;

	// Token: 0x040006A9 RID: 1705
	public AudioClip portalSpawnSound;

	// Token: 0x040006AA RID: 1706
	public AudioClip swordShootSound;

	// Token: 0x040006AB RID: 1707
	public AudioClip hitSound;

	// Token: 0x040006AC RID: 1708
	private LightSwordsItem.SwordHit swordHit;

	// Token: 0x040006AD RID: 1709
	private Material mat;

	// Token: 0x040006AE RID: 1710
	private float heightDif;

	// Token: 0x040006AF RID: 1711
	private bool finished;
}
