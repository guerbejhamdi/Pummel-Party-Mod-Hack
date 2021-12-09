using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x02000816 RID: 2070
	[AddComponentMenu("I2/Localization/Localize Dropdown")]
	public class LocalizeDropdown : MonoBehaviour
	{
		// Token: 0x06003AB9 RID: 15033 RVA: 0x000279C0 File Offset: 0x00025BC0
		public void Start()
		{
			LocalizationManager.OnLocalizeEvent += this.OnLocalize;
			this.OnLocalize();
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x000279D9 File Offset: 0x00025BD9
		public void OnDestroy()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x000279EC File Offset: 0x00025BEC
		private void OnEnable()
		{
			if (this._Terms.Count == 0)
			{
				this.FillValues();
			}
			this.OnLocalize();
		}

		// Token: 0x06003ABC RID: 15036 RVA: 0x00027A07 File Offset: 0x00025C07
		public void OnLocalize()
		{
			if (!base.enabled || base.gameObject == null || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
			{
				return;
			}
			this.UpdateLocalization();
		}

		// Token: 0x06003ABD RID: 15037 RVA: 0x00129D68 File Offset: 0x00127F68
		private void FillValues()
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (component == null && I2Utils.IsPlaying())
			{
				this.FillValuesTMPro();
				return;
			}
			foreach (Dropdown.OptionData optionData in component.options)
			{
				this._Terms.Add(optionData.text);
			}
		}

		// Token: 0x06003ABE RID: 15038 RVA: 0x00129DE4 File Offset: 0x00127FE4
		public void UpdateLocalization()
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (component == null)
			{
				this.UpdateLocalizationTMPro();
				return;
			}
			component.options.Clear();
			foreach (string term in this._Terms)
			{
				string translation = LocalizationManager.GetTranslation(term, true, 0, true, false, null, null, true);
				component.options.Add(new Dropdown.OptionData(translation));
			}
			component.RefreshShownValue();
		}

		// Token: 0x06003ABF RID: 15039 RVA: 0x00129E78 File Offset: 0x00128078
		public void UpdateLocalizationTMPro()
		{
			TMP_Dropdown component = base.GetComponent<TMP_Dropdown>();
			if (component == null)
			{
				return;
			}
			component.options.Clear();
			foreach (string term in this._Terms)
			{
				string translation = LocalizationManager.GetTranslation(term, true, 0, true, false, null, null, true);
				component.options.Add(new TMP_Dropdown.OptionData(translation));
			}
			component.RefreshShownValue();
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x00129F04 File Offset: 0x00128104
		private void FillValuesTMPro()
		{
			TMP_Dropdown component = base.GetComponent<TMP_Dropdown>();
			if (component == null)
			{
				return;
			}
			foreach (TMP_Dropdown.OptionData optionData in component.options)
			{
				this._Terms.Add(optionData.text);
			}
		}

		// Token: 0x040038D9 RID: 14553
		public List<string> _Terms = new List<string>();
	}
}
