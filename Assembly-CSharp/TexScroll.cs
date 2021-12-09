using System;
using UnityEngine;

// Token: 0x0200046F RID: 1135
public class TexScroll : MonoBehaviour
{
	// Token: 0x06001EA3 RID: 7843 RVA: 0x000168D2 File Offset: 0x00014AD2
	private void Start()
	{
		this.mat = base.GetComponent<MeshRenderer>().material;
	}

	// Token: 0x06001EA4 RID: 7844 RVA: 0x000168E5 File Offset: 0x00014AE5
	private void Update()
	{
		this.mat.SetTextureOffset("_MainTex", this.scroll_direction * (this.scroll_speed * Time.time));
	}

	// Token: 0x040021BE RID: 8638
	public Vector2 scroll_direction = new Vector2(1f, 1f);

	// Token: 0x040021BF RID: 8639
	public float scroll_speed = 1f;

	// Token: 0x040021C0 RID: 8640
	private Material mat;
}
