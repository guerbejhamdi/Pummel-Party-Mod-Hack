using System;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class ParticleFollower : MonoBehaviour
{
	// Token: 0x0600036C RID: 876 RVA: 0x00005D4A File Offset: 0x00003F4A
	private void Start()
	{
		this.target = base.transform.parent;
		base.transform.parent = null;
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00005D69 File Offset: 0x00003F69
	private void Update()
	{
		if (this.target != null)
		{
			base.transform.position = this.target.position;
		}
	}

	// Token: 0x0400037F RID: 895
	private Transform target;
}
