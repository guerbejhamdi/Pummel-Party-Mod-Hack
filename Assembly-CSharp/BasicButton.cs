using System;
using UnityEngine;

// Token: 0x020004EB RID: 1259
public class BasicButton : BasicButtonBase
{
	// Token: 0x0600212C RID: 8492 RVA: 0x000CDF98 File Offset: 0x000CC198
	protected override void Start()
	{
		this.ui_canvas = GameObject.Find("Canvas");
		if (this.ui_canvas == null)
		{
			this.ui_canvas = GameObject.Find("BoardCanvas");
		}
		if (this.ui_canvas != null)
		{
			this.ui_controller = this.ui_canvas.GetComponent<UIController>();
		}
		base.Start();
	}

	// Token: 0x0600212D RID: 8493 RVA: 0x00018060 File Offset: 0x00016260
	public override void OnSubmit()
	{
		if (this.ui_controller != null)
		{
			this.ui_controller.DoButtonEvent(this.OnClickEvent);
		}
		else
		{
			GameManager.UIController.DoButtonEvent(this.OnClickEvent);
		}
		base.OnSubmit();
	}

	// Token: 0x0600212E RID: 8494 RVA: 0x00018099 File Offset: 0x00016299
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x040023DC RID: 9180
	[Header("Basic Button Vars")]
	public MainMenuButtonEventType OnClickEvent;

	// Token: 0x040023DD RID: 9181
	private GameObject ui_canvas;

	// Token: 0x040023DE RID: 9182
	private UIController ui_controller;
}
