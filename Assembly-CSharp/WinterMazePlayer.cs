using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000599 RID: 1433
public class WinterMazePlayer : Movement1
{
	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x0600253C RID: 9532 RVA: 0x0001AB3B File Offset: 0x00018D3B
	public bool IsFinished
	{
		get
		{
			return this.m_hasFinished;
		}
	}

	// Token: 0x0600253D RID: 9533 RVA: 0x000E1008 File Offset: 0x000DF208
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.minigameController = (WinterMazeController)GameManager.Minigame;
		this.mover = base.GetComponent<CharacterMover>();
		CharacterMover characterMover = this.mover;
		characterMover.OnJump = (CharacterMover.OnJumpDelegate)Delegate.Combine(characterMover.OnJump, new CharacterMover.OnJumpDelegate(this.OnJump));
		bool flag = true;
		using (List<GamePlayer>.Enumerator enumerator = GameManager.PlayerList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsAI)
				{
					flag = false;
					break;
				}
			}
		}
		if (base.IsOwner)
		{
			this.net_z_rotation.Value = ZPMath.CompressFloat(0f, -45f, 45f);
			if (!this.player.IsAI || flag)
			{
				this.m_hasCamera = true;
				this.m_cameraParent.SetActive(true);
				this.m_cameraParent.transform.parent = null;
				this.m_cameraParent.transform.localRotation = Quaternion.identity;
				this.cameraShake = this.m_cameraParent.GetComponentInChildren<CameraShake>();
				this.minigameController.minigameCameras.Add(this.m_cam);
				List<GamePlayer> list = GameManager.GetLocalNonAIPlayers();
				if (flag)
				{
					list = GameManager.GetLocalAIPlayers();
				}
				if (list.Count > 1)
				{
					if (!flag)
					{
						this.m_cam.rect = base.GetPlayerSplitScreenRect(this.player);
					}
					else
					{
						this.m_cam.rect = base.GetPlayerSplitScreenRectWithAI(this.player);
					}
				}
				if (list.Count > 0 && list[0] == this.player)
				{
					this.m_listener.enabled = true;
				}
			}
		}
		if (!NetSystem.IsServer)
		{
			this.score.Recieve = new RecieveProxy(base.ScoreRecieve);
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

	// Token: 0x0600253E RID: 9534 RVA: 0x000E122C File Offset: 0x000DF42C
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

	// Token: 0x0600253F RID: 9535 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06002540 RID: 9536 RVA: 0x0001AB43 File Offset: 0x00018D43
	public void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.m_cameraParent);
	}

	// Token: 0x06002541 RID: 9537 RVA: 0x0001AB50 File Offset: 0x00018D50
	protected override void Start()
	{
		base.Start();
		this.minigameController.AddPlayer(this);
		this.playerAnim.RegisterListener(new AnimationEventListener(this.PunchImpactEvent), AnimationEventType.PunchImpact);
	}

	// Token: 0x06002542 RID: 9538 RVA: 0x000E127C File Offset: 0x000DF47C
	private void Update()
	{
		if (base.IsOwner && !base.IsDead && (double)base.transform.position.y < -0.2)
		{
			base.transform.position = this.startPosition;
		}
		if (base.IsOwner && !this.player.IsAI && !this.gotAchievement && this.Score >= 35)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_WINTER_MAZE");
			this.gotAchievement = true;
		}
		base.UpdateController();
		if (base.IsOwner && this.m_hasCamera && this.minigameController.Playable && !base.IsDead)
		{
			this.m_cameraParent.transform.position = base.transform.position;
		}
		if (base.IsOwner && this.minigameController.Playable && Time.time - this.lastPunchTime > this.punchInterval)
		{
			if (base.GamePlayer.IsAI)
			{
				if (this.agent.remainingDistance < 0.5f)
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
	}

	// Token: 0x06002543 RID: 9539 RVA: 0x000E143C File Offset: 0x000DF63C
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		bool val = !this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || this.isDead || this.Stunned;
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

	// Token: 0x06002544 RID: 9540 RVA: 0x000E15F0 File Offset: 0x000DF7F0
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

	// Token: 0x06002545 RID: 9541 RVA: 0x000E1698 File Offset: 0x000DF898
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.z);
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
			if (this.pathUpdateTimer.Elapsed(true) && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
			{
				this.agent.SetDestination(this.targetPosition);
			}
			Debug.DrawLine(base.transform.position, this.targetPosition, Color.green);
			Debug.DrawLine(base.transform.position, this.agent.pathEndPosition, Color.red);
			Vector2 a = new Vector2(this.targetPosition.x, this.targetPosition.z);
			Vector2 b = new Vector2(this.agent.pathEndPosition.x, this.agent.pathEndPosition.z);
			if ((a - vector).sqrMagnitude > num && Vector2.Distance(vector, b) > 0.5f)
			{
				Vector3 vector2 = this.agent.steeringTarget - base.transform.position;
				Vector2 normalized = new Vector2(vector2.x, vector2.z).normalized;
				result = new CharacterMoverInput(normalized, false, false);
			}
			else
			{
				int num2 = UnityEngine.Random.Range(-6, 7);
				int num3 = UnityEngine.Random.Range(-6, 7);
				this.targetPosition = new Vector3((float)num2 * 3.333f, 0f, (float)num3 * 3.333f);
				result.NullInput();
			}
		}
		return result;
	}

	// Token: 0x06002546 RID: 9542 RVA: 0x000E1890 File Offset: 0x000DFA90
	public override void ResetPlayer()
	{
		this.playerAnim.SetPlayerRotationImmediate(this.startRotation.eulerAngles.y);
		if (base.IsOwner)
		{
			this.mover.Velocity = Vector3.zero;
			this.velocity.Value = Vector3.zero;
		}
		base.ResetPlayer();
	}

	// Token: 0x06002547 RID: 9543 RVA: 0x0001AB7C File Offset: 0x00018D7C
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x06002548 RID: 9544 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06002549 RID: 9545 RVA: 0x0001AB84 File Offset: 0x00018D84
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void PunchRPC(NetPlayer sender, bool hand)
	{
		this.StartPunch(hand);
	}

	// Token: 0x0600254A RID: 9546 RVA: 0x000E18E8 File Offset: 0x000DFAE8
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

	// Token: 0x0600254B RID: 9547 RVA: 0x0001AB8D File Offset: 0x00018D8D
	private void PunchImpactEvent(PlayerAnimationEvent anim_event)
	{
		this.Punch();
	}

	// Token: 0x0600254C RID: 9548 RVA: 0x000E195C File Offset: 0x000DFB5C
	private void Punch()
	{
		if (!this.minigameController.Playable || this.Stunned || base.IsDead || !GameManager.PollInput || GameManager.IsGamePaused)
		{
			return;
		}
		AudioSystem.PlayOneShot(this.punch_miss, 0.75f, 0f, 1f);
		if (NetSystem.IsServer)
		{
			WinterMazePlayer winterMazePlayer = null;
			float num = float.MaxValue;
			for (int i = 0; i < this.minigameController.GetPlayerCount(); i++)
			{
				WinterMazePlayer winterMazePlayer2 = (WinterMazePlayer)this.minigameController.GetPlayer(i);
				if (!(winterMazePlayer2 == this) && !winterMazePlayer2.Stunned && Time.time - this.stunStartTime > this.stunImmunityLength)
				{
					float num2 = Vector3.Distance(winterMazePlayer2.transform.position, base.transform.position);
					if (num2 < this.punchHitDistance)
					{
						Vector3 b = base.transform.position - base.transform.forward * 0.75f;
						if (Vector3.Dot(base.transform.forward, (winterMazePlayer2.transform.position - b).normalized) > 1f - this.punchAngle / 180f && num2 < num)
						{
							num = num2;
							winterMazePlayer = winterMazePlayer2;
						}
					}
				}
			}
			if (winterMazePlayer != null)
			{
				winterMazePlayer.Stunned = true;
			}
		}
	}

	// Token: 0x0600254D RID: 9549 RVA: 0x0001AB95 File Offset: 0x00018D95
	public void StunnedRecieve(object val)
	{
		this.Stunned = (bool)val;
	}

	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x0600254E RID: 9550 RVA: 0x0001ABA3 File Offset: 0x00018DA3
	// (set) Token: 0x0600254F RID: 9551 RVA: 0x000E1AC4 File Offset: 0x000DFCC4
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

	// Token: 0x040028AF RID: 10415
	[SerializeField]
	protected GameObject m_cameraParent;

	// Token: 0x040028B0 RID: 10416
	[SerializeField]
	protected Camera m_cam;

	// Token: 0x040028B1 RID: 10417
	[SerializeField]
	protected AudioListener m_listener;

	// Token: 0x040028B2 RID: 10418
	[SerializeField]
	protected AudioClip m_finishSound;

	// Token: 0x040028B3 RID: 10419
	[SerializeField]
	protected GameObject m_finishFX;

	// Token: 0x040028B4 RID: 10420
	public float base_speed = 6f;

	// Token: 0x040028B5 RID: 10421
	public AudioClip punch_miss;

	// Token: 0x040028B6 RID: 10422
	public AudioClip punch_hit;

	// Token: 0x040028B7 RID: 10423
	public GameObject stun_fx_obj;

	// Token: 0x040028B8 RID: 10424
	private float stunStartTime;

	// Token: 0x040028B9 RID: 10425
	private float stunLength = 2.5f;

	// Token: 0x040028BA RID: 10426
	private float stunImmunityLength = 3.5f;

	// Token: 0x040028BB RID: 10427
	private float punchHitDistance = 3f;

	// Token: 0x040028BC RID: 10428
	private float punchAngle = 120f;

	// Token: 0x040028BD RID: 10429
	private float lastPunchTime;

	// Token: 0x040028BE RID: 10430
	private float punchInterval = 0.75f;

	// Token: 0x040028BF RID: 10431
	private WinterMazeController minigameController;

	// Token: 0x040028C0 RID: 10432
	private CharacterMover mover;

	// Token: 0x040028C1 RID: 10433
	private CameraShake cameraShake;

	// Token: 0x040028C2 RID: 10434
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x040028C3 RID: 10435
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x040028C4 RID: 10436
	private bool m_hasFinished;

	// Token: 0x040028C5 RID: 10437
	private bool gotAchievement;

	// Token: 0x040028C6 RID: 10438
	private bool m_hasCamera;

	// Token: 0x040028C7 RID: 10439
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<bool> stunned = new NetVar<bool>(false);
}
