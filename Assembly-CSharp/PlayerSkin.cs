using System;
using UnityEngine;

// Token: 0x02000030 RID: 48
[Serializable]
public struct PlayerSkin
{
	// Token: 0x04000123 RID: 291
	public string skinName;

	// Token: 0x04000124 RID: 292
	public string skinNameToken;

	// Token: 0x04000125 RID: 293
	public Mesh mesh;

	// Token: 0x04000126 RID: 294
	public Mesh editedMesh;

	// Token: 0x04000127 RID: 295
	public int[] replaceColorID1;

	// Token: 0x04000128 RID: 296
	public int[] replaceColorID2;

	// Token: 0x04000129 RID: 297
	public int[] replaceColorID3;

	// Token: 0x0400012A RID: 298
	public Material mat;

	// Token: 0x0400012B RID: 299
	public SkinAccessory[] accessories;
}
