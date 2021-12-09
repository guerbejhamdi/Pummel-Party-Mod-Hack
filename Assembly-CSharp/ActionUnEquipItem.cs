using System;
using ZP.Utility;

// Token: 0x020003AE RID: 942
public class ActionUnEquipItem : BoardAction
{
	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06001927 RID: 6439 RVA: 0x00012A8C File Offset: 0x00010C8C
	public short playerID
	{
		get
		{
			return this.player_id;
		}
	}

	// Token: 0x06001928 RID: 6440 RVA: 0x00012A94 File Offset: 0x00010C94
	public ActionUnEquipItem(short _player_id)
	{
		this.action_type = BoardActionType.UnEquipItem;
		this.player_id = _player_id;
	}

	// Token: 0x06001929 RID: 6441 RVA: 0x00012AAB File Offset: 0x00010CAB
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.player_id);
			return;
		}
		this.player_id = bs.ReadShort();
	}

	// Token: 0x04001ACA RID: 6858
	private short player_id;
}
