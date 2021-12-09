using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020006A4 RID: 1700
	[AddComponentMenu("")]
	[RequireComponent(typeof(Image))]
	public class UIImageHelper : MonoBehaviour
	{
		// Token: 0x06003121 RID: 12577 RVA: 0x00102804 File Offset: 0x00100A04
		public void SetEnabledState(bool newState)
		{
			this.currentState = newState;
			UIImageHelper.State state = newState ? this.enabledState : this.disabledState;
			if (state == null)
			{
				return;
			}
			Image component = base.gameObject.GetComponent<Image>();
			if (component == null)
			{
				Debug.LogError("Image is missing!");
				return;
			}
			state.Set(component);
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x00021506 File Offset: 0x0001F706
		public void SetEnabledStateColor(Color color)
		{
			this.enabledState.color = color;
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x00021514 File Offset: 0x0001F714
		public void SetDisabledStateColor(Color color)
		{
			this.disabledState.color = color;
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x00102858 File Offset: 0x00100A58
		public void Refresh()
		{
			UIImageHelper.State state = this.currentState ? this.enabledState : this.disabledState;
			Image component = base.gameObject.GetComponent<Image>();
			if (component == null)
			{
				return;
			}
			state.Set(component);
		}

		// Token: 0x04003045 RID: 12357
		[SerializeField]
		private UIImageHelper.State enabledState;

		// Token: 0x04003046 RID: 12358
		[SerializeField]
		private UIImageHelper.State disabledState;

		// Token: 0x04003047 RID: 12359
		private bool currentState;

		// Token: 0x020006A5 RID: 1701
		[Serializable]
		private class State
		{
			// Token: 0x06003126 RID: 12582 RVA: 0x00021522 File Offset: 0x0001F722
			public void Set(Image image)
			{
				if (image == null)
				{
					return;
				}
				image.color = this.color;
			}

			// Token: 0x04003048 RID: 12360
			[SerializeField]
			public Color color;
		}
	}
}
