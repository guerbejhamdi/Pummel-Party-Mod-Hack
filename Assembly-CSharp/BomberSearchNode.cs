using System;

// Token: 0x02000161 RID: 353
public class BomberSearchNode
{
	// Token: 0x06000A33 RID: 2611 RVA: 0x0000AA06 File Offset: 0x00008C06
	public BomberSearchNode(Node node, int cost, int pathCost, BomberSearchNode next)
	{
		this.node = node;
		this.cost = cost;
		this.pathCost = pathCost;
		this.next = next;
	}

	// Token: 0x04000904 RID: 2308
	public Node node;

	// Token: 0x04000905 RID: 2309
	public int cost;

	// Token: 0x04000906 RID: 2310
	public int pathCost;

	// Token: 0x04000907 RID: 2311
	public BomberSearchNode next;

	// Token: 0x04000908 RID: 2312
	public BomberSearchNode nextListElem;
}
