using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000331 RID: 817
public class AudioSystemObj : MonoBehaviour
{
	// Token: 0x0600163F RID: 5695 RVA: 0x0009EAC8 File Offset: 0x0009CCC8
	private void Awake()
	{
		if (this.dont_destroy_on_load)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		AudioSystem.Initialize(this);
		this.temp_audio_source_prefab = Resources.Load<GameObject>("AudioSystem/TempAudioSourceNew");
		for (int i = 0; i < this.CachedClips.Count; i++)
		{
			AudioSystem.LoadClip(this.CachedClips[i]);
		}
		this.audio_sources = new List<TempAudioSource>();
		if (this.PoolAudioSources)
		{
			for (int j = 0; j < this.DefaultSourceCount; j++)
			{
				TempAudioSource tempAudioSource = this.CreateNewSource(Vector3.zero, true);
				tempAudioSource.SetSourceType(SourceType.NonPositional, 1f, AudioRolloffMode.Linear, 10f, 30f, false);
				this.audio_sources.Add(tempAudioSource);
			}
		}
		Debug.Log("AudioSystem Awake");
		this.current_music = this.CreateNewSource(Vector3.zero, false);
		base.transform.position = new Vector3(0f, 0f, 0f);
	}

	// Token: 0x06001640 RID: 5696 RVA: 0x00010CB9 File Offset: 0x0000EEB9
	private void Update()
	{
		AudioSystem.Update();
	}

	// Token: 0x06001641 RID: 5697 RVA: 0x0009EBB8 File Offset: 0x0009CDB8
	public void UpdateVolumes()
	{
		for (int i = 0; i < this.audio_sources.Count; i++)
		{
			this.audio_sources[i].UpdateVolume();
		}
		this.current_music.UpdateVolume();
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x0009EBF8 File Offset: 0x0009CDF8
	public void PlayOneShot(AudioClip clip, float volume, float pitch)
	{
		TempAudioSource freeSource = this.GetFreeSource(Vector3.zero);
		freeSource.PlayOneShot(clip, volume, pitch);
		if (!this.PoolAudioSources)
		{
			UnityEngine.Object.Destroy(freeSource.gameObject, clip.length + 1f);
		}
	}

	// Token: 0x06001643 RID: 5699 RVA: 0x0009EC3C File Offset: 0x0009CE3C
	public void PlayOneShot(AudioClip clip, Vector3 position, float volume, AudioRolloffMode rolloff_mode, float min_distance, float max_distance)
	{
		TempAudioSource freeSource = this.GetFreeSource(position);
		freeSource.PlayOneShot(clip, position, volume, rolloff_mode, min_distance, max_distance);
		if (!this.PoolAudioSources)
		{
			UnityEngine.Object.Destroy(freeSource.gameObject, clip.length + 1f);
		}
	}

	// Token: 0x06001644 RID: 5700 RVA: 0x0009EC80 File Offset: 0x0009CE80
	public void PlayMusic(AudioClip clip, float fade_time, float volume)
	{
		if (this.current_music != null)
		{
			this.current_music.FadeAudio(fade_time, FadeType.Out);
		}
		this.current_music = this.CreateNewSource(Vector3.zero, false);
		this.current_music.PlayLooping(clip, volume, SoundType.Music);
		this.current_music.FadeAudio(fade_time, FadeType.In);
	}

	// Token: 0x06001645 RID: 5701 RVA: 0x00010CC0 File Offset: 0x0000EEC0
	public void PlayAmbience(AudioClip clip, float fadeTime, float volume)
	{
		TempAudioSource freeSource = this.GetFreeSource(Vector3.zero);
		freeSource.PlayLooping(clip, volume, SoundType.Ambience);
		freeSource.FadeAudio(fadeTime, FadeType.In);
	}

	// Token: 0x06001646 RID: 5702 RVA: 0x00010CDD File Offset: 0x0000EEDD
	public TempAudioSource PlayLooping(AudioClip clip, float fadeTime, float volume)
	{
		TempAudioSource freeSource = this.GetFreeSource(Vector3.zero);
		freeSource.PlayLooping(clip, volume, SoundType.Looping);
		freeSource.FadeAudio(fadeTime, FadeType.In);
		return freeSource;
	}

	// Token: 0x06001647 RID: 5703 RVA: 0x00010CFB File Offset: 0x0000EEFB
	private TempAudioSource CreateNewSource(Vector3 position, bool pooled)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.temp_audio_source_prefab, position, Quaternion.identity);
		gameObject.transform.parent = base.transform;
		TempAudioSource component = gameObject.GetComponent<TempAudioSource>();
		component.Initialize(position, pooled);
		return component;
	}

	// Token: 0x06001648 RID: 5704 RVA: 0x0009ECD8 File Offset: 0x0009CED8
	private TempAudioSource GetFreeSource(Vector3 position)
	{
		if (!this.PoolAudioSources)
		{
			return this.CreateNewSource(position, false);
		}
		int i = 0;
		while (i < this.audio_sources.Count)
		{
			if (this.audio_sources[i].audio_source == null)
			{
				UnityEngine.Object.Destroy(this.audio_sources[i]);
				this.audio_sources.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
		for (int j = 0; j < this.audio_sources.Count; j++)
		{
			if (!this.audio_sources[j].IsPlaying())
			{
				return this.audio_sources[j];
			}
		}
		if (this.audio_sources.Count < this.MaxSourceCount)
		{
			Debug.LogWarning("All audio sources in use, creating new source");
			TempAudioSource tempAudioSource = this.CreateNewSource(position, false);
			tempAudioSource.SetSourceType(SourceType.NonPositional, 1f, AudioRolloffMode.Linear, 10f, 30f, false);
			this.audio_sources.Add(tempAudioSource);
			return tempAudioSource;
		}
		Debug.LogWarning("All audio sources in use, and max sources reached, stopping random source.");
		int index = UnityEngine.Random.Range(0, this.audio_sources.Count - 1);
		return this.audio_sources[index];
	}

	// Token: 0x06001649 RID: 5705 RVA: 0x0009EDF0 File Offset: 0x0009CFF0
	public void StopPooledSounds(float fadeTime)
	{
		for (int i = 0; i < this.audio_sources.Count; i++)
		{
			if (this.audio_sources[i].pooled)
			{
				if (fadeTime == 0f)
				{
					this.audio_sources[i].StopAudio();
				}
				else
				{
					this.audio_sources[i].FadeAudio(fadeTime, FadeType.Out);
				}
			}
		}
	}

	// Token: 0x04001775 RID: 6005
	public bool dont_destroy_on_load = true;

	// Token: 0x04001776 RID: 6006
	public bool PoolAudioSources = true;

	// Token: 0x04001777 RID: 6007
	public int DefaultSourceCount = 8;

	// Token: 0x04001778 RID: 6008
	public int MaxSourceCount = 32;

	// Token: 0x04001779 RID: 6009
	public List<AudioClip> CachedClips;

	// Token: 0x0400177A RID: 6010
	private GameObject temp_audio_source_prefab;

	// Token: 0x0400177B RID: 6011
	private List<TempAudioSource> audio_sources;

	// Token: 0x0400177C RID: 6012
	private TempAudioSource current_music;
}
