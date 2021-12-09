using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200033F RID: 831
public class FireStatueEvent : BoardNodeEvent
{
	// Token: 0x06001683 RID: 5763 RVA: 0x00010F9B File Offset: 0x0000F19B
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode boardNode, int seed)
	{
		this.rand = new System.Random(seed);
		int damage = this.rand.Next(7, 9);
		float segmentLength = this.ps.duration / (float)damage;
		AudioSystem.PlayOneShot(this.flameClip, this.flameVolume, 0f, 1f);
		this.ps.Play();
		float startTime = Time.time;
		int num;
		for (int i = 0; i < damage; i = num)
		{
			bool flag = i == damage - 1;
			DamageInstance d = new DamageInstance
			{
				damage = 1,
				origin = player.transform.position + Vector3.up - Vector3.forward,
				blood = flag,
				ragdoll = flag,
				ragdollVel = 0f,
				bloodVel = 7f,
				bloodAmount = 1.5f,
				details = "Fire Statue",
				removeKeys = flag,
				sound = flag,
				volume = 1f
			};
			player.ApplyDamage(d);
			float segmentStartTime = Time.time;
			yield return new WaitUntil(delegate()
			{
				this.pointLight.intensity = this.lightCurve.Evaluate((Time.time - startTime) / this.ps.duration);
				return Time.time - segmentStartTime >= segmentLength;
			});
			num = i + 1;
		}
		this.pointLight.intensity = 0f;
		yield return new WaitForSeconds(1f);
		yield break;
	}

	// Token: 0x040017AF RID: 6063
	public ParticleSystem ps;

	// Token: 0x040017B0 RID: 6064
	public AudioClip flameClip;

	// Token: 0x040017B1 RID: 6065
	public float flameVolume = 1f;

	// Token: 0x040017B2 RID: 6066
	public Light pointLight;

	// Token: 0x040017B3 RID: 6067
	public AnimationCurve lightCurve;
}
