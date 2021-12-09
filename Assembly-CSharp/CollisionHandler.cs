using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class CollisionHandler : MonoBehaviour
{
	// Token: 0x060000E9 RID: 233 RVA: 0x000042E8 File Offset: 0x000024E8
	public void Update()
	{
		if (!this.destroying && base.transform.position.x > -7f)
		{
			this.StartDestroy();
		}
	}

	// Token: 0x060000EA RID: 234 RVA: 0x0000430F File Offset: 0x0000250F
	public void StartDestroy()
	{
		this.destroying = true;
		UnityEngine.Object.Destroy(base.gameObject, 0.1f);
		base.StartCoroutine(this.Kill());
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00004335 File Offset: 0x00002535
	private IEnumerator Kill()
	{
		float startVol = this.baseVol;
		while (base.transform.localScale.x > 0.05f)
		{
			base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, new Vector3(0.05f, 0.05f, 0.05f), Time.deltaTime * 10f);
			this.baseVol = Mathf.MoveTowards(this.baseVol, 0f, startVol * Time.deltaTime * 10f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000EC RID: 236 RVA: 0x0002FB0C File Offset: 0x0002DD0C
	private void FixedUpdate()
	{
		float volume = AudioSystem.GetVolume(SoundType.Effect, this.baseVol);
		if (this.colliding)
		{
			this.drift.volume = Mathf.MoveTowards(this.drift.volume, volume, Time.deltaTime * volume * 5f);
			if (!this.replaying)
			{
				this.colliding = false;
				return;
			}
		}
		else
		{
			this.drift.volume = Mathf.MoveTowards(this.drift.volume, 0f, Time.deltaTime * volume * 5f);
		}
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00004344 File Offset: 0x00002544
	private void OnCollisionEnter(Collision collision)
	{
		bool flag = this.replaying;
	}

	// Token: 0x060000EE RID: 238 RVA: 0x0000434D File Offset: 0x0000254D
	private void OnCollisionStay(Collision collision)
	{
		if (this.replaying)
		{
			return;
		}
		this.colliding = true;
	}

	// Token: 0x04000141 RID: 321
	public AudioSource drift;

	// Token: 0x04000142 RID: 322
	public SidestepSlopeController cont;

	// Token: 0x04000143 RID: 323
	public bool colliding;

	// Token: 0x04000144 RID: 324
	public bool replaying;

	// Token: 0x04000145 RID: 325
	private float minHitInterval = 0.25f;

	// Token: 0x04000146 RID: 326
	public float lastHit;

	// Token: 0x04000147 RID: 327
	private float baseVol = 0.25f;

	// Token: 0x04000148 RID: 328
	private bool destroying;
}
