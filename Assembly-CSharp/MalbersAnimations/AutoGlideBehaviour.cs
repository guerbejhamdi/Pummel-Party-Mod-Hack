using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000709 RID: 1801
	public class AutoGlideBehaviour : StateMachineBehaviour
	{
		// Token: 0x060034EF RID: 13551 RVA: 0x00110530 File Offset: 0x0010E730
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animal = animator.GetComponent<Animal>();
			this.Default_UseShift = this.animal.UseShift;
			this.animal.UseShift = false;
			this.FlyStyleTime = this.GlideChance.RandomValue;
			this.currentTime = Time.time;
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x00110584 File Offset: 0x0010E784
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!this.animal.Fly)
			{
				return;
			}
			if (Time.time - this.FlyStyleTime >= this.currentTime)
			{
				this.currentTime = Time.time;
				this.isGliding = !this.isGliding;
				this.FlyStyleTime = (this.isGliding ? this.GlideChance.RandomValue : this.FlapChange.RandomValue);
				this.animal.GroundSpeed = (this.isGliding ? 2f : UnityEngine.Random.Range(1f, 1.5f));
			}
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x00023F8C File Offset: 0x0002218C
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animal.UseShift = this.Default_UseShift;
		}

		// Token: 0x040033A9 RID: 13225
		[MinMaxRange(0f, 10f)]
		public RangedFloat GlideChance = new RangedFloat(0.8f, 4f);

		// Token: 0x040033AA RID: 13226
		[MinMaxRange(0f, 10f)]
		public RangedFloat FlapChange = new RangedFloat(0.5f, 4f);

		// Token: 0x040033AB RID: 13227
		protected bool isGliding;

		// Token: 0x040033AC RID: 13228
		protected float FlyStyleTime = 1f;

		// Token: 0x040033AD RID: 13229
		protected float currentTime = 1f;

		// Token: 0x040033AE RID: 13230
		protected bool Default_UseShift;

		// Token: 0x040033AF RID: 13231
		protected Animal animal;
	}
}
