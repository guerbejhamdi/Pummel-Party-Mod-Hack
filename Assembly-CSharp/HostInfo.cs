using System;
using System.Net;

// Token: 0x0200047A RID: 1146
public class HostInfo
{
	// Token: 0x06001EEC RID: 7916 RVA: 0x00016CEB File Offset: 0x00014EEB
	public HostInfo(IPEndPoint internalEndPoint, IPEndPoint externalEndPoint)
	{
		this.m_internalEndPoint = internalEndPoint;
		this.m_externalEndPoint = externalEndPoint;
	}

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x06001EED RID: 7917 RVA: 0x00016D01 File Offset: 0x00014F01
	public IPEndPoint Internal
	{
		get
		{
			return this.m_internalEndPoint;
		}
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x06001EEE RID: 7918 RVA: 0x00016D09 File Offset: 0x00014F09
	public IPEndPoint External
	{
		get
		{
			return this.m_externalEndPoint;
		}
	}

	// Token: 0x040021E7 RID: 8679
	private IPEndPoint m_internalEndPoint;

	// Token: 0x040021E8 RID: 8680
	private IPEndPoint m_externalEndPoint;
}
