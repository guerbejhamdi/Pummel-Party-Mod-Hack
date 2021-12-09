using System;
using UnityEngine;

// Token: 0x020000B5 RID: 181
public class InventoryWindow : MainMenuWindow
{
	// Token: 0x060003CB RID: 971 RVA: 0x0000611A File Offset: 0x0000431A
	public override void TransitionIn()
	{
		LeanTween.cancel(base.gameObject, false);
		this.canvas_group.alpha = 1f;
		LeanTween.moveLocal(base.gameObject, this.up, 0.1f).setEase(LeanTweenType.easeInOutSine);
	}

	// Token: 0x060003CC RID: 972 RVA: 0x00006156 File Offset: 0x00004356
	public override void TransitionOut()
	{
		LeanTween.cancel(base.gameObject, false);
		LeanTween.moveLocal(base.gameObject, this.down, 0.1f).setEase(LeanTweenType.easeInOutSine).setOnComplete(delegate()
		{
			this.canvas_group.alpha = 0f;
		});
	}

	// Token: 0x0400040C RID: 1036
	private Vector3 up = new Vector3(0f, 0f, 0f);

	// Token: 0x0400040D RID: 1037
	private Vector3 down = new Vector3(0f, -150f, 0f);
}
