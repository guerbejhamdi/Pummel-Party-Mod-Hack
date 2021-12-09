using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200022D RID: 557
public class RunnerPath : MonoBehaviour
{
	// Token: 0x0600102A RID: 4138 RVA: 0x0007F2F8 File Offset: 0x0007D4F8
	public void SetupPath(int seed)
	{
		this.m_curSeed = seed;
		RunnerPath.m_nextPathPos = -200f;
		if (RunnerPath.rand == null)
		{
			RunnerPath.rand = new System.Random(this.m_curSeed);
		}
		this.m_curLevel = this.m_startLevel;
		this.SpawnCollideables(this.m_curLevel);
	}

	// Token: 0x0600102B RID: 4139 RVA: 0x0007F348 File Offset: 0x0007D548
	private void Update()
	{
		if (this.m_pathEnd.transform.position.x - this.m_playerRoot.position.x > 10f)
		{
			base.transform.position = new Vector3(RunnerPath.m_nextPathPos, base.transform.position.y, base.transform.position.z);
			RunnerPath.m_nextPathPos -= 100f;
			this.m_curLevel += 2;
			this.SpawnCollideables(this.m_curLevel);
		}
	}

	// Token: 0x0600102C RID: 4140 RVA: 0x0007F3E4 File Offset: 0x0007D5E4
	private void SpawnCollideables(int level)
	{
		RunnerPath.rand = new System.Random(this.m_curSeed + level);
		if (this.m_collideables == null || level == 0)
		{
			return;
		}
		foreach (GameObject obj in this.m_spawnedObjects)
		{
			UnityEngine.Object.Destroy(obj);
		}
		List<RunnerCollideable> list = new List<RunnerCollideable>();
		foreach (RunnerCollideable runnerCollideable in this.m_collideables)
		{
			if (runnerCollideable.requiredLevel <= level)
			{
				list.Add(runnerCollideable);
			}
		}
		float num = -50f;
		int num2;
		float num3;
		if (level <= 6)
		{
			num2 = 5;
			num3 = 20f;
		}
		else
		{
			num2 = 7;
			num3 = 14.28f;
		}
		for (int i = 0; i < num2; i++)
		{
			int num4 = RunnerPath.rand.Next(-1, list.Count - 1);
			if (num4 != -1)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(list[num4].prefab, base.transform);
				gameObject.transform.localPosition = new Vector3(num, 0f, 0f);
				gameObject.transform.localRotation = Quaternion.identity;
				this.m_spawnedObjects.Add(gameObject);
			}
			num += num3;
		}
	}

	// Token: 0x0400106F RID: 4207
	[SerializeField]
	public Transform m_playerRoot;

	// Token: 0x04001070 RID: 4208
	[SerializeField]
	public Transform m_pathEnd;

	// Token: 0x04001071 RID: 4209
	[SerializeField]
	public int m_startLevel;

	// Token: 0x04001072 RID: 4210
	[SerializeField]
	public List<RunnerCollideable> m_collideables;

	// Token: 0x04001073 RID: 4211
	private int m_curLevel;

	// Token: 0x04001074 RID: 4212
	private static float m_nextPathPos = -200f;

	// Token: 0x04001075 RID: 4213
	private static System.Random rand = null;

	// Token: 0x04001076 RID: 4214
	private List<GameObject> m_spawnedObjects = new List<GameObject>();

	// Token: 0x04001077 RID: 4215
	private int m_curSeed = 11742;
}
