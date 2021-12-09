using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace LlockhamIndustries.Decals
{
	// Token: 0x0200086D RID: 2157
	internal class CameraData
	{
		// Token: 0x06003D42 RID: 15682 RVA: 0x00028F88 File Offset: 0x00027188
		public void Initialize(Camera Camera, DynamicDecals System)
		{
			this.initialized = true;
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x00028F91 File Offset: 0x00027191
		public void Terminate(Camera Camera)
		{
			this.RestoreDepthTextureMode(Camera);
			this.ReleaseTextures();
			this.initialized = false;
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x00028FA7 File Offset: 0x000271A7
		public void Update(Camera Camera, DynamicDecals System)
		{
			this.UpdateRenderingMethod(Camera, System);
			this.UpdateRenderTextures(Camera, System, false);
			this.UpdateShaderReplacement(Camera, System);
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x00131420 File Offset: 0x0012F620
		public void AssignGlobalProperties(Camera Camera)
		{
			if (this.replacement != ShaderReplacement.Null)
			{
				switch (this.replacement)
				{
				case ShaderReplacement.SingleTarget:
					this.depthBuffer.SetGlobalShaderProperty("_CustomDepthNormalMaskTexture");
					Shader.DisableKeyword("_PrecisionDepthNormals");
					Shader.DisableKeyword("_CustomDepthNormals");
					Shader.EnableKeyword("_PackedDepthNormals");
					break;
				case ShaderReplacement.SinglePass:
					this.depthBuffer.SetGlobalShaderProperty("_CustomDepthTexture");
					this.normalBuffer.SetGlobalShaderProperty("_CustomNormalTexture");
					this.maskBuffer.SetGlobalShaderProperty("_MaskBuffer_0");
					Shader.DisableKeyword("_PrecisionDepthNormals");
					Shader.EnableKeyword("_CustomDepthNormals");
					Shader.DisableKeyword("_PackedDepthNormals");
					return;
				case ShaderReplacement.DoublePass:
				case ShaderReplacement.TriplePass:
					if (Camera.actualRenderingPath == RenderingPath.DeferredShading)
					{
						this.depthBuffer.SetGlobalShaderProperty("_CustomDepthTexture");
						this.normalBuffer.SetGlobalShaderProperty("_CustomNormalTexture");
						Shader.DisableKeyword("_PrecisionDepthNormals");
						Shader.EnableKeyword("_CustomDepthNormals");
						Shader.DisableKeyword("_PackedDepthNormals");
					}
					else
					{
						this.normalBuffer.SetGlobalShaderProperty("_CustomNormalTexture");
						Shader.EnableKeyword("_PrecisionDepthNormals");
						Shader.DisableKeyword("_CustomDepthNormals");
						Shader.DisableKeyword("_PackedDepthNormals");
					}
					this.maskBuffer.SetGlobalShaderProperty("_MaskBuffer_0");
					return;
				case ShaderReplacement.Classic:
					this.maskBuffer.SetGlobalShaderProperty("_MaskBuffer_0");
					Shader.DisableKeyword("_PrecisionDepthNormals");
					Shader.DisableKeyword("_CustomDepthNormals");
					Shader.DisableKeyword("_PackedDepthNormals");
					return;
				case ShaderReplacement.Stencil:
					this.maskBuffer.SetGlobalShaderProperty("_MaskBuffer_0");
					Shader.DisableKeyword("_PrecisionDepthNormals");
					Shader.EnableKeyword("_CustomDepthNormals");
					Shader.DisableKeyword("_PackedDepthNormals");
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x00028FC2 File Offset: 0x000271C2
		private ShaderReplacement Standard(bool VR)
		{
			if (VR)
			{
				return ShaderReplacement.TriplePass;
			}
			if (SystemInfo.supportedRenderTargetCount < 3)
			{
				return ShaderReplacement.DoublePass;
			}
			return ShaderReplacement.SinglePass;
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x00028FD4 File Offset: 0x000271D4
		private bool VRCamera(Camera Source)
		{
			return Source.cameraType != CameraType.SceneView && Source.cameraType != CameraType.Preview && XRSettings.enabled && Source.stereoTargetEye > StereoTargetEyeMask.None;
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x001315C4 File Offset: 0x0012F7C4
		private void UpdateRenderingMethod(Camera Camera, DynamicDecals System)
		{
			ShaderReplacement shaderReplacement = this.Standard(XRSettings.enabled && Camera.stereoTargetEye > StereoTargetEyeMask.None);
			ShaderReplacementType shaderReplacementType = System.Settings.Replacement;
			if (shaderReplacementType != ShaderReplacementType.VR)
			{
				if (shaderReplacementType != ShaderReplacementType.Mobile)
				{
				}
			}
			shaderReplacement = ShaderReplacement.Stencil;
			if (this.replacement != shaderReplacement)
			{
				this.replacement = shaderReplacement;
				this.SwitchRenderingMethod(Camera);
				this.UpdateRenderTextures(Camera, System, true);
			}
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x0013162C File Offset: 0x0012F82C
		private void SwitchRenderingMethod(Camera Camera)
		{
			switch (this.replacement)
			{
			case ShaderReplacement.SingleTarget:
			case ShaderReplacement.SinglePass:
				this.RestoreDepthTextureMode(Camera);
				return;
			case ShaderReplacement.DoublePass:
			case ShaderReplacement.TriplePass:
				if (Camera.actualRenderingPath == RenderingPath.DeferredShading)
				{
					this.RestoreDepthTextureMode(Camera);
					return;
				}
				this.desiredDTM = new DepthTextureMode?(DepthTextureMode.Depth);
				this.SetDepthTextureMode(Camera);
				return;
			case ShaderReplacement.Classic:
				this.desiredDTM = new DepthTextureMode?(DepthTextureMode.DepthNormals);
				this.SetDepthTextureMode(Camera);
				return;
			case ShaderReplacement.Stencil:
				this.RestoreDepthTextureMode(Camera);
				return;
			default:
				return;
			}
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x001316AC File Offset: 0x0012F8AC
		private void SetDepthTextureMode(Camera Camera)
		{
			if (this.desiredDTM != null)
			{
				DepthTextureMode depthTextureMode = Camera.depthTextureMode;
				DepthTextureMode? depthTextureMode2 = this.desiredDTM;
				if (!(depthTextureMode == depthTextureMode2.GetValueOrDefault() & depthTextureMode2 != null))
				{
					if (this.originalDTM == null)
					{
						this.originalDTM = new DepthTextureMode?(Camera.depthTextureMode);
					}
					else
					{
						Camera.depthTextureMode = this.originalDTM.Value;
					}
					Camera.depthTextureMode |= this.desiredDTM.Value;
					return;
				}
			}
			else
			{
				this.RestoreDepthTextureMode(Camera);
			}
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x00028FFC File Offset: 0x000271FC
		public void RestoreDepthTextureMode(Camera Camera)
		{
			if (this.originalDTM != null && Camera != null)
			{
				Camera.depthTextureMode = this.originalDTM.Value;
			}
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x00131738 File Offset: 0x0012F938
		private void UpdateRenderTextures(Camera Camera, DynamicDecals System, bool ForceNewTextures = false)
		{
			int num = Camera.pixelWidth;
			int num2 = Camera.pixelHeight;
			if (this.VRCamera(Camera))
			{
				num = (System.Settings.SinglePassVR ? (XRSettings.eyeTextureWidth * 2) : XRSettings.eyeTextureWidth);
				num2 = XRSettings.eyeTextureHeight;
			}
			if (this.maskBuffer == null || this.maskBuffer.width != num || this.maskBuffer.height != num2 || ForceNewTextures)
			{
				this.ReleaseTextures();
				this.GetTextures(Camera, System, num, num2);
				this.CreateStencilResolveCommand();
			}
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x001317C8 File Offset: 0x0012F9C8
		private void CreateStencilResolveCommand()
		{
			if (this.stencilResolveCommand != null)
			{
				this.stencilResolveCommand.Clear();
			}
			this.stencilResolveCommand = new CommandBuffer();
			this.stencilResolveCommand.name = "StencilResolve";
			int nameID = Shader.PropertyToID("_MaskBuffer_0");
			this.stencilResolveCommand.GetTemporaryRT(nameID, -1, -1, 0, FilterMode.Point, DynamicDecals.System.stencilMaskFormat);
			this.stencilResolveCommand.SetRenderTarget(nameID, BuiltinRenderTextureType.CameraTarget);
			this.stencilResolveCommand.ClearRenderTarget(false, true, new Color(0f, 0f, 0f, 0f));
			this.stencilResolveCommand.DrawMesh(DynamicDecals.System.FulllscreenMesh, Matrix4x4.identity, DynamicDecals.System.StencilResolveMaterial);
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x0013188C File Offset: 0x0012FA8C
		private void GetTextures(Camera Camera, DynamicDecals System, int Width, int Height)
		{
			switch (this.replacement)
			{
			case ShaderReplacement.SingleTarget:
				this.depthBuffer = RenderTexture.GetTemporary(Width, Height, 24, RenderTextureFormat.RGFloat);
				if (this.VRCamera(Camera) && System.Settings.SinglePassVR)
				{
					this.depthEye = RenderTexture.GetTemporary(XRSettings.eyeTextureWidth, XRSettings.eyeTextureHeight, 24, RenderTextureFormat.RGFloat);
					return;
				}
				break;
			case ShaderReplacement.SinglePass:
				this.depthBuffer = RenderTexture.GetTemporary(Width, Height, 24, System.depthFormat);
				this.normalBuffer = RenderTexture.GetTemporary(Width, Height, 0, System.normalFormat);
				this.maskBuffer = RenderTexture.GetTemporary(Width, Height, 0, System.maskFormat);
				return;
			case ShaderReplacement.DoublePass:
				if (Camera.actualRenderingPath == RenderingPath.DeferredShading)
				{
					this.depthBuffer = RenderTexture.GetTemporary(Width, Height, 24, System.depthFormat);
				}
				this.normalBuffer = RenderTexture.GetTemporary(Width, Height, 24, System.normalFormat);
				this.maskBuffer = RenderTexture.GetTemporary(Width, Height, 0, System.maskFormat);
				return;
			case ShaderReplacement.TriplePass:
				if (Camera.actualRenderingPath == RenderingPath.DeferredShading)
				{
					this.depthBuffer = RenderTexture.GetTemporary(Width, Height, 24, System.depthFormat);
				}
				this.normalBuffer = RenderTexture.GetTemporary(Width, Height, 24, System.normalFormat);
				this.maskBuffer = RenderTexture.GetTemporary(Width, Height, 24, System.maskFormat);
				if (this.VRCamera(Camera) && System.Settings.SinglePassVR)
				{
					if (Camera.actualRenderingPath == RenderingPath.DeferredShading)
					{
						this.depthEye = RenderTexture.GetTemporary(XRSettings.eyeTextureWidth, XRSettings.eyeTextureHeight, 24, System.depthFormat);
					}
					this.normalEye = RenderTexture.GetTemporary(XRSettings.eyeTextureWidth, XRSettings.eyeTextureHeight, 24, System.normalFormat);
					this.maskEye = RenderTexture.GetTemporary(XRSettings.eyeTextureWidth, XRSettings.eyeTextureHeight, 24, System.maskFormat);
					return;
				}
				break;
			case ShaderReplacement.Classic:
				this.maskBuffer = RenderTexture.GetTemporary(Width, Height, 24, System.maskFormat);
				return;
			case ShaderReplacement.Stencil:
				this.maskBuffer = RenderTexture.GetTemporary(Width, Height, 0, System.stencilMaskFormat);
				break;
			default:
				return;
			}
		}

		// Token: 0x06003D4F RID: 15695 RVA: 0x00131A8C File Offset: 0x0012FC8C
		public void ReleaseTextures()
		{
			if (this.depthBuffer != null && this.depthBuffer.IsCreated())
			{
				RenderTexture.ReleaseTemporary(this.depthBuffer);
				this.depthBuffer = null;
				if (this.depthEye != null && this.depthEye.IsCreated())
				{
					RenderTexture.ReleaseTemporary(this.depthEye);
					this.depthEye = null;
				}
			}
			if (this.normalBuffer != null && this.normalBuffer.IsCreated())
			{
				RenderTexture.ReleaseTemporary(this.normalBuffer);
				this.normalBuffer = null;
				if (this.normalEye != null && this.normalEye.IsCreated())
				{
					RenderTexture.ReleaseTemporary(this.normalEye);
					this.normalEye = null;
				}
			}
			if (this.maskBuffer != null && this.maskBuffer.IsCreated())
			{
				RenderTexture.ReleaseTemporary(this.maskBuffer);
				this.maskBuffer = null;
				if (this.maskEye != null && this.maskEye.IsCreated())
				{
					RenderTexture.ReleaseTemporary(this.maskEye);
					this.maskEye = null;
				}
			}
		}

		// Token: 0x06003D50 RID: 15696 RVA: 0x00131BA8 File Offset: 0x0012FDA8
		private void UpdateShaderReplacement(Camera Source, DynamicDecals System)
		{
			if (System.ShaderReplacement)
			{
				Camera customCamera = System.CustomCamera;
				this.SetupReplacementCamera(Source, customCamera);
				if (this.VRCamera(Source) && System.Settings.SinglePassVR)
				{
					if (Source.stereoTargetEye == StereoTargetEyeMask.Both || Source.stereoTargetEye == StereoTargetEyeMask.Left)
					{
						if (Source.transform.parent != null)
						{
							customCamera.transform.position = Source.transform.parent.TransformPoint(InputTracking.GetLocalPosition(XRNode.LeftEye));
						}
						else
						{
							customCamera.transform.position = InputTracking.GetLocalPosition(XRNode.LeftEye);
						}
						customCamera.transform.rotation = Source.transform.rotation * InputTracking.GetLocalRotation(XRNode.LeftEye);
						customCamera.projectionMatrix = Source.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
						customCamera.worldToCameraMatrix = Source.worldToCameraMatrix;
						this.RenderToTextures(Source, customCamera, System, this.depthEye, this.normalEye, this.maskEye);
						this.StereoBlit(Source, System, true);
					}
					if (Source.stereoTargetEye == StereoTargetEyeMask.Both || Source.stereoTargetEye == StereoTargetEyeMask.Right)
					{
						if (Source.transform.parent != null)
						{
							customCamera.transform.position = Source.transform.parent.TransformPoint(InputTracking.GetLocalPosition(XRNode.RightEye));
							Matrix4x4 worldToCameraMatrix = Source.worldToCameraMatrix;
							worldToCameraMatrix.m03 -= Source.stereoSeparation * Source.transform.parent.localScale.x;
							customCamera.worldToCameraMatrix = worldToCameraMatrix;
						}
						else
						{
							customCamera.transform.position = InputTracking.GetLocalPosition(XRNode.RightEye);
							Matrix4x4 worldToCameraMatrix2 = Source.worldToCameraMatrix;
							worldToCameraMatrix2.m03 -= Source.stereoSeparation;
							customCamera.worldToCameraMatrix = worldToCameraMatrix2;
						}
						customCamera.transform.rotation = Source.transform.rotation * InputTracking.GetLocalRotation(XRNode.RightEye);
						customCamera.projectionMatrix = Source.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
						this.RenderToTextures(Source, customCamera, System, this.depthEye, this.normalEye, this.maskEye);
						this.StereoBlit(Source, System, false);
						return;
					}
				}
				else
				{
					this.RenderToTextures(Source, customCamera, System, this.depthBuffer, this.normalBuffer, this.maskBuffer);
				}
			}
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x00131DC4 File Offset: 0x0012FFC4
		private void RenderToTextures(Camera Source, Camera Renderer, DynamicDecals System, RenderTexture depth, RenderTexture normal, RenderTexture mask)
		{
			switch (this.replacement)
			{
			case ShaderReplacement.SingleTarget:
				Renderer.targetTexture = depth;
				this.DrawSplitPass(Source, Renderer, System, System.DepthNormalMaskShader_Packed, true);
				break;
			case ShaderReplacement.SinglePass:
			{
				RenderBuffer[] colorBuffer = new RenderBuffer[]
				{
					mask.colorBuffer,
					normal.colorBuffer,
					depth.colorBuffer
				};
				Renderer.SetTargetBuffers(colorBuffer, depth.depthBuffer);
				this.DrawSplitPass(Source, Renderer, System, System.DepthNormalMaskShader, true);
				break;
			}
			case ShaderReplacement.DoublePass:
			{
				if (Source.actualRenderingPath == RenderingPath.DeferredShading)
				{
					Renderer.cullingMask = Source.cullingMask;
					Renderer.targetTexture = depth;
					this.DrawRegualarPass(Renderer, System.DepthShader);
				}
				RenderBuffer[] colorBuffer2 = new RenderBuffer[]
				{
					normal.colorBuffer,
					mask.colorBuffer
				};
				Renderer.SetTargetBuffers(colorBuffer2, normal.depthBuffer);
				this.DrawSplitPass(Source, Renderer, System, System.NormalMaskShader, true);
				break;
			}
			case ShaderReplacement.TriplePass:
				Renderer.cullingMask = Source.cullingMask;
				if (Source.actualRenderingPath == RenderingPath.DeferredShading)
				{
					Renderer.targetTexture = depth;
					this.DrawRegualarPass(Renderer, System.DepthShader);
				}
				Renderer.targetTexture = normal;
				this.DrawRegualarPass(Renderer, System.NormalShader);
				Renderer.targetTexture = mask;
				this.DrawSplitPass(Source, Renderer, System, System.MaskShader, true);
				break;
			case ShaderReplacement.Classic:
				Renderer.targetTexture = mask;
				this.DrawSplitPass(Source, Renderer, System, System.MaskShader, false);
				break;
			case ShaderReplacement.Stencil:
				if (this.stencilResolveCommand != null)
				{
					CameraEvent evt = CameraEvent.BeforeLighting;
					bool flag = false;
					foreach (CommandBuffer commandBuffer in Source.GetCommandBuffers(evt))
					{
						if (commandBuffer != this.stencilResolveCommand && commandBuffer.name == "StencilResolve")
						{
							Source.RemoveCommandBuffer(evt, commandBuffer);
						}
						else if (commandBuffer.name == "StencilResolve")
						{
							flag = true;
						}
					}
					if (!flag)
					{
						Source.AddCommandBuffer(evt, this.stencilResolveCommand);
					}
				}
				break;
			}
			Renderer.targetTexture = null;
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x00029025 File Offset: 0x00027225
		private void DrawRegualarPass(Camera Renderer, Shader ReplacementShader)
		{
			Renderer.clearFlags = CameraClearFlags.Color;
			Renderer.backgroundColor = Color.clear;
			Renderer.RenderWithShader(ReplacementShader, "RenderType");
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x00131FE0 File Offset: 0x001301E0
		private void DrawSplitPass(Camera Source, Camera Renderer, DynamicDecals System, Shader ReplacementShader, bool RenderInvalid = true)
		{
			List<ReplacementPass> passes = System.Settings.Passes;
			Renderer.clearFlags = CameraClearFlags.Color;
			Renderer.backgroundColor = Color.clear;
			if (System.Settings.UseMaskLayers)
			{
				for (int i = 0; i < passes.Count; i++)
				{
					if (passes[i].vector != Vector4.zero || RenderInvalid)
					{
						Renderer.cullingMask = (passes[i].layers & Source.cullingMask);
						Shader.SetGlobalVector("_MaskWrite", passes[i].vector);
						Renderer.RenderWithShader(ReplacementShader, "RenderType");
						Renderer.clearFlags = CameraClearFlags.Nothing;
					}
				}
				return;
			}
			if (RenderInvalid)
			{
				Renderer.cullingMask = -1;
				Shader.SetGlobalVector("_MaskWrite", Vector4.zero);
				Renderer.RenderWithShader(ReplacementShader, "RenderType");
				Renderer.clearFlags = CameraClearFlags.Nothing;
			}
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x001320BC File Offset: 0x001302BC
		private void StereoBlit(Camera Source, DynamicDecals System, bool Left)
		{
			ShaderReplacement shaderReplacement = this.replacement;
			if (shaderReplacement == ShaderReplacement.SingleTarget)
			{
				Graphics.Blit(this.depthEye, this.depthBuffer, Left ? System.StereoBlitLeft : System.StereoBlitRight);
				return;
			}
			if (shaderReplacement != ShaderReplacement.TriplePass)
			{
				return;
			}
			if (Source.actualRenderingPath == RenderingPath.DeferredShading)
			{
				Material material = Left ? System.StereoDepthBlitLeft : System.StereoDepthBlitRight;
				material.SetTexture("_DepthTex", this.depthEye);
				Graphics.Blit(this.depthEye, this.depthBuffer, material);
			}
			Graphics.Blit(this.normalEye, this.normalBuffer, Left ? System.StereoBlitLeft : System.StereoBlitRight);
			Graphics.Blit(this.maskEye, this.maskBuffer, Left ? System.StereoBlitLeft : System.StereoBlitRight);
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x00132180 File Offset: 0x00130380
		private void SetupReplacementCamera(Camera Source, Camera Target)
		{
			Target.CopyFrom(Source);
			Target.renderingPath = RenderingPath.Forward;
			Target.depthTextureMode = DepthTextureMode.None;
			Target.useOcclusionCulling = false;
			Target.allowMSAA = false;
			Target.allowHDR = false;
			Target.rect = new Rect(0f, 0f, 1f, 1f);
		}

		// Token: 0x06003D56 RID: 15702 RVA: 0x001321D8 File Offset: 0x001303D8
		private void SetupReplacementCameraExperimental(Camera Source, Camera Target)
		{
			Target.transform.position = Source.transform.position;
			Target.transform.rotation = Source.transform.rotation;
			if (!XRSettings.enabled)
			{
				Target.fieldOfView = Source.fieldOfView;
			}
			Target.nearClipPlane = Source.nearClipPlane;
			Target.farClipPlane = Source.farClipPlane;
			Target.rect = new Rect(0f, 0f, 1f, 1f);
			Target.orthographic = Source.orthographic;
			Target.orthographicSize = Source.orthographicSize;
			Target.ResetProjectionMatrix();
			Target.ResetWorldToCameraMatrix();
			Target.renderingPath = RenderingPath.Forward;
			Target.depthTextureMode = DepthTextureMode.None;
			Target.useOcclusionCulling = false;
			Target.allowMSAA = false;
			Target.allowHDR = false;
			Target.eventMask = 0;
		}

		// Token: 0x04003A02 RID: 14850
		public bool hasProjectionBlocker;

		// Token: 0x04003A03 RID: 14851
		public DepthTextureMode? originalDTM;

		// Token: 0x04003A04 RID: 14852
		public DepthTextureMode? desiredDTM;

		// Token: 0x04003A05 RID: 14853
		public ShaderReplacement replacement;

		// Token: 0x04003A06 RID: 14854
		private RenderTexture depthBuffer;

		// Token: 0x04003A07 RID: 14855
		private RenderTexture normalBuffer;

		// Token: 0x04003A08 RID: 14856
		private RenderTexture maskBuffer;

		// Token: 0x04003A09 RID: 14857
		private CommandBuffer stencilResolveCommand;

		// Token: 0x04003A0A RID: 14858
		private RenderTexture depthEye;

		// Token: 0x04003A0B RID: 14859
		private RenderTexture normalEye;

		// Token: 0x04003A0C RID: 14860
		private RenderTexture maskEye;

		// Token: 0x04003A0D RID: 14861
		public bool initialized;
	}
}
