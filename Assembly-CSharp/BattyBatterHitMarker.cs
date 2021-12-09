using System;
using UnityEngine;

// Token: 0x0200014A RID: 330
public class BattyBatterHitMarker : MonoBehaviour
{
	// Token: 0x06000978 RID: 2424 RVA: 0x0000A4EB File Offset: 0x000086EB
	private void Start()
	{
		LeanTween.scale(base.gameObject, Vector3.zero, 0.25f).setDelay(1f).setEaseOutSine().setOnComplete(delegate()
		{
			UnityEngine.Object.Destroy(this);
		});
	}
}
