using System;
using System.Collections.Generic;

namespace I2.Loc
{
	// Token: 0x020007F0 RID: 2032
	public class BaseSpecializationManager
	{
		// Token: 0x060039B5 RID: 14773 RVA: 0x00120E2C File Offset: 0x0011F02C
		public virtual void InitializeSpecializations()
		{
			this.mSpecializations = new string[]
			{
				"Any",
				"PC",
				"Touch",
				"Controller",
				"VR",
				"XBox",
				"PS4",
				"OculusVR",
				"ViveVR",
				"GearVR",
				"Android",
				"IOS"
			};
			this.mSpecializationsFallbacks = new Dictionary<string, string>
			{
				{
					"XBox",
					"Controller"
				},
				{
					"PS4",
					"Controller"
				},
				{
					"OculusVR",
					"VR"
				},
				{
					"ViveVR",
					"VR"
				},
				{
					"GearVR",
					"VR"
				},
				{
					"Android",
					"Touch"
				},
				{
					"IOS",
					"Touch"
				}
			};
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x000272E6 File Offset: 0x000254E6
		public virtual string GetCurrentSpecialization()
		{
			if (this.mSpecializations == null)
			{
				this.InitializeSpecializations();
			}
			return "PC";
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x00120F24 File Offset: 0x0011F124
		public virtual string GetFallbackSpecialization(string specialization)
		{
			if (this.mSpecializationsFallbacks == null)
			{
				this.InitializeSpecializations();
			}
			string result;
			if (this.mSpecializationsFallbacks.TryGetValue(specialization, out result))
			{
				return result;
			}
			return "Any";
		}

		// Token: 0x04003813 RID: 14355
		public string[] mSpecializations;

		// Token: 0x04003814 RID: 14356
		public Dictionary<string, string> mSpecializationsFallbacks;
	}
}
