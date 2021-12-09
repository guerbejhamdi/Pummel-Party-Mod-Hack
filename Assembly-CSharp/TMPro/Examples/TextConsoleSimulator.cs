using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020005D1 RID: 1489
	public class TextConsoleSimulator : MonoBehaviour
	{
		// Token: 0x06002634 RID: 9780 RVA: 0x0001B473 File Offset: 0x00019673
		private void Awake()
		{
			this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x0001B486 File Offset: 0x00019686
		private void Start()
		{
			base.StartCoroutine(this.RevealCharacters(this.m_TextComponent));
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x0001B49B File Offset: 0x0001969B
		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x0001B4B3 File Offset: 0x000196B3
		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x0001B4CB File Offset: 0x000196CB
		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			this.hasTextChanged = true;
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x0001B4D4 File Offset: 0x000196D4
		private IEnumerator RevealCharacters(TMP_Text textComponent)
		{
			textComponent.ForceMeshUpdate(false, false);
			TMP_TextInfo textInfo = textComponent.textInfo;
			int totalVisibleCharacters = textInfo.characterCount;
			int visibleCount = 0;
			for (;;)
			{
				if (this.hasTextChanged)
				{
					totalVisibleCharacters = textInfo.characterCount;
					this.hasTextChanged = false;
				}
				if (visibleCount > totalVisibleCharacters)
				{
					yield return new WaitForSeconds(1f);
					visibleCount = 0;
				}
				textComponent.maxVisibleCharacters = visibleCount;
				visibleCount++;
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x0001B4EA File Offset: 0x000196EA
		private IEnumerator RevealWords(TMP_Text textComponent)
		{
			textComponent.ForceMeshUpdate(false, false);
			int totalWordCount = textComponent.textInfo.wordCount;
			int totalVisibleCharacters = textComponent.textInfo.characterCount;
			int counter = 0;
			int visibleCount = 0;
			for (;;)
			{
				int num = counter % (totalWordCount + 1);
				if (num == 0)
				{
					visibleCount = 0;
				}
				else if (num < totalWordCount)
				{
					visibleCount = textComponent.textInfo.wordInfo[num - 1].lastCharacterIndex + 1;
				}
				else if (num == totalWordCount)
				{
					visibleCount = totalVisibleCharacters;
				}
				textComponent.maxVisibleCharacters = visibleCount;
				if (visibleCount >= totalVisibleCharacters)
				{
					yield return new WaitForSeconds(1f);
				}
				counter++;
				yield return new WaitForSeconds(0.1f);
			}
			yield break;
		}

		// Token: 0x040029F1 RID: 10737
		private TMP_Text m_TextComponent;

		// Token: 0x040029F2 RID: 10738
		private bool hasTextChanged;
	}
}
