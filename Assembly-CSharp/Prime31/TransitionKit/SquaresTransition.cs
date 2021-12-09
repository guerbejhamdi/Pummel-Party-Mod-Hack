using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prime31.TransitionKit
{
	// Token: 0x020007D0 RID: 2000
	public class SquaresTransition : TransitionKitDelegate
	{
		// Token: 0x060038F6 RID: 14582 RVA: 0x00026C14 File Offset: 0x00024E14
		public Shader shaderForTransition()
		{
			return Shader.Find("prime[31]/Transitions/Squares");
		}

		// Token: 0x060038F7 RID: 14583 RVA: 0x000053AE File Offset: 0x000035AE
		public Mesh meshForDisplay()
		{
			return null;
		}

		// Token: 0x060038F8 RID: 14584 RVA: 0x000053AE File Offset: 0x000035AE
		public Texture2D textureForDisplay()
		{
			return null;
		}

		// Token: 0x060038F9 RID: 14585 RVA: 0x00026C20 File Offset: 0x00024E20
		public IEnumerator onScreenObscured(TransitionKit transitionKit)
		{
			transitionKit.transitionKitCamera.clearFlags = CameraClearFlags.Nothing;
			transitionKit.material.color = this.squareColor;
			transitionKit.material.SetFloat("_Smoothness", this.smoothness);
			transitionKit.material.SetVector("_Size", this.squareSize);
			if (this.nextScene >= 0)
			{
				SceneManager.LoadSceneAsync(this.nextScene);
			}
			yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(this.duration, false));
			transitionKit.makeTextureTransparent();
			if (this.fadedDelay > 0f)
			{
				yield return new WaitForSeconds(this.fadedDelay);
			}
			if (this.nextScene >= 0)
			{
				yield return transitionKit.StartCoroutine(transitionKit.waitForLevelToLoad(this.nextScene));
			}
			yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(this.duration, true));
			yield break;
		}

		// Token: 0x04003777 RID: 14199
		public Color squareColor = Color.black;

		// Token: 0x04003778 RID: 14200
		public float duration = 1f;

		// Token: 0x04003779 RID: 14201
		public float fadedDelay;

		// Token: 0x0400377A RID: 14202
		public int nextScene = -1;

		// Token: 0x0400377B RID: 14203
		public Vector2 squareSize = new Vector2(13f, 9f);

		// Token: 0x0400377C RID: 14204
		public float smoothness = 0.5f;
	}
}
