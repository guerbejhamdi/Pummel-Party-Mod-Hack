using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prime31.TransitionKit
{
	// Token: 0x020007CE RID: 1998
	public class RippleTransition : TransitionKitDelegate
	{
		// Token: 0x060038EB RID: 14571 RVA: 0x00026BAB File Offset: 0x00024DAB
		public Shader shaderForTransition()
		{
			return Shader.Find("prime[31]/Transitions/Ripple");
		}

		// Token: 0x060038EC RID: 14572 RVA: 0x000053AE File Offset: 0x000035AE
		public Mesh meshForDisplay()
		{
			return null;
		}

		// Token: 0x060038ED RID: 14573 RVA: 0x000053AE File Offset: 0x000035AE
		public Texture2D textureForDisplay()
		{
			return null;
		}

		// Token: 0x060038EE RID: 14574 RVA: 0x00026BB7 File Offset: 0x00024DB7
		public IEnumerator onScreenObscured(TransitionKit transitionKit)
		{
			transitionKit.transitionKitCamera.clearFlags = CameraClearFlags.Nothing;
			transitionKit.material.SetFloat("_Speed", this.speed);
			transitionKit.material.SetFloat("_Amplitude", this.amplitude);
			if (this.nextScene >= 0)
			{
				SceneManager.LoadSceneAsync(this.nextScene);
				yield return transitionKit.StartCoroutine(transitionKit.waitForLevelToLoad(this.nextScene));
			}
			yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(this.duration, false));
			yield break;
		}

		// Token: 0x0400376F RID: 14191
		public float duration = 0.5f;

		// Token: 0x04003770 RID: 14192
		public int nextScene = -1;

		// Token: 0x04003771 RID: 14193
		public float speed = 50f;

		// Token: 0x04003772 RID: 14194
		public float amplitude = 100f;
	}
}
