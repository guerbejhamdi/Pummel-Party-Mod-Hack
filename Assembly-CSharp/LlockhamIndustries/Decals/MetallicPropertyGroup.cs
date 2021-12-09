using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000873 RID: 2163
	[Serializable]
	public class MetallicPropertyGroup
	{
		// Token: 0x06003D81 RID: 15745 RVA: 0x000292CF File Offset: 0x000274CF
		protected void Mark()
		{
			this.projection.Mark(false);
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x000292DD File Offset: 0x000274DD
		public MetallicPropertyGroup(Projection Projection)
		{
			this.projection = Projection;
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x00029302 File Offset: 0x00027502
		public void GenerateIDs()
		{
			this._MetallicGlossMap = Shader.PropertyToID("_MetallicGlossMap");
			this._Metallic = Shader.PropertyToID("_Metallic");
			this._Glossiness = Shader.PropertyToID("_Glossiness");
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x00029334 File Offset: 0x00027534
		public void Apply(Material Material)
		{
			Material.SetTexture(this._MetallicGlossMap, this.texture);
			Material.SetFloat(this._Metallic, this.metallicity);
			Material.SetFloat(this._Glossiness, this.glossiness);
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06003D85 RID: 15749 RVA: 0x0002936C File Offset: 0x0002756C
		// (set) Token: 0x06003D86 RID: 15750 RVA: 0x00029374 File Offset: 0x00027574
		public Texture Texture
		{
			get
			{
				return this.texture;
			}
			set
			{
				if (this.texture != value)
				{
					this.texture = value;
					this.Mark();
				}
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06003D87 RID: 15751 RVA: 0x00029391 File Offset: 0x00027591
		// (set) Token: 0x06003D88 RID: 15752 RVA: 0x00029399 File Offset: 0x00027599
		public float Metallicity
		{
			get
			{
				return this.metallicity;
			}
			set
			{
				if (this.metallicity != value)
				{
					this.metallicity = value;
					this.Mark();
				}
			}
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06003D89 RID: 15753 RVA: 0x000293B1 File Offset: 0x000275B1
		// (set) Token: 0x06003D8A RID: 15754 RVA: 0x000293B9 File Offset: 0x000275B9
		public float Glossiness
		{
			get
			{
				return this.glossiness;
			}
			set
			{
				if (this.glossiness != value)
				{
					this.glossiness = value;
					this.Mark();
				}
			}
		}

		// Token: 0x04003A23 RID: 14883
		protected Projection projection;

		// Token: 0x04003A24 RID: 14884
		[SerializeField]
		private Texture texture;

		// Token: 0x04003A25 RID: 14885
		[SerializeField]
		private float metallicity = 0.5f;

		// Token: 0x04003A26 RID: 14886
		[SerializeField]
		private float glossiness = 1f;

		// Token: 0x04003A27 RID: 14887
		public int _MetallicGlossMap;

		// Token: 0x04003A28 RID: 14888
		public int _Metallic;

		// Token: 0x04003A29 RID: 14889
		public int _Glossiness;
	}
}
