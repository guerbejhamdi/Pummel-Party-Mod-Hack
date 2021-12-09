using System;

namespace Rewired
{
	// Token: 0x0200063F RID: 1599
	public interface IFlightPedalsTemplate : IControllerTemplate
	{
		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06002AC7 RID: 10951
		IControllerTemplateAxis leftPedal { get; }

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06002AC8 RID: 10952
		IControllerTemplateAxis rightPedal { get; }

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06002AC9 RID: 10953
		IControllerTemplateAxis slide { get; }
	}
}
