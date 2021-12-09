using System;
using UnityEngine;
using UnityEngine.Events;

namespace I2.Loc
{
	// Token: 0x0200083B RID: 2107
	[AddComponentMenu("I2/Localization/I2 Localize Callback")]
	public class CustomLocalizeCallback : MonoBehaviour
	{
		// Token: 0x06003BD4 RID: 15316 RVA: 0x000281CC File Offset: 0x000263CC
		public void OnEnable()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
			LocalizationManager.OnLocalizeEvent += this.OnLocalize;
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x000281F0 File Offset: 0x000263F0
		public void OnDisable()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x00028203 File Offset: 0x00026403
		public void OnLocalize()
		{
			this._OnLocalize.Invoke();
		}

		// Token: 0x04003926 RID: 14630
		public UnityEvent _OnLocalize = new UnityEvent();
	}
}
