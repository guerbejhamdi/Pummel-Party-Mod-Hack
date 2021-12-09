using System;
using ZP.Utility;

// Token: 0x020003A9 RID: 937
public class ActionDoNodeEvent : BoardAction
{
	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06001912 RID: 6418 RVA: 0x00012969 File Offset: 0x00010B69
	public ushort NodeID
	{
		get
		{
			return this.node_id;
		}
	}

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06001913 RID: 6419 RVA: 0x00012971 File Offset: 0x00010B71
	public ushort PlayerID
	{
		get
		{
			return this.player_id;
		}
	}

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06001914 RID: 6420 RVA: 0x00012979 File Offset: 0x00010B79
	public int Seed
	{
		get
		{
			return this.seed;
		}
	}

	// Token: 0x06001915 RID: 6421 RVA: 0x00012981 File Offset: 0x00010B81
	public ActionDoNodeEvent(ushort _node_id, ushort _player_id, int _seed)
	{
		this.action_type = BoardActionType.DoNodeEvent;
		this.node_id = _node_id;
		this.player_id = _player_id;
		this.seed = _seed;
	}

	// Token: 0x06001916 RID: 6422 RVA: 0x000A9A34 File Offset: 0x000A7C34
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.node_id);
			bs.Write(this.player_id);
			bs.Write(this.seed);
			return;
		}
		this.node_id = bs.ReadUShort();
		this.player_id = bs.ReadUShort();
		this.seed = bs.ReadInt();
	}

	// Token: 0x04001AB7 RID: 6839
	private ushort node_id;

	// Token: 0x04001AB8 RID: 6840
	private ushort player_id;

	// Token: 0x04001AB9 RID: 6841
	private int seed;
}
