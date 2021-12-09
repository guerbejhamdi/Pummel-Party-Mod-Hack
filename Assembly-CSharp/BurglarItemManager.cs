using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000185 RID: 389
public class BurglarItemManager : MonoBehaviour
{
	// Token: 0x06000B11 RID: 2833 RVA: 0x0005F8BC File Offset: 0x0005DABC
	public void Awake()
	{
		foreach (BurglarItemSpawnPoint burglarItemSpawnPoint in UnityEngine.Object.FindObjectsOfType<BurglarItemSpawnPoint>())
		{
			this.m_spawnPoints.Add(burglarItemSpawnPoint.Index, burglarItemSpawnPoint);
			this.m_freePoints.Add(burglarItemSpawnPoint);
		}
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x0005F900 File Offset: 0x0005DB00
	public void OnDestroy()
	{
		foreach (BurglarItem burglarItem in this.m_spawnedItems)
		{
			if (!(burglarItem == null) && !(burglarItem.gameObject == null))
			{
				UnityEngine.Object.Destroy(burglarItem.gameObject);
			}
		}
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x0000B17B File Offset: 0x0000937B
	public List<BurglarItem> GetSpawnedItems()
	{
		return this.m_spawnedItems;
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x0000B183 File Offset: 0x00009383
	public bool IsItemActive(BurglarItem item)
	{
		return !(item == null) && this.IsItemActive(item.ItemID);
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x0005F970 File Offset: 0x0005DB70
	public bool IsItemActive(int id)
	{
		BurglarItem x = null;
		return this.m_spawnedItemMap.TryGetValue(id, out x) && x != null;
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x0005F998 File Offset: 0x0005DB98
	public void SpawnItem(int itemID, int spawnIndex, int itemIndex)
	{
		if (this.m_spawnedItemMap.ContainsKey(itemID))
		{
			Debug.LogError("itemID already in spawn item map, something weird happened so we won't spawn the item..");
			return;
		}
		GameObject original = this.m_itemPrefabs[itemIndex];
		BurglarItemSpawnPoint burglarItemSpawnPoint = null;
		if (!this.m_spawnPoints.TryGetValue(spawnIndex, out burglarItemSpawnPoint))
		{
			Debug.LogError("Unable to find item spawn point " + spawnIndex.ToString() + " for minigame.");
			return;
		}
		burglarItemSpawnPoint.ItemID = (short)itemID;
		BurglarItem component = UnityEngine.Object.Instantiate<GameObject>(original, burglarItemSpawnPoint.transform.position, burglarItemSpawnPoint.transform.rotation).GetComponent<BurglarItem>();
		component.ItemID = itemID;
		this.m_spawnedItems.Add(component);
		this.m_spawnedItemMap.Add(itemID, component);
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x0005FA40 File Offset: 0x0005DC40
	public void DestroyItem(int itemID)
	{
		if (this.m_spawnedItemMap.ContainsKey(itemID))
		{
			BurglarItem burglarItem = this.m_spawnedItemMap[itemID];
			if (burglarItem != null)
			{
				foreach (KeyValuePair<int, BurglarItemSpawnPoint> keyValuePair in this.m_spawnPoints)
				{
					if ((int)keyValuePair.Value.ItemID == itemID)
					{
						keyValuePair.Value.ItemID = 0;
						this.m_freePoints.Add(keyValuePair.Value);
					}
				}
				this.m_spawnedItems.Remove(burglarItem);
				UnityEngine.Object.Destroy(burglarItem.gameObject);
				this.m_spawnedItemMap[itemID] = null;
				return;
			}
		}
		else
		{
			this.m_spawnedItemMap[itemID] = null;
		}
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x0000B19C File Offset: 0x0000939C
	public int GetRandomItemIndex()
	{
		return UnityEngine.Random.Range(0, this.m_itemPrefabs.Length);
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x0005FB18 File Offset: 0x0005DD18
	public int GetFreeSpawnPointIndex()
	{
		if (this.m_freePoints.Count <= 0)
		{
			return -1;
		}
		int index = UnityEngine.Random.Range(0, this.m_freePoints.Count);
		BurglarItemSpawnPoint burglarItemSpawnPoint = this.m_freePoints[index];
		this.m_freePoints.RemoveAt(index);
		return burglarItemSpawnPoint.Index;
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x0000B1AC File Offset: 0x000093AC
	private void FreeSpawnPoint(BurglarItemSpawnPoint point)
	{
		this.m_freePoints.Add(point);
	}

	// Token: 0x04000A26 RID: 2598
	[SerializeField]
	private GameObject[] m_itemPrefabs;

	// Token: 0x04000A27 RID: 2599
	private Dictionary<int, BurglarItemSpawnPoint> m_spawnPoints = new Dictionary<int, BurglarItemSpawnPoint>();

	// Token: 0x04000A28 RID: 2600
	private List<BurglarItemSpawnPoint> m_freePoints = new List<BurglarItemSpawnPoint>();

	// Token: 0x04000A29 RID: 2601
	private List<BurglarItem> m_spawnedItems = new List<BurglarItem>();

	// Token: 0x04000A2A RID: 2602
	private Dictionary<int, BurglarItem> m_spawnedItemMap = new Dictionary<int, BurglarItem>();
}
