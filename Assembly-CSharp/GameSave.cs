using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x020003EB RID: 1003
[Serializable]
public class GameSave
{
	// Token: 0x06001BA9 RID: 7081 RVA: 0x000B9DC0 File Offset: 0x000B7FC0
	public void Serialize(BinaryWriter bs)
	{
		bs.Write(this.ownersName);
		bs.Write(this.time.Ticks);
		for (int i = 0; i < this.lobbyOptions.Length; i++)
		{
			bs.Write(this.lobbyOptions[i]);
		}
		bs.Write((byte)this.turnSaves.Count);
		for (int j = 0; j < this.turnSaves.Count; j++)
		{
			this.turnSaves[j].Serialize(bs);
		}
	}

	// Token: 0x06001BAA RID: 7082 RVA: 0x000B9E48 File Offset: 0x000B8048
	public void Serialize(BinaryReader bs)
	{
		this.ownersName = bs.ReadString();
		this.time = new DateTime(bs.ReadInt64());
		for (int i = 0; i < this.lobbyOptions.Length; i++)
		{
			this.lobbyOptions[i] = bs.ReadByte();
		}
		int num = (int)bs.ReadByte();
		for (int j = 0; j < num; j++)
		{
			TurnSave turnSave = new TurnSave();
			turnSave.Serialize(bs);
			this.turnSaves.Add(turnSave);
		}
	}

	// Token: 0x04001DB2 RID: 7602
	public string ownersName = "Default";

	// Token: 0x04001DB3 RID: 7603
	public DateTime time = DateTime.Now;

	// Token: 0x04001DB4 RID: 7604
	public byte[] lobbyOptions = new byte[7];

	// Token: 0x04001DB5 RID: 7605
	public List<TurnSave> turnSaves = new List<TurnSave>();
}
