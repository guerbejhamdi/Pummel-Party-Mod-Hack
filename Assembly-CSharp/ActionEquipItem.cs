using System;
using ZP.Utility;

// Token: 0x020003AC RID: 940
public class ActionEquipItem : BoardAction
{
	// Token: 0x1700028D RID: 653
	// (get) Token: 0x0600191D RID: 6429 RVA: 0x000129E3 File Offset: 0x00010BE3
	public byte ItemID
	{
		get
		{
			return this.itemID;
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x0600191E RID: 6430 RVA: 0x000129EB File Offset: 0x00010BEB
	public short playerID
	{
		get
		{
			return this.player_id;
		}
	}

	// Token: 0x0600191F RID: 6431 RVA: 0x000129F3 File Offset: 0x00010BF3
	public ActionEquipItem(byte _item_id, short _player_id)
	{
		this.action_type = BoardActionType.EquipItem;
		this.itemID = _item_id;
		this.player_id = _player_id;
	}

	// Token: 0x06001920 RID: 6432 RVA: 0x00012A11 File Offset: 0x00010C11
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.itemID);
			bs.Write(this.player_id);
			return;
		}
		this.itemID = bs.ReadByte();
		this.player_id = bs.ReadShort();
	}

	// Token: 0x04001AC4 RID: 6852
	private short player_id;

	// Token: 0x04001AC5 RID: 6853
	private byte itemID;
}
