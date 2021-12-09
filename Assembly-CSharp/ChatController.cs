using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020004D4 RID: 1236
public class ChatController : MonoBehaviour
{
	// Token: 0x060020B7 RID: 8375 RVA: 0x00017CE9 File Offset: 0x00015EE9
	private void OnEnable()
	{
		this.ChatInputField.onSubmit.AddListener(new UnityAction<string>(this.AddToChatOutput));
	}

	// Token: 0x060020B8 RID: 8376 RVA: 0x00017D07 File Offset: 0x00015F07
	private void OnDisable()
	{
		this.ChatInputField.onSubmit.RemoveListener(new UnityAction<string>(this.AddToChatOutput));
	}

	// Token: 0x060020B9 RID: 8377 RVA: 0x000CCBD4 File Offset: 0x000CADD4
	private void AddToChatOutput(string newText)
	{
		this.ChatInputField.text = string.Empty;
		DateTime now = DateTime.Now;
		string text = string.Concat(new string[]
		{
			"[<#FFFF80>",
			now.Hour.ToString("d2"),
			":",
			now.Minute.ToString("d2"),
			":",
			now.Second.ToString("d2"),
			"</color>] ",
			newText
		});
		if (this.ChatDisplayOutput != null)
		{
			if (this.ChatDisplayOutput.text == string.Empty)
			{
				this.ChatDisplayOutput.text = text;
			}
			else
			{
				TMP_Text chatDisplayOutput = this.ChatDisplayOutput;
				chatDisplayOutput.text = chatDisplayOutput.text + "\n" + text;
			}
		}
		this.ChatInputField.ActivateInputField();
		this.ChatScrollbar.value = 0f;
	}

	// Token: 0x04002380 RID: 9088
	public TMP_InputField ChatInputField;

	// Token: 0x04002381 RID: 9089
	public TMP_Text ChatDisplayOutput;

	// Token: 0x04002382 RID: 9090
	public Scrollbar ChatScrollbar;
}
