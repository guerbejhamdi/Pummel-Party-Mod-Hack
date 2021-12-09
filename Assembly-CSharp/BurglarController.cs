using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000181 RID: 385
public class BurglarController : MinigameController
{
	// Token: 0x06000AE4 RID: 2788 RVA: 0x0000AFFC File Offset: 0x000091FC
	public void Awake()
	{
		this.FindItemManager();
		this.FindVehicleSpawns();
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x0000B00A File Offset: 0x0000920A
	private void FindItemManager()
	{
		if (this.m_itemManager == null)
		{
			this.m_itemManager = UnityEngine.Object.FindObjectOfType<BurglarItemManager>();
		}
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x0005F110 File Offset: 0x0005D310
	private void FindVehicleSpawns()
	{
		if (this.m_vehicleSpawns == null || this.m_vehicleSpawns.Length == 0)
		{
			this.m_vehicleSpawns = UnityEngine.Object.FindObjectsOfType<BurglarVehicleSpawn>();
			BurglarVehicleSpawn[] vehicleSpawns = this.m_vehicleSpawns;
			for (int i = 0; i < vehicleSpawns.Length; i++)
			{
				vehicleSpawns[i].NextSpawnTime = Time.time + UnityEngine.Random.Range(2f, 6f);
			}
		}
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x0005F16C File Offset: 0x0005D36C
	private void OnDestroy()
	{
		foreach (GameObject gameObject in this.m_spawnedVehicles)
		{
			if (!(gameObject == null))
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		this.m_spawnedVehicles.Clear();
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x0005F1D4 File Offset: 0x0005D3D4
	public Vector3 GetRandomNavMeshPoint()
	{
		if (this.binaryTree == null)
		{
			this.triangulation = NavMesh.CalculateTriangulation();
			if (this.triangulation.vertices.Length != 0)
			{
				List<float> list = new List<float>();
				for (int i = 0; i < this.triangulation.indices.Length / 3; i++)
				{
					int num = i * 3;
					Vector3 vector = this.triangulation.vertices[this.triangulation.indices[num]];
					Vector3 vector2 = this.triangulation.vertices[this.triangulation.indices[num + 1]];
					Vector3 vector3 = this.triangulation.vertices[this.triangulation.indices[num + 2]];
					float num2 = Vector3.Distance(vector, vector2);
					float num3 = Vector3.Distance(vector2, vector3);
					float num4 = Vector3.Distance(vector3, vector);
					float num5 = (num2 + num3 + num4) / 2f;
					float num6 = Mathf.Sqrt(num5 * (num5 - num2) * (num5 - num3) * (num5 - num4));
					list.Add(this.totalArea);
					this.totalArea += num6;
				}
				this.binaryTree = new BinaryTree(list.ToArray());
			}
		}
		if (this.binaryTree != null)
		{
			float p = ZPMath.RandomFloat(this.rand, 0f, this.totalArea);
			int num7 = this.binaryTree.FindPoint(p) * 3;
			Vector3[] vertices = this.triangulation.vertices;
			int[] indices = this.triangulation.indices;
			return ZPMath.RandomTrianglePoint(vertices[indices[num7]], vertices[indices[num7 + 1]], vertices[indices[num7 + 2]], this.rand);
		}
		return Vector3.zero;
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x0000B025 File Offset: 0x00009225
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.rand = new System.Random(UnityEngine.Random.Range(0, int.MaxValue));
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("BurglarPlayer", null);
		}
		this.triangulation = NavMesh.CalculateTriangulation();
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x0005F38C File Offset: 0x0005D58C
	public override void StartMinigame()
	{
		this.FindItemManager();
		this.FindVehicleSpawns();
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		if (NetSystem.IsServer)
		{
			this.SpawnInitialItems();
		}
		base.StartMinigame();
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x0005F3F0 File Offset: 0x0005D5F0
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			if (this.ui_timer.time_test <= 0f)
			{
				base.EndRound(1f, 1f, false);
			}
			if (this.ui_timer.time_test > 3f)
			{
				for (int i = this.m_respawnItemTime.Count - 1; i >= 0; i--)
				{
					if (Time.time >= this.m_respawnItemTime[i])
					{
						this.SpawnRandomItem();
						this.m_respawnItemTime.RemoveAt(i);
					}
				}
				foreach (BurglarVehicleSpawn burglarVehicleSpawn in this.m_vehicleSpawns)
				{
					if (Time.time > burglarVehicleSpawn.NextSpawnTime)
					{
						this.SpawnVehicle(burglarVehicleSpawn.SpawnIndex, UnityEngine.Random.Range(0, this.m_vehiclePfbs.Length), UnityEngine.Random.Range(8f, 12f));
						burglarVehicleSpawn.NextSpawnTime = Time.time + UnityEngine.Random.Range(2f, 6f);
					}
				}
			}
		}
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x0005F4F4 File Offset: 0x0005D6F4
	public void SpawnVehicle(int spawnIndex, int vehicleIndex, float speed)
	{
		this.FindVehicleSpawns();
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSpawnVehicle", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				(short)spawnIndex,
				(short)vehicleIndex,
				speed
			});
		}
		BurglarVehicleSpawn burglarVehicleSpawn = null;
		foreach (BurglarVehicleSpawn burglarVehicleSpawn2 in this.m_vehicleSpawns)
		{
			if (burglarVehicleSpawn2.SpawnIndex == spawnIndex)
			{
				burglarVehicleSpawn = burglarVehicleSpawn2;
				break;
			}
		}
		if (burglarVehicleSpawn != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_vehiclePfbs[vehicleIndex], burglarVehicleSpawn.transform.position, burglarVehicleSpawn.transform.rotation);
			gameObject.GetComponent<BurglarVehicle>().SetSpeed(speed);
			this.m_spawnedVehicles.Add(gameObject);
			UnityEngine.Object.Destroy(gameObject, 10f);
			return;
		}
		Debug.LogError("Vehicle spawn was null with id = " + spawnIndex.ToString());
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x0000B061 File Offset: 0x00009261
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawnVehicle(NetPlayer sender, short spawnIndex, short vehicleIndex, float speed)
	{
		this.SpawnVehicle((int)spawnIndex, (int)vehicleIndex, speed);
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x0005F5D0 File Offset: 0x0005D7D0
	private void DestroyItem(BurglarItem itm)
	{
		if (itm == null)
		{
			return;
		}
		this.FindItemManager();
		if (this.m_itemManager == null)
		{
			return;
		}
		this.m_itemManager.DestroyItem(itm.ItemID);
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCServerDestroyItem", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				(short)itm.ItemID
			});
			this.m_respawnItemTime.Add(Time.time + UnityEngine.Random.Range(2.5f, 5.5f));
			return;
		}
		base.SendRPC("RPCClientDestroyItem", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			(short)itm.ItemID
		});
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x0005F678 File Offset: 0x0005D878
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void RPCClientDestroyItem(NetPlayer sender, short itemID)
	{
		this.FindItemManager();
		if (this.m_itemManager == null)
		{
			return;
		}
		if (this.m_itemManager.IsItemActive((int)itemID))
		{
			this.m_respawnItemTime.Add(Time.time + UnityEngine.Random.Range(2.5f, 5.5f));
			this.m_itemManager.DestroyItem((int)itemID);
			if (NetSystem.IsServer)
			{
				base.SendRPC("RPCServerDestroyItem", NetRPCDelivery.RELIABLE_ORDERED, new object[]
				{
					itemID
				});
			}
		}
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x0000B06D File Offset: 0x0000926D
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCServerDestroyItem(NetPlayer sender, short itemID)
	{
		this.FindItemManager();
		if (this.m_itemManager == null)
		{
			return;
		}
		if (this.m_itemManager.IsItemActive((int)itemID))
		{
			this.m_itemManager.DestroyItem((int)itemID);
		}
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x0000B09E File Offset: 0x0000929E
	public bool CanTakeItem(BurglarItem itm)
	{
		if (itm == null)
		{
			return false;
		}
		if (this.m_itemManager.IsItemActive(itm))
		{
			this.DestroyItem(itm);
			return true;
		}
		return false;
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x0000B0C3 File Offset: 0x000092C3
	public void TakeItem(BurglarItem itm)
	{
		if (itm == null)
		{
			return;
		}
		if (this.m_itemManager.IsItemActive(itm))
		{
			this.DestroyItem(itm);
		}
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x0005F6F8 File Offset: 0x0005D8F8
	public void SpawnInitialItems()
	{
		if (!NetSystem.IsServer)
		{
			return;
		}
		int num = 9;
		for (int i = 0; i < num; i++)
		{
			this.SpawnRandomItem();
		}
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x0005F724 File Offset: 0x0005D924
	public void SpawnRandomItem()
	{
		if (this.m_itemManager == null)
		{
			this.m_itemManager = UnityEngine.Object.FindObjectOfType<BurglarItemManager>();
		}
		int itemID = this.m_itemID;
		int freeSpawnPointIndex = this.m_itemManager.GetFreeSpawnPointIndex();
		int randomItemIndex = this.m_itemManager.GetRandomItemIndex();
		this.SpawnItem(itemID, freeSpawnPointIndex, randomItemIndex);
		this.m_itemID++;
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x0005F780 File Offset: 0x0005D980
	public void SpawnItem(int itemID, int spawnIndex, int itemIndex)
	{
		this.FindItemManager();
		if (this.m_itemManager == null)
		{
			return;
		}
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSpawnItem", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				(short)itemID,
				(byte)spawnIndex,
				(byte)itemIndex
			});
		}
		this.m_itemManager.SpawnItem(itemID, spawnIndex, itemIndex);
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x0000B0E4 File Offset: 0x000092E4
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawnItem(NetPlayer sender, short itemID, byte spawnIndex, byte itemIndex)
	{
		this.SpawnItem((int)itemID, (int)spawnIndex, (int)itemIndex);
	}

	// Token: 0x04000A11 RID: 2577
	[SerializeField]
	private GameObject m_presentPfb;

	// Token: 0x04000A12 RID: 2578
	[SerializeField]
	private AudioClip m_grabPresentClip;

	// Token: 0x04000A13 RID: 2579
	[SerializeField]
	private AudioClip m_swipePresentClip;

	// Token: 0x04000A14 RID: 2580
	[SerializeField]
	private GameObject[] m_vehiclePfbs;

	// Token: 0x04000A15 RID: 2581
	private System.Random rand;

	// Token: 0x04000A16 RID: 2582
	private NavMeshTriangulation triangulation;

	// Token: 0x04000A17 RID: 2583
	private BinaryTree binaryTree;

	// Token: 0x04000A18 RID: 2584
	private float totalArea;

	// Token: 0x04000A19 RID: 2585
	private BurglarItemManager m_itemManager;

	// Token: 0x04000A1A RID: 2586
	private BurglarPlayerGoal[] m_goals;

	// Token: 0x04000A1B RID: 2587
	private BurglarVehicleSpawn[] m_vehicleSpawns;

	// Token: 0x04000A1C RID: 2588
	private List<GameObject> m_spawnedVehicles = new List<GameObject>();

	// Token: 0x04000A1D RID: 2589
	private List<float> m_respawnItemTime = new List<float>();

	// Token: 0x04000A1E RID: 2590
	private int m_itemID = 1;
}
