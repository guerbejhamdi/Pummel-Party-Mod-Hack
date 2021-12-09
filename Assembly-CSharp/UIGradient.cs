using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010E RID: 270
[AddComponentMenu("UI/Effects/Gradient")]
public class UIGradient : BaseMeshEffect
{
	// Token: 0x06000821 RID: 2081 RVA: 0x00009A85 File Offset: 0x00007C85
	protected override void Start()
	{
		this.targetGraphic = base.GetComponent<Graphic>();
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x0004C2C4 File Offset: 0x0004A4C4
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

	// Token: 0x06000823 RID: 2083 RVA: 0x0004C2F8 File Offset: 0x0004A4F8
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

	// Token: 0x06000824 RID: 2084 RVA: 0x0004C374 File Offset: 0x0004A574
	public void ModifyVertices(List<UIVertex> vertexList)
	{
		if (this.targetGraphic == null)
		{
			this.targetGraphic = base.GetComponent<Graphic>();
		}
		if (!this.IsActive() || vertexList.Count == 0)
		{
			return;
		}
		int count = vertexList.Count;
		UIVertex uivertex = vertexList[0];
		if (this.gradientMode == global::GradientMode.Global)
		{
			if (this.gradientDir == GradientDir.DiagonalLeftToRight || this.gradientDir == GradientDir.DiagonalRightToLeft)
			{
				this.gradientDir = GradientDir.Vertical;
			}
			float num = (this.gradientDir == GradientDir.Vertical) ? vertexList[vertexList.Count - 1].position.y : vertexList[vertexList.Count - 1].position.x;
			float num2 = ((this.gradientDir == GradientDir.Vertical) ? vertexList[0].position.y : vertexList[0].position.x) - num;
			for (int i = 0; i < count; i++)
			{
				uivertex = vertexList[i];
				if (this.overwriteAllColor || !(uivertex.color != this.targetGraphic.color))
				{
					uivertex.color *= Color.Lerp(this.vertex2, this.vertex1, (((this.gradientDir == GradientDir.Vertical) ? uivertex.position.y : uivertex.position.x) - num) / num2);
					vertexList[i] = uivertex;
				}
			}
			return;
		}
		for (int j = 0; j < count; j++)
		{
			uivertex = vertexList[j];
			if (this.overwriteAllColor || this.CompareCarefully(uivertex.color, this.targetGraphic.color))
			{
				switch (this.gradientDir)
				{
				case GradientDir.Vertical:
					if (j % 2 == 0)
					{
						uivertex.color *= ((j % 3 == 0 || j % 3 == 0) ? this.vertex1 : this.vertex2);
					}
					else
					{
						uivertex.color *= ((j % 3 == 0 || j % 3 == 0) ? this.vertex2 : this.vertex1);
					}
					break;
				case GradientDir.Horizontal:
					uivertex.color *= ((j % 4 == 0 || (j - 3) % 4 == 0) ? this.vertex1 : this.vertex2);
					break;
				case GradientDir.DiagonalLeftToRight:
					uivertex.color *= ((j % 4 == 0) ? this.vertex1 : (((j - 2) % 4 == 0) ? this.vertex2 : Color.Lerp(this.vertex2, this.vertex1, 0.5f)));
					break;
				case GradientDir.DiagonalRightToLeft:
					uivertex.color *= (((j - 1) % 4 == 0) ? this.vertex1 : (((j - 3) % 4 == 0) ? this.vertex2 : Color.Lerp(this.vertex2, this.vertex1, 0.5f)));
					break;
				}
				vertexList[j] = uivertex;
			}
		}
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x0004C6D8 File Offset: 0x0004A8D8
	private bool CompareCarefully(Color col1, Color col2)
	{
		return Mathf.Abs(col1.r - col2.r) < 0.003f && Mathf.Abs(col1.g - col2.g) < 0.003f && Mathf.Abs(col1.b - col2.b) < 0.003f && Mathf.Abs(col1.a - col2.a) < 0.003f;
	}

	// Token: 0x0400066B RID: 1643
	public global::GradientMode gradientMode;

	// Token: 0x0400066C RID: 1644
	public GradientDir gradientDir;

	// Token: 0x0400066D RID: 1645
	public bool overwriteAllColor;

	// Token: 0x0400066E RID: 1646
	public Color vertex1 = Color.white;

	// Token: 0x0400066F RID: 1647
	public Color vertex2 = Color.black;

	// Token: 0x04000670 RID: 1648
	private Graphic targetGraphic;
}
