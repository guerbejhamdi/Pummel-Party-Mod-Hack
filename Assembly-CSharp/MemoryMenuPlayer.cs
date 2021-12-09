using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001D4 RID: 468
public class MemoryMenuPlayer : Movement1
{
	// Token: 0x06000D82 RID: 3458 RVA: 0x0006DA1C File Offset: 0x0006BC1C
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

	// Token: 0x06000D83 RID: 3459 RVA: 0x0006DB08 File Offset: 0x0006BD08
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

	// Token: 0x06000D84 RID: 3460 RVA: 0x0000C463 File Offset: 0x0000A663
	public void Awake()
	{
		this.m_itemManager = UnityEngine.Object.FindObjectOfType<MemoryMenuItemManager>();
		base.InitializeController();
	}

	// Token: 0x06000D85 RID: 3461 RVA: 0x0006DB58 File Offset: 0x0006BD58
	protected override void Start()
	{
		base.Start();
		this.minigameController = (MemoryMenuController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
		if (!this.player.IsAI)
		{
			this.mover.SetForwardVector(Vector3.left);
		}
		this.m_goals = UnityEngine.Object.FindObjectsOfType<BurglarPlayerGoal>();
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x0006DBDC File Offset: 0x0006BDDC
	private void Update()
	{
		if (this.m_isSquashed && !this.m_revived && Time.time - this.m_lastSquashTime >= 1f)
		{
			this.playerAnim.FireReviveTrigger();
			this.m_revived = true;
		}
		if (base.IsOwner && this.minigameController.State == MinigameControllerState.Playing && this.minigameController.ExplanationDone)
		{
			if (this.m_isSquashed && Time.time - this.m_lastSquashTime >= 1.5f)
			{
				this.m_isSquashed = false;
			}
			this.m_targetItem = null;
			this.m_closestItem = null;
			List<MemoryMenuItem> spawnedItems = this.m_itemManager.GetSpawnedItems();
			float num = 2f;
			float positiveInfinity = float.PositiveInfinity;
			float num2 = float.MaxValue;
			Debug.DrawLine(base.transform.position, base.transform.position + base.transform.forward, Color.magenta);
			new Vector2(base.transform.forward.x, base.transform.forward.z);
			foreach (MemoryMenuItem memoryMenuItem in spawnedItems)
			{
				Vector2 a = new Vector2(base.transform.position.x, base.transform.position.z);
				Vector2 b = new Vector2(memoryMenuItem.transform.position.x, memoryMenuItem.transform.position.z);
				float num3 = Vector2.Distance(a, b);
				if (num3 < num && num3 < num2)
				{
					this.m_targetItem = memoryMenuItem;
					num2 = num3;
				}
				if (this.player.IsAI && num3 < positiveInfinity)
				{
					this.m_closestItem = memoryMenuItem;
				}
			}
			if (Time.time - this.m_lastTakeAction > this.m_takeCooldown && this.m_targetItem != null)
			{
				if (this.player.IsAI)
				{
					if (this.curAIState != MemoryMenuAIState.ReturningItems && this.agent.remainingDistance < num * 0.3f && this.m_firstItem != null && this.ItemsHeld < 4)
					{
						this.TryTakeItem();
						this.m_lastTakeAction = Time.time;
					}
				}
				else if (this.player.RewiredPlayer.GetButtonDown(InputActions.Accept))
				{
					if (this.ItemsHeld < 4)
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

	// Token: 0x06000D87 RID: 3463 RVA: 0x0000C476 File Offset: 0x0000A676
	public void LateUpdate()
	{
		this.spine.localEulerAngles += new Vector3(this.spineOffset, 0f, 0f);
	}

	// Token: 0x06000D88 RID: 3464 RVA: 0x0006DE88 File Offset: 0x0006C088
	private void TryTakeItem()
	{
		if (this.m_targetItem == null)
		{
			return;
		}
		if (this.minigameController.CanTakeItem(this.m_targetItem))
		{
			this.m_firstItem = null;
			this.m_foodIDs.Add(this.m_targetItem.ItemTypeID);
			byte itemsHeld = this.ItemsHeld;
			this.ItemsHeld = itemsHeld + 1;
			this.minigameController.TakeItem(this.m_targetItem);
			AudioSystem.PlayOneShot(this.moneyLossClip, base.transform.position, 0.25f, AudioRolloffMode.Linear, 10f, 30f, 0f);
		}
	}

	// Token: 0x06000D89 RID: 3465 RVA: 0x0006DF24 File Offset: 0x0006C124
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		bool val = this.hasFinished || !this.minigameController.Playable || !this.minigameController.ExplanationDone || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || this.isDead || this.m_isSquashed;
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

	// Token: 0x06000D8A RID: 3466 RVA: 0x0006E0DC File Offset: 0x0006C2DC
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

	// Token: 0x06000D8B RID: 3467 RVA: 0x0006E18C File Offset: 0x0006C38C
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		if (!this.minigameController.Playable || !this.minigameController.ExplanationDone)
		{
			result.NullInput();
			return result;
		}
		this.m_aiGoalItems = this.minigameController.TargetFoods.Count;
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
			if ((int)this.ItemsHeld < this.m_aiGoalItems && (this.curAIState != MemoryMenuAIState.ReturningItems || this.ItemsHeld == 0))
			{
				this.curAIState = MemoryMenuAIState.SeekingItem;
			}
			if ((int)this.ItemsHeld >= this.m_aiGoalItems)
			{
				this.curAIState = MemoryMenuAIState.ReturningItems;
			}
			MemoryMenuAIState memoryMenuAIState = this.curAIState;
			if (memoryMenuAIState != MemoryMenuAIState.SeekingItem)
			{
				if (memoryMenuAIState == MemoryMenuAIState.ReturningItems)
				{
					this.targetPosition = new Vector3(9.5f, 0.8f, 0f);
				}
			}
			else
			{
				if (this.m_firstItem == null)
				{
					List<MemoryMenuItem> spawnedItems = this.m_itemManager.GetSpawnedItems();
					List<int> list = new List<int>();
					for (int i = 0; i < spawnedItems.Count; i++)
					{
						list.Add(i);
					}
					if (spawnedItems.Count > 0)
					{
						this.targets.Clear();
						for (int j = 0; j < this.minigameController.TargetFoods.Count; j++)
						{
							if (!this.m_foodIDs.Contains((int)this.minigameController.TargetFoods[j]))
							{
								this.targets.Add(this.minigameController.TargetFoods[j]);
							}
						}
						if (this.targets.Count > 0)
						{
							short num2 = this.targets[GameManager.rand.Next(0, this.targets.Count)];
							while (list.Count > 0)
							{
								int index = GameManager.rand.Next(0, list.Count);
								if (spawnedItems[list[index]].ItemTypeID == (int)num2 || GameManager.rand.NextDouble() > 0.01)
								{
									this.m_firstItem = spawnedItems[list[index]];
									this.targetID = this.m_firstItem.ItemID;
									break;
								}
								list.RemoveAt(index);
							}
						}
					}
				}
				else if (this.m_firstItem.ItemID != this.targetID)
				{
					this.m_firstItem = null;
				}
				if (this.m_firstItem != null)
				{
					this.targetPosition = this.m_firstItem.transform.position;
				}
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

	// Token: 0x06000D8C RID: 3468 RVA: 0x0003203C File Offset: 0x0003023C
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

	// Token: 0x06000D8D RID: 3469 RVA: 0x0000C4A3 File Offset: 0x0000A6A3
	private void SetCurAIState(MemoryMenuAIState newState)
	{
		if (!base.GamePlayer.IsLocalPlayer)
		{
			return;
		}
		this.curAIState = newState;
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x0006E594 File Offset: 0x0006C794
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
		if (other.GetComponentInParent<MemoryMenuPlayerGoal>() != null)
		{
			this.curAIState = MemoryMenuAIState.Idle;
			if (this.ItemsHeld > 0)
			{
				List<short> list = new List<short>(this.minigameController.TargetFoods);
				int num = 0;
				for (int i = this.m_foodIDs.Count - 1; i >= 0; i--)
				{
					for (int j = list.Count - 1; j >= 0; j--)
					{
						if (this.m_foodIDs[i] == (int)list[j])
						{
							this.m_foodIDs.RemoveAt(i);
							list.RemoveAt(j);
							num++;
							break;
						}
					}
				}
				int count = this.m_foodIDs.Count;
				if (list.Count == 0 && count <= 0)
				{
					this.IncreaseScore((short)(num - count));
					this.hasFinished = true;
					this.SetSuccess(true);
				}
				else
				{
					this.SetSuccess(false);
				}
				this.m_foodIDs.Clear();
				this.ItemsHeld = 0;
			}
		}
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x0006E6C0 File Offset: 0x0006C8C0
	public override void ResetPlayer()
	{
		this.playerAnim.SetPlayerRotationImmediate(this.startRotation.eulerAngles.y);
		if (base.IsOwner)
		{
			this.mover.Velocity = Vector3.zero;
			this.velocity.Value = Vector3.zero;
			this.m_foodIDs.Clear();
			this.ItemsHeld = 0;
			this.curAIState = MemoryMenuAIState.SeekingItem;
			this.m_firstItem = null;
		}
		this.hasFinished = false;
		this.successParticle.SetActive(false);
		this.failParticle.SetActive(false);
		base.ResetPlayer();
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x0006E758 File Offset: 0x0006C958
	private void SetSuccess(bool success)
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCSetSuccess", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				success
			});
		}
		this.successParticle.SetActive(success);
		this.failParticle.SetActive(!success);
		if (!success)
		{
			AudioSystem.PlayOneShot(this.failureClip, 1f, 0f, 1f);
		}
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x0000C4BA File Offset: 0x0000A6BA
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCSetSuccess(NetPlayer sender, bool success)
	{
		this.SetSuccess(success);
	}

	// Token: 0x06000D92 RID: 3474 RVA: 0x0006E7C0 File Offset: 0x0006C9C0
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

	// Token: 0x06000D93 RID: 3475 RVA: 0x0000C4C3 File Offset: 0x0000A6C3
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCIncreaseScore(NetPlayer sender, short score)
	{
		this.IncreaseScore(score);
	}

	// Token: 0x06000D94 RID: 3476 RVA: 0x0000C4CC File Offset: 0x0000A6CC
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x06000D95 RID: 3477 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x06000D96 RID: 3478 RVA: 0x0006E810 File Offset: 0x0006CA10
	private void MoneyLossEffect()
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCMoneyLossEffect", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		AudioSystem.PlayOneShot(this.moneyLossClip, base.transform.position, 0.5f, AudioRolloffMode.Linear, 10f, 30f, 0f);
		UnityEngine.Object.Instantiate<GameObject>(this.moneyLossFX, base.transform.position, Quaternion.LookRotation(Vector3.up));
	}

	// Token: 0x06000D97 RID: 3479 RVA: 0x0000C4D4 File Offset: 0x0000A6D4
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCMoneyLossEffect(NetPlayer sender)
	{
		this.MoneyLossEffect();
	}

	// Token: 0x06000D98 RID: 3480 RVA: 0x0006E884 File Offset: 0x0006CA84
	private void IncreaseScoreEffects()
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCIncreaseScoreEffects", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		AudioSystem.PlayOneShot(this.increaseScoreClip, base.transform.position, 0.5f, AudioRolloffMode.Linear, 10f, 30f, 0f);
		AudioSystem.PlayOneShot(this.moneyLossClip, base.transform.position, 0.5f, AudioRolloffMode.Linear, 10f, 30f, 0f);
	}

	// Token: 0x06000D99 RID: 3481 RVA: 0x0000C4DC File Offset: 0x0000A6DC
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCIncreaseScoreEffects(NetPlayer sender)
	{
		this.IncreaseScoreEffects();
	}

	// Token: 0x06000D9A RID: 3482 RVA: 0x0000C4E4 File Offset: 0x0000A6E4
	public void ItemsHeldRecieve(object val)
	{
		this.ItemsHeld = (byte)val;
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000D9B RID: 3483 RVA: 0x0000C4F2 File Offset: 0x0000A6F2
	// (set) Token: 0x06000D9C RID: 3484 RVA: 0x0006E900 File Offset: 0x0006CB00
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
				this.carryBags[i].SetActive(false);
			}
			for (int j = 0; j < this.carryBags.Length; j++)
			{
				if ((int)(value - 1) == j)
				{
					this.carryBags[j].SetActive(true);
				}
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

	// Token: 0x06000D9D RID: 3485 RVA: 0x0006E9F8 File Offset: 0x0006CBF8
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
				if (this.m_foodIDs.Count > 0)
				{
					int num = 0;
					int index = 0;
					for (int i = 0; i < this.m_foodIDs.Count; i++)
					{
						if (this.m_foodIDs[i] > num)
						{
							num = this.m_foodIDs[i];
							index = i;
						}
					}
					this.m_foodIDs.RemoveAt(index);
				}
			}
			base.SendRPC("RPCSquash", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		this.m_isSquashed = true;
	}

	// Token: 0x06000D9E RID: 3486 RVA: 0x0000C4FF File Offset: 0x0000A6FF
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCSquash(NetPlayer sender)
	{
		this.Squash();
	}

	// Token: 0x04000CD9 RID: 3289
	public float base_speed = 6f;

	// Token: 0x04000CDA RID: 3290
	public AudioClip errorSound;

	// Token: 0x04000CDB RID: 3291
	public AudioClip crashSound;

	// Token: 0x04000CDC RID: 3292
	public AudioClip increaseScoreClip;

	// Token: 0x04000CDD RID: 3293
	public AudioClip failureClip;

	// Token: 0x04000CDE RID: 3294
	public GameObject bloodEffect;

	// Token: 0x04000CDF RID: 3295
	public Transform spine;

	// Token: 0x04000CE0 RID: 3296
	public float[] spineCarryOffset;

	// Token: 0x04000CE1 RID: 3297
	public GameObject[] carryBags;

	// Token: 0x04000CE2 RID: 3298
	public float[] carrySpeeds;

	// Token: 0x04000CE3 RID: 3299
	public Transform squashTransform;

	// Token: 0x04000CE4 RID: 3300
	public AudioClip moneyLossClip;

	// Token: 0x04000CE5 RID: 3301
	public GameObject moneyLossFX;

	// Token: 0x04000CE6 RID: 3302
	public GameObject successParticle;

	// Token: 0x04000CE7 RID: 3303
	public GameObject failParticle;

	// Token: 0x04000CE8 RID: 3304
	private MemoryMenuController minigameController;

	// Token: 0x04000CE9 RID: 3305
	private CharacterMover mover;

	// Token: 0x04000CEA RID: 3306
	private CameraShake cameraShake;

	// Token: 0x04000CEB RID: 3307
	private MemoryMenuAIState curAIState = MemoryMenuAIState.SeekingItem;

	// Token: 0x04000CEC RID: 3308
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x04000CED RID: 3309
	private ActionTimer returnUpdateTimer = new ActionTimer(0.25f, 0.25f);

	// Token: 0x04000CEE RID: 3310
	private ActionTimer hidePositionTimer = new ActionTimer(0.25f, 0.55f);

	// Token: 0x04000CEF RID: 3311
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x04000CF0 RID: 3312
	private MemoryMenuItemManager m_itemManager;

	// Token: 0x04000CF1 RID: 3313
	private float spineOffset;

	// Token: 0x04000CF2 RID: 3314
	private float m_speedModifier = 1f;

	// Token: 0x04000CF3 RID: 3315
	private List<int> m_foodIDs = new List<int>();

	// Token: 0x04000CF4 RID: 3316
	private int m_aiGoalItems = 1;

	// Token: 0x04000CF5 RID: 3317
	private bool hasFinished;

	// Token: 0x04000CF6 RID: 3318
	private BurglarPlayerGoal[] m_goals;

	// Token: 0x04000CF7 RID: 3319
	private BurglarPlayerGoal m_closestGoal;

	// Token: 0x04000CF8 RID: 3320
	private float m_lastTakeAction;

	// Token: 0x04000CF9 RID: 3321
	private float m_takeCooldown = 0.1f;

	// Token: 0x04000CFA RID: 3322
	private MemoryMenuItem m_targetItem;

	// Token: 0x04000CFB RID: 3323
	private MemoryMenuItem m_closestItem;

	// Token: 0x04000CFC RID: 3324
	private MemoryMenuItem m_firstItem;

	// Token: 0x04000CFD RID: 3325
	private List<short> targets = new List<short>();

	// Token: 0x04000CFE RID: 3326
	private int targetID;

	// Token: 0x04000CFF RID: 3327
	private int[] pointsToCheck = new int[]
	{
		3,
		6,
		18
	};

	// Token: 0x04000D00 RID: 3328
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<byte> n_itemsHeld = new NetVar<byte>(0);

	// Token: 0x04000D01 RID: 3329
	private float m_lastSquashTime;

	// Token: 0x04000D02 RID: 3330
	private bool m_revived;

	// Token: 0x04000D03 RID: 3331
	private bool m_isSquashed;
}
