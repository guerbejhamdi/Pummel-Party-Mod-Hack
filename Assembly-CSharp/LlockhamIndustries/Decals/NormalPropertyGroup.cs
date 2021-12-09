using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000875 RID: 2165
	[Serializable]
	public class NormalPropertyGroup
	{
		// Token: 0x06003D95 RID: 15765 RVA: 0x000294EC File Offset: 0x000276EC
		protected void Mark()
		{
			this.projection.Mark(false);
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x000294FA File Offset: 0x000276FA
		public NormalPropertyGroup(Projection Projection)
		{
			this.projection = Projection;
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x00029514 File Offset: 0x00027714
		public void GenerateIDs()
		{
			this._BumpMap = Shader.PropertyToID("_BumpMap");
			this._BumpScale = Shader.PropertyToID("_BumpScale");
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x00029536 File Offset: 0x00027736
		public void Apply(Material Material)
		{
			Material.SetTexture(this._BumpMap, this.texture);
			Material.SetFloat(this._BumpScale, this.strength);
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06003D99 RID: 15769 RVA: 0x0002955C File Offset: 0x0002775C
		// (set) Token: 0x06003D9A RID: 15770 RVA: 0x00029564 File Offset: 0x00027764
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

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06003D9B RID: 15771 RVA: 0x00029581 File Offset: 0x00027781
		// (set) Token: 0x06003D9C RID: 15772 RVA: 0x00029589 File Offset: 0x00027789
		public float Strength
		{
			get
			{
				return this.strength;
			}
			set
			{
				if (this.strength != value)
				{
					this.strength = value;
					this.Mark();
				}
			}
		}

		// Token: 0x04003A31 RID: 14897
		protected Projection projection;

		// Token: 0x04003A32 RID: 14898
		[SerializeField]
		private Texture texture;

		// Token: 0x04003A33 RID: 14899
		[SerializeField]
		private float strength = 1f;

		// Token: 0x04003A34 RID: 14900
		public int _BumpMap;

		// Token: 0x04003A35 RID: 14901
		public int _BumpScale;
	}
}
