using System;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000727 RID: 1831
	[ExecuteInEditMode]
	public class MWater : MonoBehaviour
	{
		// Token: 0x0600356C RID: 13676 RVA: 0x001141D8 File Offset: 0x001123D8
		public void OnWillRenderObject()
		{
			if (!base.enabled || !base.GetComponent<Renderer>() || !base.GetComponent<Renderer>().sharedMaterial || !base.GetComponent<Renderer>().enabled)
			{
				return;
			}
			Camera current = Camera.current;
			if (!current)
			{
				return;
			}
			if (MWater.s_InsideWater)
			{
				return;
			}
			MWater.s_InsideWater = true;
			this.m_HardwareWaterSupport = this.FindHardwareWaterSupport();
			MWater.WaterMode waterMode = this.GetWaterMode();
			Camera camera;
			Camera camera2;
			this.CreateWaterObjects(current, out camera, out camera2);
			Vector3 position = base.transform.position;
			Vector3 up = base.transform.up;
			int pixelLightCount = QualitySettings.pixelLightCount;
			if (this.disablePixelLights)
			{
				QualitySettings.pixelLightCount = 0;
			}
			this.UpdateCameraModes(current, camera);
			this.UpdateCameraModes(current, camera2);
			if (waterMode >= MWater.WaterMode.Reflective)
			{
				float w = -Vector3.Dot(up, position) - this.clipPlaneOffset;
				Vector4 plane = new Vector4(up.x, up.y, up.z, w);
				Matrix4x4 zero = Matrix4x4.zero;
				MWater.CalculateReflectionMatrix(ref zero, plane);
				Vector3 position2 = current.transform.position;
				Vector3 position3 = zero.MultiplyPoint(position2);
				camera.worldToCameraMatrix = current.worldToCameraMatrix * zero;
				Vector4 clipPlane = this.CameraSpacePlane(camera, position, up, 1f);
				camera.projectionMatrix = current.CalculateObliqueMatrix(clipPlane);
				camera.cullingMatrix = current.projectionMatrix * current.worldToCameraMatrix;
				camera.cullingMask = (-17 & this.reflectLayers.value);
				camera.targetTexture = this.m_ReflectionTexture;
				bool invertCulling = GL.invertCulling;
				GL.invertCulling = !invertCulling;
				camera.transform.position = position3;
				Vector3 eulerAngles = current.transform.eulerAngles;
				camera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
				try
				{
					camera.Render();
				}
				catch
				{
				}
				camera.transform.position = position2;
				GL.invertCulling = invertCulling;
				base.GetComponent<Renderer>().sharedMaterial.SetTexture("_ReflectionTex", this.m_ReflectionTexture);
			}
			if (waterMode >= MWater.WaterMode.Refractive)
			{
				camera2.worldToCameraMatrix = current.worldToCameraMatrix;
				Vector4 clipPlane2 = this.CameraSpacePlane(camera2, position, up, -1f);
				camera2.projectionMatrix = current.CalculateObliqueMatrix(clipPlane2);
				camera2.cullingMatrix = current.projectionMatrix * current.worldToCameraMatrix;
				camera2.cullingMask = (-17 & this.refractLayers.value);
				camera2.targetTexture = this.m_RefractionTexture;
				camera2.transform.position = current.transform.position;
				camera2.transform.rotation = current.transform.rotation;
				try
				{
					camera2.Render();
				}
				catch
				{
					throw new Exception("");
				}
				base.GetComponent<Renderer>().sharedMaterial.SetTexture("_RefractionTex", this.m_RefractionTexture);
			}
			if (this.disablePixelLights)
			{
				QualitySettings.pixelLightCount = pixelLightCount;
			}
			switch (waterMode)
			{
			case MWater.WaterMode.Simple:
				Shader.EnableKeyword("WATER_SIMPLE");
				Shader.DisableKeyword("WATER_REFLECTIVE");
				Shader.DisableKeyword("WATER_REFRACTIVE");
				break;
			case MWater.WaterMode.Reflective:
				Shader.DisableKeyword("WATER_SIMPLE");
				Shader.EnableKeyword("WATER_REFLECTIVE");
				Shader.DisableKeyword("WATER_REFRACTIVE");
				break;
			case MWater.WaterMode.Refractive:
				Shader.DisableKeyword("WATER_SIMPLE");
				Shader.DisableKeyword("WATER_REFLECTIVE");
				Shader.EnableKeyword("WATER_REFRACTIVE");
				break;
			}
			MWater.s_InsideWater = false;
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x00114554 File Offset: 0x00112754
		private void OnDisable()
		{
			if (this.m_ReflectionTexture)
			{
				UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
				this.m_ReflectionTexture = null;
			}
			if (this.m_RefractionTexture)
			{
				UnityEngine.Object.DestroyImmediate(this.m_RefractionTexture);
				this.m_RefractionTexture = null;
			}
			foreach (KeyValuePair<Camera, Camera> keyValuePair in this.m_ReflectionCameras)
			{
				UnityEngine.Object.DestroyImmediate(keyValuePair.Value.gameObject);
			}
			this.m_ReflectionCameras.Clear();
			foreach (KeyValuePair<Camera, Camera> keyValuePair2 in this.m_RefractionCameras)
			{
				UnityEngine.Object.DestroyImmediate(keyValuePair2.Value.gameObject);
			}
			this.m_RefractionCameras.Clear();
		}

		// Token: 0x0600356E RID: 13678 RVA: 0x00114654 File Offset: 0x00112854
		private void Update()
		{
			if (!base.GetComponent<Renderer>())
			{
				return;
			}
			Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
			if (!sharedMaterial)
			{
				return;
			}
			Vector4 vector = sharedMaterial.GetVector("WaveSpeed");
			float @float = sharedMaterial.GetFloat("_WaveScale");
			Vector4 vector2 = new Vector4(@float, @float, @float * 0.4f, @float * 0.45f);
			double num = (double)Time.timeSinceLevelLoad / 20.0;
			Vector4 value = new Vector4((float)Math.IEEERemainder((double)(vector.x * vector2.x) * num, 1.0), (float)Math.IEEERemainder((double)(vector.y * vector2.y) * num, 1.0), (float)Math.IEEERemainder((double)(vector.z * vector2.z) * num, 1.0), (float)Math.IEEERemainder((double)(vector.w * vector2.w) * num, 1.0));
			sharedMaterial.SetVector("_WaveOffset", value);
			sharedMaterial.SetVector("_WaveScale4", vector2);
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x00114768 File Offset: 0x00112968
		private void UpdateCameraModes(Camera src, Camera dest)
		{
			if (dest == null)
			{
				return;
			}
			dest.clearFlags = src.clearFlags;
			dest.backgroundColor = src.backgroundColor;
			if (src.clearFlags == CameraClearFlags.Skybox)
			{
				Skybox component = src.GetComponent<Skybox>();
				Skybox component2 = dest.GetComponent<Skybox>();
				if (!component || !component.material)
				{
					component2.enabled = false;
				}
				else
				{
					component2.enabled = true;
					component2.material = component.material;
				}
			}
			dest.farClipPlane = src.farClipPlane;
			dest.nearClipPlane = src.nearClipPlane;
			dest.orthographic = src.orthographic;
			dest.fieldOfView = src.fieldOfView;
			dest.aspect = src.aspect;
			dest.orthographicSize = src.orthographicSize;
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x00114828 File Offset: 0x00112A28
		private void CreateWaterObjects(Camera currentCamera, out Camera reflectionCamera, out Camera refractionCamera)
		{
			MWater.WaterMode waterMode = this.GetWaterMode();
			reflectionCamera = null;
			refractionCamera = null;
			if (waterMode >= MWater.WaterMode.Reflective)
			{
				if (!this.m_ReflectionTexture || this.m_OldReflectionTextureSize != this.textureSize)
				{
					if (this.m_ReflectionTexture)
					{
						UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
					}
					this.m_ReflectionTexture = new RenderTexture(this.textureSize, this.textureSize, 16);
					this.m_ReflectionTexture.name = "__WaterReflection" + base.GetInstanceID().ToString();
					this.m_ReflectionTexture.isPowerOfTwo = true;
					this.m_ReflectionTexture.hideFlags = HideFlags.DontSave;
					this.m_OldReflectionTextureSize = this.textureSize;
				}
				this.m_ReflectionCameras.TryGetValue(currentCamera, out reflectionCamera);
				if (!reflectionCamera)
				{
					GameObject gameObject = new GameObject("Water Refl Camera id" + base.GetInstanceID().ToString() + " for " + currentCamera.GetInstanceID().ToString(), new Type[]
					{
						typeof(Camera),
						typeof(Skybox)
					});
					reflectionCamera = gameObject.GetComponent<Camera>();
					reflectionCamera.enabled = false;
					reflectionCamera.transform.position = base.transform.position;
					reflectionCamera.transform.rotation = base.transform.rotation;
					reflectionCamera.gameObject.AddComponent<FlareLayer>();
					gameObject.hideFlags = HideFlags.HideAndDontSave;
					this.m_ReflectionCameras[currentCamera] = reflectionCamera;
				}
			}
			if (waterMode >= MWater.WaterMode.Refractive)
			{
				if (!this.m_RefractionTexture || this.m_OldRefractionTextureSize != this.textureSize)
				{
					if (this.m_RefractionTexture)
					{
						UnityEngine.Object.DestroyImmediate(this.m_RefractionTexture);
					}
					this.m_RefractionTexture = new RenderTexture(this.textureSize, this.textureSize, 16);
					this.m_RefractionTexture.name = "__WaterRefraction" + base.GetInstanceID().ToString();
					this.m_RefractionTexture.isPowerOfTwo = true;
					this.m_RefractionTexture.hideFlags = HideFlags.DontSave;
					this.m_OldRefractionTextureSize = this.textureSize;
				}
				this.m_RefractionCameras.TryGetValue(currentCamera, out refractionCamera);
				if (!refractionCamera)
				{
					GameObject gameObject2 = new GameObject("Water Refr Camera id" + base.GetInstanceID().ToString() + " for " + currentCamera.GetInstanceID().ToString(), new Type[]
					{
						typeof(Camera),
						typeof(Skybox)
					});
					refractionCamera = gameObject2.GetComponent<Camera>();
					refractionCamera.enabled = false;
					refractionCamera.transform.position = base.transform.position;
					refractionCamera.transform.rotation = base.transform.rotation;
					refractionCamera.gameObject.AddComponent<FlareLayer>();
					gameObject2.hideFlags = HideFlags.HideAndDontSave;
					this.m_RefractionCameras[currentCamera] = refractionCamera;
				}
			}
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x00024429 File Offset: 0x00022629
		private MWater.WaterMode GetWaterMode()
		{
			if (this.m_HardwareWaterSupport < this.waterMode)
			{
				return this.m_HardwareWaterSupport;
			}
			return this.waterMode;
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x00114B10 File Offset: 0x00112D10
		private MWater.WaterMode FindHardwareWaterSupport()
		{
			if (!base.GetComponent<Renderer>())
			{
				return MWater.WaterMode.Simple;
			}
			Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
			if (!sharedMaterial)
			{
				return MWater.WaterMode.Simple;
			}
			string tag = sharedMaterial.GetTag("WATERMODE", false);
			if (tag == "Refractive")
			{
				return MWater.WaterMode.Refractive;
			}
			if (tag == "Reflective")
			{
				return MWater.WaterMode.Reflective;
			}
			return MWater.WaterMode.Simple;
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x00114B70 File Offset: 0x00112D70
		private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
		{
			Vector3 point = pos + normal * this.clipPlaneOffset;
			Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
			Vector3 lhs = worldToCameraMatrix.MultiplyPoint(point);
			Vector3 vector = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
			return new Vector4(vector.x, vector.y, vector.z, -Vector3.Dot(lhs, vector));
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x00114BD8 File Offset: 0x00112DD8
		private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
		{
			reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
			reflectionMat.m01 = -2f * plane[0] * plane[1];
			reflectionMat.m02 = -2f * plane[0] * plane[2];
			reflectionMat.m03 = -2f * plane[3] * plane[0];
			reflectionMat.m10 = -2f * plane[1] * plane[0];
			reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
			reflectionMat.m12 = -2f * plane[1] * plane[2];
			reflectionMat.m13 = -2f * plane[3] * plane[1];
			reflectionMat.m20 = -2f * plane[2] * plane[0];
			reflectionMat.m21 = -2f * plane[2] * plane[1];
			reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
			reflectionMat.m23 = -2f * plane[3] * plane[2];
			reflectionMat.m30 = 0f;
			reflectionMat.m31 = 0f;
			reflectionMat.m32 = 0f;
			reflectionMat.m33 = 1f;
		}

		// Token: 0x040034A6 RID: 13478
		public MWater.WaterMode waterMode = MWater.WaterMode.Refractive;

		// Token: 0x040034A7 RID: 13479
		public bool disablePixelLights = true;

		// Token: 0x040034A8 RID: 13480
		public int textureSize = 256;

		// Token: 0x040034A9 RID: 13481
		public float clipPlaneOffset = 0.07f;

		// Token: 0x040034AA RID: 13482
		public LayerMask reflectLayers = -1;

		// Token: 0x040034AB RID: 13483
		public LayerMask refractLayers = -1;

		// Token: 0x040034AC RID: 13484
		private Dictionary<Camera, Camera> m_ReflectionCameras = new Dictionary<Camera, Camera>();

		// Token: 0x040034AD RID: 13485
		private Dictionary<Camera, Camera> m_RefractionCameras = new Dictionary<Camera, Camera>();

		// Token: 0x040034AE RID: 13486
		private RenderTexture m_ReflectionTexture;

		// Token: 0x040034AF RID: 13487
		private RenderTexture m_RefractionTexture;

		// Token: 0x040034B0 RID: 13488
		private MWater.WaterMode m_HardwareWaterSupport = MWater.WaterMode.Refractive;

		// Token: 0x040034B1 RID: 13489
		private int m_OldReflectionTextureSize;

		// Token: 0x040034B2 RID: 13490
		private int m_OldRefractionTextureSize;

		// Token: 0x040034B3 RID: 13491
		private static bool s_InsideWater;

		// Token: 0x02000728 RID: 1832
		public enum WaterMode
		{
			// Token: 0x040034B5 RID: 13493
			Simple,
			// Token: 0x040034B6 RID: 13494
			Reflective,
			// Token: 0x040034B7 RID: 13495
			Refractive
		}
	}
}
