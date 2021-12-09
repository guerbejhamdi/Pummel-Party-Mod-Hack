using System;
using UnityEngine;

// Token: 0x02000128 RID: 296
public class NoCollidersBehavior : StateMachineBehaviour
{
	// Token: 0x060008B1 RID: 2225 RVA: 0x000505A4 File Offset: 0x0004E7A4
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this.cap = animator.GetComponentsInChildren<Collider>();
		if (this.enter)
		{
			Collider[] array = this.cap;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
		}
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x000505E4 File Offset: 0x0004E7E4
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.exit)
		{
			Collider[] array = this.cap;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = true;
			}
		}
	}

	// Token: 0x04000717 RID: 1815
	[Header("Deactivate Colliders on Enter")]
	public bool enter = true;

	// Token: 0x04000718 RID: 1816
	[Header("Activate Colliders on Exit")]
	public bool exit = true;

	// Token: 0x04000719 RID: 1817
	private Collider[] cap;
}
