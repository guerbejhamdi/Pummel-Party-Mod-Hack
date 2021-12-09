using System;
using UnityEngine;

namespace MalbersAnimations.Scriptables
{
	// Token: 0x02000763 RID: 1891
	[Serializable]
	public class FloatReference
	{
		// Token: 0x06003676 RID: 13942 RVA: 0x00025041 File Offset: 0x00023241
		public FloatReference()
		{
			this.UseConstant = true;
			this.ConstantValue = 0f;
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x00116EEC File Offset: 0x001150EC
		public FloatReference(bool variable = false)
		{
			this.UseConstant = !variable;
			if (!variable)
			{
				this.ConstantValue = 0f;
				return;
			}
			this.Variable = ScriptableObject.CreateInstance<FloatVar>();
			this.Variable.Value = 0f;
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x00025062 File Offset: 0x00023262
		public FloatReference(float value)
		{
			this.Value = value;
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06003679 RID: 13945 RVA: 0x00025078 File Offset: 0x00023278
		// (set) Token: 0x0600367A RID: 13946 RVA: 0x00025094 File Offset: 0x00023294
		public float Value
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

		// Token: 0x0600367B RID: 13947 RVA: 0x000250B2 File Offset: 0x000232B2
		public static implicit operator float(FloatReference reference)
		{
			return reference.Value;
		}

		// Token: 0x040035D2 RID: 13778
		public bool UseConstant = true;

		// Token: 0x040035D3 RID: 13779
		public float ConstantValue;

		// Token: 0x040035D4 RID: 13780
		public FloatVar Variable;
	}
}
