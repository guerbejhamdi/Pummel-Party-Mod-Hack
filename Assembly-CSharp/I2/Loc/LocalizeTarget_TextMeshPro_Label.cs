using System;
using TMPro;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000827 RID: 2087
	public class LocalizeTarget_TextMeshPro_Label : LocalizeTarget<TextMeshPro>
	{
		// Token: 0x06003B3D RID: 15165 RVA: 0x00027D16 File Offset: 0x00025F16
		static LocalizeTarget_TextMeshPro_Label()
		{
			LocalizeTarget_TextMeshPro_Label.AutoRegister();
		}

		// Token: 0x06003B3E RID: 15166 RVA: 0x00027D1D File Offset: 0x00025F1D
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<TextMeshPro, LocalizeTarget_TextMeshPro_Label>
			{
				Name = "TextMeshPro Label",
				Priority = 100
			});
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x0000539F File Offset: 0x0000359F
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06003B40 RID: 15168 RVA: 0x00005651 File Offset: 0x00003851
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x06003B41 RID: 15169 RVA: 0x00005651 File Offset: 0x00003851
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x00005651 File Offset: 0x00003851
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x0012B758 File Offset: 0x00129958
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((this.mTarget.font != null) ? this.mTarget.font.name : string.Empty);
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x0012B7B0 File Offset: 0x001299B0
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

		// Token: 0x06003B46 RID: 15174 RVA: 0x0012B968 File Offset: 0x00129B68
		internal static TMP_FontAsset GetTMPFontFromMaterial(Localize cmp, string matName)
		{
			string text = " .\\/-[]()";
			int i = matName.Length - 1;
			while (i > 0)
			{
				while (i > 0 && text.IndexOf(matName[i]) >= 0)
				{
					i--;
				}
				if (i <= 0)
				{
					break;
				}
				string translation = matName.Substring(0, i + 1);
				TMP_FontAsset @object = cmp.GetObject<TMP_FontAsset>(translation);
				if (@object != null)
				{
					return @object;
				}
				while (i > 0 && text.IndexOf(matName[i]) < 0)
				{
					i--;
				}
			}
			return null;
		}

		// Token: 0x06003B47 RID: 15175 RVA: 0x0012B9E0 File Offset: 0x00129BE0
		internal static void InitAlignment_TMPro(bool isRTL, TextAlignmentOptions alignment, out TextAlignmentOptions alignLTR, out TextAlignmentOptions alignRTL)
		{
			alignRTL = alignment;
			alignLTR = alignment;
			if (isRTL)
			{
				if (alignment <= TextAlignmentOptions.BottomRight)
				{
					if (alignment <= TextAlignmentOptions.Left)
					{
						if (alignment == TextAlignmentOptions.TopLeft)
						{
							alignLTR = TextAlignmentOptions.TopRight;
							return;
						}
						if (alignment == TextAlignmentOptions.TopRight)
						{
							alignLTR = TextAlignmentOptions.TopLeft;
							return;
						}
						if (alignment != TextAlignmentOptions.Left)
						{
							return;
						}
						alignLTR = TextAlignmentOptions.Right;
						return;
					}
					else
					{
						if (alignment == TextAlignmentOptions.Right)
						{
							alignLTR = TextAlignmentOptions.Left;
							return;
						}
						if (alignment == TextAlignmentOptions.BottomLeft)
						{
							alignLTR = TextAlignmentOptions.BottomRight;
							return;
						}
						if (alignment != TextAlignmentOptions.BottomRight)
						{
							return;
						}
						alignLTR = TextAlignmentOptions.BottomLeft;
						return;
					}
				}
				else if (alignment <= TextAlignmentOptions.MidlineLeft)
				{
					if (alignment == TextAlignmentOptions.BaselineLeft)
					{
						alignLTR = TextAlignmentOptions.BaselineRight;
						return;
					}
					if (alignment == TextAlignmentOptions.BaselineRight)
					{
						alignLTR = TextAlignmentOptions.BaselineLeft;
						return;
					}
					if (alignment != TextAlignmentOptions.MidlineLeft)
					{
						return;
					}
					alignLTR = TextAlignmentOptions.MidlineRight;
					return;
				}
				else
				{
					if (alignment == TextAlignmentOptions.MidlineRight)
					{
						alignLTR = TextAlignmentOptions.MidlineLeft;
						return;
					}
					if (alignment == TextAlignmentOptions.CaplineLeft)
					{
						alignLTR = TextAlignmentOptions.CaplineRight;
						return;
					}
					if (alignment != TextAlignmentOptions.CaplineRight)
					{
						return;
					}
					alignLTR = TextAlignmentOptions.CaplineLeft;
					return;
				}
			}
			else if (alignment <= TextAlignmentOptions.BottomRight)
			{
				if (alignment <= TextAlignmentOptions.Left)
				{
					if (alignment == TextAlignmentOptions.TopLeft)
					{
						alignRTL = TextAlignmentOptions.TopRight;
						return;
					}
					if (alignment == TextAlignmentOptions.TopRight)
					{
						alignRTL = TextAlignmentOptions.TopLeft;
						return;
					}
					if (alignment != TextAlignmentOptions.Left)
					{
						return;
					}
					alignRTL = TextAlignmentOptions.Right;
					return;
				}
				else
				{
					if (alignment == TextAlignmentOptions.Right)
					{
						alignRTL = TextAlignmentOptions.Left;
						return;
					}
					if (alignment == TextAlignmentOptions.BottomLeft)
					{
						alignRTL = TextAlignmentOptions.BottomRight;
						return;
					}
					if (alignment != TextAlignmentOptions.BottomRight)
					{
						return;
					}
					alignRTL = TextAlignmentOptions.BottomLeft;
					return;
				}
			}
			else if (alignment <= TextAlignmentOptions.MidlineLeft)
			{
				if (alignment == TextAlignmentOptions.BaselineLeft)
				{
					alignRTL = TextAlignmentOptions.BaselineRight;
					return;
				}
				if (alignment == TextAlignmentOptions.BaselineRight)
				{
					alignRTL = TextAlignmentOptions.BaselineLeft;
					return;
				}
				if (alignment != TextAlignmentOptions.MidlineLeft)
				{
					return;
				}
				alignRTL = TextAlignmentOptions.MidlineRight;
				return;
			}
			else
			{
				if (alignment == TextAlignmentOptions.MidlineRight)
				{
					alignRTL = TextAlignmentOptions.MidlineLeft;
					return;
				}
				if (alignment == TextAlignmentOptions.CaplineLeft)
				{
					alignRTL = TextAlignmentOptions.CaplineRight;
					return;
				}
				if (alignment != TextAlignmentOptions.CaplineRight)
				{
					return;
				}
				alignRTL = TextAlignmentOptions.CaplineLeft;
				return;
			}
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x00027D3C File Offset: 0x00025F3C
		internal static void SetFont(TMP_Text label, TMP_FontAsset newFont)
		{
			if (label.font != newFont)
			{
				label.font = newFont;
			}
			if (label.linkedTextComponent != null)
			{
				LocalizeTarget_TextMeshPro_Label.SetFont(label.linkedTextComponent, newFont);
			}
		}

		// Token: 0x06003B49 RID: 15177 RVA: 0x00027D6D File Offset: 0x00025F6D
		internal static void SetMaterial(TMP_Text label, Material newMat)
		{
			if (label.fontSharedMaterial != newMat)
			{
				label.fontSharedMaterial = newMat;
			}
			if (label.linkedTextComponent != null)
			{
				LocalizeTarget_TextMeshPro_Label.SetMaterial(label.linkedTextComponent, newMat);
			}
		}

		// Token: 0x040038FE RID: 14590
		private TextAlignmentOptions mAlignment_RTL = TextAlignmentOptions.Right;

		// Token: 0x040038FF RID: 14591
		private TextAlignmentOptions mAlignment_LTR = TextAlignmentOptions.Left;

		// Token: 0x04003900 RID: 14592
		private bool mAlignmentWasRTL;

		// Token: 0x04003901 RID: 14593
		private bool mInitializeAlignment = true;
	}
}
