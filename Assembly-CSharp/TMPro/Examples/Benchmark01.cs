using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020005B2 RID: 1458
	public class Benchmark01 : MonoBehaviour
	{
		// Token: 0x060025CB RID: 9675 RVA: 0x0001B029 File Offset: 0x00019229
		private IEnumerator Start()
		{
			if (this.BenchmarkType == 0)
			{
				this.m_textMeshPro = base.gameObject.AddComponent<TextMeshPro>();
				this.m_textMeshPro.autoSizeTextContainer = true;
				if (this.TMProFont != null)
				{
					this.m_textMeshPro.font = this.TMProFont;
				}
				this.m_textMeshPro.fontSize = 48f;
				this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
				this.m_textMeshPro.extraPadding = true;
				this.m_textMeshPro.enableWordWrapping = false;
				this.m_material01 = this.m_textMeshPro.font.material;
				this.m_material02 = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Drop Shadow");
			}
			else if (this.BenchmarkType == 1)
			{
				this.m_textMesh = base.gameObject.AddComponent<TextMesh>();
				if (this.TextMeshFont != null)
				{
					this.m_textMesh.font = this.TextMeshFont;
					this.m_textMesh.GetComponent<Renderer>().sharedMaterial = this.m_textMesh.font.material;
				}
				else
				{
					this.m_textMesh.font = (Resources.Load("Fonts/ARIAL", typeof(Font)) as Font);
					this.m_textMesh.GetComponent<Renderer>().sharedMaterial = this.m_textMesh.font.material;
				}
				this.m_textMesh.fontSize = 48;
				this.m_textMesh.anchor = TextAnchor.MiddleCenter;
			}
			int num;
			for (int i = 0; i <= 1000000; i = num + 1)
			{
				if (this.BenchmarkType == 0)
				{
					this.m_textMeshPro.SetText("The <#0050FF>count is: </color>{0}", (float)(i % 1000));
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

		// Token: 0x04002936 RID: 10550
		public int BenchmarkType;

		// Token: 0x04002937 RID: 10551
		public TMP_FontAsset TMProFont;

		// Token: 0x04002938 RID: 10552
		public Font TextMeshFont;

		// Token: 0x04002939 RID: 10553
		private TextMeshPro m_textMeshPro;

		// Token: 0x0400293A RID: 10554
		private TextContainer m_textContainer;

		// Token: 0x0400293B RID: 10555
		private TextMesh m_textMesh;

		// Token: 0x0400293C RID: 10556
		private const string label01 = "The <#0050FF>count is: </color>{0}";

		// Token: 0x0400293D RID: 10557
		private const string label02 = "The <color=#0050FF>count is: </color>";

		// Token: 0x0400293E RID: 10558
		private Material m_material01;

		// Token: 0x0400293F RID: 10559
		private Material m_material02;
	}
}
