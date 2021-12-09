using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000719 RID: 1817
	public class RandomBehavior : StateMachineBehaviour
	{
		// Token: 0x06003526 RID: 13606 RVA: 0x00112C0C File Offset: 0x00110E0C
		public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
		{
			int num = UnityEngine.Random.Range(1, this.Range + 1);
			animator.SetInteger(this.Parameter, num);
			Animal component = animator.GetComponent<Animal>();
			if (component && this.Parameter == "IDInt")
			{
				component.SetIntID(num);
			}
		}

		// Token: 0x04003431 RID: 13361
		public string Parameter = "IDInt";

		// Token: 0x04003432 RID: 13362
		public int Range;
	}
}
