using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x0200071E RID: 1822
	public class UnderWaterBehaviour : StateMachineBehaviour
	{
		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06003538 RID: 13624 RVA: 0x000241EA File Offset: 0x000223EA
		// (set) Token: 0x06003539 RID: 13625 RVA: 0x000241F2 File Offset: 0x000223F2
		public float PitchAngle { get; private set; }

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x0600353A RID: 13626 RVA: 0x000241FB File Offset: 0x000223FB
		// (set) Token: 0x0600353B RID: 13627 RVA: 0x00024203 File Offset: 0x00022403
		public bool Default_UseShift { get; private set; }

		// Token: 0x0600353C RID: 13628 RVA: 0x00113008 File Offset: 0x00111208
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.ResetAllValues();
			this.rb = animator.GetComponent<Rigidbody>();
			this.animal = animator.GetComponent<Animal>();
			this.animal.RootMotion = true;
			this.transform = animator.transform;
			this.rb.constraints = RigidbodyConstraints.FreezeRotation;
			this.rb.useGravity = false;
			this.Default_UseShift = this.animal.UseShift;
			this.animal.UseShift = false;
			this.WaterLayer = LayerMask.GetMask(new string[]
			{
				"Water"
			});
			this.Speed = this.animal.underWaterSpeed;
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x0002420C File Offset: 0x0002240C
		private void ResetAllValues()
		{
			this.Shift = 0f;
			this.deltaTime = 0f;
			this.Direction = 0f;
			this.forwardAceleration = 0f;
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x001130AC File Offset: 0x001112AC
		public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!this.animal.CanGoUnderWater || !this.animal.Underwater)
			{
				return;
			}
			float t = 1f;
			if (animator.IsInTransition(layerIndex) && stateInfo.normalizedTime < 0.5f)
			{
				t = animator.GetAnimatorTransitionInfo(layerIndex).normalizedTime;
			}
			if (animator.IsInTransition(layerIndex) && stateInfo.normalizedTime > 0.5f)
			{
				t = 1f - animator.GetAnimatorTransitionInfo(layerIndex).normalizedTime;
			}
			this.deltaTime = Time.deltaTime;
			if (this.useShift)
			{
				this.Shift = Mathf.Lerp(this.Shift, this.animal.Shift ? this.ShiftMultiplier : 1f, this.Speed.lerpPosition * this.deltaTime);
			}
			if (this.animal.Up)
			{
				this.animal.Down = false;
			}
			Vector3 eulerAngles = this.transform.eulerAngles;
			eulerAngles.x = (eulerAngles.z = 0f);
			this.transform.eulerAngles = eulerAngles;
			float num = (float)((this.animal.MovementAxis.z >= 0f) ? 1 : -1);
			this.Direction = Mathf.Lerp(this.Direction, Mathf.Clamp(this.animal.Direction, -1f, 1f), this.deltaTime * this.Speed.lerpRotation);
			Vector3 direction = new Vector3(0f, this.Direction * this.Speed.rotation * num, 0f);
			Quaternion rhs = Quaternion.Euler(this.transform.InverseTransformDirection(direction));
			this.animal.DeltaRotation *= rhs;
			float movementUp = this.animal.MovementUp;
			float num2 = Mathf.Clamp(this.animal.Speed, -1f, 1f);
			Vector3 vector = Vector3.zero;
			Vector3 a = this.animal.T_Forward;
			if (this.animal.DirectionalMovement)
			{
				vector = this.animal.RawDirection;
				vector += this.transform.up * movementUp;
			}
			else
			{
				vector = this.transform.forward * num2 + this.transform.up * movementUp;
				if (vector.magnitude > 1f)
				{
					vector.Normalize();
				}
				if (this.animal.MovementAxis.z < 0f)
				{
				}
				a = vector;
			}
			this.forwardAceleration = Mathf.Lerp(this.forwardAceleration, vector.magnitude, this.deltaTime * this.Speed.lerpPosition);
			Vector3 b = a * this.forwardAceleration * this.Speed.position * this.Shift * ((this.animal.Speed < 0f) ? 0.5f : 1f) * this.deltaTime;
			b = Vector3.Lerp(Vector3.zero, b, t);
			this.animal.DeltaPosition += b;
			if ((double)vector.magnitude > 0.001)
			{
				float num3 = 90f - Vector3.Angle(Vector3.up, vector);
				float num4 = Mathf.Max(Mathf.Abs(this.animal.MovementAxis.y), Mathf.Abs(num2));
				num3 = Mathf.Clamp(-num3, -this.Ylimit, this.Ylimit);
				this.PitchAngle = Mathf.Lerp(this.PitchAngle, num3, this.deltaTime * this.animal.upDownSmoothness * 2f);
				this.transform.Rotate(Mathf.Clamp(this.PitchAngle, -this.Ylimit, this.Ylimit) * num4, 0f, 0f, Space.Self);
			}
			if (this.animal.debug)
			{
				Debug.DrawRay(this.transform.position, vector * 2f, Color.yellow);
			}
			this.transform.Rotate(0f, 0f, -this.Bank * Mathf.Clamp(this.Direction, -1f, 1f), Space.Self);
			this.CheckExitUnderWater();
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x0002423A File Offset: 0x0002243A
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animal.UseShift = this.Default_UseShift;
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x0011351C File Offset: 0x0011171C
		protected void CheckExitUnderWater()
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.transform.position + new Vector3(0f, (this.animal.height - this.animal.waterLine) * this.animal.ScaleFactor, 0f), -Vector3.up, out raycastHit, this.animal.ScaleFactor, this.WaterLayer) && !this.animal.Down)
			{
				this.animal.Underwater = false;
				this.animal.RootMotion = true;
				this.rb.useGravity = true;
				this.rb.drag = 0f;
				this.rb.constraints = Animal.Still_Constraints;
				this.animal.MovementAxis = new Vector3(this.animal.MovementAxis.x, 0f, this.animal.MovementAxis.z);
			}
		}

		// Token: 0x04003453 RID: 13395
		[Range(0f, 90f)]
		public float Bank;

		// Token: 0x04003454 RID: 13396
		[Range(0f, 90f)]
		public float Ylimit = 87f;

		// Token: 0x04003455 RID: 13397
		[Space]
		public bool useShift = true;

		// Token: 0x04003456 RID: 13398
		public float ShiftMultiplier = 2f;

		// Token: 0x04003457 RID: 13399
		[Space]
		protected Rigidbody rb;

		// Token: 0x04003458 RID: 13400
		protected Animal animal;

		// Token: 0x04003459 RID: 13401
		protected Transform transform;

		// Token: 0x0400345A RID: 13402
		protected float Shift;

		// Token: 0x0400345B RID: 13403
		protected float deltaTime;

		// Token: 0x0400345C RID: 13404
		private Speeds Speed;

		// Token: 0x0400345D RID: 13405
		private int WaterLayer;

		// Token: 0x0400345E RID: 13406
		private float Direction;

		// Token: 0x0400345F RID: 13407
		private float forwardAceleration;
	}
}
