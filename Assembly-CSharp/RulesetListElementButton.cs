using System;

// Token: 0x02000086 RID: 134
public class RulesetListElementButton : BasicButtonBase
{
	// Token: 0x060002E5 RID: 741 RVA: 0x0000580A File Offset: 0x00003A0A
	protected override void Start()
	{
		this.element = base.GetComponentInParent<RulesetUIElement>();
		base.Start();
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x00037AA0 File Offset: 0x00035CA0
	public override void OnSubmit()
	{
		if (this.isButton)
		{
			this.element.OnButtonPress();
			return;
		}
		if (this.button == null || !this.button.IsInteractable())
		{
			return;
		}
		this.element.IncrementList(this.right);
		base.OnSubmit();
	}

	// Token: 0x040002FD RID: 765
	public bool right;

	// Token: 0x040002FE RID: 766
	public bool isButton;

	// Token: 0x040002FF RID: 767
	private RulesetUIElement element;
}
