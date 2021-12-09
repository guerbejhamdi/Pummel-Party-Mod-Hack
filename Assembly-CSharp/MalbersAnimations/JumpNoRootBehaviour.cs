using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000712 RID: 1810
	public class JumpNoRootBehaviour : StateMachineBehaviour
	{
		// Token: 0x06003515 RID: 13589 RVA: 0x00112538 File Offset: 0x00110738
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animal = animator.GetComponent<Animal>();
			this.rb = animator.GetComponent<Rigidbody>();
			this.animal.InAir(true);
			this.animal.SetIntID(0);
			this.animal.OnJump.Invoke();
			this.animal.RootMotion = false;
			Vector3 force = Vector3.up * this.JumpMultiplier * this.animal.JumpHeightMultiplier + animator.transform.forward * this.ForwardMultiplier * this.animal.AirForwardMultiplier;
			this.rb.AddForce(force, ForceMode.VelocityChange);
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x001125EC File Offset: 0x001107EC
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animal.SetIntID(0);
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
			if (this.rb && currentAnimatorStateInfo.tagHash == AnimTag.Fly)
			{
				Vector3 velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
				this.rb.velocity = velocity;
			}
			if (currentAnimatorStateInfo.tagHash != AnimTag.Fall && currentAnimatorStateInfo.tagHash != AnimTag.Fly)
			{
				this.animal.IsInAir = false;
			}
		}

		// Token: 0x04003412 RID: 13330
		public float JumpMultiplier = 1f;

		// Token: 0x04003413 RID: 13331
		public float ForwardMultiplier;

		// Token: 0x04003414 RID: 13332
		private Animal animal;

		// Token: 0x04003415 RID: 13333
		private Rigidbody rb;
	}
}
