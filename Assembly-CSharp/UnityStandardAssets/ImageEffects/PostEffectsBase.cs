using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200063A RID: 1594
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class PostEffectsBase : MonoBehaviour
	{
		// Token: 0x060029E9 RID: 10729 RVA: 0x000F6B00 File Offset: 0x000F4D00
		protected Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
		{
			if (!s)
			{
				Debug.Log("Missing shader in " + this.ToString());
				base.enabled = false;
				return null;
			}
			if (s.isSupported && m2Create && m2Create.shader == s)
			{
				return m2Create;
			}
			if (!s.isSupported)
			{
				this.NotSupported();
				Debug.Log(string.Concat(new string[]
				{
					"The shader ",
					s.ToString(),
					" on effect ",
					this.ToString(),
					" is not supported on this platform!"
				}));
				return null;
			}
			m2Create = new Material(s);
			m2Create.hideFlags = HideFlags.DontSave;
			if (m2Create)
			{
				return m2Create;
			}
			return null;
		}

		// Token: 0x060029EA RID: 10730 RVA: 0x000F6BB8 File Offset: 0x000F4DB8
		protected Material CreateMaterial(Shader s, Material m2Create)
		{
			if (!s)
			{
				Debug.Log("Missing shader in " + this.ToString());
				return null;
			}
			if (m2Create && m2Create.shader == s && s.isSupported)
			{
				return m2Create;
			}
			if (!s.isSupported)
			{
				return null;
			}
			m2Create = new Material(s);
			m2Create.hideFlags = HideFlags.DontSave;
			if (m2Create)
			{
				return m2Create;
			}
			return null;
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x0001D626 File Offset: 0x0001B826
		private void OnEnable()
		{
			this.isSupported = true;
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x0001D62F File Offset: 0x0001B82F
		protected bool CheckSupport()
		{
			return this.CheckSupport(false);
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x0001D638 File Offset: 0x0001B838
		public virtual bool CheckResources()
		{
			Debug.LogWarning("CheckResources () for " + this.ToString() + " should be overwritten.");
			return this.isSupported;
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x0001D65A File Offset: 0x0001B85A
		protected void Start()
		{
			this.CheckResources();
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x000F6C2C File Offset: 0x000F4E2C
		protected bool CheckSupport(bool needDepth)
		{
			this.isSupported = true;
			this.supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
			this.supportDX11 = (SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders);
			if (!SystemInfo.supportsImageEffects)
			{
				this.NotSupported();
				return false;
			}
			if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				this.NotSupported();
				return false;
			}
			if (needDepth)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			return true;
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x0001D663 File Offset: 0x0001B863
		protected bool CheckSupport(bool needDepth, bool needHdr)
		{
			if (!this.CheckSupport(needDepth))
			{
				return false;
			}
			if (needHdr && !this.supportHDRTextures)
			{
				this.NotSupported();
				return false;
			}
			return true;
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x0001D684 File Offset: 0x0001B884
		public bool Dx11Support()
		{
			return this.supportDX11;
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x0001D68C File Offset: 0x0001B88C
		protected void ReportAutoDisable()
		{
			Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x000F6C9C File Offset: 0x000F4E9C
		private bool CheckShader(Shader s)
		{
			Debug.Log(string.Concat(new string[]
			{
				"The shader ",
				s.ToString(),
				" on effect ",
				this.ToString(),
				" is not part of the Unity 3.2+ effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package."
			}));
			if (!s.isSupported)
			{
				this.NotSupported();
				return false;
			}
			return false;
		}

		// Token: 0x060029F4 RID: 10740 RVA: 0x0001D6A8 File Offset: 0x0001B8A8
		protected void NotSupported()
		{
			base.enabled = false;
			this.isSupported = false;
		}

		// Token: 0x060029F5 RID: 10741 RVA: 0x000F6CF4 File Offset: 0x000F4EF4
		protected void DrawBorder(RenderTexture dest, Material material)
		{
			RenderTexture.active = dest;
			bool flag = true;
			GL.PushMatrix();
			GL.LoadOrtho();
			for (int i = 0; i < material.passCount; i++)
			{
				material.SetPass(i);
				float y;
				float y2;
				if (flag)
				{
					y = 1f;
					y2 = 0f;
				}
				else
				{
					y = 0f;
					y2 = 1f;
				}
				float x = 0f;
				float x2 = 0f + 1f / ((float)dest.width * 1f);
				float y3 = 0f;
				float y4 = 1f;
				GL.Begin(7);
				GL.TexCoord2(0f, y);
				GL.Vertex3(x, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x, y4, 0.1f);
				float x3 = 1f - 1f / ((float)dest.width * 1f);
				x2 = 1f;
				y3 = 0f;
				y4 = 1f;
				GL.TexCoord2(0f, y);
				GL.Vertex3(x3, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x3, y4, 0.1f);
				float x4 = 0f;
				x2 = 1f;
				y3 = 0f;
				y4 = 0f + 1f / ((float)dest.height * 1f);
				GL.TexCoord2(0f, y);
				GL.Vertex3(x4, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x4, y4, 0.1f);
				float x5 = 0f;
				x2 = 1f;
				y3 = 1f - 1f / ((float)dest.height * 1f);
				y4 = 1f;
				GL.TexCoord2(0f, y);
				GL.Vertex3(x5, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x5, y4, 0.1f);
				GL.End();
			}
			GL.PopMatrix();
		}

		// Token: 0x04002C48 RID: 11336
		protected bool supportHDRTextures = true;

		// Token: 0x04002C49 RID: 11337
		protected bool supportDX11;

		// Token: 0x04002C4A RID: 11338
		protected bool isSupported = true;
	}
}
