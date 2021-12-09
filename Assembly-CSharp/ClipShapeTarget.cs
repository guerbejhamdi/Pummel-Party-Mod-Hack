using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004CD RID: 1229
public class ClipShapeTarget
{
	// Token: 0x0600209D RID: 8349 RVA: 0x00017B83 File Offset: 0x00015D83
	public ClipShapeTarget(Material origMaterial, Material clipMaterial)
	{
		this.origMaterial = origMaterial;
		this.clipMaterial = clipMaterial;
		this.renderers = new List<Renderer>();
	}

	// Token: 0x04002365 RID: 9061
	public Material origMaterial;

	// Token: 0x04002366 RID: 9062
	public Material clipMaterial;

	// Token: 0x04002367 RID: 9063
	public List<Renderer> renderers;
}
