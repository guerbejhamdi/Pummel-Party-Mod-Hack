using System;
using System.Collections;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000354 RID: 852
public class Reaper : MonoBehaviour
{
	// Token: 0x1700021E RID: 542
	// (get) Token: 0x060016F6 RID: 5878 RVA: 0x000112F2 File Offset: 0x0000F4F2
	// (set) Token: 0x060016F7 RID: 5879 RVA: 0x000112FA File Offset: 0x0000F4FA
	public int CurHitDamage { get; set; }

	// Token: 0x060016F8 RID: 5880 RVA: 0x000A09CC File Offset: 0x0009EBCC
	private void Start()
	{
		this.scytheRenderer.material = new Material(this.scytheRenderer.material);
		this.scytheMaterial = this.scytheRenderer.material;
		this.reaperRenderer.material = new Material(this.reaperRenderer.material);
		this.reaperMaterial = this.reaperRenderer.material;
	}

	// Token: 0x060016F9 RID: 5881 RVA: 0x00011303 File Offset: 0x0000F503
	public IEnumerator DoEvent(BoardPlayer owner, BoardPlayer target, ReaperHitType type, System.Random rand)
	{
		Debug.Log("Doing reaper event");
		this.CurHitDamage = rand.Next(9, 12);
		this.finished = false;
		this.curHitType = type;
		this.rand = rand;
		this.Setup(owner, target);
		this.SetReaperPosition(1.6f, 0f, 45f, true, 0f);
		this.reaperMaterial.SetFloat("_PlayerColorStrength", 0.55f);
		this.scytheMaterial.SetFloat("_PlayerColorStrength", 0.55f);
		this.animator.SetTrigger("Swing");
		AudioSystem.PlayOneShot(this.ghostAppear, this.ghostAppearVol, 0f, 1f);
		base.StartCoroutine(this.Fade(this.scytheMaterial, true, 0.3f));
		base.StartCoroutine(this.Fade(this.reaperMaterial, true, 0.3f));
		base.StartCoroutine(this.FadePosition(true, 0.5f));
		this.animator.SetFloat("Speed", 0.1f);
		yield return new WaitForSeconds(0.425f);
		this.animator.SetFloat("Speed", 3f);
		yield return new WaitForSeconds(0.275f);
		this.animator.SetFloat("Speed", 0.5f);
		yield return new WaitForSeconds(0.125f);
		yield return new WaitUntil(() => this.finished);
		base.transform.position = Vector3.one * 100f;
		yield break;
	}

	// Token: 0x060016FA RID: 5882 RVA: 0x000A0A34 File Offset: 0x0009EC34
	private void SetReaperPosition(float dist, float min, float max, bool doNegative, float rotOffset)
	{
		Quaternion lhs = Quaternion.LookRotation(Vector3.forward);
		if (doNegative)
		{
			bool flag = this.rand.NextDouble() > 0.5;
		}
		float y = ZPMath.RandomFloat(this.rand, min, max) * (doNegative ? 1f : -1f);
		Quaternion rhs = Quaternion.Euler(0f, y, 0f);
		Vector3 a = lhs * rhs * Vector3.forward;
		this.spawnPosition = this.target.transform.position + a * dist;
		base.transform.rotation = Quaternion.LookRotation(this.target.transform.position - this.spawnPosition);
		this.spawnPosition -= Vector3.up * 0.4f;
		base.transform.position = this.spawnPosition;
	}

	// Token: 0x060016FB RID: 5883 RVA: 0x000A0B28 File Offset: 0x0009ED28
	private void Setup(BoardPlayer owner, BoardPlayer target)
	{
		this.target = target;
		this.owner = owner;
		this.reaperMaterial.SetColor("_PlayerColor", owner.GamePlayer.Color.skinColor1);
		this.scytheMaterial.SetColor("_PlayerColor", owner.GamePlayer.Color.skinColor1);
	}

