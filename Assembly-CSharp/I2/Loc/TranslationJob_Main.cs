using System;
using System.Collections.Generic;

namespace I2.Loc
{
	// Token: 0x02000800 RID: 2048
	public class TranslationJob_Main : TranslationJob
	{
		// Token: 0x06003A08 RID: 14856 RVA: 0x0002749B File Offset: 0x0002569B
		public TranslationJob_Main(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady)
		{
			this._requests = requests;
			this._OnTranslationReady = OnTranslationReady;
			this.mPost = new TranslationJob_POST(requests, OnTranslationReady);
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x00125EB8 File Offset: 0x001240B8
		public override TranslationJob.eJobState GetState()
		{
			if (this.mWeb != null)
			{
				switch (this.mWeb.GetState())
				{
				case TranslationJob.eJobState.Running:
					return TranslationJob.eJobState.Running;
				case TranslationJob.eJobState.Succeeded:
					this.mJobState = TranslationJob.eJobState.Succeeded;
					break;
				case TranslationJob.eJobState.Failed:
					this.mWeb.Dispose();
					this.mWeb = null;
					this.mPost = new TranslationJob_POST(this._requests, this._OnTranslationReady);
					break;
				}
			}
			if (this.mPost != null)
			{
				switch (this.mPost.GetState())
				{
				case TranslationJob.eJobState.Running:
					return TranslationJob.eJobState.Running;
				case TranslationJob.eJobState.Succeeded:
					this.mJobState = TranslationJob.eJobState.Succeeded;
					break;
				case TranslationJob.eJobState.Failed:
					this.mPost.Dispose();
					this.mPost = null;
					this.mGet = new TranslationJob_GET(this._requests, this._OnTranslationReady);
					break;
				}
			}
			if (this.mGet != null)
			{
				switch (this.mGet.GetState())
				{
				case TranslationJob.eJobState.Running:
					return TranslationJob.eJobState.Running;
				case TranslationJob.eJobState.Succeeded:
					this.mJobState = TranslationJob.eJobState.Succeeded;
					break;
				case TranslationJob.eJobState.Failed:
					this.mErrorMessage = this.mGet.mErrorMessage;
					if (this._OnTranslationReady != null)
					{
						this._OnTranslationReady(this._requests, this.mErrorMessage);
					}
					this.mGet.Dispose();
					this.mGet = null;
					break;
				}
			}
			return this.mJobState;
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x000274BE File Offset: 0x000256BE
		public override void Dispose()
		{
			if (this.mPost != null)
			{
				this.mPost.Dispose();
			}
			if (this.mGet != null)
			{
				this.mGet.Dispose();
			}
			this.mPost = null;
			this.mGet = null;
		}

		// Token: 0x0400383C RID: 14396
		private TranslationJob_WEB mWeb;

		// Token: 0x0400383D RID: 14397
		private TranslationJob_POST mPost;

		// Token: 0x0400383E RID: 14398
		private TranslationJob_GET mGet;

		// Token: 0x0400383F RID: 14399
		private Dictionary<string, TranslationQuery> _requests;

		// Token: 0x04003840 RID: 14400
		private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;

		// Token: 0x04003841 RID: 14401
		public string mErrorMessage;
	}
}
