using System;

// Token: 0x0200037E RID: 894
public class ItemAIUse
{
	// Token: 0x0600181E RID: 6174 RVA: 0x00011EB2 File Offset: 0x000100B2
	public ItemAIUse()
	{
		this.player = null;
		this.priority = float.MinValue;
	}

	// Token: 0x0600181F RID: 6175 RVA: 0x00011ECC File Offset: 0x000100CC
	public ItemAIUse(BoardActor player, float priority)
	{
		this.player = player;
		this.priority = priority;
	}

	// Token: 0x04001993 RID: 6547
	public BoardActor player;

	// Token: 0x04001994 RID: 6548
	public float priority;
}
