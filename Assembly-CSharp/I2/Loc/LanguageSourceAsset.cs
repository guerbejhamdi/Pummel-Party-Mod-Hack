using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000808 RID: 2056
	[CreateAssetMenu(fileName = "I2Languages", menuName = "I2 Localization/LanguageSource", order = 1)]
	public class LanguageSourceAsset : ScriptableObject, ILanguageSource
	{
		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06003A2E RID: 14894 RVA: 0x0002765B File Offset: 0x0002585B
		// (set) Token: 0x06003A2F RID: 14895 RVA: 0x00027663 File Offset: 0x00025863
		public LanguageSourceData SourceData
		{
			get
			{
				return this.mSource;
			}
			set
			{
				this.mSource = value;
			}
		}

		// Token: 0x0400386C RID: 14444
		public LanguageSourceData mSource = new LanguageSourceData();
	}
}
