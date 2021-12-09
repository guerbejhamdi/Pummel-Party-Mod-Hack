using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001BF RID: 447
public class IcebergPlayer : Movement1
{
	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06000CDD RID: 3293 RVA: 0x0000BECB File Offset: 0x0000A0CB
	public bool IsFinished
	{
		get
		{
			return this.m_hasFinished;
		}
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x0006ABC4 File Offset: 0x00068DC4
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.minigameController = (IcebergController)GameManager.Minigame;
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
			this.m_lastCheckpoint = this.minigameController.Root.transform.Find("CheckPoints/CheckPoint_01").GetComponent<IcebergCheckpoint>();
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
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x0006ADD8 File Offset: 0x00068FD8
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

	// Token: 0x06000CE0 RID: 3296 RVA: 0x0000BED3 File Offset: 0x0000A0D3
	public void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.m_cameraParent);
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x0000BEE0 File Offset: 0x0000A0E0
	protected override void Start()
	{
		base.Start();
		this.minigameController.AddPlayer(this);
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x0006AE28 File Offset: 0x00069028
	private void Update()
	{
		if (base.IsOwner && !base.IsDead && (double)base.transform.position.y < -0.1)
		{
			base.StartCoroutine(this.Kill());
		}
		base.UpdateController();
		if (base.IsOwner && this.m_hasCamera && this.minigameController.Playable && !base.IsDead)
		{
			this.m_cameraParent.transform.position = base.transform.position;
		}
		if (this.agent != null)
		{
			this.agent.updatePosition = false;
			this.agent.updateRotation = false;
		}
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x0000BEF4 File Offset: 0x0000A0F4
	private IEnumerator Kill()
	{
		this.hasDied = true;
		base.IsDead = true;
		yield return new WaitForSeconds(0.5f);
		base.IsDead = false;
		Vector3 position = this.m_lastCheckpoint.transform.position;
		Quaternion identity = Quaternion.identity;
		base.transform.position = position;
		base.transform.rotation = identity;
		if (this.agent != null)
		{
			this.agent.velocity = Vector3.zero;
			this.agent.Warp(position);
			this.agent.nextPosition = position;
		}
		if (this.playerAnim != null)
		{
			this.playerAnim.SetPlayerRotationImmediate(base.transform.rotation.eulerAngles.y);
		}
		yield break;
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x0006AEDC File Offset: 0x000690DC
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

	// Token: 0x06000CE6 RID: 3302 RVA: 0x0006B0A4 File Offset: 0x000692A4
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

	// Token: 0x06000CE7 RID: 3303 RVA: 0x0006B14C File Offset: 0x0006934C
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
			if (this.m_targetCheckpoint == null)
			{
				this.m_targetCheckpoint = IcebergCheckpoint.GetNextCheckPoint(-1);
			}
			if (this.m_lastCheckpoint != null && this.m_targetCheckpoint.Index <= this.m_lastCheckpoint.Index)
			{
				this.m_targetCheckpoint = IcebergCheckpoint.GetNextCheckPoint(this.m_lastCheckpoint.Index);
			}
			if (this.m_targetCheckpoint == null)
			{
				this.targetPosition = new Vector3(55f, 1f, 206f);
			}
			else
			{
				this.targetPosition = this.m_targetCheckpoint.transform.position;
			}
			if (this.offsetUpdateTimer.Elapsed(true))
			{
				this.m_offset = UnityEngine.Random.onUnitSphere * 0.25f;
				this.m_offset.y = 0f;
			}
			if (this.pathUpdateTimer.Elapsed(false) && !this.agent.pathPending && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
			{
				this.agent.SetDestination(this.targetPosition + this.m_offset);
				Debug.DrawLine(base.transform.position, base.transform.position + Vector3.up * 5f, Color.yellow);
				this.pathUpdateTimer.Start();
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
				result.NullInput();
			}
		}
		return result;
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x0000BF03 File Offset: 0x0000A103
	private void SetCurAIState(IcebergAIState newState)
	{
		if (!base.GamePlayer.IsLocalPlayer)
		{
			return;
		}
		this.curAIState = newState;
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x0006B430 File Offset: 0x00069630
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

	// Token: 0x06000CEA RID: 3306 RVA: 0x0006B488 File Offset: 0x00069688
	private void FinishRace()
	{
		if (this.m_hasFinished)
		{
			return;
		}
		this.m_hasFinished = true;
		if (base.IsOwner && !this.player.IsAI && !this.hasDied)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_SLIPPERY_SPRINT");
		}
		if (!NetSystem.IsServer)
		{
			base.SendRPC("RPCFinishRace", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
			return;
		}
		this.Score = this.m_finishingScores[this.minigameController.FinishedPlayers];
		IcebergController icebergController = this.minigameController;
		int finishedPlayers = icebergController.FinishedPlayers;
		icebergController.FinishedPlayers = finishedPlayers + 1;
		this.DoFinishFanfare();
	}

	// Token: 0x06000CEB RID: 3307 RVA: 0x0000BF1A File Offset: 0x0000A11A
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.SERVER)]
	public void RPCFinishRace(NetPlayer sender)
	{
		this.FinishRace();
	}

	// Token: 0x06000CEC RID: 3308 RVA: 0x0006B520 File Offset: 0x00069720
	private void DoFinishFanfare()
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCDoFinishFanfare", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		AudioSystem.PlayOneShot(this.m_finishSound, 1f, 0f, 1f);
		UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(this.m_finishFX, base.transform.position, Quaternion.identity), 5f);
	}

