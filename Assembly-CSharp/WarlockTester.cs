using System;
using UnityEngine;

// Token: 0x02000591 RID: 1425
public class WarlockTester : MonoBehaviour
{
	// Token: 0x0600250C RID: 9484 RVA: 0x0001AA15 File Offset: 0x00018C15
	private void Start()
	{
		this.animator = base.GetComponentInChildren<Animator>();
	}

	// Token: 0x0600250D RID: 9485 RVA: 0x0001AA23 File Offset: 0x00018C23
	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			this.animator.SetTrigger("Fall");
		}
		if (Input.GetMouseButtonDown(0))
		{
			this.animator.SetTrigger("ThrowObject");
		}
	}

	// Token: 0x04002895 RID: 10389
	private Animator animator;
}
