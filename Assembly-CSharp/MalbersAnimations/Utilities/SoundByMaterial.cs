using System;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x020007AE RID: 1966
	public class SoundByMaterial : MonoBehaviour
	{
		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x060037E0 RID: 14304 RVA: 0x00026085 File Offset: 0x00024285
		// (set) Token: 0x060037E1 RID: 14305 RVA: 0x000260A6 File Offset: 0x000242A6
		protected AudioSource Audio_Source
		{
			get
			{
				if (!this.audioSource)
				{
					this.audioSource = base.GetComponent<AudioSource>();
				}
				return this.audioSource;
			}
			set
			{
				this.audioSource = value;
			}
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x0011A690 File Offset: 0x00118890
		public virtual void PlayMaterialSound(RaycastHit hitSurface)
		{
			Collider collider = hitSurface.collider;
			if (collider)
			{
				this.PlayMaterialSound(collider.sharedMaterial);
			}
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x0011A6BC File Offset: 0x001188BC
		public virtual void PlayMaterialSound(GameObject hitSurface)
		{
			Collider component = hitSurface.GetComponent<Collider>();
			if (component)
			{
				this.PlayMaterialSound(component.sharedMaterial);
			}
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x000260AF File Offset: 0x000242AF
		public virtual void PlayMaterialSound(Collider hitSurface)
		{
			this.PlayMaterialSound(hitSurface.sharedMaterial);
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x0011A6E4 File Offset: 0x001188E4
		public virtual void PlayMaterialSound(PhysicMaterial hitSurface)
		{
			if (!this.Audio_Source)
			{
				this.Audio_Source = base.gameObject.AddComponent<AudioSource>();
				this.Audio_Source.spatialBlend = 1f;
			}
			MaterialSound materialSound = this.materialSounds.Find((MaterialSound item) => item.material == hitSurface);
			if (materialSound != null)
			{
				AudioClip clip = materialSound.Sounds[UnityEngine.Random.Range(0, materialSound.Sounds.Length)];
				this.Audio_Source.clip = clip;
				this.audioSource.Play();
				return;
			}
			if (this.DefaultSound)
			{
				this.Audio_Source.clip = this.DefaultSound;
				this.audioSource.Play();
			}
		}

		// Token: 0x040036C7 RID: 14023
		public AudioClip DefaultSound;

		// Token: 0x040036C8 RID: 14024
		public List<MaterialSound> materialSounds;

		// Token: 0x040036C9 RID: 14025
		private AudioSource audioSource;
	}
}
