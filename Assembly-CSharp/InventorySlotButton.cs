using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000502 RID: 1282
public class InventorySlotButton : BasicButtonBase
{
	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x06002195 RID: 8597 RVA: 0x000185BE File Offset: 0x000167BE
	// (set) Token: 0x06002196 RID: 8598 RVA: 0x000CF6AC File Offset: 0x000CD8AC
	public bool Active
	{
		get
		{
			return this.active;
		}
		set
		{
			this.borderImageIcon.color = (value ? this.borderColor : this.borderColorInactive);
			this.countText.color = (value ? this.countTextActive : this.countTextInactive);
			this.active = value;
		}
	}

	// Token: 0x06002197 RID: 8599 RVA: 0x000185C6 File Offset: 0x000167C6
	public override void OnSubmit()
	{
		this.inventoryUI.OnClick(this.itemID);
		base.OnSubmit();
	}

	// Token: 0x06002198 RID: 8600 RVA: 0x000185DF File Offset: 0x000167DF
	protected override void OnEnable()
	{
		base.OnEnable();
	}

	// Token: 0x06002199 RID: 8601 RVA: 0x000CF6F8 File Offset: 0x000CD8F8
	public void OnSelect()
	{
		this.inventoryUI.OnSelect(this.itemID);
		this.arrowIcon.enabled = true;
		if (this.active)
		{
			LeanTween.cancel(this.itemIcon);
			LeanTween.scale(this.itemIcon, this.maxScale, 0.1f).setEase(LeanTweenType.easeInOutSine);
			LeanTween.cancel(this.borderImage);
			LeanTween.color(this.borderImage, this.pulseColor1, 0.1f).setEase(LeanTweenType.easeInOutSine).setOnComplete(delegate()
			{
				LeanTween.color(this.borderImage, this.pulseColor2, 0.333f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong().setRecursive(false);
			}).setRecursive(false);
		}
	}

	// Token: 0x0600219A RID: 8602 RVA: 0x000CF794 File Offset: 0x000CD994
	public void OnDeselect()
	{
		this.inventoryUI.OnDeSelect(this.itemID);
		this.arrowIcon.enabled = false;
		if (this.active)
		{
			LeanTween.cancel(this.itemIcon);
			LeanTween.scale(this.itemIcon, this.minScale, 0.1f);
			LeanTween.cancel(this.borderImage);
			LeanTween.color(this.borderImage, this.borderColor, 0.1f).setEase(LeanTweenType.easeInOutSine).setRecursive(false);
		}
	}

	// Token: 0x04002457 RID: 9303
	public InventoryUI inventoryUI;

	// Token: 0x04002458 RID: 9304
	public int itemID;

	// Token: 0x04002459 RID: 9305
	public RectTransform borderImage;

	// Token: 0x0400245A RID: 9306
	public Image arrowIcon;

	// Token: 0x0400245B RID: 9307
	public Image borderImageIcon;

	// Token: 0x0400245C RID: 9308
	public Text countText;

	// Token: 0x0400245D RID: 9309
	public RectTransform itemIcon;

	// Token: 0x0400245E RID: 9310
	private bool active;

	// Token: 0x0400245F RID: 9311
	public Color countTextActive;

	// Token: 0x04002460 RID: 9312
	public Color countTextInactive;

	// Token: 0x04002461 RID: 9313
	public Color borderColor;

	// Token: 0x04002462 RID: 9314
	public Color borderColorInactive;

	// Token: 0x04002463 RID: 9315
	public Color pulseColor1;

	// Token: 0x04002464 RID: 9316
	public Color pulseColor2;

	// Token: 0x04002465 RID: 9317
	private Vector3 maxScale = new Vector3(1.35f, 1.35f, 1f);

	// Token: 0x04002466 RID: 9318
	private Vector3 minScale = new Vector3(1f, 1f, 1f);
}
