using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class ExplosiveTrap : BoardNodeEvent
{
	// Token: 0x060001A0 RID: 416 RVA: 0x00032BB0 File Offset: 0x00030DB0
	public void Awake()
	{
		this.startScales = new Vector3[this.renderers.Length];
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.startScales[i] = this.renderers[i].transform.localScale;
		}
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x000049ED File Offset: 0x00002BED
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode node, int seed)
	{
		float explodeTime = 0.5f;
		float startTime = Time.time;
		while (Time.time - startTime < explodeTime)
		{
			float num = (Time.time - startTime) / explodeTime;
			num = Easing.ElasticEaseIn(num);
			for (int i = 0; i < this.renderers.Length; i++)
			{
				this.renderers[i].transform.localScale = this.startScales[i] + this.startScales[i] * 0.25f * num;
			}
			yield return null;
		}
		this.rand = new System.Random(seed);
		this.SetState(false);
		UnityEngine.Object.Instantiate<GameObject>(this.explodeParticle, base.transform.position, Quaternion.identity);
		if (this.spawnedScorch == null && this.scorchMarkDecal != null)
		{
			this.spawnedScorch = UnityEngine.Object.Instantiate<GameObject>(this.scorchMarkDecal, base.transform.position, Quaternion.Euler(new Vector3(90f, 0f, 0f)));
			if (GameManager.BoardRoot != null)
			{
				this.spawnedScorch.transform.parent = GameManager.BoardRoot.transform;
			}
		}
		AudioSystem.PlayOneShot(this.explodeSound, 0.5f, 0f, 1f);
		DamageInstance d = new DamageInstance
		{
			damage = this.rand.Next(7, 9),
			origin = base.transform.position,
			blood = true,
			ragdoll = true,
			ragdollVel = 15f,
			bloodVel = 20f,
			bloodAmount = 1f,
			details = "Explosive Trap",
			removeKeys = true
		};
		if (player != null)
		{
			player.ApplyDamage(d);
		}
		GameManager.Board.boardCamera.AddShake(0.6f);
		yield return new WaitForSeconds(2f);
		base.StartCoroutine(this.Reset());
		yield break;
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x00004A0A File Offset: 0x00002C0A
	private IEnumerator Reset()
	{
		yield return new WaitForSeconds(1.4f);
		if (!this.curState)
		{
			for (int i = 0; i < this.renderers.Length; i++)
			{
				this.renderers[i].enabled = true;
			}
			float respawnTime = 0.5f;
			float startTime = Time.time;
			while (Time.time - startTime < respawnTime)
			{
				float num = (Time.time - startTime) / respawnTime;
				num = Easing.ElasticEaseOut(num);
				for (int j = 0; j < this.renderers.Length; j++)
				{
					this.renderers[j].transform.localScale = this.startScales[j] * num;
				}
				yield return null;
			}
		}
		this.SetState(true);
		yield break;
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x00004A19 File Offset: 0x00002C19
	private void OnDisable()
	{
		base.StopAllCoroutines();
		this.SetState(true);
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x00032C04 File Offset: 0x00030E04
	private void SetState(bool state)
	{
		this.curState = state;
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = state;
			if (state)
			{
				this.renderers[i].transform.localScale = this.startScales[i];
			}
			else
			{
				this.renderers[i].transform.localScale = Vector3.zero;
			}
		}
	}

	// Token: 0x040001F5 RID: 501
	public AudioClip explodeSound;

	// Token: 0x040001F6 RID: 502
	public GameObject explodeParticle;

	// Token: 0x040001F7 RID: 503
	public GameObject scorchMarkDecal;

	// Token: 0x040001F8 RID: 504
	public MeshRenderer[] renderers;

	// Token: 0x040001F9 RID: 505
	private Vector3[] startScales;

	// Token: 0x040001FA RID: 506
	private GameObject spawnedScorch;

	// Token: 0x040001FB RID: 507
	private bool curState = true;
}
