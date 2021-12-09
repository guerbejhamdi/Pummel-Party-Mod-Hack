using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x0200087A RID: 2170
	[Serializable]
	public abstract class Projection : ScriptableObject
	{
		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06003DB4 RID: 15796 RVA: 0x00132830 File Offset: 0x00130A30
		public Material Mat
		{
			get
			{
				if (this.SupportedRendering == RenderingPaths.Forward || DynamicDecals.System.SystemPath == SystemPath.Forward || this.ForceForward)
				{
					return this.Forward;
				}
				TransparencyType transparencyType = this.TransparencyType;
				if (transparencyType == TransparencyType.Cutout)
				{
					return this.DeferredOpaque;
				}
				if (transparencyType != TransparencyType.Blend)
				{
					return null;
				}
				return this.DeferredTransparent;
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06003DB5 RID: 15797 RVA: 0x00132880 File Offset: 0x00130A80
		private Material Forward
		{
			get
			{
				ShaderReplacementType replacement = DynamicDecals.System.Settings.Replacement;
				if (replacement == ShaderReplacementType.VR)
				{
					return this.PackedForward;
				}
				if (replacement == ShaderReplacementType.Mobile)
				{
					return this.MobileForward;
				}
				return this.StandardForward;
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06003DB6 RID: 15798 RVA: 0x001328BC File Offset: 0x00130ABC
		private Material DeferredOpaque
		{
			get
			{
				ShaderReplacementType replacement = DynamicDecals.System.Settings.Replacement;
				if (replacement == ShaderReplacementType.VR)
				{
					return this.PackedDeferredOpaque;
				}
				if (replacement == ShaderReplacementType.Mobile)
				{
					return this.MobileDeferredOpaque;
				}
				return this.StandardDeferredOpaque;
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06003DB7 RID: 15799 RVA: 0x001328F8 File Offset: 0x00130AF8
		private Material DeferredTransparent
		{
			get
			{
				ShaderReplacementType replacement = DynamicDecals.System.Settings.Replacement;
				if (replacement == ShaderReplacementType.VR)
				{
					return this.PackedDeferredTransparent;
				}
				if (replacement == ShaderReplacementType.Mobile)
				{
					return this.MobileDeferredTransparent;
				}
				return this.StandardDeferredTransparent;
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06003DB8 RID: 15800 RVA: 0x000053AE File Offset: 0x000035AE
		public virtual Material MobileForward
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06003DB9 RID: 15801 RVA: 0x000053AE File Offset: 0x000035AE
		public virtual Material MobileDeferredOpaque
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06003DBA RID: 15802 RVA: 0x000053AE File Offset: 0x000035AE
		public virtual Material MobileDeferredTransparent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06003DBB RID: 15803 RVA: 0x000053AE File Offset: 0x000035AE
		public virtual Material StandardForward
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06003DBC RID: 15804 RVA: 0x000053AE File Offset: 0x000035AE
		public virtual Material StandardDeferredOpaque
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06003DBD RID: 15805 RVA: 0x000053AE File Offset: 0x000035AE
		public virtual Material StandardDeferredTransparent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06003DBE RID: 15806 RVA: 0x000053AE File Offset: 0x000035AE
		public virtual Material PackedForward
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06003DBF RID: 15807 RVA: 0x000053AE File Offset: 0x000035AE
		public virtual Material PackedDeferredOpaque
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06003DC0 RID: 15808 RVA: 0x000053AE File Offset: 0x000035AE
		public virtual Material PackedDeferredTransparent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x00132934 File Offset: 0x00130B34
		protected Material MaterialFromShader(Shader p_Shader)
		{
			if (this.material == null)
			{
				this.material = new Material(p_Shader);
				this.material.hideFlags = HideFlags.DontSave;
				this.Apply(this.material);
			}
			if (this.material.shader != p_Shader)
			{
				this.material.shader = p_Shader;
			}
			return this.material;
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06003DC2 RID: 15810
		public abstract RenderingPaths SupportedRendering { get; }

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06003DC3 RID: 15811 RVA: 0x00029722 File Offset: 0x00027922
		public bool Valid
		{
			get
			{
				return this.SupportedRendering != RenderingPaths.Deferred || DynamicDecals.System.SystemPath != SystemPath.Forward;
			}
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x0013299C File Offset: 0x00130B9C
		protected void DestroyMaterial()
		{
			Material mat = this.Mat;
			if (mat != null)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(mat);
					return;
				}
				UnityEngine.Object.DestroyImmediate(mat, true);
			}
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x001329D0 File Offset: 0x00130BD0
		protected void UpdateMaterial()
		{
			Material mat = this.Mat;
			if (mat != null)
			{
				this.Apply(mat);
			}
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x001329F4 File Offset: 0x00130BF4
		protected virtual void Apply(Material Material)
		{
			ProjectionType projectionType = this.type;
			if (projectionType != ProjectionType.Decal)
			{
				if (projectionType == ProjectionType.OmniDecal)
				{
					Material.EnableKeyword("_Omni");
				}
			}
			else
			{
				Material.DisableKeyword("_Omni");
			}
			Material.enableInstancing = this.Instanced;
			TransparencyType transparencyType = this.transparencyType;
			if (transparencyType != TransparencyType.Cutout)
			{
				if (transparencyType == TransparencyType.Blend)
				{
					Material.DisableKeyword("_AlphaTest");
				}
			}
			else
			{
				Material.EnableKeyword("_AlphaTest");
			}
			Material.SetFloat(this._Cutoff, this.cutoff);
			Vector4 value = new Vector4(this.tiling.x, this.tiling.y, this.offset.x, this.offset.y);
			Material.SetVector(this._TilingOffset, value);
			if (this.masks.Length == 4)
			{
				MaskMethod maskMethod = this.maskMethod;
				if (maskMethod != MaskMethod.DrawOnEverythingExcept)
				{
					if (maskMethod == MaskMethod.OnlyDrawOn)
					{
						Material.SetFloat(this._MaskBase, 0f);
						Color clear = Color.clear;
						clear.r = (this.masks[0] ? 1f : 0.5f);
						clear.g = (this.masks[1] ? 1f : 0.5f);
						clear.b = (this.masks[2] ? 1f : 0.5f);
						clear.a = (this.masks[3] ? 1f : 0.5f);
						Material.SetVector(this._MaskLayers, clear);
					}
				}
				else
				{
					Material.SetFloat(this._MaskBase, 1f);
					Color clear2 = Color.clear;
					clear2.r = (this.masks[0] ? 0f : 0.5f);
					clear2.g = (this.masks[1] ? 0f : 0.5f);
					clear2.b = (this.masks[2] ? 0f : 0.5f);
					clear2.a = (this.masks[3] ? 0f : 0.5f);
					Material.SetVector(this._MaskLayers, clear2);
				}
			}
			float value2 = Mathf.Cos(0.017453292f * this.projectionLimit);
			Material.SetFloat(this._NormalCutoff, value2);
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06003DC7 RID: 15815 RVA: 0x0002973C File Offset: 0x0002793C
		// (set) Token: 0x06003DC8 RID: 15816 RVA: 0x00029744 File Offset: 0x00027944
		public ProjectionType ProjectionType
		{
			get
			{
				return this.type;
			}
			set
			{
				if (this.type != value)
				{
					this.type = value;
					this.Mark(false);
				}
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06003DC9 RID: 15817 RVA: 0x0002975D File Offset: 0x0002795D
		// (set) Token: 0x06003DCA RID: 15818 RVA: 0x00029765 File Offset: 0x00027965
		public int Priority
		{
			get
			{
				return this.priority;
			}
			set
			{
				if (this.priority != value)
				{
					this.priority = value;
					this.Mark(false);
				}
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06003DCB RID: 15819 RVA: 0x0002977E File Offset: 0x0002797E
		// (set) Token: 0x06003DCC RID: 15820 RVA: 0x00029786 File Offset: 0x00027986
		public TransparencyType TransparencyType
		{
			get
			{
				return this.transparencyType;
			}
			set
			{
				if (this.transparencyType != value)
				{
					this.transparencyType = value;
					this.Mark(false);
				}
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06003DCD RID: 15821 RVA: 0x0002979F File Offset: 0x0002799F
		// (set) Token: 0x06003DCE RID: 15822 RVA: 0x000297A7 File Offset: 0x000279A7
		public float Cutoff
		{
			get
			{
				return this.cutoff;
			}
			set
			{
				if (this.cutoff != value)
				{
					this.cutoff = value;
					this.Mark(false);
				}
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06003DCF RID: 15823 RVA: 0x000297C0 File Offset: 0x000279C0
		// (set) Token: 0x06003DD0 RID: 15824 RVA: 0x000297C8 File Offset: 0x000279C8
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
					this.Mark(false);
				}
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06003DD1 RID: 15825 RVA: 0x000297E6 File Offset: 0x000279E6
		// (set) Token: 0x06003DD2 RID: 15826 RVA: 0x000297EE File Offset: 0x000279EE
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
					this.Mark(false);
				}
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06003DD3 RID: 15827 RVA: 0x0002980C File Offset: 0x00027A0C
		// (set) Token: 0x06003DD4 RID: 15828 RVA: 0x00029814 File Offset: 0x00027A14
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
					this.Mark(false);
				}
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06003DD5 RID: 15829 RVA: 0x0002982D File Offset: 0x00027A2D
		// (set) Token: 0x06003DD6 RID: 15830 RVA: 0x00029837 File Offset: 0x00027A37
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
					this.Mark(false);
				}
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06003DD7 RID: 15831 RVA: 0x00029854 File Offset: 0x00027A54
		// (set) Token: 0x06003DD8 RID: 15832 RVA: 0x0002985E File Offset: 0x00027A5E
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
					this.Mark(false);
				}
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06003DD9 RID: 15833 RVA: 0x0002987B File Offset: 0x00027A7B
		// (set) Token: 0x06003DDA RID: 15834 RVA: 0x00029885 File Offset: 0x00027A85
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
					this.Mark(false);
				}
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06003DDB RID: 15835 RVA: 0x000298A2 File Offset: 0x00027AA2
		// (set) Token: 0x06003DDC RID: 15836 RVA: 0x000298AC File Offset: 0x00027AAC
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
					this.Mark(false);
				}
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06003DDD RID: 15837 RVA: 0x000298C9 File Offset: 0x00027AC9
		// (set) Token: 0x06003DDE RID: 15838 RVA: 0x000298D1 File Offset: 0x00027AD1
		public float ProjectionLimit
		{
			get
			{
				return this.projectionLimit;
			}
			set
			{
				if (this.projectionLimit != value)
				{
					this.projectionLimit = Mathf.Clamp(value, 0f, 180f);
					this.Mark(false);
				}
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06003DDF RID: 15839 RVA: 0x000298F9 File Offset: 0x00027AF9
		// (set) Token: 0x06003DE0 RID: 15840 RVA: 0x0002990F File Offset: 0x00027B0F
		public bool Instanced
		{
			get
			{
				return DynamicDecals.System.Instanced && this.instanced;
			}
			set
			{
				this.instanced = value;
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06003DE1 RID: 15841
		public abstract int InstanceLimit { get; }

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06003DE2 RID: 15842 RVA: 0x00029918 File Offset: 0x00027B18
		// (set) Token: 0x06003DE3 RID: 15843 RVA: 0x00029920 File Offset: 0x00027B20
		public bool ForceForward
		{
			get
			{
				return this.forceForward;
			}
			set
			{
				this.forceForward = value;
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06003DE4 RID: 15844 RVA: 0x00029929 File Offset: 0x00027B29
		public ProjectionProperty[] Properties
		{
			get
			{
				this.UpdateProperties();
				return this.properties;
			}
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void UpdateProperties()
		{
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x00029937 File Offset: 0x00027B37
		protected virtual void OnEnable()
		{
			this.GenerateIDs();
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x0002993F File Offset: 0x00027B3F
		protected virtual void OnDisable()
		{
			this.DestroyMaterial();
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x00029947 File Offset: 0x00027B47
		public void Update()
		{
			if (this.marked)
			{
				this.UpdateProperties();
				this.UpdateMaterial();
				this.marked = false;
			}
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x00029964 File Offset: 0x00027B64
		public void Mark(bool UpdateImmediately = false)
		{
			this.marked = true;
			if (UpdateImmediately)
			{
				this.Update();
			}
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x00132C38 File Offset: 0x00130E38
		protected virtual void GenerateIDs()
		{
			this._Cutoff = Shader.PropertyToID("_Cutoff");
			this._TilingOffset = Shader.PropertyToID("_TilingOffset");
			this._MaskBase = Shader.PropertyToID("_MaskBase");
			this._MaskLayers = Shader.PropertyToID("_MaskLayers");
			this._NormalCutoff = Shader.PropertyToID("_NormalCutoff");
		}

		// Token: 0x04003A3D RID: 14909
		private bool marked;

		// Token: 0x04003A3E RID: 14910
		protected Material material;

		// Token: 0x04003A3F RID: 14911
		protected ProjectionProperty[] properties;

		// Token: 0x04003A40 RID: 14912
		[SerializeField]
		protected ProjectionType type;

		// Token: 0x04003A41 RID: 14913
		[SerializeField]
		protected bool instanced;

		// Token: 0x04003A42 RID: 14914
		[SerializeField]
		protected bool forceForward;

		// Token: 0x04003A43 RID: 14915
		[SerializeField]
		protected int priority;

		// Token: 0x04003A44 RID: 14916
		[SerializeField]
		protected TransparencyType transparencyType;

		// Token: 0x04003A45 RID: 14917
		[SerializeField]
		protected float cutoff = 0.2f;

		// Token: 0x04003A46 RID: 14918
		[SerializeField]
		protected Vector2 tiling;

		// Token: 0x04003A47 RID: 14919
		[SerializeField]
		protected Vector2 offset;

		// Token: 0x04003A48 RID: 14920
		[SerializeField]
		protected MaskMethod maskMethod;

		// Token: 0x04003A49 RID: 14921
		[SerializeField]
		protected bool[] masks = new bool[4];

		// Token: 0x04003A4A RID: 14922
		[SerializeField]
		protected float projectionLimit = 80f;

		// Token: 0x04003A4B RID: 14923
		public int _Cutoff;

		// Token: 0x04003A4C RID: 14924
		public int _TilingOffset;

		// Token: 0x04003A4D RID: 14925
		public int _MaskBase;

		// Token: 0x04003A4E RID: 14926
		public int _MaskLayers;

		// Token: 0x04003A4F RID: 14927
		protected int _NormalCutoff;
	}
}
