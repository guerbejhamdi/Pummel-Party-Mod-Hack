using System;

// Token: 0x0200058F RID: 1423
public abstract class ThreadJob
{
	// Token: 0x14000009 RID: 9
	// (add) Token: 0x060024FC RID: 9468 RVA: 0x000E065C File Offset: 0x000DE85C
	// (remove) Token: 0x060024FD RID: 9469 RVA: 0x000E0694 File Offset: 0x000DE894
	public event EventHandler jobCompleteEvent;

	// Token: 0x060024FE RID: 9470 RVA: 0x0001A94C File Offset: 0x00018B4C
	public ThreadJob()
	{
	}

	// Token: 0x060024FF RID: 9471 RVA: 0x0001A95B File Offset: 0x00018B5B
	public virtual void DoJob(int threadID)
	{
		if (this.asyncEventComplete)
		{
			if (this.jobCompleteEvent != null)
			{
				this.jobCompleteEvent(this, EventArgs.Empty);
				return;
			}
		}
		else
		{
			ThreadManager.events.Add(this.jobCompleteEvent);
		}
	}

	// Token: 0x06002500 RID: 9472 RVA: 0x0001A98F File Offset: 0x00018B8F
	public virtual void AddEventHandler(EventHandler handler, bool asyncEventComplete = true)
	{
		if (handler != null)
		{
			this.jobCompleteEvent += handler;
			this.asyncEventComplete = asyncEventComplete;
		}
	}

	// Token: 0x0400288C RID: 10380
	protected bool asyncEventComplete = true;
}
