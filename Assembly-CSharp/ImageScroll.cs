using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000459 RID: 1113
public class ImageScroll : MonoBehaviour
{
	// Token: 0x06001E78 RID: 7800 RVA: 0x000167A4 File Offset: 0x000149A4
	private void Start()
	{
		this.mat = base.GetComponent<Image>().material;
	}

	// Token: 0x06001E79 RID: 7801 RVA: 0x000C5434 File Offset: 0x000C3634
	private void Update()
	{
		this.mat.SetTextureOffset("_MainTex", this.scroll_direction * (this.scroll_speed * Time.time));
		this.mat.mainTextureOffset = this.scroll_direction * (this.scroll_speed * Time.time);
	}

	// Token: 0x0400215C RID: 8540
	public Vector2 scroll_direction = new Vector2(1f, 1f);

	// Token: 0x0400215D RID: 8541
	public float scroll_speed = 1f;

	// Token: 0x0400215E RID: 8542
	private Material mat;
}
