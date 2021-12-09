using System;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class GobletChallengeAwards : MonoBehaviour
{
	// Token: 0x0600027F RID: 639 RVA: 0x0000525C File Offset: 0x0000345C
	public Transform GetNoteSpawn()
	{
		return this.m_noteSpawn;
	}

	// Token: 0x06000280 RID: 640 RVA: 0x00005264 File Offset: 0x00003464
	public Transform GetSpawnPoint(int totalPlayers, int playerIdx)
	{
		return this.m_spawnGroups[totalPlayers].m_spawns[playerIdx];
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00005275 File Offset: 0x00003475
	public void Awake()
	{
		this.m_anim = base.GetComponent<Animator>();
	}

	// Token: 0x06000282 RID: 642 RVA: 0x00005283 File Offset: 0x00003483
	private void Update()
	{
		RenderSettings.fogDensity = this.m_fogDensity;
	}

	// Token: 0x06000283 RID: 643 RVA: 0x00005290 File Offset: 0x00003490
	public void ShowIntro()
	{
		AudioSystem.PlayMusic(this.m_awardsShowMusic, 0.1f, 1f);
		this.m_anim.SetTrigger("Start");
	}

	// Token: 0x06000284 RID: 644 RVA: 0x000052B7 File Offset: 0x000034B7
	public void ShowOutro()
	{
		this.m_anim.SetTrigger("End");
		AudioSystem.PlayMusic(null, 1f, 1f);
	}

	// Token: 0x06000285 RID: 645 RVA: 0x000052D9 File Offset: 0x000034D9
	public void FinalWinner()
	{
		this.m_anim.SetTrigger("FinalWinner");
	}

	// Token: 0x06000286 RID: 646 RVA: 0x0000398C File Offset: 0x00001B8C
	public void OnAwardsIntro()
	{
	}

	// Token: 0x040002C9 RID: 713
	[SerializeField]
	private float m_fogDensity = 0.045f;

	// Token: 0x040002CA RID: 714
	[SerializeField]
	private Transform m_noteSpawn;

	// Token: 0x040002CB RID: 715
	[SerializeField]
	private AudioClip m_awardsShowMusic;

	// Token: 0x040002CC RID: 716
	[SerializeField]
	private AudioSource m_music;

	// Token: 0x040002CD RID: 717
	[SerializeField]
	private SpawnGroups[] m_spawnGroups;

	// Token: 0x040002CE RID: 718
	[SerializeField]
	private AudioClip m_awardsIntroMetalEffect;

	// Token: 0x040002CF RID: 719
	private Animator m_anim;
}
