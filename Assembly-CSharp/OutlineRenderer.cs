using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

// Token: 0x0200043D RID: 1085
public class OutlineRenderer : MonoBehaviour
{
	// Token: 0x06001DE8 RID: 7656 RVA: 0x000C279C File Offset: 0x000C099C
	public void Awake()
	{
		this.m_camera = base.GetComponent<Camera>();
		if (this.m_camera == null)
		{
			return;
		}
		OutlineSource.OnSourcesChanged.AddListener(new UnityAction(this.RebuildCommandBuffer));
		this.m_outlineFill = new Material(Shader.Find("Outline/OutlineFill"));
		this.m_outlineFillDepth = new Material(Shader.Find("Outline/OutlineFillDepth"));
		this.m_outlinePostSoft = new Material(Shader.Find("Outline/Post Soft Outline"));
		this.m_outlinePostSolid = new Material(Shader.Find("Outline/Post Solid Outline"));
		for (int i = 0; i < this.m_thicknessKeywords.Length; i++)
		{
			if (i == (int)this.m_thickness)
			{
				this.m_outlinePostSoft.EnableKeyword(this.m_thicknessKeywords[i]);
				this.m_outlinePostSolid.EnableKeyword(this.m_thicknessKeywords[i]);
			}
			else
			{
				this.m_outlinePostSoft.DisableKeyword(this.m_thicknessKeywords[i]);
				this.m_outlinePostSolid.DisableKeyword(this.m_thicknessKeywords[i]);
			}
		}
		this.m_outlinePostSoft.SetFloat("_OutlineOpacity", this.m_opacity);
		this.m_outlinePostSolid.SetFloat("_OutlineOpacity", this.m_opacity);
		this.m_cBuffer = new CommandBuffer();
		if (this.m_camera)
		{
			this.m_camera.AddCommandBuffer(this.m_renderOrder, this.m_cBuffer);
		}
		else
		{
			Debug.LogError("Outline Renderer was unable to find a camera on its gameObject.");
		}
		this.m_tempRT = new RenderTexture(this.m_camera.pixelWidth, this.m_camera.pixelHeight, 0, RenderTextureFormat.ARGB32);
		this.m_tempRT.name = "OutlineRenderer";
	}

	// Token: 0x06001DE9 RID: 7657 RVA: 0x000C2934 File Offset: 0x000C0B34
	public void Update()
	{
		if (this.m_camera == null)
		{
			return;
		}
		if (this.m_tempRT.width != this.m_camera.pixelWidth || this.m_tempRT.height != this.m_camera.pixelHeight)
		{
			this.ReleaseRenderTexture();
			this.m_tempRT = new RenderTexture(this.m_camera.pixelWidth, this.m_camera.pixelHeight, 0, RenderTextureFormat.ARGB32);
			this.m_tempRT.name = "OutlineRenderer";
		}
		bool flag = false;
		if (GameManager.DEBUGGING && Input.GetKeyDown(KeyCode.Alpha1))
		{
			if (this.m_renderMode == OutlineRenderMode.Soft)
			{
				this.m_renderMode = OutlineRenderMode.Solid;
			}
			else
			{
				this.m_renderMode = OutlineRenderMode.Soft;
			}
			flag = true;
		}
		if (GameManager.DEBUGGING && Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.m_depthTest = !this.m_depthTest;
			flag = true;
		}
		if (flag)
		{
			this.RebuildCommandBuffer();
		}
	}

	// Token: 0x06001DEA RID: 7658 RVA: 0x00016124 File Offset: 0x00014324
	public void LateUpdate()
	{
		if (OutlineSource.m_sourcesChanged)
		{
			this.RebuildCommandBuffer();
			OutlineSource.m_sourcesChanged = false;
		}
	}

	// Token: 0x06001DEB RID: 7659 RVA: 0x00016139 File Offset: 0x00014339
	public void OnDestroy()
	{
		OutlineSource.OnSourcesChanged.RemoveListener(new UnityAction(this.RebuildCommandBuffer));
		this.ReleaseRenderTexture();
	}

	// Token: 0x06001DEC RID: 7660 RVA: 0x00016157 File Offset: 0x00014357
	private void ReleaseRenderTexture()
	{
		if (this.m_tempRT != null)
		{
			this.m_tempRT.Release();
			UnityEngine.Object.Destroy(this.m_tempRT);
		}
	}

	// Token: 0x06001DED RID: 7661 RVA: 0x000C2A10 File Offset: 0x000C0C10
	private void RebuildCommandBuffer()
	{
		this.m_cBuffer.Clear();
		List<OutlineSource> sources = OutlineSource.GetSources();
		int nameID = Shader.PropertyToID("_OutlineTempTarget");
		this.m_cBuffer.GetTemporaryRT(nameID, -1, -1, 0, FilterMode.Point, RenderTextureFormat.ARGB32);
		this.m_cBuffer.SetRenderTarget(nameID, BuiltinRenderTextureType.CameraTarget);
		this.m_cBuffer.ClearRenderTarget(false, true, new Color(0f, 0f, 0f, 0f));
		foreach (OutlineSource outlineSource in sources)
		{
			if (!(outlineSource.outlineRenderer == null))
			{
				this.m_cBuffer.SetGlobalColor("_OutlineColor", outlineSource.outlineColor);
				this.m_cBuffer.DrawRenderer(outlineSource.outlineRenderer, this.m_depthTest ? this.m_outlineFillDepth : this.m_outlineFill, 0);
			}
		}
		Material mat = (this.m_renderMode == OutlineRenderMode.Soft) ? this.m_outlinePostSoft : this.m_outlinePostSolid;
		this.m_cBuffer.Blit(nameID, BuiltinRenderTextureType.CameraTarget, mat);
		this.m_cBuffer.ReleaseTemporaryRT(nameID);
	}

	// Token: 0x040020C1 RID: 8385
	[SerializeField]
	protected OutlineThickness m_thickness;

	// Token: 0x040020C2 RID: 8386
	[SerializeField]
	protected float m_opacity = 1f;

	// Token: 0x040020C3 RID: 8387
	[SerializeField]
	protected OutlineRenderMode m_renderMode;

	// Token: 0x040020C4 RID: 8388
	[SerializeField]
	protected bool m_depthTest;

	// Token: 0x040020C5 RID: 8389
	[SerializeField]
	protected CameraEvent m_renderOrder = CameraEvent.BeforeImageEffects;

	// Token: 0x040020C6 RID: 8390
	private Camera m_camera;

	// Token: 0x040020C7 RID: 8391
	private CommandBuffer m_cBuffer;

	// Token: 0x040020C8 RID: 8392
	private Material m_outlineFill;

	// Token: 0x040020C9 RID: 8393
	private Material m_outlineFillDepth;

	// Token: 0x040020CA RID: 8394
	private Material m_outlinePostSoft;

	// Token: 0x040020CB RID: 8395
	private Material m_outlinePostSolid;

	// Token: 0x040020CC RID: 8396
	private RenderTexture m_tempRT;

	// Token: 0x040020CD RID: 8397
	private string[] m_thicknessKeywords = new string[]
	{
		"OUTLINE_THICKNESS_LOW",
		"OUTLINE_THICKNESS_MEDIUM",
		"OUTLINE_THICKNESS_HIGH"
	};
}
