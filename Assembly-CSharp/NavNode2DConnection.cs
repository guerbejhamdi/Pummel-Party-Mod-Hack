using System;

// Token: 0x02000142 RID: 322
[Serializable]
public class NavNode2DConnection
{
	// Token: 0x0600093B RID: 2363 RVA: 0x0000A379 File Offset: 0x00008579
	public NavNode2DConnection(NavNode2D t)
	{
		this.target = t;
	}

	// Token: 0x040007C2 RID: 1986
	public bool useDistance = true;

	// Token: 0x040007C3 RID: 1987
	public NavNode2D target;
}
