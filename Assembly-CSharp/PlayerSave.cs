using System;
using System.IO;

// Token: 0x020003ED RID: 1005
[Serializable]
public class PlayerSave
{
	// Token: 0x06001BAF RID: 7087 RVA: 0x000BA2E8 File Offset: 0x000B84E8
	public void Serialize(BinaryWriter bs)
	{
		bs.Write(this.turnOrderRoll);
		bs.Write(this.health);
		bs.Write(this.gold);
		bs.Write(this.goalScore);
		bs.Write(this.curNodeID);
		bs.Write(this.postMinigameItem);
		bs.Write(this.cactusActive);
		for (int i = 0; i < this.inventory.Length; i++)
		{
			bs.Write(this.inventory[i]);
		}
		bs.Write(this.name);
		bs.Write(this.slotColor);
		bs.Write(this.slotSkin);
		bs.Write(this.slotHat);
		bs.Write(this.botDifficulty);
	}

	// Token: 0x06001BB0 RID: 7088 RVA: 0x000BA3A8 File Offset: 0x000B85A8
	public void Serialize(BinaryReader bs)
	{
		this.turnOrderRoll = bs.ReadInt32();
		this.health = bs.ReadInt16();
		this.gold = bs.ReadInt16();
		this.goalScore = bs.ReadByte();
		this.curNodeID = bs.ReadInt16();
		this.postMinigameItem = bs.ReadByte();
		this.cactusActive = bs.ReadBoolean();
		for (int i = 0; i < this.inventory.Length; i++)
		{
			this.inventory[i] = bs.ReadByte();
		}
		this.name = bs.ReadString();
		this.slotColor = bs.ReadUInt16();
		this.slotSkin = bs.ReadUInt16();
		this.slotHat = bs.ReadByte();
		this.botDifficulty = bs.ReadByte();
	}

	// Token: 0x04001DC8 RID: 7624
	public int turnOrderRoll;

	// Token: 0x04001DC9 RID: 7625
	public short health = 30;

	// Token: 0x04001DCA RID: 7626
	public short gold = 35;

	// Token: 0x04001DCB RID: 7627
	public byte goalScore;

	// Token: 0x04001DCC RID: 7628
	public short curNodeID;

	// Token: 0x04001DCD RID: 7629
	public byte postMinigameItem = byte.MaxValue;

	// Token: 0x04001DCE RID: 7630
	public byte[] inventory = new byte[14];

	// Token: 0x04001DCF RID: 7631
	public bool cactusActive;

	// Token: 0x04001DD0 RID: 7632
	public string name = "DefualtPlayer";

	// Token: 0x04001DD1 RID: 7633
	public ushort slotColor;

	// Token: 0x04001DD2 RID: 7634
	public ushort slotSkin;

	// Token: 0x04001DD3 RID: 7635
	public byte slotHat;

	// Token: 0x04001DD4 RID: 7636
	public byte botDifficulty;
}
