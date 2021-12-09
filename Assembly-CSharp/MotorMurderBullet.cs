using System;
using UnityEngine;

// Token: 0x020001DF RID: 479
public class MotorMurderBullet : MonoBehaviour
{
	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x0000C794 File Offset: 0x0000A994
	// (set) Token: 0x06000DE6 RID: 3558 RVA: 0x0000C79C File Offset: 0x0000A99C
	public bool HitSomething { get; set; }

	// Token: 0x06000DE7 RID: 3559 RVA: 0x000700B0 File Offset: 0x0006E2B0
	private void Update()
	{
		Vector3 b = base.transform.forward * this.speed * Time.deltaTime;
		this.distanceMoved += b;
		base.transform.position += b;
		if (this.distanceMoved.sqrMagnitude > this.maxDistance * this.maxDistance)
		{
			if (this.HitSomething)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.hitFX, base.transform.position - base.transform.forward * 0.35f, Quaternion.identity);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x0000C7A5 File Offset: 0x0000A9A5
	public void SetType(byte typeID)
	{
		this.bulletObjects[(int)typeID].SetActive(true);
	}

	// Token: 0x04000D4D RID: 3405
	public float maxDistance = 5f;

	// Token: 0x04000D4E RID: 3406
	public float speed = 125f;

	// Token: 0x04000D4F RID: 3407
	public LayerMask hitMask;

	// Token: 0x04000D50 RID: 3408
	public GameObject[] bulletObjects;

	// Token: 0x04000D52 RID: 3410
	public GameObject hitFX;

	// Token: 0x04000D53 RID: 3411
	private Vector3 distanceMoved = Vector3.zero;
}
