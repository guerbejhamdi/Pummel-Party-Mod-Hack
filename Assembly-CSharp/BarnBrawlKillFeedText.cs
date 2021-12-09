using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class BarnBrawlKillFeedText : MonoBehaviour
{
	// Token: 0x0600002F RID: 47 RVA: 0x00003B06 File Offset: 0x00001D06
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(this.lifeLength - this.fadeTime);
		while (this.canvasGroup.alpha > 0f)
		{
			this.canvasGroup.alpha -= this.fadeTime / 1f * Time.deltaTime;
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400002B RID: 43
	public float lifeLength = 5f;

	// Token: 0x0400002C RID: 44
	public float fadeTime = 1f;

	// Token: 0x0400002D RID: 45
	public CanvasGroup canvasGroup;
}
