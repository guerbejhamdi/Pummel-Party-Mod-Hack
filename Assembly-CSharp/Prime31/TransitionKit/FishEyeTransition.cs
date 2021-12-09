using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prime31.TransitionKit
{
	// Token: 0x020007C5 RID: 1989
	public class FishEyeTransition : TransitionKitDelegate
	{
		// Token: 0x060038C1 RID: 14529 RVA: 0x00026A2B File Offset: 0x00024C2B
		public Shader shaderForTransition()
		{
			return Shader.Find("prime[31]/Transitions/Fish Eye");
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x000053AE File Offset: 0x000035AE
		public Mesh meshForDisplay()
		{
			return null;
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x000053AE File Offset: 0x000035AE
		public Texture2D textureForDisplay()
		{
			return null;
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x00026A37 File Offset: 0x00024C37
		public IEnumerator onScreenObscured(TransitionKit transitionKit)
		{
			transitionKit.transitionKitCamera.clearFlags = CameraClearFlags.Nothing;
			transitionKit.material.SetFloat("_Size", this.size);
			transitionKit.material.SetFloat("_Zoom", this.zoom);
			transitionKit.material.SetFloat("_ColorSeparation", this.zoom);
			if (this.nextScene >= 0)
			{
				SceneManager.LoadSceneAsync(this.nextScene);
				yield return transitionKit.StartCoroutine(transitionKit.waitForLevelToLoad(this.nextScene));
			}
			yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(this.duration, false));
			yield break;
		}

		// Token: 0x04003742 RID: 14146
		public float duration = 0.5f;

		// Token: 0x04003743 RID: 14147
		public int nextScene = -1;

		// Token: 0x04003744 RID: 14148
		public float size = 0.2f;

		// Token: 0x04003745 RID: 14149
		public float zoom = 100f;

		// Token: 0x04003746 RID: 14150
		public float colorSeparation = 0.2f;
	}
}
