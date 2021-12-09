using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001F5 RID: 501
public class PassTheBombPlayer : Movement1
{
	// Token: 0x06000E98 RID: 3736 RVA: 0x00074588 File Offset: 0x00072788
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
			this.holding_bomb.Recieve = new RecieveProxy(this.HoldingBombRecieve);
			this.stunned.Recieve = new RecieveProxy(this.StunnedRecieve);
			this.score.Recieve = new RecieveProxy(base.ScoreRecieve);
			this.HoldingBomb = this.holding_bomb.Value;
		}
		if (!base.IsOwner)
		{
			base.GetComponent<CharacterController>().enabled = false;
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

	// Token: 0x06000E99 RID: 3737 RVA: 0x000746A8 File Offset: 0x000728A8
	public override void OnOwnerChanged()
	{
		if (NetSystem.IsServer)
		{
			base.GetComponent<CharacterController>().enabled = true;
			base.GetComponent<NavMeshAgent>().enabled = true;
			this.mover.IsAI = true;
			this.mover.SetForwardVector(Vector3.forward);
		}
		base.OnOwnerChanged();
	}

	// Token: 0x06000E9A RID: 3738 RVA: 0x0000CD32 File Offset: 0x0000AF32
	public void Awake()
	{
		base.InitializeController();
		if (this.bomb != null)
		{
			this.bombLCD = this.bomb.GetComponentInChildren<MeshRenderer>().materials[1];
		}
	}

	// Token: 0x06000E9B RID: 3739 RVA: 0x000746F8 File Offset: 0x000728F8
	protected override void Start()
	{
		base.Start();
		this.minigameController = (PassTheBombController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.playerAnim.RegisterListener(new AnimationEventListener(this.PunchImpactEvent), AnimationEventType.PunchImpact);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
		if (!this.player.IsAI)
		{
			this.mover.SetForwardVector(Vector3.left);
		}
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x00074788 File Offset: 0x00072988
	private void Update()
	{
		base.UpdateController();
		if (!this.gotAchievement && base.IsOwner && !this.player.IsAI && this.achivementStunCount.Value > 10)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_EXPLOSIVE_EXCHANGE");
			this.gotAchievement = true;
		}
		if (base.IsOwner && this.minigameController.Playable && Time.time - this.lastPunchTime > this.punchInterval)
		{
			if (base.GamePlayer.IsAI)
			{
				if (this.curAIState != PassTheBombPlayer.PassTheBombAIState.Hiding && this.agent.remainingDistance < 0.5f)
				{
					this.StartPunch((double)UnityEngine.Random.value > 0.5);
				}
			}
			else if (!GameManager.IsGamePaused && base.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.UseItemShoot))
			{
				this.StartPunch((double)UnityEngine.Random.value > 0.5);
			}
		}
		if (NetSystem.IsServer && this.Stunned && Time.time - this.stunStartTime > this.stunLength)
		{
			this.Stunned = false;
		}
		this.playerAnim.Stunned = this.stunned.Value;
		this.stun_fx_obj.SetActive(this.stunned.Value);
		if (this.bomb != null && this.bombLCD != null && this.HoldingBomb)
		{
			this.bombLCD.SetInt("_AtlasIndex", this.minigameController.GetBombTimer());
		}
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x00074918 File Offset: 0x00072B18
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		bool val = !this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || this.Stunned || this.isDead;
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
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(this.mover.MovementAxis.x, 0f, this.mover.MovementAxis.y), Vector3.up), 1500f * Time.deltaTime);
			}
		}
		this.velocity.Value = this.mover.Velocity;
		this.netIsGrounded.Value = (this.curOffMeshLinkTranslationType != OffMeshLinkTranslateType.Parabola && this.mover.Grounded);
		base.DoMovement();
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x00074ACC File Offset: 0x00072CCC
	protected override void UpdateAnimationState(PlayerAnimation playerAnim)
	{
		Vector2 vector = new Vector2(this.velocity.Value.x, this.velocity.Value.z);
		float num = Mathf.Clamp01(vector.magnitude / this.mover.maxSpeed);
		playerAnim.Velocity = num;
		playerAnim.VelocityY = this.velocity.Value.y;
		playerAnim.MovementAxis = ((num > 0.01f) ? Vector2.up : Vector2.zero);
		playerAnim.Grounded = this.netIsGrounded.Value;
		playerAnim.SetPlayerRotation(this.rotation.Value);
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x00074B74 File Offset: 0x00072D74
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
			if (this.curAIState != PassTheBombPlayer.PassTheBombAIState.HasBomb && this.minigameController.PlayersAlive > 2 && this.stateChangeTimer.Elapsed(false))
			{
				this.SetCurAIState((UnityEngine.Random.value < this.chanceToChase[(int)base.GamePlayer.Difficulty]) ? PassTheBombPlayer.PassTheBombAIState.Chasing : PassTheBombPlayer.PassTheBombAIState.Hiding);
			}
			if (this.curAIState == PassTheBombPlayer.PassTheBombAIState.Chasing && ((this.targetPlayer != null && (this.targetPlayer.HoldingBomb || this.targetPlayer.IsDead || this.targetPlayer.Stunned)) || this.minigameController.PlayersAlive <= 2))
			{
				this.SetCurAIState(PassTheBombPlayer.PassTheBombAIState.Hiding);
			}
			switch (this.curAIState)
			{
			case PassTheBombPlayer.PassTheBombAIState.HasBomb:
			case PassTheBombPlayer.PassTheBombAIState.Chasing:
			{
				bool flag = false;
				if (this.followTimer.Elapsed(true))
				{
					this.followClosest = (GameManager.rand.NextDouble() < (double)this.followClosestChance[(int)base.GamePlayer.Difficulty]);
					flag = true;
				}
				float num2 = float.MaxValue;
				if (this.targetPlayer != null && !this.targetPlayer.IsDead)
				{
					num2 = (this.targetPlayer.transform.position - base.transform.position).magnitude * 0.75f;
				}
				this.targets.Clear();
				for (int i = 0; i < this.minigameController.GetPlayerCount(); i++)
				{
					PassTheBombPlayer passTheBombPlayer = (PassTheBombPlayer)this.minigameController.GetPlayer(i);
					if (passTheBombPlayer != this && !passTheBombPlayer.IsDead && !passTheBombPlayer.HoldingBomb)
					{
						if (this.followClosest)
						{
							float magnitude = (passTheBombPlayer.transform.position - base.transform.position).magnitude;
							if (magnitude < num2)
							{
								this.targetPlayer = passTheBombPlayer;
								num2 = magnitude;
							}
						}
						else
						{
							this.targets.Add(passTheBombPlayer);
						}
					}
				}
				if (this.targets.Count > 0 && flag)
				{
					this.targetPlayer = this.targets[GameManager.rand.Next(0, this.targets.Count)];
				}
				if (this.targetPlayer != null)
				{
					this.targetPosition = this.targetPlayer.transform.position;
				}
				else
				{
					Debug.LogWarning("Target null: " + this.curAIState.ToString() + " -- ID: " + base.GamePlayer.GlobalID.ToString());
				}
				break;
			}
			case PassTheBombPlayer.PassTheBombAIState.Hiding:
				this.GetHidePosition();
				break;
			}
			if (this.pathUpdateTimer.Elapsed(true) && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
			{
				NavMeshHit navMeshHit;
				if (NavMesh.SamplePosition(this.targetPosition, out navMeshHit, 3f, -1))
				{
					this.agent.SetDestination(navMeshHit.position);
				}
				else
				{
					this.agent.SetDestination(this.targetPosition);
				}
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

	// Token: 0x06000EA0 RID: 3744 RVA: 0x00074F84 File Offset: 0x00073184
	private void GetHidePosition()
	{
		float num = Vector3.Distance(this.targetPosition, base.transform.position);
		if (this.hidePositionTimer.Elapsed(true) || num < 0.6f)
		{
			int num2 = this.pointsToCheck[(int)base.GamePlayer.Difficulty];
			Vector3 position = this.minigameController.GetBombPlayer().transform.position;
			Vector3 lhs = this.targetPosition;
			float num3 = this.GetPointValue(position, this.targetPosition) * 0.8f;
			for (int i = 0; i < num2; i++)
			{
				Vector3 randomNavMeshPoint = this.minigameController.GetRandomNavMeshPoint();
				float pointValue = this.GetPointValue(position, randomNavMeshPoint);
				if (pointValue > num3)
				{
					lhs = randomNavMeshPoint;
					num3 = pointValue;
				}
			}
			if (lhs == this.targetPosition && num < 0.6f)
			{
				this.SetCurAIState(PassTheBombPlayer.PassTheBombAIState.Chasing);
				return;
			}
			this.targetPosition = lhs;
		}
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x0003203C File Offset: 0x0003023C
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

	// Token: 0x06000EA2 RID: 3746 RVA: 0x00075060 File Offset: 0x00073260
	private void SetCurAIState(PassTheBombPlayer.PassTheBombAIState newState)
	{
		switch (newState)
		{
		case PassTheBombPlayer.PassTheBombAIState.HasBomb:
			this.targetPlayer = null;
			this.targetPosition = Vector3.one * 9000f;
			break;
		case PassTheBombPlayer.PassTheBombAIState.Hiding:
			if (this.minigameController != null)
			{
				PassTheBombPlayer bombPlayer = this.minigameController.GetBombPlayer();
				if (bombPlayer != null)
				{
					this.targetPosition = bombPlayer.transform.position;
				}
			}
			break;
		case PassTheBombPlayer.PassTheBombAIState.Chasing:
			this.targetPlayer = null;
			this.targetPosition = Vector3.one * 9000f;
			break;
		}
		this.stateChangeTimer.Start();
		this.curAIState = newState;
	}

	// Token: 0x06000EA3 RID: 3747 RVA: 0x00075104 File Offset: 0x00073304
	public override void ResetPlayer()
	{
		if (!this.isDead)
		{
			this.KillPlayer(false);
		}
		this.playerAnim.SetPlayerRotationImmediate(this.startRotation.eulerAngles.y);
		this.HoldingBomb = false;
		this.Stunned = false;
		if (base.IsOwner)
		{
			this.mover.Velocity = Vector3.zero;
			this.velocity.Value = Vector3.zero;
		}
		base.ResetPlayer();
	}

	// Token: 0x06000EA4 RID: 3748 RVA: 0x0000CD60 File Offset: 0x0000AF60
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x06000EA5 RID: 3749 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06000EA6 RID: 3750 RVA: 0x0000CD68 File Offset: 0x0000AF68
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender)
	{
		this.KillPlayer(true);
	}

	// Token: 0x06000EA7 RID: 3751 RVA: 0x00075178 File Offset: 0x00073378
	public void KillPlayer(bool send_rpc = true)
	{
		if (!this.isDead)
		{
			Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
			onUnitSphere.y = 100f;
			onUnitSphere.x *= 100f;
			onUnitSphere.z *= 40f;
			this.playerAnim.SpawnRagdoll(onUnitSphere * 0.05f);
			this.isDead = true;
			this.Deactivate();
			if (!this.HoldingBomb)
			{
				AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
			}
			else
			{
				this.HoldingBomb = false;
				UnityEngine.Object.Instantiate<GameObject>(this.nuke_explosion_fx, base.transform.position, Quaternion.LookRotation(Vector3.up));
				AudioSystem.PlayOneShot(this.nuke_explosion_sound, 1f, 0.1f, 1f);
			}
			if (Settings.BloodEffects)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.player_death_effect, base.transform.position, Quaternion.LookRotation(Vector3.up));
			}
			this.cameraShake.AddShake(0.5f);
			this.minigameController.PlayerDied(this);
			if (NetSystem.IsServer && send_rpc)
			{
				base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
			}
		}
	}

	// Token: 0x06000EA8 RID: 3752 RVA: 0x0000CD71 File Offset: 0x0000AF71
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void PunchRPC(NetPlayer sender, bool hand)
	{
		this.StartPunch(hand);
	}

	// Token: 0x06000EA9 RID: 3753 RVA: 0x000752A4 File Offset: 0x000734A4
	private void StartPunch(bool hand)
	{
		if (!this.minigameController.Playable || this.Stunned || base.IsDead || !GameManager.PollInput || GameManager.IsGamePaused)
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
		this.playerAnim.FirePunchTrigger(hand);
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x0000CD7A File Offset: 0x0000AF7A
	private void PunchImpactEvent(PlayerAnimationEvent anim_event)
	{
		this.Punch();
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x00075318 File Offset: 0x00073518
	private void Punch()
	{
		if (!this.minigameController.Playable || this.Stunned || base.IsDead || !GameManager.PollInput || GameManager.IsGamePaused)
		{
			return;
		}
		AudioSystem.PlayOneShot(this.punch_miss, 0.75f, 0f, 1f);
		if (NetSystem.IsServer)
		{
			PassTheBombPlayer passTheBombPlayer = null;
			float num = float.MaxValue;
			for (int i = 0; i < this.minigameController.GetPlayerCount(); i++)
			{
				PassTheBombPlayer passTheBombPlayer2 = (PassTheBombPlayer)this.minigameController.GetPlayer(i);
				if (!(passTheBombPlayer2 == this) && !passTheBombPlayer2.isDead && !passTheBombPlayer2.HoldingBomb && (this.HoldingBomb || (!passTheBombPlayer2.Stunned && Time.time - this.stunStartTime > this.stunImmunityLength)))
				{
					Vector3 vector = passTheBombPlayer2.transform.position - base.transform.position;
					float num2 = Mathf.Abs(vector.y);
					vector.y = 0f;
					float magnitude = vector.magnitude;
					if (magnitude < this.punchHitDistance && num2 < 0.9f)
					{
						Vector3 b = base.transform.position - base.transform.forward * 0.75f;
						if (Vector3.Dot(base.transform.forward, (passTheBombPlayer2.transform.position - b).normalized) > 1f - this.punchAngle / 180f && magnitude < num)
						{
							num = magnitude;
							passTheBombPlayer = passTheBombPlayer2;
						}
					}
				}
			}
			if (passTheBombPlayer != null)
			{
				if (this.HoldingBomb)
				{
					passTheBombPlayer.HoldingBomb = true;
					this.HoldingBomb = false;
				}
				else
				{
					NetVar<byte> netVar = this.achivementStunCount;
					byte value = netVar.Value;
					netVar.Value = value + 1;
				}
				passTheBombPlayer.Stunned = true;
			}
		}
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x0000CD82 File Offset: 0x0000AF82
	public void StunnedRecieve(object val)
	{
		this.Stunned = (bool)val;
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000EAD RID: 3757 RVA: 0x0000CD90 File Offset: 0x0000AF90
	// (set) Token: 0x06000EAE RID: 3758 RVA: 0x000754FC File Offset: 0x000736FC
	public bool Stunned
	{
		get
		{
			return this.stunned.Value;
		}
		set
		{
			this.stunned.Value = value;
			if (value)
			{
				this.stunStartTime = Time.time;
				this.mover.Velocity = Vector3.zero;
				this.velocity.Value = Vector3.zero;
				AudioSystem.PlayOneShot(this.punch_hit, 1f, 0f, 1f);
			}
		}
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x0000CD9D File Offset: 0x0000AF9D
	public void HoldingBombRecieve(object val)
	{
		this.HoldingBomb = (bool)val;
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x0000CDAB File Offset: 0x0000AFAB
	// (set) Token: 0x06000EB1 RID: 3761 RVA: 0x00075560 File Offset: 0x00073760
	public bool HoldingBomb
	{
		get
		{
			return this.holding_bomb.Value;
		}
		set
		{
			this.holding_bomb.Value = value;
			this.bomb.SetActive(value);
			this.playerAnim.Carrying = value;
			this.SetCurAIState(value ? PassTheBombPlayer.PassTheBombAIState.HasBomb : PassTheBombPlayer.PassTheBombAIState.Hiding);
			this.mover.maxSpeed = (value ? this.bomb_speed : this.base_speed);
			this.targetPlayer = null;
		}
	}

	// Token: 0x04000E22 RID: 3618
	public GameObject nuke_explosion_fx;

	// Token: 0x04000E23 RID: 3619
	public GameObject player_death_effect;

	// Token: 0x04000E24 RID: 3620
	public GameObject bomb;

	// Token: 0x04000E25 RID: 3621
	public AudioClip nuke_explosion_sound;

	// Token: 0x04000E26 RID: 3622
	public AudioClip punch_miss;

	// Token: 0x04000E27 RID: 3623
	public AudioClip punch_hit;

	// Token: 0x04000E28 RID: 3624
	public GameObject stun_fx_obj;

	// Token: 0x04000E29 RID: 3625
	public float base_speed = 6f;

	// Token: 0x04000E2A RID: 3626
	public float bomb_speed = 6.75f;

	// Token: 0x04000E2B RID: 3627
	private PassTheBombController minigameController;

	// Token: 0x04000E2C RID: 3628
	private CharacterMover mover;

	// Token: 0x04000E2D RID: 3629
	private float stunStartTime;

	// Token: 0x04000E2E RID: 3630
	private float stunLength = 2.5f;

	// Token: 0x04000E2F RID: 3631
	private float stunImmunityLength = 3.5f;

	// Token: 0x04000E30 RID: 3632
	private float punchHitDistance = 3f;

	// Token: 0x04000E31 RID: 3633
	private float punchAngle = 120f;

	// Token: 0x04000E32 RID: 3634
	private float lastPunchTime;

	// Token: 0x04000E33 RID: 3635
	private float punchInterval = 0.75f;

	// Token: 0x04000E34 RID: 3636
	private CameraShake cameraShake;

	// Token: 0x04000E35 RID: 3637
	private Material bombLCD;

	// Token: 0x04000E36 RID: 3638
	private PassTheBombPlayer.PassTheBombAIState curAIState = PassTheBombPlayer.PassTheBombAIState.Hiding;

	// Token: 0x04000E37 RID: 3639
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04000E38 RID: 3640
	private ActionTimer hidePositionTimer = new ActionTimer(0.25f, 0.55f);

	// Token: 0x04000E39 RID: 3641
	private ActionTimer stateChangeTimer = new ActionTimer(2.5f, 3.5f);

	// Token: 0x04000E3A RID: 3642
	private PassTheBombPlayer targetPlayer;

	// Token: 0x04000E3B RID: 3643
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x04000E3C RID: 3644
	private bool gotAchievement;

	// Token: 0x04000E3D RID: 3645
	private float[] followClosestChance = new float[]
	{
		0.35f,
		0.6f,
		1.1f
	};

	// Token: 0x04000E3E RID: 3646
	private float[] chanceToChase = new float[]
	{
		0.075f,
		0.15f,
		0.25f
	};

	// Token: 0x04000E3F RID: 3647
	private ActionTimer followTimer = new ActionTimer(1f, 2f);

	// Token: 0x04000E40 RID: 3648
	private bool followClosest;

	// Token: 0x04000E41 RID: 3649
	private List<PassTheBombPlayer> targets = new List<PassTheBombPlayer>();

	// Token: 0x04000E42 RID: 3650
	private int[] pointsToCheck = new int[]
	{
		3,
		6,
		18
	};

	// Token: 0x04000E43 RID: 3651
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<byte> achivementStunCount = new NetVar<byte>(0);

	// Token: 0x04000E44 RID: 3652
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<bool> stunned = new NetVar<bool>(false);

	// Token: 0x04000E45 RID: 3653
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<bool> holding_bomb = new NetVar<bool>(false);

	// Token: 0x020001F6 RID: 502
	private enum PassTheBombAIState
	{
		// Token: 0x04000E47 RID: 3655
		HasBomb,
		// Token: 0x04000E48 RID: 3656
		Hiding,
		// Token: 0x04000E49 RID: 3657
		Chasing
	}
}