	// Token: 0x06000CED RID: 3309 RVA: 0x0000BF22 File Offset: 0x0000A122
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCDoFinishFanfare(NetPlayer sender)
	{
		this.DoFinishFanfare();
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x0000BF2A File Offset: 0x0000A12A
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x0006B584 File Offset: 0x00069784
	public void OnTriggerEnter(Collider other)
	{
		if (!base.IsOwner)
		{
			return;
		}
		IcebergCheckpoint component = other.gameObject.GetComponent<IcebergCheckpoint>();
		if (component != null && (this.m_lastCheckpoint == null || component.Index > this.m_lastCheckpoint.Index))
		{
			this.m_lastCheckpoint = component;
			if (this.m_lastCheckpoint.Index == 100)
			{
				this.FinishRace();
			}
		}
	}

	// Token: 0x04000C32 RID: 3122
	[SerializeField]
	protected GameObject m_cameraParent;

	// Token: 0x04000C33 RID: 3123
	[SerializeField]
	protected Camera m_cam;

	// Token: 0x04000C34 RID: 3124
	[SerializeField]
	protected AudioListener m_listener;

	// Token: 0x04000C35 RID: 3125
	[SerializeField]
	protected AudioClip m_finishSound;

	// Token: 0x04000C36 RID: 3126
	[SerializeField]
	protected GameObject m_finishFX;

	// Token: 0x04000C37 RID: 3127
	public float base_speed = 6f;

	// Token: 0x04000C38 RID: 3128
	private IcebergController minigameController;

	// Token: 0x04000C39 RID: 3129
	private CharacterMover mover;

	// Token: 0x04000C3A RID: 3130
	private CameraShake cameraShake;

	// Token: 0x04000C3B RID: 3131
	private IcebergAIState curAIState;

	// Token: 0x04000C3C RID: 3132
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04000C3D RID: 3133
	private ActionTimer offsetUpdateTimer = new ActionTimer(1f, 1f);

	// Token: 0x04000C3E RID: 3134
	private ActionTimer hidePositionTimer = new ActionTimer(0.25f, 0.55f);

	// Token: 0x04000C3F RID: 3135
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x04000C40 RID: 3136
	private IcebergCheckpoint m_lastCheckpoint;

	// Token: 0x04000C41 RID: 3137
	private bool m_hasFinished;

	// Token: 0x04000C42 RID: 3138
	private bool m_hasCamera;

	// Token: 0x04000C43 RID: 3139
	private float[] followClosestChance = new float[]
	{
		0.35f,
		0.6f,
		1.1f
	};

	// Token: 0x04000C44 RID: 3140
	private float[] chanceToChase = new float[]
	{
		0.075f,
		0.15f,
		0.25f
	};

	// Token: 0x04000C45 RID: 3141
	private ActionTimer followTimer = new ActionTimer(1f, 2f);

	// Token: 0x04000C46 RID: 3142
	private bool followClosest;

	// Token: 0x04000C47 RID: 3143
	private List<PassTheBombPlayer> targets = new List<PassTheBombPlayer>();

	// Token: 0x04000C48 RID: 3144
	private IcebergCheckpoint m_targetCheckpoint;

	// Token: 0x04000C49 RID: 3145
	private Vector3 m_offset = Vector3.zero;

	// Token: 0x04000C4A RID: 3146
	private short[] m_finishingScores = new short[]
	{
		200,
		150,
		100,
		75,
		50,
		25,
		10,
		5,
		2,
		1
	};

	// Token: 0x04000C4B RID: 3147
	private bool hasDied;
}
