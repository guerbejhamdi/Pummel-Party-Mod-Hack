using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000204 RID: 516
[ExecuteInEditMode]
public class PongPlayer : CharacterBase
{
	// Token: 0x06000F2F RID: 3887 RVA: 0x0000D200 File Offset: 0x0000B400
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		if (!base.IsOwner)
		{
			this.n_position.Recieve = new RecieveProxy(this.RecievePosition);
			return;
		}
		this.UpdateNetPosition();
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x00078C80 File Offset: 0x00076E80
	protected override void Start()
	{
		base.Start();
		this.m_minigameController = (PongController)GameManager.Minigame;
		if (this.m_minigameController != null)
		{
			this.m_minigameController.AddPlayer(this);
		}
		this.m_cameraShake = this.m_minigameController.Root.GetComponentInChildren<CameraShake>();
		if (this.m_minigameController.SpawnPoints != null && base.Owner.Slot < this.m_minigameController.SpawnPoints.Length)
		{
			this.m_startPos = this.m_minigameController.SpawnPoints[(int)base.GamePlayer.GlobalID].position;
		}
		Color skinColor = this.player.Color.skinColor1;
		skinColor.r = Mathf.Max(0.25f, skinColor.r);
		skinColor.g = Mathf.Max(0.25f, skinColor.g);
		skinColor.b = Mathf.Max(0.25f, skinColor.b);
		this.m_barrierLight.color = skinColor;
		this.m_barrierRenderer.material.SetColor("_TintColor", skinColor);
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x00078D94 File Offset: 0x00076F94
	public void Update()
	{
		Player player = null;
		if (Time.time > this.m_nextHitOffsetChange)
		{
			this.m_hitOffset = UnityEngine.Random.value * 2f - 1f;
			this.m_nextHitOffsetChange = Time.time + 3f;
		}
		if (base.IsOwner && !this.player.IsAI && !this.gotAchievement && this.Score >= 5)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_BOUNCING_BALLS");
			this.gotAchievement = true;
		}
		float num = 0f;
		if (base.IsOwner)
		{
			if (!this.player.IsAI)
			{
				if (this.player != null)
				{
					player = this.player.RewiredPlayer;
				}
				if (player != null)
				{
					num = this.player.RewiredPlayer.GetAxis(InputActions.Horizontal);
					if (this.player.GlobalID == 1 || this.player.GlobalID == 3)
					{
						num = -num;
					}
				}
			}
			else
			{
				List<PongBall> activeBalls = this.m_minigameController.GetActiveBalls();
				PongBall pongBall = null;
				float num2 = float.PositiveInfinity;
				foreach (PongBall pongBall2 in activeBalls)
				{
					if (Vector3.Dot(base.transform.forward, pongBall2.Velocity.normalized) < 0f)
					{
						float num3 = Vector3.Distance(pongBall2.transform.position, base.transform.position);
						if (num3 < num2)
						{
							pongBall = pongBall2;
							num2 = num3;
						}
					}
				}
				if (pongBall != null)
				{
					Debug.DrawLine(base.transform.position, pongBall.transform.position, Color.yellow);
					Plane plane = new Plane(base.transform.forward, this.m_hitPoint.position);
					Ray ray = new Ray(pongBall.transform.position, pongBall.Velocity.normalized);
					Debug.DrawLine(this.m_hitPoint.position, this.m_hitPoint.position + base.transform.forward, Color.white);
					float distance = 0f;
					if (plane.Raycast(ray, out distance))
					{
						Vector3 vector = ray.GetPoint(distance) + base.transform.right * this.m_hitOffset;
						Debug.DrawLine(pongBall.transform.position, vector, Color.magenta);
						Debug.DrawLine(vector, vector + Vector3.up, Color.white);
						if (Vector3.Distance(vector, this.m_hitPoint.position) > 0.05f)
						{
							num = Vector3.Dot(base.transform.right, (vector - this.m_hitPoint.position).normalized);
						}
					}
				}
			}
		}
		if (base.IsOwner)
		{
			if (this.m_pos <= -this.m_moveDistance && num < 0f)
			{
				num = 0f;
				this.m_velocity = 0f;
			}
			if (this.m_pos >= this.m_moveDistance && num > 0f)
			{
				num = 0f;
				this.m_velocity = 0f;
			}
			if (Mathf.Abs(num) < 0.01f)
			{
				if (this.player.IsAI)
				{
					this.m_velocity = Mathf.SmoothDamp(this.m_velocity, 0f, ref this.m_smoothDampVelocity, 0.05f);
				}
				else
				{
					this.m_velocity = Mathf.SmoothDamp(this.m_velocity, 0f, ref this.m_smoothDampVelocity, 0.075f);
				}
			}
			float num4 = (num == 0f) ? 20f : 2f;
			if (this.player.IsAI)
			{
				num4 = 1f;
			}
			this.m_velocity += num * this.m_movementSpeed * Time.deltaTime * num4;
			this.m_velocity = Mathf.Clamp(this.m_velocity, -this.m_maxSpeed, this.m_maxSpeed);
			this.m_pos += this.m_velocity * Time.deltaTime;
			this.m_pos = Mathf.Clamp(this.m_pos, -this.m_moveDistance, this.m_moveDistance);
			base.transform.position = this.m_startPos + base.transform.right * this.m_pos;
			this.n_position.Value = this.m_pos;
		}
		float num5 = Mathf.Clamp(Vector3.Dot(base.transform.position - this.m_lastPosition, base.transform.right) / 0.2f, -1f, 1f);
		this.m_droneTransform.localRotation = Quaternion.Euler(0f, 0f, num5 * -30f);
		this.m_lastPosition = base.transform.position;
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x0000D22E File Offset: 0x0000B42E
	public void OnDrawGizmos()
	{
		Debug.DrawLine(base.transform.position, base.transform.position + base.transform.right);
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x00079278 File Offset: 0x00077478
	public Vector3 GetBounceDirection(Vector3 hitPoint)
	{
		Vector3 position = this.m_bounceLinePoints[0].position;
		Vector3 position2 = this.m_bounceLinePoints[1].position;
		Vector3 b = ZPMath.ClosestPointOnLine(position, position2, hitPoint);
		float num = Vector3.Distance(position, b);
		float num2 = Vector3.Distance(position, position2);
		float num3 = num / num2 * 2f - 1f;
		return (Quaternion.Euler(0f, num3 * 45f, 0f) * base.transform.forward).normalized;
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x0000D25B File Offset: 0x0000B45B
	public void OnBallHitWall()
	{
		if (Time.time > this.m_nextAllowedShake)
		{
			this.m_cameraShake.AddShake(0.1f);
			this.m_nextAllowedShake = Time.time + 1f;
		}
	}

	// Token: 0x06000F35 RID: 3893 RVA: 0x0000398C File Offset: 0x00001B8C
	private void UpdateNetPosition()
	{
	}

	// Token: 0x06000F36 RID: 3894 RVA: 0x000792FC File Offset: 0x000774FC
	public void RecievePosition(object _pos)
	{
		if (this == null || base.transform == null)
		{
			return;
		}
		base.transform.position = this.m_startPos + base.transform.right * (float)_pos;
		this.m_gotNewPosition = true;
	}

	// Token: 0x04000EF0 RID: 3824
	[SerializeField]
	protected float m_movementSpeed = 1f;

	// Token: 0x04000EF1 RID: 3825
	[SerializeField]
	protected float m_maxSpeed = 10f;

	// Token: 0x04000EF2 RID: 3826
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<float> n_position = new NetVar<float>(0f);

	// Token: 0x04000EF3 RID: 3827
	[SerializeField]
	protected Transform[] m_bounceLinePoints;

	// Token: 0x04000EF4 RID: 3828
	[SerializeField]
	protected Transform m_hitPoint;

	// Token: 0x04000EF5 RID: 3829
	[SerializeField]
	protected Light m_barrierLight;

	// Token: 0x04000EF6 RID: 3830
	[SerializeField]
	protected MeshRenderer m_barrierRenderer;

	// Token: 0x04000EF7 RID: 3831
	[SerializeField]
	protected Transform m_droneTransform;

	// Token: 0x04000EF8 RID: 3832
	private bool m_gotNewPosition;

	// Token: 0x04000EF9 RID: 3833
	private PongController m_minigameController;

	// Token: 0x04000EFA RID: 3834
	private CameraShake m_cameraShake;

	// Token: 0x04000EFB RID: 3835
	private float m_moveDistance = 3.7f;

	// Token: 0x04000EFC RID: 3836
	private float m_velocity;

	// Token: 0x04000EFD RID: 3837
	private float m_smoothDampVelocity;

	// Token: 0x04000EFE RID: 3838
	private float m_pos;

	// Token: 0x04000EFF RID: 3839
	private Vector3 m_startPos;

	// Token: 0x04000F00 RID: 3840
	private float m_hitOffset;

	// Token: 0x04000F01 RID: 3841
	private float m_nextHitOffsetChange;

	// Token: 0x04000F02 RID: 3842
	private bool gotAchievement;

	// Token: 0x04000F03 RID: 3843
	private Vector3 m_lastPosition;

	// Token: 0x04000F04 RID: 3844
	private float m_nextAllowedShake;
}
