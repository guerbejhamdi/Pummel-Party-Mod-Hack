using System;
using LlockhamIndustries.Decals;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class DecalCleanup : MonoBehaviour
{
	// Token: 0x06000135 RID: 309 RVA: 0x000045C4 File Offset: 0x000027C4
	public void OnDestroy()
	{
		DynamicDecals.System.CleanCamera(base.gameObject.GetComponent<Camera>());
	}
}
