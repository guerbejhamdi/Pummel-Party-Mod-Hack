using System;
using UnityEngine;
using UnityEngine.Video;

namespace I2.Loc
{
	// Token: 0x02000831 RID: 2097
	public class LocalizeTarget_UnityStandard_VideoPlayer : LocalizeTarget<VideoPlayer>
	{
		// Token: 0x06003B98 RID: 15256 RVA: 0x00027FC5 File Offset: 0x000261C5
		static LocalizeTarget_UnityStandard_VideoPlayer()
		{
			LocalizeTarget_UnityStandard_VideoPlayer.AutoRegister();
		}

		// Token: 0x06003B99 RID: 15257 RVA: 0x00027FCC File Offset: 0x000261CC
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<VideoPlayer, LocalizeTarget_UnityStandard_VideoPlayer>
			{
				Name = "VideoPlayer",
				Priority = 100
			});
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x00027FEB File Offset: 0x000261EB
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Video;
		}

		// Token: 0x06003B9B RID: 15259 RVA: 0x0000539F File Offset: 0x0000359F
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x00027FEF File Offset: 0x000261EF
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = ((this.mTarget.clip != null) ? this.mTarget.clip.name : string.Empty);
			secondaryTerm = null;
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x0012C2B0 File Offset: 0x0012A4B0
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			VideoClip clip = this.mTarget.clip;
			if (clip == null || clip.name != mainTranslation)
			{
				this.mTarget.clip = cmp.FindTranslatedObject<VideoClip>(mainTranslation);
			}
		}
	}
}
