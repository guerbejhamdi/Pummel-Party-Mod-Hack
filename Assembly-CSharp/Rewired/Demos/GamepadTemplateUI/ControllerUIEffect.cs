using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos.GamepadTemplateUI
{
	// Token: 0x020006E3 RID: 1763
	[RequireComponent(typeof(Image))]
	public class ControllerUIEffect : MonoBehaviour
	{
		// Token: 0x06003363 RID: 13155 RVA: 0x00022FBC File Offset: 0x000211BC
		private void Awake()
		{
			this._image = base.GetComponent<Image>();
			this._origColor = this._image.color;
			this._color = this._origColor;
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x0010A930 File Offset: 0x00108B30
		public void Activate(float amount)
		{
			amount = Mathf.Clamp01(amount);
			if (this._isActive && amount == this._highlightAmount)
			{
				return;
			}
			this._highlightAmount = amount;
			this._color = Color.Lerp(this._origColor, this._highlightColor, this._highlightAmount);
			this._isActive = true;
			this.RedrawImage();
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x00022FE7 File Offset: 0x000211E7
		public void Deactivate()
		{
			if (!this._isActive)
			{
				return;
			}
			this._color = this._origColor;
			this._highlightAmount = 0f;
			this._isActive = false;
			this.RedrawImage();
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x00023016 File Offset: 0x00021216
		private void RedrawImage()
		{
			this._image.color = this._color;
			this._image.enabled = this._isActive;
		}

		// Token: 0x040031A0 RID: 12704
		[SerializeField]
		private Color _highlightColor = Color.white;

		// Token: 0x040031A1 RID: 12705
		private Image _image;

		// Token: 0x040031A2 RID: 12706
		private Color _color;

		// Token: 0x040031A3 RID: 12707
		private Color _origColor;

		// Token: 0x040031A4 RID: 12708
		private bool _isActive;

		// Token: 0x040031A5 RID: 12709
		private float _highlightAmount;
	}
}
