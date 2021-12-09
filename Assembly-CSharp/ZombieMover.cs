using System;
using UnityEngine;

// Token: 0x020000A3 RID: 163
public class ZombieMover : MonoBehaviour
{
	// Token: 0x0600036F RID: 879 RVA: 0x00005D8F File Offset: 0x00003F8F
	private void Awake()
	{
		base.GetComponent<Animator>().Play("ZombieWalk", 0, UnityEngine.Random.Range(0f, 1f));
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00005DB1 File Offset: 0x00003FB1
	private void Update()
	{
		base.transform.position += base.transform.forward * this.m_speed * Time.deltaTime;
	}

	// Token: 0x04000380 RID: 896
	[SerializeField]
	protected float m_speed = 1f;
}
