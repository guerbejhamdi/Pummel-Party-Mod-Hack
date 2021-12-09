using System;
using UnityEngine;

// Token: 0x020000C3 RID: 195
public class OldGUIExamplesCS : MonoBehaviour
{
	// Token: 0x060003F9 RID: 1017 RVA: 0x0003BC8C File Offset: 0x00039E8C
	private void Start()
	{
		this.w = (float)Screen.width;
		this.h = (float)Screen.height;
		this.buttonRect1 = new LTRect(0.1f * this.w, 0.8f * this.h, 0.2f * this.w, 0.14f * this.h);
		this.buttonRect2 = new LTRect(1.2f * this.w, 0.8f * this.h, 0.2f * this.w, 0.14f * this.h);
		this.buttonRect3 = new LTRect(0.35f * this.w, 0f * this.h, 0.3f * this.w, 0.2f * this.h, 0f);
		this.buttonRect4 = new LTRect(0f * this.w, 0.4f * this.h, 0.3f * this.w, 0.2f * this.h, 1f, 15f);
		this.grumpyRect = new LTRect(0.5f * this.w - (float)this.grumpy.width * 0.5f, 0.5f * this.h - (float)this.grumpy.height * 0.5f, (float)this.grumpy.width, (float)this.grumpy.height);
		this.beautyTileRect = new LTRect(0f, 0f, 1f, 1f);
		LeanTween.move(this.buttonRect2, new Vector2(0.55f * this.w, this.buttonRect2.rect.y), 0.7f).setEase(LeanTweenType.easeOutQuad);
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x0000631B File Offset: 0x0000451B
	public void catMoved()
	{
		Debug.Log("cat moved...");
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x0003BE68 File Offset: 0x0003A068
	private void OnGUI()
	{
		GUI.DrawTexture(this.grumpyRect.rect, this.grumpy);
		if (GUI.Button(new Rect(0f * this.w, 0f * this.h, 0.2f * this.w, 0.14f * this.h), "Move Cat") && !LeanTween.isTweening(this.grumpyRect))
		{
			Vector2 to = new Vector2(this.grumpyRect.rect.x, this.grumpyRect.rect.y);
			LeanTween.move(this.grumpyRect, new Vector2(1f * (float)Screen.width - (float)this.grumpy.width, 0f * (float)Screen.height), 1f).setEase(LeanTweenType.easeOutBounce).setOnComplete(new Action(this.catMoved));
			LeanTween.move(this.grumpyRect, to, 1f).setDelay(1f).setEase(LeanTweenType.easeOutBounce);
		}
		if (GUI.Button(this.buttonRect1.rect, "Scale Centered"))
		{
			LeanTween.scale(this.buttonRect1, new Vector2(this.buttonRect1.rect.width, this.buttonRect1.rect.height) * 1.2f, 0.25f).setEase(LeanTweenType.easeOutQuad);
			LeanTween.move(this.buttonRect1, new Vector2(this.buttonRect1.rect.x - this.buttonRect1.rect.width * 0.1f, this.buttonRect1.rect.y - this.buttonRect1.rect.height * 0.1f), 0.25f).setEase(LeanTweenType.easeOutQuad);
		}
		if (GUI.Button(this.buttonRect2.rect, "Scale"))
		{
			LeanTween.scale(this.buttonRect2, new Vector2(this.buttonRect2.rect.width, this.buttonRect2.rect.height) * 1.2f, 0.25f).setEase(LeanTweenType.easeOutBounce);
		}
		if (GUI.Button(new Rect(0.76f * this.w, 0.53f * this.h, 0.2f * this.w, 0.14f * this.h), "Flip Tile"))
		{
			LeanTween.move(this.beautyTileRect, new Vector2(0f, this.beautyTileRect.rect.y + 1f), 1f).setEase(LeanTweenType.easeOutBounce);
		}
		GUI.DrawTextureWithTexCoords(new Rect(0.8f * this.w, 0.5f * this.h - (float)this.beauty.height * 0.5f, (float)this.beauty.width * 0.5f, (float)this.beauty.height * 0.5f), this.beauty, this.beautyTileRect.rect);
		if (GUI.Button(this.buttonRect3.rect, "Alpha"))
		{
			LeanTween.alpha(this.buttonRect3, 0f, 1f).setEase(LeanTweenType.easeOutQuad);
			LeanTween.alpha(this.buttonRect3, 1f, 1f).setDelay(1f).setEase(LeanTweenType.easeInQuad);
			LeanTween.alpha(this.grumpyRect, 0f, 1f).setEase(LeanTweenType.easeOutQuad);
			LeanTween.alpha(this.grumpyRect, 1f, 1f).setDelay(1f).setEase(LeanTweenType.easeInQuad);
		}
		GUI.color = new Color(1f, 1f, 1f, 1f);
		if (GUI.Button(this.buttonRect4.rect, "Rotate"))
		{
			LeanTween.rotate(this.buttonRect4, 150f, 1f).setEase(LeanTweenType.easeOutElastic);
			LeanTween.rotate(this.buttonRect4, 0f, 1f).setDelay(1f).setEase(LeanTweenType.easeOutElastic);
		}
		GUI.matrix = Matrix4x4.identity;
	}

	// Token: 0x0400044E RID: 1102
	public Texture2D grumpy;

	// Token: 0x0400044F RID: 1103
	public Texture2D beauty;

	// Token: 0x04000450 RID: 1104
	private float w;

	// Token: 0x04000451 RID: 1105
	private float h;

	// Token: 0x04000452 RID: 1106
	private LTRect buttonRect1;

	// Token: 0x04000453 RID: 1107
	private LTRect buttonRect2;

	// Token: 0x04000454 RID: 1108
	private LTRect buttonRect3;

	// Token: 0x04000455 RID: 1109
	private LTRect buttonRect4;

	// Token: 0x04000456 RID: 1110
	private LTRect grumpyRect;

	// Token: 0x04000457 RID: 1111
	private LTRect beautyTileRect;
}
