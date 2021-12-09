using System;
using Rewired;
using UnityEngine;

// Token: 0x020003D2 RID: 978
public class ThirdPersonCamera : MonoBehaviour
{
	// Token: 0x06001A3A RID: 6714 RVA: 0x00013595 File Offset: 0x00011795
	private void Awake()
	{
		this.targetCamera = base.GetComponentInChildren<Camera>();
		this.controller = base.GetComponent<CharacterController>();
		this.curCamDistance = this.maxCamDistance;
		this.positionalTarget = base.transform;
		this.rotationalTarget = base.transform;
	}

	// Token: 0x06001A3B RID: 6715 RVA: 0x000AE294 File Offset: 0x000AC494
	public void RotateCamera()
	{
		if (!this.cameraLocked)
		{
			Vector2 vector = Vector2.zero;
			if (!GameManager.IsGamePaused && this.RewiredPlayer != null)
			{
				vector = new Vector2(this.RewiredPlayer.GetAxis(InputActions.ThirdPersonLookHorizontal), this.RewiredPlayer.GetAxis(InputActions.ThirdPersonLookVertical));
			}
			float num = this.mouseSensitivity;
			if (this.RewiredPlayer != null && this.RewiredPlayer.controllers.GetLastActiveController() != null && this.RewiredPlayer.controllers.GetLastActiveController().type == ControllerType.Joystick)
			{
				num = this.joystickSensitivity;
				vector *= Time.deltaTime;
			}
			this.zRotation -= vector.y * this.zSpeed * num;
			this.yRotation += vector.x * this.camRotationSpeed * num;
			if (this.yRotation > 360f)
			{
				this.yRotation -= 360f;
			}
			else if (this.yRotation < 0f)
			{
				this.yRotation += 360f;
			}
		}
		this.zRotation = Mathf.Clamp(this.zRotation, this.zRotMin, this.zRotMax);
		this.lookPos = Quaternion.Euler(this.zRotation, this.yRotation, 0f) * Vector3.forward;
		base.transform.rotation = Quaternion.Euler(0f, this.yRotation, 0f);
	}

	// Token: 0x06001A3C RID: 6716 RVA: 0x000AE410 File Offset: 0x000AC610
	public void UpdateCamera()
	{
		float num = this.controller.height / 2f + this.camOffsetY;
		float num2 = this.zRotMax - this.zRotMin;
		float num3 = this.zRotation - this.zRotMin;
		float num4 = num * (-1f + num3 / (num2 / 2f));
		Vector3 position = this.positionalTarget.position;
		Vector3 vector = position - base.transform.forward * this.maxCamDistance + base.transform.right * this.camOffsetZ + new Vector3(0f, this.camOffsetY + num4, 0f);
		Vector3 normalized = (vector - position).normalized;
		RaycastHit raycastHit = default(RaycastHit);
		float magnitude = (vector - position).magnitude;
		if (Physics.Raycast(position, normalized, out raycastHit, this.curCamDistance + this.wallOffset + 0.05f, this.rayCastMask))
		{
			if (raycastHit.distance >= magnitude)
			{
				this.curCamDistance = magnitude;
				this.hitWall = false;
			}
			else
			{
				this.curCamDistance = raycastHit.distance - this.wallOffset;
				this.hitWall = true;
			}
			vector = position + this.curCamDistance * normalized;
		}
		else
		{
			if (this.curCamDistance < magnitude && this.hitWall)
			{
				this.curCamDistance += this.camDRecoverySpeed * Time.deltaTime;
				this.curCamDistance = Mathf.Clamp(this.curCamDistance, this.minCamDistance, magnitude);
			}
			else
			{
				this.curCamDistance = magnitude;
				this.hitWall = false;
			}
			vector = position + this.curCamDistance * normalized;
		}
		Vector3 worldPosition = vector + base.transform.forward * (this.maxCamDistance + this.lookForwardOffset) + new Vector3(0f, -num4 * this.lookScale, 0f);
		this.targetCamera.transform.parent.position = vector;
		this.targetCamera.transform.parent.LookAt(worldPosition);
	}

	// Token: 0x06001A3D RID: 6717 RVA: 0x000AE644 File Offset: 0x000AC844
	public Ray GetScreenPointRay()
	{
		float x = this.targetCamera.pixelRect.x + this.targetCamera.pixelRect.width / 2f;
		float y = this.targetCamera.pixelRect.y + this.targetCamera.pixelRect.height / 2f;
		return this.targetCamera.ScreenPointToRay(new Vector3(x, y, 0f));
	}

	// Token: 0x06001A3E RID: 6718 RVA: 0x000AE6C4 File Offset: 0x000AC8C4
	public Ray GetScreenPointRayOffset()
	{
		Vector3 pos = this.targetCamera.WorldToScreenPoint(this.positionalTarget.transform.position + new Vector3(0f, 1f, 0f));
		return this.targetCamera.ScreenPointToRay(pos);
	}

