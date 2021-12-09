using System;
using System.Collections;
using UnityEngine;
using ZP.Utility;

// Token: 0x0200030B RID: 779
public class RandomFlyingObjectEvent : BoardNodeEvent
{
	// Token: 0x06001593 RID: 5523 RVA: 0x000105BC File Offset: 0x0000E7BC
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode node, int seed)
	{
		System.Random rand = new System.Random(seed);
		int id = rand.Next(0, this.flyingObjects.Length);
		Vector3 dir = Vector3.zero;
		dir = Quaternion.Euler(ZPMath.RandomFloat(rand, -45f, -110f), ZPMath.RandomFloat(rand, -90f, 90f), 0f) * Vector3.forward;
		this.flyingObjects[id].SetActive(true);
		this.flyingObjects[id].transform.position = node.transform.position + new Vector3(0f, 1f, 0f) + dir * 8f;
		this.flyingObjects[id].transform.rotation = Quaternion.Euler(ZPMath.RandomFloat(rand, -360f, 360f), ZPMath.RandomFloat(rand, -360f, 360f), ZPMath.RandomFloat(rand, -360f, 360f));
		Rigidbody rb = this.flyingObjects[id].GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.angularVelocity = Vector3.zero;
		rb.velocity = Vector3.zero;
		yield return null;
		rb.AddForce(-dir * 20f, ForceMode.Impulse);
		rb.angularVelocity = new Vector3(ZPMath.RandomFloat(rand, -90f, -90f), ZPMath.RandomFloat(rand, -90f, -90f), ZPMath.RandomFloat(rand, -90f, -90f)) * 3f;
		yield return new WaitForSeconds(0.3f);
		DamageInstance d = new DamageInstance
		{
			damage = 6,
			origin = this.flyingObjects[id].transform.position,
			blood = true,
			ragdoll = true,
			ragdollVel = 15f,
			bloodVel = 13f,
			bloodAmount = 1f,
			sound = true,
			volume = 0.75f,
			details = "Random Flying Object",
			removeKeys = true
		};
		player.ApplyDamage(d);
		GameManager.Board.boardCamera.AddShake(0.4f);
		yield return new WaitForSeconds(2f);
		yield break;
	}

	// Token: 0x04001692 RID: 5778
	public GameObject[] flyingObjects;
}
