using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000394 RID: 916
public class SummoningStaffItem : Item
{
	// Token: 0x060018B0 RID: 6320 RVA: 0x000A8D08 File Offset: 0x000A6F08
	public override void Setup()
	{
		base.Setup();
		this.player.BoardObject.PlayerAnimation.RegisterListener(new AnimationEventListener(this.OnAnimationEvent), AnimationEventType.StaffRaise);
		this.player.BoardObject.PlayerAnimation.RegisterListener(new AnimationEventListener(this.OnAnimationEvent), AnimationEventType.StaffHit);
		this.m_staffHelper = this.heldObject.GetComponentInChildren<SummoningStaffHelper>();
		base.SetNetworkState(Item.ItemState.Setup);
	}

	// Token: 0x060018B1 RID: 6321 RVA: 0x000A8D7C File Offset: 0x000A6F7C
	public void OnDestroy()
	{
		this.player.BoardObject.PlayerAnimation.UnregisterListener(new AnimationEventListener(this.OnAnimationEvent), AnimationEventType.StaffRaise);
		this.player.BoardObject.PlayerAnimation.UnregisterListener(new AnimationEventListener(this.OnAnimationEvent), AnimationEventType.StaffHit);
		if (this.m_summonInProgress)
		{
			AudioSystem.MusicVolume = this.startVolume;
		}
	}

	// Token: 0x060018B2 RID: 6322 RVA: 0x000123FA File Offset: 0x000105FA
	protected override void Use(int seed)
	{
		base.Use(seed);
		this.rand.Next(0, 3);
		base.StartCoroutine(this.BeginSummon());
	}

	// Token: 0x060018B3 RID: 6323 RVA: 0x0001241E File Offset: 0x0001061E
	private IEnumerator BeginSummon()
	{
		yield return null;
		this.cam = GameManager.Board.Camera;
		this.m_sun = RenderSettings.sun;
		if (this.cam != null && this.m_sun != null)
		{
			this.m_summonInProgress = true;
			this.startVolume = AudioSystem.MusicVolume;
			this.player.BoardObject.PlayerAnimation.StartStaffCasting();
			this.m_lightStartColor = this.m_sun.color;
			this.m_lightStartIntensity = this.m_sun.intensity;
			this.m_ambStartSkyColor = RenderSettings.ambientSkyColor;
			this.m_ambStartEquatorColor = RenderSettings.ambientEquatorColor;
			this.m_ambStartGroundColor = RenderSettings.ambientGroundColor;
			this.m_startAmbientIntensity = RenderSettings.ambientIntensity;
			this.m_startReflectionIntensity = RenderSettings.reflectionIntensity;
			AudioSystem.PlayOneShot(this.m_demonicAmbienceClip, 1f, 0f, 1f);
			this.startVolume = AudioSystem.MusicVolume;
			ParticleSystem[] summonParticles = this.m_staffHelper.m_summonParticles;
			for (int i = 0; i < summonParticles.Length; i++)
			{
				summonParticles[i].gameObject.SetActive(true);
			}
			float startTime = Time.time;
			float nextShake = 0f;
			while (Time.time - startTime < this.m_introTime)
			{
				float num = Mathf.Clamp01((Time.time - startTime) / this.m_introTime);
				this.SetLighting(num);
				AudioSystem.MusicVolume = Mathf.Lerp(this.startVolume, 0f, Mathf.Clamp01(num / 0.5f));
				if (Time.time > nextShake)
				{
					nextShake = Time.time + 0.1f;
					this.cam.AddShake(0.1f);
				}
				yield return null;
			}
			yield return new WaitForSeconds(2f);
			this.player.BoardObject.PlayerAnimation.FireStaffCast();
			yield return new WaitForSeconds(2f);
			summonParticles = this.m_staffHelper.m_summonParticles;
			for (int i = 0; i < summonParticles.Length; i++)
			{
				summonParticles[i].Stop();
			}
			yield return new WaitForSeconds(4f);
			startTime = Time.time;
			while (Time.time - startTime < this.m_introTime)
			{
				float num2 = Mathf.Clamp01((Time.time - startTime) / this.m_introTime);
				this.SetLighting(1f - num2);
				AudioSystem.MusicVolume = Mathf.Lerp(0f, this.startVolume, Mathf.Clamp01(num2 / 0.5f));
				yield return null;
			}
			AudioSystem.MusicVolume = this.startVolume;
		}
		else
		{
			Debug.LogError("Error doing summoning staff - camera or light not found");
		}
		yield return this.m_persistentItemRef.Move(0, 0);
		this.player.BoardObject.PlayerAnimation.UnregisterListener(new AnimationEventListener(this.OnAnimationEvent), AnimationEventType.StaffRaise);
		this.player.BoardObject.PlayerAnimation.UnregisterListener(new AnimationEventListener(this.OnAnimationEvent), AnimationEventType.StaffHit);
		base.Finish(false);
		this.m_summonInProgress = false;
		yield break;
	}

