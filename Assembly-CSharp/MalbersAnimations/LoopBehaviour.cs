using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000715 RID: 1813
	public class LoopBehaviour : StateMachineBehaviour
	{
		// Token: 0x0600351C RID: 13596 RVA: 0x00112848 File Offset: 0x00110A48
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animal = animator.GetComponent<Animal>();
			if (this.animal == null)
			{
				return;
			}
			if (!this.hasEntered)
			{
				this.hasEntered = true;
				this.CurrentLoop = 1;
				this.animal.SetIntID(0);
			}
			else
			{
				this.CurrentLoop++;
			}
			if (this.CurrentLoop >= this.animal.Loops)
			{
				this.hasEntered = false;
				this.animal.SetIntID(this.IntIDExitValue);
			}
		}

		// Token: 0x0400341D RID: 13341
		[Header("This behaviour requires a transition to itself")]
		[Header("With the contidion 'IntID' != -1")]
		public int IntIDExitValue = -1;

		// Token: 0x0400341E RID: 13342
		[Header("")]
		protected int CurrentLoop;

		// Token: 0x0400341F RID: 13343
		protected int loop;

		// Token: 0x04003420 RID: 13344
		private bool hasEntered;

		// Token: 0x04003421 RID: 13345
		private Animal animal;
	}
}
