using System;
using ZP.Utility;

// Token: 0x0200039F RID: 927
public class ActionChangeGameState : BoardAction
{
	// Token: 0x17000270 RID: 624
	// (get) Token: 0x060018E3 RID: 6371 RVA: 0x00012659 File Offset: 0x00010859
	public GameBoardState BoardState
	{
		get
		{
			return this.board_state;
		}
	}

	// Token: 0x060018E4 RID: 6372 RVA: 0x00012661 File Offset: 0x00010861
	public ActionChangeGameState(GameBoardState new_state)
	{
		this.action_type = BoardActionType.ChangeGameState;
		this.board_state = new_state;
	}

	// Token: 0x060018E5 RID: 6373 RVA: 0x00012677 File Offset: 0x00010877
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write((byte)this.board_state);
			return;
		}
		this.board_state = (GameBoardState)bs.ReadByte();
	}

	// Token: 0x04001A9F RID: 6815
	private GameBoardState board_state;
}
