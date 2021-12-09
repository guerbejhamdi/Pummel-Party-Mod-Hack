using System;
using System.Collections.Generic;
using ZP.Utility;

// Token: 0x020003B2 RID: 946
public class ActionShowWinner : BoardAction
{
	// Token: 0x06001934 RID: 6452 RVA: 0x00012BB5 File Offset: 0x00010DB5
	public ActionShowWinner(bool buildResults)
	{
		if (buildResults)
		{
			this.placements = GameManager.GetPlayerPlacements();
		}
		this.action_type = BoardActionType.ShowWinner;
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x000AA078 File Offset: 0x000A8278
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		for (int i = 0; i < GameManager.GetPlayerCount(); i++)
		{
			if (write)
			{
				bs.Write(this.placements[i].GlobalID);
			}
			else
			{
				this.placements.Add(GameManager.GetPlayerAt((int)bs.ReadShort()));
			}
			this.placements[i].Placement = (byte)i;
		}
	}

	// Token: 0x04001AD2 RID: 6866
	public List<GamePlayer> placements = new List<GamePlayer>();
}
