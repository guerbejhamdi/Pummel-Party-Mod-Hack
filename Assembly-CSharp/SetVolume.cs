using System;
using UnityEngine;

// Token: 0x02000498 RID: 1176
[RequireComponent(typeof(AudioSource))]
public class SetVolume : MonoBehaviour
{
	// Token: 0x06001F79 RID: 8057 RVA: 0x000C7EE0 File Offset: 0x000C60E0
	private void Start()
	{
		if (this.audioSource == null)
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		this.audioSource.volume = AudioSystem.GetVolume(SoundType.Effect, (this.volume == -1f) ? this.audioSource.volume : this.volume);
	}

	// Token: 0x04002258 RID: 8792
	public AudioSource audioSource;

	// Token: 0x04002259 RID: 8793
	public float volume = -1f;
}
