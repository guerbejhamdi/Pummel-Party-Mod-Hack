using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200011A RID: 282
public class LightningPlayer : Movement1
{
	// Token: 0x06000867 RID: 2151 RVA: 0x0004E280 File Offset: 0x0004C480
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
		this.pointLight.color = this.player.Color.skinColor1;
		GameObject[] array = this.lightningHolderEffects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x0004E334 File Offset: 0x0004C534
	public override void OnOwnerChanged()
	{
		if (NetSystem.IsServer)
		{
			base.GetComponent<CharacterController>().enabled = true;
			base.GetComponent<NavMeshAgent>().enabled = true;
			this.mover.SetForwardVector(Vector3.forward);
			if (this.agent != null)
			{
				this.agent.updatePosition = false;
				this.agent.updateRotation = false;
			}
		}
		base.OnOwnerChanged();
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x0004E39C File Offset: 0x0004C59C
	public void LightningHolderRecieve(object o)
	{
		bool flag = (bool)o;
		if (flag)
		{
			this.mover.maxSpeed = 6.5f;
			this.mover.gravity = 0f;
			this.mover.useGravity = false;
			this.playerAnim.Animator.SetTrigger("Floating");
			this.pointLight.enabled = false;
			if (!this.lightningHolderSetup)
			{
				this.lightningHolderSetup = true;
			}
		}
		if (!NetSystem.IsServer)
		{
			this.lightningHolder.Recieve = new RecieveProxy(this.LightningHolderRecieve);
		}
		this.newTargetTimer = new ActionTimer(0.8f, 2.2f);
		if (NetSystem.IsServer)
		{
			this.lightningHolder.Value = flag;
		}
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x0004E458 File Offset: 0x0004C658
	public override void FinishedSpawning()
	{
		if (this.lightningHolder.Value)
		{
			this.LightningHolderRecieve(this.lightningHolder.Value);
		}
		if (this.lightningHolder.Value)
		{
			GameObject[] array = this.lightningHolderEffects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
		}
		base.FinishedSpawning();
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x0004E4BC File Offset: 0x0004C6BC
	protected override void Start()
	{
		base.Start();
		this.minigameController = (LightningController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
		if (!this.player.IsAI)
		{
			this.mover.SetForwardVector(Vector3.forward);
		}
		if (this.lightningHolder.Value)
		{
			this.minigameController.lightningUser = this;
		}
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x0004E550 File Offset: 0x0004C750
	private void Update()
	{
		base.UpdateController();
		if (!this.isDead && this.minigameController.Playable && base.IsOwner)
		{
			if (this.lightningHolder.Value && this.lightningTimer.Elapsed(true))
			{
				this.minigameController.DestroyTile(base.transform.position);
			}
			if (base.transform.position.y < -4.5f)
			{
				this.KillPlayer(true);
			}
		}
		if (this.agent != null)
		{
			this.agent.updatePosition = false;
			this.agent.updateRotation = false;
		}
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x0004E5F8 File Offset: 0x0004C7F8
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

	// Token: 0x0600086F RID: 2159 RVA: 0x0004E7AC File Offset: 0x0004C9AC
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

	// Token: 0x06000870 RID: 2160 RVA: 0x0004E854 File Offset: 0x0004CA54
	private LightningPlayer GetClosestPlayer(LightningPlayer filter)
	{
		float num = float.MaxValue;
		LightningPlayer result = this.targetPlayer;
		for (int i = 0; i < this.minigameController.GetPlayerCount(); i++)
		{
			LightningPlayer lightningPlayer = (LightningPlayer)this.minigameController.GetPlayer(i);
			if (!(lightningPlayer == this) && !lightningPlayer.IsDead && !(lightningPlayer == filter))
			{
				float sqrMagnitude = (lightningPlayer.transform.position - base.transform.position).sqrMagnitude;
				if (sqrMagnitude < num * 0.8f)
				{
					num = sqrMagnitude;
					result = lightningPlayer;
				}
			}
		}
		return result;
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x0004E8E8 File Offset: 0x0004CAE8
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		if (this.lightningHolder.Value)
		{
			if (base.GamePlayer.Difficulty == BotDifficulty.Hard)
			{
				this.targetPlayer = this.GetClosestPlayer(null);
			}
			else if (base.GamePlayer.Difficulty == BotDifficulty.Normal)
			{
				if (this.newTargetTimer.Elapsed(true))
				{
					LightningPlayer closestPlayer = this.GetClosestPlayer(null);
					LightningPlayer closestPlayer2 = this.GetClosestPlayer(closestPlayer);
					this.targetPlayer = ((GameManager.rand.NextDouble() > 0.5) ? closestPlayer : closestPlayer2);
				}
			}
			else if (this.newTargetTimer.Elapsed(true))
			{
				List<LightningPlayer> list = new List<LightningPlayer>();
				for (int i = 0; i < this.minigameController.players.Count; i++)
				{
					LightningPlayer lightningPlayer = (LightningPlayer)this.minigameController.GetPlayer(i);
					if (lightningPlayer != this && !lightningPlayer.IsDead)
					{
						list.Add(lightningPlayer);
					}
				}
				if (list.Count > 0)
				{
					this.targetPlayer = list[UnityEngine.Random.Range(0, list.Count)];
				}
			}
			if (this.newTargetTimer.Elapsed(true))
			{
				if (base.GamePlayer.Difficulty == BotDifficulty.Hard)
				{
					this.offset = new Vector3(ZPMath.RandomFloat(GameManager.rand, -2f, 2f), 0f, ZPMath.RandomFloat(GameManager.rand, -2f, 2f));
				}
				else if (base.GamePlayer.Difficulty == BotDifficulty.Normal)
				{
					this.offset = new Vector3(ZPMath.RandomFloat(GameManager.rand, -3.75f, 3.75f), 0f, ZPMath.RandomFloat(GameManager.rand, -3.75f, 3.75f));
				}
				else
				{
					this.offset = new Vector3(ZPMath.RandomFloat(GameManager.rand, -5f, 5f), 0f, ZPMath.RandomFloat(GameManager.rand, -5f, 5f));
				}
			}
			Vector3 vector = Vector3.zero;
			if (this.targetPlayer != null)
			{
				vector = (this.targetPlayer.transform.position + this.offset - base.transform.position).normalized;
			}
			result = new CharacterMoverInput(new Vector2(vector.x, vector.z), false, false);
		}
		else
		{
			float num = 0.36f;
			Vector2 b = new Vector2(base.transform.position.x, base.transform.position.z);
			if (this.pathUpdateTimer.Elapsed(true) && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
			{
				this.agent.SetDestination(this.targetPosition);
			}
			if ((new Vector2(this.targetPosition.x, this.targetPosition.z) - b).sqrMagnitude > num && !this.newTargetTimer.Elapsed(true))
			{
				Vector3 vector2 = this.agent.steeringTarget - base.transform.position;
				Vector2 normalized = new Vector2(vector2.x, vector2.z).normalized;
				result = new CharacterMoverInput(normalized, false, false);
			}
			else
			{
				this.GetRandomAIPos();
			}
		}
		return result;
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x0004EC40 File Offset: 0x0004CE40
	private void GetRandomAIPos()
	{
		if ((base.GamePlayer.Difficulty == BotDifficulty.Normal && GameManager.rand.NextDouble() > 0.8500000238418579) || (base.GamePlayer.Difficulty == BotDifficulty.Easy && GameManager.rand.NextDouble() > 0.699999988079071))
		{
			this.possibleTiles.Clear();
			for (int i = 0; i < this.minigameController.tiles.Length; i++)
			{
				if (!this.minigameController.tiles[i].IsDestroyed)
				{
					this.possibleTiles.Add(this.minigameController.tiles[i]);
				}
			}
			this.targetPosition = this.possibleTiles[UnityEngine.Random.Range(0, this.possibleTiles.Count)].transform.position;
		}
		LightningFloorTile lightningFloorTile;
		if (GameManager.rand.NextDouble() > 0.5 && this.minigameController.lightningUser != null)
		{
			float num = float.MinValue;
			int num2 = 0;
			for (int j = 0; j < this.minigameController.tiles.Length; j++)
			{
				if (this.minigameController.tiles[j] != null && !this.minigameController.tiles[j].IsDestroyed && this.minigameController.tiles[j].gameObject != null)
				{
					float sqrMagnitude = (this.minigameController.lightningUser.transform.position - this.minigameController.tiles[j].transform.position).sqrMagnitude;
					if (sqrMagnitude > num)
					{
						num = sqrMagnitude;
						num2 = j;
					}
				}
			}
			lightningFloorTile = this.minigameController.tiles[num2];
		}
		else
		{
			lightningFloorTile = this.minigameController.tiles[GameManager.rand.Next(0, this.minigameController.tiles.Length)];
		}
		this.targetPosition = lightningFloorTile.transform.position + new Vector3(ZPMath.RandomFloat(GameManager.rand, -2.5f, 2.5f), 0f, ZPMath.RandomFloat(GameManager.rand, -2.5f, 2.5f));
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x00009C76 File Offset: 0x00007E76
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender)
	{
		this.KillPlayer(false);
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x0004EE78 File Offset: 0x0004D078
	public void KillPlayer(bool send_rpc = true)
	{
		if (!this.isDead)
		{
			this.playerAnim.SpawnRagdoll(Vector3.zero);
			this.isDead = true;
			this.Deactivate();
			this.cameraShake.AddShake(0.3f);
			this.minigameController.PlayerDied(this);
			if (base.IsOwner && send_rpc)
			{
				base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
			}
		}
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x00009C7F File Offset: 0x00007E7F
	public override void Deactivate()
	{
		this.pointLight.enabled = false;
		base.Deactivate();
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x00009C93 File Offset: 0x00007E93
	public override void Activate()
	{
		this.pointLight.enabled = true;
		base.Activate();
	}

	// Token: 0x040006C1 RID: 1729
	public Light pointLight;

	// Token: 0x040006C2 RID: 1730
	public GameObject[] lightningHolderEffects;

	// Token: 0x040006C3 RID: 1731
	private LightningController minigameController;

	// Token: 0x040006C4 RID: 1732
	private CharacterMover mover;

	// Token: 0x040006C5 RID: 1733
	private CameraShake cameraShake;

	// Token: 0x040006C6 RID: 1734
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<bool> lightningHolder = new NetVar<bool>(false);

	// Token: 0x040006C7 RID: 1735
	private bool lightningHolderSetup;

	// Token: 0x040006C8 RID: 1736
	private ActionTimer lightningTimer = new ActionTimer(1.75f, 2.5f);

	// Token: 0x040006C9 RID: 1737
	private ActionTimer pathUpdateTimer = new ActionTimer(0.1f, 0.15f);

	// Token: 0x040006CA RID: 1738
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x040006CB RID: 1739
	private ActionTimer newOffsetTimer = new ActionTimer(2f, 3f);

	// Token: 0x040006CC RID: 1740
	private LightningPlayer targetPlayer;

	// Token: 0x040006CD RID: 1741
	private Vector3 offset;

	// Token: 0x040006CE RID: 1742
	private ActionTimer newTargetTimer = new ActionTimer(2f, 5f);

	// Token: 0x040006CF RID: 1743
	private List<LightningFloorTile> possibleTiles = new List<LightningFloorTile>();
}
