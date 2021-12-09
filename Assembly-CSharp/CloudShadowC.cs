using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000240 RID: 576
[ExecuteInEditMode]
[RequireComponent(typeof(Light))]
public class CloudShadowC : MonoBehaviour
{
	// Token: 0x060010AE RID: 4270 RVA: 0x00082B8C File Offset: 0x00080D8C
	private void OnEnable()
	{
		this.lightComponent = base.GetComponent<Light>();
		if (this.material == null)
		{
			this.material = new Material(Shader.Find("Hidden/CloudShadowC"));
			this.material.hideFlags = HideFlags.HideAndDontSave;
		}
		if (this.commandBuffer == null)
		{
			this.commandBuffer = new CommandBuffer();
			this.commandBuffer.name = "CloudShadows";
			this.commandBuffer.Blit(BuiltinRenderTextureType.None, BuiltinRenderTextureType.CurrentActive, this.material);
			this.lightComponent.AddCommandBuffer(LightEvent.AfterScreenspaceMask, this.commandBuffer);
		}
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(this.UpdateMaterial));
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x00082C48 File Offset: 0x00080E48
	public void Update()
	{
		Shader.SetGlobalTexture("_NoiseTex", this.noiseTex);
		Shader.SetGlobalFloat("_CloudSpeed1", this.cloudSpeed1);
		Shader.SetGlobalFloat("_CloudSpeed2", this.cloudSpeed2);
		Shader.SetGlobalFloat("_CloudStrength", this.cloudStrength);
		Shader.SetGlobalFloat("_CloudScale", this.cloudScale);
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x00082CA8 File Offset: 0x00080EA8
	private void OnDisable()
	{
		if (this.commandBuffer != null)
		{
			this.lightComponent.RemoveCommandBuffer(LightEvent.AfterScreenspaceMask, this.commandBuffer);
			this.commandBuffer = null;
		}
		if (this.material != null)
		{
			UnityEngine.Object.DestroyImmediate(this.material);
			this.material = null;
		}
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(this.UpdateMaterial));
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x00082D18 File Offset: 0x00080F18
	private void UpdateMaterial(Camera camera)
	{
		Matrix4x4 worldToCameraMatrix = camera.worldToCameraMatrix;
		Matrix4x4 matrix4x = GL.GetGPUProjectionMatrix(camera.projectionMatrix, false) * worldToCameraMatrix;
		this.material.SetMatrix("_ViewProjInv", matrix4x.inverse);
	}

	// Token: 0x0400111F RID: 4383
	public Texture2D noiseTex;

	// Token: 0x04001120 RID: 4384
	public float cloudSpeed1 = 0.12f;

	// Token: 0x04001121 RID: 4385
	public float cloudSpeed2 = 0.1375f;

	// Token: 0x04001122 RID: 4386
	public float cloudStrength = 1f;

	// Token: 0x04001123 RID: 4387
	public float cloudScale = 200f;

	// Token: 0x04001124 RID: 4388
	private Light lightComponent;

	// Token: 0x04001125 RID: 4389
	private Material material;

	// Token: 0x04001126 RID: 4390
	private CommandBuffer commandBuffer;
}
