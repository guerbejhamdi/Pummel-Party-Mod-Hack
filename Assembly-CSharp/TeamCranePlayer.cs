using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200027A RID: 634
public class TeamCranePlayer : Movement1
{
	// Token: 0x06001281 RID: 4737 RVA: 0x0008F068 File Offset: 0x0008D268
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

	// Token: 0x06001282 RID: 4738 RVA: 0x0008F190 File Offset: 0x0008D390
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

	// Token: 0x06001283 RID: 4739 RVA: 0x0008F1E8 File Offset: 0x0008D3E8
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

	// Token: 0x06001284 RID: 4740 RVA: 0x0008F25C File Offset: 0x0008D45C
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

	// Token: 0x06001285 RID: 4741 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06001286 RID: 4742 RVA: 0x0000EE73 File Offset: 0x0000D073
	protected override void Start()
	{
		base.Start();
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
	}

	// Token: 0x06001287 RID: 4743 RVA: 0x0000EE9D File Offset: 0x0000D09D
	private void Update()
	{
		base.UpdateController();
		if (this.agent != null)
		{
			this.agent.updatePosition = false;
			this.agent.updateRotation = false;
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06001288 RID: 4744 RVA: 0x0000EECB File Offset: 0x0000D0CB
	public int Placement
	{
		get
		{
			return this.m_placement;
		}
	}

	// Token: 0x06001289 RID: 4745 RVA: 0x0008F2AC File Offset: 0x0008D4AC
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

	// Token: 0x0600128A RID: 4746 RVA: 0x0008F474 File Offset: 0x0008D674
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

	// Token: 0x0600128B RID: 4747 RVA: 0x0008F51C File Offset: 0x0008D71C
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
			if (this.pathUpdateTimer.Elapsed(true))
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

	// Token: 0x0600128C RID: 4748 RVA: 0x0000EED3 File Offset: 0x0000D0D3
	private void SetCurAIState(IcebergAIState newState)
	{
		if (!base.GamePlayer.IsLocalPlayer)
		{
			return;
		}
		this.curAIState = newState;
	}

	// Token: 0x0600128D RID: 4749 RVA: 0x0008F714 File Offset: 0x0008D914
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

	// Token: 0x0600128E RID: 4750 RVA: 0x0000EEEA File Offset: 0x0000D0EA
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x0000398C File Offset: 0x00001B8C
	public void OnTriggerEnter(Collider other)
	{
	}

	// Token: 0x04001390 RID: 5008
	[SerializeField]
	protected GameObject m_playerDeathEffect;

	// Token: 0x04001391 RID: 5009
	public float base_speed = 6f;

	// Token: 0x04001392 RID: 5010
	private BoxDropController minigameController;

	// Token: 0x04001393 RID: 5011
	private CharacterMover mover;

	// Token: 0x04001394 RID: 5012
	private CameraShake cameraShake;

	// Token: 0x04001395 RID: 5013
	private IcebergAIState curAIState;

	// Token: 0x04001396 RID: 5014
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04001397 RID: 5015
	private ActionTimer hidePositionTimer = new ActionTimer(0.25f, 0.55f);

	// Token: 0x04001398 RID: 5016
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x04001399 RID: 5017
	private BoxController m_boxController;

	// Token: 0x0400139A RID: 5018
	private bool m_chooseDropping;

	// Token: 0x0400139B RID: 5019
	private int m_placement = 10;

	// Token: 0x0400139C RID: 5020
	private float[] followClosestChance = new float[]
	{
		0.35f,
		0.6f,
		1.1f
	};

	// Token: 0x0400139D RID: 5021
	private float[] chanceToChase = new float[]
	{
		0.075f,
		0.15f,
		0.25f
	};

	// Token: 0x0400139E RID: 5022
	private ActionTimer followTimer = new ActionTimer(1f, 2f);

	// Token: 0x0400139F RID: 5023
	private bool followClosest;

	// Token: 0x040013A0 RID: 5024
	private List<PassTheBombPlayer> targets = new List<PassTheBombPlayer>();

	// Token: 0x040013A1 RID: 5025
	private IcebergCheckpoint m_targetCheckpoint;

	// Token: 0x040013A2 RID: 5026
	private float m_lastUpdateTime;

	// Token: 0x040013A3 RID: 5027
	private float m_moveTime;
}
