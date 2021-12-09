using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000563 RID: 1379
public class UIPlayerScoreNew : MonoBehaviour
{
	// Token: 0x06002451 RID: 9297 RVA: 0x0001A1A9 File Offset: 0x000183A9
	public void OnEnable()
	{
		if (this.player != null)
		{
			this.UpdateStats(this.lastPosition);
			this.IsTurn = this.IsTurn;
		}
	}

	// Token: 0x06002452 RID: 9298 RVA: 0x000DA66C File Offset: 0x000D886C
	public void Setup(BoardPlayer player, int position)
	{
		this.player = player;
		Color uiColor = player.GamePlayer.Color.uiColor;
		uiColor.a = this.colorBackgroundBaseAlpha;
		this.colorBackground.color = uiColor;
		this.nameText.text = player.GamePlayer.Name;
		this.UpdateStats(position);
	}

	// Token: 0x06002453 RID: 9299 RVA: 0x000DA6C8 File Offset: 0x000D88C8
	public void UpdateStats(int position)
	{
		this.player.CurScorePosition = position;
		this.lastPosition = position;
		if (this.animator.GetInteger("Position") != position)
		{
			this.animator.SetInteger("Position", position);
		}
		this.healthBackground1.fillAmount = (float)this.player.ServerHealth / (float)this.player.maxHealth;
		this.healthBackground2.fillAmount = 1f - this.healthBackground1.fillAmount;
		this.healthText.text = this.player.ServerHealth.ToString();
		this.goldText.text = this.player.Gold.ToString();
		for (int i = 0; i < this.crowns.Length; i++)
		{
			if (i >= GameManager.WinningRelics)
			{
				this.crowns[i].enabled = false;
			}
			else
			{
				this.crowns[i].color = ((this.player.GoalScore > i) ? this.crownActiveColor : this.crownInActiveColor);
			}
		}
	}

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x06002454 RID: 9300 RVA: 0x0001A1D1 File Offset: 0x000183D1
	// (set) Token: 0x06002455 RID: 9301 RVA: 0x0001A1D9 File Offset: 0x000183D9
	public bool IsTurn
	{
		get
		{
			return this.isTurn;
		}
		set
		{
			this.isTurn = value;
			this.animator.SetBool("Turn", this.isTurn);
		}
	}

	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x06002456 RID: 9302 RVA: 0x0001A1F8 File Offset: 0x000183F8
	public BoardPlayer Player
	{
		get
		{
			return this.player;
		}
	}

	// Token: 0x0400277D RID: 10109
	[Header("References")]
	public Image colorBackground;

	// Token: 0x0400277E RID: 10110
	public Image[] crowns;

	// Token: 0x0400277F RID: 10111
	public Text nameText;

	// Token: 0x04002780 RID: 10112
	public Text healthText;

	// Token: 0x04002781 RID: 10113
	public Text goldText;

	// Token: 0x04002782 RID: 10114
	public Image healthBackground1;

	// Token: 0x04002783 RID: 10115
	public Image healthBackground2;

	// Token: 0x04002784 RID: 10116
	public Animator animator;

	// Token: 0x04002785 RID: 10117
	[Header("Values")]
	public float colorBackgroundBaseAlpha = 1f;

	// Token: 0x04002786 RID: 10118
	public Color crownInActiveColor;

	// Token: 0x04002787 RID: 10119
	public Color crownActiveColor;

	// Token: 0x04002788 RID: 10120
	public Sprite itemEmptyIcon;

	// Token: 0x04002789 RID: 10121
	public Sprite icon;

	// Token: 0x0400278A RID: 10122
	private bool isTurn;

	// Token: 0x0400278B RID: 10123
	private BoardPlayer player;

	// Token: 0x0400278C RID: 10124
	private int lastPosition;
}
