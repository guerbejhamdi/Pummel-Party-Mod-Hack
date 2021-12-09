using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x02000725 RID: 1829
	public class MFreeLookCamera : MonoBehaviour
	{
		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x0600355D RID: 13661 RVA: 0x00024328 File Offset: 0x00022528
		public Transform Target
		{
			get
			{
				return this.m_Target;
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x0600355E RID: 13662 RVA: 0x00024330 File Offset: 0x00022530
		// (set) Token: 0x0600355F RID: 13663 RVA: 0x00024338 File Offset: 0x00022538
		public Transform Cam
		{
			get
			{
				return this.cam;
			}
			set
			{
				this.cam = value;
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06003560 RID: 13664 RVA: 0x00024341 File Offset: 0x00022541
		// (set) Token: 0x06003561 RID: 13665 RVA: 0x00024349 File Offset: 0x00022549
		public Transform Pivot
		{
			get
			{
				return this.pivot;
			}
			set
			{
				this.pivot = value;
			}
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x00113F10 File Offset: 0x00112110
		protected void Awake()
		{
			this.Cam = base.GetComponentInChildren<Camera>().transform;
			this.Pivot = this.Cam.parent;
			if (this.manager)
			{
				this.manager.SetCamera(this);
			}
			if (this.startState)
			{
				this.SetState(this.startState);
			}
			Cursor.lockState = (this.m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None);
			Cursor.visible = !this.m_LockCursor;
			this.m_PivotEulers = this.Pivot.rotation.eulerAngles;
			this.m_PivotTargetRot = this.Pivot.transform.localRotation;
			this.m_TransformTargetRot = base.transform.localRotation;
			this.inputSystem = DefaultInput.GetInputSystem(this.PlayerID);
			this.Horizontal.InputSystem = (this.Vertical.InputSystem = this.inputSystem);
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x00024352 File Offset: 0x00022552
		public virtual void SetState(FreeLookCameraState profile)
		{
			this.Pivot.localPosition = profile.PivotPos;
			this.Cam.localPosition = profile.CamPos;
			this.Cam.GetComponent<Camera>().fieldOfView = profile.CamFOV;
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x0002438C File Offset: 0x0002258C
		protected void FollowTarget(float deltaTime)
		{
			if (this.m_Target == null)
			{
				return;
			}
			base.transform.position = Vector3.Lerp(base.transform.position, this.m_Target.position, deltaTime * this.m_MoveSpeed);
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x00114000 File Offset: 0x00112200
		private void HandleRotationMovement()
		{
			if (Time.timeScale < 1E-45f)
			{
				return;
			}
			this.x = this.Horizontal.GetAxis;
			this.y = this.Vertical.GetAxis;
			this.m_LookAngle += this.x * this.m_TurnSpeed;
			this.m_TransformTargetRot = Quaternion.Euler(0f, this.m_LookAngle, 0f);
			this.m_TiltAngle -= this.y * this.m_TurnSpeed;
			this.m_TiltAngle = Mathf.Clamp(this.m_TiltAngle, -this.m_TiltMin, this.m_TiltMax);
			this.m_PivotTargetRot = Quaternion.Euler(this.m_TiltAngle, this.m_PivotEulers.y, this.m_PivotEulers.z);
			if (this.m_TurnSmoothing > 0f)
			{
				this.Pivot.localRotation = Quaternion.Slerp(this.Pivot.localRotation, this.m_PivotTargetRot, this.m_TurnSmoothing * Time.deltaTime);
				base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, this.m_TransformTargetRot, this.m_TurnSmoothing * Time.deltaTime);
				return;
			}
			this.Pivot.localRotation = this.m_PivotTargetRot;
			base.transform.localRotation = this.m_TransformTargetRot;
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x000243CB File Offset: 0x000225CB
		private void Update()
		{
			this.HandleRotationMovement();
			if (this.updateType == MFreeLookCamera.UpdateType.Update)
			{
				this.FollowTarget(Time.deltaTime);
			}
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x000243E7 File Offset: 0x000225E7
		private void FixedUpdate()
		{
			if (this.updateType == MFreeLookCamera.UpdateType.FixedUpdate)
			{
				this.FollowTarget(Time.fixedDeltaTime);
			}
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x000243FC File Offset: 0x000225FC
		private void LateUpdate()
		{
			if (this.updateType == MFreeLookCamera.UpdateType.LateUpdate)
			{
				this.FollowTarget(Time.deltaTime);
			}
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x00024412 File Offset: 0x00022612
		public virtual void SetTarget(Transform newTransform)
		{
			this.m_Target = newTransform;
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x0002441B File Offset: 0x0002261B
		public virtual void SetTarget(GameObject newGO)
		{
			this.m_Target = newGO.transform;
		}

		// Token: 0x0400348A RID: 13450
		[HideInInspector]
		public string PlayerID = "Player0";

		// Token: 0x0400348B RID: 13451
		[Space]
		public Transform m_Target;

		// Token: 0x0400348C RID: 13452
		public MFreeLookCamera.UpdateType updateType;

		// Token: 0x0400348D RID: 13453
		private Transform cam;

		// Token: 0x0400348E RID: 13454
		private Transform pivot;

		// Token: 0x0400348F RID: 13455
		public float m_MoveSpeed = 10f;

		// Token: 0x04003490 RID: 13456
		[Range(0f, 10f)]
		public float m_TurnSpeed = 10f;

		// Token: 0x04003491 RID: 13457
		public float m_TurnSmoothing = 10f;

		// Token: 0x04003492 RID: 13458
		public float m_TiltMax = 75f;

		// Token: 0x04003493 RID: 13459
		public float m_TiltMin = 45f;

		// Token: 0x04003494 RID: 13460
		public InputAxis Vertical = new InputAxis("Mouse Y", true, false);

		// Token: 0x04003495 RID: 13461
		public InputAxis Horizontal = new InputAxis("Mouse X", true, false);

		// Token: 0x04003496 RID: 13462
		[Space]
		public bool m_LockCursor;

		// Token: 0x04003497 RID: 13463
		[Space]
		public FreeLockCameraManager manager;

		// Token: 0x04003498 RID: 13464
		public FreeLookCameraState startState;

		// Token: 0x04003499 RID: 13465
		private float m_LookAngle;

		// Token: 0x0400349A RID: 13466
		private float m_TiltAngle;

		// Token: 0x0400349B RID: 13467
		private const float k_LookDistance = 100f;

		// Token: 0x0400349C RID: 13468
		private Vector3 m_PivotEulers;

		// Token: 0x0400349D RID: 13469
		private Quaternion m_PivotTargetRot;

		// Token: 0x0400349E RID: 13470
		private Quaternion m_TransformTargetRot;

		// Token: 0x0400349F RID: 13471
		private float x;

		// Token: 0x040034A0 RID: 13472
		private float y;

		// Token: 0x040034A1 RID: 13473
		private IInputSystem inputSystem;

		// Token: 0x02000726 RID: 1830
		public enum UpdateType
		{
			// Token: 0x040034A3 RID: 13475
			FixedUpdate,
			// Token: 0x040034A4 RID: 13476
			LateUpdate,
			// Token: 0x040034A5 RID: 13477
			Update
		}
	}
}
