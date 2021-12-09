using System;

// Token: 0x02000509 RID: 1289
public class LobbyOptionButton : BasicButtonBase
{
	// Token: 0x060021BC RID: 8636 RVA: 0x00018798 File Offset: 0x00016998
	protected override void Start()
	{
		this.tab = base.GetComponentInParent<LobbyOptionTab>();
		base.Start();
	}

	// Token: 0x060021BD RID: 8637 RVA: 0x000187AC File Offset: 0x000169AC
	public override void OnSubmit()
	{
		if (this.button == null || !this.button.IsInteractable())
		{
			return;
		}
		this.tab.IncrementOption(this.right);
		base.OnSubmit();
	}

	// Token: 0x0400248A RID: 9354
	public bool right;

	// Token: 0x0400248B RID: 9355
	private LobbyOptionTab tab;
}
