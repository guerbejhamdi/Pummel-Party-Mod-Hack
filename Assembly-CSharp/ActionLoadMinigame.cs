using System;
using System.Collections.Generic;
using ZP.Utility;

// Token: 0x020003A8 RID: 936
public class ActionLoadMinigame : BoardAction
{
	// Token: 0x17000286 RID: 646
	// (get) Token: 0x0600190E RID: 6414 RVA: 0x0001293B File Offset: 0x00010B3B
	public string GameName
	{
		get
		{
			return this.minigame_name;
		}
	}

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x0600190F RID: 6415 RVA: 0x00012943 File Offset: 0x00010B43
	public int AlternateIndex
	{
		get
		{
			return this.minigame_alt_index;
		}
	}

	// Token: 0x06001910 RID: 6416 RVA: 0x0001294B File Offset: 0x00010B4B
	public ActionLoadMinigame(string game, int alt_index)
	{
		this.action_type = BoardActionType.LoadMinigame;
		this.minigame_name = game;
		this.minigame_alt_index = alt_index;
	}

	// Token: 0x06001911 RID: 6417 RVA: 0x000A9980 File Offset: 0x000A7B80
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.minigame_name);
			bs.Write((byte)this.minigame_alt_index);
			List<GamePlayer> playerList = GameManager.PlayerList;
			bs.Write((byte)playerList.Count);
			for (int i = 0; i < playerList.Count; i++)
			{
				bs.Write(playerList[i].GlobalID);
				bs.Write((short)playerList[i].MinigameTeam);
			}
			return;
		}
		this.minigame_name = bs.ReadString();
		this.minigame_alt_index = (int)bs.ReadByte();
		int num = (int)bs.ReadByte();
		for (int j = 0; j < num; j++)
		{
			short global_id = bs.ReadShort();
			int team = (int)bs.ReadShort();
			GameManager.SetPlayerTeam(global_id, team);
		}
	}

	// Token: 0x04001AB5 RID: 6837
	private string minigame_name;

	// Token: 0x04001AB6 RID: 6838
	private int minigame_alt_index;
}
