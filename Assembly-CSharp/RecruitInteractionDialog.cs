using System;
using I2.Loc;
using UnityEngine;

// Token: 0x02000313 RID: 787
public class RecruitInteractionDialog : InteractionDialog
{
	// Token: 0x060015AA RID: 5546 RVA: 0x0009C1A4 File Offset: 0x0009A3A4
	public void Setup(string title, string desc, Sprite sprite, GamePlayer player)
	{
		if (player.IsLocalPlayer)
		{
			GameManager.UIController.SetInputStatus(true);
			this.eventSystemGroup.EventSystemID = (int)player.LocalIDAndAI;
		}
		this.window.SetState(MainMenuWindowState.Visible);
		this.titleText.text = LocalizationManager.GetTranslation(title, true, 0, true, false, null, null, true);
		this.descText.text = LocalizationManager.GetTranslation(desc, true, 0, true, false, null, null, true);
		this.icon.sprite = sprite;
		for (int i = 0; i < this.buttons.Length; i++)
		{
			this.buttons[i].Setup(player);
		}
	}
}
