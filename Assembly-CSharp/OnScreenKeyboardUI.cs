using System;
using I2.Loc;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002B9 RID: 697
public class OnScreenKeyboardUI : MonoBehaviour
{
	// Token: 0x0600141C RID: 5148 RVA: 0x0000FD17 File Offset: 0x0000DF17
	private void Start()
	{
		GameManager.MainMenuUIController.onScreenKeyboardUI = this;
	}

	// Token: 0x0600141D RID: 5149 RVA: 0x00097B80 File Offset: 0x00095D80
	private void Update()
	{
		if (this.mainMenuWindow != null && this.mainMenuWindow.Visible && ReInput.isReady && ReInput.players.GetPlayer(0).GetButtonDown(InputActions.Cancel))
		{
			this.BackSpace();
		}
	}

	// Token: 0x0600141E RID: 5150 RVA: 0x00097BCC File Offset: 0x00095DCC
	public void Show(ControllerTextInputHelper helper)
	{
		this.mainMenuWindow.SetState(MainMenuWindowState.Visible);
		this.textbox.text = LocalizationManager.GetTranslation("Enter text", true, 0, true, false, null, null, true) + "...";
		this.curText = "";
		this.helper = helper;
	}

	// Token: 0x0600141F RID: 5151 RVA: 0x0000FD24 File Offset: 0x0000DF24
	public void OnClick(string key)
	{
		this.curText += key;
		this.textbox.text = this.curText;
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x00097C20 File Offset: 0x00095E20
	public void BackSpace()
	{
		if (this.curText.Length > 1)
		{
			this.curText = this.curText.Substring(0, this.curText.Length - 1);
		}
		else if (this.curText.Length == 1)
		{
			this.curText = "";
		}
		this.textbox.text = this.curText;
	}

	// Token: 0x06001421 RID: 5153 RVA: 0x0000FD49 File Offset: 0x0000DF49
	public void Finish()
	{
		this.helper.Finish(this.curText);
		this.mainMenuWindow.SetState(MainMenuWindowState.Hidden);
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x0000FD68 File Offset: 0x0000DF68
	public void Cancel()
	{
		this.helper.Cancel();
		this.mainMenuWindow.SetState(MainMenuWindowState.Hidden);
	}

	// Token: 0x0400155B RID: 5467
	public Text textbox;

	// Token: 0x0400155C RID: 5468
	public MainMenuWindow mainMenuWindow;

	// Token: 0x0400155D RID: 5469
	private string curText = "";

	// Token: 0x0400155E RID: 5470
	private ControllerTextInputHelper helper;
}
