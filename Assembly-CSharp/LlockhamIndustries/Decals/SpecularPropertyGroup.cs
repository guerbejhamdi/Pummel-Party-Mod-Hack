using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000874 RID: 2164
	[Serializable]
	public class SpecularPropertyGroup
	{
		// Token: 0x06003D8B RID: 15755 RVA: 0x000293D1 File Offset: 0x000275D1
		protected void Mark()
		{
			this.projection.Mark(false);
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x000293DF File Offset: 0x000275DF
		public SpecularPropertyGroup(Projection Projection)
		{
			this.projection = Projection;
		}

		// Token: 0x06003D8D RID: 15757 RVA: 0x00029418 File Offset: 0x00027618
		public void GenerateIDs()
		{
			this._SpecGlossMap = Shader.PropertyToID("_SpecGlossMap");
			this._SpecColor = Shader.PropertyToID("_SpecColor");
			this._Glossiness = Shader.PropertyToID("_Glossiness");
		}

		// Token: 0x06003D8E RID: 15758 RVA: 0x0002944A File Offset: 0x0002764A
		public void Apply(Material Material)
		{
			Material.SetTexture(this._SpecGlossMap, this.texture);
			Material.SetColor(this._SpecColor, this.color);
			Material.SetFloat(this._Glossiness, this.glossiness);
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06003D8F RID: 15759 RVA: 0x00029482 File Offset: 0x00027682
		// (set) Token: 0x06003D90 RID: 15760 RVA: 0x0002948A File Offset: 0x0002768A
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

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06003D91 RID: 15761 RVA: 0x000294A7 File Offset: 0x000276A7
		// (set) Token: 0x06003D92 RID: 15762 RVA: 0x000294AF File Offset: 0x000276AF
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

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06003D93 RID: 15763 RVA: 0x000294CC File Offset: 0x000276CC
		// (set) Token: 0x06003D94 RID: 15764 RVA: 0x000294D4 File Offset: 0x000276D4
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

		// Token: 0x04003A2A RID: 14890
		protected Projection projection;

		// Token: 0x04003A2B RID: 14891
		[SerializeField]
		private Texture texture;

		// Token: 0x04003A2C RID: 14892
		[SerializeField]
		private Color color = new Color(0.2f, 0.2f, 0.2f, 1f);

		// Token: 0x04003A2D RID: 14893
		[SerializeField]
		private float glossiness = 1f;

		// Token: 0x04003A2E RID: 14894
		public int _SpecGlossMap;

		// Token: 0x04003A2F RID: 14895
		public int _SpecColor;

		// Token: 0x04003A30 RID: 14896
		public int _Glossiness;
	}
}
