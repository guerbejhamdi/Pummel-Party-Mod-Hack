using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x0200087F RID: 2175
	[Serializable]
	public class Gloss : Deferred
	{
		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06003DEC RID: 15852 RVA: 0x000299A0 File Offset: 0x00027BA0
		// (set) Token: 0x06003DED RID: 15853 RVA: 0x000299A8 File Offset: 0x00027BA8
		public GlossType GlossType
		{
			get
			{
				return this.glossType;
			}
			set
			{
				if (this.glossType != value)
				{
					this.glossType = value;
					base.Mark(false);
				}
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06003DEE RID: 15854 RVA: 0x00132C98 File Offset: 0x00130E98
		private Material Mobile
		{
			get
			{
				Shader p_Shader = null;
				GlossType glossType = this.glossType;
				if (glossType != GlossType.Shine)
				{
					if (glossType == GlossType.Dull)
					{
						p_Shader = Shader.Find("Projection/Decal/Mobile/Dry");
					}
				}
				else
				{
					p_Shader = Shader.Find("Projection/Decal/Mobile/Wet");
				}
				return base.MaterialFromShader(p_Shader);
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06003DEF RID: 15855 RVA: 0x000299C1 File Offset: 0x00027BC1
		public override Material MobileDeferredOpaque
		{
			get
			{
				return this.Mobile;
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06003DF0 RID: 15856 RVA: 0x000299C1 File Offset: 0x00027BC1
		public override Material MobileDeferredTransparent
		{
			get
			{
				return this.Mobile;
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06003DF1 RID: 15857 RVA: 0x00132CD8 File Offset: 0x00130ED8
		private Material Standard
		{
			get
			{
				Shader p_Shader = null;
				GlossType glossType = this.glossType;
				if (glossType != GlossType.Shine)
				{
					if (glossType == GlossType.Dull)
					{
						p_Shader = Shader.Find("Projection/Decal/Standard/Dry");
					}
				}
				else
				{
					p_Shader = Shader.Find("Projection/Decal/Standard/Wet");
				}
				return base.MaterialFromShader(p_Shader);
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06003DF2 RID: 15858 RVA: 0x000299C9 File Offset: 0x00027BC9
		public override Material StandardDeferredOpaque
		{
			get
			{
				return this.Standard;
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06003DF3 RID: 15859 RVA: 0x000299C9 File Offset: 0x00027BC9
		public override Material StandardDeferredTransparent
		{
			get
			{
				return this.Standard;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06003DF4 RID: 15860 RVA: 0x00132D18 File Offset: 0x00130F18
		private Material Packed
		{
			get
			{
				Shader p_Shader = null;
				GlossType glossType = this.glossType;
				if (glossType != GlossType.Shine)
				{
					if (glossType == GlossType.Dull)
					{
						p_Shader = Shader.Find("Projection/Decal/Packed/Dry");
					}
				}
				else
				{
					p_Shader = Shader.Find("Projection/Decal/Packed/Wet");
				}
				return base.MaterialFromShader(p_Shader);
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06003DF5 RID: 15861 RVA: 0x000299D1 File Offset: 0x00027BD1
		public override Material PackedDeferredOpaque
		{
			get
			{
				return this.Packed;
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06003DF6 RID: 15862 RVA: 0x000299D1 File Offset: 0x00027BD1
		public override Material PackedDeferredTransparent
		{
			get
			{
				return this.Packed;
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06003DF7 RID: 15863 RVA: 0x000296D7 File Offset: 0x000278D7
		public override int InstanceLimit
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x000299D9 File Offset: 0x00027BD9
		protected override void Apply(Material Material)
		{
			base.Apply(Material);
			this.gloss.Apply(Material);
		}

		// Token: 0x06003DF9 RID: 15865 RVA: 0x000299EE File Offset: 0x00027BEE
		protected override void OnEnable()
		{
			if (this.gloss == null)
			{
				this.gloss = new GlossPropertyGroup(this);
			}
			base.OnEnable();
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x00029A0A File Offset: 0x00027C0A
		protected override void GenerateIDs()
		{
			base.GenerateIDs();
			this.gloss.GenerateIDs();
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x00132D58 File Offset: 0x00130F58
		public override void UpdateProperties()
		{
			if (this.properties == null || this.properties.Length != 1)
			{
				this.properties = new ProjectionProperty[1];
			}
			this.properties[0] = new ProjectionProperty("Glossiness", this.gloss._Glossiness, this.gloss.Glossiness);
		}

		// Token: 0x04003A5D RID: 14941
		[SerializeField]
		public GlossType glossType;

		// Token: 0x04003A5E RID: 14942
		public GlossPropertyGroup gloss;
	}
}
