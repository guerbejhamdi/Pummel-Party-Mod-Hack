using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prime31.TransitionKit
{
	// Token: 0x020007C3 RID: 1987
	public class FadeTransition : TransitionKitDelegate
	{
		// Token: 0x060038B6 RID: 14518 RVA: 0x000269C9 File Offset: 0x00024BC9
		public Shader shaderForTransition()
		{
			return Shader.Find("prime[31]/Transitions/Fader");
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x000053AE File Offset: 0x000035AE
		public Mesh meshForDisplay()
		{
			return null;
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x000053AE File Offset: 0x000035AE
		public Texture2D textureForDisplay()
		{
			return null;
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x000269D5 File Offset: 0x00024BD5
		public IEnumerator onScreenObscured(TransitionKit transitionKit)
		{
			transitionKit.transitionKitCamera.clearFlags = CameraClearFlags.Nothing;
			transitionKit.material.color = this.fadeToColor;
			this.sceneSwitcher.switch_state = SceneSwitchState.FadeIn;
			yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(this.duration, false));
			transitionKit.makeTextureTransparent();
			this.sceneSwitcher.switch_state = SceneSwitchState.ScreenObscured;
			AsyncOperation async = null;
			if (this.nextScene != "")
			{
				async = SceneManager.LoadSceneAsync(this.nextScene, LoadSceneMode.Additive);
			}
			if (this.fadedDelay > 0f)
			{
				yield return new WaitForSeconds(this.fadedDelay);
			}
			if (this.nextScene != "")
			{
				yield return transitionKit.StartCoroutine(transitionKit.waitForAsyncLoad(async));
			}
			this.sceneSwitcher.switch_state = SceneSwitchState.FadeOut;
			yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(this.duration, true));
			this.sceneSwitcher.switch_state = SceneSwitchState.None;
			yield break;
		}

		// Token: 0x04003738 RID: 14136
		public Color fadeToColor = Color.black;

		// Token: 0x04003739 RID: 14137
		public float duration = 0.5f;

		// Token: 0x0400373A RID: 14138
		public float fadedDelay;

		// Token: 0x0400373B RID: 14139
		public string nextScene = "";

		// Token: 0x0400373C RID: 14140
		public SceneSwitcher sceneSwitcher;
	}
}
