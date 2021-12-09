using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class BearTrap : BoardNodeEvent
{
	// Token: 0x0600004C RID: 76 RVA: 0x00003C1E File Offset: 0x00001E1E
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode boardNode, int seed)
	{
		Debug.Log("Doing Bear Trap");
		this.rand = new System.Random(seed);
		yield return new WaitForSeconds(0.25f);
		this.trapAnimation.Play();
		AudioSystem.PlayOneShot(this.bearTrapClips[GameManager.rand.Next(0, this.bearTrapClips.Length)], 0.35f, 0f, 1f);
		yield return new WaitForSeconds(0.05f);
		DamageInstance d = new DamageInstance
		{
			damage = this.rand.Next(7, 9),
			origin = player.transform.position - Vector3.up,
			blood = true,
			ragdoll = true,
			ragdollVel = 0.5f,
			bloodVel = 4f,
			bloodAmount = 0.65f,
			details = "Bear Trap",
			removeKeys = true
		};
		player.ApplyDamage(d);
		GameManager.Board.boardCamera.AddShake(0.15f);
		yield return new WaitForSeconds(1.5f);
		yield break;
	}

	// Token: 0x04000043 RID: 67
	public AudioClip[] bearTrapClips;

	// Token: 0x04000044 RID: 68
	public Animation trapAnimation;
}
