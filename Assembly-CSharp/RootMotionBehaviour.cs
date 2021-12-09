using System;
using UnityEngine;

// Token: 0x02000129 RID: 297
public class RootMotionBehaviour : StateMachineBehaviour
{
	// Token: 0x060008B4 RID: 2228 RVA: 0x00009E11 File Offset: 0x00008011
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.OnEnter)
		{
			animator.applyRootMotion = this.RootMotionOnEnter;
		}
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x00009E27 File Offset: 0x00008027
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.OnExit)
		{
			animator.applyRootMotion = this.RootMotionOnExit;
		}
	}

	// Token: 0x0400071A RID: 1818
	public bool OnEnter;

	// Token: 0x0400071B RID: 1819
	public bool RootMotionOnEnter;

	// Token: 0x0400071C RID: 1820
	[Space]
	public bool OnExit;

	// Token: 0x0400071D RID: 1821
	public bool RootMotionOnExit;
}
