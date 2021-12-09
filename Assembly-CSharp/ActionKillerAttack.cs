using System;
using ZP.Utility;

// Token: 0x020003B6 RID: 950
public class ActionKillerAttack : BoardAction
{
	// Token: 0x17000298 RID: 664
	// (get) Token: 0x0600193F RID: 6463 RVA: 0x00012C7B File Offset: 0x00010E7B
	public byte ActorID
	{
		get
		{
			return this.actorID;
		}
	}

	// Token: 0x06001940 RID: 6464 RVA: 0x00012C83 File Offset: 0x00010E83
	public ActionKillerAttack(byte actorID)
	{
		this.actorID = actorID;
		this.action_type = BoardActionType.KillerAttack;
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x00012C9A File Offset: 0x00010E9A
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.actorID);
			return;
		}
		this.actorID = bs.ReadByte();
	}

	// Token: 0x04001AD6 RID: 6870
	private byte actorID;
}
