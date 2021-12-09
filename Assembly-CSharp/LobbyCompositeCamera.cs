using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x0200043A RID: 1082
public class LobbyCompositeCamera : PostEffectsBase
{
	// Token: 0x06001DE5 RID: 7653 RVA: 0x000160D0 File Offset: 0x000142D0
	public override bool CheckResources()
	{
		base.CheckSupport(false);
		this.composite_mat = base.CheckShaderAndCreateMaterial(this.composite_shader, this.composite_mat);
		if (!this.isSupported)
		{
			base.ReportAutoDisable();
		}
		return this.isSupported;
	}

	// Token: 0x06001DE6 RID: 7654 RVA: 0x000C26D0 File Offset: 0x000C08D0
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.CheckResources() || this.target_tex == null)
		{
			Graphics.Blit(source, destination);
			return;
		}
		this.zoom_time += Time.deltaTime;
		this.composite_mat.SetTexture("_TargetTex", this.target_tex);
		this.composite_mat.SetTexture("_ColorBuffer", this.source_tex);
		this.composite_mat.SetFloat("_Brightness", this.brightness);
		this.composite_mat.SetFloat("_Zoom", this.zoom_curve.Evaluate(this.zoom_time * 0.1f));
		this.composite_mat.SetFloat("_Fade", this.fade);
		Graphics.Blit(source, destination, this.composite_mat, 0);
	}

	// Token: 0x040020B2 RID: 8370
	public RenderTexture target_tex;

	// Token: 0x040020B3 RID: 8371
	public RenderTexture source_tex;

	// Token: 0x040020B4 RID: 8372
	public Shader composite_shader;

	// Token: 0x040020B5 RID: 8373
	public AnimationCurve zoom_curve;

	// Token: 0x040020B6 RID: 8374
	public float zoom_time;

	// Token: 0x040020B7 RID: 8375
	public float brightness = 1f;

	// Token: 0x040020B8 RID: 8376
	public float fade = 1f;

	// Token: 0x040020B9 RID: 8377
	private Material composite_mat;
}
