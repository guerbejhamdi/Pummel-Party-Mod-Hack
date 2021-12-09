using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x02000834 RID: 2100
	public class LocalizeTarget_UnityUI_Text : LocalizeTarget<Text>
	{
		// Token: 0x06003BB6 RID: 15286 RVA: 0x000280D0 File Offset: 0x000262D0
		static LocalizeTarget_UnityUI_Text()
		{
			LocalizeTarget_UnityUI_Text.AutoRegister();
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x000280D7 File Offset: 0x000262D7
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<Text, LocalizeTarget_UnityUI_Text>
			{
				Name = "Text",
				Priority = 100
			});
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x0000539F File Offset: 0x0000359F
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x00005651 File Offset: 0x00003851
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x00005651 File Offset: 0x00003851
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x00005651 File Offset: 0x00003851
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x0012C408 File Offset: 0x0012A608
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((this.mTarget.font != null) ? this.mTarget.font.name : string.Empty);
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x0012C460 File Offset: 0x0012A660
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Font secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null && secondaryTranslatedObj != this.mTarget.font)
			{
				this.mTarget.font = secondaryTranslatedObj;
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
				this.InitAlignment(this.mAlignmentWasRTL, this.mTarget.alignment, out this.mAlignment_LTR, out this.mAlignment_RTL);
			}
			else
			{
				TextAnchor textAnchor;
				TextAnchor textAnchor2;
				this.InitAlignment(this.mAlignmentWasRTL, this.mTarget.alignment, out textAnchor, out textAnchor2);
				if ((this.mAlignmentWasRTL && this.mAlignment_RTL != textAnchor2) || (!this.mAlignmentWasRTL && this.mAlignment_LTR != textAnchor))
				{
					this.mAlignment_LTR = textAnchor;
					this.mAlignment_RTL = textAnchor2;
				}
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
			}
			if (mainTranslation != null && this.mTarget.text != mainTranslation)
			{
				if (cmp.CorrectAlignmentForRTL)
				{
					this.mTarget.alignment = (LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR);
				}
				this.mTarget.text = mainTranslation;
				this.mTarget.SetVerticesDirty();
			}
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x0012C58C File Offset: 0x0012A78C
		private void InitAlignment(bool isRTL, TextAnchor alignment, out TextAnchor alignLTR, out TextAnchor alignRTL)
		{
			alignRTL = alignment;
			alignLTR = alignment;
			if (isRTL)
			{
				switch (alignment)
				{
				case TextAnchor.UpperLeft:
					alignLTR = TextAnchor.UpperRight;
					return;
				case TextAnchor.UpperCenter:
				case TextAnchor.MiddleCenter:
				case TextAnchor.LowerCenter:
					break;
				case TextAnchor.UpperRight:
					alignLTR = TextAnchor.UpperLeft;
					return;
				case TextAnchor.MiddleLeft:
					alignLTR = TextAnchor.MiddleRight;
					return;
				case TextAnchor.MiddleRight:
					alignLTR = TextAnchor.MiddleLeft;
					return;
				case TextAnchor.LowerLeft:
					alignLTR = TextAnchor.LowerRight;
					return;
				case TextAnchor.LowerRight:
					alignLTR = TextAnchor.LowerLeft;
					return;
				default:
					return;
				}
			}
			else
			{
				switch (alignment)
				{
				case TextAnchor.UpperLeft:
					alignRTL = TextAnchor.UpperRight;
					return;
				case TextAnchor.UpperCenter:
				case TextAnchor.MiddleCenter:
				case TextAnchor.LowerCenter:
					break;
				case TextAnchor.UpperRight:
					alignRTL = TextAnchor.UpperLeft;
					return;
				case TextAnchor.MiddleLeft:
					alignRTL = TextAnchor.MiddleRight;
					return;
				case TextAnchor.MiddleRight:
					alignRTL = TextAnchor.MiddleLeft;
					return;
				case TextAnchor.LowerLeft:
					alignRTL = TextAnchor.LowerRight;
					break;
				case TextAnchor.LowerRight:
					alignRTL = TextAnchor.LowerLeft;
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x0400390A RID: 14602
		private TextAnchor mAlignment_RTL = TextAnchor.UpperRight;

		// Token: 0x0400390B RID: 14603
		private TextAnchor mAlignment_LTR;

		// Token: 0x0400390C RID: 14604
		private bool mAlignmentWasRTL;

		// Token: 0x0400390D RID: 14605
		private bool mInitializeAlignment = true;
	}
}
