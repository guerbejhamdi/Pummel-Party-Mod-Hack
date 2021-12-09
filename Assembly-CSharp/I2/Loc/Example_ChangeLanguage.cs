using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020007E6 RID: 2022
	public class Example_ChangeLanguage : MonoBehaviour
	{
		// Token: 0x0600397E RID: 14718 RVA: 0x00027094 File Offset: 0x00025294
		public void SetLanguage_English()
		{
			this.SetLanguage("English");
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x000270A1 File Offset: 0x000252A1
		public void SetLanguage_French()
		{
			this.SetLanguage("French");
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x000270AE File Offset: 0x000252AE
		public void SetLanguage_Spanish()
		{
			this.SetLanguage("Spanish");
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x000270BB File Offset: 0x000252BB
		public void SetLanguage(string LangName)
		{
			if (LocalizationManager.HasLanguage(LangName, true, true, true))
			{
				LocalizationManager.CurrentLanguage = LangName;
			}
		}
	}
}
