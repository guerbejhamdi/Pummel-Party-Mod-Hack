using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000360 RID: 864
public class BeeHive : MonoBehaviour
{
	// Token: 0x0600173E RID: 5950 RVA: 0x0001156A File Offset: 0x0000F76A
	private void Start()
	{
		this.m_beeBuzzSource.volume = AudioSystem.GetVolume(SoundType.Effect, this.m_buzzVolume);
	}

	// Token: 0x0600173F RID: 5951 RVA: 0x000A190C File Offset: 0x0009FB0C
	public void Update()
	{
		if (this.m_broken && this.m_targetPlayer != null && !this.m_reached)
		{
			this.m_beeHive.transform.position = Vector3.MoveTowards(this.m_beeHive.transform.position, this.m_targetHeadBone.position, Time.deltaTime * this.m_yVelocity);
			this.m_yVelocity += 9.8f * Time.deltaTime * 2f;
			if (!this.m_playedSound && (this.m_beeHive.transform.position - this.m_targetHeadBone.position).sqrMagnitude < 0.85f)
			{
				AudioSystem.PlayOneShot(this.m_suction, 0.9f, 0f, 1f);
				this.m_playedSound = true;
			}
			if (this.m_beeHive.transform.position == this.m_targetHeadBone.position)
			{
				this.m_targetPlayer.PlayerAnimation.Animator.SetBool("BeeHiveHead", true);
				this.m_beeHive.transform.parent = this.m_targetHeadBone;
				this.m_beeHive.transform.localPosition = new Vector3(0f, 0.16f, 0.065f);
				this.m_beeHive.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
				this.m_beeHive.GetComponent<SphereCollider>().enabled = false;
				this.m_reached = true;
			}
		}
	}

	// Token: 0x06001740 RID: 5952 RVA: 0x000A1AA8 File Offset: 0x0009FCA8
	public void BreakBranch(short targetPlayer)
	{
		if (this.m_broken)
		{
			return;
		}
		this.m_broken = true;
		this.m_branch.SetActive(false);
		for (int i = 0; i < this.m_branchGibs.Length; i++)
		{
			this.m_branchGibs[i].transform.parent = null;
			this.m_branchGibs[i].SetActive(true);
			Rigidbody component = this.m_branchGibs[i].GetComponent<Rigidbody>();
			component.AddRelativeTorque(UnityEngine.Random.onUnitSphere * 10f);
			component.AddForce(UnityEngine.Random.onUnitSphere * 10f);
			UnityEngine.Object.Destroy(this.m_branchGibs[i], 4f);
		}
		if (targetPlayer == -1)
		{
			Rigidbody rigidbody = this.m_beeHive.AddComponent<Rigidbody>();
			rigidbody.AddRelativeTorque(UnityEngine.Random.onUnitSphere);
			rigidbody.AddForce(UnityEngine.Random.onUnitSphere * 10f);
		}
		else
		{
			this.m_targetPlayer = GameManager.GetPlayerAt((int)targetPlayer).BoardObject;
			this.m_targetHeadBone = this.m_targetPlayer.PlayerAnimation.GetBone(PlayerBone.Head);
		}
		this.m_beeHive.transform.parent = null;
		AudioSystem.PlayOneShot(this.m_branchBreak, 1f, 0f, 1f);
	}

	// Token: 0x06001741 RID: 5953 RVA: 0x00011583 File Offset: 0x0000F783
	public void SetBeesAngry(Transform target)
	{
		if (this.m_angry)
		{
			return;
		}
		this.m_angry = true;
		this.m_attackBees.Target = target;
		this.m_attackBees.gameObject.SetActive(true);
		base.StartCoroutine(this.UpdateBuzzAudioSource());
	}

	// Token: 0x06001742 RID: 5954 RVA: 0x000115BF File Offset: 0x0000F7BF
	private IEnumerator UpdateBuzzAudioSource()
	{
		float startTime = Time.time;
		float lerpTime = 0.5f;
		float startVolume = AudioSystem.GetVolume(SoundType.Effect, this.m_buzzVolume);
		float startPitch = this.m_beeBuzzSource.pitch;
		while (Time.time - startTime <= lerpTime)
		{
			float t = (Time.time - startTime) / lerpTime;
			float volume = AudioSystem.GetVolume(SoundType.Effect, this.m_angryVolume);
			this.m_beeBuzzSource.volume = Mathf.Lerp(startVolume, volume, t);
			this.m_beeBuzzSource.pitch = Mathf.Lerp(startPitch, this.m_angryPitch, t);
			yield return null;
		}
		this.m_beeBuzzSource.volume = AudioSystem.GetVolume(SoundType.Effect, this.m_angryVolume);
		this.m_beeBuzzSource.pitch = this.m_angryPitch;
		yield break;
		yield break;
	}

	// Token: 0x06001743 RID: 5955 RVA: 0x000115CE File Offset: 0x0000F7CE
	public void OnDestroy()
	{
		if (this.m_targetPlayer == null)
		{
			UnityEngine.Object.Destroy(this.m_beeHive);
		}
		if (this.m_idleBees != null)
		{
			this.m_idleBees.GetComponentInChildren<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmitting);
		}
	}

	// Token: 0x04001884 RID: 6276
	[SerializeField]
	protected GameObject m_beeHive;

	// Token: 0x04001885 RID: 6277
	[SerializeField]
	protected GameObject m_branch;

	// Token: 0x04001886 RID: 6278
	[SerializeField]
	protected GameObject[] m_branchGibs;

	// Token: 0x04001887 RID: 6279
	[SerializeField]
	protected AudioClip m_branchBreak;

	// Token: 0x04001888 RID: 6280
	[SerializeField]
	protected AudioSource m_beeBuzzSource;

	// Token: 0x04001889 RID: 6281
	[SerializeField]
	protected float m_buzzVolume = 0.3f;

	// Token: 0x0400188A RID: 6282
	[SerializeField]
	protected float m_angryPitch;

	// Token: 0x0400188B RID: 6283
	[SerializeField]
	protected float m_angryVolume;

	// Token: 0x0400188C RID: 6284
	[SerializeField]
	protected GameObject m_idleBees;

	// Token: 0x0400188D RID: 6285
	[SerializeField]
	protected AudioClip m_suction;

	// Token: 0x0400188E RID: 6286
	[SerializeField]
	protected FX_Bees m_attackBees;

	// Token: 0x0400188F RID: 6287
	private bool m_broken;

	// Token: 0x04001890 RID: 6288
	private bool m_angry;

	// Token: 0x04001891 RID: 6289
	private BoardPlayer m_targetPlayer;

	// Token: 0x04001892 RID: 6290
	private Transform m_targetHeadBone;

	// Token: 0x04001893 RID: 6291
	private float m_yVelocity;

	// Token: 0x04001894 RID: 6292
	private bool m_reached;

	// Token: 0x04001895 RID: 6293
	private bool m_playedSound;
}
