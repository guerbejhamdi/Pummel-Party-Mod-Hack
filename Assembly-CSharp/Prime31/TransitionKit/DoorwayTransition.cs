using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prime31.TransitionKit
{
	// Token: 0x020007C1 RID: 1985
	public class DoorwayTransition : TransitionKitDelegate
	{
		// Token: 0x060038AB RID: 14507 RVA: 0x00026960 File Offset: 0x00024B60
		public Shader shaderForTransition()
		{
			return Shader.Find("prime[31]/Transitions/Doorway");
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x000053AE File Offset: 0x000035AE
		public Mesh meshForDisplay()
		{
			return null;
		}

		// Token: 0x060038AD RID: 14509 RVA: 0x000053AE File Offset: 0x000035AE
		public Texture2D textureForDisplay()
		{
			return null;
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x0002696C File Offset: 0x00024B6C
		public IEnumerator onScreenObscured(TransitionKit transitionKit)
		{
			transitionKit.transitionKitCamera.clearFlags = CameraClearFlags.Nothing;
			transitionKit.material.SetFloat("_Perspective", this.perspective);
			transitionKit.material.SetFloat("_Depth", this.depth);
			transitionKit.material.SetInt("_Direction", this.runEffectInReverse ? 1 : 0);
			if (this.nextScene >= 0)
			{
				SceneManager.LoadSceneAsync(this.nextScene);
				yield return transitionKit.StartCoroutine(transitionKit.waitForLevelToLoad(this.nextScene));
			}
			yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(this.duration, false));
			yield break;
		}

		// Token: 0x0400372F RID: 14127
		public float duration = 0.5f;

		// Token: 0x04003730 RID: 14128
		public int nextScene = -1;

		// Token: 0x04003731 RID: 14129
		public float perspective = 1.5f;

		// Token: 0x04003732 RID: 14130
		public float depth = 3f;

		// Token: 0x04003733 RID: 14131
		public bool runEffectInReverse;
	}
}
