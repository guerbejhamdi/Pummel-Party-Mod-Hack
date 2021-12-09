using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000564 RID: 1380
public class UIRelayDebug : MonoBehaviour
{
	// Token: 0x06002458 RID: 9304 RVA: 0x0001A213 File Offset: 0x00018413
	public void SetText(string str)
	{
		this.m_text.text = str;
	}

	// Token: 0x0400278D RID: 10125
	[SerializeField]
	private Text m_text;
}
