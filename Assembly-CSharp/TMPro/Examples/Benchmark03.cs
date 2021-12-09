using System;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace TMPro.Examples
{
	// Token: 0x020005B7 RID: 1463
	public class Benchmark03 : MonoBehaviour
	{
		// Token: 0x060025DD RID: 9693 RVA: 0x0000398C File Offset: 0x00001B8C
		private void Awake()
		{
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x000E448C File Offset: 0x000E268C
		private void Start()
		{
			TMP_FontAsset tmp_FontAsset = null;
			switch (this.Benchmark)
			{
			case Benchmark03.BenchmarkType.TMP_SDF_MOBILE:
				tmp_FontAsset = TMP_FontAsset.CreateFontAsset(this.SourceFont, 90, 9, GlyphRenderMode.SDFAA, 256, 256, AtlasPopulationMode.Dynamic, true);
				break;
			case Benchmark03.BenchmarkType.TMP_SDF__MOBILE_SSD:
				tmp_FontAsset = TMP_FontAsset.CreateFontAsset(this.SourceFont, 90, 9, GlyphRenderMode.SDFAA, 256, 256, AtlasPopulationMode.Dynamic, true);
				tmp_FontAsset.material.shader = Shader.Find("TextMeshPro/Mobile/Distance Field SSD");
				break;
			case Benchmark03.BenchmarkType.TMP_SDF:
				tmp_FontAsset = TMP_FontAsset.CreateFontAsset(this.SourceFont, 90, 9, GlyphRenderMode.SDFAA, 256, 256, AtlasPopulationMode.Dynamic, true);
				tmp_FontAsset.material.shader = Shader.Find("TextMeshPro/Distance Field");
				break;
			case Benchmark03.BenchmarkType.TMP_BITMAP_MOBILE:
				tmp_FontAsset = TMP_FontAsset.CreateFontAsset(this.SourceFont, 90, 9, GlyphRenderMode.SMOOTH, 256, 256, AtlasPopulationMode.Dynamic, true);
				break;
			}
			for (int i = 0; i < this.NumberOfSamples; i++)
			{
				Benchmark03.BenchmarkType benchmark = this.Benchmark;
				if (benchmark > Benchmark03.BenchmarkType.TMP_BITMAP_MOBILE)
				{
					if (benchmark == Benchmark03.BenchmarkType.TEXTMESH_BITMAP)
					{
						TextMesh textMesh = new GameObject
						{
							transform = 
							{
								position = new Vector3(0f, 1.2f, 0f)
							}
						}.AddComponent<TextMesh>();
						textMesh.GetComponent<Renderer>().sharedMaterial = this.SourceFont.material;
						textMesh.font = this.SourceFont;
						textMesh.anchor = TextAnchor.MiddleCenter;
						textMesh.fontSize = 130;
						textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
						textMesh.text = "@";
					}
				}
				else
				{
					TextMeshPro textMeshPro = new GameObject
					{
						transform = 
						{
							position = new Vector3(0f, 1.2f, 0f)
						}
					}.AddComponent<TextMeshPro>();
					textMeshPro.font = tmp_FontAsset;
					textMeshPro.fontSize = 128f;
					textMeshPro.text = "@";
					textMeshPro.alignment = TextAlignmentOptions.Center;
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					if (this.Benchmark == Benchmark03.BenchmarkType.TMP_BITMAP_MOBILE)
					{
						textMeshPro.fontSize = 132f;
					}
				}
			}
		}

		// Token: 0x04002956 RID: 10582
		public int NumberOfSamples = 100;

		// Token: 0x04002957 RID: 10583
		public Benchmark03.BenchmarkType Benchmark;

		// Token: 0x04002958 RID: 10584
		public Font SourceFont;

		// Token: 0x020005B8 RID: 1464
		public enum BenchmarkType
		{
			// Token: 0x0400295A RID: 10586
			TMP_SDF_MOBILE,
			// Token: 0x0400295B RID: 10587
			TMP_SDF__MOBILE_SSD,
			// Token: 0x0400295C RID: 10588
			TMP_SDF,
			// Token: 0x0400295D RID: 10589
			TMP_BITMAP_MOBILE,
			// Token: 0x0400295E RID: 10590
			TEXTMESH_BITMAP
		}
	}
}
