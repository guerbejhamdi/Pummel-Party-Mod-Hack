using System;
using System.Collections.Generic;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x0200089A RID: 2202
	[ExecuteInEditMode]
	[RequireComponent(typeof(ParticleSystem))]
	public class ParticleCollisionPrinter : Printer
	{
		// Token: 0x06003E91 RID: 16017 RVA: 0x0002A1C3 File Offset: 0x000283C3
		private void Start()
		{
			this.partSystem = base.GetComponent<ParticleSystem>();
			if (Application.isPlaying)
			{
				this.collisionEvents = new List<ParticleCollisionEvent>();
			}
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x00134230 File Offset: 0x00132430
		private void Update()
		{
			if (!this.partSystem.collision.enabled)
			{
				Debug.LogWarning("Particle system collisions must be enabled for the particle system to print decals");
				return;
			}
			if (!this.partSystem.collision.sendCollisionMessages)
			{
				Debug.LogWarning("Particle system must send collision messages for the particle system to print decals. This option can be enabled under the collisions menu.");
			}
		}

		// Token: 0x06003E93 RID: 16019 RVA: 0x0013427C File Offset: 0x0013247C
		private void OnParticleCollision(GameObject other)
		{
			if (base.enabled && Application.isPlaying && this.ratio > 0f)
			{
				int num = this.partSystem.GetCollisionEvents(other, this.collisionEvents);
				for (int i = 0; i < num; i++)
				{
					if (this.ratio == 1f || this.ratio > UnityEngine.Random.Range(0f, 1f))
					{
						Vector3 vector = this.collisionEvents[i].intersection;
						Vector3 normal = this.collisionEvents[i].normal;
						Transform transform = other.transform;
						int layerMask = 1 << other.layer;
						RaycastHit raycastHit;
						if (Physics.Raycast(vector, -normal, out raycastHit, float.PositiveInfinity, layerMask))
						{
							vector = raycastHit.point;
							normal = raycastHit.normal;
							transform = raycastHit.collider.transform;
							Vector3 upwards;
							if (this.rotationSource == RotationSource.Velocity && this.collisionEvents[i].velocity != Vector3.zero)
							{
								upwards = this.collisionEvents[i].velocity.normalized;
							}
							else if (this.rotationSource == RotationSource.Random)
							{
								upwards = UnityEngine.Random.insideUnitSphere.normalized;
							}
							else
							{
								upwards = Vector3.up;
							}
							base.Print(vector, Quaternion.LookRotation(-normal, upwards), transform, raycastHit.collider.gameObject.layer);
						}
					}
				}
			}
		}

		// Token: 0x04003ABF RID: 15039
		public RotationSource rotationSource;

		// Token: 0x04003AC0 RID: 15040
		public float ratio = 1f;

		// Token: 0x04003AC1 RID: 15041
		private ParticleSystem partSystem;

		// Token: 0x04003AC2 RID: 15042
		private float maxparticleCollisionSize;

		// Token: 0x04003AC3 RID: 15043
		private List<ParticleCollisionEvent> collisionEvents;
	}
}
