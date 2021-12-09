using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020005D8 RID: 1496
	public class VertexColorCycler : MonoBehaviour
	{
		// Token: 0x0600265D RID: 9821 RVA: 0x0001B5DA File Offset: 0x000197DA
		private void Awake()
		{
			this.m_TextComponent = base.GetComponent<TMP_Text>();
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x0001B5E8 File Offset: 0x000197E8
		private void Start()
		{
			base.StartCoroutine(this.AnimateVertexColors());
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x0001B5F7 File Offset: 0x000197F7
		private IEnumerator AnimateVertexColors()
		{
			this.m_TextComponent.ForceMeshUpdate(false, false);
			TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
			int currentCharacter = 0;
			Color32 color = this.m_TextComponent.color;
			for (;;)
			{
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
					yield return new WaitForSeconds(0.25f);
				}
				else
				{
					int materialReferenceIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;
					Color32[] colors = textInfo.meshInfo[materialReferenceIndex].colors32;
					int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;
					if (textInfo.characterInfo[currentCharacter].isVisible)
					{
						color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), byte.MaxValue);
						colors[vertexIndex] = color;
						colors[vertexIndex + 1] = color;
						colors[vertexIndex + 2] = color;
						colors[vertexIndex + 3] = color;
						this.m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
					}
					currentCharacter = (currentCharacter + 1) % characterCount;
					yield return new WaitForSeconds(0.05f);
				}
			}
			yield break;
		}

		// Token: 0x04002A26 RID: 10790
		private TMP_Text m_TextComponent;
	}
}
