using System;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200068E RID: 1678
	[Serializable]
	public abstract class LanguageDataBase : ScriptableObject
	{
		// Token: 0x0600305F RID: 12383
		public abstract void Initialize();

		// Token: 0x06003060 RID: 12384
		public abstract string GetCustomEntry(string key);

		// Token: 0x06003061 RID: 12385
		public abstract bool ContainsCustomEntryKey(string key);

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06003062 RID: 12386
		public abstract string yes { get; }

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06003063 RID: 12387
		public abstract string no { get; }

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06003064 RID: 12388
		public abstract string add { get; }

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06003065 RID: 12389
		public abstract string replace { get; }

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06003066 RID: 12390
		public abstract string remove { get; }

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06003067 RID: 12391
		public abstract string swap { get; }

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06003068 RID: 12392
		public abstract string cancel { get; }

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06003069 RID: 12393
		public abstract string none { get; }

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x0600306A RID: 12394
		public abstract string okay { get; }

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x0600306B RID: 12395
		public abstract string done { get; }

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x0600306C RID: 12396
		public abstract string default_ { get; }

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x0600306D RID: 12397
		public abstract string assignControllerWindowTitle { get; }

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x0600306E RID: 12398
		public abstract string assignControllerWindowMessage { get; }

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x0600306F RID: 12399
		public abstract string controllerAssignmentConflictWindowTitle { get; }

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06003070 RID: 12400
		public abstract string elementAssignmentPrePollingWindowMessage { get; }

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06003071 RID: 12401
		public abstract string elementAssignmentConflictWindowMessage { get; }

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06003072 RID: 12402
		public abstract string mouseAssignmentConflictWindowTitle { get; }

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06003073 RID: 12403
		public abstract string calibrateControllerWindowTitle { get; }

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06003074 RID: 12404
		public abstract string calibrateAxisStep1WindowTitle { get; }

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06003075 RID: 12405
		public abstract string calibrateAxisStep2WindowTitle { get; }

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06003076 RID: 12406
		public abstract string inputBehaviorSettingsWindowTitle { get; }

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06003077 RID: 12407
		public abstract string restoreDefaultsWindowTitle { get; }

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06003078 RID: 12408
		public abstract string actionColumnLabel { get; }

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06003079 RID: 12409
		public abstract string keyboardColumnLabel { get; }

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x0600307A RID: 12410
		public abstract string mouseColumnLabel { get; }

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x0600307B RID: 12411
		public abstract string controllerColumnLabel { get; }

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x0600307C RID: 12412
		public abstract string removeControllerButtonLabel { get; }

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x0600307D RID: 12413
		public abstract string calibrateControllerButtonLabel { get; }

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x0600307E RID: 12414
		public abstract string assignControllerButtonLabel { get; }

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x0600307F RID: 12415
		public abstract string inputBehaviorSettingsButtonLabel { get; }

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06003080 RID: 12416
		public abstract string doneButtonLabel { get; }

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06003081 RID: 12417
		public abstract string restoreDefaultsButtonLabel { get; }

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06003082 RID: 12418
		public abstract string controllerSettingsGroupLabel { get; }

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06003083 RID: 12419
		public abstract string playersGroupLabel { get; }

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06003084 RID: 12420
		public abstract string assignedControllersGroupLabel { get; }

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06003085 RID: 12421
		public abstract string settingsGroupLabel { get; }

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06003086 RID: 12422
		public abstract string mapCategoriesGroupLabel { get; }

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06003087 RID: 12423
		public abstract string restoreDefaultsWindowMessage { get; }

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06003088 RID: 12424
		public abstract string calibrateWindow_deadZoneSliderLabel { get; }

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06003089 RID: 12425
		public abstract string calibrateWindow_zeroSliderLabel { get; }

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x0600308A RID: 12426
		public abstract string calibrateWindow_sensitivitySliderLabel { get; }

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x0600308B RID: 12427
		public abstract string calibrateWindow_invertToggleLabel { get; }

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x0600308C RID: 12428
		public abstract string calibrateWindow_calibrateButtonLabel { get; }

		// Token: 0x0600308D RID: 12429
		public abstract string GetControllerAssignmentConflictWindowMessage(string joystickName, string otherPlayerName, string currentPlayerName);

		// Token: 0x0600308E RID: 12430
		public abstract string GetJoystickElementAssignmentPollingWindowMessage(string actionName);

		// Token: 0x0600308F RID: 12431
		public abstract string GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(string actionName);

		// Token: 0x06003090 RID: 12432
		public abstract string GetKeyboardElementAssignmentPollingWindowMessage(string actionName);

		// Token: 0x06003091 RID: 12433
		public abstract string GetMouseElementAssignmentPollingWindowMessage(string actionName);

		// Token: 0x06003092 RID: 12434
		public abstract string GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(string actionName);

		// Token: 0x06003093 RID: 12435
		public abstract string GetElementAlreadyInUseBlocked(string elementName);

		// Token: 0x06003094 RID: 12436
		public abstract string GetElementAlreadyInUseCanReplace(string elementName, bool allowConflicts);

		// Token: 0x06003095 RID: 12437
		public abstract string GetMouseAssignmentConflictWindowMessage(string otherPlayerName, string thisPlayerName);

		// Token: 0x06003096 RID: 12438
		public abstract string GetCalibrateAxisStep1WindowMessage(string axisName);

		// Token: 0x06003097 RID: 12439
		public abstract string GetCalibrateAxisStep2WindowMessage(string axisName);

		// Token: 0x06003098 RID: 12440
		public abstract string GetPlayerName(int playerId);

		// Token: 0x06003099 RID: 12441
		public abstract string GetControllerName(Controller controller);

		// Token: 0x0600309A RID: 12442
		public abstract string GetElementIdentifierName(ActionElementMap actionElementMap);

		// Token: 0x0600309B RID: 12443
		public abstract string GetElementIdentifierName(Controller controller, int elementIdentifierId, AxisRange axisRange);

		// Token: 0x0600309C RID: 12444
		public abstract string GetElementIdentifierName(KeyCode keyCode, ModifierKeyFlags modifierKeyFlags);

		// Token: 0x0600309D RID: 12445
		public abstract string GetActionName(int actionId);

		// Token: 0x0600309E RID: 12446
		public abstract string GetActionName(int actionId, AxisRange axisRange);

		// Token: 0x0600309F RID: 12447
		public abstract string GetMapCategoryName(int id);

		// Token: 0x060030A0 RID: 12448
		public abstract string GetActionCategoryName(int id);

		// Token: 0x060030A1 RID: 12449
		public abstract string GetLayoutName(ControllerType controllerType, int id);

		// Token: 0x060030A2 RID: 12450
		public abstract string ModifierKeyFlagsToString(ModifierKeyFlags flags);
	}
}
