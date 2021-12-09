using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x02000850 RID: 2128
	[AddComponentMenu("I2/Localization/SetLanguage Dropdown")]
	public class SetLanguageDropdown : MonoBehaviour
	{
		// Token: 0x06003C23 RID: 15395 RVA: 0x0012E88C File Offset: 0x0012CA8C
		private void OnEnable()
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (component == null)
			{
				return;
			}
			string currentLanguage = LocalizationManager.CurrentLanguage;
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			List<string> allLanguages = LocalizationManager.GetAllLanguages(true);
			component.ClearOptions();
			component.AddOptions(allLanguages);
			component.value = allLanguages.IndexOf(currentLanguage);
			component.onValueChanged.RemoveListener(new UnityAction<int>(this.OnValueChanged));
			component.onValueChanged.AddListener(new UnityAction<int>(this.OnValueChanged));
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x0012E914 File Offset: 0x0012CB14
		private void OnValueChanged(int index)
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (index < 0)
			{
				index = 0;
				component.value = index;
			}
			LocalizationManager.CurrentLanguage = component.options[index].text;
		}
	}
}
