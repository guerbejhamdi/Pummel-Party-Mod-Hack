using System;
using UnityEngine;

// Token: 0x020004D2 RID: 1234
public class TestAnimationBehaviour : StateMachineBehaviour
{
	// Token: 0x060020B1 RID: 8369 RVA: 0x000CCB2C File Offset: 0x000CAD2C
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
	}

	// Token: 0x060020B2 RID: 8370 RVA: 0x000CCB44 File Offset: 0x000CAD44
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);
	}

	// Token: 0x0400237B RID: 9083
	private MainMenuWindow m;
}
