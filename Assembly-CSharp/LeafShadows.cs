using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000242 RID: 578
[ExecuteInEditMode]
[RequireComponent(typeof(Light))]
public class LeafShadows : MonoBehaviour
{
	// Token: 0x060010B6 RID: 4278 RVA: 0x00082E50 File Offset: 0x00081050
	private void OnEnable()
	{
		this.lightComponent = base.GetComponent<Light>();
		if (this.material == null)
		{
			this.material = new Material(Shader.Find("Hidden/LeafShadows"));
			this.material.hideFlags = HideFlags.HideAndDontSave;
		}
		if (this.commandBuffer == null)
		{
			this.commandBuffer = new CommandBuffer();
			this.commandBuffer.name = "LeafShadows";
			this.commandBuffer.Blit(BuiltinRenderTextureType.None, BuiltinRenderTextureType.CurrentActive, this.material);
			this.lightComponent.AddCommandBuffer(LightEvent.AfterScreenspaceMask, this.commandBuffer);
		}
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(this.UpdateMaterial));
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x00082F0C File Offset: 0x0008110C
	public void Update()
	{
		Shader.SetGlobalTexture("_NoiseTex", this.noiseTex);
		Shader.SetGlobalTexture("_NoiseTex2", this.noiseTex2);
		Shader.SetGlobalFloat("_CloudSpeed1", this.cloudSpeed1);
		Shader.SetGlobalFloat("_CloudSpeed2", this.cloudSpeed2);
		Shader.SetGlobalFloat("_CloudStrength", this.cloudStrength);
		Shader.SetGlobalFloat("_CloudScale", this.cloudScale);
		this.UpdateLeaves();
	}

	// Token: 0x060010B8 RID: 4280 RVA: 0x0000398C File Offset: 0x00001B8C
	public void OnRenderObject()
	{
	}

	// Token: 0x060010B9 RID: 4281 RVA: 0x00082F80 File Offset: 0x00081180
	private void UpdateLeaves()
	{
		this.leafLayer1.Update(Time.deltaTime);
		this.leafLayer2.Update(Time.deltaTime);
		Shader.SetGlobalFloat("_ShadowLeafTime1", this.leafLayer1.GetTime());
		Shader.SetGlobalFloat("_ShadowLeafTime2", this.leafLayer2.GetTime());
	}

	// Token: 0x060010BA RID: 4282 RVA: 0x00082FD8 File Offset: 0x000811D8
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

	// Token: 0x060010BB RID: 4283 RVA: 0x00083048 File Offset: 0x00081248
	private void UpdateMaterial(Camera camera)
	{
		Matrix4x4 worldToCameraMatrix = camera.worldToCameraMatrix;
		Matrix4x4 matrix4x = GL.GetGPUProjectionMatrix(camera.projectionMatrix, false) * worldToCameraMatrix;
		this.material.SetMatrix("_ViewProjInv", matrix4x.inverse);
	}

	// Token: 0x04001133 RID: 4403
	public LeafShadowAnimator leafLayer1 = new LeafShadowAnimator();

	// Token: 0x04001134 RID: 4404
	public LeafShadowAnimator leafLayer2 = new LeafShadowAnimator();

	// Token: 0x04001135 RID: 4405
	public Texture2D noiseTex;

	// Token: 0x04001136 RID: 4406
	public Texture2D noiseTex2;

	// Token: 0x04001137 RID: 4407
	public float cloudSpeed1 = 0.12f;

	// Token: 0x04001138 RID: 4408
	public float cloudSpeed2 = 0.1375f;

	// Token: 0x04001139 RID: 4409
	public float cloudStrength = 1f;

	// Token: 0x0400113A RID: 4410
	public float cloudScale = 200f;

	// Token: 0x0400113B RID: 4411
	private Light lightComponent;

	// Token: 0x0400113C RID: 4412
	private Material material;

	// Token: 0x0400113D RID: 4413
	private CommandBuffer commandBuffer;
}
