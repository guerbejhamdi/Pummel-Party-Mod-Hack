using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// Token: 0x0200058D RID: 1421
public class JobThread
{
	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x060024F0 RID: 9456 RVA: 0x0001A8DE File Offset: 0x00018ADE
	public Thread Thread
	{
		get
		{
			return this.curThread;
		}
	}

	// Token: 0x060024F1 RID: 9457 RVA: 0x0001A8E6 File Offset: 0x00018AE6
	public JobThread(int id)
	{
		this.id = id;
		this.curThread = new Thread(new ThreadStart(this.DoJobs));
		this.curThread.Start();
	}

	// Token: 0x060024F2 RID: 9458 RVA: 0x000E035C File Offset: 0x000DE55C
	private void DoJobs()
	{
		try
		{
			while (!this.abort)
			{
				ThreadJob threadJob = null;
				Queue<ThreadJob> jobQueue = ThreadManager.jobQueue;
				lock (jobQueue)
				{
					while (!this.abort && ThreadManager.jobQueue.Count == 0)
					{
						Monitor.Wait(ThreadManager.jobQueue);
					}
					if (ThreadManager.jobQueue.Count != 0)
					{
						threadJob = ThreadManager.jobQueue.Dequeue();
					}
					Monitor.PulseAll(ThreadManager.jobQueue);
				}
				if (threadJob != null)
				{
					try
					{
						ThreadManager.JobsStartedThisFrame++;
						threadJob.DoJob(this.id);
					}
					catch (Exception ex)
					{
						Debug.LogError("Thread " + this.id.ToString() + " encountered an error: " + ex.ToString());
					}
				}
			}
			this.finished = true;
			this.curThread.Join();
		}
		catch (Exception ex2)
		{
			if (!(ex2 is ThreadAbortException))
			{
				Debug.LogError("Thread " + this.id.ToString() + " encountered an error: " + ex2.ToString());
			}
		}
	}

	// Token: 0x04002881 RID: 10369
	private Thread curThread;

	// Token: 0x04002882 RID: 10370
	private int id;

	// Token: 0x04002883 RID: 10371
	public bool abort;

	// Token: 0x04002884 RID: 10372
	public bool finished;
}
