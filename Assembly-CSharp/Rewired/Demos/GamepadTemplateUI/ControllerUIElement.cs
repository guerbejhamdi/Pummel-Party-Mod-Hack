using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos.GamepadTemplateUI
{
	// Token: 0x020006E4 RID: 1764
	[RequireComponent(typeof(Image))]
	public class ControllerUIElement : MonoBehaviour
	{
		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06003368 RID: 13160 RVA: 0x0002304D File Offset: 0x0002124D
		private bool hasEffects
		{
			get
			{
				return this._positiveUIEffect != null || this._negativeUIEffect != null;
			}
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x0002306B File Offset: 0x0002126B
		private void Awake()
		{
			this._image = base.GetComponent<Image>();
			this._origColor = this._image.color;
			this._color = this._origColor;
			this.ClearLabels();
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x0010A988 File Offset: 0x00108B88
		public void Activate(float amount)
		{
			amount = Mathf.Clamp(amount, -1f, 1f);
			if (this.hasEffects)
			{
				if (amount < 0f && this._negativeUIEffect != null)
				{
					this._negativeUIEffect.Activate(Mathf.Abs(amount));
				}
				if (amount > 0f && this._positiveUIEffect != null)
				{
					this._positiveUIEffect.Activate(Mathf.Abs(amount));
				}
			}
			else
			{
				if (this._isActive && amount == this._highlightAmount)
				{
					return;
				}
				this._highlightAmount = amount;
				this._color = Color.Lerp(this._origColor, this._highlightColor, this._highlightAmount);
			}
			this._isActive = true;
			this.RedrawImage();
			if (this._childElements.Length != 0)
			{
				for (int i = 0; i < this._childElements.Length; i++)
				{
					if (!(this._childElements[i] == null))
					{
						this._childElements[i].Activate(amount);
					}
				}
			}
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x0010AA7C File Offset: 0x00108C7C
		public void Deactivate()
		{
			if (!this._isActive)
			{
				return;
			}
			this._color = this._origColor;
			this._highlightAmount = 0f;
			if (this._positiveUIEffect != null)
			{
				this._positiveUIEffect.Deactivate();
			}
			if (this._negativeUIEffect != null)
			{
				this._negativeUIEffect.Deactivate();
			}
			this._isActive = false;
			this.RedrawImage();
			if (this._childElements.Length != 0)
			{
				for (int i = 0; i < this._childElements.Length; i++)
				{
					if (!(this._childElements[i] == null))
					{
						this._childElements[i].Deactivate();
					}
				}
			}
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x0010AB24 File Offset: 0x00108D24
		public void SetLabel(string text, AxisRange labelType)
		{
			Text text2;
			switch (labelType)
			{
			case AxisRange.Full:
				text2 = this._label;
				break;
			case AxisRange.Positive:
				text2 = this._positiveLabel;
				break;
			case AxisRange.Negative:
				text2 = this._negativeLabel;
				break;
			default:
				text2 = null;
				break;
			}
			if (text2 != null)
			{
				text2.text = text;
			}
			if (this._childElements.Length != 0)
			{
				for (int i = 0; i < this._childElements.Length; i++)
				{
					if (!(this._childElements[i] == null))
					{
						this._childElements[i].SetLabel(text, labelType);
					}
				}
			}
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x0010ABB0 File Offset: 0x00108DB0
		public void ClearLabels()
		{
			if (this._label != null)
			{
				this._label.text = string.Empty;
			}
			if (this._positiveLabel != null)
			{
				this._positiveLabel.text = string.Empty;
			}
			if (this._negativeLabel != null)
			{
				this._negativeLabel.text = string.Empty;
			}
			if (this._childElements.Length != 0)
			{
				for (int i = 0; i < this._childElements.Length; i++)
				{
					if (!(this._childElements[i] == null))
					{
						this._childElements[i].ClearLabels();
					}
				}
			}
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x0002309C File Offset: 0x0002129C
		private void RedrawImage()
		{
			this._image.color = this._color;
		}

		// Token: 0x040031A6 RID: 12710
		[SerializeField]
		private Color _highlightColor = Color.white;

		// Token: 0x040031A7 RID: 12711
		[SerializeField]
		private ControllerUIEffect _positiveUIEffect;

		// Token: 0x040031A8 RID: 12712
		[SerializeField]
		private ControllerUIEffect _negativeUIEffect;

		// Token: 0x040031A9 RID: 12713
		[SerializeField]
		private Text _label;

		// Token: 0x040031AA RID: 12714
		[SerializeField]
		private Text _positiveLabel;

		// Token: 0x040031AB RID: 12715
		[SerializeField]
		private Text _negativeLabel;

		// Token: 0x040031AC RID: 12716
		[SerializeField]
		private ControllerUIElement[] _childElements = new ControllerUIElement[0];

		// Token: 0x040031AD RID: 12717
		private Image _image;

		// Token: 0x040031AE RID: 12718
		private Color _color;

		// Token: 0x040031AF RID: 12719
		private Color _origColor;

		// Token: 0x040031B0 RID: 12720
		private bool _isActive;

		// Token: 0x040031B1 RID: 12721
		private float _highlightAmount;
	}
}
