using System;
using ZP.Utility;

// Token: 0x020003A2 RID: 930
public class ActionSpawnWeapon : BoardAction
{
	// Token: 0x17000277 RID: 631
	// (get) Token: 0x060018F3 RID: 6387 RVA: 0x00012773 File Offset: 0x00010973
	public byte WeaponIndex
	{
		get
		{
			return this.weaponIndex;
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x060018F4 RID: 6388 RVA: 0x0001277B File Offset: 0x0001097B
	public byte WeaponID
	{
		get
		{
			return this.weaponID;
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x060018F5 RID: 6389 RVA: 0x00012783 File Offset: 0x00010983
	public short NodeIndex
	{
		get
		{
			return this.nodeIndex;
		}
	}

	// Token: 0x060018F6 RID: 6390 RVA: 0x0001278B File Offset: 0x0001098B
	public ActionSpawnWeapon(byte _weaponIndex, byte _weaponID, short _nodeIndex)
	{
		this.action_type = BoardActionType.SpawnWeapon;
		this.weaponIndex = _weaponIndex;
		this.weaponID = _weaponID;
		this.nodeIndex = _nodeIndex;
	}

	// Token: 0x060018F7 RID: 6391 RVA: 0x000A9810 File Offset: 0x000A7A10
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.weaponIndex);
			bs.Write(this.weaponID);
			bs.Write(this.nodeIndex);
			return;
		}
		this.weaponIndex = bs.ReadByte();
		this.weaponID = bs.ReadByte();
		this.nodeIndex = bs.ReadShort();
	}

	// Token: 0x04001AA6 RID: 6822
	private byte weaponIndex;

	// Token: 0x04001AA7 RID: 6823
	private byte weaponID;

	// Token: 0x04001AA8 RID: 6824
	private short nodeIndex;
}
