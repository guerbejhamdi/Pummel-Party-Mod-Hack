using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class ClothAttachment
{
	// Token: 0x060000CF RID: 207 RVA: 0x000041B3 File Offset: 0x000023B3
	public ClothAttachment(GameObject obj, int vertexIndex)
	{
		this.vertexIndex = vertexIndex;
		this.obj = obj;
	}

	// Token: 0x04000104 RID: 260
	public int vertexIndex;

	// Token: 0x04000105 RID: 261
	public GameObject obj;
}
