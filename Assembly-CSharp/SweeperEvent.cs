using System;
using UnityEngine;
using ZP.Net;

// Token: 0x0200017E RID: 382
public class SweeperEvent : BulletDodgeEvent
{
	// Token: 0x06000ADE RID: 2782 RVA: 0x0000AFF4 File Offset: 0x000091F4
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x0005ED14 File Offset: 0x0005CF14
	private void Update()
	{
		if (NetSystem.NetTime.GameTime - this.net_time > this.phase_offset_time)
		{
			Vector3 vector = this.dir * this.speed * Time.deltaTime;
			base.transform.position += vector;
			this.moved_x_distance += Mathf.Abs(vector.x);
		}
		if (!this.phase_ended && this.moved_x_distance > this.phase_end_distance)
		{
			this.phase_ended = true;
			base.FinishPhase();
		}
		if (this.moved_x_distance > this.max_move_distance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040009FC RID: 2556
	public float speed = 5f;

	// Token: 0x040009FD RID: 2557
	public Vector3 dir = new Vector3(1f, 0f, 0f);

	// Token: 0x040009FE RID: 2558
	private float phase_end_distance = 45f;

	// Token: 0x040009FF RID: 2559
	private float max_move_distance = 68f;

	// Token: 0x04000A00 RID: 2560
	private float moved_x_distance;
}
