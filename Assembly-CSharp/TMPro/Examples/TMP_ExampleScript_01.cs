using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020005C3 RID: 1475
	public class TMP_ExampleScript_01 : MonoBehaviour
	{
		// Token: 0x06002602 RID: 9730 RVA: 0x000E5860 File Offset: 0x000E3A60
		private void Awake()
		{
			if (this.ObjectType == TMP_ExampleScript_01.objectType.TextMeshPro)
			{
				this.m_text = (base.GetComponent<TextMeshPro>() ?? base.gameObject.AddComponent<TextMeshPro>());
			}
			else
			{
				this.m_text = (base.GetComponent<TextMeshProUGUI>() ?? base.gameObject.AddComponent<TextMeshProUGUI>());
			}
			this.m_text.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Anton SDF");
			this.m_text.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/Anton SDF - Drop Shadow");
			this.m_text.fontSize = 120f;
			this.m_text.text = "A <#0080ff>simple</color> line of text.";
			Vector2 preferredValues = this.m_text.GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
			this.m_text.rectTransform.sizeDelta = new Vector2(preferredValues.x, preferredValues.y);
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x0001B1E5 File Offset: 0x000193E5
		private void Update()
		{
			if (!this.isStatic)
			{
				this.m_text.SetText("The count is <#0080ff>{0}</color>", (float)(this.count % 1000));
				this.count++;
			}
		}

		// Token: 0x040029A3 RID: 10659
		public TMP_ExampleScript_01.objectType ObjectType;

		// Token: 0x040029A4 RID: 10660
		public bool isStatic;

		// Token: 0x040029A5 RID: 10661
		private TMP_Text m_text;

		// Token: 0x040029A6 RID: 10662
		private const string k_label = "The count is <#0080ff>{0}</color>";

		// Token: 0x040029A7 RID: 10663
		private int count;

		// Token: 0x020005C4 RID: 1476
		public enum objectType
		{
			// Token: 0x040029A9 RID: 10665
			TextMeshPro,
			// Token: 0x040029AA RID: 10666
			TextMeshProUGUI
		}
	}
}
