using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000335 RID: 821
public class TempAudioSource : MonoBehaviour
{
	// Token: 0x170001FB RID: 507
	// (get) Token: 0x0600164B RID: 5707 RVA: 0x00010D51 File Offset: 0x0000EF51
	public SoundType SoundType
	{
		get
		{
			return this.soundType;
		}
	}

	// Token: 0x0600164C RID: 5708 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Awake()
	{
	}

	// Token: 0x0600164D RID: 5709 RVA: 0x00010D59 File Offset: 0x0000EF59
	public void Initialize(Vector3 position, bool pooled)
	{
		base.transform.position = position;
		this.pooled = pooled;
	}

	// Token: 0x0600164E RID: 5710 RVA: 0x00010D6E File Offset: 0x0000EF6E
	public bool IsPlaying()
	{
		return !(this.audio_source == null) && !(this.audio_source.clip == null) && this.audio_source.isPlaying;
	}

	// Token: 0x0600164F RID: 5711 RVA: 0x00010D9E File Offset: 0x0000EF9E
	public void UpdateVolume()
	{
		this.audio_source.volume = AudioSystem.GetVolume(this.soundType, this.originalVolume);
	}

	// Token: 0x06001650 RID: 5712 RVA: 0x0009EE54 File Offset: 0x0009D054
	public void PlayOneShot(AudioClip clip, float volume, float pitch)
	{
		this.soundType = SoundType.Effect;
		this.originalVolume = volume;
		this.audio_source.Stop();
		this.audio_source.clip = clip;
		this.audio_source.pitch = pitch;
		this.SetSourceType(SourceType.NonPositional, volume, AudioRolloffMode.Linear, 10f, 30f, false);
		this.audio_source.Play();
	}

	// Token: 0x06001651 RID: 5713 RVA: 0x0009EEB4 File Offset: 0x0009D0B4
	public void PlayOneShot(AudioClip clip, Vector3 position, float volume, AudioRolloffMode rolloff_mode, float min_distance, float max_distance)
	{
		this.soundType = SoundType.Effect;
		this.originalVolume = volume;
		this.audio_source.enabled = true;
		this.audio_source.Pause();
		this.audio_source.clip = clip;
		this.audio_source.pitch = 1f;
		base.transform.position = position;
		this.SetSourceType(SourceType.Positional, volume, rolloff_mode, min_distance, max_distance, false);
		this.audio_source.Play();
	}

	// Token: 0x06001652 RID: 5714 RVA: 0x0009EF28 File Offset: 0x0009D128
	public void PlayLooping(AudioClip clip, float volume, SoundType soundType)
	{
		this.soundType = soundType;
		this.originalVolume = volume;
		this.audio_source.enabled = true;
		this.audio_source.clip = clip;
		this.audio_source.pitch = 1f;
		this.SetSourceType(SourceType.NonPositional, 0f, AudioRolloffMode.Linear, 1f, 1f, true);
		this.audio_source.Play();
	}

	// Token: 0x06001653 RID: 5715 RVA: 0x00010DBC File Offset: 0x0000EFBC
	public void FadeAudio(float fade_time, FadeType type)
	{
		base.StartCoroutine(this.InternalFadeAudio(fade_time, type));
	}

	// Token: 0x06001654 RID: 5716 RVA: 0x0009EF90 File Offset: 0x0009D190
	public void SetSourceType(SourceType type, float volume, AudioRolloffMode rolloff_mode = AudioRolloffMode.Linear, float min_distance = 10f, float max_distance = 30f, bool loop = false)
	{
		this.audio_source.volume = AudioSystem.GetVolume(this.soundType, this.originalVolume);
		if (type == SourceType.NonPositional)
		{
			this.audio_source.bypassEffects = true;
			this.audio_source.bypassListenerEffects = true;
			this.audio_source.bypassReverbZones = true;
			this.audio_source.rolloffMode = rolloff_mode;
			this.audio_source.minDistance = min_distance;
			this.audio_source.maxDistance = max_distance;
			this.audio_source.loop = loop;
			this.audio_source.spatialBlend = 0f;
			return;
		}
		if (type != SourceType.Positional)
		{
			return;
		}
		this.audio_source.bypassEffects = false;
		this.audio_source.bypassListenerEffects = false;
		this.audio_source.bypassReverbZones = false;
		this.audio_source.rolloffMode = rolloff_mode;
		this.audio_source.minDistance = min_distance;
		this.audio_source.maxDistance = max_distance;
		this.audio_source.loop = loop;
		this.audio_source.spatialBlend = 1f;
	}

	// Token: 0x06001655 RID: 5717 RVA: 0x00010DCD File Offset: 0x0000EFCD
	private IEnumerator InternalFadeAudio(float timer, FadeType fadeType)
	{
		float start = (fadeType == FadeType.In) ? 0f : 1f;
		float end = (fadeType == FadeType.In) ? 1f : 0f;
		this.i = 0f;
		while (this.i < timer)
		{
			this.i += Time.deltaTime;
			this.audio_source.volume = Mathf.Lerp(start, end, this.i / timer) * AudioSystem.GetVolume(this.soundType, this.originalVolume);
			yield return null;
		}
		if (fadeType == FadeType.Out)
		{
			if (!this.pooled)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				this.audio_source.volume = 0f;
				this.audio_source.Stop();
			}
		}
		else
		{
			this.audio_source.volume = AudioSystem.GetVolume(this.soundType, this.originalVolume);
		}
		yield break;
	}

	// Token: 0x06001656 RID: 5718 RVA: 0x00010DEA File Offset: 0x0000EFEA
	public void StopAudio()
	{
		this.audio_source.Stop();
	}

	// Token: 0x04001788 RID: 6024
	public AudioSource audio_source;

	// Token: 0x04001789 RID: 6025
	private SoundType soundType;

	// Token: 0x0400178A RID: 6026
	private float originalVolume;

	// Token: 0x0400178B RID: 6027
	public bool pooled;

	// Token: 0x0400178C RID: 6028
	private float i;
}
