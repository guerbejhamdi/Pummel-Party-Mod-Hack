using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000511 RID: 1297
public class LocalizationFontSetter : MonoBehaviour
{
	// Token: 0x060021D4 RID: 8660 RVA: 0x000D0918 File Offset: 0x000CEB18
	public void Awake()
	{
		this.m_text = base.GetComponent<Text>();
		LocalizationManager.OnLocalizeEvent += this.OnLanguageChanged;
		this.m_languagesInternal = new Dictionary<string, LocalizationFontSettings>();
		foreach (LocalizationFontSettings localizationFontSettings in this.m_languages)
		{
			this.m_languagesInternal.Add(localizationFontSettings.languageCode, localizationFontSettings);
		}
		this.OnLanguageChanged();
	}

	// Token: 0x060021D5 RID: 8661 RVA: 0x00018881 File Offset: 0x00016A81
	public void OnEnable()
	{
		this.SetLanguage(LocalizationManager.CurrentLanguageCode);
	}

	// Token: 0x060021D6 RID: 8662 RVA: 0x0001888E File Offset: 0x00016A8E
	public void OnDestroy()
	{
		LocalizationManager.OnLocalizeEvent -= this.OnLanguageChanged;
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x00018881 File Offset: 0x00016A81
	private void OnLanguageChanged()
	{
		this.SetLanguage(LocalizationManager.CurrentLanguageCode);
	}

	// Token: 0x060021D8 RID: 8664 RVA: 0x000D0980 File Offset: 0x000CEB80
	private void SetLanguage(string lang)
	{
		LocalizationFontSettings localizationFontSettings = this.m_default;
		if (this.m_languagesInternal.ContainsKey(lang))
		{
			localizationFontSettings = this.m_languagesInternal[lang];
		}
		if (this.m_text != null)
		{
			this.m_text.fontSize = localizationFontSettings.fontSize;
		}
	}

	// Token: 0x040024C4 RID: 9412
	[SerializeField]
	private LocalizationFontSettings m_default;

	// Token: 0x040024C5 RID: 9413
	[SerializeField]
	private LocalizationFontSettings[] m_languages;

	// Token: 0x040024C6 RID: 9414
	private Text m_text;

	// Token: 0x040024C7 RID: 9415
	private Dictionary<string, LocalizationFontSettings> m_languagesInternal;
}
