using System;
using UnityEngine;

// Token: 0x020003C3 RID: 963
[Serializable]
public class BoardNodeConnection
{
	// Token: 0x06001982 RID: 6530 RVA: 0x00012F08 File Offset: 0x00011108
	public BoardNodeConnection()
	{
		this.transition = BoardNodeTransition.Walking;
		this.connection_type = BoardNodeConnectionDirection.Forward;
		this.node = null;
	}

	// Token: 0x06001983 RID: 6531 RVA: 0x000AAB90 File Offset: 0x000A8D90
	public BoardNodeConnection GetCopy()
	{
		return new BoardNodeConnection
		{
			transition = this.transition,
			connection_type = this.connection_type,
			node = this.node,
			bezier_handle_a = this.bezier_handle_a,
			bezier_handle_b = this.bezier_handle_b
		};
	}

	// Token: 0x04001B2B RID: 6955
	public BoardNodeTransition transition;

	// Token: 0x04001B2C RID: 6956
	public BoardNodeConnectionDirection connection_type;

	// Token: 0x04001B2D RID: 6957
	public BoardNode node;

	// Token: 0x04001B2E RID: 6958
	public Vector3 bezier_handle_a;

	// Token: 0x04001B2F RID: 6959
	public Vector3 bezier_handle_b;
}
