using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000438 RID: 1080
[ExecuteInEditMode]
[RequireComponent(typeof(Light))]
public class CloudShadow : MonoBehaviour
{
	// Token: 0x06001DDD RID: 7645 RVA: 0x000C2420 File Offset: 0x000C0620
	private void OnEnable()
	{
		this.lightComponent = base.GetComponent<Light>();
		if (this.material == null)
		{
			this.material = new Material(Shader.Find("Hidden/CloudShadow"));
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

	// Token: 0x06001DDE RID: 7646 RVA: 0x000C24DC File Offset: 0x000C06DC
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

	// Token: 0x06001DDF RID: 7647 RVA: 0x000C254C File Offset: 0x000C074C
	private void UpdateMaterial(Camera camera)
	{
		Matrix4x4 worldToCameraMatrix = camera.worldToCameraMatrix;
		Matrix4x4 matrix4x = GL.GetGPUProjectionMatrix(camera.projectionMatrix, false) * worldToCameraMatrix;
		this.material.SetMatrix("_ViewProjInv", matrix4x.inverse);
		this.material.SetTexture("_NoiseTex", this.noiseTex);
		this.material.SetFloat("_CloudSpeed1", this.cloudSpeed1);
		this.material.SetFloat("_CloudSpeed2", this.cloudSpeed2);
		this.material.SetFloat("_CloudStrength", this.cloudStrength);
		this.material.SetFloat("_CloudScale", this.cloudScale);
	}

	// Token: 0x040020A2 RID: 8354
	public Texture2D noiseTex;

	// Token: 0x040020A3 RID: 8355
	public float cloudSpeed1 = 0.12f;

	// Token: 0x040020A4 RID: 8356
	public float cloudSpeed2 = 0.1375f;

	// Token: 0x040020A5 RID: 8357
	public float cloudStrength = 1f;

	// Token: 0x040020A6 RID: 8358
	public float cloudScale = 200f;

	// Token: 0x040020A7 RID: 8359
	private Light lightComponent;

	// Token: 0x040020A8 RID: 8360
	private Material material;

	// Token: 0x040020A9 RID: 8361
	private CommandBuffer commandBuffer;
}
