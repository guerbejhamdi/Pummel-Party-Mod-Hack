using System;

namespace MalbersAnimations.Scriptables
{
	// Token: 0x02000761 RID: 1889
	[Serializable]
	public class BoolReference
	{
		// Token: 0x0600366C RID: 13932 RVA: 0x00024F67 File Offset: 0x00023167
		public BoolReference()
		{
			this.UseConstant = true;
			this.ConstantValue = false;
			this.DefaultValue = false;
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x00024F8B File Offset: 0x0002318B
		public BoolReference(bool value)
		{
			this.Value = value;
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x0600366E RID: 13934 RVA: 0x00024FA1 File Offset: 0x000231A1
		// (set) Token: 0x0600366F RID: 13935 RVA: 0x00024FBD File Offset: 0x000231BD
		public bool Value
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

		// Token: 0x06003670 RID: 13936 RVA: 0x00024FDB File Offset: 0x000231DB
		public static implicit operator bool(BoolReference reference)
		{
			return reference.Value;
		}

		// Token: 0x040035CB RID: 13771
		public bool UseConstant = true;

		// Token: 0x040035CC RID: 13772
		public bool ConstantValue;

		// Token: 0x040035CD RID: 13773
		public bool DefaultValue;

		// Token: 0x040035CE RID: 13774
		public BoolVar Variable;
	}
}
