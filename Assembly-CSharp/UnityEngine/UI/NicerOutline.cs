using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	// Token: 0x020007B7 RID: 1975
	[AddComponentMenu("UI/Effects/NicerOutline", 15)]
	public class NicerOutline : BaseMeshEffect
	{
		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x0600385C RID: 14428 RVA: 0x0002674A File Offset: 0x0002494A
		// (set) Token: 0x0600385D RID: 14429 RVA: 0x00026752 File Offset: 0x00024952
		public Color effectColor
		{
			get
			{
				return this.m_EffectColor;
			}
			set
			{
				this.m_EffectColor = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x0600385E RID: 14430 RVA: 0x00026774 File Offset: 0x00024974
		// (set) Token: 0x0600385F RID: 14431 RVA: 0x0011B280 File Offset: 0x00119480
		public Vector2 effectDistance
		{
			get
			{
				return this.m_EffectDistance;
			}
			set
			{
				if (value.x > 600f)
				{
					value.x = 600f;
				}
				if (value.x < -600f)
				{
					value.x = -600f;
				}
				if (value.y > 600f)
				{
					value.y = 600f;
				}
				if (value.y < -600f)
				{
					value.y = -600f;
				}
				if (this.m_EffectDistance == value)
				{
					return;
				}
				this.m_EffectDistance = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06003860 RID: 14432 RVA: 0x0002677C File Offset: 0x0002497C
		// (set) Token: 0x06003861 RID: 14433 RVA: 0x00026784 File Offset: 0x00024984
		public bool useGraphicAlpha
		{
			get
			{
				return this.m_UseGraphicAlpha;
			}
			set
			{
				this.m_UseGraphicAlpha = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x06003862 RID: 14434 RVA: 0x0011B320 File Offset: 0x00119520
		protected void ApplyShadow(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
		{
			int num = verts.Count * 2;
			if (verts.Capacity < num)
			{
				verts.Capacity = num;
			}
			for (int i = start; i < end; i++)
			{
				UIVertex uivertex = verts[i];
				verts.Add(uivertex);
				Vector3 position = uivertex.position;
				position.x += x;
				position.y += y;
				uivertex.position = position;
				Color32 color2 = color;
				if (this.m_UseGraphicAlpha)
				{
					color2.a = color2.a * verts[i].color.a / byte.MaxValue;
				}
				uivertex.color = color2;
				verts[i] = uivertex;
			}
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x0011B3D4 File Offset: 0x001195D4
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			this.ModifyVertices(list);
			vh.AddUIVertexTriangleStream(list);
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x0011B408 File Offset: 0x00119608
		public override void ModifyMesh(Mesh mesh)
		{
			if (!this.IsActive())
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			using (VertexHelper vertexHelper = new VertexHelper(mesh))
			{
				vertexHelper.GetUIVertexStream(list);
			}
			this.ModifyVertices(list);
			using (VertexHelper vertexHelper2 = new VertexHelper())
			{
				vertexHelper2.AddUIVertexTriangleStream(list);
				vertexHelper2.FillMesh(mesh);
			}
		}

		// Token: 0x06003865 RID: 14437 RVA: 0x0011B484 File Offset: 0x00119684
		public void ModifyVertices(List<UIVertex> verts)
		{
			if (!this.IsActive())
			{
				return;
			}
			Text component = base.GetComponent<Text>();
			float num = 1f;
			if (component && component.resizeTextForBestFit)
			{
				num = (float)component.cachedTextGenerator.fontSizeUsedForBestFit / (float)(component.resizeTextMaxSize - 1);
			}
			float num2 = this.effectDistance.x * num;
			float num3 = this.effectDistance.y * num;
			int start = 0;
			int count = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, num2, num3);
			start = count;
			int count2 = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, num2, -num3);
			start = count2;
			int count3 = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, -num2, num3);
			start = count3;
			int count4 = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, -num2, -num3);
			start = count4;
			int count5 = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, num2, 0f);
			start = count5;
			int count6 = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, -num2, 0f);
			start = count6;
			int count7 = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, 0f, num3);
			start = count7;
			int count8 = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, 0f, -num3);
		}

		// Token: 0x04003703 RID: 14083
		[SerializeField]
		private Color m_EffectColor = new Color(0f, 0f, 0f, 0.5f);

		// Token: 0x04003704 RID: 14084
		[SerializeField]
		private Vector2 m_EffectDistance = new Vector2(1f, -1f);

		// Token: 0x04003705 RID: 14085
		[SerializeField]
		private bool m_UseGraphicAlpha = true;
	}
}
