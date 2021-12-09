using System;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000878 RID: 2168
	[Serializable]
	public abstract class Deferred : Projection
	{
		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06003DAB RID: 15787 RVA: 0x00005550 File Offset: 0x00003750
		public override RenderingPaths SupportedRendering
		{
			get
			{
				return RenderingPaths.Deferred;
			}
		}
	}
}
