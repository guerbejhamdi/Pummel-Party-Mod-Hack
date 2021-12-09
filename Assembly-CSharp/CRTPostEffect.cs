using System;
using UnityEngine;

// Token: 0x02000140 RID: 320
public class CRTPostEffect : MonoBehaviour
{
	// Token: 0x06000935 RID: 2357 RVA: 0x0000A2DC File Offset: 0x000084DC
	private void Start()
	{
		this.m_effectMat = new Material(Shader.Find("Post/CRTPostEffect"));
		Shader.SetGlobalTexture("_CRTVignette", this.m_vignette);
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0000A303 File Offset: 0x00008503
	public void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, this.m_effectMat);
	}

	// Token: 0x040007BB RID: 1979
	[SerializeField]
	private Texture2D m_vignette;

	// Token: 0x040007BC RID: 1980
	private Camera m_cam;

	// Token: 0x040007BD RID: 1981
	private Material m_effectMat;
}
