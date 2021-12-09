using System;
using UnityEngine;

// Token: 0x02000405 RID: 1029
public class RagdollForce
{
	// Token: 0x06001CBD RID: 7357 RVA: 0x0001533B File Offset: 0x0001353B
	public RagdollForce(Vector3 _direction, float _start_time, float _apply_time)
	{
		this.direction = _direction;
		this.startTime = _start_time;
		this.applyTime = _apply_time;
		this.last_update_time = this.startTime;
	}

	// Token: 0x04001F29 RID: 7977
	public Vector3 direction;

	// Token: 0x04001F2A RID: 7978
	public float startTime;

	// Token: 0x04001F2B RID: 7979
	public float applyTime;

	// Token: 0x04001F2C RID: 7980
	public float last_update_time;
}
