using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020001A8 RID: 424
public class TerritorySystem : MonoBehaviour
{
	// Token: 0x06000C1D RID: 3101 RVA: 0x000659EC File Offset: 0x00063BEC
	private void Awake()
	{
		this.m_bufferTexture = new RenderTexture(this.m_bufferSize, this.m_bufferSize, 0, RenderTextureFormat.ARGB32);
		this.m_bufferTexture.name = "TerritorySystem Render Texture";
		Shader.SetGlobalTexture("_TerritoryBuffer", this.m_bufferTexture);
		this.m_splatCommandBuffer = new CommandBuffer();
		this.m_splatCommandBuffer.name = "SplatCommandBuffer";
		this.m_camera.targetTexture = this.m_bufferTexture;
		this.m_camera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, this.m_splatCommandBuffer);
		this.m_camera.nearClipPlane = -10f;
		this.m_camera.farClipPlane = 10f;
		this.m_camera.orthographicSize = this.m_areaSize;
		this.m_remap = new Texture2D(256, 256, TextureFormat.RGBA32, false);
		this.m_remap.wrapMode = TextureWrapMode.Clamp;
		for (int i = 0; i < this.m_remap.width; i++)
		{
			float num = (float)i / (float)this.m_remap.width;
			for (int j = 0; j < this.m_remap.height; j++)
			{
				int num2 = (int)((float)j / (float)this.m_remap.width * 4f);
				Color color = this.m_elementGradients[num2].Evaluate((float)i / (float)this.m_remap.width);
				this.m_remap.SetPixel(i, j, color);
			}
		}
		this.m_remap.Apply();
		this.m_remapEmission = new Texture2D(256, 256, TextureFormat.RGBA32, false);
		this.m_remapEmission.wrapMode = TextureWrapMode.Clamp;
		for (int k = 0; k < this.m_remapEmission.width; k++)
		{
			float num3 = (float)k / (float)this.m_remapEmission.width;
			for (int l = 0; l < this.m_remapEmission.height; l++)
			{
				int num4 = (int)((float)l / (float)this.m_remapEmission.width * 4f);
				Color color2 = this.m_elementEmissionGradients[num4].Evaluate((float)k / (float)this.m_remapEmission.width);
				this.m_remapEmission.SetPixel(k, l, color2);
			}
		}
		this.m_remapEmission.Apply();
		Shader.SetGlobalTexture("_TerritoryRemap", this.m_remap);
		Shader.SetGlobalTexture("_TerritoryEmissionRemap", this.m_remapEmission);
		Shader.SetGlobalFloat("_TerritorySize", this.m_areaSize);
		this.m_teamColors = new Color[4];
		this.m_teamColors[0] = new Color(1f, 0f, 0f, 0f);
		this.m_teamColors[1] = new Color(0f, 1f, 0f, 0f);
		this.m_teamColors[2] = new Color(0f, 0f, 1f, 0f);
		this.m_teamColors[3] = new Color(0f, 0f, 0f, 1f);
		this.m_scoreDownsampleSteps = (int)Mathf.Floor(Mathf.Log((float)this.m_bufferSize) / Mathf.Log(2f)) - 2;
		this.m_downsampleBuffers = new RenderTexture[this.m_scoreDownsampleSteps];
		this.m_scoreTexture = new Texture2D(4, 4, TextureFormat.ARGB32, false);
		int num5 = this.m_bufferSize / 2;
		for (int m = this.m_scoreDownsampleSteps - 1; m >= 0; m--)
		{
			this.m_downsampleBuffers[m] = new RenderTexture(num5, num5, 0, RenderTextureFormat.ARGB32);
			this.m_downsampleBuffers[m].name = "TerritorySystemDownSampleBuffer" + m.ToString();
			if (m == 0)
			{
				this.m_downsampleBuffers[m].filterMode = FilterMode.Point;
			}
			num5 /= 2;
		}
		this.m_curDownsampleStep = this.m_scoreDownsampleSteps;
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00065D9C File Offset: 0x00063F9C
	private void LateUpdate()
	{
		if (!this.m_bufferCleared)
		{
			this.m_splatCommandBuffer.Clear();
			this.m_splatCommandBuffer.ClearRenderTarget(true, true, new Color(0f, 0f, 0f, 0f));
			this.m_camera.Render();
			this.m_bufferCleared = true;
		}
		this.m_splatCommandBuffer.Clear();
		foreach (TerritorySystem.TempSplatInfo tempSplatInfo in this.m_queueSplats)
		{
			Matrix4x4 matrix = Matrix4x4.TRS(tempSplatInfo.position, tempSplatInfo.rot, tempSplatInfo.scale);
			Matrix4x4 matrix2 = Matrix4x4.TRS(tempSplatInfo.position, tempSplatInfo.rot, tempSplatInfo.scale);
			this.m_splatCommandBuffer.SetGlobalColor("_TerritoryColor", this.m_teamColors[tempSplatInfo.team]);
			this.m_splatCommandBuffer.DrawMesh(this.m_brushes[(int)tempSplatInfo.brush].mesh, matrix2, this.m_brushes[(int)tempSplatInfo.brush].subMat);
			this.m_splatCommandBuffer.DrawMesh(this.m_brushes[(int)tempSplatInfo.brush].mesh, matrix, this.m_brushes[(int)tempSplatInfo.brush].mat);
		}
		if (this.m_calculateScore)
		{
			if (this.m_curDownsampleStep > 0)
			{
				if (this.m_curDownsampleStep == this.m_scoreDownsampleSteps)
				{
					this.m_splatCommandBuffer.Blit(this.m_bufferTexture, this.m_downsampleBuffers[this.m_curDownsampleStep - 1]);
				}
				else
				{
					this.m_splatCommandBuffer.Blit(this.m_downsampleBuffers[this.m_curDownsampleStep], this.m_downsampleBuffers[this.m_curDownsampleStep - 1]);
				}
				if (this.m_curDownsampleStep > 0)
				{
					this.m_curDownsampleStep--;
				}
			}
			else
			{
				this.m_curDownsampleStep = this.m_scoreDownsampleSteps;
			}
		}
		this.m_camera.Render();
		this.m_queueSplats.Clear();
		if (this.m_calculateScore && this.m_curDownsampleStep == 0)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = this.m_downsampleBuffers[0];
			this.m_scoreTexture.ReadPixels(new Rect(0f, 0f, 4f, 4f), 0, 0);
			this.m_scoreTexture.Apply();
			RenderTexture.active = active;
			float[] array = new float[4];
			float num = 0f;
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					Color pixel = this.m_scoreTexture.GetPixel(i, j);
					array[0] += pixel[0];
					array[1] += pixel[1];
					array[2] += pixel[2];
					array[3] += pixel[3];
					num += pixel[0] + pixel[1] + pixel[2] + pixel[3];
				}
			}
			for (int k = 0; k < 4; k++)
			{
				this.m_teamScores[k] = ((num == 0f || array[k] == 0f) ? 0f : (array[k] / num * 100f));
			}
		}
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x0000B9C1 File Offset: 0x00009BC1
	public void ApplySplat(Vector3 position, Quaternion rot, Vector3 scale, int team, byte brush)
	{
		this.m_queueSplats.Add(new TerritorySystem.TempSplatInfo(new Vector3(position.x, position.z, position.y), rot, scale, team, brush));
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x0000B9F0 File Offset: 0x00009BF0
	public void SetCalculateScore(bool enabled)
	{
		this.m_calculateScore = enabled;
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x0000B9F9 File Offset: 0x00009BF9
	public float GetTeamScore(int teamIndex)
	{
		if (teamIndex < 0 || teamIndex >= this.m_teamScores.Length)
		{
			return 0f;
		}
		return this.m_teamScores[teamIndex];
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x0006610C File Offset: 0x0006430C
	public void OnDestroy()
	{
		this.m_bufferTexture.Release();
		UnityEngine.Object.Destroy(this.m_bufferTexture);
		foreach (RenderTexture renderTexture in this.m_downsampleBuffers)
		{
			renderTexture.Release();
			UnityEngine.Object.Destroy(renderTexture);
		}
		UnityEngine.Object.Destroy(this.m_remap);
		UnityEngine.Object.Destroy(this.m_remapEmission);
		UnityEngine.Object.Destroy(this.m_scoreTexture);
	}

	// Token: 0x04000B54 RID: 2900
	[Header("Settings")]
	[SerializeField]
	private float m_areaSize = 5f;

	// Token: 0x04000B55 RID: 2901
	[SerializeField]
	private Material m_splatMaterial;

	// Token: 0x04000B56 RID: 2902
	[SerializeField]
	private Material m_subSplatMaterial;

	// Token: 0x04000B57 RID: 2903
	[SerializeField]
	private Mesh m_splatMesh;

	// Token: 0x04000B58 RID: 2904
	[SerializeField]
	private TerritorySystem.TerritoryBrush[] m_brushes;

	// Token: 0x04000B59 RID: 2905
	[SerializeField]
	private int m_bufferSize = 1024;

	// Token: 0x04000B5A RID: 2906
	[SerializeField]
	private Gradient m_remapGradient;

	// Token: 0x04000B5B RID: 2907
	[SerializeField]
	private Gradient[] m_elementGradients;

	// Token: 0x04000B5C RID: 2908
	[SerializeField]
	private Gradient[] m_elementEmissionGradients;

	// Token: 0x04000B5D RID: 2909
	[Header("References")]
	[SerializeField]
	private Camera m_camera;

	// Token: 0x04000B5E RID: 2910
	private RenderTexture m_bufferTexture;

	// Token: 0x04000B5F RID: 2911
	private CommandBuffer m_splatCommandBuffer;

	// Token: 0x04000B60 RID: 2912
	private Texture2D m_remap;

	// Token: 0x04000B61 RID: 2913
	private Texture2D m_remapEmission;

	// Token: 0x04000B62 RID: 2914
	private Color[] m_teamColors;

	// Token: 0x04000B63 RID: 2915
	private bool m_calculateScore = true;

	// Token: 0x04000B64 RID: 2916
	private int m_scoreDownsampleSteps;

	// Token: 0x04000B65 RID: 2917
	private int m_curDownsampleStep;

	// Token: 0x04000B66 RID: 2918
	private RenderTexture[] m_downsampleBuffers;

	// Token: 0x04000B67 RID: 2919
	private Texture2D m_scoreTexture;

	// Token: 0x04000B68 RID: 2920
	private float[] m_teamScores = new float[4];

	// Token: 0x04000B69 RID: 2921
	private List<TerritorySystem.TempSplatInfo> m_queueSplats = new List<TerritorySystem.TempSplatInfo>();

	// Token: 0x04000B6A RID: 2922
	private bool m_bufferCleared;

	// Token: 0x020001A9 RID: 425
	[Serializable]
	private class TerritoryBrush
	{
		// Token: 0x06000C24 RID: 3108 RVA: 0x0000BA54 File Offset: 0x00009C54
		public TerritoryBrush(Mesh mesh, Material mat, Material subMat)
		{
			this.mesh = mesh;
			this.mat = mat;
			this.subMat = subMat;
		}

		// Token: 0x04000B6B RID: 2923
		public Mesh mesh;

		// Token: 0x04000B6C RID: 2924
		public Material mat;

		// Token: 0x04000B6D RID: 2925
		public Material subMat;
	}

	// Token: 0x020001AA RID: 426
	private class TempSplatInfo
	{
		// Token: 0x06000C25 RID: 3109 RVA: 0x0000BA71 File Offset: 0x00009C71
		public TempSplatInfo(Vector3 p, Quaternion r, Vector3 s, int t, byte b)
		{
			this.position = p;
			this.rot = r;
			this.scale = s;
			this.team = t;
			this.brush = b;
		}

		// Token: 0x04000B6E RID: 2926
		public Vector3 position;

		// Token: 0x04000B6F RID: 2927
		public Quaternion rot;

		// Token: 0x04000B70 RID: 2928
		public Vector3 scale;

		// Token: 0x04000B71 RID: 2929
		public byte brush;

		// Token: 0x04000B72 RID: 2930
		public int team;
	}
}
