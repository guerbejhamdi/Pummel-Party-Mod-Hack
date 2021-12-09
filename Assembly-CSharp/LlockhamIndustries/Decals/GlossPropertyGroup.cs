using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000872 RID: 2162
	[Serializable]
	public class GlossPropertyGroup
	{
		// Token: 0x06003D79 RID: 15737 RVA: 0x0002921A File Offset: 0x0002741A
		protected void Mark()
		{
			this.projection.Mark(false);
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x00029228 File Offset: 0x00027428
		public GlossPropertyGroup(Projection Projection)
		{
			this.projection = Projection;
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x00029242 File Offset: 0x00027442
		public void GenerateIDs()
		{
			this._GlossMap = Shader.PropertyToID("_GlossMap");
			this._Glossiness = Shader.PropertyToID("_Glossiness");
		}

		// Token: 0x06003D7C RID: 15740 RVA: 0x00029264 File Offset: 0x00027464
		public void Apply(Material Material)
		{
			Material.SetTexture(this._GlossMap, this.texture);
			Material.SetFloat(this._Glossiness, this.glossiness);
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06003D7D RID: 15741 RVA: 0x0002928A File Offset: 0x0002748A
		// (set) Token: 0x06003D7E RID: 15742 RVA: 0x00029292 File Offset: 0x00027492
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

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06003D7F RID: 15743 RVA: 0x000292AF File Offset: 0x000274AF
		// (set) Token: 0x06003D80 RID: 15744 RVA: 0x000292B7 File Offset: 0x000274B7
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

		// Token: 0x04003A1E RID: 14878
		protected Projection projection;

		// Token: 0x04003A1F RID: 14879
		[SerializeField]
		private Texture texture;

		// Token: 0x04003A20 RID: 14880
		[SerializeField]
		private float glossiness = 1f;

		// Token: 0x04003A21 RID: 14881
		public int _GlossMap;

		// Token: 0x04003A22 RID: 14882
		public int _Glossiness;
	}
}
