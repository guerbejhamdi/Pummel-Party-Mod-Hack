using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020007EB RID: 2027
	public class ToggleLanguage : MonoBehaviour
	{
		// Token: 0x06003995 RID: 14741 RVA: 0x00027180 File Offset: 0x00025380
		private void Start()
		{
			base.Invoke("test", 3f);
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x0012082C File Offset: 0x0011EA2C
		private void test()
		{
			List<string> allLanguages = LocalizationManager.GetAllLanguages(true);
			int num = allLanguages.IndexOf(LocalizationManager.CurrentLanguage);
			if (num >= 0)
			{
				num = (num + 1) % allLanguages.Count;
			}
			base.Invoke("test", 3f);
		}
	}
}
