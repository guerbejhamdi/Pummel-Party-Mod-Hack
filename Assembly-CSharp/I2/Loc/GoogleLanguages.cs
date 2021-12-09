using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020007F4 RID: 2036
	public static class GoogleLanguages
	{
		// Token: 0x060039C3 RID: 14787 RVA: 0x00121258 File Offset: 0x0011F458
		public static string GetLanguageCode(string Filter, bool ShowWarnings = false)
		{
			if (string.IsNullOrEmpty(Filter))
			{
				return string.Empty;
			}
			string[] filters = Filter.ToLowerInvariant().Split(" /(),".ToCharArray());
			foreach (KeyValuePair<string, GoogleLanguages.LanguageCodeDef> keyValuePair in GoogleLanguages.mLanguageDef)
			{
				if (GoogleLanguages.LanguageMatchesFilter(keyValuePair.Key, filters))
				{
					return keyValuePair.Value.Code;
				}
			}
			if (ShowWarnings)
			{
				Debug.Log(string.Format("Language '{0}' not recognized. Please, add the language code to GoogleTranslation.cs", Filter));
			}
			return string.Empty;
		}

		// Token: 0x060039C4 RID: 14788 RVA: 0x00121300 File Offset: 0x0011F500
		public static List<string> GetLanguagesForDropdown(string Filter, string CodesToExclude)
		{
			string[] filters = Filter.ToLowerInvariant().Split(" /(),".ToCharArray());
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, GoogleLanguages.LanguageCodeDef> keyValuePair in GoogleLanguages.mLanguageDef)
			{
				if (string.IsNullOrEmpty(Filter) || GoogleLanguages.LanguageMatchesFilter(keyValuePair.Key, filters))
				{
					string text = string.Concat(new string[]
					{
						"[" + keyValuePair.Value.Code + "]"
					});
					if (!CodesToExclude.Contains(text))
					{
						list.Add(keyValuePair.Key + " " + text);
					}
				}
			}
			for (int i = list.Count - 2; i >= 0; i--)
			{
				string text2 = list[i].Substring(0, list[i].IndexOf(" ["));
				if (list[i + 1].StartsWith(text2))
				{
					list[i] = text2 + "/" + list[i];
					list.Insert(i + 1, text2 + "/");
				}
			}
			return list;
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x0012144C File Offset: 0x0011F64C
		private static bool LanguageMatchesFilter(string Language, string[] Filters)
		{
			Language = Language.ToLowerInvariant();
			int i = 0;
			int num = Filters.Length;
			while (i < num)
			{
				if (Filters[i] != "")
				{
					if (!Language.Contains(Filters[i].ToLower()))
					{
						return false;
					}
					Language = Language.Remove(Language.IndexOf(Filters[i]), Filters[i].Length);
				}
				i++;
			}
			return true;
		}

		// Token: 0x060039C6 RID: 14790 RVA: 0x001214AC File Offset: 0x0011F6AC
		public static string GetFormatedLanguageName(string Language)
		{
			string text = string.Empty;
			int num = Language.IndexOf(" [");
			if (num > 0)
			{
				Language = Language.Substring(0, num);
			}
			num = Language.IndexOf('/');
			if (num > 0)
			{
				text = Language.Substring(0, num);
				if (Language == text + "/" + text)
				{
					return text;
				}
				Language = Language.Replace("/", " (") + ")";
			}
			return Language;
		}

		// Token: 0x060039C7 RID: 14791 RVA: 0x00121524 File Offset: 0x0011F724
		public static string GetCodedLanguage(string Language, string code)
		{
			string languageCode = GoogleLanguages.GetLanguageCode(Language, false);
			if (string.Compare(code, languageCode, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return Language;
			}
			return Language + " [" + code + "]";
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x00121558 File Offset: 0x0011F758
		public static void UnPackCodeFromLanguageName(string CodedLanguage, out string Language, out string code)
		{
			if (string.IsNullOrEmpty(CodedLanguage))
			{
				Language = string.Empty;
				code = string.Empty;
				return;
			}
			int num = CodedLanguage.IndexOf("[");
			if (num < 0)
			{
				Language = CodedLanguage;
				code = GoogleLanguages.GetLanguageCode(Language, false);
				return;
			}
			Language = CodedLanguage.Substring(0, num).Trim();
			code = CodedLanguage.Substring(num + 1, CodedLanguage.IndexOf("]", num) - num - 1);
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x001215C4 File Offset: 0x0011F7C4
		public static string GetGoogleLanguageCode(string InternationalCode)
		{
			foreach (KeyValuePair<string, GoogleLanguages.LanguageCodeDef> keyValuePair in GoogleLanguages.mLanguageDef)
			{
				if (InternationalCode == keyValuePair.Value.Code)
				{
					if (keyValuePair.Value.GoogleCode == "-")
					{
						return null;
					}
					return (!string.IsNullOrEmpty(keyValuePair.Value.GoogleCode)) ? keyValuePair.Value.GoogleCode : InternationalCode;
				}
			}
			return InternationalCode;
		}

		// Token: 0x060039CA RID: 14794 RVA: 0x00121668 File Offset: 0x0011F868
		public static string GetLanguageName(string code, bool useParenthesesForRegion = false, bool allowDiscardRegion = true)
		{
			foreach (KeyValuePair<string, GoogleLanguages.LanguageCodeDef> keyValuePair in GoogleLanguages.mLanguageDef)
			{
				if (code == keyValuePair.Value.Code)
				{
					string text = keyValuePair.Key;
					if (useParenthesesForRegion)
					{
						int num = text.IndexOf('/');
						if (num > 0)
						{
							text = text.Substring(0, num) + " (" + text.Substring(num + 1) + ")";
						}
					}
					return text;
				}
			}
			if (allowDiscardRegion)
			{
				int num2 = code.IndexOf("-");
				if (num2 > 0)
				{
					return GoogleLanguages.GetLanguageName(code.Substring(0, num2), useParenthesesForRegion, false);
				}
			}
			return null;
		}

		// Token: 0x060039CB RID: 14795 RVA: 0x00121730 File Offset: 0x0011F930
		public static List<string> GetAllInternationalCodes()
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (KeyValuePair<string, GoogleLanguages.LanguageCodeDef> keyValuePair in GoogleLanguages.mLanguageDef)
			{
				hashSet.Add(keyValuePair.Value.Code);
			}
			return new List<string>(hashSet);
		}

		// Token: 0x060039CC RID: 14796 RVA: 0x0012179C File Offset: 0x0011F99C
		public static bool LanguageCode_HasJoinedWord(string languageCode)
		{
			foreach (KeyValuePair<string, GoogleLanguages.LanguageCodeDef> keyValuePair in GoogleLanguages.mLanguageDef)
			{
				if (languageCode == keyValuePair.Value.GoogleCode || languageCode == keyValuePair.Value.Code)
				{
					return keyValuePair.Value.HasJoinedWords;
				}
			}
			return false;
		}

		// Token: 0x060039CD RID: 14797 RVA: 0x00121824 File Offset: 0x0011FA24
		private static int GetPluralRule(string langCode)
		{
			if (langCode.Length > 2)
			{
				langCode = langCode.Substring(0, 2);
			}
			langCode = langCode.ToLower();
			foreach (KeyValuePair<string, GoogleLanguages.LanguageCodeDef> keyValuePair in GoogleLanguages.mLanguageDef)
			{
				if (keyValuePair.Value.Code == langCode)
				{
					return keyValuePair.Value.PluralRule;
				}
			}
			return 0;
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x001218B0 File Offset: 0x0011FAB0
		public static bool LanguageHasPluralType(string langCode, string pluralType)
		{
			if (pluralType == "Plural" || pluralType == "Zero" || pluralType == "One")
			{
				return true;
			}
			switch (GoogleLanguages.GetPluralRule(langCode))
			{
			case 3:
				return pluralType == "Two" || pluralType == "Few";
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				return pluralType == "Few";
			case 9:
				return pluralType == "Two" || pluralType == "Few";
			case 10:
			case 11:
			case 15:
			case 16:
				return pluralType == "Two" || pluralType == "Few" || pluralType == "Many";
			case 12:
				return pluralType == "Few" || pluralType == "Many";
			case 13:
				return pluralType == "Two";
			}
			return false;
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x001219C4 File Offset: 0x0011FBC4
		public static ePluralType GetPluralType(string langCode, int n)
		{
			if (n == 0)
			{
				return ePluralType.Zero;
			}
			if (n == 1)
			{
				return ePluralType.One;
			}
			switch (GoogleLanguages.GetPluralRule(langCode))
			{
			case 0:
				return ePluralType.Plural;
			case 1:
				if (n != 1)
				{
					return ePluralType.Plural;
				}
				return ePluralType.One;
			case 2:
				if (n > 1)
				{
					return ePluralType.Plural;
				}
				return ePluralType.One;
			case 3:
				if (n == 1 || n == 11)
				{
					return ePluralType.One;
				}
				if (n == 2 || n == 12)
				{
					return ePluralType.Two;
				}
				if (!GoogleLanguages.inRange(n, 3, 10) && !GoogleLanguages.inRange(n, 13, 19))
				{
					return ePluralType.Plural;
				}
				return ePluralType.Few;
			case 4:
				if (n == 1)
				{
					return ePluralType.One;
				}
				if (!GoogleLanguages.inRange(n % 100, 1, 19))
				{
					return ePluralType.Plural;
				}
				return ePluralType.Few;
			case 5:
				if (n % 10 == 1 && n % 100 != 11)
				{
					return ePluralType.One;
				}
				if (n % 10 < 2 || (n % 100 >= 10 && n % 100 < 20))
				{
					return ePluralType.Plural;
				}
				return ePluralType.Few;
			case 6:
				if (n % 10 == 1 && n % 100 != 11)
				{
					return ePluralType.One;
				}
				if (!GoogleLanguages.inRange(n % 10, 2, 4) || GoogleLanguages.inRange(n % 100, 12, 14))
				{
					return ePluralType.Plural;
				}
				return ePluralType.Few;
			case 7:
				if (n == 1)
				{
					return ePluralType.One;
				}
				if (!GoogleLanguages.inRange(n, 2, 4))
				{
					return ePluralType.Plural;
				}
				return ePluralType.Few;
			case 8:
				if (n == 1)
				{
					return ePluralType.One;
				}
				if (!GoogleLanguages.inRange(n % 10, 2, 4) || GoogleLanguages.inRange(n % 100, 12, 14))
				{
					return ePluralType.Plural;
				}
				return ePluralType.Few;
			case 9:
				if (n % 100 == 1)
				{
					return ePluralType.One;
				}
				if (n % 100 == 2)
				{
					return ePluralType.Two;
				}
				if (!GoogleLanguages.inRange(n % 100, 3, 4))
				{
					return ePluralType.Plural;
				}
				return ePluralType.Few;
			case 10:
				if (n == 1)
				{
					return ePluralType.One;
				}
				if (n == 2)
				{
					return ePluralType.Two;
				}
				if (GoogleLanguages.inRange(n, 3, 6))
				{
					return ePluralType.Few;
				}
				if (!GoogleLanguages.inRange(n, 7, 10))
				{
					return ePluralType.Plural;
				}
				return ePluralType.Many;
			case 11:
				if (n == 0)
				{
					return ePluralType.Zero;
				}
				if (n == 1)
				{
					return ePluralType.One;
				}
				if (n == 2)
				{
					return ePluralType.Two;
				}
				if (GoogleLanguages.inRange(n % 100, 3, 10))
				{
					return ePluralType.Few;
				}
				if (n % 100 < 11)
				{
					return ePluralType.Plural;
				}
				return ePluralType.Many;
			case 12:
				if (n == 1)
				{
					return ePluralType.One;
				}
				if (GoogleLanguages.inRange(n % 100, 1, 10))
				{
					return ePluralType.Few;
				}
				if (!GoogleLanguages.inRange(n % 100, 11, 19))
				{
					return ePluralType.Plural;
				}
				return ePluralType.Many;
			case 13:
				if (n % 10 == 1)
				{
					return ePluralType.One;
				}
				if (n % 10 != 2)
				{
					return ePluralType.Plural;
				}
				return ePluralType.Two;
			case 14:
				if (n % 10 != 1 || n % 100 == 11)
				{
					return ePluralType.Plural;
				}
				return ePluralType.One;
			case 15:
				if (n % 10 == 1 && n % 100 != 11 && n % 100 != 71 && n % 100 != 91)
				{
					return ePluralType.One;
				}
				if (n % 10 == 2 && n % 100 != 12 && n % 100 != 72 && n % 100 != 92)
				{
					return ePluralType.Two;
				}
				if ((n % 10 == 3 || n % 10 == 4 || n % 10 == 9) && n % 100 != 13 && n % 100 != 14 && n % 100 != 19 && n % 100 != 73 && n % 100 != 74 && n % 100 != 79 && n % 100 != 93 && n % 100 != 94 && n % 100 != 99)
				{
					return ePluralType.Few;
				}
				if (n % 1000000 != 0)
				{
					return ePluralType.Plural;
				}
				return ePluralType.Many;
			case 16:
				if (n == 0)
				{
					return ePluralType.Zero;
				}
				if (n == 1)
				{
					return ePluralType.One;
				}
				if (n == 2)
				{
					return ePluralType.Two;
				}
				if (n == 3)
				{
					return ePluralType.Few;
				}
				if (n != 6)
				{
					return ePluralType.Plural;
				}
				return ePluralType.Many;
			default:
				return ePluralType.Plural;
			}
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x00121CC0 File Offset: 0x0011FEC0
		public static int GetPluralTestNumber(string langCode, ePluralType pluralType)
		{
			switch (pluralType)
			{
			case ePluralType.Zero:
				return 0;
			case ePluralType.One:
				return 1;
			case ePluralType.Few:
				return 3;
			case ePluralType.Many:
			{
				int pluralRule = GoogleLanguages.GetPluralRule(langCode);
				if (pluralRule == 10)
				{
					return 8;
				}
				if (pluralRule == 11 || pluralRule == 12)
				{
					return 13;
				}
				if (pluralRule == 15)
				{
					return 1000000;
				}
				return 6;
			}
			}
			return 936;
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x00027371 File Offset: 0x00025571
		private static bool inRange(int amount, int min, int max)
		{
			return amount >= min && amount <= max;
		}

		// Token: 0x0400381F RID: 14367
		public static Dictionary<string, GoogleLanguages.LanguageCodeDef> mLanguageDef = new Dictionary<string, GoogleLanguages.LanguageCodeDef>(StringComparer.Ordinal)
		{
			{
				"Abkhazian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ab",
					GoogleCode = "-"
				}
			},
			{
				"Afar",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "aa",
					GoogleCode = "-"
				}
			},
			{
				"Afrikaans",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "af"
				}
			},
			{
				"Akan",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ak",
					GoogleCode = "-"
				}
			},
			{
				"Albanian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "sq"
				}
			},
			{
				"Amharic",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "am"
				}
			},
			{
				"Arabic",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar"
				}
			},
			{
				"Arabic/Algeria",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-DZ",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Bahrain",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-BH",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Egypt",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-EG",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Iraq",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-IQ",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Jordan",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-JO",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Kuwait",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-KW",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Lebanon",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-LB",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Libya",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-LY",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Morocco",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-MA",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Oman",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-OM",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Qatar",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-QA",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Saudi Arabia",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-SA",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Syria",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-SY",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Tunisia",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-TN",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/U.A.E.",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-AE",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Yemen",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 11,
					Code = "ar-YE",
					GoogleCode = "ar"
				}
			},
			{
				"Aragonese",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "an",
					GoogleCode = "-"
				}
			},
			{
				"Armenian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "hy"
				}
			},
			{
				"Assamese",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "as",
					GoogleCode = "-"
				}
			},
			{
				"Avaric",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "av",
					GoogleCode = "-"
				}
			},
			{
				"Avestan",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ae",
					GoogleCode = "-"
				}
			},
			{
				"Aymara",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ay",
					GoogleCode = "-"
				}
			},
			{
				"Azerbaijani",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "az"
				}
			},
			{
				"Bambara",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "bm",
					GoogleCode = "-"
				}
			},
			{
				"Bashkir",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ba",
					GoogleCode = "-"
				}
			},
			{
				"Basque",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "eu"
				}
			},
			{
				"Basque/Spain",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "eu-ES",
					GoogleCode = "eu"
				}
			},
			{
				"Belarusian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 6,
					Code = "be"
				}
			},
			{
				"Bengali",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "bn"
				}
			},
			{
				"Bihari",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "bh",
					GoogleCode = "-"
				}
			},
			{
				"Bislama",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "bi",
					GoogleCode = "-"
				}
			},
			{
				"Bosnian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 6,
					Code = "bs"
				}
			},
			{
				"Breton",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "br",
					GoogleCode = "-"
				}
			},
			{
				"Bulgariaa",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "bg"
				}
			},
			{
				"Burmese",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "my"
				}
			},
			{
				"Catalan",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ca"
				}
			},
			{
				"Chamorro",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ch",
					GoogleCode = "-"
				}
			},
			{
				"Chechen",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ce",
					GoogleCode = "-"
				}
			},
			{
				"Chichewa",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ny"
				}
			},
			{
				"Chinese",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "zh",
					GoogleCode = "zh-CN",
					HasJoinedWords = true
				}
			},
			{
				"Chinese/Hong Kong",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "zh-HK",
					GoogleCode = "zh-TW",
					HasJoinedWords = true
				}
			},
			{
				"Chinese/Macau",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "zh-MO",
					GoogleCode = "zh-CN",
					HasJoinedWords = true
				}
			},
			{
				"Chinese/PRC",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "zh-CN",
					GoogleCode = "zh-CN",
					HasJoinedWords = true
				}
			},
			{
				"Chinese/Simplified",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "zh-CN",
					GoogleCode = "zh-CN",
					HasJoinedWords = true
				}
			},
			{
				"Chinese/Singapore",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "zh-SG",
					GoogleCode = "zh-CN",
					HasJoinedWords = true
				}
			},
			{
				"Chinese/Taiwan",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "zh-TW",
					GoogleCode = "zh-TW",
					HasJoinedWords = true
				}
			},
			{
				"Chinese/Traditional",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "zh-TW",
					GoogleCode = "zh-TW",
					HasJoinedWords = true
				}
			},
			{
				"Chuvash",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "cv",
					GoogleCode = "-"
				}
			},
			{
				"Cornish",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "kw",
					GoogleCode = "-"
				}
			},
			{
				"Corsican",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "co"
				}
			},
			{
				"Cree",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "cr",
					GoogleCode = "-"
				}
			},
			{
				"Croatian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 6,
					Code = "hr"
				}
			},
			{
				"Croatian/Bosnia and Herzegovina",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 5,
					Code = "hr-BA",
					GoogleCode = "hr"
				}
			},
			{
				"Czech",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 7,
					Code = "cs"
				}
			},
			{
				"Danish",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "da"
				}
			},
			{
				"Divehi",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "dv",
					GoogleCode = "-"
				}
			},
			{
				"Dutch",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "nl"
				}
			},
			{
				"Dutch/Belgium",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "nl-BE",
					GoogleCode = "nl"
				}
			},
			{
				"Dutch/Netherlands",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "nl-NL",
					GoogleCode = "nl"
				}
			},
			{
				"Dzongkha",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "dz",
					GoogleCode = "-"
				}
			},
			{
				"English",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "en"
				}
			},
			{
				"English/Australia",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "en-AU",
					GoogleCode = "en"
				}
			},
			{
				"English/Belize",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "en-BZ",
					GoogleCode = "en"
				}
			},
			{
				"English/Canada",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "en-CA",
					GoogleCode = "en"
				}
			},
			{
				"English/Caribbean",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "en-CB",
					GoogleCode = "en"
				}
			},
			{
				"English/Ireland",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "en-IE",
					GoogleCode = "en"
				}
			},
			{
				"English/Jamaica",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "en-JM",
					GoogleCode = "en"
				}
			},
			{
				"English/New Zealand",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "en-NZ",
					GoogleCode = "en"
				}
			},
			{
				"English/Republic of the Philippines",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "en-PH",
					GoogleCode = "en"
				}
			},
			{
				"English/South Africa",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "en-ZA",
					GoogleCode = "en"
				}
			},
			{
				"English/Trinidad",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "en-TT",
					GoogleCode = "en"
				}
			},
			{
				"English/United Kingdom",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "en-GB",
					GoogleCode = "en"
				}
			},
			{
				"English/United States",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "en-US",
					GoogleCode = "en"
				}
			},
			{
				"English/Zimbabwe",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "en-ZW",
					GoogleCode = "en"
				}
			},
			{
				"Esperanto",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "eo"
				}
			},
			{
				"Estonian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "et"
				}
			},
			{
				"Ewe",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ee",
					GoogleCode = "-"
				}
			},
			{
				"Faeroese",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "fo",
					GoogleCode = "-"
				}
			},
			{
				"Fijian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "fj",
					GoogleCode = "-"
				}
			},
			{
				"Finnish",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "fi"
				}
			},
			{
				"French",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 2,
					Code = "fr"
				}
			},
			{
				"French/Belgium",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 2,
					Code = "fr-BE",
					GoogleCode = "fr"
				}
			},
			{
				"French/Canada",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 2,
					Code = "fr-CA",
					GoogleCode = "fr"
				}
			},
			{
				"French/France",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 2,
					Code = "fr-FR",
					GoogleCode = "fr"
				}
			},
			{
				"French/Luxembourg",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 2,
					Code = "fr-LU",
					GoogleCode = "fr"
				}
			},
			{
				"French/Principality of Monaco",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 2,
					Code = "fr-MC",
					GoogleCode = "fr"
				}
			},
			{
				"French/Switzerland",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 2,
					Code = "fr-CH",
					GoogleCode = "fr"
				}
			},
			{
				"Fulah",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ff",
					GoogleCode = "-"
				}
			},
			{
				"Galician",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "gl"
				}
			},
			{
				"Galician/Spain",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "gl-ES",
					GoogleCode = "gl"
				}
			},
			{
				"Georgian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "ka"
				}
			},
			{
				"German",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "de"
				}
			},
			{
				"German/Austria",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "de-AT",
					GoogleCode = "de"
				}
			},
			{
				"German/Germany",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "de-DE",
					GoogleCode = "de"
				}
			},
			{
				"German/Liechtenstein",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "de-LI",
					GoogleCode = "de"
				}
			},
			{
				"German/Luxembourg",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "de-LU",
					GoogleCode = "de"
				}
			},
			{
				"German/Switzerland",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "de-CH",
					GoogleCode = "de"
				}
			},
			{
				"Greek",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "el"
				}
			},
			{
				"Guaraní",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "gn",
					GoogleCode = "-"
				}
			},
			{
				"Gujarati",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "gu"
				}
			},
			{
				"Haitian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ht"
				}
			},
			{
				"Hausa",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ha"
				}
			},
			{
				"Hebrew",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "he",
					GoogleCode = "iw"
				}
			},
			{
				"Herero",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "hz",
					GoogleCode = "-"
				}
			},
			{
				"Hindi",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "hi"
				}
			},
			{
				"Hiri Motu",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ho",
					GoogleCode = "-"
				}
			},
			{
				"Hungarian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "hu"
				}
			},
			{
				"Interlingua",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ia",
					GoogleCode = "-"
				}
			},
			{
				"Indonesian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "id"
				}
			},
			{
				"Interlingue",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ie",
					GoogleCode = "-"
				}
			},
			{
				"Irish",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 10,
					Code = "ga"
				}
			},
			{
				"Igbo",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ig"
				}
			},
			{
				"Inupiaq",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ik",
					GoogleCode = "-"
				}
			},
			{
				"Ido",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "io",
					GoogleCode = "-"
				}
			},
			{
				"Icelandic",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 14,
					Code = "is"
				}
			},
			{
				"Italian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "it"
				}
			},
			{
				"Italian/Italy",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "it-IT",
					GoogleCode = "it"
				}
			},
			{
				"Italian/Switzerland",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "it-CH",
					GoogleCode = "it"
				}
			},
			{
				"Inuktitut",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "iu",
					GoogleCode = "-"
				}
			},
			{
				"Japanese",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "ja",
					HasJoinedWords = true
				}
			},
			{
				"Javanese",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "jv"
				}
			},
			{
				"Kalaallisut",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "kl",
					GoogleCode = "-"
				}
			},
			{
				"Kannada",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "kn"
				}
			},
			{
				"Kanuri",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "kr",
					GoogleCode = "-"
				}
			},
			{
				"Kashmiri",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ks",
					GoogleCode = "-"
				}
			},
			{
				"Kazakh",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "kk"
				}
			},
			{
				"Central Khmer",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "km"
				}
			},
			{
				"Kikuyu",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ki",
					GoogleCode = "-"
				}
			},
			{
				"Kinyarwanda",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "rw",
					GoogleCode = "-"
				}
			},
			{
				"Kirghiz",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ky"
				}
			},
			{
				"Komi",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "kv",
					GoogleCode = "-"
				}
			},
			{
				"Kongo",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "kg",
					GoogleCode = "-"
				}
			},
			{
				"Korean",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "ko"
				}
			},
			{
				"Kurdish",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ku"
				}
			},
			{
				"Kuanyama",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "kj",
					GoogleCode = "-"
				}
			},
			{
				"Latin",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "la"
				}
			},
			{
				"Luxembourgish",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "lb"
				}
			},
			{
				"Ganda",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "lg",
					GoogleCode = "-"
				}
			},
			{
				"Limburgan",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "li",
					GoogleCode = "-"
				}
			},
			{
				"Lingala",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ln",
					GoogleCode = "-"
				}
			},
			{
				"Lao",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "lo"
				}
			},
			{
				"Latvian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 5,
					Code = "lv"
				}
			},
			{
				"Luba-Katanga",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "lu",
					GoogleCode = "-"
				}
			},
			{
				"Lithuanian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 5,
					Code = "lt"
				}
			},
			{
				"Manx",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "gv",
					GoogleCode = "-"
				}
			},
			{
				"Macedonian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 13,
					Code = "mk"
				}
			},
			{
				"Malagasy",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "mg"
				}
			},
			{
				"Malay",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "ms"
				}
			},
			{
				"Malay/Brunei Darussalam",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "ms-BN",
					GoogleCode = "ms"
				}
			},
			{
				"Malay/Malaysia",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "ms-MY",
					GoogleCode = "ms"
				}
			},
			{
				"Malayalam",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ml"
				}
			},
			{
				"Maltese",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 12,
					Code = "mt"
				}
			},
			{
				"Maori",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 2,
					Code = "mi"
				}
			},
			{
				"Marathi",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "mr"
				}
			},
			{
				"Marshallese",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "mh",
					GoogleCode = "-"
				}
			},
			{
				"Mongolian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "mn"
				}
			},
			{
				"Nauru",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "na",
					GoogleCode = "-"
				}
			},
			{
				"Navajo",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "nv",
					GoogleCode = "-"
				}
			},
			{
				"North Ndebele",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "nd",
					GoogleCode = "-"
				}
			},
			{
				"Nepali",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ne"
				}
			},
			{
				"Ndonga",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ng",
					GoogleCode = "-"
				}
			},
			{
				"Northern Sotho",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ns",
					GoogleCode = "st"
				}
			},
			{
				"Norwegian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "nb",
					GoogleCode = "no"
				}
			},
			{
				"Norwegian/Nynorsk",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "nn",
					GoogleCode = "no"
				}
			},
			{
				"Sichuan Yi",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ii",
					GoogleCode = "-"
				}
			},
			{
				"South Ndebele",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "nr",
					GoogleCode = "-"
				}
			},
			{
				"Occitan",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "oc",
					GoogleCode = "-"
				}
			},
			{
				"Ojibwa",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "oj",
					GoogleCode = "-"
				}
			},
			{
				"Church\u00a0Slavic",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "cu",
					GoogleCode = "-"
				}
			},
			{
				"Oromo",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "om",
					GoogleCode = "-"
				}
			},
			{
				"Oriya",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "or",
					GoogleCode = "-"
				}
			},
			{
				"Ossetian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "os",
					GoogleCode = "-"
				}
			},
			{
				"Pali",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "pi",
					GoogleCode = "-"
				}
			},
			{
				"Pashto",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ps"
				}
			},
			{
				"Persian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "fa"
				}
			},
			{
				"Polish",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 8,
					Code = "pl"
				}
			},
			{
				"Portuguese",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "pt"
				}
			},
			{
				"Portuguese/Brazil",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 2,
					Code = "pt-BR",
					GoogleCode = "pt"
				}
			},
			{
				"Portuguese/Portugal",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "pt-PT",
					GoogleCode = "pt"
				}
			},
			{
				"Punjabi",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "pa"
				}
			},
			{
				"Quechua",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "qu",
					GoogleCode = "-"
				}
			},
			{
				"Quechua/Bolivia",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "qu-BO",
					GoogleCode = "-"
				}
			},
			{
				"Quechua/Ecuador",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "qu-EC",
					GoogleCode = "-"
				}
			},
			{
				"Quechua/Peru",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "qu-PE",
					GoogleCode = "-"
				}
			},
			{
				"Rhaeto-Romanic",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "rm",
					GoogleCode = "ro"
				}
			},
			{
				"Romanian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 4,
					Code = "ro"
				}
			},
			{
				"Rundi",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "rn",
					GoogleCode = "-"
				}
			},
			{
				"Russian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 6,
					Code = "ru"
				}
			},
			{
				"Russian/Republic of Moldova",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 6,
					Code = "ru-MO",
					GoogleCode = "ru"
				}
			},
			{
				"Sanskrit",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "sa",
					GoogleCode = "-"
				}
			},
			{
				"Sardinian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "sc",
					GoogleCode = "-"
				}
			},
			{
				"Sindhi",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "sd"
				}
			},
			{
				"Northern Sami",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "se",
					GoogleCode = "-"
				}
			},
			{
				"Samoan",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "sm"
				}
			},
			{
				"Sango",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "sg",
					GoogleCode = "-"
				}
			},
			{
				"Serbian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 6,
					Code = "sr"
				}
			},
			{
				"Serbian/Bosnia and Herzegovina",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 5,
					Code = "sr-BA",
					GoogleCode = "sr"
				}
			},
			{
				"Serbian/Serbia and Montenegro",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 5,
					Code = "sr-SP",
					GoogleCode = "sr"
				}
			},
			{
				"Scottish Gaelic",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "gd"
				}
			},
			{
				"Shona",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "sn"
				}
			},
			{
				"Sinhala",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "si"
				}
			},
			{
				"Slovak",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 7,
					Code = "sk"
				}
			},
			{
				"Slovenian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 9,
					Code = "sl"
				}
			},
			{
				"Somali",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "so"
				}
			},
			{
				"Southern Sotho",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "st"
				}
			},
			{
				"Spanish",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es"
				}
			},
			{
				"Spanish/Argentina",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-AR",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Bolivia",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-BO",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Castilian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-ES",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Chile",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-CL",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Colombia",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-CO",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Costa Rica",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-CR",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Dominican Republic",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-DO",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Ecuador",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-EC",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/El Salvador",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-SV",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Guatemala",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-GT",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Honduras",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-HN",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Mexico",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-MX",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Nicaragua",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-NI",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Panama",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-PA",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Paraguay",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-PY",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Peru",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-PE",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Puerto Rico",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-PR",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Spain",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-ES",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Uruguay",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-UY",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Venezuela",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-VE",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Latin Americas",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "es-US",
					GoogleCode = "es"
				}
			},
			{
				"Sundanese",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "su"
				}
			},
			{
				"Swahili",
				new GoogleLanguages.LanguageCodeDef
				{
					Code = "sw"
				}
			},
			{
				"Swati",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ss",
					GoogleCode = "-"
				}
			},
			{
				"Swedish",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "sv"
				}
			},
			{
				"Swedish/Finland",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "sv-FI",
					GoogleCode = "sv"
				}
			},
			{
				"Swedish/Sweden",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "sv-SE",
					GoogleCode = "sv"
				}
			},
			{
				"Tamil",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ta"
				}
			},
			{
				"Tatar",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "tt",
					GoogleCode = "-"
				}
			},
			{
				"Telugu",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "te"
				}
			},
			{
				"Tajik",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "tg"
				}
			},
			{
				"Thai",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "th",
					HasJoinedWords = true
				}
			},
			{
				"Tigrinya",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ti",
					GoogleCode = "-"
				}
			},
			{
				"Tibetan",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "bo",
					GoogleCode = "-"
				}
			},
			{
				"Turkmen",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "tk",
					GoogleCode = "-"
				}
			},
			{
				"Tagalog",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "tl"
				}
			},
			{
				"Tswana",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "tn",
					GoogleCode = "-"
				}
			},
			{
				"Tonga",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "to",
					GoogleCode = "-"
				}
			},
			{
				"Turkish",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 0,
					Code = "tr"
				}
			},
			{
				"Tsonga",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ts",
					GoogleCode = "-"
				}
			},
			{
				"Twi",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "tw",
					GoogleCode = "-"
				}
			},
			{
				"Tahitian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ty",
					GoogleCode = "-"
				}
			},
			{
				"Uighur",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ug",
					GoogleCode = "-"
				}
			},
			{
				"Ukrainian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 6,
					Code = "uk"
				}
			},
			{
				"Urdu",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ur"
				}
			},
			{
				"Uzbek",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 2,
					Code = "uz"
				}
			},
			{
				"Venda",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "ve",
					GoogleCode = "-"
				}
			},
			{
				"Vietnamese",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "vi"
				}
			},
			{
				"Volapük",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "vo",
					GoogleCode = "-"
				}
			},
			{
				"Walloon",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "wa",
					GoogleCode = "-"
				}
			},
			{
				"Welsh",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 16,
					Code = "cy"
				}
			},
			{
				"Wolof",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "wo",
					GoogleCode = "-"
				}
			},
			{
				"Frisian",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "fy"
				}
			},
			{
				"Xhosa",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "xh"
				}
			},
			{
				"Yiddish",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "yi"
				}
			},
			{
				"Yoruba",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "yo"
				}
			},
			{
				"Zhuang",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "za",
					GoogleCode = "-"
				}
			},
			{
				"Zulu",
				new GoogleLanguages.LanguageCodeDef
				{
					PluralRule = 1,
					Code = "zu"
				}
			}
		};

		// Token: 0x020007F5 RID: 2037
		public struct LanguageCodeDef
		{
			// Token: 0x04003820 RID: 14368
			public string Code;

			// Token: 0x04003821 RID: 14369
			public string GoogleCode;

			// Token: 0x04003822 RID: 14370
			public bool HasJoinedWords;

			// Token: 0x04003823 RID: 14371
			public int PluralRule;
		}
	}
}
