using System;
using UnityEngine;

// Token: 0x02000378 RID: 888
[ExecuteInEditMode]
public class FX_Bees : MonoBehaviour
{
	// Token: 0x17000245 RID: 581
	// (get) Token: 0x060017E8 RID: 6120 RVA: 0x00011C1F File Offset: 0x0000FE1F
	// (set) Token: 0x060017E9 RID: 6121 RVA: 0x00011C27 File Offset: 0x0000FE27
	public Transform Target
	{
		get
		{
			return this.m_target;
		}
		set
		{
			this.m_target = value;
		}
	}

	// Token: 0x060017EA RID: 6122 RVA: 0x00011C30 File Offset: 0x0000FE30
	private void Awake()
	{
		this.m_particles = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x060017EB RID: 6123 RVA: 0x000A5824 File Offset: 0x000A3A24
	public void Update()
	{
		if (this.m_particles.isStopped)
		{
			return;
		}
		int size = this.m_particles.GetParticles(this.particles);
		if (this.particles != null)
		{
			Vector3 a = (this.m_target != null) ? this.m_target.position : base.transform.position;
			for (int i = 0; i < this.particles.Length; i++)
			{
				Vector3.Distance(a, this.particles[i].position);
				Vector3 normalized = (a - this.particles[i].position).normalized;
				ParticleSystem.Particle[] array = this.particles;
				int num = i;
				array[num].velocity = array[num].velocity + normalized * Time.deltaTime * this.m_forwardAcceleration;
				ParticleSystem.Particle[] array2 = this.particles;
				int num2 = i;
				array2[num2].velocity = array2[num2].velocity + Vector3.Cross(Vector3.up, normalized) * Time.deltaTime * this.m_sideAcceleration;
			}
			this.m_particles.SetParticles(this.particles, size);
		}
	}

	// Token: 0x0400196A RID: 6506
	[SerializeField]
	private Transform m_target;

	// Token: 0x0400196B RID: 6507
	[SerializeField]
	private float m_forwardAcceleration = 6f;

	// Token: 0x0400196C RID: 6508
	[SerializeField]
	private float m_sideAcceleration = 1f;

	// Token: 0x0400196D RID: 6509
	private ParticleSystem m_particles;

	// Token: 0x0400196E RID: 6510
	private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1024];
}
