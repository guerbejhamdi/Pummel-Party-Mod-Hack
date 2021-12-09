using System;
using MalbersAnimations.Utilities;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000711 RID: 1809
	public class JumpBehaviour : StateMachineBehaviour
	{
		// Token: 0x0600350D RID: 13581 RVA: 0x00111B44 File Offset: 0x0010FD44
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.animal = animator.GetComponent<Animal>();
			this.rb = animator.GetComponent<Rigidbody>();
			this.transform = animator.transform;
			this.animal.RootMotion = true;
			this.animal.IsInAir = true;
			this.jumpPoint = this.transform.position.y;
			this.animal.InAir(true);
			this.animal.SetIntID(0);
			this.animal.OnJump.Invoke();
			this.Rb_Y_Speed = 0f;
			this.cast_WillFall_Ray = false;
			Vector3 rawDirection = this.animal.RawDirection;
			rawDirection.y = 0f;
			this.animal.AirControlDir = rawDirection;
			this.Can_Add_ExtraJump = ((this.JumpMultiplier > 0f && this.animal.JumpHeightMultiplier > 0f) || (this.ForwardMultiplier > 0f && this.animal.AirForwardMultiplier > 0f));
			this.ExtraJump = Vector3.up * this.JumpMultiplier * this.animal.JumpHeightMultiplier + this.animal.T_ForwardNoY * this.ForwardMultiplier * this.animal.AirForwardMultiplier;
			this.JumpSmoothPressed = 1f;
			this.JumpPressed = true;
			if (this.animal.JumpPress)
			{
				this.Can_Add_ExtraJump = (this.JumpPressed = this.animal.Jump);
			}
			this.JumpEnd = false;
			animator.SetFloat(Hash.IDFloat, 1f);
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x00111CEC File Offset: 0x0010FEEC
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			bool flag = animator.IsInTransition(layerIndex);
			bool flag2 = flag && stateInfo.normalizedTime > 0.5f;
			if (this.animal.AnimState != AnimTag.Jump)
			{
				return;
			}
			if (this.rb && flag2 && animator.GetNextAnimatorStateInfo(layerIndex).tagHash == AnimTag.Fly)
			{
				float normalizedTime = animator.GetAnimatorTransitionInfo(layerIndex).normalizedTime;
				Vector3 velocity = this.rb.velocity;
				if (this.Rb_Y_Speed < velocity.y)
				{
					this.Rb_Y_Speed = velocity.y;
				}
				velocity.y = Mathf.Lerp(this.Rb_Y_Speed, 0f, normalizedTime);
				this.rb.velocity = velocity;
			}
			if (flag2)
			{
				return;
			}
			if (this.JumpPressed)
			{
				this.JumpPressed = this.animal.Jump;
			}
			if (this.animal.CanDoubleJump && stateInfo.normalizedTime >= this.DoubleJumpTime && this.animal.Double_Jump != 1 && this.animal.Jump)
			{
				this.animal.Double_Jump++;
				this.animal.SetIntID(112);
				return;
			}
			if (!flag && this.Can_Add_ExtraJump && !this.JumpEnd)
			{
				if (this.animal.JumpPress)
				{
					int num = this.JumpPressed ? 1 : 0;
					this.JumpSmoothPressed = Mathf.Lerp(this.JumpSmoothPressed, (float)num, Time.deltaTime * 5f);
				}
				this.animal.DeltaPosition += this.ExtraJump * Time.deltaTime * this.JumpSmoothPressed;
			}
			if (stateInfo.normalizedTime >= this.willFall && !this.cast_WillFall_Ray)
			{
				this.Can_Fall(stateInfo.normalizedTime);
				this.cast_WillFall_Ray = true;
			}
			if (this.animal.FrameCounter % this.animal.FallRayInterval == 0)
			{
				this.Can_Jump_on_Cliff(stateInfo.normalizedTime);
			}
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x00024050 File Offset: 0x00022250
		public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.animal.AirControl)
			{
				this.AirControl();
			}
		}

		// Token: 0x06003510 RID: 13584 RVA: 0x00111EF4 File Offset: 0x001100F4
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
			if (currentAnimatorStateInfo.tagHash == AnimTag.Fly)
			{
				this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
			}
			else if (currentAnimatorStateInfo.tagHash != AnimTag.Fall && currentAnimatorStateInfo.tagHash != AnimTag.Jump)
			{
				this.animal.IsInAir = false;
			}
			if (currentAnimatorStateInfo.tagHash == AnimTag.JumpEnd)
			{
				this.JumpEnd = true;
			}
		}

		// Token: 0x06003511 RID: 13585 RVA: 0x00111F8C File Offset: 0x0011018C
		private void Can_Fall(float normalizedTime)
		{
			Debug.DrawRay(this.animal.Pivot_fall, -this.transform.up * this.animal.Pivot_Multiplier * this.fallRay, Color.red);
			if (this.MinJumpLand > 0f)
			{
				if (!Physics.Raycast(this.animal.Pivot_fall, -this.transform.up, out this.JumpRay, this.animal.Pivot_Multiplier * this.fallRay, this.animal.GroundLayer))
				{
					this.animal.SetIntID(111);
					return;
				}
				float num = Vector3.Distance(this.animal.Pivot_fall, this.JumpRay.point);
				float num2 = Vector3.Angle(Vector3.up, this.JumpRay.normal);
				if (this.animal.debug)
				{
					Debug.Log("Min Distance to complete the Jump: " + num.ToString());
				}
				if (this.MinJumpLand * this.animal.ScaleFactor < num || num2 > this.animal.maxAngleSlope)
				{
					this.animal.SetIntID(111);
					MalbersTools.DebugTriangle(this.JumpRay.point, 0.1f, Color.yellow);
					return;
				}
			}
			else if (Physics.Raycast(this.animal.Pivot_fall, -this.transform.up, out this.JumpRay, this.animal.Pivot_Multiplier * this.fallRay, this.animal.GroundLayer))
			{
				if (this.jumpPoint - this.JumpRay.point.y <= this.stepHeight * this.animal.ScaleFactor && Vector3.Angle(this.JumpRay.normal, Vector3.up) < this.animal.maxAngleSlope)
				{
					MalbersTools.DebugTriangle(this.JumpRay.point, 0.1f, Color.red);
					return;
				}
				this.animal.SetIntID(111);
				MalbersTools.DebugTriangle(this.JumpRay.point, 0.1f, Color.yellow);
				return;
			}
			else
			{
				this.animal.SetIntID(111);
				MalbersTools.DebugPlane(this.animal.Pivot_fall - this.transform.up * this.animal.Pivot_Multiplier * this.fallRay, 0.1f, Color.red, false);
			}
		}

		// Token: 0x06003512 RID: 13586 RVA: 0x00112218 File Offset: 0x00110418
		private void Can_Jump_on_Cliff(float normalizedTime)
		{
			if (normalizedTime >= this.Cliff.minValue && normalizedTime <= this.Cliff.maxValue)
			{
				if (Physics.Raycast(this.animal.Main_Pivot_Point, -this.transform.up, out this.JumpRay, this.CliffRay * this.animal.ScaleFactor, this.animal.GroundLayer))
				{
					if (Vector3.Angle(this.JumpRay.normal, Vector3.up) < this.animal.maxAngleSlope)
					{
						if (this.animal.debug)
						{
							Debug.DrawLine(this.animal.Main_Pivot_Point, this.JumpRay.point, Color.black);
							MalbersTools.DebugTriangle(this.JumpRay.point, 0.1f, Color.black);
						}
						this.animal.SetIntID(110);
						return;
					}
				}
				else if (this.animal.debug)
				{
					Debug.DrawRay(this.animal.Main_Pivot_Point, -this.transform.up * this.CliffRay * this.animal.ScaleFactor, Color.black);
					MalbersTools.DebugPlane(this.animal.Main_Pivot_Point - this.transform.up * this.CliffRay * this.animal.ScaleFactor, 0.1f, Color.black, false);
				}
			}
		}

		// Token: 0x06003513 RID: 13587 RVA: 0x001123A4 File Offset: 0x001105A4
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
			this.animal.AirControlDir = Vector3.Lerp(this.animal.AirControlDir, rawDirection * this.ForwardMultiplier, deltaTime * this.animal.airSmoothness);
			Debug.DrawRay(this.transform.position, this.transform.TransformDirection(this.animal.AirControlDir), Color.yellow);
			Vector3 vector = this.animal.AirControlDir * this.animal.AirForwardMultiplier;
			if (!this.animal.DirectionalMovement)
			{
				vector = this.transform.TransformDirection(vector);
			}
			vector.y = y;
			this.rb.velocity = vector;
		}

		// Token: 0x040033FD RID: 13309
		[Header("Checking Fall")]
		[Tooltip("Ray Length to check if the ground is at the same level all the time")]
		public float fallRay = 1.7f;

		// Token: 0x040033FE RID: 13310
		[Tooltip("Terrain difference to be sure the animal will fall ")]
		public float stepHeight = 0.1f;

		// Token: 0x040033FF RID: 13311
		[Tooltip("Min Distance to land and End the Jump")]
		public float MinJumpLand;

		// Token: 0x04003400 RID: 13312
		[Tooltip("Animation normalized time to change to fall animation if the ray checks if the animal is falling ")]
		[Range(0f, 1f)]
		public float willFall = 0.7f;

		// Token: 0x04003401 RID: 13313
		[Header("Jump on Higher Ground")]
		[Tooltip("Range to Calcultate if we can land on Higher ground")]
		[MinMaxRange(0f, 1f)]
		public RangedFloat Cliff = new RangedFloat(0.5f, 0.65f);

		// Token: 0x04003402 RID: 13314
		public float CliffRay = 0.6f;

		// Token: 0x04003403 RID: 13315
		[Space]
		[Header("Add more Height and Distance to the Jump")]
		public float JumpMultiplier = 1f;

		// Token: 0x04003404 RID: 13316
		public float ForwardMultiplier = 1f;

		// Token: 0x04003405 RID: 13317
		[Space]
		[Header("Double Jump")]
		[Tooltip("Enable the Double Jump after x normalized time of the animation")]
		[Range(0f, 1f)]
		public float DoubleJumpTime = 0.33f;

		// Token: 0x04003406 RID: 13318
		private Animal animal;

		// Token: 0x04003407 RID: 13319
		private Rigidbody rb;

		// Token: 0x04003408 RID: 13320
		private Transform transform;

		// Token: 0x04003409 RID: 13321
		private bool Can_Add_ExtraJump;

		// Token: 0x0400340A RID: 13322
		private Vector3 ExtraJump;

		// Token: 0x0400340B RID: 13323
		private bool JumpPressed;

		// Token: 0x0400340C RID: 13324
		private float jumpPoint;

		// Token: 0x0400340D RID: 13325
		private float Rb_Y_Speed;

		// Token: 0x0400340E RID: 13326
		private RaycastHit JumpRay;

		// Token: 0x0400340F RID: 13327
		private float JumpSmoothPressed = 1f;

		// Token: 0x04003410 RID: 13328
		private bool JumpEnd;

		// Token: 0x04003411 RID: 13329
		private bool cast_WillFall_Ray;
	}
}
