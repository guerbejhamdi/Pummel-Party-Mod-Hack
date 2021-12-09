using System;
using System.Collections;
using UnityEngine;

namespace Prime31.TransitionKit
{
	// Token: 0x020007BE RID: 1982
	public interface TransitionKitDelegate
	{
		// Token: 0x0600389C RID: 14492
		Shader shaderForTransition();

		// Token: 0x0600389D RID: 14493
		Mesh meshForDisplay();

		// Token: 0x0600389E RID: 14494
		Texture2D textureForDisplay();

		// Token: 0x0600389F RID: 14495
		IEnumerator onScreenObscured(TransitionKit transitionKit);
	}
}
