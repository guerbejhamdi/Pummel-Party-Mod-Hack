using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000829 RID: 2089
	public class LocalizeTarget_UnityStandard_AudioSource : LocalizeTarget<AudioSource>
	{
		// Token: 0x06003B55 RID: 15189 RVA: 0x00027E0E File Offset: 0x0002600E
		static LocalizeTarget_UnityStandard_AudioSource()
		{
			LocalizeTarget_UnityStandard_AudioSource.AutoRegister();
		}

		// Token: 0x06003B56 RID: 15190 RVA: 0x00027E15 File Offset: 0x00026015
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<AudioSource, LocalizeTarget_UnityStandard_AudioSource>
			{
				Name = "AudioSource",
				Priority = 100
			});
		}

		// Token: 0x06003B57 RID: 15191 RVA: 0x000054CD File Offset: 0x000036CD
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.AudioClip;
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x0000539F File Offset: 0x0000359F
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x00027E34 File Offset: 0x00026034
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget.clip ? this.mTarget.clip.name : string.Empty);
			secondaryTerm = null;
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x0012BDD4 File Offset: 0x00129FD4
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			bool flag = (this.mTarget.isPlaying || this.mTarget.loop) && Application.isPlaying;
			UnityEngine.Object clip = this.mTarget.clip;
			AudioClip audioClip = cmp.FindTranslatedObject<AudioClip>(mainTranslation);
			if (clip != audioClip)
			{
				this.mTarget.clip = audioClip;
			}
			if (flag && this.mTarget.clip)
			{
				this.mTarget.Play();
			}
		}
	}
}
