using System;
using UnityEngine;
using ZP.Net;

// Token: 0x02000281 RID: 641
public class TreasureHuntTreasure : TreasureHuntObject
{
	// Token: 0x060012C5 RID: 4805 RVA: 0x0000F09A File Offset: 0x0000D29A
	public override void Start()
	{
		base.Start();
		this.minigame_controller.treasure = this;
	}

	// Token: 0x060012C6 RID: 4806 RVA: 0x0000F0AE File Offset: 0x0000D2AE
	public override void Interact(short player_slot, bool remote_interact = false)
	{
		if (this.interactable)
		{
			if (NetSystem.IsServer)
			{
				this.minigame_controller.SetWinner(player_slot);
			}
			this.root.SetActive(false);
			base.Clickable = false;
			base.Interact(player_slot, remote_interact);
		}
	}

	// Token: 0x040013F6 RID: 5110
	public GameObject pickupParticle;
}
