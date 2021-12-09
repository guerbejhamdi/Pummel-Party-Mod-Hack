using System;
using MalbersAnimations.Scriptables;

namespace MalbersAnimations
{
	// Token: 0x02000718 RID: 1816
	[Serializable]
	public class MesssageItem
	{
		// Token: 0x06003525 RID: 13605 RVA: 0x00024087 File Offset: 0x00022287
		public MesssageItem()
		{
			this.message = string.Empty;
			this.Active = true;
		}

		// Token: 0x04003427 RID: 13351
		public string message;

		// Token: 0x04003428 RID: 13352
		public TypeMessage typeM;

		// Token: 0x04003429 RID: 13353
		public bool boolValue;

		// Token: 0x0400342A RID: 13354
		public int intValue;

		// Token: 0x0400342B RID: 13355
		public float floatValue;

		// Token: 0x0400342C RID: 13356
		public string stringValue;

		// Token: 0x0400342D RID: 13357
		public IntVar intVarValue;

		// Token: 0x0400342E RID: 13358
		public float time;

		// Token: 0x0400342F RID: 13359
		public bool sent;

		// Token: 0x04003430 RID: 13360
		public bool Active = true;
	}
}
