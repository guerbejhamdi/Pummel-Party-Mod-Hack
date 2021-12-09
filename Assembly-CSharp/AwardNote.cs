using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200006A RID: 106
public class AwardNote : MonoBehaviour
{
	// Token: 0x060001F5 RID: 501 RVA: 0x00004CEA File Offset: 0x00002EEA
	public void Update()
	{
		if (!this.m_isAlive)
		{
			return;
		}
		if (Time.time > this.m_despawnTime)
		{
			this.Despawn();
		}
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x00004D08 File Offset: 0x00002F08
	private void Despawn()
	{
		this.m_isAlive = false;
		this.m_anim.SetTrigger("Despawn");
		UnityEngine.Object.Destroy(base.gameObject, 2f);
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x00004D31 File Offset: 0x00002F31
	public void Setup(string title, string message, float lifeTime)
	{
		this.m_titleText.text = title;
		this.m_messageText.text = message;
		this.m_despawnTime = Time.time + lifeTime;
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x00004D58 File Offset: 0x00002F58
	public void SetupReward(string rewardGainText)
	{
		this.m_rewardGainText.text = rewardGainText;
		this.m_gobletRewardModel.SetActive(true);
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x00004D72 File Offset: 0x00002F72
	public void DoWinnersEffect(string winners)
	{
		this.m_winnerText.gameObject.SetActive(true);
		this.m_winnerText.text = winners;
		this.m_anim.SetTrigger("Win");
	}

	// Token: 0x060001FA RID: 506 RVA: 0x00034318 File Offset: 0x00032518
	public static AwardNote ShowNote(string title, string message, Transform spawnPoint, float lifeTime)
	{
		if (AwardNote.m_note == null)
		{
			AwardNote.m_note = Resources.Load<GameObject>("Prefabs/AwardNote");
		}
		AwardNote component = UnityEngine.Object.Instantiate<GameObject>(AwardNote.m_note, spawnPoint.position, spawnPoint.rotation).GetComponent<AwardNote>();
		component.Setup(title, message, lifeTime);
		return component;
	}

	// Token: 0x060001FB RID: 507 RVA: 0x00004DA1 File Offset: 0x00002FA1
	public void SetMessage(string text)
	{
		this.m_messageText.text = text;
	}

	// Token: 0x060001FC RID: 508 RVA: 0x00004DAF File Offset: 0x00002FAF
	public void OnWinSoundEffect()
	{
		AudioSystem.PlayOneShot(this.m_winSoundEffect, 1f, 0f, 1f);
	}

	// Token: 0x04000256 RID: 598
	[SerializeField]
	private Text m_titleText;

	// Token: 0x04000257 RID: 599
	[SerializeField]
	private Text m_messageText;

	// Token: 0x04000258 RID: 600
	[SerializeField]
	private Animator m_anim;

	// Token: 0x04000259 RID: 601
	[SerializeField]
	private Text m_rewardGainText;

	// Token: 0x0400025A RID: 602
	[SerializeField]
	private GameObject m_gobletRewardModel;

	// Token: 0x0400025B RID: 603
	[SerializeField]
	private GameObject m_winnersEffects;

	// Token: 0x0400025C RID: 604
	[SerializeField]
	private AudioClip m_winSoundEffect;

	// Token: 0x0400025D RID: 605
	[SerializeField]
	private Text m_winnerText;

	// Token: 0x0400025E RID: 606
	private float m_despawnTime = 1f;

	// Token: 0x0400025F RID: 607
	private bool m_isAlive = true;

	// Token: 0x04000260 RID: 608
	private static GameObject m_note;
}
