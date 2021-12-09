using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// Token: 0x02000590 RID: 1424
[ExecuteInEditMode]
public static class ThreadManager
{
	// Token: 0x06002501 RID: 9473 RVA: 0x000E06CC File Offset: 0x000DE8CC
	static ThreadManager()
	{
		Debug.Log("Thread Manager Started");
		ThreadManager.Setup();
	}

	// Token: 0x06002502 RID: 9474 RVA: 0x000E0720 File Offset: 0x000DE920
	public static void Setup()
	{
		ThreadManager.Reset();
		GameManager.OnQuit = (GameManager.OnQuitEvent)Delegate.Combine(GameManager.OnQuit, new GameManager.OnQuitEvent(ThreadManager.OnQuit));
		int num = Environment.ProcessorCount * 4;
		ThreadManager.threads = new JobThread[num];
		for (int i = 0; i < num; i++)
		{
			ThreadManager.threads[i] = new JobThread(i);
		}
	}

	// Token: 0x06002503 RID: 9475 RVA: 0x000E0780 File Offset: 0x000DE980
	public static void OnQuit()
	{
		for (int i = 0; i < ThreadManager.threads.Length; i++)
		{
			ThreadManager.threads[i].Thread.Abort();
		}
	}

	// Token: 0x06002504 RID: 9476 RVA: 0x0001A9A2 File Offset: 0x00018BA2
	public static void Reset()
	{
		ThreadManager.jobQueue.Clear();
		ThreadManager.events.Clear();
		ThreadManager.dynamicEvents.Clear();
		ThreadManager.dynamicParams.Clear();
		if (ThreadManager.threads != null)
		{
			ThreadManager.OnQuit();
		}
	}

	// Token: 0x06002505 RID: 9477 RVA: 0x000E07B0 File Offset: 0x000DE9B0
	public static void Enqueue(ThreadJob job, EventHandler eventhandler = null, bool asyncEventComplete = true)
	{
		Queue<ThreadJob> obj = ThreadManager.jobQueue;
		lock (obj)
		{
			job.AddEventHandler(eventhandler, true);
			ThreadManager.jobQueue.Enqueue(job);
			Monitor.PulseAll(ThreadManager.jobQueue);
		}
	}

	// Token: 0x06002506 RID: 9478 RVA: 0x000E0808 File Offset: 0x000DEA08
	public static void EnqueueBatch(ThreadJob job)
	{
		Queue<ThreadJob> obj = ThreadManager.jobQueue;
		lock (obj)
		{
			if (!ThreadManager.jobQueue.Contains(job))
			{
				ThreadManager.curBatchJob.AddJobToBatch(job);
				ThreadManager.jobQueue.Enqueue(job);
				Monitor.PulseAll(ThreadManager.jobQueue);
			}
		}
	}

	// Token: 0x06002507 RID: 9479 RVA: 0x0001A9D8 File Offset: 0x00018BD8
	public static void StartJobBatchAssign(EventHandler eventHandler = null, bool asyncEventComplete = true)
	{
		ThreadManager.curBatchJob = new ThreadBatchJob();
		ThreadManager.curBatchJob.AddEventHandler(eventHandler, asyncEventComplete);
	}

	// Token: 0x06002508 RID: 9480 RVA: 0x0001A9F0 File Offset: 0x00018BF0
	public static void FinishJobBatchAssign()
	{
		ThreadManager.curBatchJob.BatchAssignComplete = true;
	}

	// Token: 0x06002509 RID: 9481 RVA: 0x000E0870 File Offset: 0x000DEA70
	public static void Update()
	{
		if (ThreadManager.events.Count > 0)
		{
			ThreadManager.eventsArray.Clear();
			List<EventHandler> obj = ThreadManager.events;
			lock (obj)
			{
				ThreadManager.eventsArray.AddRange(ThreadManager.events);
				ThreadManager.events.Clear();
			}
			for (int i = 0; i < ThreadManager.eventsArray.Count; i++)
			{
				ThreadManager.eventsArray[i](null, EventArgs.Empty);
			}
		}
		int jobsStartedThisFrame = ThreadManager.JobsStartedThisFrame;
		ThreadManager.JobsStartedThisFrame = 0;
	}

	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x0600250A RID: 9482 RVA: 0x0001A9FD File Offset: 0x00018BFD
	public static int BatchJobsTotalComplete
	{
		get
		{
			return ThreadManager.curBatchJob.completeJobs;
		}
	}

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x0600250B RID: 9483 RVA: 0x0001AA09 File Offset: 0x00018C09
	public static int BatchJobTotalJobs
	{
		get
		{
			return ThreadManager.curBatchJob.totalJobs;
		}
	}

	// Token: 0x0400288D RID: 10381
	public static Queue<ThreadJob> jobQueue = new Queue<ThreadJob>();

	// Token: 0x0400288E RID: 10382
	private static JobThread[] threads;

	// Token: 0x0400288F RID: 10383
	private static ThreadBatchJob curBatchJob;

	// Token: 0x04002890 RID: 10384
	public static List<EventHandler> events = new List<EventHandler>();

	// Token: 0x04002891 RID: 10385
	public static List<EventHandler> dynamicEvents = new List<EventHandler>();

	// Token: 0x04002892 RID: 10386
	public static List<object[]> dynamicParams = new List<object[]>();

	// Token: 0x04002893 RID: 10387
	public static int JobsStartedThisFrame = 0;

	// Token: 0x04002894 RID: 10388
	private static List<EventHandler> eventsArray = new List<EventHandler>();
}
