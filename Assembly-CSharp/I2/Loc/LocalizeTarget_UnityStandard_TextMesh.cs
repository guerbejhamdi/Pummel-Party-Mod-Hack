using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000830 RID: 2096
	public class LocalizeTarget_UnityStandard_TextMesh : LocalizeTarget<TextMesh>
	{
		// Token: 0x06003B8E RID: 15246 RVA: 0x00027F89 File Offset: 0x00026189
		static LocalizeTarget_UnityStandard_TextMesh()
		{
			LocalizeTarget_UnityStandard_TextMesh.AutoRegister();
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x00027F90 File Offset: 0x00026190
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<TextMesh, LocalizeTarget_UnityStandard_TextMesh>
			{
				Name = "TextMesh",
				Priority = 100
			});
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x0000539F File Offset: 0x0000359F
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x00005651 File Offset: 0x00003851
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x00005651 File Offset: 0x00003851
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x00005651 File Offset: 0x00003851
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x0012C13C File Offset: 0x0012A33C
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((string.IsNullOrEmpty(Secondary) && this.mTarget.font != null) ? this.mTarget.font.name : null);
		}

		// Token: 0x06003B96 RID: 15254 RVA: 0x0012C198 File Offset: 0x0012A398
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Font secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null && this.mTarget.font != secondaryTranslatedObj)
			{
				this.mTarget.font = secondaryTranslatedObj;
				this.mTarget.GetComponentInChildren<MeshRenderer>().material = secondaryTranslatedObj.material;
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignment_LTR = (this.mAlignment_RTL = this.mTarget.alignment);
				if (LocalizationManager.IsRight2Left && this.mAlignment_RTL == TextAlignment.Right)
				{
					this.mAlignment_LTR = TextAlignment.Left;
				}
				if (!LocalizationManager.IsRight2Left && this.mAlignment_LTR == TextAlignment.Left)
				{
					this.mAlignment_RTL = TextAlignment.Right;
				}
			}
			if (mainTranslation != null && this.mTarget.text != mainTranslation)
			{
				if (cmp.CorrectAlignmentForRTL && this.mTarget.alignment != TextAlignment.Center)
				{
					this.mTarget.alignment = (LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR);
				}
				this.mTarget.font.RequestCharactersInTexture(mainTranslation);
				this.mTarget.text = mainTranslation;
			}
		}

		// Token: 0x04003906 RID: 14598
		private TextAlignment mAlignment_RTL = TextAlignment.Right;

		// Token: 0x04003907 RID: 14599
		private TextAlignment mAlignment_LTR;

		// Token: 0x04003908 RID: 14600
		private bool mAlignmentWasRTL;

		// Token: 0x04003909 RID: 14601
		private bool mInitializeAlignment = true;
	}
}
