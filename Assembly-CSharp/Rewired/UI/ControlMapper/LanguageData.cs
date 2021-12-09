using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200068B RID: 1675
	[Serializable]
	public class LanguageData : LanguageDataBase
	{
		// Token: 0x06003016 RID: 12310 RVA: 0x00020C92 File Offset: 0x0001EE92
		public override void Initialize()
		{
			if (this._initialized)
			{
				return;
			}
			this.customDict = LanguageData.CustomEntry.ToDictionary(this._customEntries);
			this._initialized = true;
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x001015D4 File Offset: 0x000FF7D4
		public override string GetCustomEntry(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return string.Empty;
			}
			string result;
			if (!this.customDict.TryGetValue(key, out result))
			{
				return string.Empty;
			}
			return result;
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x00020CB5 File Offset: 0x0001EEB5
		public override bool ContainsCustomEntryKey(string key)
		{
			return !string.IsNullOrEmpty(key) && this.customDict.ContainsKey(key);
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06003019 RID: 12313 RVA: 0x00020CCD File Offset: 0x0001EECD
		public override string yes
		{
			get
			{
				return this._yes;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x0600301A RID: 12314 RVA: 0x00020CD5 File Offset: 0x0001EED5
		public override string no
		{
			get
			{
				return this._no;
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x0600301B RID: 12315 RVA: 0x00020CDD File Offset: 0x0001EEDD
		public override string add
		{
			get
			{
				return this._add;
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x0600301C RID: 12316 RVA: 0x00020CE5 File Offset: 0x0001EEE5
		public override string replace
		{
			get
			{
				return this._replace;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x0600301D RID: 12317 RVA: 0x00020CED File Offset: 0x0001EEED
		public override string remove
		{
			get
			{
				return this._remove;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x0600301E RID: 12318 RVA: 0x00020CF5 File Offset: 0x0001EEF5
		public override string swap
		{
			get
			{
				return this._swap;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x0600301F RID: 12319 RVA: 0x00020CFD File Offset: 0x0001EEFD
		public override string cancel
		{
			get
			{
				return this._cancel;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06003020 RID: 12320 RVA: 0x00020D05 File Offset: 0x0001EF05
		public override string none
		{
			get
			{
				return this._none;
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06003021 RID: 12321 RVA: 0x00020D0D File Offset: 0x0001EF0D
		public override string okay
		{
			get
			{
				return this._okay;
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06003022 RID: 12322 RVA: 0x00020D15 File Offset: 0x0001EF15
		public override string done
		{
			get
			{
				return this._done;
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06003023 RID: 12323 RVA: 0x00020D1D File Offset: 0x0001EF1D
		public override string default_
		{
			get
			{
				return this._default;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06003024 RID: 12324 RVA: 0x00020D25 File Offset: 0x0001EF25
		public override string assignControllerWindowTitle
		{
			get
			{
				return this._assignControllerWindowTitle;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06003025 RID: 12325 RVA: 0x00020D2D File Offset: 0x0001EF2D
		public override string assignControllerWindowMessage
		{
			get
			{
				return this._assignControllerWindowMessage;
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06003026 RID: 12326 RVA: 0x00020D35 File Offset: 0x0001EF35
		public override string controllerAssignmentConflictWindowTitle
		{
			get
			{
				return this._controllerAssignmentConflictWindowTitle;
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06003027 RID: 12327 RVA: 0x00020D3D File Offset: 0x0001EF3D
		public override string elementAssignmentPrePollingWindowMessage
		{
			get
			{
				return this._elementAssignmentPrePollingWindowMessage;
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06003028 RID: 12328 RVA: 0x00020D45 File Offset: 0x0001EF45
		public override string elementAssignmentConflictWindowMessage
		{
			get
			{
				return this._elementAssignmentConflictWindowMessage;
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06003029 RID: 12329 RVA: 0x00020D4D File Offset: 0x0001EF4D
		public override string mouseAssignmentConflictWindowTitle
		{
			get
			{
				return this._mouseAssignmentConflictWindowTitle;
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x0600302A RID: 12330 RVA: 0x00020D55 File Offset: 0x0001EF55
		public override string calibrateControllerWindowTitle
		{
			get
			{
				return this._calibrateControllerWindowTitle;
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x0600302B RID: 12331 RVA: 0x00020D5D File Offset: 0x0001EF5D
		public override string calibrateAxisStep1WindowTitle
		{
			get
			{
				return this._calibrateAxisStep1WindowTitle;
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x0600302C RID: 12332 RVA: 0x00020D65 File Offset: 0x0001EF65
		public override string calibrateAxisStep2WindowTitle
		{
			get
			{
				return this._calibrateAxisStep2WindowTitle;
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x0600302D RID: 12333 RVA: 0x00020D6D File Offset: 0x0001EF6D
		public override string inputBehaviorSettingsWindowTitle
		{
			get
			{
				return this._inputBehaviorSettingsWindowTitle;
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x0600302E RID: 12334 RVA: 0x00020D75 File Offset: 0x0001EF75
		public override string restoreDefaultsWindowTitle
		{
			get
			{
				return this._restoreDefaultsWindowTitle;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x0600302F RID: 12335 RVA: 0x00020D7D File Offset: 0x0001EF7D
		public override string actionColumnLabel
		{
			get
			{
				return this._actionColumnLabel;
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06003030 RID: 12336 RVA: 0x00020D85 File Offset: 0x0001EF85
		public override string keyboardColumnLabel
		{
			get
			{
				return this._keyboardColumnLabel;
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06003031 RID: 12337 RVA: 0x00020D8D File Offset: 0x0001EF8D
		public override string mouseColumnLabel
		{
			get
			{
				return this._mouseColumnLabel;
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06003032 RID: 12338 RVA: 0x00020D95 File Offset: 0x0001EF95
		public override string controllerColumnLabel
		{
			get
			{
				return this._controllerColumnLabel;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06003033 RID: 12339 RVA: 0x00020D9D File Offset: 0x0001EF9D
		public override string removeControllerButtonLabel
		{
			get
			{
				return this._removeControllerButtonLabel;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06003034 RID: 12340 RVA: 0x00020DA5 File Offset: 0x0001EFA5
		public override string calibrateControllerButtonLabel
		{
			get
			{
				return this._calibrateControllerButtonLabel;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06003035 RID: 12341 RVA: 0x00020DAD File Offset: 0x0001EFAD
		public override string assignControllerButtonLabel
		{
			get
			{
				return this._assignControllerButtonLabel;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06003036 RID: 12342 RVA: 0x00020DB5 File Offset: 0x0001EFB5
		public override string inputBehaviorSettingsButtonLabel
		{
			get
			{
				return this._inputBehaviorSettingsButtonLabel;
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06003037 RID: 12343 RVA: 0x00020DBD File Offset: 0x0001EFBD
		public override string doneButtonLabel
		{
			get
			{
				return this._doneButtonLabel;
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06003038 RID: 12344 RVA: 0x00020DC5 File Offset: 0x0001EFC5
		public override string restoreDefaultsButtonLabel
		{
			get
			{
				return this._restoreDefaultsButtonLabel;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06003039 RID: 12345 RVA: 0x00020DCD File Offset: 0x0001EFCD
		public override string controllerSettingsGroupLabel
		{
			get
			{
				return this._controllerSettingsGroupLabel;
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x0600303A RID: 12346 RVA: 0x00020DD5 File Offset: 0x0001EFD5
		public override string playersGroupLabel
		{
			get
			{
				return this._playersGroupLabel;
			}
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x0600303B RID: 12347 RVA: 0x00020DDD File Offset: 0x0001EFDD
		public override string assignedControllersGroupLabel
		{
			get
			{
				return this._assignedControllersGroupLabel;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x0600303C RID: 12348 RVA: 0x00020DE5 File Offset: 0x0001EFE5
		public override string settingsGroupLabel
		{
			get
			{
				return this._settingsGroupLabel;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x0600303D RID: 12349 RVA: 0x00020DED File Offset: 0x0001EFED
		public override string mapCategoriesGroupLabel
		{
			get
			{
				return this._mapCategoriesGroupLabel;
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x0600303E RID: 12350 RVA: 0x00020DF5 File Offset: 0x0001EFF5
		public override string restoreDefaultsWindowMessage
		{
			get
			{
				if (ReInput.players.playerCount > 1)
				{
					return this._restoreDefaultsWindowMessage_multiPlayer;
				}
				return this._restoreDefaultsWindowMessage_onePlayer;
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x0600303F RID: 12351 RVA: 0x00020E11 File Offset: 0x0001F011
		public override string calibrateWindow_deadZoneSliderLabel
		{
			get
			{
				return this._calibrateWindow_deadZoneSliderLabel;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06003040 RID: 12352 RVA: 0x00020E19 File Offset: 0x0001F019
		public override string calibrateWindow_zeroSliderLabel
		{
			get
			{
				return this._calibrateWindow_zeroSliderLabel;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06003041 RID: 12353 RVA: 0x00020E21 File Offset: 0x0001F021
		public override string calibrateWindow_sensitivitySliderLabel
		{
			get
			{
				return this._calibrateWindow_sensitivitySliderLabel;
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06003042 RID: 12354 RVA: 0x00020E29 File Offset: 0x0001F029
		public override string calibrateWindow_invertToggleLabel
		{
			get
			{
				return this._calibrateWindow_invertToggleLabel;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06003043 RID: 12355 RVA: 0x00020E31 File Offset: 0x0001F031
		public override string calibrateWindow_calibrateButtonLabel
		{
			get
			{
				return this._calibrateWindow_calibrateButtonLabel;
			}
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x00020E39 File Offset: 0x0001F039
		public override string GetControllerAssignmentConflictWindowMessage(string joystickName, string otherPlayerName, string currentPlayerName)
		{
			return string.Format(this._controllerAssignmentConflictWindowMessage, joystickName, otherPlayerName, currentPlayerName);
		}

		// Token: 0x06003045 RID: 12357 RVA: 0x00020E49 File Offset: 0x0001F049
		public override string GetJoystickElementAssignmentPollingWindowMessage(string actionName)
		{
			return string.Format(this._joystickElementAssignmentPollingWindowMessage, actionName);
		}

		// Token: 0x06003046 RID: 12358 RVA: 0x00020E57 File Offset: 0x0001F057
		public override string GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(string actionName)
		{
			return string.Format(this._joystickElementAssignmentPollingWindowMessage_fullAxisFieldOnly, actionName);
		}

		// Token: 0x06003047 RID: 12359 RVA: 0x00020E65 File Offset: 0x0001F065
		public override string GetKeyboardElementAssignmentPollingWindowMessage(string actionName)
		{
			return string.Format(this._keyboardElementAssignmentPollingWindowMessage, actionName);
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x00020E73 File Offset: 0x0001F073
		public override string GetMouseElementAssignmentPollingWindowMessage(string actionName)
		{
			return string.Format(this._mouseElementAssignmentPollingWindowMessage, actionName);
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x00020E81 File Offset: 0x0001F081
		public override string GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(string actionName)
		{
			return string.Format(this._mouseElementAssignmentPollingWindowMessage_fullAxisFieldOnly, actionName);
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x00020E8F File Offset: 0x0001F08F
		public override string GetElementAlreadyInUseBlocked(string elementName)
		{
			return string.Format(this._elementAlreadyInUseBlocked, elementName);
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x00020E9D File Offset: 0x0001F09D
		public override string GetElementAlreadyInUseCanReplace(string elementName, bool allowConflicts)
		{
			if (!allowConflicts)
			{
				return string.Format(this._elementAlreadyInUseCanReplace, elementName);
			}
			return string.Format(this._elementAlreadyInUseCanReplace_conflictAllowed, elementName);
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x00020EBB File Offset: 0x0001F0BB
		public override string GetMouseAssignmentConflictWindowMessage(string otherPlayerName, string thisPlayerName)
		{
			return string.Format(this._mouseAssignmentConflictWindowMessage, otherPlayerName, thisPlayerName);
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x00020ECA File Offset: 0x0001F0CA
		public override string GetCalibrateAxisStep1WindowMessage(string axisName)
		{
			return string.Format(this._calibrateAxisStep1WindowMessage, axisName);
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x00020ED8 File Offset: 0x0001F0D8
		public override string GetCalibrateAxisStep2WindowMessage(string axisName)
		{
			return string.Format(this._calibrateAxisStep2WindowMessage, axisName);
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x00020EE6 File Offset: 0x0001F0E6
		public override string GetPlayerName(int playerId)
		{
			Player player = ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				throw new ArgumentException("Invalid player id: " + playerId.ToString());
			}
			return player.descriptiveName;
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x00020F12 File Offset: 0x0001F112
		public override string GetControllerName(Controller controller)
		{
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			return controller.name;
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x00101608 File Offset: 0x000FF808
		public override string GetElementIdentifierName(ActionElementMap actionElementMap)
		{
			if (actionElementMap == null)
			{
				throw new ArgumentNullException("actionElementMap");
			}
			if (actionElementMap.controllerMap.controllerType == ControllerType.Keyboard)
			{
				return this.GetElementIdentifierName(actionElementMap.keyCode, actionElementMap.modifierKeyFlags);
			}
			return this.GetElementIdentifierName(actionElementMap.controllerMap.controller, actionElementMap.elementIdentifierId, actionElementMap.axisRange);
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x00101660 File Offset: 0x000FF860
		public override string GetElementIdentifierName(Controller controller, int elementIdentifierId, AxisRange axisRange)
		{
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			ControllerElementIdentifier elementIdentifierById = controller.GetElementIdentifierById(elementIdentifierId);
			if (elementIdentifierById == null)
			{
				throw new ArgumentException("Invalid element identifier id: " + elementIdentifierId.ToString());
			}
			Controller.Element elementById = controller.GetElementById(elementIdentifierId);
			if (elementById == null)
			{
				return string.Empty;
			}
			ControllerElementType type = elementById.type;
			if (type == ControllerElementType.Axis)
			{
				return elementIdentifierById.GetDisplayName(elementById.type, axisRange);
			}
			if (type != ControllerElementType.Button)
			{
				return elementIdentifierById.name;
			}
			return elementIdentifierById.name;
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x00020F28 File Offset: 0x0001F128
		public override string GetElementIdentifierName(KeyCode keyCode, ModifierKeyFlags modifierKeyFlags)
		{
			if (modifierKeyFlags != ModifierKeyFlags.None)
			{
				return string.Format("{0}{1}{2}", this.ModifierKeyFlagsToString(modifierKeyFlags), this._modifierKeys.separator, Keyboard.GetKeyName(keyCode));
			}
			return Keyboard.GetKeyName(keyCode);
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x00020F56 File Offset: 0x0001F156
		public override string GetActionName(int actionId)
		{
			InputAction action = ReInput.mapping.GetAction(actionId);
			if (action == null)
			{
				throw new ArgumentException("Invalid action id: " + actionId.ToString());
			}
			return action.descriptiveName;
		}

		// Token: 0x06003055 RID: 12373 RVA: 0x001016DC File Offset: 0x000FF8DC
		public override string GetActionName(int actionId, AxisRange axisRange)
		{
			InputAction action = ReInput.mapping.GetAction(actionId);
			if (action == null)
			{
				throw new ArgumentException("Invalid action id: " + actionId.ToString());
			}
			switch (axisRange)
			{
			case AxisRange.Full:
				return action.descriptiveName;
			case AxisRange.Positive:
				if (string.IsNullOrEmpty(action.positiveDescriptiveName))
				{
					return action.descriptiveName + " +";
				}
				return action.positiveDescriptiveName;
			case AxisRange.Negative:
				if (string.IsNullOrEmpty(action.negativeDescriptiveName))
				{
					return action.descriptiveName + " -";
				}
				return action.negativeDescriptiveName;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x00020F82 File Offset: 0x0001F182
		public override string GetMapCategoryName(int id)
		{
			InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(id);
			if (mapCategory == null)
			{
				throw new ArgumentException("Invalid map category id: " + id.ToString());
			}
			return mapCategory.descriptiveName;
		}

		// Token: 0x06003057 RID: 12375 RVA: 0x00020FAE File Offset: 0x0001F1AE
		public override string GetActionCategoryName(int id)
		{
			InputCategory actionCategory = ReInput.mapping.GetActionCategory(id);
			if (actionCategory == null)
			{
				throw new ArgumentException("Invalid action category id: " + id.ToString());
			}
			return actionCategory.descriptiveName;
		}

		// Token: 0x06003058 RID: 12376 RVA: 0x00020FDA File Offset: 0x0001F1DA
		public override string GetLayoutName(ControllerType controllerType, int id)
		{
			InputLayout layout = ReInput.mapping.GetLayout(controllerType, id);
			if (layout == null)
			{
				throw new ArgumentException("Invalid " + controllerType.ToString() + " layout id: " + id.ToString());
			}
			return layout.descriptiveName;
		}

		// Token: 0x06003059 RID: 12377 RVA: 0x0010177C File Offset: 0x000FF97C
		public override string ModifierKeyFlagsToString(ModifierKeyFlags flags)
		{
			int num = 0;
			string text = string.Empty;
			if (Keyboard.ModifierKeyFlagsContain(flags, ModifierKey.Control))
			{
				text += this._modifierKeys.control;
				num++;
			}
			if (Keyboard.ModifierKeyFlagsContain(flags, ModifierKey.Command))
			{
				if (num > 0 && !string.IsNullOrEmpty(this._modifierKeys.separator))
				{
					text += this._modifierKeys.separator;
				}
				text += this._modifierKeys.command;
				num++;
			}
			if (Keyboard.ModifierKeyFlagsContain(flags, ModifierKey.Alt))
			{
				if (num > 0 && !string.IsNullOrEmpty(this._modifierKeys.separator))
				{
					text += this._modifierKeys.separator;
				}
				text += this._modifierKeys.alt;
				num++;
			}
			if (num >= 3)
			{
				return text;
			}
			if (Keyboard.ModifierKeyFlagsContain(flags, ModifierKey.Shift))
			{
				if (num > 0 && !string.IsNullOrEmpty(this._modifierKeys.separator))
				{
					text += this._modifierKeys.separator;
				}
				text += this._modifierKeys.shift;
				num++;
			}
			return text;
		}

		// Token: 0x04002FA6 RID: 12198
		[SerializeField]
		private string _yes = "Yes";

		// Token: 0x04002FA7 RID: 12199
		[SerializeField]
		private string _no = "No";

		// Token: 0x04002FA8 RID: 12200
		[SerializeField]
		private string _add = "Add";

		// Token: 0x04002FA9 RID: 12201
		[SerializeField]
		private string _replace = "Replace";

		// Token: 0x04002FAA RID: 12202
		[SerializeField]
		private string _remove = "Remove";

		// Token: 0x04002FAB RID: 12203
		[SerializeField]
		private string _swap = "Swap";

		// Token: 0x04002FAC RID: 12204
		[SerializeField]
		private string _cancel = "Cancel";

		// Token: 0x04002FAD RID: 12205
		[SerializeField]
		private string _none = "None";

		// Token: 0x04002FAE RID: 12206
		[SerializeField]
		private string _okay = "Okay";

		// Token: 0x04002FAF RID: 12207
		[SerializeField]
		private string _done = "Done";

		// Token: 0x04002FB0 RID: 12208
		[SerializeField]
		private string _default = "Default";

		// Token: 0x04002FB1 RID: 12209
		[SerializeField]
		private string _assignControllerWindowTitle = "Choose Controller";

		// Token: 0x04002FB2 RID: 12210
		[SerializeField]
		private string _assignControllerWindowMessage = "Press any button or move an axis on the controller you would like to use.";

		// Token: 0x04002FB3 RID: 12211
		[SerializeField]
		private string _controllerAssignmentConflictWindowTitle = "Controller Assignment";

		// Token: 0x04002FB4 RID: 12212
		[SerializeField]
		[Tooltip("{0} = Joystick Name\n{1} = Other Player Name\n{2} = This Player Name")]
		private string _controllerAssignmentConflictWindowMessage = "{0} is already assigned to {1}. Do you want to assign this controller to {2} instead?";

		// Token: 0x04002FB5 RID: 12213
		[SerializeField]
		private string _elementAssignmentPrePollingWindowMessage = "First center or zero all sticks and axes and press any button or wait for the timer to finish.";

		// Token: 0x04002FB6 RID: 12214
		[SerializeField]
		[Tooltip("{0} = Action Name")]
		private string _joystickElementAssignmentPollingWindowMessage = "Now press a button or move an axis to assign it to {0}.";

		// Token: 0x04002FB7 RID: 12215
		[SerializeField]
		[Tooltip("This text is only displayed when split-axis fields have been disabled and the user clicks on the full-axis field. Button/key/D-pad input cannot be assigned to a full-axis field.\n{0} = Action Name")]
		private string _joystickElementAssignmentPollingWindowMessage_fullAxisFieldOnly = "Now move an axis to assign it to {0}.";

		// Token: 0x04002FB8 RID: 12216
		[SerializeField]
		[Tooltip("{0} = Action Name")]
		private string _keyboardElementAssignmentPollingWindowMessage = "Press a key to assign it to {0}. Modifier keys may also be used. To assign a modifier key alone, hold it down for 1 second.";

		// Token: 0x04002FB9 RID: 12217
		[SerializeField]
		[Tooltip("{0} = Action Name")]
		private string _mouseElementAssignmentPollingWindowMessage = "Press a mouse button or move an axis to assign it to {0}.";

		// Token: 0x04002FBA RID: 12218
		[SerializeField]
		[Tooltip("This text is only displayed when split-axis fields have been disabled and the user clicks on the full-axis field. Button/key/D-pad input cannot be assigned to a full-axis field.\n{0} = Action Name")]
		private string _mouseElementAssignmentPollingWindowMessage_fullAxisFieldOnly = "Move an axis to assign it to {0}.";

		// Token: 0x04002FBB RID: 12219
		[SerializeField]
		private string _elementAssignmentConflictWindowMessage = "Assignment Conflict";

		// Token: 0x04002FBC RID: 12220
		[SerializeField]
		[Tooltip("{0} = Element Name")]
		private string _elementAlreadyInUseBlocked = "{0} is already in use cannot be replaced.";

		// Token: 0x04002FBD RID: 12221
		[SerializeField]
		[Tooltip("{0} = Element Name")]
		private string _elementAlreadyInUseCanReplace = "{0} is already in use. Do you want to replace it?";

		// Token: 0x04002FBE RID: 12222
		[SerializeField]
		[Tooltip("{0} = Element Name")]
		private string _elementAlreadyInUseCanReplace_conflictAllowed = "{0} is already in use. Do you want to replace it? You may also choose to add the assignment anyway.";

		// Token: 0x04002FBF RID: 12223
		[SerializeField]
		private string _mouseAssignmentConflictWindowTitle = "Mouse Assignment";

		// Token: 0x04002FC0 RID: 12224
		[SerializeField]
		[Tooltip("{0} = Other Player Name\n{1} = This Player Name")]
		private string _mouseAssignmentConflictWindowMessage = "The mouse is already assigned to {0}. Do you want to assign the mouse to {1} instead?";

		// Token: 0x04002FC1 RID: 12225
		[SerializeField]
		private string _calibrateControllerWindowTitle = "Calibrate Controller";

		// Token: 0x04002FC2 RID: 12226
		[SerializeField]
		private string _calibrateAxisStep1WindowTitle = "Calibrate Zero";

		// Token: 0x04002FC3 RID: 12227
		[SerializeField]
		[Tooltip("{0} = Axis Name")]
		private string _calibrateAxisStep1WindowMessage = "Center or zero {0} and press any button or wait for the timer to finish.";

		// Token: 0x04002FC4 RID: 12228
		[SerializeField]
		private string _calibrateAxisStep2WindowTitle = "Calibrate Range";

		// Token: 0x04002FC5 RID: 12229
		[SerializeField]
		[Tooltip("{0} = Axis Name")]
		private string _calibrateAxisStep2WindowMessage = "Move {0} through its entire range then press any button or wait for the timer to finish.";

		// Token: 0x04002FC6 RID: 12230
		[SerializeField]
		private string _inputBehaviorSettingsWindowTitle = "Sensitivity Settings";

		// Token: 0x04002FC7 RID: 12231
		[SerializeField]
		private string _restoreDefaultsWindowTitle = "Restore Defaults";

		// Token: 0x04002FC8 RID: 12232
		[SerializeField]
		[Tooltip("Message for a single player game.")]
		private string _restoreDefaultsWindowMessage_onePlayer = "This will restore the default input configuration. Are you sure you want to do this?";

		// Token: 0x04002FC9 RID: 12233
		[SerializeField]
		[Tooltip("Message for a multi-player game.")]
		private string _restoreDefaultsWindowMessage_multiPlayer = "This will restore the default input configuration for all players. Are you sure you want to do this?";

		// Token: 0x04002FCA RID: 12234
		[SerializeField]
		private string _actionColumnLabel = "Actions";

		// Token: 0x04002FCB RID: 12235
		[SerializeField]
		private string _keyboardColumnLabel = "Keyboard";

		// Token: 0x04002FCC RID: 12236
		[SerializeField]
		private string _mouseColumnLabel = "Mouse";

		// Token: 0x04002FCD RID: 12237
		[SerializeField]
		private string _controllerColumnLabel = "Controller";

		// Token: 0x04002FCE RID: 12238
		[SerializeField]
		private string _removeControllerButtonLabel = "Remove";

		// Token: 0x04002FCF RID: 12239
		[SerializeField]
		private string _calibrateControllerButtonLabel = "Calibrate";

		// Token: 0x04002FD0 RID: 12240
		[SerializeField]
		private string _assignControllerButtonLabel = "Assign Controller";

		// Token: 0x04002FD1 RID: 12241
		[SerializeField]
		private string _inputBehaviorSettingsButtonLabel = "Sensitivity";

		// Token: 0x04002FD2 RID: 12242
		[SerializeField]
		private string _doneButtonLabel = "Done";

		// Token: 0x04002FD3 RID: 12243
		[SerializeField]
		private string _restoreDefaultsButtonLabel = "Restore Defaults";

		// Token: 0x04002FD4 RID: 12244
		[SerializeField]
		private string _playersGroupLabel = "Players:";

		// Token: 0x04002FD5 RID: 12245
		[SerializeField]
		private string _controllerSettingsGroupLabel = "Controller:";

		// Token: 0x04002FD6 RID: 12246
		[SerializeField]
		private string _assignedControllersGroupLabel = "Assigned Controllers:";

		// Token: 0x04002FD7 RID: 12247
		[SerializeField]
		private string _settingsGroupLabel = "Settings:";

		// Token: 0x04002FD8 RID: 12248
		[SerializeField]
		private string _mapCategoriesGroupLabel = "Categories:";

		// Token: 0x04002FD9 RID: 12249
		[SerializeField]
		private string _calibrateWindow_deadZoneSliderLabel = "Dead Zone:";

		// Token: 0x04002FDA RID: 12250
		[SerializeField]
		private string _calibrateWindow_zeroSliderLabel = "Zero:";

		// Token: 0x04002FDB RID: 12251
		[SerializeField]
		private string _calibrateWindow_sensitivitySliderLabel = "Sensitivity:";

		// Token: 0x04002FDC RID: 12252
		[SerializeField]
		private string _calibrateWindow_invertToggleLabel = "Invert";

		// Token: 0x04002FDD RID: 12253
		[SerializeField]
		private string _calibrateWindow_calibrateButtonLabel = "Calibrate";

		// Token: 0x04002FDE RID: 12254
		[SerializeField]
		private LanguageData.ModifierKeys _modifierKeys;

		// Token: 0x04002FDF RID: 12255
		[SerializeField]
		private LanguageData.CustomEntry[] _customEntries;

		// Token: 0x04002FE0 RID: 12256
		private bool _initialized;

		// Token: 0x04002FE1 RID: 12257
		private Dictionary<string, string> customDict;

		// Token: 0x0200068C RID: 1676
		[Serializable]
		protected class CustomEntry
		{
			// Token: 0x0600305B RID: 12379 RVA: 0x00004023 File Offset: 0x00002223
			public CustomEntry()
			{
			}

			// Token: 0x0600305C RID: 12380 RVA: 0x00021019 File Offset: 0x0001F219
			public CustomEntry(string key, string value)
			{
				this.key = key;
				this.value = value;
			}

			// Token: 0x0600305D RID: 12381 RVA: 0x00101B08 File Offset: 0x000FFD08
			public static Dictionary<string, string> ToDictionary(LanguageData.CustomEntry[] array)
			{
				if (array == null)
				{
					return new Dictionary<string, string>();
				}
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null && !string.IsNullOrEmpty(array[i].key) && !string.IsNullOrEmpty(array[i].value))
					{
						if (dictionary.ContainsKey(array[i].key))
						{
							Debug.LogError("Key \"" + array[i].key + "\" is already in dictionary!");
						}
						else
						{
							dictionary.Add(array[i].key, array[i].value);
						}
					}
				}
				return dictionary;
			}

			// Token: 0x04002FE2 RID: 12258
			public string key;

			// Token: 0x04002FE3 RID: 12259
			public string value;
		}

		// Token: 0x0200068D RID: 1677
		[Serializable]
		protected class ModifierKeys
		{
			// Token: 0x04002FE4 RID: 12260
			public string control = "Control";

			// Token: 0x04002FE5 RID: 12261
			public string alt = "Alt";

			// Token: 0x04002FE6 RID: 12262
			public string shift = "Shift";

			// Token: 0x04002FE7 RID: 12263
			public string command = "Command";

			// Token: 0x04002FE8 RID: 12264
			public string separator = " + ";
		}
	}
}
