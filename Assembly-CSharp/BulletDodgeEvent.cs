using System;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class BulletDodgeEvent : MonoBehaviour
{
	// Token: 0x06000AB0 RID: 2736 RVA: 0x0000AE4E File Offset: 0x0000904E
	public virtual void Start()
	{
		base.transform.position = this.start_position;
		base.transform.rotation = Quaternion.Euler(this.start_rotation);
		this.minigame_controller = (BulletDodgeController)GameManager.Minigame;
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x0000AE87 File Offset: 0x00009087
	protected void FinishPhase()
	{
		this.minigame_controller.event_active = false;
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x0000AE95 File Offset: 0x00009095
	// (set) Token: 0x06000AB3 RID: 2739 RVA: 0x0000AE9D File Offset: 0x0000909D
	public float NetTime
	{
		get
		{
			return this.net_time;
		}
		set
		{
			this.net_time = value;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x0000AEA6 File Offset: 0x000090A6
	public bool PhaseEnded
	{
		get
		{
			return this.phase_ended;
		}
	}

	// Token: 0x04000993 RID: 2451
	public float phase_offset_time = 0.5f;

	// Token: 0x04000994 RID: 2452
	public Vector3 start_position = Vector3.zero;

	// Token: 0x04000995 RID: 2453
	public Vector3 start_rotation = Vector3.zero;

	// Token: 0x04000996 RID: 2454
	protected float net_time;

	// Token: 0x04000997 RID: 2455
	protected BulletDodgeController minigame_controller;

	// Token: 0x04000998 RID: 2456
	protected bool phase_ended;
}
