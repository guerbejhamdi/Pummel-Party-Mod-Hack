using System;
using System.IO;

// Token: 0x020003EF RID: 1007
[Serializable]
public class KeySave
{
	// Token: 0x06001BB5 RID: 7093 RVA: 0x0001444C File Offset: 0x0001264C
	public void Serialize(BinaryWriter bs)
	{
		bs.Write(this.playerID);
		bs.Write(this.keyID);
		bs.Write(this.nodeID);
		bs.Write(this.seed);
	}

	// Token: 0x06001BB6 RID: 7094 RVA: 0x0001447E File Offset: 0x0001267E
	public void Serialize(BinaryReader bs)
	{
		this.playerID = bs.ReadByte();
		this.keyID = bs.ReadInt32();
		this.nodeID = bs.ReadInt16();
		this.seed = bs.ReadInt32();
	}

	// Token: 0x04001DD8 RID: 7640
	public byte playerID;

	// Token: 0x04001DD9 RID: 7641
	public int keyID;

	// Token: 0x04001DDA RID: 7642
	public short nodeID;

	// Token: 0x04001DDB RID: 7643
	public int seed;
}
