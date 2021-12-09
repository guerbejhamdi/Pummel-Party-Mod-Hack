using System;
using UnityEngine;

// Token: 0x02000571 RID: 1393
public class MusicPlayer : MonoBehaviour
{
	// Token: 0x06002492 RID: 9362 RVA: 0x0001A46F File Offset: 0x0001866F
	public void Start()
	{
		AudioSystem.PlayMusic(this.clip, this.fade_time, 1f);
	}

	// Token: 0x040027D3 RID: 10195
	public AudioClip clip;

	// Token: 0x040027D4 RID: 10196
	public float fade_time = 1.5f;
}
