using System;
using UnityEngine;

// Token: 0x0200022B RID: 555
public class RunnerMovement : MonoBehaviour
{
	// Token: 0x06001026 RID: 4134 RVA: 0x0007F294 File Offset: 0x0007D494
	private void Update()
	{
		if (this.m_movementStareted)
		{
			base.transform.position += this.m_moveSpeed * Time.deltaTime;
			this.m_moveSpeed += this.m_moveSpeedIncrease * (Time.deltaTime / 90f);
		}
	}

	// Token: 0x06001027 RID: 4135 RVA: 0x0000DACA File Offset: 0x0000BCCA
	public void StartMovement()
	{
		this.m_movementStareted = true;
	}

	// Token: 0x0400106A RID: 4202
	[SerializeField]
	protected Vector3 m_moveSpeed;

	// Token: 0x0400106B RID: 4203
	[SerializeField]
	protected Vector3 m_moveSpeedIncrease;

	// Token: 0x0400106C RID: 4204
	protected bool m_movementStareted;
}
