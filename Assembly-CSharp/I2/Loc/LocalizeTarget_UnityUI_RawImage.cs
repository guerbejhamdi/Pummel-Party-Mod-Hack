using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x02000833 RID: 2099
	public class LocalizeTarget_UnityUI_RawImage : LocalizeTarget<RawImage>
	{
		// Token: 0x06003BAC RID: 15276 RVA: 0x00028070 File Offset: 0x00026270
		static LocalizeTarget_UnityUI_RawImage()
		{
			LocalizeTarget_UnityUI_RawImage.AutoRegister();
		}

		// Token: 0x06003BAD RID: 15277 RVA: 0x00028077 File Offset: 0x00026277
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<RawImage, LocalizeTarget_UnityUI_RawImage>
			{
				Name = "RawImage",
				Priority = 100
			});
		}

		// Token: 0x06003BAE RID: 15278 RVA: 0x00005550 File Offset: 0x00003750
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Texture;
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x0000539F File Offset: 0x0000359F
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x00028096 File Offset: 0x00026296
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget.mainTexture ? this.mTarget.mainTexture.name : "");
			secondaryTerm = null;
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x0012C3C4 File Offset: 0x0012A5C4
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Texture texture = this.mTarget.texture;
			if (texture == null || texture.name != mainTranslation)
			{
				this.mTarget.texture = cmp.FindTranslatedObject<Texture>(mainTranslation);
			}
		}
	}
}
