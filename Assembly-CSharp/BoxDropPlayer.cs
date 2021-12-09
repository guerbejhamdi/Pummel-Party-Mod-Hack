using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200016F RID: 367
public class BoxDropPlayer : Movement1
{
	// Token: 0x06000A8D RID: 2701 RVA: 0x0005C7C4 File Offset: 0x0005A9C4
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.minigameController = (BoxDropController)GameManager.Minigame;
		this.minigameController.OnDropBoxes.AddListener(new UnityAction(this.OnDropBoxes));
		this.m_boxController = this.minigameController.Root.GetComponentInChildren<BoxController>();
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
		if (!base.IsOwner)
		{
			base.GetComponent<CharacterController>().enabled = false;
		}
		if (!this.player.IsAI || !base.IsOwner)
		{
			base.GetComponent<NavMeshAgent>().enabled = false;
		}
		this.targetPosition = UnityEngine.Random.onUnitSphere * 10f;
		this.targetPosition.y = 0f;
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x0005C8EC File Offset: 0x0005AAEC
	public void OnDropBoxes()
	{
		if (this.player.IsAI)
		{
			if (UnityEngine.Random.value < 0.8f)
			{
				this.UpdateTargetBox(false, 6f, 1000f);
				this.m_chooseDropping = false;
				return;
			}
			this.UpdateTargetBox(true, 6f, 1000f);
			this.m_chooseDropping = true;
		}
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x0005C944 File Offset: 0x0005AB44
	private void UpdateTargetBox(bool isDropping, float minDistance, float maxDistance = 1000f)
	{
		BoxDropBox closestBox = this.m_boxController.GetClosestBox(base.transform.position, isDropping, minDistance, maxDistance);
		if (closestBox != null)
		{
			this.targetPosition = closestBox.transform.position + UnityEngine.Random.onUnitSphere;
		}
		else
		{
			this.targetPosition = UnityEngine.Random.onUnitSphere * 10f;
		}
		this.targetPosition.y = 0f;
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x0005C9B8 File Offset: 0x0005ABB8
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

	// Token: 0x06000A91 RID: 2705 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0000AD8E File Offset: 0x00008F8E
	protected override void Start()
	{
		base.Start();
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x0005CA08 File Offset: 0x0005AC08
	private void Update()
	{
		if (base.IsOwner && !base.IsDead && base.transform.position.y < -6f)
		{
			if (NetSystem.IsServer)
			{
				this.KillPlayer(true);
			}
			else
			{
				base.SendRPC("RPCOwnerKillPlayer", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
			}
		}
		base.UpdateController();
		if (this.agent != null)
		{
			this.agent.updatePosition = false;
			this.agent.updateRotation = false;
		}
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x0000ADB8 File Offset: 0x00008FB8
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.SERVER)]
	public void RPCOwnerKillPlayer(NetPlayer sender)
	{
		this.KillPlayer(true);
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x0000ADB8 File Offset: 0x00008FB8
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender)
	{
		this.KillPlayer(true);
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000A96 RID: 2710 RVA: 0x0000ADC1 File Offset: 0x00008FC1
	public int Placement
	{
		get
		{
			return this.m_placement;
		}
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x0005CA8C File Offset: 0x0005AC8C
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
			AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
			if (Settings.BloodEffects)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.m_playerDeathEffect, base.transform.position, Quaternion.LookRotation(Vector3.up));
			}
			this.minigameController.PlayerDied(this);
			if (NetSystem.IsServer)
			{
				base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
			}
			this.m_placement = this.minigameController.DeadPlayerCount;
			BoxDropController boxDropController = this.minigameController;
			int deadPlayerCount = boxDropController.DeadPlayerCount;
			boxDropController.DeadPlayerCount = deadPlayerCount + 1;
		}
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x0005CB80 File Offset: 0x0005AD80
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
		input.NullInput(val);
		this.mover.CalculateVelocity(input, Time.deltaTime);
		this.mover.DoMovement(Time.deltaTime);
		this.mover.SmoothSlope();
		if (this.mover.MovementAxis != Vector2.zero)
		{
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(this.mover.MovementAxis.x, 0f, this.mover.MovementAxis.y), Vector3.up), 1500f * Time.deltaTime);
		}
		if (this.agent != null)
		{
			this.agent.nextPosition = base.transform.position;
			this.agent.velocity = this.mover.Velocity;
		}
		this.velocity.Value = this.mover.Velocity;
		this.netIsGrounded.Value = (this.curOffMeshLinkTranslationType != OffMeshLinkTranslateType.Parabola && this.mover.Grounded);
		base.DoMovement();
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x0005CD48 File Offset: 0x0005AF48
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

	// Token: 0x06000A9A RID: 2714 RVA: 0x0005CDF0 File Offset: 0x0005AFF0
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
				this.m_moveTime = Time.time + 1.5f + UnityEngine.Random.value;
			}
			else
			{
				result.NullInput();
				if (Time.time >= this.m_moveTime)
				{
					this.m_moveTime = Time.time + 1.5f + UnityEngine.Random.value;
					this.UpdateTargetBox(this.m_chooseDropping, 0f, 2.5f);
				}
			}
		}
		return result;
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x0000ADC9 File Offset: 0x00008FC9
	private void SetCurAIState(IcebergAIState newState)
	{
		if (!base.GamePlayer.IsLocalPlayer)
		{
			return;
		}
		this.curAIState = newState;
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x0005D004 File Offset: 0x0005B204
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

	// Token: 0x06000A9D RID: 2717 RVA: 0x0000ADE0 File Offset: 0x00008FE0
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x0000398C File Offset: 0x00001B8C
	public void OnTriggerEnter(Collider other)
	{
	}

	// Token: 0x04000974 RID: 2420
	[SerializeField]
	protected GameObject m_playerDeathEffect;

	// Token: 0x04000975 RID: 2421
	public float base_speed = 6f;

	// Token: 0x04000976 RID: 2422
	private BoxDropController minigameController;

	// Token: 0x04000977 RID: 2423
	private CharacterMover mover;

	// Token: 0x04000978 RID: 2424
	private CameraShake cameraShake;

	// Token: 0x04000979 RID: 2425
	private IcebergAIState curAIState;

	// Token: 0x0400097A RID: 2426
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x0400097B RID: 2427
	private ActionTimer hidePositionTimer = new ActionTimer(0.25f, 0.55f);

	// Token: 0x0400097C RID: 2428
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x0400097D RID: 2429
	private BoxController m_boxController;

	// Token: 0x0400097E RID: 2430
	private bool m_chooseDropping;

	// Token: 0x0400097F RID: 2431
	private int m_placement = 10;

	// Token: 0x04000980 RID: 2432
	private float[] followClosestChance = new float[]
	{
		0.35f,
		0.6f,
		1.1f
	};

	// Token: 0x04000981 RID: 2433
	private float[] chanceToChase = new float[]
	{
		0.075f,
		0.15f,
		0.25f
	};

	// Token: 0x04000982 RID: 2434
	private ActionTimer followTimer = new ActionTimer(1f, 2f);

	// Token: 0x04000983 RID: 2435
	private bool followClosest;

	// Token: 0x04000984 RID: 2436
	private List<PassTheBombPlayer> targets = new List<PassTheBombPlayer>();

	// Token: 0x04000985 RID: 2437
	private IcebergCheckpoint m_targetCheckpoint;

	// Token: 0x04000986 RID: 2438
	private float m_lastUpdateTime;

	// Token: 0x04000987 RID: 2439
	private float m_moveTime;
}
