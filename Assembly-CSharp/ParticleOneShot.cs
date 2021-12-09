using System;
using UnityEngine;

// Token: 0x02000572 RID: 1394
public class ParticleOneShot : MonoBehaviour
{
	// Token: 0x06002494 RID: 9364 RVA: 0x000DB780 File Offset: 0x000D9980
	public void Start()
	{
		this.ps = base.GetComponentInChildren<ParticleSystem>();
		if (this.ps.main.loop)
		{
			Debug.LogError(base.gameObject.name + ": Using Particle One Shot with particle system set to looping, this may cause particles to pop in at the end of the loop");
		}
		this.duration = this.ps.main.duration;
	}

	// Token: 0x06002495 RID: 9365 RVA: 0x000DB7E4 File Offset: 0x000D99E4
	public void Update()
	{
		if (this.is_alive)
		{
			if (this.ps && !this.ps.IsAlive())
			{
				UnityEngine.Object.Destroy(base.gameObject);
				this.is_alive = false;
			}
			this.life_count += Time.deltaTime;
			if (this.life_count > this.duration)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				this.is_alive = false;
			}
		}
	}

	// Token: 0x040027D5 RID: 10197
	private ParticleSystem ps;

	// Token: 0x040027D6 RID: 10198
	private float duration;

	// Token: 0x040027D7 RID: 10199
	private float life_count;

	// Token: 0x040027D8 RID: 10200
	private bool is_alive = true;
}
