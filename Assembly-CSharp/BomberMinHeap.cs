using System;

// Token: 0x0200015E RID: 350
public class BomberMinHeap
{
	// Token: 0x06000A1B RID: 2587 RVA: 0x0000A9B5 File Offset: 0x00008BB5
	public bool HasNext()
	{
		return this.listHead != null;
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x00059874 File Offset: 0x00057A74
	public void Add(BomberSearchNode item)
	{
		if (this.listHead == null)
		{
			this.listHead = item;
			return;
		}
		if (this.listHead.next == null && item.cost <= this.listHead.cost)
		{
			item.nextListElem = this.listHead;
			this.listHead = item;
			return;
		}
		BomberSearchNode nextListElem = this.listHead;
		while (nextListElem.nextListElem != null && nextListElem.nextListElem.cost < item.cost)
		{
			nextListElem = nextListElem.nextListElem;
		}
		item.nextListElem = nextListElem.nextListElem;
		nextListElem.nextListElem = item;
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0000A9C0 File Offset: 0x00008BC0
	public BomberSearchNode ExtractFirst()
	{
		BomberSearchNode result = this.listHead;
		this.listHead = this.listHead.nextListElem;
		return result;
	}

	// Token: 0x040008E5 RID: 2277
	private BomberSearchNode listHead;
}
