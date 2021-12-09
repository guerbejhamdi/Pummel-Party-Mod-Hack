using System;
using ZP.Utility;

// Token: 0x020003A4 RID: 932
public class ActionChooseDirection : BoardAction
{
	// Token: 0x1700027E RID: 638
	// (get) Token: 0x060018FE RID: 6398 RVA: 0x000127FC File Offset: 0x000109FC
	public byte Direction
	{
		get
		{
			return this.direction_choice;
		}
	}

	// Token: 0x060018FF RID: 6399 RVA: 0x00012804 File Offset: 0x00010A04
	public ActionChooseDirection(short _player_id, byte _direction_choice)
	{
		this.action_type = BoardActionType.ChooseDirection;
		this.direction_choice = _direction_choice;
	}

	// Token: 0x06001900 RID: 6400 RVA: 0x0001281A File Offset: 0x00010A1A
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.direction_choice);
			return;
		}
		this.direction_choice = bs.ReadByte();
	}

	// Token: 0x04001AAD RID: 6829
	private byte direction_choice;
}
