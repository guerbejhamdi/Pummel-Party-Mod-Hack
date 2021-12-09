using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000063 RID: 99
public class FrameTimeTracker : MonoBehaviour
{
	// Token: 0x060001DF RID: 479 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x00033FA4 File Offset: 0x000321A4
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.Output();
		this.lastStats = this.curStats;
		FrameTimeTracker.SceneStats sceneStats = this.sceneStats.Find((FrameTimeTracker.SceneStats s) => s.name == scene.name);
		if (sceneStats != null)
		{
			this.curStats = sceneStats;
			return;
		}
		this.curStats = new FrameTimeTracker.SceneStats(scene.name);
		this.sceneStats.Add(this.curStats);
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x00004BF5 File Offset: 0x00002DF5
	private void OnSceneUnLoaded(Scene scene)
	{
		this.curStats = this.lastStats;
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x0003401C File Offset: 0x0003221C
	private void Output()
	{
		string text = "Scene Name,Avg,Avg FPS,Min,Max,FrameCount,TotalFrameTime \n";
		for (int i = 0; i < this.sceneStats.Count; i++)
		{
			float num = this.sceneStats[i].totalFrameTime / (float)this.sceneStats[i].frameCount;
			text = text + this.sceneStats[i].name + ",";
			text = text + num.ToString() + ",";
			text = text + (1f / num).ToString() + ",";
			text = text + this.sceneStats[i].min.ToString() + ",";
			text = text + this.sceneStats[i].max.ToString() + ",";
			text = text + this.sceneStats[i].frameCount.ToString() + ",";
			text = text + this.sceneStats[i].totalFrameTime.ToString() + "\n";
		}
		File.WriteAllText(Application.dataPath + "Stats.csv", text);
	}

	// Token: 0x04000241 RID: 577
	private bool inMingame;

	// Token: 0x04000242 RID: 578
	private List<FrameTimeTracker.SceneStats> sceneStats = new List<FrameTimeTracker.SceneStats>();

	// Token: 0x04000243 RID: 579
	private FrameTimeTracker.SceneStats curStats;

	// Token: 0x04000244 RID: 580
	private FrameTimeTracker.SceneStats lastStats;

	// Token: 0x04000245 RID: 581
	private bool wasMinigame;

	// Token: 0x02000064 RID: 100
	[Serializable]
	private class SceneStats
	{
		// Token: 0x060001E5 RID: 485 RVA: 0x00004C16 File Offset: 0x00002E16
		public SceneStats(string name)
		{
			this.name = name;
		}

		// Token: 0x04000246 RID: 582
		public string name;

		// Token: 0x04000247 RID: 583
		public int frameCount;

		// Token: 0x04000248 RID: 584
		public float totalFrameTime;

		// Token: 0x04000249 RID: 585
		public float max = float.MinValue;

		// Token: 0x0400024A RID: 586
		public float min = float.MaxValue;
	}
}
