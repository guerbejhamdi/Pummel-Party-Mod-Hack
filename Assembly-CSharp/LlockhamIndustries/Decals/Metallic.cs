using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000881 RID: 2177
	[Serializable]
	public class Metallic : Projection
	{
		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06003DFD RID: 15869 RVA: 0x00029A25 File Offset: 0x00027C25
		public override Material MobileForward
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Mobile/Metallic/Forward"));
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06003DFE RID: 15870 RVA: 0x00029A37 File Offset: 0x00027C37
		public override Material MobileDeferredOpaque
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Mobile/Metallic/DeferredOpaque"));
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06003DFF RID: 15871 RVA: 0x00029A49 File Offset: 0x00027C49
		public override Material MobileDeferredTransparent
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Mobile/Metallic/DeferredTransparent"));
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06003E00 RID: 15872 RVA: 0x00029A5B File Offset: 0x00027C5B
		public override Material StandardForward
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Standard/Metallic/Forward"));
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06003E01 RID: 15873 RVA: 0x00029A6D File Offset: 0x00027C6D
		public override Material StandardDeferredOpaque
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Standard/Metallic/DeferredOpaque"));
			}
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06003E02 RID: 15874 RVA: 0x00029A7F File Offset: 0x00027C7F
		public override Material StandardDeferredTransparent
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Standard/Metallic/DeferredTransparent"));
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06003E03 RID: 15875 RVA: 0x00029A91 File Offset: 0x00027C91
		public override Material PackedForward
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Packed/Metallic/Forward"));
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06003E04 RID: 15876 RVA: 0x00029AA3 File Offset: 0x00027CA3
		public override Material PackedDeferredOpaque
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Packed/Metallic/DeferredOpaque"));
			}
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x06003E05 RID: 15877 RVA: 0x00029AB5 File Offset: 0x00027CB5
		public override Material PackedDeferredTransparent
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Packed/Metallic/DeferredTransparent"));
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06003E06 RID: 15878 RVA: 0x0000539F File Offset: 0x0000359F
		public override RenderingPaths SupportedRendering
		{
			get
			{
				return RenderingPaths.Both;
			}
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06003E07 RID: 15879 RVA: 0x000296D7 File Offset: 0x000278D7
		public override int InstanceLimit
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x00029AC7 File Offset: 0x00027CC7
		protected override void Apply(Material Material)
		{
			base.Apply(Material);
			this.albedo.Apply(Material);
			this.metallic.Apply(Material);
			this.normal.Apply(Material);
			this.emissive.Apply(Material);
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x00132DB0 File Offset: 0x00130FB0
		protected override void OnEnable()
		{
			if (this.albedo == null)
			{
				this.albedo = new AlbedoPropertyGroup(this);
			}
			if (this.metallic == null)
			{
				this.metallic = new MetallicPropertyGroup(this);
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

		// Token: 0x06003E0A RID: 15882 RVA: 0x00029B00 File Offset: 0x00027D00
		protected override void GenerateIDs()
		{
			base.GenerateIDs();
			this.albedo.GenerateIDs();
			this.metallic.GenerateIDs();
			this.normal.GenerateIDs();
			this.emissive.GenerateIDs();
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x00132E14 File Offset: 0x00131014
		public override void UpdateProperties()
		{
			if (this.properties == null || this.properties.Length != 2)
			{
				this.properties = new ProjectionProperty[2];
			}
			this.properties[0] = new ProjectionProperty("Albedo", this.albedo._Color, this.albedo.Color);
			this.properties[1] = new ProjectionProperty("Emission", this.emissive._EmissionColor, this.emissive.Color, this.emissive.Intensity);
		}

		// Token: 0x04003A62 RID: 14946
		public AlbedoPropertyGroup albedo;

		// Token: 0x04003A63 RID: 14947
		public MetallicPropertyGroup metallic;

		// Token: 0x04003A64 RID: 14948
		public NormalPropertyGroup normal;

		// Token: 0x04003A65 RID: 14949
		public EmissivePropertyGroup emissive;
	}
}
