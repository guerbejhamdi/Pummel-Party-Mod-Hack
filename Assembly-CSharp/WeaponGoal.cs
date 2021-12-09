using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000472 RID: 1138
public class WeaponGoal : MonoBehaviour
{
	// Token: 0x06001EC3 RID: 7875 RVA: 0x00016B72 File Offset: 0x00014D72
	public void OnEnable()
	{
		this.m_anim.SetFloat("SpawnSpeed", 10f);
		this.m_anim.SetBool("Visible", this.m_visible);
	}

	// Token: 0x06001EC4 RID: 7876 RVA: 0x00016B9F File Offset: 0x00014D9F
	public void Open()
	{
		AudioSystem.PlayOneShot(this.m_doorOpenClip, 1f, 0f, 1f);
		this.m_anim.SetTrigger("Open");
	}

	// Token: 0x06001EC5 RID: 7877 RVA: 0x00016BCB File Offset: 0x00014DCB
	public void Spawn(byte weaponID)
	{
		this.itemID = weaponID;
		base.StartCoroutine(this.DoWeaponSpaceSpawn());
	}

	// Token: 0x06001EC6 RID: 7878 RVA: 0x00016BE1 File Offset: 0x00014DE1
	private IEnumerator DoWeaponSpaceSpawn()
	{
		this.m_anim.SetFloat("SpawnSpeed", 1f);
		this.m_anim.SetBool("Visible", true);
		TempAudioSource audioSource = AudioSystem.PlayLooping(this.m_rumbleSound, 0.25f, 1f);
		float m_spawnTime = 4.5f;
		float m_startTime = Time.time;
		while (Time.time - m_startTime < m_spawnTime)
		{
			float num = Time.time - m_startTime;
			if (audioSource != null && num > m_spawnTime - 1f)
			{
				audioSource.FadeAudio(1f, FadeType.Out);
				audioSource = null;
			}
			yield return new WaitForSeconds(0.25f);
		}
		this.m_visible = true;
		yield break;
	}

	// Token: 0x06001EC7 RID: 7879 RVA: 0x00016BF0 File Offset: 0x00014DF0
	public void Despawn()
	{
		base.StartCoroutine(this.DoWeaponSpaceDespawn());
	}

	// Token: 0x06001EC8 RID: 7880 RVA: 0x00016BFF File Offset: 0x00014DFF
	private IEnumerator DoWeaponSpaceDespawn()
	{
		this.m_visible = false;
		this.m_anim.SetFloat("SpawnSpeed", 1f);
		this.m_anim.SetBool("Visible", false);
		TempAudioSource audioSource = AudioSystem.PlayLooping(this.m_rumbleSound, 0.25f, 1f);
		float m_spawnTime = 4.5f;
		float m_startTime = Time.time;
		while (Time.time - m_startTime < m_spawnTime)
		{
			float num = Time.time - m_startTime;
			if (audioSource != null && num > m_spawnTime - 1f)
			{
				audioSource.FadeAudio(1f, FadeType.Out);
				audioSource = null;
			}
			yield return new WaitForSeconds(0.25f);
		}
		yield break;
	}

	// Token: 0x040021C4 RID: 8644
	[SerializeField]
	private AudioClip m_rumbleSound;

	// Token: 0x040021C5 RID: 8645
	[SerializeField]
	private Animator m_anim;

	// Token: 0x040021C6 RID: 8646
	[SerializeField]
	private AudioClip m_doorOpenClip;

	// Token: 0x040021C7 RID: 8647
	public byte itemID;

	// Token: 0x040021C8 RID: 8648
	private bool m_visible;
}
