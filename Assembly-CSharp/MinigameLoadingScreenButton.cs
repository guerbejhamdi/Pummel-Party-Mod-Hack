using System;
using UnityEngine;

// Token: 0x0200051E RID: 1310
public class MinigameLoadingScreenButton : BasicButtonBase
{
	// Token: 0x0600222B RID: 8747 RVA: 0x0001882F File Offset: 0x00016A2F
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x0600222C RID: 8748 RVA: 0x00018C32 File Offset: 0x00016E32
	public override void OnSubmit()
	{
		GameManager.UIController.MinigameLoadScreen.DoButtonEvent(this.OnClickEvent);
		base.OnSubmit();
	}

	// Token: 0x0600222D RID: 8749 RVA: 0x00018099 File Offset: 0x00016299
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x0400254D RID: 9549
	[Header("Basic Button Vars")]
	public MinigameLoadingScreenButtonEvent OnClickEvent;

	// Token: 0x0400254E RID: 9550
	private GameObject ui_canvas;
}
