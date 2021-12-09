using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004F6 RID: 1270
public class DialogWindow : MainMenuWindow
{
	// Token: 0x06002177 RID: 8567 RVA: 0x000183DB File Offset: 0x000165DB
	protected override void Initialize()
	{
		if (base.CurState == MainMenuWindowState.Uninitialized)
		{
			base.Initialize();
			this.dialog_text = base.transform.Find("Window/Lbl").GetComponent<Text>();
		}
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x00018406 File Offset: 0x00016606
	public void SetDialog(string msg, Color txt_color, DialogType type)
	{
		this.Initialize();
		this.dialog_text.text = msg;
		this.dialog_text.color = txt_color;
	}

	// Token: 0x04002424 RID: 9252
	private DialogType type;

	// Token: 0x04002425 RID: 9253
	private Text dialog_text;
}
