using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000887 RID: 2183
	public abstract class Modifier : MonoBehaviour
	{
		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06003E37 RID: 15927 RVA: 0x00029DAF File Offset: 0x00027FAF
		// (set) Token: 0x06003E38 RID: 15928 RVA: 0x00029DB7 File Offset: 0x00027FB7
		public Frequency Frequency
		{
			get
			{
				return this.frequency;
			}
			set
			{
				if (value != this.frequency)
				{
					this.Deregister();
					this.frequency = value;
					this.Register();
				}
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06003E39 RID: 15929 RVA: 0x0013301C File Offset: 0x0013121C
		protected float UpdateRate
		{
			get
			{
				Frequency frequency = this.frequency;
				if (frequency == Frequency.TenPerSec)
				{
					return 0.1f;
				}
				if (frequency != Frequency.OncePerSec)
				{
					return Time.deltaTime;
				}
				return 1f;
			}
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x00029DD5 File Offset: 0x00027FD5
		protected virtual void OnEnable()
		{
			this.Begin();
			this.Register();
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x00029DE3 File Offset: 0x00027FE3
		protected virtual void OnDisable()
		{
			this.Deregister();
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x00029DEB File Offset: 0x00027FEB
		private void Register()
		{
			if (Application.isPlaying && base.gameObject.activeInHierarchy)
			{
				ModifierManager.Register(this);
			}
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x00029E07 File Offset: 0x00028007
		private void Deregister()
		{
			if (Application.isPlaying)
			{
				ModifierManager.Deregister(this);
			}
		}

		// Token: 0x06003E3E RID: 15934
		protected abstract void Begin();

		// Token: 0x06003E3F RID: 15935
		public abstract void Perform();

		// Token: 0x04003A72 RID: 14962
		[SerializeField]
		private Frequency frequency;
	}
}
