using System;

namespace Rewired.Internal
{
	// Token: 0x02000653 RID: 1619
	public static class ControllerTemplateFactory
	{
		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002CD0 RID: 11472 RVA: 0x0001E320 File Offset: 0x0001C520
		public static Type[] templateTypes
		{
			get
			{
				return ControllerTemplateFactory._defaultTemplateTypes;
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06002CD1 RID: 11473 RVA: 0x0001E327 File Offset: 0x0001C527
		public static Type[] templateInterfaceTypes
		{
			get
			{
				return ControllerTemplateFactory._defaultTemplateInterfaceTypes;
			}
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x000F8718 File Offset: 0x000F6918
		public static IControllerTemplate Create(Guid typeGuid, object payload)
		{
			if (typeGuid == GamepadTemplate.typeGuid)
			{
				return new GamepadTemplate(payload);
			}
			if (typeGuid == RacingWheelTemplate.typeGuid)
			{
				return new RacingWheelTemplate(payload);
			}
			if (typeGuid == HOTASTemplate.typeGuid)
			{
				return new HOTASTemplate(payload);
			}
			if (typeGuid == FlightYokeTemplate.typeGuid)
			{
				return new FlightYokeTemplate(payload);
			}
			if (typeGuid == FlightPedalsTemplate.typeGuid)
			{
				return new FlightPedalsTemplate(payload);
			}
			if (typeGuid == SixDofControllerTemplate.typeGuid)
			{
				return new SixDofControllerTemplate(payload);
			}
			return null;
		}

		// Token: 0x04002E0E RID: 11790
		private static readonly Type[] _defaultTemplateTypes = new Type[]
		{
			typeof(GamepadTemplate),
			typeof(RacingWheelTemplate),
			typeof(HOTASTemplate),
			typeof(FlightYokeTemplate),
			typeof(FlightPedalsTemplate),
			typeof(SixDofControllerTemplate)
		};

		// Token: 0x04002E0F RID: 11791
		private static readonly Type[] _defaultTemplateInterfaceTypes = new Type[]
		{
			typeof(IGamepadTemplate),
			typeof(IRacingWheelTemplate),
			typeof(IHOTASTemplate),
			typeof(IFlightYokeTemplate),
			typeof(IFlightPedalsTemplate),
			typeof(ISixDofControllerTemplate)
		};
	}
}
