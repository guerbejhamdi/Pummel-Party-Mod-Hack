using System;

namespace I2.Loc
{
	// Token: 0x02000843 RID: 2115
	[Serializable]
	public struct LocalizedString
	{
		// Token: 0x06003BF7 RID: 15351 RVA: 0x000282DC File Offset: 0x000264DC
		public static implicit operator string(LocalizedString s)
		{
			return s.ToString();
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x0012D044 File Offset: 0x0012B244
		public static implicit operator LocalizedString(string term)
		{
			return new LocalizedString
			{
				mTerm = term
			};
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x000282EB File Offset: 0x000264EB
		public LocalizedString(LocalizedString str)
		{
			this.mTerm = str.mTerm;
			this.mRTL_IgnoreArabicFix = str.mRTL_IgnoreArabicFix;
			this.mRTL_MaxLineLength = str.mRTL_MaxLineLength;
			this.mRTL_ConvertNumbers = str.mRTL_ConvertNumbers;
			this.m_DontLocalizeParameters = str.m_DontLocalizeParameters;
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x0012D064 File Offset: 0x0012B264
		public override string ToString()
		{
			string translation = LocalizationManager.GetTranslation(this.mTerm, !this.mRTL_IgnoreArabicFix, this.mRTL_MaxLineLength, !this.mRTL_ConvertNumbers, true, null, null, true);
			LocalizationManager.ApplyLocalizationParams(ref translation, !this.m_DontLocalizeParameters);
			return translation;
		}

		// Token: 0x04003932 RID: 14642
		public string mTerm;

		// Token: 0x04003933 RID: 14643
		public bool mRTL_IgnoreArabicFix;

		// Token: 0x04003934 RID: 14644
		public int mRTL_MaxLineLength;

		// Token: 0x04003935 RID: 14645
		public bool mRTL_ConvertNumbers;

		// Token: 0x04003936 RID: 14646
		public bool m_DontLocalizeParameters;
	}
}
