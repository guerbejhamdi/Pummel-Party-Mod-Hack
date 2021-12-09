using System;
using UnityEngine;
using ZP.Utility;

// Token: 0x020003CE RID: 974
public class ActionTimer
{
	// Token: 0x06001A18 RID: 6680 RVA: 0x000133F8 File Offset: 0x000115F8
	public ActionTimer(float interval)
	{
		this.variable_interval = false;
		this.interval = interval;
	}

	// Token: 0x06001A19 RID: 6681 RVA: 0x00013419 File Offset: 0x00011619
	public ActionTimer(float min_interval, float max_interval)
	{
		this.variable_interval = true;
		this.min_interval = min_interval;
		this.max_interval = max_interval;
		this.interval = (min_interval + max_interval) / 2f;
	}

	// Token: 0x06001A1A RID: 6682 RVA: 0x00013450 File Offset: 0x00011650
	public bool Elapsed(bool auto_restart = true)
	{
		bool flag = Time.time - this.last > this.interval;
		if (flag && auto_restart)
		{
			this.Start();
		}
		return flag;
	}

	// Token: 0x06001A1B RID: 6683 RVA: 0x00013471 File Offset: 0x00011671
	public void Start()
	{
		this.last = Time.time;
		if (this.variable_interval)
		{
			this.interval = ZPMath.RandomFloat(GameManager.rand, this.min_interval, this.max_interval);
		}
	}

	// Token: 0x06001A1C RID: 6684 RVA: 0x000134A2 File Offset: 0x000116A2
	public void SetInterval(float interval, bool reset = true)
	{
		this.variable_interval = false;
		this.interval = interval;
		if (reset)
		{
			this.Start();
		}
	}

	// Token: 0x06001A1D RID: 6685 RVA: 0x000134BB File Offset: 0x000116BB
	public void SetInterval(float min_interval, float max_interval, bool reset = true)
	{
		this.min_interval = min_interval;
		this.max_interval = max_interval;
		this.interval = ZPMath.RandomFloat(GameManager.rand, min_interval, max_interval);
		if (reset)
		{
			this.Start();
		}
	}

	// Token: 0x06001A1E RID: 6686 RVA: 0x000134E6 File Offset: 0x000116E6
	public float TimeSinceLast()
	{
		return Time.time - this.last;
	}

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x06001A1F RID: 6687 RVA: 0x000134F4 File Offset: 0x000116F4
	public float Remaining
	{
		get
		{
			return this.interval - (Time.time - this.last);
		}
	}

	// Token: 0x04001BCF RID: 7119
	public float last = float.MinValue;

	// Token: 0x04001BD0 RID: 7120
	public float interval;

	// Token: 0x04001BD1 RID: 7121
	private float min_interval;

	// Token: 0x04001BD2 RID: 7122
	private float max_interval;

	// Token: 0x04001BD3 RID: 7123
	private bool variable_interval;
}
