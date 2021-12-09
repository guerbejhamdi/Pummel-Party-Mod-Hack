using System;
using UnityEngine;
using ZP.Net;

// Token: 0x02000237 RID: 567
public class SelfishStrideKey : MonoBehaviour
{
	// Token: 0x06001064 RID: 4196 RVA: 0x00080B00 File Offset: 0x0007ED00
	private void Update()
	{
		if (this.lookingForTarget)
		{
			for (int i = 0; i < GameManager.GetPlayerCount(); i++)
			{
				CharacterBase playerInSlot = GameManager.Minigame.GetPlayerInSlot((short)i);
				if ((playerInSlot.transform.position - base.transform.position).sqrMagnitude <= this.activateDistance)
				{
					this.target = playerInSlot;
					this.lookingForTarget = false;
					return;
				}
			}
			return;
		}
		Vector3 vector = this.target.transform.position - base.transform.position;
		if (vector.sqrMagnitude <= this.absorbDistance)
		{
			if (NetSystem.IsServer)
			{
				CharacterBase characterBase = this.target;
				short score = characterBase.Score;
				characterBase.Score = score + 1;
			}
			AudioSystem.PlayOneShot(this.coinCollectSound, 1f, 0f, 1f);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		base.transform.position += vector.normalized * Time.deltaTime * this.speed;
	}

	// Token: 0x040010C7 RID: 4295
	public float activateDistance = 5f;

	// Token: 0x040010C8 RID: 4296
	public AudioClip coinCollectSound;

	// Token: 0x040010C9 RID: 4297
	public GameObject gold;

	// Token: 0x040010CA RID: 4298
	public GameObject silver;

	// Token: 0x040010CB RID: 4299
	public GameObject bronze;

	// Token: 0x040010CC RID: 4300
	private bool lookingForTarget = true;

	// Token: 0x040010CD RID: 4301
	private CharacterBase target;

	// Token: 0x040010CE RID: 4302
	private float speed = 10f;

	// Token: 0x040010CF RID: 4303
	private float absorbDistance = 1f;
}
