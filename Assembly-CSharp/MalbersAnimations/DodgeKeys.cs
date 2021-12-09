using System;
using UnityEngine;
using UnityEngine.Events;

namespace MalbersAnimations
{
	// Token: 0x02000743 RID: 1859
	public class DodgeKeys : MonoBehaviour
	{
		// Token: 0x06003616 RID: 13846 RVA: 0x00024AC6 File Offset: 0x00022CC6
		private void Start()
		{
			this.animal = base.GetComponent<Animal>();
			this.animal.OnMovementReleased.AddListener(new UnityAction<bool>(this.OnMovementReleased));
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x0011666C File Offset: 0x0011486C
		private void OnMovementReleased(bool released)
		{
			if (!released)
			{
				if (this.animal.Direction != 0f && !this.DodgePressOne)
				{
					this.DodgePressOne = true;
					base.Invoke("ResetDodgeKeys", this.DoubleKeyTime);
					return;
				}
				if (this.animal.Direction != 0f && this.DodgePressOne)
				{
					this.animal.Dodge = true;
					base.Invoke("ResetDodgeKeys", 0.1f);
				}
			}
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x00024AF0 File Offset: 0x00022CF0
		private void ResetDodgeKeys()
		{
			this.DodgePressOne = false;
			this.animal.Dodge = false;
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x00024B05 File Offset: 0x00022D05
		private void OnDisable()
		{
			this.animal.OnMovementReleased.RemoveListener(new UnityAction<bool>(this.OnMovementReleased));
		}

		// Token: 0x0400352B RID: 13611
		private Animal animal;

		// Token: 0x0400352C RID: 13612
		public float DoubleKeyTime = 0.3f;

		// Token: 0x0400352D RID: 13613
		private bool DodgePressOne;
	}
}
