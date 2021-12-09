using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200082B RID: 2091
	public class LocalizeTarget_UnityStandard_Child : LocalizeTarget<GameObject>
	{
		// Token: 0x06003B61 RID: 15201 RVA: 0x00027E86 File Offset: 0x00026086
		static LocalizeTarget_UnityStandard_Child()
		{
			LocalizeTarget_UnityStandard_Child.AutoRegister();
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x00027E8D File Offset: 0x0002608D
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Child
			{
				Name = "Child",
				Priority = 200
			});
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x00027E6E File Offset: 0x0002606E
		public override bool IsValid(Localize cmp)
		{
			return cmp.transform.childCount > 1;
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x00005743 File Offset: 0x00003943
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.GameObject;
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x0000539F File Offset: 0x0000359F
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x00027EAF File Offset: 0x000260AF
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = cmp.name;
			secondaryTerm = null;
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x0012BE4C File Offset: 0x0012A04C
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			if (string.IsNullOrEmpty(mainTranslation))
			{
				return;
			}
			Transform transform = cmp.transform;
			string text = mainTranslation;
			int num = mainTranslation.LastIndexOfAny(LanguageSourceData.CategorySeparators);
			if (num >= 0)
			{
				text = text.Substring(num + 1);
			}
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				child.gameObject.SetActive(child.name == text);
			}
		}
	}
}