	// Token: 0x060018B4 RID: 6324 RVA: 0x000A8DE4 File Offset: 0x000A6FE4
	public void OnAnimationEvent(PlayerAnimationEvent ev)
	{
		AnimationEventType event_type = ev.event_type;
		if (event_type == AnimationEventType.StaffRaise)
		{
			this.OnStaffRaised();
			return;
		}
		if (event_type != AnimationEventType.StaffHit)
		{
			return;
		}
		this.OnStaffHitGround();
	}

	// Token: 0x060018B5 RID: 6325 RVA: 0x0001242D File Offset: 0x0001062D
	public void OnStaffRaised()
	{
		AudioSystem.PlayOneShot(this.m_staffRaiseClip, 1f, 0f, 1f);
	}

	// Token: 0x060018B6 RID: 6326 RVA: 0x000A8E10 File Offset: 0x000A7010
	public void OnStaffHitGround()
	{
		AudioSystem.PlayOneShot(this.m_staffSlamClip, 1f, 0f, 1f);
		if (this.cam != null)
		{
			this.cam.AddShake(4f);
		}
		if (this.m_staffHelper == null)
		{
			this.m_staffHelper = this.heldObject.GetComponentInChildren<SummoningStaffHelper>();
		}
		if (this.m_staffHelper != null)
		{
			Vector3 position = this.m_staffHelper.m_staffAttachPoint.position - this.player.BoardObject.transform.forward * 1f;
			UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(this.m_summoningCircleFX, position, Quaternion.identity), 5f);
			int i = 0;
			while (i < GameManager.Board.PersistentItems.Count)
			{
				PersistentItem persistentItem = GameManager.Board.PersistentItems[i];
				if (persistentItem.GetType() == typeof(SummonItemPersistent))
				{
					GameManager.Board.PersistentItems.RemoveAt(i);
					UnityEngine.Object.Destroy(persistentItem.gameObject);
				}
				else
				{
					i++;
				}
			}
			this.m_persistentItemRef = UnityEngine.Object.Instantiate<GameObject>(this.m_persistentItem, position, Quaternion.Euler(0f, 180f, 0f)).GetComponent<SummonItemPersistent>();
			this.m_persistentItemRef.Initialize(this.player, null);
		}
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x000A8F74 File Offset: 0x000A7174
	private void SetLighting(float t)
	{
		this.m_sun.color = Color.Lerp(this.m_lightStartColor, this.m_lightColor, t);
		this.m_sun.intensity = Mathf.Lerp(this.m_lightStartIntensity, this.m_lightIntensity, t);
		RenderSettings.ambientSkyColor = Color.Lerp(this.m_ambStartSkyColor, this.m_ambientColor, t);
		RenderSettings.ambientEquatorColor = Color.Lerp(this.m_ambStartEquatorColor, this.m_ambientColor, t);
		RenderSettings.ambientGroundColor = Color.Lerp(this.m_ambStartGroundColor, this.m_ambientColor, t);
		RenderSettings.ambientIntensity = Mathf.Lerp(this.m_startAmbientIntensity, this.m_ambientIntensity * this.m_startAmbientIntensity, t);
		RenderSettings.reflectionIntensity = Mathf.Lerp(this.m_startReflectionIntensity, this.m_ambientIntensity * this.m_startAmbientIntensity, t);
	}

