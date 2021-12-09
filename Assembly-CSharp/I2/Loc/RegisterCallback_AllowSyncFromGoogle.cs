using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200084B RID: 2123
	public class RegisterCallback_AllowSyncFromGoogle : MonoBehaviour
	{
		// Token: 0x06003C0D RID: 15373 RVA: 0x00028385 File Offset: 0x00026585
		public void Awake()
		{
			LocalizationManager.Callback_AllowSyncFromGoogle = new Func<LanguageSourceData, bool>(this.AllowSyncFromGoogle);
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x00028385 File Offset: 0x00026585
		public void OnEnable()
		{
			LocalizationManager.Callback_AllowSyncFromGoogle = new Func<LanguageSourceData, bool>(this.AllowSyncFromGoogle);
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x00028399 File Offset: 0x00026599
		public void OnDisable()
		{
			LocalizationManager.Callback_AllowSyncFromGoogle = null;
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x00005651 File Offset: 0x00003851
		public virtual bool AllowSyncFromGoogle(LanguageSourceData Source)
		{
			return true;
		}
	}
}
