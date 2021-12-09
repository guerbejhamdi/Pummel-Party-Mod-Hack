using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200055B RID: 1371
public class UIMinigameTimer : MonoBehaviour
{
	// Token: 0x06002405 RID: 9221 RVA: 0x000D9354 File Offset: 0x000D7554
	private void OnEnable()
	{
		LeanTween.cancel(base.gameObject);
		LeanTween.cancel(this.timer_text.gameObject);
		base.transform.localScale = Vector3.zero;
		LeanTween.scale(base.gameObject, this.targetScale, 0.3333f).setEase(this.spawnCurve);
	}

	// Token: 0x06002406 RID: 9222 RVA: 0x00019F29 File Offset: 0x00018129
	public void Update()
	{
		this.time_test -= Time.deltaTime;
		this.SetTime(this.time_test);
	}

	// Token: 0x06002407 RID: 9223 RVA: 0x000D93B0 File Offset: 0x000D75B0
	public void SetTime(float time)
	{
		int num = (int)Math.Truncate((double)time);
		double num2 = (double)time - (double)num;
		if (num != this.last_time_integral)
		{
			LeanTween.scale(this.timer_text.gameObject, this.targetScale * 1.25f, 0.125f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong(1);
			int num3 = (num >= 0) ? num : 0;
			this.timer_text.text = num3.ToString();
		}
		if (num2 != (double)this.last_fraction)
		{
			this.timer_circle.fillAmount = (float)num2;
		}
		this.last_time_integral = num;
		this.last_fraction = (float)num2;
	}

	// Token: 0x04002720 RID: 10016
	public float time_test = 60f;

	// Token: 0x04002721 RID: 10017
	public bool timerSoundTick;

	// Token: 0x04002722 RID: 10018
	public Text timer_text;

	// Token: 0x04002723 RID: 10019
	public Image timer_circle;

	// Token: 0x04002724 RID: 10020
	public AnimationCurve spawnCurve;

	// Token: 0x04002725 RID: 10021
	private int last_time_integral;

	// Token: 0x04002726 RID: 10022
	private float last_fraction;

	// Token: 0x04002727 RID: 10023
	private Vector3 targetScale = new Vector3(1f, 1f, 1f);
}
