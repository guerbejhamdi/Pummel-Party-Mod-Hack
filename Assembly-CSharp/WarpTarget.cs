using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002FA RID: 762
public class WarpTarget
{
	// Token: 0x0600152C RID: 5420 RVA: 0x0001024B File Offset: 0x0000E44B
	public WarpTarget(Material origMaterial, Material warpMaterial, bool disabled = false)
	{
		this.origMaterial = origMaterial;
		this.warpMaterial = warpMaterial;
		this.disabled = disabled;
		this.renderers = new List<Renderer>();
	}

	// Token: 0x04001632 RID: 5682
	public Material origMaterial;

	// Token: 0x04001633 RID: 5683
	public Material warpMaterial;

	// Token: 0x04001634 RID: 5684
	public bool disabled;

	// Token: 0x04001635 RID: 5685
	public List<Renderer> renderers;
}
