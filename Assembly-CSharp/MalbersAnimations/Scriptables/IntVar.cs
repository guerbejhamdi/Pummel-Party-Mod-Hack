using System;
using MalbersAnimations.Events;
using UnityEngine;

namespace MalbersAnimations.Scriptables
{
	// Token: 0x02000766 RID: 1894
	[CreateAssetMenu(menuName = "Malbers Animations/Scriptable Variables/Int Var")]
	public class IntVar : ScriptableObject
	{
		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06003686 RID: 13958 RVA: 0x000251B3 File Offset: 0x000233B3
		// (set) Token: 0x06003687 RID: 13959 RVA: 0x000251BB File Offset: 0x000233BB
		public virtual int Value
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

		// Token: 0x06003688 RID: 13960 RVA: 0x000251E1 File Offset: 0x000233E1
		public virtual void SetValue(IntVar var)
		{
			this.Value = var.Value;
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x000251EF File Offset: 0x000233EF
		public static implicit operator int(IntVar reference)
		{
			return reference.Value;
		}

		// Token: 0x040035DC RID: 13788
		[SerializeField]
		private int value;

		// Token: 0x040035DD RID: 13789
		public bool UseEvent;

		// Token: 0x040035DE RID: 13790
		public IntEvent OnValueChanged = new IntEvent();
	}
}
