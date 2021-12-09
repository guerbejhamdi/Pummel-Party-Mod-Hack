using System;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200024B RID: 587
public class SnowySpinPlayer : CharacterBase
{
	// Token: 0x17000178 RID: 376
	// (get) Token: 0x060010F6 RID: 4342 RVA: 0x0000E0A8 File Offset: 0x0000C2A8
	// (set) Token: 0x060010F7 RID: 4343 RVA: 0x0000E0B0 File Offset: 0x0000C2B0
	public CharacterMover Mover { get; set; }

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x060010F8 RID: 4344 RVA: 0x0000E0B9 File Offset: 0x0000C2B9
	public float Scale
	{
		get
		{
			return this.scale;
		}
	}

	// Token: 0x060010F9 RID: 4345 RVA: 0x00084FDC File Offset: 0x000831DC
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.Mover = base.GetComponent<CharacterMover>();
		this.Mover.SetForwardVector(new Vector3(0f, 0f, 1f));
		if (!NetSystem.IsServer)
		{
			this.position.Recieve = new RecieveProxy(this.RecievePosition);
			this.velocity.Recieve = new RecieveProxy(this.RecieveVelocity);
			this.netScale.Recieve = new RecieveProxy(this.RecieveScale);
			return;
		}
		this.position.Value = base.transform.position;
	}

	// Token: 0x060010FA RID: 4346 RVA: 0x00085080 File Offset: 0x00083280
	protected override void Start()
	{
		base.Start();
		this.minigameController = (SnowySpinController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.baseRadius = this.controller.radius;
		this.snowball = this.player_root.transform.Find("Character/SnowBall");
		this.originalScale = this.snowball.localScale;
	}

	// Token: 0x060010FB RID: 4347 RVA: 0x00085104 File Offset: 0x00083304
	private void Update()
	{
		this.controller.radius = this.baseRadius * this.scale;
		this.controller.height = this.baseRadius * this.scale * 2f;
		this.snowball.localScale = this.originalScale * this.scale;
		if (base.IsOwner)
		{
			if (!this.player.IsAI && !this.gotAchievement && this.achivementKillCount.Value >= 5)
			{
				PlatformAchievementManager.Instance.TriggerAchievement("ACH_SNOWY_SPIN");
				this.gotAchievement = true;
			}
			Vector2 vector = this.player.IsAI ? this.GetAIInput() : this.GetInput();
			if (!this.player.IsAI && (!GameManager.PollInput || GameManager.IsGamePaused))
			{
				vector = Vector2.zero;
			}
			this.inputX.Value = ZPMath.CompressFloatToShort(vector.x, -1f, 1f);
			this.inputY.Value = ZPMath.CompressFloatToShort(vector.y, -1f, 1f);
		}
		if (NetSystem.IsServer)
		{
			this.DoInput();
		}
		else
		{
			Vector3 pre_position = base.transform.position;
			if (this.gotPosition)
			{
				base.transform.position = this.position.Value;
				this.gotPosition = false;
				this.DoRotation(pre_position);
			}
			else
			{
				this.controller.Move(this.velocity.Value * Time.deltaTime);
				base.transform.position = pre_position;
				this.DoInput();
				if (Vector3.Distance(base.transform.position, this.position.Value) > 1f)
				{
					base.transform.position = this.position.Value;
				}
			}
		}
		ParticleSystem.EmissionModule emission = this.dirtParticle.emission;
		float constant = 0f;
		if (!base.IsDead)
		{
			Vector3 vector2 = NetSystem.IsServer ? this.Mover.Velocity : this.velocity.Value;
			vector2.y = 0f;
			constant = Mathf.Clamp(110f * (vector2.magnitude / this.Mover.maxSpeed), 0f, 110f);
		}
		emission.rateOverTime = new ParticleSystem.MinMaxCurve(constant);
		if (NetSystem.IsServer)
		{
			if (!this.isDead && this.minigameController.State == MinigameControllerState.Playing && base.transform.position.y <= -7f)
			{
				this.KillPlayer(true);
			}
			this.position.Value = base.transform.position;
			this.velocity.Value = this.Mover.Velocity;
			this.netScale.Value = this.scale;
		}
	}

	// Token: 0x060010FC RID: 4348 RVA: 0x0000E0C1 File Offset: 0x0000C2C1
	private Vector2 GetInput()
	{
		return new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
	}

	// Token: 0x060010FD RID: 4349 RVA: 0x000853D0 File Offset: 0x000835D0
	private Vector2 GetAIInput()
	{
		float num = (this.AITarget != null) ? this.AITarget.controller.height : this.controller.height;
		float num2 = 2f;
		float num3 = 6f;
		float min = (num2 + this.controller.height) * (num2 + num);
		float max = (num3 + this.controller.height) * (num3 + num);
		float num4 = this.controller.height * 1.2f * (num * 1.2f);
		float num5 = 20.25f;
		switch (base.GamePlayer.Difficulty)
		{
		case BotDifficulty.Easy:
			num5 = 12.25f;
			break;
		case BotDifficulty.Normal:
			num5 = 25f;
			break;
		case BotDifficulty.Hard:
			num5 = 36f;
			break;
		}
		float num6 = 12.25f;
		switch (base.GamePlayer.Difficulty)
		{
		case BotDifficulty.Easy:
			num5 = 6.25f;
			break;
		case BotDifficulty.Normal:
			num5 = 16f;
			break;
		case BotDifficulty.Hard:
			num5 = 25f;
			break;
		}
		Vector3 vector = Vector3.zero - base.transform.position;
		vector.y = 0f;
		float sqrMagnitude = vector.sqrMagnitude;
		if (this.curAIState == SnowySpinPlayer.SnowySpinsAIState.RetreatingFromEdge)
		{
			if (this.retreatDirChangeTimer.Elapsed(true))
			{
				this.curRetreatTargetPoint = ZPMath.RandomPointInUnitSphere(GameManager.rand) * 6f;
				this.curRetreatTargetPoint.y = 0f;
			}
			if (sqrMagnitude < num6)
			{
				this.curAIState = SnowySpinPlayer.SnowySpinsAIState.FindingTarget;
			}
		}
		else if (sqrMagnitude > num5)
		{
			this.retreatDirChangeTimer.Start();
			this.curAIState = SnowySpinPlayer.SnowySpinsAIState.RetreatingFromEdge;
		}
		if ((this.AITarget != null && this.AITarget.IsDead) || (this.curAIState == SnowySpinPlayer.SnowySpinsAIState.Attacking && this.maxAttackLengthTimer.Elapsed(true)))
		{
			this.AITarget = null;
		}
		if (this.AITarget == null || this.curAIState == SnowySpinPlayer.SnowySpinsAIState.FindingTarget)
		{
			SnowySpinPlayer snowySpinPlayer = null;
			float num7 = float.MinValue;
			for (int i = 0; i < this.minigameController.GetPlayerCount(); i++)
			{
				SnowySpinPlayer snowySpinPlayer2 = (SnowySpinPlayer)this.minigameController.GetPlayerInSlot((short)i);
				if (!(snowySpinPlayer2 == this) && !snowySpinPlayer2.IsDead)
				{
					float sqrMagnitude2 = (snowySpinPlayer2.transform.position - base.transform.position).sqrMagnitude;
					if ((snowySpinPlayer == null || snowySpinPlayer2.transform.position.y > -0.25f) && ((sqrMagnitude2 > this.curAttackSqrDist && sqrMagnitude2 < num7) || (num7 < this.curAttackSqrDist && sqrMagnitude2 > num7)))
					{
						snowySpinPlayer = snowySpinPlayer2;
						num7 = sqrMagnitude2;
					}
				}
			}
			if (snowySpinPlayer != null)
			{
				this.curAIState = SnowySpinPlayer.SnowySpinsAIState.MoveAway;
				this.AITarget = snowySpinPlayer;
			}
		}
		if (this.AITarget == null)
		{
			return Vector2.zero;
		}
		float sqrMagnitude3 = (this.AITarget.transform.position - base.transform.position).sqrMagnitude;
		if (this.attackOffsetChangeTimer.Elapsed(true))
		{
			this.curAttackOffset = ZPMath.RandomPointInUnitSphere(GameManager.rand) * 2f;
			this.curAttackOffset.y = 0f;
		}
		if (this.curAIState == SnowySpinPlayer.SnowySpinsAIState.MoveAway)
		{
			if (sqrMagnitude3 > this.curAttackSqrDist)
			{
				this.curAIState = SnowySpinPlayer.SnowySpinsAIState.Attacking;
				this.maxAttackLengthTimer.Start();
			}
		}
		else if (this.curAIState == SnowySpinPlayer.SnowySpinsAIState.Attacking && sqrMagnitude3 < num4)
		{
			this.curAIState = SnowySpinPlayer.SnowySpinsAIState.FindingTarget;
			this.curAttackSqrDist = ZPMath.RandomFloat(GameManager.rand, min, max);
		}
		switch (this.curAIState)
		{
		case SnowySpinPlayer.SnowySpinsAIState.FindingTarget:
			return Vector2.zero;
		case SnowySpinPlayer.SnowySpinsAIState.MoveAway:
		{
			Vector3 vector2 = base.transform.position - (this.AITarget.transform.position + this.curAttackOffset);
			vector2.y = 0f;
			Vector3 normalized = vector2.normalized;
			Vector3 normalized2 = vector.normalized;
			Vector3 vector3 = normalized + normalized2 * 0.5f;
			return new Vector2(vector3.x, vector3.z).normalized;
		}
		case SnowySpinPlayer.SnowySpinsAIState.Attacking:
		{
			Vector3 vector4 = this.AITarget.transform.position + this.curAttackOffset - base.transform.position;
			vector4.y = 0f;
			Vector3 normalized3 = vector4.normalized;
			return new Vector2(normalized3.x, normalized3.z).normalized;
		}
		case SnowySpinPlayer.SnowySpinsAIState.RetreatingFromEdge:
		{
			Vector3 vector5 = this.curRetreatTargetPoint - base.transform.position;
			vector5.y = 0f;
			Vector3 normalized4 = vector5.normalized;
			return new Vector2(normalized4.x, normalized4.z).normalized;
		}
		default:
			return Vector2.zero;
		}
	}

	// Token: 0x060010FE RID: 4350 RVA: 0x000858B4 File Offset: 0x00083AB4
	private void DoInput()
	{
		if (this.isDead)
		{
			return;
		}
		Vector2 zero = new Vector2(ZPMath.DecompressShortToFloat(this.inputX.Value, -1f, 1f), ZPMath.DecompressShortToFloat(this.inputY.Value, -1f, 1f));
		MinigameControllerState state = this.minigameController.State;
		bool flag = state != MinigameControllerState.Playing && state != MinigameControllerState.RoundResetWait;
		if (flag || base.transform.position.y <= -0.6f)
		{
			zero = Vector2.zero;
		}
		Vector3 vector = base.transform.position;
		vector.y -= this.controller.height / 2f + this.controller.skinWidth;
		float num = Vector3.Distance(this.lastHitPoint, vector);
		if (this.controller.isGrounded && num > 0.2f)
		{
			this.Mover.Velocity += (vector - this.lastHitPoint).normalized * (num / 0.04f) * Time.deltaTime;
		}
		Vector3 pre_position = base.transform.position;
		this.Mover.CalculateVelocity(new CharacterMoverInput(zero, false, false), Time.deltaTime);
		if (flag)
		{
			this.Mover.Velocity = new Vector3(0f, this.Mover.Velocity.y, 0f);
		}
		this.Mover.DoMovement(Time.deltaTime);
		this.DoRotation(pre_position);
	}

	// Token: 0x060010FF RID: 4351 RVA: 0x00085A40 File Offset: 0x00083C40
	private void DoRotation(Vector3 pre_position)
	{
		Vector3 vector = base.transform.position - pre_position;
		vector.y = 0f;
		Vector3 axis = Vector3.Cross(vector.normalized, Vector3.up);
		float magnitude = vector.magnitude;
		if (NetSystem.IsServer)
		{
			float num = magnitude / 100f;
			this.scale = Mathf.Clamp(this.scale + num, 0f, this.maxScale);
			base.transform.position += Vector3.up * (this.controller.radius * num);
		}
		float num2 = 6.2831855f * this.controller.height;
		this.player_root.transform.RotateAround(this.player_root.transform.position, axis, -(magnitude / num2 * 360f));
	}

	// Token: 0x06001100 RID: 4352 RVA: 0x00085B20 File Offset: 0x00083D20
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (NetSystem.IsServer && hit.gameObject.tag == "Player")
		{
			SnowySpinPlayer component = hit.gameObject.GetComponent<SnowySpinPlayer>();
			if (component.IsDead || base.IsDead)
			{
				return;
			}
			this.lastHitPlayer = (int)component.OwnerSlot;
			component.lastHitPlayer = (int)base.OwnerSlot;
			Vector3 vector = base.transform.position - component.transform.position;
			float magnitude = vector.magnitude;
			float radius = this.controller.radius;
			float skinWidth = this.controller.skinWidth;
			float num = this.baseRadius;
			float num2 = component.Scale;
			float skinWidth2 = this.controller.skinWidth;
			float num3 = 12.566371f * Mathf.Pow(this.controller.radius, 3f) / 3f;
			float num4 = 12.566371f * Mathf.Pow(component.controller.radius, 3f) / 3f;
			float num5 = 1f / (num3 / 10f);
			float num6 = 1f / (num4 / 10f);
			vector.Normalize();
			float num7 = Vector3.Dot(this.Mover.Velocity - component.Mover.Velocity, vector);
			if (num7 > 0f)
			{
				return;
			}
			float d = -2.1f * num7 / (num5 + num6);
			Vector3 a = vector * d;
			this.Mover.Velocity += a * num5;
			component.Mover.Velocity -= a * num6;
			if (Time.time - this.lastSoundTime > this.minSoundInterval)
			{
				float num8 = Mathf.Clamp((component.Mover.Velocity.magnitude + this.Mover.Velocity.magnitude) / (this.Mover.maxSpeed * 2f), 0f, 1f);
				AudioSystem.PlayOneShot(this.hitSound, num8, 0f, 1f);
				base.SendRPC("RPCPlayHitSound", NetRPCDelivery.RELIABLE_ORDERED, new object[]
				{
					num8
				});
				this.lastSoundTime = Time.time;
			}
			if (this.AITarget == component)
			{
				this.AITarget = null;
				return;
			}
		}
		else if (hit.gameObject.tag != "Player")
		{
			this.lastHitPoint = hit.point;
		}
	}

	// Token: 0x06001101 RID: 4353 RVA: 0x00085DA4 File Offset: 0x00083FA4
	public override void ResetPlayer()
	{
		if (!this.isDead)
		{
			this.KillPlayer(false);
		}
		this.isDead = false;
		base.transform.position = this.startPosition;
		base.transform.rotation = this.startRotation;
		this.player_root.transform.localRotation = Quaternion.identity;
		this.Mover.Velocity = Vector3.zero;
		base.transform.position.y -= this.controller.height / 2f + this.controller.skinWidth;
		this.scale = 1f;
		this.lastHitPlayer = -1;
	}

	// Token: 0x06001102 RID: 4354 RVA: 0x00085E54 File Offset: 0x00084054
	public void KillPlayer(bool send_rpc)
	{
		if (!this.isDead)
		{
			if (send_rpc && NetSystem.IsServer && this.lastHitPlayer != -1)
			{
				NetVar<byte> netVar = ((SnowySpinPlayer)this.minigameController.GetPlayerInSlot((short)this.lastHitPlayer)).achivementKillCount;
				byte value = netVar.Value;
				netVar.Value = value + 1;
			}
			this.isDead = true;
			this.Deactivate();
			UnityEngine.Object.Instantiate<GameObject>(this.playerDeathEffect, base.transform.position, Quaternion.LookRotation(Vector3.up));
			AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
			if (NetSystem.IsServer && send_rpc)
			{
				this.minigameController.PlayerDied(this);
				base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
			}
		}
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x0000E0F2 File Offset: 0x0000C2F2
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender)
	{
		this.KillPlayer(false);
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x0000E0FB File Offset: 0x0000C2FB
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCPlayHitSound(NetPlayer sender, float volume)
	{
		AudioSystem.PlayOneShot(this.hitSound, volume, 0f, 1f);
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x0000E113 File Offset: 0x0000C313
	public void RecievePosition(object _pos)
	{
		this.gotPosition = true;
	}

	// Token: 0x06001106 RID: 4358 RVA: 0x0000E11C File Offset: 0x0000C31C
	public void RecieveVelocity(object val)
	{
		this.Mover.Velocity = (Vector3)val;
	}

	// Token: 0x06001107 RID: 4359 RVA: 0x0000E12F File Offset: 0x0000C32F
	public void RecieveScale(object val)
	{
		this.scale = (float)val;
	}

	// Token: 0x04001196 RID: 4502
	public ParticleSystem dirtParticle;

	// Token: 0x04001197 RID: 4503
	public GameObject playerDeathEffect;

	// Token: 0x04001198 RID: 4504
	public AudioClip hitSound;

	// Token: 0x0400119A RID: 4506
	private SnowySpinController minigameController;

	// Token: 0x0400119B RID: 4507
	private SnowySpinPlayer AITarget;

	// Token: 0x0400119C RID: 4508
	private Vector3 lastHitPoint = Vector3.zero;

	// Token: 0x0400119D RID: 4509
	private float lastSoundTime;

	// Token: 0x0400119E RID: 4510
	private float minSoundInterval = 0.1f;

	// Token: 0x0400119F RID: 4511
	private bool gotPosition;

	// Token: 0x040011A0 RID: 4512
	private SnowySpinPlayer.SnowySpinsAIState curAIState;

	// Token: 0x040011A1 RID: 4513
	private float scale = 1f;

	// Token: 0x040011A2 RID: 4514
	private float maxScale = 2f;

	// Token: 0x040011A3 RID: 4515
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 position = new NetVec3(Vector3.zero);

	// Token: 0x040011A4 RID: 4516
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 velocity = new NetVec3(Vector3.zero);

	// Token: 0x040011A5 RID: 4517
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<short> inputX = new NetVar<short>(0);

	// Token: 0x040011A6 RID: 4518
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<short> inputY = new NetVar<short>(0);

	// Token: 0x040011A7 RID: 4519
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<float> netScale = new NetVar<float>(0f);

	// Token: 0x040011A8 RID: 4520
	private float baseRadius;

	// Token: 0x040011A9 RID: 4521
	private Vector3 originalScale = Vector3.zero;

	// Token: 0x040011AA RID: 4522
	private Transform snowball;

	// Token: 0x040011AB RID: 4523
	private bool gotAchievement;

	// Token: 0x040011AC RID: 4524
	private ActionTimer retreatDirChangeTimer = new ActionTimer(0.3f, 0.8f);

	// Token: 0x040011AD RID: 4525
	private Vector3 curRetreatTargetPoint = Vector3.zero;

	// Token: 0x040011AE RID: 4526
	private ActionTimer attackOffsetChangeTimer = new ActionTimer(0.4f, 1.5f);

	// Token: 0x040011AF RID: 4527
	private Vector3 curAttackOffset = Vector3.zero;

	// Token: 0x040011B0 RID: 4528
	private ActionTimer maxAttackLengthTimer = new ActionTimer(5f, 8f);

	// Token: 0x040011B1 RID: 4529
	private float curAttackSqrDist = 10f;

	// Token: 0x040011B2 RID: 4530
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<byte> achivementKillCount = new NetVar<byte>(0);

	// Token: 0x040011B3 RID: 4531
	public int lastHitPlayer = -1;

	// Token: 0x0200024C RID: 588
	private enum SnowySpinsAIState
	{
		// Token: 0x040011B5 RID: 4533
		FindingTarget,
		// Token: 0x040011B6 RID: 4534
		MoveAway,
		// Token: 0x040011B7 RID: 4535
		Attacking,
		// Token: 0x040011B8 RID: 4536
		RetreatingFromEdge
	}
}
