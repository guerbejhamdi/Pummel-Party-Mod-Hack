using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000882 RID: 2178
	[Serializable]
	public class Multiplicative : Forward
	{
		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06003E0D RID: 15885 RVA: 0x00029B34 File Offset: 0x00027D34
		public override Material MobileForward
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Mobile/Multiplicative"));
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06003E0E RID: 15886 RVA: 0x00029B46 File Offset: 0x00027D46
		public override Material StandardForward
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Standard/Multiplicative"));
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06003E0F RID: 15887 RVA: 0x00029B58 File Offset: 0x00027D58
		public override Material PackedForward
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Packed/Multiplicative"));
			}
		}
	}
}
