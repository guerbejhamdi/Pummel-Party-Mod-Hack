using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000503 RID: 1283
public class InventoryUI : MonoBehaviour
{
	// Token: 0x0600219D RID: 8605 RVA: 0x0001864E File Offset: 0x0001684E
	private void Start()
	{
		if (GameManager.ItemList == null)
		{
			return;
		}
		this.window = base.GetComponent<InventoryWindow>();
		this.eventSystemGroup = base.GetComponent<EventSystemGroup>();
		base.StartCoroutine(this.WaitForItemList());
	}

	// Token: 0x0600219E RID: 8606 RVA: 0x00018683 File Offset: 0x00016883
	private IEnumerator WaitForItemList()
	{
		yield return new WaitUntil(() => GameManager.ItemList != null);
		if (GameManager.ItemList != null)
		{
			this.items = GameManager.ItemList.items;
		}
		this.itemSlots = new InventoryUI.ItemUISlot[this.items.Length];
		float y = 32f;
		float num = 64f;
		float num2 = 4f;
		float num3 = -((num2 * (float)(this.items.Length - 1) + num * (float)this.items.Length) / 2f) + num / 2f;
		for (int i = 0; i < this.items.Length; i++)
		{
			int rarity = (int)this.items[i].rarity;
			InventoryUI.ItemUISlot itemUISlot = new InventoryUI.ItemUISlot(UnityEngine.Object.Instantiate<GameObject>(this.slotPrefab, this.parent));
			itemUISlot.border.color = this.rarityBorderColors[rarity];
			itemUISlot.itemIcon.sprite = this.items[i].icon;
			itemUISlot.itemIcon.rectTransform.localPosition = this.items[i].iconOffset;
			itemUISlot.itemIcon.rectTransform.sizeDelta = this.items[i].iconSize;
			itemUISlot.buttonScript.itemID = i;
			itemUISlot.buttonScript.inventoryUI = this;
			((RectTransform)itemUISlot.gameObject.transform).anchoredPosition = new Vector3(num3 + (float)i * (num + num2), y, 0f);
			this.itemSlots[i] = itemUISlot;
		}
		this.window.UpdateSelectables();
		base.StartCoroutine(this.UpdateInventory());
		yield break;
	}

	// Token: 0x0600219F RID: 8607 RVA: 0x00018692 File Offset: 0x00016892
	public void OnClick(int itemID)
	{
		if (this.itemSlots[itemID].button.IsInteractable())
		{
			this.Hide();
			GameManager.Board.ClickedItem(itemID);
		}
	}

	// Token: 0x060021A0 RID: 8608 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x060021A1 RID: 8609 RVA: 0x0000398C File Offset: 0x00001B8C
	public void OnEnable()
	{
	}

	// Token: 0x060021A2 RID: 8610 RVA: 0x000CF818 File Offset: 0x000CDA18
	public void SetInventoryPlayer(BoardPlayer player)
	{
		this.eventSystemGroup.EventSystemID = (int)player.GamePlayer.LocalIDAndAI;
		bool isNull = player.GamePlayer.RewiredPlayer == null;
		for (int i = 0; i < this.itemSlots.Length; i++)
		{
			this.itemSlots[i].buttonScript.SetPlayer(player.GamePlayer.RewiredPlayer, isNull);
		}
		this.curPlayer = player;
	}

	// Token: 0x060021A3 RID: 8611 RVA: 0x000186BD File Offset: 0x000168BD
	public void Hide()
	{
		this.window.SetState(MainMenuWindowState.Hidden);
	}

	// Token: 0x060021A4 RID: 8612 RVA: 0x000186CB File Offset: 0x000168CB
	public void Show()
	{
		this.window.SetState(MainMenuWindowState.Visible);
		base.StartCoroutine(this.UpdateInventory());
	}

	// Token: 0x060021A5 RID: 8613 RVA: 0x000186E6 File Offset: 0x000168E6
	private IEnumerator UpdateInventory()
	{
		yield return null;
		for (int i = 0; i < this.itemSlots.Length; i++)
		{
			int num = (int)((this.curPlayer == null) ? 0 : this.curPlayer.GetItemCount((byte)i));
			this.itemSlots[i].quantityText.text = num.ToString();
			bool flag = num != 0 && !this.curPlayer.HasUsedItem;
			this.itemSlots[i].itemIcon.color = (flag ? this.activeColor : this.inactiveColor);
			this.itemSlots[i].buttonScript.Active = flag;
			this.itemSlots[i].button.interactable = flag;
		}
		yield break;
	}

