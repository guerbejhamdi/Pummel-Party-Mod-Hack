using System;

namespace I2.Loc
{
	// Token: 0x020007E8 RID: 2024
	public class GlobalParametersExample : RegisterGlobalParameters
	{
		// Token: 0x06003985 RID: 14725 RVA: 0x001204CC File Offset: 0x0011E6CC
		public override string GetParameterValue(string ParamName)
		{
			if (ParamName == "WINNER")
			{
				return "Javier";
			}
			if (ParamName == "NUM PLAYERS")
			{
				return 5.ToString();
			}
			return null;
		}
	}
}
