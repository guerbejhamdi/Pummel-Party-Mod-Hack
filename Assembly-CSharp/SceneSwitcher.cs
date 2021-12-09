using System;
using Prime31.TransitionKit;
using UnityEngine;

// Token: 0x0200010D RID: 269
public class SceneSwitcher : MonoBehaviour
{
	// Token: 0x06000818 RID: 2072 RVA: 0x00009A23 File Offset: 0x00007C23
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x0000398C File Offset: 0x00001B8C
	private void OnGUI()
	{
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x0004C274 File Offset: 0x0004A474
	public void DoTransition(string scene)
	{
		ImageMaskTransition transitionKitDelegate = new ImageMaskTransition
		{
			maskTexture = this.maskTexture,
			backgroundColor = Color.black,
			nextScene = scene,
			sceneSwitcher = this,
			duration = 0.5f
		};
		TransitionKit.instance.transitionWithDelegate(transitionKitDelegate);
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x00009A30 File Offset: 0x00007C30
	public void DoTransition(TransitionKitDelegate transitionDelegate)
	{
		TransitionKit.instance.transitionWithDelegate(transitionDelegate);
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x00009A3D File Offset: 0x00007C3D
	private void OnEnable()
	{
		TransitionKit.onScreenObscured += this.onScreenObscured;
		TransitionKit.onTransitionComplete += this.onTransitionComplete;
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x00009A61 File Offset: 0x00007C61
	private void OnDisable()
	{
		TransitionKit.onScreenObscured -= this.onScreenObscured;
		TransitionKit.onTransitionComplete -= this.onTransitionComplete;
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x0000398C File Offset: 0x00001B8C
	private void onScreenObscured()
	{
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x0000398C File Offset: 0x00001B8C
	private void onTransitionComplete()
	{
	}

	// Token: 0x04000669 RID: 1641
	public Texture2D maskTexture;

	// Token: 0x0400066A RID: 1642
	public SceneSwitchState switch_state;
}
