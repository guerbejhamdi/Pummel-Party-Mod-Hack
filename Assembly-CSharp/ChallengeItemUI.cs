using System;
using UnityEngine;

// Token: 0x02000319 RID: 793
public class ChallengeItemUI : MonoBehaviour
{
	// Token: 0x060015BC RID: 5564 RVA: 0x0001072F File Offset: 0x0000E92F
	public void Despawn()
	{
		this.m_despawning = true;
		this.m_anim.SetTrigger("Despawn");
	}

	// Token: 0x060015BD RID: 5565 RVA: 0x0009C61C File Offset: 0x0009A81C
	public void SetPlayer(int index, GamePlayer player)
	{
		if (this.m_despawning)
		{
			return;
		}
		ChallengeItemPlayerUI challengeItemPlayerUI = this.m_playerUI[index];
		Color uiColor = player.Color.uiColor;
		challengeItemPlayerUI.border.color = uiColor;
		challengeItemPlayerUI.nameBorder.color = uiColor;
		challengeItemPlayerUI.speedLines.color = player.Color.skinColor1;
		challengeItemPlayerUI.nameText.text = player.Name;
		challengeItemPlayerUI.nameText.color = uiColor;
		this.m_anim.SetTrigger("Spawn");
		challengeItemPlayerUI.face.uvRect = this.portraitUVs[(int)player.GlobalID];
		if (player.GlobalID >= 4)
		{
			challengeItemPlayerUI.face.texture = this.m_rt2;
		}
		AudioSystem.PlayOneShot(this.m_spawnAudio, 1f, 0f, 1f);
	}

	// Token: 0x040016DE RID: 5854
	[SerializeField]
	private ChallengeItemPlayerUI[] m_playerUI;

	// Token: 0x040016DF RID: 5855
	[SerializeField]
	private Animator m_anim;

	// Token: 0x040016E0 RID: 5856
	[SerializeField]
	private AudioClip m_spawnAudio;

	// Token: 0x040016E1 RID: 5857
	[SerializeField]
	private RenderTexture m_rt1;

	// Token: 0x040016E2 RID: 5858
	[SerializeField]
	private RenderTexture m_rt2;

	// Token: 0x040016E3 RID: 5859
	private bool m_despawning;

	// Token: 0x040016E4 RID: 5860
	private Rect[] portraitUVs = new Rect[]
	{
		new Rect(0f, 0f, 0.25f, 1f),
		new Rect(0.25f, 0f, 0.25f, 1f),
		new Rect(0.5f, 0f, 0.25f, 1f),
		new Rect(0.75f, 0f, 0.25f, 1f),
		new Rect(0f, 0f, 0.25f, 1f),
		new Rect(0.25f, 0f, 0.25f, 1f),
		new Rect(0.5f, 0f, 0.25f, 1f),
		new Rect(0.75f, 0f, 0.25f, 1f)
	};
}
