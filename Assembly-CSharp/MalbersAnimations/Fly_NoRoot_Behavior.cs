using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000710 RID: 1808
	public class Fly_NoRoot_Behavior : StateMachineBehaviour
	{
		// Token: 0x06003508 RID: 13576 RVA: 0x001113CC File Offset: 0x0010F5CC
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.ResetAllValues();
			this.rb = animator.GetComponent<Rigidbody>();
			this.animal = animator.GetComponent<Animal>();
			this.BehaviourSpeed = this.animal.flySpeed;
			this.animal.RootMotion = true;
			this.transform = animator.transform;
			this.acceleration = 0f;
			this.vertical = this.animal.Speed;
			this.FallVector = ((this.animal.CurrentAnimState == AnimTag.Fall || this.animal.CurrentAnimState == AnimTag.Jump) ? this.rb.velocity : Vector3.zero);
			this.rb.constraints = RigidbodyConstraints.FreezeRotation;
			this.rb.useGravity = false;
			this.rb.drag = this.Drag;
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x001114A4 File Offset: 0x0010F6A4
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!this.animal.Fly)
			{
				return;
			}
			float num = 1f;
			if (animator.IsInTransition(layerIndex) && stateInfo.normalizedTime < 0.5f)
			{
				num = animator.GetAnimatorTransitionInfo(layerIndex).normalizedTime;
			}
			if (animator.IsInTransition(layerIndex) && stateInfo.normalizedTime > 0.5f)
			{
				num = 1f - animator.GetAnimatorTransitionInfo(layerIndex).normalizedTime;
			}
			this.deltaTime = Time.deltaTime;
			Vector3 eulerAngles = this.transform.eulerAngles;
			eulerAngles.x = (eulerAngles.z = 0f);
			this.transform.eulerAngles = eulerAngles;
			float num2 = (float)((this.animal.MovementAxis.z >= 0f) ? 1 : -1);
			this.Direction = Mathf.Lerp(this.Direction, Mathf.Clamp(this.animal.Direction, -1f, 1f), this.deltaTime * this.BehaviourSpeed.lerpRotation);
			Vector3 direction = new Vector3(0f, this.Direction * this.BehaviourSpeed.rotation * num2, 0f);
			Quaternion rhs = Quaternion.Euler(this.transform.InverseTransformDirection(direction));
			this.animal.DeltaRotation *= rhs;
			float movementUp = this.animal.MovementUp;
			this.vertical = Mathf.Lerp(this.vertical, Mathf.Clamp(this.animal.Speed, -1f, 1f), this.deltaTime * 6f);
			Vector3 vector = Vector3.zero;
			Vector3 a = this.animal.T_Forward;
			if (this.animal.DirectionalMovement)
			{
				vector = this.animal.RawDirection;
				if (this.animal.IgnoreYDir)
				{
					vector.y = 0f;
				}
				vector.Normalize();
				vector += this.transform.up * movementUp;
				if (vector.magnitude > 1f)
				{
					vector.Normalize();
				}
			}
			else
			{
				vector = this.transform.forward * this.vertical + this.transform.up * movementUp;
				if (vector.magnitude > 1f)
				{
					vector.Normalize();
				}
				if (this.animal.MovementAxis.z < 0f)
				{
				}
				a = vector;
			}
			this.forwardAceleration = Mathf.Lerp(this.forwardAceleration, vector.magnitude, this.deltaTime * this.BehaviourSpeed.lerpPosition);
			Vector3 vector2 = a * this.forwardAceleration * this.BehaviourSpeed.position * ((this.animal.Speed < 0f) ? 0.5f : 1f) * this.deltaTime;
			vector2 = Vector3.Lerp(Vector3.zero, vector2, num);
			if (this.CanNotSwim)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(this.animal.Main_Pivot_Point, -Vector2.up, out raycastHit, this.animal.Pivot_Multiplier * this.animal.ScaleFactor * this.animal.FallRayMultiplier, 16))
				{
					this.foundWater = true;
				}
				else
				{
					this.foundWater = false;
				}
			}
			if (this.foundWater && vector2.y < 0f)
			{
				vector2.y = 0.001f;
				this.animal.DeltaPosition.y = 0f;
				this.animal.MovementUp = 0f;
			}
			this.animal.DeltaPosition += vector2;
			if (this.animal.debug)
			{
				Debug.DrawRay(this.transform.position, vector * 2f, Color.yellow);
			}
			if ((double)vector.magnitude > 0.001)
			{
				float num3 = 90f - Vector3.Angle(Vector3.up, vector);
				float num4 = Mathf.Max(Mathf.Abs(this.animal.MovementAxis.y), Mathf.Abs(this.vertical));
				num3 = Mathf.Clamp(-num3, -this.Ylimit, this.Ylimit);
				this.PitchAngle = Mathf.Lerp(this.PitchAngle, num3, this.deltaTime * this.animal.upDownSmoothness * 2f);
				this.animal.DeltaRotation *= Quaternion.Euler(this.PitchAngle * num4 * num, 0f, 0f);
			}
			this.animal.DeltaRotation *= Quaternion.Euler(0f, 0f, -this.Bank * this.Direction);
			if (this.foundWater)
			{
				return;
			}
			if (this.FallVector != Vector3.zero)
			{
				this.animal.DeltaPosition += this.FallVector * this.deltaTime;
				this.FallVector = Vector3.Lerp(this.FallVector, Vector3.zero, this.deltaTime * this.FallRecovery);
			}
			if (this.UseDownAcceleration)
			{
				this.GravityAcceleration(vector);
			}
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x00111A08 File Offset: 0x0010FC08
		private void GravityAcceleration(Vector3 DirectionVector)
		{
			if ((double)this.animal.MovementAxis.y < -0.1)
			{
				this.acceleration = Mathf.Lerp(this.acceleration, this.acceleration + this.DownAcceleration, this.deltaTime);
			}
			else
			{
				float num = this.acceleration - this.DownAcceleration;
				if (num < 0f)
				{
					num = 0f;
				}
				this.acceleration = Mathf.Lerp(this.acceleration, num, this.deltaTime);
			}
			this.animal.DeltaPosition += DirectionVector * (this.acceleration * this.deltaTime);
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x00111AB4 File Offset: 0x0010FCB4
		private void ResetAllValues()
		{
			this.deltaTime = (this.acceleration = (this.forwardAceleration = (this.PitchAngle = (this.Direction = 0f))));
		}

		// Token: 0x040033E9 RID: 13289
		[Range(0f, 90f)]
		[Tooltip("Adds Banking to the Fly animation when turning")]
		public float Bank = 30f;

		// Token: 0x040033EA RID: 13290
		[Range(0f, 90f)]
		[Tooltip("Top Angle the Animal Can go UP or Down ")]
		public float Ylimit = 80f;

		// Token: 0x040033EB RID: 13291
		public float Drag = 5f;

		// Token: 0x040033EC RID: 13292
		[Space]
		public bool UseDownAcceleration = true;

		// Token: 0x040033ED RID: 13293
		public float DownAcceleration = 3f;

		// Token: 0x040033EE RID: 13294
		public float FallRecovery = 1.5f;

		// Token: 0x040033EF RID: 13295
		[Space]
		public bool CanNotSwim;

		// Token: 0x040033F0 RID: 13296
		protected float acceleration;

		// Token: 0x040033F1 RID: 13297
		protected Rigidbody rb;

		// Token: 0x040033F2 RID: 13298
		protected Animal animal;

		// Token: 0x040033F3 RID: 13299
		protected Transform transform;

		// Token: 0x040033F4 RID: 13300
		protected float Shift;

		// Token: 0x040033F5 RID: 13301
		protected float Direction;

		// Token: 0x040033F6 RID: 13302
		protected float deltaTime;

		// Token: 0x040033F7 RID: 13303
		private Vector3 FallVector;

		// Token: 0x040033F8 RID: 13304
		protected float forwardAceleration;

		// Token: 0x040033F9 RID: 13305
		protected Speeds BehaviourSpeed;

		// Token: 0x040033FA RID: 13306
		private float PitchAngle;

		// Token: 0x040033FB RID: 13307
		private float vertical;

		// Token: 0x040033FC RID: 13308
		private bool foundWater;
	}
}
