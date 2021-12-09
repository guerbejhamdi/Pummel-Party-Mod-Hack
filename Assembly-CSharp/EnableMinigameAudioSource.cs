using System;
using UnityEngine;

// Token: 0x02000455 RID: 1109
public class EnableMinigameAudioSource : MonoBehaviour
{
	// Token: 0x06001E69 RID: 7785 RVA: 0x000C4210 File Offset: 0x000C2410
	private void Update()
	{
		if (GameManager.Minigame != null && GameManager.Minigame.AllClientsReady())
		{
			AudioSource component = base.GetComponent<AudioSource>();
			component.volume = AudioSystem.GetVolume(SoundType.Effect, component.volume);
			component.enabled = true;
			UnityEngine.Object.Destroy(this);
		}
	}
}
