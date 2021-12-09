using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020007E7 RID: 2023
	public class Example_LocalizedString : MonoBehaviour
	{
		// Token: 0x06003983 RID: 14723 RVA: 0x001203F8 File Offset: 0x0011E5F8
		public void Start()
		{
			Debug.Log(this._MyLocalizedString);
			Debug.Log(LocalizationManager.GetTranslation(this._NormalString, true, 0, true, false, null, null, true));
			Debug.Log(LocalizationManager.GetTranslation(this._StringWithTermPopup, true, 0, true, false, null, null, true));
			Debug.Log("Term2");
			Debug.Log(this._MyLocalizedString);
			Debug.Log("Term3");
			LocalizedString localizedString = "Term3";
			localizedString.mRTL_IgnoreArabicFix = true;
			Debug.Log(localizedString);
			LocalizedString localizedString2 = "Term3";
			localizedString2.mRTL_ConvertNumbers = true;
			localizedString2.mRTL_MaxLineLength = 20;
			Debug.Log(localizedString2);
			Debug.Log(localizedString2);
		}

		// Token: 0x04003807 RID: 14343
		public LocalizedString _MyLocalizedString;

		// Token: 0x04003808 RID: 14344
		public string _NormalString;

		// Token: 0x04003809 RID: 14345
		[TermsPopup("")]
		public string _StringWithTermPopup;
	}
}
