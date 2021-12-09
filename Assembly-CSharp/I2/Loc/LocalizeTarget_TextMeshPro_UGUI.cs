using System;
using TMPro;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000828 RID: 2088
	public class LocalizeTarget_TextMeshPro_UGUI : LocalizeTarget<TextMeshProUGUI>
	{
		// Token: 0x06003B4B RID: 15179 RVA: 0x00027DC3 File Offset: 0x00025FC3
		static LocalizeTarget_TextMeshPro_UGUI()
		{
			LocalizeTarget_TextMeshPro_UGUI.AutoRegister();
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x00027DCA File Offset: 0x00025FCA
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<TextMeshProUGUI, LocalizeTarget_TextMeshPro_UGUI>
			{
				Name = "TextMeshPro UGUI",
				Priority = 100
			});
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x0000539F File Offset: 0x0000359F
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06003B4E RID: 15182 RVA: 0x0000FB79 File Offset: 0x0000DD79
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.TextMeshPFont;
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x00005651 File Offset: 0x00003851
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x00005651 File Offset: 0x00003851
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x0012BBC4 File Offset: 0x00129DC4
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((this.mTarget.font != null) ? this.mTarget.font.name : string.Empty);
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x0012BC1C File Offset: 0x00129E1C
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			TMP_FontAsset tmp_FontAsset = cmp.GetSecondaryTranslatedObj<TMP_FontAsset>(ref mainTranslation, ref secondaryTranslation);
			if (tmp_FontAsset != null)
			{
				LocalizeTarget_TextMeshPro_Label.SetFont(this.mTarget, tmp_FontAsset);
			}
			else
			{
				Material secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Material>(ref mainTranslation, ref secondaryTranslation);
				if (secondaryTranslatedObj != null && this.mTarget.fontMaterial != secondaryTranslatedObj)
				{
					if (!secondaryTranslatedObj.name.StartsWith(this.mTarget.font.name, StringComparison.Ordinal))
					{
						tmp_FontAsset = LocalizeTarget_TextMeshPro_Label.GetTMPFontFromMaterial(cmp, secondaryTranslation.EndsWith(secondaryTranslatedObj.name, StringComparison.Ordinal) ? secondaryTranslation : secondaryTranslatedObj.name);
						if (tmp_FontAsset != null)
						{
							LocalizeTarget_TextMeshPro_Label.SetFont(this.mTarget, tmp_FontAsset);
						}
					}
					LocalizeTarget_TextMeshPro_Label.SetMaterial(this.mTarget, secondaryTranslatedObj);
				}
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
				LocalizeTarget_TextMeshPro_Label.InitAlignment_TMPro(this.mAlignmentWasRTL, this.mTarget.alignment, out this.mAlignment_LTR, out this.mAlignment_RTL);
			}
			else
			{
				TextAlignmentOptions textAlignmentOptions;
				TextAlignmentOptions textAlignmentOptions2;
				LocalizeTarget_TextMeshPro_Label.InitAlignment_TMPro(this.mAlignmentWasRTL, this.mTarget.alignment, out textAlignmentOptions, out textAlignmentOptions2);
				if ((this.mAlignmentWasRTL && this.mAlignment_RTL != textAlignmentOptions2) || (!this.mAlignmentWasRTL && this.mAlignment_LTR != textAlignmentOptions))
				{
					this.mAlignment_LTR = textAlignmentOptions;
					this.mAlignment_RTL = textAlignmentOptions2;
				}
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
			}
			if (mainTranslation != null && this.mTarget.text != mainTranslation)
			{
				if (mainTranslation != null && cmp.CorrectAlignmentForRTL)
				{
					this.mTarget.alignment = (LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR);
					this.mTarget.isRightToLeftText = LocalizationManager.IsRight2Left;
					if (LocalizationManager.IsRight2Left)
					{
						mainTranslation = I2Utils.ReverseText(mainTranslation);
					}
				}
				this.mTarget.text = mainTranslation;
			}
		}

		// Token: 0x04003902 RID: 14594
		public TextAlignmentOptions mAlignment_RTL = TextAlignmentOptions.Right;

		// Token: 0x04003903 RID: 14595
		public TextAlignmentOptions mAlignment_LTR = TextAlignmentOptions.Left;

		// Token: 0x04003904 RID: 14596
		public bool mAlignmentWasRTL;

		// Token: 0x04003905 RID: 14597
		public bool mInitializeAlignment = true;
	}
}
