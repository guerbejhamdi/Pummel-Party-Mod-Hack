using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001CD RID: 461
public class MemoryMenuController : MinigameController
{
	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06000D3D RID: 3389 RVA: 0x0000C146 File Offset: 0x0000A346
	public bool ExplanationDone
	{
		get
		{
			return this.m_explanationDone;
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06000D3E RID: 3390 RVA: 0x0000C14E File Offset: 0x0000A34E
	public List<short> TargetFoods
	{
		get
		{
			return this.m_targetFoods;
		}
	}

	// Token: 0x06000D3F RID: 3391 RVA: 0x0000C156 File Offset: 0x0000A356
	public void Awake()
	{
		this.FindItemManager();
		this.m_memoryUI = UnityEngine.Object.FindObjectOfType<MemoryMenuUI>();
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x0000C169 File Offset: 0x0000A369
	private void FindItemManager()
	{
		if (this.m_itemManager == null)
		{
			this.m_itemManager = UnityEngine.Object.FindObjectOfType<MemoryMenuItemManager>();
		}
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x0006CEC0 File Offset: 0x0006B0C0
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

	// Token: 0x06000D42 RID: 3394 RVA: 0x0006CF28 File Offset: 0x0006B128
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

	// Token: 0x06000D43 RID: 3395 RVA: 0x0000C184 File Offset: 0x0000A384
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.rand = new System.Random(UnityEngine.Random.Range(0, int.MaxValue));
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("MemoryMenuPlayer", null);
		}
		this.triangulation = NavMesh.CalculateTriangulation();
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x0006D0E0 File Offset: 0x0006B2E0
	public override void StartMinigame()
	{
		this.FindItemManager();
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.StartMinigame();
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x0000C1C0 File Offset: 0x0000A3C0
	public override void RoundEnded()
	{
		this.ClearItems();
		base.RoundEnded();
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x0000C1CE File Offset: 0x0000A3CE
	public override void ResetRound()
	{
		this.m_isNewRound = true;
		base.ResetRound();
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x0000C1DD File Offset: 0x0000A3DD
	private void OnNewRound()
	{
		this.ui_timer.time_test = this.round_length;
		this.m_explanationDone = false;
		Debug.LogError("Round Starting");
		if (NetSystem.IsServer)
		{
			this.GetTargetFoods();
			this.SpawnInitialItems();
		}
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x0006D134 File Offset: 0x0006B334
	private void GetTargetFoods()
	{
		this.FindItemManager();
		if (this.m_itemManager == null)
		{
			return;
		}
		List<short> randomItems = this.m_itemManager.GetRandomItems(this.m_foodsToRemember);
		this.m_foodsToRemember++;
		this.SetAndShowTargetFoods(randomItems);
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x0006D180 File Offset: 0x0006B380
	private void SetAndShowTargetFoods(List<short> targetFoods)
	{
		this.m_targetFoods = targetFoods;
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCShowTargetFoods", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				targetFoods.ToArray()
			});
		}
		List<Sprite> list = new List<Sprite>();
		foreach (short id in targetFoods)
		{
			list.Add(this.m_itemManager.GetItemIcon(id));
		}
		this.m_memoryUI.ShowMemoryPanel(list);
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x0000C214 File Offset: 0x0000A414
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCShowTargetFoods(NetPlayer sender, short[] foodIDs)
	{
		this.FindItemManager();
		if (this.m_itemManager == null)
		{
			return;
		}
		this.SetAndShowTargetFoods(new List<short>(foodIDs));
	}

	// Token: 0x06000D4C RID: 3404 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000D4D RID: 3405 RVA: 0x0006D214 File Offset: 0x0006B414
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			if (this.ui_timer.time_test <= 20f && !this.m_explanationDone)
			{
				this.m_explanationDone = true;
			}
			if (NetSystem.IsServer)
			{
				if (this.m_isNewRound)
				{
					this.OnNewRound();
					this.m_isNewRound = false;
				}
				else if (this.ui_timer.time_test <= 0f)
				{
					base.EndRound(0f, 1f, false);
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
					return;
				}
			}
			else if (this.m_isNewRound)
			{
				this.OnNewRound();
				this.m_isNewRound = false;
			}
		}
	}

	// Token: 0x06000D4E RID: 3406 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x0006D2F8 File Offset: 0x0006B4F8
	private void DestroyItem(MemoryMenuItem itm)
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
		this.m_itemManager.DestroyItem(itm.ItemID, false);
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCServerDestroyItem", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				(short)itm.ItemID
			});
			this.m_respawnItemTime.Add(Time.time + UnityEngine.Random.Range(1f, 2f));
			return;
		}
		base.SendRPC("RPCClientDestroyItem", NetRPCDelivery.RELIABLE_ORDERED, new object[]
		{
			(short)itm.ItemID
		});
	}

	// Token: 0x06000D51 RID: 3409 RVA: 0x0000C237 File Offset: 0x0000A437
	private void ClearItems()
	{
		this.FindItemManager();
		if (this.m_itemManager == null)
		{
			return;
		}
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCClearItems", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		this.m_itemManager.ClearItems();
	}

