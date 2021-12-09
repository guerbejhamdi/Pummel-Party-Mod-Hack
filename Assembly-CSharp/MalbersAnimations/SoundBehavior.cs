using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200071D RID: 1821
	public class SoundBehavior : StateMachineBehaviour
	{
		// Token: 0x06003533 RID: 13619 RVA: 0x00112ED4 File Offset: 0x001110D4
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this._audio = animator.GetComponent<AudioSource>();
			if (!this._audio)
			{
				this._audio = animator.gameObject.AddComponent<AudioSource>();
			}
			this._audio.spatialBlend = 1f;
			if (this.playOnEnter && this._audio)
			{
				this.PlaySound();
			}
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x00112F38 File Offset: 0x00111138
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.playOnTime && this._audio && stateInfo.normalizedTime > this.NormalizedTime && !this._audio.isPlaying && !animator.IsInTransition(layerIndex))
			{
				this.PlaySound();
			}
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x0000398C File Offset: 0x00001B8C
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x00112F88 File Offset: 0x00111188
		public virtual void PlaySound()
		{
			if (this._audio && this.sounds.Length != 0 && this._audio.enabled)
			{
				this._audio.clip = this.sounds[UnityEngine.Random.Range(0, this.sounds.Length)];
				this._audio.pitch = this.pitch;
				this._audio.volume = this.volume;
				this._audio.Play();
			}
		}

		// Token: 0x0400344C RID: 13388
		public AudioClip[] sounds;

		// Token: 0x0400344D RID: 13389
		public bool playOnEnter = true;

		// Token: 0x0400344E RID: 13390
		public bool playOnTime;

		// Token: 0x0400344F RID: 13391
		[Range(0f, 1f)]
		public float NormalizedTime = 0.5f;

		// Token: 0x04003450 RID: 13392
		[Space]
		[Range(-0.5f, 3f)]
		public float pitch = 1f;

		// Token: 0x04003451 RID: 13393
		[Range(0f, 1f)]
		public float volume = 1f;

		// Token: 0x04003452 RID: 13394
		private AudioSource _audio;
	}
}
