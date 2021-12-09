using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000195 RID: 405
public class ChristmasTheftPlayer : Movement1
{
	// Token: 0x06000BA3 RID: 2979 RVA: 0x00063174 File Offset: 0x00061374
	public bool GetFreeSlotInfo(out int index, out Vector3 position)
	{
		for (int i = 0; i < this.m_slotStatus.Length; i++)
		{
			if (!this.m_slotStatus[i])
			{
				index = i;
				position = this.m_goal.GetPosition(index);
				return true;
			}
		}
		index = -1;
		position = Vector3.zero;
		return false;
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x0000B5C7 File Offset: 0x000097C7
	public void SetSlotStatus(int index, bool status)
	{
		this.m_slotStatus[index] = status;
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x000631C8 File Offset: 0x000613C8
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		foreach (ChristmasTheftPresentGoal christmasTheftPresentGoal in UnityEngine.Object.FindObjectsOfType<ChristmasTheftPresentGoal>())
		{
			if (christmasTheftPresentGoal.PlayerIndex == (int)this.player.GlobalID)
			{
				this.m_goal = christmasTheftPresentGoal;
				break;
			}
		}
		this.mover = base.GetComponent<CharacterMover>();
		CharacterMover characterMover = this.mover;
		characterMover.OnJump = (CharacterMover.OnJumpDelegate)Delegate.Combine(characterMover.OnJump, new CharacterMover.OnJumpDelegate(this.OnJump));
		if (base.IsOwner)
		{
			this.net_z_rotation.Value = ZPMath.CompressFloat(0f, -45f, 45f);
		}
		if (!NetSystem.IsServer)
		{
			this.n_presentHeld.Recieve = new RecieveProxy(this.PresentHeldRecieve);
			this.score.Recieve = new RecieveProxy(base.ScoreRecieve);
			this.PresentsHeld = this.n_presentHeld.Value;
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
		int globalID = (int)this.player.GlobalID;
		if (globalID < this.m_areaParticleMats.Length)
		{
			this.m_areaParticleMats[globalID].SetColor("_TintColor", base.GamePlayer.Color.skinColor1 * 0.75f);
		}
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x00063348 File Offset: 0x00061548
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

	// Token: 0x06000BA7 RID: 2983 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06000BA8 RID: 2984 RVA: 0x00063398 File Offset: 0x00061598
	protected override void Start()
	{
		base.Start();
		this.minigameController = (ChristmasTheftController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
		if (!this.player.IsAI)
		{
			this.mover.SetForwardVector(Vector3.left);
		}
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x0000B5D2 File Offset: 0x000097D2
	private void Update()
	{
		base.UpdateController();
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x00063410 File Offset: 0x00061610
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
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(this.mover.MovementAxis.x, 0f, this.mover.MovementAxis.y), Vector3.up), 1500f * Time.deltaTime);
			}
		}
		this.velocity.Value = this.mover.Velocity;
		this.netIsGrounded.Value = (this.curOffMeshLinkTranslationType != OffMeshLinkTranslateType.Parabola && this.mover.Grounded);
		base.DoMovement();
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x000635BC File Offset: 0x000617BC
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

	// Token: 0x06000BAC RID: 2988 RVA: 0x00063664 File Offset: 0x00061864
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
			ChristmasTheftAIState christmasTheftAIState = this.curAIState;
			if (christmasTheftAIState != ChristmasTheftAIState.SeekingPresent)
			{
				if (christmasTheftAIState == ChristmasTheftAIState.ReturningPresent)
				{
					if (this.m_goal != null)
					{
						this.targetPosition = this.m_goal.transform.position;
					}
				}
			}
			else
			{
				if (this.m_targetPresent == null || this.m_targetPresent.State == ChristmasTheftPresentState.Held)
				{
					List<ChristmasTheftPresent> presentsByDistance = this.minigameController.GetPresentsByDistance(this, base.transform.position);
					if (presentsByDistance.Count > 0)
					{
						int index = UnityEngine.Random.Range(0, Mathf.Min(3, presentsByDistance.Count));
						this.m_targetPresent = presentsByDistance[index];
					}
				}
				if (this.m_targetPresent != null)
				{
					this.targetPosition = this.m_targetPresent.transform.position;
				}
			}
			if (this.pathUpdateTimer.Elapsed(true) && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
			{
				this.agent.SetDestination(this.targetPosition);
			}
			Debug.DrawLine(base.transform.position, this.targetPosition, Color.green);
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

	// Token: 0x06000BAD RID: 2989 RVA: 0x0003203C File Offset: 0x0003023C
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

	// Token: 0x06000BAE RID: 2990 RVA: 0x0000B5DA File Offset: 0x000097DA
	private void SetCurAIState(ChristmasTheftAIState newState)
	{
		if (!base.GamePlayer.IsLocalPlayer)
		{
			return;
		}
		this.m_targetPresent = null;
		this.curAIState = newState;
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x00063894 File Offset: 0x00061A94
	private void OnTriggerEnter(Collider other)
	{
		if (!base.IsOwner)
		{
			return;
		}
		ChristmasTheftPresent component = other.GetComponent<ChristmasTheftPresent>();
		if (component != null)
		{
			if (base.IsOwner && !this.player.IsAI && component.Player != null && component.Player.Score == 1 && component.Player != this)
			{
				PlatformAchievementManager.Instance.TriggerAchievement("ACH_GRIFTING_GIFTS");
			}
			this.minigameController.RequestGrabPresent(component.ID, this);
		}
		ChristmasTheftPresentGoal component2 = other.GetComponent<ChristmasTheftPresentGoal>();
		if (component2 != null)
		{
			if (component2 == this.m_goal)
			{
				this.minigameController.RequestDropPresents(this);
				return;
			}
			Debug.LogWarning("Wrong Goal!");
		}
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x00063950 File Offset: 0x00061B50
	public override void ResetPlayer()
	{
		this.playerAnim.SetPlayerRotationImmediate(this.startRotation.eulerAngles.y);
		this.PresentsHeld = 0;
		if (base.IsOwner)
		{
			this.mover.Velocity = Vector3.zero;
			this.velocity.Value = Vector3.zero;
		}
		base.ResetPlayer();
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x0000B5F8 File Offset: 0x000097F8
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x0000B600 File Offset: 0x00009800
	public void PresentHeldRecieve(object val)
	{
		this.PresentsHeld = (byte)val;
	}

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x0000B60E File Offset: 0x0000980E
	// (set) Token: 0x06000BB5 RID: 2997 RVA: 0x000639B0 File Offset: 0x00061BB0
	public byte PresentsHeld
	{
		get
		{
			return this.n_presentHeld.Value;
		}
		set
		{
			this.n_presentHeld.Value = value;
			this.m_presentGraphic.SetActive(value > 0);
			this.playerAnim.Carrying = (value > 0);
			this.SetCurAIState((value > 0) ? ChristmasTheftAIState.ReturningPresent : ChristmasTheftAIState.SeekingPresent);
			this.mover.maxSpeed = this.base_speed;
		}
	}

	// Token: 0x04000ACB RID: 2763
	[SerializeField]
	protected GameObject m_presentGraphic;

	// Token: 0x04000ACC RID: 2764
	[SerializeField]
	protected Material[] m_areaParticleMats;

	// Token: 0x04000ACD RID: 2765
	public float base_speed = 6f;

	// Token: 0x04000ACE RID: 2766
	private ChristmasTheftController minigameController;

	// Token: 0x04000ACF RID: 2767
	private CharacterMover mover;

	// Token: 0x04000AD0 RID: 2768
	private CameraShake cameraShake;

	// Token: 0x04000AD1 RID: 2769
	private ChristmasTheftAIState curAIState;

	// Token: 0x04000AD2 RID: 2770
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04000AD3 RID: 2771
	private ActionTimer hidePositionTimer = new ActionTimer(0.25f, 0.55f);

	// Token: 0x04000AD4 RID: 2772
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x04000AD5 RID: 2773
	private bool[] m_slotStatus = new bool[8];

	// Token: 0x04000AD6 RID: 2774
	private ChristmasTheftPresentGoal m_goal;

	// Token: 0x04000AD7 RID: 2775
	private ChristmasTheftPresent m_targetPresent;

	// Token: 0x04000AD8 RID: 2776
	private float[] followClosestChance = new float[]
	{
		0.35f,
		0.6f,
		1.1f
	};

	// Token: 0x04000AD9 RID: 2777
	private float[] chanceToChase = new float[]
	{
		0.075f,
		0.15f,
		0.25f
	};

	// Token: 0x04000ADA RID: 2778
	private ActionTimer followTimer = new ActionTimer(1f, 2f);

	// Token: 0x04000ADB RID: 2779
	private bool followClosest;

	// Token: 0x04000ADC RID: 2780
	private List<PassTheBombPlayer> targets = new List<PassTheBombPlayer>();

	// Token: 0x04000ADD RID: 2781
	private int[] pointsToCheck = new int[]
	{
		3,
		6,
		18
	};

	// Token: 0x04000ADE RID: 2782
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<byte> n_presentHeld = new NetVar<byte>(0);
}
