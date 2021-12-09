using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004AB RID: 1195
public class SnowBallEvent : BoardNodeEvent
{
	// Token: 0x06001FED RID: 8173 RVA: 0x000175C1 File Offset: 0x000157C1
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode node, int seed)
	{
		this.snowball.SetActive(true);
		this.snowball.transform.position = node.transform.position + Vector3.up * 10f;
		Rigidbody component = this.snowball.GetComponent<Rigidbody>();
		component.isKinematic = false;
		component.velocity = Vector3.zero;
		component.angularVelocity = Vector3.zero;
		yield return new WaitForSeconds(1.3f);
		DamageInstance d = new DamageInstance
		{
			damage = 6,
			origin = this.snowball.transform.position,
			blood = true,
			ragdoll = true,
			ragdollVel = 15f,
			bloodVel = 13f,
			bloodAmount = 1f,
			sound = true,
			volume = 0.75f,
			details = "Bus",
			removeKeys = true
		};
		player.ApplyDamage(d);
		GameManager.Board.boardCamera.AddShake(0.4f);
		yield return new WaitForSeconds(2f);
		yield break;
	}

	// Token: 0x040022BD RID: 8893
	public GameObject snowball;

	// Token: 0x040022BE RID: 8894
	public Vector3 moveDirection = new Vector3(-1f, 0f, 0f);

	// Token: 0x040022BF RID: 8895
	public float spawnOffset = 20f;

	// Token: 0x040022C0 RID: 8896
	public float speed = 20f;

	// Token: 0x040022C1 RID: 8897
	public LayerMask mask;

	// Token: 0x040022C2 RID: 8898
	public float maxSnowballScale = 3f;
}
