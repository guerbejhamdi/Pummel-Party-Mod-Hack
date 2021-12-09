using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace LlockhamIndustries.Decals
{
	// Token: 0x020008A4 RID: 2212
	[ExecuteInEditMode]
	public class ProjectionRenderer : MonoBehaviour
	{
		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06003EA4 RID: 16036 RVA: 0x0002A2DE File Offset: 0x000284DE
		// (set) Token: 0x06003EA5 RID: 16037 RVA: 0x0002A2E6 File Offset: 0x000284E6
		public ProjectionProperty[] Properties
		{
			get
			{
				return this.properties;
			}
			set
			{
				if (value != null)
				{
					this.properties = (ProjectionProperty[])value.Clone();
				}
				else
				{
					this.properties = null;
				}
				this.MarkProperties(false);
			}
		}

		// Token: 0x06003EA6 RID: 16038 RVA: 0x00134B64 File Offset: 0x00132D64
		public void ResetProperties(bool UpdateImmediately = false)
		{
			this.tiling = Vector2.one;
			this.offset = Vector2.zero;
			this.maskMethod = MaskMethod.DrawOnEverythingExcept;
			this.masks = new bool[4];
			if (this.projection != null)
			{
				this.properties = (ProjectionProperty[])this.projection.Properties.Clone();
			}
			else
			{
				this.properties = null;
			}
			this.MarkProperties(UpdateImmediately);
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x00134BD4 File Offset: 0x00132DD4
		public void UpdateProperties()
		{
			if (this.meshRenderer != null && this.marked && this.Projection != null)
			{
				this.UpdateRendererBlock(this.Properties, this.Projection.Properties);
				this.meshRenderer.SetPropertyBlock(this.block);
				this.marked = false;
			}
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x0002A30C File Offset: 0x0002850C
		public void MarkProperties(bool UpdateImmediately = false)
		{
			this.marked = true;
			if (UpdateImmediately && this.meshRenderer != null)
			{
				this.UpdateProperties();
			}
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x00134C34 File Offset: 0x00132E34
		public void SetFloat(int PropertyIndex, float Float)
		{
			if ((this.properties[PropertyIndex].type == PropertyType.Float || this.properties[PropertyIndex].type == PropertyType.Combo) && this.properties[PropertyIndex].value != Float)
			{
				this.properties[PropertyIndex].value = Float;
				this.properties[PropertyIndex].enabled = true;
				this.MarkProperties(false);
			}
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x00134CA8 File Offset: 0x00132EA8
		public void SetColor(int PropertyIndex, Color Color)
		{
			if ((this.properties[PropertyIndex].type == PropertyType.Color || this.properties[PropertyIndex].type == PropertyType.Combo) && this.properties[PropertyIndex].color != Color)
			{
				this.properties[PropertyIndex].color = Color;
				this.properties[PropertyIndex].enabled = true;
				this.MarkProperties(false);
			}
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06003EAB RID: 16043 RVA: 0x0002A32C File Offset: 0x0002852C
		// (set) Token: 0x06003EAC RID: 16044 RVA: 0x0002A334 File Offset: 0x00028534
		public Vector2 Tiling
		{
			get
			{
				return this.tiling;
			}
			set
			{
				if (this.tiling != value)
				{
					this.tiling = value;
					this.MarkProperties(false);
				}
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06003EAD RID: 16045 RVA: 0x0002A352 File Offset: 0x00028552
		// (set) Token: 0x06003EAE RID: 16046 RVA: 0x0002A35A File Offset: 0x0002855A
		public Vector2 Offset
		{
			get
			{
				return this.offset;
			}
			set
			{
				if (this.offset != value)
				{
					this.offset = value;
					this.MarkProperties(false);
				}
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06003EAF RID: 16047 RVA: 0x0002A378 File Offset: 0x00028578
		// (set) Token: 0x06003EB0 RID: 16048 RVA: 0x0002A380 File Offset: 0x00028580
		public MaskMethod MaskMethod
		{
			get
			{
				return this.maskMethod;
			}
			set
			{
				if (this.maskMethod != value)
				{
					this.maskMethod = value;
					this.MarkProperties(false);
				}
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06003EB1 RID: 16049 RVA: 0x0002A399 File Offset: 0x00028599
		// (set) Token: 0x06003EB2 RID: 16050 RVA: 0x0002A3A3 File Offset: 0x000285A3
		public bool MaskLayer1
		{
			get
			{
				return this.masks[0];
			}
			set
			{
				if (this.masks[0] != value)
				{
					this.masks[0] = value;
					this.MarkProperties(false);
				}
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06003EB3 RID: 16051 RVA: 0x0002A3C0 File Offset: 0x000285C0
		// (set) Token: 0x06003EB4 RID: 16052 RVA: 0x0002A3CA File Offset: 0x000285CA
		public bool MaskLayer2
		{
			get
			{
				return this.masks[1];
			}
			set
			{
				if (this.masks[1] != value)
				{
					this.masks[1] = value;
					this.MarkProperties(false);
				}
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06003EB5 RID: 16053 RVA: 0x0002A3E7 File Offset: 0x000285E7
		// (set) Token: 0x06003EB6 RID: 16054 RVA: 0x0002A3F1 File Offset: 0x000285F1
		public bool MaskLayer3
		{
			get
			{
				return this.masks[2];
			}
			set
			{
				if (this.masks[2] != value)
				{
					this.masks[2] = value;
					this.MarkProperties(false);
				}
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06003EB7 RID: 16055 RVA: 0x0002A40E File Offset: 0x0002860E
		// (set) Token: 0x06003EB8 RID: 16056 RVA: 0x0002A418 File Offset: 0x00028618
		public bool MaskLayer4
		{
			get
			{
				return this.masks[3];
			}
			set
			{
				if (this.masks[3] != value)
				{
					this.masks[3] = value;
					this.MarkProperties(false);
				}
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06003EB9 RID: 16057 RVA: 0x0002A435 File Offset: 0x00028635
		// (set) Token: 0x06003EBA RID: 16058 RVA: 0x0002A45F File Offset: 0x0002865F
		public Projection Projection
		{
			get
			{
				if (base.gameObject.activeInHierarchy && this.active != null)
				{
					return this.active;
				}
				return this.projection;
			}
			set
			{
				if (this.projection != value)
				{
					this.projection = value;
					this.ChangeProjection();
				}
			}
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x00134D20 File Offset: 0x00132F20
		public void ChangeProjection()
		{
			if (this.active != this.projection)
			{
				if (base.gameObject.activeInHierarchy && base.enabled && this.active != null)
				{
					this.Deregister();
				}
				this.active = this.projection;
				if (base.gameObject.activeInHierarchy && base.enabled && this.active != null)
				{
					this.Register();
				}
				this.tiling = Vector2.one;
				this.offset = Vector2.zero;
				if (this.active != null)
				{
					this.Properties = this.active.Properties;
				}
				else
				{
					this.Properties = null;
				}
			}
			this.UpdateProjection();
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x00134DE8 File Offset: 0x00132FE8
		public void UpdateProjection()
		{
			if (this.meshRenderer != null)
			{
				if (this.Projection != null && this.Projection.Valid)
				{
					this.meshRenderer.gameObject.SetActive(true);
					this.meshRenderer.sharedMaterial = this.Projection.Mat;
					this.UpdateRendererBlock(this.Properties, this.Projection.Properties);
					this.meshRenderer.SetPropertyBlock(this.block);
					this.marked = false;
					return;
				}
				this.meshRenderer.gameObject.SetActive(false);
				this.meshRenderer.sharedMaterial = null;
				if (this.block != null)
				{
					this.block.Clear();
				}
				this.meshRenderer.SetPropertyBlock(this.block);
			}
		}

		// Token: 0x06003EBD RID: 16061 RVA: 0x0002A47C File Offset: 0x0002867C
		private bool Register()
		{
			return this != null && DynamicDecals.System.Register(this);
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x0002A494 File Offset: 0x00028694
		private void Deregister()
		{
			if (this != null)
			{
				DynamicDecals.System.Deregister(this);
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06003EBF RID: 16063 RVA: 0x0002A4AA File Offset: 0x000286AA
		public MeshRenderer Renderer
		{
			get
			{
				return this.meshRenderer;
			}
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x00134EBC File Offset: 0x001330BC
		public void InitializeRenderer(bool Active)
		{
			if (this.meshRenderer == null)
			{
				foreach (object obj in base.transform)
				{
					Transform transform = (Transform)obj;
					if (transform.name == "Renderer")
					{
						this.meshRenderer = transform.GetComponent<MeshRenderer>();
						break;
					}
				}
			}
			if (this.meshRenderer == null)
			{
				GameObject gameObject = new GameObject("Renderer");
				gameObject.transform.SetParent(base.transform, false);
				gameObject.layer = base.gameObject.layer;
				gameObject.hideFlags = (HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild);
				gameObject.AddComponent<MeshFilter>().sharedMesh = DynamicDecals.System.Cube;
				this.meshRenderer = gameObject.AddComponent<MeshRenderer>();
				this.meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				this.meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;
				this.meshRenderer.lightProbeUsage = LightProbeUsage.BlendProbes;
			}
			this.meshRenderer.gameObject.SetActive(Active);
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x0002A4B2 File Offset: 0x000286B2
		public void TerminateRenderer()
		{
			this.meshRenderer.gameObject.SetActive(false);
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x00134FD8 File Offset: 0x001331D8
		private void UpdateRendererBlock(ProjectionProperty[] Local, ProjectionProperty[] Global)
		{
			if (this.block == null)
			{
				this.block = new MaterialPropertyBlock();
			}
			else
			{
				this.block.Clear();
			}
			this.block.SetVector(this._TilingOffset, new Vector4(this.Tiling.x, this.Tiling.y, this.Offset.x, this.Offset.y));
			MaskMethod maskMethod = this.maskMethod;
			if (maskMethod != MaskMethod.DrawOnEverythingExcept)
			{
				if (maskMethod == MaskMethod.OnlyDrawOn)
				{
					this.block.SetFloat(this._MaskBase, 0f);
					Color clear = Color.clear;
					clear.r = (this.masks[0] ? 1f : 0.5f);
					clear.g = (this.masks[1] ? 1f : 0.5f);
					clear.b = (this.masks[2] ? 1f : 0.5f);
					clear.a = (this.masks[3] ? 1f : 0.5f);
					this.block.SetVector(this._MaskLayers, clear);
				}
			}
			else
			{
				this.block.SetFloat(this._MaskBase, 1f);
				Color clear2 = Color.clear;
				clear2.r = (this.masks[0] ? 0f : 0.5f);
				clear2.g = (this.masks[1] ? 0f : 0.5f);
				clear2.b = (this.masks[2] ? 0f : 0.5f);
				clear2.a = (this.masks[3] ? 0f : 0.5f);
				this.block.SetVector(this._MaskLayers, clear2);
			}
			for (int i = 0; i < Local.Length; i++)
			{
				if (Local[i].type == PropertyType.Float)
				{
					float value = Local[i].enabled ? Local[i].value : Global[i].value;
					this.block.SetFloat(Global[i].nameID, value);
				}
				if (Local[i].type == PropertyType.Color)
				{
					Color value2 = Local[i].enabled ? Local[i].color : Global[i].color;
					this.block.SetColor(Global[i].nameID, value2);
				}
				if (Local[i].type == PropertyType.Combo)
				{
					Color value3 = Local[i].enabled ? (Local[i].color * Local[i].value) : (Global[i].color * Global[i].value);
					this.block.SetColor(Global[i].nameID, value3);
				}
			}
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x0002A4C5 File Offset: 0x000286C5
		private void OnEnable()
		{
			this._TilingOffset = Shader.PropertyToID("_TilingOffset");
			this._MaskBase = Shader.PropertyToID("_MaskBase");
			this._MaskLayers = Shader.PropertyToID("_MaskLayers");
			this.Initialize();
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x0002A4FD File Offset: 0x000286FD
		private void OnDisable()
		{
			this.Terminate();
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x001352DC File Offset: 0x001334DC
		private void Initialize()
		{
			if (this.projection != null)
			{
				this.active = this.projection;
				bool flag = this.Register();
				this.InitializeRenderer(flag);
			}
			else
			{
				this.InitializeRenderer(false);
			}
			this.UpdateProjection();
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x0002A505 File Offset: 0x00028705
		private void Terminate()
		{
			if (this.projection != null)
			{
				this.Deregister();
			}
			this.TerminateRenderer();
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06003EC7 RID: 16071 RVA: 0x0002A521 File Offset: 0x00028721
		// (set) Token: 0x06003EC8 RID: 16072 RVA: 0x0002A529 File Offset: 0x00028729
		public ProjectionData Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
			}
		}

		// Token: 0x06003EC9 RID: 16073 RVA: 0x0002A532 File Offset: 0x00028732
		public void MoveToTop()
		{
			if (this.data != null)
			{
				this.data.MoveToTop(this);
			}
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x0002A548 File Offset: 0x00028748
		public void MoveToBottom()
		{
			if (this.data != null)
			{
				this.data.MoveToBottom(this);
			}
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06003ECB RID: 16075 RVA: 0x0002A55E File Offset: 0x0002875E
		public ProjectionPool Pool
		{
			get
			{
				if (this.poolItem != null)
				{
					return this.poolItem.Pool;
				}
				return null;
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06003ECC RID: 16076 RVA: 0x0002A575 File Offset: 0x00028775
		// (set) Token: 0x06003ECD RID: 16077 RVA: 0x0002A57D File Offset: 0x0002877D
		public PoolItem PoolItem
		{
			get
			{
				return this.poolItem;
			}
			set
			{
				this.poolItem = value;
			}
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x00135320 File Offset: 0x00133520
		public float CheckIntersecting(Vector3 Point)
		{
			Vector3 vector = base.transform.InverseTransformPoint(Point);
			return Mathf.Clamp01(2f * (0.5f - Mathf.Max(Mathf.Max(Mathf.Abs(vector.x), Mathf.Abs(vector.y)), Mathf.Abs(vector.z))));
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x0002A586 File Offset: 0x00028786
		public void Destroy()
		{
			if (this.poolItem != null)
			{
				this.poolItem.Return();
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04003AF1 RID: 15089
		private MaterialPropertyBlock block;

		// Token: 0x04003AF2 RID: 15090
		private bool marked = true;

		// Token: 0x04003AF3 RID: 15091
		[SerializeField]
		private Projection projection;

		// Token: 0x04003AF4 RID: 15092
		private Projection active;

		// Token: 0x04003AF5 RID: 15093
		[SerializeField]
		private ProjectionProperty[] properties;

		// Token: 0x04003AF6 RID: 15094
		[SerializeField]
		private Vector2 tiling;

		// Token: 0x04003AF7 RID: 15095
		[SerializeField]
		private Vector2 offset;

		// Token: 0x04003AF8 RID: 15096
		[SerializeField]
		protected MaskMethod maskMethod;

		// Token: 0x04003AF9 RID: 15097
		[SerializeField]
		protected bool[] masks = new bool[4];

		// Token: 0x04003AFA RID: 15098
		private MeshRenderer meshRenderer;

		// Token: 0x04003AFB RID: 15099
		private ProjectionData data;

		// Token: 0x04003AFC RID: 15100
		private PoolItem poolItem;

		// Token: 0x04003AFD RID: 15101
		public int _TilingOffset;

		// Token: 0x04003AFE RID: 15102
		public int _MaskBase;

		// Token: 0x04003AFF RID: 15103
		public int _MaskLayers;
	}
}
