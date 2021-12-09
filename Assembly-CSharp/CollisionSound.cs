using System;
using UnityEngine;

// Token: 0x02000451 RID: 1105
public class CollisionSound : MonoBehaviour
{
	// Token: 0x06001E59 RID: 7769 RVA: 0x000C3F58 File Offset: 0x000C2158
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.relativeVelocity.magnitude > this.min_velocity && this.clips.Length != 0)
		{
			int num = UnityEngine.Random.Range(0, this.clips.Length - 1);
			if (this.positional)
			{
				AudioSystem.PlayOneShot(this.clips[num], collision.contacts[0].point, this.volume, this.rolloff_mode, this.min_distance, this.max_distance, 0.1f);
				return;
			}
			AudioSystem.PlayOneShot(this.clips[num], this.volume, 0.1f, 1f);
		}
	}

	// Token: 0x04002141 RID: 8513
	public float min_velocity = 1f;

	// Token: 0x04002142 RID: 8514
	public float volume = 1f;

	// Token: 0x04002143 RID: 8515
	public bool positional = true;

	// Token: 0x04002144 RID: 8516
	public AudioRolloffMode rolloff_mode;

	// Token: 0x04002145 RID: 8517
	public float min_distance = 10f;

	// Token: 0x04002146 RID: 8518
	public float max_distance = 30f;

	// Token: 0x04002147 RID: 8519
	public AudioClip[] clips;
}
