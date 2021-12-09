using System;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro.Examples
{
	// Token: 0x020005C7 RID: 1479
	public class TMP_TextEventCheck : MonoBehaviour
	{
		// Token: 0x0600260A RID: 9738 RVA: 0x000E5C94 File Offset: 0x000E3E94
		private void OnEnable()
		{
			if (this.TextEventHandler != null)
			{
				this.m_TextComponent = this.TextEventHandler.GetComponent<TMP_Text>();
				this.TextEventHandler.onCharacterSelection.AddListener(new UnityAction<char, int>(this.OnCharacterSelection));
				this.TextEventHandler.onSpriteSelection.AddListener(new UnityAction<char, int>(this.OnSpriteSelection));
				this.TextEventHandler.onWordSelection.AddListener(new UnityAction<string, int, int>(this.OnWordSelection));
				this.TextEventHandler.onLineSelection.AddListener(new UnityAction<string, int, int>(this.OnLineSelection));
				this.TextEventHandler.onLinkSelection.AddListener(new UnityAction<string, string, int>(this.OnLinkSelection));
			}
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x000E5D50 File Offset: 0x000E3F50
		private void OnDisable()
		{
			if (this.TextEventHandler != null)
			{
				this.TextEventHandler.onCharacterSelection.RemoveListener(new UnityAction<char, int>(this.OnCharacterSelection));
				this.TextEventHandler.onSpriteSelection.RemoveListener(new UnityAction<char, int>(this.OnSpriteSelection));
				this.TextEventHandler.onWordSelection.RemoveListener(new UnityAction<string, int, int>(this.OnWordSelection));
				this.TextEventHandler.onLineSelection.RemoveListener(new UnityAction<string, int, int>(this.OnLineSelection));
				this.TextEventHandler.onLinkSelection.RemoveListener(new UnityAction<string, string, int>(this.OnLinkSelection));
			}
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x0001B248 File Offset: 0x00019448
		private void OnCharacterSelection(char c, int index)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Character [",
				c.ToString(),
				"] at Index: ",
				index.ToString(),
				" has been selected."
			}));
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x0001B286 File Offset: 0x00019486
		private void OnSpriteSelection(char c, int index)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Sprite [",
				c.ToString(),
				"] at Index: ",
				index.ToString(),
				" has been selected."
			}));
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x000E5DFC File Offset: 0x000E3FFC
		private void OnWordSelection(string word, int firstCharacterIndex, int length)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Word [",
				word,
				"] with first character index of ",
				firstCharacterIndex.ToString(),
				" and length of ",
				length.ToString(),
				" has been selected."
			}));
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x000E5E54 File Offset: 0x000E4054
		private void OnLineSelection(string lineText, int firstCharacterIndex, int length)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Line [",
				lineText,
				"] with first character index of ",
				firstCharacterIndex.ToString(),
				" and length of ",
				length.ToString(),
				" has been selected."
			}));
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x000E5EAC File Offset: 0x000E40AC
		private void OnLinkSelection(string linkID, string linkText, int linkIndex)
		{
			if (this.m_TextComponent != null)
			{
				TMP_LinkInfo[] linkInfo = this.m_TextComponent.textInfo.linkInfo;
			}
			Debug.Log(string.Concat(new string[]
			{
				"Link Index: ",
				linkIndex.ToString(),
				" with ID [",
				linkID,
				"] and Text \"",
				linkText,
				"\" has been selected."
			}));
		}

		// Token: 0x040029BA RID: 10682
		public TMP_TextEventHandler TextEventHandler;

		// Token: 0x040029BB RID: 10683
		private TMP_Text m_TextComponent;
	}
}
