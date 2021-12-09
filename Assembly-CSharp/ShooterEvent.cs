using System;
using UnityEngine;
using ZP.Net;

// Token: 0x02000179 RID: 377
public class ShooterEvent : BulletDodgeEvent
{
	// Token: 0x06000AD7 RID: 2775 RVA: 0x0005E2B8 File Offset: 0x0005C4B8
	public override void Start()
	{
		this.shooter = base.transform.Find("Shooter").gameObject;
		this.bullet_spawns = base.transform.Find("Bullets").gameObject;
		if (!this.right)
		{
			this.shooter_start_position.x = -this.shooter_start_position.x;
			this.shooter_speed = -this.shooter_speed;
			this.shooter_stop_x = -this.shooter_stop_x;
			this.bullets_start_position.x = -this.bullets_start_position.x;
			this.bullets_speed = -this.bullets_speed;
			this.phase_end_bullet_x = -this.phase_end_bullet_x;
			this.finish_bullet_x = -this.finish_bullet_x;
		}
		this.shooter.SetActive(false);
		this.bullet_spawns.SetActive(false);
		base.Start();
		if (this.minigame_controller.CurrentPhase == 3)
		{
			this.phase_end_bullet_x = (this.right ? 20f : -20f);
			this.bullets_speed = (this.right ? 25f : -25f);
		}
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x0005E3D4 File Offset: 0x0005C5D4
	private void Update()
	{
		if (this.cur_state == ShooterEvent.ShooterEventState.Waiting)
		{
			if (NetSystem.NetTime.GameTime - this.net_time > this.phase_offset_time)
			{
				this.cur_state = ShooterEvent.ShooterEventState.MovingShooter;
				this.shooter.transform.position = this.shooter_start_position;
				this.shooter.SetActive(true);
				return;
			}
		}
		else if (this.cur_state == ShooterEvent.ShooterEventState.MovingShooter)
		{
			if (!(this.right ? (this.shooter.transform.position.x < this.shooter_stop_x) : (this.shooter.transform.position.x > this.shooter_stop_x)))
			{
				this.shooter.transform.position -= new Vector3(this.shooter_speed * Time.deltaTime, 0f, 0f);
				return;
			}
			this.bullet_spawns.transform.position = this.bullets_start_position;
			this.bullet_spawns.SetActive(true);
			if (this.projectile_prefab != null)
			{
				for (int i = 0; i < this.bullet_spawns.transform.childCount; i++)
				{
					GameObject gameObject = this.minigame_controller.Spawn(this.projectile_prefab, this.bullet_spawns.transform.GetChild(i).transform.position, Quaternion.identity);
					gameObject.name = "Bullet";
					BulletDodgeProjectile component = gameObject.GetComponent<BulletDodgeProjectile>();
					component.Launch(new Vector3(-this.bullets_speed, 0f, 0f), 4f);
					component.type = this.bulletType;
				}
			}
			Vector3 b = this.bullets_start_position;
			b.x = this.phase_end_bullet_x;
			float num = Vector3.Distance(this.bullets_start_position, b);
			this.phase_end_time = Time.time + num / Mathf.Abs(this.bullets_speed);
			this.cur_state = ShooterEvent.ShooterEventState.MovingBullets;
			if (this.spawnSound != null)
			{
				AudioSystem.PlayOneShot(this.spawnSound, this.spawnSoundVol, 0f, 1f);
				return;
			}
		}
		else if (this.cur_state == ShooterEvent.ShooterEventState.MovingBullets)
		{
			this.shooter.transform.position += new Vector3(this.shooter_speed * Time.deltaTime, 0f, 0f);
			if (!this.phase_ended && Time.time >= this.phase_end_time)
			{
				this.phase_ended = true;
				if (this.do_event_finish)
				{
					base.FinishPhase();
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x040009CE RID: 2510
	public bool right = true;

	// Token: 0x040009CF RID: 2511
	public bool do_event_finish = true;

	// Token: 0x040009D0 RID: 2512
	public GameObject projectile_prefab;

	// Token: 0x040009D1 RID: 2513
	public BulletDodgeBulletType bulletType;

	// Token: 0x040009D2 RID: 2514
	public AudioClip spawnSound;

	// Token: 0x040009D3 RID: 2515
	public float spawnSoundVol = 0.5f;

	// Token: 0x040009D4 RID: 2516
	private GameObject shooter;

	// Token: 0x040009D5 RID: 2517
	private GameObject bullet_spawns;

	// Token: 0x040009D6 RID: 2518
	private ShooterEvent.ShooterEventState cur_state;

	// Token: 0x040009D7 RID: 2519
	private Vector3 shooter_start_position = new Vector3(35f, -10.5f, 0f);

	// Token: 0x040009D8 RID: 2520
	private float shooter_speed = 14f;

	// Token: 0x040009D9 RID: 2521
	private float shooter_stop_x = 27f;

	// Token: 0x040009DA RID: 2522
	private Vector3 bullets_start_position = new Vector3(23f, -10.5f, 0f);

	// Token: 0x040009DB RID: 2523
	private float bullets_speed = 20f;

	// Token: 0x040009DC RID: 2524
	private float phase_end_bullet_x = 10f;

	// Token: 0x040009DD RID: 2525
	private float finish_bullet_x = -35f;

	// Token: 0x040009DE RID: 2526
	private float phase_end_time;

	// Token: 0x0200017A RID: 378
	private enum ShooterEventState
	{
		// Token: 0x040009E0 RID: 2528
		Waiting,
		// Token: 0x040009E1 RID: 2529
		MovingShooter,
		// Token: 0x040009E2 RID: 2530
		MovingBullets
	}
}
