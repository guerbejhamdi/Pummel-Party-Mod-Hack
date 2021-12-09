using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003B RID: 59
public class ControllerTextInputHelper : MonoBehaviour
{
	// Token: 0x060000FC RID: 252 RVA: 0x0000439D File Offset: 0x0000259D
	private void Start()
	{
		this.mainMenuWindow = base.GetComponentInParent<MainMenuWindow>();
		this.inputField = base.GetComponentInParent<InputField>();
	}

	// Token: 0x060000FD RID: 253 RVA: 0x000043B7 File Offset: 0x000025B7
	public void OnClick()
	{
		this.mainMenuWindow.SetState(MainMenuWindowState.Disabled);
		GameManager.MainMenuUIController.onScreenKeyboardUI.Show(this);
	}

	// Token: 0x060000FE RID: 254 RVA: 0x000043D5 File Offset: 0x000025D5
	public void Finish(string text)
	{
		this.inputField.text = text;
		this.mainMenuWindow.SetState(MainMenuWindowState.Visible);
	}

	// Token: 0x060000FF RID: 255 RVA: 0x000043EF File Offset: 0x000025EF
	public void Cancel()
	{
		this.mainMenuWindow.SetState(MainMenuWindowState.Visible);
	}

	// Token: 0x04000155 RID: 341
	private Button button;

	// Token: 0x04000156 RID: 342
	private MainMenuWindow mainMenuWindow;

	// Token: 0x04000157 RID: 343
	private InputField inputField;
}
