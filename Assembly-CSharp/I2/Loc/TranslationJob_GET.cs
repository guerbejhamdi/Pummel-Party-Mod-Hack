using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace I2.Loc
{
	// Token: 0x020007FF RID: 2047
	public class TranslationJob_GET : TranslationJob_WWW
	{
		// Token: 0x06003A04 RID: 14852 RVA: 0x00027471 File Offset: 0x00025671
		public TranslationJob_GET(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady)
		{
			this._requests = requests;
			this._OnTranslationReady = OnTranslationReady;
			this.mQueries = GoogleTranslation.ConvertTranslationRequest(requests, true);
			this.GetState();
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x00125D74 File Offset: 0x00123F74
		private void ExecuteNextQuery()
		{
			if (this.mQueries.Count == 0)
			{
				this.mJobState = TranslationJob.eJobState.Succeeded;
				return;
			}
			int index = this.mQueries.Count - 1;
			string arg = this.mQueries[index];
			this.mQueries.RemoveAt(index);
			string uri = string.Format("{0}?action=Translate&list={1}", LocalizationManager.GetWebServiceURL(null), arg);
			this.www = UnityWebRequest.Get(uri);
			I2Utils.SendWebRequest(this.www);
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x00125DE8 File Offset: 0x00123FE8
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
				this.ExecuteNextQuery();
			}
			return this.mJobState;
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x00125E54 File Offset: 0x00124054
		public void ProcessResult(byte[] bytes, string errorMsg)
		{
			if (string.IsNullOrEmpty(errorMsg))
			{
				errorMsg = GoogleTranslation.ParseTranslationResult(Encoding.UTF8.GetString(bytes, 0, bytes.Length), this._requests);
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

		// Token: 0x04003838 RID: 14392
		private Dictionary<string, TranslationQuery> _requests;

		// Token: 0x04003839 RID: 14393
		private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;

		// Token: 0x0400383A RID: 14394
		private List<string> mQueries;

		// Token: 0x0400383B RID: 14395
		public string mErrorMessage;
	}
}
