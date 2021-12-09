using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000708 RID: 1800
	public class AttackBehaviour : StateMachineBehaviour
	{
		// Token: 0x060034EB RID: 13547 RVA: 0x001103E0 File Offset: 0x0010E5E0
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animal = animator.GetComponent<Animal>();
			this.animal.IsAttacking = true;
			this.animal.Attack1 = false;
			this.isOn = (this.isOff = false);
			this.attackDelay = this.animal.attackDelay;
			this.startAttackTime = Time.time;
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x00110440 File Offset: 0x0010E640
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animal.IsAttacking = true;
			if (!this.isOn && stateInfo.normalizedTime % 1f >= this.AttackActivation.minValue)
			{
				this.animal.AttackTrigger(this.AttackTrigger);
				this.isOn = true;
			}
			if (!this.isOff && stateInfo.normalizedTime % 1f >= this.AttackActivation.maxValue)
			{
				this.animal.AttackTrigger(0);
				this.isOff = true;
			}
			if (this.attackDelay > 0f && Time.time - this.startAttackTime >= this.attackDelay)
			{
				this.animal.IsAttacking = false;
			}
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x001104F8 File Offset: 0x0010E6F8
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animal.AttackTrigger(0);
			this.isOn = (this.isOff = false);
			this.animal.IsAttacking = false;
		}

		// Token: 0x040033A2 RID: 13218
		public int AttackTrigger = 1;

		// Token: 0x040033A3 RID: 13219
		[Tooltip("Range on the Animation that the Attack Trigger will be Active")]
		[MinMaxRange(0f, 1f)]
		public RangedFloat AttackActivation = new RangedFloat(0.3f, 0.6f);

		// Token: 0x040033A4 RID: 13220
		private bool isOn;

		// Token: 0x040033A5 RID: 13221
		private bool isOff;

		// Token: 0x040033A6 RID: 13222
		private Animal animal;

		// Token: 0x040033A7 RID: 13223
		private float startAttackTime;

		// Token: 0x040033A8 RID: 13224
		private float attackDelay;
	}
}
