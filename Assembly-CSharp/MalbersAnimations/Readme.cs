using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000738 RID: 1848
	public class Readme : ScriptableObject
	{
		// Token: 0x040034EB RID: 13547
		public Texture2D icon;

		// Token: 0x040034EC RID: 13548
		public string title;

		// Token: 0x040034ED RID: 13549
		public Readme.Section[] sections;

		// Token: 0x02000739 RID: 1849
		[Serializable]
		public class Section
		{
			// Token: 0x040034EE RID: 13550
			public string heading;

			// Token: 0x040034EF RID: 13551
			public string text;

			// Token: 0x040034F0 RID: 13552
			public string linkText;

			// Token: 0x040034F1 RID: 13553
			public string url;
		}
	}
}
