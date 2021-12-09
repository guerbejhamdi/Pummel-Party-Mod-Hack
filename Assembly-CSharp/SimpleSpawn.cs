using System;
using UnityEngine;

// Token: 0x020004A1 RID: 1185
public class SimpleSpawn : MonoBehaviour
{
	// Token: 0x06001FA9 RID: 8105 RVA: 0x0001734B File Offset: 0x0001554B
	private void Start()
	{
		this.animator = base.GetComponent<Animator>();
	}

	// Token: 0x06001FAA RID: 8106 RVA: 0x00017359 File Offset: 0x00015559
	public void Despawn()
	{
		this.animator.SetTrigger("Despawn");
	}

	// Token: 0x06001FAB RID: 8107 RVA: 0x0001248D File Offset: 0x0001068D
	public void AnimationFinished()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400228E RID: 8846
	private Animator animator;
}
