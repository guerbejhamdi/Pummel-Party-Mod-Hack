using System;
using UnityEngine;

// Token: 0x02000275 RID: 629
public class FX_TracerShooter : MonoBehaviour
{
	// Token: 0x06001252 RID: 4690 RVA: 0x0000ECD8 File Offset: 0x0000CED8
	private void Start()
	{
		this.system = base.GetComponent<ParticleSystem>();
		this.m_nextFire = Time.time + UnityEngine.Random.Range(this.m_minCooldown, this.m_maxCooldown);
	}

	// Token: 0x06001253 RID: 4691 RVA: 0x0008D75C File Offset: 0x0008B95C
	private void Update()
	{
		if (!this.m_isFiring && Time.time > this.m_nextFire)
		{
			this.m_isFiring = true;
			this.m_bulletsRemaining = UnityEngine.Random.Range(this.m_minFiredBullets, this.m_maxFiredBullets);
		}
		if (this.m_isFiring)
		{
			if (this.m_bulletsRemaining > 0 && Time.time > this.m_nextBulletTime)
			{
				this.system.Emit(1);
				this.m_nextBulletTime = Time.time + this.m_fireRate;
				this.m_bulletsRemaining--;
				return;
			}
			if (this.m_bulletsRemaining <= 0)
			{
				this.m_isFiring = false;
				this.m_nextFire = Time.time + UnityEngine.Random.Range(this.m_minCooldown, this.m_maxCooldown);
			}
		}
	}

	// Token: 0x0400134A RID: 4938
	[SerializeField]
	private float m_fireRate = 0.25f;

	// Token: 0x0400134B RID: 4939
	[SerializeField]
	private float m_minCooldown = 3f;

	// Token: 0x0400134C RID: 4940
	[SerializeField]
	private float m_maxCooldown = 6f;

	// Token: 0x0400134D RID: 4941
	[SerializeField]
	private int m_minFiredBullets = 5;

	// Token: 0x0400134E RID: 4942
	[SerializeField]
	private int m_maxFiredBullets = 10;

	// Token: 0x0400134F RID: 4943
	private bool m_isFiring;

	// Token: 0x04001350 RID: 4944
	private float m_nextFire;

	// Token: 0x04001351 RID: 4945
	private int m_bulletsRemaining;

	// Token: 0x04001352 RID: 4946
	private float m_nextBulletTime;

	// Token: 0x04001353 RID: 4947
	private ParticleSystem system;
}
