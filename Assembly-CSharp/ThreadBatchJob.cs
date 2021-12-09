using System;
using System.Collections.Generic;

// Token: 0x0200058E RID: 1422
public class ThreadBatchJob
{
	// Token: 0x14000008 RID: 8
	// (add) Token: 0x060024F3 RID: 9459 RVA: 0x000E0490 File Offset: 0x000DE690
	// (remove) Token: 0x060024F4 RID: 9460 RVA: 0x000E04C8 File Offset: 0x000DE6C8
	public event EventHandler batchCompleteEvent;

	// Token: 0x060024F5 RID: 9461 RVA: 0x000E0500 File Offset: 0x000DE700
	public void AddJobToBatch(ThreadJob job)
	{
		job.AddEventHandler(new EventHandler(this.JobComplete), true);
		List<ThreadJob> obj = this.activeJobs;
		lock (obj)
		{
			this.activeJobs.Add(job);
			this.totalJobs++;
		}
	}

	// Token: 0x060024F6 RID: 9462 RVA: 0x000E0568 File Offset: 0x000DE768
	public void JobComplete(object sender, EventArgs e)
	{
		List<ThreadJob> obj = this.activeJobs;
		lock (obj)
		{
			this.activeJobs.Remove((ThreadJob)sender);
			this.TestFinished();
			this.completeJobs++;
		}
	}

	// Token: 0x060024F7 RID: 9463 RVA: 0x0001A917 File Offset: 0x00018B17
	public void AddEventHandler(EventHandler eventHandler, bool asyncEventComplete = true)
	{
		if (eventHandler != null)
		{
			this.batchCompleteEvent += eventHandler;
			this.asyncEventComplete = asyncEventComplete;
		}
	}

	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x060024F8 RID: 9464 RVA: 0x0001A92A File Offset: 0x00018B2A
	// (set) Token: 0x060024F9 RID: 9465 RVA: 0x000E05C8 File Offset: 0x000DE7C8
	public bool BatchAssignComplete
	{
		get
		{
			return this.batchComplete;
		}
		set
		{
			this.batchComplete = value;
			List<ThreadJob> obj = this.activeJobs;
			lock (obj)
			{
				this.TestFinished();
			}
		}
	}

	// Token: 0x060024FA RID: 9466 RVA: 0x000E0610 File Offset: 0x000DE810
	private void TestFinished()
	{
		if (this.batchComplete && this.activeJobs.Count == 0)
		{
			if (this.asyncEventComplete)
			{
				this.batchCompleteEvent(this, EventArgs.Empty);
				return;
			}
			ThreadManager.events.Add(this.batchCompleteEvent);
		}
	}

	// Token: 0x04002886 RID: 10374
	private List<ThreadJob> activeJobs = new List<ThreadJob>();

	// Token: 0x04002887 RID: 10375
	private bool batchComplete;

	// Token: 0x04002888 RID: 10376
	private bool asyncEventComplete = true;

	// Token: 0x04002889 RID: 10377
	public int completeJobs;

	// Token: 0x0400288A RID: 10378
	public int totalJobs;
}
