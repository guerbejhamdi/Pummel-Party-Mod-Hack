using System;
using ZP.Utility;

// Token: 0x020003AD RID: 941
public class ActionUseItem : BoardAction
{
	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06001921 RID: 6433 RVA: 0x00012A47 File Offset: 0x00010C47
	public byte ItemID
	{
		get
		{
			return this.item_id;
		}
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06001922 RID: 6434 RVA: 0x00012A4F File Offset: 0x00010C4F
	public short PlayerID
	{
		get
		{
			return this.player_id;
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06001923 RID: 6435 RVA: 0x00012A57 File Offset: 0x00010C57
	public byte[] Variables
	{
		get
		{
			return this.variables;
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06001924 RID: 6436 RVA: 0x00012A5F File Offset: 0x00010C5F
	public int Seed
	{
		get
		{
			return this.seed;
		}
	}

	// Token: 0x06001925 RID: 6437 RVA: 0x00012A67 File Offset: 0x00010C67
	public ActionUseItem(byte _item_id, short _player_id, byte[] _variables, int seed)
	{
		this.action_type = BoardActionType.UseItem;
		this.item_id = _item_id;
		this.player_id = _player_id;
		this.variables = _variables;
	}

	// Token: 0x06001926 RID: 6438 RVA: 0x000A9F4C File Offset: 0x000A814C
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (!write)
		{
			this.item_id = bs.ReadByte();
			this.player_id = bs.ReadShort();
			this.seed = bs.ReadInt();
			int num = bs.ReadInt();
			if (num != 0)
			{
				this.variables = new byte[num];
				for (int i = 0; i < this.variables.Length; i++)
				{
					this.variables[i] = bs.ReadByte();
				}
			}
			return;
		}
		bs.Write(this.item_id);
		bs.Write(this.player_id);
		bs.Write(this.seed);
		if (this.variables != null)
		{
			bs.Write(this.variables.Length);
			for (int j = 0; j < this.variables.Length; j++)
			{
				bs.Write(this.variables[j]);
			}
			return;
		}
		bs.Write(0);
	}

	// Token: 0x04001AC6 RID: 6854
	private byte item_id;

	// Token: 0x04001AC7 RID: 6855
	private short player_id;

	// Token: 0x04001AC8 RID: 6856
	private byte[] variables;

	// Token: 0x04001AC9 RID: 6857
	private int seed;
}
