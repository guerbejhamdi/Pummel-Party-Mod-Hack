using System;
using UnityEngine;

// Token: 0x02000284 RID: 644
public class VacuumSound : MonoBehaviour
{
	// Token: 0x060012DC RID: 4828 RVA: 0x00091F28 File Offset: 0x00090128
	public void Update()
	{
		float volume = AudioSystem.GetVolume(SoundType.Effect, this.active_volume);
		float volume2 = AudioSystem.GetVolume(SoundType.Effect, this.inactive_volume);
		float num = this.engineSpeed;
		if (this.vacuum_active)
		{
			this.engineSpeed += this.rev_speed * Time.deltaTime;
		}
		else
		{
			this.engineSpeed -= this.cooldown_speed * Time.deltaTime;
		}
		this.engineSpeed = Mathf.Clamp01(this.engineSpeed);
		if (num != this.engineSpeed)
		{
			this.source.volume = Mathf.Lerp(volume2, volume, this.engineSpeed);
			this.source.pitch = Mathf.Lerp(this.inactive_pitch, this.active_pitch, this.engineSpeed);
		}
	}

	// Token: 0x04001414 RID: 5140
	public AudioSource source;

	// Token: 0x04001415 RID: 5141
	public float active_volume;

	// Token: 0x04001416 RID: 5142
	public float inactive_volume;

	// Token: 0x04001417 RID: 5143
	public float active_pitch;

	// Token: 0x04001418 RID: 5144
	public float inactive_pitch;

	// Token: 0x04001419 RID: 5145
	public float rev_speed;

	// Token: 0x0400141A RID: 5146
	public float cooldown_speed;

	// Token: 0x0400141B RID: 5147
	public bool vacuum_active;

	// Token: 0x0400141C RID: 5148
	private float engineSpeed;
}
