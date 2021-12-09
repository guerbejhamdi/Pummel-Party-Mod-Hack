using System;
using UnityEngine;
using ZP.Utility;

// Token: 0x020003B5 RID: 949
public class ActionKillerMove : BoardAction
{
	// Token: 0x17000297 RID: 663
	// (get) Token: 0x0600193C RID: 6460 RVA: 0x00012C51 File Offset: 0x00010E51
	public Vector3 Point
	{
		get
		{
			return this.point;
		}
	}

	// Token: 0x0600193D RID: 6461 RVA: 0x00012C59 File Offset: 0x00010E59
	public ActionKillerMove(Vector3 point)
	{
		this.point = point;
		this.action_type = BoardActionType.KillerMove;
	}

	// Token: 0x0600193E RID: 6462 RVA: 0x000AA124 File Offset: 0x000A8324
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.point.x);
			bs.Write(this.point.y);
			bs.Write(this.point.z);
			return;
		}
		this.point.Set(bs.ReadFloat(), bs.ReadFloat(), bs.ReadFloat());
	}

	// Token: 0x04001AD5 RID: 6869
	private Vector3 point = Vector3.zero;
}
