using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200028A RID: 650
public class VoxelTowerPlayer : Movement1
{
	// Token: 0x06001310 RID: 4880 RVA: 0x00092E74 File Offset: 0x00091074
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		this.minigameController = (VoxelTowerController)GameManager.Minigame;
		if (base.IsOwner)
		{
			CharacterMover characterMover = this.mover;
			characterMover.OnJump = (CharacterMover.OnJumpDelegate)Delegate.Combine(characterMover.OnJump, new CharacterMover.OnJumpDelegate(this.OnJump));
			List<GamePlayer> localNonAIPlayers = GameManager.GetLocalNonAIPlayers();
			if (!this.player.IsAI || localNonAIPlayers.Count == 0)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.camera_prefab, Vector3.zero, Quaternion.identity);
				gameObject.transform.parent = base.transform;
				this.cam = base.gameObject.GetComponentInChildren<Camera>();
				this.playerCam = base.gameObject.GetComponent<ThirdPersonCamera>();
				this.playerCam.SetTargetCamera(this.cam);
				this.playerCam.YRotation = base.transform.rotation.eulerAngles.y;
				this.playerCam.ZRotation = 5f;
				if (!this.player.IsAI)
				{
					this.playerCam.RotateCamera();
				}
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
				if ((localNonAIPlayers.Count > 0 && localNonAIPlayers[0] == this.player) || (localNonAIPlayers.Count == 0 && base.GetIndex(this.player) == 0))
				{
					gameObject.GetComponent<AudioListener>().enabled = true;
				}
			}
		}
		if (base.IsOwner)
		{
			bool isServer = NetSystem.IsServer;
		}
		if (!base.IsOwner || this.player.IsAI)
		{
			base.GetComponent<ThirdPersonCamera>().enabled = false;
		}
		if (!this.player.IsAI || !base.IsOwner)
		{
			base.GetComponent<NavMeshAgent>().enabled = false;
		}
	}

	// Token: 0x06001311 RID: 4881 RVA: 0x00093084 File Offset: 0x00091284
	public override void OnOwnerChanged()
	{
		if (NetSystem.IsServer)
		{
			base.GetComponent<NavMeshAgent>().enabled = true;
			if (this.agent != null)
			{
				this.agent.updatePosition = false;
				this.agent.updateRotation = false;
			}
			this.playerAnim.RegisterListener(new AnimationEventListener(this.BombReleaseEvent), AnimationEventType.ThrowRelease);
		}
		base.OnOwnerChanged();
	}

	// Token: 0x06001312 RID: 4882 RVA: 0x000930E8 File Offset: 0x000912E8
	private void UpdateTargets()
	{
		this.targetSlots.Clear();
		for (int i = 0; i < 8; i++)
		{
			if (i != (int)base.OwnerSlot && GameManager.IsPlayerInSlot(i) && !this.minigameController.GetPlayer(i).IsDead)
			{
				this.targetSlots.Add(i);
			}
		}
	}

	// Token: 0x06001313 RID: 4883 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06001314 RID: 4884 RVA: 0x0000F3C7 File Offset: 0x0000D5C7
	public override void FinishedSpawning()
	{
		this.playerAnim.Animator.SetFloat("ShotgunStrength", 0f);
		base.FinishedSpawning();
	}

	// Token: 0x06001315 RID: 4885 RVA: 0x0000F3E9 File Offset: 0x0000D5E9
	public override void Activate()
	{
		base.Activate();
		if (base.IsOwner)
		{
			this.playerAnim.RegisterListener(new AnimationEventListener(this.BombReleaseEvent), AnimationEventType.ThrowRelease);
		}
	}

	// Token: 0x06001316 RID: 4886 RVA: 0x0000F411 File Offset: 0x0000D611
	protected override void Start()
	{
		base.Start();
		this.minigameController.AddPlayer(this);
		this.target_sphere = base.transform.Find("TargetMarker").gameObject;
	}

	// Token: 0x06001317 RID: 4887 RVA: 0x0009313C File Offset: 0x0009133C
	private void Update()
	{
		base.UpdateController();
		if (this.agent != null)
		{
			this.agent.updatePosition = false;
			this.agent.updateRotation = false;
		}
		if (base.IsOwner && !this.player.IsAI && this.minigameController.Playable)
		{
			VoxelTowerBarrel interactTargetBigSmall = base.GetInteractTargetBigSmall<VoxelTowerBarrel>(262144, 524288, this.pickup_range, this.playerCam.GetScreenPointRay());
			if (interactTargetBigSmall != null && !interactTargetBigSmall.thrown)
			{
				if (interactTargetBigSmall != this.selected)
				{
					if (this.selected != null)
					{
						this.selected.Outline = false;
					}
					this.selected = interactTargetBigSmall;
					this.selected.Outline = true;
				}
			}
			else if (this.selected != null)
			{
				this.selected.Outline = false;
				this.selected = null;
			}
			Player rewiredPlayer = this.player.RewiredPlayer;
			bool buttonDown;
			if (rewiredPlayer != null && rewiredPlayer.controllers.GetLastActiveController() != null && rewiredPlayer.controllers.GetLastActiveController().type == ControllerType.Joystick)
			{
				buttonDown = this.player.RewiredPlayer.GetButtonDown(InputActions.Action1);
			}
			else
			{
				buttonDown = this.player.RewiredPlayer.GetButtonDown(InputActions.UseItemShoot);
			}
			if (GameManager.PollInput && !GameManager.IsGamePaused && buttonDown && !this.isDead)
			{
				if (this.held == null && this.selected != null && this.selected.holding_player == -1)
				{
					this.minigameController.AttemptPickup(this.selected.id, (short)base.OwnerSlot);
				}
				else if (this.held != null && !this.throw_started)
				{
					this.Throw();
				}
			}
			base.transform.position + base.transform.forward + new Vector3(0f, 0.3f, 0f);
			Vector3 vector = Quaternion.Euler(this.playerCam.ZRotation - 20f, this.playerCam.YRotation, 0f) * Vector3.forward;
			vector = new Vector3(base.transform.forward.x, vector.y, base.transform.forward.z);
			Debug.DrawRay(base.transform.position, vector * 10f, Color.green);
			vector.normalized * 17f;
		}
		if (base.IsOwner && this.minigameController.Playable && !base.IsDead && base.transform.position.y < -1.5f)
		{
			this.KillPlayer();
		}
	}

	// Token: 0x06001318 RID: 4888 RVA: 0x0000F440 File Offset: 0x0000D640
	private void Throw()
	{
		base.SendRPC("StartThrowRPC", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		this.playerAnim.FireThrowObjectTrigger();
		this.throw_started = true;
	}

	// Token: 0x06001319 RID: 4889 RVA: 0x00093414 File Offset: 0x00091614
	protected override void DoMovement()
	{
		if (!this.isDead)
		{
			CharacterMoverInput input = default(CharacterMoverInput);
			if (!this.player.IsAI)
			{
				Vector3 lookPos = this.playerCam.GetLookPos();
				this.mover.SetForwardVector(new Vector3(lookPos.x, 0f, lookPos.z).normalized);
				input = new CharacterMoverInput(new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical)), this.player.RewiredPlayer.GetButtonDown(InputActions.Accept), false);
			}
			else
			{
				input = this.GetAIInput();
			}
			input.NullInput(!this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || (this.player.IsAI && base.AIActive));
			this.mover.CalculateVelocity(input, Time.deltaTime);
			this.mover.DoMovement(Time.deltaTime);
			this.mover.SmoothSlope();
			if (this.agent != null)
			{
				this.agent.nextPosition = base.transform.position;
				this.agent.velocity = this.mover.Velocity;
			}
		}
		else if (!this.player.IsAI)
		{
			if (!this.spectating && this.player.RewiredPlayer.GetButtonDown(InputActions.Accept))
			{
				this.spectating = true;
			}
			else if (this.spectating)
			{
				if (this.spectating_player == null || this.spectating_player.IsDead)
				{
					for (int i = 0; i < this.minigameController.GetPlayerCount(); i++)
					{
						VoxelTowerPlayer voxelTowerPlayer = (VoxelTowerPlayer)this.minigameController.GetPlayerInSlot((short)i);
						if (voxelTowerPlayer != null && !voxelTowerPlayer.IsDead && voxelTowerPlayer != this)
						{
							this.spectating_player = voxelTowerPlayer;
							this.playerCam.SetTargetCamera(this.cam);
							this.playerCam.PositionalTargetTransform = this.spectating_player.transform;
							break;
						}
					}
				}
				else if (this.player.RewiredPlayer.GetButtonDown(InputActions.Accept))
				{
					int j = 0;
					int num = (int)this.spectating_player.OwnerSlot;
					while (j < this.minigameController.GetPlayerCount())
					{
						num++;
						if (num >= this.minigameController.GetPlayerCount())
						{
							num = 0;
						}
						VoxelTowerPlayer voxelTowerPlayer2 = (VoxelTowerPlayer)this.minigameController.GetPlayerInSlot((short)num);
						if (voxelTowerPlayer2 != null && !voxelTowerPlayer2.IsDead && voxelTowerPlayer2 != this)
						{
							this.spectating_player = voxelTowerPlayer2;
							this.playerCam.PositionalTargetTransform = this.spectating_player.transform;
							break;
						}
						j++;
					}
				}
			}
		}
		if (this.player.IsAI)
		{
			Vector3 vector = base.transform.position;
			if (this.targetBarrel != null)
			{
				vector = this.targetBarrel.transform.position;
			}
			else if (this.held != null && this.targetPlayer != -1)
			{
				Vector3 position = this.minigameController.GetPlayer(this.targetPlayer).transform.position;
				Vector3 position2 = base.transform.position;
				float magnitude = (position - position2).magnitude;
				vector = position - Vector3.up * (magnitude / 4f);
			}
			Debug.DrawLine(base.transform.position, vector, Color.blue);
			Vector3 vector2 = vector + this.lookAtOffset - base.transform.position;
			float magnitude2 = vector2.magnitude;
			Vector2 vector3 = new Vector2(0f, base.transform.position.y);
			Vector2 vector4 = new Vector2(vector2.magnitude, vector.y);
			float num2 = Mathf.Atan2(vector4.y - vector3.y, vector4.x - vector3.x) * 57.29578f;
			num2 = Mathf.Clamp(num2, -45f, 45f);
			this.aiTargetRot = num2;
			this.aiZRot = Mathf.MoveTowards(this.aiZRot, num2, Time.deltaTime * 360f);
		}
		if (!this.player.IsAI && this.minigameController.Playable && !GameManager.IsGamePaused && GameManager.PollInput && this.minigameController.State != MinigameControllerState.EnablePlayers && !this.spectating)
		{
			if (!this.player.IsAI)
			{
				this.playerCam.RotateCamera();
			}
			this.playerCam.UpdateCamera();
		}
		this.velocity.Value = this.mover.Velocity;
		this.netIsGrounded.Value = this.mover.Grounded;
		this.net_movement_axis.Value = this.playerAnim.GetMovementAxis();
		this.net_z_rotation.Value = ZPMath.CompressFloat(this.player.IsAI ? this.aiZRot : this.playerCam.ZRotation, -45f, 45f);
	}

	// Token: 0x0600131A RID: 4890 RVA: 0x00093974 File Offset: 0x00091B74
	private void LateUpdate()
	{
		if (this.spectating && !this.player.IsAI && this.minigameController.Playable && !GameManager.IsGamePaused && GameManager.PollInput && this.minigameController.State != MinigameControllerState.EnablePlayers)
		{
			this.playerCam.RotateCamera();
			this.playerCam.UpdateCamera();
		}
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x000939D8 File Offset: 0x00091BD8
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		Vector3 vector = Vector3.zero;
		if (this.held == null)
		{
			if (this.targetBarrel == null || this.targetBarrel.thrown || this.targetBarrel.holding_player != -1 || this.targetChangeTimer.Elapsed(false))
			{
				VoxelTowerBarrel voxelTowerBarrel = null;
				float num = float.MaxValue;
				foreach (VoxelTowerBarrel voxelTowerBarrel2 in this.minigameController.barrels)
				{
					if (voxelTowerBarrel2 != null && !voxelTowerBarrel2.thrown && voxelTowerBarrel2.holding_player == -1)
					{
						float sqrMagnitude = (voxelTowerBarrel2.transform.position - base.transform.position).sqrMagnitude;
						if (sqrMagnitude < num && sqrMagnitude < 144f)
						{
							num = sqrMagnitude;
							voxelTowerBarrel = voxelTowerBarrel2;
						}
					}
				}
				this.targetBarrel = voxelTowerBarrel;
				if (this.targetBarrel != null)
				{
					this.targetChangeTimer.Start();
				}
			}
			float num2 = 1.5f;
			if (this.targetBarrel != null)
			{
				vector = this.targetBarrel.transform.position - base.transform.position;
				vector.y = 0f;
				float magnitude = vector.magnitude;
				if (magnitude < num2 && this.pickupTimerInterval.Elapsed(true))
				{
					this.minigameController.AttemptPickup(this.targetBarrel.id, (short)base.OwnerSlot);
					this.targetBarrel = null;
					this.pickupTimerInterval.Start();
				}
				float num3 = 0.15f;
				float max = 2f;
				Mathf.Clamp01(Mathf.Clamp(magnitude - num2, 0f, max) / 2f + num3);
				Vector3 vector2 = this.agent.steeringTarget - base.transform.position;
				vector2.y = 0f;
				vector2.Normalize();
				result = new CharacterMoverInput(new Vector2(vector2.x, vector2.z), false, false);
				if (this.pathUpdateTimer.Elapsed(true) && this.targetBarrel != null)
				{
					NavMeshHit navMeshHit;
					NavMesh.SamplePosition(this.targetBarrel.transform.position, out navMeshHit, 1f, -1);
					Debug.DrawLine(base.transform.position, navMeshHit.position, Color.cyan);
					if (this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
					{
						this.agent.SetDestination(this.targetBarrel.transform.position);
					}
				}
			}
			else
			{
				result = this.RandomMovement();
			}
			this.targetPlayer = -1;
		}
		else
		{
			if (this.targetPlayer == -1)
			{
				this.UpdateTargets();
				if (this.targetSlots.Count != 0)
				{
					this.targetPlayer = this.targetSlots[GameManager.rand.Next(0, this.targetSlots.Count)];
				}
			}
			if (this.targetPlayer != -1)
			{
				vector = this.minigameController.GetPlayer(this.targetPlayer).transform.position - base.transform.position;
				vector.y = 0f;
				vector.Normalize();
				if (this.throwInterval.Elapsed(false) && Vector3.Angle(vector, base.transform.forward) < 10f && !this.throw_started && Mathf.Abs(this.aiZRot - this.aiTargetRot) < 1f)
				{
					this.throwInterval.Start();
					this.Throw();
				}
			}
			result = this.RandomMovement();
		}
		if (vector != Vector3.zero)
		{
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(vector), 550f * Time.deltaTime);
		}
		return result;
	}

	// Token: 0x0600131C RID: 4892 RVA: 0x00093DD8 File Offset: 0x00091FD8
	private CharacterMoverInput RandomMovement()
	{
		if (this.randomMovementTimer.Elapsed(true))
		{
			this.randomMovementVector = ZPMath.RandomPointInUnitSphere(GameManager.rand) * 3f;
			this.randomMovementVector.y = -0.875f;
		}
		Vector3 vector = this.agent.steeringTarget - base.transform.position;
		vector.y = 0f;
		vector.Normalize();
		CharacterMoverInput result = new CharacterMoverInput(new Vector2(vector.x, vector.z), false, false);
		if (this.pathUpdateTimer.Elapsed(true))
		{
			NavMeshHit navMeshHit;
			NavMesh.SamplePosition(base.transform.position + this.randomMovementVector, out navMeshHit, 5f, -1);
			if (this.agent.isOnNavMesh)
			{
				this.agent.SetDestination(navMeshHit.position);
			}
		}
		return result;
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x00093EB8 File Offset: 0x000920B8
	protected override void UpdateAnimationState(PlayerAnimation playerAnim)
	{
		Vector2 vector = new Vector2(this.velocity.Value.x, this.velocity.Value.z);
		playerAnim.Velocity = vector.magnitude / this.mover.maxSpeed;
		playerAnim.VelocityY = this.velocity.Value.y;
		playerAnim.Grounded = this.netIsGrounded.Value;
		playerAnim.Carrying = (this.held != null);
		if (base.IsOwner)
		{
			playerAnim.SpineRotation = (this.player.IsAI ? this.aiZRot : this.playerCam.ZRotation);
			Vector3 vector2 = base.transform.rotation * new Vector3(this.mover.MovementAxis.y, 0f, this.mover.MovementAxis.x);
			playerAnim.MovementAxis = new Vector2(vector2.z, vector2.x);
		}
		else
		{
			playerAnim.MovementAxisByte = this.net_movement_axis.Value;
			playerAnim.SpineRotation = ZPMath.DecompressFloat(this.net_z_rotation.Value, -45f, 45f);
		}
		playerAnim.SetPlayerRotation(this.rotation.Value);
	}

	// Token: 0x0600131E RID: 4894 RVA: 0x00094004 File Offset: 0x00092204
	private void BombReleaseEvent(PlayerAnimationEvent anim_event)
	{
		Vector3 vector = Quaternion.Euler((this.player.IsAI ? this.aiZRot : this.playerCam.ZRotation) - 0f, this.playerCam.YRotation, 0f) * Vector3.forward;
		vector = new Vector3(base.transform.forward.x, vector.y, base.transform.forward.z);
		this.throw_started = false;
		this.minigameController.RelayThrow(this.held.id, this.held.transform.position, vector, this.minigameController.RandomAngularVelocity(this.minigameController.angular_vel_range), false);
		this.hasThrownbarrel = true;
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x0000F465 File Offset: 0x0000D665
	public Transform GetHoldTransform()
	{
		return this.playerAnim.GetBone(PlayerBone.LeftHand);
	}

	// Token: 0x06001320 RID: 4896 RVA: 0x0000F474 File Offset: 0x0000D674
	public Vector3 GetHoldPosition()
	{
		return this.playerAnim.GetBone(PlayerBone.LeftHand).position;
	}

	// Token: 0x06001321 RID: 4897 RVA: 0x0000F488 File Offset: 0x0000D688
	public Quaternion GetHoldRotation()
	{
		return this.playerAnim.GetBone(PlayerBone.LeftHand).rotation;
	}

	// Token: 0x06001322 RID: 4898 RVA: 0x000940D0 File Offset: 0x000922D0
	private void OnTriggerEnter(Collider c)
	{
		if (base.IsOwner && this.minigameController.Playable && !base.IsDead)
		{
			if (c.gameObject.name == "DeathZone")
			{
				this.KillPlayer();
				return;
			}
			c.gameObject.name == "DamageBox";
		}
	}

	// Token: 0x06001323 RID: 4899 RVA: 0x0000F49C File Offset: 0x0000D69C
	private void OnTriggerStay(Collider c)
	{
		if (Time.time - this.last_damage_time > this.damage_interval)
		{
			bool isDead = base.IsDead;
		}
	}

	// Token: 0x06001324 RID: 4900 RVA: 0x0000F4B9 File Offset: 0x0000D6B9
	private void OnTriggerExit(Collider c)
	{
		this.last_damage_time = 0f;
	}

	// Token: 0x06001325 RID: 4901 RVA: 0x00094130 File Offset: 0x00092330
	private void DoDamage(short amount)
	{
		this.last_damage_time = Time.time;
		this.health -= amount;
		AudioSystem.PlayOneShot(this.damage_sound, 1f, 0f, 1f);
		UnityEngine.Object.Instantiate<GameObject>(this.damage_particle, base.transform.position, Quaternion.identity);
		if (this.health <= 0)
		{
			this.KillPlayer();
		}
	}

	// Token: 0x06001326 RID: 4902 RVA: 0x0000F4C6 File Offset: 0x0000D6C6
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x06001327 RID: 4903 RVA: 0x0000480A File Offset: 0x00002A0A
	private void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06001328 RID: 4904 RVA: 0x0009419C File Offset: 0x0009239C
	public override void ResetPlayer()
	{
		this.health = 10;
		base.IsDead = false;
		base.transform.position = this.startPosition;
		base.transform.rotation = this.startRotation;
		if (base.IsOwner)
		{
			this.mover.Velocity = Vector3.zero;
			this.playerCam.YRotation = base.transform.rotation.eulerAngles.y;
			this.playerCam.ZRotation = 10f;
			this.playerCam.RotateCamera();
			this.playerCam.UpdateCamera();
		}
		this.player_root.SetActive(true);
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x0000F4CE File Offset: 0x0000D6CE
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void StartThrowRPC(NetPlayer sender)
	{
		this.playerAnim.FireThrowObjectTrigger();
	}

	// Token: 0x0600132A RID: 4906 RVA: 0x0000F4DB File Offset: 0x0000D6DB
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender)
	{
		this.KillPlayer();
	}

	// Token: 0x0600132B RID: 4907 RVA: 0x00094248 File Offset: 0x00092448
	public void KillPlayer()
	{
		if (!this.isDead)
		{
			if (base.IsOwner && !this.player.IsAI && !this.hasThrownbarrel)
			{
				PlatformAchievementManager.Instance.TriggerAchievement("ACH_ACIDIC_ATOLL");
			}
			this.death_time = Time.time;
			UnityEngine.Object.Instantiate<GameObject>(this.player_death_effect, base.transform.position, Quaternion.LookRotation(Vector3.up));
			AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
			this.isDead = true;
			this.player_root.SetActive(false);
			if (NetSystem.IsServer)
			{
				this.minigameController.PlayerDied(this);
			}
			if (base.IsOwner)
			{
				if (!this.player.IsAI && !VoxelTowerPlayer.spectateActive)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.spectateUIElement);
					gameObject.transform.SetParent(GameManager.UIController.MinigameUIRoot, false);
					((RectTransform)gameObject.transform).anchoredPosition = new Vector2(0f, 150f);
					gameObject.GetComponentInChildren<UIGlyph>().SetPlayer(base.GamePlayer.RewiredPlayer);
					VoxelTowerPlayer.spectateActive = false;
				}
				this.mover.Velocity = Vector3.zero;
				base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
			}
		}
	}

	// Token: 0x04001457 RID: 5207
	public GameObject camera_prefab;

	// Token: 0x04001458 RID: 5208
	public AudioClip damage_sound;

	// Token: 0x04001459 RID: 5209
	public GameObject damage_particle;

	// Token: 0x0400145A RID: 5210
	public VoxelTowerBomb held_bomb;

	// Token: 0x0400145B RID: 5211
	public VoxelTowerBarrel held;

	// Token: 0x0400145C RID: 5212
	public GameObject player_death_effect;

	// Token: 0x0400145D RID: 5213
	public VoxelTowerController minigameController;

	// Token: 0x0400145E RID: 5214
	public short health = 10;

	// Token: 0x0400145F RID: 5215
	public GameObject spectateUIElement;

	// Token: 0x04001460 RID: 5216
	private Camera cam;

	// Token: 0x04001461 RID: 5217
	private float death_time;

	// Token: 0x04001462 RID: 5218
	private float death_length = 0.1f;

	// Token: 0x04001463 RID: 5219
	private CharacterMover mover;

	// Token: 0x04001464 RID: 5220
	private float last_damage_time;

	// Token: 0x04001465 RID: 5221
	private float damage_interval = 0.5f;

	// Token: 0x04001466 RID: 5222
	private Vector3 marker_offset = new Vector3(0f, 0.15f, 0f);

	// Token: 0x04001467 RID: 5223
	private bool throw_started;

	// Token: 0x04001468 RID: 5224
	private VoxelTowerBarrel selected;

	// Token: 0x04001469 RID: 5225
	private VoxelTowerPlayer spectating_player;

	// Token: 0x0400146A RID: 5226
	private bool spectating;

	// Token: 0x0400146B RID: 5227
	private GameObject target_sphere;

	// Token: 0x0400146C RID: 5228
	private bool hasThrownbarrel;

	// Token: 0x0400146D RID: 5229
	private VoxelTowerBarrel targetBarrel;

	// Token: 0x0400146E RID: 5230
	private Vector3 lookAtOffset = Vector3.zero;

	// Token: 0x0400146F RID: 5231
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04001470 RID: 5232
	private List<int> targetSlots = new List<int>();

	// Token: 0x04001471 RID: 5233
	private int targetPlayer = -1;

	// Token: 0x04001472 RID: 5234
	private ActionTimer pickupTimerInterval = new ActionTimer(0.5f);

	// Token: 0x04001473 RID: 5235
	private ActionTimer targetChangeTimer = new ActionTimer(2.5f, 3f);

	// Token: 0x04001474 RID: 5236
	private ActionTimer throwInterval = new ActionTimer(1f, 1.5f);

	// Token: 0x04001475 RID: 5237
	private Vector3 randomMovementVector = Vector3.zero;

	// Token: 0x04001476 RID: 5238
	private ActionTimer randomMovementTimer = new ActionTimer(0.75f, 2f);

	// Token: 0x04001477 RID: 5239
	private float aiTargetRot;

	// Token: 0x04001478 RID: 5240
	private float aiZRot;

	// Token: 0x04001479 RID: 5241
	private float pickup_range = 4.5f;

	// Token: 0x0400147A RID: 5242
	private static bool spectateActive;
}
