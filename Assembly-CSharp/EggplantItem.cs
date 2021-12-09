using System;
using UnityEngine;
using ZP.Net;

// Token: 0x0200036E RID: 878
public class EggplantItem : Item
{
	// Token: 0x1700023A RID: 570
	// (get) Token: 0x0600179A RID: 6042 RVA: 0x00011925 File Offset: 0x0000FB25
	// (set) Token: 0x0600179B RID: 6043 RVA: 0x0001192D File Offset: 0x0000FB2D
	public SearchNode AttackPath { get; set; }

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x0600179C RID: 6044 RVA: 0x00011936 File Offset: 0x0000FB36
	public BoardActor Target
	{
		get
		{
			return this.AITarget;
		}
	}

	// Token: 0x0600179D RID: 6045 RVA: 0x00003C5A File Offset: 0x00001E5A
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x0600179E RID: 6046 RVA: 0x0001193E File Offset: 0x0000FB3E
	public override void Setup()
	{
		base.Setup();
		this.player.BoardObject.PlayerAnimation.Carrying = true;
		base.GetAITarget();
		base.SetNetworkState(Item.ItemState.Setup);
	}

	// Token: 0x0600179F RID: 6047 RVA: 0x000A3E0C File Offset: 0x000A200C
	protected override void Use(int seed)
	{
		base.Use(seed);
		if (NetSystem.IsServer)
		{
			this.eggplant = NetSystem.Spawn("EggplantController", this.heldObject.transform.position + this.heldObject.transform.up * 0.5f, this.heldObject.transform.rotation, base.OwnerSlot, this.player.NetOwner).GetComponent<Eggplant>();
		}
	}

	// Token: 0x060017A0 RID: 6048 RVA: 0x00011969 File Offset: 0x0000FB69
	public override void Unequip(bool endingTurn)
	{
		this.player.BoardObject.PlayerAnimation.Carrying = false;
		base.Unequip(endingTurn);
	}

	// Token: 0x060017A1 RID: 6049 RVA: 0x00011988 File Offset: 0x0000FB88
	public void NetObjectSpawned()
	{
		UnityEngine.Object.Destroy(this.heldObject);
		this.player.BoardObject.PlayerAnimation.Carrying = false;
	}

	// Token: 0x060017A2 RID: 6050 RVA: 0x000A3E8C File Offset: 0x000A208C
	public override ItemAIUse GetTarget(BoardPlayer user)
	{
		ItemAIUse itemAIUse = null;
		int num = int.MaxValue;
		int num2 = 8;
		for (int i = 0; i < GameManager.GetPlayerCount(); i++)
		{
			BoardPlayer boardObject = GameManager.GetPlayerAt(i).BoardObject;
			if (!(boardObject == user) && boardObject.LocalHealth > 0)
			{
				SearchNode path = GameManager.Board.CurrentMap.GetPath(boardObject.CurrentNode, user.CurrentNode, BoardNodeConnectionDirection.Both, true);
				int num3 = path.pathCost;
				if (user.GamePlayer.IsAI && !boardObject.GamePlayer.IsAI)
				{
					num3 += this.difficultyDistanceChange[(int)user.GamePlayer.Difficulty];
				}
				if (num3 < num && num3 <= num2)
				{
					float priority = 1f - (float)num3 / (float)num2;
					itemAIUse = new ItemAIUse(boardObject, 0f);
					itemAIUse.priority = priority;
					num = num3;
					this.AttackPath = path;
				}
			}
		}
		return itemAIUse;
	}

	// Token: 0x060017A3 RID: 6051 RVA: 0x000119AB File Offset: 0x0000FBAB
	public EggplantItem()
	{
		int[] array = new int[3];
		array[0] = 4;
		array[1] = 2;
		this.difficultyDistanceChange = array;
		base..ctor();
	}

	// Token: 0x0400191C RID: 6428
	private Eggplant eggplant;

	// Token: 0x0400191E RID: 6430
	private int[] difficultyDistanceChange;
}
