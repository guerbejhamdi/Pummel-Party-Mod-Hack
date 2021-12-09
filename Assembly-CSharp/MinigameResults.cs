using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x0200041E RID: 1054
public class MinigameResults
{
	// Token: 0x06001D2E RID: 7470 RVA: 0x00015829 File Offset: 0x00013A29
	public MinigameResults(int numPlayers)
	{
		this.placements = new byte[numPlayers];
		this.itemIDs = new byte[numPlayers];
		this.gold = new byte[numPlayers];
	}

	// Token: 0x06001D2F RID: 7471 RVA: 0x00015867 File Offset: 0x00013A67
	public void SortPlayers()
	{
		this.players = (from o in this.players
		orderby o.MinigameScore descending
		select o).ToList<GamePlayer>();
	}

	// Token: 0x04001FC4 RID: 8132
	public List<GamePlayer> players = new List<GamePlayer>();

	// Token: 0x04001FC5 RID: 8133
	public byte[] placements;

	// Token: 0x04001FC6 RID: 8134
	public byte[] itemIDs;

	// Token: 0x04001FC7 RID: 8135
	public byte[] gold;

	// Token: 0x04001FC8 RID: 8136
	public bool show_order = true;
}
