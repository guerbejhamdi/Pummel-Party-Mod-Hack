using System;
using UnityEngine;

namespace MalbersAnimations.Scriptables
{
	// Token: 0x02000765 RID: 1893
	[Serializable]
	public class IntReference
	{
		// Token: 0x06003680 RID: 13952 RVA: 0x00025103 File Offset: 0x00023303
		public IntReference()
		{
			this.UseConstant = true;
			this.ConstantValue = 0;
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x00025120 File Offset: 0x00023320
		public IntReference(bool variable = false)
		{
			this.UseConstant = !variable;
			if (!variable)
			{
				this.ConstantValue = 0;
				return;
			}
			this.Variable = ScriptableObject.CreateInstance<IntVar>();
			this.Variable.Value = 0;
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x0002515B File Offset: 0x0002335B
		public IntReference(int value)
		{
			this.Value = value;
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06003683 RID: 13955 RVA: 0x00025171 File Offset: 0x00023371
		// (set) Token: 0x06003684 RID: 13956 RVA: 0x0002518D File Offset: 0x0002338D
		public int Value
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

		// Token: 0x06003685 RID: 13957 RVA: 0x000251AB File Offset: 0x000233AB
		public static implicit operator int(IntReference reference)
		{
			return reference.Value;
		}

		// Token: 0x040035D8 RID: 13784
		public bool UseConstant = true;

		// Token: 0x040035D9 RID: 13785
		public int ConstantValue;

		// Token: 0x040035DA RID: 13786
		public int ResetValue;

		// Token: 0x040035DB RID: 13787
		public IntVar Variable;
	}
}
