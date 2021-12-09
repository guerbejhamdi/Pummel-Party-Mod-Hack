using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000443 RID: 1091
public class SamuraiSwordEvent : BoardNodeEvent
{
	// Token: 0x06001E18 RID: 7704 RVA: 0x00016380 File Offset: 0x00014580
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode boardNode, int seed)
	{
		Debug.Log("Samurai Event");
		this.rand = new System.Random(seed);
		this.swordRoot.transform.position = player.transform.position - Vector3.forward * this.swordForwardPosition + new Vector3(-0.2f, 1f, 0f);
		this.swordRoot.transform.rotation = Quaternion.LookRotation(Vector3.forward) * Quaternion.Euler(0f, 90f, 0f);
		this.startRotation = this.swordRoot.transform.rotation;
		float startTime = Time.time;
		AudioSystem.PlayOneShot(this.swordAppear, this.swordAppearVol, 0f, 1f);
		while (Time.time - startTime < this.fadeInTime)
		{
			float t = (Time.time - startTime) / this.fadeInTime;
			this.FadeIn(t);
			yield return null;
		}
		this.FadeIn(1f);
		startTime = Time.time;
		Vector3 startPos = this.swordRoot.transform.position;
		Vector3 endPos = startPos + new Vector3(0f, 0f, this.stabOffset);
		Vector3 dir = (endPos - startPos).normalized;
		bool hasDoneDamage = false;
		bool hasDoneSFX = false;
		while (Time.time - startTime < this.stabTime)
		{
			float num = (Time.time - startTime) / this.stabTime;
			this.swordRoot.transform.position = startPos + dir * this.stabCurve.Evaluate(num);
			if (!hasDoneSFX && num > 0.45f)
			{
				AudioSystem.PlayOneShot(this.stabClip, this.stabVolume, 0f, 1f);
				hasDoneSFX = true;
			}
			if (!hasDoneDamage && num > 0.85f)
			{
				hasDoneDamage = true;
				DamageInstance d = new DamageInstance
				{
					damage = this.rand.Next(7, 9),
					origin = player.transform.position + Vector3.up - Vector3.forward,
					blood = true,
					ragdoll = true,
					ragdollVel = 0f,
					bloodVel = 7f,
					bloodAmount = 1.5f,
					details = "Samurai Sword",
					removeKeys = true,
					sound = true,
					volume = 1f
				};
				bool flag = player.CactusScript != null || player.PresentScript != null;
				player.ApplyDamage(d);
				GameManager.Board.boardCamera.AddShake(0.15f);
				if (!flag)
				{
					this.attachedRagdoll = player.NewestRagdoll.transform.Find("Character/Armature");
				}
			}
			yield return null;
		}
		this.swordRoot.transform.position = endPos;
		startTime = Time.time;
		Quaternion raiseRotation = Quaternion.LookRotation(Vector3.up);
		while (Time.time - startTime < this.raiseTime)
		{
			float num2 = (Time.time - startTime) / this.raiseTime;
			this.swordRoot.transform.rotation = Quaternion.RotateTowards(this.swordRoot.transform.rotation, raiseRotation, Time.deltaTime * this.rotSpeed);
			this.swordRoot.transform.position += this.swordRoot.transform.forward * Time.deltaTime * this.raiseSpeed;
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		if (this.attachedRagdoll != null)
		{
			Rigidbody[] componentsInChildren = this.attachedRagdoll.GetComponentsInChildren<Rigidbody>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].velocity = Vector3.zero;
				componentsInChildren[i].angularVelocity = Vector3.zero;
			}
			this.attachedRagdoll = null;
		}
		yield return new WaitForSeconds(0.5f);
		startTime = Time.time;
		AudioSystem.PlayOneShot(this.swordAppear, this.swordAppearVol, 0f, 1f);
		while (Time.time - startTime < this.fadeInTime)
		{
			float t2 = (Time.time - startTime) / this.fadeInTime;
			this.FadeOut(t2);
			yield return null;
		}
		this.FadeOut(1f);
		this.swordRoot.transform.position = Vector3.one * 1000f;
		yield return new WaitForSeconds(1f);
		yield break;
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x000C3038 File Offset: 0x000C1238
	private void Update()
	{
		if (this.attachedRagdoll != null)
		{
			this.attachedRagdoll.transform.position = this.attachPoint.position;
			this.attachedRagdoll.transform.rotation = this.attachPoint.rotation;
		}
	}

	// Token: 0x06001E1A RID: 7706 RVA: 0x000C308C File Offset: 0x000C128C
	private void FadeIn(float t)
	{
		this.swordMaterial.material.SetFloat("_DissolveAmount", Mathf.Lerp(0.27f, 0.92f, t));
		this.swordRoot.transform.rotation = Quaternion.RotateTowards(this.startRotation, this.startRotation * Quaternion.Euler(0f, -90f, 0f), 90f * this.rotateAnim.Evaluate(t));
	}

	// Token: 0x06001E1B RID: 7707 RVA: 0x0001639D File Offset: 0x0001459D
	private void FadeOut(float t)
	{
		this.swordMaterial.sharedMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(0.92f, 0.27f, t));
	}

	// Token: 0x040020DD RID: 8413
	public GameObject swordRoot;

	// Token: 0x040020DE RID: 8414
	public float swordForwardPosition = 3f;

	// Token: 0x040020DF RID: 8415
	public Transform attachPoint;

	// Token: 0x040020E0 RID: 8416
	[Header("Fade In")]
	public float fadeInTime = 1f;

	// Token: 0x040020E1 RID: 8417
	public MeshRenderer swordMaterial;

	// Token: 0x040020E2 RID: 8418
	public AnimationCurve rotateAnim;

	// Token: 0x040020E3 RID: 8419
	public AudioClip swordAppear;

	// Token: 0x040020E4 RID: 8420
	public float swordAppearVol;

	// Token: 0x040020E5 RID: 8421
	[Header("Stab")]
	public float stabTime = 0.25f;

	// Token: 0x040020E6 RID: 8422
	public float stabOffset = 1.25f;

	// Token: 0x040020E7 RID: 8423
	public AnimationCurve stabCurve;

	// Token: 0x040020E8 RID: 8424
	public AudioClip stabClip;

	// Token: 0x040020E9 RID: 8425
	public float stabVolume;

	// Token: 0x040020EA RID: 8426
	[Header("Raise")]
	public float raiseTime = 2f;

	// Token: 0x040020EB RID: 8427
	public float rotSpeed = 90f;

	// Token: 0x040020EC RID: 8428
	public float raiseSpeed = 2.5f;

	// Token: 0x040020ED RID: 8429
	public AnimationCurve heightCurve;

	// Token: 0x040020EE RID: 8430
	private Quaternion startRotation;

	// Token: 0x040020EF RID: 8431
	private Transform attachedRagdoll;
}
