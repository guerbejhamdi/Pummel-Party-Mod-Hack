using System;

namespace MalbersAnimations
{
	// Token: 0x02000713 RID: 1811
	[Serializable]
	public class LayersActivation
	{
		// Token: 0x04003416 RID: 13334
		public string layer;

		// Token: 0x04003417 RID: 13335
		public bool activate;

		// Token: 0x04003418 RID: 13336
		public StateTransition transA;

		// Token: 0x04003419 RID: 13337
		public bool deactivate;

		// Token: 0x0400341A RID: 13338
		public StateTransition transD;
	}
}
