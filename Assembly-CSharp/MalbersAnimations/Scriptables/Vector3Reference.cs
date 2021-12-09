using System;
using UnityEngine;

namespace MalbersAnimations.Scriptables
{
	// Token: 0x02000769 RID: 1897
	[Serializable]
	public class Vector3Reference
	{
		// Token: 0x06003699 RID: 13977 RVA: 0x00025327 File Offset: 0x00023527
		public Vector3Reference()
		{
			this.UseConstant = true;
			this.ConstantValue = Vector3.zero;
			this.DefaultValue = Vector3.zero;
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x00116F9C File Offset: 0x0011519C
		public Vector3Reference(bool variable = false)
		{
			this.UseConstant = !variable;
			if (!variable)
			{
				this.ConstantValue = Vector3.zero;
				return;
			}
			this.Variable = ScriptableObject.CreateInstance<Vector3Var>();
			this.Variable.Value = Vector3.zero;
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x0002535E File Offset: 0x0002355E
		public Vector3Reference(Vector3 value)
		{
			this.Value = value;
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x0600369C RID: 13980 RVA: 0x0002537F File Offset: 0x0002357F
		// (set) Token: 0x0600369D RID: 13981 RVA: 0x0002539B File Offset: 0x0002359B
		public Vector3 Value
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

		// Token: 0x0600369E RID: 13982 RVA: 0x000253B9 File Offset: 0x000235B9
		public static implicit operator Vector3(Vector3Reference reference)
		{
			return reference.Value;
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x000253C1 File Offset: 0x000235C1
		public static implicit operator Vector2(Vector3Reference reference)
		{
			return reference.Value;
		}

		// Token: 0x040035E6 RID: 13798
		public bool UseConstant = true;

		// Token: 0x040035E7 RID: 13799
		public Vector3 ConstantValue = Vector3.zero;

		// Token: 0x040035E8 RID: 13800
		public Vector3 DefaultValue;

		// Token: 0x040035E9 RID: 13801
		public Vector3Var Variable;
	}
}
