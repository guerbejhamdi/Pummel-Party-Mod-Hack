using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004AF RID: 1199
public class SpearTrap : BoardNodeEvent
{
	// Token: 0x06001FFB RID: 8187 RVA: 0x00017618 File Offset: 0x00015818
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode boardNode, int seed)
	{
		Debug.Log("Doing Spear Trap");
		this.rand = new System.Random(seed);
		AudioSystem.PlayOneShot(this.spearSound, 0.35f, 0f, 1f);
		yield return new WaitForSeconds(0.5f);
		this.animFinished = false;
		this.targetPlayer = player;
		this.anim.Play();
		yield return new WaitUntil(() => this.animFinished);
		yield break;
	}

	// Token: 0x06001FFC RID: 8188 RVA: 0x000C98AC File Offset: 0x000C7AAC
	public void OnTrapHit()
	{
		DamageInstance d = new DamageInstance
		{
			damage = this.rand.Next(7, 9),
			origin = this.targetPlayer.transform.position - Vector3.up,
			blood = true,
			ragdoll = true,
			ragdollVel = 0.5f,
			bloodVel = 4f,
			bloodAmount = 0.65f,
			details = "Spear Trap",
			removeKeys = true
		};
		this.targetPlayer.ApplyDamage(d);
		GameManager.Board.boardCamera.AddShake(0.35f);
	}

	// Token: 0x06001FFD RID: 8189 RVA: 0x00017635 File Offset: 0x00015835
	public void TrapRetracted()
	{
		this.animFinished = true;
	}

	// Token: 0x040022D2 RID: 8914
	public Animation anim;

	// Token: 0x040022D3 RID: 8915
	public AudioClip spearSound;

	// Token: 0x040022D4 RID: 8916
	private BoardPlayer targetPlayer;

	// Token: 0x040022D5 RID: 8917
	private bool animFinished;
}
