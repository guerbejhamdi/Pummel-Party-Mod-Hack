using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000885 RID: 2181
	[Serializable]
	public class Unlit : Forward
	{
		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06003E30 RID: 15920 RVA: 0x00029D36 File Offset: 0x00027F36
		public override Material MobileForward
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Mobile/Unlit"));
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06003E31 RID: 15921 RVA: 0x00029D48 File Offset: 0x00027F48
		public override Material StandardForward
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Standard/Unlit"));
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06003E32 RID: 15922 RVA: 0x00029D5A File Offset: 0x00027F5A
		public override Material PackedForward
		{
			get
			{
				return base.MaterialFromShader(Shader.Find("Projection/Decal/Packed/Unlit"));
			}
		}
	}
}
