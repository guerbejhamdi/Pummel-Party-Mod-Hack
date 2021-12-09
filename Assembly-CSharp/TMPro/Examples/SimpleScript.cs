using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020005C0 RID: 1472
	public class SimpleScript : MonoBehaviour
	{
		// Token: 0x060025F4 RID: 9716 RVA: 0x000E51B0 File Offset: 0x000E33B0
		private void Start()
		{
			this.m_textMeshPro = base.gameObject.AddComponent<TextMeshPro>();
			this.m_textMeshPro.autoSizeTextContainer = true;
			this.m_textMeshPro.fontSize = 48f;
			this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
			this.m_textMeshPro.enableWordWrapping = false;
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x0001B154 File Offset: 0x00019354
		private void Update()
		{
			this.m_textMeshPro.SetText("The <#0050FF>count is: </color>{0:2}", this.m_frame % 1000f);
			this.m_frame += 1f * Time.deltaTime;
		}

		// Token: 0x04002996 RID: 10646
		private TextMeshPro m_textMeshPro;

		// Token: 0x04002997 RID: 10647
		private const string label = "The <#0050FF>count is: </color>{0:2}";

		// Token: 0x04002998 RID: 10648
		private float m_frame;
	}
}
