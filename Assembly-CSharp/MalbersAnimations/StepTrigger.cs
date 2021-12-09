using System;
using System.Collections;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000758 RID: 1880
	public class StepTrigger : MonoBehaviour
	{
		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06003649 RID: 13897 RVA: 0x00024D2F File Offset: 0x00022F2F
		// (set) Token: 0x0600364A RID: 13898 RVA: 0x00024D37 File Offset: 0x00022F37
		public bool HasTrack
		{
			get
			{
				return this.hastrack;
			}
			set
			{
				this.hastrack = value;
			}
		}

		// Token: 0x0600364B RID: 13899 RVA: 0x00116A2C File Offset: 0x00114C2C
		private void Awake()
		{
			this._StepsManager = base.GetComponentInParent<StepsManager>();
			if (this._StepsManager == null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (!this._StepsManager.Active)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.StepAudio = base.GetComponent<AudioSource>();
			if (this.StepAudio == null)
			{
				this.StepAudio = base.gameObject.AddComponent<AudioSource>();
			}
			this.StepAudio.spatialBlend = 1f;
			if (this._StepsManager)
			{
				this.StepAudio.volume = this._StepsManager.StepsVolume;
			}
			this.wait = new WaitForSeconds(this.WaitNextStep);
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x00024D40 File Offset: 0x00022F40
		private void OnTriggerEnter(Collider other)
		{
			if (!this.waitrack && this._StepsManager)
			{
				base.StartCoroutine(this.WaitForStep());
				this._StepsManager.EnterStep(this);
				this.hastrack = true;
			}
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x00024D77 File Offset: 0x00022F77
		private void OnTriggerExit(Collider other)
		{
			this.hastrack = false;
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x00024D80 File Offset: 0x00022F80
		private IEnumerator WaitForStep()
		{
			this.waitrack = true;
			yield return this.wait;
			this.waitrack = false;
			yield break;
		}

		// Token: 0x04003590 RID: 13712
		private StepsManager _StepsManager;

		// Token: 0x04003591 RID: 13713
		public float WaitNextStep = 0.2f;

		// Token: 0x04003592 RID: 13714
		[HideInInspector]
		public AudioSource StepAudio;

		// Token: 0x04003593 RID: 13715
		private WaitForSeconds wait;

		// Token: 0x04003594 RID: 13716
		private bool hastrack;

		// Token: 0x04003595 RID: 13717
		private bool waitrack;
	}
}
