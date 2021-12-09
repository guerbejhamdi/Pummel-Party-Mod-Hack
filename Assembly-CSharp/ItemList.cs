using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Utility;

// Token: 0x02000416 RID: 1046
[CreateAssetMenu(fileName = "ItemList", menuName = "Item/ItemList", order = 1)]
public class ItemList : ScriptableObject
{
	// Token: 0x06001D1B RID: 7451 RVA: 0x000BED48 File Offset: 0x000BCF48
	public bool HasWeaponSpaceItem()
	{
		bool result = false;
		foreach (ItemDetails itemDetails in GameManager.ItemList.items)
		{
			if (itemDetails.GetIsActive() && itemDetails.weaponSpcaeItem)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06001D1C RID: 7452 RVA: 0x000BED8C File Offset: 0x000BCF8C
	public ItemDetails GetRandomItem(BoardPlayer player, bool weaponSpaceItem = false)
	{
		List<Items> list = new List<Items>();
		foreach (ItemDetails itemDetails in GameManager.ItemList.items)
		{
			if (itemDetails.GetIsActive() && ((weaponSpaceItem && itemDetails.weaponSpcaeItem) || (!weaponSpaceItem && !itemDetails.weaponSpcaeItem)))
			{
				list.Add(itemDetails.enumReference);
			}
		}
		return this.GetRandomItem(player, list.ToArray());
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x000BEDF4 File Offset: 0x000BCFF4
	public ItemDetails GetRandomItem(BoardPlayer player, Items[] itemPool)
	{
		if (itemPool == null || itemPool.Length == 0)
		{
			return this.fallbackItem;
		}
		float num = 0f;
		float[] array = new float[itemPool.Length];
		for (int i = 0; i < array.Length; i++)
		{
			float num2;
			if (player == null)
			{
				num2 = 1f;
			}
			else
			{
				switch (player.ObtainedInventory[(int)itemPool[i]])
				{
				case 0:
					num2 = 1f;
					break;
				case 1:
					num2 = 0.1f;
					break;
				case 2:
					num2 = 0.03f;
					break;
				default:
					num2 = 0.01f;
					break;
				}
			}
			array[i] = num;
			num += num2;
		}
		BinaryTree binaryTree = new BinaryTree(array);
		float p = ZPMath.RandomFloat(GameManager.rand, 0f, num);
		int num3 = (int)itemPool[binaryTree.FindPoint(p)];
		return this.items[num3];
	}

	// Token: 0x06001D1E RID: 7454 RVA: 0x000BEEC4 File Offset: 0x000BD0C4
	public bool IsItemActive(int id)
	{
		foreach (ItemDetails itemDetails in GameManager.ItemList.items)
		{
			if ((int)itemDetails.itemID == id && itemDetails.GetIsActive())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04001F8A RID: 8074
	public ItemDetails[] items;

	// Token: 0x04001F8B RID: 8075
	public ItemDetails fallbackItem;
}
