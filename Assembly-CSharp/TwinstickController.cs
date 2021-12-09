using System;
using UnityEngine;
using ZP.Net;

// Token: 0x020003D3 RID: 979
public class TwinstickController : NetBehaviour
{
	// Token: 0x06001A4F RID: 6735 RVA: 0x0001366A File Offset: 0x0001186A
	public void InitializeController()
	{
		this.controller = base.GetComponent<CharacterController>();
		if (!GameManager.Minigame)
		{
			this.cam = GameManager.Minigame.MinigameCamera;
		}
	}

	// Token: 0x06001A50 RID: 6736 RVA: 0x00013694 File Offset: 0x00011894
	public override void OnNetInitialize()
	{
		if (base.IsOwner)
		{
			this.position.Value = base.transform.position;
			return;
		}
		this.position.Recieve = new RecieveProxy(this.RecievePosition);
	}

	// Token: 0x06001A51 RID: 6737 RVA: 0x000136CC File Offset: 0x000118CC
	public void Push(Vector3 direction, float force)
	{
		direction.y = 0f;
		this.player_velocity = direction * force;
	}

	// Token: 0x06001A52 RID: 6738 RVA: 0x000AE7FC File Offset: 0x000AC9FC
	protected void UpdateController()
	{
		if (base.IsOwner)
		{
			if (this.cam != null)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(this.cam.ScreenPointToRay(Input.mousePosition), out raycastHit, 100f, 1024))
				{
					Vector3 point = raycastHit.point;
					point.y = 0f;
					Vector3 a = base.transform.position;
					a.y = 0f;
					this.look_direction = (a - point).normalized;
					Quaternion identity = Quaternion.identity;
					identity.SetLookRotation(this.look_direction, Vector3.up);
					base.transform.rotation = identity;
				}
			}
			else if (GameManager.Minigame)
			{
				this.cam = GameManager.Minigame.MinigameCamera;
				Debug.Log("Camera Null!");
			}
			this.DoMovement();
			this.facing_angle.Value = base.transform.rotation.eulerAngles.y;
			this.position.Value = base.transform.position;
			return;
		}
		base.transform.rotation = Quaternion.AngleAxis(this.facing_angle.Value, Vector3.up);
	}

	// Token: 0x06001A53 RID: 6739 RVA: 0x000AE93C File Offset: 0x000ACB3C
	private void DoMovement()
	{
		Vector3 normalized = new Vector3(0f, 0f, 0f);
		Vector3 normalized2 = new Vector3(0f, 0f, 0f);
		Vector3 a = new Vector3(0f, 0f, 0f);
		normalized = new Vector3(0f, 0f, 1f);
		normalized.y = 0f;
		normalized = normalized.normalized;
		normalized2 = Vector3.Cross(Vector3.up, normalized).normalized;
		Vector3 zero = Vector3.zero;
		bool flag = true;
		if (Input.GetKey(KeyCode.W))
		{
			a += normalized;
			zero.z += 1f;
			flag = false;
		}
		if (Input.GetKey(KeyCode.S))
		{
			a -= normalized;
			zero.z -= 1f;
			flag = false;
		}
		if (Input.GetKey(KeyCode.D))
		{
			a += normalized2;
			zero.x += 1f;
			flag = false;
		}
		if (Input.GetKey(KeyCode.A))
		{
			a -= normalized2;
			zero.x -= 1f;
			flag = false;
		}
		Vector3 vector = zero.normalized * this.acceleration * Time.deltaTime;
		if (this.controller.isGrounded)
		{
			this.player_velocity.y = 0f;
		}
		this.player_velocity.y = this.player_velocity.y - this.gravity * Time.deltaTime;
		if (this.use_acceleration)
		{
			Vector3 vector2 = this.player_velocity;
			vector2.y = 0f;
			Vector3 vector3 = new Vector3(1f, 0f, 1f).normalized;
			vector3 *= this.max_speed;
			if (vector2.x > vector3.x && vector.x < 0f)
			{
				vector2.x += vector.x;
			}
			else if (vector2.x < -vector3.x && vector.x > 0f)
			{
				vector2.x += vector.x;
			}
			else if (vector2.x + vector.x > -vector3.x && vector2.x + vector.x < vector3.x)
			{
				vector2.x += vector.x;
			}
			if (vector2.z > vector3.z && vector.z < 0f)
			{
				vector2.z += vector.z;
			}
			else if (vector2.z < -vector3.z && vector.z > 0f)
			{
				vector2.z += vector.z;
			}
			else if (vector2.z + vector.z > -vector3.z && vector2.z + vector.z < vector3.z)
			{
				vector2.z += vector.z;
			}
			vector2 += this.friction * -vector2.normalized * Time.deltaTime;
			vector2.y = this.player_velocity.y;
			this.player_velocity = vector2;
		}
		else
		{
			Vector3 a2 = this.player_velocity;
			a2.y = 0f;
			if (!flag)
			{
				a2 += zero.normalized * this.acceleration * Time.deltaTime;
				if (a2.magnitude > this.max_speed)
				{
					a2 = a2.normalized * this.max_speed;
				}
			}
			else
			{
				Vector3 b = a2.normalized * this.decceleration * Time.deltaTime;
				if (b.magnitude >= a2.magnitude)
				{
					a2 = Vector3.zero;
				}
				else
				{
					a2 -= b;
				}
			}
			a2.y = this.player_velocity.y;
			this.player_velocity = a2;
		}
		Vector3 motion = this.player_velocity * Time.deltaTime;
		this.controller.Move(motion);
	}

	// Token: 0x06001A54 RID: 6740 RVA: 0x000136E7 File Offset: 0x000118E7
	public void RecievePosition(object _pos)
	{
		base.transform.position = (Vector3)_pos;
	}

	// Token: 0x04001C18 RID: 7192
	public float acceleration;

	// Token: 0x04001C19 RID: 7193
	public float decceleration;

	// Token: 0x04001C1A RID: 7194
	public float max_speed = 2f;

	// Token: 0x04001C1B RID: 7195
	public float gravity = 8f;

	// Token: 0x04001C1C RID: 7196
	public float friction = 0.05f;

	// Token: 0x04001C1D RID: 7197
	public bool use_acceleration = true;

	// Token: 0x04001C1E RID: 7198
	protected Vector3 look_direction = Vector3.right;

	// Token: 0x04001C1F RID: 7199
	private Vector3 player_velocity = Vector3.zero;

	// Token: 0x04001C20 RID: 7200
	protected CharacterController controller;

	// Token: 0x04001C21 RID: 7201
	private Camera cam;

	// Token: 0x04001C22 RID: 7202
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 position = new NetVec3(Vector3.zero);

	// Token: 0x04001C23 RID: 7203
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 velocity = new NetVec3(Vector3.zero);

	// Token: 0x04001C24 RID: 7204
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<float> facing_angle = new NetVar<float>(0f);
}
