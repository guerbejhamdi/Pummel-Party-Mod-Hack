using System;
using UnityEngine;
using ZP.Net;

// Token: 0x0200004E RID: 78
public class ElementalMagesCrystal : MonoBehaviour
{
	// Token: 0x0600014C RID: 332 RVA: 0x000046A4 File Offset: 0x000028A4
	public void Update()
	{
		if (this.despawning && this.destroy.Elapsed(true))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600014D RID: 333 RVA: 0x000310E4 File Offset: 0x0002F2E4
	public void FixedUpdate()
	{
		if (NetSystem.IsServer && this.despawning)
		{
			this.flip = !this.flip;
			if (this.flip)
			{
				Vector3 position = base.transform.position;
				position.y = 0f;
				((ElementalMagesController)GameManager.Minigame).DoSplat(position, 0f, 2, this.team);
			}
		}
	}

	// Token: 0x0600014E RID: 334 RVA: 0x0003114C File Offset: 0x0002F34C
	public void Despawn(byte team)
	{
		this.team = team;
		this.despawning = true;
		this.animator.SetTrigger("Despawn");
		this.destroy.Start();
		UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(this.elementExplosionPrefabs[(int)team], base.transform.position, Quaternion.identity), 4f);
	}

	// Token: 0x0600014F RID: 335 RVA: 0x0000398C File Offset: 0x00001B8C
	public void DespawnAnimationFinished()
	{
	}

	// Token: 0x040001A9 RID: 425
	public Animator animator;

	// Token: 0x040001AA RID: 426
	public GameObject[] elementExplosionPrefabs;

	// Token: 0x040001AB RID: 427
	private bool despawning;

	// Token: 0x040001AC RID: 428
	private byte team;

	// Token: 0x040001AD RID: 429
	private ActionTimer destroy = new ActionTimer(1f);

	// Token: 0x040001AE RID: 430
	private bool flip;
}
