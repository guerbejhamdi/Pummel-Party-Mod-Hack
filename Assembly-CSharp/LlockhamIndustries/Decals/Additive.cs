using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000877 RID: 2167
	[Serializable]
	public class Additive : Forward
	{
		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06003DA7 RID: 15783 RVA: 0x00029691 File Offset: 0x00027891
		public override Material MobileForward
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Mobile/Additive"));
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06003DA8 RID: 15784 RVA: 0x000296A3 File Offset: 0x000278A3
		public override Material StandardForward
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Standard/Additive"));
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06003DA9 RID: 15785 RVA: 0x000296B5 File Offset: 0x000278B5
		public override Material PackedForward
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Packed/Additive"));
			}
		}
	}
}
