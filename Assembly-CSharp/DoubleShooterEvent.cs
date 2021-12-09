using System;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class DoubleShooterEvent : BulletDodgeEvent
{
	// Token: 0x06000AD4 RID: 2772 RVA: 0x0005E1E8 File Offset: 0x0005C3E8
	public override void Start()
	{
		this.left_shooter = base.transform.Find("ShooterEventLeft").GetComponent<ShooterEvent>();
		this.right_shooter = base.transform.Find("ShooterEventRight").GetComponent<ShooterEvent>();
		this.left_shooter.NetTime = this.net_time;
		this.right_shooter.NetTime = this.net_time;
		base.Start();
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x0005E254 File Offset: 0x0005C454
	private void Update()
	{
		if (!this.phase_ended && this.left_shooter.PhaseEnded && this.right_shooter.PhaseEnded)
		{
			this.phase_ended = true;
			base.FinishPhase();
		}
		if (this.left_shooter == null && this.right_shooter == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040009CC RID: 2508
	private ShooterEvent left_shooter;

	// Token: 0x040009CD RID: 2509
	private ShooterEvent right_shooter;
}
