using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002C1 RID: 705
public class TextureCombiner : ScriptableObject
{
	// Token: 0x04001599 RID: 5529
	public Material masterMat;

	// Token: 0x0400159A RID: 5530
	public List<Texture2D> albedoTextures;

	// Token: 0x0400159B RID: 5531
	public List<Texture2D> emissionTextures;

	// Token: 0x0400159C RID: 5532
	public int size = 4096;

	// Token: 0x0400159D RID: 5533
	public string textureName = "Default";

	// Token: 0x0400159E RID: 5534
	public Rect[] albedoRects;

	// Token: 0x0400159F RID: 5535
	public List<Mesh> meshPairs = new List<Mesh>();

	// Token: 0x040015A0 RID: 5536
	public List<Texture2D> texturePairs = new List<Texture2D>();

	// Token: 0x040015A1 RID: 5537
	public List<Mesh> editedMeshPairs = new List<Mesh>();
}
