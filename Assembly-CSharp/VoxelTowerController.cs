using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000288 RID: 648
public class VoxelTowerController : MinigameController
{
	// Token: 0x060012EE RID: 4846 RVA: 0x00092488 File Offset: 0x00090688
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.lava_container = GameObject.Find("Water");
		if (NetSystem.IsServer)
		{
			base.NetSpawn("VoxelTower", Vector3.zero, Quaternion.identity, 0, null);
			base.SpawnPlayers("VoxelTowerPlayer", null);
			for (int i = 0; i < 4; i++)
			{
				GameManager.GetPlayerAt(i);
			}
		}
	}

	// Token: 0x060012EF RID: 4847 RVA: 0x00009BEC File Offset: 0x00007DEC
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x060012F0 RID: 4848 RVA: 0x0000A3A2 File Offset: 0x000085A2
	public override void OnPlayersReady()
	{
		base.OnPlayersReady();
		GameManager.CaptureInput = true;
	}

	// Token: 0x060012F1 RID: 4849 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x060012F2 RID: 4850 RVA: 0x0000F2BF File Offset: 0x0000D4BF
	public override void StartNewRound()
	{
		this.ui_timer.time_test = this.round_length;
		base.StartNewRound();
	}

	// Token: 0x060012F3 RID: 4851 RVA: 0x000924EC File Offset: 0x000906EC
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			if (NetSystem.IsServer && this.players_alive <= 1)
			{
				base.EndRound(1f, 3f, false);
			}
			if (this.lava_rise && Time.time >= this.lava_rise_time)
			{
				this.lava_container.transform.position += Vector3.up * this.lava_rise_speed * Time.deltaTime;
			}
			if (NetSystem.IsServer)
			{
				if (Time.time - this.last_fire_time > this.cur_fire_interval)
				{
					this.cur_fire_interval = UnityEngine.Random.Range(this.min_fire_interval, this.max_fire_interval);
					this.last_fire_time = Time.time;
					for (int i = 0; i < 8; i++)
					{
						if (!this.playerOrRandomPoint && GameManager.IsPlayerInSlot(i))
						{
							this.CreateBomb((short)i);
						}
						else
						{
							this.CreateBomb(-1);
						}
						this.playerOrRandomPoint = !this.playerOrRandomPoint;
					}
				}
				if (!this.lava_rise && Time.time - this.round_begin_time >= this.lava_rise_message_time)
				{
					this.StartLava(NetSystem.NetTime.GameTime);
				}
			}
		}
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x00092618 File Offset: 0x00090818
	public override void BuildResults()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			if (!((VoxelTowerPlayer)this.players[i]).IsDead)
			{
				this.players[i].Score = (short)(this.players.Count - 1);
			}
		}
		base.BuildResults();
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x00092678 File Offset: 0x00090878
	public override bool HasLoadedLocally()
	{
		if (this.players.Count >= GameManager.GetPlayerCount())
		{
			return this.voxel_towers[0] != null && this.voxel_towers[0].voxel_grid != null && this.voxel_towers[0].voxel_grid.data != null && this.voxel_towers[0].voxel_grid.buildCompleted;
		}
		Debug.Log("Not Loaded : " + this.players.Count.ToString() + " < " + GameManager.GetPlayerCount().ToString());
		return false;
	}

	// Token: 0x060012F6 RID: 4854 RVA: 0x0000A3B0 File Offset: 0x000085B0
	public override void ReleaseMinigame()
	{
		GameManager.CaptureInput = false;
		base.ReleaseMinigame();
	}

	// Token: 0x060012F7 RID: 4855 RVA: 0x0000F2D8 File Offset: 0x0000D4D8
	public void PlayerDied(VoxelTowerPlayer player)
	{
		if (NetSystem.IsServer)
		{
			player.Score = (short)(this.players.Count - this.players_alive);
		}
		this.players_alive--;
	}

	// Token: 0x060012F8 RID: 4856 RVA: 0x0000F308 File Offset: 0x0000D508
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void StartLavaRPC(NetPlayer sender, float net_time)
	{
		this.StartLava(net_time);
	}

	// Token: 0x060012F9 RID: 4857 RVA: 0x00092720 File Offset: 0x00090920
	private void StartLava(float net_time)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("StartLavaRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				net_time
			});
		}
		float num = NetSystem.NetTime.GameTime - net_time;
		this.lava_rise_time = Time.time + (this.lava_rise_start_time_offset - num);
		this.lava_rise = true;
	}

	// Token: 0x060012FA RID: 4858 RVA: 0x0000F311 File Offset: 0x0000D511
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void PickupRPC(NetPlayer sender, short id, short player_slot)
	{
		this.Pickup(id, player_slot);
	}

	// Token: 0x060012FB RID: 4859 RVA: 0x00092778 File Offset: 0x00090978
	private void Pickup(short id, short player_slot)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("PickupRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				id,
				player_slot
			});
		}
		VoxelTowerBarrel barrel = this.GetBarrel(id);
		if (barrel != null)
		{
			barrel.Pickup(player_slot);
		}
	}

	// Token: 0x060012FC RID: 4860 RVA: 0x0000F31B File Offset: 0x0000D51B
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void AttemptPickupRPC(NetPlayer sender, short id, short playerSlot)
	{
		this.AttemptPickup(id, playerSlot);
	}

	// Token: 0x060012FD RID: 4861 RVA: 0x000927C8 File Offset: 0x000909C8
	public void AttemptPickup(short id, short playerSlot)
	{
		VoxelTowerBarrel barrel = this.GetBarrel(id);
		if (barrel != null && ((VoxelTowerPlayer)this.players[(int)playerSlot]).held == null && barrel.holding_player == -1 && !barrel.thrown)
		{
			if (NetSystem.IsServer)
			{
				this.Pickup(id, playerSlot);
				return;
			}
			base.SendRPC("AttemptPickupRPC", NetRPCDelivery.RELIABLE_UNORDERED, new object[]
			{
				id,
				playerSlot
			});
		}
	}

	// Token: 0x060012FE RID: 4862 RVA: 0x0009284C File Offset: 0x00090A4C
	private VoxelTowerBarrel GetBarrel(short id)
	{
		for (int i = 0; i < this.barrels.Count; i++)
		{
			if (this.barrels[i].id == id)
			{
				return this.barrels[i];
			}
		}
		return null;
	}

	// Token: 0x060012FF RID: 4863 RVA: 0x0000F325 File Offset: 0x0000D525
	public Vector3 RandomAngularVelocity(float range)
	{
		return new Vector3(UnityEngine.Random.Range(-range, range), UnityEngine.Random.Range(-range, range), UnityEngine.Random.Range(-range, range));
	}

	// Token: 0x06001300 RID: 4864 RVA: 0x0000F344 File Offset: 0x0000D544
	public void Throw(short id, Vector3 start_position, Vector3 target_position)
	{
		this.RelayThrow(id, start_position, this.GetThrowDir(start_position, target_position, 17f), this.RandomAngularVelocity(this.angular_vel_range), false);
	}

	// Token: 0x06001301 RID: 4865 RVA: 0x00092894 File Offset: 0x00090A94
	private Vector3 GetThrowDir(Vector3 start_position, Vector3 target_position, float velocity)
	{
		Quaternion quaternion = Quaternion.LookRotation((target_position - start_position).normalized, Vector3.up);
		return Quaternion.Euler(this.GetAngle(start_position, target_position, velocity), quaternion.eulerAngles.y, quaternion.eulerAngles.z) * Vector3.forward;
	}

	// Token: 0x06001302 RID: 4866 RVA: 0x0000F368 File Offset: 0x0000D568
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void RelayThrowRPC(NetPlayer sender, short id, Vector3 start_position, Vector3 dir, Vector3 angular_velocity)
	{
		this.RelayThrow(id, start_position, dir, angular_velocity, true);
	}

	// Token: 0x06001303 RID: 4867 RVA: 0x000928EC File Offset: 0x00090AEC
	public void RelayThrow(short id, Vector3 start_position, Vector3 dir, Vector3 angular_velocity, bool proxy)
	{
		if (!proxy)
		{
			base.SendRPC("RelayThrowRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				id,
				start_position,
				dir,
				angular_velocity
			});
		}
		VoxelTowerBarrel barrel = this.GetBarrel(id);
		if (barrel != null)
		{
			barrel.Throw(start_position, dir, angular_velocity, 17f);
		}
	}

	// Token: 0x06001304 RID: 4868 RVA: 0x0000F377 File Offset: 0x0000D577
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void CreateBombRPC(NetPlayer sender, short id, short targetPlayer, Vector3 start_position, Vector3 dir, Vector3 angular_velocity, float explode_net_time)
	{
		this.CreateBomb(id, targetPlayer, start_position, dir, angular_velocity, explode_net_time);
	}

	// Token: 0x06001305 RID: 4869 RVA: 0x00092954 File Offset: 0x00090B54
	public void CreateBomb(short player_slot)
	{
		Vector3 target_position = (player_slot == -1) ? this.FindBombTarget2(0) : this.FindBombTarget((int)player_slot);
		this.CreateBomb(this.bomb_id, player_slot, this.barrel_start_position, this.GetThrowDir(this.barrel_start_position, target_position, 17f), this.RandomAngularVelocity(this.angular_vel_range), NetSystem.NetTime.GameTime + UnityEngine.Random.Range(this.min_explode_time, this.max_explode_time));
		this.bomb_id += 1;
	}

	// Token: 0x06001306 RID: 4870 RVA: 0x000929D4 File Offset: 0x00090BD4
	private void CreateBomb(short id, short targetPlayer, Vector3 start_position, Vector3 dir, Vector3 angular_velocity, float explode_net_time)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("CreateBombRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				id,
				targetPlayer,
				start_position,
				dir,
				angular_velocity,
				explode_net_time
			});
		}
		VoxelTowerBarrel component = base.Spawn(this.barrel_prefab, start_position, Quaternion.identity).GetComponent<VoxelTowerBarrel>();
		component.Setup(id, targetPlayer, explode_net_time);
		this.barrels.Add(component);
		VoxelTowerBarrel barrel = this.GetBarrel(id);
		if (barrel != null)
		{
			barrel.Throw(start_position, dir, angular_velocity, 17f);
		}
	}

	// Token: 0x06001307 RID: 4871 RVA: 0x00092A84 File Offset: 0x00090C84
	public Vector3 FindBombTarget(int i)
	{
		Vector3 b = ZPMath.RandomPointInUnitSphere(GameManager.rand);
		b.y = 0f;
		return this.players[i].transform.position + b;
	}

	// Token: 0x06001308 RID: 4872 RVA: 0x00092AC4 File Offset: 0x00090CC4
	public Vector3 FindBombTarget2(int i)
	{
		VoxelGrid voxel_grid = this.voxel_towers[i].voxel_grid;
		Vector3 vector = Vector3.zero;
		int j = 0;
		int num = 1000;
		while (j < num)
		{
			vector = new Vector3((float)UnityEngine.Random.Range(1, (int)voxel_grid.gridSize.x - 1), 0f, (float)UnityEngine.Random.Range(1, (int)voxel_grid.gridSize.z - 1));
			bool flag = false;
			for (int k = (int)voxel_grid.gridSize.y - 1; k >= 0; k--)
			{
				if (voxel_grid.data[(int)vector.x][k][(int)vector.z] < 0)
				{
					vector.y = (float)k;
					flag = true;
					break;
				}
			}
			if (flag && vector.y >= 6f)
			{
				break;
			}
			j++;
		}
		vector -= (voxel_grid.gridSize - Vector3.one) / 2f;
		vector *= voxel_grid.voxelSize;
		return vector + voxel_grid.transform.position;
	}

	// Token: 0x06001309 RID: 4873 RVA: 0x00092BD0 File Offset: 0x00090DD0
	private float GetAngle(Vector3 targetTransform, Vector3 barrelTransform, float projectileSpeed)
	{
		float num = targetTransform.y - barrelTransform.y;
		targetTransform.y = (barrelTransform.y = 0f);
		float magnitude = (targetTransform - barrelTransform).magnitude;
		float y = Physics.gravity.y;
		float num2 = projectileSpeed * projectileSpeed * projectileSpeed * projectileSpeed - y * (y * (magnitude * magnitude) + 2f * num * (projectileSpeed * projectileSpeed));
		if (num2 < 0f)
		{
			Debug.Log("No Solution");
			return 0f;
		}
		num2 = Mathf.Sqrt(num2);
		float num3 = Mathf.Atan((projectileSpeed * projectileSpeed + num2) / (y * magnitude));
		Mathf.Atan((projectileSpeed * projectileSpeed - num2) / (y * magnitude));
		return num3 * 57.29578f;
	}

	// Token: 0x0400143B RID: 5179
	[Header("Minigame specific attributes")]
	public VoxelTowerGrid[] voxel_towers = new VoxelTowerGrid[4];

	// Token: 0x0400143C RID: 5180
	public GameObject barrel_prefab;

	// Token: 0x0400143D RID: 5181
	public List<VoxelTowerBarrel> barrels = new List<VoxelTowerBarrel>();

	// Token: 0x0400143E RID: 5182
	public float angular_vel_range = 5f;

	// Token: 0x0400143F RID: 5183
	public GameObject camPrefab;

	// Token: 0x04001440 RID: 5184
	private Vector3 barrel_start_position = new Vector3(0f, 4f, 0f);

	// Token: 0x04001441 RID: 5185
	private short bomb_id;

	// Token: 0x04001442 RID: 5186
	private float min_explode_time = 8f;

	// Token: 0x04001443 RID: 5187
	private float max_explode_time = 11.5f;

	// Token: 0x04001444 RID: 5188
	private float last_fire_time;

	// Token: 0x04001445 RID: 5189
	private float min_fire_interval = 2.1f;

	// Token: 0x04001446 RID: 5190
	private float max_fire_interval = 2.8f;

	// Token: 0x04001447 RID: 5191
	private float cur_fire_interval;

	// Token: 0x04001448 RID: 5192
	private float lava_rise_message_time = 85f;

	// Token: 0x04001449 RID: 5193
	private float lava_rise_start_time_offset = 5f;

	// Token: 0x0400144A RID: 5194
	private float lava_rise_time;

	// Token: 0x0400144B RID: 5195
	private bool lava_rise;

	// Token: 0x0400144C RID: 5196
	private float lava_rise_speed = 0.1f;

	// Token: 0x0400144D RID: 5197
	private GameObject lava_container;

	// Token: 0x0400144E RID: 5198
	private bool playerOrRandomPoint;
}
