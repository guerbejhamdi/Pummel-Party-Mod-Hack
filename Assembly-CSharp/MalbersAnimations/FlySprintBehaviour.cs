using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200070F RID: 1807
	public class FlySprintBehaviour : StateMachineBehaviour
	{
		// Token: 0x06003505 RID: 13573 RVA: 0x0011115C File Offset: 0x0010F35C
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animal = animator.GetComponent<Animal>();
			this.BehaviourSpeed = this.animal.flySpeed;
			this.Shift = 0f;
			if (this.Speed_Param != string.Empty)
			{
				this.SpeedHash = Animator.StringToHash(this.Speed_Param);
				animator.SetFloat(this.SpeedHash, this.AnimSpeedDefault);
			}
			this.CurrentSpeedMultiplier = this.AnimSpeedDefault;
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x001111D4 File Offset: 0x0010F3D4
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.animal.MovementReleased)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			this.Shift = Mathf.Lerp(this.Shift, this.animal.Shift ? this.ShiftMultiplier : 1f, this.BehaviourSpeed.lerpPosition * deltaTime);
			this.CurrentSpeedMultiplier = Mathf.Lerp(this.CurrentSpeedMultiplier, (this.animal.Shift && this.animal.MovementForward > 0f) ? this.AnimSprintSpeed : this.AnimSpeedDefault, deltaTime * this.AnimSprintLerp);
			if (this.Speed_Param != string.Empty)
			{
				this.animal.Anim.SetFloat(this.SpeedHash, this.CurrentSpeedMultiplier);
			}
			if (this.IsRootMotion)
			{
				this.animal.DeltaPosition += animator.velocity * this.Shift * deltaTime;
			}
			else
			{
				this.animal.DeltaPosition += this.animal.T_Forward * this.Shift * Mathf.Clamp(this.animal.Speed, 0f, 1f) * deltaTime;
			}
			if (this.animal.Shift && this.NoGliding)
			{
				this.animal.Speed = Mathf.Lerp(this.animal.Speed, 1f, deltaTime * 6f);
			}
		}

		// Token: 0x040033DD RID: 13277
		public bool IsRootMotion;

		// Token: 0x040033DE RID: 13278
		[Tooltip("Float Parameter on the Animator to Modify When Sprint is Enabled, if this value is null it will not change the multiplier")]
		public string Speed_Param = "SpeedMultiplier";

		// Token: 0x040033DF RID: 13279
		public float ShiftMultiplier = 2f;

		// Token: 0x040033E0 RID: 13280
		public float AnimSpeedDefault = 1f;

		// Token: 0x040033E1 RID: 13281
		[Tooltip("Amount of Speed Multiplier  to use on the Speed Multiplier Animator parameter when 'UseSprint' is Enabled\n if this value is null it will not change the multiplier")]
		public float AnimSprintSpeed = 2f;

		// Token: 0x040033E2 RID: 13282
		[Tooltip("Smoothness to use when the SpeedMultiplier changes")]
		public float AnimSprintLerp = 2f;

		// Token: 0x040033E3 RID: 13283
		[Tooltip("Do not Glide while pressing shift")]
		public bool NoGliding = true;

		// Token: 0x040033E4 RID: 13284
		protected int SpeedHash = Animator.StringToHash("SpeedMultiplier");

		// Token: 0x040033E5 RID: 13285
		protected float CurrentSpeedMultiplier;

		// Token: 0x040033E6 RID: 13286
		protected float Shift;

		// Token: 0x040033E7 RID: 13287
		protected Animal animal;

		// Token: 0x040033E8 RID: 13288
		protected Speeds BehaviourSpeed;
	}
}
