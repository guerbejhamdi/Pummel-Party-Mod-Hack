using System;
using I2.Loc;
using UnityEngine;

// Token: 0x0200052D RID: 1325
public class SimpleInteractionDialog : InteractionDialog
{
	// Token: 0x060022E6 RID: 8934 RVA: 0x000D4874 File Offset: 0x000D2A74
	public void Activate(string title, string desc, InteractionButtonSettings[] buttonSettings, GamePlayer player, Sprite sprite)
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
			if (buttonSettings != null && i < buttonSettings.Length)
			{
				this.buttons[i].gameObject.SetActive(true);
				this.buttons[i].Setup(player, buttonSettings[i]);
			}
			else
			{
				this.buttons[i].gameObject.SetActive(false);
			}
		}
	}
}
