using System;
using System.Collections;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A4 RID: 164
public class HalloweenTextDissolve : MonoBehaviour
{
	// Token: 0x06000372 RID: 882 RVA: 0x00039150 File Offset: 0x00037350
	public void Show(string text, float transitionTime)
	{
		this.fallback = (LocalizationManager.CurrentLanguage != "English");
		this.startColor = this.textFallback.color;
		this.textFallback.gameObject.SetActive(this.fallback);
		this.textElement.gameObject.SetActive(!this.fallback);
		this.SetStates(true);
		this.textElement.SetText(text, true);
		this.textFallback.text = text;
		base.StartCoroutine(this.FadeRoutine(true, transitionTime));
	}

	// Token: 0x06000373 RID: 883 RVA: 0x00005DFC File Offset: 0x00003FFC
	public void Hide(float transitionTime)
	{
		base.StartCoroutine(this.FadeRoutine(false, transitionTime));
	}

	// Token: 0x06000374 RID: 884 RVA: 0x00005E0D File Offset: 0x0000400D
	private IEnumerator FadeRoutine(bool fadeIn, float transitionTime)
	{
		float startTime = Time.time;
		while (Time.time - startTime < transitionTime)
		{
			float num = (Time.time - startTime) / transitionTime;
			if (!fadeIn)
			{
				num = 1f - num;
			}
			if (!this.fallback)
			{
				this.image.materialForRendering.SetFloat("_MonochromeStrength", num);
			}
			else
			{
				this.textFallback.color = new Color(this.startColor.r, this.startColor.g, this.startColor.b, num);
			}
			yield return null;
		}
		if (!fadeIn)
		{
			this.SetStates(false);
		}
		yield break;
	}

	// Token: 0x06000375 RID: 885 RVA: 0x00005E2A File Offset: 0x0000402A
	private void SetStates(bool enabled)
	{
		this.textElement.enabled = enabled;
		this.image.enabled = enabled;
		this.mask.enabled = (enabled && !this.fallback);
		this.textFallback.enabled = enabled;
	}

	// Token: 0x04000381 RID: 897
	public Image image;

	// Token: 0x04000382 RID: 898
	public Mask mask;

	// Token: 0x04000383 RID: 899
	public TextMeshProUGUI textElement;

	// Token: 0x04000384 RID: 900
	public Text textFallback;

	// Token: 0x04000385 RID: 901
	private bool fallback;

	// Token: 0x04000386 RID: 902
	private Color startColor;
}
