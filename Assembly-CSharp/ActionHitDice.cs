using System;
using ZP.Utility;

// Token: 0x020003A5 RID: 933
public class ActionHitDice : BoardAction
{
	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06001901 RID: 6401 RVA: 0x00012838 File Offset: 0x00010A38
	public short PlayerID
	{
		get
		{
			return this.player_id;
		}
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06001902 RID: 6402 RVA: 0x00012840 File Offset: 0x00010A40
	public byte RollNumber
	{
		get
		{
			return this.roll_number;
		}
	}

	// Token: 0x06001903 RID: 6403 RVA: 0x00012848 File Offset: 0x00010A48
	public ActionHitDice(short _player_id, byte _roll_number)
	{
		this.action_type = BoardActionType.HitDice;
		this.player_id = _player_id;
		this.roll_number = _roll_number;
	}

	// Token: 0x06001904 RID: 6404 RVA: 0x00012865 File Offset: 0x00010A65
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.player_id);
			bs.Write(this.roll_number);
			return;
		}
		this.player_id = bs.ReadShort();
		this.roll_number = bs.ReadByte();
	}

	// Token: 0x04001AAE RID: 6830
	private short player_id;

	// Token: 0x04001AAF RID: 6831
	private byte roll_number;
}
