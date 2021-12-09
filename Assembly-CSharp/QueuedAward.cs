using System;
using System.Collections.Generic;

// Token: 0x0200006B RID: 107
public class QueuedAward
{
	// Token: 0x060001FF RID: 511 RVA: 0x00004DE5 File Offset: 0x00002FE5
	public QueuedAward(List<GamePlayer> players, StatChallengeExtent extent, StatType stat, bool winner = false)
	{
		this.players = players;
		this.extent = extent;
		this.stat = stat;
		this.winner = winner;
	}

	// Token: 0x04000261 RID: 609
	public List<GamePlayer> players;

	// Token: 0x04000262 RID: 610
	public StatChallengeExtent extent;

	// Token: 0x04000263 RID: 611
	public StatType stat;

	// Token: 0x04000264 RID: 612
	public bool winner;
}
