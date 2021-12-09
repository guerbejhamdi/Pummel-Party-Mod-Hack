using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200082F RID: 2095
	public class LocalizeTarget_UnityStandard_SpriteRenderer : LocalizeTarget<SpriteRenderer>
	{
		// Token: 0x06003B84 RID: 15236 RVA: 0x00027F28 File Offset: 0x00026128
		static LocalizeTarget_UnityStandard_SpriteRenderer()
		{
			LocalizeTarget_UnityStandard_SpriteRenderer.AutoRegister();
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x00027F2F File Offset: 0x0002612F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<SpriteRenderer, LocalizeTarget_UnityStandard_SpriteRenderer>
			{
				Name = "SpriteRenderer",
				Priority = 100
			});
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x0000FB68 File Offset: 0x0000DD68
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Sprite;
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x0000539F File Offset: 0x0000359F
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x00027F4E File Offset: 0x0002614E
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = ((this.mTarget.sprite != null) ? this.mTarget.sprite.name : string.Empty);
			secondaryTerm = null;
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x0012C0F8 File Offset: 0x0012A2F8
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
