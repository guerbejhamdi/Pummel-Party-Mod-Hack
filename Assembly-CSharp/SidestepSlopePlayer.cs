using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200023F RID: 575
public class SidestepSlopePlayer : Movement1
{
	// Token: 0x06001099 RID: 4249 RVA: 0x00082088 File Offset: 0x00080288
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		this.minigameController = (SidestepSlopeController)GameManager.Minigame;
		if (base.IsOwner)
		{
			CharacterMover characterMover = this.mover;
			characterMover.OnJump = (CharacterMover.OnJumpDelegate)Delegate.Combine(characterMover.OnJump, new CharacterMover.OnJumpDelegate(this.OnJump));
			List<GamePlayer> localNonAIPlayers = GameManager.GetLocalNonAIPlayers();
			if (!this.player.IsAI || localNonAIPlayers.Count == 0)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.cameraPrefab, Vector3.zero, Quaternion.identity);
				gameObject.transform.parent = base.transform;
				this.cam = base.gameObject.GetComponentInChildren<Camera>();
				this.cameraShake = base.gameObject.GetComponentInChildren<CameraShake>();
				this.playerCam = base.gameObject.GetComponent<ThirdPersonCamera>();
				this.playerCam.SetTargetCamera(this.cam);
				this.playerCam.YRotation = base.transform.rotation.eulerAngles.y;
				this.playerCam.ZRotation = 5f;
				this.playerCam.RotateCamera();
				this.playerCam.UpdateCamera();
				this.minigameController.minigameCameras.Add(this.cam);
				if (localNonAIPlayers.Count > 1)
				{
					this.cam.rect = base.GetPlayerSplitScreenRect(this.player);
				}
				else if (localNonAIPlayers.Count == 0)
				{
					this.cam.rect = base.GetPlayerSplitScreenRectWithAI(this.player);
				}
				if (localNonAIPlayers.Count > 0 && localNonAIPlayers[0] == this.player)
				{
					gameObject.GetComponent<AudioListener>().enabled = true;
				}
				new Vector3((float)Screen.width * this.cam.rect.x + (float)Screen.width * (this.cam.rect.width * 0.5f), (float)Screen.height * this.cam.rect.y + (float)Screen.height * (this.cam.rect.height * 0.5f));
			}
		}
		if (!base.IsOwner || this.player.IsAI)
		{
			base.GetComponent<ThirdPersonCamera>().enabled = false;
		}
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x00004FF6 File Offset: 0x000031F6
	public override void OnOwnerChanged()
	{
		base.OnOwnerChanged();
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x0000DDE1 File Offset: 0x0000BFE1
	protected override void Start()
	{
		base.Start();
		this.minigameController.AddPlayer(this);
	}

	// Token: 0x0600109D RID: 4253 RVA: 0x0000DDF5 File Offset: 0x0000BFF5
	public override void FinishedSpawning()
	{
		this.playerAnim.ShotgunStrength = 0f;
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x0000A455 File Offset: 0x00008655
	public override void Activate()
	{
		base.Activate();
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x0000DE07 File Offset: 0x0000C007
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender, Vector3 origin)
	{
		this.KillPlayer(origin, 13f);
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x000822D4 File Offset: 0x000804D4
	public void KillPlayer(Vector3 origin, float force)
	{
		if (base.IsDead)
		{
			return;
		}
		if (base.IsOwner)
		{
			base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				origin
			});
		}
		float d = 20f;
		float num = 1f;
		if (Settings.BloodEffects && Time.time - this.lastBlood > this.minBloodInterval)
		{
			ParticleSystem component = UnityEngine.Object.Instantiate<GameObject>(this.bloodyDamageEffect, base.MidPoint, Quaternion.identity).GetComponent<ParticleSystem>();
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = component.velocityOverLifetime;
			velocityOverLifetime.enabled = true;
			velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
			Vector3 vector = (base.MidPoint - origin).normalized * d;
			velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, vector.x), Mathf.Max(0f, vector.x));
			velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, vector.y), Mathf.Max(0f, vector.y));
			velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, vector.z), Mathf.Max(0f, vector.z));
			ParticleSystem.EmissionModule emission = component.emission;
			ParticleSystem.Burst[] array = new ParticleSystem.Burst[emission.burstCount];
			emission.GetBursts(array);
			array[0].maxCount = (short)((float)array[0].maxCount * num);
			array[0].minCount = (short)((float)array[0].minCount * num);
			emission.SetBursts(array);
			this.lastBlood = Time.time;
		}
		base.IsDead = true;
		this.deathTimer.Start();
		this.SpawnRagdoll(origin, force);
		AudioSystem.PlayOneShot("DeathSplash01", base.transform.position, 0.25f, AudioRolloffMode.Logarithmic, 15f, 100f, 0f);
		AudioSystem.PlayOneShot(this.carHit, base.transform.position, 0.25f, AudioRolloffMode.Logarithmic, 15f, 100f, 0f);
		if (base.IsOwner)
		{
			this.mover.Velocity = Vector3.zero;
		}
		if (this.cameraShake != null)
		{
			this.cameraShake.enabled = false;
		}
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x00053F14 File Offset: 0x00052114
	private void SpawnRagdoll(Vector3 origin, float force)
	{
		Vector3 normalized = (base.transform.position + Vector3.up * 0.875f - origin).normalized;
		this.playerAnim.SpawnRagdoll(normalized * force);
		this.Deactivate();
	}

	// Token: 0x060010A2 RID: 4258 RVA: 0x0008251C File Offset: 0x0008071C
	private void Update()
	{
		if (!this.isDead)
		{
			base.UpdateController();
			if (base.IsOwner && this.minigameController.Playable)
			{
				float x = this.minigameController.ScoreTransform.InverseTransformPoint(base.transform.position).x;
				if (x < this.furthest.Value)
				{
					this.furthest.Value = x;
				}
			}
			if (NetSystem.IsServer && this.furthest.Value < 0f)
			{
				this.Score = (short)(Mathf.Abs(this.furthest.Value) / 130f * 100f);
			}
			if (base.IsOwner)
			{
				bool isAI = this.player.IsAI;
				return;
			}
		}
		else if (this.deathTimer.Elapsed(false))
		{
			this.ResetPlayer();
		}
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x0000DE15 File Offset: 0x0000C015
	private void LateUpdate()
	{
		this.closestTrigger = null;
		this.closestSqrDist = float.MaxValue;
		this.closestPoint = Vector3.zero;
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x000825F0 File Offset: 0x000807F0
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		if (!this.player.IsAI)
		{
			Vector3 lookPos = this.playerCam.GetLookPos();
			this.mover.SetForwardVector(new Vector3(lookPos.x, 0f, lookPos.z).normalized);
			Vector2 axis = new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
			input = new CharacterMoverInput(axis, this.player.RewiredPlayer.GetButtonDown(InputActions.Accept), false);
		}
		else
		{
			this.mover.SetForwardVector(-Vector3.forward);
			input = this.GetAIInput();
		}
		input.NullInput(!this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput);
		if (this.agent == null || !this.agent.isOnOffMeshLink)
		{
			this.mover.CalculateVelocity(input, Time.deltaTime);
			this.mover.DoMovement(Time.deltaTime);
		}
		this.mover.SmoothSlope();
		if (!this.player.IsAI && this.minigameController.Playable && !GameManager.IsGamePaused && GameManager.PollInput && this.minigameController.State != MinigameControllerState.EnablePlayers)
		{
			this.playerCam.RotateCamera();
			this.playerCam.UpdateCamera();
		}
		this.velocity.Value = this.mover.Velocity;
		this.netIsGrounded.Value = this.mover.Grounded;
		this.net_movement_axis.Value = this.playerAnim.GetMovementAxis();
		this.net_z_rotation.Value = ZPMath.CompressFloat(this.player.IsAI ? 0f : this.playerCam.ZRotation, -45f, 45f);
		base.DoMovement();
	}

	// Token: 0x060010A5 RID: 4261 RVA: 0x00082800 File Offset: 0x00080A00
	private CharacterMoverInput GetAIInput()
	{
		if (this.closestTrigger == null)
		{
			this.yOffset = 0f;
		}
		else
		{
			Vector3 vector = this.closestPoint - base.transform.position;
			this.yOffset = ((vector.z > 0f) ? 1f : -1f);
		}
		return new CharacterMoverInput(new Vector2(1f, this.yOffset), false, false);
	}

	// Token: 0x060010A6 RID: 4262 RVA: 0x00082878 File Offset: 0x00080A78
	protected override void UpdateAnimationState(PlayerAnimation playerAnim)
	{
		Vector2 vector = new Vector2(this.velocity.Value.x, this.velocity.Value.z);
		playerAnim.Velocity = vector.magnitude / this.mover.maxSpeed;
		playerAnim.VelocityY = this.velocity.Value.y;
		playerAnim.Grounded = this.netIsGrounded.Value;
		if (base.IsOwner)
		{
			playerAnim.SpineRotation = (this.player.IsAI ? 0f : this.playerCam.ZRotation);
			Vector3 vector2 = base.transform.rotation * new Vector3(this.mover.MovementAxis.y, 0f, this.mover.MovementAxis.x);
			playerAnim.MovementAxis = new Vector2(vector2.z, vector2.x);
		}
		else
		{
			playerAnim.MovementAxisByte = this.net_movement_axis.Value;
			playerAnim.SpineRotation = ZPMath.DecompressFloat(this.net_z_rotation.Value, -45f, 45f);
		}
		playerAnim.SetPlayerRotation(this.rotation.Value + 10f);
	}

	// Token: 0x060010A7 RID: 4263 RVA: 0x000829B8 File Offset: 0x00080BB8
	public override void ResetPlayer()
	{
		this.Activate();
		this.mover.Velocity = Vector3.zero;
		if (this.cam != null)
		{
			this.cam.transform.localRotation = Quaternion.identity;
		}
		if (this.cameraShake != null)
		{
			this.cameraShake.enabled = true;
		}
		base.ResetPlayer();
		base.transform.position = this.startPosition;
		base.transform.rotation = this.startRotation;
		if (this.playerCam != null)
		{
			this.playerCam.ZRotation = 5f;
			this.playerCam.YRotation = base.transform.rotation.eulerAngles.y;
		}
		this.playerAnim.ShotgunStrength = 0f;
	}

	// Token: 0x060010A8 RID: 4264 RVA: 0x0000DE34 File Offset: 0x0000C034
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x060010A9 RID: 4265 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x060010AA RID: 4266 RVA: 0x00082A94 File Offset: 0x00080C94
	public void OnTriggerEnter(Collider collider)
	{
		if (!base.IsOwner)
		{
			return;
		}
		if (collider.gameObject.name.Equals("KillTrigger"))
		{
			this.KillPlayer(collider.ClosestPoint(base.transform.position), 13f);
			return;
		}
		this.DoAITrigger(collider);
	}

	// Token: 0x060010AB RID: 4267 RVA: 0x00082A94 File Offset: 0x00080C94
	public void OnTriggerStay(Collider collider)
	{
		if (!base.IsOwner)
		{
			return;
		}
		if (collider.gameObject.name.Equals("KillTrigger"))
		{
			this.KillPlayer(collider.ClosestPoint(base.transform.position), 13f);
			return;
		}
		this.DoAITrigger(collider);
	}

	// Token: 0x060010AC RID: 4268 RVA: 0x00082AE8 File Offset: 0x00080CE8
	private void DoAITrigger(Collider collider)
	{
		Vector3 b = collider.ClosestPoint(base.transform.position);
		b = collider.gameObject.transform.position;
		Vector3 vector = base.transform.position - b;
		float sqrMagnitude = vector.sqrMagnitude;
		if (vector.x < 0f)
		{
			return;
		}
		if (this.closestTrigger == null)
		{
			this.closestTrigger = collider.gameObject;
			this.closestSqrDist = sqrMagnitude;
			this.closestPoint = b;
			return;
		}
		if (sqrMagnitude < this.closestSqrDist)
		{
			this.closestTrigger = collider.gameObject;
			this.closestSqrDist = sqrMagnitude;
			this.closestPoint = b;
		}
	}

	// Token: 0x04001110 RID: 4368
	public GameObject cameraPrefab;

	// Token: 0x04001111 RID: 4369
	public GameObject bloodyDamageEffect;

	// Token: 0x04001112 RID: 4370
	public AudioClip carHit;

	// Token: 0x04001113 RID: 4371
	private float lastBlood;

	// Token: 0x04001114 RID: 4372
	private float minBloodInterval = 0.5f;

	// Token: 0x04001115 RID: 4373
	private Camera cam;

	// Token: 0x04001116 RID: 4374
	private CharacterMover mover;

	// Token: 0x04001117 RID: 4375
	private ActionTimer deathTimer = new ActionTimer(2f);

	// Token: 0x04001118 RID: 4376
	private SidestepSlopeController minigameController;

	// Token: 0x04001119 RID: 4377
	private CameraShake cameraShake;

	// Token: 0x0400111A RID: 4378
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<float> furthest = new NetVar<float>(10f);

	// Token: 0x0400111B RID: 4379
	private float yOffset;

	// Token: 0x0400111C RID: 4380
	private GameObject closestTrigger;

	// Token: 0x0400111D RID: 4381
	private Vector3 closestPoint;

	// Token: 0x0400111E RID: 4382
	private float closestSqrDist = float.MaxValue;
}
