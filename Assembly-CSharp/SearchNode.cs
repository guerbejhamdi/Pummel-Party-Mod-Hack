using System;

// Token: 0x02000495 RID: 1173
public class SearchNode
{
	// Token: 0x06001F74 RID: 8052 RVA: 0x00017111 File Offset: 0x00015311
	public SearchNode(BoardNode node, int cost, int pathCost, SearchNode next)
	{
		this.node = node;
		this.cost = cost;
		this.pathCost = pathCost;
		this.next = next;
	}

	// Token: 0x0400224F RID: 8783
	public BoardNode node;

	// Token: 0x04002250 RID: 8784
	public int cost;

	// Token: 0x04002251 RID: 8785
	public int pathCost;

	// Token: 0x04002252 RID: 8786
	public SearchNode next;

	// Token: 0x04002253 RID: 8787
	public SearchNode nextListElem;
}
