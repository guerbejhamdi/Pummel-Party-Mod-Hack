using System;
using UnityEngine;

// Token: 0x020004E2 RID: 1250
public class TreasureHuntIntro : MonoBehaviour
{
	// Token: 0x06002102 RID: 8450 RVA: 0x00017F68 File Offset: 0x00016168
	public void StartIntro(TreasureHuntController controller)
	{
		this.controller = controller;
		this.introAnimation.Play();
		AudioSystem.PlayOneShot(this.sound, 0.7f, 0f, 1f);
	}

	// Token: 0x06002103 RID: 8451 RVA: 0x00017F97 File Offset: 0x00016197
	public void HitSand()
	{
		AudioSystem.PlayOneShot(this.sandHitSound, 0.06f, 0f, 1f);
	}

	// Token: 0x06002104 RID: 8452 RVA: 0x000CD76C File Offset: 0x000CB96C
	public void TreasureHuntIntroFinished()
	{
		this.controller.introFinished = true;
		this.cam.enabled = false;
		for (int i = 0; i < this.controller.GetPlayerCount(); i++)
		{
			TreasureHuntPlayer treasureHuntPlayer = (TreasureHuntPlayer)this.controller.GetPlayer(i);
			if (treasureHuntPlayer.cam != null)
			{
				treasureHuntPlayer.cam.enabled = true;
			}
		}
	}

	// Token: 0x040023BD RID: 9149
	public Animation introAnimation;

	// Token: 0x040023BE RID: 9150
	public Camera cam;

	// Token: 0x040023BF RID: 9151
	public AudioClip sound;

	// Token: 0x040023C0 RID: 9152
	public AudioClip sandHitSound;

	// Token: 0x040023C1 RID: 9153
	private TreasureHuntController controller;
}
