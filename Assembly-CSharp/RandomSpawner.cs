using System;
using UnityEngine;

// Token: 0x02000315 RID: 789
[ExecuteInEditMode]
public class RandomSpawner : MonoBehaviour
{
	// Token: 0x060015B0 RID: 5552 RVA: 0x00010660 File Offset: 0x0000E860
	public void Awake()
	{
		this.sys = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x060015B1 RID: 5553 RVA: 0x0001066E File Offset: 0x0000E86E
	private void Update()
	{
		if (Time.time > this.nextEmit)
		{
			this.sys.Emit(1);
			this.nextEmit = Time.time + UnityEngine.Random.Range(this.minTime, this.maxTime);
		}
	}

	// Token: 0x040016AF RID: 5807
	public float minTime = 1f;

	// Token: 0x040016B0 RID: 5808
	public float maxTime = 5f;

	// Token: 0x040016B1 RID: 5809
	private ParticleSystem sys;

	// Token: 0x040016B2 RID: 5810
	private float nextEmit;
}
