using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000879 RID: 2169
	[Serializable]
	public abstract class Forward : Projection
	{
		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06003DAD RID: 15789 RVA: 0x000296D7 File Offset: 0x000278D7
		public override int InstanceLimit
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06003DAE RID: 15790 RVA: 0x00005651 File Offset: 0x00003851
		public override RenderingPaths SupportedRendering
		{
			get
			{
				return RenderingPaths.Forward;
			}
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x000296DE File Offset: 0x000278DE
		protected override void Apply(Material Material)
		{
			base.Apply(Material);
			this.albedo.Apply(Material);
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x000296F3 File Offset: 0x000278F3
		protected override void OnEnable()
		{
			if (this.albedo == null)
			{
				this.albedo = new AlbedoPropertyGroup(this);
			}
			base.OnEnable();
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x0002970F File Offset: 0x0002790F
		protected override void GenerateIDs()
		{
			base.GenerateIDs();
			this.albedo.GenerateIDs();
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x001327D8 File Offset: 0x001309D8
		public override void UpdateProperties()
		{
			if (this.properties == null || this.properties.Length != 1)
			{
				this.properties = new ProjectionProperty[1];
			}
			this.properties[0] = new ProjectionProperty("Albedo", this.albedo._Color, this.albedo.Color);
		}

		// Token: 0x04003A3C RID: 14908
		public AlbedoPropertyGroup albedo;
	}
}
