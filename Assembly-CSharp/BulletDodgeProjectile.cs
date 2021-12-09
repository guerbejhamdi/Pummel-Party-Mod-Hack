using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000176 RID: 374
public class BulletDodgeProjectile : MonoBehaviour
{
	// Token: 0x06000ACE RID: 2766 RVA: 0x0000AF80 File Offset: 0x00009180
	public void Start()
	{
		BulletDodgeProjectile.projectiles.Add(this);
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x0005E194 File Offset: 0x0005C394
	public void Update()
	{
		base.transform.position += Time.deltaTime * this.velocity;
		if (this.type == BulletDodgeBulletType.Spinner)
		{
			Debug.DrawLine(base.transform.position, this.spinnerHitPoint);
		}
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x0000AF8D File Offset: 0x0000918D
	public void Launch(Vector3 vel, float life)
	{
		this.velocity = vel;
		UnityEngine.Object.Destroy(this, life);
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x0000AF9D File Offset: 0x0000919D
	private void OnDestroy()
	{
		BulletDodgeProjectile.projectiles.Remove(this);
	}

	// Token: 0x040009BF RID: 2495
	public static List<BulletDodgeProjectile> projectiles = new List<BulletDodgeProjectile>();

	// Token: 0x040009C0 RID: 2496
	public Vector3 velocity;

	// Token: 0x040009C1 RID: 2497
	public BulletDodgeBulletType type;

	// Token: 0x040009C2 RID: 2498
	public Vector3 spinnerHitPoint = new Vector3(1000f, 1000f, 1000f);
}
