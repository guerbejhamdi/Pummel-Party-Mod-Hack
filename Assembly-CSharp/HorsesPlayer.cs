using System;

// Token: 0x020001B6 RID: 438
public class HorsesPlayer : CharacterBase
{
	// Token: 0x06000CA6 RID: 3238 RVA: 0x0000BCE3 File Offset: 0x00009EE3
	protected override void Start()
	{
		base.Start();
		this.minigameController = (HorsesController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
	}

	// Token: 0x04000C08 RID: 3080
	private HorsesController minigameController;
}
