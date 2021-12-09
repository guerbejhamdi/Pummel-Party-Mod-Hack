using System;
using UnityEngine;

// Token: 0x020004D0 RID: 1232
public class PortalEffect : MonoBehaviour
{
	// Token: 0x060020A6 RID: 8358 RVA: 0x00017BFE File Offset: 0x00015DFE
	public void Awake()
	{
		this.m_startTime = Time.time;
	}

	// Token: 0x060020A7 RID: 8359 RVA: 0x00017C0B File Offset: 0x00015E0B
	public void Update()
	{
		if (Time.time - this.m_startTime > 10f && !this.m_released)
		{
			this.Release(true);
		}
	}

	// Token: 0x060020A8 RID: 8360 RVA: 0x000CC924 File Offset: 0x000CAB24
	public void Init(PortalOrientation orientantion, Material targetClipMat)
	{
		this.m_clipSource = base.GetComponent<ClipShapeSource>();
		this.m_anim = base.GetComponent<Animator>();
		this.m_particles = base.GetComponentsInChildren<ParticleSystem>();
		this.m_clipSource.SetMaterial(targetClipMat);
		AudioSystem.PlayOneShot(this.m_portalOpen, 2f, 0f, 1f);
		this.m_portalPlaneRenderer.material.SetFloat("_TimeOffset", UnityEngine.Random.value);
	}

	// Token: 0x060020A9 RID: 8361 RVA: 0x000CC998 File Offset: 0x000CAB98
	public void Init(PortalOrientation orientantion, GameObject clipTarget, bool playSound = true)
	{
		this.m_clipSource = base.GetComponent<ClipShapeSource>();
		this.m_anim = base.GetComponent<Animator>();
		this.m_particles = base.GetComponentsInChildren<ParticleSystem>();
		if (clipTarget != null)
		{
			this.m_clipSource.SetTarget(clipTarget);
		}
		if (playSound)
		{
			AudioSystem.PlayOneShot(this.m_portalOpen, 2f, 0f, 1f);
		}
		this.m_portalPlaneRenderer.material.SetFloat("_TimeOffset", UnityEngine.Random.value);
	}

	// Token: 0x060020AA RID: 8362 RVA: 0x000CCA18 File Offset: 0x000CAC18
	public void Release(bool playSound = true)
	{
		if (this.m_released || this.m_anim == null)
		{
			this.m_released = true;
			return;
		}
		this.m_released = true;
		this.m_anim.SetTrigger("Destroy");
		ParticleSystem[] particles = this.m_particles;
		for (int i = 0; i < particles.Length; i++)
		{
			particles[i].Stop();
		}
		if (playSound)
		{
			AudioSystem.PlayOneShot(this.m_portalClose, 2f, 0f, 1f);
		}
		UnityEngine.Object.Destroy(base.gameObject, 2f);
	}

	// Token: 0x060020AB RID: 8363 RVA: 0x00017C2F File Offset: 0x00015E2F
	public static PortalEffect Spawn(Vector3 position, Vector3 direction, PortalOrientation orientation, Material targetClipMat)
	{
		if (PortalEffect.m_portalPrefab == null)
		{
			PortalEffect.m_portalPrefab = Resources.Load<GameObject>("FX_Portal");
		}
		PortalEffect component = UnityEngine.Object.Instantiate<GameObject>(PortalEffect.m_portalPrefab, position, Quaternion.LookRotation(direction)).GetComponent<PortalEffect>();
		component.Init(orientation, targetClipMat);
		return component;
	}

	// Token: 0x060020AC RID: 8364 RVA: 0x00017C6B File Offset: 0x00015E6B
	public static PortalEffect Spawn(Vector3 position, Vector3 direction, PortalOrientation orientation, GameObject clipTarget, bool playSound = true)
	{
		if (PortalEffect.m_portalPrefab == null)
		{
			PortalEffect.m_portalPrefab = Resources.Load<GameObject>("FX_Portal");
		}
		PortalEffect component = UnityEngine.Object.Instantiate<GameObject>(PortalEffect.m_portalPrefab, position, Quaternion.LookRotation(direction)).GetComponent<PortalEffect>();
		component.Init(orientation, clipTarget, playSound);
		return component;
	}

	// Token: 0x04002370 RID: 9072
	[SerializeField]
	protected AudioClip m_portalOpen;

	// Token: 0x04002371 RID: 9073
	[SerializeField]
	protected AudioClip m_portalClose;

	// Token: 0x04002372 RID: 9074
	[SerializeField]
	protected MeshRenderer m_portalPlaneRenderer;

	// Token: 0x04002373 RID: 9075
	private PortalOrientation m_orientation;

	// Token: 0x04002374 RID: 9076
	private ClipShapeSource m_clipSource;

	// Token: 0x04002375 RID: 9077
	private Animator m_anim;

	// Token: 0x04002376 RID: 9078
	private ParticleSystem[] m_particles;

	// Token: 0x04002377 RID: 9079
	private float m_startTime;

	// Token: 0x04002378 RID: 9080
	private bool m_released;

	// Token: 0x04002379 RID: 9081
	private static GameObject m_portalPrefab;
}
