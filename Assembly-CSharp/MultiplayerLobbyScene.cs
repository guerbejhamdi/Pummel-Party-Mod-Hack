using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

// Token: 0x02000527 RID: 1319
public class MultiplayerLobbyScene : MonoBehaviour
{
	// Token: 0x060022A7 RID: 8871 RVA: 0x000D37A0 File Offset: 0x000D19A0
	public void Awake()
	{
		GameManager.MultiplayerLobbyScene = this;
		this.SetupRT(ref this.player_rt);
		foreach (RawImage rawImage in this.player_rt_ui)
		{
			if (!(rawImage == null))
			{
				rawImage.texture = this.player_rt;
			}
		}
	}

	// Token: 0x060022A8 RID: 8872 RVA: 0x00019020 File Offset: 0x00017220
	private void Start()
	{
		this.cam.enabled = false;
	}

	// Token: 0x060022A9 RID: 8873 RVA: 0x000D37F0 File Offset: 0x000D19F0
	private void Update()
	{
		if (this.started)
		{
			float a = Mathf.Clamp01((Time.time - this.fadeStartTime) / 0.17f);
			this.lerpColor.a = a;
			foreach (RawImage rawImage in this.player_rt_ui)
			{
				if (!(rawImage == null))
				{
					rawImage.color = this.lerpColor;
				}
			}
		}
	}

	// Token: 0x060022AA RID: 8874 RVA: 0x000D3858 File Offset: 0x000D1A58
	public void Show()
	{
		this.fadeStartTime = Time.time + 0.17f;
		this.cam.enabled = true;
		this.started = true;
		foreach (RawImage rawImage in this.player_rt_ui)
		{
			if (!(rawImage == null))
			{
				rawImage.enabled = true;
			}
		}
	}

	// Token: 0x060022AB RID: 8875 RVA: 0x000D38B4 File Offset: 0x000D1AB4
	public void Hide()
	{
		this.started = false;
		this.cam.enabled = true;
		foreach (RawImage rawImage in this.player_rt_ui)
		{
			if (rawImage != null)
			{
				rawImage.enabled = false;
				rawImage.color = this.empty;
			}
		}
	}

	// Token: 0x060022AC RID: 8876 RVA: 0x0001902E File Offset: 0x0001722E
	public ScoreScreenPlayer GetPlayer(int index)
	{
		return this.result_player_objs[index];
	}

	// Token: 0x060022AD RID: 8877 RVA: 0x00019038 File Offset: 0x00017238
	public IEnumerator Initialize()
	{
		yield return new WaitForEndOfFrame();
		this.fadeStartTime = Time.time + 0.5f;
		this.cam.enabled = true;
		yield break;
	}

	// Token: 0x060022AE RID: 8878 RVA: 0x00019047 File Offset: 0x00017247
	private void SetupRT(ref RenderTexture texture)
	{
		texture = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32);
		texture.filterMode = FilterMode.Bilinear;
		this.cam.targetTexture = texture;
	}

	// Token: 0x060022AF RID: 8879 RVA: 0x000D390C File Offset: 0x000D1B0C
	private void OnResize()
	{
		this.ReleaseRenderTexture();
		this.SetupRT(ref this.player_rt);
		foreach (RawImage rawImage in this.player_rt_ui)
		{
			if (!(rawImage == null))
			{
				rawImage.texture = this.player_rt;
			}
		}
		Debug.Log("resizing RT = " + Screen.width.ToString() + " * " + Screen.height.ToString());
	}

	// Token: 0x060022B0 RID: 8880 RVA: 0x00019071 File Offset: 0x00017271
	private IEnumerator CheckForResize()
	{
		this.lastWidth = Screen.width;
		this.lastHeight = Screen.height;
		for (;;)
		{
			if (this.lastWidth != Screen.width || this.lastHeight != Screen.height)
			{
				this.OnResize();
				this.lastWidth = Screen.width;
				this.lastHeight = Screen.height;
			}
			yield return new WaitForSeconds(0.3f);
		}
		yield break;
	}

	// Token: 0x060022B1 RID: 8881 RVA: 0x00019080 File Offset: 0x00017280
	private void OnDestroy()
	{
		this.ReleaseRenderTexture();
	}

	// Token: 0x060022B2 RID: 8882 RVA: 0x00019088 File Offset: 0x00017288
	private void ReleaseRenderTexture()
	{
		if (this.player_rt != null)
		{
			if (this.cam != null)
			{
				this.cam.targetTexture = null;
			}
			this.player_rt.Release();
			UnityEngine.Object.Destroy(this.player_rt);
		}
	}

	// Token: 0x04002589 RID: 9609
	public Camera cam;

	// Token: 0x0400258A RID: 9610
	public ScoreScreenPlayer[] result_player_objs;

	// Token: 0x0400258B RID: 9611
	public RawImage[] player_rt_ui;

	// Token: 0x0400258C RID: 9612
	public bool forceResolution;

	// Token: 0x0400258D RID: 9613
	public float width = 1920f;

	// Token: 0x0400258E RID: 9614
	public float height = 1080f;

	// Token: 0x0400258F RID: 9615
	public Light directionalLight;

	// Token: 0x04002590 RID: 9616
	public PostProcessLayer postProcessLayer;

	// Token: 0x04002591 RID: 9617
	private RenderTexture player_rt;

	// Token: 0x04002592 RID: 9618
	private bool started;

	// Token: 0x04002593 RID: 9619
	private float fadeStartTime;

	// Token: 0x04002594 RID: 9620
	private int lastWidth;

	// Token: 0x04002595 RID: 9621
	private int lastHeight;

	// Token: 0x04002596 RID: 9622
	private Color lerpColor = new Color(1f, 1f, 1f, 0f);

	// Token: 0x04002597 RID: 9623
	private Color empty = new Color(1f, 1f, 1f, 0f);
}
