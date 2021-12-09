using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000841 RID: 2113
	public class LocalizationParamsManager : MonoBehaviour, ILocalizationParamsManager
	{
		// Token: 0x06003BF0 RID: 15344 RVA: 0x0012CF2C File Offset: 0x0012B12C
		public string GetParameterValue(string ParamName)
		{
			if (this._Params != null)
			{
				int i = 0;
				int count = this._Params.Count;
				while (i < count)
				{
					if (this._Params[i].Name == ParamName)
					{
						return this._Params[i].Value;
					}
					i++;
				}
			}
			return null;
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x0012CF88 File Offset: 0x0012B188
		public void SetParameterValue(string ParamName, string ParamValue, bool localize = true)
		{
			bool flag = false;
			int i = 0;
			int count = this._Params.Count;
			while (i < count)
			{
				if (this._Params[i].Name == ParamName)
				{
					LocalizationParamsManager.ParamValue value = this._Params[i];
					value.Value = ParamValue;
					this._Params[i] = value;
					flag = true;
					break;
				}
				i++;
			}
			if (!flag)
			{
				this._Params.Add(new LocalizationParamsManager.ParamValue
				{
					Name = ParamName,
					Value = ParamValue
				});
			}
			if (localize)
			{
				this.OnLocalize();
			}
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x0012D020 File Offset: 0x0012B220
		public void OnLocalize()
		{
			Localize component = base.GetComponent<Localize>();
			if (component != null)
			{
				component.OnLocalize(true);
			}
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x0002828B File Offset: 0x0002648B
		public virtual void OnEnable()
		{
			if (this._IsGlobalManager)
			{
				this.DoAutoRegister();
			}
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x0002829B File Offset: 0x0002649B
		public void DoAutoRegister()
		{
			if (!LocalizationManager.ParamManagers.Contains(this))
			{
				LocalizationManager.ParamManagers.Add(this);
				LocalizationManager.LocalizeAll(true);
			}
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x000282BB File Offset: 0x000264BB
		public void OnDisable()
		{
			LocalizationManager.ParamManagers.Remove(this);
		}

		// Token: 0x0400392E RID: 14638
		[SerializeField]
		public List<LocalizationParamsManager.ParamValue> _Params = new List<LocalizationParamsManager.ParamValue>();

		// Token: 0x0400392F RID: 14639
		public bool _IsGlobalManager;

		// Token: 0x02000842 RID: 2114
		[Serializable]
		public struct ParamValue
		{
			// Token: 0x04003930 RID: 14640
			public string Name;

			// Token: 0x04003931 RID: 14641
			public string Value;
		}
	}
}
