using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000F RID: 15
public class BarnBrawlKilledText : MonoBehaviour
{
	// Token: 0x06000037 RID: 55 RVA: 0x0002B31C File Offset: 0x0002951C
	public void Set(BarnBrawlPlayer victim)
	{
		if (this.routine != null)
		{
			base.StopCoroutine(this.routine);
		}
		Color32 c = victim.GamePlayer.Color.uiColor;
		c.a = 200;
		string text = ColorUtility.ToHtmlStringRGBA(c);
		this.text.text = string.Concat(new string[]
		{
			"killed <color=#",
			text,
			">",
			victim.GamePlayer.Name,
			"</color>"
		});
		this.routine = base.StartCoroutine("FadeTimer");
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00003B4A File Offset: 0x00001D4A
	private IEnumerator FadeTimer()
	{
		this.canvasGroup.alpha = 1f;
		yield return new WaitForSeconds(this.lifeLength - this.fadeTime);
		while (this.canvasGroup.alpha > 0f)
		{
			this.canvasGroup.alpha -= this.fadeTime / 1f * Time.deltaTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000031 RID: 49
	public float lifeLength = 5f;

	// Token: 0x04000032 RID: 50
	public float fadeTime = 0.5f;

	// Token: 0x04000033 RID: 51
	public CanvasGroup canvasGroup;

	// Token: 0x04000034 RID: 52
	public Text text;

	// Token: 0x04000035 RID: 53
	private Coroutine routine;
}
