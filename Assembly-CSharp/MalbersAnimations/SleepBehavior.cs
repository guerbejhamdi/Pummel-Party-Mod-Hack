using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200071C RID: 1820
	public class SleepBehavior : StateMachineBehaviour
	{
		// Token: 0x06003530 RID: 13616 RVA: 0x00112DC4 File Offset: 0x00110FC4
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.animal == null)
			{
				this.animal = animator.GetComponent<Animal>();
			}
			if (!this.animal)
			{
				return;
			}
			if (this.animal.GotoSleep == 0)
			{
				return;
			}
			if (animator.GetCurrentAnimatorStateInfo(layerIndex).tagHash == AnimTag.Idle)
			{
				Animal animal = this.animal;
				int tired = animal.Tired;
				animal.Tired = tired + 1;
				if (this.animal.Tired >= this.animal.GotoSleep)
				{
					this.animal.SetIntID(this.transitionID);
					this.animal.Tired = 0;
					return;
				}
			}
			else
			{
				this.CyclesToSleep();
			}
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x00112E70 File Offset: 0x00111070
		private void CyclesToSleep()
		{
			if (this.CyclesFromController)
			{
				this.Cycles = this.animal.GotoSleep;
				if (this.Cycles == 0)
				{
					return;
				}
			}
			this.currentCycle++;
			if (this.currentCycle >= this.Cycles)
			{
				this.animal.SetIntID(this.transitionID);
				this.currentCycle = 0;
			}
		}

		// Token: 0x04003447 RID: 13383
		public bool CyclesFromController;

		// Token: 0x04003448 RID: 13384
		public int Cycles;

		// Token: 0x04003449 RID: 13385
		public int transitionID;

		// Token: 0x0400344A RID: 13386
		private int currentCycle;

		// Token: 0x0400344B RID: 13387
		private Animal animal;
	}
}
