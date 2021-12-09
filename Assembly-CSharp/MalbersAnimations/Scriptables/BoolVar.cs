using System;
using MalbersAnimations.Events;
using UnityEngine;

namespace MalbersAnimations.Scriptables
{
	// Token: 0x02000762 RID: 1890
	[CreateAssetMenu(menuName = "Malbers Animations/Scriptable Variables/Bool Var")]
	public class BoolVar : ScriptableObject
	{
		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06003671 RID: 13937 RVA: 0x00024FE3 File Offset: 0x000231E3
		// (set) Token: 0x06003672 RID: 13938 RVA: 0x00024FEB File Offset: 0x000231EB
		public virtual bool Value
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

		// Token: 0x06003673 RID: 13939 RVA: 0x00025011 File Offset: 0x00023211
		public virtual void SetValue(BoolVar var)
		{
			this.Value = var.Value;
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x0002501F File Offset: 0x0002321F
		public static implicit operator bool(BoolVar reference)
		{
			return reference.Value;
		}

		// Token: 0x040035CF RID: 13775
		[SerializeField]
		private bool value;

		// Token: 0x040035D0 RID: 13776
		public bool UseEvent = true;

		// Token: 0x040035D1 RID: 13777
		public BoolEvent OnValueChanged = new BoolEvent();
	}
}
