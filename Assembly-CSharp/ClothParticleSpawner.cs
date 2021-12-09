using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200002B RID: 43
[ExecuteInEditMode]
public class ClothParticleSpawner : MonoBehaviour
{
	// Token: 0x060000C8 RID: 200 RVA: 0x0002EC28 File Offset: 0x0002CE28
	public void Awake()
	{
		this.sys = base.GetComponent<ParticleSystem>();
		this.rend = base.GetComponent<ParticleSystemRenderer>();
		this.m_baseMaterial = this.rend.sharedMaterial;
		this.m_unlitMaterial = new Material(this.rend.sharedMaterial);
		this.m_unlitMaterial.CopyPropertiesFromMaterial(this.rend.sharedMaterial);
		this.m_unlitMaterial.SetColor("_TintColor", new Color(0f, 0f, 0f, 0f));
		this.lastPos = base.transform.position;
		GameManager.OnGlobalPlayerEmissionChanged.AddListener(new UnityAction<float>(this.OnGlobalPlayerEmissionChanged));
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x0000410E File Offset: 0x0000230E
	public void OnDestroy()
	{
		GameManager.OnGlobalPlayerEmissionChanged.RemoveListener(new UnityAction<float>(this.OnGlobalPlayerEmissionChanged));
		this.rend.sharedMaterial = this.m_baseMaterial;
	}

	// Token: 0x060000CA RID: 202 RVA: 0x0002ECDC File Offset: 0x0002CEDC
	private void Update()
	{
		if (this.spawnOverMovement > 0f && this.lastPos != base.transform.position)
		{
			this.distanceSinceLastSpawn += (base.transform.position - this.lastPos).sqrMagnitude;
			this.lastPos = base.transform.position;
		}
		if (this.maxTime > 0f && Time.time > this.nextEmit)
		{
			this.Spawn();
		}
		if (this.spawnOverMovement > 0f && this.distanceSinceLastSpawn >= this.spawnOverMovement)
		{
			this.Spawn();
			this.distanceSinceLastSpawn = 0f;
		}
	}

	// Token: 0x060000CB RID: 203 RVA: 0x0002ED98 File Offset: 0x0002CF98
	private void Spawn()
	{
		this.emitParams.position = this.GetRandomClothPosition();
		if (this.randomVelocity)
		{
			this.emitParams.velocity = UnityEngine.Random.onUnitSphere * this.randomVelocityScale;
		}
		this.sys.Emit(this.emitParams, 1);
		this.nextEmit = Time.time + UnityEngine.Random.Range(this.minTime, this.maxTime);
	}

	// Token: 0x060000CC RID: 204 RVA: 0x0002EE08 File Offset: 0x0002D008
	private Vector3 GetRandomClothPosition()
	{
		if (this.clothTarget == null || this.clothTarget.vertices.Length == 0)
		{
			return Vector3.zero;
		}
		int num = UnityEngine.Random.Range(0, this.clothTarget.vertices.Length - 1);
		this.mat.SetTRS(this.clothTarget.transform.position, this.clothTarget.transform.rotation, Vector3.one);
		return this.mat.MultiplyPoint(this.clothTarget.vertices[num]);
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00004137 File Offset: 0x00002337
	private void OnGlobalPlayerEmissionChanged(float value)
	{
		if (value != this.m_lastValue)
		{
			this.m_lastValue = value;
			if (value < 0.5f)
			{
				this.rend.sharedMaterial = this.m_unlitMaterial;
				return;
			}
			this.rend.sharedMaterial = this.m_baseMaterial;
		}
	}

	// Token: 0x040000F3 RID: 243
	[Header("Cloth")]
	public Cloth clothTarget;

	// Token: 0x040000F4 RID: 244
	[Header("Spawning")]
	public float minTime = 1f;

	// Token: 0x040000F5 RID: 245
	public float maxTime = 5f;

	// Token: 0x040000F6 RID: 246
	public float spawnOverMovement;

	// Token: 0x040000F7 RID: 247
	public bool randomVelocity;

	// Token: 0x040000F8 RID: 248
	public float randomVelocityScale = 1f;

	// Token: 0x040000F9 RID: 249
	[Header("")]
	private ParticleSystem sys;

	// Token: 0x040000FA RID: 250
	private ParticleSystemRenderer rend;

	// Token: 0x040000FB RID: 251
	private float nextEmit;

	// Token: 0x040000FC RID: 252
	private float distanceSinceLastSpawn;

	// Token: 0x040000FD RID: 253
	private Vector3 lastPos = Vector3.zero;

	// Token: 0x040000FE RID: 254
	private ParticleSystem.EmitParams emitParams;

	// Token: 0x040000FF RID: 255
	private Material m_unlitMaterial;

	// Token: 0x04000100 RID: 256
	private Material m_baseMaterial;

	// Token: 0x04000101 RID: 257
	private float nextLines;

	// Token: 0x04000102 RID: 258
	private Matrix4x4 mat;

	// Token: 0x04000103 RID: 259
	private float m_lastValue = 1f;
}
