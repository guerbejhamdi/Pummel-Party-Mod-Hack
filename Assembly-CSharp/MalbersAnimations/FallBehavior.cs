using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200070C RID: 1804
	public class FallBehavior : StateMachineBehaviour
	{
		// Token: 0x060034F8 RID: 13560 RVA: 0x00110954 File Offset: 0x0010EB54
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animal = animator.GetComponent<Animal>();
			this.rb = animator.GetComponent<Rigidbody>();
			this.transform = animator.transform;
			this.GroundLayer = this.animal.GroundLayer;
			this.IncomingSpeed = this.rb.velocity;
			this.IncomingSpeed.y = 0f;
			this.animal.IsInAir = true;
			animator.SetFloat(Hash.IDFloat, 1f);
			this.MaxHeight = float.MinValue;
			this.animal.RootMotion = false;
			this.rb.drag = 0f;
			this.rb.useGravity = true;
			this.FallBlend = 1f;
			this.check_Water = false;
			this.animal.Waterlevel = Animal.LowWaterLevel;
			this.waterlevel = Animal.LowWaterLevel;
			this.animal.RaycastWater();
			this.PivotsRayInterval = this.animal.PivotsRayInterval;
			this.FallRayInterval = this.animal.FallRayInterval;
			this.WaterRayInterval = this.animal.WaterRayInterval;
			this.animal.PivotsRayInterval = (this.animal.FallRayInterval = (this.animal.WaterRayInterval = 1));
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x00110AA0 File Offset: 0x0010ECA0
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.animal.debug)
			{
				Debug.DrawRay(this.animal.Main_Pivot_Point, -this.animal.transform.up * 50f, Color.magenta);
			}
			if (this.animal.CanDoubleJump && this.animal.Double_Jump == 0 && this.animal.Jump)
			{
				this.animal.Double_Jump++;
				this.animal.SetIntID(112);
			}
			if (Physics.Raycast(this.animal.Main_Pivot_Point, -this.animal.transform.up, out this.FallRay, 50f, this.GroundLayer))
			{
				if (this.MaxHeight < this.FallRay.distance)
				{
					this.MaxHeight = this.FallRay.distance;
				}
				this.FallBlend = Mathf.Lerp(this.FallBlend, (this.FallRay.distance - this.LowerDistance) / (this.MaxHeight - this.LowerDistance), Time.deltaTime * 20f);
				animator.SetFloat(Hash.IDFloat, this.FallBlend);
			}
			this.CheckforWater();
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x00110BE4 File Offset: 0x0010EDE4
		private void CheckforWater()
		{
			if (this.waterlevel != this.animal.Waterlevel && this.animal.Waterlevel != Animal.LowWaterLevel)
			{
				this.waterlevel = this.animal.Waterlevel;
			}
			if (!this.check_Water && this.waterlevel > this.animal.Main_Pivot_Point.y)
			{
				this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
				this.check_Water = true;
				this.animal.Swim = true;
				this.animal.Waterlevel = this.waterlevel;
			}
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x00023FAE File Offset: 0x000221AE
		public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.animal.AirControl)
			{
				this.AirControl();
			}
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x00110CA0 File Offset: 0x0010EEA0
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animal.PivotsRayInterval = this.PivotsRayInterval;
			this.animal.FallRayInterval = this.FallRayInterval;
			this.animal.WaterRayInterval = this.WaterRayInterval;
			this.animal.AirControlDir = Vector3.zero;
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x00110CF0 File Offset: 0x0010EEF0
		private void AirControl()
		{
			RaycastHit fallRayCast = this.animal.FallRayCast;
			if (Vector3.Angle(Vector3.up, fallRayCast.normal) > this.animal.maxAngleSlope)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			float y = this.rb.velocity.y;
			Vector3 rawDirection = this.animal.RawDirection;
			rawDirection.y = 0f;
			this.animal.AirControlDir = Vector3.Lerp(this.animal.AirControlDir, rawDirection, deltaTime * this.animal.airSmoothness);
			Debug.DrawRay(this.transform.position, this.transform.TransformDirection(this.animal.AirControlDir), Color.yellow);
			Vector3 vector = this.animal.AirControlDir * this.animal.AirForwardMultiplier;
			if (!this.animal.DirectionalMovement)
			{
				vector = this.transform.TransformDirection(vector);
			}
			vector.y = y;
			this.rb.velocity = vector;
		}

		// Token: 0x040033BD RID: 13245
		private RaycastHit FallRay;

		// Token: 0x040033BE RID: 13246
		[Tooltip("The Lower Fall animation will set to 1 if this distance the current distance to the ground")]
		public float LowerDistance;

		// Token: 0x040033BF RID: 13247
		private Animal animal;

		// Token: 0x040033C0 RID: 13248
		private Rigidbody rb;

		// Token: 0x040033C1 RID: 13249
		private float MaxHeight;

		// Token: 0x040033C2 RID: 13250
		private float FallBlend;

		// Token: 0x040033C3 RID: 13251
		private bool check_Water;

		// Token: 0x040033C4 RID: 13252
		private int PivotsRayInterval;

		// Token: 0x040033C5 RID: 13253
		private int FallRayInterval;

		// Token: 0x040033C6 RID: 13254
		private int WaterRayInterval;

		// Token: 0x040033C7 RID: 13255
		private int GroundLayer;

		// Token: 0x040033C8 RID: 13256
		private Transform transform;

		// Token: 0x040033C9 RID: 13257
		private Vector3 IncomingSpeed;

		// Token: 0x040033CA RID: 13258
		private float waterlevel;
	}
}
