using System;

namespace Rewired
{
	// Token: 0x02000645 RID: 1605
	public sealed class FlightPedalsTemplate : ControllerTemplate, IFlightPedalsTemplate, IControllerTemplate
	{
		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06002BCB RID: 11211 RVA: 0x0001D783 File Offset: 0x0001B983
		IControllerTemplateAxis IFlightPedalsTemplate.leftPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(0);
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06002BCC RID: 11212 RVA: 0x0001D78C File Offset: 0x0001B98C
		IControllerTemplateAxis IFlightPedalsTemplate.rightPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(1);
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06002BCD RID: 11213 RVA: 0x0001D795 File Offset: 0x0001B995
		IControllerTemplateAxis IFlightPedalsTemplate.slide
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(2);
			}
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x0001D769 File Offset: 0x0001B969
		public FlightPedalsTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x04002D99 RID: 11673
		public static readonly Guid typeGuid = new Guid("f6fe76f8-be2a-4db2-b853-9e3652075913");

		// Token: 0x04002D9A RID: 11674
		public const int elementId_leftPedal = 0;

		// Token: 0x04002D9B RID: 11675
		public const int elementId_rightPedal = 1;

		// Token: 0x04002D9C RID: 11676
		public const int elementId_slide = 2;
	}
}
