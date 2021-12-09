using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000871 RID: 2161
	[Serializable]
	public class AlbedoPropertyGroup
	{
		// Token: 0x06003D71 RID: 15729 RVA: 0x00029160 File Offset: 0x00027360
		protected void Mark()
		{
			this.projection.Mark(false);
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x0002916E File Offset: 0x0002736E
		public AlbedoPropertyGroup(Projection Projection)
		{
			this.projection = Projection;
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x00029188 File Offset: 0x00027388
		public void GenerateIDs()
		{
			this._MainTex = Shader.PropertyToID("_MainTex");
			this._Color = Shader.PropertyToID("_Color");
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x000291AA File Offset: 0x000273AA
		public void Apply(Material Material)
		{
			Material.SetTexture(this._MainTex, this.texture);
			Material.SetColor(this._Color, this.color);
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06003D75 RID: 15733 RVA: 0x000291D0 File Offset: 0x000273D0
		// (set) Token: 0x06003D76 RID: 15734 RVA: 0x000291D8 File Offset: 0x000273D8
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

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06003D77 RID: 15735 RVA: 0x000291F5 File Offset: 0x000273F5
		// (set) Token: 0x06003D78 RID: 15736 RVA: 0x000291FD File Offset: 0x000273FD
		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				if (this.color != value)
				{
					this.color = value;
					this.Mark();
				}
			}
		}

		// Token: 0x04003A19 RID: 14873
		protected Projection projection;

		// Token: 0x04003A1A RID: 14874
		[SerializeField]
		private Texture texture;

		// Token: 0x04003A1B RID: 14875
		[SerializeField]
		private Color color = Color.grey;

		// Token: 0x04003A1C RID: 14876
		public int _MainTex;

		// Token: 0x04003A1D RID: 14877
		public int _Color;
	}
}
