using System;
using UnityEngine;

// Token: 0x02000377 RID: 887
public class FX_Bandage : MonoBehaviour
{
	// Token: 0x060017E4 RID: 6116 RVA: 0x00011BCB File Offset: 0x0000FDCB
	public void PlayBandage()
	{
		AudioSystem.PlayOneShot(this.bandageSound, 0.15f, 0.1f, 1f);
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x00011BE7 File Offset: 0x0000FDE7
	public void PlayClick()
	{
		AudioSystem.PlayOneShot(this.clickSound, 0.5f, 0.1f, 1f);
	}

	// Token: 0x060017E6 RID: 6118 RVA: 0x00011C03 File Offset: 0x0000FE03
	public void PlayMagic()
	{
		AudioSystem.PlayOneShot(this.magicSound, 0.5f, 0.1f, 1f);
	}

	// Token: 0x04001967 RID: 6503
	public AudioClip bandageSound;

	// Token: 0x04001968 RID: 6504
	public AudioClip clickSound;

	// Token: 0x04001969 RID: 6505
	public AudioClip magicSound;
}
