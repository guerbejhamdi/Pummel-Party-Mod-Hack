using System;
using System.Collections.Generic;
using ZP.Utility;

// Token: 0x020003AB RID: 939
public class ActionShowMinigameResults : BoardAction
{
	// Token: 0x0600191B RID: 6427 RVA: 0x000A9B3C File Offset: 0x000A7D3C
	public ActionShowMinigameResults()
	{
		this.action_type = BoardActionType.ShowMinigameResults;
	}

	// Token: 0x0600191C RID: 6428 RVA: 0x000A9C40 File Offset: 0x000A7E40
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			MinigameResults minigameResults = new MinigameResults(GameManager.GetPlayerCount());
			List<GamePlayer> list = new List<GamePlayer>(GameManager.PlayerList);
			list.Shuffle<GamePlayer>();
			minigameResults.players = list;
			minigameResults.SortPlayers();
			for (int i = 0; i < minigameResults.placements.Length; i++)
			{
				minigameResults.placements[i] = (byte)i;
				int num = 0;
				int num2 = i + 1;
				while (num2 < minigameResults.players.Count && minigameResults.players[i].MinigameScore == minigameResults.players[num2].MinigameScore)
				{
					minigameResults.placements[num2] = (byte)i;
					num++;
					num2++;
				}
				i += num;
			}
			for (int j = 0; j < minigameResults.placements.Length; j++)
			{
				int num3 = (int)minigameResults.placements[j];
				minigameResults.gold[j] = this.gold[num3];
				if (GameManager.partyGameMode == PartyGameMode.BoardGame)
				{
					if (GameManager.rand.NextDouble() < (double)this.itemChances[num3])
					{
						List<Items> list2 = new List<Items>();
						int num4 = 0;
						int num5 = this.itemPoolPlacementStart[num3];
						while (num5 < this.itemRank.Length && num4 < this.placementItemCount[num3])
						{
							ItemDetails itemFromEnum = GameManager.GetItemFromEnum(this.itemRank[num5]);
							if (itemFromEnum.GetIsActive())
							{
								list2.Add(itemFromEnum.enumReference);
								num4++;
							}
							num5++;
						}
						if (num4 < this.placementItemCount[num3])
						{
							int num6 = this.itemPoolPlacementStart[num3];
							while (num6 >= 0 && num4 < this.placementItemCount[num3])
							{
								ItemDetails itemFromEnum2 = GameManager.GetItemFromEnum(this.itemRank[num6]);
								if (itemFromEnum2.GetIsActive())
								{
									list2.Add(itemFromEnum2.enumReference);
									num4++;
								}
								num6--;
							}
						}
						minigameResults.itemIDs[j] = GameManager.ItemList.GetRandomItem(minigameResults.players[j].BoardObject, list2.ToArray()).itemID;
					}
					else
					{
						minigameResults.itemIDs[j] = byte.MaxValue;
					}
				}
			}
			GameManager.SetMinigameResults(minigameResults);
			for (int k = 0; k < minigameResults.players.Count; k++)
			{
				bs.Write(minigameResults.players[k].GlobalID);
				bs.Write(minigameResults.players[k].MinigameScore);
				bs.Write(minigameResults.placements[k]);
				bs.Write(minigameResults.gold[k]);
				bs.Write(minigameResults.itemIDs[k]);
			}
			return;
		}
		MinigameResults minigameResults2 = new MinigameResults(GameManager.GetPlayerCount());
		for (int l = 0; l < GameManager.GetPlayerCount(); l++)
		{
			GamePlayer playerWithID = GameManager.GetPlayerWithID(bs.ReadShort());
			playerWithID.MinigameScore = bs.ReadInt();
			minigameResults2.players.Add(playerWithID);
			minigameResults2.placements[l] = bs.ReadByte();
			minigameResults2.gold[l] = bs.ReadByte();
			minigameResults2.itemIDs[l] = bs.ReadByte();
		}
		GameManager.SetMinigameResults(minigameResults2);
	}

	// Token: 0x04001ABE RID: 6846
	private byte[] gold = new byte[]
	{
		6,
		5,
		4,
		3,
		2,
		1,
		0,
		0
	};

	// Token: 0x04001ABF RID: 6847
	private float[] itemChances = new float[]
	{
		2f,
		2f,
		0.5f,
		0f,
		0f,
		0f,
		0f,
		0f
	};

	// Token: 0x04001AC0 RID: 6848
	private Items[][] itemPools = new Items[][]
	{
		new Items[]
		{
			Items.SwapItem,
			Items.RocketSkewer,
			Items.Eggplant,
			Items.WreckingBall,
			Items.Present
		},
		new Items[]
		{
			Items.WreckingBall,
			Items.BeeHive,
			Items.Magnet,
			Items.TacticalCactus
		},
		new Items[]
		{
			Items.TacticalCactus,
			Items.BoxingGlove,
			Items.Shotgun,
			Items.HealthKit
		},
		new Items[0],
		new Items[0],
		new Items[0],
		new Items[0],
		new Items[0]
	};

	// Token: 0x04001AC1 RID: 6849
	private Items[] itemRank = new Items[]
	{
		Items.SwapItem,
		Items.RocketSkewer,
		Items.Eggplant,
		Items.Present,
		Items.WreckingBall,
		Items.BeeHive,
		Items.Magnet,
		Items.TacticalCactus,
		Items.BoxingGlove,
		Items.Shotgun,
		Items.HealthKit
	};

	// Token: 0x04001AC2 RID: 6850
	private int[] itemPoolPlacementStart = new int[]
	{
		0,
		4,
		7,
		11,
		11,
		11,
		11,
		11
	};

	// Token: 0x04001AC3 RID: 6851
	private int[] placementItemCount = new int[]
	{
		4,
		4,
		4,
		0,
		0,
		0,
		0,
		0
	};
}
