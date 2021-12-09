using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x020003C9 RID: 969
public class BoardPlayer : BoardActor
{
	// Token: 0x170002AE RID: 686
	// (get) Token: 0x0600198D RID: 6541 RVA: 0x00012F44 File Offset: 0x00011144
	// (set) Token: 0x0600198E RID: 6542 RVA: 0x00012F4C File Offset: 0x0001114C
	public bool GroundSnap { get; set; }

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x0600198F RID: 6543 RVA: 0x00012F55 File Offset: 0x00011155
	// (set) Token: 0x06001990 RID: 6544 RVA: 0x00012F5D File Offset: 0x0001115D
	public bool TwitchMapEvent { get; set; }

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06001991 RID: 6545 RVA: 0x00012F66 File Offset: 0x00011166
	// (set) Token: 0x06001992 RID: 6546 RVA: 0x00012F6E File Offset: 0x0001116E
	public short[] ObtainedInventory
	{
		get
		{
			return this.obtainedInventory;
		}
		set
		{
			this.obtainedInventory = value;
		}
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06001993 RID: 6547 RVA: 0x00012F77 File Offset: 0x00011177
	// (set) Token: 0x06001994 RID: 6548 RVA: 0x00012F7F File Offset: 0x0001117F
	public int CurScorePosition { get; set; }

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06001995 RID: 6549 RVA: 0x00012F88 File Offset: 0x00011188
	// (set) Token: 0x06001996 RID: 6550 RVA: 0x00012F90 File Offset: 0x00011190
	public GameObject NewestRagdoll { get; set; }

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06001997 RID: 6551 RVA: 0x00012F99 File Offset: 0x00011199
	public int MoveStepsRemaining
	{
		get
		{
			return this.curMoveSteps;
		}
	}

	// Token: 0x06001998 RID: 6552 RVA: 0x00012FA1 File Offset: 0x000111A1
	public byte GetItemCount(byte itemID)
	{
		return this.inventory[(int)itemID];
	}

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06001999 RID: 6553 RVA: 0x00012FAF File Offset: 0x000111AF
	public int Gold
	{
		get
		{
			return (int)this.gold.Value;
		}
	}

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x0600199A RID: 6554 RVA: 0x00012FBC File Offset: 0x000111BC
	public int GoalScore
	{
		get
		{
			return (int)this.goalScore.Value;
		}
	}

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x0600199B RID: 6555 RVA: 0x00012FC9 File Offset: 0x000111C9
	// (set) Token: 0x0600199C RID: 6556 RVA: 0x00012FD1 File Offset: 0x000111D1
	public int TurnOrderRoll
	{
		get
		{
			return this.turnOrderRoll;
		}
		set
		{
			this.turnOrderRoll = value;
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x0600199D RID: 6557 RVA: 0x00012FDA File Offset: 0x000111DA
	// (set) Token: 0x0600199E RID: 6558 RVA: 0x00012FE2 File Offset: 0x000111E2
	public bool IsRollingDice
	{
		get
		{
			return this.isRollingDice;
		}
		set
		{
			this.isRollingDice = value;
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x0600199F RID: 6559 RVA: 0x00012FEB File Offset: 0x000111EB
	// (set) Token: 0x060019A0 RID: 6560 RVA: 0x00012FF3 File Offset: 0x000111F3
	public Vector3 MovementTangent
	{
		get
		{
			return this.tangent;
		}
		set
		{
			this.tangent = value;
		}
	}

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x060019A1 RID: 6561 RVA: 0x00012FFC File Offset: 0x000111FC
	public Item EquippedItem
	{
		get
		{
			return this.curEquippedItem;
		}
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x060019A2 RID: 6562 RVA: 0x00013004 File Offset: 0x00011204
	// (set) Token: 0x060019A3 RID: 6563 RVA: 0x0001300C File Offset: 0x0001120C
	public GamePlayer GamePlayer
	{
		get
		{
			return this.player;
		}
		set
		{
			this.player = value;
		}
	}

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x060019A4 RID: 6564 RVA: 0x00013015 File Offset: 0x00011215
	public PlayerAnimation PlayerAnimation
	{
		get
		{
			return this.playerAnim;
		}
	}

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x060019A5 RID: 6565 RVA: 0x0001301D File Offset: 0x0001121D
	public List<BoardNode> NodeChoices
	{
		get
		{
			return this.forwardNodes;
		}
	}

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x060019A6 RID: 6566 RVA: 0x00013025 File Offset: 0x00011225
	// (set) Token: 0x060019A7 RID: 6567 RVA: 0x0001302D File Offset: 0x0001122D
	public BoardPlayerState PlayerState
	{
		get
		{
			return this.curState;
		}
		set
		{
			this.SetState(value);
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x060019A8 RID: 6568 RVA: 0x00013036 File Offset: 0x00011236
	// (set) Token: 0x060019A9 RID: 6569 RVA: 0x0001303E File Offset: 0x0001123E
	public BoardNode CurrentNode
	{
		get
		{
			return this.curNode;
		}
		set
		{
			if (this.curNode != null)
			{
				this.curNode.LeaveNode(this);
			}
			this.curNode = value;
			if (this.curNode != null)
			{
				this.curNode.EnterNode(this);
			}
		}
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x060019AA RID: 6570 RVA: 0x0001307B File Offset: 0x0001127B
	public ActionTimer AIDelayTimer
	{
		get
		{
			return this.AiDelayTimer;
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x060019AB RID: 6571 RVA: 0x00013083 File Offset: 0x00011283
	public bool HasUsedItem
	{
		get
		{
			return this.hasUsedItem;
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x060019AC RID: 6572 RVA: 0x0001308B File Offset: 0x0001128B
	// (set) Token: 0x060019AD RID: 6573 RVA: 0x00013093 File Offset: 0x00011293
	public bool ItemsDisabled
	{
		get
		{
			return this.itemsDisabled;
		}
		set
		{
			this.itemsDisabled = value;
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x060019AE RID: 6574 RVA: 0x0001309C File Offset: 0x0001129C
	public new Vector3 MidPoint
	{
		get
		{
			return base.transform.position + Vector3.up * 0.875f;
		}
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x060019AF RID: 6575 RVA: 0x000130BD File Offset: 0x000112BD
	// (set) Token: 0x060019B0 RID: 6576 RVA: 0x000130C5 File Offset: 0x000112C5
	public byte TurnSkip { get; set; }

	// Token: 0x060019B1 RID: 6577 RVA: 0x000130CE File Offset: 0x000112CE
	public bool GetGoalStatus(int i)
	{
		return this.goals[i];
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x060019B2 RID: 6578 RVA: 0x000130DC File Offset: 0x000112DC
	// (set) Token: 0x060019B3 RID: 6579 RVA: 0x000130E4 File Offset: 0x000112E4
	public TacticalCactusVisual CactusScript { get; set; }

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x060019B4 RID: 6580 RVA: 0x000130ED File Offset: 0x000112ED
	// (set) Token: 0x060019B5 RID: 6581 RVA: 0x000130F5 File Offset: 0x000112F5
	public PresentAnimController PresentScript { get; set; }

	// Token: 0x060019B6 RID: 6582 RVA: 0x000AAC1C File Offset: 0x000A8E1C
	public void EquipCactus()
	{
		this.Visible = false;
		this.CactusScript = UnityEngine.Object.Instantiate<GameObject>(this.cactusPrefab, base.transform).GetComponent<TacticalCactusVisual>();
		this.CactusScript.transform.localPosition = Vector3.zero;
		this.CactusScript.transform.localRotation = Quaternion.identity;
		this.CactusScript.player = this;
	}

	// Token: 0x060019B7 RID: 6583 RVA: 0x000130FE File Offset: 0x000112FE
	public override void OnNetInitialize()
	{
		if (!NetSystem.IsServer)
		{
			this.Initialize(GameManager.GetPlayerWithID(this.playerOwnerID.Value));
		}
		this.GroundSnap = true;
		base.OnNetInitialize();
	}

	// Token: 0x060019B8 RID: 6584 RVA: 0x0001312A File Offset: 0x0001132A
	public void Awake()
	{
		this.playerAnim = base.GetComponentInChildren<PlayerAnimation>();
		this.diceEffectPrefab = Resources.Load<GameObject>("Prefabs/DiceGraphic");
		this.objectRecievePrefab = Resources.Load<GameObject>("Prefabs/BoardEvents/ObjectRecievePrefab");
		this.trophyRecievePrefab = Resources.Load<GameObject>("Prefabs/BoardEvents/TrophyRecievePrefab");
	}

	// Token: 0x060019B9 RID: 6585 RVA: 0x000AAC84 File Offset: 0x000A8E84
	public void OnDisable()
	{
		if (this.curState == BoardPlayerState.Ragdolling)
		{
			base.StopCoroutine(this.startRagdoll);
			base.transform.position = this.CurrentNode.GetPlayersSlotPosition(this) + Vector3.up * this.height;
			base.transform.position = this.GetGroundPosition();
			this.Visible = true;
			this.SetState(BoardPlayerState.Idle);
		}
	}

	// Token: 0x060019BA RID: 6586 RVA: 0x000AACF4 File Offset: 0x000A8EF4
	public void Start()
	{
		if (GameManager.Board.isHalloweenMap)
		{
			this.playerAnim.footStepEffects[0] = this.haloweenSplashStepEffect;
		}
		else if (GameManager.Board.isWinterMap)
		{
			this.playerAnim.footStepEffects[0] = this.winterStepEffect;
		}
		this.playerAnim.SetPlayer(this.player);
		this.UpdateRendererList();
		this.playerAnim.RegisterListener(new AnimationEventListener(this.PlayerJumpPeek), AnimationEventType.JumpPeek);
		this.playerAnim.RegisterListener(new AnimationEventListener(this.OnDeathAnimationFinished), AnimationEventType.DeathFinish);
	}

	// Token: 0x060019BB RID: 6587 RVA: 0x000AAD88 File Offset: 0x000A8F88
	public void Initialize(GamePlayer player)
	{
		this.uiController = GameManager.UIController;
		this.player = player;
		if (NetSystem.IsServer)
		{
			this.playerOwnerID.Value = player.GlobalID;
		}
		else
		{
			this.inventory.ArrayRecieve = new ArrayRecieveProxy(this.RecieveInventory);
			this.gold.Recieve = new RecieveProxy(this.RecieveGold);
			this.goalScore.Recieve = new RecieveProxy(this.RecieveGoalScore);
			this.goals.ArrayRecieve = new ArrayRecieveProxy(this.RecieveGoals);
		}
		if (!base.IsOwner)
		{
			this.cameraPosition.Recieve = new RecieveProxy(this.RecieveCamPosition);
			this.interpolator = new Interpolator(Interpolator.InterpolationType.Position);
		}
		if (player != null)
		{
			player.BoardObject = this;
		}
		GameManager.Board.RegisterBoardPlayer(this);
		this.uiPlayerScore = this.uiController.AddPlayerScoreUI(this);
		if (GameManager.SaveToLoad == null)
		{
			this.CurrentNode = GameManager.Board.BoardStartNode;
		}
	}

	// Token: 0x060019BC RID: 6588 RVA: 0x000AAE88 File Offset: 0x000A9088
	public void Update()
	{
		if (GameManager.DEBUGGING && Input.GetKeyDown(KeyCode.P))
		{
			this.GiveGold(1, true);
		}
		if (GameManager.DEBUGGING && Input.GetKeyDown(KeyCode.O))
		{
			this.RemoveGold(1, false, false);
		}
		Vector3 end = (this.curNode != null) ? this.curNode.transform.position : Vector3.zero;
		Debug.DrawLine(base.transform.position + Vector3.up * 2f, end, Color.red);
		if (this.PlayerState != BoardPlayerState.Ragdolling)
		{
			this.playerAnim.Grounded = true;
			this.playerAnim.MovementAxis = this.movementAxis;
			this.playerAnim.Velocity = this.moveVelocity / this.moveSpeed;
			this.playerAnim.VelocityY = 0f;
		}
		this.playerAnim.UpdateAnimationState();
		bool flag = false;
		if (GameManager.Board != null && GameManager.Board.GoalNode != null)
		{
			for (int i = 0; i < GameManager.Board.GoalNode.Length; i++)
			{
				if (GameManager.Board.GoalNode[i] != null)
				{
					flag = true;
				}
			}
		}
		if (flag && (this.PlayerState == BoardPlayerState.WaitingIntersection || this.PlayerState == BoardPlayerState.ViewingMap || this.PlayerState == BoardPlayerState.GetTurnInput || this.PlayerState == BoardPlayerState.Interacting || this.PlayerState == BoardPlayerState.InventoryOpen || this.PlayerState == BoardPlayerState.MakingInteractionChoice || this.PlayerState == BoardPlayerState.ItemEquipped || this.PlayerState == BoardPlayerState.ItemUsing) && this.objectiveTrailTimer.Elapsed(true) && !this.TwitchMapEvent)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.objectiveTrailPrefab);
			gameObject.GetComponent<ObjectiveTrail>().Setup(this.CurrentNode);
			gameObject.transform.parent = GameManager.BoardRoot.transform;
		}
		if (GameManager.Board.CurPlayer == this && GameManager.TurnLengthLimited && !this.ForceTurnEnd)
		{
			GameManager.UIController.turnTimer.SetTime(this.turnTimeTaken, GameManager.TurnLength);
			if (this.turnTimeTaken >= GameManager.TurnLength)
			{
				this.ForceTurnEnd = true;
				GameManager.UIController.turnTimer.SetState(false);
				GameManager.UIController.ShowLargeText(LocalizationManager.GetTranslation("Board_TimeExpired", true, 0, true, false, null, null, true), LargeTextType.PlayerStart, 3f, false, false);
				AudioSystem.PlayOneShot(this.turnFailedClip, 0.5f, 0f, 1f);
			}
			if (this.curState == BoardPlayerState.ItemEquipped || this.curState == BoardPlayerState.GetTurnInput || this.curState == BoardPlayerState.InventoryOpen || this.curState == BoardPlayerState.MakingInteractionChoice || this.curState == BoardPlayerState.ViewingMap || this.curState == BoardPlayerState.WaitingIntersection)
			{
				this.turnTimeTaken += Time.deltaTime;
				GameManager.UIController.turnTimer.SetState(true);
			}
			else
			{
				GameManager.UIController.turnTimer.SetState(false);
			}
		}
		switch (this.curState)
		{
		case BoardPlayerState.MoveOffset:
			this.curMoveCounter += Time.deltaTime;
			if (this.curMoveCounter <= this.curMoveTotalTime)
			{
				base.transform.position = Vector3.Lerp(this.startLerp, this.endLerp, this.curMoveCounter / this.curMoveTotalTime);
			}
			else
			{
				base.transform.position = this.endLerp;
				this.SetState(BoardPlayerState.Idle);
			}
			break;
		case BoardPlayerState.MoveArc:
			this.curMoveCounter += Time.deltaTime;
			if (this.curMoveCounter <= this.curMoveTotalTime)
			{
				float num = this.curMoveCounter / this.curMoveTotalTime;
				Vector3 position = Vector3.Lerp(this.startLerp, this.endLerp, num);
				position.y += this.arcHeight * Mathf.Sin(Mathf.Clamp01(num) * 3.1415927f);
				base.transform.position = position;
			}
			else
			{
				base.transform.position = this.endLerp;
				this.SetState(BoardPlayerState.Idle);
			}
			break;
		case BoardPlayerState.ViewingMap:
			if (base.IsOwner)
			{
				if (!GameManager.IsGamePaused)
				{
					float d = 12f;
					Vector3 a = Vector3.zero;
					if (this.player.IsAI)
					{
						a = this.AIViewMapDir;
					}
					else
					{
						a = new Vector3(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), 0f, this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
						float d2 = Mathf.Clamp(a.magnitude, -1f, 1f);
						a = a.normalized * d2;
					}
					Vector3 vector = this.cameraPosition.Value;
					vector += a * d * Time.deltaTime;
					Rect mapExtents = GameManager.Board.CurrentMap.mapExtents;
					vector.x = Mathf.Clamp(vector.x, mapExtents.xMin, mapExtents.xMax);
					vector.z = Mathf.Clamp(vector.z, mapExtents.yMin, mapExtents.yMax);
					this.cameraPosition.Value = vector;
					GameManager.GetCamera().MoveToInstant(this.cameraPosition.Value, Vector3.zero);
				}
			}
			else
			{
				this.interpolator.Update();
				GameManager.GetCamera().MoveToInstant(this.interpolator.CurrentPosition, Vector3.zero);
			}
			break;
		case BoardPlayerState.Dead:
		{
			float d3 = 0.15f;
			base.transform.position -= Vector3.up * Time.deltaTime * d3;
			break;
		}
		}
		this.DoMovement();
	}

	// Token: 0x060019BD RID: 6589 RVA: 0x0000398C File Offset: 0x00001B8C
	public void FixedUpdate()
	{
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x000AB450 File Offset: 0x000A9650
	public void SetState(BoardPlayerState newState)
	{
		Debug.Log("Setting Board Player " + base.OwnerSlot.ToString() + " State: " + newState.ToString());
		switch (newState)
		{
		case BoardPlayerState.GetTurnInput:
			this.AiDelayTimer.SetInterval(0.5f, 1.5f, true);
			break;
		case BoardPlayerState.WaitingIntersection:
			if (GameManager.TurnLengthLimited)
			{
				this.turnTimeTaken = Mathf.Min(this.turnTimeTaken, GameManager.TurnLength - this.bonusTime);
			}
			this.AiDelayTimer.SetInterval(0.25f, 0.5f, true);
			break;
		case BoardPlayerState.ViewingMap:
			this.AIViewMapDir = new Vector3(ZPMath.RandomFloat(GameManager.rand, -1f, 1f), 0f, ZPMath.RandomFloat(GameManager.rand, -1f, 1f));
			this.AIViewMapDir.Normalize();
			break;
		case BoardPlayerState.ItemEquipped:
			this.AiDelayTimer.SetInterval(1f, 1.5f, true);
			break;
		case BoardPlayerState.MakingInteractionChoice:
			if (GameManager.TurnLengthLimited)
			{
				this.turnTimeTaken = Mathf.Min(this.turnTimeTaken, GameManager.TurnLength - this.bonusTime);
			}
			this.AiDelayTimer.SetInterval(0.75f, 1.2f, true);
			break;
		case BoardPlayerState.Interacting:
			this.AiDelayTimer.SetInterval(0.75f, 1.2f, true);
			break;
		case BoardPlayerState.Dying:
			this.preKillState = this.curState;
			break;
		case BoardPlayerState.InventoryOpen:
			this.AiDelayTimer.SetInterval(0.5f, 0.75f, true);
			break;
		}
		this.curState = newState;
	}

	// Token: 0x060019BF RID: 6591 RVA: 0x000AB614 File Offset: 0x000A9814
	private void DoMovement()
	{
		BoardPlayerState boardPlayerState = this.curState;
		if (boardPlayerState != BoardPlayerState.Idle)
		{
			if (boardPlayerState == BoardPlayerState.Moving)
			{
				this.curMoveCounter += Time.deltaTime;
				if (this.targetNode == null)
				{
					if (this.curMoveSteps > 0)
					{
						this.forwardNodes = this.curNode.GetForwardNodes(null, false);
						if (this.forwardNodes.Count > 1)
						{
							this.SetState(BoardPlayerState.WaitingIntersection);
						}
						else if (this.forwardNodes.Count == 1)
						{
							bool flag = false;
							bool removeStep = false;
							for (int i = 0; i < this.curNode.nodeConnections.Length; i++)
							{
								if (this.curNode.nodeConnections[i].connection_type == BoardNodeConnectionDirection.Forward && (this.curNode.nodeConnections[i].transition == BoardNodeTransition.Teleport || this.curNode.nodeConnections[i].transition == BoardNodeTransition.RandomTeleport))
								{
									removeStep = (this.curNode.nodeConnections[i].transition == BoardNodeTransition.RandomTeleport);
									flag = true;
									break;
								}
							}
							if (flag)
							{
								this.MoveTeleport(this.forwardNodes[0], removeStep);
							}
							else
							{
								this.StartMove(0, false);
							}
						}
						else
						{
							Debug.LogError("Node has no forward nodes, this shouldn't ever happen, board nodes connected incorrectly! " + this.curNode.gameObject.name);
						}
					}
					else
					{
						this.SetState(BoardPlayerState.Idle);
					}
				}
				if (this.targetNode != null)
				{
					if (!this.moveSpline.StepSpline(this.moveVelocity * Time.deltaTime))
					{
						this.movementAxis = Vector2.up;
						this.moveVelocity += this.acceleration * Time.deltaTime;
						this.moveVelocity = Mathf.Clamp(this.moveVelocity, 0f, this.moveSpeed);
						Vector3 zero = Vector3.zero;
						this.moveSpline.EvaluateSpline(this.moveSpline.CurrentStepT, ref zero, ref this.tangent);
						this.tangent.y = 0f;
						this.tangent = -this.tangent.normalized;
						this.playerAnim.SetPlayerRotation(Quaternion.LookRotation(-this.tangent).eulerAngles.y);
						base.transform.position = zero;
					}
					else
					{
						this.movementAxis = Vector2.zero;
						this.moveVelocity = 0f;
						this.targetNode = null;
						if (this.curNode.CurHasInteraction)
						{
							this.SetState(BoardPlayerState.MakingInteractionChoice);
							this.curNode.CurInteractionScript.Setup(this);
						}
					}
				}
			}
		}
		else
		{
			this.playerAnim.SetPlayerRotation(180f);
		}
		if (this.curState != BoardPlayerState.MoveArc && this.curState != BoardPlayerState.Dying && this.curState != BoardPlayerState.Dead && this.curState != BoardPlayerState.Ragdolling && this.GroundSnap)
		{
			base.transform.position = this.GetGroundPosition();
		}
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x000AB8E8 File Offset: 0x000A9AE8
	private Vector3 GetGroundPosition()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, Vector3.down, out raycastHit, 256f, 1024))
		{
			this.groundPosition = new Vector3(base.transform.position.x, raycastHit.point.y + this.height, base.transform.position.z);
		}
		else
		{
			this.groundPosition = base.transform.position;
		}
		this.lastPosition = base.transform.position;
		return this.groundPosition;
	}

	// Token: 0x060019C1 RID: 6593 RVA: 0x000AB994 File Offset: 0x000A9B94
	public void StartTurn()
	{
		this.SetState(BoardPlayerState.GetTurnInput);
		this.uiPlayerScore.IsTurn = true;
		this.hasUsedItem = false;
		this.ForceTurnEnd = false;
		this.turnTimeTaken = 0f;
		GameManager.UIController.turnTimer.SetState(true);
		GameManager.UIController.turnTimer.SetTime(0f, GameManager.TurnLength);
		if (this.PresentScript != null)
		{
			base.StartCoroutine(this.PresentRelease());
		}
		GameManager.UIController.SetInputPlayer(this);
		GameManager.UIController.SetBoardInputHelpType((this.HasUsedItem || this.ItemsDisabled || this.TwitchMapEvent) ? ((this.ItemsDisabled || this.TwitchMapEvent) ? BoardInputType.PlayerTurnDisabledItem : BoardInputType.PlayerTurnUsedItem) : BoardInputType.PlayerTurn);
		GameManager.UIController.SetInputStatus(this.GamePlayer.IsLocalPlayer);
	}

	// Token: 0x060019C2 RID: 6594 RVA: 0x00013168 File Offset: 0x00011368
	public IEnumerator PresentRelease()
	{
		this.PresentScript.anim.Play("PresentRelease2");
		yield return new WaitForSeconds(1f);
		this.Visible = true;
		yield return new WaitForSeconds(0.34f);
		UnityEngine.Object.Destroy(this.PresentScript.gameObject);
		this.PresentScript = null;
		yield break;
	}

	// Token: 0x060019C3 RID: 6595 RVA: 0x00013177 File Offset: 0x00011377
	public void EndTurn()
	{
		this.uiPlayerScore.IsTurn = false;
		this.ForceTurnEnd = false;
		this.turnTimeTaken = 0f;
	}

	// Token: 0x060019C4 RID: 6596 RVA: 0x000ABA6C File Offset: 0x000A9C6C
	public void StartViewMap()
	{
		if (base.IsOwner)
		{
			this.cameraPosition.Value = base.transform.position + GameManager.Board.PlayerCamOffset;
		}
		this.originalState = this.curState;
		this.originalCameraAngle = GameManager.GetCamera().targetCameraAngle;
		this.originalCamDistScale = GameManager.GetCamera().targetDistScale;
		GameManager.GetCamera().targetDistScale = 1.45f;
		GameManager.GetCamera().targetCameraAngle = 55f;
		GameManager.GetCamera().MoveToInstant(this.cameraPosition.Value, GameManager.Board.PlayerCamOffset);
		GameManager.UIController.HideScoreBoard();
		GameManager.UIController.ShowMapViewUI();
		GameManager.UIController.SetMapViewUITitle(this.player.Name + " " + LocalizationManager.GetTranslation("Viewing Map", true, 0, true, false, null, null, true));
		GameManager.UIController.SetBoardInputHelpType(BoardInputType.MapView);
		if (this.diceEffect)
		{
			this.diceEffect.gameObject.SetActive(false);
		}
		this.SetState(BoardPlayerState.ViewingMap);
	}

	// Token: 0x060019C5 RID: 6597 RVA: 0x000ABB84 File Offset: 0x000A9D84
	public void EndViewMap()
	{
		if (base.IsOwner)
		{
			this.cameraPosition.Value = base.transform.position;
		}
		GameManager.GetCamera().targetDistScale = this.originalCamDistScale;
		GameManager.GetCamera().targetCameraAngle = this.originalCameraAngle;
		GameManager.GetCamera().SetTrackedObject(base.transform, GameManager.Board.PlayerCamOffset);
		GameManager.UIController.ShowScoreBoard();
		GameManager.UIController.HideMapViewUI();
		GameManager.UIController.SetBoardInputHelpType((this.HasUsedItem || this.ItemsDisabled || this.TwitchMapEvent) ? ((this.ItemsDisabled || this.TwitchMapEvent) ? BoardInputType.PlayerTurnDisabledItem : BoardInputType.PlayerTurnUsedItem) : BoardInputType.PlayerTurn);
		if (this.diceEffect)
		{
			this.diceEffect.gameObject.SetActive(true);
		}
		this.SetState(this.originalState);
	}

	// Token: 0x060019C6 RID: 6598 RVA: 0x000ABC60 File Offset: 0x000A9E60
	public void Move(int steps)
	{
		if (this.curState != BoardPlayerState.GetTurnInput && this.curState != BoardPlayerState.CompletedInteraction)
		{
			Debug.LogError(base.OwnerSlot.ToString() + " Move shouldn't be called when the player state is not GetTurnInput || completed interaction!: " + this.curState.ToString());
		}
		this.SetState(BoardPlayerState.Moving);
		this.curMoveSteps = steps;
	}

	// Token: 0x060019C7 RID: 6599 RVA: 0x00013197 File Offset: 0x00011397
	public void MoveOffset(Vector3 position)
	{
		if (this.curState == BoardPlayerState.Idle)
		{
			this.SetState(BoardPlayerState.MoveOffset);
			this.SetMoveTarget(position);
		}
	}

	// Token: 0x060019C8 RID: 6600 RVA: 0x000ABCBC File Offset: 0x000A9EBC
	public void MoveArc(BoardNode node, float velocity, float height)
	{
		this.CurrentNode = node;
		this.SetState(BoardPlayerState.MoveArc);
		this.curMoveCounter = 0f;
		this.startLerp = base.transform.position;
		this.endLerp = node.NodePosition;
		this.curMoveTotalTime = Vector3.Distance(this.startLerp, this.endLerp) / velocity;
		this.arcHeight = height;
	}

	// Token: 0x060019C9 RID: 6601 RVA: 0x000ABD20 File Offset: 0x000A9F20
	public void MoveTeleport(BoardNode node, bool removeStep)
	{
		this.CurrentNode = node;
		if (removeStep)
		{
			this.curMoveSteps--;
		}
		UnityEngine.Object.Instantiate<GameObject>(this.blinkEffectPrefab, base.transform.position, Quaternion.identity);
		base.transform.position = node.transform.position;
		UnityEngine.Object.Instantiate<GameObject>(this.blinkEffectPrefab, base.transform.position, Quaternion.identity);
		AudioSystem.PlayOneShot(this.blinkClip, 0.5f, 0f, 1f);
	}

	// Token: 0x060019CA RID: 6602 RVA: 0x000ABDB0 File Offset: 0x000A9FB0
	public void UseItem()
	{
		this.hasUsedItem = true;
		if (NetSystem.IsServer)
		{
			if (!NetSystem.IsServer || this.inventory[(int)this.curEquippedItem.details.itemID] == 0)
			{
				return;
			}
			if (BoardModifier.ShouldConsumeItems())
			{
				NetArray<byte> netArray = this.inventory;
				int itemID = (int)this.curEquippedItem.details.itemID;
				byte b = netArray[itemID];
				netArray[itemID] = b - 1;
			}
			if (this.curState == BoardPlayerState.InventoryOpen)
			{
				this.uiController.InventoryUI.SetInventoryPlayer(this);
			}
		}
		this.SetState(BoardPlayerState.ItemUsing);
	}

	// Token: 0x060019CB RID: 6603 RVA: 0x000ABE44 File Offset: 0x000AA044
	public void EquipItem(Item item)
	{
		if (this.CactusScript != null)
		{
			this.RemoveCactus(base.transform.position, 5f);
		}
		Debug.Log("Player " + base.OwnerSlot.ToString() + " Equipping Item: " + item.details.itemName);
		this.curEquippedItem = item;
		if (this.diceEffect)
		{
			this.diceEffect.gameObject.SetActive(false);
		}
		this.SetState(BoardPlayerState.ItemEquipped);
	}

	// Token: 0x060019CC RID: 6604 RVA: 0x000131AF File Offset: 0x000113AF
	public void SetEquippedItem(Item item)
	{
		this.curEquippedItem = item;
	}

	// Token: 0x060019CD RID: 6605 RVA: 0x000ABED0 File Offset: 0x000AA0D0
	public void UnEquipItem(bool endingTurn)
	{
		if (this.diceEffect)
		{
			if (!endingTurn)
			{
				this.diceEffect.gameObject.SetActive(true);
			}
			else
			{
				UnityEngine.Object.Destroy(this.diceEffect.gameObject);
			}
		}
		if (!endingTurn)
		{
			this.SetState(BoardPlayerState.GetTurnInput);
			return;
		}
		this.SetState(BoardPlayerState.Idle);
	}

	// Token: 0x060019CE RID: 6606 RVA: 0x000ABF24 File Offset: 0x000AA124
	public bool GiveItem(byte itemID, bool doEffects = true)
	{
		ItemDetails itemDetails = GameManager.ItemList.items[(int)itemID];
		if (doEffects)
		{
			this.GiveItemEffets(itemDetails.recievePrefab, itemDetails.rotateSpeed, true);
		}
		short[] array = this.obtainedInventory;
		array[(int)itemID] = array[(int)itemID] + 1;
		if (!NetSystem.IsServer)
		{
			return true;
		}
		if (this.inventory[(int)itemID] >= 255)
		{
			return false;
		}
		NetArray<byte> netArray = this.inventory;
		byte b = netArray[(int)itemID];
		netArray[(int)itemID] = b + 1;
		if (this.curState == BoardPlayerState.InventoryOpen)
		{
			this.uiController.InventoryUI.SetInventoryPlayer(this);
		}
		return true;
	}

	// Token: 0x060019CF RID: 6607 RVA: 0x000ABFBC File Offset: 0x000AA1BC
	private void GiveItemEffets(GameObject recievePrefab, float rotateSpeed, bool isItem)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(isItem ? this.objectRecievePrefab : this.trophyRecievePrefab, base.transform.position + Vector3.up * 2.5f, Quaternion.identity);
		gameObject.GetComponent<ObjectRecieve>().rotateSpeed = rotateSpeed;
		UnityEngine.Object.Instantiate<GameObject>(recievePrefab, gameObject.transform).transform.localPosition = (isItem ? Vector3.zero : new Vector3(0f, -0.17f, 0f));
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x000AC044 File Offset: 0x000AA244
	public void RemoveTrophy()
	{
		if (NetSystem.IsServer)
		{
			NetVar<byte> netVar = this.goalScore;
			byte value = netVar.Value;
			netVar.Value = value - 1;
			this.uiController.UpdateScores();
		}
	}

	// Token: 0x060019D1 RID: 6609 RVA: 0x000AC07C File Offset: 0x000AA27C
	public bool GiveTrophy(byte trophyID, short trophyCost, bool doEffects = false)
	{
		bool result;
		if (NetSystem.IsServer)
		{
			if (this.gold.Value >= trophyCost)
			{
				NetVar<short> netVar = this.gold;
				netVar.Value -= trophyCost;
				NetVar<byte> netVar2 = this.goalScore;
				netVar2.Value += 1;
				this.goals.Set((int)(trophyID - 1), true);
			}
			result = true;
		}
		else
		{
			result = true;
		}
		this.uiController.UpdateScores();
		if (this.GamePlayer.IsLocalPlayer && !this.GamePlayer.IsAI)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_FIRST_GOAL");
			GameManager.TrophyCount++;
			GameManager.SaveUnlocks();
		}
		if (doEffects)
		{
			this.GiveItemEffets(this.gobletPrefab, 100f, false);
		}
		return result;
	}

	// Token: 0x060019D2 RID: 6610 RVA: 0x000AC138 File Offset: 0x000AA338
	public void ApplyDamage(int amount)
	{
		DamageInstance d = new DamageInstance
		{
			damage = amount,
			details = "Apply Damage as int"
		};
		this.ApplyDamage(d);
	}

	// Token: 0x060019D3 RID: 6611 RVA: 0x000AC16C File Offset: 0x000AA36C
	public override void ApplyDamage(DamageInstance d)
	{
		BoardModifier.ModifyAppliedDamage(this, ref d);
		base.ApplyDamage(d);
		if (this.curState == BoardPlayerState.Dying || this.curState == BoardPlayerState.Dead)
		{
			return;
		}
		if (this.CactusScript == null && this.PresentScript == null && !this.TwitchMapEvent)
		{
			base.SpawnBlood(d);
			GameManager.UIController.SpawnWorldText("-" + d.damage.ToString(), base.transform.position + Vector3.up, 2f, WorldTextType.Damage, 0.5f, null);
			StatTracker.IncrementStat(StatType.DamageRecieved, this.player.GlobalID, (double)d.damage);
			if (d.killer != null)
			{
				StatTracker.IncrementStat(StatType.DamageDealt, d.killer.player.GlobalID, (double)d.damage);
			}
			bool flag = false;
			if ((int)this.ProxyHealth <= d.damage)
			{
				this.KillPlayer(d.killer, d.origin, d.ragdollVel);
				flag = true;
			}
			else
			{
				this.ProxyHealth -= (short)d.damage;
				if (d.ragdoll && this.curState != BoardPlayerState.Ragdolling)
				{
					this.SpawnRagdoll(d.origin, d.ragdollVel);
					this.SetState(BoardPlayerState.Ragdolling);
					this.startRagdoll = base.StartCoroutine(this.StartRagdoll(1.5f, 16f, true, 0.5f));
				}
			}
			if (d.removeKeys || flag)
			{
				if (BoardModifier.IsBoardModifierActive(BoardModifierID.Pinata))
				{
					this.RemoveGold((int)this.gold.Value, false, true);
				}
				else
				{
					this.RemoveGold(flag ? Mathf.Clamp((int)(this.gold.Value / 3), 0, 30) : 3, false, true);
				}
			}
		}
		else if (d.removeKeys && this.CactusScript != null)
		{
			this.RemoveCactus(d.origin, d.ragdollVel);
		}
		else
		{
			GameManager.UIController.SpawnWorldText("0", base.transform.position + Vector3.up, 2f, WorldTextType.Static, 0.5f, null);
		}
		if (d.sound)
		{
			AudioSystem.PlayOneShot(this.sfxBloodyDamage, d.volume, 0f, 1f);
		}
		if (d.hitAnim)
		{
			this.playerAnim.FireHitTrigger();
		}
		this.uiController.UpdateScores();
	}

	// Token: 0x060019D4 RID: 6612 RVA: 0x000131B8 File Offset: 0x000113B8
	public void RemoveCactus(Vector3 origin, float vel)
	{
		this.CactusScript.Ragdoll(origin, vel);
		this.CactusScript = null;
		this.Visible = true;
	}

	// Token: 0x060019D5 RID: 6613 RVA: 0x000131D5 File Offset: 0x000113D5
	public void ExternalSpawnRagDoll(Vector3 origin, float force, float waitTime)
	{
		this.SpawnRagdoll(origin, force);
		this.SetState(BoardPlayerState.Ragdolling);
		this.startRagdoll = base.StartCoroutine(this.StartRagdoll(waitTime, 16f, true, 0.5f));
	}

	// Token: 0x060019D6 RID: 6614 RVA: 0x00013205 File Offset: 0x00011405
	public IEnumerator StartRagdoll(float startWaitTime = 1.5f, float gravity = 16f, bool setAnim = true, float endWaitTime = 0.5f)
	{
		if (GameManager.Board.boardCamera.TrackedObject == base.transform)
		{
			GameManager.Board.boardCamera.SetTrackedObject(null, GameManager.Board.boardCamera.TrackOffset);
		}
		yield return new WaitForSeconds(startWaitTime);
		this.Visible = true;
		Vector3 nodeSlotPos = this.CurrentNode.GetPlayersSlotPosition(this);
		base.transform.position = nodeSlotPos + Vector3.up * 10f;
		if (setAnim)
		{
			this.playerAnim.Grounded = false;
			this.playerAnim.FireFallingTrigger();
		}
		float yVelocity = 0f;
		Vector3 targetPosition = this.GetGroundPosition();
		for (;;)
		{
			yVelocity += gravity * Time.deltaTime;
			base.transform.position -= new Vector3(0f, yVelocity * Time.deltaTime, 0f);
			if (base.transform.position.y < targetPosition.y + 1.5f)
			{
				this.playerAnim.Grounded = true;
			}
			if (base.transform.position.y < targetPosition.y)
			{
				break;
			}
			yield return null;
		}
		base.transform.position = targetPosition;
		this.playerAnim.VelocityY = yVelocity;
		yield return new WaitForSeconds(endWaitTime);
		this.SetState(BoardPlayerState.Idle);
		Vector3 playersSlotPosition = this.CurrentNode.GetPlayersSlotPosition(this);
		if (!playersSlotPosition.Equals(nodeSlotPos))
		{
			this.MoveOffset(playersSlotPosition);
		}
		yield break;
	}

	// Token: 0x060019D7 RID: 6615 RVA: 0x000AC3D0 File Offset: 0x000AA5D0
	public void ApplyHeal(int amount)
	{
		this.ProxyHealth = (short)Mathf.Clamp((int)this.ProxyHealth + amount, 0, (int)this.maxHealth);
		AudioSystem.PlayOneShot(this.sfxHealthGain, 0.3f, 0f, 1f);
		GameManager.UIController.SpawnWorldText("+" + amount.ToString(), base.transform.position, 2f, WorldTextType.Heal, 0f, null);
		this.uiController.UpdateScores();
	}

	// Token: 0x060019D8 RID: 6616 RVA: 0x000AC450 File Offset: 0x000AA650
	public void GiveGold(int amount, bool doEffects = true)
	{
		if (NetSystem.IsServer)
		{
			NetVar<short> netVar = this.gold;
			netVar.Value += (short)amount;
			StatTracker.IncrementStat(StatType.KeysGained, this.player.GlobalID, (double)amount);
			this.uiController.UpdateScores();
		}
		if (doEffects)
		{
			AudioSystem.PlayOneShot(this.sfxCurrencyGain, 0.5f, 0f, 1f);
			GameManager.UIController.SpawnWorldText("+" + amount.ToString(), base.transform.position, 2f, WorldTextType.GiveGold, 0f, null);
		}
	}

	// Token: 0x060019D9 RID: 6617 RVA: 0x000AC4E8 File Offset: 0x000AA6E8
	public void RemoveGold(int amount, bool doParticle = true, bool spawnKeys = false)
	{
		if (NetSystem.IsServer)
		{
			short value = this.gold.Value;
			this.gold.Value = (short)Mathf.Clamp((int)this.gold.Value - amount, 0, 32767);
			short num = value - this.gold.Value;
			StatTracker.IncrementStat(StatType.KeysLost, this.player.GlobalID, (double)num);
			this.uiController.UpdateScores();
			if (spawnKeys)
			{
				GameManager.KeyController.SpawnKeys(amount, this, null);
			}
		}
		if (doParticle)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.goldParticleEffect, base.transform.position + Vector3.up * 0.875f, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
		}
	}

	// Token: 0x060019DA RID: 6618 RVA: 0x000AC5B0 File Offset: 0x000AA7B0
	public void KillPlayer(BoardPlayer killer, Vector3 origin, float force)
	{
		if (this.curState != BoardPlayerState.Ragdolling)
		{
			this.SpawnRagdoll(origin, force);
		}
		else
		{
			base.StopCoroutine(this.startRagdoll);
			this.Visible = false;
		}
		this.SetState(BoardPlayerState.Dying);
		this.ProxyHealth = 0;
		GameManager.Board.KillPlayer(this, killer, origin, force);
	}

	// Token: 0x060019DB RID: 6619 RVA: 0x000AC604 File Offset: 0x000AA804
	public void SpawnRagdoll(Vector3 origin, float force)
	{
		Vector3 normalized = (base.transform.position + Vector3.up * 0.875f - origin).normalized;
		this.NewestRagdoll = this.playerAnim.SpawnRagdoll(normalized * force);
		this.Visible = false;
	}

	// Token: 0x060019DC RID: 6620 RVA: 0x000AC660 File Offset: 0x000AA860
	public void RevivePlayer()
	{
		if (this.preKillState != BoardPlayerState.GetTurnInput && this.preKillState != BoardPlayerState.Idle)
		{
			Debug.LogError("Pre Kill State was: " + this.preKillState.ToString() + " This probably shouldn't happen");
		}
		this.SetState(this.preKillState);
		this.playerAnim.FireReviveTrigger();
		this.ProxyHealth = this.maxHealth;
		this.Visible = true;
		this.uiController.UpdateScores();
	}

	// Token: 0x060019DD RID: 6621 RVA: 0x00013231 File Offset: 0x00011431
	public void UpdateRendererList()
	{
		this.renderers = base.GetComponentsInChildren<Renderer>();
	}

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x060019DE RID: 6622 RVA: 0x0001323F File Offset: 0x0001143F
	// (set) Token: 0x060019DF RID: 6623 RVA: 0x00013247 File Offset: 0x00011447
	public bool WasVisible { get; set; }

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x060019E0 RID: 6624 RVA: 0x00013250 File Offset: 0x00011450
	// (set) Token: 0x060019E1 RID: 6625 RVA: 0x000AC6D8 File Offset: 0x000AA8D8
	public bool Visible
	{
		get
		{
			return this.visible;
		}
		set
		{
			if (!(this.CactusScript != null))
			{
				this.UpdateRendererList();
				this.visible = value;
				for (int i = 0; i < this.renderers.Length; i++)
				{
					this.renderers[i].enabled = this.visible;
				}
			}
		}
	}

	// Token: 0x060019E2 RID: 6626 RVA: 0x00013258 File Offset: 0x00011458
	public void ChooseDirection(int direction)
	{
		this.SetState(BoardPlayerState.Moving);
		this.StartMove(direction, true);
	}

	// Token: 0x060019E3 RID: 6627 RVA: 0x000AC728 File Offset: 0x000AA928
	public void RollDice(BoardDiceType type)
	{
		if (this.diceEffect != null)
		{
			this.diceEffect.DestroyEffect();
		}
		if (type <= BoardDiceType.TurnOrder)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.diceEffectPrefab, this.DicePosition(), Quaternion.identity);
			this.diceEffect = gameObject.GetComponent<DiceEffect>();
		}
		this.isRollingDice = true;
		this.curDiceType = type;
	}

	// Token: 0x060019E4 RID: 6628 RVA: 0x00013269 File Offset: 0x00011469
	public Vector3 DicePosition()
	{
		return base.transform.position + new Vector3(0f, 2.8f, 0f);
	}

	// Token: 0x060019E5 RID: 6629 RVA: 0x000AC784 File Offset: 0x000AA984
	public void HitDice(int rolled_number)
	{
		if (this.curDiceType == BoardDiceType.TurnOrder)
		{
			this.TurnOrderRoll = rolled_number;
		}
		else
		{
			GameManager.UIController.turnTimer.SetState(false);
		}
		AudioSystem.PlayOneShot((UnityEngine.Random.value > 0.5f) ? "Jump_01_Pond5" : "Jump_02_Pond5", 1f, 0.01f);
		this.playerAnim.FireJumpTrigger();
		if (this.CactusScript != null)
		{
			this.CactusScript.HitDice();
		}
		this.lastRolledNumber = rolled_number;
		this.isRollingDice = false;
	}

	// Token: 0x060019E6 RID: 6630 RVA: 0x000AC80C File Offset: 0x000AAA0C
	public void PlayerJumpPeek(PlayerAnimationEvent anim_event)
	{
		if (this.diceEffect != null)
		{
			this.uiController.SpawnWorldText(this.lastRolledNumber.ToString(), this.diceEffect.transform.position, 4.5f, WorldTextType.DiceRoll, 0f, null);
			this.diceEffect.DestroyEffect();
		}
	}

	// Token: 0x060019E7 RID: 6631 RVA: 0x000AC864 File Offset: 0x000AAA64
	private void StartMove(int dirIndex, bool startIntersection)
	{
		Debug.Log(string.Concat(new string[]
		{
			"StartMove - CurMoveSteps: ",
			this.curMoveSteps.ToString(),
			" DirIndex: ",
			dirIndex.ToString(),
			" CurNode: ",
			(this.curNode == null) ? "NULL" : this.curNode.gameObject.name
		}));
		this.curMoveCounter = 0f;
		this.moveVelocity = 0f;
		this.moveSpline.ResetStep();
		this.moveSpline.Clear();
		this.moveSpline.AddPoint(base.transform.position);
		BoardNode boardNode = this.curNode;
		bool flag = true;
		while (this.curMoveSteps > 0)
		{
			List<BoardNode> list = boardNode.GetForwardNodes(null, false);
			bool flag2 = (!flag || !startIntersection) && list.Count != 1;
			bool flag3 = !flag && boardNode.CurHasInteraction;
			bool flag4 = false;
			if (!flag)
			{
				for (int i = 0; i < boardNode.nodeConnections.Length; i++)
				{
					if (boardNode.nodeConnections[i].connection_type == BoardNodeConnectionDirection.Forward && (boardNode.nodeConnections[i].transition == BoardNodeTransition.Teleport || boardNode.nodeConnections[i].transition == BoardNodeTransition.RandomTeleport))
					{
						flag4 = true;
					}
				}
			}
			if (flag2 || flag3 || flag4)
			{
				this.forwardNodes = list;
				break;
			}
			boardNode = list[(flag && startIntersection) ? dirIndex : 0];
			this.moveSpline.AddPoint(boardNode.NodePosition);
			this.targetNode = boardNode;
			if (boardNode.baseNodeType != BoardNodeType.Pathing)
			{
				flag = false;
				this.curMoveSteps--;
			}
		}
		Debug.Log("Move Target Node: " + this.targetNode.gameObject.name);
		this.CurrentNode = this.targetNode;
		this.moveSpline.CalculateSpline(0.3f);
		this.curMoveTotalTime = this.moveSpline.SplineLength / this.moveSpeed;
	}

	// Token: 0x060019E8 RID: 6632 RVA: 0x000ACA60 File Offset: 0x000AAC60
	private void SetMoveTarget(Vector3 position)
	{
		this.curMoveCounter = 0f;
		this.startLerp = base.transform.position;
		this.endLerp = position;
		this.curMoveTotalTime = Vector3.Distance(this.startLerp, this.endLerp) / this.moveSpeed;
		Vector3 a = new Vector3(this.startLerp.x, 0f, this.startLerp.z);
		Vector3 b = new Vector3(this.endLerp.x, 0f, this.endLerp.z);
		Vector3 normalized = (a - b).normalized;
		if (normalized != Vector3.zero)
		{
			this.targetRotation.SetLookRotation(normalized, Vector3.up);
		}
	}

	// Token: 0x060019E9 RID: 6633 RVA: 0x0001328F File Offset: 0x0001148F
	private void OnDeathAnimationFinished(PlayerAnimationEvent anim_event)
	{
		this.SetState(BoardPlayerState.Dead);
	}

	// Token: 0x060019EA RID: 6634 RVA: 0x000ACB20 File Offset: 0x000AAD20
	public void OpenInventory()
	{
		this.originalState = this.curState;
		this.SetState(BoardPlayerState.InventoryOpen);
		if (this.diceEffect)
		{
			this.diceEffect.gameObject.SetActive(false);
		}
		this.uiController.InventoryUI.SetInventoryPlayer(this);
		this.uiController.InventoryUI.Show();
		AudioSystem.PlayOneShot("MultimediaButtonClick007_STD_ZapSplat", 0.3f, 0f);
	}

	// Token: 0x060019EB RID: 6635 RVA: 0x000ACB94 File Offset: 0x000AAD94
	public void CloseInventory()
	{
		if (this.diceEffect)
		{
			this.diceEffect.gameObject.SetActive(true);
		}
		this.SetState(this.originalState);
		this.uiController.InventoryUI.Hide();
		AudioSystem.PlayOneShot("MultimediaButtonClick007_STD_ZapSplat", 0.3f, 0f);
	}

	// Token: 0x060019EC RID: 6636 RVA: 0x000ACBF0 File Offset: 0x000AADF0
	public void Load(PlayerSave save)
	{
		this.turnOrderRoll = save.turnOrderRoll;
		this.health.Value = save.health;
		this.ProxyHealth = save.health;
		this.gold.Value = save.gold;
		this.goalScore.Value = save.goalScore;
		this.postMinigameItem = save.postMinigameItem;
		if (save.cactusActive)
		{
			this.EquipCactus();
		}
		for (int i = 0; i < save.inventory.Length; i++)
		{
			this.inventory.Set(i, save.inventory[i]);
		}
		this.SetNode((int)save.curNodeID);
	}

	// Token: 0x060019ED RID: 6637 RVA: 0x000ACC98 File Offset: 0x000AAE98
	public PlayerSave Save()
	{
		PlayerSave playerSave = new PlayerSave();
		playerSave.turnOrderRoll = this.TurnOrderRoll;
		playerSave.health = this.health.Value;
		playerSave.gold = this.gold.Value;
		playerSave.goalScore = this.goalScore.Value;
		playerSave.curNodeID = (short)this.CurrentNode.NodeID;
		playerSave.postMinigameItem = this.postMinigameItem;
		playerSave.cactusActive = (this.CactusScript != null);
		for (int i = 0; i < playerSave.inventory.Length; i++)
		{
			playerSave.inventory[i] = this.inventory[i];
		}
		playerSave.name = this.GamePlayer.Name;
		playerSave.slotColor = this.GamePlayer.ColorIndex;
		playerSave.slotSkin = this.GamePlayer.SkinIndex;
		playerSave.slotHat = this.GamePlayer.HatIndex;
		playerSave.botDifficulty = (byte)this.GamePlayer.Difficulty;
		return playerSave;
	}

	// Token: 0x060019EE RID: 6638 RVA: 0x000ACD98 File Offset: 0x000AAF98
	public void SetNode(int nodeID)
	{
		Debug.Log("NodeID: " + nodeID.ToString());
		BoardNode currentNode = GameManager.Board.BoardNodes[nodeID];
		this.CurrentNode = currentNode;
		base.transform.position = this.CurrentNode.GetPlayersSlotPosition(this);
		if (this.diceEffect != null)
		{
			this.diceEffect.startPos = this.DicePosition();
		}
	}

	// Token: 0x060019EF RID: 6639 RVA: 0x00013299 File Offset: 0x00011499
	public void RecieveGold(object gold)
	{
		this.uiController.UpdateScores();
	}

	// Token: 0x060019F0 RID: 6640 RVA: 0x00013299 File Offset: 0x00011499
	public override void RecieveHealth(object health)
	{
		this.uiController.UpdateScores();
	}

	// Token: 0x060019F1 RID: 6641 RVA: 0x00013299 File Offset: 0x00011499
	public void RecieveGoalScore(object health)
	{
		this.uiController.UpdateScores();
	}

	// Token: 0x060019F2 RID: 6642 RVA: 0x0000398C File Offset: 0x00001B8C
	public void RecieveGoals(int index, object state)
	{
	}

	// Token: 0x060019F3 RID: 6643 RVA: 0x000132A6 File Offset: 0x000114A6
	public void RecieveInventory(int index, object _pos)
	{
		if (this.curState == BoardPlayerState.InventoryOpen)
		{
			this.uiController.InventoryUI.SetInventoryPlayer(this);
		}
	}

	// Token: 0x060019F4 RID: 6644 RVA: 0x000132C3 File Offset: 0x000114C3
	public void RecieveCamPosition(object _pos)
	{
		this.interpolator.NewPosition(_pos);
	}

	// Token: 0x060019F5 RID: 6645 RVA: 0x000132D1 File Offset: 0x000114D1
	public override void OnOwnerChanged()
	{
		if (base.Owner == NetSystem.MyPlayer)
		{
			GameManager.Board.AddToMyPlayerList(this);
		}
		base.OnOwnerChanged();
	}

	// Token: 0x04001B4E RID: 6990
	[Header("Movement")]
	public float height;

	// Token: 0x04001B4F RID: 6991
	public float moveSpeed = 4.5f;

	// Token: 0x04001B50 RID: 6992
	public float acceleration = 11f;

	// Token: 0x04001B51 RID: 6993
	public float moveVelocity;

	// Token: 0x04001B52 RID: 6994
	public float turnRate = 5f;

	// Token: 0x04001B53 RID: 6995
	[Header("Jumping")]
	public float jumpHeight = 0.3f;

	// Token: 0x04001B54 RID: 6996
	public float jumpTime = 0.25f;

	// Token: 0x04001B55 RID: 6997
	public AnimationCurve jumpCurve;

	// Token: 0x04001B56 RID: 6998
	[Header("Effects")]
	public AudioClip sfxCurrencyGain;

	// Token: 0x04001B57 RID: 6999
	public AudioClip sfxHealthGain;

	// Token: 0x04001B58 RID: 7000
	public AudioClip sfxBloodyDamage;

	// Token: 0x04001B59 RID: 7001
	public GameObject goldParticleEffect;

	// Token: 0x04001B5A RID: 7002
	public GameObject objectiveTrailPrefab;

	// Token: 0x04001B5B RID: 7003
	public GameObject gobletPrefab;

	// Token: 0x04001B5C RID: 7004
	public AudioClip turnFailedClip;

	// Token: 0x04001B5F RID: 7007
	public int TwitchMapEventFailRolls;

	// Token: 0x04001B60 RID: 7008
	private GamePlayer player;

	// Token: 0x04001B61 RID: 7009
	private BoardPlayerState curState;

	// Token: 0x04001B62 RID: 7010
	private GameObject objectRecievePrefab;

	// Token: 0x04001B63 RID: 7011
	private GameObject trophyRecievePrefab;

	// Token: 0x04001B64 RID: 7012
	private int turnOrderRoll = -1;

	// Token: 0x04001B65 RID: 7013
	private int lastRolledNumber;

	// Token: 0x04001B66 RID: 7014
	private bool isRollingDice;

	// Token: 0x04001B67 RID: 7015
	private BoardDiceType curDiceType;

	// Token: 0x04001B68 RID: 7016
	private GameObject diceEffectPrefab;

	// Token: 0x04001B69 RID: 7017
	[HideInInspector]
	public DiceEffect diceEffect;

	// Token: 0x04001B6A RID: 7018
	private float curMoveCounter;

	// Token: 0x04001B6B RID: 7019
	private float curMoveTotalTime;

	// Token: 0x04001B6C RID: 7020
	private float arcHeight = 1f;

	// Token: 0x04001B6D RID: 7021
	private int curMoveSteps;

	// Token: 0x04001B6E RID: 7022
	private List<BoardNode> forwardNodes;

	// Token: 0x04001B6F RID: 7023
	private PlayerAnimation playerAnim;

	// Token: 0x04001B70 RID: 7024
	private Quaternion targetRotation;

	// Token: 0x04001B71 RID: 7025
	private BoardNode curNode;

	// Token: 0x04001B72 RID: 7026
	private BoardNode targetNode;

	// Token: 0x04001B73 RID: 7027
	private Vector3 startLerp;

	// Token: 0x04001B74 RID: 7028
	private Vector3 endLerp;

	// Token: 0x04001B75 RID: 7029
	private Vector3 tangent;

	// Token: 0x04001B76 RID: 7030
	private Vector2 movementAxis = Vector2.zero;

	// Token: 0x04001B77 RID: 7031
	private Spline moveSpline = new Spline();

	// Token: 0x04001B78 RID: 7032
	private GameUIController uiController;

	// Token: 0x04001B79 RID: 7033
	private UIPlayerScoreNew uiPlayerScore;

	// Token: 0x04001B7A RID: 7034
	private Item curEquippedItem;

	// Token: 0x04001B7B RID: 7035
	private Interpolator interpolator;

	// Token: 0x04001B7C RID: 7036
	private BoardPlayerState originalState;

	// Token: 0x04001B7D RID: 7037
	private float originalCameraAngle;

	// Token: 0x04001B7E RID: 7038
	private float originalCamDistScale;

	// Token: 0x04001B7F RID: 7039
	private Renderer[] renderers;

	// Token: 0x04001B80 RID: 7040
	private ActionTimer AiDelayTimer = new ActionTimer(10f, 10f);

	// Token: 0x04001B81 RID: 7041
	private bool hasUsedItem;

	// Token: 0x04001B82 RID: 7042
	private Coroutine startRagdoll;

	// Token: 0x04001B83 RID: 7043
	private ActionTimer objectiveTrailTimer = new ActionTimer(1f);

	// Token: 0x04001B84 RID: 7044
	private float turnTimeTaken;

	// Token: 0x04001B85 RID: 7045
	public bool ForceTurnEnd;

	// Token: 0x04001B86 RID: 7046
	private short[] obtainedInventory = new short[15];

	// Token: 0x04001B87 RID: 7047
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetVar<short> playerOwnerID = new NetVar<short>();

	// Token: 0x04001B88 RID: 7048
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetVar<short> gold = new NetVar<short>(35);

	// Token: 0x04001B89 RID: 7049
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetVar<byte> goalScore = new NetVar<byte>(0);

	// Token: 0x04001B8A RID: 7050
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetArray<byte> inventory = new NetArray<byte>(15);

	// Token: 0x04001B8B RID: 7051
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	private NetVec3 cameraPosition = new NetVec3();

	// Token: 0x04001B8C RID: 7052
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	private NetArray<bool> goals = new NetArray<bool>(6);

	// Token: 0x04001B8F RID: 7055
	private bool itemsDisabled;

	// Token: 0x04001B91 RID: 7057
	[Header("CactusStuff")]
	public GameObject cactusPrefab;

	// Token: 0x04001B94 RID: 7060
	[HideInInspector]
	public byte postMinigameItem = byte.MaxValue;

	// Token: 0x04001B95 RID: 7061
	public GameObject haloweenSplashStepEffect;

	// Token: 0x04001B96 RID: 7062
	public GameObject winterStepEffect;

	// Token: 0x04001B97 RID: 7063
	private Vector3 AIViewMapDir = Vector3.zero;

	// Token: 0x04001B98 RID: 7064
	private BoardPlayerState preKillState;

	// Token: 0x04001B99 RID: 7065
	private float bonusTime = 10f;

	// Token: 0x04001B9A RID: 7066
	private Vector3 groundPosition = Vector3.zero;

	// Token: 0x04001B9B RID: 7067
	private Vector3 lastPosition = Vector3.zero;

	// Token: 0x04001B9C RID: 7068
	public GameObject blinkEffectPrefab;

	// Token: 0x04001B9D RID: 7069
	public AudioClip blinkClip;

	// Token: 0x04001B9F RID: 7071
	private bool visible = true;
}
