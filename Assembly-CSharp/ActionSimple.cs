using System;
using ZP.Utility;

// Token: 0x0200039D RID: 925
public class ActionSimple : BoardAction
{
	// Token: 0x1700026E RID: 622
	// (get) Token: 0x060018DD RID: 6365 RVA: 0x000125E0 File Offset: 0x000107E0
	public SimpleBoardAction SimpleAction
	{
		get
		{
			return this.simple_action;
		}
	}

	// Token: 0x060018DE RID: 6366 RVA: 0x000125E8 File Offset: 0x000107E8
	public ActionSimple(SimpleBoardAction _action)
	{
		this.action_type = BoardActionType.Simple;
		this.simple_action = _action;
	}

	// Token: 0x060018DF RID: 6367 RVA: 0x000125FE File Offset: 0x000107FE
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write((byte)this.simple_action);
			return;
		}
		this.simple_action = (SimpleBoardAction)bs.ReadByte();
	}

	// Token: 0x04001A9D RID: 6813
	public SimpleBoardAction simple_action;
}
