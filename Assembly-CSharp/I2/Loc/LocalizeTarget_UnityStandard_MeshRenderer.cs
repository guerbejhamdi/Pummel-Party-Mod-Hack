using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200082C RID: 2092
	public class LocalizeTarget_UnityStandard_MeshRenderer : LocalizeTarget<MeshRenderer>
	{
		// Token: 0x06003B6C RID: 15212 RVA: 0x00027EC6 File Offset: 0x000260C6
		static LocalizeTarget_UnityStandard_MeshRenderer()
		{
			LocalizeTarget_UnityStandard_MeshRenderer.AutoRegister();
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x00027ECD File Offset: 0x000260CD
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<MeshRenderer, LocalizeTarget_UnityStandard_MeshRenderer>
			{
				Name = "MeshRenderer",
				Priority = 800
			});
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x0000FB10 File Offset: 0x0000DD10
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Mesh;
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x0000FAEA File Offset: 0x0000DCEA
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Material;
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x00005651 File Offset: 0x00003851
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x0000539F File Offset: 0x0000359F
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x0012BEB8 File Offset: 0x0012A0B8
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			if (this.mTarget == null)
			{
				string text;
				secondaryTerm = (text = null);
				primaryTerm = text;
			}
			else
			{
				MeshFilter component = this.mTarget.GetComponent<MeshFilter>();
				if (component == null || component.sharedMesh == null)
				{
					primaryTerm = null;
				}
				else
				{
					primaryTerm = component.sharedMesh.name;
				}
			}
			if (this.mTarget == null || this.mTarget.sharedMaterial == null)
			{
				secondaryTerm = null;
				return;
			}
			secondaryTerm = this.mTarget.sharedMaterial.name;
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x0012BF50 File Offset: 0x0012A150
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Material secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Material>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null && this.mTarget.sharedMaterial != secondaryTranslatedObj)
			{
				this.mTarget.material = secondaryTranslatedObj;
			}
			Mesh mesh = cmp.FindTranslatedObject<Mesh>(mainTranslation);
			MeshFilter component = this.mTarget.GetComponent<MeshFilter>();
			if (mesh != null && component.sharedMesh != mesh)
			{
				component.mesh = mesh;
			}
		}
	}
}
