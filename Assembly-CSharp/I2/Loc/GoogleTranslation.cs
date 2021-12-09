using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

namespace I2.Loc
{
	// Token: 0x020007F6 RID: 2038
	public static class GoogleTranslation
	{
		// Token: 0x060039D3 RID: 14803 RVA: 0x00027380 File Offset: 0x00025580
		public static bool CanTranslate()
		{
			return LocalizationManager.Sources.Count > 0 && !string.IsNullOrEmpty(LocalizationManager.GetWebServiceURL(null));
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x00125018 File Offset: 0x00123218
		public static void Translate(string text, string LanguageCodeFrom, string LanguageCodeTo, GoogleTranslation.fnOnTranslated OnTranslationReady)
		{
			LocalizationManager.InitializeIfNeeded();
			if (!GoogleTranslation.CanTranslate())
			{
				OnTranslationReady(null, "WebService is not set correctly or needs to be reinstalled");
				return;
			}
			if (LanguageCodeTo == LanguageCodeFrom)
			{
				OnTranslationReady(text, null);
				return;
			}
			Dictionary<string, TranslationQuery> queries = new Dictionary<string, TranslationQuery>();
			if (string.IsNullOrEmpty(LanguageCodeTo))
			{
				OnTranslationReady(string.Empty, null);
				return;
			}
			GoogleTranslation.CreateQueries(text, LanguageCodeFrom, LanguageCodeTo, queries);
			GoogleTranslation.Translate(queries, delegate(Dictionary<string, TranslationQuery> results, string error)
			{
				if (!string.IsNullOrEmpty(error) || results.Count == 0)
				{
					OnTranslationReady(null, error);
					return;
				}
				string translation = GoogleTranslation.RebuildTranslation(text, queries, LanguageCodeTo);
				OnTranslationReady(translation, null);
			}, true);
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x001250DC File Offset: 0x001232DC
		public static string ForceTranslate(string text, string LanguageCodeFrom, string LanguageCodeTo)
		{
			Dictionary<string, TranslationQuery> dictionary = new Dictionary<string, TranslationQuery>();
			GoogleTranslation.AddQuery(text, LanguageCodeFrom, LanguageCodeTo, dictionary);
			TranslationJob_Main translationJob_Main = new TranslationJob_Main(dictionary, null);
			TranslationJob.eJobState state;
			do
			{
				state = translationJob_Main.GetState();
			}
			while (state == TranslationJob.eJobState.Running);
			if (state == TranslationJob.eJobState.Failed)
			{
				return null;
			}
			return GoogleTranslation.GetQueryResult(text, "", dictionary);
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x0002739F File Offset: 0x0002559F
		public static void Translate(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady, bool usePOST = true)
		{
			GoogleTranslation.AddTranslationJob(new TranslationJob_Main(requests, OnTranslationReady));
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x0012511C File Offset: 0x0012331C
		public static bool ForceTranslate(Dictionary<string, TranslationQuery> requests, bool usePOST = true)
		{
			TranslationJob_Main translationJob_Main = new TranslationJob_Main(requests, null);
			TranslationJob.eJobState state;
			do
			{
				state = translationJob_Main.GetState();
			}
			while (state == TranslationJob.eJobState.Running);
			return state != TranslationJob.eJobState.Failed;
		}

		// Token: 0x060039D8 RID: 14808 RVA: 0x00125144 File Offset: 0x00123344
		public static List<string> ConvertTranslationRequest(Dictionary<string, TranslationQuery> requests, bool encodeGET)
		{
			List<string> list = new List<string>();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, TranslationQuery> keyValuePair in requests)
			{
				TranslationQuery value = keyValuePair.Value;
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append("<I2Loc>");
				}
				stringBuilder.Append(GoogleLanguages.GetGoogleLanguageCode(value.LanguageCode));
				stringBuilder.Append(":");
				for (int i = 0; i < value.TargetLanguagesCode.Length; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(GoogleLanguages.GetGoogleLanguageCode(value.TargetLanguagesCode[i]));
				}
				stringBuilder.Append("=");
				string text = (GoogleTranslation.TitleCase(value.Text) == value.Text) ? value.Text.ToLowerInvariant() : value.Text;
				if (!encodeGET)
				{
					stringBuilder.Append(text);
				}
				else
				{
					stringBuilder.Append(Uri.EscapeDataString(text));
					if (stringBuilder.Length > 4000)
					{
						list.Add(stringBuilder.ToString());
						stringBuilder.Length = 0;
					}
				}
			}
			list.Add(stringBuilder.ToString());
			return list;
		}

		// Token: 0x060039D9 RID: 14809 RVA: 0x000273AD File Offset: 0x000255AD
		private static void AddTranslationJob(TranslationJob job)
		{
			GoogleTranslation.mTranslationJobs.Add(job);
			if (GoogleTranslation.mTranslationJobs.Count == 1)
			{
				CoroutineManager.Start(GoogleTranslation.WaitForTranslations());
			}
		}

		// Token: 0x060039DA RID: 14810 RVA: 0x000273D2 File Offset: 0x000255D2
		private static IEnumerator WaitForTranslations()
		{
			while (GoogleTranslation.mTranslationJobs.Count > 0)
			{
				foreach (TranslationJob translationJob in GoogleTranslation.mTranslationJobs.ToArray())
				{
					if (translationJob.GetState() != TranslationJob.eJobState.Running)
					{
						GoogleTranslation.mTranslationJobs.Remove(translationJob);
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060039DB RID: 14811 RVA: 0x001252AC File Offset: 0x001234AC
		public static string ParseTranslationResult(string html, Dictionary<string, TranslationQuery> requests)
		{
			if (!html.StartsWith("<!DOCTYPE html>") && !html.StartsWith("<HTML>"))
			{
				string[] array = html.Split(new string[]
				{
					"<I2Loc>"
				}, StringSplitOptions.None);
				string[] separator = new string[]
				{
					"<i2>"
				};
				int num = 0;
				foreach (string text in requests.Keys.ToArray<string>())
				{
					TranslationQuery translationQuery = GoogleTranslation.FindQueryFromOrigText(text, requests);
					string text2 = array[num++];
					if (translationQuery.Tags != null)
					{
						for (int j = translationQuery.Tags.Length - 1; j >= 0; j--)
						{
							text2 = text2.Replace(GoogleTranslation.GetGoogleNoTranslateTag(j), translationQuery.Tags[j]);
						}
					}
					translationQuery.Results = text2.Split(separator, StringSplitOptions.None);
					if (GoogleTranslation.TitleCase(text) == text)
					{
						for (int k = 0; k < translationQuery.Results.Length; k++)
						{
							translationQuery.Results[k] = GoogleTranslation.TitleCase(translationQuery.Results[k]);
						}
					}
					requests[translationQuery.OrigText] = translationQuery;
				}
				return null;
			}
			if (html.Contains("The script completed but did not return anything"))
			{
				return "The current Google WebService is not supported.\nPlease, delete the WebService from the Google Drive and Install the latest version.";
			}
			if (html.Contains("Service invoked too many times in a short time"))
			{
				return "";
			}
			return "There was a problem contacting the WebService. Please try again later\n" + html;
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x000273DA File Offset: 0x000255DA
		public static bool IsTranslating()
		{
			return GoogleTranslation.mCurrentTranslations.Count > 0 || GoogleTranslation.mTranslationJobs.Count > 0;
		}

		// Token: 0x060039DD RID: 14813 RVA: 0x00125408 File Offset: 0x00123608
		public static void CancelCurrentGoogleTranslations()
		{
			GoogleTranslation.mCurrentTranslations.Clear();
			foreach (TranslationJob translationJob in GoogleTranslation.mTranslationJobs)
			{
				translationJob.Dispose();
			}
			GoogleTranslation.mTranslationJobs.Clear();
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x0012546C File Offset: 0x0012366C
		public static void CreateQueries(string text, string LanguageCodeFrom, string LanguageCodeTo, Dictionary<string, TranslationQuery> dict)
		{
			if (!text.Contains("[i2s_"))
			{
				GoogleTranslation.CreateQueries_Plurals(text, LanguageCodeFrom, LanguageCodeTo, dict);
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in SpecializationManager.GetSpecializations(text, null))
			{
				GoogleTranslation.CreateQueries_Plurals(keyValuePair.Value, LanguageCodeFrom, LanguageCodeTo, dict);
			}
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x001254E0 File Offset: 0x001236E0
		private static void CreateQueries_Plurals(string text, string LanguageCodeFrom, string LanguageCodeTo, Dictionary<string, TranslationQuery> dict)
		{
			bool flag = text.Contains("{[#");
			bool flag2 = text.Contains("[i2p_");
			if (!GoogleTranslation.HasParameters(text) || (!flag && !flag2))
			{
				GoogleTranslation.AddQuery(text, LanguageCodeFrom, LanguageCodeTo, dict);
				return;
			}
			bool forceTag = flag;
			for (ePluralType ePluralType = ePluralType.Zero; ePluralType <= ePluralType.Plural; ePluralType++)
			{
				string pluralType = ePluralType.ToString();
				if (GoogleLanguages.LanguageHasPluralType(LanguageCodeTo, pluralType))
				{
					string text2 = GoogleTranslation.GetPluralText(text, pluralType);
					int pluralTestNumber = GoogleLanguages.GetPluralTestNumber(LanguageCodeTo, ePluralType);
					string pluralParameter = GoogleTranslation.GetPluralParameter(text2, forceTag);
					if (!string.IsNullOrEmpty(pluralParameter))
					{
						text2 = text2.Replace(pluralParameter, pluralTestNumber.ToString());
					}
					GoogleTranslation.AddQuery(text2, LanguageCodeFrom, LanguageCodeTo, dict);
				}
			}
		}

		// Token: 0x060039E0 RID: 14816 RVA: 0x00125588 File Offset: 0x00123788
		public static void AddQuery(string text, string LanguageCodeFrom, string LanguageCodeTo, Dictionary<string, TranslationQuery> dict)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (!dict.ContainsKey(text))
			{
				TranslationQuery value = new TranslationQuery
				{
					OrigText = text,
					LanguageCode = LanguageCodeFrom,
					TargetLanguagesCode = new string[]
					{
						LanguageCodeTo
					}
				};
				value.Text = text;
				GoogleTranslation.ParseNonTranslatableElements(ref value);
				dict[text] = value;
				return;
			}
			TranslationQuery translationQuery = dict[text];
			if (Array.IndexOf<string>(translationQuery.TargetLanguagesCode, LanguageCodeTo) < 0)
			{
				translationQuery.TargetLanguagesCode = translationQuery.TargetLanguagesCode.Concat(new string[]
				{
					LanguageCodeTo
				}).Distinct<string>().ToArray<string>();
			}
			dict[text] = translationQuery;
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x00125630 File Offset: 0x00123830
		private static string GetTranslation(string text, string LanguageCodeTo, Dictionary<string, TranslationQuery> dict)
		{
			if (!dict.ContainsKey(text))
			{
				return null;
			}
			TranslationQuery translationQuery = dict[text];
			int num = Array.IndexOf<string>(translationQuery.TargetLanguagesCode, LanguageCodeTo);
			if (num < 0)
			{
				return "";
			}
			if (translationQuery.Results == null)
			{
				return "";
			}
			return translationQuery.Results[num];
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x00125680 File Offset: 0x00123880
		private static TranslationQuery FindQueryFromOrigText(string origText, Dictionary<string, TranslationQuery> dict)
		{
			foreach (KeyValuePair<string, TranslationQuery> keyValuePair in dict)
			{
				if (keyValuePair.Value.OrigText == origText)
				{
					return keyValuePair.Value;
				}
			}
			return default(TranslationQuery);
		}

		// Token: 0x060039E3 RID: 14819 RVA: 0x001256F0 File Offset: 0x001238F0
		public static bool HasParameters(string text)
		{
			int num = text.IndexOf("{[");
			return num >= 0 && text.IndexOf("]}", num) > 0;
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x00125720 File Offset: 0x00123920
		public static string GetPluralParameter(string text, bool forceTag)
		{
			int num = text.IndexOf("{[#");
			if (num < 0)
			{
				if (forceTag)
				{
					return null;
				}
				num = text.IndexOf("{[");
			}
			if (num < 0)
			{
				return null;
			}
			int num2 = text.IndexOf("]}", num + 2);
			if (num2 < 0)
			{
				return null;
			}
			return text.Substring(num, num2 - num + 2);
		}

		// Token: 0x060039E5 RID: 14821 RVA: 0x00125778 File Offset: 0x00123978
		public static string GetPluralText(string text, string pluralType)
		{
			pluralType = "[i2p_" + pluralType + "]";
			int num = text.IndexOf(pluralType);
			if (num >= 0)
			{
				num += pluralType.Length;
				int num2 = text.IndexOf("[i2p_", num);
				if (num2 < 0)
				{
					num2 = text.Length;
				}
				return text.Substring(num, num2 - num);
			}
			num = text.IndexOf("[i2p_");
			if (num < 0)
			{
				return text;
			}
			if (num > 0)
			{
				return text.Substring(0, num);
			}
			num = text.IndexOf("]");
			if (num < 0)
			{
				return text;
			}
			num++;
			int num3 = text.IndexOf("[i2p_", num);
			if (num3 < 0)
			{
				num3 = text.Length;
			}
			return text.Substring(num, num3 - num);
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x00125828 File Offset: 0x00123A28
		private static int FindClosingTag(string tag, MatchCollection matches, int startIndex)
		{
			int i = startIndex;
			int count = matches.Count;
			while (i < count)
			{
				string captureMatch = I2Utils.GetCaptureMatch(matches[i]);
				if (captureMatch[0] == '/' && tag.StartsWith(captureMatch.Substring(1)))
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x00125874 File Offset: 0x00123A74
		private static string GetGoogleNoTranslateTag(int tagNumber)
		{
			if (tagNumber < 70)
			{
				return "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++".Substring(0, tagNumber + 1);
			}
			string text = "";
			for (int i = -1; i < tagNumber; i++)
			{
				text += "+";
			}
			return text;
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x001258B4 File Offset: 0x00123AB4
		private static void ParseNonTranslatableElements(ref TranslationQuery query)
		{
			MatchCollection matchCollection = Regex.Matches(query.Text, "\\{\\[(.*?)]}|\\[(.*?)]|\\<(.*?)>");
			if (matchCollection == null || matchCollection.Count == 0)
			{
				return;
			}
			string text = query.Text;
			List<string> list = new List<string>();
			int i = 0;
			int count = matchCollection.Count;
			while (i < count)
			{
				string captureMatch = I2Utils.GetCaptureMatch(matchCollection[i]);
				int num = GoogleTranslation.FindClosingTag(captureMatch, matchCollection, i);
				if (num < 0)
				{
					string text2 = matchCollection[i].ToString();
					if (text2.StartsWith("{[") && text2.EndsWith("]}"))
					{
						text = text.Replace(text2, GoogleTranslation.GetGoogleNoTranslateTag(list.Count) + " ");
						list.Add(text2);
					}
				}
				else if (captureMatch == "i2nt")
				{
					string text3 = query.Text.Substring(matchCollection[i].Index, matchCollection[num].Index - matchCollection[i].Index + matchCollection[num].Length);
					text = text.Replace(text3, GoogleTranslation.GetGoogleNoTranslateTag(list.Count) + " ");
					list.Add(text3);
				}
				else
				{
					string text4 = matchCollection[i].ToString();
					text = text.Replace(text4, GoogleTranslation.GetGoogleNoTranslateTag(list.Count) + " ");
					list.Add(text4);
					string text5 = matchCollection[num].ToString();
					text = text.Replace(text5, GoogleTranslation.GetGoogleNoTranslateTag(list.Count) + " ");
					list.Add(text5);
				}
				i++;
			}
			query.Text = text;
			query.Tags = list.ToArray();
		}

		// Token: 0x060039E9 RID: 14825 RVA: 0x00125A74 File Offset: 0x00123C74
		public static string GetQueryResult(string text, string LanguageCodeTo, Dictionary<string, TranslationQuery> dict)
		{
			if (!dict.ContainsKey(text))
			{
				return null;
			}
			TranslationQuery translationQuery = dict[text];
			if (translationQuery.Results == null || translationQuery.Results.Length < 0)
			{
				return null;
			}
			if (string.IsNullOrEmpty(LanguageCodeTo))
			{
				return translationQuery.Results[0];
			}
			int num = Array.IndexOf<string>(translationQuery.TargetLanguagesCode, LanguageCodeTo);
			if (num < 0)
			{
				return null;
			}
			return translationQuery.Results[num];
		}

		// Token: 0x060039EA RID: 14826 RVA: 0x00125AD8 File Offset: 0x00123CD8
		public static string RebuildTranslation(string text, Dictionary<string, TranslationQuery> dict, string LanguageCodeTo)
		{
			if (!text.Contains("[i2s_"))
			{
				return GoogleTranslation.RebuildTranslation_Plural(text, dict, LanguageCodeTo);
			}
			Dictionary<string, string> specializations = SpecializationManager.GetSpecializations(text, null);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> keyValuePair in specializations)
			{
				dictionary[keyValuePair.Key] = GoogleTranslation.RebuildTranslation_Plural(keyValuePair.Value, dict, LanguageCodeTo);
			}
			return SpecializationManager.SetSpecializedText(dictionary);
		}

		// Token: 0x060039EB RID: 14827 RVA: 0x00125B64 File Offset: 0x00123D64
		private static string RebuildTranslation_Plural(string text, Dictionary<string, TranslationQuery> dict, string LanguageCodeTo)
		{
			bool flag = text.Contains("{[#");
			bool flag2 = text.Contains("[i2p_");
			if (!GoogleTranslation.HasParameters(text) || (!flag && !flag2))
			{
				return GoogleTranslation.GetTranslation(text, LanguageCodeTo, dict);
			}
			StringBuilder stringBuilder = new StringBuilder();
			string b = null;
			bool forceTag = flag;
			for (ePluralType ePluralType = ePluralType.Plural; ePluralType >= ePluralType.Zero; ePluralType--)
			{
				string text2 = ePluralType.ToString();
				if (GoogleLanguages.LanguageHasPluralType(LanguageCodeTo, text2))
				{
					string text3 = GoogleTranslation.GetPluralText(text, text2);
					int pluralTestNumber = GoogleLanguages.GetPluralTestNumber(LanguageCodeTo, ePluralType);
					string pluralParameter = GoogleTranslation.GetPluralParameter(text3, forceTag);
					if (!string.IsNullOrEmpty(pluralParameter))
					{
						text3 = text3.Replace(pluralParameter, pluralTestNumber.ToString());
					}
					string text4 = GoogleTranslation.GetTranslation(text3, LanguageCodeTo, dict);
					if (!string.IsNullOrEmpty(pluralParameter))
					{
						text4 = text4.Replace(pluralTestNumber.ToString(), pluralParameter);
					}
					if (ePluralType == ePluralType.Plural)
					{
						b = text4;
					}
					else
					{
						if (text4 == b)
						{
							goto IL_E9;
						}
						stringBuilder.AppendFormat("[i2p_{0}]", text2);
					}
					stringBuilder.Append(text4);
				}
				IL_E9:;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x00125C70 File Offset: 0x00123E70
		public static string UppercaseFirst(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			char[] array = s.ToLower().ToCharArray();
			array[0] = char.ToUpper(array[0]);
			return new string(array);
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x000273F8 File Offset: 0x000255F8
		public static string TitleCase(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
		}

		// Token: 0x04003824 RID: 14372
		private static List<UnityWebRequest> mCurrentTranslations = new List<UnityWebRequest>();

		// Token: 0x04003825 RID: 14373
		private static List<TranslationJob> mTranslationJobs = new List<TranslationJob>();

		// Token: 0x020007F7 RID: 2039
		// (Invoke) Token: 0x060039F0 RID: 14832
		public delegate void fnOnTranslated(string Translation, string Error);

		// Token: 0x020007F8 RID: 2040
		// (Invoke) Token: 0x060039F4 RID: 14836
		public delegate void fnOnTranslationReady(Dictionary<string, TranslationQuery> dict, string error);
	}
}
