using System;
using ZP.Utility;

// Token: 0x0200039E RID: 926
public class ActionWait : BoardAction
{
	// Token: 0x1700026F RID: 623
	// (get) Token: 0x060018E0 RID: 6368 RVA: 0x0001261D File Offset: 0x0001081D
	public float WaitTime
	{
		get
		{
			return this.wait_time;
		}
	}

	// Token: 0x060018E1 RID: 6369 RVA: 0x00012625 File Offset: 0x00010825
	public ActionWait(float time)
	{
		this.action_type = BoardActionType.Wait;
		this.wait_time = time;
	}

	// Token: 0x060018E2 RID: 6370 RVA: 0x0001263B File Offset: 0x0001083B
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.wait_time);
			return;
		}
		this.wait_time = bs.ReadFloat();
	}

	// Token: 0x04001A9E RID: 6814
	private float wait_time;
}
