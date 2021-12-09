using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x0200089F RID: 2207
	public class RayCollisionPrinter : Printer
	{
		// Token: 0x06003E9B RID: 16027 RVA: 0x0002A274 File Offset: 0x00028474
		private void FixedUpdate()
		{
			this.CastCollision(Time.fixedDeltaTime);
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x0013474C File Offset: 0x0013294C
		private void CastCollision(float deltaTime)
		{
			Transform transform = (this.castCenter != null) ? this.castCenter : base.transform;
			Quaternion rotation = transform.rotation * Quaternion.Euler(this.rotationOffset);
			Vector3 vector = transform.position + rotation * this.positionOffset;
			if (this.method == CastMethod.Area)
			{
				Vector3 zero = Vector3.zero;
				zero.x = UnityEngine.Random.Range(-this.castDimensions.x, this.castDimensions.x);
				zero.y = UnityEngine.Random.Range(-this.castDimensions.y, this.castDimensions.y);
				vector += rotation * zero;
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(new Ray(vector, rotation * Vector3.forward), out raycastHit, this.castLength, this.layers.value, this.hitTriggers))
			{
				this.collision = new CollisionData(raycastHit.point, Quaternion.LookRotation(-raycastHit.normal, rotation * Vector3.up), raycastHit.transform, raycastHit.collider.gameObject.layer);
				if (this.condition == CollisionCondition.Constant)
				{
					this.PrintCollision(this.collision);
				}
				if (this.timeElapsed == 0f && this.condition == CollisionCondition.Enter)
				{
					this.PrintCollision(this.collision);
				}
				this.timeElapsed += deltaTime;
				if (this.condition == CollisionCondition.Delay && this.timeElapsed >= this.conditionTime && !this.delayPrinted)
				{
					this.PrintCollision(this.collision);
					this.delayPrinted = true;
					return;
				}
			}
			else
			{
				if (this.timeElapsed > 0f && (this.condition == CollisionCondition.Exit || (this.condition == CollisionCondition.Delay && this.timeElapsed < this.conditionTime)))
				{
					this.PrintCollision(this.collision);
				}
				this.timeElapsed = 0f;
				this.delayPrinted = false;
			}
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x0002A281 File Offset: 0x00028481
		private void PrintCollision(CollisionData collision)
		{
			base.Print(collision.position, collision.rotation, collision.surface, collision.layer);
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x00134944 File Offset: 0x00132B44
		private void OnDrawGizmosSelected()
		{
			Transform transform = (this.castCenter != null) ? this.castCenter : base.transform;
			Quaternion rotation = transform.rotation * Quaternion.Euler(this.rotationOffset);
			Vector3 vector = transform.position + rotation * this.positionOffset;
			Gizmos.color = Color.white;
			CastMethod castMethod = this.method;
			if (castMethod == CastMethod.Point)
			{
				Gizmos.DrawRay(vector, rotation * Vector3.forward * this.castLength);
				return;
			}
			if (castMethod != CastMethod.Area)
			{
				return;
			}
			Gizmos.DrawRay(vector + rotation * new Vector3(this.castDimensions.x, this.castDimensions.y, 0f), rotation * Vector3.forward * this.castLength);
			Gizmos.DrawRay(vector + rotation * new Vector3(-this.castDimensions.x, this.castDimensions.y, 0f), rotation * Vector3.forward * this.castLength);
			Gizmos.DrawRay(vector + rotation * new Vector3(this.castDimensions.x, -this.castDimensions.y, 0f), rotation * Vector3.forward * this.castLength);
			Gizmos.DrawRay(vector + rotation * new Vector3(-this.castDimensions.x, -this.castDimensions.y, 0f), rotation * Vector3.forward * this.castLength);
		}

		// Token: 0x04003ADC RID: 15068
		public CollisionCondition condition;

		// Token: 0x04003ADD RID: 15069
		public float conditionTime = 1f;

		// Token: 0x04003ADE RID: 15070
		public LayerMask layers;

		// Token: 0x04003ADF RID: 15071
		public CastMethod method;

		// Token: 0x04003AE0 RID: 15072
		public Transform castCenter;

		// Token: 0x04003AE1 RID: 15073
		public Vector2 castDimensions;

		// Token: 0x04003AE2 RID: 15074
		public Vector3 positionOffset;

		// Token: 0x04003AE3 RID: 15075
		public Vector3 rotationOffset;

		// Token: 0x04003AE4 RID: 15076
		public float castLength = 1f;

		// Token: 0x04003AE5 RID: 15077
		public QueryTriggerInteraction hitTriggers;

		// Token: 0x04003AE6 RID: 15078
		private float timeElapsed;

		// Token: 0x04003AE7 RID: 15079
		private bool delayPrinted;

		// Token: 0x04003AE8 RID: 15080
		private CollisionData collision;
	}
}
