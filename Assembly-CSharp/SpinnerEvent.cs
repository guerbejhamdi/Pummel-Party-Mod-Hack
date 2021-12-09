using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x0200017B RID: 379
public class SpinnerEvent : BulletDodgeEvent
{
	// Token: 0x06000ADA RID: 2778 RVA: 0x0005E6F4 File Offset: 0x0005C8F4
	public override void Start()
	{
		this.shooter = base.transform.Find("Shooter").gameObject;
		this.rotator = base.transform.Find("Shooter/Rotator").gameObject;
		this.shooter.SetActive(false);
		System.Random random = new System.Random((int)(this.net_time * 1000f));
		this.shooter_start_position.x = this.spinner_min_x + (float)random.NextDouble() * (this.spinner_max_x - this.spinner_min_x);
		base.Start();
		if (this.minigame_controller.CurrentPhase == 3)
		{
			this.spinner_phase_end_rotation = 150f;
			this.fire_rate = 0.1f;
			this.shooter_speed = 8f;
		}
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x0005E7B4 File Offset: 0x0005C9B4
	private void Update()
	{
		if (this.cur_state == SpinnerEvent.SpinnerEventState.Waiting)
		{
			if (NetSystem.NetTime.GameTime - this.net_time > this.phase_offset_time)
			{
				this.cur_state = SpinnerEvent.SpinnerEventState.MovingShooter;
				this.shooter.transform.position = this.shooter_start_position;
				this.shooter.SetActive(true);
			}
		}
		else if (this.cur_state == SpinnerEvent.SpinnerEventState.MovingShooter)
		{
			if (this.shooter.transform.position.y < this.shooter_stop_y)
			{
				this.shooter.transform.position = new Vector3(this.shooter.transform.position.x, this.shooter_stop_y, this.shooter.transform.position.z);
				this.cur_state = SpinnerEvent.SpinnerEventState.ShootingBullets;
			}
			else
			{
				this.shooter.transform.position -= new Vector3(0f, this.shooter_speed * Time.deltaTime, 0f);
			}
		}
		else if (this.cur_state == SpinnerEvent.SpinnerEventState.ShootingBullets)
		{
			this.rotator.transform.Rotate(new Vector3(0f, 0f, 1f), this.rotate_speed * Time.deltaTime);
			this.rotation_amount += this.rotate_speed * Time.deltaTime;
			if (Time.time - this.last_bullet_fire > this.fire_rate)
			{
				this.last_bullet_fire = Time.time;
				SpinnerEvent.Bullet bullet = default(SpinnerEvent.Bullet);
				bullet.direction = this.rotator.transform.rotation * Vector3.left;
				bullet.bullet = this.minigame_controller.Spawn(this.bullet, this.rotator.transform.position + bullet.direction * 2f, Quaternion.identity);
				bullet.bullet.name = "Bullet";
				bullet.bullet.GetComponent<BulletDodgeProjectile>().type = BulletDodgeBulletType.Spinner;
				RaycastHit raycastHit;
				if (Physics.SphereCast(bullet.bullet.transform.position, 0.75f, bullet.direction, out raycastHit, 100f, 1024))
				{
					bullet.bullet.GetComponent<BulletDodgeProjectile>().spinnerHitPoint = raycastHit.point;
				}
				this.bullets.Add(bullet);
				bullet = default(SpinnerEvent.Bullet);
				bullet.direction = -(this.rotator.transform.rotation * Vector3.left);
				bullet.bullet = this.minigame_controller.Spawn(this.bullet, this.rotator.transform.position + bullet.direction * 2f, Quaternion.identity);
				bullet.bullet.name = "Bullet";
				bullet.bullet.GetComponent<BulletDodgeProjectile>().type = BulletDodgeBulletType.Spinner;
				if (Physics.SphereCast(bullet.bullet.transform.position, 0.75f, bullet.direction, out raycastHit, 100f, 1024))
				{
					bullet.bullet.GetComponent<BulletDodgeProjectile>().spinnerHitPoint = raycastHit.point;
				}
				this.bullets.Add(bullet);
			}
			if (!this.phase_ended && this.rotation_amount > this.spinner_phase_end_rotation)
			{
				this.phase_ended = true;
				base.FinishPhase();
			}
			else if (this.rotation_amount > this.spinner_retract_rotation)
			{
				this.rotator.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
				this.cur_state = SpinnerEvent.SpinnerEventState.Returning;
			}
		}
		else if (this.cur_state == SpinnerEvent.SpinnerEventState.Returning)
		{
			if (this.bullets.Count == 0)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				this.shooter.transform.position += new Vector3(0f, this.shooter_speed * Time.deltaTime, 0f);
			}
		}
		int i = 0;
		while (i < this.bullets.Count)
		{
			if (!this.IsVisibleFrom(this.bullets[i].bullet.GetComponentInChildren<ParticleSystemRenderer>(), GameManager.Minigame.MinigameCamera))
			{
				UnityEngine.Object.Destroy(this.bullets[i].bullet);
				this.bullets.RemoveAt(i);
			}
			else
			{
				this.bullets[i].bullet.transform.position += this.bullets[i].direction * this.bullet_speed * Time.deltaTime;
				i++;
			}
		}
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x0000AFE1 File Offset: 0x000091E1
	public bool IsVisibleFrom(Renderer renderer, Camera camera)
	{
		return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), renderer.bounds);
	}

	// Token: 0x040009E3 RID: 2531
	public GameObject bullet;

	// Token: 0x040009E4 RID: 2532
	private float rotate_speed = 90f;

	// Token: 0x040009E5 RID: 2533
	private float shooter_speed = 12f;

	// Token: 0x040009E6 RID: 2534
	private float bullet_speed = 20f;

	// Token: 0x040009E7 RID: 2535
	private List<SpinnerEvent.Bullet> bullets = new List<SpinnerEvent.Bullet>();

	// Token: 0x040009E8 RID: 2536
	private GameObject shooter;

	// Token: 0x040009E9 RID: 2537
	private Vector3 shooter_start_position = new Vector3(0f, 21f, 0f);

	// Token: 0x040009EA RID: 2538
	private float shooter_stop_y = 7f;

	// Token: 0x040009EB RID: 2539
	private GameObject rotator;

	// Token: 0x040009EC RID: 2540
	private SpinnerEvent.SpinnerEventState cur_state;

	// Token: 0x040009ED RID: 2541
	private float fire_rate = 0.125f;

	// Token: 0x040009EE RID: 2542
	private float last_bullet_fire;

	// Token: 0x040009EF RID: 2543
	private float rotation_amount;

	// Token: 0x040009F0 RID: 2544
	private float spinner_min_x = -7f;

	// Token: 0x040009F1 RID: 2545
	private float spinner_max_x = 7f;

	// Token: 0x040009F2 RID: 2546
	private float spinner_phase_end_rotation = 250f;

	// Token: 0x040009F3 RID: 2547
	private float spinner_retract_rotation = 360f;

	// Token: 0x0200017C RID: 380
	private enum SpinnerEventState
	{
		// Token: 0x040009F5 RID: 2549
		Waiting,
		// Token: 0x040009F6 RID: 2550
		MovingShooter,
		// Token: 0x040009F7 RID: 2551
		ShootingBullets,
		// Token: 0x040009F8 RID: 2552
		Returning,
		// Token: 0x040009F9 RID: 2553
		Finished
	}

	// Token: 0x0200017D RID: 381
	private struct Bullet
	{
		// Token: 0x040009FA RID: 2554
		public GameObject bullet;

		// Token: 0x040009FB RID: 2555
		public Vector3 direction;
	}
}
