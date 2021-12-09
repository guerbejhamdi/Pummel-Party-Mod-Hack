using System;
using MalbersAnimations.Events;
using UnityEngine;

namespace MalbersAnimations.Scriptables
{
	// Token: 0x0200076A RID: 1898
	[CreateAssetMenu(menuName = "Malbers Animations/Scriptable Variables/Vector3 Var")]
	public class Vector3Var : ScriptableObject
	{
		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x060036A0 RID: 13984 RVA: 0x000253CE File Offset: 0x000235CE
		// (set) Token: 0x060036A1 RID: 13985 RVA: 0x000253D6 File Offset: 0x000235D6
		public virtual Vector3 Value
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

		// Token: 0x060036A2 RID: 13986 RVA: 0x00025401 File Offset: 0x00023601
		public virtual void SetValue(Vector3Var var)
		{
			this.Value = var.Value;
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x0002540F File Offset: 0x0002360F
		public static implicit operator Vector3(Vector3Var reference)
		{
			return reference.Value;
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x00025417 File Offset: 0x00023617
		public static implicit operator Vector2(Vector3Var reference)
		{
			return reference.Value;
		}

		// Token: 0x040035EA RID: 13802
		[SerializeField]
		private Vector3 value = Vector3.zero;

		// Token: 0x040035EB RID: 13803
		public bool UseEvent = true;

		// Token: 0x040035EC RID: 13804
		public Vector3Event OnValueChanged = new Vector3Event();
	}
}
