using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001E7 RID: 487
public class MysteryMazePlayer : Movement1
{
	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000E2C RID: 3628 RVA: 0x0000C9E5 File Offset: 0x0000ABE5
	public bool IsFinished
	{
		get
		{
			return this.m_hasFinished;
		}
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x00071B0C File Offset: 0x0006FD0C
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.minigameController = (MysteryMazeController)GameManager.Minigame;
		this.mover = base.GetComponent<CharacterMover>();
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
		switch (this.player.Difficulty)
		{
		case BotDifficulty.Easy:
			this.m_curAITargetGroupIndex = 0;
			return;
		case BotDifficulty.Normal:
			this.m_curAITargetGroupIndex = 1;
			return;
		case BotDifficulty.Hard:
			this.m_curAITargetGroupIndex = 1;
			return;
		default:
			return;
		}
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x00071D08 File Offset: 0x0006FF08
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

	// Token: 0x06000E2F RID: 3631 RVA: 0x0000C9ED File Offset: 0x0000ABED
	public void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.m_cameraParent);
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x0000C9FA File Offset: 0x0000ABFA
	public void Awake()
	{
		base.InitializeController();
		this.m_nextFreezeCheck = Time.time + 2f + UnityEngine.Random.value;
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x0000CA19 File Offset: 0x0000AC19
	protected override void Start()
	{
		base.Start();
		this.minigameController.AddPlayer(this);
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x00071D58 File Offset: 0x0006FF58
	private void Update()
	{
		if (Time.time > this.m_nextFreezeCheck)
		{
			this.m_nextFreezeCheck = Time.time + 2f + UnityEngine.Random.value;
			float num = 0f;
			switch (this.player.Difficulty)
			{
			case BotDifficulty.Easy:
				num = 0.45f;
				break;
			case BotDifficulty.Normal:
				num = 0.3f;
				break;
			case BotDifficulty.Hard:
				num = 0.1f;
				break;
			}
			if (UnityEngine.Random.value < num)
			{
				this.m_freezeUntilTime = Time.time + 0.75f + UnityEngine.Random.value * 0.4f;
			}
		}
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

	// Token: 0x06000E33 RID: 3635 RVA: 0x0000CA2D File Offset: 0x0000AC2D
	private IEnumerator Kill()
	{
		this.hasDied = true;
		base.IsDead = true;
		yield return new WaitForSeconds(0.5f);
		base.IsDead = false;
		Vector3 zero = Vector3.zero;
		Quaternion identity = Quaternion.identity;
		base.transform.position = zero;
		base.transform.rotation = identity;
		if (this.agent != null)
		{
			this.agent.velocity = Vector3.zero;
			this.agent.Warp(zero);
			this.agent.nextPosition = zero;
		}
		if (this.playerAnim != null)
		{
			this.playerAnim.SetPlayerRotationImmediate(base.transform.rotation.eulerAngles.y);
		}
		if (this.player.IsAI)
		{
			if (this.player.Difficulty == BotDifficulty.Hard)
			{
				this.m_curAITargetGroupIndex += 2;
			}
			else
			{
				this.m_curAITargetGroupIndex++;
			}
			Debug.Log(this.m_curAITargetGroupIndex);
			this.m_targetNode = null;
		}
		yield break;
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x00071E90 File Offset: 0x00070090
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		bool val = !this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || this.isDead;
		if (!base.GamePlayer.IsAI)
		{
			Vector2 axis = new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
			input = new CharacterMoverInput(axis, false, false);
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

	// Token: 0x06000E35 RID: 3637 RVA: 0x00072044 File Offset: 0x00070244
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

	// Token: 0x06000E36 RID: 3638 RVA: 0x000720EC File Offset: 0x000702EC
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.z);
		if (this.agent.isOnOffMeshLink)
		{
			this.agent.updatePosition = true;
			this.agent.CompleteOffMeshLink();
			this.curOffMeshLinkTranslationType = OffMeshLinkTranslateType.None;
		}
		else
		{
			float num = 0.36f;
			if (this.m_targetNode == null)
			{
				this.m_targetNode = this.minigameController.GetRandomAITarget(this.m_curAITargetGroupIndex);
			}
			if (this.m_targetNode != null)
			{
				this.targetPosition = this.m_targetNode.transform.position;
			}
			else
			{
				this.targetPosition = Vector3.zero;
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
				this.m_timeStopped = 0f;
				Vector3 vector2 = this.agent.steeringTarget - base.transform.position;
				Vector2 axis = new Vector2(vector2.x, vector2.z).normalized;
				if (Time.time < this.m_freezeUntilTime)
				{
					axis = Vector3.zero;
				}
				result = new CharacterMoverInput(axis, false, false);
			}
			else
			{
				this.m_timeStopped += Time.deltaTime;
				if (this.m_timeStopped >= 5f)
				{
					if (this.player.Difficulty == BotDifficulty.Hard)
					{
						this.m_curAITargetGroupIndex += 2;
					}
					else
					{
						this.m_curAITargetGroupIndex++;
					}
					this.m_targetNode = null;
				}
				result.NullInput();
			}
		}
		return result;
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x0000CA3C File Offset: 0x0000AC3C
	private void SetCurAIState(IcebergAIState newState)
	{
		if (!base.GamePlayer.IsLocalPlayer)
		{
			return;
		}
		this.curAIState = newState;
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x000723E0 File Offset: 0x000705E0
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

	// Token: 0x06000E39 RID: 3641 RVA: 0x00072438 File Offset: 0x00070638
	private void FinishRace()
	{
		if (this.m_hasFinished)
		{
			return;
		}
		this.m_hasFinished = true;
		this.DoFinishFanfare();
		if (NetSystem.IsServer)
		{
			this.Score = this.m_finishingScores[this.minigameController.FinishedPlayers];
			MysteryMazeController mysteryMazeController = this.minigameController;
			int finishedPlayers = mysteryMazeController.FinishedPlayers;
			mysteryMazeController.FinishedPlayers = finishedPlayers + 1;
			return;
		}
		if (base.IsOwner)
		{
			base.SendRPC("RPCFinish", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06000E3A RID: 3642 RVA: 0x0000CA53 File Offset: 0x0000AC53
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCFinish(NetPlayer sender)
	{
		this.FinishRace();
	}

	// Token: 0x06000E3B RID: 3643 RVA: 0x000724AC File Offset: 0x000706AC
	private void DoFinishFanfare()
	{
		AudioSystem.PlayOneShot(this.m_finishSound, 1f, 0f, 1f);
		UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(this.m_finishFX, base.transform.position, Quaternion.identity), 5f);
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x0000CA5B File Offset: 0x0000AC5B
	public void OnTriggerEnter(Collider other)
	{
		if (!base.IsOwner || this.m_hasFinished)
		{
			return;
		}
		if (other.gameObject != null && other.gameObject.name == "WinCollider")
		{
			this.FinishRace();
		}
	}

	// Token: 0x04000D9F RID: 3487
	[SerializeField]
	protected GameObject m_cameraParent;

	// Token: 0x04000DA0 RID: 3488
	[SerializeField]
	protected Camera m_cam;

	// Token: 0x04000DA1 RID: 3489
	[SerializeField]
	protected AudioListener m_listener;

	// Token: 0x04000DA2 RID: 3490
	[SerializeField]
	protected AudioClip m_finishSound;

	// Token: 0x04000DA3 RID: 3491
	[SerializeField]
	protected GameObject m_finishFX;

	// Token: 0x04000DA4 RID: 3492
	public float base_speed = 6f;

	// Token: 0x04000DA5 RID: 3493
	private MysteryMazeController minigameController;

	// Token: 0x04000DA6 RID: 3494
	private CharacterMover mover;

	// Token: 0x04000DA7 RID: 3495
	private CameraShake cameraShake;

	// Token: 0x04000DA8 RID: 3496
	private IcebergAIState curAIState;

	// Token: 0x04000DA9 RID: 3497
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04000DAA RID: 3498
	private ActionTimer offsetUpdateTimer = new ActionTimer(1f, 1f);

	// Token: 0x04000DAB RID: 3499
	private ActionTimer hidePositionTimer = new ActionTimer(0.25f, 0.55f);

	// Token: 0x04000DAC RID: 3500
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x04000DAD RID: 3501
	private bool m_hasFinished;

	// Token: 0x04000DAE RID: 3502
	private bool m_hasCamera;

	// Token: 0x04000DAF RID: 3503
	private int m_curAITargetGroupIndex;

	// Token: 0x04000DB0 RID: 3504
	private MysteryMazeAITarget m_targetNode;

	// Token: 0x04000DB1 RID: 3505
	private float m_nextFreezeCheck;

	// Token: 0x04000DB2 RID: 3506
	private float[] followClosestChance = new float[]
	{
		0.35f,
		0.6f,
		1.1f
	};

	// Token: 0x04000DB3 RID: 3507
	private float[] chanceToChase = new float[]
	{
		0.075f,
		0.15f,
		0.25f
	};

	// Token: 0x04000DB4 RID: 3508
	private ActionTimer followTimer = new ActionTimer(1f, 2f);

	// Token: 0x04000DB5 RID: 3509
	private bool followClosest;

	// Token: 0x04000DB6 RID: 3510
	private List<PassTheBombPlayer> targets = new List<PassTheBombPlayer>();

	// Token: 0x04000DB7 RID: 3511
	private Vector3 m_offset = Vector3.zero;

	// Token: 0x04000DB8 RID: 3512
	private float m_freezeUntilTime;

	// Token: 0x04000DB9 RID: 3513
	private float m_timeStopped;

	// Token: 0x04000DBA RID: 3514
	private short[] m_finishingScores = new short[]
	{
		200,
		100,
		50,
		25,
		10,
		5,
		2,
		1,
		0,
		0
	};

	// Token: 0x04000DBB RID: 3515
	private bool hasDied;
}
