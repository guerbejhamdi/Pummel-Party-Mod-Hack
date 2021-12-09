using System;

// Token: 0x02000525 RID: 1317
[Serializable]
public class Name
{
	// Token: 0x060022A6 RID: 8870 RVA: 0x00019003 File Offset: 0x00017203
	public Name(string name, NameStatus status, bool save)
	{
		this.name = name;
		this.status = status;
		this.save = save;
	}

	// Token: 0x04002582 RID: 9602
	public NameStatus status;

	// Token: 0x04002583 RID: 9603
	public string name;

	// Token: 0x04002584 RID: 9604
	public bool save;
}
