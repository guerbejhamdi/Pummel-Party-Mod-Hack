using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000883 RID: 2179
	[Serializable]
	public class Normal : Deferred
	{
		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06003E11 RID: 15889 RVA: 0x00029B6A File Offset: 0x00027D6A
		private Material Mobile
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Mobile/Normal"));
			}
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06003E12 RID: 15890 RVA: 0x00029B7C File Offset: 0x00027D7C
		public override Material MobileDeferredOpaque
		{
			get
			{
				return this.Mobile;
			}
		}

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06003E13 RID: 15891 RVA: 0x00029B7C File Offset: 0x00027D7C
		public override Material MobileDeferredTransparent
		{
			get
			{
				return this.Mobile;
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06003E14 RID: 15892 RVA: 0x00029B84 File Offset: 0x00027D84
		private Material Standard
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Standard/Normal"));
			}
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06003E15 RID: 15893 RVA: 0x00029B96 File Offset: 0x00027D96
		public override Material StandardDeferredOpaque
		{
			get
			{
				return this.Standard;
			}
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06003E16 RID: 15894 RVA: 0x00029B96 File Offset: 0x00027D96
		public override Material StandardDeferredTransparent
		{
			get
			{
				return this.Standard;
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06003E17 RID: 15895 RVA: 0x00029B9E File Offset: 0x00027D9E
		private Material Packed
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Packed/Normal"));
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06003E18 RID: 15896 RVA: 0x00029BB0 File Offset: 0x00027DB0
		public override Material PackedDeferredOpaque
		{
			get
			{
				return this.Packed;
			}
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06003E19 RID: 15897 RVA: 0x00029BB0 File Offset: 0x00027DB0
		public override Material PackedDeferredTransparent
		{
			get
			{
				return this.Packed;
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06003E1A RID: 15898 RVA: 0x000296D7 File Offset: 0x000278D7
		public override int InstanceLimit
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x00029BB8 File Offset: 0x00027DB8
		protected override void Apply(Material Material)
		{
			base.Apply(Material);
			this.shape.Apply(Material);
			this.normal.Apply(Material);
		}

		// Token: 0x06003E1C RID: 15900 RVA: 0x00029BD9 File Offset: 0x00027DD9
		protected override void OnEnable()
		{
			if (this.shape == null)
			{
				this.shape = new ShapePropertyGroup(this);
			}
			if (this.normal == null)
			{
				this.normal = new NormalPropertyGroup(this);
			}
			base.OnEnable();
		}

		// Token: 0x06003E1D RID: 15901 RVA: 0x00029C09 File Offset: 0x00027E09
		protected override void GenerateIDs()
		{
			base.GenerateIDs();
			this.shape.GenerateIDs();
			this.normal.GenerateIDs();
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x00132EA4 File Offset: 0x001310A4
		public override void UpdateProperties()
		{
			if (this.properties == null || this.properties.Length != 2)
			{
				this.properties = new ProjectionProperty[2];
			}
			this.properties[0] = new ProjectionProperty("Opacity", this.shape._Multiplier, this.shape.Multiplier);
			this.properties[1] = new ProjectionProperty("Normal Strength", this.normal._BumpScale, this.normal.Strength);
		}

		// Token: 0x04003A66 RID: 14950
		public ShapePropertyGroup shape;

		// Token: 0x04003A67 RID: 14951
		public NormalPropertyGroup normal;

		// Token: 0x04003A68 RID: 14952
		protected Material[] deferredMaterials;
	}
}
