using System;
using System.IO;

// Token: 0x020003EE RID: 1006
[Serializable]
public class ReaperNode
{
	// Token: 0x06001BB2 RID: 7090 RVA: 0x00014400 File Offset: 0x00012600
	public void Serialize(BinaryWriter bs)
	{
		bs.Write(this.nodeID);
		bs.Write(this.playerID);
		bs.Write(this.choice);
	}

	// Token: 0x06001BB3 RID: 7091 RVA: 0x00014426 File Offset: 0x00012626
	public void Serialize(BinaryReader bs)
	{
		this.nodeID = bs.ReadInt16();
		this.playerID = bs.ReadByte();
		this.choice = bs.ReadByte();
	}

	// Token: 0x04001DD5 RID: 7637
	public short nodeID;

	// Token: 0x04001DD6 RID: 7638
	public byte playerID;

	// Token: 0x04001DD7 RID: 7639
	public byte choice;
}
