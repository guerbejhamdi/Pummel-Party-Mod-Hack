using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200005D RID: 93
public class FinderBeaconPickup : MonoBehaviour
{
	// Token: 0x1700003B RID: 59
	// (get) Token: 0x060001B2 RID: 434 RVA: 0x00004A65 File Offset: 0x00002C65
	// (set) Token: 0x060001B3 RID: 435 RVA: 0x00004A6D File Offset: 0x00002C6D
	public bool PickedUp { get; set; }

	// Token: 0x060001B4 RID: 436 RVA: 0x0003302C File Offset: 0x0003122C
	public void OnPickup()
	{
		AudioSystem.PlayOneShot(this.pickupSound, 0.35f, 0f, 1f);
		this.controller.Spawn(this.pickupEffectPrefab, base.transform.position, Quaternion.identity);
		this.PickedUp = true;
		base.StartCoroutine(this.Despawn());
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x00004A76 File Offset: 0x00002C76
	private IEnumerator Despawn()
	{
		float startTime = Time.time;
		float despawnTime = 1f;
		while (Time.time - startTime < despawnTime)
		{
			float d = this.despawnCurve.Evaluate((Time.time - startTime) / despawnTime);
			base.transform.localScale = Vector3.one * d;
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04000208 RID: 520
	public AudioClip pickupSound;

	// Token: 0x04000209 RID: 521
	public GameObject pickupEffectPrefab;

	// Token: 0x0400020A RID: 522
	public AnimationCurve despawnCurve;

	// Token: 0x0400020B RID: 523
	public byte beaconID;

	// Token: 0x0400020C RID: 524
	public FinderController controller;
}
