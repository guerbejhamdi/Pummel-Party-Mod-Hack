using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000884 RID: 2180
	[Serializable]
	public class Specular : Projection
	{
		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06003E20 RID: 15904 RVA: 0x00029C27 File Offset: 0x00027E27
		public override Material MobileForward
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Mobile/Specular/Forward"));
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06003E21 RID: 15905 RVA: 0x00029C39 File Offset: 0x00027E39
		public override Material MobileDeferredOpaque
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Mobile/Specular/DeferredOpaque"));
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06003E22 RID: 15906 RVA: 0x00029C4B File Offset: 0x00027E4B
		public override Material MobileDeferredTransparent
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Mobile/Specular/DeferredTransparent"));
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06003E23 RID: 15907 RVA: 0x00029C5D File Offset: 0x00027E5D
		public override Material StandardForward
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Standard/Specular/Forward"));
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06003E24 RID: 15908 RVA: 0x00029C6F File Offset: 0x00027E6F
		public override Material StandardDeferredOpaque
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Standard/Specular/DeferredOpaque"));
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06003E25 RID: 15909 RVA: 0x00029C81 File Offset: 0x00027E81
		public override Material StandardDeferredTransparent
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Standard/Specular/DeferredTransparent"));
			}
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06003E26 RID: 15910 RVA: 0x00029C93 File Offset: 0x00027E93
		public override Material PackedForward
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Packed/Specular/Forward"));
			}
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06003E27 RID: 15911 RVA: 0x00029CA5 File Offset: 0x00027EA5
		public override Material PackedDeferredOpaque
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Packed/Specular/DeferredOpaque"));
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06003E28 RID: 15912 RVA: 0x00029CB7 File Offset: 0x00027EB7
		public override Material PackedDeferredTransparent
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Packed/Specular/DeferredTransparent"));
			}
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06003E29 RID: 15913 RVA: 0x0000539F File Offset: 0x0000359F
		public override RenderingPaths SupportedRendering
		{
			get
			{
				return RenderingPaths.Both;
			}
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06003E2A RID: 15914 RVA: 0x000296D7 File Offset: 0x000278D7
		public override int InstanceLimit
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x00029CC9 File Offset: 0x00027EC9
		protected override void Apply(Material Material)
		{
			base.Apply(Material);
			this.albedo.Apply(Material);
			this.specular.Apply(Material);
			this.normal.Apply(Material);
			this.emissive.Apply(Material);
		}

		// Token: 0x06003E2C RID: 15916 RVA: 0x00132F28 File Offset: 0x00131128
		protected override void OnEnable()
		{
			if (this.albedo == null)
			{
				this.albedo = new AlbedoPropertyGroup(this);
			}
			if (this.specular == null)
			{
				this.specular = new SpecularPropertyGroup(this);
			}
			if (this.normal == null)
			{
				this.normal = new NormalPropertyGroup(this);
			}
			if (this.emissive == null)
			{
				this.emissive = new EmissivePropertyGroup(this);
			}
			base.OnEnable();
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x00029D02 File Offset: 0x00027F02
		protected override void GenerateIDs()
		{
			base.GenerateIDs();
			this.albedo.GenerateIDs();
			this.specular.GenerateIDs();
			this.normal.GenerateIDs();
			this.emissive.GenerateIDs();
		}

		// Token: 0x06003E2E RID: 15918 RVA: 0x00132F8C File Offset: 0x0013118C
		public override void UpdateProperties()
		{
			if (this.properties == null || this.properties.Length != 2)
			{
				this.properties = new ProjectionProperty[2];
			}
			this.properties[0] = new ProjectionProperty("Albedo", this.albedo._Color, this.albedo.Color);
			this.properties[1] = new ProjectionProperty("Emission", this.emissive._EmissionColor, this.emissive.Color, this.emissive.Intensity);
		}

		// Token: 0x04003A69 RID: 14953
		public AlbedoPropertyGroup albedo;

		// Token: 0x04003A6A RID: 14954
		public SpecularPropertyGroup specular;

		// Token: 0x04003A6B RID: 14955
		public NormalPropertyGroup normal;

		// Token: 0x04003A6C RID: 14956
		public EmissivePropertyGroup emissive;

		// Token: 0x04003A6D RID: 14957
		protected Material[] forwardMaterials;

		// Token: 0x04003A6E RID: 14958
		protected Material[] deferredOpaqueMaterials;

		// Token: 0x04003A6F RID: 14959
		protected Material[] deferredTransparentMaterials;
	}
}
