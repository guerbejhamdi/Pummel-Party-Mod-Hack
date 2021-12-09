using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200031D RID: 797
public class HorsehoeMagnet : MonoBehaviour
{
	// Token: 0x060015CD RID: 5581 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Awake()
	{
	}

	// Token: 0x060015CE RID: 5582 RVA: 0x0001085F File Offset: 0x0000EA5F
	public void ActivateMagnet()
	{
		this.FindMetalSpawnPoints();
		base.StartCoroutine(this.SpawnMetalFragments());
		this.m_light.enabled = true;
		this.m_effectLines.Play();
	}

	// Token: 0x060015CF RID: 5583 RVA: 0x0009C8BC File Offset: 0x0009AABC
	public void Update()
	{
		int size = this.m_metalFragments.GetParticles(this.particles);
		if (this.particles != null)
		{
			float num = 6f;
			for (int i = 0; i < this.particles.Length; i++)
			{
				float num2 = Vector3.Distance(this.m_particleTarget.position, this.particles[i].position);
				ParticleSystem.Particle[] array = this.particles;
				int num3 = i;
				array[num3].velocity = array[num3].velocity + (this.m_particleTarget.position - this.particles[i].position).normalized * Time.deltaTime * num;
				if (num2 < 2f)
				{
					this.particles[i].velocity = Vector3.ClampMagnitude(this.particles[i].velocity, num * (num2 / 2f));
				}
				else
				{
					this.particles[i].velocity = Vector3.ClampMagnitude(this.particles[i].velocity, num);
				}
			}
			this.m_metalFragments.SetParticles(this.particles, size);
		}
	}

	// Token: 0x060015D0 RID: 5584 RVA: 0x0009C9F4 File Offset: 0x0009ABF4
	private void FindMetalSpawnPoints()
	{
		int num = 20;
		for (int i = 0; i < num; i++)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.m_metalFragmentSpawn.position + new Vector3(UnityEngine.Random.value * 2f - 1f, 0f, UnityEngine.Random.value * 2f - 1f) * this.m_spawnAreaSize, Vector3.down, out raycastHit))
			{
				this.m_metalSpawnPoints.Add(raycastHit.point);
				Debug.DrawLine(raycastHit.point, raycastHit.point + raycastHit.normal, Color.green, 5f);
			}
		}
	}

	// Token: 0x060015D1 RID: 5585 RVA: 0x0001088B File Offset: 0x0000EA8B
	private IEnumerator SpawnMetalFragments()
	{
		for (;;)
		{
			ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
			if (this.m_metalSpawnPoints.Count >= 1)
			{
				emitParams.position = this.m_metalSpawnPoints[UnityEngine.Random.Range(0, this.m_metalSpawnPoints.Count)];
			}
			emitParams.startSize = 1.5f;
			this.m_metalFragments.Emit(emitParams, 1);
			yield return new WaitForSeconds(0.1f);
		}
		yield break;
	}

	// Token: 0x040016F4 RID: 5876
	[SerializeField]
	private ParticleSystem m_metalFragments;

	// Token: 0x040016F5 RID: 5877
	[SerializeField]
	private Transform m_metalFragmentSpawn;

	// Token: 0x040016F6 RID: 5878
	[SerializeField]
	private Transform m_particleTarget;

	// Token: 0x040016F7 RID: 5879
	[SerializeField]
	private float m_spawnAreaSize = 2f;

	// Token: 0x040016F8 RID: 5880
	[SerializeField]
	private Light m_light;

	// Token: 0x040016F9 RID: 5881
	[SerializeField]
	private ParticleSystem m_effectLines;

	// Token: 0x040016FA RID: 5882
	private List<Vector3> m_metalSpawnPoints = new List<Vector3>();

	// Token: 0x040016FB RID: 5883
	private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1024];
}
