using System;
using MalbersAnimations.Events;
using UnityEngine;

namespace MalbersAnimations.Scriptables
{
	// Token: 0x02000768 RID: 1896
	[CreateAssetMenu(menuName = "Malbers Animations/Scriptable Variables/String Var")]
	public class StringVar : ScriptableObject
	{
		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06003691 RID: 13969 RVA: 0x00025283 File Offset: 0x00023483
		// (set) Token: 0x06003692 RID: 13970 RVA: 0x0002528B File Offset: 0x0002348B
		public virtual string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (this.value != value)
				{
					this.value = value;
					if (this.UseEvent)
					{
						this.OnValueChanged.Invoke(value);
					}
				}
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06003693 RID: 13971 RVA: 0x000252B6 File Offset: 0x000234B6
		// (set) Token: 0x06003694 RID: 13972 RVA: 0x000252BE File Offset: 0x000234BE
		public virtual string DefaultValue
		{
			get
			{
				return this.defaultValue;
			}
			set
			{
				this.defaultValue = value;
			}
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x000252C7 File Offset: 0x000234C7
		public virtual void ResetValue()
		{
			this.Value = this.DefaultValue;
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x000252D5 File Offset: 0x000234D5
		public virtual void SetValue(StringVar var)
		{
			this.Value = var.Value;
			this.DefaultValue = var.DefaultValue;
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x000252EF File Offset: 0x000234EF
		public static implicit operator string(StringVar reference)
		{
			return reference.Value;
		}

		// Token: 0x040035E2 RID: 13794
		[SerializeField]
		private string value = "";

		// Token: 0x040035E3 RID: 13795
		[SerializeField]
		private string defaultValue = "";

		// Token: 0x040035E4 RID: 13796
		public bool UseEvent = true;

		// Token: 0x040035E5 RID: 13797
		public StringEvent OnValueChanged = new StringEvent();
	}
}
