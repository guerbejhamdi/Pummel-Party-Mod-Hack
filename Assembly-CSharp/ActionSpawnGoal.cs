using System;
using ZP.Utility;

// Token: 0x020003A1 RID: 929
public class ActionSpawnGoal : BoardAction
{
	// Token: 0x17000274 RID: 628
	// (get) Token: 0x060018EE RID: 6382 RVA: 0x00012727 File Offset: 0x00010927
	public byte GoalIndex
	{
		get
		{
			return this.goalIndex;
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x060018EF RID: 6383 RVA: 0x0001272F File Offset: 0x0001092F
	public byte HackyFixSeed
	{
		get
		{
			return this.hackyFixSeed;
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x060018F0 RID: 6384 RVA: 0x00012737 File Offset: 0x00010937
	public short NodeIndex
	{
		get
		{
			return this.node_index;
		}
	}

	// Token: 0x060018F1 RID: 6385 RVA: 0x0001273F File Offset: 0x0001093F
	public ActionSpawnGoal(byte _goalIndex, short _node_index)
	{
		this.action_type = BoardActionType.SpawnGoal;
		this.goalIndex = _goalIndex;
		this.node_index = _node_index;
		this.hackyFixSeed = (byte)GameManager.rand.Next(0, 254);
	}

	// Token: 0x060018F2 RID: 6386 RVA: 0x000A97B4 File Offset: 0x000A79B4
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.goalIndex);
			bs.Write(this.node_index);
			bs.Write(this.hackyFixSeed);
			return;
		}
		this.goalIndex = bs.ReadByte();
		this.node_index = bs.ReadShort();
		this.hackyFixSeed = bs.ReadByte();
	}

	// Token: 0x04001AA3 RID: 6819
	private byte goalIndex;

	// Token: 0x04001AA4 RID: 6820
	private short node_index;

	// Token: 0x04001AA5 RID: 6821
	private byte hackyFixSeed;
}
