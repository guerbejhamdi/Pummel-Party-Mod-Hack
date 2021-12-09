using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200084C RID: 2124
	public class RegisterGlobalParameters : MonoBehaviour, ILocalizationParamsManager
	{
		// Token: 0x06003C12 RID: 15378 RVA: 0x0002829B File Offset: 0x0002649B
		public virtual void OnEnable()
		{
			if (!LocalizationManager.ParamManagers.Contains(this))
			{
				LocalizationManager.ParamManagers.Add(this);
				LocalizationManager.LocalizeAll(true);
			}
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x000282BB File Offset: 0x000264BB
		public virtual void OnDisable()
		{
			LocalizationManager.ParamManagers.Remove(this);
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x000053AE File Offset: 0x000035AE
		public virtual string GetParameterValue(string ParamName)
		{
			return null;
		}
	}
}
