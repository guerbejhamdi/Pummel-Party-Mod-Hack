using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace I2.Loc
{
	// Token: 0x02000802 RID: 2050
	public class TranslationJob_WEB : TranslationJob_WWW
	{
		// Token: 0x06003A0E RID: 14862 RVA: 0x000274F4 File Offset: 0x000256F4
		public TranslationJob_WEB(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady)
		{
			this._requests = requests;
			this._OnTranslationReady = OnTranslationReady;
			this.FindAllQueries();
			this.ExecuteNextBatch();
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x00126120 File Offset: 0x00124320
		private void FindAllQueries()
		{
			this.mQueries = new List<KeyValuePair<string, string>>();
			foreach (KeyValuePair<string, TranslationQuery> keyValuePair in this._requests)
			{
				foreach (string str in keyValuePair.Value.TargetLanguagesCode)
				{
					this.mQueries.Add(new KeyValuePair<string, string>(keyValuePair.Value.OrigText, keyValuePair.Value.LanguageCode + ":" + str));
				}
			}
			this.mQueries.Sort((KeyValuePair<string, string> a, KeyValuePair<string, string> b) => a.Value.CompareTo(b.Value));
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x001261F8 File Offset: 0x001243F8
		private void ExecuteNextBatch()
		{
			if (this.mQueries.Count == 0)
			{
				this.mJobState = TranslationJob.eJobState.Succeeded;
				return;
			}
			this.mCurrentBatch_Text = new List<string>();
			string text = null;
			int num = 200;
			StringBuilder stringBuilder = new StringBuilder();
			int i;
			for (i = 0; i < this.mQueries.Count; i++)
			{
				string key = this.mQueries[i].Key;
				string value = this.mQueries[i].Value;
				if (text == null || value == text)
				{
					if (i != 0)
					{
						stringBuilder.Append("|||");
					}
					stringBuilder.Append(key);
					this.mCurrentBatch_Text.Add(key);
					text = value;
				}
				if (stringBuilder.Length > num)
				{
					break;
				}
			}
			this.mQueries.RemoveRange(0, i);
			string[] array = text.Split(new char[]
			{
				':'
			});
			this.mCurrentBatch_FromLanguageCode = array[0];
			this.mCurrentBatch_ToLanguageCode = array[1];
			string text2 = string.Format("http://www.google.com/translate_t?hl=en&vi=c&ie=UTF8&oe=UTF8&submit=Translate&langpair={0}|{1}&text={2}", this.mCurrentBatch_FromLanguageCode, this.mCurrentBatch_ToLanguageCode, Uri.EscapeUriString(stringBuilder.ToString()));
			Debug.Log(text2);
			this.www = UnityWebRequest.Get(text2);
			I2Utils.SendWebRequest(this.www);
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x0012632C File Offset: 0x0012452C
		public override TranslationJob.eJobState GetState()
		{
			if (this.www != null && this.www.isDone)
			{
				this.ProcessResult(this.www.downloadHandler.data, this.www.error);
				this.www.Dispose();
				this.www = null;
			}
			if (this.www == null)
			{
				this.ExecuteNextBatch();
			}
			return this.mJobState;
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x00126398 File Offset: 0x00124598
		public void ProcessResult(byte[] bytes, string errorMsg)
		{
			if (string.IsNullOrEmpty(errorMsg))
			{
				string @string = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
				Debug.Log(this.ParseTranslationResult(@string, "aab"));
				if (string.IsNullOrEmpty(errorMsg))
				{
					if (this._OnTranslationReady != null)
					{
						this._OnTranslationReady(this._requests, null);
					}
					return;
				}
			}
			this.mJobState = TranslationJob.eJobState.Failed;
			this.mErrorMessage = errorMsg;
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x00126400 File Offset: 0x00124600
		private string ParseTranslationResult(string html, string OriginalText)
		{
			string result;
			try
			{
				int num = html.IndexOf("TRANSLATED_TEXT='") + "TRANSLATED_TEXT='".Length;
				int num2 = html.IndexOf("';var", num);
				string text = html.Substring(num, num2 - num);
				text = Regex.Replace(text, "\\\\x([a-fA-F0-9]{2})", (Match match) => char.ConvertFromUtf32(int.Parse(match.Groups[1].Value, NumberStyles.HexNumber)));
				text = Regex.Replace(text, "&#(\\d+);", (Match match) => char.ConvertFromUtf32(int.Parse(match.Groups[1].Value)));
				text = text.Replace("<br>", "\n");
				if (OriginalText.ToUpper() == OriginalText)
				{
					text = text.ToUpper();
				}
				else if (GoogleTranslation.UppercaseFirst(OriginalText) == OriginalText)
				{
					text = GoogleTranslation.UppercaseFirst(text);
				}
				else if (GoogleTranslation.TitleCase(OriginalText) == OriginalText)
				{
					text = GoogleTranslation.TitleCase(text);
				}
				result = text;
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x04003844 RID: 14404
		private Dictionary<string, TranslationQuery> _requests;

		// Token: 0x04003845 RID: 14405
		private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;

		// Token: 0x04003846 RID: 14406
		public string mErrorMessage;

		// Token: 0x04003847 RID: 14407
		private string mCurrentBatch_ToLanguageCode;

		// Token: 0x04003848 RID: 14408
		private string mCurrentBatch_FromLanguageCode;

		// Token: 0x04003849 RID: 14409
		private List<string> mCurrentBatch_Text;

		// Token: 0x0400384A RID: 14410
		private List<KeyValuePair<string, string>> mQueries;
	}
}
