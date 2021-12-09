using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020007E5 RID: 2021
	public class CallbackNotification : MonoBehaviour
	{
		// Token: 0x0600397C RID: 14716 RVA: 0x001203B4 File Offset: 0x0011E5B4
		public void OnModifyLocalization()
		{
			if (string.IsNullOrEmpty(Localize.MainTranslation))
			{
				return;
			}
			string translation = LocalizationManager.GetTranslation("Color/Red", true, 0, true, false, null, null, true);
			Localize.MainTranslation = Localize.MainTranslation.Replace("{PLAYER_COLOR}", translation);
		}
	}
}
