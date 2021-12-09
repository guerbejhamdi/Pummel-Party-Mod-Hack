using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000216 RID: 534
public class RhythmButtonUI : MonoBehaviour
{
	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000F9C RID: 3996 RVA: 0x0000D5E7 File Offset: 0x0000B7E7
	public int Index
	{
		get
		{
			return this.m_index;
		}
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000F9D RID: 3997 RVA: 0x0000D5EF File Offset: 0x0000B7EF
	public RhythmHit HitInfo
	{
		get
		{
			return this.m_hitInfo;
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06000F9E RID: 3998 RVA: 0x0000D5F7 File Offset: 0x0000B7F7
	public RhythmTrack Track
	{
		get
		{
			return this.m_track;
		}
	}

	// Token: 0x06000F9F RID: 3999 RVA: 0x0000D5FF File Offset: 0x0000B7FF
	public void Awake()
	{
		this.m_transform = (RectTransform)base.transform;
		this.m_anim = base.GetComponent<Animator>();
	}

	// Token: 0x06000FA0 RID: 4000 RVA: 0x0000D61E File Offset: 0x0000B81E
	public void Hit()
	{
		this.m_anim.SetTrigger("Hit");
		UnityEngine.Object.Destroy(base.gameObject, 1f);
	}

	// Token: 0x06000FA1 RID: 4001 RVA: 0x0000D640 File Offset: 0x0000B840
	public void Init(RhythmHit hit, RhythmTrack track, int index)
	{
		this.m_hitInfo = hit;
		this.m_track = track;
		this.m_index = index;
		this.SetupButton();
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x0000D65D File Offset: 0x0000B85D
	public void SetPosition(Vector2 pos)
	{
		((RectTransform)base.transform).anchoredPosition = pos;
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x0007C7C0 File Offset: 0x0007A9C0
	private void SetupButton()
	{
		int button = (int)this.m_hitInfo.button;
		if (this.m_buttonInfo[button].sharePC)
		{
			this.m_buttonImg.sprite = this.m_buttonInfo[button].pc_sprite;
			this.m_glowImg.color = this.m_buttonInfo[button].pc_glowColor;
			return;
		}
		switch (this.m_inputType)
		{
		case RhythmInputType.PC:
			this.m_buttonImg.sprite = this.m_buttonInfo[button].pc_sprite;
			this.m_glowImg.color = this.m_buttonInfo[button].pc_glowColor;
			return;
		case RhythmInputType.Xbox:
			this.m_buttonImg.sprite = this.m_buttonInfo[button].xb_sprite;
			this.m_glowImg.color = this.m_buttonInfo[button].xb_glowColor;
			return;
		case RhythmInputType.Playstation:
			this.m_buttonImg.sprite = this.m_buttonInfo[button].ps_sprite;
			this.m_glowImg.color = this.m_buttonInfo[button].ps_glowColor;
			return;
		default:
			return;
		}
	}

	// Token: 0x06000FA4 RID: 4004 RVA: 0x0000D670 File Offset: 0x0000B870
	public void SetInputType(RhythmInputType inputType)
	{
		this.m_inputType = inputType;
		this.SetupButton();
	}

	// Token: 0x04000FB6 RID: 4022
	[SerializeField]
	private RhythmButtonInfo[] m_buttonInfo;

	// Token: 0x04000FB7 RID: 4023
	[SerializeField]
	private Image m_buttonImg;

	// Token: 0x04000FB8 RID: 4024
	[SerializeField]
	private Image m_glowImg;

	// Token: 0x04000FB9 RID: 4025
	private RectTransform m_transform;

	// Token: 0x04000FBA RID: 4026
	private RhythmInputType m_inputType;

	// Token: 0x04000FBB RID: 4027
	private RhythmHit m_hitInfo;

	// Token: 0x04000FBC RID: 4028
	private RhythmTrack m_track;

	// Token: 0x04000FBD RID: 4029
	private Animator m_anim;

	// Token: 0x04000FBE RID: 4030
	private int m_index;
}
