using System;
using UnityEngine;

namespace MalbersAnimations.Scriptables
{
	// Token: 0x02000767 RID: 1895
	[Serializable]
	public class StringReference
	{
		// Token: 0x0600368B RID: 13963 RVA: 0x0002520A File Offset: 0x0002340A
		public StringReference()
		{
			this.UseConstant = true;
			this.ConstantValue = string.Empty;
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x00116F3C File Offset: 0x0011513C
		public StringReference(bool variable = false)
		{
			this.UseConstant = !variable;
			if (!variable)
			{
				this.ConstantValue = string.Empty;
				return;
			}
			this.Variable = ScriptableObject.CreateInstance<StringVar>();
			this.Variable.Value = string.Empty;
			this.Variable.DefaultValue = string.Empty;
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x0002522B File Offset: 0x0002342B
		public StringReference(string value)
		{
			this.Value = value;
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x0600368E RID: 13966 RVA: 0x00025241 File Offset: 0x00023441
		// (set) Token: 0x0600368F RID: 13967 RVA: 0x0002525D File Offset: 0x0002345D
		public string Value
		{
			get
			{
				if (!this.UseConstant)
				{
					return this.Variable.Value;
				}
				return this.ConstantValue;
			}
			set
			{
				if (this.UseConstant)
				{
					this.ConstantValue = value;
					return;
				}
				this.Variable.Value = value;
			}
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x0002527B File Offset: 0x0002347B
		public static implicit operator string(StringReference reference)
		{
			return reference.Value;
		}

		// Token: 0x040035DF RID: 13791
		public bool UseConstant = true;

		// Token: 0x040035E0 RID: 13792
		public string ConstantValue;

		// Token: 0x040035E1 RID: 13793
		public StringVar Variable;
	}
}
