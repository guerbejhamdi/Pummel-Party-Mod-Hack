using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000155 RID: 341
public class BombKingPlayer : Movement1
{
	// Token: 0x060009CD RID: 2509 RVA: 0x00057688 File Offset: 0x00055888
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
			this.holdingCrown.Recieve = new RecieveProxy(this.HoldingBombRecieve);
			this.stunned.Recieve = new RecieveProxy(this.StunnedRecieve);
			this.score.Recieve = new RecieveProxy(base.ScoreRecieve);
			this.HoldingCrown = this.holdingCrown.Value;
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

	// Token: 0x060009CE RID: 2510 RVA: 0x000577A8 File Offset: 0x000559A8
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

	// Token: 0x060009CF RID: 2511 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x000577F8 File Offset: 0x000559F8
	protected override void Start()
	{
		base.Start();
		this.minigameController = (BombKingController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.playerAnim.RegisterListener(new AnimationEventListener(this.PunchImpactEvent), AnimationEventType.PunchImpact);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
		if (!this.player.IsAI)
		{
			this.mover.SetForwardVector(Vector3.left);
		}
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x00057888 File Offset: 0x00055A88
	private void Update()
	{
		base.UpdateController();
		if (base.IsOwner && this.minigameController.Playable && Time.time - this.lastPunchTime > this.punchInterval)
		{
			if (base.GamePlayer.IsAI)
			{
				if (this.curAIState != BombKingPlayer.BombKingAIState.HasCrown && this.agent.remainingDistance < 0.5f)
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
		if (NetSystem.IsServer && this.HoldingCrown && this.scoreUpdateTimer.Elapsed(true) && this.minigameController.Playable)
		{
			short score = this.Score;
			this.Score = score + 1;
		}
		this.playerAnim.Stunned = this.stunned.Value;
		this.stun_fx_obj.SetActive(this.stunned.Value);
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x000579D0 File Offset: 0x00055BD0
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

	// Token: 0x060009D3 RID: 2515 RVA: 0x00057B84 File Offset: 0x00055D84
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

	// Token: 0x060009D4 RID: 2516 RVA: 0x00057C2C File Offset: 0x00055E2C
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
			if (this.curAIState != BombKingPlayer.BombKingAIState.HasCrown && this.stateChangeTimer.Elapsed(false))
			{
				this.SetCurAIState((UnityEngine.Random.value < this.chanceToChase[(int)base.GamePlayer.Difficulty]) ? BombKingPlayer.BombKingAIState.Chasing : BombKingPlayer.BombKingAIState.ChasingCrown);
			}
			BombKingPlayer.BombKingAIState bombKingAIState = this.curAIState;
			if (bombKingAIState != BombKingPlayer.BombKingAIState.HasCrown)
			{
				if (bombKingAIState - BombKingPlayer.BombKingAIState.Chasing <= 1)
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
						BombKingPlayer bombKingPlayer = (BombKingPlayer)this.minigameController.GetPlayer(i);
						if (bombKingPlayer != this && !bombKingPlayer.IsDead && !bombKingPlayer.HoldingCrown)
						{
							if (this.followClosest)
							{
								float magnitude = (bombKingPlayer.transform.position - base.transform.position).magnitude;
								if (magnitude < num2)
								{
									this.targetPlayer = bombKingPlayer;
									num2 = magnitude;
								}
							}
							else
							{
								this.targets.Add(bombKingPlayer);
							}
						}
					}
					if (this.targets.Count > 0 && flag)
					{
						this.targetPlayer = this.targets[GameManager.rand.Next(0, this.targets.Count)];
					}
					if (this.curAIState == BombKingPlayer.BombKingAIState.ChasingCrown)
					{
						this.targetPlayer = this.minigameController.GetBombPlayer();
					}
					if (this.targetPlayer != null)
					{
						this.targetPosition = this.targetPlayer.transform.position;
					}
					else
					{
						Debug.LogWarning("Target null: " + this.curAIState.ToString() + " -- ID: " + base.GamePlayer.GlobalID.ToString());
					}
				}
			}
			else
			{
				this.GetHidePosition();
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

	// Token: 0x060009D5 RID: 2517 RVA: 0x00057FEC File Offset: 0x000561EC
	private void GetHidePosition()
	{
		float num = Vector3.Distance(this.targetPosition, base.transform.position);
		if (this.hidePositionTimer.Elapsed(true) || num < 0.6f)
		{
			int num2 = this.pointsToCheck[(int)base.GamePlayer.Difficulty];
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < this.minigameController.players.Count; i++)
			{
				BombKingPlayer bombKingPlayer = (BombKingPlayer)this.minigameController.GetPlayer(i);
				if (bombKingPlayer != this)
				{
					vector += bombKingPlayer.transform.position;
				}
			}
			vector /= (float)this.minigameController.players.Count;
			Vector3 vector2 = this.targetPosition;
			float num3 = this.GetPointValue(vector, this.targetPosition) * 0.8f;
			for (int j = 0; j < num2; j++)
			{
				Vector3 randomNavMeshPoint = this.minigameController.GetRandomNavMeshPoint();
				float pointValue = this.GetPointValue(vector, randomNavMeshPoint);
				if (pointValue > num3)
				{
					vector2 = randomNavMeshPoint;
					num3 = pointValue;
				}
			}
			this.targetPosition = vector2;
		}
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x0003203C File Offset: 0x0003023C
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

	// Token: 0x060009D7 RID: 2519 RVA: 0x0000A7D0 File Offset: 0x000089D0
	private void SetCurAIState(BombKingPlayer.BombKingAIState newState)
	{
		if (newState != BombKingPlayer.BombKingAIState.HasCrown)
		{
			if (newState == BombKingPlayer.BombKingAIState.Chasing)
			{
				this.targetPlayer = null;
				this.targetPosition = Vector3.one * 9000f;
			}
		}
		else
		{
			this.targetPlayer = null;
		}
		this.stateChangeTimer.Start();
		this.curAIState = newState;
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x00058104 File Offset: 0x00056304
	public override void ResetPlayer()
	{
		this.playerAnim.SetPlayerRotationImmediate(this.startRotation.eulerAngles.y);
		this.HoldingCrown = false;
		this.Stunned = false;
		if (base.IsOwner)
		{
			this.mover.Velocity = Vector3.zero;
			this.velocity.Value = Vector3.zero;
		}
		base.ResetPlayer();
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x0000A810 File Offset: 0x00008A10
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x0000A818 File Offset: 0x00008A18
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void PunchRPC(NetPlayer sender, bool hand)
	{
		this.StartPunch(hand);
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x00058168 File Offset: 0x00056368
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

	// Token: 0x060009DD RID: 2525 RVA: 0x0000A821 File Offset: 0x00008A21
	private void PunchImpactEvent(PlayerAnimationEvent anim_event)
	{
		this.Punch();
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x000581DC File Offset: 0x000563DC
	private void Punch()
	{
		if (!this.minigameController.Playable || this.Stunned || base.IsDead || !GameManager.PollInput || GameManager.IsGamePaused)
		{
			return;
		}
		AudioSystem.PlayOneShot(this.punch_miss, 0.75f, 0f, 1f);
		if (NetSystem.IsServer)
		{
			BombKingPlayer bombKingPlayer = null;
			float num = float.MaxValue;
			for (int i = 0; i < this.minigameController.GetPlayerCount(); i++)
			{
				BombKingPlayer bombKingPlayer2 = (BombKingPlayer)this.minigameController.GetPlayer(i);
				if (!(bombKingPlayer2 == this) && !bombKingPlayer2.isDead && !bombKingPlayer2.Stunned && Time.time - this.stunStartTime > this.stunImmunityLength)
				{
					float num2 = Vector3.Distance(bombKingPlayer2.transform.position, base.transform.position);
					if (num2 < this.punchHitDistance)
					{
						Vector3 b = base.transform.position - base.transform.forward * 0.75f;
						if (Vector3.Dot(base.transform.forward, (bombKingPlayer2.transform.position - b).normalized) > 1f - this.punchAngle / 180f && num2 < num)
						{
							num = num2;
							bombKingPlayer = bombKingPlayer2;
						}
					}
				}
			}
			if (bombKingPlayer != null)
			{
				if (bombKingPlayer.HoldingCrown)
				{
					bombKingPlayer.HoldingCrown = false;
					this.HoldingCrown = true;
				}
				else
				{
					NetVar<byte> netVar = this.achivementStunCount;
					byte value = netVar.Value;
					netVar.Value = value + 1;
				}
				bombKingPlayer.Stunned = true;
			}
		}
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x0000A829 File Offset: 0x00008A29
	public void StunnedRecieve(object val)
	{
		this.Stunned = (bool)val;
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x060009E0 RID: 2528 RVA: 0x0000A837 File Offset: 0x00008A37
	// (set) Token: 0x060009E1 RID: 2529 RVA: 0x00058380 File Offset: 0x00056580
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

	// Token: 0x060009E2 RID: 2530 RVA: 0x0000A844 File Offset: 0x00008A44
	public void HoldingBombRecieve(object val)
	{
		this.HoldingCrown = (bool)val;
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x060009E3 RID: 2531 RVA: 0x0000A852 File Offset: 0x00008A52
	// (set) Token: 0x060009E4 RID: 2532 RVA: 0x000583E4 File Offset: 0x000565E4
	public bool HoldingCrown
	{
		get
		{
			return this.holdingCrown.Value;
		}
		set
		{
			if (value)
			{
				AudioSystem.PlayOneShot(this.crownPickupSound, 1f, 0f, 1f);
			}
			this.holdingCrown.Value = value;
			this.crownObject.SetActive(value);
			this.scoreUpdateTimer.Start();
			this.SetCurAIState(value ? BombKingPlayer.BombKingAIState.HasCrown : BombKingPlayer.BombKingAIState.Chasing);
			this.mover.maxSpeed = (value ? this.bomb_speed : this.base_speed);
			this.targetPlayer = null;
		}
	}

	// Token: 0x04000890 RID: 2192
	public GameObject player_death_effect;

	// Token: 0x04000891 RID: 2193
	public AudioClip punch_miss;

	// Token: 0x04000892 RID: 2194
	public AudioClip punch_hit;

	// Token: 0x04000893 RID: 2195
	public AudioClip crownPickupSound;

	// Token: 0x04000894 RID: 2196
	public GameObject stun_fx_obj;

	// Token: 0x04000895 RID: 2197
	public GameObject crownObject;

	// Token: 0x04000896 RID: 2198
	public float base_speed = 6f;

	// Token: 0x04000897 RID: 2199
	public float bomb_speed = 6.75f;

	// Token: 0x04000898 RID: 2200
	private BombKingController minigameController;

	// Token: 0x04000899 RID: 2201
	private CharacterMover mover;

	// Token: 0x0400089A RID: 2202
	private float stunStartTime;

	// Token: 0x0400089B RID: 2203
	private float stunLength = 2.5f;

	// Token: 0x0400089C RID: 2204
	private float stunImmunityLength = 3.5f;

	// Token: 0x0400089D RID: 2205
	private float punchHitDistance = 3f;

	// Token: 0x0400089E RID: 2206
	private float punchAngle = 120f;

	// Token: 0x0400089F RID: 2207
	private float lastPunchTime;

	// Token: 0x040008A0 RID: 2208
	private float punchInterval = 0.75f;

	// Token: 0x040008A1 RID: 2209
	private CameraShake cameraShake;

	// Token: 0x040008A2 RID: 2210
	private BombKingPlayer.BombKingAIState curAIState = BombKingPlayer.BombKingAIState.Chasing;

	// Token: 0x040008A3 RID: 2211
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x040008A4 RID: 2212
	private ActionTimer hidePositionTimer = new ActionTimer(0.25f, 0.55f);

	// Token: 0x040008A5 RID: 2213
	private ActionTimer stateChangeTimer = new ActionTimer(2.5f, 3.5f);

	// Token: 0x040008A6 RID: 2214
	private BombKingPlayer targetPlayer;

	// Token: 0x040008A7 RID: 2215
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x040008A8 RID: 2216
	private bool gotAchievement;

	// Token: 0x040008A9 RID: 2217
	private ActionTimer scoreUpdateTimer = new ActionTimer(0.1f);

	// Token: 0x040008AA RID: 2218
	private float[] followClosestChance = new float[]
	{
		0.35f,
		0.6f,
		1.1f
	};

	// Token: 0x040008AB RID: 2219
	private float[] chanceToChase = new float[]
	{
		0.075f,
		0.15f,
		0.25f
	};

	// Token: 0x040008AC RID: 2220
	private ActionTimer followTimer = new ActionTimer(1f, 2f);

	// Token: 0x040008AD RID: 2221
	private bool followClosest;

	// Token: 0x040008AE RID: 2222
	private List<BombKingPlayer> targets = new List<BombKingPlayer>();

	// Token: 0x040008AF RID: 2223
	private int[] pointsToCheck = new int[]
	{
		3,
		6,
		18
	};

	// Token: 0x040008B0 RID: 2224
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<byte> achivementStunCount = new NetVar<byte>(0);

	// Token: 0x040008B1 RID: 2225
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<bool> stunned = new NetVar<bool>(false);

	// Token: 0x040008B2 RID: 2226
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<bool> holdingCrown = new NetVar<bool>(false);

	// Token: 0x02000156 RID: 342
	private enum BombKingAIState
	{
		// Token: 0x040008B4 RID: 2228
		HasCrown,
		// Token: 0x040008B5 RID: 2229
		Chasing,
		// Token: 0x040008B6 RID: 2230
		ChasingCrown
	}
}
