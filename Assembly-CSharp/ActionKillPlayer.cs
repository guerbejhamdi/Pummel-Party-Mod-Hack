using System;
using UnityEngine;
using ZP.Utility;

// Token: 0x020003AA RID: 938
public class ActionKillPlayer : BoardAction
{
	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06001917 RID: 6423 RVA: 0x000129A6 File Offset: 0x00010BA6
	public short PlayerID
	{
		get
		{
			return this.target_id;
		}
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06001918 RID: 6424 RVA: 0x000129AE File Offset: 0x00010BAE
	public short KillerID
	{
		get
		{
			return this.killer_id;
		}
	}

	// Token: 0x06001919 RID: 6425 RVA: 0x000129B6 File Offset: 0x00010BB6
	public ActionKillPlayer(short _target_id, short _killer_id, Vector3 _origin, float _force)
	{
		this.action_type = BoardActionType.KillPlayer;
		this.target_id = _target_id;
		this.killer_id = _killer_id;
		this.origin = _origin;
		this.force = _force;
	}

	// Token: 0x0600191A RID: 6426 RVA: 0x000A9A90 File Offset: 0x000A7C90
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.target_id);
			bs.Write(this.killer_id);
			bs.Write(this.origin.x);
			bs.Write(this.origin.y);
			bs.Write(this.origin.z);
			bs.Write(this.force);
			return;
		}
		this.target_id = bs.ReadShort();
		this.killer_id = bs.ReadShort();
		this.origin = new Vector3(bs.ReadFloat(), bs.ReadFloat(), bs.ReadFloat());
		this.force = bs.ReadFloat();
	}

	// Token: 0x04001ABA RID: 6842
	private short target_id;

	// Token: 0x04001ABB RID: 6843
	private short killer_id;

	// Token: 0x04001ABC RID: 6844
	private Vector3 origin;

	// Token: 0x04001ABD RID: 6845
	private float force;
}
