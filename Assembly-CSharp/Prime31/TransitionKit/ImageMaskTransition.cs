using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prime31.TransitionKit
{
	// Token: 0x020007C7 RID: 1991
	public class ImageMaskTransition : TransitionKitDelegate
	{
		// Token: 0x060038CC RID: 14540 RVA: 0x00026A9F File Offset: 0x00024C9F
		public Shader shaderForTransition()
		{
			return Shader.Find("prime[31]/Transitions/Mask");
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x000053AE File Offset: 0x000035AE
		public Mesh meshForDisplay()
		{
			return null;
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x000053AE File Offset: 0x000035AE
		public Texture2D textureForDisplay()
		{
			return null;
		}

		// Token: 0x060038CF RID: 14543 RVA: 0x00026AAB File Offset: 0x00024CAB
		public IEnumerator onScreenObscured(TransitionKit transitionKit)
		{
			transitionKit.transitionKitCamera.clearFlags = CameraClearFlags.Nothing;
			transitionKit.material.color = this.backgroundColor;
			transitionKit.material.SetTexture("_MaskTex", this.maskTexture);
			this.sceneSwitcher.switch_state = SceneSwitchState.FadeIn;
			transitionKit.PlaySound(Resources.Load<AudioClip>("MaskTransitionOut"));
			yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(this.duration, false));
			this.sceneSwitcher.switch_state = SceneSwitchState.ScreenObscured;
			AsyncOperation async = null;
			if (this.nextScene != "")
			{
				async = SceneManager.LoadSceneAsync(this.nextScene, LoadSceneMode.Additive);
			}
			yield return new WaitUntil(() => async.isDone);
			yield return new WaitForSeconds(0.5f);
			this.sceneSwitcher.switch_state = SceneSwitchState.FadeOut;
			transitionKit.PlaySound(Resources.Load<AudioClip>("MaskTransitionIn"));
			transitionKit.makeTextureTransparent();
			yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(this.duration, true));
			this.sceneSwitcher.switch_state = SceneSwitchState.None;
			yield break;
		}

		// Token: 0x0400374B RID: 14155
		public Texture2D maskTexture;

		// Token: 0x0400374C RID: 14156
		public Color backgroundColor = Color.black;

		// Token: 0x0400374D RID: 14157
		public float duration = 0.5f;

		// Token: 0x0400374E RID: 14158
		public string nextScene = "";

		// Token: 0x0400374F RID: 14159
		public SceneSwitcher sceneSwitcher;
	}
}
