using System;

// Token: 0x02000107 RID: 263
public class LTEvent
{
	// Token: 0x060007A2 RID: 1954 RVA: 0x00009416 File Offset: 0x00007616
	public LTEvent(int id, object data)
	{
		this.id = id;
		this.data = data;
	}

	// Token: 0x04000650 RID: 1616
	public int id;

	// Token: 0x04000651 RID: 1617
	public object data;
}
