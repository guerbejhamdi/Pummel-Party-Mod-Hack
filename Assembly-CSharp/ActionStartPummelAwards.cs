using System;
using ZP.Utility;

// Token: 0x020003B0 RID: 944
public class ActionStartPummelAwards : BoardAction
{
	// Token: 0x0600192E RID: 6446 RVA: 0x00012B2D File Offset: 0x00010D2D
	public ActionStartPummelAwards()
	{
		this.action_type = BoardActionType.StartPummelAwards;
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x00012B44 File Offset: 0x00010D44
	public ActionStartPummelAwards(bool doIntroduction, StatChallengeBoardEvent ev, bool isGameFinished)
	{
		this.action_type = BoardActionType.StartPummelAwards;
		this.doIntroduction = doIntroduction;
		this.ev = ev;
		this.isGameFinished = isGameFinished;
	}

	// Token: 0x06001930 RID: 6448 RVA: 0x000AA01C File Offset: 0x000A821C
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.doIntroduction);
			bs.Write((byte)this.ev);
			bs.Write(this.isGameFinished);
			return;
		}
		this.doIntroduction = bs.ReadBool();
		this.ev = (StatChallengeBoardEvent)bs.ReadByte();
		this.isGameFinished = bs.ReadBool();
	}

	// Token: 0x04001ACE RID: 6862
	public bool doIntroduction = true;

	// Token: 0x04001ACF RID: 6863
	public StatChallengeBoardEvent ev;

	// Token: 0x04001AD0 RID: 6864
	public bool isGameFinished;
}
