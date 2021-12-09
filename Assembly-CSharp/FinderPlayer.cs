using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001AD RID: 429
public class FinderPlayer : Movement1
{
	// Token: 0x06000C3E RID: 3134 RVA: 0x00066DDC File Offset: 0x00064FDC
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		if (base.IsOwner)
		{
			this.net_z_rotation.Value = ZPMath.CompressFloat(0f, -45f, 45f);
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

	// Token: 0x06000C3F RID: 3135 RVA: 0x00066E78 File Offset: 0x00065078
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

	// Token: 0x06000C40 RID: 3136 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x00066EC8 File Offset: 0x000650C8
	protected override void Start()
	{
		base.Start();
		this.minigameController = (FinderController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
		Material material = new Material(this.floatyRenderer.material);
		material.color = base.GamePlayer.Color.skinColor1;
		this.floatyRenderer.material = material;
		if (!this.player.IsAI)
		{
			this.mover.SetForwardVector(Vector3.forward);
		}
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x00066F60 File Offset: 0x00065160
	public override void FinishedSpawning()
	{
		base.FinishedSpawning();
		this.playerAnim.Animator.SetTrigger("TreadingWater");
		this.playerAnim.Animator.SetFloat("RandomOffset", (float)GameManager.rand.NextDouble());
		this.floatyRenderer.enabled = true;
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x0000ABAF File Offset: 0x00008DAF
	private void Update()
	{
		if (!this.isDead)
		{
			base.UpdateController();
		}
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x0000BB11 File Offset: 0x00009D11
	public void FixedUpdate()
	{
		if (!this.isDead && NetSystem.IsServer)
		{
			bool playable = this.minigameController.Playable;
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06000C45 RID: 3141 RVA: 0x0000BB2E File Offset: 0x00009D2E
	// (set) Token: 0x06000C46 RID: 3142 RVA: 0x0000BB36 File Offset: 0x00009D36
	public bool SharkAttack
	{
		get
		{
			return this.sharkAttack;
		}
		set
		{
			this.sharkAttack = value;
			this.mover.Velocity = Vector3.zero;
		}
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x00066FB4 File Offset: 0x000651B4
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		bool val = !this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || this.isDead || this.sharkAttack;
		if (!base.GamePlayer.IsAI)
		{
			Vector2 axis = new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
			input = new CharacterMoverInput(axis, false, false);
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
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(this.mover.MovementAxis.x, 0f, this.mover.MovementAxis.y), Vector3.up), this.turnSpeed * Time.deltaTime);
			}
		}
		this.velocity.Value = this.mover.Velocity;
		this.netIsGrounded.Value = (this.curOffMeshLinkTranslationType != OffMeshLinkTranslateType.Parabola && this.mover.Grounded);
		base.DoMovement();
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x00067158 File Offset: 0x00065358
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

	// Token: 0x06000C49 RID: 3145 RVA: 0x00067200 File Offset: 0x00065400
	private CharacterMoverInput GetAIInput()
	{
		float num = 1f;
		Vector2 b = new Vector2(base.transform.position.x, base.transform.position.z);
		if (this.pathUpdateTimer.Elapsed(true) && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
		{
			this.agent.SetDestination(this.targetPosition);
		}
		if ((new Vector2(this.targetPosition.x, this.targetPosition.z) - b).sqrMagnitude <= num || this.targetPosition == Vector3.zero)
		{
			if (this.minigameController.GameState == FinderGameState.HelicopterActive)
			{
				this.targetPosition = new Vector3(0f, 1f, 0f);
			}
			else
			{
				List<FinderBeaconPickup> list = new List<FinderBeaconPickup>();
				for (int i = 0; i < this.minigameController.beacons.Length; i++)
				{
					if (this.minigameController.beacons[i] != null && !this.minigameController.beacons[i].PickedUp)
					{
						list.Add(this.minigameController.beacons[i]);
					}
				}
				if (list.Count > 2)
				{
					this.targetPosition = list[GameManager.rand.Next(0, list.Count)].transform.position;
				}
				else
				{
					this.targetPosition = this.minigameController.GetRandomNavMeshPoint(GameManager.rand);
				}
			}
		}
		Vector3 vector = this.agent.steeringTarget - base.transform.position;
		Vector2 normalized = new Vector2(vector.x, vector.z).normalized;
		CharacterMoverInput result = new CharacterMoverInput(normalized, false, false);
		Debug.DrawLine(base.transform.position, this.targetPosition, Color.red);
		return result;
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x000673EC File Offset: 0x000655EC
	public void KillPlayer(Vector3 dir)
	{
		if (!this.isDead)
		{
			Vector3 a = UnityEngine.Random.onUnitSphere;
			a = dir;
			a.y = 50f;
			a.x *= 100f;
			a.z *= 40f;
			this.playerAnim.SpawnRagdoll(a * 0.05f);
			this.isDead = true;
			this.Deactivate();
			this.minigameController.bloodHandler.ActivateBlood(base.transform.position, (int)base.GamePlayer.GlobalID);
			UnityEngine.Object.Instantiate<GameObject>(this.deathEffect, base.transform.position, Quaternion.LookRotation(Vector3.up));
			AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
			this.cameraShake.AddShake(0.1f);
			if (NetSystem.IsServer)
			{
				this.Score += (short)(this.minigameController.playersDead * 10);
			}
			this.minigameController.PlayerDied(this);
		}
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x0000BB4F File Offset: 0x00009D4F
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCEscape(NetPlayer sender)
	{
		this.OnEscape();
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x000674F8 File Offset: 0x000656F8
	public void OnEscape()
	{
		if (this.escaped)
		{
			return;
		}
		if (base.IsOwner)
		{
			base.SendRPC("RPCEscape", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		if (NetSystem.IsServer)
		{
			this.Score += (short)(GameManager.GetPlayerCount() * 10);
		}
		this.escaped = true;
		this.Deactivate();
		string text = string.Concat(new string[]
		{
			"<color=#",
			ColorUtility.ToHtmlStringRGBA(base.GamePlayer.Color.uiColor),
			"> ",
			base.GamePlayer.Name,
			"</color><color=#4FF051FF> has escaped.</color>"
		});
		GameManager.UIController.ShowLargeText(text, LargeTextType.PlayerWins, 1f, true, true);
		this.minigameController.PlayerEscaped(this);
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x000675BC File Offset: 0x000657BC
	private void OnTriggerEnter(Collider other)
	{
		if (!base.IsOwner)
		{
			return;
		}
		FinderBeaconPickup component = other.GetComponent<FinderBeaconPickup>();
		if (component != null)
		{
			this.minigameController.PickupBeacon(component.beaconID, false);
			return;
		}
		if (other.name == "EscapeTrigger")
		{
			this.OnEscape();
		}
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x0000BB57 File Offset: 0x00009D57
	private void OnTriggerStay(Collider other)
	{
		if (!base.IsOwner)
		{
			return;
		}
		if (other.name == "EscapeTrigger")
		{
			this.OnEscape();
		}
	}

	// Token: 0x04000BA1 RID: 2977
	public float turnSpeed = 1500f;

	// Token: 0x04000BA2 RID: 2978
	public MeshRenderer floatyRenderer;

	// Token: 0x04000BA3 RID: 2979
	public GameObject deathEffect;

	// Token: 0x04000BA4 RID: 2980
	private FinderController minigameController;

	// Token: 0x04000BA5 RID: 2981
	private CharacterMover mover;

	// Token: 0x04000BA6 RID: 2982
	private CameraShake cameraShake;

	// Token: 0x04000BA7 RID: 2983
	private bool sharkAttack;

	// Token: 0x04000BA8 RID: 2984
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04000BA9 RID: 2985
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x04000BAA RID: 2986
	public bool escaped;
}
