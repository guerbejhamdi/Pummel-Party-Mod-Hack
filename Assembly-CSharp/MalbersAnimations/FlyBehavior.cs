using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200070D RID: 1805
	public class FlyBehavior : StateMachineBehaviour
	{
		// Token: 0x060034FF RID: 13567 RVA: 0x00110DFC File Offset: 0x0010EFFC
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.rb = animator.GetComponent<Rigidbody>();
			this.animal = animator.GetComponent<Animal>();
			this.acceleration = 0f;
			this.animal.IsInAir = true;
			this.animal.RootMotion = true;
			this.FallVector = ((this.animal.CurrentAnimState == AnimTag.Fall) ? this.rb.velocity : Vector3.zero);
			this.rb.constraints = RigidbodyConstraints.FreezeRotation;
			this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
			this.rb.useGravity = false;
			this.rb.drag = this.Drag;
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x00110ED0 File Offset: 0x0010F0D0
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.deltaTime = Time.deltaTime;
			if (this.FallVector != Vector3.zero)
			{
				this.animal.DeltaPosition += this.FallVector * this.deltaTime;
				this.FallVector = Vector3.Lerp(this.FallVector, Vector3.zero, this.deltaTime * this.FallRecovery);
			}
			if ((double)this.animal.MovementAxis.y < -0.1)
			{
				this.acceleration = Mathf.Lerp(this.acceleration, this.acceleration + this.DownAcceleration, this.deltaTime);
			}
			else if ((double)this.animal.MovementAxis.y > -0.1 || this.animal.MovementReleased)
			{
				float num = this.acceleration - this.DownInertia;
				if (num < 0f)
				{
					num = 0f;
				}
				this.acceleration = Mathf.Lerp(this.acceleration, num, this.deltaTime * 2f);
			}
			this.animal.DeltaPosition += animator.velocity * (this.acceleration / 2f) * this.deltaTime;
			if (this.animal.LockUp)
			{
				this.animal.DeltaPosition += Physics.gravity * this.LockUpDownForce * this.deltaTime * this.deltaTime;
			}
		}

		// Token: 0x040033CB RID: 13259
		public float Drag = 5f;

		// Token: 0x040033CC RID: 13260
		public float DownAcceleration = 4f;

		// Token: 0x040033CD RID: 13261
		[Tooltip("If is changing from ")]
		public float DownInertia = 2f;

		// Token: 0x040033CE RID: 13262
		[Tooltip("If is changing from fall to fly this will smoothly ")]
		public float FallRecovery = 1.5f;

		// Token: 0x040033CF RID: 13263
		[Tooltip("If Lock up is Enabled this apply to the dragon an extra Down Force")]
		public float LockUpDownForce = 4f;

		// Token: 0x040033D0 RID: 13264
		private float acceleration;

		// Token: 0x040033D1 RID: 13265
		private Rigidbody rb;

		// Token: 0x040033D2 RID: 13266
		private Animal animal;

		// Token: 0x040033D3 RID: 13267
		private float deltaTime;

		// Token: 0x040033D4 RID: 13268
		private Vector3 FallVector;
	}
}
