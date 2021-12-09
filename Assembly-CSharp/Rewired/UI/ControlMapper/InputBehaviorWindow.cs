using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000684 RID: 1668
	[AddComponentMenu("")]
	public class InputBehaviorWindow : Window
	{
		// Token: 0x06002FEB RID: 12267 RVA: 0x00100F84 File Offset: 0x000FF184
		public override void Initialize(int id, Func<int, bool> isFocusedCallback)
		{
			if (this.spawnTransform == null || this.doneButton == null || this.cancelButton == null || this.defaultButton == null || this.uiControlSetPrefab == null || this.uiSliderControlPrefab == null || this.doneButtonLabel == null || this.cancelButtonLabel == null || this.defaultButtonLabel == null)
			{
				Debug.LogError("Rewired Control Mapper: All inspector values must be assigned!");
				return;
			}
			this.inputBehaviorInfo = new List<InputBehaviorWindow.InputBehaviorInfo>();
			this.buttonCallbacks = new Dictionary<int, Action<int>>();
			this.doneButtonLabel.text = ControlMapper.GetLanguage().done;
			this.cancelButtonLabel.text = ControlMapper.GetLanguage().cancel;
			this.defaultButtonLabel.text = ControlMapper.GetLanguage().default_;
			base.Initialize(id, isFocusedCallback);
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x00101078 File Offset: 0x000FF278
		public void SetData(int playerId, ControlMapper.InputBehaviorSettings[] data)
		{
			if (!base.initialized)
			{
				return;
			}
			this.playerId = playerId;
			foreach (ControlMapper.InputBehaviorSettings inputBehaviorSettings in data)
			{
				if (inputBehaviorSettings != null && inputBehaviorSettings.isValid)
				{
					InputBehavior inputBehavior = this.GetInputBehavior(inputBehaviorSettings.inputBehaviorId);
					if (inputBehavior != null)
					{
						UIControlSet uicontrolSet = this.CreateControlSet();
						Dictionary<int, InputBehaviorWindow.PropertyType> dictionary = new Dictionary<int, InputBehaviorWindow.PropertyType>();
						string customEntry = ControlMapper.GetLanguage().GetCustomEntry(inputBehaviorSettings.labelLanguageKey);
						if (!string.IsNullOrEmpty(customEntry))
						{
							uicontrolSet.SetTitle(customEntry);
						}
						else
						{
							uicontrolSet.SetTitle(inputBehavior.name);
						}
						if (inputBehaviorSettings.showJoystickAxisSensitivity)
						{
							UISliderControl uisliderControl = this.CreateSlider(uicontrolSet, inputBehavior.id, null, ControlMapper.GetLanguage().GetCustomEntry(inputBehaviorSettings.joystickAxisSensitivityLabelLanguageKey), inputBehaviorSettings.joystickAxisSensitivityIcon, inputBehaviorSettings.joystickAxisSensitivityMin, inputBehaviorSettings.joystickAxisSensitivityMax, new Action<int, int, float>(this.JoystickAxisSensitivityValueChanged), new Action<int, int>(this.JoystickAxisSensitivityCanceled));
							uisliderControl.slider.value = Mathf.Clamp(inputBehavior.joystickAxisSensitivity, inputBehaviorSettings.joystickAxisSensitivityMin, inputBehaviorSettings.joystickAxisSensitivityMax);
							dictionary.Add(uisliderControl.id, InputBehaviorWindow.PropertyType.JoystickAxisSensitivity);
						}
						if (inputBehaviorSettings.showMouseXYAxisSensitivity)
						{
							UISliderControl uisliderControl2 = this.CreateSlider(uicontrolSet, inputBehavior.id, null, ControlMapper.GetLanguage().GetCustomEntry(inputBehaviorSettings.mouseXYAxisSensitivityLabelLanguageKey), inputBehaviorSettings.mouseXYAxisSensitivityIcon, inputBehaviorSettings.mouseXYAxisSensitivityMin, inputBehaviorSettings.mouseXYAxisSensitivityMax, new Action<int, int, float>(this.MouseXYAxisSensitivityValueChanged), new Action<int, int>(this.MouseXYAxisSensitivityCanceled));
							uisliderControl2.slider.value = Mathf.Clamp(inputBehavior.mouseXYAxisSensitivity, inputBehaviorSettings.mouseXYAxisSensitivityMin, inputBehaviorSettings.mouseXYAxisSensitivityMax);
							dictionary.Add(uisliderControl2.id, InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity);
						}
						this.inputBehaviorInfo.Add(new InputBehaviorWindow.InputBehaviorInfo(inputBehavior, uicontrolSet, dictionary));
					}
				}
			}
			base.defaultUIElement = this.doneButton.gameObject;
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x00020ABB File Offset: 0x0001ECBB
		public void SetButtonCallback(InputBehaviorWindow.ButtonIdentifier buttonIdentifier, Action<int> callback)
		{
			if (!base.initialized)
			{
				return;
			}
			if (callback == null)
			{
				return;
			}
			if (this.buttonCallbacks.ContainsKey((int)buttonIdentifier))
			{
				this.buttonCallbacks[(int)buttonIdentifier] = callback;
				return;
			}
			this.buttonCallbacks.Add((int)buttonIdentifier, callback);
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x00101240 File Offset: 0x000FF440
		public override void Cancel()
		{
			if (!base.initialized)
			{
				return;
			}
			foreach (InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo in this.inputBehaviorInfo)
			{
				inputBehaviorInfo.RestorePreviousData();
			}
			Action<int> action;
			if (!this.buttonCallbacks.TryGetValue(1, out action))
			{
				if (this.cancelCallback != null)
				{
					this.cancelCallback();
				}
				return;
			}
			action(base.id);
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x001012CC File Offset: 0x000FF4CC
		public void OnDone()
		{
			if (!base.initialized)
			{
				return;
			}
			Action<int> action;
			if (!this.buttonCallbacks.TryGetValue(0, out action))
			{
				return;
			}
			action(base.id);
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x0001E3D7 File Offset: 0x0001C5D7
		public void OnCancel()
		{
			this.Cancel();
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x00101300 File Offset: 0x000FF500
		public void OnRestoreDefault()
		{
			if (!base.initialized)
			{
				return;
			}
			foreach (InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo in this.inputBehaviorInfo)
			{
				inputBehaviorInfo.RestoreDefaultData();
			}
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x00020AF3 File Offset: 0x0001ECF3
		private void JoystickAxisSensitivityValueChanged(int inputBehaviorId, int controlId, float value)
		{
			this.GetInputBehavior(inputBehaviorId).joystickAxisSensitivity = value;
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x00020B02 File Offset: 0x0001ED02
		private void MouseXYAxisSensitivityValueChanged(int inputBehaviorId, int controlId, float value)
		{
			this.GetInputBehavior(inputBehaviorId).mouseXYAxisSensitivity = value;
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x0010135C File Offset: 0x000FF55C
		private void JoystickAxisSensitivityCanceled(int inputBehaviorId, int controlId)
		{
			InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo = this.GetInputBehaviorInfo(inputBehaviorId);
			if (inputBehaviorInfo == null)
			{
				return;
			}
			inputBehaviorInfo.RestoreData(InputBehaviorWindow.PropertyType.JoystickAxisSensitivity, controlId);
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x00101380 File Offset: 0x000FF580
		private void MouseXYAxisSensitivityCanceled(int inputBehaviorId, int controlId)
		{
			InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo = this.GetInputBehaviorInfo(inputBehaviorId);
			if (inputBehaviorInfo == null)
			{
				return;
			}
			inputBehaviorInfo.RestoreData(InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity, controlId);
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x00020B11 File Offset: 0x0001ED11
		public override void TakeInputFocus()
		{
			base.TakeInputFocus();
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x00020B19 File Offset: 0x0001ED19
		private UIControlSet CreateControlSet()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.uiControlSetPrefab);
			gameObject.transform.SetParent(this.spawnTransform, false);
			return gameObject.GetComponent<UIControlSet>();
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x001013A4 File Offset: 0x000FF5A4
		private UISliderControl CreateSlider(UIControlSet set, int inputBehaviorId, string defaultTitle, string overrideTitle, Sprite icon, float minValue, float maxValue, Action<int, int, float> valueChangedCallback, Action<int, int> cancelCallback)
		{
			UISliderControl uisliderControl = set.CreateSlider(this.uiSliderControlPrefab, icon, minValue, maxValue, delegate(int cId, float value)
			{
				valueChangedCallback(inputBehaviorId, cId, value);
			}, delegate(int cId)
			{
				cancelCallback(inputBehaviorId, cId);
			});
			string text = string.IsNullOrEmpty(overrideTitle) ? defaultTitle : overrideTitle;
			if (!string.IsNullOrEmpty(text))
			{
				uisliderControl.showTitle = true;
				uisliderControl.title.text = text;
			}
			else
			{
				uisliderControl.showTitle = false;
			}
			uisliderControl.showIcon = (icon != null);
			return uisliderControl;
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x00020B3D File Offset: 0x0001ED3D
		private InputBehavior GetInputBehavior(int id)
		{
			return ReInput.mapping.GetInputBehavior(this.playerId, id);
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x0010143C File Offset: 0x000FF63C
		private InputBehaviorWindow.InputBehaviorInfo GetInputBehaviorInfo(int inputBehaviorId)
		{
			int count = this.inputBehaviorInfo.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.inputBehaviorInfo[i].inputBehavior.id == inputBehaviorId)
				{
					return this.inputBehaviorInfo[i];
				}
			}
			return null;
		}

		// Token: 0x04002F82 RID: 12162
		private const float minSensitivity = 0.1f;

		// Token: 0x04002F83 RID: 12163
		[SerializeField]
		private RectTransform spawnTransform;

		// Token: 0x04002F84 RID: 12164
		[SerializeField]
		private Button doneButton;

		// Token: 0x04002F85 RID: 12165
		[SerializeField]
		private Button cancelButton;

		// Token: 0x04002F86 RID: 12166
		[SerializeField]
		private Button defaultButton;

		// Token: 0x04002F87 RID: 12167
		[SerializeField]
		private Text doneButtonLabel;

		// Token: 0x04002F88 RID: 12168
		[SerializeField]
		private Text cancelButtonLabel;

		// Token: 0x04002F89 RID: 12169
		[SerializeField]
		private Text defaultButtonLabel;

		// Token: 0x04002F8A RID: 12170
		[SerializeField]
		private GameObject uiControlSetPrefab;

		// Token: 0x04002F8B RID: 12171
		[SerializeField]
		private GameObject uiSliderControlPrefab;

		// Token: 0x04002F8C RID: 12172
		private List<InputBehaviorWindow.InputBehaviorInfo> inputBehaviorInfo;

		// Token: 0x04002F8D RID: 12173
		private Dictionary<int, Action<int>> buttonCallbacks;

		// Token: 0x04002F8E RID: 12174
		private int playerId;

		// Token: 0x02000685 RID: 1669
		private class InputBehaviorInfo
		{
			// Token: 0x17000810 RID: 2064
			// (get) Token: 0x06002FFC RID: 12284 RVA: 0x00020B58 File Offset: 0x0001ED58
			public InputBehavior inputBehavior
			{
				get
				{
					return this._inputBehavior;
				}
			}

			// Token: 0x17000811 RID: 2065
			// (get) Token: 0x06002FFD RID: 12285 RVA: 0x00020B60 File Offset: 0x0001ED60
			public UIControlSet controlSet
			{
				get
				{
					return this._controlSet;
				}
			}

			// Token: 0x06002FFE RID: 12286 RVA: 0x00020B68 File Offset: 0x0001ED68
			public InputBehaviorInfo(InputBehavior inputBehavior, UIControlSet controlSet, Dictionary<int, InputBehaviorWindow.PropertyType> idToProperty)
			{
				this._inputBehavior = inputBehavior;
				this._controlSet = controlSet;
				this.idToProperty = idToProperty;
				this.copyOfOriginal = new InputBehavior(inputBehavior);
			}

			// Token: 0x06002FFF RID: 12287 RVA: 0x00020B91 File Offset: 0x0001ED91
			public void RestorePreviousData()
			{
				this._inputBehavior.ImportData(this.copyOfOriginal);
			}

			// Token: 0x06003000 RID: 12288 RVA: 0x00020BA5 File Offset: 0x0001EDA5
			public void RestoreDefaultData()
			{
				this._inputBehavior.Reset();
				this.RefreshControls();
			}

			// Token: 0x06003001 RID: 12289 RVA: 0x00101488 File Offset: 0x000FF688
			public void RestoreData(InputBehaviorWindow.PropertyType propertyType, int controlId)
			{
				if (propertyType != InputBehaviorWindow.PropertyType.JoystickAxisSensitivity)
				{
					if (propertyType != InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity)
					{
						return;
					}
					float mouseXYAxisSensitivity = this.copyOfOriginal.mouseXYAxisSensitivity;
					this._inputBehavior.mouseXYAxisSensitivity = mouseXYAxisSensitivity;
					UISliderControl control = this._controlSet.GetControl<UISliderControl>(controlId);
					if (control != null)
					{
						control.slider.value = mouseXYAxisSensitivity;
					}
				}
				else
				{
					float joystickAxisSensitivity = this.copyOfOriginal.joystickAxisSensitivity;
					this._inputBehavior.joystickAxisSensitivity = joystickAxisSensitivity;
					UISliderControl control2 = this._controlSet.GetControl<UISliderControl>(controlId);
					if (control2 != null)
					{
						control2.slider.value = joystickAxisSensitivity;
						return;
					}
				}
			}

			// Token: 0x06003002 RID: 12290 RVA: 0x00101514 File Offset: 0x000FF714
			public void RefreshControls()
			{
				if (this._controlSet == null)
				{
					return;
				}
				if (this.idToProperty == null)
				{
					return;
				}
				foreach (KeyValuePair<int, InputBehaviorWindow.PropertyType> keyValuePair in this.idToProperty)
				{
					UISliderControl control = this._controlSet.GetControl<UISliderControl>(keyValuePair.Key);
					if (!(control == null))
					{
						InputBehaviorWindow.PropertyType value = keyValuePair.Value;
						if (value != InputBehaviorWindow.PropertyType.JoystickAxisSensitivity)
						{
							if (value == InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity)
							{
								control.slider.value = this._inputBehavior.mouseXYAxisSensitivity;
							}
						}
						else
						{
							control.slider.value = this._inputBehavior.joystickAxisSensitivity;
						}
					}
				}
			}

			// Token: 0x04002F8F RID: 12175
			private InputBehavior _inputBehavior;

			// Token: 0x04002F90 RID: 12176
			private UIControlSet _controlSet;

			// Token: 0x04002F91 RID: 12177
			private Dictionary<int, InputBehaviorWindow.PropertyType> idToProperty;

			// Token: 0x04002F92 RID: 12178
			private InputBehavior copyOfOriginal;
		}

		// Token: 0x02000686 RID: 1670
		public enum ButtonIdentifier
		{
			// Token: 0x04002F94 RID: 12180
			Done,
			// Token: 0x04002F95 RID: 12181
			Cancel,
			// Token: 0x04002F96 RID: 12182
			Default
		}

		// Token: 0x02000687 RID: 1671
		private enum PropertyType
		{
			// Token: 0x04002F98 RID: 12184
			JoystickAxisSensitivity,
			// Token: 0x04002F99 RID: 12185
			MouseXYAxisSensitivity
		}
	}
}
