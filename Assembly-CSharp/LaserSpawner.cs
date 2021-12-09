using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001CB RID: 459
public class LaserSpawner : MonoBehaviour
{
	// Token: 0x06000D30 RID: 3376 RVA: 0x0000C0B5 File Offset: 0x0000A2B5
	private void Start()
	{
		if (this.m_testIndex >= 0)
		{
			this.SpawnSequence(this.m_testIndex);
		}
	}

	// Token: 0x06000D31 RID: 3377 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x06000D32 RID: 3378 RVA: 0x0006CB64 File Offset: 0x0006AD64
	private void OnDestroy()
	{
		foreach (GameObject gameObject in this.m_spawnedObjects)
		{
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		this.m_spawnedObjects.Clear();
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x0006CB64 File Offset: 0x0006AD64
	public void Clear()
	{
		foreach (GameObject gameObject in this.m_spawnedObjects)
		{
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		this.m_spawnedObjects.Clear();
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x0000C0CC File Offset: 0x0000A2CC
	public void SpawnSequence(int index)
	{
		if (index < 0 || this.m_sequences.Count <= index)
		{
			return;
		}
		this.SpawnSequence(this.m_sequences[index]);
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x0006CBCC File Offset: 0x0006ADCC
	public void SpawnSequence(LaserLeapSequence curSequence)
	{
		foreach (GameObject gameObject in this.m_spawnedObjects)
		{
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		this.m_spawnedObjects.Clear();
		foreach (LaserLeapLaserConfig config in curSequence.config)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.m_laserPfb);
			this.m_spawnedObjects.Add(gameObject2);
			gameObject2.GetComponentInChildren<LaserLeapLaser>().SetConfig(config, curSequence.sequenceLength);
		}
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x0006CC9C File Offset: 0x0006AE9C
	public LaserLeapSequence GetRandomSequence(int level)
	{
		List<LaserLeapSequence> list = new List<LaserLeapSequence>();
		foreach (LaserLeapSequence laserLeapSequence in this.m_sequences)
		{
			if (laserLeapSequence.level == level)
			{
				list.Add(laserLeapSequence);
			}
		}
		int index = UnityEngine.Random.Range(0, list.Count);
		return list[index];
	}

	// Token: 0x06000D37 RID: 3383 RVA: 0x0006CD14 File Offset: 0x0006AF14
	public int GetSequenceIndex(LaserLeapSequence sequence)
	{
		for (int i = 0; i < this.m_sequences.Count; i++)
		{
			if (sequence == this.m_sequences[i])
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x04000CA8 RID: 3240
	[SerializeField]
	private int m_testIndex;

	// Token: 0x04000CA9 RID: 3241
	[SerializeField]
	private GameObject m_laserPfb;

	// Token: 0x04000CAA RID: 3242
	[SerializeField]
	private List<LaserLeapSequence> m_sequences;

	// Token: 0x04000CAB RID: 3243
	private List<GameObject> m_spawnedObjects = new List<GameObject>();
}
