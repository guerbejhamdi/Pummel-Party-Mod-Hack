using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x0200032E RID: 814
public class ScoreUIScene : MonoBehaviour
{
	// Token: 0x06001624 RID: 5668 RVA: 0x0009E718 File Offset: 0x0009C918
	public void Setup()
	{
		for (int i = 0; i < this.playerObjects.Length; i++)
		{
			if (i < GameManager.GetPlayerCount())
			{
				this.playerAnimations[i].SetPlayer(i);
			}
			this.playerObjects[i].SetActive(false);
		}
	}

	// Token: 0x06001625 RID: 5669 RVA: 0x0009E75C File Offset: 0x0009C95C
	public void Update()
	{
		if (this.active && this.fps != 0f)
		{
			this.elapsed += Time.deltaTime;
			if (this.elapsed > 1f / this.fps)
			{
				this.elapsed -= 1f / this.fps;
				this.cam.Render();
			}
		}
	}

	// Token: 0x06001626 RID: 5670 RVA: 0x0009E7C8 File Offset: 0x0009C9C8
	public void State(bool enabled)
	{
		this.active = enabled;
		this.elapsed = 0f;
		for (int i = 0; i < 4; i++)
		{
			this.playerObjects[i].SetActive(enabled);
		}
		if (this.active)
		{
			this.postProcessLayer.enabled = true;
			this.postProcessVolume.enabled = true;
			this.cam.targetTexture = this.rt1;
			this.cam.Render();
		}
		if (this.fps == 0f)
		{
			for (int j = 0; j < 4; j++)
			{
				this.playerObjects[j].SetActive(false);
			}
		}
		for (int k = 4; k < 8; k++)
		{
			this.playerObjects[k].SetActive(enabled);
		}
		if (this.active)
		{
			this.cam.targetTexture = this.rt2;
			this.cam.Render();
			this.postProcessLayer.enabled = false;
			this.postProcessVolume.enabled = false;
		}
		if (this.fps == 0f)
		{
			for (int l = 4; l < 8; l++)
			{
				this.playerObjects[l].SetActive(false);
			}
		}
	}

	// Token: 0x04001760 RID: 5984
	public RenderTexture rt1;

	// Token: 0x04001761 RID: 5985
	public RenderTexture rt2;

	// Token: 0x04001762 RID: 5986
	public PlayerAnimation[] playerAnimations;

	// Token: 0x04001763 RID: 5987
	public GameObject[] playerObjects;

	// Token: 0x04001764 RID: 5988
	public Camera cam;

	// Token: 0x04001765 RID: 5989
	public float fps = 30f;

	// Token: 0x04001766 RID: 5990
	public PostProcessLayer postProcessLayer;

	// Token: 0x04001767 RID: 5991
	public PostProcessVolume postProcessVolume;

	// Token: 0x04001768 RID: 5992
	public bool active;

	// Token: 0x04001769 RID: 5993
	private float elapsed;
}
