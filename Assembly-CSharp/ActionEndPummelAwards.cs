using System;
using ZP.Utility;

// Token: 0x020003B1 RID: 945
public class ActionEndPummelAwards : BoardAction
{
	// Token: 0x06001931 RID: 6449 RVA: 0x00012B70 File Offset: 0x00010D70
	public ActionEndPummelAwards()
	{
		this.action_type = BoardActionType.EndPummelAwards;
	}

	// Token: 0x06001932 RID: 6450 RVA: 0x00012B80 File Offset: 0x00010D80
	public ActionEndPummelAwards(bool isGameFinished)
	{
		this.action_type = BoardActionType.EndPummelAwards;
		this.isGameFinished = isGameFinished;
	}

	// Token: 0x06001933 RID: 6451 RVA: 0x00012B97 File Offset: 0x00010D97
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.isGameFinished);
			return;
		}
		this.isGameFinished = bs.ReadBool();
	}

	// Token: 0x04001AD1 RID: 6865
	public bool isGameFinished;
}
