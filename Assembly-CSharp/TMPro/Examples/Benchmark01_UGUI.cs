using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro.Examples
{
	// Token: 0x020005B4 RID: 1460
	public class Benchmark01_UGUI : MonoBehaviour
	{
		// Token: 0x060025D3 RID: 9683 RVA: 0x0001B04F File Offset: 0x0001924F
		private IEnumerator Start()
		{
			if (this.BenchmarkType == 0)
			{
				this.m_textMeshPro = base.gameObject.AddComponent<TextMeshProUGUI>();
				if (this.TMProFont != null)
				{
					this.m_textMeshPro.font = this.TMProFont;
				}
				this.m_textMeshPro.fontSize = 48f;
				this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
				this.m_textMeshPro.extraPadding = true;
				this.m_material01 = this.m_textMeshPro.font.material;
				this.m_material02 = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - BEVEL");
			}
			else if (this.BenchmarkType == 1)
			{
				this.m_textMesh = base.gameObject.AddComponent<Text>();
				if (this.TextMeshFont != null)
				{
					this.m_textMesh.font = this.TextMeshFont;
				}
				this.m_textMesh.fontSize = 48;
				this.m_textMesh.alignment = TextAnchor.MiddleCenter;
			}
			int num;
			for (int i = 0; i <= 1000000; i = num + 1)
			{
				if (this.BenchmarkType == 0)
				{
					this.m_textMeshPro.text = "The <#0050FF>count is: </color>" + (i % 1000).ToString();
					if (i % 1000 == 999)
					{
						this.m_textMeshPro.fontSharedMaterial = ((this.m_textMeshPro.fontSharedMaterial == this.m_material01) ? (this.m_textMeshPro.fontSharedMaterial = this.m_material02) : (this.m_textMeshPro.fontSharedMaterial = this.m_material01));
					}
				}
				else if (this.BenchmarkType == 1)
				{
					this.m_textMesh.text = "The <color=#0050FF>count is: </color>" + (i % 1000).ToString();
				}
				yield return null;
				num = i;
			}
			yield return null;
			yield break;
		}

		// Token: 0x04002944 RID: 10564
		public int BenchmarkType;

		// Token: 0x04002945 RID: 10565
		public Canvas canvas;

		// Token: 0x04002946 RID: 10566
		public TMP_FontAsset TMProFont;

		// Token: 0x04002947 RID: 10567
		public Font TextMeshFont;

		// Token: 0x04002948 RID: 10568
		private TextMeshProUGUI m_textMeshPro;

		// Token: 0x04002949 RID: 10569
		private Text m_textMesh;

		// Token: 0x0400294A RID: 10570
		private const string label01 = "The <#0050FF>count is: </color>";

		// Token: 0x0400294B RID: 10571
		private const string label02 = "The <color=#0050FF>count is: </color>";

		// Token: 0x0400294C RID: 10572
		private Material m_material01;

		// Token: 0x0400294D RID: 10573
		private Material m_material02;
	}
}
