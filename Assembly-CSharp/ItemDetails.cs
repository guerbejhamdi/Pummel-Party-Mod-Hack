using System;
using Rewired;
using UnityEngine;

// Token: 0x020000BD RID: 189
[CreateAssetMenu(fileName = "ItemDetail", menuName = "Item/ItemDetail", order = 1)]
public class ItemDetails : ScriptableObject
{
	// Token: 0x17000087 RID: 135
	// (get) Token: 0x060003DC RID: 988 RVA: 0x00006216 File Offset: 0x00004416
	public int ActionID
	{
		get
		{
			if (this.actionID == -2)
			{
				this.actionID = ReInput.mapping.GetActionId(this.useAction);
			}
			return this.actionID;
		}
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0000623E File Offset: 0x0000443E
	public bool GetIsActive()
	{
		return GameManager.RulesetManager.ActiveRuleset.Items.IsItemEnabled(this);
	}

	// Token: 0x060003DE RID: 990 RVA: 0x0000398C File Offset: 0x00001B8C
	public void SetIsActive(bool active)
	{
	}

	// Token: 0x0400041E RID: 1054
	public ushort itemUniqueID;

	// Token: 0x0400041F RID: 1055
	public bool enabled = true;

	// Token: 0x04000420 RID: 1056
	public bool weaponSpcaeItem;

	// Token: 0x04000421 RID: 1057
	public Items enumReference;

	// Token: 0x04000422 RID: 1058
	public string itemName;

	// Token: 0x04000423 RID: 1059
	public string itemNameToken;

	// Token: 0x04000424 RID: 1060
	public string description;

	// Token: 0x04000425 RID: 1061
	public string descriptionToken;

	// Token: 0x04000426 RID: 1062
	public string netPrefabName;

	// Token: 0x04000427 RID: 1063
	public string useAction;

	// Token: 0x04000428 RID: 1064
	public GameObject prefab;

	// Token: 0x04000429 RID: 1065
	public string prefabPath;

	// Token: 0x0400042A RID: 1066
	public ItemRarity rarity;

	// Token: 0x0400042B RID: 1067
	public byte itemID;

	// Token: 0x0400042C RID: 1068
	public bool skipTurnAfterUse;

	// Token: 0x0400042D RID: 1069
	[Header("Board Item Recieve")]
	public GameObject recievePrefab;

	// Token: 0x0400042E RID: 1070
	public string recievePrefabPath;

	// Token: 0x0400042F RID: 1071
	public float rotateSpeed = 100f;

	// Token: 0x04000430 RID: 1072
	public GameObject heldPrefab;

	// Token: 0x04000431 RID: 1073
	public string heldPrefabPath;

	// Token: 0x04000432 RID: 1074
	public Vector3 heldPosition;

	// Token: 0x04000433 RID: 1075
	public Vector3 heldRotation;

	// Token: 0x04000434 RID: 1076
	public Vector3 heldScale = new Vector3(100f, 100f, 100f);

	// Token: 0x04000435 RID: 1077
	public PlayerBone heldBone;

	// Token: 0x04000436 RID: 1078
	[Header("Icon")]
	public Sprite icon;

	// Token: 0x04000437 RID: 1079
	public Vector2 iconOffset = Vector2.zero;

	// Token: 0x04000438 RID: 1080
	public Vector2 iconSize = new Vector2(61f, 61f);

	// Token: 0x04000439 RID: 1081
	public InputHelp inputHelp;

	// Token: 0x0400043A RID: 1082
	public InputHelp usingInputHelp;

	// Token: 0x0400043B RID: 1083
	private int actionID = -2;
}
