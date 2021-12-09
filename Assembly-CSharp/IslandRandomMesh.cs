using System;
using UnityEngine;

// Token: 0x020000BC RID: 188
[RequireComponent(typeof(MeshFilter))]
public class IslandRandomMesh : IslandRandomBase
{
	// Token: 0x060003DA RID: 986 RVA: 0x00006201 File Offset: 0x00004401
	public override void DoRandom(int index)
	{
		this.meshFilter.mesh = this.meshes[index];
	}

	// Token: 0x0400041C RID: 1052
	public MeshFilter meshFilter;

	// Token: 0x0400041D RID: 1053
	public Mesh[] meshes;
}
