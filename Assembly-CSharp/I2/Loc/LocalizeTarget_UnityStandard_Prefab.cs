using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200082E RID: 2094
	public class LocalizeTarget_UnityStandard_Prefab : LocalizeTarget<GameObject>
	{
		// Token: 0x06003B78 RID: 15224 RVA: 0x00027EFF File Offset: 0x000260FF
		static LocalizeTarget_UnityStandard_Prefab()
		{
			LocalizeTarget_UnityStandard_Prefab.AutoRegister();
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x00027F06 File Offset: 0x00026106
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Prefab
			{
				Name = "Prefab",
				Priority = 250
			});
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x00005651 File Offset: 0x00003851
		public override bool IsValid(Localize cmp)
		{
			return true;
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x00005743 File Offset: 0x00003943
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.GameObject;
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x0000539F File Offset: 0x0000359F
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x00027EAF File Offset: 0x000260AF
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = cmp.name;
			secondaryTerm = null;
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x0012BFC4 File Offset: 0x0012A1C4
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			if (string.IsNullOrEmpty(mainTranslation))
			{
				return;
			}
			if (this.mTarget && this.mTarget.name == mainTranslation)
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
			Transform transform2 = this.InstantiateNewPrefab(cmp, mainTranslation);
			if (transform2 == null)
			{
				return;
			}
			transform2.name = text;
			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				Transform child = transform.GetChild(i);
				if (child != transform2)
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
			}
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x0012C070 File Offset: 0x0012A270
		private Transform InstantiateNewPrefab(Localize cmp, string mainTranslation)
		{
			GameObject gameObject = cmp.FindTranslatedObject<GameObject>(mainTranslation);
			if (gameObject == null)
			{
				return null;
			}
			GameObject mTarget = this.mTarget;
			this.mTarget = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			if (this.mTarget == null)
			{
				return null;
			}
			Transform transform = cmp.transform;
			Transform transform2 = this.mTarget.transform;
			transform2.SetParent(transform);
			Transform transform3 = mTarget ? mTarget.transform : transform;
			transform2.rotation = transform3.rotation;
			transform2.position = transform3.position;
			return transform2;
		}
	}
}
