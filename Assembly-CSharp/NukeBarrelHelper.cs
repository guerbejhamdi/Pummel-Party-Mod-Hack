using System;
using UnityEngine;

// Token: 0x0200031F RID: 799
public class NukeBarrelHelper : MonoBehaviour
{
	// Token: 0x060015D9 RID: 5593 RVA: 0x000108DF File Offset: 0x0000EADF
	public void Pour()
	{
		AudioSystem.PlayOneShot(this.m_poorClip, 1f, 0f, 1f);
		this.m_anim.SetTrigger("Pour");
	}

	// Token: 0x040016FF RID: 5887
	[SerializeField]
	private Animator m_anim;

	// Token: 0x04001700 RID: 5888
	[SerializeField]
	private AudioClip m_poorClip;
}
