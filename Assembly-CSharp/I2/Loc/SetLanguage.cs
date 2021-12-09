using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200084F RID: 2127
	[AddComponentMenu("I2/Localization/SetLanguage Button")]
	public class SetLanguage : MonoBehaviour
	{
		// Token: 0x06003C20 RID: 15392 RVA: 0x0002840D File Offset: 0x0002660D
		private void OnClick()
		{
			this.ApplyLanguage();
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x00028415 File Offset: 0x00026615
		public void ApplyLanguage()
		{
			if (LocalizationManager.HasLanguage(this._Language, true, true, true))
			{
				LocalizationManager.CurrentLanguage = this._Language;
			}
		}

		// Token: 0x04003997 RID: 14743
		public string _Language;
	}
}
