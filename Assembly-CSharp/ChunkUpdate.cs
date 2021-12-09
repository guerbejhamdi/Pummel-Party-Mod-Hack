using System;
using UnityEngine;

// Token: 0x0200057A RID: 1402
public struct ChunkUpdate
{
	// Token: 0x060024A7 RID: 9383 RVA: 0x0001A4FE File Offset: 0x000186FE
	public ChunkUpdate(Chunk chunk, Vector3[] vertices, Vector3[] normals, int[] indices, Color32[] colors)
	{
		this.chunk = chunk;
		this.vertices = vertices;
		this.normals = normals;
		this.indices = indices;
		this.colors = colors;
		this.colliderSet = false;
	}

	// Token: 0x04002814 RID: 10260
	public Chunk chunk;

	// Token: 0x04002815 RID: 10261
	public Vector3[] vertices;

	// Token: 0x04002816 RID: 10262
	public Vector3[] normals;

	// Token: 0x04002817 RID: 10263
	public int[] indices;

	// Token: 0x04002818 RID: 10264
	public Color32[] colors;

	// Token: 0x04002819 RID: 10265
	public bool colliderSet;
}
