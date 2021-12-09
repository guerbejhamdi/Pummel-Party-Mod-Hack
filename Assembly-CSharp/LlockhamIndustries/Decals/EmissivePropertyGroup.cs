using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000876 RID: 2166
	[Serializable]
	public class EmissivePropertyGroup
	{
		// Token: 0x06003D9D RID: 15773 RVA: 0x000295A1 File Offset: 0x000277A1
		protected void Mark()
		{
			this.projection.Mark(false);
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x000295AF File Offset: 0x000277AF
		public EmissivePropertyGroup(Projection Projection)
		{
			this.projection = Projection;
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x000295D4 File Offset: 0x000277D4
		public void GenerateIDs()
		{
			this._EmissionMap = Shader.PropertyToID("_EmissionMap");
			this._EmissionColor = Shader.PropertyToID("_EmissionColor");
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x000295F6 File Offset: 0x000277F6
		public void Apply(Material Material)
		{
			Material.SetTexture(this._EmissionMap, this.texture);
			Material.SetColor(this._EmissionColor, this.color * this.intensity);
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06003DA1 RID: 15777 RVA: 0x00029627 File Offset: 0x00027827
		// (set) Token: 0x06003DA2 RID: 15778 RVA: 0x0002962F File Offset: 0x0002782F
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

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06003DA3 RID: 15779 RVA: 0x0002964C File Offset: 0x0002784C
		// (set) Token: 0x06003DA4 RID: 15780 RVA: 0x00029654 File Offset: 0x00027854
		public float Intensity
		{
			get
			{
				return this.intensity;
			}
			set
			{
				if (this.intensity != value)
				{
					this.intensity = value;
					this.Mark();
				}
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06003DA5 RID: 15781 RVA: 0x0002966C File Offset: 0x0002786C
		// (set) Token: 0x06003DA6 RID: 15782 RVA: 0x00029674 File Offset: 0x00027874
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

		// Token: 0x04003A36 RID: 14902
		protected Projection projection;

		// Token: 0x04003A37 RID: 14903
		[SerializeField]
		private Texture texture;

		// Token: 0x04003A38 RID: 14904
		[SerializeField]
		private Color color = Color.black;

		// Token: 0x04003A39 RID: 14905
		[SerializeField]
		private float intensity = 1f;

		// Token: 0x04003A3A RID: 14906
		public int _EmissionMap;

		// Token: 0x04003A3B RID: 14907
		public int _EmissionColor;
	}
}
