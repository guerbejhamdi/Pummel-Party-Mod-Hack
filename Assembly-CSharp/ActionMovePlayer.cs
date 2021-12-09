using System;
using ZP.Utility;

// Token: 0x020003A3 RID: 931
public class ActionMovePlayer : BoardAction
{
	// Token: 0x1700027A RID: 634
	// (get) Token: 0x060018F8 RID: 6392 RVA: 0x000127B0 File Offset: 0x000109B0
	public short PlayerID
	{
		get
		{
			return this.player_id;
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x060018F9 RID: 6393 RVA: 0x000127B8 File Offset: 0x000109B8
	public byte Steps
	{
		get
		{
			return this.steps;
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x060018FA RID: 6394 RVA: 0x000127C0 File Offset: 0x000109C0
	public byte Direction
	{
		get
		{
			return this.direction_choice;
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x060018FB RID: 6395 RVA: 0x000127C8 File Offset: 0x000109C8
	public bool IntersectionStart
	{
		get
		{
			return this.intersection_start;
		}
	}

	// Token: 0x060018FC RID: 6396 RVA: 0x000127D0 File Offset: 0x000109D0
	public ActionMovePlayer(short _player_id, byte _steps, byte _direction_choice, bool _intersection_start = false)
	{
		this.action_type = BoardActionType.MovePlayer;
		this.player_id = _player_id;
		this.steps = _steps;
		this.direction_choice = _direction_choice;
		this.intersection_start = _intersection_start;
	}

	// Token: 0x060018FD RID: 6397 RVA: 0x000A986C File Offset: 0x000A7A6C
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.player_id);
			bs.Write(this.steps);
			bs.Write(this.direction_choice);
			bs.Write(this.intersection_start);
			return;
		}
		this.player_id = bs.ReadShort();
		this.steps = bs.ReadByte();
		this.direction_choice = bs.ReadByte();
		this.intersection_start = bs.ReadBool();
	}

	// Token: 0x04001AA9 RID: 6825
	private short player_id;

	// Token: 0x04001AAA RID: 6826
	private byte steps;

	// Token: 0x04001AAB RID: 6827
	private byte direction_choice;

	// Token: 0x04001AAC RID: 6828
	private bool intersection_start;
}
