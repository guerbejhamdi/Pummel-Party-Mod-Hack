using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public class MemoryMenuItemManager : MonoBehaviour
{
	// Token: 0x06000D70 RID: 3440 RVA: 0x0006D66C File Offset: 0x0006B86C
	public void Awake()
	{
		foreach (MemoryMenuItemSpawnPoint memoryMenuItemSpawnPoint in UnityEngine.Object.FindObjectsOfType<MemoryMenuItemSpawnPoint>())
		{
			this.m_spawnPoints.Add(memoryMenuItemSpawnPoint.Index, memoryMenuItemSpawnPoint);
			this.m_freePoints.Add(memoryMenuItemSpawnPoint);
		}
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x0006D6B0 File Offset: 0x0006B8B0
	public List<short> GetRandomItems(int count)
	{
		List<short> list = new List<short>();
		for (int i = 0; i < count; i++)
		{
			list.Add((short)UnityEngine.Random.Range(0, this.m_itemPrefabs.Length));
		}
		return list;
	}

	// Token: 0x06000D72 RID: 3442 RVA: 0x0000C3CD File Offset: 0x0000A5CD
	public Sprite GetItemIcon(short id)
	{
		return this.m_itemIcons[(int)id];
	}

	// Token: 0x06000D73 RID: 3443 RVA: 0x0006D6E8 File Offset: 0x0006B8E8
	public void OnDestroy()
	{
		foreach (MemoryMenuItem memoryMenuItem in this.m_spawnedItems)
		{
			if (!(memoryMenuItem == null) && !(memoryMenuItem.gameObject == null))
			{
				UnityEngine.Object.Destroy(memoryMenuItem.gameObject);
			}
		}
	}

	// Token: 0x06000D74 RID: 3444 RVA: 0x0000C3D7 File Offset: 0x0000A5D7
	public List<MemoryMenuItem> GetSpawnedItems()
	{
		return this.m_spawnedItems;
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x0000C3DF File Offset: 0x0000A5DF
	public bool IsItemActive(MemoryMenuItem item)
	{
		return !(item == null) && this.IsItemActive(item.ItemID);
	}

	// Token: 0x06000D76 RID: 3446 RVA: 0x0006D758 File Offset: 0x0006B958
	public bool IsItemActive(int id)
	{
		MemoryMenuItem x = null;
		return this.m_spawnedItemMap.TryGetValue(id, out x) && x != null;
	}

	// Token: 0x06000D77 RID: 3447 RVA: 0x0006D780 File Offset: 0x0006B980
	public void SpawnItem(int itemID, int spawnIndex, int itemIndex)
	{
		if (this.m_spawnedItemMap.ContainsKey(itemID))
		{
			Debug.LogError("itemID already in spawn item map, something weird happened so we won't spawn the item..");
			return;
		}
		GameObject original = this.m_itemPrefabs[itemIndex];
		MemoryMenuItemSpawnPoint memoryMenuItemSpawnPoint = null;
		if (!this.m_spawnPoints.TryGetValue(spawnIndex, out memoryMenuItemSpawnPoint))
		{
			Debug.LogError("Unable to find item spawn point " + spawnIndex.ToString() + " for minigame.");
			return;
		}
		memoryMenuItemSpawnPoint.ItemID = (short)itemID;
		MemoryMenuItem component = UnityEngine.Object.Instantiate<GameObject>(original, memoryMenuItemSpawnPoint.transform.position, memoryMenuItemSpawnPoint.transform.rotation).GetComponent<MemoryMenuItem>();
		component.ItemID = itemID;
		this.m_spawnedItems.Add(component);
		this.m_spawnedItemMap.Add(itemID, component);
	}

	// Token: 0x06000D78 RID: 3448 RVA: 0x0006D828 File Offset: 0x0006BA28
	public void ClearItems()
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, MemoryMenuItem> keyValuePair in this.m_spawnedItemMap)
		{
			list.Add(keyValuePair.Key);
		}
		foreach (int itemID in list)
		{
			this.DestroyItem(itemID, true);
		}
	}

	// Token: 0x06000D79 RID: 3449 RVA: 0x0006D8C8 File Offset: 0x0006BAC8
	public void DestroyItem(int itemID, bool despawnAnim = false)
	{
		if (this.m_spawnedItemMap.ContainsKey(itemID))
		{
			MemoryMenuItem memoryMenuItem = this.m_spawnedItemMap[itemID];
			if (memoryMenuItem != null)
			{
				foreach (KeyValuePair<int, MemoryMenuItemSpawnPoint> keyValuePair in this.m_spawnPoints)
				{
					if ((int)keyValuePair.Value.ItemID == itemID)
					{
						keyValuePair.Value.ItemID = 0;
						this.m_freePoints.Add(keyValuePair.Value);
					}
				}
				this.m_spawnedItems.Remove(memoryMenuItem);
				if (despawnAnim)
				{
					LeanTween.scale(memoryMenuItem.gameObject, Vector3.zero, 0.5f).setEaseOutBounce();
					UnityEngine.Object.Destroy(memoryMenuItem.gameObject, 1.5f);
				}
				else
				{
					UnityEngine.Object.Destroy(memoryMenuItem.gameObject);
				}
				this.m_spawnedItemMap[itemID] = null;
				return;
			}
		}
		else
		{
			this.m_spawnedItemMap[itemID] = null;
		}
	}

	// Token: 0x06000D7A RID: 3450 RVA: 0x0000C3F8 File Offset: 0x0000A5F8
	public int GetRandomItemIndex()
	{
		return UnityEngine.Random.Range(0, this.m_itemPrefabs.Length);
	}

	// Token: 0x06000D7B RID: 3451 RVA: 0x0006D9D0 File Offset: 0x0006BBD0
	public int GetFreeSpawnPointIndex()
	{
		if (this.m_freePoints.Count <= 0)
		{
			return -1;
		}
		int index = UnityEngine.Random.Range(0, this.m_freePoints.Count);
		MemoryMenuItemSpawnPoint memoryMenuItemSpawnPoint = this.m_freePoints[index];
		this.m_freePoints.RemoveAt(index);
		return memoryMenuItemSpawnPoint.Index;
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x0000C408 File Offset: 0x0000A608
	private void FreeSpawnPoint(MemoryMenuItemSpawnPoint point)
	{
		this.m_freePoints.Add(point);
	}

	// Token: 0x04000CCD RID: 3277
	[SerializeField]
	private GameObject[] m_itemPrefabs;

	// Token: 0x04000CCE RID: 3278
	[SerializeField]
	private Sprite[] m_itemIcons;

	// Token: 0x04000CCF RID: 3279
	private Dictionary<int, MemoryMenuItemSpawnPoint> m_spawnPoints = new Dictionary<int, MemoryMenuItemSpawnPoint>();

	// Token: 0x04000CD0 RID: 3280
	private List<MemoryMenuItemSpawnPoint> m_freePoints = new List<MemoryMenuItemSpawnPoint>();

	// Token: 0x04000CD1 RID: 3281
	private List<MemoryMenuItem> m_spawnedItems = new List<MemoryMenuItem>();

	// Token: 0x04000CD2 RID: 3282
	private Dictionary<int, MemoryMenuItem> m_spawnedItemMap = new Dictionary<int, MemoryMenuItem>();
}
