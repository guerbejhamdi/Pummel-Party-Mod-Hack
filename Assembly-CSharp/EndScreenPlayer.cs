using System;
using System.Collections;
using Rewired;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000054 RID: 84
public class EndScreenPlayer : Movement1
{
	// Token: 0x0600016A RID: 362 RVA: 0x000316F4 File Offset: 0x0002F8F4
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
			this.holdingCrown.Recieve = new RecieveProxy(this.HoldingCrownRecieve);
			this.score.Recieve = new RecieveProxy(base.ScoreRecieve);
			this.HoldingCrown = this.holdingCrown.Value;
		}
		if (this.player.IsAI)
		{
			base.GetComponent<CharacterController>().enabled = false;
			this.mover.IsAI = true;
		}
	}

	// Token: 0x0600016B RID: 363 RVA: 0x000317CC File Offset: 0x0002F9CC
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

	// Token: 0x0600016C RID: 364 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0003181C File Offset: 0x0002FA1C
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.SetLoaded());
		this.Activate();
		this.playerAnim.RegisterListener(new AnimationEventListener(this.PunchImpactEvent), AnimationEventType.PunchImpact);
		this.playerAnim.Animator.SetInteger("Placement", (int)GameManager.GetPlayerAt((int)base.OwnerSlot).Placement);
		if (!this.player.IsAI)
		{
			this.mover.SetForwardVector(Vector3.forward);
		}
	}

	// Token: 0x0600016E RID: 366 RVA: 0x000047DB File Offset: 0x000029DB
	private IEnumerator SetLoaded()
	{
		while (!(GameManager.Board.endScreenManager != null) || !(GameManager.Board.endScreenManager.Root != null))
		{
			yield return null;
		}
		this.endScreenManager = GameManager.Board.endScreenManager;
		CameraFollow componentInChildren = this.endScreenManager.Root.GetComponentInChildren<CameraFollow>();
		if (componentInChildren != null)
		{
			componentInChildren.AddTarget(this);
		}
		this.endScreenManager.players.Add(this);
		this.cameraShake = this.endScreenManager.Root.GetComponentInChildren<CameraShake>();
		this.loaded = true;
		yield break;
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0003189C File Offset: 0x0002FA9C
	private void Update()
	{
		base.UpdateController();
		if (base.IsOwner && this.endScreenManager != null && this.endScreenManager.Playable && Time.time - this.lastPunchTime > this.punchInterval)
		{
			if (base.GamePlayer.IsAI)
			{
				if (this.curAIState != EndScreenPlayer.EndScreenAIState.Hiding && this.agent.remainingDistance < 0.5f)
				{
					this.StartPunch((double)UnityEngine.Random.value > 0.5);
				}
			}
			else if (!GameManager.IsGamePaused)
			{
				Player rewiredPlayer = base.GamePlayer.RewiredPlayer;
				if (rewiredPlayer.GetButtonDown(InputActions.OpenStatistics) || (rewiredPlayer.controllers.GetLastActiveController().type == ControllerType.Joystick && rewiredPlayer.GetButtonDown(InputActions.UITabLeft)))
				{
					if (this.m_statWindow == null)
					{
						this.m_statWindow = UnityEngine.Object.FindObjectOfType<StatUIWindow>();
					}
					if (this.m_statWindow != null)
					{
						if (!this.m_viewingStatWindow)
						{
							this.m_statWindow.ShowCount = this.m_statWindow.ShowCount + 1;
							this.m_viewingStatWindow = true;
						}
						else
						{
							this.m_statWindow.ShowCount = this.m_statWindow.ShowCount - 1;
							this.m_viewingStatWindow = false;
						}
					}
				}
				if (base.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.UseItemShoot))
				{
					this.StartPunch((double)UnityEngine.Random.value > 0.5);
				}
			}
		}
		if (this.isDead && this.deathTimer.Elapsed(false))
		{
			this.ResetPlayer();
		}
	}

	// Token: 0x06000170 RID: 368 RVA: 0x00031A38 File Offset: 0x0002FC38
	protected override void DoMovement()
	{
		if (this.endScreenManager == null)
		{
			return;
		}
		CharacterMoverInput input = default(CharacterMoverInput);
		bool val = !this.endScreenManager.Playable || GameManager.IsGamePaused || !GameManager.PollInput || this.isDead;
		if (!base.GamePlayer.IsAI && !this.m_viewingStatWindow)
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

	// Token: 0x06000171 RID: 369 RVA: 0x00031BF0 File Offset: 0x0002FDF0
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

	// Token: 0x06000172 RID: 370 RVA: 0x00031C98 File Offset: 0x0002FE98
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
			if (this.curAIState != EndScreenPlayer.EndScreenAIState.HasCrown && this.endScreenManager.PlayersAlive > 2 && this.stateChangeTimer.Elapsed(false))
			{
				this.SetCurAIState((UnityEngine.Random.value > 0.25f) ? EndScreenPlayer.EndScreenAIState.Chasing : EndScreenPlayer.EndScreenAIState.Hiding);
			}
			if (this.curAIState == EndScreenPlayer.EndScreenAIState.Chasing && ((this.targetPlayer != null && (this.targetPlayer.HoldingCrown || this.targetPlayer.IsDead)) || this.endScreenManager.PlayersAlive <= 2))
			{
				this.SetCurAIState(EndScreenPlayer.EndScreenAIState.Hiding);
			}
			switch (this.curAIState)
			{
			case EndScreenPlayer.EndScreenAIState.HasCrown:
			case EndScreenPlayer.EndScreenAIState.Chasing:
			{
				float num2 = float.MaxValue;
				if (this.targetPlayer != null && !this.targetPlayer.IsDead)
				{
					num2 = (this.targetPlayer.transform.position - base.transform.position).magnitude * 0.75f;
				}
				for (int i = 0; i < this.endScreenManager.players.Count; i++)
				{
					EndScreenPlayer endScreenPlayer = this.endScreenManager.players[i];
					if (endScreenPlayer != this && !endScreenPlayer.IsDead && !endScreenPlayer.HoldingCrown)
					{
						float magnitude = (endScreenPlayer.transform.position - base.transform.position).magnitude;
						if (magnitude < num2)
						{
							this.targetPlayer = endScreenPlayer;
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
			case EndScreenPlayer.EndScreenAIState.Hiding:
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

	// Token: 0x06000173 RID: 371 RVA: 0x00031FF0 File Offset: 0x000301F0
	private void GetHidePosition()
	{
		float num = Vector3.Distance(this.targetPosition, base.transform.position);
		if (this.hidePositionTimer.Elapsed(true) || num < 0.6f)
		{
			this.targetPosition = this.endScreenManager.GetRandomNavMeshPoint();
		}
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0003203C File Offset: 0x0003023C
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

	// Token: 0x06000175 RID: 373 RVA: 0x000320C8 File Offset: 0x000302C8
	private void SetCurAIState(EndScreenPlayer.EndScreenAIState newState)
	{
		if (!base.GamePlayer.IsLocalPlayer)
		{
			return;
		}
		switch (newState)
		{
		case EndScreenPlayer.EndScreenAIState.HasCrown:
			this.targetPlayer = null;
			this.targetPosition = Vector3.one * 9000f;
			break;
		case EndScreenPlayer.EndScreenAIState.Hiding:
			this.endScreenManager != null;
			break;
		case EndScreenPlayer.EndScreenAIState.Chasing:
			this.targetPlayer = null;
			this.targetPosition = Vector3.one * 9000f;
			break;
		}
		this.stateChangeTimer.Start();
		this.curAIState = newState;
	}

	// Token: 0x06000176 RID: 374 RVA: 0x00032154 File Offset: 0x00030354
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
	}

	// Token: 0x06000177 RID: 375 RVA: 0x000047EA File Offset: 0x000029EA
	public void StopPlacementAnim()
	{
		this.playerAnim.Animator.SetInteger("Placement", -1);
	}

	// Token: 0x06000178 RID: 376 RVA: 0x00004802 File Offset: 0x00002A02
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x0600017A RID: 378 RVA: 0x00004830 File Offset: 0x00002A30
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender, Vector3 force)
	{
		this.KillPlayer(force, true);
	}

	// Token: 0x0600017B RID: 379 RVA: 0x000321B0 File Offset: 0x000303B0
	public void KillPlayer(Vector3 force, bool send_rpc = true)
	{
		if (!this.loaded)
		{
			return;
		}
		if (!this.isDead)
		{
			this.deathTimer.Start();
			this.playerAnim.SpawnRagdoll(force * 0.05f);
			this.isDead = true;
			this.Deactivate();
			if (!this.HoldingCrown)
			{
				AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
			}
			else
			{
				this.HoldingCrown = false;
			}
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
				base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_UNORDERED, new object[]
				{
					force
				});
			}
		}
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0000483A File Offset: 0x00002A3A
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void PunchRPC(NetPlayer sender, bool hand)
	{
		this.StartPunch(hand);
	}

	// Token: 0x0600017D RID: 381 RVA: 0x00032388 File Offset: 0x00030588
	private void StartPunch(bool hand)
	{
		if (!this.endScreenManager.Playable || base.IsDead || !GameManager.PollInput || GameManager.IsGamePaused)
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

	// Token: 0x0600017E RID: 382 RVA: 0x00004843 File Offset: 0x00002A43
	private void PunchImpactEvent(PlayerAnimationEvent anim_event)
	{
		this.Punch();
	}

	// Token: 0x0600017F RID: 383 RVA: 0x000323F4 File Offset: 0x000305F4
	private void Punch()
	{
		if (!this.endScreenManager.Playable || base.IsDead || !GameManager.PollInput || GameManager.IsGamePaused || !this.loaded)
		{
			return;
		}
		AudioSystem.PlayOneShot(this.punchMiss, 0.75f, 0f, 1f);
		if (NetSystem.IsServer)
		{
			EndScreenPlayer endScreenPlayer = null;
			float num = float.MaxValue;
			for (int i = 0; i < this.endScreenManager.players.Count; i++)
			{
				EndScreenPlayer endScreenPlayer2 = this.endScreenManager.players[i];
				if (!(endScreenPlayer2 == this) && !endScreenPlayer2.isDead)
				{
					float num2 = Vector3.Distance(endScreenPlayer2.transform.position, base.transform.position);
					if (num2 < this.punchHitDistance)
					{
						Vector3 b = base.transform.position - base.transform.forward * 0.75f;
						if (Vector3.Dot(base.transform.forward, (endScreenPlayer2.transform.position - b).normalized) > 1f - this.punchAngle / 180f && num2 < num)
						{
							num = num2;
							endScreenPlayer = endScreenPlayer2;
						}
					}
				}
			}
			if (endScreenPlayer != null)
			{
				if (this.HoldingCrown)
				{
					endScreenPlayer.HoldingCrown = true;
					this.HoldingCrown = false;
				}
				Vector3 force = (endScreenPlayer.transform.position - base.transform.position) * 30f;
				endScreenPlayer.KillPlayer(force, true);
			}
		}
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000484B File Offset: 0x00002A4B
	public void HoldingCrownRecieve(object val)
	{
		this.HoldingCrown = (bool)val;
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000181 RID: 385 RVA: 0x00004859 File Offset: 0x00002A59
	// (set) Token: 0x06000182 RID: 386 RVA: 0x00004866 File Offset: 0x00002A66
	public bool HoldingCrown
	{
		get
		{
			return this.holdingCrown.Value;
		}
		set
		{
			this.holdingCrown.Value = value;
			this.crown.SetActive(value);
			this.playerAnim.Carrying = value;
			this.SetCurAIState(value ? EndScreenPlayer.EndScreenAIState.HasCrown : EndScreenPlayer.EndScreenAIState.Hiding);
			this.targetPlayer = null;
		}
	}

	// Token: 0x040001C4 RID: 452
	private EndScreenManager endScreenManager;

	// Token: 0x040001C5 RID: 453
	public GameObject playerDeathEffect;

	// Token: 0x040001C6 RID: 454
	public AudioClip punchMiss;

	// Token: 0x040001C7 RID: 455
	public AudioClip punchHit;

	// Token: 0x040001C8 RID: 456
	public GameObject crown;

	// Token: 0x040001C9 RID: 457
	private PassTheBombController minigameController;

	// Token: 0x040001CA RID: 458
	private CharacterMover mover;

	// Token: 0x040001CB RID: 459
	private float punchHitDistance = 1.5f;

	// Token: 0x040001CC RID: 460
	private float punchAngle = 120f;

	// Token: 0x040001CD RID: 461
	private float lastPunchTime;

	// Token: 0x040001CE RID: 462
	private float punchInterval = 0.75f;

	// Token: 0x040001CF RID: 463
	private CameraShake cameraShake;

	// Token: 0x040001D0 RID: 464
	private ActionTimer deathTimer = new ActionTimer(4f);

	// Token: 0x040001D1 RID: 465
	private EndScreenPlayer.EndScreenAIState curAIState = EndScreenPlayer.EndScreenAIState.Hiding;

	// Token: 0x040001D2 RID: 466
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x040001D3 RID: 467
	private ActionTimer hidePositionTimer = new ActionTimer(0.5f, 4f);

	// Token: 0x040001D4 RID: 468
	private ActionTimer stateChangeTimer = new ActionTimer(2.5f, 3.5f);

	// Token: 0x040001D5 RID: 469
	private EndScreenPlayer targetPlayer;

	// Token: 0x040001D6 RID: 470
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x040001D7 RID: 471
	private bool loaded;

	// Token: 0x040001D8 RID: 472
	private StatUIWindow m_statWindow;

	// Token: 0x040001D9 RID: 473
	private bool m_viewingStatWindow;

	// Token: 0x040001DA RID: 474
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<bool> holdingCrown = new NetVar<bool>(false);

	// Token: 0x02000055 RID: 85
	private enum EndScreenAIState
	{
		// Token: 0x040001DC RID: 476
		HasCrown,
		// Token: 0x040001DD RID: 477
		Hiding,
		// Token: 0x040001DE RID: 478
		Chasing
	}
}
