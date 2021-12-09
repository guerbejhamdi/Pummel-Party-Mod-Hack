using System;
using UnityEngine;

// Token: 0x02000345 RID: 837
public class CraneCollisionHitSound : MonoBehaviour
{
	// Token: 0x06001699 RID: 5785 RVA: 0x00011046 File Offset: 0x0000F246
	public void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600169A RID: 5786 RVA: 0x0009FDC0 File Offset: 0x0009DFC0
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.rigidbody == null || this.rb.velocity.sqrMagnitude > collision.rigidbody.velocity.sqrMagnitude)
		{
			float magnitude = collision.relativeVelocity.magnitude;
			if (magnitude > this.minMagnitude)
			{
				float pitch = UnityEngine.Random.Range(this.minPitch, this.maxPitch);
				float num = UnityEngine.Random.Range(this.minVol, this.maxVol);
				num *= magnitude / 3f;
				AudioSystem.PlayOneShot(this.clips[UnityEngine.Random.Range(0, this.clips.Length)], num, 0f, pitch);
			}
		}
	}

	// Token: 0x040017CD RID: 6093
	public AudioClip[] clips;

	// Token: 0x040017CE RID: 6094
	public float minVol = 1f;

	// Token: 0x040017CF RID: 6095
	public float maxVol = 1f;

	// Token: 0x040017D0 RID: 6096
	public float maxPitch = 1f;

	// Token: 0x040017D1 RID: 6097
	public float minPitch = 1f;

	// Token: 0x040017D2 RID: 6098
	public float minMagnitude = 0.3f;

	// Token: 0x040017D3 RID: 6099
	private Rigidbody rb;
}
