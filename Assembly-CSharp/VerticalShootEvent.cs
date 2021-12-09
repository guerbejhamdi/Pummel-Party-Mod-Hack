using System;
using UnityEngine;
using ZP.Net;

// Token: 0x0200017F RID: 383
public class VerticalShootEvent : BulletDodgeEvent
{
	// Token: 0x06000AE1 RID: 2785 RVA: 0x0005EE14 File Offset: 0x0005D014
	public override void Start()
	{
		this.shooter = base.transform.Find("Shooter").gameObject;
		this.bullets = base.transform.Find("Bullets").gameObject;
		System.Random random = new System.Random((int)(this.net_time * 1000f));
		this.shooter_start_position.x = this.rand_min_x + (float)random.NextDouble() * (this.rand_max_x - this.rand_min_x);
		this.bullets_start_position.x = this.shooter_start_position.x;
		this.shooter.SetActive(false);
		this.bullets.SetActive(false);
		base.Start();
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x0005EEC8 File Offset: 0x0005D0C8
	private void Update()
	{
		if (this.cur_state == VerticalShootEvent.ShooterEventState.Waiting)
		{
			if (NetSystem.NetTime.GameTime - this.net_time > this.phase_offset_time)
			{
				this.cur_state = VerticalShootEvent.ShooterEventState.MovingShooter;
				this.shooter.transform.position = this.shooter_start_position;
				this.shooter.SetActive(true);
				return;
			}
		}
		else if (this.cur_state == VerticalShootEvent.ShooterEventState.MovingShooter)
		{
			if (this.shooter.transform.position.y < this.shooter_stop_y)
			{
				this.bullets.transform.position = this.bullets_start_position;
				this.bullets.SetActive(true);
				this.cur_state = VerticalShootEvent.ShooterEventState.MovingBullets;
				return;
			}
			this.shooter.transform.position -= new Vector3(0f, this.shooter_speed * Time.deltaTime, 0f);
			return;
		}
		else if (this.cur_state == VerticalShootEvent.ShooterEventState.MovingBullets)
		{
			this.shooter.transform.position += new Vector3(0f, this.shooter_speed * Time.deltaTime, 0f);
			this.bullets.transform.position -= new Vector3(0f, this.bullets_speed * Time.deltaTime, 0f);
			if (!this.phase_ended && this.bullets.transform.position.y > this.phase_end_bullet_y)
			{
				this.phase_ended = true;
				base.FinishPhase();
			}
			if (this.bullets.transform.position.y < this.finish_bullet_y)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x04000A01 RID: 2561
	private GameObject shooter;

	// Token: 0x04000A02 RID: 2562
	private GameObject bullets;

	// Token: 0x04000A03 RID: 2563
	private VerticalShootEvent.ShooterEventState cur_state;

	// Token: 0x04000A04 RID: 2564
	private Vector3 shooter_start_position = new Vector3(0f, 27.5f, 0f);

	// Token: 0x04000A05 RID: 2565
	private float shooter_speed = 10f;

	// Token: 0x04000A06 RID: 2566
	private float shooter_stop_y = 21.5f;

	// Token: 0x04000A07 RID: 2567
	private Vector3 bullets_start_position = new Vector3(0f, 15.6f, 0f);

	// Token: 0x04000A08 RID: 2568
	private float bullets_speed = 20f;

	// Token: 0x04000A09 RID: 2569
	private float phase_end_bullet_y = 14f;

	// Token: 0x04000A0A RID: 2570
	private float finish_bullet_y = -17f;

	// Token: 0x04000A0B RID: 2571
	private float rand_min_x = -3f;

	// Token: 0x04000A0C RID: 2572
	private float rand_max_x = 3f;

	// Token: 0x02000180 RID: 384
	private enum ShooterEventState
	{
		// Token: 0x04000A0E RID: 2574
		Waiting,
		// Token: 0x04000A0F RID: 2575
		MovingShooter,
		// Token: 0x04000A10 RID: 2576
		MovingBullets
	}
}
