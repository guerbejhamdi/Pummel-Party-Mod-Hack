using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200001B RID: 27
public class BoardEventUI : MonoBehaviour
{
	// Token: 0x06000075 RID: 117 RVA: 0x0002CDF4 File Offset: 0x0002AFF4
	private void Update()
	{
		if (this.board == null)
		{
			this.board = GameManager.Board;
			return;
		}
		if (this.board.curMainBoardEvent != this.curEvent)
		{
			this.curEvent = this.board.curMainBoardEvent;
			if (this.curEvent != null)
			{
				this.icon.sprite = this.curEvent.eventSprite;
				this.icon.color = this.curEvent.spriteColor;
			}
		}
		if (this.curEvent == null)
		{
			this.group.alpha = Mathf.Clamp01(this.group.alpha - Time.deltaTime * this.fadeSpeed);
			return;
		}
		this.group.alpha = Mathf.Clamp01(this.group.alpha + Time.deltaTime * this.fadeSpeed);
	}

	// Token: 0x04000076 RID: 118
	public CanvasGroup group;

	// Token: 0x04000077 RID: 119
	public Image icon;

	// Token: 0x04000078 RID: 120
	public float fadeSpeed = 1f;

	// Token: 0x04000079 RID: 121
	private MainBoardEvent curEvent;

	// Token: 0x0400007A RID: 122
	private GameBoardController board;
}
