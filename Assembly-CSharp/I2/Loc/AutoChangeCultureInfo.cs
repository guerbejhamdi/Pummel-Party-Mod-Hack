using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000839 RID: 2105
	public class AutoChangeCultureInfo : MonoBehaviour
	{
		// Token: 0x06003BCE RID: 15310 RVA: 0x000281A3 File Offset: 0x000263A3
		public void Start()
		{
			LocalizationManager.EnableChangingCultureInfo(true);
		}
	}
}
