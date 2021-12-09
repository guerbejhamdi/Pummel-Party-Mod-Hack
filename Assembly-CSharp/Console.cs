using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000452 RID: 1106
public class Console : MonoBehaviour
{
	// Token: 0x06001E5B RID: 7771 RVA: 0x00016605 File Offset: 0x00014805
	private void Start()
	{
		if (!GameManager.DEBUGGING)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06001E5C RID: 7772 RVA: 0x00016615 File Offset: 0x00014815
	private void OnEnable()
	{
		Application.logMessageReceived += this.HandleLog;
	}

	// Token: 0x06001E5D RID: 7773 RVA: 0x00016628 File Offset: 0x00014828
	private void OnDisable()
	{
		Application.logMessageReceived -= this.HandleLog;
	}

	// Token: 0x06001E5E RID: 7774 RVA: 0x000C3FF8 File Offset: 0x000C21F8
	private void HandleLog(string logString, string stackTrace, LogType type)
	{
		string[] array = new string[]
		{
			"#E06666",
			"#C8C8C8",
			"#FFD966",
			"#C8C8C8",
			"#E06666"
		};
		string[] array2 = new string[]
		{
			"#E58080",
			"#d9d9d9",
			"#ffe699",
			"#d9d9d9",
			"#E58080"
		};
		string text = string.Format("\n<color={0}>{1} <i>{2}</i></color>", this.alternate ? array[(int)type] : array2[(int)type], logString, stackTrace.Split(new string[]
		{
			Environment.NewLine,
			"\r\n",
			"\n"
		}, StringSplitOptions.None)[0]);
		this.entries.Add(text);
		if (this.entries.Count >= this.maxEntries)
		{
			this.consoleText.text = this.consoleText.text.Remove(0, this.entries[0].Length) + text;
			this.entries.RemoveAt(0);
			return;
		}
		Text text2 = this.consoleText;
		text2.text += text;
	}

	// Token: 0x06001E5F RID: 7775 RVA: 0x0001663B File Offset: 0x0001483B
	private void Update()
	{
		if (GameManager.DEBUGGING && Input.GetKeyDown(KeyCode.BackQuote))
		{
			this.parent.SetActive(!this.parent.activeInHierarchy);
			this.scrollRect.verticalNormalizedPosition = 0f;
		}
	}

	// Token: 0x04002148 RID: 8520
	public string output = "";

	// Token: 0x04002149 RID: 8521
	public string stack = "";

	// Token: 0x0400214A RID: 8522
	public GameObject parent;

	// Token: 0x0400214B RID: 8523
	public Text consoleText;

	// Token: 0x0400214C RID: 8524
	public ScrollRect scrollRect;

	// Token: 0x0400214D RID: 8525
	private bool alternate = true;

	// Token: 0x0400214E RID: 8526
	private int maxEntries = 30;

	// Token: 0x0400214F RID: 8527
	private List<string> entries = new List<string>();
}
