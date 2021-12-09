using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000547 RID: 1351
public class UIInputPanel : MonoBehaviour
{
	// Token: 0x060023C0 RID: 9152 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Awake()
	{
	}

	// Token: 0x060023C1 RID: 9153 RVA: 0x000D7F40 File Offset: 0x000D6140
	public void SetInput(InputDetails inputDetails, string prefix = "")
	{
		this.curDetails = inputDetails;
		this.descText.text = prefix + LocalizationManager.GetTranslation(inputDetails.description, true, 0, true, false, null, null, true);
		this.glyph.SetValues(inputDetails);
		base.gameObject.SetActive(true);
		if (this.tween != null)
		{
			this.backgroundImage.color = this.baseColor;
			LeanTween.cancel(this.backgroundRect);
		}
		if (inputDetails.priority == InputDetailsPriority.Large)
		{
			this.SetSizes(48f, 40f, 23);
			this.backgroundImage.color = this.baseColor;
			this.tween = LeanTween.color(this.backgroundRect, this.pulseColor, 0.333f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
		}
		else
		{
			this.SetSizes(40f, 32f, 18);
			this.tween = LeanTween.color(this.backgroundRect, this.baseColor, 0.25f).setEase(LeanTweenType.easeInOutSine);
		}
		this.disabled.enabled = (inputDetails.priority == InputDetailsPriority.Disabled);
	}

	// Token: 0x060023C2 RID: 9154 RVA: 0x000D8054 File Offset: 0x000D6254
	private void SetSizes(float height, float glyphSize, int fontSize)
	{
		this.transformRect.sizeDelta = new Vector2(this.transformRect.sizeDelta.x, height);
		this.backgroundRect.sizeDelta = new Vector2(this.backgroundRect.sizeDelta.x, height);
		((RectTransform)this.glyph.transform).sizeDelta = Vector3.one * glyphSize;
		this.descText.fontSize = fontSize;
	}

	// Token: 0x040026A1 RID: 9889
	public UIGlyph glyph;

	// Token: 0x040026A2 RID: 9890
	public RectTransform transformRect;

	// Token: 0x040026A3 RID: 9891
	public Text descText;

	// Token: 0x040026A4 RID: 9892
	public RectTransform backgroundRect;

	// Token: 0x040026A5 RID: 9893
	public Image backgroundImage;

	// Token: 0x040026A6 RID: 9894
	public Image disabled;

	// Token: 0x040026A7 RID: 9895
	public Color pulseColor = new Color(1f, 0.7340772f, 0.1617647f, 0.6f);

	// Token: 0x040026A8 RID: 9896
	public Color baseColor = new Color(0f, 0f, 0f, 0.31f);

	// Token: 0x040026A9 RID: 9897
	private InputDetails curDetails;

	// Token: 0x040026AA RID: 9898
	private LTDescr tween;
}
