using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000330 RID: 816
public class AudioSystem
{
	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x06001629 RID: 5673 RVA: 0x00010B49 File Offset: 0x0000ED49
	// (set) Token: 0x0600162A RID: 5674 RVA: 0x00010B50 File Offset: 0x0000ED50
	public static float MasterVolume
	{
		get
		{
			return AudioSystem.masterVolume;
		}
		set
		{
			AudioSystem.masterVolume = value;
			AudioSystem.audio_system_obj.UpdateVolumes();
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x0600162B RID: 5675 RVA: 0x00010B62 File Offset: 0x0000ED62
	// (set) Token: 0x0600162C RID: 5676 RVA: 0x00010B69 File Offset: 0x0000ED69
	public static float MusicVolume
	{
		get
		{
			return AudioSystem.musicVolume;
		}
		set
		{
			AudioSystem.musicVolume = value;
			AudioSystem.audio_system_obj.UpdateVolumes();
		}
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x0600162D RID: 5677 RVA: 0x00010B7B File Offset: 0x0000ED7B
	// (set) Token: 0x0600162E RID: 5678 RVA: 0x00010B82 File Offset: 0x0000ED82
	public static float EffectsVolume
	{
		get
		{
			return AudioSystem.effectsVolume;
		}
		set
		{
			AudioSystem.effectsVolume = value;
			AudioSystem.audio_system_obj.UpdateVolumes();
		}
	}

	// Token: 0x0600162F RID: 5679 RVA: 0x00010B94 File Offset: 0x0000ED94
	public static float GetVolume(SoundType soundType, float originalVolume)
	{
		if (soundType == SoundType.Music)
		{
			return originalVolume * (AudioSystem.MasterVolume * AudioSystem.MusicVolume);
		}
		return originalVolume * (AudioSystem.MasterVolume * AudioSystem.EffectsVolume);
	}

	// Token: 0x06001630 RID: 5680 RVA: 0x00010BB4 File Offset: 0x0000EDB4
	public static void Initialize(AudioSystemObj _audio_system_obj)
	{
		AudioSystem.audio_system_obj = _audio_system_obj;
		if (!AudioSystem.initialized)
		{
			AudioSystem.cached_clips = new Dictionary<string, AudioClip>();
			AudioSystem.delayed_clips = new Dictionary<string, DelayedClip>();
		}
		AudioSystem.cached_clips.Clear();
	}

	// Token: 0x06001631 RID: 5681 RVA: 0x0009E8E4 File Offset: 0x0009CAE4
	public static void Update()
	{
		if (AudioSystem.delayed_clips.Count > 0)
		{
			List<AudioClip> list = new List<AudioClip>();
			foreach (KeyValuePair<string, DelayedClip> keyValuePair in AudioSystem.delayed_clips)
			{
				keyValuePair.Value.time_counter += Time.deltaTime;
				if (keyValuePair.Value.time_counter >= keyValuePair.Value.delay_time)
				{
					list.Add(keyValuePair.Value.clip);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				AudioSystem.delayed_clips.Remove(list[i].name);
			}
		}
	}

	// Token: 0x06001632 RID: 5682 RVA: 0x00010BE1 File Offset: 0x0000EDE1
	public static void LoadClip(AudioClip clip)
	{
		if (clip == null)
		{
			return;
		}
		if (!AudioSystem.cached_clips.ContainsKey(clip.name))
		{
			AudioSystem.cached_clips.Add(clip.name, clip);
		}
	}

	// Token: 0x06001633 RID: 5683 RVA: 0x00010C10 File Offset: 0x0000EE10
	public static AudioClip GetClip(string name)
	{
		return AudioSystem.GetAudioClip(name);
	}

	// Token: 0x06001634 RID: 5684 RVA: 0x0009E9B4 File Offset: 0x0009CBB4
	public static void PlayOneShot(AudioClip clip, float volume, float delay_next_time = 0f, float pitch = 1f)
	{
		if (clip == null)
		{
			return;
		}
		if (delay_next_time > 0f)
		{
			if (AudioSystem.delayed_clips.ContainsKey(clip.name))
			{
				return;
			}
			AudioSystem.delayed_clips.Add(clip.name, new DelayedClip(clip, delay_next_time));
		}
		AudioSystem.audio_system_obj.PlayOneShot(clip, volume, pitch);
	}

	// Token: 0x06001635 RID: 5685 RVA: 0x0009EA0C File Offset: 0x0009CC0C
	public static void PlayOneShot(AudioClip clip, Vector3 position, float volume = 0.5f, AudioRolloffMode rolloff_mode = AudioRolloffMode.Linear, float min_distance = 10f, float max_distance = 30f, float delay_next_time = 0f)
	{
		if (clip == null)
		{
			return;
		}
		if (delay_next_time > 0f)
		{
			if (AudioSystem.delayed_clips.ContainsKey(clip.name))
			{
				return;
			}
			AudioSystem.delayed_clips.Add(clip.name, new DelayedClip(clip, delay_next_time));
		}
		if (AudioSystem.audio_system_obj != null)
		{
			AudioSystem.audio_system_obj.PlayOneShot(clip, position, volume, rolloff_mode, min_distance, max_distance);
		}
	}

	// Token: 0x06001636 RID: 5686 RVA: 0x00010C18 File Offset: 0x0000EE18
	public static void PlayOneShot(string name, float volume, float delay_next_time = 0f)
	{
		AudioSystem.PlayOneShot(AudioSystem.GetAudioClip(name), volume, delay_next_time, 1f);
	}

	// Token: 0x06001637 RID: 5687 RVA: 0x00010C2C File Offset: 0x0000EE2C
	public static void PlayOneShot(string name, Vector3 position, float volume = 0.5f, AudioRolloffMode rolloff_mode = AudioRolloffMode.Linear, float min_distance = 10f, float max_distance = 30f, float delay_next_time = 0f)
	{
		AudioSystem.PlayOneShot(AudioSystem.GetAudioClip(name), position, volume, rolloff_mode, min_distance, max_distance, delay_next_time);
	}

	// Token: 0x06001638 RID: 5688 RVA: 0x00010C42 File Offset: 0x0000EE42
	public static void PlayMusic(AudioClip clip, float fade_time, float volume = 1f)
	{
		AudioSystem.audio_system_obj.PlayMusic(clip, fade_time, volume);
	}

	// Token: 0x06001639 RID: 5689 RVA: 0x00010C51 File Offset: 0x0000EE51
	public static void PlayAmbient(AudioClip clip, float fadeTime, float volume = 1f)
	{
		AudioSystem.audio_system_obj.PlayAmbience(clip, fadeTime, volume);
	}

	// Token: 0x0600163A RID: 5690 RVA: 0x00010C60 File Offset: 0x0000EE60
	public static TempAudioSource PlayLooping(AudioClip clip, float fadeTime, float volume = 1f)
	{
		return AudioSystem.audio_system_obj.PlayLooping(clip, fadeTime, volume);
	}

	// Token: 0x0600163B RID: 5691 RVA: 0x0009EA78 File Offset: 0x0009CC78
	private static AudioClip GetAudioClip(string name)
	{
		AudioClip audioClip = null;
		if (!AudioSystem.cached_clips.TryGetValue(name, out audioClip))
		{
			audioClip = Resources.Load<AudioClip>(AudioSystem.audio_location + name);
			if (audioClip == null)
			{
				Debug.LogWarning("Unable to play audio clip [" + name + "] clip was not loaded in cache, and does not exist in resource folder.");
				return null;
			}
		}
		return audioClip;
	}

	// Token: 0x0600163C RID: 5692 RVA: 0x00010C6F File Offset: 0x0000EE6F
	public static void StopPooledSounds(float fadeTime)
	{
		if (AudioSystem.audio_system_obj != null)
		{
			AudioSystem.audio_system_obj.StopPooledSounds(fadeTime);
		}
	}

	// Token: 0x0400176D RID: 5997
	private static bool initialized = false;

	// Token: 0x0400176E RID: 5998
	private static AudioSystemObj audio_system_obj;

	// Token: 0x0400176F RID: 5999
	private static string audio_location = "Audio/";

	// Token: 0x04001770 RID: 6000
	private static Dictionary<string, AudioClip> cached_clips;

	// Token: 0x04001771 RID: 6001
	private static Dictionary<string, DelayedClip> delayed_clips;

	// Token: 0x04001772 RID: 6002
	private static float masterVolume = 1f;

	// Token: 0x04001773 RID: 6003
	private static float musicVolume = 1f;

	// Token: 0x04001774 RID: 6004
	private static float effectsVolume = 1f;
}
