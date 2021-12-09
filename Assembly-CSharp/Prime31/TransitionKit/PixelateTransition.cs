using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prime31.TransitionKit
{
	// Token: 0x020007CA RID: 1994
	public class PixelateTransition : TransitionKitDelegate
	{
		// Token: 0x060038D9 RID: 14553 RVA: 0x00026B0E File Offset: 0x00024D0E
		public Shader shaderForTransition()
		{
			return Shader.Find("prime[31]/Transitions/Pixelate");
		}

		// Token: 0x060038DA RID: 14554 RVA: 0x000053AE File Offset: 0x000035AE
		public Mesh meshForDisplay()
		{
			return null;
		}

		// Token: 0x060038DB RID: 14555 RVA: 0x000053AE File Offset: 0x000035AE
		public Texture2D textureForDisplay()
		{
			return null;
		}

		// Token: 0x060038DC RID: 14556 RVA: 0x00026B1A File Offset: 0x00024D1A
		public IEnumerator onScreenObscured(TransitionKit transitionKit)
		{
			if (this.nextScene >= 0)
			{
				SceneManager.LoadSceneAsync(this.nextScene);
			}
			transitionKit.transitionKitCamera.clearFlags = CameraClearFlags.Nothing;
			float startValue = this.pixellateMin;
			float endValue = this.pixellateMax;
			transitionKit.material.SetFloat("_WidthAspectMultiplier", 1f / Camera.main.aspect);
			float elapsed = 0f;
			while (elapsed < this.duration)
			{
				elapsed += transitionKit.deltaTime;
				float t = Mathf.Pow(elapsed / this.duration, 2f);
				transitionKit.material.SetFloat("_CellSize", Mathf.Lerp(startValue, endValue, t));
				yield return null;
			}
			if (this.pixelatedDelay > 0f)
			{
				yield return new WaitForSeconds(this.pixelatedDelay);
			}
			if (this.nextScene >= 0)
			{
				yield return transitionKit.StartCoroutine(transitionKit.waitForLevelToLoad(this.nextScene));
			}
			Vector3 zero = Vector3.zero;
			switch (this.finalScaleEffect)
			{
			case PixelateTransition.PixelateFinalScaleEffect.ToPoint:
				zero = new Vector3(0f, 0f, transitionKit.transform.localScale.z);
				break;
			case PixelateTransition.PixelateFinalScaleEffect.Zoom:
				zero = new Vector3(transitionKit.transform.localScale.x * 5f, transitionKit.transform.localScale.y * 5f, transitionKit.transform.localScale.z);
				break;
			case PixelateTransition.PixelateFinalScaleEffect.Horizontal:
				zero = new Vector3(transitionKit.transform.localScale.x, 0f, transitionKit.transform.localScale.z);
				break;
			case PixelateTransition.PixelateFinalScaleEffect.Vertical:
				zero = new Vector3(0f, transitionKit.transform.localScale.y, transitionKit.transform.localScale.z);
				break;
			}
			yield return transitionKit.StartCoroutine(this.animateScale(transitionKit, this.duration * 0.5f, zero));
			yield break;
		}

		// Token: 0x060038DD RID: 14557 RVA: 0x00026B30 File Offset: 0x00024D30
		public IEnumerator animateScale(TransitionKit transitionKit, float duration, Vector3 desiredScale)
		{
			Vector3 originalScale = transitionKit.transform.localScale;
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += transitionKit.deltaTime;
				float t = Mathf.Pow(elapsed / duration, 2f);
				transitionKit.transform.localScale = Vector3.Lerp(originalScale, desiredScale, t);
				yield return null;
			}
			yield break;
		}

		// Token: 0x04003756 RID: 14166
		public float pixellateMin = 0.001f;

		// Token: 0x04003757 RID: 14167
		public float pixellateMax = 0.08f;

		// Token: 0x04003758 RID: 14168
		public float duration = 0.6f;

		// Token: 0x04003759 RID: 14169
		public float pixelatedDelay;

		// Token: 0x0400375A RID: 14170
		public PixelateTransition.PixelateFinalScaleEffect finalScaleEffect;

		// Token: 0x0400375B RID: 14171
		public int nextScene = -1;

		// Token: 0x020007CB RID: 1995
		public enum PixelateFinalScaleEffect
		{
			// Token: 0x0400375D RID: 14173
			ToPoint,
			// Token: 0x0400375E RID: 14174
			Zoom,
			// Token: 0x0400375F RID: 14175
			Horizontal,
			// Token: 0x04003760 RID: 14176
			Vertical
		}
	}
}
