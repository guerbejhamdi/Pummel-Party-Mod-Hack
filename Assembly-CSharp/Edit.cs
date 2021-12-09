using System;
using UnityEngine;

// Token: 0x02000588 RID: 1416
public struct Edit
{
	// Token: 0x060024DD RID: 9437 RVA: 0x0001A73F File Offset: 0x0001893F
	public Edit(BrushShape shape, BrushAction action, Vector3 pos, float size, float opacity)
	{
		this.shape = shape;
		this.action = action;
		this.pos = pos;
		this.size = size;
		this.opacity = opacity;
	}

	// Token: 0x060024DE RID: 9438 RVA: 0x0001A766 File Offset: 0x00018966
	public Edit(BrushShape shape, BrushAction action, Vector3 pos, float size)
	{
		this.shape = shape;
		this.action = action;
		this.pos = pos;
		this.size = size;
		this.opacity = 1f;
	}

	// Token: 0x0400286F RID: 10351
	public BrushShape shape;

	// Token: 0x04002870 RID: 10352
	public BrushAction action;

	// Token: 0x04002871 RID: 10353
	public Vector3 pos;

	// Token: 0x04002872 RID: 10354
	public float size;

	// Token: 0x04002873 RID: 10355
	public float opacity;
}
