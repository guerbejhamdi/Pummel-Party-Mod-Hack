using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x02000832 RID: 2098
	public class LocalizeTarget_UnityUI_Image : LocalizeTarget<Image>
	{
		// Token: 0x06003BA2 RID: 15266 RVA: 0x0002802A File Offset: 0x0002622A
		static LocalizeTarget_UnityUI_Image()
		{
			LocalizeTarget_UnityUI_Image.AutoRegister();
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x00028031 File Offset: 0x00026231
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<Image, LocalizeTarget_UnityUI_Image>
			{
				Name = "Image",
				Priority = 100
			});
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x00028050 File Offset: 0x00026250
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			if (!(this.mTarget.sprite == null))
			{
				return eTermType.Sprite;
			}
			return eTermType.Texture;
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x0000539F File Offset: 0x0000359F
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x0012C2F4 File Offset: 0x0012A4F4
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget.mainTexture ? this.mTarget.mainTexture.name : "");
			if (this.mTarget.sprite != null && this.mTarget.sprite.name != primaryTerm)
			{
				primaryTerm = primaryTerm + "." + this.mTarget.sprite.name;
			}
			secondaryTerm = null;
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x0012C380 File Offset: 0x0012A580
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Sprite sprite = this.mTarget.sprite;
			if (sprite == null || sprite.name != mainTranslation)
			{
				this.mTarget.sprite = cmp.FindTranslatedObject<Sprite>(mainTranslation);
			}
		}
	}
}