	// Token: 0x060018B8 RID: 6328 RVA: 0x000A903C File Offset: 0x000A723C
	public override void Unequip(bool endingTurn)
	{
		GameManager.Board.CameraTrackCurrentPlayer();
		this.player.BoardObject.PlayerAnimation.UnregisterListener(new AnimationEventListener(this.OnAnimationEvent), AnimationEventType.StaffRaise);
		this.player.BoardObject.PlayerAnimation.UnregisterListener(new AnimationEventListener(this.OnAnimationEvent), AnimationEventType.StaffHit);
		base.Unequip(endingTurn);
	}

	// Token: 0x060018B9 RID: 6329 RVA: 0x00010475 File Offset: 0x0000E675
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		return new ItemAIUse(user, 1f);
	}

	// Token: 0x04001A40 RID: 6720
	[SerializeField]
	private GameObject m_persistentItem;

	// Token: 0x04001A41 RID: 6721
	[SerializeField]
	private float m_introTime = 1f;

	// Token: 0x04001A42 RID: 6722
	[SerializeField]
	private Color m_lightColor = Color.white;

	// Token: 0x04001A43 RID: 6723
	[SerializeField]
	private Color m_ambientColor = Color.black;

	// Token: 0x04001A44 RID: 6724
	[SerializeField]
	private float m_ambientIntensity = 0.25f;

	// Token: 0x04001A45 RID: 6725
	[SerializeField]
	private float m_lightIntensity = 0.5f;

	// Token: 0x04001A46 RID: 6726
	[SerializeField]
	private float m_cloudSpeedMultiplier = 8f;

	// Token: 0x04001A47 RID: 6727
	[SerializeField]
	private float m_cloudStrengthMultiplier = 8f;

	// Token: 0x04001A48 RID: 6728
	[Header("Audio")]
	[SerializeField]
	private AudioClip m_staffRaiseClip;

	// Token: 0x04001A49 RID: 6729
	[SerializeField]
	private AudioClip m_staffSlamClip;

	// Token: 0x04001A4A RID: 6730
	[SerializeField]
	private AudioClip m_demonicAmbienceClip;

	// Token: 0x04001A4B RID: 6731
	[Header("Effects")]
	[SerializeField]
	private GameObject m_summoningCircleFX;

	// Token: 0x04001A4C RID: 6732
	private SummoningStaffHelper m_staffHelper;

	// Token: 0x04001A4D RID: 6733
	private SummonItemPersistent m_persistentItemRef;

	// Token: 0x04001A4E RID: 6734
	private Light m_sun;

	// Token: 0x04001A4F RID: 6735
	private GameBoardCamera cam;

	// Token: 0x04001A50 RID: 6736
	private Color m_lightStartColor;

	// Token: 0x04001A51 RID: 6737
	private float m_lightStartIntensity;

	// Token: 0x04001A52 RID: 6738
	private Color m_ambStartSkyColor;

	// Token: 0x04001A53 RID: 6739
	private Color m_ambStartEquatorColor;

	// Token: 0x04001A54 RID: 6740
	private Color m_ambStartGroundColor;

	// Token: 0x04001A55 RID: 6741
	private float m_startAmbientIntensity;

	// Token: 0x04001A56 RID: 6742
	private float m_startReflectionIntensity;

	// Token: 0x04001A57 RID: 6743
	private float startVolume = 1f;

	// Token: 0x04001A58 RID: 6744
	private bool m_summonInProgress;
}
