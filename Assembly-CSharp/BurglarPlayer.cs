using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000188 RID: 392
public class BurglarPlayer : Movement1
{
	// Token: 0x06000B20 RID: 2848 RVA: 0x0005FB64 File Offset: 0x0005DD64
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
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
			this.n_itemsHeld.Recieve = new RecieveProxy(this.ItemsHeldRecieve);
			this.ItemsHeld = this.n_itemsHeld.Value;
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

	// Token: 0x06000B21 RID: 2849 RVA: 0x0005FC50 File Offset: 0x0005DE50
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

	// Token: 0x06000B22 RID: 2850 RVA: 0x0000B207 File Offset: 0x00009407
	public void Awake()
	{
		this.m_itemManager = UnityEngine.Object.FindObjectOfType<BurglarItemManager>();
		base.InitializeController();
		this.m_aiGoalItems = UnityEngine.Random.Range(1, 3);
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x0005FCA0 File Offset: 0x0005DEA0
	protected override void Start()
	{
		base.Start();
		this.minigameController = (BurglarController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
		if (!this.player.IsAI)
		{
			this.mover.SetForwardVector(Vector3.left);
		}
		this.m_goals = UnityEngine.Object.FindObjectsOfType<BurglarPlayerGoal>();
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x0005FD24 File Offset: 0x0005DF24
	private void Update()
	{
		if (this.m_isSquashed && !this.m_revived && Time.time - this.m_lastSquashTime >= 1f)
		{
			this.playerAnim.FireReviveTrigger();
			this.m_revived = true;
		}
		if (base.IsOwner && this.minigameController.State == MinigameControllerState.Playing)
		{
			if (this.m_isSquashed && Time.time - this.m_lastSquashTime >= 1.5f)
			{
				this.m_isSquashed = false;
			}
			this.m_targetItem = null;
			this.m_closestItem = null;
			List<BurglarItem> spawnedItems = this.m_itemManager.GetSpawnedItems();
			float num = 2f;
			float positiveInfinity = float.PositiveInfinity;
			float num2 = float.PositiveInfinity;
			Debug.DrawLine(base.transform.position, base.transform.position + base.transform.forward, Color.magenta);
			Vector2 from = new Vector2(base.transform.forward.x, base.transform.forward.z);
			foreach (BurglarItem burglarItem in spawnedItems)
			{
				Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.z);
				Vector2 vector2 = new Vector2(burglarItem.transform.position.x, burglarItem.transform.position.z);
				float num3 = Vector2.Distance(vector, vector2);
				if (num3 < num)
				{
					float num4 = Vector2.Angle(from, (vector2 - vector).normalized);
					if (num4 < num2)
					{
						this.m_targetItem = burglarItem;
						num2 = num4;
					}
				}
				if (this.player.IsAI && num3 < positiveInfinity)
				{
					this.m_closestItem = burglarItem;
				}
			}
			if (Time.time - this.m_lastTakeAction > this.m_takeCooldown && this.m_targetItem != null)
			{
				if (this.player.IsAI)
				{
					if (this.curAIState != BurglarAIState.ReturningItems && this.agent.remainingDistance < num * 0.75f && this.ItemsHeld < 3)
					{
						this.TryTakeItem();
						this.m_lastTakeAction = Time.time;
					}
				}
				else if (this.player.RewiredPlayer.GetButtonDown(InputActions.Accept))
				{
					if (this.ItemsHeld < 3)
					{
						this.TryTakeItem();
						this.m_lastTakeAction = Time.time;
					}
					else
					{
						AudioSystem.PlayOneShot(this.errorSound, 0.5f, 0f, 1f);
					}
				}
			}
		}
		base.UpdateController();
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x0000B227 File Offset: 0x00009427
	public void LateUpdate()
	{
		this.spine.localEulerAngles += new Vector3(this.spineOffset, 0f, 0f);
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x0005FFCC File Offset: 0x0005E1CC
	private void TryTakeItem()
	{
		if (this.m_targetItem == null)
		{
			return;
		}
		if (this.minigameController.CanTakeItem(this.m_targetItem))
		{
			this.m_points.Add(this.m_targetItem.PointValue);
			byte itemsHeld = this.ItemsHeld;
			this.ItemsHeld = itemsHeld + 1;
			this.minigameController.TakeItem(this.m_targetItem);
			AudioSystem.PlayOneShot(this.moneyLossClip, base.transform.position, 0.25f, AudioRolloffMode.Linear, 10f, 30f, 0f);
		}
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x00060060 File Offset: 0x0005E260
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		bool val = !this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || this.isDead || this.m_isSquashed;
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
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(this.mover.MovementAxis.x, 0f, this.mover.MovementAxis.y), Vector3.up), 1500f * Time.deltaTime);
			}
		}
		this.velocity.Value = this.mover.Velocity;
		this.netIsGrounded.Value = (this.curOffMeshLinkTranslationType != OffMeshLinkTranslateType.Parabola && this.mover.Grounded);
		base.DoMovement();
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x00060200 File Offset: 0x0005E400
	protected override void UpdateAnimationState(PlayerAnimation playerAnim)
	{
		Vector2 vector = new Vector2(this.velocity.Value.x, this.velocity.Value.z);
		float num = Mathf.Clamp01(vector.magnitude / this.mover.maxSpeed) * this.m_speedModifier;
		playerAnim.Velocity = num;
		playerAnim.VelocityY = this.velocity.Value.y;
		playerAnim.MovementAxis = ((num > 0.01f) ? Vector2.up : Vector2.zero);
		playerAnim.Grounded = this.netIsGrounded.Value;
		playerAnim.SetPlayerRotation(this.rotation.Value);
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x000602B0 File Offset: 0x0005E4B0
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
			if ((int)this.ItemsHeld < this.m_aiGoalItems && (this.curAIState != BurglarAIState.ReturningItems || this.ItemsHeld == 0))
			{
				if (this.ItemsHeld == 0)
				{
					this.curAIState = BurglarAIState.SeekingFirstItem;
				}
				else
				{
					this.curAIState = BurglarAIState.SeekingAdditionalItems;
				}
			}
			if ((int)this.ItemsHeld >= this.m_aiGoalItems)
			{
				this.curAIState = BurglarAIState.ReturningItems;
			}
			switch (this.curAIState)
			{
			case BurglarAIState.SeekingFirstItem:
				if (this.m_firstItem == null)
				{
					List<BurglarItem> spawnedItems = this.m_itemManager.GetSpawnedItems();
					if (spawnedItems.Count > 0)
					{
						this.m_firstItem = spawnedItems[UnityEngine.Random.Range(0, spawnedItems.Count - 1)];
					}
				}
				if (this.m_firstItem != null)
				{
					this.targetPosition = this.m_firstItem.transform.position;
				}
				break;
			case BurglarAIState.SeekingAdditionalItems:
				if (this.m_closestItem != null)
				{
					this.targetPosition = this.m_closestItem.transform.position;
				}
				break;
			case BurglarAIState.ReturningItems:
				if (this.m_goals != null && this.returnUpdateTimer.Elapsed(true))
				{
					this.m_closestGoal = null;
					float num2 = float.PositiveInfinity;
					foreach (BurglarPlayerGoal burglarPlayerGoal in this.m_goals)
					{
						if (!(base.transform == null) && !(burglarPlayerGoal == null) && !(burglarPlayerGoal.AITarget == null))
						{
							float num3 = Vector3.Distance(base.transform.position, burglarPlayerGoal.AITarget.position);
							if (num3 < num2)
							{
								this.m_closestGoal = burglarPlayerGoal;
								num2 = num3;
							}
						}
					}
				}
				if (this.m_closestGoal != null)
				{
					this.targetPosition = this.m_closestGoal.AITarget.position;
				}
				break;
			}
			if (this.pathUpdateTimer.Elapsed(true) && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
			{
				this.agent.SetDestination(this.targetPosition);
			}
			Debug.DrawLine(base.transform.position, this.targetPosition, Color.green);
			int mask = LayerMask.GetMask(new string[]
			{
				"MinigameUtil1"
			});
			bool flag = false;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit, 2f, mask, QueryTriggerInteraction.Collide))
			{
				flag = true;
			}
			if ((new Vector2(this.targetPosition.x, this.targetPosition.z) - b).sqrMagnitude > num && !flag)
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

	// Token: 0x06000B2A RID: 2858 RVA: 0x0003203C File Offset: 0x0003023C
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

	// Token: 0x06000B2B RID: 2859 RVA: 0x0000B254 File Offset: 0x00009454
	private void SetCurAIState(BurglarAIState newState)
	{
		if (!base.GamePlayer.IsLocalPlayer)
		{
			return;
		}
		this.curAIState = newState;
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x00060630 File Offset: 0x0005E830
	private void OnTriggerEnter(Collider other)
	{
		if (!base.IsOwner)
		{
			return;
		}
		if (other.gameObject.CompareTag("MinigameCustom01") && !this.m_isSquashed && Time.time - this.m_lastSquashTime > 2f)
		{
			this.Squash();
		}
		if (other.GetComponentInParent<BurglarPlayerGoal>() != null)
		{
			this.curAIState = BurglarAIState.Idle;
			if (this.ItemsHeld > 0)
			{
				int num = 0;
				foreach (int num2 in this.m_points)
				{
					num += num2;
				}
				this.IncreaseScore((short)num);
				this.m_points.Clear();
				this.ItemsHeld = 0;
			}
		}
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x000606F8 File Offset: 0x0005E8F8
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

	// Token: 0x06000B2E RID: 2862 RVA: 0x00060750 File Offset: 0x0005E950
	private void IncreaseScore(short score)
	{
		this.IncreaseScoreEffects();
		if (NetSystem.IsServer)
		{
			this.Score += score;
			return;
		}
		if (base.IsOwner)
		{
			base.SendRPC("RPCIncreaseScore", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				score
			});
		}
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x0000B26B File Offset: 0x0000946B
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCIncreaseScore(NetPlayer sender, short score)
	{
		this.IncreaseScore(score);
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x0000B274 File Offset: 0x00009474
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x000607A0 File Offset: 0x0005E9A0
	private void MoneyLossEffect()
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCMoneyLossEffect", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		AudioSystem.PlayOneShot(this.moneyLossClip, base.transform.position, 0.5f, AudioRolloffMode.Linear, 10f, 30f, 0f);
		UnityEngine.Object.Instantiate<GameObject>(this.moneyLossFX, base.transform.position, Quaternion.LookRotation(Vector3.up));
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x0000B27C File Offset: 0x0000947C
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCMoneyLossEffect(NetPlayer sender)
	{
		this.MoneyLossEffect();
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x00060814 File Offset: 0x0005EA14
	private void IncreaseScoreEffects()
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCIncreaseScoreEffects", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		AudioSystem.PlayOneShot(this.increaseScoreClip, base.transform.position, 0.5f, AudioRolloffMode.Linear, 10f, 30f, 0f);
		AudioSystem.PlayOneShot(this.moneyLossClip, base.transform.position, 0.5f, AudioRolloffMode.Linear, 10f, 30f, 0f);
		UnityEngine.Object.Instantiate<GameObject>(this.moneyLossFX, base.transform.position, Quaternion.LookRotation(Vector3.up));
	}

	// Token: 0x06000B35 RID: 2869 RVA: 0x0000B284 File Offset: 0x00009484
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCIncreaseScoreEffects(NetPlayer sender)
	{
		this.IncreaseScoreEffects();
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x0000B28C File Offset: 0x0000948C
	public void ItemsHeldRecieve(object val)
	{
		this.ItemsHeld = (byte)val;
	}

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000B37 RID: 2871 RVA: 0x0000B29A File Offset: 0x0000949A
	// (set) Token: 0x06000B38 RID: 2872 RVA: 0x000608B4 File Offset: 0x0005EAB4
	public byte ItemsHeld
	{
		get
		{
			return this.n_itemsHeld.Value;
		}
		set
		{
			this.n_itemsHeld.Value = value;
			for (int i = 0; i < this.carryBags.Length; i++)
			{
				this.carryBags[i].SetActive((int)(value - 1) == i);
			}
			if (value == 0)
			{
				this.spineOffset = 0f;
			}
			else if ((int)(value - 1) < this.spineCarryOffset.Length)
			{
				this.spineOffset = this.spineCarryOffset[(int)(value - 1)];
			}
			this.playerAnim.Carrying = (value > 0);
			if (value == 0)
			{
				this.mover.maxSpeed = this.base_speed;
				this.m_speedModifier = 1f;
				return;
			}
			if ((int)(value - 1) < this.carrySpeeds.Length)
			{
				this.mover.maxSpeed = this.base_speed * this.carrySpeeds[(int)(value - 1)];
				this.m_speedModifier = this.carrySpeeds[(int)(value - 1)];
			}
		}
	}

	// Token: 0x06000B39 RID: 2873 RVA: 0x00060988 File Offset: 0x0005EB88
	public void Squash()
	{
		this.m_lastSquashTime = Time.time;
		this.m_revived = false;
		this.playerAnim.FireDeathTrigger();
		AudioSystem.PlayOneShot(this.crashSound, base.transform.position, 1f, AudioRolloffMode.Linear, 10f, 30f, 0f);
		if (Settings.BloodEffects)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.bloodEffect, base.transform.position, Quaternion.LookRotation(Vector3.up));
		}
		if (base.IsOwner)
		{
			if (this.ItemsHeld >= 1)
			{
				this.MoneyLossEffect();
				this.ItemsHeld -= 1;
				if (this.m_points.Count > 0)
				{
					int num = 0;
					int index = 0;
					for (int i = 0; i < this.m_points.Count; i++)
					{
						if (this.m_points[i] > num)
						{
							num = this.m_points[i];
							index = i;
						}
					}
					this.m_points.RemoveAt(index);
				}
			}
			base.SendRPC("RPCSquash", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		this.m_isSquashed = true;
	}

	// Token: 0x06000B3A RID: 2874 RVA: 0x0000B2A7 File Offset: 0x000094A7
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCSquash(NetPlayer sender)
	{
		this.Squash();
	}

	// Token: 0x04000A32 RID: 2610
	public float base_speed = 6f;

	// Token: 0x04000A33 RID: 2611
	public AudioClip errorSound;

	// Token: 0x04000A34 RID: 2612
	public AudioClip crashSound;

	// Token: 0x04000A35 RID: 2613
	public AudioClip increaseScoreClip;

	// Token: 0x04000A36 RID: 2614
	public GameObject bloodEffect;

	// Token: 0x04000A37 RID: 2615
	public Transform spine;

	// Token: 0x04000A38 RID: 2616
	public float[] spineCarryOffset;

	// Token: 0x04000A39 RID: 2617
	public GameObject[] carryBags;

	// Token: 0x04000A3A RID: 2618
	public float[] carrySpeeds;

	// Token: 0x04000A3B RID: 2619
	public Transform squashTransform;

	// Token: 0x04000A3C RID: 2620
	public AudioClip moneyLossClip;

	// Token: 0x04000A3D RID: 2621
	public GameObject moneyLossFX;

	// Token: 0x04000A3E RID: 2622
	private BurglarController minigameController;

	// Token: 0x04000A3F RID: 2623
	private CharacterMover mover;

	// Token: 0x04000A40 RID: 2624
	private CameraShake cameraShake;

	// Token: 0x04000A41 RID: 2625
	private BurglarAIState curAIState = BurglarAIState.SeekingFirstItem;

	// Token: 0x04000A42 RID: 2626
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04000A43 RID: 2627
	private ActionTimer returnUpdateTimer = new ActionTimer(0.25f, 0.25f);

	// Token: 0x04000A44 RID: 2628
	private ActionTimer hidePositionTimer = new ActionTimer(0.25f, 0.55f);

	// Token: 0x04000A45 RID: 2629
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x04000A46 RID: 2630
	private BurglarItemManager m_itemManager;

	// Token: 0x04000A47 RID: 2631
	private float spineOffset;

	// Token: 0x04000A48 RID: 2632
	private float m_speedModifier = 1f;

	// Token: 0x04000A49 RID: 2633
	private List<int> m_points = new List<int>();

	// Token: 0x04000A4A RID: 2634
	private int m_aiGoalItems = 1;

	// Token: 0x04000A4B RID: 2635
	private BurglarPlayerGoal[] m_goals;

	// Token: 0x04000A4C RID: 2636
	private BurglarPlayerGoal m_closestGoal;

	// Token: 0x04000A4D RID: 2637
	private float m_lastTakeAction;

	// Token: 0x04000A4E RID: 2638
	private float m_takeCooldown = 0.1f;

	// Token: 0x04000A4F RID: 2639
	private BurglarItem m_targetItem;

	// Token: 0x04000A50 RID: 2640
	private BurglarItem m_closestItem;

	// Token: 0x04000A51 RID: 2641
	private BurglarItem m_firstItem;

	// Token: 0x04000A52 RID: 2642
	private int[] pointsToCheck = new int[]
	{
		3,
		6,
		18
	};

	// Token: 0x04000A53 RID: 2643
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<byte> n_itemsHeld = new NetVar<byte>(0);

	// Token: 0x04000A54 RID: 2644
	private float m_lastSquashTime;

	// Token: 0x04000A55 RID: 2645
	private bool m_revived;

	// Token: 0x04000A56 RID: 2646
	private bool m_isSquashed;
}
