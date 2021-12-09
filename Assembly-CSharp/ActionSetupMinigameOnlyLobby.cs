using System;
using ZP.Utility;

// Token: 0x020003B4 RID: 948
public class ActionSetupMinigameOnlyLobby : BoardAction
{
	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06001939 RID: 6457 RVA: 0x00012C14 File Offset: 0x00010E14
	public byte MinigameID
	{
		get
		{
			return this.minigameID;
		}
	}

	// Token: 0x0600193A RID: 6458 RVA: 0x00012C1C File Offset: 0x00010E1C
	public ActionSetupMinigameOnlyLobby(byte minigameID)
	{
		this.minigameID = minigameID;
		this.action_type = BoardActionType.SetupMinigameOnlyLobby;
	}

	// Token: 0x0600193B RID: 6459 RVA: 0x00012C33 File Offset: 0x00010E33
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.minigameID);
			return;
		}
		this.minigameID = bs.ReadByte();
	}

	// Token: 0x04001AD4 RID: 6868
	private byte minigameID;
}
