using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200071A RID: 1818
	public class RecoverBehavior : StateMachineBehaviour
	{
		// Token: 0x06003528 RID: 13608 RVA: 0x00112C60 File Offset: 0x00110E60
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animal = animator.GetComponent<Animal>();
			this.rb = animator.GetComponent<Rigidbody>();
			this.animal.RootMotion = false;
			if (this.RigidY)
			{
				this.rb.constraints = Animal.Still_Constraints;
			}
			this.rb.drag = 0f;
			if (this.Landing)
			{
				this.animal.IsInAir = false;
				return;
			}
			this.rb.useGravity = false;
		}

		// Token: 0x06003529 RID: 13609 RVA: 0x000240BB File Offset: 0x000222BB
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.rb.drag = Mathf.Lerp(this.rb.drag, this.MaxDrag, Time.deltaTime * this.smoothness);
		}

		// Token: 0x0600352A RID: 13610 RVA: 0x000240EA File Offset: 0x000222EA
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.rb.drag = 0f;
		}

		// Token: 0x04003433 RID: 13363
		public float smoothness = 10f;

		// Token: 0x04003434 RID: 13364
		public float MaxDrag = 3f;

		// Token: 0x04003435 RID: 13365
		public bool stillContraints = true;

		// Token: 0x04003436 RID: 13366
		public bool Landing = true;

		// Token: 0x04003437 RID: 13367
		public bool RigidY = true;

		// Token: 0x04003438 RID: 13368
		private Animal animal;

		// Token: 0x04003439 RID: 13369
		private Rigidbody rb;

		// Token: 0x0400343A RID: 13370
		private float deltatime;
	}
}
