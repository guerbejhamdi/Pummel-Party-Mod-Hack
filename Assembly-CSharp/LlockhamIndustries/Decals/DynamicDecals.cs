using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

namespace LlockhamIndustries.Decals
{
	// Token: 0x0200086A RID: 2154
	[ExecuteInEditMode]
	public class DynamicDecals : MonoBehaviour
	{
		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06003D01 RID: 15617 RVA: 0x00028B8B File Offset: 0x00026D8B
		public static bool Initialized
		{
			get
			{
				return DynamicDecals.system != null;
			}
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06003D02 RID: 15618 RVA: 0x00028B98 File Offset: 0x00026D98
		public static DynamicDecals System
		{
			get
			{
				if (DynamicDecals.system == null)
				{
					new GameObject("Dynamic Decals")
					{
						hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild)
					}.AddComponent<DynamicDecals>();
				}
				return DynamicDecals.system;
			}
		}

		// Token: 0x06003D03 RID: 15619 RVA: 0x000281AB File Offset: 0x000263AB
		private void Start()
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x06003D04 RID: 15620 RVA: 0x00130724 File Offset: 0x0012E924
		private void OnEnable()
		{
			if (DynamicDecals.system == null)
			{
				DynamicDecals.system = this;
			}
			else if (DynamicDecals.system != this)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(base.gameObject);
					return;
				}
				UnityEngine.Object.DestroyImmediate(base.gameObject, true);
				return;
			}
			this.Initialize();
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x00028BC4 File Offset: 0x00026DC4
		private void OnDisable()
		{
			this.Terminate();
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06003D06 RID: 15622 RVA: 0x00028BCC File Offset: 0x00026DCC
		public DynamicDecalSettings Settings
		{
			get
			{
				if (this.settings == null)
				{
					this.settings = Resources.Load<DynamicDecalSettings>("Settings");
				}
				if (this.settings == null)
				{
					this.settings = ScriptableObject.CreateInstance<DynamicDecalSettings>();
				}
				return this.settings;
			}
		}

		// Token: 0x06003D07 RID: 15623 RVA: 0x00028C0B File Offset: 0x00026E0B
		public static void ApplySettings()
		{
			DynamicDecals.system.settings = Resources.Load<DynamicDecalSettings>("Settings");
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06003D08 RID: 15624 RVA: 0x00028C21 File Offset: 0x00026E21
		private bool FireInCulling
		{
			get
			{
				return !XRSettings.enabled && this.Settings.Replacement != ShaderReplacementType.VR;
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06003D09 RID: 15625 RVA: 0x00028C3D File Offset: 0x00026E3D
		public SystemPath SystemPath
		{
			get
			{
				return this.renderingPath;
			}
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06003D0A RID: 15626 RVA: 0x00028C45 File Offset: 0x00026E45
		// (set) Token: 0x06003D0B RID: 15627 RVA: 0x00028C5D File Offset: 0x00026E5D
		public bool ShaderReplacement
		{
			get
			{
				return this.Projections.Count > 0 && this.shaderReplacement;
			}
			set
			{
				this.shaderReplacement = value;
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06003D0C RID: 15628 RVA: 0x00028C66 File Offset: 0x00026E66
		public bool Instanced
		{
			get
			{
				return SystemInfo.supportsInstancing;
			}
		}

		// Token: 0x06003D0D RID: 15629 RVA: 0x0013077C File Offset: 0x0012E97C
		private void UpdateSystemPath()
		{
			Camera camera = null;
			if (Camera.main != null)
			{
				camera = Camera.main;
			}
			else if (Camera.current != null)
			{
				camera = Camera.current;
			}
			if (camera != null)
			{
				if (camera.actualRenderingPath == RenderingPath.Forward || camera.actualRenderingPath == RenderingPath.DeferredShading)
				{
					SystemPath systemPath = SystemPath.Forward;
					if (camera.actualRenderingPath == RenderingPath.DeferredShading)
					{
						systemPath = SystemPath.Deferred;
					}
					if (this.renderingPath != systemPath)
					{
						this.renderingPath = systemPath;
						this.UpdateRenderers();
						return;
					}
				}
				else
				{
					Debug.LogWarning("Current Rendering Path not supported! Please use either Forward or Deferred");
				}
			}
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x001307FC File Offset: 0x0012E9FC
		public void RestoreDepthTextureModes()
		{
			for (int i = 0; i < this.cameraData.Count; i++)
			{
				Camera key = this.cameraData.ElementAt(i).Key;
				if (key != null)
				{
					this.cameraData.ElementAt(i).Value.RestoreDepthTextureMode(key);
				}
			}
		}

		// Token: 0x06003D0F RID: 15631 RVA: 0x00130858 File Offset: 0x0012EA58
		private ProjectionData GetProjectionData(Projection Projection)
		{
			for (int i = 0; i < this.Projections.Count; i++)
			{
				if (this.Projections[i].projection == Projection)
				{
					return this.Projections[i];
				}
			}
			return null;
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x001308A4 File Offset: 0x0012EAA4
		private void UpdateProjectionData()
		{
			for (int i = 0; i < this.Projections.Count; i++)
			{
				this.Projections[i].Update();
			}
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x001308D8 File Offset: 0x0012EAD8
		public bool Register(ProjectionRenderer Instance)
		{
			if (!(Instance != null))
			{
				return false;
			}
			Projection projection = Instance.Projection;
			ProjectionData projectionData = this.GetProjectionData(projection);
			if (projectionData != null)
			{
				projectionData.Add(Instance);
				return base.isActiveAndEnabled;
			}
			projectionData = new ProjectionData(projection);
			projectionData.Add(Instance);
			for (int i = 0; i < this.Projections.Count; i++)
			{
				if (projection.Priority < this.Projections[i].projection.Priority)
				{
					this.Projections.Insert(i, projectionData);
					return true;
				}
			}
			this.Projections.Add(projectionData);
			return base.isActiveAndEnabled;
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x00130978 File Offset: 0x0012EB78
		public void Deregister(ProjectionRenderer Instance)
		{
			if (Instance != null)
			{
				Projection projection = Instance.Projection;
				for (int i = 0; i < this.Projections.Count; i++)
				{
					if (this.Projections[i].projection == projection)
					{
						this.Projections[i].Remove(Instance);
						if (this.Projections[i].instances.Count == 0)
						{
							this.Projections.RemoveAt(i);
						}
						return;
					}
				}
			}
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x001309FC File Offset: 0x0012EBFC
		public void Reorder(Projection Projection)
		{
			ProjectionData projectionData = this.GetProjectionData(Projection);
			if (projectionData != null)
			{
				this.Projections.Remove(projectionData);
				for (int i = 0; i < this.Projections.Count; i++)
				{
					if (Projection.Priority < this.Projections[i].projection.Priority)
					{
						this.Projections.Insert(i, projectionData);
						return;
					}
				}
				this.Projections.Add(projectionData);
				this.OrderRenderers();
			}
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x00130A78 File Offset: 0x0012EC78
		public void OrderRenderers()
		{
			if (this.renderersMarked && this.Projections != null)
			{
				int num = 1;
				foreach (ProjectionData projectionData in this.Projections)
				{
					projectionData.AssertOrder(ref num);
				}
			}
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x00028C6D File Offset: 0x00026E6D
		public void MarkRenderers()
		{
			this.renderersMarked = true;
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x00130ADC File Offset: 0x0012ECDC
		public void UpdateRenderers()
		{
			if (this.Projections != null)
			{
				for (int i = 0; i < this.Projections.Count; i++)
				{
					this.Projections[i].UpdateRenderers();
				}
			}
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x00130B18 File Offset: 0x0012ED18
		public void UpdateRenderers(Projection Projection)
		{
			if (this.Projections != null)
			{
				for (int i = 0; i < this.Projections.Count; i++)
				{
					if (this.Projections[i].projection == Projection)
					{
						this.Projections[i].UpdateRenderers();
						return;
					}
				}
			}
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06003D18 RID: 15640 RVA: 0x00028C76 File Offset: 0x00026E76
		public int ProjectionCount
		{
			get
			{
				return this.Projections.Count;
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06003D19 RID: 15641 RVA: 0x00130B70 File Offset: 0x0012ED70
		public int RendererCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.Projections.Count; i++)
				{
					num += this.Projections[i].instances.Count;
				}
				return num;
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06003D1A RID: 15642 RVA: 0x00028C83 File Offset: 0x00026E83
		public Mesh Cube
		{
			get
			{
				if (this.cube == null)
				{
					this.cube = Resources.Load<Mesh>("Decal");
					this.cube.name = "Projection";
				}
				return this.cube;
			}
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06003D1B RID: 15643 RVA: 0x00028CB9 File Offset: 0x00026EB9
		public Shader DepthShader
		{
			get
			{
				if (this.depthShader == null)
				{
					this.depthShader = Shader.Find("Projection/Internal/Depth");
				}
				return this.depthShader;
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06003D1C RID: 15644 RVA: 0x00028CDF File Offset: 0x00026EDF
		public Shader NormalShader
		{
			get
			{
				if (this.normalShader == null)
				{
					this.normalShader = Shader.Find("Projection/Internal/Normal");
				}
				return this.normalShader;
			}
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06003D1D RID: 15645 RVA: 0x00028D05 File Offset: 0x00026F05
		public Shader MaskShader
		{
			get
			{
				if (this.maskShader == null)
				{
					this.maskShader = Shader.Find("Projection/Internal/Mask");
				}
				return this.maskShader;
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06003D1E RID: 15646 RVA: 0x00028D2B File Offset: 0x00026F2B
		public Material StencilResolveMaterial
		{
			get
			{
				if (this.stencilResolveMaterial == null)
				{
					this.stencilResolveMaterial = new Material(Shader.Find("Projection/Internal/StencilResolve"));
				}
				return this.stencilResolveMaterial;
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06003D1F RID: 15647 RVA: 0x00130BB0 File Offset: 0x0012EDB0
		public Mesh FulllscreenMesh
		{
			get
			{
				if (this.fullscreenMesh == null)
				{
					this.fullscreenMesh = new Mesh();
					Vector3[] vertices = new Vector3[]
					{
						new Vector3(-1f, -1f, 0f),
						new Vector3(3f, -1f, 0f),
						new Vector3(-1f, 3f, 0f)
					};
					this.fullscreenMesh.vertices = vertices;
					int[] indices = new int[]
					{
						0,
						1,
						2
					};
					this.fullscreenMesh.SetIndices(indices, MeshTopology.Triangles, 0, false);
				}
				return this.fullscreenMesh;
			}
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06003D20 RID: 15648 RVA: 0x00028D56 File Offset: 0x00026F56
		public Shader NormalMaskShader
		{
			get
			{
				if (this.normalMaskShader == null)
				{
					this.normalMaskShader = Shader.Find("Projection/Internal/NormalMask");
				}
				return this.normalMaskShader;
			}
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06003D21 RID: 15649 RVA: 0x00028D7C File Offset: 0x00026F7C
		public Shader DepthNormalMaskShader
		{
			get
			{
				if (this.depthNormalMaskShader == null)
				{
					this.depthNormalMaskShader = Shader.Find("Projection/Internal/DepthNormalMask");
				}
				return this.depthNormalMaskShader;
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06003D22 RID: 15650 RVA: 0x00028DA2 File Offset: 0x00026FA2
		public Shader DepthNormalMaskShader_Packed
		{
			get
			{
				if (this.depthNormalMaskShader_Packed == null)
				{
					this.depthNormalMaskShader_Packed = Shader.Find("Projection/Internal/DepthNormalMask_Packed");
				}
				return this.depthNormalMaskShader_Packed;
			}
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06003D23 RID: 15651 RVA: 0x00028DC8 File Offset: 0x00026FC8
		public Material StereoBlitLeft
		{
			get
			{
				if (this.stereoBlitLeft == null)
				{
					this.stereoBlitLeft = new Material(Shader.Find("Projection/Internal/StereoBlitLeft"));
				}
				return this.stereoBlitLeft;
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06003D24 RID: 15652 RVA: 0x00028DF3 File Offset: 0x00026FF3
		public Material StereoBlitRight
		{
			get
			{
				if (this.stereoBlitRight == null)
				{
					this.stereoBlitRight = new Material(Shader.Find("Projection/Internal/StereoBlitRight"));
				}
				return this.stereoBlitRight;
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06003D25 RID: 15653 RVA: 0x00028E1E File Offset: 0x0002701E
		public Material StereoDepthBlitLeft
		{
			get
			{
				if (this.stereoDepthBlitLeft == null)
				{
					this.stereoDepthBlitLeft = new Material(Shader.Find("Projection/Internal/StereoDepthBlitLeft"));
				}
				return this.stereoDepthBlitLeft;
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06003D26 RID: 15654 RVA: 0x00028E49 File Offset: 0x00027049
		public Material StereoDepthBlitRight
		{
			get
			{
				if (this.stereoDepthBlitRight == null)
				{
					this.stereoDepthBlitRight = new Material(Shader.Find("Projection/Internal/StereoDepthBlitRight"));
				}
				return this.stereoDepthBlitRight;
			}
		}

		// Token: 0x06003D27 RID: 15655 RVA: 0x00130C64 File Offset: 0x0012EE64
		private void SetupMaskedMaterials()
		{
			foreach (Material material in this.Settings.Materials)
			{
				material.renderQueue = 2999;
			}
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x00130CC0 File Offset: 0x0012EEC0
		internal CameraData GetData(Camera Camera)
		{
			CameraData cameraData = null;
			if (!this.cameraData.TryGetValue(Camera, out cameraData))
			{
				cameraData = new CameraData();
				this.cameraData[Camera] = cameraData;
			}
			if (cameraData != null)
			{
				if (!cameraData.initialized && Camera.GetComponent<ProjectionBlocker>() == null)
				{
					cameraData.Initialize(Camera, this);
				}
				else if (cameraData.initialized && Camera.GetComponent<ProjectionBlocker>() != null)
				{
					cameraData.Terminate(Camera);
				}
			}
			return cameraData;
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06003D29 RID: 15657 RVA: 0x00130D34 File Offset: 0x0012EF34
		public Camera CustomCamera
		{
			get
			{
				if (this.customCamera == null)
				{
					GameObject gameObject = new GameObject("Custom Camera");
					this.customCamera = gameObject.AddComponent<Camera>();
					gameObject.AddComponent<ProjectionBlocker>();
					gameObject.hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild);
					gameObject.SetActive(false);
					if (Application.isPlaying)
					{
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
				}
				return this.customCamera;
			}
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x00130D90 File Offset: 0x0012EF90
		internal ProjectionPool PoolFromInstance(PoolInstance Instance)
		{
			if (this.Pools == null)
			{
				this.Pools = new Dictionary<int, ProjectionPool>();
			}
			ProjectionPool projectionPool;
			if (!this.Pools.TryGetValue(Instance.id, out projectionPool))
			{
				projectionPool = new ProjectionPool(Instance);
				this.Pools.Add(Instance.id, projectionPool);
			}
			return projectionPool;
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06003D2B RID: 15659 RVA: 0x00028E74 File Offset: 0x00027074
		public ProjectionPool DefaultPool
		{
			get
			{
				return this.PoolFromInstance(this.Settings.pools[0]);
			}
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x00130DE0 File Offset: 0x0012EFE0
		public ProjectionPool GetPool(string Title)
		{
			for (int i = 0; i < this.Settings.pools.Length; i++)
			{
				if (this.settings.pools[i].title == Title)
				{
					return this.PoolFromInstance(this.settings.pools[i]);
				}
			}
			Debug.LogWarning("No valid pool with the title : " + Title + " found. Returning default pool");
			return this.PoolFromInstance(this.settings.pools[0]);
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x00130E5C File Offset: 0x0012F05C
		public ProjectionPool GetPool(int ID)
		{
			for (int i = 0; i < this.Settings.pools.Length; i++)
			{
				if (this.settings.pools[i].id == ID)
				{
					return this.PoolFromInstance(this.settings.pools[i]);
				}
			}
			Debug.LogWarning("No valid pool with the ID : " + ID.ToString() + " found. Returning default pool");
			return this.PoolFromInstance(this.settings.pools[0]);
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x00130ED8 File Offset: 0x0012F0D8
		private void Initialize()
		{
			this.depthFormat = RenderTextureFormat.Depth;
			this.normalFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB2101010) ? RenderTextureFormat.ARGB2101010 : RenderTextureFormat.ARGB32);
			this.maskFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32) ? RenderTextureFormat.ARGB32 : RenderTextureFormat.ARGB32);
			this.stencilMaskFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8) ? RenderTextureFormat.R8 : RenderTextureFormat.ARGB32);
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(this.SuperLateUpdate));
			Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(this.PreRender));
			if (this.Projections == null)
			{
				this.Projections = new List<ProjectionData>();
			}
			else
			{
				for (int i = 0; i < this.Projections.Count; i++)
				{
					this.Projections[i].EnableRenderers();
				}
			}
			this.SetupMaskedMaterials();
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x00130FA8 File Offset: 0x0012F1A8
		public void PublicReleaseRenderTextures()
		{
			Camera[] allCameras = Camera.allCameras;
			for (int i = 0; i < allCameras.Length; i++)
			{
				this.CleanCamera(allCameras[i]);
			}
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x00130FD4 File Offset: 0x0012F1D4
		public void CleanCamera(Camera c)
		{
			if (c != null)
			{
				CameraData data = this.GetData(c);
				if (data != null)
				{
					data.ReleaseTextures();
				}
			}
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x00130FFC File Offset: 0x0012F1FC
		private void Terminate()
		{
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(this.SuperLateUpdate));
			Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(this.PreRender));
			foreach (KeyValuePair<Camera, CameraData> keyValuePair in this.cameraData)
			{
				keyValuePair.Value.Terminate(keyValuePair.Key);
			}
			this.cameraData.Clear();
			if (this.Projections != null)
			{
				for (int i = 0; i < this.Projections.Count; i++)
				{
					this.Projections[i].DisableRenderers();
				}
			}
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x00028E89 File Offset: 0x00027089
		private void LateUpdate()
		{
			this.UpdateSystemPath();
			this.UpdateProjectionData();
			this.OrderRenderers();
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x001310D8 File Offset: 0x0012F2D8
		private void SuperLateUpdate(Camera Camera)
		{
			if (this.FireInCulling)
			{
				CameraData data = this.GetData(Camera);
				if (data.initialized && (Camera.cameraType == CameraType.SceneView || Camera.cameraType == CameraType.Preview || Camera.isActiveAndEnabled))
				{
					data.Update(Camera, this);
				}
			}
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x00131120 File Offset: 0x0012F320
		private void PreRender(Camera Camera)
		{
			CameraData data = this.GetData(Camera);
			if (data.initialized && (Camera.cameraType == CameraType.SceneView || Camera.cameraType == CameraType.Preview || Camera.isActiveAndEnabled))
			{
				if (!this.FireInCulling)
				{
					data.Update(Camera, this);
				}
				data.AssignGlobalProperties(Camera);
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06003D35 RID: 15669 RVA: 0x00131170 File Offset: 0x0012F370
		public static string DebugLog
		{
			get
			{
				string text = "Debug Information (Copy and Paste) \r\n";
				text += "\r\nGeneral\r\n";
				text = text + "OS : " + SystemInfo.operatingSystem + "\r\n";
				text = text + "Graphics device : " + SystemInfo.graphicsDeviceName + "\r\n";
				text = text + "Graphics API : " + SystemInfo.graphicsDeviceType.ToString() + "\r\n";
				Camera main = Camera.main;
				if (main != null)
				{
					text += "\r\nCamera\r\n";
					text = text + "Rendering path : " + main.actualRenderingPath.ToString() + "\r\n";
					text = text + "Is orthographic : " + main.orthographic.ToString() + "\r\n";
					text += "\r\nShader Replacement\r\n";
					text = text + "Method : " + DynamicDecals.System.GetData(main).replacement.ToString() + "\r\n";
				}
				else
				{
					text += "\r\nMain camera not found\r\nPlease tag your main camera\r\n";
				}
				if (XRSettings.enabled)
				{
					text = text + "\r\nVirtualReality : " + XRSettings.isDeviceActive.ToString() + "\r\n";
					text = text + "VR API : " + XRSettings.loadedDeviceName + "\r\n";
				}
				return text;
			}
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x00028E9D File Offset: 0x0002709D
		public static void DebugInDevelopmentBuild()
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log(DynamicDecals.DebugLog);
			}
		}

		// Token: 0x040039E2 RID: 14818
		private static DynamicDecals system;

		// Token: 0x040039E3 RID: 14819
		private DynamicDecalSettings settings;

		// Token: 0x040039E4 RID: 14820
		public SystemPath renderingPath;

		// Token: 0x040039E5 RID: 14821
		private bool shaderReplacement = true;

		// Token: 0x040039E6 RID: 14822
		internal RenderTextureFormat depthFormat;

		// Token: 0x040039E7 RID: 14823
		internal RenderTextureFormat normalFormat;

		// Token: 0x040039E8 RID: 14824
		internal RenderTextureFormat maskFormat;

		// Token: 0x040039E9 RID: 14825
		internal RenderTextureFormat stencilMaskFormat;

		// Token: 0x040039EA RID: 14826
		private List<ProjectionData> Projections;

		// Token: 0x040039EB RID: 14827
		private bool renderersMarked;

		// Token: 0x040039EC RID: 14828
		private Mesh cube;

		// Token: 0x040039ED RID: 14829
		private Shader depthShader;

		// Token: 0x040039EE RID: 14830
		private Shader normalShader;

		// Token: 0x040039EF RID: 14831
		private Shader maskShader;

		// Token: 0x040039F0 RID: 14832
		private Material stencilResolveMaterial;

		// Token: 0x040039F1 RID: 14833
		private Mesh fullscreenMesh;

		// Token: 0x040039F2 RID: 14834
		private Shader depthNormalShader;

		// Token: 0x040039F3 RID: 14835
		private Shader normalMaskShader;

		// Token: 0x040039F4 RID: 14836
		private Shader depthNormalMaskShader;

		// Token: 0x040039F5 RID: 14837
		private Shader depthNormalMaskShader_Packed;

		// Token: 0x040039F6 RID: 14838
		private Material stereoBlitLeft;

		// Token: 0x040039F7 RID: 14839
		private Material stereoBlitRight;

		// Token: 0x040039F8 RID: 14840
		private Material stereoDepthBlitLeft;

		// Token: 0x040039F9 RID: 14841
		private Material stereoDepthBlitRight;

		// Token: 0x040039FA RID: 14842
		internal Dictionary<Camera, CameraData> cameraData = new Dictionary<Camera, CameraData>();

		// Token: 0x040039FB RID: 14843
		private Camera customCamera;

		// Token: 0x040039FC RID: 14844
		private Dictionary<int, ProjectionPool> Pools;
	}
}