	// Token: 0x06001A3F RID: 6719 RVA: 0x000135D3 File Offset: 0x000117D3
	public Vector3 GetPosition()
	{
		return this.targetCamera.transform.position;
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x000135E5 File Offset: 0x000117E5
	public Vector3 GetLookPos()
	{
		return this.lookPos;
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x000135ED File Offset: 0x000117ED
	public void SetTargetCamera(Camera target)
	{
		this.targetCamera = target;
	}

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06001A42 RID: 6722 RVA: 0x000135F6 File Offset: 0x000117F6
	public Camera TargetCamera
	{
		get
		{
			return this.targetCamera;
		}
	}

	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x06001A43 RID: 6723 RVA: 0x000135FE File Offset: 0x000117FE
	// (set) Token: 0x06001A44 RID: 6724 RVA: 0x00013606 File Offset: 0x00011806
	public Transform PositionalTargetTransform
	{
		get
		{
			return this.positionalTarget;
		}
		set
		{
			this.targetCamera.transform.parent.SetParent(value);
			this.positionalTarget = value;
		}
	}

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x06001A45 RID: 6725 RVA: 0x000135FE File Offset: 0x000117FE
	// (set) Token: 0x06001A46 RID: 6726 RVA: 0x00013625 File Offset: 0x00011825
	public Transform RotationalTargetTransform
	{
		get
		{
			return this.positionalTarget;
		}
		set
		{
			this.rotationalTarget = value;
		}
	}

	// Token: 0x170002DA RID: 730
	// (get) Token: 0x06001A47 RID: 6727 RVA: 0x0001362E File Offset: 0x0001182E
	// (set) Token: 0x06001A48 RID: 6728 RVA: 0x00013636 File Offset: 0x00011836
	public float YRotation
	{
		get
		{
			return this.yRotation;
		}
		set
		{
			this.yRotation = value;
		}
	}

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x06001A49 RID: 6729 RVA: 0x0001363F File Offset: 0x0001183F
	// (set) Token: 0x06001A4A RID: 6730 RVA: 0x00013647 File Offset: 0x00011847
	public float ZRotation
	{
		get
		{
			return this.zRotation;
		}
		set
		{
			this.zRotation = value;
		}
	}

	// Token: 0x06001A4B RID: 6731 RVA: 0x00013650 File Offset: 0x00011850
	public void SetCameraLocked(bool val)
	{
		this.cameraLocked = val;
	}

	// Token: 0x170002DC RID: 732
	// (get) Token: 0x06001A4C RID: 6732 RVA: 0x00013659 File Offset: 0x00011859
	// (set) Token: 0x06001A4D RID: 6733 RVA: 0x00013661 File Offset: 0x00011861
	public Player RewiredPlayer { get; set; }

	// Token: 0x04001BFB RID: 7163
	public float mouseSensitivity = 0.04f;

	// Token: 0x04001BFC RID: 7164
	public float joystickSensitivity = 1f;

	// Token: 0x04001BFD RID: 7165
	public float minCamDistance;

	// Token: 0x04001BFE RID: 7166
	public float maxCamDistance = 2f;

	// Token: 0x04001BFF RID: 7167
	public float camRotationSpeed = 3f;

	// Token: 0x04001C00 RID: 7168
	public float zSpeed = 3f;

	// Token: 0x04001C01 RID: 7169
	public float camOffsetY = 1f;

	// Token: 0x04001C02 RID: 7170
	public float camOffsetZ = 0.5f;

	// Token: 0x04001C03 RID: 7171
	public float zRotMin = -45f;

	// Token: 0x04001C04 RID: 7172
	public float zRotMax = 45f;

	// Token: 0x04001C05 RID: 7173
	public float lookForwardOffset = 1f;

	// Token: 0x04001C06 RID: 7174
	public float lookScale = 1.9f;

	// Token: 0x04001C07 RID: 7175
	public float wallOffset = 0.2f;

	// Token: 0x04001C08 RID: 7176
	public float roofOffset = 0.1f;

	// Token: 0x04001C09 RID: 7177
	public float camSpeed = 2f;

	// Token: 0x04001C0A RID: 7178
	public float camVRecoverySpeed = 2.5f;

	// Token: 0x04001C0B RID: 7179
	public float camDRecoverySpeed = 5f;

	// Token: 0x04001C0C RID: 7180
	public LayerMask rayCastMask;

	// Token: 0x04001C0D RID: 7181
	private CharacterController controller;

	// Token: 0x04001C0E RID: 7182
	private Transform positionalTarget;

	// Token: 0x04001C0F RID: 7183
	private Transform rotationalTarget;

	// Token: 0x04001C10 RID: 7184
	private Vector3 lookPos = new Vector3(0f, 0f, 1f);

	// Token: 0x04001C11 RID: 7185
	private Camera targetCamera;

	// Token: 0x04001C12 RID: 7186
	private float yRotation;

	// Token: 0x04001C13 RID: 7187
	private float zRotation;

	// Token: 0x04001C14 RID: 7188
	private float curCamDistance = 5f;

	// Token: 0x04001C15 RID: 7189
	private bool cameraLocked;

	// Token: 0x04001C16 RID: 7190
	private bool hitWall;
}
