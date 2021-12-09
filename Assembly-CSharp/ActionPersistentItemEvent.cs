using System;
using ZP.Utility;

// Token: 0x020003A7 RID: 935
public class ActionPersistentItemEvent : BoardAction
{
	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06001909 RID: 6409 RVA: 0x000128FE File Offset: 0x00010AFE
	public byte ItemIndex
	{
		get
		{
			return this.itemIndex;
		}
	}

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x0600190A RID: 6410 RVA: 0x00012906 File Offset: 0x00010B06
	public PersistentItemEventType EventType
	{
		get
		{
			return this.eventType;
		}
	}

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x0600190B RID: 6411 RVA: 0x0001290E File Offset: 0x00010B0E
	public byte[] Array
	{
		get
		{
			return this.array;
		}
	}

	// Token: 0x0600190C RID: 6412 RVA: 0x00012916 File Offset: 0x00010B16
	public ActionPersistentItemEvent(byte _itemIndex, PersistentItemEventType _eventType, byte[] _array)
	{
		this.action_type = BoardActionType.PersistentItemEvent;
		this.itemIndex = _itemIndex;
		this.eventType = _eventType;
		this.array = _array;
	}

	// Token: 0x0600190D RID: 6413 RVA: 0x000A98E0 File Offset: 0x000A7AE0
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write((byte)this.eventType);
			bs.Write(this.itemIndex);
			bs.Write(this.array.Length);
			for (int i = 0; i < this.array.Length; i++)
			{
				bs.Write(this.array[i]);
			}
			return;
		}
		this.eventType = (PersistentItemEventType)bs.ReadByte();
		this.itemIndex = bs.ReadByte();
		int num = bs.ReadInt();
		this.array = new byte[num];
		for (int j = 0; j < num; j++)
		{
			this.array[j] = bs.ReadByte();
		}
	}

	// Token: 0x04001AB2 RID: 6834
	private PersistentItemEventType eventType;

	// Token: 0x04001AB3 RID: 6835
	private byte itemIndex;

	// Token: 0x04001AB4 RID: 6836
	private byte[] array;
}
