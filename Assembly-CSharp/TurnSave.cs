using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZP.Utility;

// Token: 0x020003EC RID: 1004
[Serializable]
public class TurnSave
{
	// Token: 0x06001BAD RID: 7085 RVA: 0x000B9EC0 File Offset: 0x000B80C0
	public void Serialize(BinaryWriter bs)
	{
		bs.Write(this.curTurnNum);
		bs.Write(this.shellGameSeed);
		bs.Write((byte)this.goalNodeIDs.Length);
		for (int i = 0; i < this.goalNodeIDs.Length; i++)
		{
			bs.Write(this.goalNodeIDs[i]);
		}
		bs.Write((byte)this.weaponNodeIDs.Length);
		for (int j = 0; j < this.weaponNodeIDs.Length; j++)
		{
			bs.Write(this.weaponNodeIDs[j]);
			bs.Write(this.weaponIDs[j]);
		}
		bs.Write(this.killerPosition.x);
		bs.Write(this.killerPosition.y);
		bs.Write(this.killerPosition.z);
		bs.Write(this.randomMapIndex);
		bs.Write(this.randomMapSeed);
		bs.Write(this.playerCount);
		bs.Write(this.eventActive);
		bs.Write(this.eventValue1);
		bs.Write(this.eventValue2);
		for (int k = 0; k < 8; k++)
		{
			this.players[k].Serialize(bs);
		}
		bs.Write(this.curKeyID);
		bs.Write((short)this.keys.Length);
		for (int l = 0; l < this.keys.Length; l++)
		{
			this.keys[l].Serialize(bs);
		}
		bs.Write((short)this.reaperNodes.Length);
		for (int m = 0; m < this.reaperNodes.Length; m++)
		{
			this.reaperNodes[m].Serialize(bs);
		}
		bs.Write(this.persistentItems.Length);
		bs.Write(this.persistentItems);
		ZPBitStream zpbitStream = new ZPBitStream();
		StatTracker.SerializeStats(zpbitStream, this.stats);
		Debug.Log("Byte Length: " + zpbitStream.GetByteLength().ToString());
		Debug.Log("Data Copy Length: " + zpbitStream.GetDataCopy().Length.ToString());
		bs.Write(zpbitStream.GetByteLength());
		bs.Write(zpbitStream.GetDataCopy());
	}

	// Token: 0x06001BAE RID: 7086 RVA: 0x000BA0E0 File Offset: 0x000B82E0
	public void Serialize(BinaryReader bs)
	{
		this.curTurnNum = bs.ReadInt32();
		this.shellGameSeed = bs.ReadInt32();
		int num = (int)bs.ReadByte();
		this.goalNodeIDs = new short[num];
		for (int i = 0; i < num; i++)
		{
			this.goalNodeIDs[i] = bs.ReadInt16();
		}
		int num2 = (int)bs.ReadByte();
		this.weaponNodeIDs = new short[num2];
		this.weaponIDs = new byte[num2];
		for (int j = 0; j < num2; j++)
		{
			this.weaponNodeIDs[j] = bs.ReadInt16();
			this.weaponIDs[j] = bs.ReadByte();
		}
		this.killerPosition.Set(bs.ReadSingle(), bs.ReadSingle(), bs.ReadSingle());
		this.randomMapIndex = bs.ReadByte();
		this.randomMapSeed = bs.ReadInt32();
		this.playerCount = bs.ReadByte();
		this.eventActive = bs.ReadBoolean();
		this.eventValue1 = bs.ReadInt32();
		this.eventValue2 = bs.ReadInt32();
		for (int k = 0; k < 8; k++)
		{
			this.players[k] = new PlayerSave();
			this.players[k].Serialize(bs);
		}
		this.curKeyID = bs.ReadInt32();
		short num3 = bs.ReadInt16();
		this.keys = new KeySave[(int)num3];
		for (int l = 0; l < (int)num3; l++)
		{
			this.keys[l] = new KeySave();
			this.keys[l].Serialize(bs);
		}
		short num4 = bs.ReadInt16();
		this.reaperNodes = new ReaperNode[(int)num4];
		for (int m = 0; m < this.reaperNodes.Length; m++)
		{
			this.reaperNodes[m] = new ReaperNode();
			this.reaperNodes[m].Serialize(bs);
		}
		int count = bs.ReadInt32();
		this.persistentItems = bs.ReadBytes(count);
		int num5 = bs.ReadInt32();
		ZPBitStream bs2 = new ZPBitStream(bs.ReadBytes(num5), num5 * 8);
		this.stats = StatTracker.DeserializeStats(bs2);
	}

	// Token: 0x04001DB6 RID: 7606
	public int curTurnNum;

	// Token: 0x04001DB7 RID: 7607
	public int shellGameSeed;

	// Token: 0x04001DB8 RID: 7608
	public short[] goalNodeIDs;

	// Token: 0x04001DB9 RID: 7609
	public short[] weaponNodeIDs;

	// Token: 0x04001DBA RID: 7610
	public byte[] weaponIDs;

	// Token: 0x04001DBB RID: 7611
	public int curKeyID;

	// Token: 0x04001DBC RID: 7612
	public bool eventActive;

	// Token: 0x04001DBD RID: 7613
	public int eventValue1;

	// Token: 0x04001DBE RID: 7614
	public int eventValue2;

	// Token: 0x04001DBF RID: 7615
	public Vector3 killerPosition = Vector3.zero;

	// Token: 0x04001DC0 RID: 7616
	public byte randomMapIndex;

	// Token: 0x04001DC1 RID: 7617
	public int randomMapSeed;

	// Token: 0x04001DC2 RID: 7618
	public byte playerCount;

	// Token: 0x04001DC3 RID: 7619
	public KeySave[] keys;

	// Token: 0x04001DC4 RID: 7620
	public PlayerSave[] players = new PlayerSave[8];

	// Token: 0x04001DC5 RID: 7621
	public ReaperNode[] reaperNodes;

	// Token: 0x04001DC6 RID: 7622
	public byte[] persistentItems;

	// Token: 0x04001DC7 RID: 7623
	public Dictionary<short, List<StatInfo>> stats;
}
