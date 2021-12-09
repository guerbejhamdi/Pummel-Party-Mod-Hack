using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000700 RID: 1792
	public class AttackTrigger : MonoBehaviour
	{
		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x060034D9 RID: 13529 RVA: 0x00023E4E File Offset: 0x0002204E
		public Collider Collider
		{
			get
			{
				if (!this._collider)
				{
					this._collider = base.GetComponent<Collider>();
				}
				return this._collider;
			}
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x0010FBAC File Offset: 0x0010DDAC
		public void Start()
		{
			this.myAnimal = base.transform.GetComponentInParent<Animal>();
			if (this.Collider)
			{
				this.Collider.isTrigger = true;
				this.Collider.enabled = false;
			}
			else
			{
				Debug.LogWarning(base.name + " needs a Collider so 'AttackTrigger' can function correctly");
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x0010FC14 File Offset: 0x0010DE14
		private void OnTriggerEnter(Collider other)
		{
			if (other.isTrigger)
			{
				return;
			}
			this.enemy = other.GetComponentInParent<IMDamagable>();
			if (this.enemy == null)
			{
				if (other.attachedRigidbody && this.PushForce != 0f)
				{
					other.attachedRigidbody.AddForce((other.transform.position - base.transform.position).normalized * this.PushForce, ForceMode.VelocityChange);
				}
				return;
			}
			if (this.myAnimal.GetComponent<IMDamagable>() == this.enemy)
			{
				return;
			}
			DamageValues dv = new DamageValues(this.myAnimal.transform.position - other.bounds.center, this.damageMultiplier * (this.myAnimal ? this.myAnimal.attackStrength : 1f));
			this.enemy.getDamaged(dv);
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x0010FD04 File Offset: 0x0010DF04
		private void OnDrawGizmos()
		{
			if (Application.isPlaying)
			{
				Gizmos.color = this.DebugColor;
				Gizmos.matrix = base.transform.localToWorldMatrix;
				if (this.Collider && this.Collider.enabled)
				{
					if (this.Collider is BoxCollider)
					{
						BoxCollider boxCollider = this.Collider as BoxCollider;
						if (!boxCollider.enabled)
						{
							return;
						}
						float x = base.transform.lossyScale.x * boxCollider.size.x;
						float y = base.transform.lossyScale.y * boxCollider.size.y;
						float z = base.transform.lossyScale.z * boxCollider.size.z;
						Gizmos.matrix = Matrix4x4.TRS(boxCollider.bounds.center, base.transform.rotation, new Vector3(x, y, z));
						Gizmos.DrawCube(Vector3.zero, Vector3.one);
						Gizmos.color = new Color(this.DebugColor.r, this.DebugColor.g, this.DebugColor.b, 1f);
						Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
						return;
					}
					else if (this.Collider is SphereCollider)
					{
						SphereCollider sphereCollider = this.Collider as SphereCollider;
						if (!sphereCollider.enabled)
						{
							return;
						}
						Gizmos.matrix = base.transform.localToWorldMatrix;
						Gizmos.DrawSphere(Vector3.zero + sphereCollider.center, sphereCollider.radius);
						Gizmos.color = new Color(this.DebugColor.r, this.DebugColor.g, this.DebugColor.b, 1f);
						Gizmos.DrawWireSphere(Vector3.zero + sphereCollider.center, sphereCollider.radius);
					}
				}
			}
		}

		// Token: 0x0400333F RID: 13119
		public int index = 1;

		// Token: 0x04003340 RID: 13120
		public float damageMultiplier = 1f;

		// Token: 0x04003341 RID: 13121
		public float PushForce;

		// Token: 0x04003342 RID: 13122
		private Animal myAnimal;

		// Token: 0x04003343 RID: 13123
		private IMDamagable enemy;

		// Token: 0x04003344 RID: 13124
		private Collider _collider;

		// Token: 0x04003345 RID: 13125
		public bool debug = true;

		// Token: 0x04003346 RID: 13126
		public Color DebugColor = new Color(1f, 0.25f, 0f, 0.15f);
	}
}
