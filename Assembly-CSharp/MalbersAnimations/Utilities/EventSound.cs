using System;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x02000791 RID: 1937
	[Serializable]
	public class EventSound
	{
		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06003725 RID: 14117 RVA: 0x000258A4 File Offset: 0x00023AA4
		// (set) Token: 0x06003724 RID: 14116 RVA: 0x0002589B File Offset: 0x00023A9B
		public float VolumeWeight
		{
			get
			{
				return this.volumeWeight;
			}
			set
			{
				this.volumeWeight = value;
			}
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x00118388 File Offset: 0x00116588
		public void PlayAudio(AudioSource audio)
		{
			if (audio == null)
			{
				return;
			}
			if (this.Clips == null || this.Clips.Length == 0)
			{
				return;
			}
			audio.spatialBlend = 1f;
			audio.clip = this.Clips[UnityEngine.Random.Range(0, this.Clips.Length)];
			audio.pitch *= this.pitch;
			audio.volume = Mathf.Clamp01(this.volume * this.VolumeWeight);
			audio.Play();
		}

		// Token: 0x04003644 RID: 13892
		public string name = "Name Here";

		// Token: 0x04003645 RID: 13893
		public AudioClip[] Clips;

		// Token: 0x04003646 RID: 13894
		public float volume = 1f;

		// Token: 0x04003647 RID: 13895
		public float pitch = 1f;

		// Token: 0x04003648 RID: 13896
		protected float volumeWeight = 1f;
	}
}
