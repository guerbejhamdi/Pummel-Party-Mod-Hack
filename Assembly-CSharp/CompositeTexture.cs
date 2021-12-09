using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityStandardAssets.ImageEffects;

// Token: 0x02000439 RID: 1081
public class CompositeTexture : PostEffectsBase
{
	// Token: 0x06001DE1 RID: 7649 RVA: 0x000C25F8 File Offset: 0x000C07F8
	public void Awake()
	{
		this.CheckResources();
		CommandBuffer commandBuffer = new CommandBuffer();
		commandBuffer.name = "CompositeResultScreen";
		commandBuffer.Blit(this.resultScreen.screenshot, BuiltinRenderTextureType.CurrentActive, this.composite_mat);
		base.GetComponent<Camera>().AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, commandBuffer);
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x000C2648 File Offset: 0x000C0848
	public void Update()
	{
		Shader.SetGlobalTexture("_ResultScreenshot", this.resultScreen.screenshot);
		Shader.SetGlobalTexture("_ResultTargetTex", this.target_tex);
		this.composite_mat.SetFloat("_Brightness", this.brightness);
		this.composite_mat.SetFloat("_Zoom", this.zoom_curve.Evaluate(this.zoom_time * 0.1f));
		this.composite_mat.SetFloat("_Fade", this.fade);
	}

	// Token: 0x06001DE3 RID: 7651 RVA: 0x0001607C File Offset: 0x0001427C
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

	// Token: 0x040020AA RID: 8362
	public ResultSceenScene resultScreen;

	// Token: 0x040020AB RID: 8363
	public RenderTexture target_tex;

	// Token: 0x040020AC RID: 8364
	public Shader composite_shader;

	// Token: 0x040020AD RID: 8365
	public AnimationCurve zoom_curve;

	// Token: 0x040020AE RID: 8366
	public float zoom_time;

	// Token: 0x040020AF RID: 8367
	public float brightness = 1f;

	// Token: 0x040020B0 RID: 8368
	public float fade = 1f;

	// Token: 0x040020B1 RID: 8369
	private Material composite_mat;
}
