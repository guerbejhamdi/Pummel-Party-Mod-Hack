using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000897 RID: 2199
	[RequireComponent(typeof(Collider))]
	[RequireComponent(typeof(Rigidbody))]
	public class CollisionPrinter : Printer
	{
		// Token: 0x06003E8C RID: 16012 RVA: 0x0002A165 File Offset: 0x00028365
		private void OnCollisionEnter(Collision collision)
		{
			if (this.condition == CollisionCondition.Enter || this.condition == CollisionCondition.Constant)
			{
				this.PrintCollision(collision);
			}
			this.timeElapsed = 0f;
			this.delayPrinted = false;
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x0013405C File Offset: 0x0013225C
		private void OnCollisionStay(Collision collision)
		{
			this.timeElapsed += Time.deltaTime;
			if (this.condition == CollisionCondition.Constant)
			{
				this.PrintCollision(collision);
			}
			if (this.condition == CollisionCondition.Delay && this.timeElapsed > this.conditionTime && !this.delayPrinted)
			{
				this.PrintCollision(collision);
				this.delayPrinted = true;
			}
		}

		// Token: 0x06003E8E RID: 16014 RVA: 0x0002A191 File Offset: 0x00028391
		private void OnCollisionExit(Collision collision)
		{
			if (this.condition == CollisionCondition.Exit)
			{
				this.PrintCollision(collision);
			}
			if (this.condition == CollisionCondition.Delay && !this.delayPrinted)
			{
				this.PrintCollision(collision);
			}
		}

		// Token: 0x06003E8F RID: 16015 RVA: 0x001340B8 File Offset: 0x001322B8
		public void PrintCollision(Collision collision)
		{
			Vector3 vector = Vector3.zero;
			Vector3 a = Vector3.zero;
			int num = 0;
			foreach (ContactPoint contactPoint in collision.contacts)
			{
				if (this.layers.value == (this.layers.value | 1 << contactPoint.otherCollider.gameObject.layer))
				{
					num++;
					if (num == 1)
					{
						Transform transform = contactPoint.otherCollider.transform;
					}
					if (num == 1)
					{
						vector = contactPoint.point;
					}
					if (num == 1)
					{
						a = contactPoint.normal;
					}
				}
			}
			RaycastHit raycastHit;
			if (num > 0 && Physics.Raycast(vector + a * 0.3f, -a, out raycastHit, 0.4f, this.layers.value))
			{
				vector = raycastHit.point;
				a = raycastHit.normal;
				Transform transform = raycastHit.collider.transform;
				Vector3 upwards;
				if (this.rotationSource == RotationSource.Velocity && base.GetComponent<Rigidbody>().velocity != Vector3.zero)
				{
					upwards = base.GetComponent<Rigidbody>().velocity.normalized;
				}
				else if (this.rotationSource == RotationSource.Random)
				{
					upwards = UnityEngine.Random.insideUnitSphere.normalized;
				}
				else
				{
					upwards = Vector3.up;
				}
				base.Print(vector, Quaternion.LookRotation(-a, upwards), transform, raycastHit.collider.gameObject.layer);
			}
		}

		// Token: 0x04003AB1 RID: 15025
		public RotationSource rotationSource;

		// Token: 0x04003AB2 RID: 15026
		public CollisionCondition condition;

		// Token: 0x04003AB3 RID: 15027
		public float conditionTime;

		// Token: 0x04003AB4 RID: 15028
		public LayerMask layers;

		// Token: 0x04003AB5 RID: 15029
		private float timeElapsed;

		// Token: 0x04003AB6 RID: 15030
		private bool delayPrinted;
	}
}
