using System;
using UnityEngine;

// Token: 0x0200014B RID: 331
public class BattyBatterBillBoard : MonoBehaviour
{
	// Token: 0x0600097B RID: 2427 RVA: 0x000553D0 File Offset: 0x000535D0
	private void Update()
	{
		if (this.camTransform == null && GameManager.Minigame.MinigameCamera != null)
		{
			this.camTransform = GameManager.Minigame.MinigameCamera.transform;
		}
		base.transform.rotation = Quaternion.LookRotation((this.camTransform.position - base.transform.position).normalized);
	}

	// Token: 0x0400081C RID: 2076
	private Transform camTransform;
}
