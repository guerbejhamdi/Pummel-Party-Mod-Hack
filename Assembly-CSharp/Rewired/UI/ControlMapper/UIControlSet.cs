using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020006A0 RID: 1696
	[AddComponentMenu("")]
	public class UIControlSet : MonoBehaviour
	{
		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06003110 RID: 12560 RVA: 0x00102618 File Offset: 0x00100818
		private Dictionary<int, UIControl> controls
		{
			get
			{
				Dictionary<int, UIControl> result;
				if ((result = this._controls) == null)
				{
					result = (this._controls = new Dictionary<int, UIControl>());
				}
				return result;
			}
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x00021435 File Offset: 0x0001F635
		public void SetTitle(string text)
		{
			if (this.title == null)
			{
				return;
			}
			this.title.text = text;
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x00102640 File Offset: 0x00100840
		public T GetControl<T>(int uniqueId) where T : UIControl
		{
			UIControl uicontrol;
			this.controls.TryGetValue(uniqueId, out uicontrol);
			return uicontrol as T;
		}

		// Token: 0x06003113 RID: 12563 RVA: 0x00102668 File Offset: 0x00100868
		public UISliderControl CreateSlider(GameObject prefab, Sprite icon, float minValue, float maxValue, Action<int, float> valueChangedCallback, Action<int> cancelCallback)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
			UISliderControl control = gameObject.GetComponent<UISliderControl>();
			if (control == null)
			{
				UnityEngine.Object.Destroy(gameObject);
				Debug.LogError("Prefab missing UISliderControl component!");
				return null;
			}
			gameObject.transform.SetParent(base.transform, false);
			if (control.iconImage != null)
			{
				control.iconImage.sprite = icon;
			}
			if (control.slider != null)
			{
				control.slider.minValue = minValue;
				control.slider.maxValue = maxValue;
				if (valueChangedCallback != null)
				{
					control.slider.onValueChanged.AddListener(delegate(float value)
					{
						valueChangedCallback(control.id, value);
					});
				}
				if (cancelCallback != null)
				{
					control.SetCancelCallback(delegate
					{
						cancelCallback(control.id);
					});
				}
			}
			this.controls.Add(control.id, control);
			return control;
		}

		// Token: 0x0400303A RID: 12346
		[SerializeField]
		private Text title;

		// Token: 0x0400303B RID: 12347
		private Dictionary<int, UIControl> _controls;
	}
}
