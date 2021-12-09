using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000290 RID: 656
public class WarlockPlayerNew1 : Movement1
{
	// Token: 0x1700019F RID: 415
	// (get) Token: 0x0600134F RID: 4943 RVA: 0x0000F5E6 File Offset: 0x0000D7E6
	public bool Stunned
	{
		get
		{
			return this.stunned;
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06001350 RID: 4944 RVA: 0x0000F5EE File Offset: 0x0000D7EE
	// (set) Token: 0x06001351 RID: 4945 RVA: 0x00094DE4 File Offset: 0x00092FE4
	public bool BeingHit
	{
		get
		{
			return this.beingHit.Value;
		}
		set
		{
			this.flameParticles.enableEmission = (value || base.IsDead);
			if (value)
			{
				if (this.source == null)
				{
					this.source = AudioSystem.PlayLooping(this.sizzleClip, 0.25f, 0.6f);
				}
			}
			else if (this.source != null)
			{
				this.source.FadeAudio(0.5f, FadeType.Out);
				this.source = null;
			}
			if (base.IsOwner)
			{
				this.beingHit.Value = value;
			}
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06001352 RID: 4946 RVA: 0x0000F5FB File Offset: 0x0000D7FB
	// (set) Token: 0x06001353 RID: 4947 RVA: 0x00094E70 File Offset: 0x00093070
	public byte Health
	{
		get
		{
			return this.health.Value;
		}
		set
		{
			this.health.Value = value;
			if (this.minigameController != null && this.minigameController.Playable)
			{
				this.Score = (short)value;
				if (base.IsOwner && value == 0)
				{
					this.KillPlayer(GameManager.rand.Next(int.MaxValue), true);
				}
			}
		}
	}

	// Token: 0x06001354 RID: 4948 RVA: 0x00094ECC File Offset: 0x000930CC
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.lastInput = base.transform.forward;
		this.mover = base.GetComponent<CharacterMover>();
		this.mover.SetForwardVector(new Vector3(0f, 0f, -1f));
		if (NetSystem.IsServer)
		{
			this.playerAnim.RegisterListener(new AnimationEventListener(this.FireballAnimationEndEvent), AnimationEventType.ThrowRelease);
		}
		this.playerAnim.RegisterListener(new AnimationEventListener(this.OnWarlockAttackFinish), AnimationEventType.WarlockAttackFinish);
		this.playerAnim.RegisterListener(new AnimationEventListener(this.OnWarlockKnockbackFinish), AnimationEventType.WarlockKnockbackFinish);
		this.playerAnim.SetPlayerRotationImmediate(base.transform.rotation.eulerAngles.y);
		if (!base.IsOwner)
		{
			NetVar<byte> netVar = this.health;
			netVar.Recieve = (RecieveProxy)Delegate.Combine(netVar.Recieve, new RecieveProxy(this.DamageRecieve));
			NetVar<bool> netVar2 = this.beingHit;
			netVar2.Recieve = (RecieveProxy)Delegate.Combine(netVar2.Recieve, new RecieveProxy(this.BeingHitRecieve));
		}
		if (this.player.IsAI)
		{
			base.gameObject.AddComponent<Rigidbody>().isKinematic = true;
			CapsuleCollider capsuleCollider = base.gameObject.AddComponent<CapsuleCollider>();
			capsuleCollider.radius = this.controller.radius;
			capsuleCollider.height = this.controller.height;
			this.shootTimer = new ActionTimer(0.7f, 1.2f);
		}
		for (int i = 0; i < this.handFireballs.Length; i++)
		{
			if ((int)base.GamePlayer.ColorIndex == i)
			{
				this.handFireballs[i].SetActive(true);
			}
		}
		if (base.IsOwner)
		{
			bool isAI = this.player.IsAI;
		}
		if (!this.player.IsAI || !base.IsOwner)
		{
			base.GetComponent<NavMeshAgent>().enabled = false;
		}
		if (this.player.IsAI && base.IsOwner)
		{
			this.mover.IsAI = true;
		}
	}

	// Token: 0x06001355 RID: 4949 RVA: 0x000950C8 File Offset: 0x000932C8
	public override void OnOwnerChanged()
	{
		if (NetSystem.IsServer)
		{
			Rigidbody rigidbody = base.gameObject.GetComponent<Rigidbody>();
			if (rigidbody == null)
			{
				rigidbody = base.gameObject.AddComponent<Rigidbody>();
			}
			rigidbody.isKinematic = true;
			CapsuleCollider capsuleCollider = base.gameObject.GetComponent<CapsuleCollider>();
			if (capsuleCollider == null)
			{
				capsuleCollider = base.gameObject.AddComponent<CapsuleCollider>();
			}
			capsuleCollider.radius = this.controller.radius;
			capsuleCollider.height = this.controller.height;
			this.shootTimer = new ActionTimer(0.7f, 1.2f);
			base.GetComponent<NavMeshAgent>().enabled = true;
			this.mover.IsAI = true;
		}
		base.OnOwnerChanged();
	}

	// Token: 0x06001356 RID: 4950 RVA: 0x00095180 File Offset: 0x00093380
	protected override void Start()
	{
		this.minigameController = (WarlockControllerNew)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
		base.StartCoroutine(this.SetupEnemies(1f));
		base.Start();
	}

	// Token: 0x06001357 RID: 4951 RVA: 0x0000F608 File Offset: 0x0000D808
	private IEnumerator SetupEnemies(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		this.enemies = new WarlockPlayerNew1[this.minigameController.GetPlayerCount() - 1];
		int num = 0;
		for (int i = 0; i < this.minigameController.GetPlayerCount(); i++)
		{
			WarlockPlayerNew1 warlockPlayerNew = (WarlockPlayerNew1)this.minigameController.GetPlayer(i);
			if (warlockPlayerNew != this)
			{
				this.enemies[num] = warlockPlayerNew;
				num++;
			}
		}
		yield break;
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x000951F0 File Offset: 0x000933F0
	private void Update()
	{
		base.UpdateController();
		if (this.minigameController.Playable && base.IsOwner && !this.isDead)
		{
			if (base.transform.position.y < -0.255f || base.transform.position.y > 1.571f)
			{
				base.transform.position = new Vector3(0f, 1f, 0f);
				this.mover.Velocity = Vector3.zero;
			}
			if (!this.player.IsAI && !GameManager.IsGamePaused && (this.player.RewiredPlayer.GetButton(InputActions.UseItemShoot) || this.player.RewiredPlayer.GetButtonDown(InputActions.UseItemShoot)) && !this.stunned && this.attackAvailable && this.shootTimer.Elapsed(false))
			{
				this.StartShootFireBall();
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, Vector3.down, out raycastHit, 2f, 1024))
			{
				this.onLava = (raycastHit.collider.gameObject.tag == "MinigameCustom01");
				this.onLavaOrFallingRock = (raycastHit.collider.gameObject.tag == "MinigameCustom01" || raycastHit.collider.gameObject.tag == "MinigameCustom02");
			}
			if (this.onLava)
			{
				this.BeingHit = true;
				this.lavaCounter += Time.deltaTime;
				if (this.lavaCounter > 0.05f)
				{
					this.HitLava = true;
					byte b = this.Health;
					this.Health = b - 1;
					this.lavaCounter -= 0.05f;
				}
			}
			else
			{
				this.BeingHit = false;
			}
		}
		if (this.player_root.activeSelf)
		{
			this.UpdateAnimationState(this.playerAnim);
			this.playerAnim.UpdateAnimationState();
		}
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x0000F61E File Offset: 0x0000D81E
	private void StartShootFireBall()
	{
		this.shootTimer.Start();
		base.SendRPC("StartShootRPC", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		this.playerAnim.FireThrowObjectTrigger();
		this.attackAvailable = false;
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x000953F8 File Offset: 0x000935F8
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		bool val = !this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || this.Stunned || this.isDead;
		if (!base.GamePlayer.IsAI)
		{
			Vector2 axis = new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
			input = new CharacterMoverInput(axis, false, false);
			if (this.minigameController.MinigameCamera != null)
			{
				Controller lastActiveController = this.player.RewiredPlayer.controllers.GetLastActiveController();
				if (lastActiveController != null && lastActiveController.type == ControllerType.Joystick)
				{
					Vector3 vector = new Vector3(this.player.RewiredPlayer.GetAxis(InputActions.LookHorizontal), 0f, this.player.RewiredPlayer.GetAxis(InputActions.LookVertical));
					if (vector.sqrMagnitude > 0.04f)
					{
						this.lastInput = vector;
					}
					this.targetPoint = base.transform.position - this.lastInput;
				}
				else
				{
					Ray ray = this.minigameController.MinigameCamera.ScreenPointToRay(Input.mousePosition);
					float d = (0f - ray.origin.y) / ray.direction.y;
					Vector3 vector2 = ray.origin + d * ray.direction;
					this.targetPoint = vector2;
				}
			}
		}
		else
		{
			input = this.GetAIInput();
		}
		input.NullInput(val);
		this.mover.CalculateVelocity(input, Time.deltaTime);
		this.mover.DoMovement(Time.deltaTime);
		this.targetPoint -= base.transform.position;
		this.targetPoint.y = 0f;
		this.targetPoint.Normalize();
		if (this.rotate && !this.stunned)
		{
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(this.targetPoint), 1500f * Time.deltaTime);
		}
		this.velocity.Value = this.mover.Velocity;
	}

	// Token: 0x0600135B RID: 4955 RVA: 0x00095650 File Offset: 0x00093850
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		Vector2 vector = Vector2.zero;
		Vector3 a = Vector3.one;
		if (this.minigameController.PlayersAlive == 1)
		{
			return result;
		}
		if (!GameManager.IsGamePaused && GameManager.PollInput && this.minigameController.State == MinigameControllerState.Playing)
		{
			Vector2 b = new Vector2(base.transform.position.x, base.transform.position.z);
			if (this.aiGetTargetTimer.Elapsed(true) || this.targetPlayer == null || this.targetPlayer.Stunned)
			{
				this.AIGetTarget();
			}
			if (this.avoidanceTimer.Elapsed(true))
			{
				float d = 60f;
				float num = 1f;
				WarlocksFireballNew x = null;
				Vector3 a2 = Vector3.zero;
				float num2 = float.MaxValue;
				for (int i = 0; i < this.minigameController.fireballs.Count; i++)
				{
					WarlocksFireballNew warlocksFireballNew = this.minigameController.fireballs[i];
					Vector3 vector2 = ZPMath.ProjectPointOnLineSegment(warlocksFireballNew.transform.position, warlocksFireballNew.transform.position + warlocksFireballNew.Dir * d, base.transform.position);
					if (!vector2.Equals(warlocksFireballNew.transform.position) && (vector2 - base.transform.position).sqrMagnitude < num)
					{
						float sqrMagnitude = (warlocksFireballNew.transform.position - base.transform.position).sqrMagnitude;
						if (sqrMagnitude < num2)
						{
							x = warlocksFireballNew;
							a2 = vector2;
							num2 = sqrMagnitude;
						}
					}
				}
				if (base.GamePlayer.Difficulty != BotDifficulty.Hard)
				{
					x = null;
				}
				if (x != null)
				{
					Vector3 vector3 = a2 - base.transform.position;
					this.curAvoidanceVec = new Vector2(vector3.x, vector3.z);
					this.curAvoidanceVec.Normalize();
					Vector3 vector4 = base.transform.position - Vector3.zero;
					Vector2 normalized = new Vector2(vector4.x, vector4.z).normalized;
					float num3 = Vector3.Dot(normalized, this.curAvoidanceVec);
					if (Vector3.Dot(normalized, -this.curAvoidanceVec) > num3)
					{
						this.curAvoidanceVec = -this.curAvoidanceVec;
					}
				}
				else
				{
					this.curAvoidanceVec = Vector2.zero;
				}
				if (this.targetPlayer != null && (this.targetPlayer.transform.position - base.transform.position).sqrMagnitude < 4f)
				{
					Vector3 vector5 = Vector3.zero - base.transform.position;
					float sqrMagnitude2 = vector5.sqrMagnitude;
					float sqrMagnitude3 = (Vector3.zero - this.targetPlayer.transform.position).sqrMagnitude;
					if (sqrMagnitude2 <= sqrMagnitude3)
					{
						vector5.y = 0f;
						this.curAvoidanceVec = vector5.normalized;
					}
				}
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(this.targetPlayer.transform.position, Vector3.down, out raycastHit, 6f, 1024))
			{
				this.targetPosition = raycastHit.point;
				a = raycastHit.point;
				if (this.offsetTimer.Elapsed(true))
				{
					float d2 = ZPMath.RandomFloat(GameManager.rand, this.fireballOffsetMin[(int)base.GamePlayer.Difficulty], this.fireballOffsetMax[(int)base.GamePlayer.Difficulty]);
					this.tarOffset = ZPMath.RandomPointInUnitSphere(GameManager.rand).normalized * d2;
				}
				a += this.tarOffset;
			}
			if (this.onLavaOrFallingRock)
			{
				if (this.agent.isOnNavMesh)
				{
					NavMeshHit navMeshHit;
					NavMesh.FindClosestEdge(base.transform.position, out navMeshHit, 1);
					this.targetPosition = navMeshHit.position;
				}
				else
				{
					this.targetPosition = Vector3.zero;
				}
			}
			if (this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
			{
				if (this.pathUpdateTimer.Elapsed(true))
				{
					this.agent.SetDestination(this.targetPosition);
				}
				Vector2 a3 = new Vector2(this.targetPosition.x, this.targetPosition.z);
				if (!this.onLavaOrFallingRock)
				{
					float num4 = this.reachDistance;
				}
				else
				{
					float num5 = this.evadeLavaDistance;
				}
				bool flag = this.canSeeTarget;
				Vector3 vector6 = base.transform.position - this.agent.steeringTarget;
				vector = new Vector2(vector6.x, vector6.z).normalized;
				float sqrMagnitude4 = (a3 - b).sqrMagnitude;
				if (!this.onLavaOrFallingRock)
				{
					if (sqrMagnitude4 <= this.minReachDistance)
					{
						vector = -vector;
					}
					else if (sqrMagnitude4 <= this.reachDistance)
					{
						vector = Vector2.zero;
					}
				}
			}
			else
			{
				Vector3 vector7 = base.transform.position - this.targetPosition;
				vector = new Vector2(vector7.x, vector7.z).normalized;
			}
			if (this.curAvoidanceVec != Vector2.zero)
			{
				vector += this.curAvoidanceVec;
				vector.Normalize();
			}
		}
		this.targetPoint = a;
		if (this.targetPlayer != null && !this.targetPlayer.IsDead && !this.stunned && this.attackAvailable && this.shootTimer.Elapsed(false))
		{
			this.StartShootFireBall();
		}
		return new CharacterMoverInput(vector, false, false);
	}

	// Token: 0x0600135C RID: 4956 RVA: 0x00095C1C File Offset: 0x00093E1C
	private void AddPossibleTarget(float dist, WarlockPlayerNew1 player)
	{
		for (int i = 0; i < this.m_possibleTargets.Count; i++)
		{
			if (dist < this.m_possibleTargets[i].sqrDist)
			{
				this.m_possibleTargets.Insert(i, new WarlockPlayerNew1.PossibleTarget(dist, player));
				return;
			}
		}
		this.m_possibleTargets.Add(new WarlockPlayerNew1.PossibleTarget(dist, player));
	}

	// Token: 0x0600135D RID: 4957 RVA: 0x00095C7C File Offset: 0x00093E7C
	private void AIGetTarget()
	{
		this.m_possibleTargets.Clear();
		for (int i = 0; i < this.enemies.Length; i++)
		{
			if (!this.enemies[i].isDead && !this.enemies[i].Stunned)
			{
				float sqrMagnitude = (base.transform.position - this.enemies[i].transform.position).sqrMagnitude;
				this.AddPossibleTarget(sqrMagnitude, this.enemies[i]);
			}
		}
		if (this.m_possibleTargets.Count == 0)
		{
			return;
		}
		this.targetPlayer = this.m_possibleTargets[0].player;
		this.canSeeTarget = !Physics.Linecast(base.MidPoint, this.targetPlayer.MidPoint, 2048);
		Debug.DrawLine(base.MidPoint, this.targetPlayer.MidPoint, this.canSeeTarget ? Color.green : Color.red, this.aiGetTargetTimer.interval);
	}

	// Token: 0x0600135E RID: 4958 RVA: 0x0000F64E File Offset: 0x0000D84E
	private void OnWarlockKnockbackFinish(PlayerAnimationEvent anim_event)
	{
		this.stunned = false;
		this.attackAvailable = true;
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x0000F65E File Offset: 0x0000D85E
	private void OnWarlockAttackFinish(PlayerAnimationEvent anim_event)
	{
		this.attackAvailable = true;
	}

	// Token: 0x06001360 RID: 4960 RVA: 0x00095D80 File Offset: 0x00093F80
	protected override void UpdateAnimationState(PlayerAnimation player_anim)
	{
		Vector2 vector = new Vector2(this.velocity.Value.x, this.velocity.Value.z);
		float num = Mathf.Clamp01(vector.magnitude / this.mover.maxSpeed);
		this.playerAnim.Velocity = num;
		this.playerAnim.VelocityY = this.velocity.Value.y;
		this.playerAnim.MovementAxis = ((num > 0.01f) ? Vector2.up : Vector2.zero);
		this.playerAnim.Grounded = this.netIsGrounded.Value;
		this.playerAnim.SetPlayerRotation(this.rotation.Value);
	}

	// Token: 0x06001361 RID: 4961 RVA: 0x0000F667 File Offset: 0x0000D867
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void StartShootRPC(NetPlayer sender)
	{
		this.playerAnim.FireThrowObjectTrigger();
		this.attackAvailable = false;
	}

	// Token: 0x06001362 RID: 4962 RVA: 0x00095E40 File Offset: 0x00094040
	private void FireballAnimationEndEvent(PlayerAnimationEvent anim_event)
	{
		this.ShootFireball(this.playerAnim.GetBone(PlayerBone.RightHand).position, base.transform.forward, this.minigameController.fire_ball_counter);
		WarlockControllerNew warlockControllerNew = this.minigameController;
		warlockControllerNew.fire_ball_counter += 1;
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x0000F67B File Offset: 0x0000D87B
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void ShootFireballRPC(NetPlayer sender, Vector3 start_position, Vector3 dir, short id)
	{
		if (this.minigameController.State == MinigameControllerState.Playing)
		{
			this.ShootFireball(start_position, dir, id);
		}
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x00095E90 File Offset: 0x00094090
	private void ShootFireball(Vector3 start_position, Vector3 dir, short id)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("ShootFireballRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				start_position,
				dir,
				id
			});
		}
		this.minigameController.Spawn(this.fireballProjectileEffect[(int)base.GamePlayer.ColorIndex], start_position, Quaternion.identity).GetComponent<WarlocksFireballNew>().Setup(dir, (short)base.OwnerSlot, id);
		AudioSystem.PlayOneShot("MagicFireball_ATR_JoelAudio", 0.5f, 0.1f);
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x00095F1C File Offset: 0x0009411C
	public void Push(Vector3 dir)
	{
		if (base.IsOwner)
		{
			this.mover.Velocity = dir;
			base.SendRPC("PushRPC", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
			dir.y = 0f;
			base.transform.rotation = Quaternion.LookRotation(-dir.normalized);
		}
		this.stunned = true;
		this.playerAnim.FireFallTrigger();
	}

	// Token: 0x06001366 RID: 4966 RVA: 0x0000F695 File Offset: 0x0000D895
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void PushRPC(NetPlayer sender)
	{
		if (this.minigameController.State == MinigameControllerState.Playing)
		{
			this.Push(Vector3.zero);
		}
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x0000F6B0 File Offset: 0x0000D8B0
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender, int seed)
	{
		this.KillPlayer(seed, false);
	}

	// Token: 0x06001368 RID: 4968 RVA: 0x00095F88 File Offset: 0x00094188
	public void KillPlayer(int seed, bool send_rpc = true)
	{
		if (!this.isDead)
		{
			this.BeingHit = false;
			this.isDead = true;
			this.Deactivate();
			UnityEngine.Object.Instantiate<GameObject>(this.player_death_effect, base.transform.position, Quaternion.LookRotation(Vector3.up));
			AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
			this.mover.Velocity = Vector3.zero;
			this.cameraShake.AddShake(0.3f);
			this.minigameController.PlayerDied(this);
			if (base.IsOwner && send_rpc)
			{
				base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_UNORDERED, new object[]
				{
					seed
				});
			}
		}
	}

	// Token: 0x06001369 RID: 4969 RVA: 0x0009603C File Offset: 0x0009423C
	private void SpawnDamageEffects()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.player_burn_effect, base.transform.position, Quaternion.LookRotation(Vector3.up));
		if (this.health.Value > 0)
		{
			gameObject.transform.parent = base.transform;
		}
		AudioSystem.PlayOneShot("Burn01", 0.5f, 0.1f);
	}

	// Token: 0x0600136A RID: 4970 RVA: 0x0000F6BA File Offset: 0x0000D8BA
	private void BeingHitRecieve(object val)
	{
		this.BeingHit = (bool)val;
	}

	// Token: 0x0600136B RID: 4971 RVA: 0x0000F6C8 File Offset: 0x0000D8C8
	private void DamageRecieve(object val)
	{
		this.Health = (byte)val;
	}

	// Token: 0x0600136C RID: 4972 RVA: 0x0000F6D6 File Offset: 0x0000D8D6
	public override void ResetPlayer()
	{
		bool isDead = this.isDead;
		this.stunned = false;
		this.attackAvailable = true;
		if (base.IsOwner)
		{
			this.health.Value = 3;
		}
		this.Activate();
		base.ResetPlayer();
	}

	// Token: 0x0600136D RID: 4973 RVA: 0x000960A0 File Offset: 0x000942A0
	public WarlockPlayerNew1()
	{
		float[] array = new float[3];
		array[0] = 0.5f;
		this.fireballOffsetMin = array;
		float[] array2 = new float[3];
		array2[0] = 3f;
		array2[1] = 1.5f;
		this.fireballOffsetMax = array2;
		this.offsetTimer = new ActionTimer(1.5f, 3f);
		this.tarOffset = Vector3.zero;
		this.m_possibleTargets = new List<WarlockPlayerNew1.PossibleTarget>();
		base..ctor();
	}

	// Token: 0x04001493 RID: 5267
	public GameObject player_death_effect;

	// Token: 0x04001494 RID: 5268
	public GameObject player_burn_effect;

	// Token: 0x04001495 RID: 5269
	public GameObject fire_ball;

	// Token: 0x04001496 RID: 5270
	public GameObject[] fireballProjectileEffect;

	// Token: 0x04001497 RID: 5271
	public GameObject[] handFireballs;

	// Token: 0x04001498 RID: 5272
	public AudioClip sizzleClip;

	// Token: 0x04001499 RID: 5273
	public ParticleSystem flameParticles;

	// Token: 0x0400149A RID: 5274
	private CameraShake cameraShake;

	// Token: 0x0400149B RID: 5275
	private CharacterMover mover;

	// Token: 0x0400149C RID: 5276
	private WarlockControllerNew minigameController;

	// Token: 0x0400149D RID: 5277
	private ActionTimer shootTimer = new ActionTimer(0.7f);

	// Token: 0x0400149E RID: 5278
	private int lastHealth = 3;

	// Token: 0x0400149F RID: 5279
	private const int maxHealth = 3;

	// Token: 0x040014A0 RID: 5280
	private float lavaCounter;

	// Token: 0x040014A1 RID: 5281
	private bool attackAvailable = true;

	// Token: 0x040014A2 RID: 5282
	private bool stunned;

	// Token: 0x040014A3 RID: 5283
	private WarlockPlayerNew1.WarlocksAIState curAIState = WarlockPlayerNew1.WarlocksAIState.Idle;

	// Token: 0x040014A4 RID: 5284
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x040014A5 RID: 5285
	private ActionTimer aiGetTargetTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x040014A6 RID: 5286
	private ActionTimer avoidanceTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x040014A7 RID: 5287
	private Vector2 curAvoidanceVec = Vector3.zero;

	// Token: 0x040014A8 RID: 5288
	private WarlockPlayerNew1 targetPlayer;

	// Token: 0x040014A9 RID: 5289
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x040014AA RID: 5290
	private bool canSeeTarget = true;

	// Token: 0x040014AB RID: 5291
	private float reachDistance = 36f;

	// Token: 0x040014AC RID: 5292
	private float minReachDistance = 12.25f;

	// Token: 0x040014AD RID: 5293
	private float evadeLavaDistance;

	// Token: 0x040014AE RID: 5294
	private WarlockPlayerNew1[] enemies;

	// Token: 0x040014AF RID: 5295
	private Vector3 targetPoint = Vector3.zero;

	// Token: 0x040014B0 RID: 5296
	private bool onLava;

	// Token: 0x040014B1 RID: 5297
	private bool onLavaOrFallingRock;

	// Token: 0x040014B2 RID: 5298
	private TempAudioSource source;

	// Token: 0x040014B3 RID: 5299
	public bool HitLava;

	// Token: 0x040014B4 RID: 5300
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<bool> beingHit = new NetVar<bool>(false);

	// Token: 0x040014B5 RID: 5301
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<byte> health = new NetVar<byte>(0);

	// Token: 0x040014B6 RID: 5302
	private Vector3 lastInput = Vector3.zero;

	// Token: 0x040014B7 RID: 5303
	private float[] fireballOffsetMin;

	// Token: 0x040014B8 RID: 5304
	private float[] fireballOffsetMax;

	// Token: 0x040014B9 RID: 5305
	private ActionTimer offsetTimer;

	// Token: 0x040014BA RID: 5306
	private Vector3 tarOffset;

	// Token: 0x040014BB RID: 5307
	private List<WarlockPlayerNew1.PossibleTarget> m_possibleTargets;

	// Token: 0x02000291 RID: 657
	private enum WarlocksAIState
	{
		// Token: 0x040014BD RID: 5309
		Dodging,
		// Token: 0x040014BE RID: 5310
		EvadingLava,
		// Token: 0x040014BF RID: 5311
		ClosingDistance,
		// Token: 0x040014C0 RID: 5312
		Idle
	}

	// Token: 0x02000292 RID: 658
	private class PossibleTarget
	{
		// Token: 0x0600136E RID: 4974 RVA: 0x0000F70D File Offset: 0x0000D90D
		public PossibleTarget(float sqrDist, WarlockPlayerNew1 player)
		{
			this.sqrDist = sqrDist;
			this.player = player;
		}

		// Token: 0x040014C1 RID: 5313
		public float sqrDist;

		// Token: 0x040014C2 RID: 5314
		public WarlockPlayerNew1 player;
	}
}