	// Token: 0x060021A6 RID: 8614 RVA: 0x000CF888 File Offset: 0x000CDA88
	public void OnSelect(int itemID)
	{
		if (this.curItemTooltip != itemID)
		{
			if (this.curItemTooltip == -1)
			{
				this.toolTipObject.SetActive(true);
			}
			this.curItemTooltip = itemID;
			ItemDetails itemDetails = GameManager.ItemList.items[itemID];
			this.toolTipTitle.text = LocalizationManager.GetTranslation(itemDetails.itemNameToken, true, 0, true, false, null, null, true);
			this.toolTipDescription.text = LocalizationManager.GetTranslation(itemDetails.descriptionToken, true, 0, true, false, null, null, true);
		}
	}

	// Token: 0x060021A7 RID: 8615 RVA: 0x000186F5 File Offset: 0x000168F5
	public void OnDeSelect(int itemID)
	{
		if (this.curItemTooltip == itemID)
		{
			this.toolTipObject.SetActive(false);
			this.curItemTooltip = -1;
		}
	}

	// Token: 0x04002467 RID: 9319
	public GameObject slotPrefab;

	// Token: 0x04002468 RID: 9320
	public Color[] rarityBorderColors = new Color[4];

	// Token: 0x04002469 RID: 9321
	public Transform parent;

	// Token: 0x0400246A RID: 9322
	[Header("Tooltip")]
	public GameObject toolTipObject;

	// Token: 0x0400246B RID: 9323
	public Text toolTipTitle;

	// Token: 0x0400246C RID: 9324
	public Text toolTipDescription;

	// Token: 0x0400246D RID: 9325
	private ItemDetails[] items;

	// Token: 0x0400246E RID: 9326
	private InventoryUI.ItemUISlot[] itemSlots;

	// Token: 0x0400246F RID: 9327
	private BoardPlayer curPlayer;

	// Token: 0x04002470 RID: 9328
	private InventoryWindow window;

	// Token: 0x04002471 RID: 9329
	private EventSystemGroup eventSystemGroup;

	// Token: 0x04002472 RID: 9330
	private Color activeColor = new Color(1f, 1f, 1f, 0.003921569f);

	// Token: 0x04002473 RID: 9331
	private Color inactiveColor = new Color(1f, 1f, 1f, 1f);

	// Token: 0x04002474 RID: 9332
	private int curItemTooltip = -1;

	// Token: 0x02000504 RID: 1284
	private struct ItemUISlot
	{
		// Token: 0x060021A9 RID: 8617 RVA: 0x000CF968 File Offset: 0x000CDB68
		public ItemUISlot(GameObject gameObject)
		{
			this.gameObject = gameObject;
			this.buttonScript = gameObject.GetComponent<InventorySlotButton>();
			this.border = gameObject.GetComponent<Image>();
			this.lockIcon = gameObject.transform.Find("LockIcon").GetComponent<Image>();
			this.itemIcon = gameObject.transform.Find("Mask/ItemIcon").GetComponent<Image>();
			this.itemMaterial = this.itemIcon.materialForRendering;
			this.quantityText = gameObject.transform.Find("QuantityText").GetComponent<Text>();
			this.animator = gameObject.GetComponent<Animator>();
			this.button = gameObject.GetComponent<Button>();
			this.eventTrigger = gameObject.AddComponent<EventTrigger>();
		}

		// Token: 0x04002475 RID: 9333
		public GameObject gameObject;

		// Token: 0x04002476 RID: 9334
		public InventorySlotButton buttonScript;

		// Token: 0x04002477 RID: 9335
		public Image border;

		// Token: 0x04002478 RID: 9336
		public Image lockIcon;

		// Token: 0x04002479 RID: 9337
		public Image itemIcon;

		// Token: 0x0400247A RID: 9338
		public Material itemMaterial;

		// Token: 0x0400247B RID: 9339
		public Text quantityText;

		// Token: 0x0400247C RID: 9340
		public Animator animator;

		// Token: 0x0400247D RID: 9341
		public Button button;

		// Token: 0x0400247E RID: 9342
		public EventTrigger eventTrigger;
	}
}
