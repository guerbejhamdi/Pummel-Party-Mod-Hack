using System;
using UnityEngine;

// Token: 0x020000D3 RID: 211
public class GeneralEasingTypes : MonoBehaviour
{
	// Token: 0x06000436 RID: 1078 RVA: 0x00006506 File Offset: 0x00004706
	private void Start()
	{
		this.demoEaseTypes();
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x0003E66C File Offset: 0x0003C86C
	private void demoEaseTypes()
	{
		for (int i = 0; i < this.easeTypes.Length; i++)
		{
			string text = this.easeTypes[i];
			Transform obj1 = GameObject.Find(text).transform.Find("Line");
			float obj1val = 0f;
			LTDescr ltdescr = LeanTween.value(obj1.gameObject, 0f, 1f, 5f).setOnUpdate(delegate(float val)
			{
				Vector3 localPosition = obj1.localPosition;
				localPosition.x = obj1val * this.lineDrawScale;
				localPosition.y = val * this.lineDrawScale;
				obj1.localPosition = localPosition;
				obj1val += Time.deltaTime / 5f;
				if (obj1val > 1f)
				{
					obj1val = 0f;
				}
			});
			if (text.IndexOf("AnimationCurve") >= 0)
			{
				ltdescr.setEase(this.animationCurve);
			}
			else
			{
				ltdescr.GetType().GetMethod("set" + text).Invoke(ltdescr, null);
			}
			if (text.IndexOf("EasePunch") >= 0)
			{
				ltdescr.setScale(1f);
			}
			else if (text.IndexOf("EaseOutBounce") >= 0)
			{
				ltdescr.setOvershoot(2f);
			}
		}
		LeanTween.delayedCall(base.gameObject, 10f, new Action(this.resetLines));
		LeanTween.delayedCall(base.gameObject, 10.1f, new Action(this.demoEaseTypes));
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x0003E7AC File Offset: 0x0003C9AC
	private void resetLines()
	{
		for (int i = 0; i < this.easeTypes.Length; i++)
		{
			GameObject.Find(this.easeTypes[i]).transform.Find("Line").localPosition = new Vector3(0f, 0f, 0f);
		}
	}

	// Token: 0x04000490 RID: 1168
	public float lineDrawScale = 10f;

	// Token: 0x04000491 RID: 1169
	public AnimationCurve animationCurve;

	// Token: 0x04000492 RID: 1170
	private string[] easeTypes = new string[]
	{
		"EaseLinear",
		"EaseAnimationCurve",
		"EaseSpring",
		"EaseInQuad",
		"EaseOutQuad",
		"EaseInOutQuad",
		"EaseInCubic",
		"EaseOutCubic",
		"EaseInOutCubic",
		"EaseInQuart",
		"EaseOutQuart",
		"EaseInOutQuart",
		"EaseInQuint",
		"EaseOutQuint",
		"EaseInOutQuint",
		"EaseInSine",
		"EaseOutSine",
		"EaseInOutSine",
		"EaseInExpo",
		"EaseOutExpo",
		"EaseInOutExpo",
		"EaseInCirc",
		"EaseOutCirc",
		"EaseInOutCirc",
		"EaseInBounce",
		"EaseOutBounce",
		"EaseInOutBounce",
		"EaseInBack",
		"EaseOutBack",
		"EaseInOutBack",
		"EaseInElastic",
		"EaseOutElastic",
		"EaseInOutElastic",
		"EasePunch",
		"EaseShake"
	};
}
