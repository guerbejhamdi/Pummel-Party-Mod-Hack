using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000DA RID: 218
public class GeneralUISpace : MonoBehaviour
{
	// Token: 0x06000451 RID: 1105 RVA: 0x0003EF10 File Offset: 0x0003D110
	private void Start()
	{
		this.mainWindow.localScale = Vector3.zero;
		LeanTween.scale(this.mainWindow, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
		LeanTween.alphaCanvas(this.mainWindow.GetComponent<CanvasGroup>(), 0f, 1f).setDelay(2f).setLoopPingPong().setRepeat(2);
		this.mainParagraphText.anchoredPosition3D += new Vector3(0f, -10f, 0f);
		LeanTween.textAlpha(this.mainParagraphText, 0f, 0.6f).setFrom(0f).setDelay(0f);
		LeanTween.textAlpha(this.mainParagraphText, 1f, 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f);
		LeanTween.move(this.mainParagraphText, this.mainParagraphText.anchoredPosition3D + new Vector3(0f, 10f, 0f), 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f);
		LeanTween.textColor(this.mainTitleText, new Color(0.52156866f, 0.5686275f, 0.8745098f), 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f).setLoopPingPong().setRepeat(-1);
		LeanTween.textAlpha(this.mainButton2, 1f, 2f).setFrom(0f).setDelay(0f).setEase(LeanTweenType.easeOutQuad);
		LeanTween.alpha(this.mainButton2, 1f, 2f).setFrom(0f).setDelay(0f).setEase(LeanTweenType.easeOutQuad);
		LeanTween.size(this.mainButton1, this.mainButton1.sizeDelta * 1.1f, 0.5f).setDelay(3f).setEaseInOutCirc().setRepeat(6).setLoopPingPong();
		this.pauseWindow.anchoredPosition3D += new Vector3(0f, 200f, 0f);
		LeanTween.moveY(this.pauseWindow, this.pauseWindow.anchoredPosition3D.y + -200f, 0.6f).setEase(LeanTweenType.easeOutSine).setDelay(0.6f);
		RectTransform component = this.pauseWindow.Find("PauseText").GetComponent<RectTransform>();
		LeanTween.moveZ(component, component.anchoredPosition3D.z - 80f, 1.5f).setEase(LeanTweenType.punch).setDelay(2f);
		LeanTween.rotateAroundLocal(this.pauseRing1, Vector3.forward, 360f, 12f).setRepeat(-1);
		LeanTween.rotateAroundLocal(this.pauseRing2, Vector3.forward, -360f, 22f).setRepeat(-1);
		this.chatWindow.RotateAround(this.chatWindow.position, Vector3.up, -180f);
		LeanTween.rotateAround(this.chatWindow, Vector3.up, 180f, 2f).setEase(LeanTweenType.easeOutElastic).setDelay(1.2f);
		LeanTween.play(this.chatRect, this.chatSprites).setLoopPingPong();
		LeanTween.color(this.chatBar2, new Color(0.972549f, 0.2627451f, 0.42352942f, 0.5f), 1.2f).setEase(LeanTweenType.easeInQuad).setLoopPingPong().setDelay(1.2f);
		LeanTween.scale(this.chatBar2, new Vector2(1f, 0.7f), 1.2f).setEase(LeanTweenType.easeInQuad).setLoopPingPong();
		string origText = this.chatText.text;
		this.chatText.text = "";
		LeanTween.value(base.gameObject, 0f, (float)origText.Length, 6f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float val)
		{
			this.chatText.text = origText.Substring(0, Mathf.RoundToInt(val));
		}).setLoopClamp().setDelay(2f);
		LeanTween.alpha(this.rawImageRect, 0f, 1f).setLoopPingPong();
	}

	// Token: 0x040004A5 RID: 1189
	public RectTransform mainWindow;

	// Token: 0x040004A6 RID: 1190
	public RectTransform mainParagraphText;

	// Token: 0x040004A7 RID: 1191
	public RectTransform mainTitleText;

	// Token: 0x040004A8 RID: 1192
	public RectTransform mainButton1;

	// Token: 0x040004A9 RID: 1193
	public RectTransform mainButton2;

	// Token: 0x040004AA RID: 1194
	public RectTransform pauseRing1;

	// Token: 0x040004AB RID: 1195
	public RectTransform pauseRing2;

	// Token: 0x040004AC RID: 1196
	public RectTransform pauseWindow;

	// Token: 0x040004AD RID: 1197
	public RectTransform chatWindow;

	// Token: 0x040004AE RID: 1198
	public RectTransform chatRect;

	// Token: 0x040004AF RID: 1199
	public Sprite[] chatSprites;

	// Token: 0x040004B0 RID: 1200
	public RectTransform chatBar1;

	// Token: 0x040004B1 RID: 1201
	public RectTransform chatBar2;

	// Token: 0x040004B2 RID: 1202
	public Text chatText;

	// Token: 0x040004B3 RID: 1203
	public RectTransform rawImageRect;
}
