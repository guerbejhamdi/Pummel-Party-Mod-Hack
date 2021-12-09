using System;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x0200078F RID: 1935
	public class AnimatorEventSounds : MonoBehaviour
	{
		// Token: 0x0600371F RID: 14111 RVA: 0x00025846 File Offset: 0x00023A46
		private void Start()
		{
			this.anim = base.GetComponent<Animator>();
			if (this._audioSource == null)
			{
				this._audioSource = base.gameObject.AddComponent<AudioSource>();
			}
			this._audioSource.volume = 0f;
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x001182B4 File Offset: 0x001164B4
		public virtual void PlaySound(AnimationEvent e)
		{
			if ((double)e.animatorClipInfo.weight < 0.1)
			{
				return;
			}
			EventSound eventSound = this.m_EventSound.Find((EventSound item) => item.name == e.stringParameter);
			if (eventSound != null)
			{
				eventSound.VolumeWeight = e.animatorClipInfo.weight;
				if (this.anim)
				{
					this._audioSource.pitch = this.anim.speed;
				}
				if (this._audioSource.isPlaying)
				{
					if (eventSound.VolumeWeight * eventSound.volume > this._audioSource.volume)
					{
						eventSound.PlayAudio(this._audioSource);
						return;
					}
				}
				else
				{
					eventSound.PlayAudio(this._audioSource);
				}
			}
		}

		// Token: 0x04003640 RID: 13888
		public List<EventSound> m_EventSound;

		// Token: 0x04003641 RID: 13889
		public AudioSource _audioSource;

		// Token: 0x04003642 RID: 13890
		protected Animator anim;
	}
}
