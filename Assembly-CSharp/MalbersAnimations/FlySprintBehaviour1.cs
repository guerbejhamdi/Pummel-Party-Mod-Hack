using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200070A RID: 1802
	public class FlySprintBehaviour1 : StateMachineBehaviour
	{
		// Token: 0x060034F3 RID: 13555 RVA: 0x00110670 File Offset: 0x0010E870
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animal = animator.GetComponent<Animal>();
			this.BehaviourSpeed = this.animal.flySpeed;
			if (this.Speed_Param != string.Empty)
			{
				this.SpeedHash = Animator.StringToHash(this.Speed_Param);
				animator.SetFloat(this.SpeedHash, this.AnimSpeedDefault);
			}
			this.CurrentSpeedMultiplier = this.AnimSpeedDefault;
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x001106DC File Offset: 0x0010E8DC
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			float deltaTime = Time.deltaTime;
			this.Shift = Mathf.Lerp(this.Shift, this.animal.Shift ? this.ShiftMultiplier : 1f, this.BehaviourSpeed.lerpPosition * deltaTime);
			this.CurrentSpeedMultiplier = Mathf.Lerp(this.CurrentSpeedMultiplier, this.animal.Shift ? this.AnimSprintSpeed : this.AnimSpeedDefault, deltaTime * this.AnimSprintLerp);
			if (this.Speed_Param != string.Empty)
			{
				this.animal.Anim.SetFloat(this.SpeedHash, this.CurrentSpeedMultiplier);
			}
			this.animal.DeltaPosition += this.animal.T_Forward * this.Shift * deltaTime;
		}

		// Token: 0x040033B0 RID: 13232
		public bool UseSprint = true;

		// Token: 0x040033B1 RID: 13233
		[Tooltip("Float Parameter on the Animator to Modify When Sprint is Enabled, if this value is null it will not change the multiplier")]
		public string Speed_Param = "SpeedMultiplier";

		// Token: 0x040033B2 RID: 13234
		public float ShiftMultiplier = 2f;

		// Token: 0x040033B3 RID: 13235
		public float AnimSpeedDefault = 1f;

		// Token: 0x040033B4 RID: 13236
		[Tooltip("Amount of Speed Multiplier  to use on the Speed Multiplier Animator parameter when 'UseSprint' is Enabled\n if this value is null it will not change the multiplier")]
		public float AnimSprintSpeed = 2f;

		// Token: 0x040033B5 RID: 13237
		[Tooltip("Smoothness to use when the SpeedMultiplier changes")]
		public float AnimSprintLerp = 2f;

		// Token: 0x040033B6 RID: 13238
		protected int SpeedHash = Animator.StringToHash("SpeedMultiplier");

		// Token: 0x040033B7 RID: 13239
		protected float CurrentSpeedMultiplier;

		// Token: 0x040033B8 RID: 13240
		protected float Shift;

		// Token: 0x040033B9 RID: 13241
		protected Animal animal;

		// Token: 0x040033BA RID: 13242
		protected Speeds BehaviourSpeed;
	}
}
