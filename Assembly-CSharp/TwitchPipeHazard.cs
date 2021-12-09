using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004E9 RID: 1257
public class TwitchPipeHazard : BoardNodeEvent
{
	// Token: 0x06002124 RID: 8484 RVA: 0x0001802C File Offset: 0x0001622C
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode node, int seed)
	{
		this.pipe.gameObject.SetActive(true);
		TempAudioSource a = AudioSystem.PlayLooping(this.vacuum, 2f, 0.6f);
		this.pipe.transform.position = node.transform.position + Vector3.up * 17f;
		LeanTween.move(this.pipe, this.pipe.transform.position - Vector3.up * 6f, 2f).setEase(LeanTweenType.easeInOutSine);
		yield return new WaitForSeconds(2f);
		player.ExternalSpawnRagDoll(Vector3.up, 15f, 2.5f);
		yield return new WaitForSeconds(0.1f);
		AudioSystem.PlayOneShot(this.suckClip, 0.8f, 0f, 1f);
		yield return new WaitForSeconds(0.1f);
		player.transform.position += Vector3.up * 3f;
		DamageInstance d = new DamageInstance
		{
			damage = 6,
			origin = player.transform.position + Vector3.up,
			blood = true,
			ragdoll = false,
			bloodVel = 50f,
			bloodAmount = 1f,
			sound = true,
			volume = 0.75f,
			details = "Pipe hazard",
			removeKeys = true
		};
		player.ApplyDamage(d);
		GameManager.Board.boardCamera.AddShake(0.6f);
		yield return new WaitForSeconds(1f);
		a.FadeAudio(2.5f, FadeType.Out);
		LeanTween.move(this.pipe, this.pipe.transform.position + Vector3.up * 6f, 2f).setEase(LeanTweenType.easeInOutSine);
		yield return new WaitForSeconds(2f);
		this.pipe.transform.position = new Vector3(0f, -1000f, 0f);
		this.pipe.gameObject.SetActive(false);
		yield return new WaitForSeconds(1f);
		yield break;
	}

	// Token: 0x040023D3 RID: 9171
	public GameObject pipe;

	// Token: 0x040023D4 RID: 9172
	public AudioClip vacuum;

	// Token: 0x040023D5 RID: 9173
	public AudioClip suckClip;
}
