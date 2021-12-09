using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200070E RID: 1806
	public class FlyDodgeBehaviour : StateMachineBehaviour
	{
		// Token: 0x06003502 RID: 13570 RVA: 0x00024002 File Offset: 0x00022202
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.rb = animator.GetComponent<Rigidbody>();
			this.animal = animator.GetComponent<Animal>();
			this.momentum = (this.InPlace ? this.rb.velocity : animator.velocity);
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x0011106C File Offset: 0x0010F26C
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.time = ((animator.updateMode == AnimatorUpdateMode.AnimatePhysics) ? Time.fixedDeltaTime : Time.deltaTime);
			this.multiplier = 1f;
			if (animator.IsInTransition(layerIndex))
			{
				this.transition = animator.GetAnimatorTransitionInfo(layerIndex);
				this.multiplier = ((stateInfo.normalizedTime <= 0.5f) ? this.transition.normalizedTime : (1f - this.transition.normalizedTime));
			}
			this.animal.DeltaPosition += this.momentum * this.time * this.multiplier;
			this.animal.DeltaPosition += this.animal.transform.TransformDirection(this.DodgeDirection) * this.time * this.multiplier;
		}

		// Token: 0x040033D5 RID: 13269
		public bool InPlace;

		// Token: 0x040033D6 RID: 13270
		public Vector3 DodgeDirection = Vector3.zero;

		// Token: 0x040033D7 RID: 13271
		private Vector3 momentum;

		// Token: 0x040033D8 RID: 13272
		private Rigidbody rb;

		// Token: 0x040033D9 RID: 13273
		private Animal animal;

		// Token: 0x040033DA RID: 13274
		private float time;

		// Token: 0x040033DB RID: 13275
		private float multiplier;

		// Token: 0x040033DC RID: 13276
		private AnimatorTransitionInfo transition;
	}
}
