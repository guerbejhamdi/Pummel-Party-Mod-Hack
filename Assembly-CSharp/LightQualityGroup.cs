using System;
using UnityEngine;

// Token: 0x02000463 RID: 1123
[Serializable]
public class LightQualityGroup
{
	// Token: 0x0400218E RID: 8590
	[Header("General")]
	public Light[] targetLights;

	// Token: 0x0400218F RID: 8591
	[Header("Light")]
	public DetailPlatform lightEnabledPlatforms = (DetailPlatform)2147483647;

	// Token: 0x04002190 RID: 8592
	public int lightMaxPlayers;

	// Token: 0x04002191 RID: 8593
	[Header("Shadows")]
	public DetailPlatform shadowEnabledPlatforms;

	// Token: 0x04002192 RID: 8594
	public int shadowMaxPlayers;
}
