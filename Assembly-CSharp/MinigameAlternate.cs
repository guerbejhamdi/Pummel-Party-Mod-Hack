using System;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x02000425 RID: 1061
[Serializable]
public class MinigameAlternate
{
	// Token: 0x04002010 RID: 8208
	public bool enabled;

	// Token: 0x04002011 RID: 8209
	public string name = "DefaultAlternate";

	// Token: 0x04002012 RID: 8210
	public string sceneName = "DefaultMinigame_Scene";

	// Token: 0x04002013 RID: 8211
	public VideoClip videoClip;

	// Token: 0x04002014 RID: 8212
	public string videoClipPath;

	// Token: 0x04002015 RID: 8213
	public Sprite screenshot;

	// Token: 0x04002016 RID: 8214
	[HideInInspector]
	public bool isFolded;
}
