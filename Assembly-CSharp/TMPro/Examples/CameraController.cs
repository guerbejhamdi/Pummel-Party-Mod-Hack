using System;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x020005BA RID: 1466
	public class CameraController : MonoBehaviour
	{
		// Token: 0x060025E2 RID: 9698 RVA: 0x000E480C File Offset: 0x000E2A0C
		private void Awake()
		{
			if (QualitySettings.vSyncCount > 0)
			{
				Application.targetFrameRate = 60;
			}
			else
			{
				Application.targetFrameRate = -1;
			}
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			{
				Input.simulateMouseWithTouches = false;
			}
			this.cameraTransform = base.transform;
			this.previousSmoothing = this.MovementSmoothing;
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x0001B0B4 File Offset: 0x000192B4
		private void Start()
		{
			if (this.CameraTarget == null)
			{
				this.dummyTarget = new GameObject("Camera Target").transform;
				this.CameraTarget = this.dummyTarget;
			}
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x000E4860 File Offset: 0x000E2A60
		private void LateUpdate()
		{
			this.GetPlayerInput();
			if (this.CameraTarget != null)
			{
				if (this.CameraMode == CameraController.CameraModes.Isometric)
				{
					this.desiredPosition = this.CameraTarget.position + Quaternion.Euler(this.ElevationAngle, this.OrbitalAngle, 0f) * new Vector3(0f, 0f, -this.FollowDistance);
				}
				else if (this.CameraMode == CameraController.CameraModes.Follow)
				{
					this.desiredPosition = this.CameraTarget.position + this.CameraTarget.TransformDirection(Quaternion.Euler(this.ElevationAngle, this.OrbitalAngle, 0f) * new Vector3(0f, 0f, -this.FollowDistance));
				}
				if (this.MovementSmoothing)
				{
					this.cameraTransform.position = Vector3.SmoothDamp(this.cameraTransform.position, this.desiredPosition, ref this.currentVelocity, this.MovementSmoothingValue * Time.fixedDeltaTime);
				}
				else
				{
					this.cameraTransform.position = this.desiredPosition;
				}
				if (this.RotationSmoothing)
				{
					this.cameraTransform.rotation = Quaternion.Lerp(this.cameraTransform.rotation, Quaternion.LookRotation(this.CameraTarget.position - this.cameraTransform.position), this.RotationSmoothingValue * Time.deltaTime);
					return;
				}
				this.cameraTransform.LookAt(this.CameraTarget);
			}
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000E49E0 File Offset: 0x000E2BE0
		private void GetPlayerInput()
		{
			this.moveVector = Vector3.zero;
			this.mouseWheel = Input.GetAxis("Mouse ScrollWheel");
			float num = (float)Input.touchCount;
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || num > 0f)
			{
				this.mouseWheel *= 10f;
				if (Input.GetKeyDown(KeyCode.I))
				{
					this.CameraMode = CameraController.CameraModes.Isometric;
				}
				if (Input.GetKeyDown(KeyCode.F))
				{
					this.CameraMode = CameraController.CameraModes.Follow;
				}
				if (Input.GetKeyDown(KeyCode.S))
				{
					this.MovementSmoothing = !this.MovementSmoothing;
				}
				if (Input.GetMouseButton(1))
				{
					this.mouseY = Input.GetAxis("Mouse Y");
					this.mouseX = Input.GetAxis("Mouse X");
					if (this.mouseY > 0.01f || this.mouseY < -0.01f)
					{
						this.ElevationAngle -= this.mouseY * this.MoveSensitivity;
						this.ElevationAngle = Mathf.Clamp(this.ElevationAngle, this.MinElevationAngle, this.MaxElevationAngle);
					}
					if (this.mouseX > 0.01f || this.mouseX < -0.01f)
					{
						this.OrbitalAngle += this.mouseX * this.MoveSensitivity;
						if (this.OrbitalAngle > 360f)
						{
							this.OrbitalAngle -= 360f;
						}
						if (this.OrbitalAngle < 0f)
						{
							this.OrbitalAngle += 360f;
						}
					}
				}
				if (num == 1f && Input.GetTouch(0).phase == TouchPhase.Moved)
				{
					Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
					if (deltaPosition.y > 0.01f || deltaPosition.y < -0.01f)
					{
						this.ElevationAngle -= deltaPosition.y * 0.1f;
						this.ElevationAngle = Mathf.Clamp(this.ElevationAngle, this.MinElevationAngle, this.MaxElevationAngle);
					}
					if (deltaPosition.x > 0.01f || deltaPosition.x < -0.01f)
					{
						this.OrbitalAngle += deltaPosition.x * 0.1f;
						if (this.OrbitalAngle > 360f)
						{
							this.OrbitalAngle -= 360f;
						}
						if (this.OrbitalAngle < 0f)
						{
							this.OrbitalAngle += 360f;
						}
					}
				}
				RaycastHit raycastHit;
				if (Input.GetMouseButton(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 300f, 23552))
				{
					if (raycastHit.transform == this.CameraTarget)
					{
						this.OrbitalAngle = 0f;
					}
					else
					{
						this.CameraTarget = raycastHit.transform;
						this.OrbitalAngle = 0f;
						this.MovementSmoothing = this.previousSmoothing;
					}
				}
				if (Input.GetMouseButton(2))
				{
					if (this.dummyTarget == null)
					{
						this.dummyTarget = new GameObject("Camera Target").transform;
						this.dummyTarget.position = this.CameraTarget.position;
						this.dummyTarget.rotation = this.CameraTarget.rotation;
						this.CameraTarget = this.dummyTarget;
						this.previousSmoothing = this.MovementSmoothing;
						this.MovementSmoothing = false;
					}
					else if (this.dummyTarget != this.CameraTarget)
					{
						this.dummyTarget.position = this.CameraTarget.position;
						this.dummyTarget.rotation = this.CameraTarget.rotation;
						this.CameraTarget = this.dummyTarget;
						this.previousSmoothing = this.MovementSmoothing;
						this.MovementSmoothing = false;
					}
					this.mouseY = Input.GetAxis("Mouse Y");
					this.mouseX = Input.GetAxis("Mouse X");
					this.moveVector = this.cameraTransform.TransformDirection(this.mouseX, this.mouseY, 0f);
					this.dummyTarget.Translate(-this.moveVector, Space.World);
				}
			}
			if (num == 2f)
			{
				Touch touch = Input.GetTouch(0);
				Touch touch2 = Input.GetTouch(1);
				Vector2 a = touch.position - touch.deltaPosition;
				Vector2 b = touch2.position - touch2.deltaPosition;
				float magnitude = (a - b).magnitude;
				float magnitude2 = (touch.position - touch2.position).magnitude;
				float num2 = magnitude - magnitude2;
				if (num2 > 0.01f || num2 < -0.01f)
				{
					this.FollowDistance += num2 * 0.25f;
					this.FollowDistance = Mathf.Clamp(this.FollowDistance, this.MinFollowDistance, this.MaxFollowDistance);
				}
			}
			if (this.mouseWheel < -0.01f || this.mouseWheel > 0.01f)
			{
				this.FollowDistance -= this.mouseWheel * 5f;
				this.FollowDistance = Mathf.Clamp(this.FollowDistance, this.MinFollowDistance, this.MaxFollowDistance);
			}
		}

		// Token: 0x04002964 RID: 10596
		private Transform cameraTransform;

		// Token: 0x04002965 RID: 10597
		private Transform dummyTarget;

		// Token: 0x04002966 RID: 10598
		public Transform CameraTarget;

		// Token: 0x04002967 RID: 10599
		public float FollowDistance = 30f;

		// Token: 0x04002968 RID: 10600
		public float MaxFollowDistance = 100f;

		// Token: 0x04002969 RID: 10601
		public float MinFollowDistance = 2f;

		// Token: 0x0400296A RID: 10602
		public float ElevationAngle = 30f;

		// Token: 0x0400296B RID: 10603
		public float MaxElevationAngle = 85f;

		// Token: 0x0400296C RID: 10604
		public float MinElevationAngle;

		// Token: 0x0400296D RID: 10605
		public float OrbitalAngle;

		// Token: 0x0400296E RID: 10606
		public CameraController.CameraModes CameraMode;

		// Token: 0x0400296F RID: 10607
		public bool MovementSmoothing = true;

		// Token: 0x04002970 RID: 10608
		public bool RotationSmoothing;

		// Token: 0x04002971 RID: 10609
		private bool previousSmoothing;

		// Token: 0x04002972 RID: 10610
		public float MovementSmoothingValue = 25f;

		// Token: 0x04002973 RID: 10611
		public float RotationSmoothingValue = 5f;

		// Token: 0x04002974 RID: 10612
		public float MoveSensitivity = 2f;

		// Token: 0x04002975 RID: 10613
		private Vector3 currentVelocity = Vector3.zero;

		// Token: 0x04002976 RID: 10614
		private Vector3 desiredPosition;

		// Token: 0x04002977 RID: 10615
		private float mouseX;

		// Token: 0x04002978 RID: 10616
		private float mouseY;

		// Token: 0x04002979 RID: 10617
		private Vector3 moveVector;

		// Token: 0x0400297A RID: 10618
		private float mouseWheel;

		// Token: 0x0400297B RID: 10619
		private const string event_SmoothingValue = "Slider - Smoothing Value";

		// Token: 0x0400297C RID: 10620
		private const string event_FollowDistance = "Slider - Camera Zoom";

		// Token: 0x020005BB RID: 1467
		public enum CameraModes
		{
			// Token: 0x0400297E RID: 10622
			Follow,
			// Token: 0x0400297F RID: 10623
			Isometric,
			// Token: 0x04002980 RID: 10624
			Free
		}
	}
}