	// Token: 0x06000D52 RID: 3410 RVA: 0x0000C271 File Offset: 0x0000A471
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCClearItems(NetPlayer sender)
	{
		this.ClearItems();
	}

	// Token: 0x06000D53 RID: 3411 RVA: 0x0006D3A0 File Offset: 0x0006B5A0
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
			this.m_itemManager.DestroyItem((int)itemID, false);
			if (NetSystem.IsServer)
			{
				base.SendRPC("RPCServerDestroyItem", NetRPCDelivery.RELIABLE_ORDERED, new object[]
				{
					itemID
				});
			}
		}
	}

	// Token: 0x06000D54 RID: 3412 RVA: 0x0000C279 File Offset: 0x0000A479
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
			this.m_itemManager.DestroyItem((int)itemID, false);
		}
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x0000C2AB File Offset: 0x0000A4AB
	public bool CanTakeItem(MemoryMenuItem itm)
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

	// Token: 0x06000D56 RID: 3414 RVA: 0x0000C2D0 File Offset: 0x0000A4D0
	public void TakeItem(MemoryMenuItem itm)
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

	// Token: 0x06000D57 RID: 3415 RVA: 0x0006D420 File Offset: 0x0006B620
	public void SpawnInitialItems()
	{
		if (!NetSystem.IsServer)
		{
			return;
		}
		int num = GameManager.PlayerList.Count - 1;
		List<short> list = new List<short>();
		for (int i = 0; i < num; i++)
		{
			foreach (short item in this.m_targetFoods)
			{
				list.Add(item);
			}
		}
		int num2 = 21;
		while (list.Count > 0)
		{
			this.SpawnSpecificItem((int)list[0]);
			list.RemoveAt(0);
			num2--;
		}
		for (int j = 0; j < num2; j++)
		{
			this.SpawnRandomItem();
		}
	}

	// Token: 0x06000D58 RID: 3416 RVA: 0x0000C2F1 File Offset: 0x0000A4F1
	public void SpawnRandomItem()
	{
		if (this.m_itemManager == null)
		{
			this.m_itemManager = UnityEngine.Object.FindObjectOfType<MemoryMenuItemManager>();
		}
		this.SpawnSpecificItem(this.m_itemManager.GetRandomItemIndex());
	}

	// Token: 0x06000D59 RID: 3417 RVA: 0x0006D4DC File Offset: 0x0006B6DC
	public void SpawnSpecificItem(int itemIndex)
	{
		if (this.m_itemManager == null)
		{
			this.m_itemManager = UnityEngine.Object.FindObjectOfType<MemoryMenuItemManager>();
		}
		int itemID = this.m_itemID;
		int freeSpawnPointIndex = this.m_itemManager.GetFreeSpawnPointIndex();
		if (freeSpawnPointIndex != -1)
		{
			this.SpawnItem(itemID, freeSpawnPointIndex, itemIndex);
			this.m_itemID++;
		}
	}

	// Token: 0x06000D5A RID: 3418 RVA: 0x0006D530 File Offset: 0x0006B730
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

	// Token: 0x06000D5B RID: 3419 RVA: 0x0000C31D File Offset: 0x0000A51D
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawnItem(NetPlayer sender, short itemID, byte spawnIndex, byte itemIndex)
	{
		this.SpawnItem((int)itemID, (int)spawnIndex, (int)itemIndex);
	}

	// Token: 0x04000CB4 RID: 3252
	[SerializeField]
	private GameObject m_presentPfb;

	// Token: 0x04000CB5 RID: 3253
	[SerializeField]
	private AudioClip m_grabPresentClip;

	// Token: 0x04000CB6 RID: 3254
	[SerializeField]
	private AudioClip m_swipePresentClip;

	// Token: 0x04000CB7 RID: 3255
	[SerializeField]
	private GameObject[] m_vehiclePfbs;

	// Token: 0x04000CB8 RID: 3256
	private bool m_explanationDone;

	// Token: 0x04000CB9 RID: 3257
	private List<short> m_targetFoods = new List<short>();

	// Token: 0x04000CBA RID: 3258
	private System.Random rand;

	// Token: 0x04000CBB RID: 3259
	private NavMeshTriangulation triangulation;

	// Token: 0x04000CBC RID: 3260
	private BinaryTree binaryTree;

	// Token: 0x04000CBD RID: 3261
	private float totalArea;

	// Token: 0x04000CBE RID: 3262
	public MemoryMenuItemManager m_itemManager;

	// Token: 0x04000CBF RID: 3263
	private MemoryMenuPlayerGoal[] m_goals;

	// Token: 0x04000CC0 RID: 3264
	private List<GameObject> m_spawnedVehicles = new List<GameObject>();

	// Token: 0x04000CC1 RID: 3265
	private List<float> m_respawnItemTime = new List<float>();

	// Token: 0x04000CC2 RID: 3266
	private MemoryMenuUI m_memoryUI;

	// Token: 0x04000CC3 RID: 3267
	private int m_foodsToRemember = 2;

	// Token: 0x04000CC4 RID: 3268
	private bool m_isNewRound = true;

	// Token: 0x04000CC5 RID: 3269
	private int m_itemID = 1;
}
