using System;
using System.Collections.Generic;
using Rewired.Integration.UnityUI;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000655 RID: 1621
	[AddComponentMenu("")]
	public class CalibrationWindow : Window
	{
		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06002CD5 RID: 11477 RVA: 0x0001E336 File Offset: 0x0001C536
		private bool axisSelected
		{
			get
			{
				return this.joystick != null && this.selectedAxis >= 0 && this.selectedAxis < this.joystick.calibrationMap.axisCount;
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06002CD6 RID: 11478 RVA: 0x0001E366 File Offset: 0x0001C566
		private AxisCalibration axisCalibration
		{
			get
			{
				if (!this.axisSelected)
				{
					return null;
				}
				return this.joystick.calibrationMap.GetAxis(this.selectedAxis);
			}
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000F8860 File Offset: 0x000F6A60
		public override void Initialize(int id, Func<int, bool> isFocusedCallback)
		{
			if (this.rightContentContainer == null || this.valueDisplayGroup == null || this.calibratedValueMarker == null || this.rawValueMarker == null || this.calibratedZeroMarker == null || this.deadzoneArea == null || this.deadzoneSlider == null || this.sensitivitySlider == null || this.zeroSlider == null || this.invertToggle == null || this.axisScrollAreaContent == null || this.doneButton == null || this.calibrateButton == null || this.axisButtonPrefab == null || this.doneButtonLabel == null || this.cancelButtonLabel == null || this.defaultButtonLabel == null || this.deadzoneSliderLabel == null || this.zeroSliderLabel == null || this.sensitivitySliderLabel == null || this.invertToggleLabel == null || this.calibrateButtonLabel == null)
			{
				Debug.LogError("Rewired Control Mapper: All inspector values must be assigned!");
				return;
			}
			this.axisButtons = new List<Button>();
			this.buttonCallbacks = new Dictionary<int, Action<int>>();
			this.doneButtonLabel.text = ControlMapper.GetLanguage().done;
			this.cancelButtonLabel.text = ControlMapper.GetLanguage().cancel;
			this.defaultButtonLabel.text = ControlMapper.GetLanguage().default_;
			this.deadzoneSliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_deadZoneSliderLabel;
			this.zeroSliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_zeroSliderLabel;
			this.sensitivitySliderLabel.text = ControlMapper.GetLanguage().calibrateWindow_sensitivitySliderLabel;
			this.invertToggleLabel.text = ControlMapper.GetLanguage().calibrateWindow_invertToggleLabel;
			this.calibrateButtonLabel.text = ControlMapper.GetLanguage().calibrateWindow_calibrateButtonLabel;
			base.Initialize(id, isFocusedCallback);
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000F8A98 File Offset: 0x000F6C98
		public void SetJoystick(int playerId, Joystick joystick)
		{
			if (!base.initialized)
			{
				return;
			}
			this.playerId = playerId;
			this.joystick = joystick;
			if (joystick == null)
			{
				Debug.LogError("Rewired Control Mapper: Joystick cannot be null!");
				return;
			}
			float num = 0f;
			for (int i = 0; i < joystick.axisCount; i++)
			{
				int index = i;
				GameObject gameObject = UITools.InstantiateGUIObject<Button>(this.axisButtonPrefab, this.axisScrollAreaContent, "Axis" + i.ToString());
				Button button = gameObject.GetComponent<Button>();
				button.onClick.AddListener(delegate()
				{
					this.OnAxisSelected(index, button);
				});
				Text componentInSelfOrChildren = UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
				if (componentInSelfOrChildren != null)
				{
					componentInSelfOrChildren.text = ControlMapper.GetLanguage().GetElementIdentifierName(joystick, joystick.AxisElementIdentifiers[i].id, AxisRange.Full);
				}
				if (num == 0f)
				{
					num = UnityTools.GetComponentInSelfOrChildren<LayoutElement>(gameObject).minHeight;
				}
				this.axisButtons.Add(button);
			}
			float spacing = this.axisScrollAreaContent.GetComponent<VerticalLayoutGroup>().spacing;
			this.axisScrollAreaContent.sizeDelta = new Vector2(this.axisScrollAreaContent.sizeDelta.x, Mathf.Max((float)joystick.axisCount * (num + spacing) - spacing, this.axisScrollAreaContent.sizeDelta.y));
			this.origCalibrationData = joystick.calibrationMap.ToXmlString();
			this.displayAreaWidth = this.rightContentContainer.sizeDelta.x;
			this.rewiredStandaloneInputModule = base.gameObject.transform.root.GetComponentInChildren<RewiredStandaloneInputModule>();
			if (this.rewiredStandaloneInputModule != null)
			{
				this.menuHorizActionId = ReInput.mapping.GetActionId(this.rewiredStandaloneInputModule.horizontalAxis);
				this.menuVertActionId = ReInput.mapping.GetActionId(this.rewiredStandaloneInputModule.verticalAxis);
			}
			if (joystick.axisCount > 0)
			{
				this.SelectAxis(0);
			}
			base.defaultUIElement = this.doneButton.gameObject;
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x0001E388 File Offset: 0x0001C588
		public void SetButtonCallback(CalibrationWindow.ButtonIdentifier buttonIdentifier, Action<int> callback)
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

		// Token: 0x06002CDA RID: 11482 RVA: 0x000F8CAC File Offset: 0x000F6EAC
		public override void Cancel()
		{
			if (!base.initialized)
			{
				return;
			}
			if (this.joystick != null)
			{
				this.joystick.ImportCalibrationMapFromXmlString(this.origCalibrationData);
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

		// Token: 0x06002CDB RID: 11483 RVA: 0x0001E3C0 File Offset: 0x0001C5C0
		protected override void Update()
		{
			if (!base.initialized)
			{
				return;
			}
			base.Update();
			this.UpdateDisplay();
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x000F8D0C File Offset: 0x000F6F0C
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

		// Token: 0x06002CDD RID: 11485 RVA: 0x0001E3D7 File Offset: 0x0001C5D7
		public void OnCancel()
		{
			this.Cancel();
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x0001E3DF File Offset: 0x0001C5DF
		public void OnRestoreDefault()
		{
			if (!base.initialized)
			{
				return;
			}
			if (this.joystick == null)
			{
				return;
			}
			this.joystick.calibrationMap.Reset();
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000F8D40 File Offset: 0x000F6F40
		public void OnCalibrate()
		{
			if (!base.initialized)
			{
				return;
			}
			Action<int> action;
			if (!this.buttonCallbacks.TryGetValue(3, out action))
			{
				return;
			}
			action(this.selectedAxis);
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x0001E40F File Offset: 0x0001C60F
		public void OnInvert(bool state)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.invert = state;
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x0001E42F File Offset: 0x0001C62F
		public void OnZeroValueChange(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.calibratedZero = value;
			this.RedrawCalibratedZero();
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x0001E455 File Offset: 0x0001C655
		public void OnZeroCancel()
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.calibratedZero = this.origSelectedAxisCalibrationData.zero;
			this.RedrawCalibratedZero();
			this.RefreshControls();
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x000F8D74 File Offset: 0x000F6F74
		public void OnDeadzoneValueChange(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.deadZone = Mathf.Clamp(value, 0f, 0.8f);
			if (value > 0.8f)
			{
				this.deadzoneSlider.value = 0.8f;
			}
			this.RedrawDeadzone();
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x0001E48B File Offset: 0x0001C68B
		public void OnDeadzoneCancel()
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.deadZone = this.origSelectedAxisCalibrationData.deadZone;
			this.RedrawDeadzone();
			this.RefreshControls();
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x0001E4C1 File Offset: 0x0001C6C1
		public void OnSensitivityValueChange(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.SetSensitivity(this.axisCalibration, value);
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x0001E4E2 File Offset: 0x0001C6E2
		public void OnSensitivityCancel(float value)
		{
			if (!base.initialized)
			{
				return;
			}
			if (!this.axisSelected)
			{
				return;
			}
			this.axisCalibration.sensitivity = this.origSelectedAxisCalibrationData.sensitivity;
			this.RefreshControls();
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x0001E512 File Offset: 0x0001C712
		public void OnAxisScrollRectScroll(Vector2 pos)
		{
			bool initialized = base.initialized;
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x0001E51B File Offset: 0x0001C71B
		private void OnAxisSelected(int axisIndex, Button button)
		{
			if (!base.initialized)
			{
				return;
			}
			if (this.joystick == null)
			{
				return;
			}
			this.SelectAxis(axisIndex);
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x0001E542 File Offset: 0x0001C742
		private void UpdateDisplay()
		{
			this.RedrawValueMarkers();
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x0001E54A File Offset: 0x0001C74A
		private void Redraw()
		{
			this.RedrawCalibratedZero();
			this.RedrawValueMarkers();
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x000F8DCC File Offset: 0x000F6FCC
		private void RefreshControls()
		{
			if (!this.axisSelected)
			{
				this.deadzoneSlider.value = 0f;
				this.zeroSlider.value = 0f;
				this.sensitivitySlider.value = 0f;
				this.invertToggle.isOn = false;
				return;
			}
			this.deadzoneSlider.value = this.axisCalibration.deadZone;
			this.zeroSlider.value = this.axisCalibration.calibratedZero;
			this.sensitivitySlider.value = this.GetSliderSensitivity(this.axisCalibration);
			this.invertToggle.isOn = this.axisCalibration.invert;
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x000F8E78 File Offset: 0x000F7078
		private void RedrawDeadzone()
		{
			if (!this.axisSelected)
			{
				return;
			}
			float x = this.displayAreaWidth * this.axisCalibration.deadZone;
			this.deadzoneArea.sizeDelta = new Vector2(x, this.deadzoneArea.sizeDelta.y);
			this.deadzoneArea.anchoredPosition = new Vector2(this.axisCalibration.calibratedZero * -this.deadzoneArea.parent.localPosition.x, this.deadzoneArea.anchoredPosition.y);
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x000F8F04 File Offset: 0x000F7104
		private void RedrawCalibratedZero()
		{
			if (!this.axisSelected)
			{
				return;
			}
			this.calibratedZeroMarker.anchoredPosition = new Vector2(this.axisCalibration.calibratedZero * -this.deadzoneArea.parent.localPosition.x, this.calibratedZeroMarker.anchoredPosition.y);
			this.RedrawDeadzone();
		}

		// Token: 0x06002CEE RID: 11502 RVA: 0x000F8F64 File Offset: 0x000F7164
		private void RedrawValueMarkers()
		{
			if (!this.axisSelected)
			{
				this.calibratedValueMarker.anchoredPosition = new Vector2(0f, this.calibratedValueMarker.anchoredPosition.y);
				this.rawValueMarker.anchoredPosition = new Vector2(0f, this.rawValueMarker.anchoredPosition.y);
				return;
			}
			float axis = this.joystick.GetAxis(this.selectedAxis);
			float num = Mathf.Clamp(this.joystick.GetAxisRaw(this.selectedAxis), -1f, 1f);
			this.calibratedValueMarker.anchoredPosition = new Vector2(this.displayAreaWidth * 0.5f * axis, this.calibratedValueMarker.anchoredPosition.y);
			this.rawValueMarker.anchoredPosition = new Vector2(this.displayAreaWidth * 0.5f * num, this.rawValueMarker.anchoredPosition.y);
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x000F9054 File Offset: 0x000F7254
		private void SelectAxis(int index)
		{
			if (index < 0 || index >= this.axisButtons.Count)
			{
				return;
			}
			if (this.axisButtons[index] == null)
			{
				return;
			}
			this.axisButtons[index].interactable = false;
			this.axisButtons[index].Select();
			for (int i = 0; i < this.axisButtons.Count; i++)
			{
				if (i != index)
				{
					this.axisButtons[i].interactable = true;
				}
			}
			this.selectedAxis = index;
			this.origSelectedAxisCalibrationData = this.axisCalibration.GetData();
			this.SetMinSensitivity();
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x0001E558 File Offset: 0x0001C758
		public override void TakeInputFocus()
		{
			base.TakeInputFocus();
			if (this.selectedAxis >= 0)
			{
				this.SelectAxis(this.selectedAxis);
			}
			this.RefreshControls();
			this.Redraw();
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x000F90F8 File Offset: 0x000F72F8
		private void SetMinSensitivity()
		{
			if (!this.axisSelected)
			{
				return;
			}
			this.minSensitivity = 0.1f;
			if (this.rewiredStandaloneInputModule != null)
			{
				if (this.IsMenuAxis(this.menuHorizActionId, this.selectedAxis))
				{
					this.GetAxisButtonDeadZone(this.playerId, this.menuHorizActionId, ref this.minSensitivity);
					return;
				}
				if (this.IsMenuAxis(this.menuVertActionId, this.selectedAxis))
				{
					this.GetAxisButtonDeadZone(this.playerId, this.menuVertActionId, ref this.minSensitivity);
				}
			}
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x000F9180 File Offset: 0x000F7380
		private bool IsMenuAxis(int actionId, int axisIndex)
		{
			if (this.rewiredStandaloneInputModule == null)
			{
				return false;
			}
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			int count = allPlayers.Count;
			for (int i = 0; i < count; i++)
			{
				IList<JoystickMap> maps = allPlayers[i].controllers.maps.GetMaps<JoystickMap>(this.joystick.id);
				if (maps != null)
				{
					int count2 = maps.Count;
					for (int j = 0; j < count2; j++)
					{
						IList<ActionElementMap> axisMaps = maps[j].AxisMaps;
						if (axisMaps != null)
						{
							int count3 = axisMaps.Count;
							for (int k = 0; k < count3; k++)
							{
								ActionElementMap actionElementMap = axisMaps[k];
								if (actionElementMap.actionId == actionId && actionElementMap.elementIndex == axisIndex)
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x000F9250 File Offset: 0x000F7450
		private void GetAxisButtonDeadZone(int playerId, int actionId, ref float value)
		{
			InputAction action = ReInput.mapping.GetAction(actionId);
			if (action == null)
			{
				return;
			}
			int behaviorId = action.behaviorId;
			InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
			if (inputBehavior == null)
			{
				return;
			}
			value = inputBehavior.buttonDeadZone + 0.1f;
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x0001E581 File Offset: 0x0001C781
		private float GetSliderSensitivity(AxisCalibration axisCalibration)
		{
			if (axisCalibration.sensitivityType == AxisSensitivityType.Multiplier)
			{
				return axisCalibration.sensitivity;
			}
			if (axisCalibration.sensitivityType == AxisSensitivityType.Power)
			{
				return CalibrationWindow.ProcessPowerValue(axisCalibration.sensitivity, 0f, this.sensitivitySlider.maxValue);
			}
			return axisCalibration.sensitivity;
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x000F9294 File Offset: 0x000F7494
		public void SetSensitivity(AxisCalibration axisCalibration, float sliderValue)
		{
			if (axisCalibration.sensitivityType == AxisSensitivityType.Multiplier)
			{
				axisCalibration.sensitivity = Mathf.Clamp(sliderValue, this.minSensitivity, float.PositiveInfinity);
				if (sliderValue < this.minSensitivity)
				{
					this.sensitivitySlider.value = this.minSensitivity;
					return;
				}
			}
			else
			{
				if (axisCalibration.sensitivityType == AxisSensitivityType.Power)
				{
					axisCalibration.sensitivity = CalibrationWindow.ProcessPowerValue(sliderValue, 0f, this.sensitivitySlider.maxValue);
					return;
				}
				axisCalibration.sensitivity = sliderValue;
			}
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x000F9308 File Offset: 0x000F7508
		private static float ProcessPowerValue(float value, float minValue, float maxValue)
		{
			value = Mathf.Clamp(value, minValue, maxValue);
			if (value > 1f)
			{
				value = MathTools.ValueInNewRange(value, 1f, maxValue, 1f, 0f);
			}
			else if (value < 1f)
			{
				value = MathTools.ValueInNewRange(value, 0f, 1f, maxValue, 1f);
			}
			return value;
		}

		// Token: 0x04002E10 RID: 11792
		private const float minSensitivityOtherAxes = 0.1f;

		// Token: 0x04002E11 RID: 11793
		private const float maxDeadzone = 0.8f;

		// Token: 0x04002E12 RID: 11794
		[SerializeField]
		private RectTransform rightContentContainer;

		// Token: 0x04002E13 RID: 11795
		[SerializeField]
		private RectTransform valueDisplayGroup;

		// Token: 0x04002E14 RID: 11796
		[SerializeField]
		private RectTransform calibratedValueMarker;

		// Token: 0x04002E15 RID: 11797
		[SerializeField]
		private RectTransform rawValueMarker;

		// Token: 0x04002E16 RID: 11798
		[SerializeField]
		private RectTransform calibratedZeroMarker;

		// Token: 0x04002E17 RID: 11799
		[SerializeField]
		private RectTransform deadzoneArea;

		// Token: 0x04002E18 RID: 11800
		[SerializeField]
		private Slider deadzoneSlider;

		// Token: 0x04002E19 RID: 11801
		[SerializeField]
		private Slider zeroSlider;

		// Token: 0x04002E1A RID: 11802
		[SerializeField]
		private Slider sensitivitySlider;

		// Token: 0x04002E1B RID: 11803
		[SerializeField]
		private Toggle invertToggle;

		// Token: 0x04002E1C RID: 11804
		[SerializeField]
		private RectTransform axisScrollAreaContent;

		// Token: 0x04002E1D RID: 11805
		[SerializeField]
		private Button doneButton;

		// Token: 0x04002E1E RID: 11806
		[SerializeField]
		private Button calibrateButton;

		// Token: 0x04002E1F RID: 11807
		[SerializeField]
		private Text doneButtonLabel;

		// Token: 0x04002E20 RID: 11808
		[SerializeField]
		private Text cancelButtonLabel;

		// Token: 0x04002E21 RID: 11809
		[SerializeField]
		private Text defaultButtonLabel;

		// Token: 0x04002E22 RID: 11810
		[SerializeField]
		private Text deadzoneSliderLabel;

		// Token: 0x04002E23 RID: 11811
		[SerializeField]
		private Text zeroSliderLabel;

		// Token: 0x04002E24 RID: 11812
		[SerializeField]
		private Text sensitivitySliderLabel;

		// Token: 0x04002E25 RID: 11813
		[SerializeField]
		private Text invertToggleLabel;

		// Token: 0x04002E26 RID: 11814
		[SerializeField]
		private Text calibrateButtonLabel;

		// Token: 0x04002E27 RID: 11815
		[SerializeField]
		private GameObject axisButtonPrefab;

		// Token: 0x04002E28 RID: 11816
		private Joystick joystick;

		// Token: 0x04002E29 RID: 11817
		private string origCalibrationData;

		// Token: 0x04002E2A RID: 11818
		private int selectedAxis = -1;

		// Token: 0x04002E2B RID: 11819
		private AxisCalibrationData origSelectedAxisCalibrationData;

		// Token: 0x04002E2C RID: 11820
		private float displayAreaWidth;

		// Token: 0x04002E2D RID: 11821
		private List<Button> axisButtons;

		// Token: 0x04002E2E RID: 11822
		private Dictionary<int, Action<int>> buttonCallbacks;

		// Token: 0x04002E2F RID: 11823
		private int playerId;

		// Token: 0x04002E30 RID: 11824
		private RewiredStandaloneInputModule rewiredStandaloneInputModule;

		// Token: 0x04002E31 RID: 11825
		private int menuHorizActionId = -1;

		// Token: 0x04002E32 RID: 11826
		private int menuVertActionId = -1;

		// Token: 0x04002E33 RID: 11827
		private float minSensitivity;

		// Token: 0x02000656 RID: 1622
		public enum ButtonIdentifier
		{
			// Token: 0x04002E35 RID: 11829
			Done,
			// Token: 0x04002E36 RID: 11830
			Cancel,
			// Token: 0x04002E37 RID: 11831
			Default,
			// Token: 0x04002E38 RID: 11832
			Calibrate
		}
	}
}
