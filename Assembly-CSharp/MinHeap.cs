using System;

// Token: 0x02000494 RID: 1172
public class MinHeap
{
	// Token: 0x06001F70 RID: 8048 RVA: 0x000170ED File Offset: 0x000152ED
	public bool HasNext()
	{
		return this.listHead != null;
	}

	// Token: 0x06001F71 RID: 8049 RVA: 0x000C7DBC File Offset: 0x000C5FBC
	public void Add(SearchNode item)
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
		SearchNode nextListElem = this.listHead;
		while (nextListElem.nextListElem != null && nextListElem.nextListElem.cost < item.cost)
		{
			nextListElem = nextListElem.nextListElem;
		}
		item.nextListElem = nextListElem.nextListElem;
		nextListElem.nextListElem = item;
	}

	// Token: 0x06001F72 RID: 8050 RVA: 0x000170F8 File Offset: 0x000152F8
	public SearchNode ExtractFirst()
	{
		SearchNode result = this.listHead;
		this.listHead = this.listHead.nextListElem;
		return result;
	}

	// Token: 0x0400224E RID: 8782
	private SearchNode listHead;
}
