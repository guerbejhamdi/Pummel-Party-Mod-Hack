using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200071B RID: 1819
	public class RigidConstraintsB : StateMachineBehaviour
	{
		// Token: 0x0600352C RID: 13612 RVA: 0x00112CDC File Offset: 0x00110EDC
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.Amount = 0;
			this.rb = animator.GetComponent<Rigidbody>();
			if (this.PosX)
			{
				this.Amount += 2;
			}
			if (this.PosY)
			{
				this.Amount += 4;
			}
			if (this.PosZ)
			{
				this.Amount += 8;
			}
			if (this.RotX)
			{
				this.Amount += 16;
			}
			if (this.RotY)
			{
				this.Amount += 32;
			}
			if (this.RotZ)
			{
				this.Amount += 64;
			}
			if (this.OnEnter && this.rb)
			{
				this.rb.constraints = (RigidbodyConstraints)this.Amount;
			}
			this.ExitTime = false;
			this.rb.drag = this.OnEnterDrag;
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x0002412F File Offset: 0x0002232F
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!this.ExitTime && this.OnExit && stateInfo.normalizedTime > 1f)
			{
				this.rb.constraints = (RigidbodyConstraints)this.Amount;
				this.ExitTime = true;
			}
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x00024167 File Offset: 0x00022367
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.OnExit && this.rb)
			{
				this.rb.constraints = (RigidbodyConstraints)this.Amount;
			}
		}

		// Token: 0x0400343B RID: 13371
		public bool PosX;

		// Token: 0x0400343C RID: 13372
		public bool PosY = true;

		// Token: 0x0400343D RID: 13373
		public bool PosZ;

		// Token: 0x0400343E RID: 13374
		public bool RotX = true;

		// Token: 0x0400343F RID: 13375
		public bool RotY = true;

		// Token: 0x04003440 RID: 13376
		public bool RotZ = true;

		// Token: 0x04003441 RID: 13377
		public bool OnEnter = true;

		// Token: 0x04003442 RID: 13378
		public bool OnExit;

		// Token: 0x04003443 RID: 13379
		protected int Amount;

		// Token: 0x04003444 RID: 13380
		private Rigidbody rb;

		// Token: 0x04003445 RID: 13381
		private bool ExitTime;

		// Token: 0x04003446 RID: 13382
		public float OnEnterDrag;
	}
}
