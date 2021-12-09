using System;
using ZP.Utility;

// Token: 0x020003A6 RID: 934
public class ActionStartTurn : BoardAction
{
	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06001905 RID: 6405 RVA: 0x0001289B File Offset: 0x00010A9B
	public short PlayerID
	{
		get
		{
			return this.player_id;
		}
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06001906 RID: 6406 RVA: 0x000128A3 File Offset: 0x00010AA3
	public short TurnOrderIndex
	{
		get
		{
			return this.turnOrderIndex;
		}
	}

	// Token: 0x06001907 RID: 6407 RVA: 0x000128AB File Offset: 0x00010AAB
	public ActionStartTurn(short _player_id, short _turn_order_index)
	{
		this.action_type = BoardActionType.StartTurn;
		this.player_id = _player_id;
		this.turnOrderIndex = _turn_order_index;
	}

	// Token: 0x06001908 RID: 6408 RVA: 0x000128C8 File Offset: 0x00010AC8
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.player_id);
			bs.Write(this.turnOrderIndex);
			return;
		}
		this.player_id = bs.ReadShort();
		this.turnOrderIndex = bs.ReadShort();
	}

	// Token: 0x04001AB0 RID: 6832
	private short player_id;

	// Token: 0x04001AB1 RID: 6833
	private short turnOrderIndex;
}
