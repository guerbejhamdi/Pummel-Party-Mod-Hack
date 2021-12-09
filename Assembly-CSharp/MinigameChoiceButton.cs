using System;
using UnityEngine;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x0200051A RID: 1306
public class MinigameChoiceButton : MonoBehaviour
{
	// Token: 0x06002216 RID: 8726 RVA: 0x00018B27 File Offset: 0x00016D27
	public void Initialize(MinigameChoiceWindow new_window, string name)
	{
		this.window = new_window;
		this.button_text.text = name;
		this.minigame_name = name;
	}

	// Token: 0x06002217 RID: 8727 RVA: 0x00018B43 File Offset: 0x00016D43
	public void OnButtonPress()
	{
		if (NetSystem.IsServer)
		{
			this.window.ChooseMinigame(this.minigame_name);
		}
	}

	// Token: 0x0400252F RID: 9519
	public Text button_text;

	// Token: 0x04002530 RID: 9520
	private MinigameChoiceWindow window;

	// Token: 0x04002531 RID: 9521
	private string minigame_name;
}
