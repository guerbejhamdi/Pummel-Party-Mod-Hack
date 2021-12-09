using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000870 RID: 2160
	[Serializable]
	public class ShapePropertyGroup
	{
		// Token: 0x06003D69 RID: 15721 RVA: 0x000290AB File Offset: 0x000272AB
		protected void Mark()
		{
			this.projection.Mark(false);
		}

		// Token: 0x06003D6A RID: 15722 RVA: 0x000290B9 File Offset: 0x000272B9
		public ShapePropertyGroup(Projection Projection)
		{
			this.projection = Projection;
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x000290D3 File Offset: 0x000272D3
		public void GenerateIDs()
		{
			this._MainTex = Shader.PropertyToID("_MainTex");
			this._Multiplier = Shader.PropertyToID("_Multiplier");
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x000290F5 File Offset: 0x000272F5
		public void Apply(Material Material)
		{
			Material.SetTexture(this._MainTex, this.texture);
			Material.SetFloat(this._Multiplier, this.multiplier);
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06003D6D RID: 15725 RVA: 0x0002911B File Offset: 0x0002731B
		// (set) Token: 0x06003D6E RID: 15726 RVA: 0x00029123 File Offset: 0x00027323
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

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06003D6F RID: 15727 RVA: 0x00029140 File Offset: 0x00027340
		// (set) Token: 0x06003D70 RID: 15728 RVA: 0x00029148 File Offset: 0x00027348
		public float Multiplier
		{
			get
			{
				return this.multiplier;
			}
			set
			{
				if (this.multiplier != value)
				{
					this.multiplier = value;
					this.Mark();
				}
			}
		}

		// Token: 0x04003A14 RID: 14868
		protected Projection projection;

		// Token: 0x04003A15 RID: 14869
		[SerializeField]
		private Texture texture;

		// Token: 0x04003A16 RID: 14870
		[SerializeField]
		private float multiplier = 1f;

		// Token: 0x04003A17 RID: 14871
		public int _MainTex;

		// Token: 0x04003A18 RID: 14872
		public int _Multiplier;
	}
}
