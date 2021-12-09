using System;
using UnityEngine;
using ZP.Net;

// Token: 0x0200027B RID: 635
public class TreasureHuntCollectible : TreasureHuntObject
{
	// Token: 0x06001292 RID: 4754 RVA: 0x0000EEF2 File Offset: 0x0000D0F2
	public override void Start()
	{
		base.gameObject.name = "Collectible";
		base.Start();
	}

	// Token: 0x06001293 RID: 4755 RVA: 0x0008F818 File Offset: 0x0008DA18
	public override void Interact(short player_slot, bool remote_interact)
	{
		if (this.interactable)
		{
			TreasureHuntPlayer treasureHuntPlayer = (TreasureHuntPlayer)this.minigame_controller.GetPlayerInSlot(player_slot);
			if (NetSystem.IsServer)
			{
				TreasureHuntPlayer treasureHuntPlayer2 = treasureHuntPlayer;
				short score = treasureHuntPlayer2.Score;
				treasureHuntPlayer2.Score = score + 1;
			}
			this.root.SetActive(false);
			base.Clickable = false;
			AudioSystem.PlayOneShot(this.collect_sound, 1f, 0f, 1f);
			UnityEngine.Object.Instantiate<GameObject>(this.collect_particle, base.transform.position, Quaternion.identity);
			base.Interact(player_slot, remote_interact);
		}
	}

	// Token: 0x040013A4 RID: 5028
	public AudioClip collect_sound;

	// Token: 0x040013A5 RID: 5029
	public GameObject collect_particle;
}
