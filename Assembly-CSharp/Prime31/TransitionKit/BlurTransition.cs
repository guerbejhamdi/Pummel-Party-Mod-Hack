using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prime31.TransitionKit
{
	// Token: 0x020007BF RID: 1983
	public class BlurTransition : TransitionKitDelegate
	{
		// Token: 0x060038A0 RID: 14496 RVA: 0x00026902 File Offset: 0x00024B02
		public Shader shaderForTransition()
		{
			return Shader.Find("prime[31]/Transitions/Blur");
		}

		// Token: 0x060038A1 RID: 14497 RVA: 0x000053AE File Offset: 0x000035AE
		public Mesh meshForDisplay()
		{
			return null;
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x000053AE File Offset: 0x000035AE
		public Texture2D textureForDisplay()
		{
			return null;
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x0002690E File Offset: 0x00024B0E
		public IEnumerator onScreenObscured(TransitionKit transitionKit)
		{
			transitionKit.transitionKitCamera.clearFlags = CameraClearFlags.Nothing;
			if (this.nextScene >= 0)
			{
				SceneManager.LoadSceneAsync(this.nextScene);
			}
			float elapsed = 0f;
			while (elapsed < this.duration)
			{
				elapsed += transitionKit.deltaTime;
				float t = Mathf.Pow(elapsed / this.duration, 2f);
				float value = Mathf.Lerp(this.blurMin, this.blurMax, t);
				transitionKit.material.SetFloat("_BlurSize", value);
				yield return null;
			}
			if (this.nextScene >= 0)
			{
				yield return transitionKit.StartCoroutine(transitionKit.waitForLevelToLoad(this.nextScene));
			}
			yield break;
		}

		// Token: 0x04003726 RID: 14118
		public float duration = 0.5f;

		// Token: 0x04003727 RID: 14119
		public int nextScene = -1;

		// Token: 0x04003728 RID: 14120
		public float blurMin;

		// Token: 0x04003729 RID: 14121
		public float blurMax = 0.01f;
	}
}
