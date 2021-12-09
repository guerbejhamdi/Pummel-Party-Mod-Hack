using System;
using UnityEngine;

// Token: 0x020001CC RID: 460
[RequireComponent(typeof(ParticleSystem))]
public class LightningParticleController : MonoBehaviour
{
	// Token: 0x06000D39 RID: 3385 RVA: 0x0006CD4C File Offset: 0x0006AF4C
	public void Awake()
	{
		if (this.system == null)
		{
			this.system = base.GetComponent<ParticleSystem>();
		}
		this.startPos = new Vector3[this.numBolts];
		LightningParticleController.particles = new ParticleSystem.Particle[this.numBolts];
		for (int i = 0; i < this.numBolts; i++)
		{
			this.startPos[i] = base.transform.position;
		}
	}

	// Token: 0x06000D3A RID: 3386 RVA: 0x0000C106 File Offset: 0x0000A306
	public void Fire(Vector3 position)
	{
		this.t = 0f;
		this.system.Emit(this.numBolts);
		this.Target.position = position;
		this.hit = false;
	}

	// Token: 0x06000D3B RID: 3387 RVA: 0x0006CDBC File Offset: 0x0006AFBC
	private void Update()
	{
		int num = this.system.GetParticles(LightningParticleController.particles);
		this.t += Time.deltaTime;
		for (int i = 0; i < num; i++)
		{
			Vector3 b = UnityEngine.Random.insideUnitSphere * 0.75f;
			float num2 = this.t / 0.1f;
			if (num2 < 1f)
			{
				LightningParticleController.particles[i].position = Vector3.Lerp(this.startPos[i], this.Target.position, num2) + b;
			}
			else if (!this.hit)
			{
				this.hit = true;
				UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(this.lightningHit, this.Target.position, this.Target.rotation), 3f);
				LightningParticleController.particles[i].position = this.Target.position;
			}
		}
		this.system.SetParticles(LightningParticleController.particles, num);
	}

	// Token: 0x04000CAC RID: 3244
	public Transform Target;

	// Token: 0x04000CAD RID: 3245
	public GameObject lightningHit;

	// Token: 0x04000CAE RID: 3246
	private ParticleSystem system;

	// Token: 0x04000CAF RID: 3247
	private static ParticleSystem.Particle[] particles;

	// Token: 0x04000CB0 RID: 3248
	private int numBolts = 2;

	// Token: 0x04000CB1 RID: 3249
	private Vector3[] startPos;

	// Token: 0x04000CB2 RID: 3250
	private float t;

	// Token: 0x04000CB3 RID: 3251
	private bool hit;
}
