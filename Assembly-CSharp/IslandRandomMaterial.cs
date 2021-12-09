using System;
using UnityEngine;

// Token: 0x020000BB RID: 187
public class IslandRandomMaterial : IslandRandomBase
{
	// Token: 0x060003D8 RID: 984 RVA: 0x0003B6A8 File Offset: 0x000398A8
	public override void DoRandom(int index)
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].sharedMaterial = this.materials[index];
		}
	}

	// Token: 0x0400041A RID: 1050
	public Material[] materials;

	// Token: 0x0400041B RID: 1051
	public MeshRenderer[] renderers;
}
