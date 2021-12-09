using System;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x02000795 RID: 1941
	public class BlinkEyes : MonoBehaviour, IAnimatorListener
	{
		// Token: 0x06003744 RID: 14148 RVA: 0x000259BD File Offset: 0x00023BBD
		public virtual void Eyes(int ID)
		{
			if (this.animator)
			{
				this.animator.SetInteger(this.parameter, ID);
			}
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x0002339D File Offset: 0x0002159D
		public virtual void OnAnimatorBehaviourMessage(string message, object value)
		{
			this.InvokeWithParams(message, value);
		}

		// Token: 0x04003653 RID: 13907
		public Animator animator;

		// Token: 0x04003654 RID: 13908
		public string parameter;
	}
}
