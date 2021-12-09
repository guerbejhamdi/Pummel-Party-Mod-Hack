using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000817 RID: 2071
	public static class LocalizationManager
	{
		// Token: 0x06003AC2 RID: 15042 RVA: 0x00027A53 File Offset: 0x00025C53
		public static void InitializeIfNeeded()
		{
			if (string.IsNullOrEmpty(LocalizationManager.mCurrentLanguage) || LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.AutoLoadGlobalParamManagers();
				LocalizationManager.UpdateSources();
				LocalizationManager.SelectStartupLanguage();
			}
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x00027A7D File Offset: 0x00025C7D
		public static string GetVersion()
		{
			return "2.8.13 f1";
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x0000FB68 File Offset: 0x0000DD68
		public static int GetRequiredWebServiceVersion()
		{
			return 5;
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x00129F74 File Offset: 0x00128174
		public static string GetWebServiceURL(LanguageSourceData source = null)
		{
			if (source != null && !string.IsNullOrEmpty(source.Google_WebServiceURL))
			{
				return source.Google_WebServiceURL;
			}
			LocalizationManager.InitializeIfNeeded();
			for (int i = 0; i < LocalizationManager.Sources.Count; i++)
			{
				if (LocalizationManager.Sources[i] != null && !string.IsNullOrEmpty(LocalizationManager.Sources[i].Google_WebServiceURL))
				{
					return LocalizationManager.Sources[i].Google_WebServiceURL;
				}
			}
			return string.Empty;
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06003AC6 RID: 15046 RVA: 0x00027A84 File Offset: 0x00025C84
		// (set) Token: 0x06003AC7 RID: 15047 RVA: 0x00129FEC File Offset: 0x001281EC
		public static string CurrentLanguage
		{
			get
			{
				LocalizationManager.InitializeIfNeeded();
				return LocalizationManager.mCurrentLanguage;
			}
			set
			{
				LocalizationManager.InitializeIfNeeded();
				string supportedLanguage = LocalizationManager.GetSupportedLanguage(value, false);
				if (!string.IsNullOrEmpty(supportedLanguage) && LocalizationManager.mCurrentLanguage != supportedLanguage)
				{
					LocalizationManager.SetLanguageAndCode(supportedLanguage, LocalizationManager.GetLanguageCode(supportedLanguage), true, false);
				}
			}
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06003AC8 RID: 15048 RVA: 0x00027A90 File Offset: 0x00025C90
		// (set) Token: 0x06003AC9 RID: 15049 RVA: 0x0012A02C File Offset: 0x0012822C
		public static string CurrentLanguageCode
		{
			get
			{
				LocalizationManager.InitializeIfNeeded();
				return LocalizationManager.mLanguageCode;
			}
			set
			{
				LocalizationManager.InitializeIfNeeded();
				if (LocalizationManager.mLanguageCode != value)
				{
					string languageFromCode = LocalizationManager.GetLanguageFromCode(value, true);
					if (!string.IsNullOrEmpty(languageFromCode))
					{
						LocalizationManager.SetLanguageAndCode(languageFromCode, value, true, false);
					}
				}
			}
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06003ACA RID: 15050 RVA: 0x0012A064 File Offset: 0x00128264
		// (set) Token: 0x06003ACB RID: 15051 RVA: 0x0012A0D4 File Offset: 0x001282D4
		public static string CurrentRegion
		{
			get
			{
				string currentLanguage = LocalizationManager.CurrentLanguage;
				int num = currentLanguage.IndexOfAny("/\\".ToCharArray());
				if (num > 0)
				{
					return currentLanguage.Substring(num + 1);
				}
				num = currentLanguage.IndexOfAny("[(".ToCharArray());
				int num2 = currentLanguage.LastIndexOfAny("])".ToCharArray());
				if (num > 0 && num != num2)
				{
					return currentLanguage.Substring(num + 1, num2 - num - 1);
				}
				return string.Empty;
			}
			set
			{
				string text = LocalizationManager.CurrentLanguage;
				int num = text.IndexOfAny("/\\".ToCharArray());
				if (num > 0)
				{
					LocalizationManager.CurrentLanguage = text.Substring(num + 1) + value;
					return;
				}
				num = text.IndexOfAny("[(".ToCharArray());
				int num2 = text.LastIndexOfAny("])".ToCharArray());
				if (num > 0 && num != num2)
				{
					text = text.Substring(num);
				}
				LocalizationManager.CurrentLanguage = text + "(" + value + ")";
			}
		}

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06003ACC RID: 15052 RVA: 0x0012A15C File Offset: 0x0012835C
		// (set) Token: 0x06003ACD RID: 15053 RVA: 0x0012A194 File Offset: 0x00128394
		public static string CurrentRegionCode
		{
			get
			{
				string currentLanguageCode = LocalizationManager.CurrentLanguageCode;
				int num = currentLanguageCode.IndexOfAny(" -_/\\".ToCharArray());
				if (num >= 0)
				{
					return currentLanguageCode.Substring(num + 1);
				}
				return string.Empty;
			}
			set
			{
				string text = LocalizationManager.CurrentLanguageCode;
				int num = text.IndexOfAny(" -_/\\".ToCharArray());
				if (num > 0)
				{
					text = text.Substring(0, num);
				}
				LocalizationManager.CurrentLanguageCode = text + "-" + value;
			}
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06003ACE RID: 15054 RVA: 0x00027A9C File Offset: 0x00025C9C
		public static CultureInfo CurrentCulture
		{
			get
			{
				return LocalizationManager.mCurrentCulture;
			}
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x0012A1D8 File Offset: 0x001283D8
		public static void SetLanguageAndCode(string LanguageName, string LanguageCode, bool RememberLanguage = true, bool Force = false)
		{
			if (LocalizationManager.mCurrentLanguage != LanguageName || LocalizationManager.mLanguageCode != LanguageCode || Force)
			{
				if (RememberLanguage)
				{
					PersistentStorage.SetSetting_String("I2 LanguageRB", LanguageName);
				}
				LocalizationManager.mCurrentLanguage = LanguageName;
				LocalizationManager.mLanguageCode = LanguageCode;
				LocalizationManager.mCurrentCulture = LocalizationManager.CreateCultureForCode(LanguageCode);
				if (LocalizationManager.mChangeCultureInfo)
				{
					LocalizationManager.SetCurrentCultureInfo();
				}
				LocalizationManager.IsRight2Left = LocalizationManager.IsRTL(LocalizationManager.mLanguageCode);
				LocalizationManager.HasJoinedWords = GoogleLanguages.LanguageCode_HasJoinedWord(LocalizationManager.mLanguageCode);
				LocalizationManager.LocalizeAll(Force);
			}
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x0012A25C File Offset: 0x0012845C
		private static CultureInfo CreateCultureForCode(string code)
		{
			CultureInfo result;
			try
			{
				result = CultureInfo.CreateSpecificCulture(code);
			}
			catch (Exception)
			{
				result = CultureInfo.InvariantCulture;
			}
			return result;
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x00027AA3 File Offset: 0x00025CA3
		public static void EnableChangingCultureInfo(bool bEnable)
		{
			if (!LocalizationManager.mChangeCultureInfo && bEnable)
			{
				LocalizationManager.SetCurrentCultureInfo();
			}
			LocalizationManager.mChangeCultureInfo = bEnable;
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x00027ABC File Offset: 0x00025CBC
		private static void SetCurrentCultureInfo()
		{
			Thread.CurrentThread.CurrentCulture = LocalizationManager.mCurrentCulture;
		}

		// Token: 0x06003AD3 RID: 15059 RVA: 0x0012A28C File Offset: 0x0012848C
		private static void SelectStartupLanguage()
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				return;
			}
			string setting_String = PersistentStorage.GetSetting_String("I2 LanguageRB", string.Empty);
			string currentDeviceLanguage = LocalizationManager.GetCurrentDeviceLanguage();
			if (!string.IsNullOrEmpty(setting_String) && LocalizationManager.HasLanguage(setting_String, true, false, true))
			{
				LocalizationManager.SetLanguageAndCode(setting_String, LocalizationManager.GetLanguageCode(setting_String), true, false);
				return;
			}
			if (!LocalizationManager.Sources[0].IgnoreDeviceLanguage)
			{
				string supportedLanguage = LocalizationManager.GetSupportedLanguage(currentDeviceLanguage, true);
				if (!string.IsNullOrEmpty(supportedLanguage))
				{
					LocalizationManager.SetLanguageAndCode(supportedLanguage, LocalizationManager.GetLanguageCode(supportedLanguage), false, false);
					return;
				}
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].mLanguages.Count > 0)
				{
					for (int j = 0; j < LocalizationManager.Sources[i].mLanguages.Count; j++)
					{
						if (LocalizationManager.Sources[i].mLanguages[j].IsEnabled())
						{
							LocalizationManager.SetLanguageAndCode(LocalizationManager.Sources[i].mLanguages[j].Name, LocalizationManager.Sources[i].mLanguages[j].Code, false, false);
							return;
						}
					}
				}
				i++;
			}
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x0012A3CC File Offset: 0x001285CC
		public static bool HasLanguage(string Language, bool AllowDiscartingRegion = true, bool Initialize = true, bool SkipDisabled = true)
		{
			if (Initialize)
			{
				LocalizationManager.InitializeIfNeeded();
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].GetLanguageIndex(Language, false, SkipDisabled) >= 0)
				{
					return true;
				}
				i++;
			}
			if (AllowDiscartingRegion)
			{
				int j = 0;
				int count2 = LocalizationManager.Sources.Count;
				while (j < count2)
				{
					if (LocalizationManager.Sources[j].GetLanguageIndex(Language, true, SkipDisabled) >= 0)
					{
						return true;
					}
					j++;
				}
			}
			return false;
		}

		// Token: 0x06003AD5 RID: 15061 RVA: 0x0012A444 File Offset: 0x00128644
		public static string GetSupportedLanguage(string Language, bool ignoreDisabled = false)
		{
			string languageCode = GoogleLanguages.GetLanguageCode(Language, false);
			if (!string.IsNullOrEmpty(languageCode))
			{
				int i = 0;
				int count = LocalizationManager.Sources.Count;
				while (i < count)
				{
					int languageIndexFromCode = LocalizationManager.Sources[i].GetLanguageIndexFromCode(languageCode, true, ignoreDisabled);
					if (languageIndexFromCode >= 0)
					{
						return LocalizationManager.Sources[i].mLanguages[languageIndexFromCode].Name;
					}
					i++;
				}
				int j = 0;
				int count2 = LocalizationManager.Sources.Count;
				while (j < count2)
				{
					int languageIndexFromCode2 = LocalizationManager.Sources[j].GetLanguageIndexFromCode(languageCode, false, ignoreDisabled);
					if (languageIndexFromCode2 >= 0)
					{
						return LocalizationManager.Sources[j].mLanguages[languageIndexFromCode2].Name;
					}
					j++;
				}
			}
			int k = 0;
			int count3 = LocalizationManager.Sources.Count;
			while (k < count3)
			{
				int languageIndex = LocalizationManager.Sources[k].GetLanguageIndex(Language, false, ignoreDisabled);
				if (languageIndex >= 0)
				{
					return LocalizationManager.Sources[k].mLanguages[languageIndex].Name;
				}
				k++;
			}
			int l = 0;
			int count4 = LocalizationManager.Sources.Count;
			while (l < count4)
			{
				int languageIndex2 = LocalizationManager.Sources[l].GetLanguageIndex(Language, true, ignoreDisabled);
				if (languageIndex2 >= 0)
				{
					return LocalizationManager.Sources[l].mLanguages[languageIndex2].Name;
				}
				l++;
			}
			return string.Empty;
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x0012A5B8 File Offset: 0x001287B8
		public static string GetLanguageCode(string Language)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(Language, true, true);
				if (languageIndex >= 0)
				{
					return LocalizationManager.Sources[i].mLanguages[languageIndex].Code;
				}
				i++;
			}
			return string.Empty;
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x0012A628 File Offset: 0x00128828
		public static string GetLanguageFromCode(string Code, bool exactMatch = true)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				int languageIndexFromCode = LocalizationManager.Sources[i].GetLanguageIndexFromCode(Code, exactMatch, false);
				if (languageIndexFromCode >= 0)
				{
					return LocalizationManager.Sources[i].mLanguages[languageIndexFromCode].Name;
				}
				i++;
			}
			return string.Empty;
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x0012A698 File Offset: 0x00128898
		public static List<string> GetAllLanguages(bool SkipDisabled = true)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			List<string> Languages = new List<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			Func<string, bool> <>9__0;
			while (i < count)
			{
				List<string> languages = Languages;
				IEnumerable<string> languages2 = LocalizationManager.Sources[i].GetLanguages(SkipDisabled);
				Func<string, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((string x) => !Languages.Contains(x)));
				}
				languages.AddRange(languages2.Where(predicate));
				i++;
			}
			return Languages;
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x0012A728 File Offset: 0x00128928
		public static List<string> GetAllLanguagesCode(bool allowRegions = true, bool SkipDisabled = true)
		{
			List<string> Languages = new List<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			Func<string, bool> <>9__0;
			while (i < count)
			{
				List<string> languages = Languages;
				IEnumerable<string> languagesCode = LocalizationManager.Sources[i].GetLanguagesCode(allowRegions, SkipDisabled);
				Func<string, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((string x) => !Languages.Contains(x)));
				}
				languages.AddRange(languagesCode.Where(predicate));
				i++;
			}
			return Languages;
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x0012A7A4 File Offset: 0x001289A4
		public static bool IsLanguageEnabled(string Language)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (!LocalizationManager.Sources[i].IsLanguageEnabled(Language))
				{
					return false;
				}
				i++;
			}
			return true;
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x0012A7E0 File Offset: 0x001289E0
		private static void LoadCurrentLanguage()
		{
			for (int i = 0; i < LocalizationManager.Sources.Count; i++)
			{
				int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(LocalizationManager.mCurrentLanguage, true, false);
				LocalizationManager.Sources[i].LoadLanguage(languageIndex, true, true, true, false);
			}
		}

		// Token: 0x06003ADC RID: 15068 RVA: 0x00027ACD File Offset: 0x00025CCD
		public static void PreviewLanguage(string NewLanguage)
		{
			LocalizationManager.mCurrentLanguage = NewLanguage;
			LocalizationManager.mLanguageCode = LocalizationManager.GetLanguageCode(LocalizationManager.mCurrentLanguage);
			LocalizationManager.IsRight2Left = LocalizationManager.IsRTL(LocalizationManager.mLanguageCode);
			LocalizationManager.HasJoinedWords = GoogleLanguages.LanguageCode_HasJoinedWord(LocalizationManager.mLanguageCode);
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x0012A830 File Offset: 0x00128A30
		public static void AutoLoadGlobalParamManagers()
		{
			foreach (LocalizationParamsManager localizationParamsManager in UnityEngine.Object.FindObjectsOfType<LocalizationParamsManager>())
			{
				if (localizationParamsManager._IsGlobalManager && !LocalizationManager.ParamManagers.Contains(localizationParamsManager))
				{
					Debug.Log(localizationParamsManager);
					LocalizationManager.ParamManagers.Add(localizationParamsManager);
				}
			}
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x00027B02 File Offset: 0x00025D02
		public static void ApplyLocalizationParams(ref string translation, bool allowLocalizedParameters = true)
		{
			LocalizationManager.ApplyLocalizationParams(ref translation, (string p) => LocalizationManager.GetLocalizationParam(p, null), allowLocalizedParameters);
		}

		// Token: 0x06003ADF RID: 15071 RVA: 0x0012A87C File Offset: 0x00128A7C
		public static void ApplyLocalizationParams(ref string translation, GameObject root, bool allowLocalizedParameters = true)
		{
			LocalizationManager.ApplyLocalizationParams(ref translation, (string p) => LocalizationManager.GetLocalizationParam(p, root), allowLocalizedParameters);
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x0012A8AC File Offset: 0x00128AAC
		public static void ApplyLocalizationParams(ref string translation, Dictionary<string, object> parameters, bool allowLocalizedParameters = true)
		{
			LocalizationManager.ApplyLocalizationParams(ref translation, delegate(string p)
			{
				object result = null;
				if (parameters.TryGetValue(p, out result))
				{
					return result;
				}
				return null;
			}, allowLocalizedParameters);
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x0012A8DC File Offset: 0x00128ADC
		public static void ApplyLocalizationParams(ref string translation, LocalizationManager._GetParam getParam, bool allowLocalizedParameters = true)
		{
			if (translation == null)
			{
				return;
			}
			string text = null;
			int num = translation.Length;
			int num2 = 0;
			while (num2 >= 0 && num2 < translation.Length)
			{
				int num3 = translation.IndexOf("{[", num2);
				if (num3 < 0)
				{
					break;
				}
				int num4 = translation.IndexOf("]}", num3);
				if (num4 < 0)
				{
					break;
				}
				int num5 = translation.IndexOf("{[", num3 + 1);
				if (num5 > 0 && num5 < num4)
				{
					num2 = num5;
				}
				else
				{
					int num6 = (translation[num3 + 2] == '#') ? 3 : 2;
					string param = translation.Substring(num3 + num6, num4 - num3 - num6);
					string text2 = (string)getParam(param);
					if (text2 != null)
					{
						if (allowLocalizedParameters)
						{
							LanguageSourceData languageSourceData;
							TermData termData = LocalizationManager.GetTermData(text2, out languageSourceData);
							if (termData != null)
							{
								int languageIndex = languageSourceData.GetLanguageIndex(LocalizationManager.CurrentLanguage, true, true);
								if (languageIndex >= 0)
								{
									text2 = termData.GetTranslation(languageIndex, null, false);
								}
							}
						}
						string oldValue = translation.Substring(num3, num4 - num3 + 2);
						translation = translation.Replace(oldValue, text2);
						int n = 0;
						if (int.TryParse(text2, out n))
						{
							text = GoogleLanguages.GetPluralType(LocalizationManager.CurrentLanguageCode, n).ToString();
						}
						num2 = num3 + text2.Length;
					}
					else
					{
						num2 = num4 + 2;
					}
				}
			}
			if (text != null)
			{
				string text3 = "[i2p_" + text + "]";
				int num7 = translation.IndexOf(text3, StringComparison.OrdinalIgnoreCase);
				if (num7 < 0)
				{
					num7 = 0;
				}
				else
				{
					num7 += text3.Length;
				}
				num = translation.IndexOf("[i2p_", num7 + 1, StringComparison.OrdinalIgnoreCase);
				if (num < 0)
				{
					num = translation.Length;
				}
				translation = translation.Substring(num7, num - num7);
			}
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x0012AA94 File Offset: 0x00128C94
		internal static string GetLocalizationParam(string ParamName, GameObject root)
		{
			if (root)
			{
				MonoBehaviour[] components = root.GetComponents<MonoBehaviour>();
				int i = 0;
				int num = components.Length;
				while (i < num)
				{
					ILocalizationParamsManager localizationParamsManager = components[i] as ILocalizationParamsManager;
					if (localizationParamsManager != null && components[i].enabled)
					{
						string parameterValue = localizationParamsManager.GetParameterValue(ParamName);
						if (parameterValue != null)
						{
							return parameterValue;
						}
					}
					i++;
				}
			}
			int j = 0;
			int count = LocalizationManager.ParamManagers.Count;
			while (j < count)
			{
				string parameterValue = LocalizationManager.ParamManagers[j].GetParameterValue(ParamName);
				if (parameterValue != null)
				{
					return parameterValue;
				}
				j++;
			}
			return null;
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x0012AB20 File Offset: 0x00128D20
		private static string GetPluralType(MatchCollection matches, string langCode, LocalizationManager._GetParam getParam)
		{
			int i = 0;
			int count = matches.Count;
			while (i < count)
			{
				Match match = matches[i];
				string value = match.Groups[match.Groups.Count - 1].Value;
				string text = (string)getParam(value);
				if (text != null)
				{
					int n = 0;
					if (int.TryParse(text, out n))
					{
						return GoogleLanguages.GetPluralType(langCode, n).ToString();
					}
				}
				i++;
			}
			return null;
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x00027B2A File Offset: 0x00025D2A
		public static string ApplyRTLfix(string line)
		{
			return LocalizationManager.ApplyRTLfix(line, 0, true);
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x0012ABA0 File Offset: 0x00128DA0
		public static string ApplyRTLfix(string line, int maxCharacters, bool ignoreNumbers)
		{
			if (string.IsNullOrEmpty(line))
			{
				return line;
			}
			char c = line[0];
			if (c == '!' || c == '.' || c == '?')
			{
				line = line.Substring(1) + c.ToString();
			}
			int num = -1;
			int num2 = 0;
			int num3 = 40000;
			num2 = 0;
			List<string> list = new List<string>();
			while (I2Utils.FindNextTag(line, num2, out num, out num2))
			{
				string str = "@@" + ((char)(num3 + list.Count)).ToString() + "@@";
				list.Add(line.Substring(num, num2 - num + 1));
				line = line.Substring(0, num) + str + line.Substring(num2 + 1);
				num2 = num + 5;
			}
			line = line.Replace("\r\n", "\n");
			line = I2Utils.SplitLine(line, maxCharacters);
			line = RTLFixer.Fix(line, true, !ignoreNumbers);
			for (int i = 0; i < list.Count; i++)
			{
				int length = line.Length;
				for (int j = 0; j < length; j++)
				{
					if (line[j] == '@' && line[j + 1] == '@' && (int)line[j + 2] >= num3 && line[j + 3] == '@' && line[j + 4] == '@')
					{
						int num4 = (int)line[j + 2] - num3;
						if (num4 % 2 == 0)
						{
							num4++;
						}
						else
						{
							num4--;
						}
						if (num4 >= list.Count)
						{
							num4 = list.Count - 1;
						}
						line = line.Substring(0, j) + list[num4] + line.Substring(j + 5);
						break;
					}
				}
			}
			return line;
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x00027B34 File Offset: 0x00025D34
		public static string FixRTL_IfNeeded(string text, int maxCharacters = 0, bool ignoreNumber = false)
		{
			if (LocalizationManager.IsRight2Left)
			{
				return LocalizationManager.ApplyRTLfix(text, maxCharacters, ignoreNumber);
			}
			return text;
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x00027B47 File Offset: 0x00025D47
		public static bool IsRTL(string Code)
		{
			return Array.IndexOf<string>(LocalizationManager.LanguagesRTL, Code) >= 0;
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x00027B5A File Offset: 0x00025D5A
		public static bool UpdateSources()
		{
			LocalizationManager.UnregisterDeletededSources();
			LocalizationManager.RegisterSourceInResources();
			LocalizationManager.RegisterSceneSources();
			return LocalizationManager.Sources.Count > 0;
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x0012AD64 File Offset: 0x00128F64
		private static void UnregisterDeletededSources()
		{
			for (int i = LocalizationManager.Sources.Count - 1; i >= 0; i--)
			{
				if (LocalizationManager.Sources[i] == null)
				{
					LocalizationManager.RemoveSource(LocalizationManager.Sources[i]);
				}
			}
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x0012ADA8 File Offset: 0x00128FA8
		private static void RegisterSceneSources()
		{
			foreach (LanguageSource languageSource in (LanguageSource[])Resources.FindObjectsOfTypeAll(typeof(LanguageSource)))
			{
				if (!LocalizationManager.Sources.Contains(languageSource.mSource))
				{
					if (languageSource.mSource.owner == null)
					{
						languageSource.mSource.owner = languageSource;
					}
					LocalizationManager.AddSource(languageSource.mSource);
				}
			}
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x0012AE14 File Offset: 0x00129014
		private static void RegisterSourceInResources()
		{
			foreach (string name in LocalizationManager.GlobalSources)
			{
				LanguageSourceAsset asset = ResourceManager.pInstance.GetAsset<LanguageSourceAsset>(name);
				if (asset && !LocalizationManager.Sources.Contains(asset.mSource))
				{
					if (!asset.mSource.mIsGlobalSource)
					{
						asset.mSource.mIsGlobalSource = true;
					}
					asset.mSource.owner = asset;
					LocalizationManager.AddSource(asset.mSource);
				}
			}
		}

		// Token: 0x06003AEC RID: 15084 RVA: 0x00027B78 File Offset: 0x00025D78
		private static bool AllowSyncFromGoogle(LanguageSourceData Source)
		{
			return LocalizationManager.Callback_AllowSyncFromGoogle == null || LocalizationManager.Callback_AllowSyncFromGoogle(Source);
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x0012AE90 File Offset: 0x00129090
		internal static void AddSource(LanguageSourceData Source)
		{
			if (LocalizationManager.Sources.Contains(Source))
			{
				return;
			}
			LocalizationManager.Sources.Add(Source);
			if (Source.HasGoogleSpreadsheet() && Source.GoogleUpdateFrequency != LanguageSourceData.eGoogleUpdateFrequency.Never && LocalizationManager.AllowSyncFromGoogle(Source))
			{
				Source.Import_Google_FromCache();
				bool justCheck = false;
				if (Source.GoogleUpdateDelay > 0f)
				{
					CoroutineManager.Start(LocalizationManager.Delayed_Import_Google(Source, Source.GoogleUpdateDelay, justCheck));
				}
				else
				{
					Source.Import_Google(false, justCheck);
				}
			}
			for (int i = 0; i < Source.mLanguages.Count<LanguageData>(); i++)
			{
				Source.mLanguages[i].SetLoaded(true);
			}
			if (Source.mDictionary.Count == 0)
			{
				Source.UpdateDictionary(true);
			}
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x00027B8E File Offset: 0x00025D8E
		private static IEnumerator Delayed_Import_Google(LanguageSourceData source, float delay, bool justCheck)
		{
			yield return new WaitForSeconds(delay);
			if (source != null)
			{
				source.Import_Google(false, justCheck);
			}
			yield break;
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x00027BAB File Offset: 0x00025DAB
		internal static void RemoveSource(LanguageSourceData Source)
		{
			LocalizationManager.Sources.Remove(Source);
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x00027BB9 File Offset: 0x00025DB9
		public static bool IsGlobalSource(string SourceName)
		{
			return Array.IndexOf<string>(LocalizationManager.GlobalSources, SourceName) >= 0;
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x0012AF40 File Offset: 0x00129140
		public static LanguageSourceData GetSourceContaining(string term, bool fallbackToFirst = true)
		{
			if (!string.IsNullOrEmpty(term))
			{
				int i = 0;
				int count = LocalizationManager.Sources.Count;
				while (i < count)
				{
					if (LocalizationManager.Sources[i].GetTermData(term, false) != null)
					{
						return LocalizationManager.Sources[i];
					}
					i++;
				}
			}
			if (!fallbackToFirst || LocalizationManager.Sources.Count <= 0)
			{
				return null;
			}
			return LocalizationManager.Sources[0];
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x0012AFAC File Offset: 0x001291AC
		public static UnityEngine.Object FindAsset(string value)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				UnityEngine.Object @object = LocalizationManager.Sources[i].FindAsset(value);
				if (@object)
				{
					return @object;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06003AF3 RID: 15091 RVA: 0x0012AFF0 File Offset: 0x001291F0
		public static void ApplyDownloadedDataFromGoogle()
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				LocalizationManager.Sources[i].ApplyDownloadedDataFromGoogle();
				i++;
			}
		}

		// Token: 0x06003AF4 RID: 15092 RVA: 0x00027BCC File Offset: 0x00025DCC
		public static string GetCurrentDeviceLanguage()
		{
			if (string.IsNullOrEmpty(LocalizationManager.mCurrentDeviceLanguage))
			{
				LocalizationManager.DetectDeviceLanguage();
			}
			return LocalizationManager.mCurrentDeviceLanguage;
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x0012B024 File Offset: 0x00129224
		private static void DetectDeviceLanguage()
		{
			LocalizationManager.mCurrentDeviceLanguage = Application.systemLanguage.ToString();
			if (LocalizationManager.mCurrentDeviceLanguage == "ChineseSimplified")
			{
				LocalizationManager.mCurrentDeviceLanguage = "Chinese (Simplified)";
			}
			if (LocalizationManager.mCurrentDeviceLanguage == "ChineseTraditional")
			{
				LocalizationManager.mCurrentDeviceLanguage = "Chinese (Traditional)";
			}
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x0012B080 File Offset: 0x00129280
		public static void RegisterTarget(ILocalizeTargetDescriptor desc)
		{
			if (LocalizationManager.mLocalizeTargets.FindIndex((ILocalizeTargetDescriptor x) => x.Name == desc.Name) != -1)
			{
				return;
			}
			for (int i = 0; i < LocalizationManager.mLocalizeTargets.Count; i++)
			{
				if (LocalizationManager.mLocalizeTargets[i].Priority > desc.Priority)
				{
					LocalizationManager.mLocalizeTargets.Insert(i, desc);
					return;
				}
			}
			LocalizationManager.mLocalizeTargets.Add(desc);
		}

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06003AF7 RID: 15095 RVA: 0x0012B108 File Offset: 0x00129308
		// (remove) Token: 0x06003AF8 RID: 15096 RVA: 0x0012B13C File Offset: 0x0012933C
		public static event LocalizationManager.OnLocalizeCallback OnLocalizeEvent;

		// Token: 0x06003AF9 RID: 15097 RVA: 0x0012B170 File Offset: 0x00129370
		public static string GetTranslation(string Term, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null, bool allowLocalizedParameters = true)
		{
			string result = null;
			LocalizationManager.TryGetTranslation(Term, out result, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage, allowLocalizedParameters);
			return result;
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x00027BE4 File Offset: 0x00025DE4
		public static string GetTermTranslation(string Term, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null, bool allowLocalizedParameters = true)
		{
			return LocalizationManager.GetTranslation(Term, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage, allowLocalizedParameters);
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x0012B194 File Offset: 0x00129394
		public static bool TryGetTranslation(string Term, out string Translation, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null, bool allowLocalizedParameters = true)
		{
			Translation = null;
			if (string.IsNullOrEmpty(Term))
			{
				return false;
			}
			LocalizationManager.InitializeIfNeeded();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].TryGetTranslation(Term, out Translation, overrideLanguage, null, false, false))
				{
					if (applyParameters)
					{
						LocalizationManager.ApplyLocalizationParams(ref Translation, localParametersRoot, allowLocalizedParameters);
					}
					if (LocalizationManager.IsRight2Left && FixForRTL)
					{
						Translation = LocalizationManager.ApplyRTLfix(Translation, maxLineLengthForRTL, ignoreRTLnumbers);
					}
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x0012B20C File Offset: 0x0012940C
		public static T GetTranslatedObject<T>(string AssetName, Localize optionalLocComp = null) where T : UnityEngine.Object
		{
			if (optionalLocComp != null)
			{
				return optionalLocComp.FindTranslatedObject<T>(AssetName);
			}
			T t = LocalizationManager.FindAsset(AssetName) as T;
			if (t)
			{
				return t;
			}
			return ResourceManager.pInstance.GetAsset<T>(AssetName);
		}

		// Token: 0x06003AFD RID: 15101 RVA: 0x00027BF7 File Offset: 0x00025DF7
		public static T GetTranslatedObjectByTermName<T>(string Term, Localize optionalLocComp = null) where T : UnityEngine.Object
		{
			return LocalizationManager.GetTranslatedObject<T>(LocalizationManager.GetTranslation(Term, false, 0, true, false, null, null, true), null);
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x0012B258 File Offset: 0x00129458
		public static string GetAppName(string languageCode)
		{
			if (!string.IsNullOrEmpty(languageCode))
			{
				for (int i = 0; i < LocalizationManager.Sources.Count; i++)
				{
					if (!string.IsNullOrEmpty(LocalizationManager.Sources[i].mTerm_AppName))
					{
						int languageIndexFromCode = LocalizationManager.Sources[i].GetLanguageIndexFromCode(languageCode, false, false);
						if (languageIndexFromCode >= 0)
						{
							TermData termData = LocalizationManager.Sources[i].GetTermData(LocalizationManager.Sources[i].mTerm_AppName, false);
							if (termData != null)
							{
								string translation = termData.GetTranslation(languageIndexFromCode, null, false);
								if (!string.IsNullOrEmpty(translation))
								{
									return translation;
								}
							}
						}
					}
				}
			}
			return Application.productName;
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x00027C0C File Offset: 0x00025E0C
		public static void LocalizeAll(bool Force = false)
		{
			LocalizationManager.LoadCurrentLanguage();
			if (!Application.isPlaying)
			{
				LocalizationManager.DoLocalizeAll(Force);
				return;
			}
			LocalizationManager.mLocalizeIsScheduledWithForcedValue = (LocalizationManager.mLocalizeIsScheduledWithForcedValue || Force);
			if (LocalizationManager.mLocalizeIsScheduled)
			{
				return;
			}
			CoroutineManager.Start(LocalizationManager.Coroutine_LocalizeAll());
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x00027C40 File Offset: 0x00025E40
		private static IEnumerator Coroutine_LocalizeAll()
		{
			LocalizationManager.mLocalizeIsScheduled = true;
			yield return null;
			LocalizationManager.mLocalizeIsScheduled = false;
			bool force = LocalizationManager.mLocalizeIsScheduledWithForcedValue;
			LocalizationManager.mLocalizeIsScheduledWithForcedValue = false;
			LocalizationManager.DoLocalizeAll(force);
			yield break;
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x0012B2F0 File Offset: 0x001294F0
		private static void DoLocalizeAll(bool Force = false)
		{
			Localize[] array = (Localize[])Resources.FindObjectsOfTypeAll(typeof(Localize));
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				array[i].OnLocalize(Force);
				i++;
			}
			if (LocalizationManager.OnLocalizeEvent != null)
			{
				LocalizationManager.OnLocalizeEvent();
			}
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x0012B33C File Offset: 0x0012953C
		public static List<string> GetCategories()
		{
			List<string> list = new List<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				LocalizationManager.Sources[i].GetCategories(false, list);
				i++;
			}
			return list;
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x0012B37C File Offset: 0x0012957C
		public static List<string> GetTermsList(string Category = null)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			if (LocalizationManager.Sources.Count == 1)
			{
				return LocalizationManager.Sources[0].GetTermsList(Category);
			}
			HashSet<string> hashSet = new HashSet<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				hashSet.UnionWith(LocalizationManager.Sources[i].GetTermsList(Category));
				i++;
			}
			return new List<string>(hashSet);
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x0012B3F4 File Offset: 0x001295F4
		public static TermData GetTermData(string term)
		{
			LocalizationManager.InitializeIfNeeded();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				TermData termData = LocalizationManager.Sources[i].GetTermData(term, false);
				if (termData != null)
				{
					return termData;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x0012B438 File Offset: 0x00129638
		public static TermData GetTermData(string term, out LanguageSourceData source)
		{
			LocalizationManager.InitializeIfNeeded();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				TermData termData = LocalizationManager.Sources[i].GetTermData(term, false);
				if (termData != null)
				{
					source = LocalizationManager.Sources[i];
					return termData;
				}
				i++;
			}
			source = null;
			return null;
		}

		// Token: 0x040038DA RID: 14554
		private static string mCurrentLanguage;

		// Token: 0x040038DB RID: 14555
		private static string mLanguageCode;

		// Token: 0x040038DC RID: 14556
		private static CultureInfo mCurrentCulture;

		// Token: 0x040038DD RID: 14557
		private static bool mChangeCultureInfo = false;

		// Token: 0x040038DE RID: 14558
		public static bool IsRight2Left = false;

		// Token: 0x040038DF RID: 14559
		public static bool HasJoinedWords = false;

		// Token: 0x040038E0 RID: 14560
		public static List<ILocalizationParamsManager> ParamManagers = new List<ILocalizationParamsManager>();

		// Token: 0x040038E1 RID: 14561
		private static string[] LanguagesRTL = new string[]
		{
			"ar-DZ",
			"ar",
			"ar-BH",
			"ar-EG",
			"ar-IQ",
			"ar-JO",
			"ar-KW",
			"ar-LB",
			"ar-LY",
			"ar-MA",
			"ar-OM",
			"ar-QA",
			"ar-SA",
			"ar-SY",
			"ar-TN",
			"ar-AE",
			"ar-YE",
			"fa",
			"he",
			"ur",
			"ji"
		};

		// Token: 0x040038E2 RID: 14562
		public static List<LanguageSourceData> Sources = new List<LanguageSourceData>();

		// Token: 0x040038E3 RID: 14563
		public static string[] GlobalSources = new string[]
		{
			"I2Languages"
		};

		// Token: 0x040038E4 RID: 14564
		public static Func<LanguageSourceData, bool> Callback_AllowSyncFromGoogle = null;

		// Token: 0x040038E5 RID: 14565
		private static string mCurrentDeviceLanguage;

		// Token: 0x040038E6 RID: 14566
		public static List<ILocalizeTargetDescriptor> mLocalizeTargets = new List<ILocalizeTargetDescriptor>();

		// Token: 0x040038E8 RID: 14568
		private static bool mLocalizeIsScheduled = false;

		// Token: 0x040038E9 RID: 14569
		private static bool mLocalizeIsScheduledWithForcedValue = false;

		// Token: 0x040038EA RID: 14570
		public static bool HighlightLocalizedTargets = false;

		// Token: 0x02000818 RID: 2072
		// (Invoke) Token: 0x06003B08 RID: 15112
		public delegate object _GetParam(string param);

		// Token: 0x02000819 RID: 2073
		// (Invoke) Token: 0x06003B0C RID: 15116
		public delegate void OnLocalizeCallback();
	}
}
