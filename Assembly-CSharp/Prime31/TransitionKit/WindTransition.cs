using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prime31.TransitionKit
{
	// Token: 0x020007D8 RID: 2008
	public class WindTransition : TransitionKitDelegate
	{
		// Token: 0x0600391B RID: 14619 RVA: 0x00026CE9 File Offset: 0x00024EE9
		public Shader shaderForTransition()
		{
			if (!this.useCurvedWind)
			{
				return Shader.Find("prime[31]/Transitions/Wind");
			}
			return Shader.Find("prime[31]/Transitions/CurvedWind");
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x000053AE File Offset: 0x000035AE
		public Mesh meshForDisplay()
		{
			return null;
		}

		// Token: 0x0600391D RID: 14621 RVA: 0x000053AE File Offset: 0x000035AE
		public Texture2D textureForDisplay()
		{
			return null;
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x00026D08 File Offset: 0x00024F08
		public IEnumerator onScreenObscured(TransitionKit transitionKit)
		{
			transitionKit.transitionKitCamera.clearFlags = CameraClearFlags.Nothing;
			transitionKit.material.SetFloat("_Size", this.size);
			transitionKit.material.SetFloat("_WindVerticalSegments", this.windVerticalSegments);
			if (this.nextScene >= 0)
			{
				SceneManager.LoadSceneAsync(this.nextScene);
				yield return transitionKit.StartCoroutine(transitionKit.waitForLevelToLoad(this.nextScene));
			}
			yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(this.duration, false));
			yield break;
		}

		// Token: 0x0400379D RID: 14237
		public bool useCurvedWind;

		// Token: 0x0400379E RID: 14238
		public float duration = 0.5f;

		// Token: 0x0400379F RID: 14239
		public int nextScene = -1;

		// Token: 0x040037A0 RID: 14240
		public float size = 0.3f;

		// Token: 0x040037A1 RID: 14241
		public float windVerticalSegments = 100f;
	}
}
