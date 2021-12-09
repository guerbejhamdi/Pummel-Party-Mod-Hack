using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace I2.Loc
{
	// Token: 0x02000801 RID: 2049
	public class TranslationJob_POST : TranslationJob_WWW
	{
		// Token: 0x06003A0B RID: 14859 RVA: 0x00125FF8 File Offset: 0x001241F8
		public TranslationJob_POST(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady)
		{
			this._requests = requests;
			this._OnTranslationReady = OnTranslationReady;
			List<string> list = GoogleTranslation.ConvertTranslationRequest(requests, false);
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("action", "Translate");
			wwwform.AddField("list", list[0]);
			this.www = UnityWebRequest.Post(LocalizationManager.GetWebServiceURL(null), wwwform);
			I2Utils.SendWebRequest(this.www);
		}

		// Token: 0x06003A0C RID: 14860 RVA: 0x00126068 File Offset: 0x00124268
		public override TranslationJob.eJobState GetState()
		{
			if (this.www != null && this.www.isDone)
			{
				this.ProcessResult(this.www.downloadHandler.data, this.www.error);
				this.www.Dispose();
				this.www = null;
			}
			return this.mJobState;
		}

		// Token: 0x06003A0D RID: 14861 RVA: 0x001260C4 File Offset: 0x001242C4
		public void ProcessResult(byte[] bytes, string errorMsg)
		{
			if (!string.IsNullOrEmpty(errorMsg))
			{
				this.mJobState = TranslationJob.eJobState.Failed;
				return;
			}
			errorMsg = GoogleTranslation.ParseTranslationResult(Encoding.UTF8.GetString(bytes, 0, bytes.Length), this._requests);
			if (this._OnTranslationReady != null)
			{
				this._OnTranslationReady(this._requests, errorMsg);
			}
			this.mJobState = TranslationJob.eJobState.Succeeded;
		}

		// Token: 0x04003842 RID: 14402
		private Dictionary<string, TranslationQuery> _requests;

		// Token: 0x04003843 RID: 14403
		private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;
	}
}
