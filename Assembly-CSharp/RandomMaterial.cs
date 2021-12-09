using System;
using UnityEngine;

// Token: 0x0200030F RID: 783
public class RandomMaterial : MonoBehaviour
{
	// Token: 0x060015A0 RID: 5536 RVA: 0x0009BF4C File Offset: 0x0009A14C
	private void Start()
	{
		Renderer component = base.GetComponent<Renderer>();
		if (component != null && this.materials != null && this.materials.Length != 0)
		{
			component.sharedMaterial = this.materials[UnityEngine.Random.Range(0, this.materials.Length)];
		}
	}

	// Token: 0x0400169F RID: 5791
	public Material[] materials;
}