	// Token: 0x060016FC RID: 5884 RVA: 0x0001132F File Offset: 0x0000F52F
	public IEnumerator FadeReaper(bool fadeIn, BoardPlayer player, System.Random rand)
	{
		this.rand = rand;
		if (fadeIn)
		{
			this.Setup(player, player);
			this.SetReaperPosition(1.9f, -55f, -35f, false, -20f);
			this.reaperMaterial.SetFloat("_PlayerColorStrength", 0f);
			this.scytheMaterial.SetFloat("_PlayerColorStrength", 0f);
		}
		AudioSystem.PlayOneShot(this.ghostAppear, this.ghostAppearVol, 0f, 1f);
		base.StartCoroutine(this.Fade(this.scytheMaterial, fadeIn, 0.3f));
		if (fadeIn)
		{
			base.StartCoroutine(this.Fade(this.reaperMaterial, fadeIn, 0.3f));
			yield return base.StartCoroutine(this.FadePosition(true, 0.5f));
		}
		else
		{
			yield return base.StartCoroutine(this.Fade(this.reaperMaterial, fadeIn, 0.3f));
			base.transform.position = Vector3.one * 100f;
		}
		yield break;
	}

	// Token: 0x060016FD RID: 5885 RVA: 0x00011353 File Offset: 0x0000F553
	private IEnumerator Fade(Material m, bool fadeIn, float duration)
	{
		Color startColor = m.color;
		float num = 0.75f;
		float startAlpha = fadeIn ? 0f : num;
		float endAlpha = fadeIn ? num : 0f;
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / duration;
			startColor.a = Mathf.Lerp(startAlpha, endAlpha, t);
			m.color = startColor;
			yield return null;
		}
		if (!fadeIn)
		{
			this.finished = true;
		}
		yield break;
	}

	// Token: 0x060016FE RID: 5886 RVA: 0x00011377 File Offset: 0x0000F577
	private IEnumerator FadePosition(bool fadeIn, float duration)
	{
		float elapsed = 0f;
		Vector3 startPosition = fadeIn ? (this.spawnPosition - base.transform.forward * this.moveDist) : this.spawnPosition;
		Vector3 endPosition = fadeIn ? (this.spawnPosition + base.transform.forward * this.moveDist) : this.spawnPosition;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float time = elapsed / duration;
			if (fadeIn)
			{
				base.transform.position = Vector3.Lerp(startPosition, endPosition, this.movementCurve.Evaluate(time));
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060016FF RID: 5887 RVA: 0x00011394 File Offset: 0x0000F594
	public void SoundPlay()
	{
		AudioSystem.PlayOneShot(this.scytheHit, this.scytheHitVol, 0f, 1f);
	}

	// Token: 0x06001700 RID: 5888 RVA: 0x000A0B84 File Offset: 0x0009ED84
	public void SwingStart()
	{
		this.last = UnityEngine.Object.Instantiate<GameObject>(this.scytheTrailPfb, this.scytheTransform);
		GradientColorKey[] colorKeys = new GradientColorKey[]
		{
			new GradientColorKey(this.owner.GamePlayer.Color.skinColor1, 0f),
			new GradientColorKey(this.owner.GamePlayer.Color.skinColor1, 1f)
		};
		Gradient colorGradient = this.last.GetComponent<TrailRenderer>().colorGradient;
		colorGradient.SetKeys(colorKeys, colorGradient.alphaKeys);
		this.last.GetComponent<TrailRenderer>().colorGradient = colorGradient;
		this.last.transform.localPosition = this.localPosition;
		this.last.transform.localRotation = Quaternion.Euler(this.localRotation);
		if (this.curHitType == ReaperHitType.Gold)
		{
			AudioSystem.PlayOneShot(this.goldHit, this.goldHitVol, 0f, 1f);
		}
	}

	// Token: 0x06001701 RID: 5889 RVA: 0x000A0C80 File Offset: 0x0009EE80
	public void SwingEnd()
	{
		this.last.transform.parent = null;
		this.last.GetComponent<TrailRenderer>().autodestruct = true;
		base.StartCoroutine(this.Fade(this.scytheMaterial, false, 0.2f));
		base.StartCoroutine(this.Fade(this.reaperMaterial, false, 0.2f));
	}

	// Token: 0x06001702 RID: 5890 RVA: 0x000A0CE4 File Offset: 0x0009EEE4
	public void SwingHit()
	{
		if (this.curHitType == ReaperHitType.Health)
		{
			DamageInstance d = new DamageInstance
			{
				damage = this.CurHitDamage,
				origin = this.target.MidPoint + base.transform.right,
				blood = true,
				ragdoll = true,
				ragdollVel = 8f,
				bloodVel = 20f,
				bloodAmount = 1f,
				details = "Reaper",
				removeKeys = true
			};
			this.target.ApplyDamage(d);
			GameManager.Board.boardCamera.AddShake(0.3f);
			return;
		}
		this.target.RemoveGold(6, false, false);
		if (NetSystem.IsServer)
		{
			GameManager.KeyController.SpawnKeys(6, this.target, this.owner);
		}
		UnityEngine.Object.Instantiate<GameObject>(this.goldParticle, this.target.transform.position + Vector3.up * 0.875f, Quaternion.LookRotation(base.transform.forward));
	}

	// Token: 0x0400182D RID: 6189
	[Header("References")]
	public Animator animator;

	// Token: 0x0400182E RID: 6190
	public MeshRenderer scytheRenderer;

	// Token: 0x0400182F RID: 6191
	public SkinnedMeshRenderer reaperRenderer;

	// Token: 0x04001830 RID: 6192
	[Header("Scythe Trail")]
	public GameObject scytheTrailPfb;

	// Token: 0x04001831 RID: 6193
	public Transform scytheTransform;

	// Token: 0x04001832 RID: 6194
	public Vector3 localPosition;

	// Token: 0x04001833 RID: 6195
	public Vector3 localRotation;

	// Token: 0x04001834 RID: 6196
	public Vector3 localScale = new Vector3(100f, 100f, 100f);

	// Token: 0x04001835 RID: 6197
	[Header("SFX")]
	public AudioClip scytheHit;

	// Token: 0x04001836 RID: 6198
	public AudioClip ghostAppear;

	// Token: 0x04001837 RID: 6199
	public AudioClip goldHit;

	// Token: 0x04001838 RID: 6200
	public float scytheHitVol = 1f;

	// Token: 0x04001839 RID: 6201
	public float ghostAppearVol = 1f;

	// Token: 0x0400183A RID: 6202
	public float goldHitVol = 1f;

	// Token: 0x0400183B RID: 6203
	[Header("VFX")]
	public GameObject bloodParticle;

	// Token: 0x0400183C RID: 6204
	public GameObject goldParticle;

	// Token: 0x0400183D RID: 6205
	[Header("Movement")]
	public AnimationCurve movementCurve;

	// Token: 0x0400183E RID: 6206
	public float moveDist = 0.5f;

	// Token: 0x0400183F RID: 6207
	private BoardPlayer target;

	// Token: 0x04001840 RID: 6208
	private BoardPlayer owner;

	// Token: 0x04001841 RID: 6209
	private Material scytheMaterial;

	// Token: 0x04001842 RID: 6210
	private Material reaperMaterial;

	// Token: 0x04001843 RID: 6211
	private GameObject last;

	// Token: 0x04001844 RID: 6212
	private ReaperHitType curHitType;

	// Token: 0x04001845 RID: 6213
	private bool finished;

	// Token: 0x04001846 RID: 6214
	private Vector3 spawnPosition;

	// Token: 0x04001847 RID: 6215
	private System.Random rand;
}
