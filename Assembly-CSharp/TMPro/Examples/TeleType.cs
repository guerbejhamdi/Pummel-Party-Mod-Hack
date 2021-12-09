using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020005CF RID: 1487
	public class TeleType : MonoBehaviour
	{
		// Token: 0x0600262B RID: 9771 RVA: 0x0001B3F4 File Offset: 0x000195F4
		private void Awake()
		{
			this.m_textMeshPro = base.GetComponent<TMP_Text>();
			this.m_textMeshPro.text = this.label01;
			this.m_textMeshPro.enableWordWrapping = true;
			this.m_textMeshPro.alignment = TextAlignmentOptions.Top;
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x0001B42F File Offset: 0x0001962F
		private IEnumerator Start()
		{
			this.m_textMeshPro.ForceMeshUpdate(false, false);
			int totalVisibleCharacters = this.m_textMeshPro.textInfo.characterCount;
			int counter = 0;
			for (;;)
			{
				int num = counter % (totalVisibleCharacters + 1);
				this.m_textMeshPro.maxVisibleCharacters = num;
				if (num >= totalVisibleCharacters)
				{
					yield return new WaitForSeconds(1f);
					this.m_textMeshPro.text = this.label02;
					yield return new WaitForSeconds(1f);
					this.m_textMeshPro.text = this.label01;
					yield return new WaitForSeconds(1f);
				}
				counter++;
				yield return new WaitForSeconds(0.05f);
			}
			yield break;
		}

		// Token: 0x040029E9 RID: 10729
		private string label01 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=1>";

		// Token: 0x040029EA RID: 10730
		private string label02 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=2>";

		// Token: 0x040029EB RID: 10731
		private TMP_Text m_textMeshPro;
	}
}
