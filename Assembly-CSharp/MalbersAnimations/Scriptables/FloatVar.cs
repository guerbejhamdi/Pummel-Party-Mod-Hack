using System;
using MalbersAnimations.Events;
using UnityEngine;

namespace MalbersAnimations.Scriptables
{
	// Token: 0x02000764 RID: 1892
	[CreateAssetMenu(menuName = "Malbers Animations/Scriptable Variables/Float Var")]
	public class FloatVar : ScriptableObject
	{
		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x0600367C RID: 13948 RVA: 0x000250BA File Offset: 0x000232BA
		// (set) Token: 0x0600367D RID: 13949 RVA: 0x000250C2 File Offset: 0x000232C2
		public virtual float Value
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

		// Token: 0x0600367E RID: 13950 RVA: 0x000250E8 File Offset: 0x000232E8
		public static implicit operator float(FloatVar reference)
		{
			return reference.Value;
		}

		// Token: 0x040035D5 RID: 13781
		[SerializeField]
		private float value;

		// Token: 0x040035D6 RID: 13782
		public bool UseEvent;

		// Token: 0x040035D7 RID: 13783
		public FloatEvent OnValueChanged = new FloatEvent();
	}
}
