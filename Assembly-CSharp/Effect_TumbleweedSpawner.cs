using System;
using UnityEngine;

// Token: 0x02000244 RID: 580
public class Effect_TumbleweedSpawner : MonoBehaviour
{
	// Token: 0x060010C0 RID: 4288 RVA: 0x0000DEE9 File Offset: 0x0000C0E9
	public void Awake()
	{
		this.m_nextSpawn = Time.time + UnityEngine.Random.Range(this.m_minSpawnFrequency, this.m_maxSpawnFrequency);
	}

	// Token: 0x060010C1 RID: 4289 RVA: 0x00083150 File Offset: 0x00081350
	public void Update()
	{
		if (Time.time >= this.m_nextSpawn)
		{
			this.m_nextSpawn = Time.time + UnityEngine.Random.Range(this.m_minSpawnFrequency, this.m_maxSpawnFrequency);
			Transform child = base.transform.GetChild(UnityEngine.Random.Range(0, base.transform.childCount - 1));
			UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(this.m_prefabToSpawn, child.position, Quaternion.identity), this.m_lifeTime);
		}
	}

	// Token: 0x04001141 RID: 4417
	[SerializeField]
	protected GameObject m_prefabToSpawn;

	// Token: 0x04001142 RID: 4418
	[SerializeField]
	protected float m_minSpawnFrequency = 1f;

	// Token: 0x04001143 RID: 4419
	[SerializeField]
	protected float m_maxSpawnFrequency = 2f;

	// Token: 0x04001144 RID: 4420
	[SerializeField]
	protected float m_lifeTime = 20f;

	// Token: 0x04001145 RID: 4421
	private float m_nextSpawn;
}
