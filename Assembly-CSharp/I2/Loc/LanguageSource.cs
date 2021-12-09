using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000806 RID: 2054
	[AddComponentMenu("I2/Localization/Source")]
	[ExecuteInEditMode]
	public class LanguageSource : MonoBehaviour, ISerializationCallbackReceiver, ILanguageSource
	{
		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06003A20 RID: 14880 RVA: 0x0002760C File Offset: 0x0002580C
		// (set) Token: 0x06003A21 RID: 14881 RVA: 0x00027614 File Offset: 0x00025814
		public LanguageSourceData SourceData
		{
			get
			{
				return this.mSource;
			}
			set
			{
				this.mSource = value;
			}
		}

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06003A22 RID: 14882 RVA: 0x00126510 File Offset: 0x00124710
		// (remove) Token: 0x06003A23 RID: 14883 RVA: 0x00126548 File Offset: 0x00124748
		public event LanguageSource.fnOnSourceUpdated Event_OnSourceUpdateFromGoogle;

		// Token: 0x06003A24 RID: 14884 RVA: 0x0002761D File Offset: 0x0002581D
		private void Awake()
		{
			this.mSource.owner = this;
			this.mSource.Awake();
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x00027636 File Offset: 0x00025836
		private void OnDestroy()
		{
			this.NeverDestroy = false;
			if (!this.NeverDestroy)
			{
				this.mSource.OnDestroy();
			}
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x00126580 File Offset: 0x00124780
		public string GetSourceName()
		{
			string text = base.gameObject.name;
			Transform parent = base.transform.parent;
			while (parent)
			{
				text = parent.name + "_" + text;
				parent = parent.parent;
			}
			return text;
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x00027652 File Offset: 0x00025852
		public void OnBeforeSerialize()
		{
			this.version = 1;
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x001265CC File Offset: 0x001247CC
		public void OnAfterDeserialize()
		{
			if (this.version == 0 || this.mSource == null)
			{
				this.mSource = new LanguageSourceData();
				this.mSource.owner = this;
				this.mSource.UserAgreesToHaveItOnTheScene = this.UserAgreesToHaveItOnTheScene;
				this.mSource.UserAgreesToHaveItInsideThePluginsFolder = this.UserAgreesToHaveItInsideThePluginsFolder;
				this.mSource.IgnoreDeviceLanguage = this.IgnoreDeviceLanguage;
				this.mSource._AllowUnloadingLanguages = this._AllowUnloadingLanguages;
				this.mSource.CaseInsensitiveTerms = this.CaseInsensitiveTerms;
				this.mSource.OnMissingTranslation = this.OnMissingTranslation;
				this.mSource.mTerm_AppName = this.mTerm_AppName;
				this.mSource.GoogleLiveSyncIsUptoDate = this.GoogleLiveSyncIsUptoDate;
				this.mSource.Google_WebServiceURL = this.Google_WebServiceURL;
				this.mSource.Google_SpreadsheetKey = this.Google_SpreadsheetKey;
				this.mSource.Google_SpreadsheetName = this.Google_SpreadsheetName;
				this.mSource.Google_LastUpdatedVersion = this.Google_LastUpdatedVersion;
				this.mSource.GoogleUpdateFrequency = this.GoogleUpdateFrequency;
				this.mSource.GoogleUpdateDelay = this.GoogleUpdateDelay;
				this.mSource.Event_OnSourceUpdateFromGoogle += this.Event_OnSourceUpdateFromGoogle;
				if (this.mLanguages != null && this.mLanguages.Count > 0)
				{
					this.mSource.mLanguages.Clear();
					this.mSource.mLanguages.AddRange(this.mLanguages);
					this.mLanguages.Clear();
				}
				if (this.Assets != null && this.Assets.Count > 0)
				{
					this.mSource.Assets.Clear();
					this.mSource.Assets.AddRange(this.Assets);
					this.Assets.Clear();
				}
				if (this.mTerms != null && this.mTerms.Count > 0)
				{
					this.mSource.mTerms.Clear();
					for (int i = 0; i < this.mTerms.Count; i++)
					{
						this.mSource.mTerms.Add(this.mTerms[i]);
					}
					this.mTerms.Clear();
				}
				this.version = 1;
				this.Event_OnSourceUpdateFromGoogle = null;
			}
		}

		// Token: 0x04003857 RID: 14423
		public LanguageSourceData mSource = new LanguageSourceData();

		// Token: 0x04003858 RID: 14424
		public int version;

		// Token: 0x04003859 RID: 14425
		public bool NeverDestroy;

		// Token: 0x0400385A RID: 14426
		public bool UserAgreesToHaveItOnTheScene;

		// Token: 0x0400385B RID: 14427
		public bool UserAgreesToHaveItInsideThePluginsFolder;

		// Token: 0x0400385C RID: 14428
		public bool GoogleLiveSyncIsUptoDate = true;

		// Token: 0x0400385D RID: 14429
		public List<UnityEngine.Object> Assets = new List<UnityEngine.Object>();

		// Token: 0x0400385E RID: 14430
		public string Google_WebServiceURL;

		// Token: 0x0400385F RID: 14431
		public string Google_SpreadsheetKey;

		// Token: 0x04003860 RID: 14432
		public string Google_SpreadsheetName;

		// Token: 0x04003861 RID: 14433
		public string Google_LastUpdatedVersion;

		// Token: 0x04003862 RID: 14434
		public LanguageSourceData.eGoogleUpdateFrequency GoogleUpdateFrequency = LanguageSourceData.eGoogleUpdateFrequency.Weekly;

		// Token: 0x04003863 RID: 14435
		public float GoogleUpdateDelay = 5f;

		// Token: 0x04003865 RID: 14437
		public List<LanguageData> mLanguages = new List<LanguageData>();

		// Token: 0x04003866 RID: 14438
		public bool IgnoreDeviceLanguage;

		// Token: 0x04003867 RID: 14439
		public LanguageSourceData.eAllowUnloadLanguages _AllowUnloadingLanguages;

		// Token: 0x04003868 RID: 14440
		public List<TermData> mTerms = new List<TermData>();

		// Token: 0x04003869 RID: 14441
		public bool CaseInsensitiveTerms;

		// Token: 0x0400386A RID: 14442
		public LanguageSourceData.MissingTranslationAction OnMissingTranslation = LanguageSourceData.MissingTranslationAction.Fallback;

		// Token: 0x0400386B RID: 14443
		public string mTerm_AppName;

		// Token: 0x02000807 RID: 2055
		// (Invoke) Token: 0x06003A2B RID: 14891
		public delegate void fnOnSourceUpdated(LanguageSourceData source, bool ReceivedNewData, string errorMsg);
	}
}
