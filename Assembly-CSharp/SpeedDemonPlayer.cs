using System;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200025A RID: 602
public class SpeedDemonPlayer : Movement1
{
	// Token: 0x0600117A RID: 4474 RVA: 0x00087CC8 File Offset: 0x00085EC8
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		CharacterMover characterMover = this.mover;
		characterMover.OnJump = (CharacterMover.OnJumpDelegate)Delegate.Combine(characterMover.OnJump, new CharacterMover.OnJumpDelegate(this.OnJump));
		if (base.IsOwner)
		{
			this.net_z_rotation.Value = ZPMath.CompressFloat(0f, -45f, 45f);
		}
		if (!NetSystem.IsServer)
		{
			this.score.Recieve = new RecieveProxy(base.ScoreRecieve);
		}
		if (!this.player.IsAI || !base.IsOwner)
		{
			base.GetComponent<NavMeshAgent>().enabled = false;
		}
		if (this.player.IsAI && base.IsOwner)
		{
			this.mover.IsAI = true;
		}
		if (this.player.Difficulty == BotDifficulty.Easy)
		{
			this.pathUpdateTimer = new ActionTimer(0.5f, 1f);
			this.hidePositionTimer = new ActionTimer(2f, 8f);
			return;
		}
		if (this.player.Difficulty == BotDifficulty.Normal)
		{
			this.pathUpdateTimer = new ActionTimer(0.3f, 0.75f);
			this.hidePositionTimer = new ActionTimer(1.25f, 6f);
			return;
		}
		if (this.player.Difficulty == BotDifficulty.Hard)
		{
			this.pathUpdateTimer = new ActionTimer(0.1f, 0.15f);
			this.hidePositionTimer = new ActionTimer(0.5f, 1f);
		}
	}

	// Token: 0x0600117B RID: 4475 RVA: 0x0000E617 File Offset: 0x0000C817
	public override void OnOwnerChanged()
	{
		if (NetSystem.IsServer)
		{
			base.GetComponent<NavMeshAgent>().enabled = true;
			this.mover.IsAI = true;
			this.mover.SetForwardVector(Vector3.forward);
		}
		base.OnOwnerChanged();
	}

	// Token: 0x0600117C RID: 4476 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x0600117D RID: 4477 RVA: 0x00087E40 File Offset: 0x00086040
	protected override void Start()
	{
		base.Start();
		this.minigameController = (SpeedDemonController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.playerAnim.RegisterListener(new AnimationEventListener(this.PunchImpactEvent), AnimationEventType.SwordHit);
		this.playerAnim.RegisterListener(new AnimationEventListener(this.SlashEndEvent), AnimationEventType.SlashEnd);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
		if (!this.player.IsAI)
		{
			this.mover.SetForwardVector(Vector3.left);
		}
		this.swordRenderer.sharedMaterial = new Material(this.swordRenderer.sharedMaterial);
		this.swordRenderer.sharedMaterial.color = base.GamePlayer.Color.skinColor1;
		Color skinColor = base.GamePlayer.Color.skinColor1;
		skinColor.a = 0.5f;
		if (this.trail._trailObject == null)
		{
			this.trail._material = new Material(this.trail._material);
			this.trail._material.SetColor("_TintColor", skinColor);
			return;
		}
		Material material = new Material(this.trail._material);
		material.SetColor("_TintColor", skinColor);
		this.trail._trailObject.GetComponent<MeshRenderer>().sharedMaterial = material;
	}

	// Token: 0x0600117E RID: 4478 RVA: 0x0000E64E File Offset: 0x0000C84E
	private void SetSpeed()
	{
		if (this.playerAnim != null && this.playerAnim != null)
		{
			this.playerAnim.Animator.SetFloat("SpeedMultiplier", this.speedMultiplier);
		}
	}

	// Token: 0x0600117F RID: 4479 RVA: 0x0000E687 File Offset: 0x0000C887
	public override void Activate()
	{
		base.Activate();
		this.SetSpeed();
	}

	// Token: 0x06001180 RID: 4480 RVA: 0x0000E695 File Offset: 0x0000C895
	public void OnEnable()
	{
		this.SetSpeed();
	}

	// Token: 0x06001181 RID: 4481 RVA: 0x0000E69D File Offset: 0x0000C89D
	public override void FinishedSpawning()
	{
		base.FinishedSpawning();
		this.SetSpeed();
	}

	// Token: 0x06001182 RID: 4482 RVA: 0x00087FBC File Offset: 0x000861BC
	private void Update()
	{
		base.UpdateController();
		if (!this.gotAchievement && base.IsOwner && !this.player.IsAI && (int)this.Score >= this.lastScore + 5)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_SPEEDY_SABERS");
			this.gotAchievement = true;
		}
		if (base.IsOwner && this.minigameController.Playable && !this.swordCollider.enabled)
		{
			if (base.GamePlayer.IsAI)
			{
				if (this.curAIState != SpeedDemonPlayer.SpeedDemonAiState.Hiding && this.agent.remainingDistance < 0.5f && UnityEngine.Random.value < this.notAttackChance[(int)base.GamePlayer.Difficulty])
				{
					this.StartPunch((double)UnityEngine.Random.value > 0.5);
				}
			}
			else if (!GameManager.IsGamePaused && base.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.UseItemShoot))
			{
				this.StartPunch((double)UnityEngine.Random.value > 0.5);
			}
		}
		if (this.isDead && this.deathTimer.Elapsed(false))
		{
			this.ResetPlayer();
		}
	}

	// Token: 0x06001183 RID: 4483 RVA: 0x000880E8 File Offset: 0x000862E8
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		bool val = !this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || this.isDead;
		if (!base.GamePlayer.IsAI)
		{
			Vector2 axis = new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
			input = new CharacterMoverInput(axis, this.player.RewiredPlayer.GetButtonDown(InputActions.Accept), false);
		}
		else
		{
			input = this.GetAIInput();
		}
		if (this.agent == null || !this.agent.isOnOffMeshLink)
		{
			input.NullInput(val);
			this.mover.CalculateVelocity(input, Time.deltaTime);
			this.mover.DoMovement(Time.deltaTime);
			this.mover.SmoothSlope();
			if (this.mover.MovementAxis != Vector2.zero)
			{
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(this.mover.MovementAxis.x, 0f, this.mover.MovementAxis.y), Vector3.up), 1500f * Time.deltaTime * 6f);
			}
		}
		this.velocity.Value = this.mover.Velocity;
		this.netIsGrounded.Value = (this.curOffMeshLinkTranslationType != OffMeshLinkTranslateType.Parabola && this.mover.Grounded);
		base.DoMovement();
	}

	// Token: 0x06001184 RID: 4484 RVA: 0x0008829C File Offset: 0x0008649C
	protected override void UpdateAnimationState(PlayerAnimation playerAnim)
	{
		Vector2 vector = new Vector2(this.velocity.Value.x, this.velocity.Value.z);
		float num = Mathf.Clamp01(vector.magnitude / this.mover.maxSpeed);
		playerAnim.Velocity = num * this.speedMultiplier;
		playerAnim.VelocityY = this.velocity.Value.y;
		playerAnim.MovementAxis = ((num > 0.01f) ? Vector2.up : Vector2.zero);
		playerAnim.Grounded = this.netIsGrounded.Value;
		playerAnim.SetPlayerRotation(this.rotation.Value);
	}

	// Token: 0x06001185 RID: 4485 RVA: 0x0008834C File Offset: 0x0008654C
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		Vector2 b = new Vector2(base.transform.position.x, base.transform.position.z);
		if (this.agent.isOnOffMeshLink)
		{
			if (this.curOffMeshLinkTranslationType == OffMeshLinkTranslateType.None)
			{
				this.curOffMeshLinkTranslationType = OffMeshLinkTranslateType.Parabola;
				this.OnJump();
				float initialHorizontalVelocity = 5f;
				base.StartCoroutine(base.GetParabolicPath(this.mover, this.mover.gravity, 1500f, initialHorizontalVelocity, true));
			}
		}
		else
		{
			float num = 0.36f;
			if (this.curAIState != SpeedDemonPlayer.SpeedDemonAiState.HasCrown && this.minigameController.PlayersAlive > 2 && this.stateChangeTimer.Elapsed(false))
			{
				this.SetCurAIState((UnityEngine.Random.value > this.difficultyStateChances[(int)base.GamePlayer.Difficulty]) ? SpeedDemonPlayer.SpeedDemonAiState.Chasing : SpeedDemonPlayer.SpeedDemonAiState.Hiding);
			}
			if (this.curAIState == SpeedDemonPlayer.SpeedDemonAiState.Chasing && ((this.targetPlayer != null && this.targetPlayer.IsDead) || this.minigameController.PlayersAlive <= 2))
			{
				this.SetCurAIState(SpeedDemonPlayer.SpeedDemonAiState.Hiding);
			}
			switch (this.curAIState)
			{
			case SpeedDemonPlayer.SpeedDemonAiState.HasCrown:
			case SpeedDemonPlayer.SpeedDemonAiState.Chasing:
			{
				float num2 = float.MaxValue;
				if (this.targetPlayer != null && !this.targetPlayer.IsDead)
				{
					num2 = (this.targetPlayer.transform.position - base.transform.position).magnitude * 0.75f;
				}
				for (int i = 0; i < this.minigameController.players.Count; i++)
				{
					SpeedDemonPlayer speedDemonPlayer = (SpeedDemonPlayer)this.minigameController.players[i];
					if (speedDemonPlayer != this && !speedDemonPlayer.IsDead)
					{
						float magnitude = (speedDemonPlayer.transform.position - base.transform.position).magnitude;
						if (magnitude < num2)
						{
							this.targetPlayer = speedDemonPlayer;
							num2 = magnitude;
						}
					}
				}
				if (this.targetPlayer != null)
				{
					RaycastHit raycastHit;
					if (Physics.Raycast(this.targetPlayer.transform.position, Vector3.down, out raycastHit, 6f, 1024))
					{
						this.targetPosition = raycastHit.point;
					}
				}
				else
				{
					Debug.LogWarning("Target null: " + this.curAIState.ToString() + " -- ID: " + base.GamePlayer.GlobalID.ToString());
				}
				break;
			}
			case SpeedDemonPlayer.SpeedDemonAiState.Hiding:
				this.GetHidePosition();
				break;
			}
			if (this.pathUpdateTimer.Elapsed(true) && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
			{
				this.agent.SetDestination(this.targetPosition);
			}
			if ((new Vector2(this.targetPosition.x, this.targetPosition.z) - b).sqrMagnitude > num)
			{
				Vector3 vector = this.agent.steeringTarget - base.transform.position;
				Vector2 normalized = new Vector2(vector.x, vector.z).normalized;
				result = new CharacterMoverInput(normalized, false, false);
			}
			else
			{
				result.NullInput();
			}
		}
		return result;
	}

	// Token: 0x06001186 RID: 4486 RVA: 0x0008869C File Offset: 0x0008689C
	private void GetHidePosition()
	{
		float num = Vector3.Distance(this.targetPosition, base.transform.position);
		if (this.hidePositionTimer.Elapsed(true) || num < 0.6f)
		{
			this.targetPosition = this.minigameController.GetRandomNavMeshPoint();
		}
	}

	// Token: 0x06001187 RID: 4487 RVA: 0x0003203C File Offset: 0x0003023C
	private float GetPointValue(Vector3 bombPosition, Vector3 target)
	{
		float num = 20f;
		float num2 = 0.75f;
		float num3 = 0.25f;
		float magnitude = (target - bombPosition).magnitude;
		Vector3 normalized = (target - base.transform.position).normalized;
		Vector3 normalized2 = (bombPosition - base.transform.position).normalized;
		float num4 = 1f - (Vector3.Dot(normalized, normalized2) + 1f) / 2f;
		return magnitude / num * num2 + num4 * num3;
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x000886E8 File Offset: 0x000868E8
	private void SetCurAIState(SpeedDemonPlayer.SpeedDemonAiState newState)
	{
		if (!base.GamePlayer.IsLocalPlayer)
		{
			return;
		}
		switch (newState)
		{
		case SpeedDemonPlayer.SpeedDemonAiState.HasCrown:
			this.targetPlayer = null;
			this.targetPosition = Vector3.one * 9000f;
			break;
		case SpeedDemonPlayer.SpeedDemonAiState.Chasing:
			this.targetPlayer = null;
			this.targetPosition = Vector3.one * 9000f;
			break;
		}
		this.stateChangeTimer.Start();
		this.curAIState = newState;
	}

	// Token: 0x06001189 RID: 4489 RVA: 0x00088764 File Offset: 0x00086964
	public override void ResetPlayer()
	{
		this.Activate();
		if (base.IsOwner)
		{
			this.mover.Velocity = Vector3.zero;
			this.velocity.Value = Vector3.zero;
		}
		base.ResetPlayer();
		this.playerAnim.SetPlayerRotationImmediate(this.startRotation.eulerAngles.y);
		if (base.IsOwner)
		{
			base.transform.position = this.minigameController.GetRandomNavMeshPoint() + Vector3.up * 0.875f;
		}
		this.lastScore = (int)this.Score;
	}

	// Token: 0x0600118A RID: 4490 RVA: 0x0000E6AB File Offset: 0x0000C8AB
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCReset(NetPlayer sender)
	{
		this.ResetPlayer();
	}

	// Token: 0x0600118B RID: 4491 RVA: 0x000047EA File Offset: 0x000029EA
	public void StopPlacementAnim()
	{
		this.playerAnim.Animator.SetInteger("Placement", -1);
	}

	// Token: 0x0600118C RID: 4492 RVA: 0x0000E6B3 File Offset: 0x0000C8B3
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x0600118D RID: 4493 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x0600118E RID: 4494 RVA: 0x0000E6BB File Offset: 0x0000C8BB
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender, Vector3 force)
	{
		this.KillPlayer(null, force, true);
	}

	// Token: 0x0600118F RID: 4495 RVA: 0x00088800 File Offset: 0x00086A00
	public void KillPlayer(SpeedDemonPlayer killer, Vector3 force, bool send_rpc = true)
	{
		if (!this.isDead)
		{
			this.deathTimer.Start();
			this.swordCollider.enabled = false;
			this.playerAnim.SpawnRagdoll(force);
			this.isDead = true;
			this.Deactivate();
			AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
			if (Settings.BloodEffects)
			{
				ParticleSystem component = UnityEngine.Object.Instantiate<GameObject>(this.playerDeathEffect, base.MidPoint, Quaternion.identity).GetComponent<ParticleSystem>();
				ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = component.velocityOverLifetime;
				velocityOverLifetime.enabled = true;
				velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
				Vector3 midPoint = base.MidPoint;
				velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, force.x), Mathf.Max(0f, force.x));
				velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, force.y), Mathf.Max(0f, force.y));
				velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(Mathf.Min(0f, force.z), Mathf.Max(0f, force.z));
				ParticleSystem.EmissionModule emission = component.emission;
				ParticleSystem.Burst[] array = new ParticleSystem.Burst[emission.burstCount];
				emission.GetBursts(array);
				array[0].maxCount = (short)((float)array[0].maxCount * 0.5f);
				array[0].minCount = (short)((float)array[0].minCount * 0.5f);
				emission.SetBursts(array);
			}
			this.cameraShake.AddShake(0.3f);
			if (NetSystem.IsServer && send_rpc)
			{
				short score = killer.Score;
				killer.Score = score + 1;
				base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_UNORDERED, new object[]
				{
					force
				});
			}
		}
	}

	// Token: 0x06001190 RID: 4496 RVA: 0x0000E6C6 File Offset: 0x0000C8C6
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void PunchRPC(NetPlayer sender, bool hand)
	{
		this.StartPunch(hand);
	}

	// Token: 0x06001191 RID: 4497 RVA: 0x000889D4 File Offset: 0x00086BD4
	private void StartPunch(bool hand)
	{
		if (!this.minigameController.Playable || base.IsDead || !GameManager.PollInput || GameManager.IsGamePaused)
		{
			return;
		}
		if (base.IsOwner)
		{
			this.lastPunchTime = Time.time;
			base.SendRPC("PunchRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				hand
			});
		}
		this.playerAnim.Animator.SetTrigger("SwordSlash");
		this.swordCollider.enabled = true;
		AudioSystem.PlayOneShot(this.swingSword, 1.25f, 0f, 1f);
	}

	// Token: 0x06001192 RID: 4498 RVA: 0x0000398C File Offset: 0x00001B8C
	private void PunchImpactEvent(PlayerAnimationEvent anim_event)
	{
	}

	// Token: 0x06001193 RID: 4499 RVA: 0x0000E6CF File Offset: 0x0000C8CF
	private void SlashEndEvent(PlayerAnimationEvent anim_event)
	{
		this.swordCollider.enabled = false;
	}

	// Token: 0x06001194 RID: 4500 RVA: 0x00088A70 File Offset: 0x00086C70
	public void OnTriggerEnter(Collider other)
	{
		if (NetSystem.IsServer)
		{
			SpeedDemonPlayer componentInParent = other.GetComponentInParent<SpeedDemonPlayer>();
			if (componentInParent != null && componentInParent != this && !base.IsDead)
			{
				Vector3 force = (base.transform.position - componentInParent.transform.position) * 10f;
				this.KillPlayer(componentInParent, force, true);
			}
		}
	}

	// Token: 0x06001195 RID: 4501 RVA: 0x00088AD4 File Offset: 0x00086CD4
	private void Punch()
	{
		if (!this.minigameController.Playable || base.IsDead || !GameManager.PollInput || GameManager.IsGamePaused)
		{
			return;
		}
		AudioSystem.PlayOneShot(this.punchMiss, 0.75f, 0f, 1f);
		if (NetSystem.IsServer)
		{
			SpeedDemonPlayer speedDemonPlayer = null;
			float num = float.MaxValue;
			for (int i = 0; i < this.minigameController.players.Count; i++)
			{
				SpeedDemonPlayer speedDemonPlayer2 = (SpeedDemonPlayer)this.minigameController.players[i];
				if (!(speedDemonPlayer2 == this) && !speedDemonPlayer2.isDead)
				{
					float num2 = Vector3.Distance(speedDemonPlayer2.transform.position, base.transform.position);
					if (num2 < this.punchHitDistance)
					{
						Vector3 b = base.transform.position - base.transform.forward * 0.75f;
						if (Vector3.Dot(base.transform.forward, (speedDemonPlayer2.transform.position - b).normalized) > 1f - this.punchAngle / 180f && num2 < num)
						{
							num = num2;
							speedDemonPlayer = speedDemonPlayer2;
						}
					}
				}
			}
			if (speedDemonPlayer != null)
			{
				Vector3 force = (speedDemonPlayer.transform.position - base.transform.position) * 10f;
				speedDemonPlayer.KillPlayer(this, force, true);
			}
		}
	}

	// Token: 0x04001229 RID: 4649
	public GameObject playerDeathEffect;

	// Token: 0x0400122A RID: 4650
	public AudioClip punchMiss;

	// Token: 0x0400122B RID: 4651
	public AudioClip punchHit;

	// Token: 0x0400122C RID: 4652
	public AudioClip swingSword;

	// Token: 0x0400122D RID: 4653
	public MeshRenderer swordRenderer;

	// Token: 0x0400122E RID: 4654
	public MeleeWeaponTrail trail;

	// Token: 0x0400122F RID: 4655
	public CapsuleCollider swordCollider;

	// Token: 0x04001230 RID: 4656
	private SpeedDemonController minigameController;

	// Token: 0x04001231 RID: 4657
	private CharacterMover mover;

	// Token: 0x04001232 RID: 4658
	private float punchHitDistance = 2.5f;

	// Token: 0x04001233 RID: 4659
	private float punchAngle = 220f;

	// Token: 0x04001234 RID: 4660
	private float lastPunchTime;

	// Token: 0x04001235 RID: 4661
	private float punchInterval = 0.15f;

	// Token: 0x04001236 RID: 4662
	private CameraShake cameraShake;

	// Token: 0x04001237 RID: 4663
	private ActionTimer deathTimer = new ActionTimer(2f);

	// Token: 0x04001238 RID: 4664
	private SpeedDemonPlayer.SpeedDemonAiState curAIState = SpeedDemonPlayer.SpeedDemonAiState.Hiding;

	// Token: 0x04001239 RID: 4665
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x0400123A RID: 4666
	private ActionTimer hidePositionTimer = new ActionTimer(0.5f, 4f);

	// Token: 0x0400123B RID: 4667
	private ActionTimer stateChangeTimer = new ActionTimer(2.5f, 3.5f);

	// Token: 0x0400123C RID: 4668
	private SpeedDemonPlayer targetPlayer;

	// Token: 0x0400123D RID: 4669
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x0400123E RID: 4670
	private int lastScore;

	// Token: 0x0400123F RID: 4671
	private float[] notAttackChance = new float[]
	{
		0.1f,
		0.3f,
		1.1f
	};

	// Token: 0x04001240 RID: 4672
	private bool gotAchievement;

	// Token: 0x04001241 RID: 4673
	private float speedMultiplier = 5.5f;

	// Token: 0x04001242 RID: 4674
	private float[] difficultyStateChances = new float[]
	{
		0.75f,
		0.5f,
		0.15f
	};

	// Token: 0x0200025B RID: 603
	private enum SpeedDemonAiState
	{
		// Token: 0x04001244 RID: 4676
		HasCrown,
		// Token: 0x04001245 RID: 4677
		Hiding,
		// Token: 0x04001246 RID: 4678
		Chasing
	}
}
