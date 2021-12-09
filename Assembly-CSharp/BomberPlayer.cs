using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x0200015F RID: 351
public class BomberPlayer : Movement1
{
	// Token: 0x06000A1F RID: 2591 RVA: 0x00059904 File Offset: 0x00057B04
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		this.mover.SetForwardVector(-Vector3.forward);
		this.playerAnim.SetPlayerRotationImmediate(base.transform.rotation.eulerAngles.y);
		this.maxPlaceSqrRange = this.maxPlaceRange * this.maxPlaceRange;
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x00059970 File Offset: 0x00057B70
	protected override void Start()
	{
		base.Start();
		this.minigameController = (BomberController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.nodes = GameObject.Find("Nodes").GetComponent<NodeCreator>();
		if (base.OwnerSlot >= 0 && (int)base.OwnerSlot < BomberController.PLAYER_LAYERS.Length)
		{
			base.gameObject.layer = BomberController.PLAYER_LAYERS[(int)base.OwnerSlot];
		}
		for (int i = 0; i < this.minigameController.GetPlayerCount(); i++)
		{
			CharacterBase player = this.minigameController.GetPlayer(i);
			if (!(player == this))
			{
				Physics.IgnoreCollision(base.GetComponent<CharacterController>(), player.GetComponent<CharacterController>());
			}
		}
		this.playerColorLight.color = base.GamePlayer.Color.skinColor1;
		if (base.GamePlayer.Difficulty == BotDifficulty.Easy)
		{
			this.checkSafetyTimer = new ActionTimer(0f, 4f);
			return;
		}
		if (base.GamePlayer.Difficulty == BotDifficulty.Normal)
		{
			this.checkSafetyTimer = new ActionTimer(0f, 2f);
			return;
		}
		BotDifficulty difficulty = base.GamePlayer.Difficulty;
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x00059AA8 File Offset: 0x00057CA8
	private void Update()
	{
		if (!this.isDead)
		{
			base.UpdateController();
			if (base.IsOwner)
			{
				if (base.transform.position.y < 3.75f || base.transform.position.y > 6.75f)
				{
					base.transform.position = this.startPosition;
				}
				if ((!this.player.IsAI && !GameManager.IsGamePaused && this.player.RewiredPlayer.GetButtonDown(InputActions.Accept)) || this.AISpawnBomb)
				{
					this.SpawnBomb();
					this.AISpawnBomb = false;
				}
			}
		}
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x00059B50 File Offset: 0x00057D50
	private void SpawnBomb()
	{
		if (this.minigameController.Playable && !GameManager.IsGamePaused && GameManager.PollInput)
		{
			if (this.BombsRemaining > 0)
			{
				int nearestNodeIndex = this.GetNearestNodeIndex(base.transform.position, this.maxPlaceSqrRange, true);
				if (nearestNodeIndex != -1)
				{
					this.minigameController.PlaceBomb(NetSystem.NetTime.GameTime, nearestNodeIndex, this.bombRange, (int)base.OwnerSlot);
					return;
				}
				AudioSystem.PlayOneShot(this.error_sound, 0.5f, 0.1f, 1f);
				return;
			}
			else
			{
				AudioSystem.PlayOneShot(this.error_sound, 0.5f, 0.1f, 1f);
			}
		}
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x00059BFC File Offset: 0x00057DFC
	private int GetNearestNodeIndex(Vector3 pos, float maxDistSqr, bool checkOccupier)
	{
		float num = float.MaxValue;
		int result = -1;
		for (int i = 0; i < this.nodes.nodes.Length; i++)
		{
			if ((!checkOccupier || this.nodes.nodes[i].occupier == -1) && !this.nodes.nodes[i].bombNode.Blocked)
			{
				float sqrMagnitude = (pos - this.nodes.nodes[i].position).sqrMagnitude;
				if (sqrMagnitude < maxDistSqr && sqrMagnitude < num)
				{
					num = sqrMagnitude;
					result = i;
				}
			}
		}
		return result;
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x00059C8C File Offset: 0x00057E8C
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		if (!base.GamePlayer.IsAI)
		{
			Vector2 axis = new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
			input = new CharacterMoverInput(axis, false, false);
		}
		else
		{
			input = this.GetAIInput();
		}
		input.NullInput(!this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput);
		this.mover.CalculateVelocity(input, Time.deltaTime);
		this.mover.DoMovement(Time.deltaTime);
		if (this.mover.MovementAxis != Vector2.zero)
		{
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(this.mover.MovementAxis.x, 0f, this.mover.MovementAxis.y), Vector3.up), 1500f * Time.deltaTime);
		}
		this.velocity.Value = this.mover.Velocity;
		this.netIsGrounded.Value = this.controller.isGrounded;
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x00059DE4 File Offset: 0x00057FE4
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		if (!this.minigameController.Playable || GameManager.IsGamePaused || !GameManager.PollInput)
		{
			return result;
		}
		int nearestNodeIndex = this.GetNearestNodeIndex(base.transform.position, float.MaxValue, false);
		Node node = this.nodes.nodes[nearestNodeIndex];
		if ((this.curAIState != BomberPlayer.BomberAIState.EscapingBlast || (this.targetNode != null && this.targetNode.HittingBombs.Count > 0)) && this.checkSafetyTimer.Elapsed(true) && node.HittingBombs.Count > 0)
		{
			this.targetNode = null;
			this.targetNode = this.minigameController.GetPathClosestSafe(node, (short)base.OwnerSlot);
			this.curAIState = BomberPlayer.BomberAIState.EscapingBlast;
		}
		if (this.curAIState == BomberPlayer.BomberAIState.Idle)
		{
			if (this.bombsRemaining > 0 && this.bombPlaceTestTimer.Elapsed(true))
			{
				this.bombSpawnHits.Clear();
				this.safeNodes.Clear();
				this.visitedNodes.Clear();
				this.GetSafeNodes(nearestNodeIndex, this.maxSafeDistance, 0);
				this.minigameController.GetHitNodes(nearestNodeIndex, this.bombRange, this.bombSpawnHits);
				bool flag = false;
				for (int i = 0; i < this.safeNodes.Count; i++)
				{
					if (!this.bombSpawnHits.Contains(this.safeNodes[i]))
					{
						BomberSearchNode path = this.minigameController.GetPath(this.safeNodes[i], node, (short)base.OwnerSlot);
						if (this.IsPathSafe(path))
						{
							flag = true;
							break;
						}
					}
				}
				if (GameManager.rand.NextDouble() < (double)this.difficultySafeOverrideChance[(int)base.GamePlayer.Difficulty])
				{
					flag = true;
				}
				if (flag)
				{
					this.AISpawnBomb = true;
				}
				else if (this.safeNodes.Count > 0)
				{
					Node node2 = null;
					for (int j = 0; j < this.safeNodes.Count; j++)
					{
						if (this.safeNodes[j] != node)
						{
							for (int k = 0; k < this.safeNodes[j].connections.Length; k++)
							{
								int nodeIndex = this.minigameController.GetNodeIndex(this.safeNodes[j].connections[k]);
								if (this.nodes.nodes[nodeIndex].bombNode.Blocked)
								{
									BomberSearchNode path2 = this.minigameController.GetPath(this.safeNodes[j], node, (short)base.OwnerSlot);
									if (this.IsPathSafe(path2))
									{
										node2 = this.safeNodes[j];
										break;
									}
								}
							}
							if (node2 != null)
							{
								break;
							}
						}
					}
					if (node2 != null)
					{
						this.targetNode = node2;
					}
					else
					{
						while (this.safeNodes.Count > 0)
						{
							int index = GameManager.rand.Next(this.safeNodes.Count);
							BomberSearchNode path3 = this.minigameController.GetPath(node, this.safeNodes[index], (short)base.OwnerSlot);
							if (this.IsPathSafe(path3))
							{
								this.targetNode = this.safeNodes[index];
								break;
							}
							this.safeNodes.RemoveAt(index);
						}
					}
					this.curAIState = BomberPlayer.BomberAIState.MovingToBombableNode;
				}
			}
			int num = int.MaxValue;
			BomberBuff bomberBuff = null;
			Node node3 = null;
			for (int l = 0; l < this.minigameController.bomberBuffs.Count; l++)
			{
				int nearestNodeIndex2 = this.GetNearestNodeIndex(this.minigameController.bomberBuffs[l].transform.position, float.MaxValue, false);
				if (this.nodes.nodes[nearestNodeIndex2].HittingBombs.Count == 0)
				{
					BomberSearchNode path4 = this.minigameController.GetPath(this.nodes.nodes[nearestNodeIndex2], node, (short)base.OwnerSlot);
					if (path4 != null && path4.cost < 5 && path4.pathCost < num && this.IsPathSafe(path4))
					{
						num = path4.pathCost;
						bomberBuff = this.minigameController.bomberBuffs[l];
						node3 = this.nodes.nodes[nearestNodeIndex2];
					}
				}
			}
			if (node3 != null)
			{
				this.curAIState = BomberPlayer.BomberAIState.GettingBuff;
				this.targetBuff = bomberBuff;
				this.targetNode = node3;
			}
		}
		else if (this.curAIState == BomberPlayer.BomberAIState.GettingBuff && this.targetBuff == null)
		{
			this.curAIState = BomberPlayer.BomberAIState.Idle;
			this.targetNode = null;
		}
		if (this.findPathTimer.Elapsed(true) && this.targetNode != null)
		{
			this.curPath = this.minigameController.GetPath(this.targetNode, node, this.player.GlobalID);
			if (this.curPath != null && this.curPath.next != null)
			{
				this.curPath = this.curPath.next;
			}
		}
		if (this.curPath != null)
		{
			float num2 = 0.35f;
			float num3 = num2 * num2;
			Vector3 vector = this.curPath.node.position - base.transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude < num3)
			{
				this.curPath = this.curPath.next;
				if (this.curPath == null)
				{
					this.curAIState = BomberPlayer.BomberAIState.Idle;
				}
			}
			if (this.curPath != null)
			{
				result = new CharacterMoverInput(new Vector2(-vector.x, -vector.z), false, false);
			}
		}
		return result;
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x0005A33C File Offset: 0x0005853C
	private bool IsPathSafe(BomberSearchNode path)
	{
		BomberSearchNode bomberSearchNode = path;
		while (bomberSearchNode.node.HittingBombs.Count == 0)
		{
			if (bomberSearchNode.next == null)
			{
				return true;
			}
			bomberSearchNode = bomberSearchNode.next;
		}
		return false;
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x0005A374 File Offset: 0x00058574
	private void GetSafeNodes(int nodeIndex, int maxDepth, int depth)
	{
		Node node = this.nodes.nodes[nodeIndex];
		if (node.bombNode.Blocked || (node.occupier != -1 && node.occupier != (short)base.OwnerSlot) || depth >= maxDepth || this.visitedNodes.Contains(node))
		{
			return;
		}
		this.visitedNodes.Add(node);
		if (node.HittingBombs.Count == 0)
		{
			this.safeNodes.Add(node);
		}
		for (int i = 0; i < node.connections.Length; i++)
		{
			int nodeIndex2 = this.minigameController.GetNodeIndex(node.connections[i]);
			this.GetSafeNodes(nodeIndex2, maxDepth, depth++);
		}
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x0005A424 File Offset: 0x00058624
	protected override void UpdateAnimationState(PlayerAnimation playerAnim)
	{
		Vector2 vector = new Vector2(this.velocity.Value.x, this.velocity.Value.z);
		float num = Mathf.Clamp01(vector.magnitude / this.mover.maxSpeed);
		playerAnim.Velocity = num;
		playerAnim.VelocityY = this.velocity.Value.y;
		playerAnim.MovementAxis = ((num > 0.01f) ? Vector2.up : Vector2.zero);
		if (base.IsOwner)
		{
			playerAnim.Grounded = this.controller.isGrounded;
		}
		else
		{
			playerAnim.Grounded = this.netIsGrounded.Value;
		}
		playerAnim.SetPlayerRotation(this.rotation.Value);
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x0005A4E8 File Offset: 0x000586E8
	private void OnTriggerEnter(Collider c)
	{
		if (base.IsOwner && this.minigameController.Playable && c.gameObject.name == "DeathZone")
		{
			this.KillPlayer(c.gameObject.transform.position, -1, true);
		}
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x0005A53C File Offset: 0x0005873C
	public override void ResetPlayer()
	{
		if (!this.isDead)
		{
			this.KillPlayer(base.transform.position - Vector3.up, -1, false);
		}
		this.targetNode = null;
		this.curPath = null;
		this.curAIState = BomberPlayer.BomberAIState.Idle;
		this.mover.Velocity = Vector3.zero;
		base.ResetPlayer();
		this.controller.enabled = true;
		this.MaxBombs = 1;
		this.BombsRemaining = 1;
		this.bombRange = 1;
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000A2C RID: 2604 RVA: 0x0000A9D9 File Offset: 0x00008BD9
	// (set) Token: 0x06000A2D RID: 2605 RVA: 0x0000A9E1 File Offset: 0x00008BE1
	public int MaxBombs
	{
		get
		{
			return this.maxBombs;
		}
		set
		{
			this.maxBombs = value;
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000A2E RID: 2606 RVA: 0x0000A9EA File Offset: 0x00008BEA
	// (set) Token: 0x06000A2F RID: 2607 RVA: 0x0000A9F2 File Offset: 0x00008BF2
	public int BombsRemaining
	{
		get
		{
			return this.bombsRemaining;
		}
		set
		{
			this.bombsRemaining = value;
		}
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x0000A9FB File Offset: 0x00008BFB
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender, int killer_slot, Vector3 pos)
	{
		this.KillPlayer(pos, killer_slot, false);
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x0005A5BC File Offset: 0x000587BC
	public void KillPlayer(Vector3 pos, int killer_slot, bool send_rpc)
	{
		if (!this.isDead)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.player_death_effect, base.transform.position, Quaternion.LookRotation(Vector3.up));
			AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
			Vector3 force = (base.transform.position - pos).normalized * UnityEngine.Random.Range(3.5f, 6f);
			force.y = UnityEngine.Random.Range(1f, 2.5f);
			this.playerAnim.SpawnRagdoll(force);
			this.isDead = true;
			this.Deactivate();
			this.controller.enabled = false;
			if ((int)base.OwnerSlot == killer_slot && base.GamePlayer.IsLocalPlayer && !base.GamePlayer.IsAI)
			{
				PlatformAchievementManager.Instance.TriggerAchievement("ACH_MADE_A_MISTAKE");
			}
			if (NetSystem.IsServer)
			{
				this.minigameController.PlayerDied((int)base.OwnerSlot, killer_slot);
			}
			if (base.IsOwner && send_rpc)
			{
				base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_UNORDERED, new object[]
				{
					killer_slot,
					pos
				});
			}
		}
	}

	// Token: 0x040008E6 RID: 2278
	public GameObject player_death_effect;

	// Token: 0x040008E7 RID: 2279
	public GameObject explode_effect;

	// Token: 0x040008E8 RID: 2280
	public int bombRange = 1;

	// Token: 0x040008E9 RID: 2281
	public AudioClip error_sound;

	// Token: 0x040008EA RID: 2282
	public Light playerColorLight;

	// Token: 0x040008EB RID: 2283
	private BomberController minigameController;

	// Token: 0x040008EC RID: 2284
	private int maxBombs = 1;

	// Token: 0x040008ED RID: 2285
	private int bombsRemaining = 1;

	// Token: 0x040008EE RID: 2286
	private float maxPlaceRange = 1f;

	// Token: 0x040008EF RID: 2287
	private float maxPlaceSqrRange = 1f;

	// Token: 0x040008F0 RID: 2288
	private NodeCreator nodes;

	// Token: 0x040008F1 RID: 2289
	private CharacterMover mover;

	// Token: 0x040008F2 RID: 2290
	private BomberPlayer.BomberAIState curAIState;

	// Token: 0x040008F3 RID: 2291
	private bool AISpawnBomb;

	// Token: 0x040008F4 RID: 2292
	private List<Node> safeNodes = new List<Node>();

	// Token: 0x040008F5 RID: 2293
	private List<Node> visitedNodes = new List<Node>();

	// Token: 0x040008F6 RID: 2294
	private List<Node> bombSpawnHits = new List<Node>();

	// Token: 0x040008F7 RID: 2295
	private ActionTimer bombPlaceTestTimer = new ActionTimer(0.5f, 1f);

	// Token: 0x040008F8 RID: 2296
	private ActionTimer checkSafetyTimer = new ActionTimer(0.03f, 0.06f);

	// Token: 0x040008F9 RID: 2297
	private ActionTimer findPathTimer = new ActionTimer(0.1f, 0.125f);

	// Token: 0x040008FA RID: 2298
	private int maxSafeDistance = 8;

	// Token: 0x040008FB RID: 2299
	private Node targetNode;

	// Token: 0x040008FC RID: 2300
	private BomberBuff targetBuff;

	// Token: 0x040008FD RID: 2301
	private BomberSearchNode curPath;

	// Token: 0x040008FE RID: 2302
	private float[] difficultySafeOverrideChance = new float[]
	{
		0.2f,
		0.1f,
		-1f
	};

	// Token: 0x02000160 RID: 352
	private enum BomberAIState
	{
		// Token: 0x04000900 RID: 2304
		Idle,
		// Token: 0x04000901 RID: 2305
		EscapingBlast,
		// Token: 0x04000902 RID: 2306
		GettingBuff,
		// Token: 0x04000903 RID: 2307
		MovingToBombableNode
	}
}
