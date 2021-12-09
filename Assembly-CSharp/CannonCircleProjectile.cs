using System;
using UnityEngine;

// Token: 0x0200018E RID: 398
public class CannonCircleProjectile : MonoBehaviour
{
	// Token: 0x06000B62 RID: 2914 RVA: 0x0000B36A File Offset: 0x0000956A
	private void Start()
	{
		UnityEngine.Object.Destroy(base.gameObject, this.lifeTime);
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x0000B37D File Offset: 0x0000957D
	private void Update()
	{
		base.transform.position += base.transform.forward * this.speed * Time.deltaTime;
	}

	// Token: 0x04000A72 RID: 2674
	public float lifeTime = 5f;

	// Token: 0x04000A73 RID: 2675
	public float speed = 10f;
}
