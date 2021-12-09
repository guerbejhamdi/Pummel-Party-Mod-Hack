using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200065B RID: 1627
	[AddComponentMenu("")]
	public class ControlMapper : MonoBehaviour
	{
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06002D01 RID: 11521 RVA: 0x0001E652 File Offset: 0x0001C852
		// (remove) Token: 0x06002D02 RID: 11522 RVA: 0x0001E66B File Offset: 0x0001C86B
		public event Action ScreenClosedEvent
		{
			add
			{
				this._ScreenClosedEvent = (Action)Delegate.Combine(this._ScreenClosedEvent, value);
			}
			remove
			{
				this._ScreenClosedEvent = (Action)Delegate.Remove(this._ScreenClosedEvent, value);
			}
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06002D03 RID: 11523 RVA: 0x0001E684 File Offset: 0x0001C884
		// (remove) Token: 0x06002D04 RID: 11524 RVA: 0x0001E69D File Offset: 0x0001C89D
		public event Action ScreenOpenedEvent
		{
			add
			{
				this._ScreenOpenedEvent = (Action)Delegate.Combine(this._ScreenOpenedEvent, value);
			}
			remove
			{
				this._ScreenOpenedEvent = (Action)Delegate.Remove(this._ScreenOpenedEvent, value);
			}
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06002D05 RID: 11525 RVA: 0x0001E6B6 File Offset: 0x0001C8B6
		// (remove) Token: 0x06002D06 RID: 11526 RVA: 0x0001E6CF File Offset: 0x0001C8CF
		public event Action PopupWindowClosedEvent
		{
			add
			{
				this._PopupWindowClosedEvent = (Action)Delegate.Combine(this._PopupWindowClosedEvent, value);
			}
			remove
			{
				this._PopupWindowClosedEvent = (Action)Delegate.Remove(this._PopupWindowClosedEvent, value);
			}
		}

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06002D07 RID: 11527 RVA: 0x0001E6E8 File Offset: 0x0001C8E8
		// (remove) Token: 0x06002D08 RID: 11528 RVA: 0x0001E701 File Offset: 0x0001C901
		public event Action PopupWindowOpenedEvent
		{
			add
			{
				this._PopupWindowOpenedEvent = (Action)Delegate.Combine(this._PopupWindowOpenedEvent, value);
			}
			remove
			{
				this._PopupWindowOpenedEvent = (Action)Delegate.Remove(this._PopupWindowOpenedEvent, value);
			}
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06002D09 RID: 11529 RVA: 0x0001E71A File Offset: 0x0001C91A
		// (remove) Token: 0x06002D0A RID: 11530 RVA: 0x0001E733 File Offset: 0x0001C933
		public event Action InputPollingStartedEvent
		{
			add
			{
				this._InputPollingStartedEvent = (Action)Delegate.Combine(this._InputPollingStartedEvent, value);
			}
			remove
			{
				this._InputPollingStartedEvent = (Action)Delegate.Remove(this._InputPollingStartedEvent, value);
			}
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06002D0B RID: 11531 RVA: 0x0001E74C File Offset: 0x0001C94C
		// (remove) Token: 0x06002D0C RID: 11532 RVA: 0x0001E765 File Offset: 0x0001C965
		public event Action InputPollingEndedEvent
		{
			add
			{
				this._InputPollingEndedEvent = (Action)Delegate.Combine(this._InputPollingEndedEvent, value);
			}
			remove
			{
				this._InputPollingEndedEvent = (Action)Delegate.Remove(this._InputPollingEndedEvent, value);
			}
		}

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06002D0D RID: 11533 RVA: 0x0001E77E File Offset: 0x0001C97E
		// (remove) Token: 0x06002D0E RID: 11534 RVA: 0x0001E78C File Offset: 0x0001C98C
		public event UnityAction onScreenClosed
		{
			add
			{
				this._onScreenClosed.AddListener(value);
			}
			remove
			{
				this._onScreenClosed.RemoveListener(value);
			}
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06002D0F RID: 11535 RVA: 0x0001E79A File Offset: 0x0001C99A
		// (remove) Token: 0x06002D10 RID: 11536 RVA: 0x0001E7A8 File Offset: 0x0001C9A8
		public event UnityAction onScreenOpened
		{
			add
			{
				this._onScreenOpened.AddListener(value);
			}
			remove
			{
				this._onScreenOpened.RemoveListener(value);
			}
		}

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06002D11 RID: 11537 RVA: 0x0001E7B6 File Offset: 0x0001C9B6
		// (remove) Token: 0x06002D12 RID: 11538 RVA: 0x0001E7C4 File Offset: 0x0001C9C4
		public event UnityAction onPopupWindowClosed
		{
			add
			{
				this._onPopupWindowClosed.AddListener(value);
			}
			remove
			{
				this._onPopupWindowClosed.RemoveListener(value);
			}
		}

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06002D13 RID: 11539 RVA: 0x0001E7D2 File Offset: 0x0001C9D2
		// (remove) Token: 0x06002D14 RID: 11540 RVA: 0x0001E7E0 File Offset: 0x0001C9E0
		public event UnityAction onPopupWindowOpened
		{
			add
			{
				this._onPopupWindowOpened.AddListener(value);
			}
			remove
			{
				this._onPopupWindowOpened.RemoveListener(value);
			}
		}

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06002D15 RID: 11541 RVA: 0x0001E7EE File Offset: 0x0001C9EE
		// (remove) Token: 0x06002D16 RID: 11542 RVA: 0x0001E7FC File Offset: 0x0001C9FC
		public event UnityAction onInputPollingStarted
		{
			add
			{
				this._onInputPollingStarted.AddListener(value);
			}
			remove
			{
				this._onInputPollingStarted.RemoveListener(value);
			}
		}

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06002D17 RID: 11543 RVA: 0x0001E80A File Offset: 0x0001CA0A
		// (remove) Token: 0x06002D18 RID: 11544 RVA: 0x0001E818 File Offset: 0x0001CA18
		public event UnityAction onInputPollingEnded
		{
			add
			{
				this._onInputPollingEnded.AddListener(value);
			}
			remove
			{
				this._onInputPollingEnded.RemoveListener(value);
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002D19 RID: 11545 RVA: 0x0001E826 File Offset: 0x0001CA26
		// (set) Token: 0x06002D1A RID: 11546 RVA: 0x0001E82E File Offset: 0x0001CA2E
		public InputManager rewiredInputManager
		{
			get
			{
				return this._rewiredInputManager;
			}
			set
			{
				this._rewiredInputManager = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002D1B RID: 11547 RVA: 0x0001E83E File Offset: 0x0001CA3E
		// (set) Token: 0x06002D1C RID: 11548 RVA: 0x0001E846 File Offset: 0x0001CA46
		public bool dontDestroyOnLoad
		{
			get
			{
				return this._dontDestroyOnLoad;
			}
			set
			{
				if (value != this._dontDestroyOnLoad && value)
				{
					UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
				}
				this._dontDestroyOnLoad = value;
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06002D1D RID: 11549 RVA: 0x0001E86B File Offset: 0x0001CA6B
		// (set) Token: 0x06002D1E RID: 11550 RVA: 0x0001E873 File Offset: 0x0001CA73
		public int keyboardMapDefaultLayout
		{
			get
			{
				return this._keyboardMapDefaultLayout;
			}
			set
			{
				this._keyboardMapDefaultLayout = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06002D1F RID: 11551 RVA: 0x0001E883 File Offset: 0x0001CA83
		// (set) Token: 0x06002D20 RID: 11552 RVA: 0x0001E88B File Offset: 0x0001CA8B
		public int mouseMapDefaultLayout
		{
			get
			{
				return this._mouseMapDefaultLayout;
			}
			set
			{
				this._mouseMapDefaultLayout = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06002D21 RID: 11553 RVA: 0x0001E89B File Offset: 0x0001CA9B
		// (set) Token: 0x06002D22 RID: 11554 RVA: 0x0001E8A3 File Offset: 0x0001CAA3
		public int joystickMapDefaultLayout
		{
			get
			{
				return this._joystickMapDefaultLayout;
			}
			set
			{
				this._joystickMapDefaultLayout = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06002D23 RID: 11555 RVA: 0x0001E8B3 File Offset: 0x0001CAB3
		// (set) Token: 0x06002D24 RID: 11556 RVA: 0x0001E8CC File Offset: 0x0001CACC
		public bool showPlayers
		{
			get
			{
				return this._showPlayers && ReInput.players.playerCount > 1;
			}
			set
			{
				this._showPlayers = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06002D25 RID: 11557 RVA: 0x0001E8DC File Offset: 0x0001CADC
		// (set) Token: 0x06002D26 RID: 11558 RVA: 0x0001E8E4 File Offset: 0x0001CAE4
		public bool showControllers
		{
			get
			{
				return this._showControllers;
			}
			set
			{
				this._showControllers = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06002D27 RID: 11559 RVA: 0x0001E8F4 File Offset: 0x0001CAF4
		// (set) Token: 0x06002D28 RID: 11560 RVA: 0x0001E8FC File Offset: 0x0001CAFC
		public bool showKeyboard
		{
			get
			{
				return this._showKeyboard;
			}
			set
			{
				this._showKeyboard = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06002D29 RID: 11561 RVA: 0x0001E90C File Offset: 0x0001CB0C
		// (set) Token: 0x06002D2A RID: 11562 RVA: 0x0001E914 File Offset: 0x0001CB14
		public bool showMouse
		{
			get
			{
				return this._showMouse;
			}
			set
			{
				this._showMouse = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06002D2B RID: 11563 RVA: 0x0001E924 File Offset: 0x0001CB24
		// (set) Token: 0x06002D2C RID: 11564 RVA: 0x0001E92C File Offset: 0x0001CB2C
		public int maxControllersPerPlayer
		{
			get
			{
				return this._maxControllersPerPlayer;
			}
			set
			{
				this._maxControllersPerPlayer = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06002D2D RID: 11565 RVA: 0x0001E93C File Offset: 0x0001CB3C
		// (set) Token: 0x06002D2E RID: 11566 RVA: 0x0001E944 File Offset: 0x0001CB44
		public bool showActionCategoryLabels
		{
			get
			{
				return this._showActionCategoryLabels;
			}
			set
			{
				this._showActionCategoryLabels = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06002D2F RID: 11567 RVA: 0x0001E954 File Offset: 0x0001CB54
		// (set) Token: 0x06002D30 RID: 11568 RVA: 0x0001E95C File Offset: 0x0001CB5C
		public int keyboardInputFieldCount
		{
			get
			{
				return this._keyboardInputFieldCount;
			}
			set
			{
				this._keyboardInputFieldCount = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06002D31 RID: 11569 RVA: 0x0001E96C File Offset: 0x0001CB6C
		// (set) Token: 0x06002D32 RID: 11570 RVA: 0x0001E974 File Offset: 0x0001CB74
		public int mouseInputFieldCount
		{
			get
			{
				return this._mouseInputFieldCount;
			}
			set
			{
				this._mouseInputFieldCount = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06002D33 RID: 11571 RVA: 0x0001E984 File Offset: 0x0001CB84
		// (set) Token: 0x06002D34 RID: 11572 RVA: 0x0001E98C File Offset: 0x0001CB8C
		public int controllerInputFieldCount
		{
			get
			{
				return this._controllerInputFieldCount;
			}
			set
			{
				this._controllerInputFieldCount = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002D35 RID: 11573 RVA: 0x0001E99C File Offset: 0x0001CB9C
		// (set) Token: 0x06002D36 RID: 11574 RVA: 0x0001E9A4 File Offset: 0x0001CBA4
		public bool showFullAxisInputFields
		{
			get
			{
				return this._showFullAxisInputFields;
			}
			set
			{
				this._showFullAxisInputFields = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06002D37 RID: 11575 RVA: 0x0001E9B4 File Offset: 0x0001CBB4
		// (set) Token: 0x06002D38 RID: 11576 RVA: 0x0001E9BC File Offset: 0x0001CBBC
		public bool showSplitAxisInputFields
		{
			get
			{
				return this._showSplitAxisInputFields;
			}
			set
			{
				this._showSplitAxisInputFields = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002D39 RID: 11577 RVA: 0x0001E9CC File Offset: 0x0001CBCC
		// (set) Token: 0x06002D3A RID: 11578 RVA: 0x0001E9D4 File Offset: 0x0001CBD4
		public bool allowElementAssignmentConflicts
		{
			get
			{
				return this._allowElementAssignmentConflicts;
			}
			set
			{
				this._allowElementAssignmentConflicts = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002D3B RID: 11579 RVA: 0x0001E9E4 File Offset: 0x0001CBE4
		// (set) Token: 0x06002D3C RID: 11580 RVA: 0x0001E9EC File Offset: 0x0001CBEC
		public bool allowElementAssignmentSwap
		{
			get
			{
				return this._allowElementAssignmentSwap;
			}
			set
			{
				this._allowElementAssignmentSwap = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002D3D RID: 11581 RVA: 0x0001E9FC File Offset: 0x0001CBFC
		// (set) Token: 0x06002D3E RID: 11582 RVA: 0x0001EA04 File Offset: 0x0001CC04
		public int actionLabelWidth
		{
			get
			{
				return this._actionLabelWidth;
			}
			set
			{
				this._actionLabelWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002D3F RID: 11583 RVA: 0x0001EA14 File Offset: 0x0001CC14
		// (set) Token: 0x06002D40 RID: 11584 RVA: 0x0001EA1C File Offset: 0x0001CC1C
		public int keyboardColMaxWidth
		{
			get
			{
				return this._keyboardColMaxWidth;
			}
			set
			{
				this._keyboardColMaxWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002D41 RID: 11585 RVA: 0x0001EA2C File Offset: 0x0001CC2C
		// (set) Token: 0x06002D42 RID: 11586 RVA: 0x0001EA34 File Offset: 0x0001CC34
		public int mouseColMaxWidth
		{
			get
			{
				return this._mouseColMaxWidth;
			}
			set
			{
				this._mouseColMaxWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002D43 RID: 11587 RVA: 0x0001EA44 File Offset: 0x0001CC44
		// (set) Token: 0x06002D44 RID: 11588 RVA: 0x0001EA4C File Offset: 0x0001CC4C
		public int controllerColMaxWidth
		{
			get
			{
				return this._controllerColMaxWidth;
			}
			set
			{
				this._controllerColMaxWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002D45 RID: 11589 RVA: 0x0001EA5C File Offset: 0x0001CC5C
		// (set) Token: 0x06002D46 RID: 11590 RVA: 0x0001EA64 File Offset: 0x0001CC64
		public int inputRowHeight
		{
			get
			{
				return this._inputRowHeight;
			}
			set
			{
				this._inputRowHeight = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002D47 RID: 11591 RVA: 0x0001EA74 File Offset: 0x0001CC74
		// (set) Token: 0x06002D48 RID: 11592 RVA: 0x0001EA7C File Offset: 0x0001CC7C
		public int inputColumnSpacing
		{
			get
			{
				return this._inputColumnSpacing;
			}
			set
			{
				this._inputColumnSpacing = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002D49 RID: 11593 RVA: 0x0001EA8C File Offset: 0x0001CC8C
		// (set) Token: 0x06002D4A RID: 11594 RVA: 0x0001EA94 File Offset: 0x0001CC94
		public int inputRowCategorySpacing
		{
			get
			{
				return this._inputRowCategorySpacing;
			}
			set
			{
				this._inputRowCategorySpacing = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06002D4B RID: 11595 RVA: 0x0001EAA4 File Offset: 0x0001CCA4
		// (set) Token: 0x06002D4C RID: 11596 RVA: 0x0001EAAC File Offset: 0x0001CCAC
		public int invertToggleWidth
		{
			get
			{
				return this._invertToggleWidth;
			}
			set
			{
				this._invertToggleWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06002D4D RID: 11597 RVA: 0x0001EABC File Offset: 0x0001CCBC
		// (set) Token: 0x06002D4E RID: 11598 RVA: 0x0001EAC4 File Offset: 0x0001CCC4
		public int defaultWindowWidth
		{
			get
			{
				return this._defaultWindowWidth;
			}
			set
			{
				this._defaultWindowWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06002D4F RID: 11599 RVA: 0x0001EAD4 File Offset: 0x0001CCD4
		// (set) Token: 0x06002D50 RID: 11600 RVA: 0x0001EADC File Offset: 0x0001CCDC
		public int defaultWindowHeight
		{
			get
			{
				return this._defaultWindowHeight;
			}
			set
			{
				this._defaultWindowHeight = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002D51 RID: 11601 RVA: 0x0001EAEC File Offset: 0x0001CCEC
		// (set) Token: 0x06002D52 RID: 11602 RVA: 0x0001EAF4 File Offset: 0x0001CCF4
		public float controllerAssignmentTimeout
		{
			get
			{
				return this._controllerAssignmentTimeout;
			}
			set
			{
				this._controllerAssignmentTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002D53 RID: 11603 RVA: 0x0001EB04 File Offset: 0x0001CD04
		// (set) Token: 0x06002D54 RID: 11604 RVA: 0x0001EB0C File Offset: 0x0001CD0C
		public float preInputAssignmentTimeout
		{
			get
			{
				return this._preInputAssignmentTimeout;
			}
			set
			{
				this._preInputAssignmentTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002D55 RID: 11605 RVA: 0x0001EB1C File Offset: 0x0001CD1C
		// (set) Token: 0x06002D56 RID: 11606 RVA: 0x0001EB24 File Offset: 0x0001CD24
		public float inputAssignmentTimeout
		{
			get
			{
				return this._inputAssignmentTimeout;
			}
			set
			{
				this._inputAssignmentTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002D57 RID: 11607 RVA: 0x0001EB34 File Offset: 0x0001CD34
		// (set) Token: 0x06002D58 RID: 11608 RVA: 0x0001EB3C File Offset: 0x0001CD3C
		public float axisCalibrationTimeout
		{
			get
			{
				return this._axisCalibrationTimeout;
			}
			set
			{
				this._axisCalibrationTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002D59 RID: 11609 RVA: 0x0001EB4C File Offset: 0x0001CD4C
		// (set) Token: 0x06002D5A RID: 11610 RVA: 0x0001EB54 File Offset: 0x0001CD54
		public bool ignoreMouseXAxisAssignment
		{
			get
			{
				return this._ignoreMouseXAxisAssignment;
			}
			set
			{
				this._ignoreMouseXAxisAssignment = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002D5B RID: 11611 RVA: 0x0001EB64 File Offset: 0x0001CD64
		// (set) Token: 0x06002D5C RID: 11612 RVA: 0x0001EB6C File Offset: 0x0001CD6C
		public bool ignoreMouseYAxisAssignment
		{
			get
			{
				return this._ignoreMouseYAxisAssignment;
			}
			set
			{
				this._ignoreMouseYAxisAssignment = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002D5D RID: 11613 RVA: 0x0001EB7C File Offset: 0x0001CD7C
		// (set) Token: 0x06002D5E RID: 11614 RVA: 0x0001EB84 File Offset: 0x0001CD84
		public bool universalCancelClosesScreen
		{
			get
			{
				return this._universalCancelClosesScreen;
			}
			set
			{
				this._universalCancelClosesScreen = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002D5F RID: 11615 RVA: 0x0001EB94 File Offset: 0x0001CD94
		// (set) Token: 0x06002D60 RID: 11616 RVA: 0x0001EB9C File Offset: 0x0001CD9C
		public bool showInputBehaviorSettings
		{
			get
			{
				return this._showInputBehaviorSettings;
			}
			set
			{
				this._showInputBehaviorSettings = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002D61 RID: 11617 RVA: 0x0001EBAC File Offset: 0x0001CDAC
		// (set) Token: 0x06002D62 RID: 11618 RVA: 0x0001EBB4 File Offset: 0x0001CDB4
		public bool useThemeSettings
		{
			get
			{
				return this._useThemeSettings;
			}
			set
			{
				this._useThemeSettings = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002D63 RID: 11619 RVA: 0x0001EBC4 File Offset: 0x0001CDC4
		// (set) Token: 0x06002D64 RID: 11620 RVA: 0x0001EBCC File Offset: 0x0001CDCC
		public LanguageDataBase language
		{
			get
			{
				return this._language;
			}
			set
			{
				this._language = value;
				if (this._language != null)
				{
					this._language.Initialize();
				}
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002D65 RID: 11621 RVA: 0x0001EBF5 File Offset: 0x0001CDF5
		// (set) Token: 0x06002D66 RID: 11622 RVA: 0x0001EBFD File Offset: 0x0001CDFD
		public bool showPlayersGroupLabel
		{
			get
			{
				return this._showPlayersGroupLabel;
			}
			set
			{
				this._showPlayersGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002D67 RID: 11623 RVA: 0x0001EC0D File Offset: 0x0001CE0D
		// (set) Token: 0x06002D68 RID: 11624 RVA: 0x0001EC15 File Offset: 0x0001CE15
		public bool showControllerGroupLabel
		{
			get
			{
				return this._showControllerGroupLabel;
			}
			set
			{
				this._showControllerGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06002D69 RID: 11625 RVA: 0x0001EC25 File Offset: 0x0001CE25
		// (set) Token: 0x06002D6A RID: 11626 RVA: 0x0001EC2D File Offset: 0x0001CE2D
		public bool showAssignedControllersGroupLabel
		{
			get
			{
				return this._showAssignedControllersGroupLabel;
			}
			set
			{
				this._showAssignedControllersGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06002D6B RID: 11627 RVA: 0x0001EC3D File Offset: 0x0001CE3D
		// (set) Token: 0x06002D6C RID: 11628 RVA: 0x0001EC45 File Offset: 0x0001CE45
		public bool showSettingsGroupLabel
		{
			get
			{
				return this._showSettingsGroupLabel;
			}
			set
			{
				this._showSettingsGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06002D6D RID: 11629 RVA: 0x0001EC55 File Offset: 0x0001CE55
		// (set) Token: 0x06002D6E RID: 11630 RVA: 0x0001EC5D File Offset: 0x0001CE5D
		public bool showMapCategoriesGroupLabel
		{
			get
			{
				return this._showMapCategoriesGroupLabel;
			}
			set
			{
				this._showMapCategoriesGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06002D6F RID: 11631 RVA: 0x0001EC6D File Offset: 0x0001CE6D
		// (set) Token: 0x06002D70 RID: 11632 RVA: 0x0001EC75 File Offset: 0x0001CE75
		public bool showControllerNameLabel
		{
			get
			{
				return this._showControllerNameLabel;
			}
			set
			{
				this._showControllerNameLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06002D71 RID: 11633 RVA: 0x0001EC85 File Offset: 0x0001CE85
		// (set) Token: 0x06002D72 RID: 11634 RVA: 0x0001EC8D File Offset: 0x0001CE8D
		public bool showAssignedControllers
		{
			get
			{
				return this._showAssignedControllers;
			}
			set
			{
				this._showAssignedControllers = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06002D73 RID: 11635 RVA: 0x0001EC9D File Offset: 0x0001CE9D
		// (set) Token: 0x06002D74 RID: 11636 RVA: 0x0001ECA5 File Offset: 0x0001CEA5
		public Action restoreDefaultsDelegate
		{
			get
			{
				return this._restoreDefaultsDelegate;
			}
			set
			{
				this._restoreDefaultsDelegate = value;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06002D75 RID: 11637 RVA: 0x0001ECAE File Offset: 0x0001CEAE
		public bool isOpen
		{
			get
			{
				if (!this.initialized)
				{
					return this.references.canvas != null && this.references.canvas.gameObject.activeInHierarchy;
				}
				return this.canvas.activeInHierarchy;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06002D76 RID: 11638 RVA: 0x0001ECEE File Offset: 0x0001CEEE
		private bool isFocused
		{
			get
			{
				return this.initialized && !this.windowManager.isWindowOpen;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06002D77 RID: 11639 RVA: 0x0001ED08 File Offset: 0x0001CF08
		private bool inputAllowed
		{
			get
			{
				return this.blockInputOnFocusEndTime <= Time.unscaledTime;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06002D78 RID: 11640 RVA: 0x000F9414 File Offset: 0x000F7614
		private int inputGridColumnCount
		{
			get
			{
				int num = 1;
				if (this._showKeyboard)
				{
					num++;
				}
				if (this._showMouse)
				{
					num++;
				}
				if (this._showControllers)
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06002D79 RID: 11641 RVA: 0x000F9448 File Offset: 0x000F7648
		private int inputGridWidth
		{
			get
			{
				return this._actionLabelWidth + (this._showKeyboard ? this._keyboardColMaxWidth : 0) + (this._showMouse ? this._mouseColMaxWidth : 0) + (this._showControllers ? this._controllerColMaxWidth : 0) + (this.inputGridColumnCount - 1) * this._inputColumnSpacing;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06002D7A RID: 11642 RVA: 0x0001ED1A File Offset: 0x0001CF1A
		private Player currentPlayer
		{
			get
			{
				return ReInput.players.GetPlayer(this.currentPlayerId);
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002D7B RID: 11643 RVA: 0x0001ED2C File Offset: 0x0001CF2C
		private InputCategory currentMapCategory
		{
			get
			{
				return ReInput.mapping.GetMapCategory(this.currentMapCategoryId);
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06002D7C RID: 11644 RVA: 0x000F94A4 File Offset: 0x000F76A4
		private ControlMapper.MappingSet currentMappingSet
		{
			get
			{
				if (this.currentMapCategoryId < 0)
				{
					return null;
				}
				for (int i = 0; i < this._mappingSets.Length; i++)
				{
					if (this._mappingSets[i].mapCategoryId == this.currentMapCategoryId)
					{
						return this._mappingSets[i];
					}
				}
				return null;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06002D7D RID: 11645 RVA: 0x0001ED3E File Offset: 0x0001CF3E
		private Joystick currentJoystick
		{
			get
			{
				return ReInput.controllers.GetJoystick(this.currentJoystickId);
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06002D7E RID: 11646 RVA: 0x0001ED50 File Offset: 0x0001CF50
		private bool isJoystickSelected
		{
			get
			{
				return this.currentJoystickId >= 0;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06002D7F RID: 11647 RVA: 0x0001ED5E File Offset: 0x0001CF5E
		private GameObject currentUISelection
		{
			get
			{
				if (!(EventSystem.current != null))
				{
					return null;
				}
				return EventSystem.current.currentSelectedGameObject;
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06002D80 RID: 11648 RVA: 0x0001ED79 File Offset: 0x0001CF79
		private bool showSettings
		{
			get
			{
				return this._showInputBehaviorSettings && this._inputBehaviorSettings.Length != 0;
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06002D81 RID: 11649 RVA: 0x0001ED8F File Offset: 0x0001CF8F
		private bool showMapCategories
		{
			get
			{
				return this._mappingSets != null && this._mappingSets.Length > 1;
			}
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x0001EDA9 File Offset: 0x0001CFA9
		private void Awake()
		{
			if (this._dontDestroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
			}
			this.PreInitialize();
			if (this.isOpen)
			{
				this.Initialize();
				this.Open(true);
			}
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x0001EDDE File Offset: 0x0001CFDE
		private void Start()
		{
			if (this._openOnStart)
			{
				this.Open(false);
			}
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x0001EDEF File Offset: 0x0001CFEF
		private void Update()
		{
			if (!this.isOpen)
			{
				return;
			}
			if (!this.initialized)
			{
				return;
			}
			this.CheckUISelection();
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x0001EE09 File Offset: 0x0001D009
		private void OnDestroy()
		{
			ReInput.ControllerConnectedEvent -= this.OnJoystickConnected;
			ReInput.ControllerDisconnectedEvent -= this.OnJoystickDisconnected;
			ReInput.ControllerPreDisconnectEvent -= this.OnJoystickPreDisconnect;
			this.UnsubscribeMenuControlInputEvents();
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x0001EE44 File Offset: 0x0001D044
		private void PreInitialize()
		{
			if (!ReInput.isReady)
			{
				Debug.LogError("Rewired Control Mapper: Rewired has not been initialized! Are you missing a Rewired Input Manager in your scene?");
				return;
			}
			this.SubscribeMenuControlInputEvents();
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000F94F0 File Offset: 0x000F76F0
		private void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			if (this._rewiredInputManager == null)
			{
				this._rewiredInputManager = UnityEngine.Object.FindObjectOfType<InputManager>();
				if (this._rewiredInputManager == null)
				{
					Debug.LogError("Rewired Control Mapper: A Rewired Input Manager was not assigned in the inspector or found in the current scene! Control Mapper will not function.");
					return;
				}
			}
			if (ControlMapper.Instance != null)
			{
				Debug.LogError("Rewired Control Mapper: Only one ControlMapper can exist at one time!");
				return;
			}
			ControlMapper.Instance = this;
			if (this.prefabs == null || !this.prefabs.Check())
			{
				Debug.LogError("Rewired Control Mapper: All prefabs must be assigned in the inspector!");
				return;
			}
			if (this.references == null || !this.references.Check())
			{
				Debug.LogError("Rewired Control Mapper: All references must be assigned in the inspector!");
				return;
			}
			this.references.inputGridLayoutElement = this.references.inputGridContainer.GetComponent<LayoutElement>();
			if (this.references.inputGridLayoutElement == null)
			{
				Debug.LogError("Rewired Control Mapper: InputGridContainer is missing LayoutElement component!");
				return;
			}
			if (this._showKeyboard && this._keyboardInputFieldCount < 1)
			{
				Debug.LogWarning("Rewired Control Mapper: Keyboard Input Fields must be at least 1!");
				this._keyboardInputFieldCount = 1;
			}
			if (this._showMouse && this._mouseInputFieldCount < 1)
			{
				Debug.LogWarning("Rewired Control Mapper: Mouse Input Fields must be at least 1!");
				this._mouseInputFieldCount = 1;
			}
			if (this._showControllers && this._controllerInputFieldCount < 1)
			{
				Debug.LogWarning("Rewired Control Mapper: Controller Input Fields must be at least 1!");
				this._controllerInputFieldCount = 1;
			}
			if (this._maxControllersPerPlayer < 0)
			{
				Debug.LogWarning("Rewired Control Mapper: Max Controllers Per Player must be at least 0 (no limit)!");
				this._maxControllersPerPlayer = 0;
			}
			if (this._useThemeSettings && this._themeSettings == null)
			{
				Debug.LogWarning("Rewired Control Mapper: To use theming, Theme Settings must be set in the inspector! Theming has been disabled.");
				this._useThemeSettings = false;
			}
			if (this._language == null)
			{
				Debug.LogError("Rawired UI: Language must be set in the inspector!");
				return;
			}
			this._language.Initialize();
			this.inputFieldActivatedDelegate = new Action<InputFieldInfo>(this.OnInputFieldActivated);
			this.inputFieldInvertToggleStateChangedDelegate = new Action<ToggleInfo, bool>(this.OnInputFieldInvertToggleStateChanged);
			ReInput.ControllerConnectedEvent += this.OnJoystickConnected;
			ReInput.ControllerDisconnectedEvent += this.OnJoystickDisconnected;
			ReInput.ControllerPreDisconnectEvent += this.OnJoystickPreDisconnect;
			this.playerCount = ReInput.players.playerCount;
			this.canvas = this.references.canvas.gameObject;
			this.windowManager = new ControlMapper.WindowManager(this.prefabs.window, this.prefabs.fader, this.references.mainCanvasGroup.transform.parent.Find("WindowParent"));
			this.playerButtons = new List<ControlMapper.GUIButton>();
			this.mapCategoryButtons = new List<ControlMapper.GUIButton>();
			this.assignedControllerButtons = new List<ControlMapper.GUIButton>();
			this.miscInstantiatedObjects = new List<GameObject>();
			this.currentMapCategoryId = this._mappingSets[0].mapCategoryId;
			this.Draw();
			this.CreateInputGrid();
			this.CreateLayout();
			this.SubscribeFixedUISelectionEvents();
			this.initialized = true;
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x0001EE5E File Offset: 0x0001D05E
		private void OnJoystickConnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this._showControllers)
			{
				return;
			}
			this.ClearVarsOnJoystickChange();
			this.ForceRefresh();
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x0001EE5E File Offset: 0x0001D05E
		private void OnJoystickDisconnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this._showControllers)
			{
				return;
			}
			this.ClearVarsOnJoystickChange();
			this.ForceRefresh();
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x0001EE7E File Offset: 0x0001D07E
		private void OnJoystickPreDisconnect(ControllerStatusChangedEventArgs args)
		{
			if (!this.initialized)
			{
				return;
			}
			bool showControllers = this._showControllers;
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x000F97C0 File Offset: 0x000F79C0
		public void OnButtonActivated(ButtonInfo buttonInfo)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.inputAllowed)
			{
				return;
			}
			string identifier = buttonInfo.identifier;
			if (identifier != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(identifier);
				if (num <= 1656078790U)
				{
					if (num <= 1293854844U)
					{
						if (num != 36291085U)
						{
							if (num != 1293854844U)
							{
								return;
							}
							if (!(identifier == "AssignController"))
							{
								return;
							}
							this.ShowAssignControllerWindow();
							return;
						}
						else
						{
							if (!(identifier == "MapCategorySelection"))
							{
								return;
							}
							this.OnMapCategorySelected(buttonInfo.intData, true);
							return;
						}
					}
					else if (num != 1619204974U)
					{
						if (num != 1656078790U)
						{
							return;
						}
						if (!(identifier == "EditInputBehaviors"))
						{
							return;
						}
						this.ShowEditInputBehaviorsWindow();
						return;
					}
					else
					{
						if (!(identifier == "PlayerSelection"))
						{
							return;
						}
						this.OnPlayerSelected(buttonInfo.intData, true);
						return;
					}
				}
				else if (num <= 2379421585U)
				{
					if (num != 2139278426U)
					{
						if (num != 2379421585U)
						{
							return;
						}
						if (!(identifier == "Done"))
						{
							return;
						}
						this.Close(true);
						return;
					}
					else
					{
						if (!(identifier == "CalibrateController"))
						{
							return;
						}
						this.ShowCalibrateControllerWindow();
						return;
					}
				}
				else if (num != 2857234147U)
				{
					if (num != 3019194153U)
					{
						if (num != 3496297297U)
						{
							return;
						}
						if (!(identifier == "AssignedControllerSelection"))
						{
							return;
						}
						this.OnControllerSelected(buttonInfo.intData);
						return;
					}
					else
					{
						if (!(identifier == "RemoveController"))
						{
							return;
						}
						this.OnRemoveCurrentController();
						return;
					}
				}
				else
				{
					if (!(identifier == "RestoreDefaults"))
					{
						return;
					}
					this.OnRestoreDefaults();
				}
			}
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x000F9930 File Offset: 0x000F7B30
		public void OnInputFieldActivated(InputFieldInfo fieldInfo)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.inputAllowed)
			{
				return;
			}
			if (this.currentPlayer == null)
			{
				return;
			}
			InputAction action = ReInput.mapping.GetAction(fieldInfo.actionId);
			if (action == null)
			{
				return;
			}
			AxisRange axisRange = (action.type == InputActionType.Axis) ? fieldInfo.axisRange : AxisRange.Full;
			string actionName = this._language.GetActionName(action.id, axisRange);
			ControllerMap controllerMap = this.GetControllerMap(fieldInfo.controllerType);
			if (controllerMap == null)
			{
				return;
			}
			ActionElementMap actionElementMap = (fieldInfo.actionElementMapId >= 0) ? controllerMap.GetElementMap(fieldInfo.actionElementMapId) : null;
			if (actionElementMap != null)
			{
				this.ShowBeginElementAssignmentReplacementWindow(fieldInfo, action, controllerMap, actionElementMap, actionName);
				return;
			}
			this.ShowCreateNewElementAssignmentWindow(fieldInfo, action, controllerMap, actionName);
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x0001EE90 File Offset: 0x0001D090
		public void OnInputFieldInvertToggleStateChanged(ToggleInfo toggleInfo, bool newState)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.inputAllowed)
			{
				return;
			}
			this.SetActionAxisInverted(newState, toggleInfo.controllerType, toggleInfo.actionElementMapId);
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x0001EEB7 File Offset: 0x0001D0B7
		private void OnPlayerSelected(int playerId, bool redraw)
		{
			if (!this.initialized)
			{
				return;
			}
			this.currentPlayerId = playerId;
			this.ClearVarsOnPlayerChange();
			if (redraw)
			{
				this.Redraw(true, true);
			}
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x0001EEDA File Offset: 0x0001D0DA
		private void OnControllerSelected(int joystickId)
		{
			if (!this.initialized)
			{
				return;
			}
			this.currentJoystickId = joystickId;
			this.Redraw(true, true);
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x0001EEF4 File Offset: 0x0001D0F4
		private void OnRemoveCurrentController()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this.currentJoystickId < 0)
			{
				return;
			}
			this.RemoveController(this.currentPlayer, this.currentJoystickId);
			this.ClearVarsOnJoystickChange();
			this.Redraw(false, false);
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x0001EF29 File Offset: 0x0001D129
		private void OnMapCategorySelected(int id, bool redraw)
		{
			if (!this.initialized)
			{
				return;
			}
			this.currentMapCategoryId = id;
			if (redraw)
			{
				this.Redraw(true, true);
			}
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x0001EF46 File Offset: 0x0001D146
		public void ForceRestoreDefaults()
		{
			this.OnRestoreDefaultsConfirmed(0);
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x0001EF4F File Offset: 0x0001D14F
		public void OnRestoreDefaults()
		{
			if (!this.initialized)
			{
				return;
			}
			this.ShowRestoreDefaultsWindow();
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x0001EF60 File Offset: 0x0001D160
		private void OnScreenToggleActionPressed(InputActionEventData data)
		{
			if (!this.isOpen)
			{
				this.Open();
				return;
			}
			if (!this.initialized)
			{
				return;
			}
			if (!this.isFocused)
			{
				return;
			}
			this.Close(true);
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x0001EF8A File Offset: 0x0001D18A
		private void OnScreenOpenActionPressed(InputActionEventData data)
		{
			this.Open();
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x0001EF92 File Offset: 0x0001D192
		private void OnScreenCloseActionPressed(InputActionEventData data)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (!this.isFocused)
			{
				return;
			}
			this.Close(true);
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x0001EFB6 File Offset: 0x0001D1B6
		private void OnUniversalCancelActionPressed(InputActionEventData data)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (this._universalCancelClosesScreen)
			{
				if (this.isFocused)
				{
					this.Close(true);
					return;
				}
			}
			else if (this.isFocused)
			{
				return;
			}
			this.CloseAllWindows();
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x0001EFF1 File Offset: 0x0001D1F1
		private void OnWindowCancel(int windowId)
		{
			if (!this.initialized)
			{
				return;
			}
			if (windowId < 0)
			{
				return;
			}
			this.CloseWindow(windowId);
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x0001F008 File Offset: 0x0001D208
		private void OnRemoveElementAssignment(int windowId, ControllerMap map, ActionElementMap aem)
		{
			if (map == null || aem == null)
			{
				return;
			}
			map.DeleteElementMap(aem.id);
			this.CloseWindow(windowId);
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x000F99D8 File Offset: 0x000F7BD8
		private void OnBeginElementAssignment(InputFieldInfo fieldInfo, ControllerMap map, ActionElementMap aem, string actionName)
		{
			if (fieldInfo == null || map == null)
			{
				return;
			}
			this.pendingInputMapping = new ControlMapper.InputMapping(actionName, fieldInfo, map, aem, fieldInfo.controllerType, fieldInfo.controllerId);
			switch (fieldInfo.controllerType)
			{
			case ControllerType.Keyboard:
				this.ShowElementAssignmentPollingWindow();
				return;
			case ControllerType.Mouse:
				this.ShowElementAssignmentPollingWindow();
				return;
			case ControllerType.Joystick:
				this.ShowElementAssignmentPrePollingWindow();
				return;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x0001F025 File Offset: 0x0001D225
		private void OnControllerAssignmentConfirmed(int windowId, Player player, int controllerId)
		{
			if (windowId < 0 || player == null || controllerId < 0)
			{
				return;
			}
			this.AssignController(player, controllerId);
			this.CloseWindow(windowId);
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x000F9A44 File Offset: 0x000F7C44
		private void OnMouseAssignmentConfirmed(int windowId, Player player)
		{
			if (windowId < 0 || player == null)
			{
				return;
			}
			IList<Player> players = ReInput.players.Players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i] != player)
				{
					players[i].controllers.hasMouse = false;
				}
			}
			player.controllers.hasMouse = true;
			this.CloseWindow(windowId);
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x000F9AA4 File Offset: 0x000F7CA4
		private void OnElementAssignmentConflictReplaceConfirmed(int windowId, ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers, bool allowSwap)
		{
			if (this.currentPlayer == null || mapping == null)
			{
				return;
			}
			ElementAssignmentConflictCheck conflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
			{
				Debug.LogError("Rewired Control Mapper: Error creating conflict check!");
				this.CloseWindow(windowId);
				return;
			}
			ElementAssignmentConflictInfo elementAssignmentConflictInfo = default(ElementAssignmentConflictInfo);
			ActionElementMap actionElementMap = null;
			ActionElementMap actionElementMap2 = null;
			bool flag = false;
			if (allowSwap && mapping.aem != null && this.GetFirstElementAssignmentConflict(conflictCheck, out elementAssignmentConflictInfo, skipOtherPlayers))
			{
				flag = true;
				actionElementMap2 = new ActionElementMap(mapping.aem);
				actionElementMap = new ActionElementMap(elementAssignmentConflictInfo.elementMap);
			}
			IList<Player> allPlayers = ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				Player player = allPlayers[i];
				if (!skipOtherPlayers || player == this.currentPlayer || player == ReInput.players.SystemPlayer)
				{
					player.controllers.conflictChecking.RemoveElementAssignmentConflicts(conflictCheck);
				}
			}
			mapping.map.ReplaceOrCreateElementMap(assignment);
			if (allowSwap && flag)
			{
				int actionId = actionElementMap.actionId;
				Pole axisContribution = actionElementMap.axisContribution;
				bool invert = actionElementMap.invert;
				AxisRange axisRange = actionElementMap2.axisRange;
				ControllerElementType elementType = actionElementMap2.elementType;
				int elementIdentifierId = actionElementMap2.elementIdentifierId;
				KeyCode keyCode = actionElementMap2.keyCode;
				ModifierKeyFlags modifierKeyFlags = actionElementMap2.modifierKeyFlags;
				if (elementType == actionElementMap.elementType && elementType == ControllerElementType.Axis)
				{
					if (axisRange != actionElementMap.axisRange)
					{
						if (axisRange == AxisRange.Full)
						{
							axisRange = AxisRange.Positive;
						}
						else if (actionElementMap.axisRange == AxisRange.Full)
						{
						}
					}
				}
				else if (elementType == ControllerElementType.Axis && (actionElementMap.elementType == ControllerElementType.Button || (actionElementMap.elementType == ControllerElementType.Axis && actionElementMap.axisRange != AxisRange.Full)) && axisRange == AxisRange.Full)
				{
					axisRange = AxisRange.Positive;
				}
				if (elementType != ControllerElementType.Axis || axisRange != AxisRange.Full)
				{
					invert = false;
				}
				int num = 0;
				foreach (ActionElementMap actionElementMap3 in elementAssignmentConflictInfo.controllerMap.ElementMapsWithAction(actionId))
				{
					if (this.SwapIsSameInputRange(elementType, axisRange, axisContribution, actionElementMap3.elementType, actionElementMap3.axisRange, actionElementMap3.axisContribution))
					{
						num++;
					}
				}
				if (num < this.GetControllerInputFieldCount(mapping.controllerType))
				{
					elementAssignmentConflictInfo.controllerMap.ReplaceOrCreateElementMap(ElementAssignment.CompleteAssignment(mapping.controllerType, elementType, elementIdentifierId, axisRange, keyCode, modifierKeyFlags, actionId, axisContribution, invert));
				}
			}
			this.CloseWindow(windowId);
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x0001F042 File Offset: 0x0001D242
		private void OnElementAssignmentAddConfirmed(int windowId, ControlMapper.InputMapping mapping, ElementAssignment assignment)
		{
			if (this.currentPlayer == null || mapping == null)
			{
				return;
			}
			mapping.map.ReplaceOrCreateElementMap(assignment);
			this.CloseWindow(windowId);
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x000F9CE0 File Offset: 0x000F7EE0
		private void OnRestoreDefaultsConfirmed(int windowId)
		{
			if (this._restoreDefaultsDelegate == null)
			{
				IList<Player> players = ReInput.players.Players;
				for (int i = 0; i < players.Count; i++)
				{
					Player player = players[i];
					if (this._showControllers)
					{
						player.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);
					}
					if (this._showKeyboard)
					{
						player.controllers.maps.LoadDefaultMaps(ControllerType.Keyboard);
					}
					if (this._showMouse)
					{
						player.controllers.maps.LoadDefaultMaps(ControllerType.Mouse);
					}
				}
			}
			this.CloseWindow(windowId);
			if (this._restoreDefaultsDelegate != null)
			{
				this._restoreDefaultsDelegate();
			}
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x000F9D80 File Offset: 0x000F7F80
		private void OnAssignControllerWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			ControllerPollingInfo controllerPollingInfo = ReInput.controllers.polling.PollAllControllersOfTypeForFirstElementDown(ControllerType.Joystick);
			if (!controllerPollingInfo.success)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				return;
			}
			this.InputPollingStopped();
			if (ReInput.controllers.IsControllerAssigned(ControllerType.Joystick, controllerPollingInfo.controllerId) && !this.currentPlayer.controllers.ContainsController(ControllerType.Joystick, controllerPollingInfo.controllerId))
			{
				this.ShowControllerAssignmentConflictWindow(controllerPollingInfo.controllerId);
				return;
			}
			this.OnControllerAssignmentConfirmed(windowId, this.currentPlayer, controllerPollingInfo.controllerId);
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x000F9E5C File Offset: 0x000F805C
		private void OnElementAssignmentPrePollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (!window.timer.finished)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				ControllerType controllerType = this.pendingInputMapping.controllerType;
				ControllerPollingInfo controllerPollingInfo;
				if (controllerType > ControllerType.Mouse)
				{
					if (controllerType != ControllerType.Joystick)
					{
						throw new NotImplementedException();
					}
					if (this.currentPlayer.controllers.joystickCount == 0)
					{
						return;
					}
					controllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstButtonDown(this.pendingInputMapping.controllerType, this.currentJoystick.id);
				}
				else
				{
					controllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstButtonDown(this.pendingInputMapping.controllerType, 0);
				}
				if (!controllerPollingInfo.success)
				{
					return;
				}
			}
			this.ShowElementAssignmentPollingWindow();
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x000F9F48 File Offset: 0x000F8148
		private void OnJoystickElementAssignmentPollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
			if (this.currentPlayer.controllers.joystickCount == 0)
			{
				return;
			}
			ControllerPollingInfo pollingInfo = ReInput.controllers.polling.PollControllerForFirstElementDown(ControllerType.Joystick, this.currentJoystick.id);
			if (!pollingInfo.success)
			{
				return;
			}
			if (!this.IsAllowedAssignment(this.pendingInputMapping, pollingInfo))
			{
				return;
			}
			ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(pollingInfo);
			if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, false))
			{
				this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			this.InputPollingStopped();
			this.ShowElementAssignmentConflictWindow(elementAssignment, false);
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000FA054 File Offset: 0x000F8254
		private void OnKeyboardElementAssignmentPollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			ControllerPollingInfo pollingInfo;
			bool flag;
			ModifierKeyFlags modifierKeyFlags;
			string text;
			this.PollKeyboardForAssignment(out pollingInfo, out flag, out modifierKeyFlags, out text);
			if (flag)
			{
				window.timer.Start(this._inputAssignmentTimeout);
			}
			window.SetContentText(flag ? string.Empty : Mathf.CeilToInt(window.timer.remaining).ToString(), 2);
			window.SetContentText(text, 1);
			if (!pollingInfo.success)
			{
				return;
			}
			if (!this.IsAllowedAssignment(this.pendingInputMapping, pollingInfo))
			{
				return;
			}
			ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(pollingInfo, modifierKeyFlags);
			if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, false))
			{
				this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			this.InputPollingStopped();
			this.ShowElementAssignmentConflictWindow(elementAssignment, false);
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000FA16C File Offset: 0x000F836C
		private void OnMouseElementAssignmentPollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
			ControllerPollingInfo pollingInfo;
			if (this._ignoreMouseXAxisAssignment || this._ignoreMouseYAxisAssignment)
			{
				pollingInfo = default(ControllerPollingInfo);
				using (IEnumerator<ControllerPollingInfo> enumerator = ReInput.controllers.polling.PollControllerForAllElementsDown(ControllerType.Mouse, 0).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ControllerPollingInfo controllerPollingInfo = enumerator.Current;
						if (controllerPollingInfo.elementType != ControllerElementType.Axis || ((!this._ignoreMouseXAxisAssignment || controllerPollingInfo.elementIndex != 0) && (!this._ignoreMouseYAxisAssignment || controllerPollingInfo.elementIndex != 1)))
						{
							pollingInfo = controllerPollingInfo;
							break;
						}
					}
					goto IL_F9;
				}
			}
			pollingInfo = ReInput.controllers.polling.PollControllerForFirstElementDown(ControllerType.Mouse, 0);
			IL_F9:
			if (!pollingInfo.success)
			{
				return;
			}
			if (!this.IsAllowedAssignment(this.pendingInputMapping, pollingInfo))
			{
				return;
			}
			ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(pollingInfo);
			if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, true))
			{
				this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			this.InputPollingStopped();
			this.ShowElementAssignmentConflictWindow(elementAssignment, true);
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x000FA2F0 File Offset: 0x000F84F0
		private void OnCalibrateAxisStep1WindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingAxisCalibration == null || !this.pendingAxisCalibration.isValid)
			{
				return;
			}
			this.InputPollingStarted();
			if (!window.timer.finished)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				if (this.currentPlayer.controllers.joystickCount == 0)
				{
					return;
				}
				if (!this.pendingAxisCalibration.joystick.PollForFirstButtonDown().success)
				{
					return;
				}
			}
			this.pendingAxisCalibration.RecordZero();
			this.CloseWindow(windowId);
			this.ShowCalibrateAxisStep2Window();
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000FA3A8 File Offset: 0x000F85A8
		private void OnCalibrateAxisStep2WindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingAxisCalibration == null || !this.pendingAxisCalibration.isValid)
			{
				return;
			}
			if (!window.timer.finished)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				this.pendingAxisCalibration.RecordMinMax();
				if (this.currentPlayer.controllers.joystickCount == 0)
				{
					return;
				}
				if (!this.pendingAxisCalibration.joystick.PollForFirstButtonDown().success)
				{
					return;
				}
			}
			this.EndAxisCalibration();
			this.InputPollingStopped();
			this.CloseWindow(windowId);
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x000FA460 File Offset: 0x000F8660
		private void ShowAssignControllerWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (ReInput.controllers.joystickCount == 0)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.SetUpdateCallback(new Action<int>(this.OnAssignControllerWindowUpdate));
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.assignControllerWindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.assignControllerWindowMessage);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
			window.timer.Start(this._controllerAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x000FA540 File Offset: 0x000F8740
		private void ShowControllerAssignmentConflictWindow(int controllerId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (ReInput.controllers.joystickCount == 0)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string otherPlayerName = string.Empty;
			IList<Player> players = ReInput.players.Players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i] != this.currentPlayer && players[i].controllers.ContainsController(ControllerType.Joystick, controllerId))
				{
					otherPlayerName = this._language.GetPlayerName(players[i].id);
					break;
				}
			}
			Joystick joystick = ReInput.controllers.GetJoystick(controllerId);
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.controllerAssignmentConflictWindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetControllerAssignmentConflictWindowMessage(this._language.GetControllerName(joystick), otherPlayerName, this._language.GetPlayerName(this.currentPlayer.id)));
			UnityAction unityAction = delegate()
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.yes, delegate()
			{
				this.OnControllerAssignmentConfirmed(window.id, this.currentPlayer, controllerId);
			}, unityAction, true);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.no, unityAction, unityAction, false);
			this.windowManager.Focus(window);
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x000FA734 File Offset: 0x000F8934
		private void ShowBeginElementAssignmentReplacementWindow(InputFieldInfo fieldInfo, InputAction action, ControllerMap map, ActionElementMap aem, string actionName)
		{
			ControlMapper.GUIInputField guiinputField = this.inputGrid.GetGUIInputField(this.currentMapCategoryId, action.id, fieldInfo.axisRange, fieldInfo.controllerType, fieldInfo.intData);
			if (guiinputField == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), guiinputField.GetLabel());
			UnityAction unityAction = delegate()
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.replace, delegate()
			{
				this.OnBeginElementAssignment(fieldInfo, map, aem, actionName);
			}, unityAction, true);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.remove, delegate()
			{
				this.OnRemoveElementAssignment(window.id, map, aem);
			}, unityAction, false);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.cancel, unityAction, unityAction, false);
			this.windowManager.Focus(window);
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x0001F064 File Offset: 0x0001D264
		private void ShowCreateNewElementAssignmentWindow(InputFieldInfo fieldInfo, InputAction action, ControllerMap map, string actionName)
		{
			if (this.inputGrid.GetGUIInputField(this.currentMapCategoryId, action.id, fieldInfo.axisRange, fieldInfo.controllerType, fieldInfo.intData) == null)
			{
				return;
			}
			this.OnBeginElementAssignment(fieldInfo, map, null, actionName);
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x000FA8F4 File Offset: 0x000F8AF4
		private void ShowElementAssignmentPrePollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.elementAssignmentPrePollingWindowMessage);
			if (this.prefabs.centerStickGraphic != null)
			{
				window.AddContentImage(this.prefabs.centerStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, new Vector2(0f, 40f));
			}
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
			window.SetUpdateCallback(new Action<int>(this.OnElementAssignmentPrePollingWindowUpdate));
			window.timer.Start(this._preInputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x000FAA04 File Offset: 0x000F8C04
		private void ShowElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			switch (this.pendingInputMapping.controllerType)
			{
			case ControllerType.Keyboard:
				this.ShowKeyboardElementAssignmentPollingWindow();
				return;
			case ControllerType.Mouse:
				if (this.currentPlayer.controllers.hasMouse)
				{
					this.ShowMouseElementAssignmentPollingWindow();
					return;
				}
				this.ShowMouseAssignmentConflictWindow();
				return;
			case ControllerType.Joystick:
				this.ShowJoystickElementAssignmentPollingWindow();
				return;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x000FAA70 File Offset: 0x000F8C70
		private void ShowJoystickElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string text = (this.pendingInputMapping.axisRange == AxisRange.Full && this._showFullAxisInputFields && !this._showSplitAxisInputFields) ? this._language.GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(this.pendingInputMapping.actionName) : this._language.GetJoystickElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName);
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), text);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
			window.SetUpdateCallback(new Action<int>(this.OnJoystickElementAssignmentPollingWindowUpdate));
			window.timer.Start(this._inputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x000FAB84 File Offset: 0x000F8D84
		private void ShowKeyboardElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetKeyboardElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName));
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -(window.GetContentTextHeight(0) + 50f)), "");
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
			window.SetUpdateCallback(new Action<int>(this.OnKeyboardElementAssignmentPollingWindowUpdate));
			window.timer.Start(this._inputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x000FAC9C File Offset: 0x000F8E9C
		private void ShowMouseElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string text = (this.pendingInputMapping.axisRange == AxisRange.Full && this._showFullAxisInputFields && !this._showSplitAxisInputFields) ? this._language.GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(this.pendingInputMapping.actionName) : this._language.GetMouseElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName);
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), text);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
			window.SetUpdateCallback(new Action<int>(this.OnMouseElementAssignmentPollingWindowUpdate));
			window.timer.Start(this._inputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x000FADB0 File Offset: 0x000F8FB0
		private void ShowElementAssignmentConflictWindow(ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			bool flag = this.IsBlockingAssignmentConflict(this.pendingInputMapping, assignment, skipOtherPlayers);
			string text = flag ? this._language.GetElementAlreadyInUseBlocked(this.pendingInputMapping.elementName) : this._language.GetElementAlreadyInUseCanReplace(this.pendingInputMapping.elementName, this._allowElementAssignmentConflicts);
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.elementAssignmentConflictWindowMessage);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), text);
			UnityAction unityAction = delegate()
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			if (flag)
			{
				window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.okay, unityAction, unityAction, true);
			}
			else
			{
				window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.replace, delegate()
				{
					this.OnElementAssignmentConflictReplaceConfirmed(window.id, this.pendingInputMapping, assignment, skipOtherPlayers, false);
				}, unityAction, true);
				if (this._allowElementAssignmentConflicts)
				{
					window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.add, delegate()
					{
						this.OnElementAssignmentAddConfirmed(window.id, this.pendingInputMapping, assignment);
					}, unityAction, false);
				}
				else if (this.ShowSwapButton(window.id, this.pendingInputMapping, assignment, skipOtherPlayers))
				{
					window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.swap, delegate()
					{
						this.OnElementAssignmentConflictReplaceConfirmed(window.id, this.pendingInputMapping, assignment, skipOtherPlayers, true);
					}, unityAction, false);
				}
				window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.cancel, unityAction, unityAction, false);
			}
			this.windowManager.Focus(window);
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x000FB028 File Offset: 0x000F9228
		private void ShowMouseAssignmentConflictWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string otherPlayerName = string.Empty;
			IList<Player> players = ReInput.players.Players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i] != this.currentPlayer && players[i].controllers.hasMouse)
				{
					otherPlayerName = this._language.GetPlayerName(players[i].id);
					break;
				}
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.mouseAssignmentConflictWindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetMouseAssignmentConflictWindowMessage(otherPlayerName, this._language.GetPlayerName(this.currentPlayer.id)));
			UnityAction unityAction = delegate()
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.yes, delegate()
			{
				this.OnMouseAssignmentConfirmed(window.id, this.currentPlayer);
			}, unityAction, true);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.no, unityAction, unityAction, false);
			this.windowManager.Focus(window);
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x000FB1E0 File Offset: 0x000F93E0
		private void ShowCalibrateControllerWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this.currentPlayer.controllers.joystickCount == 0)
			{
				return;
			}
			CalibrationWindow calibrationWindow = this.OpenWindow(this.prefabs.calibrationWindow, "CalibrationWindow", true) as CalibrationWindow;
			if (calibrationWindow == null)
			{
				return;
			}
			Joystick currentJoystick = this.currentJoystick;
			calibrationWindow.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.calibrateControllerWindowTitle);
			calibrationWindow.SetJoystick(this.currentPlayer.id, currentJoystick);
			calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Done, new Action<int>(this.CloseWindow));
			calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Calibrate, new Action<int>(this.StartAxisCalibration));
			calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Cancel, new Action<int>(this.CloseWindow));
			this.windowManager.Focus(calibrationWindow);
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x000FB2B0 File Offset: 0x000F94B0
		private void ShowCalibrateAxisStep1Window()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.OpenWindow(false);
			if (window == null)
			{
				return;
			}
			if (this.pendingAxisCalibration == null)
			{
				return;
			}
			Joystick joystick = this.pendingAxisCalibration.joystick;
			if (joystick.axisCount == 0)
			{
				return;
			}
			int axisIndex = this.pendingAxisCalibration.axisIndex;
			if (axisIndex < 0 || axisIndex >= joystick.axisCount)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.calibrateAxisStep1WindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetCalibrateAxisStep1WindowMessage(this._language.GetElementIdentifierName(joystick, joystick.AxisElementIdentifiers[axisIndex].id, AxisRange.Full)));
			if (this.prefabs.centerStickGraphic != null)
			{
				window.AddContentImage(this.prefabs.centerStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, new Vector2(0f, 40f));
			}
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
			window.SetUpdateCallback(new Action<int>(this.OnCalibrateAxisStep1WindowUpdate));
			window.timer.Start(this._axisCalibrationTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x000FB418 File Offset: 0x000F9618
		private void ShowCalibrateAxisStep2Window()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.OpenWindow(false);
			if (window == null)
			{
				return;
			}
			if (this.pendingAxisCalibration == null)
			{
				return;
			}
			Joystick joystick = this.pendingAxisCalibration.joystick;
			if (joystick.axisCount == 0)
			{
				return;
			}
			int axisIndex = this.pendingAxisCalibration.axisIndex;
			if (axisIndex < 0 || axisIndex >= joystick.axisCount)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.calibrateAxisStep2WindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetCalibrateAxisStep2WindowMessage(this._language.GetElementIdentifierName(joystick, joystick.AxisElementIdentifiers[axisIndex].id, AxisRange.Full)));
			if (this.prefabs.moveStickGraphic != null)
			{
				window.AddContentImage(this.prefabs.moveStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, new Vector2(0f, 40f));
			}
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
			window.SetUpdateCallback(new Action<int>(this.OnCalibrateAxisStep2WindowUpdate));
			window.timer.Start(this._axisCalibrationTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x000FB580 File Offset: 0x000F9780
		private void ShowEditInputBehaviorsWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this._inputBehaviorSettings == null)
			{
				return;
			}
			InputBehaviorWindow inputBehaviorWindow = this.OpenWindow(this.prefabs.inputBehaviorsWindow, "EditInputBehaviorsWindow", true) as InputBehaviorWindow;
			if (inputBehaviorWindow == null)
			{
				return;
			}
			inputBehaviorWindow.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.inputBehaviorSettingsWindowTitle);
			inputBehaviorWindow.SetData(this.currentPlayer.id, this._inputBehaviorSettings);
			inputBehaviorWindow.SetButtonCallback(InputBehaviorWindow.ButtonIdentifier.Done, new Action<int>(this.CloseWindow));
			inputBehaviorWindow.SetButtonCallback(InputBehaviorWindow.ButtonIdentifier.Cancel, new Action<int>(this.CloseWindow));
			this.windowManager.Focus(inputBehaviorWindow);
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x000FB630 File Offset: 0x000F9830
		private void ShowRestoreDefaultsWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			this.OpenModal(this._language.restoreDefaultsWindowTitle, this._language.restoreDefaultsWindowMessage, this._language.yes, new Action<int>(this.OnRestoreDefaultsConfirmed), this._language.no, new Action<int>(this.OnWindowCancel), true);
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x000FB694 File Offset: 0x000F9894
		private void CreateInputGrid()
		{
			this.InitializeInputGrid();
			this.CreateHeaderLabels();
			this.CreateActionLabelColumn();
			this.CreateKeyboardInputFieldColumn();
			this.CreateMouseInputFieldColumn();
			this.CreateControllerInputFieldColumn();
			this.CreateInputActionLabels();
			this.CreateInputFields();
			this.inputGrid.HideAll();
			this.ResetInputGridScrollBar();
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x000FB6E4 File Offset: 0x000F98E4
		private void InitializeInputGrid()
		{
			if (this.inputGrid == null)
			{
				this.inputGrid = new ControlMapper.InputGrid();
			}
			else
			{
				this.inputGrid.ClearAll();
			}
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null && mappingSet.isValid)
				{
					InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(mappingSet.mapCategoryId);
					if (mapCategory != null && mapCategory.userAssignable)
					{
						this.inputGrid.AddMapCategory(mappingSet.mapCategoryId);
						if (mappingSet.actionListMode == ControlMapper.MappingSet.ActionListMode.ActionCategory)
						{
							IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
							for (int j = 0; j < actionCategoryIds.Count; j++)
							{
								int num = actionCategoryIds[j];
								InputCategory actionCategory = ReInput.mapping.GetActionCategory(num);
								if (actionCategory != null && actionCategory.userAssignable)
								{
									this.inputGrid.AddActionCategory(mappingSet.mapCategoryId, num);
									foreach (InputAction inputAction in ReInput.mapping.UserAssignableActionsInCategory(num))
									{
										if (inputAction.type == InputActionType.Axis)
										{
											if (this._showFullAxisInputFields)
											{
												this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Full);
											}
											if (this._showSplitAxisInputFields)
											{
												this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Positive);
												this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Negative);
											}
										}
										else if (inputAction.type == InputActionType.Button)
										{
											this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Positive);
										}
									}
								}
							}
						}
						else
						{
							IList<int> actionIds = mappingSet.actionIds;
							for (int k = 0; k < actionIds.Count; k++)
							{
								InputAction action = ReInput.mapping.GetAction(actionIds[k]);
								if (action != null)
								{
									if (action.type == InputActionType.Axis)
									{
										if (this._showFullAxisInputFields)
										{
											this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Full);
										}
										if (this._showSplitAxisInputFields)
										{
											this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Positive);
											this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Negative);
										}
									}
									else if (action.type == InputActionType.Button)
									{
										this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Positive);
									}
								}
							}
						}
					}
				}
			}
			this.references.inputGridInnerGroup.GetComponent<HorizontalLayoutGroup>().spacing = (float)this._inputColumnSpacing;
			this.references.inputGridLayoutElement.flexibleWidth = 0f;
			this.references.inputGridLayoutElement.preferredWidth = (float)this.inputGridWidth;
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x000FB9A0 File Offset: 0x000F9BA0
		private void RefreshInputGridStructure()
		{
			if (this.currentMappingSet == null)
			{
				return;
			}
			this.inputGrid.HideAll();
			this.inputGrid.Show(this.currentMappingSet.mapCategoryId);
			this.references.inputGridInnerGroup.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.inputGrid.GetColumnHeight(this.currentMappingSet.mapCategoryId));
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x000FBA04 File Offset: 0x000F9C04
		private void CreateHeaderLabels()
		{
			this.references.inputGridHeader1 = this.CreateNewColumnGroup("ActionsHeader", this.references.inputGridHeadersGroup, this._actionLabelWidth).transform;
			this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.actionColumnLabel, this.references.inputGridHeader1, Vector2.zero);
			if (this._showKeyboard)
			{
				this.references.inputGridHeader2 = this.CreateNewColumnGroup("KeybordHeader", this.references.inputGridHeadersGroup, this._keyboardColMaxWidth).transform;
				this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.keyboardColumnLabel, this.references.inputGridHeader2, Vector2.zero).SetTextAlignment(TextAnchor.MiddleCenter);
			}
			if (this._showMouse)
			{
				this.references.inputGridHeader3 = this.CreateNewColumnGroup("MouseHeader", this.references.inputGridHeadersGroup, this._mouseColMaxWidth).transform;
				this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.mouseColumnLabel, this.references.inputGridHeader3, Vector2.zero).SetTextAlignment(TextAnchor.MiddleCenter);
			}
			if (this._showControllers)
			{
				this.references.inputGridHeader4 = this.CreateNewColumnGroup("ControllerHeader", this.references.inputGridHeadersGroup, this._controllerColMaxWidth).transform;
				this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.controllerColumnLabel, this.references.inputGridHeader4, Vector2.zero).SetTextAlignment(TextAnchor.MiddleCenter);
			}
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x000FBB9C File Offset: 0x000F9D9C
		private void CreateActionLabelColumn()
		{
			Transform transform = this.CreateNewColumnGroup("ActionLabelColumn", this.references.inputGridInnerGroup, this._actionLabelWidth).transform;
			this.references.inputGridActionColumn = transform;
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x0001F09D File Offset: 0x0001D29D
		private void CreateKeyboardInputFieldColumn()
		{
			if (!this._showKeyboard)
			{
				return;
			}
			this.CreateInputFieldColumn("KeyboardColumn", ControllerType.Keyboard, this._keyboardColMaxWidth, this._keyboardInputFieldCount, true);
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x0001F0C1 File Offset: 0x0001D2C1
		private void CreateMouseInputFieldColumn()
		{
			if (!this._showMouse)
			{
				return;
			}
			this.CreateInputFieldColumn("MouseColumn", ControllerType.Mouse, this._mouseColMaxWidth, this._mouseInputFieldCount, false);
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x0001F0E5 File Offset: 0x0001D2E5
		private void CreateControllerInputFieldColumn()
		{
			if (!this._showControllers)
			{
				return;
			}
			this.CreateInputFieldColumn("ControllerColumn", ControllerType.Joystick, this._controllerColMaxWidth, this._controllerInputFieldCount, false);
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x000FBBD8 File Offset: 0x000F9DD8
		private void CreateInputFieldColumn(string name, ControllerType controllerType, int maxWidth, int cols, bool disableFullAxis)
		{
			Transform transform = this.CreateNewColumnGroup(name, this.references.inputGridInnerGroup, maxWidth).transform;
			switch (controllerType)
			{
			case ControllerType.Keyboard:
				this.references.inputGridKeyboardColumn = transform;
				return;
			case ControllerType.Mouse:
				this.references.inputGridMouseColumn = transform;
				return;
			case ControllerType.Joystick:
				this.references.inputGridControllerColumn = transform;
				return;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x000FBC40 File Offset: 0x000F9E40
		private void CreateInputActionLabels()
		{
			Transform inputGridActionColumn = this.references.inputGridActionColumn;
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null && mappingSet.isValid)
				{
					int num = 0;
					if (mappingSet.actionListMode == ControlMapper.MappingSet.ActionListMode.ActionCategory)
					{
						int num2 = 0;
						IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
						for (int j = 0; j < actionCategoryIds.Count; j++)
						{
							InputCategory actionCategory = ReInput.mapping.GetActionCategory(actionCategoryIds[j]);
							if (actionCategory != null && actionCategory.userAssignable && this.CountIEnumerable<InputAction>(ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id)) != 0)
							{
								if (this._showActionCategoryLabels)
								{
									if (num2 > 0)
									{
										num -= this._inputRowCategorySpacing;
									}
									ControlMapper.GUILabel guilabel = this.CreateLabel(this._language.GetActionCategoryName(actionCategory.id), inputGridActionColumn, new Vector2(0f, (float)num));
									guilabel.SetFontStyle(FontStyle.Bold);
									guilabel.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
									this.inputGrid.AddActionCategoryLabel(mappingSet.mapCategoryId, actionCategory.id, guilabel);
									num -= this._inputRowHeight;
								}
								foreach (InputAction inputAction in ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id, true))
								{
									if (inputAction.type == InputActionType.Axis)
									{
										if (this._showFullAxisInputFields)
										{
											ControlMapper.GUILabel guilabel2 = this.CreateLabel(this._language.GetActionName(inputAction.id, AxisRange.Full), inputGridActionColumn, new Vector2(0f, (float)num));
											guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
											this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Full, guilabel2);
											num -= this._inputRowHeight;
										}
										if (this._showSplitAxisInputFields)
										{
											string actionName = this._language.GetActionName(inputAction.id, AxisRange.Positive);
											ControlMapper.GUILabel guilabel2 = this.CreateLabel(actionName, inputGridActionColumn, new Vector2(0f, (float)num));
											guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
											this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Positive, guilabel2);
											num -= this._inputRowHeight;
											string actionName2 = this._language.GetActionName(inputAction.id, AxisRange.Negative);
											guilabel2 = this.CreateLabel(actionName2, inputGridActionColumn, new Vector2(0f, (float)num));
											guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
											this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Negative, guilabel2);
											num -= this._inputRowHeight;
										}
									}
									else if (inputAction.type == InputActionType.Button)
									{
										ControlMapper.GUILabel guilabel2 = this.CreateLabel(this._language.GetActionName(inputAction.id), inputGridActionColumn, new Vector2(0f, (float)num));
										guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
										this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Positive, guilabel2);
										num -= this._inputRowHeight;
									}
								}
								num2++;
							}
						}
					}
					else
					{
						IList<int> actionIds = mappingSet.actionIds;
						for (int k = 0; k < actionIds.Count; k++)
						{
							InputAction action = ReInput.mapping.GetAction(actionIds[k]);
							if (action != null && action.userAssignable)
							{
								InputCategory actionCategory2 = ReInput.mapping.GetActionCategory(action.categoryId);
								if (actionCategory2 != null && actionCategory2.userAssignable)
								{
									if (action.type == InputActionType.Axis)
									{
										if (this._showFullAxisInputFields)
										{
											ControlMapper.GUILabel guilabel3 = this.CreateLabel(this._language.GetActionName(action.id, AxisRange.Full), inputGridActionColumn, new Vector2(0f, (float)num));
											guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
											this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Full, guilabel3);
											num -= this._inputRowHeight;
										}
										if (this._showSplitAxisInputFields)
										{
											ControlMapper.GUILabel guilabel3 = this.CreateLabel(this._language.GetActionName(action.id, AxisRange.Positive), inputGridActionColumn, new Vector2(0f, (float)num));
											guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
											this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Positive, guilabel3);
											num -= this._inputRowHeight;
											guilabel3 = this.CreateLabel(this._language.GetActionName(action.id, AxisRange.Negative), inputGridActionColumn, new Vector2(0f, (float)num));
											guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
											this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Negative, guilabel3);
											num -= this._inputRowHeight;
										}
									}
									else if (action.type == InputActionType.Button)
									{
										ControlMapper.GUILabel guilabel3 = this.CreateLabel(this._language.GetActionName(action.id), inputGridActionColumn, new Vector2(0f, (float)num));
										guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
										this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Positive, guilabel3);
										num -= this._inputRowHeight;
									}
								}
							}
						}
					}
					this.inputGrid.SetColumnHeight(mappingSet.mapCategoryId, (float)(-(float)num));
				}
			}
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x000FC1CC File Offset: 0x000FA3CC
		private void CreateInputFields()
		{
			if (this._showControllers)
			{
				this.CreateInputFields(this.references.inputGridControllerColumn, ControllerType.Joystick, this._controllerColMaxWidth, this._controllerInputFieldCount, false);
			}
			if (this._showKeyboard)
			{
				this.CreateInputFields(this.references.inputGridKeyboardColumn, ControllerType.Keyboard, this._keyboardColMaxWidth, this._keyboardInputFieldCount, true);
			}
			if (this._showMouse)
			{
				this.CreateInputFields(this.references.inputGridMouseColumn, ControllerType.Mouse, this._mouseColMaxWidth, this._mouseInputFieldCount, false);
			}
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x000FC250 File Offset: 0x000FA450
		private void CreateInputFields(Transform columnXform, ControllerType controllerType, int maxWidth, int cols, bool disableFullAxis)
		{
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null && mappingSet.isValid)
				{
					int fieldWidth = maxWidth / cols;
					int num = 0;
					int num2 = 0;
					if (mappingSet.actionListMode == ControlMapper.MappingSet.ActionListMode.ActionCategory)
					{
						IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
						for (int j = 0; j < actionCategoryIds.Count; j++)
						{
							InputCategory actionCategory = ReInput.mapping.GetActionCategory(actionCategoryIds[j]);
							if (actionCategory != null && actionCategory.userAssignable && this.CountIEnumerable<InputAction>(ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id)) != 0)
							{
								if (this._showActionCategoryLabels)
								{
									num -= ((num2 > 0) ? (this._inputRowHeight + this._inputRowCategorySpacing) : this._inputRowHeight);
								}
								foreach (InputAction inputAction in ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id, true))
								{
									if (inputAction.type == InputActionType.Axis)
									{
										if (this._showFullAxisInputFields)
										{
											this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Full, controllerType, cols, fieldWidth, ref num, disableFullAxis);
										}
										if (this._showSplitAxisInputFields)
										{
											this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Positive, controllerType, cols, fieldWidth, ref num, false);
											this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Negative, controllerType, cols, fieldWidth, ref num, false);
										}
									}
									else if (inputAction.type == InputActionType.Button)
									{
										this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Positive, controllerType, cols, fieldWidth, ref num, false);
									}
									num2++;
								}
							}
						}
					}
					else
					{
						IList<int> actionIds = mappingSet.actionIds;
						for (int k = 0; k < actionIds.Count; k++)
						{
							InputAction action = ReInput.mapping.GetAction(actionIds[k]);
							if (action != null && action.userAssignable)
							{
								InputCategory actionCategory2 = ReInput.mapping.GetActionCategory(action.categoryId);
								if (actionCategory2 != null && actionCategory2.userAssignable)
								{
									if (action.type == InputActionType.Axis)
									{
										if (this._showFullAxisInputFields)
										{
											this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Full, controllerType, cols, fieldWidth, ref num, disableFullAxis);
										}
										if (this._showSplitAxisInputFields)
										{
											this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Positive, controllerType, cols, fieldWidth, ref num, false);
											this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Negative, controllerType, cols, fieldWidth, ref num, false);
										}
									}
									else if (action.type == InputActionType.Button)
									{
										this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Positive, controllerType, cols, fieldWidth, ref num, false);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x000FC504 File Offset: 0x000FA704
		private void CreateInputFieldSet(Transform parent, int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, int cols, int fieldWidth, ref int yPos, bool disableFullAxis)
		{
			GameObject gameObject = this.CreateNewGUIObject("FieldLayoutGroup", parent, new Vector2(0f, (float)yPos));
			HorizontalLayoutGroup horizontalLayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.padding = this._inputRowPadding;
			horizontalLayoutGroup.spacing = (float)this._inputRowFieldSpacing;
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 1f);
			component.anchorMax = new Vector2(1f, 1f);
			component.pivot = new Vector2(0f, 1f);
			component.sizeDelta = Vector2.zero;
			component.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)this._inputRowHeight);
			this.inputGrid.AddInputFieldSet(mapCategoryId, action, axisRange, controllerType, gameObject);
			for (int i = 0; i < cols; i++)
			{
				int num = (axisRange == AxisRange.Full) ? this._invertToggleWidth : 0;
				ControlMapper.GUIInputField guiinputField = this.CreateInputField(horizontalLayoutGroup.transform, Vector2.zero, "", action.id, axisRange, controllerType, i);
				guiinputField.SetFirstChildObjectWidth(ControlMapper.LayoutElementSizeType.PreferredSize, fieldWidth - num);
				this.inputGrid.AddInputField(mapCategoryId, action, axisRange, controllerType, i, guiinputField);
				if (axisRange == AxisRange.Full)
				{
					if (!disableFullAxis)
					{
						ControlMapper.GUIToggle guitoggle = this.CreateToggle(this.prefabs.inputGridFieldInvertToggle, horizontalLayoutGroup.transform, Vector2.zero, "", action.id, axisRange, controllerType, i);
						guitoggle.SetFirstChildObjectWidth(ControlMapper.LayoutElementSizeType.MinSize, num);
						guiinputField.AddToggle(guitoggle);
					}
					else
					{
						guiinputField.SetInteractible(false, false, true);
					}
				}
			}
			yPos -= this._inputRowHeight;
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x000FC680 File Offset: 0x000FA880
		private void PopulateInputFields()
		{
			this.inputGrid.InitializeFields(this.currentMapCategoryId);
			if (this.currentPlayer == null)
			{
				return;
			}
			this.inputGrid.SetFieldsActive(this.currentMapCategoryId, true);
			foreach (ControlMapper.InputActionSet actionSet in this.inputGrid.GetActionSets(this.currentMapCategoryId))
			{
				if (this._showKeyboard)
				{
					ControllerType controllerType = ControllerType.Keyboard;
					int controllerId = 0;
					int layoutId = this._keyboardMapDefaultLayout;
					int maxFields = this._keyboardInputFieldCount;
					ControllerMap controllerMapOrCreateNew = this.GetControllerMapOrCreateNew(controllerType, controllerId, layoutId);
					this.PopulateInputFieldGroup(actionSet, controllerMapOrCreateNew, controllerType, controllerId, maxFields);
				}
				if (this._showMouse)
				{
					ControllerType controllerType = ControllerType.Mouse;
					int controllerId = 0;
					int layoutId = this._mouseMapDefaultLayout;
					int maxFields = this._mouseInputFieldCount;
					ControllerMap controllerMapOrCreateNew2 = this.GetControllerMapOrCreateNew(controllerType, controllerId, layoutId);
					if (this.currentPlayer.controllers.hasMouse)
					{
						this.PopulateInputFieldGroup(actionSet, controllerMapOrCreateNew2, controllerType, controllerId, maxFields);
					}
				}
				if (this.isJoystickSelected && this.currentPlayer.controllers.joystickCount > 0)
				{
					ControllerType controllerType = ControllerType.Joystick;
					int controllerId = this.currentJoystick.id;
					int layoutId = this._joystickMapDefaultLayout;
					int maxFields = this._controllerInputFieldCount;
					ControllerMap controllerMapOrCreateNew3 = this.GetControllerMapOrCreateNew(controllerType, controllerId, layoutId);
					this.PopulateInputFieldGroup(actionSet, controllerMapOrCreateNew3, controllerType, controllerId, maxFields);
				}
				else
				{
					this.DisableInputFieldGroup(actionSet, ControllerType.Joystick, this._controllerInputFieldCount);
				}
			}
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x000FC7E4 File Offset: 0x000FA9E4
		private void PopulateInputFieldGroup(ControlMapper.InputActionSet actionSet, ControllerMap controllerMap, ControllerType controllerType, int controllerId, int maxFields)
		{
			if (controllerMap == null)
			{
				return;
			}
			int num = 0;
			this.inputGrid.SetFixedFieldData(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId);
			foreach (ActionElementMap actionElementMap in controllerMap.ElementMapsWithAction(actionSet.actionId))
			{
				if (actionElementMap.elementType == ControllerElementType.Button)
				{
					if (actionSet.axisRange == AxisRange.Full)
					{
						continue;
					}
					if (actionSet.axisRange == AxisRange.Positive)
					{
						if (actionElementMap.axisContribution == Pole.Negative)
						{
							continue;
						}
					}
					else if (actionSet.axisRange == AxisRange.Negative && actionElementMap.axisContribution == Pole.Positive)
					{
						continue;
					}
					this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, this._language.GetElementIdentifierName(actionElementMap), false);
				}
				else if (actionElementMap.elementType == ControllerElementType.Axis)
				{
					if (actionSet.axisRange == AxisRange.Full)
					{
						if (actionElementMap.axisRange != AxisRange.Full)
						{
							continue;
						}
						this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, this._language.GetElementIdentifierName(actionElementMap), actionElementMap.invert);
					}
					else if (actionSet.axisRange == AxisRange.Positive)
					{
						if ((actionElementMap.axisRange == AxisRange.Full && ReInput.mapping.GetAction(actionSet.actionId).type != InputActionType.Button) || actionElementMap.axisContribution == Pole.Negative)
						{
							continue;
						}
						this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, this._language.GetElementIdentifierName(actionElementMap), false);
					}
					else if (actionSet.axisRange == AxisRange.Negative)
					{
						if (actionElementMap.axisRange == AxisRange.Full || actionElementMap.axisContribution == Pole.Positive)
						{
							continue;
						}
						this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, this._language.GetElementIdentifierName(actionElementMap), false);
					}
				}
				num++;
				if (num > maxFields)
				{
					break;
				}
			}
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x000FCA04 File Offset: 0x000FAC04
		private void DisableInputFieldGroup(ControlMapper.InputActionSet actionSet, ControllerType controllerType, int fieldCount)
		{
			for (int i = 0; i < fieldCount; i++)
			{
				ControlMapper.GUIInputField guiinputField = this.inputGrid.GetGUIInputField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, i);
				if (guiinputField != null)
				{
					guiinputField.SetInteractible(false, false);
				}
			}
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x000FCA48 File Offset: 0x000FAC48
		private void ResetInputGridScrollBar()
		{
			this.references.inputGridInnerGroup.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			this.references.inputGridVScrollbar.value = 1f;
			this.references.inputGridScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x000FCA98 File Offset: 0x000FAC98
		private void CreateLayout()
		{
			this.references.playersGroup.gameObject.SetActive(this.showPlayers);
			this.references.controllerGroup.gameObject.SetActive(this._showControllers);
			this.references.assignedControllersGroup.gameObject.SetActive(this._showControllers && this.ShowAssignedControllers());
			this.references.settingsAndMapCategoriesGroup.gameObject.SetActive(this.showSettings || this.showMapCategories);
			this.references.settingsGroup.gameObject.SetActive(this.showSettings);
			this.references.mapCategoriesGroup.gameObject.SetActive(this.showMapCategories);
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x0001F109 File Offset: 0x0001D309
		private void Draw()
		{
			this.DrawPlayersGroup();
			this.DrawControllersGroup();
			this.DrawSettingsGroup();
			this.DrawMapCategoriesGroup();
			this.DrawWindowButtonsGroup();
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x000FCB60 File Offset: 0x000FAD60
		private void DrawPlayersGroup()
		{
			if (!this.showPlayers)
			{
				return;
			}
			this.references.playersGroup.labelText = this._language.playersGroupLabel;
			this.references.playersGroup.SetLabelActive(this._showPlayersGroupLabel);
			for (int i = 0; i < this.playerCount; i++)
			{
				Player player = ReInput.players.GetPlayer(i);
				if (player != null)
				{
					ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(UITools.InstantiateGUIObject<ButtonInfo>(this.prefabs.button, this.references.playersGroup.content, "Player" + i.ToString() + "Button"));
					guibutton.SetLabel(this._language.GetPlayerName(player.id));
					guibutton.SetButtonInfoData("PlayerSelection", player.id);
					guibutton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
					guibutton.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
					this.playerButtons.Add(guibutton);
				}
			}
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x000FCC6C File Offset: 0x000FAE6C
		private void DrawControllersGroup()
		{
			if (!this._showControllers)
			{
				return;
			}
			this.references.controllerSettingsGroup.labelText = this._language.controllerSettingsGroupLabel;
			this.references.controllerSettingsGroup.SetLabelActive(this._showControllerGroupLabel);
			this.references.controllerNameLabel.gameObject.SetActive(this._showControllerNameLabel);
			this.references.controllerGroupLabelGroup.gameObject.SetActive(this._showControllerGroupLabel || this._showControllerNameLabel);
			if (this.ShowAssignedControllers())
			{
				this.references.assignedControllersGroup.labelText = this._language.assignedControllersGroupLabel;
				this.references.assignedControllersGroup.SetLabelActive(this._showAssignedControllersGroupLabel);
			}
			this.references.removeControllerButton.GetComponent<ButtonInfo>().text.text = this._language.removeControllerButtonLabel;
			this.references.calibrateControllerButton.GetComponent<ButtonInfo>().text.text = this._language.calibrateControllerButtonLabel;
			this.references.assignControllerButton.GetComponent<ButtonInfo>().text.text = this._language.assignControllerButtonLabel;
			ControlMapper.GUIButton guibutton = this.CreateButton(this._language.none, this.references.assignedControllersGroup.content, Vector2.zero);
			guibutton.SetInteractible(false, false, true);
			this.assignedControllerButtonsPlaceholder = guibutton;
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x000FCDD4 File Offset: 0x000FAFD4
		private void DrawSettingsGroup()
		{
			if (!this.showSettings)
			{
				return;
			}
			this.references.settingsGroup.labelText = this._language.settingsGroupLabel;
			this.references.settingsGroup.SetLabelActive(this._showSettingsGroupLabel);
			ControlMapper.GUIButton guibutton = this.CreateButton(this._language.inputBehaviorSettingsButtonLabel, this.references.settingsGroup.content, Vector2.zero);
			this.miscInstantiatedObjects.Add(guibutton.gameObject);
			guibutton.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
			guibutton.SetButtonInfoData("EditInputBehaviors", 0);
			guibutton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x000FCE88 File Offset: 0x000FB088
		private void DrawMapCategoriesGroup()
		{
			if (!this.showMapCategories)
			{
				return;
			}
			if (this._mappingSets == null)
			{
				return;
			}
			this.references.mapCategoriesGroup.labelText = this._language.mapCategoriesGroupLabel;
			this.references.mapCategoriesGroup.SetLabelActive(this._showMapCategoriesGroupLabel);
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null)
				{
					InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(mappingSet.mapCategoryId);
					if (mapCategory != null)
					{
						ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(UITools.InstantiateGUIObject<ButtonInfo>(this.prefabs.button, this.references.mapCategoriesGroup.content, mapCategory.name + "Button"));
						guibutton.SetLabel(this._language.GetMapCategoryName(mapCategory.id));
						guibutton.SetButtonInfoData("MapCategorySelection", mapCategory.id);
						guibutton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
						guibutton.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
						this.mapCategoryButtons.Add(guibutton);
					}
				}
			}
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x000FCFAC File Offset: 0x000FB1AC
		private void DrawWindowButtonsGroup()
		{
			this.references.doneButton.GetComponent<ButtonInfo>().text.text = this._language.doneButtonLabel;
			this.references.restoreDefaultsButton.GetComponent<ButtonInfo>().text.text = this._language.restoreDefaultsButtonLabel;
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x0001F129 File Offset: 0x0001D329
		private void Redraw(bool listsChanged, bool playTransitions)
		{
			this.RedrawPlayerGroup(playTransitions);
			this.RedrawControllerGroup();
			this.RedrawMapCategoriesGroup(playTransitions);
			this.RedrawInputGrid(listsChanged);
			if (this.currentUISelection == null || !this.currentUISelection.activeInHierarchy)
			{
				this.RestoreLastUISelection();
			}
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x000FD004 File Offset: 0x000FB204
		private void RedrawPlayerGroup(bool playTransitions)
		{
			if (!this.showPlayers)
			{
				return;
			}
			for (int i = 0; i < this.playerButtons.Count; i++)
			{
				bool state = this.currentPlayerId != this.playerButtons[i].buttonInfo.intData;
				this.playerButtons[i].SetInteractible(state, playTransitions);
			}
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x000FD068 File Offset: 0x000FB268
		private void RedrawControllerGroup()
		{
			int num = -1;
			this.references.controllerNameLabel.text = this._language.none;
			UITools.SetInteractable(this.references.removeControllerButton, false, false);
			UITools.SetInteractable(this.references.assignControllerButton, false, false);
			UITools.SetInteractable(this.references.calibrateControllerButton, false, false);
			if (this.ShowAssignedControllers())
			{
				foreach (ControlMapper.GUIButton guibutton in this.assignedControllerButtons)
				{
					if (!(guibutton.gameObject == null))
					{
						if (this.currentUISelection == guibutton.gameObject)
						{
							num = guibutton.buttonInfo.intData;
						}
						UnityEngine.Object.Destroy(guibutton.gameObject);
					}
				}
				this.assignedControllerButtons.Clear();
				this.assignedControllerButtonsPlaceholder.SetActive(true);
			}
			Player player = ReInput.players.GetPlayer(this.currentPlayerId);
			if (player == null)
			{
				return;
			}
			if (this.ShowAssignedControllers())
			{
				if (player.controllers.joystickCount > 0)
				{
					this.assignedControllerButtonsPlaceholder.SetActive(false);
				}
				foreach (Joystick joystick in player.controllers.Joysticks)
				{
					ControlMapper.GUIButton guibutton2 = this.CreateButton(this._language.GetControllerName(joystick), this.references.assignedControllersGroup.content, Vector2.zero);
					guibutton2.SetButtonInfoData("AssignedControllerSelection", joystick.id);
					guibutton2.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
					guibutton2.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
					this.assignedControllerButtons.Add(guibutton2);
					if (joystick.id == this.currentJoystickId)
					{
						guibutton2.SetInteractible(false, true);
					}
				}
				if (player.controllers.joystickCount > 0 && !this.isJoystickSelected)
				{
					this.currentJoystickId = player.controllers.Joysticks[0].id;
					this.assignedControllerButtons[0].SetInteractible(false, false);
				}
				if (num < 0)
				{
					goto IL_2B0;
				}
				using (List<ControlMapper.GUIButton>.Enumerator enumerator = this.assignedControllerButtons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ControlMapper.GUIButton guibutton3 = enumerator.Current;
						if (guibutton3.buttonInfo.intData == num)
						{
							this.SetUISelection(guibutton3.gameObject);
							break;
						}
					}
					goto IL_2B0;
				}
			}
			if (player.controllers.joystickCount > 0 && !this.isJoystickSelected)
			{
				this.currentJoystickId = player.controllers.Joysticks[0].id;
			}
			IL_2B0:
			if (this.isJoystickSelected && player.controllers.joystickCount > 0)
			{
				this.references.removeControllerButton.interactable = true;
				this.references.controllerNameLabel.text = this._language.GetControllerName(this.currentJoystick);
				if (this.currentJoystick.axisCount > 0)
				{
					this.references.calibrateControllerButton.interactable = true;
				}
			}
			int joystickCount = player.controllers.joystickCount;
			int joystickCount2 = ReInput.controllers.joystickCount;
			int maxControllersPerPlayer = this.GetMaxControllersPerPlayer();
			bool flag = maxControllersPerPlayer == 0;
			if (joystickCount2 > 0 && joystickCount < joystickCount2 && (maxControllersPerPlayer == 1 || flag || joystickCount < maxControllersPerPlayer))
			{
				UITools.SetInteractable(this.references.assignControllerButton, true, false);
			}
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x000FD404 File Offset: 0x000FB604
		private void RedrawMapCategoriesGroup(bool playTransitions)
		{
			if (!this.showMapCategories)
			{
				return;
			}
			for (int i = 0; i < this.mapCategoryButtons.Count; i++)
			{
				bool state = this.currentMapCategoryId != this.mapCategoryButtons[i].buttonInfo.intData;
				this.mapCategoryButtons[i].SetInteractible(state, playTransitions);
			}
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x0001F167 File Offset: 0x0001D367
		private void RedrawInputGrid(bool listsChanged)
		{
			if (listsChanged)
			{
				this.RefreshInputGridStructure();
			}
			this.PopulateInputFields();
			if (listsChanged)
			{
				this.ResetInputGridScrollBar();
			}
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x0001F181 File Offset: 0x0001D381
		private void ForceRefresh()
		{
			if (this.windowManager.isWindowOpen)
			{
				this.CloseAllWindows();
				return;
			}
			this.Redraw(false, false);
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x000FD468 File Offset: 0x000FB668
		private void CreateInputCategoryRow(ref int rowCount, InputCategory category)
		{
			this.CreateLabel(this._language.GetMapCategoryName(category.id), this.references.inputGridActionColumn, new Vector2(0f, (float)(rowCount * this._inputRowHeight) * -1f));
			rowCount++;
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x0001F19F File Offset: 0x0001D39F
		private ControlMapper.GUILabel CreateLabel(string labelText, Transform parent, Vector2 offset)
		{
			return this.CreateLabel(this.prefabs.inputGridLabel, labelText, parent, offset);
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x000FD4B8 File Offset: 0x000FB6B8
		private ControlMapper.GUILabel CreateLabel(GameObject prefab, string labelText, Transform parent, Vector2 offset)
		{
			GameObject gameObject = this.InstantiateGUIObject(prefab, parent, offset);
			Text componentInSelfOrChildren = UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
			if (componentInSelfOrChildren == null)
			{
				Debug.LogError("Rewired Control Mapper: Label prefab is missing Text component!");
				return null;
			}
			componentInSelfOrChildren.text = labelText;
			return new ControlMapper.GUILabel(gameObject);
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x0001F1B5 File Offset: 0x0001D3B5
		private ControlMapper.GUIButton CreateButton(string labelText, Transform parent, Vector2 offset)
		{
			ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(this.InstantiateGUIObject(this.prefabs.button, parent, offset));
			guibutton.SetLabel(labelText);
			return guibutton;
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x0001F1D6 File Offset: 0x0001D3D6
		private ControlMapper.GUIButton CreateFitButton(string labelText, Transform parent, Vector2 offset)
		{
			ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(this.InstantiateGUIObject(this.prefabs.fitButton, parent, offset));
			guibutton.SetLabel(labelText);
			return guibutton;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x000FD4FC File Offset: 0x000FB6FC
		private ControlMapper.GUIInputField CreateInputField(Transform parent, Vector2 offset, string label, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
		{
			ControlMapper.GUIInputField guiinputField = this.CreateInputField(parent, offset);
			guiinputField.SetLabel("");
			guiinputField.SetFieldInfoData(actionId, axisRange, controllerType, fieldIndex);
			guiinputField.SetOnClickCallback(this.inputFieldActivatedDelegate);
			guiinputField.fieldInfo.OnSelectedEvent += this.OnUIElementSelected;
			return guiinputField;
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x0001F1F7 File Offset: 0x0001D3F7
		private ControlMapper.GUIInputField CreateInputField(Transform parent, Vector2 offset)
		{
			return new ControlMapper.GUIInputField(this.InstantiateGUIObject(this.prefabs.inputGridFieldButton, parent, offset));
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x0001F211 File Offset: 0x0001D411
		private ControlMapper.GUIToggle CreateToggle(GameObject prefab, Transform parent, Vector2 offset, string label, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
		{
			ControlMapper.GUIToggle guitoggle = this.CreateToggle(prefab, parent, offset);
			guitoggle.SetToggleInfoData(actionId, axisRange, controllerType, fieldIndex);
			guitoggle.SetOnSubmitCallback(this.inputFieldInvertToggleStateChangedDelegate);
			guitoggle.toggleInfo.OnSelectedEvent += this.OnUIElementSelected;
			return guitoggle;
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x0001F24D File Offset: 0x0001D44D
		private ControlMapper.GUIToggle CreateToggle(GameObject prefab, Transform parent, Vector2 offset)
		{
			return new ControlMapper.GUIToggle(this.InstantiateGUIObject(prefab, parent, offset));
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x000FD550 File Offset: 0x000FB750
		private GameObject InstantiateGUIObject(GameObject prefab, Transform parent, Vector2 offset)
		{
			if (prefab == null)
			{
				Debug.LogError("Rewired Control Mapper: Prefab is null!");
				return null;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
			return this.InitializeNewGUIGameObject(gameObject, parent, offset);
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x000FD584 File Offset: 0x000FB784
		private GameObject CreateNewGUIObject(string name, Transform parent, Vector2 offset)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = name;
			gameObject.AddComponent<RectTransform>();
			return this.InitializeNewGUIGameObject(gameObject, parent, offset);
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x000FD5B0 File Offset: 0x000FB7B0
		private GameObject InitializeNewGUIGameObject(GameObject gameObject, Transform parent, Vector2 offset)
		{
			if (gameObject == null)
			{
				Debug.LogError("Rewired Control Mapper: GameObject is null!");
				return null;
			}
			RectTransform component = gameObject.GetComponent<RectTransform>();
			if (component == null)
			{
				Debug.LogError("Rewired Control Mapper: GameObject does not have a RectTransform component!");
				return gameObject;
			}
			if (parent != null)
			{
				component.SetParent(parent, false);
			}
			component.anchoredPosition = offset;
			return gameObject;
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x000FD608 File Offset: 0x000FB808
		private GameObject CreateNewColumnGroup(string name, Transform parent, int maxWidth)
		{
			GameObject gameObject = this.CreateNewGUIObject(name, parent, Vector2.zero);
			this.inputGrid.AddGroup(gameObject);
			LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();
			if (maxWidth >= 0)
			{
				layoutElement.preferredWidth = (float)maxWidth;
			}
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 0f);
			component.anchorMax = new Vector2(1f, 0f);
			return gameObject;
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x0001F25D File Offset: 0x0001D45D
		private Window OpenWindow(bool closeOthers)
		{
			return this.OpenWindow(string.Empty, closeOthers);
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x000FD674 File Offset: 0x000FB874
		private Window OpenWindow(string name, bool closeOthers)
		{
			if (closeOthers)
			{
				this.windowManager.CancelAll();
			}
			Window window = this.windowManager.OpenWindow(name, this._defaultWindowWidth, this._defaultWindowHeight);
			if (window == null)
			{
				return null;
			}
			this.ChildWindowOpened();
			return window;
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x0001F26B File Offset: 0x0001D46B
		private Window OpenWindow(GameObject windowPrefab, bool closeOthers)
		{
			return this.OpenWindow(windowPrefab, string.Empty, closeOthers);
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x000FD6BC File Offset: 0x000FB8BC
		private Window OpenWindow(GameObject windowPrefab, string name, bool closeOthers)
		{
			if (closeOthers)
			{
				this.windowManager.CancelAll();
			}
			Window window = this.windowManager.OpenWindow(windowPrefab, name);
			if (window == null)
			{
				return null;
			}
			this.ChildWindowOpened();
			return window;
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x000FD6F8 File Offset: 0x000FB8F8
		private void OpenModal(string title, string message, string confirmText, Action<int> confirmAction, string cancelText, Action<int> cancelAction, bool closeOthers)
		{
			Window window = this.OpenWindow(closeOthers);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, title);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), message);
			UnityAction unityAction = delegate()
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, confirmText, delegate()
			{
				this.OnRestoreDefaultsConfirmed(window.id);
			}, unityAction, false);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, cancelText, unityAction, unityAction, true);
			this.windowManager.Focus(window);
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x0001F27A File Offset: 0x0001D47A
		private void CloseWindow(int windowId)
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.windowManager.CloseWindow(windowId);
			this.ChildWindowClosed();
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x0001F29C File Offset: 0x0001D49C
		private void CloseTopWindow()
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.windowManager.CloseTop();
			this.ChildWindowClosed();
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x0001F2BD File Offset: 0x0001D4BD
		private void CloseAllWindows()
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.windowManager.CancelAll();
			this.ChildWindowClosed();
			this.InputPollingStopped();
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x0001F2E4 File Offset: 0x0001D4E4
		private void ChildWindowOpened()
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.SetIsFocused(false);
			if (this._PopupWindowOpenedEvent != null)
			{
				this._PopupWindowOpenedEvent();
			}
			if (this._onPopupWindowOpened != null)
			{
				this._onPopupWindowOpened.Invoke();
			}
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x0001F321 File Offset: 0x0001D521
		private void ChildWindowClosed()
		{
			if (this.windowManager.isWindowOpen)
			{
				return;
			}
			this.SetIsFocused(true);
			if (this._PopupWindowClosedEvent != null)
			{
				this._PopupWindowClosedEvent();
			}
			if (this._onPopupWindowClosed != null)
			{
				this._onPopupWindowClosed.Invoke();
			}
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x000FD804 File Offset: 0x000FBA04
		private bool HasElementAssignmentConflicts(Player player, ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (player == null || mapping == null)
			{
				return false;
			}
			ElementAssignmentConflictCheck conflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
			{
				return false;
			}
			if (skipOtherPlayers)
			{
				return ReInput.players.SystemPlayer.controllers.conflictChecking.DoesElementAssignmentConflict(conflictCheck) || player.controllers.conflictChecking.DoesElementAssignmentConflict(conflictCheck);
			}
			return ReInput.controllers.conflictChecking.DoesElementAssignmentConflict(conflictCheck);
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x000FD870 File Offset: 0x000FBA70
		private bool IsBlockingAssignmentConflict(ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			ElementAssignmentConflictCheck conflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
			{
				return false;
			}
			if (skipOtherPlayers)
			{
				foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo in ReInput.players.SystemPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!elementAssignmentConflictInfo.isUserAssignable)
					{
						return true;
					}
				}
				using (IEnumerator<ElementAssignmentConflictInfo> enumerator = this.currentPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ElementAssignmentConflictInfo elementAssignmentConflictInfo2 = enumerator.Current;
						if (!elementAssignmentConflictInfo2.isUserAssignable)
						{
							return true;
						}
					}
					return false;
				}
			}
			foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo3 in ReInput.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
			{
				if (!elementAssignmentConflictInfo3.isUserAssignable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x0001F35E File Offset: 0x0001D55E
		private IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(Player player, ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (player == null || mapping == null)
			{
				yield break;
			}
			ElementAssignmentConflictCheck conflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
			{
				yield break;
			}
			if (skipOtherPlayers)
			{
				foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo in ReInput.players.SystemPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!elementAssignmentConflictInfo.isUserAssignable)
					{
						yield return elementAssignmentConflictInfo;
					}
				}
				IEnumerator<ElementAssignmentConflictInfo> enumerator = null;
				foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo2 in player.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!elementAssignmentConflictInfo2.isUserAssignable)
					{
						yield return elementAssignmentConflictInfo2;
					}
				}
				enumerator = null;
			}
			else
			{
				foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo3 in ReInput.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!elementAssignmentConflictInfo3.isUserAssignable)
					{
						yield return elementAssignmentConflictInfo3;
					}
				}
				IEnumerator<ElementAssignmentConflictInfo> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000FD994 File Offset: 0x000FBB94
		private bool CreateConflictCheck(ControlMapper.InputMapping mapping, ElementAssignment assignment, out ElementAssignmentConflictCheck conflictCheck)
		{
			if (mapping == null || this.currentPlayer == null)
			{
				conflictCheck = default(ElementAssignmentConflictCheck);
				return false;
			}
			conflictCheck = assignment.ToElementAssignmentConflictCheck();
			conflictCheck.playerId = this.currentPlayer.id;
			conflictCheck.controllerType = mapping.controllerType;
			conflictCheck.controllerId = mapping.controllerId;
			conflictCheck.controllerMapId = mapping.map.id;
			conflictCheck.controllerMapCategoryId = mapping.map.categoryId;
			if (mapping.aem != null)
			{
				conflictCheck.elementMapId = mapping.aem.id;
			}
			return true;
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000FDA28 File Offset: 0x000FBC28
		private void PollKeyboardForAssignment(out ControllerPollingInfo pollingInfo, out bool modifierKeyPressed, out ModifierKeyFlags modifierFlags, out string label)
		{
			pollingInfo = default(ControllerPollingInfo);
			label = string.Empty;
			modifierKeyPressed = false;
			modifierFlags = ModifierKeyFlags.None;
			int num = 0;
			ControllerPollingInfo controllerPollingInfo = default(ControllerPollingInfo);
			ControllerPollingInfo controllerPollingInfo2 = default(ControllerPollingInfo);
			ModifierKeyFlags modifierKeyFlags = ModifierKeyFlags.None;
			foreach (ControllerPollingInfo controllerPollingInfo3 in ReInput.controllers.Keyboard.PollForAllKeys())
			{
				KeyCode keyboardKey = controllerPollingInfo3.keyboardKey;
				if (keyboardKey != KeyCode.AltGr)
				{
					if (Keyboard.IsModifierKey(controllerPollingInfo3.keyboardKey))
					{
						if (num == 0)
						{
							controllerPollingInfo2 = controllerPollingInfo3;
						}
						modifierKeyFlags |= Keyboard.KeyCodeToModifierKeyFlags(keyboardKey);
						num++;
					}
					else if (controllerPollingInfo.keyboardKey == KeyCode.None)
					{
						controllerPollingInfo = controllerPollingInfo3;
					}
				}
			}
			if (controllerPollingInfo.keyboardKey == KeyCode.None)
			{
				if (num > 0)
				{
					modifierKeyPressed = true;
					if (num == 1)
					{
						if (ReInput.controllers.Keyboard.GetKeyTimePressed(controllerPollingInfo2.keyboardKey) > 1.0)
						{
							pollingInfo = controllerPollingInfo2;
							return;
						}
						label = Keyboard.GetKeyName(controllerPollingInfo2.keyboardKey);
						return;
					}
					else
					{
						label = this._language.ModifierKeyFlagsToString(modifierKeyFlags);
					}
				}
				return;
			}
			if (!ReInput.controllers.Keyboard.GetKeyDown(controllerPollingInfo.keyboardKey))
			{
				return;
			}
			if (num == 0)
			{
				pollingInfo = controllerPollingInfo;
				return;
			}
			pollingInfo = controllerPollingInfo;
			modifierFlags = modifierKeyFlags;
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x000FDB74 File Offset: 0x000FBD74
		private bool GetFirstElementAssignmentConflict(ElementAssignmentConflictCheck conflictCheck, out ElementAssignmentConflictInfo conflict, bool skipOtherPlayers)
		{
			if (this.GetFirstElementAssignmentConflict(this.currentPlayer, conflictCheck, out conflict))
			{
				return true;
			}
			if (this.GetFirstElementAssignmentConflict(ReInput.players.SystemPlayer, conflictCheck, out conflict))
			{
				return true;
			}
			if (!skipOtherPlayers)
			{
				IList<Player> players = ReInput.players.Players;
				for (int i = 0; i < players.Count; i++)
				{
					Player player = players[i];
					if (player != this.currentPlayer && this.GetFirstElementAssignmentConflict(player, conflictCheck, out conflict))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x000FDBE8 File Offset: 0x000FBDE8
		private bool GetFirstElementAssignmentConflict(Player player, ElementAssignmentConflictCheck conflictCheck, out ElementAssignmentConflictInfo conflict)
		{
			using (IEnumerator<ElementAssignmentConflictInfo> enumerator = player.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					ElementAssignmentConflictInfo elementAssignmentConflictInfo = enumerator.Current;
					conflict = elementAssignmentConflictInfo;
					return true;
				}
			}
			conflict = default(ElementAssignmentConflictInfo);
			return false;
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x000FDC50 File Offset: 0x000FBE50
		private void StartAxisCalibration(int axisIndex)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this.currentPlayer.controllers.joystickCount == 0)
			{
				return;
			}
			Joystick currentJoystick = this.currentJoystick;
			if (axisIndex < 0 || axisIndex >= currentJoystick.axisCount)
			{
				return;
			}
			this.pendingAxisCalibration = new ControlMapper.AxisCalibrator(currentJoystick, axisIndex);
			this.ShowCalibrateAxisStep1Window();
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x0001F38B File Offset: 0x0001D58B
		private void EndAxisCalibration()
		{
			if (this.pendingAxisCalibration == null)
			{
				return;
			}
			this.pendingAxisCalibration.Commit();
			this.pendingAxisCalibration = null;
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x0001F3A8 File Offset: 0x0001D5A8
		private void SetUISelection(GameObject selection)
		{
			if (EventSystem.current == null)
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(selection);
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x0001F3C3 File Offset: 0x0001D5C3
		private void RestoreLastUISelection()
		{
			if (this.lastUISelection == null || !this.lastUISelection.activeInHierarchy)
			{
				this.SetDefaultUISelection();
				return;
			}
			this.SetUISelection(this.lastUISelection);
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x0001F3F3 File Offset: 0x0001D5F3
		private void SetDefaultUISelection()
		{
			if (!this.isOpen)
			{
				return;
			}
			if (this.references.defaultSelection == null)
			{
				this.SetUISelection(null);
				return;
			}
			this.SetUISelection(this.references.defaultSelection.gameObject);
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x000FDCA4 File Offset: 0x000FBEA4
		private void SelectDefaultMapCategory(bool redraw)
		{
			this.currentMapCategoryId = this.GetDefaultMapCategoryId();
			this.OnMapCategorySelected(this.currentMapCategoryId, redraw);
			if (!this.showMapCategories)
			{
				return;
			}
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				if (ReInput.mapping.GetMapCategory(this._mappingSets[i].mapCategoryId) != null)
				{
					this.currentMapCategoryId = this._mappingSets[i].mapCategoryId;
					break;
				}
			}
			if (this.currentMapCategoryId < 0)
			{
				return;
			}
			for (int j = 0; j < this._mappingSets.Length; j++)
			{
				bool state = this._mappingSets[j].mapCategoryId != this.currentMapCategoryId;
				this.mapCategoryButtons[j].SetInteractible(state, false);
			}
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x0001F42F File Offset: 0x0001D62F
		private void CheckUISelection()
		{
			if (!this.isFocused)
			{
				return;
			}
			if (this.currentUISelection == null)
			{
				this.RestoreLastUISelection();
			}
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x0001F44E File Offset: 0x0001D64E
		private void OnUIElementSelected(GameObject selectedObject)
		{
			this.lastUISelection = selectedObject;
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x0001F457 File Offset: 0x0001D657
		private void SetIsFocused(bool state)
		{
			this.references.mainCanvasGroup.interactable = state;
			if (state)
			{
				this.Redraw(false, false);
				this.RestoreLastUISelection();
				this.blockInputOnFocusEndTime = Time.unscaledTime + 0.1f;
			}
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x0001F48C File Offset: 0x0001D68C
		public void Toggle()
		{
			if (this.isOpen)
			{
				this.Close(true);
				return;
			}
			this.Open();
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x0001F4A4 File Offset: 0x0001D6A4
		public void Open()
		{
			this.Open(false);
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x000FDD60 File Offset: 0x000FBF60
		private void Open(bool force)
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			if (!this.initialized)
			{
				return;
			}
			if (!force && this.isOpen)
			{
				return;
			}
			this.Clear();
			this.canvas.SetActive(true);
			this.OnPlayerSelected(0, false);
			this.SelectDefaultMapCategory(false);
			this.SetDefaultUISelection();
			this.Redraw(true, false);
			if (this._ScreenOpenedEvent != null)
			{
				this._ScreenOpenedEvent();
			}
			if (this._onScreenOpened != null)
			{
				this._onScreenOpened.Invoke();
			}
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x000FDDE8 File Offset: 0x000FBFE8
		public void Close(bool save)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (save && ReInput.userDataStore != null)
			{
				ReInput.userDataStore.Save();
			}
			this.Clear();
			this.canvas.SetActive(false);
			this.SetUISelection(null);
			if (this._ScreenClosedEvent != null)
			{
				this._ScreenClosedEvent();
			}
			if (this._onScreenClosed != null)
			{
				this._onScreenClosed.Invoke();
			}
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x0001F4AD File Offset: 0x0001D6AD
		private void Clear()
		{
			this.windowManager.CancelAll();
			this.lastUISelection = null;
			this.pendingInputMapping = null;
			this.pendingAxisCalibration = null;
			this.InputPollingStopped();
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x0001F4D5 File Offset: 0x0001D6D5
		private void ClearCompletely()
		{
			this.ClearSpawnedObjects();
			this.ClearAllVars();
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x000FDE5C File Offset: 0x000FC05C
		private void ClearSpawnedObjects()
		{
			this.windowManager.ClearCompletely();
			this.inputGrid.ClearAll();
			foreach (ControlMapper.GUIButton guibutton in this.playerButtons)
			{
				UnityEngine.Object.Destroy(guibutton.gameObject);
			}
			this.playerButtons.Clear();
			foreach (ControlMapper.GUIButton guibutton2 in this.mapCategoryButtons)
			{
				UnityEngine.Object.Destroy(guibutton2.gameObject);
			}
			this.mapCategoryButtons.Clear();
			foreach (ControlMapper.GUIButton guibutton3 in this.assignedControllerButtons)
			{
				UnityEngine.Object.Destroy(guibutton3.gameObject);
			}
			this.assignedControllerButtons.Clear();
			if (this.assignedControllerButtonsPlaceholder != null)
			{
				UnityEngine.Object.Destroy(this.assignedControllerButtonsPlaceholder.gameObject);
				this.assignedControllerButtonsPlaceholder = null;
			}
			foreach (GameObject obj in this.miscInstantiatedObjects)
			{
				UnityEngine.Object.Destroy(obj);
			}
			this.miscInstantiatedObjects.Clear();
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x0001F4E3 File Offset: 0x0001D6E3
		private void ClearVarsOnPlayerChange()
		{
			this.currentJoystickId = -1;
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x0001F4E3 File Offset: 0x0001D6E3
		private void ClearVarsOnJoystickChange()
		{
			this.currentJoystickId = -1;
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x000FDFDC File Offset: 0x000FC1DC
		private void ClearAllVars()
		{
			this.initialized = false;
			ControlMapper.Instance = null;
			this.playerCount = 0;
			this.inputGrid = null;
			this.windowManager = null;
			this.currentPlayerId = -1;
			this.currentMapCategoryId = -1;
			this.playerButtons = null;
			this.mapCategoryButtons = null;
			this.miscInstantiatedObjects = null;
			this.canvas = null;
			this.lastUISelection = null;
			this.currentJoystickId = -1;
			this.pendingInputMapping = null;
			this.pendingAxisCalibration = null;
			this.inputFieldActivatedDelegate = null;
			this.inputFieldInvertToggleStateChangedDelegate = null;
			this.isPollingForInput = false;
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x0001F4EC File Offset: 0x0001D6EC
		public void Reset()
		{
			if (!this.initialized)
			{
				return;
			}
			this.ClearCompletely();
			this.Initialize();
			if (this.isOpen)
			{
				this.Open(true);
			}
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000FE068 File Offset: 0x000FC268
		private void SetActionAxisInverted(bool state, ControllerType controllerType, int actionElementMapId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			ControllerMapWithAxes controllerMapWithAxes = this.GetControllerMap(controllerType) as ControllerMapWithAxes;
			if (controllerMapWithAxes == null)
			{
				return;
			}
			ActionElementMap elementMap = controllerMapWithAxes.GetElementMap(actionElementMapId);
			if (elementMap == null)
			{
				return;
			}
			elementMap.invert = state;
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x000FE0A4 File Offset: 0x000FC2A4
		private ControllerMap GetControllerMap(ControllerType type)
		{
			if (this.currentPlayer == null)
			{
				return null;
			}
			int controllerId = 0;
			switch (type)
			{
			case ControllerType.Keyboard:
			case ControllerType.Mouse:
				break;
			case ControllerType.Joystick:
				if (this.currentPlayer.controllers.joystickCount <= 0)
				{
					return null;
				}
				controllerId = this.currentJoystick.id;
				break;
			default:
				throw new NotImplementedException();
			}
			return this.currentPlayer.controllers.maps.GetFirstMapInCategory(type, controllerId, this.currentMapCategoryId);
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x000FE118 File Offset: 0x000FC318
		private ControllerMap GetControllerMapOrCreateNew(ControllerType controllerType, int controllerId, int layoutId)
		{
			ControllerMap controllerMap = this.GetControllerMap(controllerType);
			if (controllerMap == null)
			{
				this.currentPlayer.controllers.maps.AddEmptyMap(controllerType, controllerId, this.currentMapCategoryId, layoutId);
				controllerMap = this.currentPlayer.controllers.maps.GetMap(controllerType, controllerId, this.currentMapCategoryId, layoutId);
			}
			return controllerMap;
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x000FE170 File Offset: 0x000FC370
		private int CountIEnumerable<T>(IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return 0;
			}
			IEnumerator<T> enumerator = enumerable.GetEnumerator();
			if (enumerator == null)
			{
				return 0;
			}
			int num = 0;
			while (enumerator.MoveNext())
			{
				num++;
			}
			return num;
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x000FE1A0 File Offset: 0x000FC3A0
		private int GetDefaultMapCategoryId()
		{
			if (this._mappingSets.Length == 0)
			{
				return 0;
			}
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				if (ReInput.mapping.GetMapCategory(this._mappingSets[i].mapCategoryId) != null)
				{
					return this._mappingSets[i].mapCategoryId;
				}
			}
			return 0;
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x000FE1F4 File Offset: 0x000FC3F4
		private void SubscribeFixedUISelectionEvents()
		{
			if (this.references.fixedSelectableUIElements == null)
			{
				return;
			}
			GameObject[] fixedSelectableUIElements = this.references.fixedSelectableUIElements;
			for (int i = 0; i < fixedSelectableUIElements.Length; i++)
			{
				UIElementInfo component = UnityTools.GetComponent<UIElementInfo>(fixedSelectableUIElements[i]);
				if (!(component == null))
				{
					component.OnSelectedEvent += this.OnUIElementSelected;
				}
			}
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x000FE250 File Offset: 0x000FC450
		private void SubscribeMenuControlInputEvents()
		{
			this.SubscribeRewiredInputEventAllPlayers(this._screenToggleAction, new Action<InputActionEventData>(this.OnScreenToggleActionPressed));
			this.SubscribeRewiredInputEventAllPlayers(this._screenOpenAction, new Action<InputActionEventData>(this.OnScreenOpenActionPressed));
			this.SubscribeRewiredInputEventAllPlayers(this._screenCloseAction, new Action<InputActionEventData>(this.OnScreenCloseActionPressed));
			this.SubscribeRewiredInputEventAllPlayers(this._universalCancelAction, new Action<InputActionEventData>(this.OnUniversalCancelActionPressed));
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x000FE2C0 File Offset: 0x000FC4C0
		private void UnsubscribeMenuControlInputEvents()
		{
			this.UnsubscribeRewiredInputEventAllPlayers(this._screenToggleAction, new Action<InputActionEventData>(this.OnScreenToggleActionPressed));
			this.UnsubscribeRewiredInputEventAllPlayers(this._screenOpenAction, new Action<InputActionEventData>(this.OnScreenOpenActionPressed));
			this.UnsubscribeRewiredInputEventAllPlayers(this._screenCloseAction, new Action<InputActionEventData>(this.OnScreenCloseActionPressed));
			this.UnsubscribeRewiredInputEventAllPlayers(this._universalCancelAction, new Action<InputActionEventData>(this.OnUniversalCancelActionPressed));
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x000FE330 File Offset: 0x000FC530
		private void SubscribeRewiredInputEventAllPlayers(int actionId, Action<InputActionEventData> callback)
		{
			if (actionId < 0 || callback == null)
			{
				return;
			}
			if (ReInput.mapping.GetAction(actionId) == null)
			{
				Debug.LogWarning("Rewired Control Mapper: " + actionId.ToString() + " is not a valid Action id!");
				return;
			}
			foreach (Player player in ReInput.players.AllPlayers)
			{
				player.AddInputEventDelegate(callback, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, actionId);
			}
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x000FE3B4 File Offset: 0x000FC5B4
		private void UnsubscribeRewiredInputEventAllPlayers(int actionId, Action<InputActionEventData> callback)
		{
			if (actionId < 0 || callback == null)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			if (ReInput.mapping.GetAction(actionId) == null)
			{
				Debug.LogWarning("Rewired Control Mapper: " + actionId.ToString() + " is not a valid Action id!");
				return;
			}
			foreach (Player player in ReInput.players.AllPlayers)
			{
				player.RemoveInputEventDelegate(callback, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, actionId);
			}
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x0001F512 File Offset: 0x0001D712
		private int GetMaxControllersPerPlayer()
		{
			if (this._rewiredInputManager.userData.ConfigVars.autoAssignJoysticks)
			{
				return this._rewiredInputManager.userData.ConfigVars.maxJoysticksPerPlayer;
			}
			return this._maxControllersPerPlayer;
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x0001F547 File Offset: 0x0001D747
		private bool ShowAssignedControllers()
		{
			return this._showControllers && (this._showAssignedControllers || this.GetMaxControllersPerPlayer() != 1);
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x0001F569 File Offset: 0x0001D769
		private void InspectorPropertyChanged(bool reset = false)
		{
			if (reset)
			{
				this.Reset();
			}
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x000FE440 File Offset: 0x000FC640
		private void AssignController(Player player, int controllerId)
		{
			if (player == null)
			{
				return;
			}
			if (player.controllers.ContainsController(ControllerType.Joystick, controllerId))
			{
				return;
			}
			if (this.GetMaxControllersPerPlayer() == 1)
			{
				this.RemoveAllControllers(player);
				this.ClearVarsOnJoystickChange();
			}
			foreach (Player player2 in ReInput.players.Players)
			{
				if (player2 != player)
				{
					this.RemoveController(player2, controllerId);
				}
			}
			player.controllers.AddController(ControllerType.Joystick, controllerId, false);
			if (ReInput.userDataStore != null)
			{
				ReInput.userDataStore.LoadControllerData(player.id, ControllerType.Joystick, controllerId);
			}
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x000FE4E8 File Offset: 0x000FC6E8
		private void RemoveAllControllers(Player player)
		{
			if (player == null)
			{
				return;
			}
			IList<Joystick> joysticks = player.controllers.Joysticks;
			for (int i = joysticks.Count - 1; i >= 0; i--)
			{
				this.RemoveController(player, joysticks[i].id);
			}
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x0001F574 File Offset: 0x0001D774
		private void RemoveController(Player player, int controllerId)
		{
			if (player == null)
			{
				return;
			}
			if (!player.controllers.ContainsController(ControllerType.Joystick, controllerId))
			{
				return;
			}
			if (ReInput.userDataStore != null)
			{
				ReInput.userDataStore.SaveControllerData(player.id, ControllerType.Joystick, controllerId);
			}
			player.controllers.RemoveController(ControllerType.Joystick, controllerId);
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x0001F5B0 File Offset: 0x0001D7B0
		private bool IsAllowedAssignment(ControlMapper.InputMapping pendingInputMapping, ControllerPollingInfo pollingInfo)
		{
			return pendingInputMapping != null && (pendingInputMapping.axisRange != AxisRange.Full || this._showSplitAxisInputFields || pollingInfo.elementType != ControllerElementType.Button);
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x0001F5D4 File Offset: 0x0001D7D4
		private void InputPollingStarted()
		{
			bool flag = this.isPollingForInput;
			this.isPollingForInput = true;
			if (!flag)
			{
				if (this._InputPollingStartedEvent != null)
				{
					this._InputPollingStartedEvent();
				}
				if (this._onInputPollingStarted != null)
				{
					this._onInputPollingStarted.Invoke();
				}
			}
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x0001F60B File Offset: 0x0001D80B
		private void InputPollingStopped()
		{
			bool flag = this.isPollingForInput;
			this.isPollingForInput = false;
			if (flag)
			{
				if (this._InputPollingEndedEvent != null)
				{
					this._InputPollingEndedEvent();
				}
				if (this._onInputPollingEnded != null)
				{
					this._onInputPollingEnded.Invoke();
				}
			}
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x0001F642 File Offset: 0x0001D842
		private int GetControllerInputFieldCount(ControllerType controllerType)
		{
			switch (controllerType)
			{
			case ControllerType.Keyboard:
				return this._keyboardInputFieldCount;
			case ControllerType.Mouse:
				return this._mouseInputFieldCount;
			case ControllerType.Joystick:
				return this._controllerInputFieldCount;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x000FE52C File Offset: 0x000FC72C
		private bool ShowSwapButton(int windowId, ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (this.currentPlayer == null)
			{
				return false;
			}
			if (!this._allowElementAssignmentSwap)
			{
				return false;
			}
			if (mapping == null || mapping.aem == null)
			{
				return false;
			}
			ElementAssignmentConflictCheck conflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
			{
				Debug.LogError("Rewired Control Mapper: Error creating conflict check!");
				return false;
			}
			List<ElementAssignmentConflictInfo> list = new List<ElementAssignmentConflictInfo>();
			list.AddRange(this.currentPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck));
			list.AddRange(ReInput.players.SystemPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck));
			if (list.Count == 0)
			{
				return false;
			}
			ActionElementMap aem2 = mapping.aem;
			ElementAssignmentConflictInfo elementAssignmentConflictInfo = list[0];
			int actionId = elementAssignmentConflictInfo.elementMap.actionId;
			Pole axisContribution = elementAssignmentConflictInfo.elementMap.axisContribution;
			AxisRange axisRange = aem2.axisRange;
			ControllerElementType elementType = aem2.elementType;
			if (elementType == elementAssignmentConflictInfo.elementMap.elementType && elementType == ControllerElementType.Axis)
			{
				if (axisRange != elementAssignmentConflictInfo.elementMap.axisRange)
				{
					if (axisRange == AxisRange.Full)
					{
						axisRange = AxisRange.Positive;
					}
					else if (elementAssignmentConflictInfo.elementMap.axisRange == AxisRange.Full)
					{
					}
				}
			}
			else if (elementType == ControllerElementType.Axis && (elementAssignmentConflictInfo.elementMap.elementType == ControllerElementType.Button || (elementAssignmentConflictInfo.elementMap.elementType == ControllerElementType.Axis && elementAssignmentConflictInfo.elementMap.axisRange != AxisRange.Full)) && axisRange == AxisRange.Full)
			{
				axisRange = AxisRange.Positive;
			}
			int num = 0;
			if (assignment.actionId == elementAssignmentConflictInfo.actionId && mapping.map == elementAssignmentConflictInfo.controllerMap)
			{
				Controller controller = ReInput.controllers.GetController(mapping.controllerType, mapping.controllerId);
				if (this.SwapIsSameInputRange(elementType, axisRange, axisContribution, controller.GetElementById(assignment.elementIdentifierId).type, assignment.axisRange, assignment.axisContribution))
				{
					num++;
				}
			}
			using (IEnumerator<ActionElementMap> enumerator = elementAssignmentConflictInfo.controllerMap.ElementMapsWithAction(actionId).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActionElementMap aem = enumerator.Current;
					if (aem.id != aem2.id && list.FindIndex((ElementAssignmentConflictInfo x) => x.elementMapId == aem.id) < 0 && this.SwapIsSameInputRange(elementType, axisRange, axisContribution, aem.elementType, aem.axisRange, aem.axisContribution))
					{
						num++;
					}
				}
			}
			return num < this.GetControllerInputFieldCount(mapping.controllerType);
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x0001F672 File Offset: 0x0001D872
		private bool SwapIsSameInputRange(ControllerElementType origElementType, AxisRange origAxisRange, Pole origAxisContribution, ControllerElementType conflictElementType, AxisRange conflictAxisRange, Pole conflictAxisContribution)
		{
			return ((origElementType == ControllerElementType.Button || (origElementType == ControllerElementType.Axis && origAxisRange != AxisRange.Full)) && (conflictElementType == ControllerElementType.Button || (conflictElementType == ControllerElementType.Axis && conflictAxisRange != AxisRange.Full)) && conflictAxisContribution == origAxisContribution) || (origElementType == ControllerElementType.Axis && origAxisRange == AxisRange.Full && conflictElementType == ControllerElementType.Axis && conflictAxisRange == AxisRange.Full);
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x0001F6A3 File Offset: 0x0001D8A3
		public static void ApplyTheme(ThemedElement.ElementInfo[] elementInfo)
		{
			if (ControlMapper.Instance == null)
			{
				return;
			}
			if (ControlMapper.Instance._themeSettings == null)
			{
				return;
			}
			if (!ControlMapper.Instance._useThemeSettings)
			{
				return;
			}
			ControlMapper.Instance._themeSettings.Apply(elementInfo);
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x0001F6E3 File Offset: 0x0001D8E3
		public static LanguageDataBase GetLanguage()
		{
			if (ControlMapper.Instance == null)
			{
				return null;
			}
			return ControlMapper.Instance._language;
		}

		// Token: 0x04002E44 RID: 11844
		public const int versionMajor = 1;

		// Token: 0x04002E45 RID: 11845
		public const int versionMinor = 1;

		// Token: 0x04002E46 RID: 11846
		public const bool usesTMPro = false;

		// Token: 0x04002E47 RID: 11847
		private const float blockInputOnFocusTimeout = 0.1f;

		// Token: 0x04002E48 RID: 11848
		private const string buttonIdentifier_playerSelection = "PlayerSelection";

		// Token: 0x04002E49 RID: 11849
		private const string buttonIdentifier_removeController = "RemoveController";

		// Token: 0x04002E4A RID: 11850
		private const string buttonIdentifier_assignController = "AssignController";

		// Token: 0x04002E4B RID: 11851
		private const string buttonIdentifier_calibrateController = "CalibrateController";

		// Token: 0x04002E4C RID: 11852
		private const string buttonIdentifier_editInputBehaviors = "EditInputBehaviors";

		// Token: 0x04002E4D RID: 11853
		private const string buttonIdentifier_mapCategorySelection = "MapCategorySelection";

		// Token: 0x04002E4E RID: 11854
		private const string buttonIdentifier_assignedControllerSelection = "AssignedControllerSelection";

		// Token: 0x04002E4F RID: 11855
		private const string buttonIdentifier_done = "Done";

		// Token: 0x04002E50 RID: 11856
		private const string buttonIdentifier_restoreDefaults = "RestoreDefaults";

		// Token: 0x04002E51 RID: 11857
		[SerializeField]
		[Tooltip("Must be assigned a Rewired Input Manager scene object or prefab.")]
		private InputManager _rewiredInputManager;

		// Token: 0x04002E52 RID: 11858
		[SerializeField]
		[Tooltip("Set to True to prevent the Game Object from being destroyed when a new scene is loaded.\n\nNOTE: Changing this value from True to False at runtime will have no effect because Object.DontDestroyOnLoad cannot be undone once set.")]
		private bool _dontDestroyOnLoad;

		// Token: 0x04002E53 RID: 11859
		[SerializeField]
		[Tooltip("Open the control mapping screen immediately on start. Mainly used for testing.")]
		private bool _openOnStart;

		// Token: 0x04002E54 RID: 11860
		[SerializeField]
		[Tooltip("The Layout of the Keyboard Maps to be displayed.")]
		private int _keyboardMapDefaultLayout;

		// Token: 0x04002E55 RID: 11861
		[SerializeField]
		[Tooltip("The Layout of the Mouse Maps to be displayed.")]
		private int _mouseMapDefaultLayout;

		// Token: 0x04002E56 RID: 11862
		[SerializeField]
		[Tooltip("The Layout of the Mouse Maps to be displayed.")]
		private int _joystickMapDefaultLayout;

		// Token: 0x04002E57 RID: 11863
		[SerializeField]
		private ControlMapper.MappingSet[] _mappingSets = new ControlMapper.MappingSet[]
		{
			ControlMapper.MappingSet.Default
		};

		// Token: 0x04002E58 RID: 11864
		[SerializeField]
		[Tooltip("Display a selectable list of Players. If your game only supports 1 player, you can disable this.")]
		private bool _showPlayers = true;

		// Token: 0x04002E59 RID: 11865
		[SerializeField]
		[Tooltip("Display the Controller column for input mapping.")]
		private bool _showControllers = true;

		// Token: 0x04002E5A RID: 11866
		[SerializeField]
		[Tooltip("Display the Keyboard column for input mapping.")]
		private bool _showKeyboard = true;

		// Token: 0x04002E5B RID: 11867
		[SerializeField]
		[Tooltip("Display the Mouse column for input mapping.")]
		private bool _showMouse = true;

		// Token: 0x04002E5C RID: 11868
		[SerializeField]
		[Tooltip("The maximum number of controllers allowed to be assigned to a Player. If set to any value other than 1, a selectable list of currently-assigned controller will be displayed to the user. [0 = infinite]")]
		private int _maxControllersPerPlayer = 1;

		// Token: 0x04002E5D RID: 11869
		[SerializeField]
		[Tooltip("Display section labels for each Action Category in the input field grid. Only applies if Action Categories are used to display the Action list.")]
		private bool _showActionCategoryLabels;

		// Token: 0x04002E5E RID: 11870
		[SerializeField]
		[Tooltip("The number of input fields to display for the keyboard. If you want to support alternate mappings on the same device, set this to 2 or more.")]
		private int _keyboardInputFieldCount = 2;

		// Token: 0x04002E5F RID: 11871
		[SerializeField]
		[Tooltip("The number of input fields to display for the mouse. If you want to support alternate mappings on the same device, set this to 2 or more.")]
		private int _mouseInputFieldCount = 1;

		// Token: 0x04002E60 RID: 11872
		[SerializeField]
		[Tooltip("The number of input fields to display for joysticks. If you want to support alternate mappings on the same device, set this to 2 or more.")]
		private int _controllerInputFieldCount = 1;

		// Token: 0x04002E61 RID: 11873
		[SerializeField]
		[Tooltip("Display a full-axis input assignment field for every axis-type Action in the input field grid. Also displays an invert toggle for the user  to invert the full-axis assignment direction.\n\n*IMPORTANT*: This field is required if you have made any full-axis assignments in the Rewired Input Manager or in saved XML user data. Disabling this field when you have full-axis assignments will result in the inability for the user to view, remove, or modify these full-axis assignments. In addition, these assignments may cause conflicts when trying to remap the same axes to Actions.")]
		private bool _showFullAxisInputFields = true;

		// Token: 0x04002E62 RID: 11874
		[SerializeField]
		[Tooltip("Display a positive and negative input assignment field for every axis-type Action in the input field grid.\n\n*IMPORTANT*: These fields are required to assign buttons, keyboard keys, and hat or D-Pad directions to axis-type Actions. If you have made any split-axis assignments or button/key/D-pad assignments to axis-type Actions in the Rewired Input Manager or in saved XML user data, disabling these fields will result in the inability for the user to view, remove, or modify these assignments. In addition, these assignments may cause conflicts when trying to remap the same elements to Actions.")]
		private bool _showSplitAxisInputFields = true;

		// Token: 0x04002E63 RID: 11875
		[SerializeField]
		[Tooltip("If enabled, when an element assignment conflict is found, an option will be displayed that allows the user to make the conflicting assignment anyway.")]
		private bool _allowElementAssignmentConflicts;

		// Token: 0x04002E64 RID: 11876
		[SerializeField]
		[Tooltip("If enabled, when an element assignment conflict is found, an option will be displayed that allows the user to swap conflicting assignments. This only applies to the first conflicting assignment found. This option will not be displayed if allowElementAssignmentConflicts is true.")]
		private bool _allowElementAssignmentSwap;

		// Token: 0x04002E65 RID: 11877
		[SerializeField]
		[Tooltip("The width in relative pixels of the Action label column.")]
		private int _actionLabelWidth = 200;

		// Token: 0x04002E66 RID: 11878
		[SerializeField]
		[Tooltip("The width in relative pixels of the Keyboard column.")]
		private int _keyboardColMaxWidth = 360;

		// Token: 0x04002E67 RID: 11879
		[SerializeField]
		[Tooltip("The width in relative pixels of the Mouse column.")]
		private int _mouseColMaxWidth = 200;

		// Token: 0x04002E68 RID: 11880
		[SerializeField]
		[Tooltip("The width in relative pixels of the Controller column.")]
		private int _controllerColMaxWidth = 200;

		// Token: 0x04002E69 RID: 11881
		[SerializeField]
		[Tooltip("The height in relative pixels of the input grid button rows.")]
		private int _inputRowHeight = 40;

		// Token: 0x04002E6A RID: 11882
		[SerializeField]
		[Tooltip("The padding of the input grid button rows.")]
		private RectOffset _inputRowPadding = new RectOffset();

		// Token: 0x04002E6B RID: 11883
		[SerializeField]
		[Tooltip("The width in relative pixels of spacing between input fields in a single column.")]
		private int _inputRowFieldSpacing;

		// Token: 0x04002E6C RID: 11884
		[SerializeField]
		[Tooltip("The width in relative pixels of spacing between columns.")]
		private int _inputColumnSpacing = 40;

		// Token: 0x04002E6D RID: 11885
		[SerializeField]
		[Tooltip("The height in relative pixels of the space between Action Category sections. Only applicable if Show Action Category Labels is checked.")]
		private int _inputRowCategorySpacing = 20;

		// Token: 0x04002E6E RID: 11886
		[SerializeField]
		[Tooltip("The width in relative pixels of the invert toggle buttons.")]
		private int _invertToggleWidth = 40;

		// Token: 0x04002E6F RID: 11887
		[SerializeField]
		[Tooltip("The width in relative pixels of generated popup windows.")]
		private int _defaultWindowWidth = 500;

		// Token: 0x04002E70 RID: 11888
		[SerializeField]
		[Tooltip("The height in relative pixels of generated popup windows.")]
		private int _defaultWindowHeight = 400;

		// Token: 0x04002E71 RID: 11889
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller when assigning a controller to a Player. If this time elapses with no user input a controller, the assignment will be canceled.")]
		private float _controllerAssignmentTimeout = 5f;

		// Token: 0x04002E72 RID: 11890
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller while waiting for axes to be centered before assigning input.")]
		private float _preInputAssignmentTimeout = 5f;

		// Token: 0x04002E73 RID: 11891
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller when assigning input. If this time elapses with no user input on the target controller, the assignment will be canceled.")]
		private float _inputAssignmentTimeout = 5f;

		// Token: 0x04002E74 RID: 11892
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller during calibration.")]
		private float _axisCalibrationTimeout = 5f;

		// Token: 0x04002E75 RID: 11893
		[SerializeField]
		[Tooltip("If checked, mouse X-axis movement will always be ignored during input assignment. Check this if you don't want the horizontal mouse axis to be user-assignable to any Actions.")]
		private bool _ignoreMouseXAxisAssignment = true;

		// Token: 0x04002E76 RID: 11894
		[SerializeField]
		[Tooltip("If checked, mouse Y-axis movement will always be ignored during input assignment. Check this if you don't want the vertical mouse axis to be user-assignable to any Actions.")]
		private bool _ignoreMouseYAxisAssignment = true;

		// Token: 0x04002E77 RID: 11895
		[SerializeField]
		[Tooltip("An Action that when activated will alternately close or open the main screen as long as no popup windows are open.")]
		private int _screenToggleAction = -1;

		// Token: 0x04002E78 RID: 11896
		[SerializeField]
		[Tooltip("An Action that when activated will open the main screen if it is closed.")]
		private int _screenOpenAction = -1;

		// Token: 0x04002E79 RID: 11897
		[SerializeField]
		[Tooltip("An Action that when activated will close the main screen as long as no popup windows are open.")]
		private int _screenCloseAction = -1;

		// Token: 0x04002E7A RID: 11898
		[SerializeField]
		[Tooltip("An Action that when activated will cancel and close any open popup window. Use with care because the element assigned to this Action can never be mapped by the user (because it would just cancel his assignment).")]
		private int _universalCancelAction = -1;

		// Token: 0x04002E7B RID: 11899
		[SerializeField]
		[Tooltip("If enabled, Universal Cancel will also close the main screen if pressed when no windows are open.")]
		private bool _universalCancelClosesScreen = true;

		// Token: 0x04002E7C RID: 11900
		[SerializeField]
		[Tooltip("If checked, controls will be displayed which will allow the user to customize certain Input Behavior settings.")]
		private bool _showInputBehaviorSettings;

		// Token: 0x04002E7D RID: 11901
		[SerializeField]
		[Tooltip("Customizable settings for user-modifiable Input Behaviors. This can be used for settings like Mouse Look Sensitivity.")]
		private ControlMapper.InputBehaviorSettings[] _inputBehaviorSettings;

		// Token: 0x04002E7E RID: 11902
		[SerializeField]
		[Tooltip("If enabled, UI elements will be themed based on the settings in Theme Settings.")]
		private bool _useThemeSettings = true;

		// Token: 0x04002E7F RID: 11903
		[SerializeField]
		[Tooltip("Must be assigned a ThemeSettings object. Used to theme UI elements.")]
		private ThemeSettings _themeSettings;

		// Token: 0x04002E80 RID: 11904
		[SerializeField]
		[Tooltip("Must be assigned a LanguageData object. Used to retrieve language entries for UI elements.")]
		private LanguageDataBase _language;

		// Token: 0x04002E81 RID: 11905
		[SerializeField]
		[Tooltip("A list of prefabs. You should not have to modify this.")]
		private ControlMapper.Prefabs prefabs;

		// Token: 0x04002E82 RID: 11906
		[SerializeField]
		[Tooltip("A list of references to elements in the hierarchy. You should not have to modify this.")]
		private ControlMapper.References references;

		// Token: 0x04002E83 RID: 11907
		[SerializeField]
		[Tooltip("Show the label for the Players button group?")]
		private bool _showPlayersGroupLabel = true;

		// Token: 0x04002E84 RID: 11908
		[SerializeField]
		[Tooltip("Show the label for the Controller button group?")]
		private bool _showControllerGroupLabel = true;

		// Token: 0x04002E85 RID: 11909
		[SerializeField]
		[Tooltip("Show the label for the Assigned Controllers button group?")]
		private bool _showAssignedControllersGroupLabel = true;

		// Token: 0x04002E86 RID: 11910
		[SerializeField]
		[Tooltip("Show the label for the Settings button group?")]
		private bool _showSettingsGroupLabel = true;

		// Token: 0x04002E87 RID: 11911
		[SerializeField]
		[Tooltip("Show the label for the Map Categories button group?")]
		private bool _showMapCategoriesGroupLabel = true;

		// Token: 0x04002E88 RID: 11912
		[SerializeField]
		[Tooltip("Show the label for the current controller name?")]
		private bool _showControllerNameLabel = true;

		// Token: 0x04002E89 RID: 11913
		[SerializeField]
		[Tooltip("Show the Assigned Controllers group? If joystick auto-assignment is enabled in the Rewired Input Manager and the max joysticks per player is set to any value other than 1, the Assigned Controllers group will always be displayed.")]
		private bool _showAssignedControllers = true;

		// Token: 0x04002E8A RID: 11914
		private Action _ScreenClosedEvent;

		// Token: 0x04002E8B RID: 11915
		private Action _ScreenOpenedEvent;

		// Token: 0x04002E8C RID: 11916
		private Action _PopupWindowOpenedEvent;

		// Token: 0x04002E8D RID: 11917
		private Action _PopupWindowClosedEvent;

		// Token: 0x04002E8E RID: 11918
		private Action _InputPollingStartedEvent;

		// Token: 0x04002E8F RID: 11919
		private Action _InputPollingEndedEvent;

		// Token: 0x04002E90 RID: 11920
		[SerializeField]
		[Tooltip("Event sent when the UI is closed.")]
		private UnityEvent _onScreenClosed;

		// Token: 0x04002E91 RID: 11921
		[SerializeField]
		[Tooltip("Event sent when the UI is opened.")]
		private UnityEvent _onScreenOpened;

		// Token: 0x04002E92 RID: 11922
		[SerializeField]
		[Tooltip("Event sent when a popup window is closed.")]
		private UnityEvent _onPopupWindowClosed;

		// Token: 0x04002E93 RID: 11923
		[SerializeField]
		[Tooltip("Event sent when a popup window is opened.")]
		private UnityEvent _onPopupWindowOpened;

		// Token: 0x04002E94 RID: 11924
		[SerializeField]
		[Tooltip("Event sent when polling for input has started.")]
		private UnityEvent _onInputPollingStarted;

		// Token: 0x04002E95 RID: 11925
		[SerializeField]
		[Tooltip("Event sent when polling for input has ended.")]
		private UnityEvent _onInputPollingEnded;

		// Token: 0x04002E96 RID: 11926
		private static ControlMapper Instance;

		// Token: 0x04002E97 RID: 11927
		private bool initialized;

		// Token: 0x04002E98 RID: 11928
		private int playerCount;

		// Token: 0x04002E99 RID: 11929
		private ControlMapper.InputGrid inputGrid;

		// Token: 0x04002E9A RID: 11930
		private ControlMapper.WindowManager windowManager;

		// Token: 0x04002E9B RID: 11931
		private int currentPlayerId;

		// Token: 0x04002E9C RID: 11932
		private int currentMapCategoryId;

		// Token: 0x04002E9D RID: 11933
		private List<ControlMapper.GUIButton> playerButtons;

		// Token: 0x04002E9E RID: 11934
		private List<ControlMapper.GUIButton> mapCategoryButtons;

		// Token: 0x04002E9F RID: 11935
		private List<ControlMapper.GUIButton> assignedControllerButtons;

		// Token: 0x04002EA0 RID: 11936
		private ControlMapper.GUIButton assignedControllerButtonsPlaceholder;

		// Token: 0x04002EA1 RID: 11937
		private List<GameObject> miscInstantiatedObjects;

		// Token: 0x04002EA2 RID: 11938
		private GameObject canvas;

		// Token: 0x04002EA3 RID: 11939
		private GameObject lastUISelection;

		// Token: 0x04002EA4 RID: 11940
		private int currentJoystickId = -1;

		// Token: 0x04002EA5 RID: 11941
		private float blockInputOnFocusEndTime;

		// Token: 0x04002EA6 RID: 11942
		private bool isPollingForInput;

		// Token: 0x04002EA7 RID: 11943
		private ControlMapper.InputMapping pendingInputMapping;

		// Token: 0x04002EA8 RID: 11944
		private ControlMapper.AxisCalibrator pendingAxisCalibration;

		// Token: 0x04002EA9 RID: 11945
		private Action<InputFieldInfo> inputFieldActivatedDelegate;

		// Token: 0x04002EAA RID: 11946
		private Action<ToggleInfo, bool> inputFieldInvertToggleStateChangedDelegate;

		// Token: 0x04002EAB RID: 11947
		private Action _restoreDefaultsDelegate;

		// Token: 0x0200065C RID: 1628
		private abstract class GUIElement
		{
			// Token: 0x17000783 RID: 1923
			// (get) Token: 0x06002E20 RID: 11808 RVA: 0x0001F6FE File Offset: 0x0001D8FE
			// (set) Token: 0x06002E21 RID: 11809 RVA: 0x0001F706 File Offset: 0x0001D906
			public RectTransform rectTransform { get; private set; }

			// Token: 0x06002E22 RID: 11810 RVA: 0x000FE914 File Offset: 0x000FCB14
			public GUIElement(GameObject gameObject)
			{
				if (gameObject == null)
				{
					Debug.LogError("Rewired Control Mapper: gameObject is null!");
					return;
				}
				this.selectable = gameObject.GetComponent<Selectable>();
				if (this.selectable == null)
				{
					Debug.LogError("Rewired Control Mapper: Selectable is null!");
					return;
				}
				this.gameObject = gameObject;
				this.rectTransform = gameObject.GetComponent<RectTransform>();
				this.text = UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
				this.uiElementInfo = gameObject.GetComponent<UIElementInfo>();
				this.children = new List<ControlMapper.GUIElement>();
			}

			// Token: 0x06002E23 RID: 11811 RVA: 0x000FE998 File Offset: 0x000FCB98
			public GUIElement(Selectable selectable, Text label)
			{
				if (selectable == null)
				{
					Debug.LogError("Rewired Control Mapper: Selectable is null!");
					return;
				}
				this.selectable = selectable;
				this.gameObject = selectable.gameObject;
				this.rectTransform = this.gameObject.GetComponent<RectTransform>();
				this.text = label;
				this.uiElementInfo = this.gameObject.GetComponent<UIElementInfo>();
				this.children = new List<ControlMapper.GUIElement>();
			}

			// Token: 0x06002E24 RID: 11812 RVA: 0x0001F70F File Offset: 0x0001D90F
			public virtual void SetInteractible(bool state, bool playTransition)
			{
				this.SetInteractible(state, playTransition, false);
			}

			// Token: 0x06002E25 RID: 11813 RVA: 0x000FEA08 File Offset: 0x000FCC08
			public virtual void SetInteractible(bool state, bool playTransition, bool permanent)
			{
				for (int i = 0; i < this.children.Count; i++)
				{
					if (this.children[i] != null)
					{
						this.children[i].SetInteractible(state, playTransition, permanent);
					}
				}
				if (this.permanentStateSet)
				{
					return;
				}
				if (this.selectable == null)
				{
					return;
				}
				if (permanent)
				{
					this.permanentStateSet = true;
				}
				if (this.selectable.interactable == state)
				{
					return;
				}
				UITools.SetInteractable(this.selectable, state, playTransition);
			}

			// Token: 0x06002E26 RID: 11814 RVA: 0x000FEA8C File Offset: 0x000FCC8C
			public virtual void SetTextWidth(int value)
			{
				if (this.text == null)
				{
					return;
				}
				LayoutElement layoutElement = this.text.GetComponent<LayoutElement>();
				if (layoutElement == null)
				{
					layoutElement = this.text.gameObject.AddComponent<LayoutElement>();
				}
				layoutElement.preferredWidth = (float)value;
			}

			// Token: 0x06002E27 RID: 11815 RVA: 0x000FEAD8 File Offset: 0x000FCCD8
			public virtual void SetFirstChildObjectWidth(ControlMapper.LayoutElementSizeType type, int value)
			{
				if (this.rectTransform.childCount == 0)
				{
					return;
				}
				Transform child = this.rectTransform.GetChild(0);
				LayoutElement layoutElement = child.GetComponent<LayoutElement>();
				if (layoutElement == null)
				{
					layoutElement = child.gameObject.AddComponent<LayoutElement>();
				}
				if (type == ControlMapper.LayoutElementSizeType.MinSize)
				{
					layoutElement.minWidth = (float)value;
					return;
				}
				if (type == ControlMapper.LayoutElementSizeType.PreferredSize)
				{
					layoutElement.preferredWidth = (float)value;
					return;
				}
				throw new NotImplementedException();
			}

			// Token: 0x06002E28 RID: 11816 RVA: 0x0001F71A File Offset: 0x0001D91A
			public virtual void SetLabel(string label)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.text = label;
			}

			// Token: 0x06002E29 RID: 11817 RVA: 0x0001F737 File Offset: 0x0001D937
			public virtual string GetLabel()
			{
				if (this.text == null)
				{
					return string.Empty;
				}
				return this.text.text;
			}

			// Token: 0x06002E2A RID: 11818 RVA: 0x0001F758 File Offset: 0x0001D958
			public virtual void AddChild(ControlMapper.GUIElement child)
			{
				this.children.Add(child);
			}

			// Token: 0x06002E2B RID: 11819 RVA: 0x0001F766 File Offset: 0x0001D966
			public void SetElementInfoData(string identifier, int intData)
			{
				if (this.uiElementInfo == null)
				{
					return;
				}
				this.uiElementInfo.identifier = identifier;
				this.uiElementInfo.intData = intData;
			}

			// Token: 0x06002E2C RID: 11820 RVA: 0x0001F78F File Offset: 0x0001D98F
			public virtual void SetActive(bool state)
			{
				if (this.gameObject == null)
				{
					return;
				}
				this.gameObject.SetActive(state);
			}

			// Token: 0x06002E2D RID: 11821 RVA: 0x000FEB3C File Offset: 0x000FCD3C
			protected virtual bool Init()
			{
				bool result = true;
				for (int i = 0; i < this.children.Count; i++)
				{
					if (this.children[i] != null && !this.children[i].Init())
					{
						result = false;
					}
				}
				if (this.selectable == null)
				{
					Debug.LogError("Rewired Control Mapper: UI Element is missing Selectable component!");
					result = false;
				}
				if (this.rectTransform == null)
				{
					Debug.LogError("Rewired Control Mapper: UI Element is missing RectTransform component!");
					result = false;
				}
				if (this.uiElementInfo == null)
				{
					Debug.LogError("Rewired Control Mapper: UI Element is missing UIElementInfo component!");
					result = false;
				}
				return result;
			}

			// Token: 0x04002EAC RID: 11948
			public readonly GameObject gameObject;

			// Token: 0x04002EAD RID: 11949
			protected readonly Text text;

			// Token: 0x04002EAE RID: 11950
			public readonly Selectable selectable;

			// Token: 0x04002EAF RID: 11951
			protected readonly UIElementInfo uiElementInfo;

			// Token: 0x04002EB0 RID: 11952
			protected bool permanentStateSet;

			// Token: 0x04002EB1 RID: 11953
			protected readonly List<ControlMapper.GUIElement> children;
		}

		// Token: 0x0200065D RID: 1629
		private class GUIButton : ControlMapper.GUIElement
		{
			// Token: 0x17000784 RID: 1924
			// (get) Token: 0x06002E2E RID: 11822 RVA: 0x0001F7AC File Offset: 0x0001D9AC
			protected Button button
			{
				get
				{
					return this.selectable as Button;
				}
			}

			// Token: 0x17000785 RID: 1925
			// (get) Token: 0x06002E2F RID: 11823 RVA: 0x0001F7B9 File Offset: 0x0001D9B9
			public ButtonInfo buttonInfo
			{
				get
				{
					return this.uiElementInfo as ButtonInfo;
				}
			}

			// Token: 0x06002E30 RID: 11824 RVA: 0x0001F7C6 File Offset: 0x0001D9C6
			public GUIButton(GameObject gameObject) : base(gameObject)
			{
				this.Init();
			}

			// Token: 0x06002E31 RID: 11825 RVA: 0x0001F7D6 File Offset: 0x0001D9D6
			public GUIButton(Button button, Text label) : base(button, label)
			{
				this.Init();
			}

			// Token: 0x06002E32 RID: 11826 RVA: 0x0001F7E7 File Offset: 0x0001D9E7
			public void SetButtonInfoData(string identifier, int intData)
			{
				base.SetElementInfoData(identifier, intData);
			}

			// Token: 0x06002E33 RID: 11827 RVA: 0x000FEBD4 File Offset: 0x000FCDD4
			public void SetOnClickCallback(Action<ButtonInfo> callback)
			{
				if (this.button == null)
				{
					return;
				}
				this.button.onClick.AddListener(delegate()
				{
					callback(this.buttonInfo);
				});
			}
		}

		// Token: 0x0200065F RID: 1631
		private class GUIInputField : ControlMapper.GUIElement
		{
			// Token: 0x17000786 RID: 1926
			// (get) Token: 0x06002E36 RID: 11830 RVA: 0x0001F7AC File Offset: 0x0001D9AC
			protected Button button
			{
				get
				{
					return this.selectable as Button;
				}
			}

			// Token: 0x17000787 RID: 1927
			// (get) Token: 0x06002E37 RID: 11831 RVA: 0x0001F809 File Offset: 0x0001DA09
			public InputFieldInfo fieldInfo
			{
				get
				{
					return this.uiElementInfo as InputFieldInfo;
				}
			}

			// Token: 0x17000788 RID: 1928
			// (get) Token: 0x06002E38 RID: 11832 RVA: 0x0001F816 File Offset: 0x0001DA16
			public bool hasToggle
			{
				get
				{
					return this.toggle != null;
				}
			}

			// Token: 0x17000789 RID: 1929
			// (get) Token: 0x06002E39 RID: 11833 RVA: 0x0001F821 File Offset: 0x0001DA21
			// (set) Token: 0x06002E3A RID: 11834 RVA: 0x0001F829 File Offset: 0x0001DA29
			public ControlMapper.GUIToggle toggle { get; private set; }

			// Token: 0x1700078A RID: 1930
			// (get) Token: 0x06002E3B RID: 11835 RVA: 0x0001F832 File Offset: 0x0001DA32
			// (set) Token: 0x06002E3C RID: 11836 RVA: 0x0001F84F File Offset: 0x0001DA4F
			public int actionElementMapId
			{
				get
				{
					if (this.fieldInfo == null)
					{
						return -1;
					}
					return this.fieldInfo.actionElementMapId;
				}
				set
				{
					if (this.fieldInfo == null)
					{
						return;
					}
					this.fieldInfo.actionElementMapId = value;
				}
			}

			// Token: 0x1700078B RID: 1931
			// (get) Token: 0x06002E3D RID: 11837 RVA: 0x0001F86C File Offset: 0x0001DA6C
			// (set) Token: 0x06002E3E RID: 11838 RVA: 0x0001F889 File Offset: 0x0001DA89
			public int controllerId
			{
				get
				{
					if (this.fieldInfo == null)
					{
						return -1;
					}
					return this.fieldInfo.controllerId;
				}
				set
				{
					if (this.fieldInfo == null)
					{
						return;
					}
					this.fieldInfo.controllerId = value;
				}
			}

			// Token: 0x06002E3F RID: 11839 RVA: 0x0001F7C6 File Offset: 0x0001D9C6
			public GUIInputField(GameObject gameObject) : base(gameObject)
			{
				this.Init();
			}

			// Token: 0x06002E40 RID: 11840 RVA: 0x0001F7D6 File Offset: 0x0001D9D6
			public GUIInputField(Button button, Text label) : base(button, label)
			{
				this.Init();
			}

			// Token: 0x06002E41 RID: 11841 RVA: 0x000FEC20 File Offset: 0x000FCE20
			public void SetFieldInfoData(int actionId, AxisRange axisRange, ControllerType controllerType, int intData)
			{
				base.SetElementInfoData(string.Empty, intData);
				if (this.fieldInfo == null)
				{
					return;
				}
				this.fieldInfo.actionId = actionId;
				this.fieldInfo.axisRange = axisRange;
				this.fieldInfo.controllerType = controllerType;
			}

			// Token: 0x06002E42 RID: 11842 RVA: 0x000FEC70 File Offset: 0x000FCE70
			public void SetOnClickCallback(Action<InputFieldInfo> callback)
			{
				if (this.button == null)
				{
					return;
				}
				this.button.onClick.AddListener(delegate()
				{
					callback(this.fieldInfo);
				});
			}

			// Token: 0x06002E43 RID: 11843 RVA: 0x0001F8A6 File Offset: 0x0001DAA6
			public virtual void SetInteractable(bool state, bool playTransition, bool permanent)
			{
				if (this.permanentStateSet)
				{
					return;
				}
				if (this.hasToggle && !state)
				{
					this.toggle.SetInteractible(state, playTransition, permanent);
				}
				base.SetInteractible(state, playTransition, permanent);
			}

			// Token: 0x06002E44 RID: 11844 RVA: 0x0001F8D3 File Offset: 0x0001DAD3
			public void AddToggle(ControlMapper.GUIToggle toggle)
			{
				if (toggle == null)
				{
					return;
				}
				this.toggle = toggle;
			}
		}

		// Token: 0x02000661 RID: 1633
		private class GUIToggle : ControlMapper.GUIElement
		{
			// Token: 0x1700078C RID: 1932
			// (get) Token: 0x06002E47 RID: 11847 RVA: 0x0001F8F8 File Offset: 0x0001DAF8
			protected Toggle toggle
			{
				get
				{
					return this.selectable as Toggle;
				}
			}

			// Token: 0x1700078D RID: 1933
			// (get) Token: 0x06002E48 RID: 11848 RVA: 0x0001F905 File Offset: 0x0001DB05
			public ToggleInfo toggleInfo
			{
				get
				{
					return this.uiElementInfo as ToggleInfo;
				}
			}

			// Token: 0x1700078E RID: 1934
			// (get) Token: 0x06002E49 RID: 11849 RVA: 0x0001F912 File Offset: 0x0001DB12
			// (set) Token: 0x06002E4A RID: 11850 RVA: 0x0001F92F File Offset: 0x0001DB2F
			public int actionElementMapId
			{
				get
				{
					if (this.toggleInfo == null)
					{
						return -1;
					}
					return this.toggleInfo.actionElementMapId;
				}
				set
				{
					if (this.toggleInfo == null)
					{
						return;
					}
					this.toggleInfo.actionElementMapId = value;
				}
			}

			// Token: 0x06002E4B RID: 11851 RVA: 0x0001F7C6 File Offset: 0x0001D9C6
			public GUIToggle(GameObject gameObject) : base(gameObject)
			{
				this.Init();
			}

			// Token: 0x06002E4C RID: 11852 RVA: 0x0001F7D6 File Offset: 0x0001D9D6
			public GUIToggle(Toggle toggle, Text label) : base(toggle, label)
			{
				this.Init();
			}

			// Token: 0x06002E4D RID: 11853 RVA: 0x000FECBC File Offset: 0x000FCEBC
			public void SetToggleInfoData(int actionId, AxisRange axisRange, ControllerType controllerType, int intData)
			{
				base.SetElementInfoData(string.Empty, intData);
				if (this.toggleInfo == null)
				{
					return;
				}
				this.toggleInfo.actionId = actionId;
				this.toggleInfo.axisRange = axisRange;
				this.toggleInfo.controllerType = controllerType;
			}

			// Token: 0x06002E4E RID: 11854 RVA: 0x000FED0C File Offset: 0x000FCF0C
			public void SetOnSubmitCallback(Action<ToggleInfo, bool> callback)
			{
				if (this.toggle == null)
				{
					return;
				}
				EventTrigger eventTrigger = this.toggle.GetComponent<EventTrigger>();
				if (eventTrigger == null)
				{
					eventTrigger = this.toggle.gameObject.AddComponent<EventTrigger>();
				}
				EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
				triggerEvent.AddListener(delegate(BaseEventData data)
				{
					PointerEventData pointerEventData = data as PointerEventData;
					if (pointerEventData != null && pointerEventData.button != PointerEventData.InputButton.Left)
					{
						return;
					}
					callback(this.toggleInfo, this.toggle.isOn);
				});
				EventTrigger.Entry item = new EventTrigger.Entry
				{
					callback = triggerEvent,
					eventID = EventTriggerType.Submit
				};
				EventTrigger.Entry item2 = new EventTrigger.Entry
				{
					callback = triggerEvent,
					eventID = EventTriggerType.PointerClick
				};
				if (eventTrigger.triggers != null)
				{
					eventTrigger.triggers.Clear();
				}
				else
				{
					eventTrigger.triggers = new List<EventTrigger.Entry>();
				}
				eventTrigger.triggers.Add(item);
				eventTrigger.triggers.Add(item2);
			}

			// Token: 0x06002E4F RID: 11855 RVA: 0x0001F94C File Offset: 0x0001DB4C
			public void SetToggleState(bool state)
			{
				if (this.toggle == null)
				{
					return;
				}
				this.toggle.isOn = state;
			}
		}

		// Token: 0x02000663 RID: 1635
		private class GUILabel
		{
			// Token: 0x1700078F RID: 1935
			// (get) Token: 0x06002E52 RID: 11858 RVA: 0x0001F969 File Offset: 0x0001DB69
			// (set) Token: 0x06002E53 RID: 11859 RVA: 0x0001F971 File Offset: 0x0001DB71
			public GameObject gameObject { get; private set; }

			// Token: 0x17000790 RID: 1936
			// (get) Token: 0x06002E54 RID: 11860 RVA: 0x0001F97A File Offset: 0x0001DB7A
			// (set) Token: 0x06002E55 RID: 11861 RVA: 0x0001F982 File Offset: 0x0001DB82
			private Text text { get; set; }

			// Token: 0x17000791 RID: 1937
			// (get) Token: 0x06002E56 RID: 11862 RVA: 0x0001F98B File Offset: 0x0001DB8B
			// (set) Token: 0x06002E57 RID: 11863 RVA: 0x0001F993 File Offset: 0x0001DB93
			public RectTransform rectTransform { get; private set; }

			// Token: 0x06002E58 RID: 11864 RVA: 0x0001F99C File Offset: 0x0001DB9C
			public GUILabel(GameObject gameObject)
			{
				if (gameObject == null)
				{
					Debug.LogError("Rewired Control Mapper: gameObject is null!");
					return;
				}
				this.text = UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
				this.Check();
			}

			// Token: 0x06002E59 RID: 11865 RVA: 0x0001F9CB File Offset: 0x0001DBCB
			public GUILabel(Text label)
			{
				this.text = label;
				this.Check();
			}

			// Token: 0x06002E5A RID: 11866 RVA: 0x0001F9E1 File Offset: 0x0001DBE1
			public void SetSize(int width, int height)
			{
				if (this.text == null)
				{
					return;
				}
				this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)width);
				this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)height);
			}

			// Token: 0x06002E5B RID: 11867 RVA: 0x0001FA0E File Offset: 0x0001DC0E
			public void SetWidth(int width)
			{
				if (this.text == null)
				{
					return;
				}
				this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)width);
			}

			// Token: 0x06002E5C RID: 11868 RVA: 0x0001FA2D File Offset: 0x0001DC2D
			public void SetHeight(int height)
			{
				if (this.text == null)
				{
					return;
				}
				this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)height);
			}

			// Token: 0x06002E5D RID: 11869 RVA: 0x0001FA4C File Offset: 0x0001DC4C
			public void SetLabel(string label)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.text = label;
			}

			// Token: 0x06002E5E RID: 11870 RVA: 0x0001FA69 File Offset: 0x0001DC69
			public void SetFontStyle(FontStyle style)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.fontStyle = style;
			}

			// Token: 0x06002E5F RID: 11871 RVA: 0x0001FA86 File Offset: 0x0001DC86
			public void SetTextAlignment(TextAnchor alignment)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.alignment = alignment;
			}

			// Token: 0x06002E60 RID: 11872 RVA: 0x0001FAA3 File Offset: 0x0001DCA3
			public void SetActive(bool state)
			{
				if (this.gameObject == null)
				{
					return;
				}
				this.gameObject.SetActive(state);
			}

			// Token: 0x06002E61 RID: 11873 RVA: 0x000FEE28 File Offset: 0x000FD028
			private bool Check()
			{
				bool result = true;
				if (this.text == null)
				{
					Debug.LogError("Rewired Control Mapper: Button is missing Text child component!");
					result = false;
				}
				this.gameObject = this.text.gameObject;
				this.rectTransform = this.text.GetComponent<RectTransform>();
				return result;
			}
		}

		// Token: 0x02000664 RID: 1636
		[Serializable]
		public class MappingSet
		{
			// Token: 0x17000792 RID: 1938
			// (get) Token: 0x06002E62 RID: 11874 RVA: 0x0001FAC0 File Offset: 0x0001DCC0
			public int mapCategoryId
			{
				get
				{
					return this._mapCategoryId;
				}
			}

			// Token: 0x17000793 RID: 1939
			// (get) Token: 0x06002E63 RID: 11875 RVA: 0x0001FAC8 File Offset: 0x0001DCC8
			public ControlMapper.MappingSet.ActionListMode actionListMode
			{
				get
				{
					return this._actionListMode;
				}
			}

			// Token: 0x17000794 RID: 1940
			// (get) Token: 0x06002E64 RID: 11876 RVA: 0x0001FAD0 File Offset: 0x0001DCD0
			public IList<int> actionCategoryIds
			{
				get
				{
					if (this._actionCategoryIds == null)
					{
						return null;
					}
					if (this._actionCategoryIdsReadOnly == null)
					{
						this._actionCategoryIdsReadOnly = new ReadOnlyCollection<int>(this._actionCategoryIds);
					}
					return this._actionCategoryIdsReadOnly;
				}
			}

			// Token: 0x17000795 RID: 1941
			// (get) Token: 0x06002E65 RID: 11877 RVA: 0x0001FAFB File Offset: 0x0001DCFB
			public IList<int> actionIds
			{
				get
				{
					if (this._actionIds == null)
					{
						return null;
					}
					if (this._actionIdsReadOnly == null)
					{
						this._actionIdsReadOnly = new ReadOnlyCollection<int>(this._actionIds);
					}
					return this._actionIds;
				}
			}

			// Token: 0x17000796 RID: 1942
			// (get) Token: 0x06002E66 RID: 11878 RVA: 0x0001FB26 File Offset: 0x0001DD26
			public bool isValid
			{
				get
				{
					return this._mapCategoryId >= 0 && ReInput.mapping.GetMapCategory(this._mapCategoryId) != null;
				}
			}

			// Token: 0x06002E67 RID: 11879 RVA: 0x0001FB46 File Offset: 0x0001DD46
			public MappingSet()
			{
				this._mapCategoryId = -1;
				this._actionCategoryIds = new int[0];
				this._actionIds = new int[0];
				this._actionListMode = ControlMapper.MappingSet.ActionListMode.ActionCategory;
			}

			// Token: 0x06002E68 RID: 11880 RVA: 0x0001FB74 File Offset: 0x0001DD74
			private MappingSet(int mapCategoryId, ControlMapper.MappingSet.ActionListMode actionListMode, int[] actionCategoryIds, int[] actionIds)
			{
				this._mapCategoryId = mapCategoryId;
				this._actionListMode = actionListMode;
				this._actionCategoryIds = actionCategoryIds;
				this._actionIds = actionIds;
			}

			// Token: 0x17000797 RID: 1943
			// (get) Token: 0x06002E69 RID: 11881 RVA: 0x0001FB99 File Offset: 0x0001DD99
			public static ControlMapper.MappingSet Default
			{
				get
				{
					return new ControlMapper.MappingSet(0, ControlMapper.MappingSet.ActionListMode.ActionCategory, new int[1], new int[0]);
				}
			}

			// Token: 0x04002EBD RID: 11965
			[SerializeField]
			[Tooltip("The Map Category that will be displayed to the user for remapping.")]
			private int _mapCategoryId;

			// Token: 0x04002EBE RID: 11966
			[SerializeField]
			[Tooltip("Choose whether you want to list Actions to display for this Map Category by individual Action or by all the Actions in an Action Category.")]
			private ControlMapper.MappingSet.ActionListMode _actionListMode;

			// Token: 0x04002EBF RID: 11967
			[SerializeField]
			private int[] _actionCategoryIds;

			// Token: 0x04002EC0 RID: 11968
			[SerializeField]
			private int[] _actionIds;

			// Token: 0x04002EC1 RID: 11969
			private IList<int> _actionCategoryIdsReadOnly;

			// Token: 0x04002EC2 RID: 11970
			private IList<int> _actionIdsReadOnly;

			// Token: 0x02000665 RID: 1637
			public enum ActionListMode
			{
				// Token: 0x04002EC4 RID: 11972
				ActionCategory,
				// Token: 0x04002EC5 RID: 11973
				Action
			}
		}

		// Token: 0x02000666 RID: 1638
		[Serializable]
		public class InputBehaviorSettings
		{
			// Token: 0x17000798 RID: 1944
			// (get) Token: 0x06002E6A RID: 11882 RVA: 0x0001FBAE File Offset: 0x0001DDAE
			public int inputBehaviorId
			{
				get
				{
					return this._inputBehaviorId;
				}
			}

			// Token: 0x17000799 RID: 1945
			// (get) Token: 0x06002E6B RID: 11883 RVA: 0x0001FBB6 File Offset: 0x0001DDB6
			public bool showJoystickAxisSensitivity
			{
				get
				{
					return this._showJoystickAxisSensitivity;
				}
			}

			// Token: 0x1700079A RID: 1946
			// (get) Token: 0x06002E6C RID: 11884 RVA: 0x0001FBBE File Offset: 0x0001DDBE
			public bool showMouseXYAxisSensitivity
			{
				get
				{
					return this._showMouseXYAxisSensitivity;
				}
			}

			// Token: 0x1700079B RID: 1947
			// (get) Token: 0x06002E6D RID: 11885 RVA: 0x0001FBC6 File Offset: 0x0001DDC6
			public string labelLanguageKey
			{
				get
				{
					return this._labelLanguageKey;
				}
			}

			// Token: 0x1700079C RID: 1948
			// (get) Token: 0x06002E6E RID: 11886 RVA: 0x0001FBCE File Offset: 0x0001DDCE
			public string joystickAxisSensitivityLabelLanguageKey
			{
				get
				{
					return this._joystickAxisSensitivityLabelLanguageKey;
				}
			}

			// Token: 0x1700079D RID: 1949
			// (get) Token: 0x06002E6F RID: 11887 RVA: 0x0001FBD6 File Offset: 0x0001DDD6
			public string mouseXYAxisSensitivityLabelLanguageKey
			{
				get
				{
					return this._mouseXYAxisSensitivityLabelLanguageKey;
				}
			}

			// Token: 0x1700079E RID: 1950
			// (get) Token: 0x06002E70 RID: 11888 RVA: 0x0001FBDE File Offset: 0x0001DDDE
			public Sprite joystickAxisSensitivityIcon
			{
				get
				{
					return this._joystickAxisSensitivityIcon;
				}
			}

			// Token: 0x1700079F RID: 1951
			// (get) Token: 0x06002E71 RID: 11889 RVA: 0x0001FBE6 File Offset: 0x0001DDE6
			public Sprite mouseXYAxisSensitivityIcon
			{
				get
				{
					return this._mouseXYAxisSensitivityIcon;
				}
			}

			// Token: 0x170007A0 RID: 1952
			// (get) Token: 0x06002E72 RID: 11890 RVA: 0x0001FBEE File Offset: 0x0001DDEE
			public float joystickAxisSensitivityMin
			{
				get
				{
					return this._joystickAxisSensitivityMin;
				}
			}

			// Token: 0x170007A1 RID: 1953
			// (get) Token: 0x06002E73 RID: 11891 RVA: 0x0001FBF6 File Offset: 0x0001DDF6
			public float joystickAxisSensitivityMax
			{
				get
				{
					return this._joystickAxisSensitivityMax;
				}
			}

			// Token: 0x170007A2 RID: 1954
			// (get) Token: 0x06002E74 RID: 11892 RVA: 0x0001FBFE File Offset: 0x0001DDFE
			public float mouseXYAxisSensitivityMin
			{
				get
				{
					return this._mouseXYAxisSensitivityMin;
				}
			}

			// Token: 0x170007A3 RID: 1955
			// (get) Token: 0x06002E75 RID: 11893 RVA: 0x0001FC06 File Offset: 0x0001DE06
			public float mouseXYAxisSensitivityMax
			{
				get
				{
					return this._mouseXYAxisSensitivityMax;
				}
			}

			// Token: 0x170007A4 RID: 1956
			// (get) Token: 0x06002E76 RID: 11894 RVA: 0x0001FC0E File Offset: 0x0001DE0E
			public bool isValid
			{
				get
				{
					return this._inputBehaviorId >= 0 && (this._showJoystickAxisSensitivity || this._showMouseXYAxisSensitivity);
				}
			}

			// Token: 0x04002EC6 RID: 11974
			[SerializeField]
			[Tooltip("The Input Behavior that will be displayed to the user for modification.")]
			private int _inputBehaviorId = -1;

			// Token: 0x04002EC7 RID: 11975
			[SerializeField]
			[Tooltip("If checked, a slider will be displayed so the user can change this value.")]
			private bool _showJoystickAxisSensitivity = true;

			// Token: 0x04002EC8 RID: 11976
			[SerializeField]
			[Tooltip("If checked, a slider will be displayed so the user can change this value.")]
			private bool _showMouseXYAxisSensitivity = true;

			// Token: 0x04002EC9 RID: 11977
			[SerializeField]
			[Tooltip("If set to a non-blank value, this key will be used to look up the name in Language to be displayed as the title for the Input Behavior control set. Otherwise, the name field of the InputBehavior will be used.")]
			private string _labelLanguageKey = string.Empty;

			// Token: 0x04002ECA RID: 11978
			[SerializeField]
			[Tooltip("If set to a non-blank value, this name will be displayed above the individual slider control. Otherwise, no name will be displayed.")]
			private string _joystickAxisSensitivityLabelLanguageKey = string.Empty;

			// Token: 0x04002ECB RID: 11979
			[SerializeField]
			[Tooltip("If set to a non-blank value, this key will be used to look up the name in Language to be displayed above the individual slider control. Otherwise, no name will be displayed.")]
			private string _mouseXYAxisSensitivityLabelLanguageKey = string.Empty;

			// Token: 0x04002ECC RID: 11980
			[SerializeField]
			[Tooltip("The icon to display next to the slider. Set to none for no icon.")]
			private Sprite _joystickAxisSensitivityIcon;

			// Token: 0x04002ECD RID: 11981
			[SerializeField]
			[Tooltip("The icon to display next to the slider. Set to none for no icon.")]
			private Sprite _mouseXYAxisSensitivityIcon;

			// Token: 0x04002ECE RID: 11982
			[SerializeField]
			[Tooltip("Minimum value the user is allowed to set for this property.")]
			private float _joystickAxisSensitivityMin;

			// Token: 0x04002ECF RID: 11983
			[SerializeField]
			[Tooltip("Maximum value the user is allowed to set for this property.")]
			private float _joystickAxisSensitivityMax = 2f;

			// Token: 0x04002ED0 RID: 11984
			[SerializeField]
			[Tooltip("Minimum value the user is allowed to set for this property.")]
			private float _mouseXYAxisSensitivityMin;

			// Token: 0x04002ED1 RID: 11985
			[SerializeField]
			[Tooltip("Maximum value the user is allowed to set for this property.")]
			private float _mouseXYAxisSensitivityMax = 2f;
		}

		// Token: 0x02000667 RID: 1639
		[Serializable]
		private class Prefabs
		{
			// Token: 0x170007A5 RID: 1957
			// (get) Token: 0x06002E78 RID: 11896 RVA: 0x0001FC2B File Offset: 0x0001DE2B
			public GameObject button
			{
				get
				{
					return this._button;
				}
			}

			// Token: 0x170007A6 RID: 1958
			// (get) Token: 0x06002E79 RID: 11897 RVA: 0x0001FC33 File Offset: 0x0001DE33
			public GameObject fitButton
			{
				get
				{
					return this._fitButton;
				}
			}

			// Token: 0x170007A7 RID: 1959
			// (get) Token: 0x06002E7A RID: 11898 RVA: 0x0001FC3B File Offset: 0x0001DE3B
			public GameObject inputGridLabel
			{
				get
				{
					return this._inputGridLabel;
				}
			}

			// Token: 0x170007A8 RID: 1960
			// (get) Token: 0x06002E7B RID: 11899 RVA: 0x0001FC43 File Offset: 0x0001DE43
			public GameObject inputGridHeaderLabel
			{
				get
				{
					return this._inputGridHeaderLabel;
				}
			}

			// Token: 0x170007A9 RID: 1961
			// (get) Token: 0x06002E7C RID: 11900 RVA: 0x0001FC4B File Offset: 0x0001DE4B
			public GameObject inputGridFieldButton
			{
				get
				{
					return this._inputGridFieldButton;
				}
			}

			// Token: 0x170007AA RID: 1962
			// (get) Token: 0x06002E7D RID: 11901 RVA: 0x0001FC53 File Offset: 0x0001DE53
			public GameObject inputGridFieldInvertToggle
			{
				get
				{
					return this._inputGridFieldInvertToggle;
				}
			}

			// Token: 0x170007AB RID: 1963
			// (get) Token: 0x06002E7E RID: 11902 RVA: 0x0001FC5B File Offset: 0x0001DE5B
			public GameObject window
			{
				get
				{
					return this._window;
				}
			}

			// Token: 0x170007AC RID: 1964
			// (get) Token: 0x06002E7F RID: 11903 RVA: 0x0001FC63 File Offset: 0x0001DE63
			public GameObject windowTitleText
			{
				get
				{
					return this._windowTitleText;
				}
			}

			// Token: 0x170007AD RID: 1965
			// (get) Token: 0x06002E80 RID: 11904 RVA: 0x0001FC6B File Offset: 0x0001DE6B
			public GameObject windowContentText
			{
				get
				{
					return this._windowContentText;
				}
			}

			// Token: 0x170007AE RID: 1966
			// (get) Token: 0x06002E81 RID: 11905 RVA: 0x0001FC73 File Offset: 0x0001DE73
			public GameObject fader
			{
				get
				{
					return this._fader;
				}
			}

			// Token: 0x170007AF RID: 1967
			// (get) Token: 0x06002E82 RID: 11906 RVA: 0x0001FC7B File Offset: 0x0001DE7B
			public GameObject calibrationWindow
			{
				get
				{
					return this._calibrationWindow;
				}
			}

			// Token: 0x170007B0 RID: 1968
			// (get) Token: 0x06002E83 RID: 11907 RVA: 0x0001FC83 File Offset: 0x0001DE83
			public GameObject inputBehaviorsWindow
			{
				get
				{
					return this._inputBehaviorsWindow;
				}
			}

			// Token: 0x170007B1 RID: 1969
			// (get) Token: 0x06002E84 RID: 11908 RVA: 0x0001FC8B File Offset: 0x0001DE8B
			public GameObject centerStickGraphic
			{
				get
				{
					return this._centerStickGraphic;
				}
			}

			// Token: 0x170007B2 RID: 1970
			// (get) Token: 0x06002E85 RID: 11909 RVA: 0x0001FC93 File Offset: 0x0001DE93
			public GameObject moveStickGraphic
			{
				get
				{
					return this._moveStickGraphic;
				}
			}

			// Token: 0x06002E86 RID: 11910 RVA: 0x000FEED4 File Offset: 0x000FD0D4
			public bool Check()
			{
				return !(this._button == null) && !(this._fitButton == null) && !(this._inputGridLabel == null) && !(this._inputGridHeaderLabel == null) && !(this._inputGridFieldButton == null) && !(this._inputGridFieldInvertToggle == null) && !(this._window == null) && !(this._windowTitleText == null) && !(this._windowContentText == null) && !(this._fader == null) && !(this._calibrationWindow == null) && !(this._inputBehaviorsWindow == null);
			}

			// Token: 0x04002ED2 RID: 11986
			[SerializeField]
			private GameObject _button;

			// Token: 0x04002ED3 RID: 11987
			[SerializeField]
			private GameObject _fitButton;

			// Token: 0x04002ED4 RID: 11988
			[SerializeField]
			private GameObject _inputGridLabel;

			// Token: 0x04002ED5 RID: 11989
			[SerializeField]
			private GameObject _inputGridHeaderLabel;

			// Token: 0x04002ED6 RID: 11990
			[SerializeField]
			private GameObject _inputGridFieldButton;

			// Token: 0x04002ED7 RID: 11991
			[SerializeField]
			private GameObject _inputGridFieldInvertToggle;

			// Token: 0x04002ED8 RID: 11992
			[SerializeField]
			private GameObject _window;

			// Token: 0x04002ED9 RID: 11993
			[SerializeField]
			private GameObject _windowTitleText;

			// Token: 0x04002EDA RID: 11994
			[SerializeField]
			private GameObject _windowContentText;

			// Token: 0x04002EDB RID: 11995
			[SerializeField]
			private GameObject _fader;

			// Token: 0x04002EDC RID: 11996
			[SerializeField]
			private GameObject _calibrationWindow;

			// Token: 0x04002EDD RID: 11997
			[SerializeField]
			private GameObject _inputBehaviorsWindow;

			// Token: 0x04002EDE RID: 11998
			[SerializeField]
			private GameObject _centerStickGraphic;

			// Token: 0x04002EDF RID: 11999
			[SerializeField]
			private GameObject _moveStickGraphic;
		}

		// Token: 0x02000668 RID: 1640
		[Serializable]
		private class References
		{
			// Token: 0x170007B3 RID: 1971
			// (get) Token: 0x06002E88 RID: 11912 RVA: 0x0001FC9B File Offset: 0x0001DE9B
			public Canvas canvas
			{
				get
				{
					return this._canvas;
				}
			}

			// Token: 0x170007B4 RID: 1972
			// (get) Token: 0x06002E89 RID: 11913 RVA: 0x0001FCA3 File Offset: 0x0001DEA3
			public CanvasGroup mainCanvasGroup
			{
				get
				{
					return this._mainCanvasGroup;
				}
			}

			// Token: 0x170007B5 RID: 1973
			// (get) Token: 0x06002E8A RID: 11914 RVA: 0x0001FCAB File Offset: 0x0001DEAB
			public Transform mainContent
			{
				get
				{
					return this._mainContent;
				}
			}

			// Token: 0x170007B6 RID: 1974
			// (get) Token: 0x06002E8B RID: 11915 RVA: 0x0001FCB3 File Offset: 0x0001DEB3
			public Transform mainContentInner
			{
				get
				{
					return this._mainContentInner;
				}
			}

			// Token: 0x170007B7 RID: 1975
			// (get) Token: 0x06002E8C RID: 11916 RVA: 0x0001FCBB File Offset: 0x0001DEBB
			public UIGroup playersGroup
			{
				get
				{
					return this._playersGroup;
				}
			}

			// Token: 0x170007B8 RID: 1976
			// (get) Token: 0x06002E8D RID: 11917 RVA: 0x0001FCC3 File Offset: 0x0001DEC3
			public Transform controllerGroup
			{
				get
				{
					return this._controllerGroup;
				}
			}

			// Token: 0x170007B9 RID: 1977
			// (get) Token: 0x06002E8E RID: 11918 RVA: 0x0001FCCB File Offset: 0x0001DECB
			public Transform controllerGroupLabelGroup
			{
				get
				{
					return this._controllerGroupLabelGroup;
				}
			}

			// Token: 0x170007BA RID: 1978
			// (get) Token: 0x06002E8F RID: 11919 RVA: 0x0001FCD3 File Offset: 0x0001DED3
			public UIGroup controllerSettingsGroup
			{
				get
				{
					return this._controllerSettingsGroup;
				}
			}

			// Token: 0x170007BB RID: 1979
			// (get) Token: 0x06002E90 RID: 11920 RVA: 0x0001FCDB File Offset: 0x0001DEDB
			public UIGroup assignedControllersGroup
			{
				get
				{
					return this._assignedControllersGroup;
				}
			}

			// Token: 0x170007BC RID: 1980
			// (get) Token: 0x06002E91 RID: 11921 RVA: 0x0001FCE3 File Offset: 0x0001DEE3
			public Transform settingsAndMapCategoriesGroup
			{
				get
				{
					return this._settingsAndMapCategoriesGroup;
				}
			}

			// Token: 0x170007BD RID: 1981
			// (get) Token: 0x06002E92 RID: 11922 RVA: 0x0001FCEB File Offset: 0x0001DEEB
			public UIGroup settingsGroup
			{
				get
				{
					return this._settingsGroup;
				}
			}

			// Token: 0x170007BE RID: 1982
			// (get) Token: 0x06002E93 RID: 11923 RVA: 0x0001FCF3 File Offset: 0x0001DEF3
			public UIGroup mapCategoriesGroup
			{
				get
				{
					return this._mapCategoriesGroup;
				}
			}

			// Token: 0x170007BF RID: 1983
			// (get) Token: 0x06002E94 RID: 11924 RVA: 0x0001FCFB File Offset: 0x0001DEFB
			public Transform inputGridGroup
			{
				get
				{
					return this._inputGridGroup;
				}
			}

			// Token: 0x170007C0 RID: 1984
			// (get) Token: 0x06002E95 RID: 11925 RVA: 0x0001FD03 File Offset: 0x0001DF03
			public Transform inputGridContainer
			{
				get
				{
					return this._inputGridContainer;
				}
			}

			// Token: 0x170007C1 RID: 1985
			// (get) Token: 0x06002E96 RID: 11926 RVA: 0x0001FD0B File Offset: 0x0001DF0B
			public Transform inputGridHeadersGroup
			{
				get
				{
					return this._inputGridHeadersGroup;
				}
			}

			// Token: 0x170007C2 RID: 1986
			// (get) Token: 0x06002E97 RID: 11927 RVA: 0x0001FD13 File Offset: 0x0001DF13
			public Scrollbar inputGridVScrollbar
			{
				get
				{
					return this._inputGridVScrollbar;
				}
			}

			// Token: 0x170007C3 RID: 1987
			// (get) Token: 0x06002E98 RID: 11928 RVA: 0x0001FD1B File Offset: 0x0001DF1B
			public ScrollRect inputGridScrollRect
			{
				get
				{
					return this._inputGridScrollRect;
				}
			}

			// Token: 0x170007C4 RID: 1988
			// (get) Token: 0x06002E99 RID: 11929 RVA: 0x0001FD23 File Offset: 0x0001DF23
			public Transform inputGridInnerGroup
			{
				get
				{
					return this._inputGridInnerGroup;
				}
			}

			// Token: 0x170007C5 RID: 1989
			// (get) Token: 0x06002E9A RID: 11930 RVA: 0x0001FD2B File Offset: 0x0001DF2B
			public Text controllerNameLabel
			{
				get
				{
					return this._controllerNameLabel;
				}
			}

			// Token: 0x170007C6 RID: 1990
			// (get) Token: 0x06002E9B RID: 11931 RVA: 0x0001FD33 File Offset: 0x0001DF33
			public Button removeControllerButton
			{
				get
				{
					return this._removeControllerButton;
				}
			}

			// Token: 0x170007C7 RID: 1991
			// (get) Token: 0x06002E9C RID: 11932 RVA: 0x0001FD3B File Offset: 0x0001DF3B
			public Button assignControllerButton
			{
				get
				{
					return this._assignControllerButton;
				}
			}

			// Token: 0x170007C8 RID: 1992
			// (get) Token: 0x06002E9D RID: 11933 RVA: 0x0001FD43 File Offset: 0x0001DF43
			public Button calibrateControllerButton
			{
				get
				{
					return this._calibrateControllerButton;
				}
			}

			// Token: 0x170007C9 RID: 1993
			// (get) Token: 0x06002E9E RID: 11934 RVA: 0x0001FD4B File Offset: 0x0001DF4B
			public Button doneButton
			{
				get
				{
					return this._doneButton;
				}
			}

			// Token: 0x170007CA RID: 1994
			// (get) Token: 0x06002E9F RID: 11935 RVA: 0x0001FD53 File Offset: 0x0001DF53
			public Button restoreDefaultsButton
			{
				get
				{
					return this._restoreDefaultsButton;
				}
			}

			// Token: 0x170007CB RID: 1995
			// (get) Token: 0x06002EA0 RID: 11936 RVA: 0x0001FD5B File Offset: 0x0001DF5B
			public Selectable defaultSelection
			{
				get
				{
					return this._defaultSelection;
				}
			}

			// Token: 0x170007CC RID: 1996
			// (get) Token: 0x06002EA1 RID: 11937 RVA: 0x0001FD63 File Offset: 0x0001DF63
			public GameObject[] fixedSelectableUIElements
			{
				get
				{
					return this._fixedSelectableUIElements;
				}
			}

			// Token: 0x170007CD RID: 1997
			// (get) Token: 0x06002EA2 RID: 11938 RVA: 0x0001FD6B File Offset: 0x0001DF6B
			public Image mainBackgroundImage
			{
				get
				{
					return this._mainBackgroundImage;
				}
			}

			// Token: 0x170007CE RID: 1998
			// (get) Token: 0x06002EA3 RID: 11939 RVA: 0x0001FD73 File Offset: 0x0001DF73
			// (set) Token: 0x06002EA4 RID: 11940 RVA: 0x0001FD7B File Offset: 0x0001DF7B
			public LayoutElement inputGridLayoutElement { get; set; }

			// Token: 0x170007CF RID: 1999
			// (get) Token: 0x06002EA5 RID: 11941 RVA: 0x0001FD84 File Offset: 0x0001DF84
			// (set) Token: 0x06002EA6 RID: 11942 RVA: 0x0001FD8C File Offset: 0x0001DF8C
			public Transform inputGridActionColumn { get; set; }

			// Token: 0x170007D0 RID: 2000
			// (get) Token: 0x06002EA7 RID: 11943 RVA: 0x0001FD95 File Offset: 0x0001DF95
			// (set) Token: 0x06002EA8 RID: 11944 RVA: 0x0001FD9D File Offset: 0x0001DF9D
			public Transform inputGridKeyboardColumn { get; set; }

			// Token: 0x170007D1 RID: 2001
			// (get) Token: 0x06002EA9 RID: 11945 RVA: 0x0001FDA6 File Offset: 0x0001DFA6
			// (set) Token: 0x06002EAA RID: 11946 RVA: 0x0001FDAE File Offset: 0x0001DFAE
			public Transform inputGridMouseColumn { get; set; }

			// Token: 0x170007D2 RID: 2002
			// (get) Token: 0x06002EAB RID: 11947 RVA: 0x0001FDB7 File Offset: 0x0001DFB7
			// (set) Token: 0x06002EAC RID: 11948 RVA: 0x0001FDBF File Offset: 0x0001DFBF
			public Transform inputGridControllerColumn { get; set; }

			// Token: 0x170007D3 RID: 2003
			// (get) Token: 0x06002EAD RID: 11949 RVA: 0x0001FDC8 File Offset: 0x0001DFC8
			// (set) Token: 0x06002EAE RID: 11950 RVA: 0x0001FDD0 File Offset: 0x0001DFD0
			public Transform inputGridHeader1 { get; set; }

			// Token: 0x170007D4 RID: 2004
			// (get) Token: 0x06002EAF RID: 11951 RVA: 0x0001FDD9 File Offset: 0x0001DFD9
			// (set) Token: 0x06002EB0 RID: 11952 RVA: 0x0001FDE1 File Offset: 0x0001DFE1
			public Transform inputGridHeader2 { get; set; }

			// Token: 0x170007D5 RID: 2005
			// (get) Token: 0x06002EB1 RID: 11953 RVA: 0x0001FDEA File Offset: 0x0001DFEA
			// (set) Token: 0x06002EB2 RID: 11954 RVA: 0x0001FDF2 File Offset: 0x0001DFF2
			public Transform inputGridHeader3 { get; set; }

			// Token: 0x170007D6 RID: 2006
			// (get) Token: 0x06002EB3 RID: 11955 RVA: 0x0001FDFB File Offset: 0x0001DFFB
			// (set) Token: 0x06002EB4 RID: 11956 RVA: 0x0001FE03 File Offset: 0x0001E003
			public Transform inputGridHeader4 { get; set; }

			// Token: 0x06002EB5 RID: 11957 RVA: 0x000FEF94 File Offset: 0x000FD194
			public bool Check()
			{
				return !(this._canvas == null) && !(this._mainCanvasGroup == null) && !(this._mainContent == null) && !(this._mainContentInner == null) && !(this._playersGroup == null) && !(this._controllerGroup == null) && !(this._controllerGroupLabelGroup == null) && !(this._controllerSettingsGroup == null) && !(this._assignedControllersGroup == null) && !(this._settingsAndMapCategoriesGroup == null) && !(this._settingsGroup == null) && !(this._mapCategoriesGroup == null) && !(this._inputGridGroup == null) && !(this._inputGridContainer == null) && !(this._inputGridHeadersGroup == null) && !(this._inputGridVScrollbar == null) && !(this._inputGridScrollRect == null) && !(this._inputGridInnerGroup == null) && !(this._controllerNameLabel == null) && !(this._removeControllerButton == null) && !(this._assignControllerButton == null) && !(this._calibrateControllerButton == null) && !(this._doneButton == null) && !(this._restoreDefaultsButton == null) && !(this._defaultSelection == null);
			}

			// Token: 0x04002EE0 RID: 12000
			[SerializeField]
			private Canvas _canvas;

			// Token: 0x04002EE1 RID: 12001
			[SerializeField]
			private CanvasGroup _mainCanvasGroup;

			// Token: 0x04002EE2 RID: 12002
			[SerializeField]
			private Transform _mainContent;

			// Token: 0x04002EE3 RID: 12003
			[SerializeField]
			private Transform _mainContentInner;

			// Token: 0x04002EE4 RID: 12004
			[SerializeField]
			private UIGroup _playersGroup;

			// Token: 0x04002EE5 RID: 12005
			[SerializeField]
			private Transform _controllerGroup;

			// Token: 0x04002EE6 RID: 12006
			[SerializeField]
			private Transform _controllerGroupLabelGroup;

			// Token: 0x04002EE7 RID: 12007
			[SerializeField]
			private UIGroup _controllerSettingsGroup;

			// Token: 0x04002EE8 RID: 12008
			[SerializeField]
			private UIGroup _assignedControllersGroup;

			// Token: 0x04002EE9 RID: 12009
			[SerializeField]
			private Transform _settingsAndMapCategoriesGroup;

			// Token: 0x04002EEA RID: 12010
			[SerializeField]
			private UIGroup _settingsGroup;

			// Token: 0x04002EEB RID: 12011
			[SerializeField]
			private UIGroup _mapCategoriesGroup;

			// Token: 0x04002EEC RID: 12012
			[SerializeField]
			private Transform _inputGridGroup;

			// Token: 0x04002EED RID: 12013
			[SerializeField]
			private Transform _inputGridContainer;

			// Token: 0x04002EEE RID: 12014
			[SerializeField]
			private Transform _inputGridHeadersGroup;

			// Token: 0x04002EEF RID: 12015
			[SerializeField]
			private Scrollbar _inputGridVScrollbar;

			// Token: 0x04002EF0 RID: 12016
			[SerializeField]
			private ScrollRect _inputGridScrollRect;

			// Token: 0x04002EF1 RID: 12017
			[SerializeField]
			private Transform _inputGridInnerGroup;

			// Token: 0x04002EF2 RID: 12018
			[SerializeField]
			private Text _controllerNameLabel;

			// Token: 0x04002EF3 RID: 12019
			[SerializeField]
			private Button _removeControllerButton;

			// Token: 0x04002EF4 RID: 12020
			[SerializeField]
			private Button _assignControllerButton;

			// Token: 0x04002EF5 RID: 12021
			[SerializeField]
			private Button _calibrateControllerButton;

			// Token: 0x04002EF6 RID: 12022
			[SerializeField]
			private Button _doneButton;

			// Token: 0x04002EF7 RID: 12023
			[SerializeField]
			private Button _restoreDefaultsButton;

			// Token: 0x04002EF8 RID: 12024
			[SerializeField]
			private Selectable _defaultSelection;

			// Token: 0x04002EF9 RID: 12025
			[SerializeField]
			private GameObject[] _fixedSelectableUIElements;

			// Token: 0x04002EFA RID: 12026
			[SerializeField]
			private Image _mainBackgroundImage;
		}

		// Token: 0x02000669 RID: 1641
		private class InputActionSet
		{
			// Token: 0x170007D7 RID: 2007
			// (get) Token: 0x06002EB7 RID: 11959 RVA: 0x0001FE0C File Offset: 0x0001E00C
			public int actionId
			{
				get
				{
					return this._actionId;
				}
			}

			// Token: 0x170007D8 RID: 2008
			// (get) Token: 0x06002EB8 RID: 11960 RVA: 0x0001FE14 File Offset: 0x0001E014
			public AxisRange axisRange
			{
				get
				{
					return this._axisRange;
				}
			}

			// Token: 0x06002EB9 RID: 11961 RVA: 0x0001FE1C File Offset: 0x0001E01C
			public InputActionSet(int actionId, AxisRange axisRange)
			{
				this._actionId = actionId;
				this._axisRange = axisRange;
			}

			// Token: 0x04002F04 RID: 12036
			private int _actionId;

			// Token: 0x04002F05 RID: 12037
			private AxisRange _axisRange;
		}

		// Token: 0x0200066A RID: 1642
		private class InputMapping
		{
			// Token: 0x170007D9 RID: 2009
			// (get) Token: 0x06002EBA RID: 11962 RVA: 0x0001FE32 File Offset: 0x0001E032
			// (set) Token: 0x06002EBB RID: 11963 RVA: 0x0001FE3A File Offset: 0x0001E03A
			public string actionName { get; private set; }

			// Token: 0x170007DA RID: 2010
			// (get) Token: 0x06002EBC RID: 11964 RVA: 0x0001FE43 File Offset: 0x0001E043
			// (set) Token: 0x06002EBD RID: 11965 RVA: 0x0001FE4B File Offset: 0x0001E04B
			public InputFieldInfo fieldInfo { get; private set; }

			// Token: 0x170007DB RID: 2011
			// (get) Token: 0x06002EBE RID: 11966 RVA: 0x0001FE54 File Offset: 0x0001E054
			// (set) Token: 0x06002EBF RID: 11967 RVA: 0x0001FE5C File Offset: 0x0001E05C
			public ControllerMap map { get; private set; }

			// Token: 0x170007DC RID: 2012
			// (get) Token: 0x06002EC0 RID: 11968 RVA: 0x0001FE65 File Offset: 0x0001E065
			// (set) Token: 0x06002EC1 RID: 11969 RVA: 0x0001FE6D File Offset: 0x0001E06D
			public ActionElementMap aem { get; private set; }

			// Token: 0x170007DD RID: 2013
			// (get) Token: 0x06002EC2 RID: 11970 RVA: 0x0001FE76 File Offset: 0x0001E076
			// (set) Token: 0x06002EC3 RID: 11971 RVA: 0x0001FE7E File Offset: 0x0001E07E
			public ControllerType controllerType { get; private set; }

			// Token: 0x170007DE RID: 2014
			// (get) Token: 0x06002EC4 RID: 11972 RVA: 0x0001FE87 File Offset: 0x0001E087
			// (set) Token: 0x06002EC5 RID: 11973 RVA: 0x0001FE8F File Offset: 0x0001E08F
			public int controllerId { get; private set; }

			// Token: 0x170007DF RID: 2015
			// (get) Token: 0x06002EC6 RID: 11974 RVA: 0x0001FE98 File Offset: 0x0001E098
			// (set) Token: 0x06002EC7 RID: 11975 RVA: 0x0001FEA0 File Offset: 0x0001E0A0
			public ControllerPollingInfo pollingInfo { get; set; }

			// Token: 0x170007E0 RID: 2016
			// (get) Token: 0x06002EC8 RID: 11976 RVA: 0x0001FEA9 File Offset: 0x0001E0A9
			// (set) Token: 0x06002EC9 RID: 11977 RVA: 0x0001FEB1 File Offset: 0x0001E0B1
			public ModifierKeyFlags modifierKeyFlags { get; set; }

			// Token: 0x170007E1 RID: 2017
			// (get) Token: 0x06002ECA RID: 11978 RVA: 0x000FF130 File Offset: 0x000FD330
			public AxisRange axisRange
			{
				get
				{
					AxisRange result = AxisRange.Positive;
					if (this.pollingInfo.elementType == ControllerElementType.Axis)
					{
						if (this.fieldInfo.axisRange == AxisRange.Full)
						{
							result = AxisRange.Full;
						}
						else
						{
							result = ((this.pollingInfo.axisPole == Pole.Positive) ? AxisRange.Positive : AxisRange.Negative);
						}
					}
					return result;
				}
			}

			// Token: 0x170007E2 RID: 2018
			// (get) Token: 0x06002ECB RID: 11979 RVA: 0x000FF178 File Offset: 0x000FD378
			public string elementName
			{
				get
				{
					if (this.controllerType == ControllerType.Keyboard)
					{
						return ControlMapper.GetLanguage().GetElementIdentifierName(this.pollingInfo.keyboardKey, this.modifierKeyFlags);
					}
					return ControlMapper.GetLanguage().GetElementIdentifierName(this.pollingInfo.controller, this.pollingInfo.elementIdentifierId, (this.pollingInfo.axisPole == Pole.Positive) ? AxisRange.Positive : AxisRange.Negative);
				}
			}

			// Token: 0x06002ECC RID: 11980 RVA: 0x0001FEBA File Offset: 0x0001E0BA
			public InputMapping(string actionName, InputFieldInfo fieldInfo, ControllerMap map, ActionElementMap aem, ControllerType controllerType, int controllerId)
			{
				this.actionName = actionName;
				this.fieldInfo = fieldInfo;
				this.map = map;
				this.aem = aem;
				this.controllerType = controllerType;
				this.controllerId = controllerId;
			}

			// Token: 0x06002ECD RID: 11981 RVA: 0x0001FEEF File Offset: 0x0001E0EF
			public ElementAssignment ToElementAssignment(ControllerPollingInfo pollingInfo)
			{
				this.pollingInfo = pollingInfo;
				return this.ToElementAssignment();
			}

			// Token: 0x06002ECE RID: 11982 RVA: 0x0001FEFE File Offset: 0x0001E0FE
			public ElementAssignment ToElementAssignment(ControllerPollingInfo pollingInfo, ModifierKeyFlags modifierKeyFlags)
			{
				this.pollingInfo = pollingInfo;
				this.modifierKeyFlags = modifierKeyFlags;
				return this.ToElementAssignment();
			}

			// Token: 0x06002ECF RID: 11983 RVA: 0x000FF1E8 File Offset: 0x000FD3E8
			public ElementAssignment ToElementAssignment()
			{
				return new ElementAssignment(this.controllerType, this.pollingInfo.elementType, this.pollingInfo.elementIdentifierId, this.axisRange, this.pollingInfo.keyboardKey, this.modifierKeyFlags, this.fieldInfo.actionId, (this.fieldInfo.axisRange == AxisRange.Negative) ? Pole.Negative : Pole.Positive, false, (this.aem != null) ? this.aem.id : -1);
			}
		}

		// Token: 0x0200066B RID: 1643
		private class AxisCalibrator
		{
			// Token: 0x170007E3 RID: 2019
			// (get) Token: 0x06002ED0 RID: 11984 RVA: 0x0001FF14 File Offset: 0x0001E114
			public bool isValid
			{
				get
				{
					return this.axis != null;
				}
			}

			// Token: 0x06002ED1 RID: 11985 RVA: 0x000FF26C File Offset: 0x000FD46C
			public AxisCalibrator(Joystick joystick, int axisIndex)
			{
				this.data = default(AxisCalibrationData);
				this.joystick = joystick;
				this.axisIndex = axisIndex;
				if (joystick != null && axisIndex >= 0 && joystick.axisCount > axisIndex)
				{
					this.axis = joystick.Axes[axisIndex];
					this.data = joystick.calibrationMap.GetAxis(axisIndex).GetData();
				}
				this.firstRun = true;
			}

			// Token: 0x06002ED2 RID: 11986 RVA: 0x000FF2DC File Offset: 0x000FD4DC
			public void RecordMinMax()
			{
				if (this.axis == null)
				{
					return;
				}
				float valueRaw = this.axis.valueRaw;
				if (this.firstRun || valueRaw < this.data.min)
				{
					this.data.min = valueRaw;
				}
				if (this.firstRun || valueRaw > this.data.max)
				{
					this.data.max = valueRaw;
				}
				this.firstRun = false;
			}

			// Token: 0x06002ED3 RID: 11987 RVA: 0x0001FF1F File Offset: 0x0001E11F
			public void RecordZero()
			{
				if (this.axis == null)
				{
					return;
				}
				this.data.zero = this.axis.valueRaw;
			}

			// Token: 0x06002ED4 RID: 11988 RVA: 0x000FF34C File Offset: 0x000FD54C
			public void Commit()
			{
				if (this.axis == null)
				{
					return;
				}
				AxisCalibration axisCalibration = this.joystick.calibrationMap.GetAxis(this.axisIndex);
				if (axisCalibration == null)
				{
					return;
				}
				if ((double)Mathf.Abs(this.data.max - this.data.min) < 0.1)
				{
					return;
				}
				axisCalibration.SetData(this.data);
			}

			// Token: 0x04002F0E RID: 12046
			public AxisCalibrationData data;

			// Token: 0x04002F0F RID: 12047
			public readonly Joystick joystick;

			// Token: 0x04002F10 RID: 12048
			public readonly int axisIndex;

			// Token: 0x04002F11 RID: 12049
			private Controller.Axis axis;

			// Token: 0x04002F12 RID: 12050
			private bool firstRun;
		}

		// Token: 0x0200066C RID: 1644
		private class IndexedDictionary<TKey, TValue>
		{
			// Token: 0x170007E4 RID: 2020
			// (get) Token: 0x06002ED5 RID: 11989 RVA: 0x0001FF40 File Offset: 0x0001E140
			public int Count
			{
				get
				{
					return this.list.Count;
				}
			}

			// Token: 0x06002ED6 RID: 11990 RVA: 0x0001FF4D File Offset: 0x0001E14D
			public IndexedDictionary()
			{
				this.list = new List<ControlMapper.IndexedDictionary<TKey, TValue>.Entry>();
			}

			// Token: 0x170007E5 RID: 2021
			public TValue this[int index]
			{
				get
				{
					return this.list[index].value;
				}
			}

			// Token: 0x06002ED8 RID: 11992 RVA: 0x000FF3B4 File Offset: 0x000FD5B4
			public TValue Get(TKey key)
			{
				int num = this.IndexOfKey(key);
				if (num < 0)
				{
					throw new Exception("Key does not exist!");
				}
				return this.list[num].value;
			}

			// Token: 0x06002ED9 RID: 11993 RVA: 0x000FF3EC File Offset: 0x000FD5EC
			public bool TryGet(TKey key, out TValue value)
			{
				value = default(TValue);
				int num = this.IndexOfKey(key);
				if (num < 0)
				{
					return false;
				}
				value = this.list[num].value;
				return true;
			}

			// Token: 0x06002EDA RID: 11994 RVA: 0x0001FF73 File Offset: 0x0001E173
			public void Add(TKey key, TValue value)
			{
				if (this.ContainsKey(key))
				{
					throw new Exception("Key " + key.ToString() + " is already in use!");
				}
				this.list.Add(new ControlMapper.IndexedDictionary<TKey, TValue>.Entry(key, value));
			}

			// Token: 0x06002EDB RID: 11995 RVA: 0x000FF428 File Offset: 0x000FD628
			public int IndexOfKey(TKey key)
			{
				int count = this.list.Count;
				for (int i = 0; i < count; i++)
				{
					if (EqualityComparer<TKey>.Default.Equals(this.list[i].key, key))
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06002EDC RID: 11996 RVA: 0x000FF470 File Offset: 0x000FD670
			public bool ContainsKey(TKey key)
			{
				int count = this.list.Count;
				for (int i = 0; i < count; i++)
				{
					if (EqualityComparer<TKey>.Default.Equals(this.list[i].key, key))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06002EDD RID: 11997 RVA: 0x0001FFB2 File Offset: 0x0001E1B2
			public void Clear()
			{
				this.list.Clear();
			}

			// Token: 0x04002F13 RID: 12051
			private List<ControlMapper.IndexedDictionary<TKey, TValue>.Entry> list;

			// Token: 0x0200066D RID: 1645
			private class Entry
			{
				// Token: 0x06002EDE RID: 11998 RVA: 0x0001FFBF File Offset: 0x0001E1BF
				public Entry(TKey key, TValue value)
				{
					this.key = key;
					this.value = value;
				}

				// Token: 0x04002F14 RID: 12052
				public TKey key;

				// Token: 0x04002F15 RID: 12053
				public TValue value;
			}
		}

		// Token: 0x0200066E RID: 1646
		private enum LayoutElementSizeType
		{
			// Token: 0x04002F17 RID: 12055
			MinSize,
			// Token: 0x04002F18 RID: 12056
			PreferredSize
		}

		// Token: 0x0200066F RID: 1647
		private enum WindowType
		{
			// Token: 0x04002F1A RID: 12058
			None,
			// Token: 0x04002F1B RID: 12059
			ChooseJoystick,
			// Token: 0x04002F1C RID: 12060
			JoystickAssignmentConflict,
			// Token: 0x04002F1D RID: 12061
			ElementAssignment,
			// Token: 0x04002F1E RID: 12062
			ElementAssignmentPrePolling,
			// Token: 0x04002F1F RID: 12063
			ElementAssignmentPolling,
			// Token: 0x04002F20 RID: 12064
			ElementAssignmentResult,
			// Token: 0x04002F21 RID: 12065
			ElementAssignmentConflict,
			// Token: 0x04002F22 RID: 12066
			Calibration,
			// Token: 0x04002F23 RID: 12067
			CalibrateStep1,
			// Token: 0x04002F24 RID: 12068
			CalibrateStep2
		}

		// Token: 0x02000670 RID: 1648
		private class InputGrid
		{
			// Token: 0x06002EDF RID: 11999 RVA: 0x0001FFD5 File Offset: 0x0001E1D5
			public InputGrid()
			{
				this.list = new ControlMapper.InputGridEntryList();
				this.groups = new List<GameObject>();
			}

			// Token: 0x06002EE0 RID: 12000 RVA: 0x0001FFF3 File Offset: 0x0001E1F3
			public void AddMapCategory(int mapCategoryId)
			{
				this.list.AddMapCategory(mapCategoryId);
			}

			// Token: 0x06002EE1 RID: 12001 RVA: 0x00020001 File Offset: 0x0001E201
			public void AddAction(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				this.list.AddAction(mapCategoryId, action, axisRange);
			}

			// Token: 0x06002EE2 RID: 12002 RVA: 0x00020011 File Offset: 0x0001E211
			public void AddActionCategory(int mapCategoryId, int actionCategoryId)
			{
				this.list.AddActionCategory(mapCategoryId, actionCategoryId);
			}

			// Token: 0x06002EE3 RID: 12003 RVA: 0x00020020 File Offset: 0x0001E220
			public void AddInputFieldSet(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, GameObject fieldSetContainer)
			{
				this.list.AddInputFieldSet(mapCategoryId, action, axisRange, controllerType, fieldSetContainer);
			}

			// Token: 0x06002EE4 RID: 12004 RVA: 0x00020034 File Offset: 0x0001E234
			public void AddInputField(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, int fieldIndex, ControlMapper.GUIInputField inputField)
			{
				this.list.AddInputField(mapCategoryId, action, axisRange, controllerType, fieldIndex, inputField);
			}

			// Token: 0x06002EE5 RID: 12005 RVA: 0x0002004A File Offset: 0x0001E24A
			public void AddGroup(GameObject group)
			{
				this.groups.Add(group);
			}

			// Token: 0x06002EE6 RID: 12006 RVA: 0x00020058 File Offset: 0x0001E258
			public void AddActionLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControlMapper.GUILabel label)
			{
				this.list.AddActionLabel(mapCategoryId, actionId, axisRange, label);
			}

			// Token: 0x06002EE7 RID: 12007 RVA: 0x0002006A File Offset: 0x0001E26A
			public void AddActionCategoryLabel(int mapCategoryId, int actionCategoryId, ControlMapper.GUILabel label)
			{
				this.list.AddActionCategoryLabel(mapCategoryId, actionCategoryId, label);
			}

			// Token: 0x06002EE8 RID: 12008 RVA: 0x0002007A File Offset: 0x0001E27A
			public bool Contains(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				return this.list.Contains(mapCategoryId, actionId, axisRange, controllerType, fieldIndex);
			}

			// Token: 0x06002EE9 RID: 12009 RVA: 0x0002008E File Offset: 0x0001E28E
			public ControlMapper.GUIInputField GetGUIInputField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				return this.list.GetGUIInputField(mapCategoryId, actionId, axisRange, controllerType, fieldIndex);
			}

			// Token: 0x06002EEA RID: 12010 RVA: 0x000200A2 File Offset: 0x0001E2A2
			public IEnumerable<ControlMapper.InputActionSet> GetActionSets(int mapCategoryId)
			{
				return this.list.GetActionSets(mapCategoryId);
			}

			// Token: 0x06002EEB RID: 12011 RVA: 0x000200B0 File Offset: 0x0001E2B0
			public void SetColumnHeight(int mapCategoryId, float height)
			{
				this.list.SetColumnHeight(mapCategoryId, height);
			}

			// Token: 0x06002EEC RID: 12012 RVA: 0x000200BF File Offset: 0x0001E2BF
			public float GetColumnHeight(int mapCategoryId)
			{
				return this.list.GetColumnHeight(mapCategoryId);
			}

			// Token: 0x06002EED RID: 12013 RVA: 0x000200CD File Offset: 0x0001E2CD
			public void SetFieldsActive(int mapCategoryId, bool state)
			{
				this.list.SetFieldsActive(mapCategoryId, state);
			}

			// Token: 0x06002EEE RID: 12014 RVA: 0x000200DC File Offset: 0x0001E2DC
			public void SetFieldLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int index, string label)
			{
				this.list.SetLabel(mapCategoryId, actionId, axisRange, controllerType, index, label);
			}

			// Token: 0x06002EEF RID: 12015 RVA: 0x000FF4B8 File Offset: 0x000FD6B8
			public void PopulateField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId, int index, int actionElementMapId, string label, bool invert)
			{
				this.list.PopulateField(mapCategoryId, actionId, axisRange, controllerType, controllerId, index, actionElementMapId, label, invert);
			}

			// Token: 0x06002EF0 RID: 12016 RVA: 0x000200F2 File Offset: 0x0001E2F2
			public void SetFixedFieldData(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId)
			{
				this.list.SetFixedFieldData(mapCategoryId, actionId, axisRange, controllerType, controllerId);
			}

			// Token: 0x06002EF1 RID: 12017 RVA: 0x00020106 File Offset: 0x0001E306
			public void InitializeFields(int mapCategoryId)
			{
				this.list.InitializeFields(mapCategoryId);
			}

			// Token: 0x06002EF2 RID: 12018 RVA: 0x00020114 File Offset: 0x0001E314
			public void Show(int mapCategoryId)
			{
				this.list.Show(mapCategoryId);
			}

			// Token: 0x06002EF3 RID: 12019 RVA: 0x00020122 File Offset: 0x0001E322
			public void HideAll()
			{
				this.list.HideAll();
			}

			// Token: 0x06002EF4 RID: 12020 RVA: 0x0002012F File Offset: 0x0001E32F
			public void ClearLabels(int mapCategoryId)
			{
				this.list.ClearLabels(mapCategoryId);
			}

			// Token: 0x06002EF5 RID: 12021 RVA: 0x000FF4E0 File Offset: 0x000FD6E0
			private void ClearGroups()
			{
				for (int i = 0; i < this.groups.Count; i++)
				{
					if (!(this.groups[i] == null))
					{
						UnityEngine.Object.Destroy(this.groups[i]);
					}
				}
			}

			// Token: 0x06002EF6 RID: 12022 RVA: 0x0002013D File Offset: 0x0001E33D
			public void ClearAll()
			{
				this.ClearGroups();
				this.list.Clear();
			}

			// Token: 0x04002F25 RID: 12069
			private ControlMapper.InputGridEntryList list;

			// Token: 0x04002F26 RID: 12070
			private List<GameObject> groups;
		}

		// Token: 0x02000671 RID: 1649
		private class InputGridEntryList
		{
			// Token: 0x06002EF7 RID: 12023 RVA: 0x00020150 File Offset: 0x0001E350
			public InputGridEntryList()
			{
				this.entries = new ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.MapCategoryEntry>();
			}

			// Token: 0x06002EF8 RID: 12024 RVA: 0x00020163 File Offset: 0x0001E363
			public void AddMapCategory(int mapCategoryId)
			{
				if (mapCategoryId < 0)
				{
					return;
				}
				if (this.entries.ContainsKey(mapCategoryId))
				{
					return;
				}
				this.entries.Add(mapCategoryId, new ControlMapper.InputGridEntryList.MapCategoryEntry());
			}

			// Token: 0x06002EF9 RID: 12025 RVA: 0x0002018A File Offset: 0x0001E38A
			public void AddAction(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				this.AddActionEntry(mapCategoryId, action, axisRange);
			}

			// Token: 0x06002EFA RID: 12026 RVA: 0x000FF528 File Offset: 0x000FD728
			private ControlMapper.InputGridEntryList.ActionEntry AddActionEntry(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				if (action == null)
				{
					return null;
				}
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return null;
				}
				return mapCategoryEntry.AddAction(action, axisRange);
			}

			// Token: 0x06002EFB RID: 12027 RVA: 0x000FF554 File Offset: 0x000FD754
			public void AddActionLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControlMapper.GUILabel label)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = mapCategoryEntry.GetActionEntry(actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.SetLabel(label);
			}

			// Token: 0x06002EFC RID: 12028 RVA: 0x00020196 File Offset: 0x0001E396
			public void AddActionCategory(int mapCategoryId, int actionCategoryId)
			{
				this.AddActionCategoryEntry(mapCategoryId, actionCategoryId);
			}

			// Token: 0x06002EFD RID: 12029 RVA: 0x000FF588 File Offset: 0x000FD788
			private ControlMapper.InputGridEntryList.ActionCategoryEntry AddActionCategoryEntry(int mapCategoryId, int actionCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return null;
				}
				return mapCategoryEntry.AddActionCategory(actionCategoryId);
			}

			// Token: 0x06002EFE RID: 12030 RVA: 0x000FF5B0 File Offset: 0x000FD7B0
			public void AddActionCategoryLabel(int mapCategoryId, int actionCategoryId, ControlMapper.GUILabel label)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				ControlMapper.InputGridEntryList.ActionCategoryEntry actionCategoryEntry = mapCategoryEntry.GetActionCategoryEntry(actionCategoryId);
				if (actionCategoryEntry == null)
				{
					return;
				}
				actionCategoryEntry.SetLabel(label);
			}

			// Token: 0x06002EFF RID: 12031 RVA: 0x000FF5E4 File Offset: 0x000FD7E4
			public void AddInputFieldSet(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, GameObject fieldSetContainer)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, action, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.AddInputFieldSet(controllerType, fieldSetContainer);
			}

			// Token: 0x06002F00 RID: 12032 RVA: 0x000FF60C File Offset: 0x000FD80C
			public void AddInputField(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, int fieldIndex, ControlMapper.GUIInputField inputField)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, action, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.AddInputField(controllerType, fieldIndex, inputField);
			}

			// Token: 0x06002F01 RID: 12033 RVA: 0x000201A1 File Offset: 0x0001E3A1
			public bool Contains(int mapCategoryId, int actionId, AxisRange axisRange)
			{
				return this.GetActionEntry(mapCategoryId, actionId, axisRange) != null;
			}

			// Token: 0x06002F02 RID: 12034 RVA: 0x000FF634 File Offset: 0x000FD834
			public bool Contains(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				return actionEntry != null && actionEntry.Contains(controllerType, fieldIndex);
			}

			// Token: 0x06002F03 RID: 12035 RVA: 0x000FF65C File Offset: 0x000FD85C
			public void SetColumnHeight(int mapCategoryId, float height)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				mapCategoryEntry.columnHeight = height;
			}

			// Token: 0x06002F04 RID: 12036 RVA: 0x000FF684 File Offset: 0x000FD884
			public float GetColumnHeight(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return 0f;
				}
				return mapCategoryEntry.columnHeight;
			}

			// Token: 0x06002F05 RID: 12037 RVA: 0x000FF6B0 File Offset: 0x000FD8B0
			public ControlMapper.GUIInputField GetGUIInputField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return null;
				}
				return actionEntry.GetGUIInputField(controllerType, fieldIndex);
			}

			// Token: 0x06002F06 RID: 12038 RVA: 0x000FF6D8 File Offset: 0x000FD8D8
			private ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(int mapCategoryId, int actionId, AxisRange axisRange)
			{
				if (actionId < 0)
				{
					return null;
				}
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return null;
				}
				return mapCategoryEntry.GetActionEntry(actionId, axisRange);
			}

			// Token: 0x06002F07 RID: 12039 RVA: 0x000201AF File Offset: 0x0001E3AF
			private ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				if (action == null)
				{
					return null;
				}
				return this.GetActionEntry(mapCategoryId, action.id, axisRange);
			}

			// Token: 0x06002F08 RID: 12040 RVA: 0x000201C4 File Offset: 0x0001E3C4
			public IEnumerable<ControlMapper.InputActionSet> GetActionSets(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					yield break;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> list = mapCategoryEntry.actionList;
				int count = (list != null) ? list.Count : 0;
				int num;
				for (int i = 0; i < count; i = num + 1)
				{
					yield return list[i].actionSet;
					num = i;
				}
				yield break;
			}

			// Token: 0x06002F09 RID: 12041 RVA: 0x000FF708 File Offset: 0x000FD908
			public void SetFieldsActive(int mapCategoryId, bool state)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
				int num = (actionList != null) ? actionList.Count : 0;
				for (int i = 0; i < num; i++)
				{
					actionList[i].SetFieldsActive(state);
				}
			}

			// Token: 0x06002F0A RID: 12042 RVA: 0x000FF754 File Offset: 0x000FD954
			public void SetLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int index, string label)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.SetFieldLabel(controllerType, index, label);
			}

			// Token: 0x06002F0B RID: 12043 RVA: 0x000FF77C File Offset: 0x000FD97C
			public void PopulateField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId, int index, int actionElementMapId, string label, bool invert)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.PopulateField(controllerType, controllerId, index, actionElementMapId, label, invert);
			}

			// Token: 0x06002F0C RID: 12044 RVA: 0x000FF7AC File Offset: 0x000FD9AC
			public void SetFixedFieldData(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.SetFixedFieldData(controllerType, controllerId);
			}

			// Token: 0x06002F0D RID: 12045 RVA: 0x000FF7D4 File Offset: 0x000FD9D4
			public void InitializeFields(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
				int num = (actionList != null) ? actionList.Count : 0;
				for (int i = 0; i < num; i++)
				{
					actionList[i].Initialize();
				}
			}

			// Token: 0x06002F0E RID: 12046 RVA: 0x000FF820 File Offset: 0x000FDA20
			public void Show(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				mapCategoryEntry.SetAllActive(true);
			}

			// Token: 0x06002F0F RID: 12047 RVA: 0x000FF848 File Offset: 0x000FDA48
			public void HideAll()
			{
				for (int i = 0; i < this.entries.Count; i++)
				{
					this.entries[i].SetAllActive(false);
				}
			}

			// Token: 0x06002F10 RID: 12048 RVA: 0x000FF880 File Offset: 0x000FDA80
			public void ClearLabels(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
				int num = (actionList != null) ? actionList.Count : 0;
				for (int i = 0; i < num; i++)
				{
					actionList[i].ClearLabels();
				}
			}

			// Token: 0x06002F11 RID: 12049 RVA: 0x000201DB File Offset: 0x0001E3DB
			public void Clear()
			{
				this.entries.Clear();
			}

			// Token: 0x04002F27 RID: 12071
			private ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.MapCategoryEntry> entries;

			// Token: 0x02000672 RID: 1650
			private class MapCategoryEntry
			{
				// Token: 0x170007E6 RID: 2022
				// (get) Token: 0x06002F12 RID: 12050 RVA: 0x000201E8 File Offset: 0x0001E3E8
				public List<ControlMapper.InputGridEntryList.ActionEntry> actionList
				{
					get
					{
						return this._actionList;
					}
				}

				// Token: 0x170007E7 RID: 2023
				// (get) Token: 0x06002F13 RID: 12051 RVA: 0x000201F0 File Offset: 0x0001E3F0
				public ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.ActionCategoryEntry> actionCategoryList
				{
					get
					{
						return this._actionCategoryList;
					}
				}

				// Token: 0x170007E8 RID: 2024
				// (get) Token: 0x06002F14 RID: 12052 RVA: 0x000201F8 File Offset: 0x0001E3F8
				// (set) Token: 0x06002F15 RID: 12053 RVA: 0x00020200 File Offset: 0x0001E400
				public float columnHeight
				{
					get
					{
						return this._columnHeight;
					}
					set
					{
						this._columnHeight = value;
					}
				}

				// Token: 0x06002F16 RID: 12054 RVA: 0x00020209 File Offset: 0x0001E409
				public MapCategoryEntry()
				{
					this._actionList = new List<ControlMapper.InputGridEntryList.ActionEntry>();
					this._actionCategoryList = new ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.ActionCategoryEntry>();
				}

				// Token: 0x06002F17 RID: 12055 RVA: 0x000FF8CC File Offset: 0x000FDACC
				public ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(int actionId, AxisRange axisRange)
				{
					int num = this.IndexOfActionEntry(actionId, axisRange);
					if (num < 0)
					{
						return null;
					}
					return this._actionList[num];
				}

				// Token: 0x06002F18 RID: 12056 RVA: 0x000FF8F4 File Offset: 0x000FDAF4
				public int IndexOfActionEntry(int actionId, AxisRange axisRange)
				{
					int count = this._actionList.Count;
					for (int i = 0; i < count; i++)
					{
						if (this._actionList[i].Matches(actionId, axisRange))
						{
							return i;
						}
					}
					return -1;
				}

				// Token: 0x06002F19 RID: 12057 RVA: 0x00020227 File Offset: 0x0001E427
				public bool ContainsActionEntry(int actionId, AxisRange axisRange)
				{
					return this.IndexOfActionEntry(actionId, axisRange) >= 0;
				}

				// Token: 0x06002F1A RID: 12058 RVA: 0x000FF934 File Offset: 0x000FDB34
				public ControlMapper.InputGridEntryList.ActionEntry AddAction(InputAction action, AxisRange axisRange)
				{
					if (action == null)
					{
						return null;
					}
					if (this.ContainsActionEntry(action.id, axisRange))
					{
						return null;
					}
					this._actionList.Add(new ControlMapper.InputGridEntryList.ActionEntry(action, axisRange));
					return this._actionList[this._actionList.Count - 1];
				}

				// Token: 0x06002F1B RID: 12059 RVA: 0x00020237 File Offset: 0x0001E437
				public ControlMapper.InputGridEntryList.ActionCategoryEntry GetActionCategoryEntry(int actionCategoryId)
				{
					if (!this._actionCategoryList.ContainsKey(actionCategoryId))
					{
						return null;
					}
					return this._actionCategoryList.Get(actionCategoryId);
				}

				// Token: 0x06002F1C RID: 12060 RVA: 0x00020255 File Offset: 0x0001E455
				public ControlMapper.InputGridEntryList.ActionCategoryEntry AddActionCategory(int actionCategoryId)
				{
					if (actionCategoryId < 0)
					{
						return null;
					}
					if (this._actionCategoryList.ContainsKey(actionCategoryId))
					{
						return null;
					}
					this._actionCategoryList.Add(actionCategoryId, new ControlMapper.InputGridEntryList.ActionCategoryEntry(actionCategoryId));
					return this._actionCategoryList.Get(actionCategoryId);
				}

				// Token: 0x06002F1D RID: 12061 RVA: 0x000FF984 File Offset: 0x000FDB84
				public void SetAllActive(bool state)
				{
					for (int i = 0; i < this._actionCategoryList.Count; i++)
					{
						this._actionCategoryList[i].SetActive(state);
					}
					for (int j = 0; j < this._actionList.Count; j++)
					{
						this._actionList[j].SetActive(state);
					}
				}

				// Token: 0x04002F28 RID: 12072
				private List<ControlMapper.InputGridEntryList.ActionEntry> _actionList;

				// Token: 0x04002F29 RID: 12073
				private ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.ActionCategoryEntry> _actionCategoryList;

				// Token: 0x04002F2A RID: 12074
				private float _columnHeight;
			}

			// Token: 0x02000673 RID: 1651
			private class ActionEntry
			{
				// Token: 0x06002F1E RID: 12062 RVA: 0x0002028B File Offset: 0x0001E48B
				public ActionEntry(InputAction action, AxisRange axisRange)
				{
					this.action = action;
					this.axisRange = axisRange;
					this.actionSet = new ControlMapper.InputActionSet(action.id, axisRange);
					this.fieldSets = new ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.FieldSet>();
				}

				// Token: 0x06002F1F RID: 12063 RVA: 0x000202BE File Offset: 0x0001E4BE
				public void SetLabel(ControlMapper.GUILabel label)
				{
					this.label = label;
				}

				// Token: 0x06002F20 RID: 12064 RVA: 0x000202C7 File Offset: 0x0001E4C7
				public bool Matches(int actionId, AxisRange axisRange)
				{
					return this.action.id == actionId && this.axisRange == axisRange;
				}

				// Token: 0x06002F21 RID: 12065 RVA: 0x000202E5 File Offset: 0x0001E4E5
				public void AddInputFieldSet(ControllerType controllerType, GameObject fieldSetContainer)
				{
					if (this.fieldSets.ContainsKey((int)controllerType))
					{
						return;
					}
					this.fieldSets.Add((int)controllerType, new ControlMapper.InputGridEntryList.FieldSet(fieldSetContainer));
				}

				// Token: 0x06002F22 RID: 12066 RVA: 0x000FF9E4 File Offset: 0x000FDBE4
				public void AddInputField(ControllerType controllerType, int fieldIndex, ControlMapper.GUIInputField inputField)
				{
					if (!this.fieldSets.ContainsKey((int)controllerType))
					{
						return;
					}
					ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets.Get((int)controllerType);
					if (fieldSet.fields.ContainsKey(fieldIndex))
					{
						return;
					}
					fieldSet.fields.Add(fieldIndex, inputField);
				}

				// Token: 0x06002F23 RID: 12067 RVA: 0x000FFA2C File Offset: 0x000FDC2C
				public ControlMapper.GUIInputField GetGUIInputField(ControllerType controllerType, int fieldIndex)
				{
					if (!this.fieldSets.ContainsKey((int)controllerType))
					{
						return null;
					}
					if (!this.fieldSets.Get((int)controllerType).fields.ContainsKey(fieldIndex))
					{
						return null;
					}
					return this.fieldSets.Get((int)controllerType).fields.Get(fieldIndex);
				}

				// Token: 0x06002F24 RID: 12068 RVA: 0x00020308 File Offset: 0x0001E508
				public bool Contains(ControllerType controllerType, int fieldId)
				{
					return this.fieldSets.ContainsKey((int)controllerType) && this.fieldSets.Get((int)controllerType).fields.ContainsKey(fieldId);
				}

				// Token: 0x06002F25 RID: 12069 RVA: 0x000FFA7C File Offset: 0x000FDC7C
				public void SetFieldLabel(ControllerType controllerType, int index, string label)
				{
					if (!this.fieldSets.ContainsKey((int)controllerType))
					{
						return;
					}
					if (!this.fieldSets.Get((int)controllerType).fields.ContainsKey(index))
					{
						return;
					}
					this.fieldSets.Get((int)controllerType).fields.Get(index).SetLabel(label);
				}

				// Token: 0x06002F26 RID: 12070 RVA: 0x000FFAD0 File Offset: 0x000FDCD0
				public void PopulateField(ControllerType controllerType, int controllerId, int index, int actionElementMapId, string label, bool invert)
				{
					if (!this.fieldSets.ContainsKey((int)controllerType))
					{
						return;
					}
					if (!this.fieldSets.Get((int)controllerType).fields.ContainsKey(index))
					{
						return;
					}
					ControlMapper.GUIInputField guiinputField = this.fieldSets.Get((int)controllerType).fields.Get(index);
					guiinputField.SetLabel(label);
					guiinputField.actionElementMapId = actionElementMapId;
					guiinputField.controllerId = controllerId;
					if (guiinputField.hasToggle)
					{
						guiinputField.toggle.SetInteractible(true, false);
						guiinputField.toggle.SetToggleState(invert);
						guiinputField.toggle.actionElementMapId = actionElementMapId;
					}
				}

				// Token: 0x06002F27 RID: 12071 RVA: 0x000FFB64 File Offset: 0x000FDD64
				public void SetFixedFieldData(ControllerType controllerType, int controllerId)
				{
					if (!this.fieldSets.ContainsKey((int)controllerType))
					{
						return;
					}
					ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets.Get((int)controllerType);
					int count = fieldSet.fields.Count;
					for (int i = 0; i < count; i++)
					{
						fieldSet.fields[i].controllerId = controllerId;
					}
				}

				// Token: 0x06002F28 RID: 12072 RVA: 0x000FFBB8 File Offset: 0x000FDDB8
				public void Initialize()
				{
					for (int i = 0; i < this.fieldSets.Count; i++)
					{
						ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[i];
						int count = fieldSet.fields.Count;
						for (int j = 0; j < count; j++)
						{
							ControlMapper.GUIInputField guiinputField = fieldSet.fields[j];
							if (guiinputField.hasToggle)
							{
								guiinputField.toggle.SetInteractible(false, false);
								guiinputField.toggle.SetToggleState(false);
								guiinputField.toggle.actionElementMapId = -1;
							}
							guiinputField.SetLabel("");
							guiinputField.actionElementMapId = -1;
							guiinputField.controllerId = -1;
						}
					}
				}

				// Token: 0x06002F29 RID: 12073 RVA: 0x000FFC64 File Offset: 0x000FDE64
				public void SetActive(bool state)
				{
					if (this.label != null)
					{
						this.label.SetActive(state);
					}
					int count = this.fieldSets.Count;
					for (int i = 0; i < count; i++)
					{
						this.fieldSets[i].groupContainer.SetActive(state);
					}
				}

				// Token: 0x06002F2A RID: 12074 RVA: 0x000FFCB4 File Offset: 0x000FDEB4
				public void ClearLabels()
				{
					for (int i = 0; i < this.fieldSets.Count; i++)
					{
						ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[i];
						int count = fieldSet.fields.Count;
						for (int j = 0; j < count; j++)
						{
							fieldSet.fields[j].SetLabel("");
						}
					}
				}

				// Token: 0x06002F2B RID: 12075 RVA: 0x000FFD14 File Offset: 0x000FDF14
				public void SetFieldsActive(bool state)
				{
					for (int i = 0; i < this.fieldSets.Count; i++)
					{
						ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[i];
						int count = fieldSet.fields.Count;
						for (int j = 0; j < count; j++)
						{
							ControlMapper.GUIInputField guiinputField = fieldSet.fields[j];
							guiinputField.SetInteractible(state, false);
							if (guiinputField.hasToggle && (!state || guiinputField.toggle.actionElementMapId >= 0))
							{
								guiinputField.toggle.SetInteractible(state, false);
							}
						}
					}
				}

				// Token: 0x04002F2B RID: 12075
				private ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.FieldSet> fieldSets;

				// Token: 0x04002F2C RID: 12076
				public ControlMapper.GUILabel label;

				// Token: 0x04002F2D RID: 12077
				public readonly InputAction action;

				// Token: 0x04002F2E RID: 12078
				public readonly AxisRange axisRange;

				// Token: 0x04002F2F RID: 12079
				public readonly ControlMapper.InputActionSet actionSet;
			}

			// Token: 0x02000674 RID: 1652
			private class FieldSet
			{
				// Token: 0x06002F2C RID: 12076 RVA: 0x00020336 File Offset: 0x0001E536
				public FieldSet(GameObject groupContainer)
				{
					this.groupContainer = groupContainer;
					this.fields = new ControlMapper.IndexedDictionary<int, ControlMapper.GUIInputField>();
				}

				// Token: 0x04002F30 RID: 12080
				public readonly GameObject groupContainer;

				// Token: 0x04002F31 RID: 12081
				public readonly ControlMapper.IndexedDictionary<int, ControlMapper.GUIInputField> fields;
			}

			// Token: 0x02000675 RID: 1653
			private class ActionCategoryEntry
			{
				// Token: 0x06002F2D RID: 12077 RVA: 0x00020350 File Offset: 0x0001E550
				public ActionCategoryEntry(int actionCategoryId)
				{
					this.actionCategoryId = actionCategoryId;
				}

				// Token: 0x06002F2E RID: 12078 RVA: 0x0002035F File Offset: 0x0001E55F
				public void SetLabel(ControlMapper.GUILabel label)
				{
					this.label = label;
				}

				// Token: 0x06002F2F RID: 12079 RVA: 0x00020368 File Offset: 0x0001E568
				public void SetActive(bool state)
				{
					if (this.label != null)
					{
						this.label.SetActive(state);
					}
				}

				// Token: 0x04002F32 RID: 12082
				public readonly int actionCategoryId;

				// Token: 0x04002F33 RID: 12083
				public ControlMapper.GUILabel label;
			}
		}

		// Token: 0x02000677 RID: 1655
		private class WindowManager
		{
			// Token: 0x170007EB RID: 2027
			// (get) Token: 0x06002F38 RID: 12088 RVA: 0x000FFEAC File Offset: 0x000FE0AC
			public bool isWindowOpen
			{
				get
				{
					for (int i = this.windows.Count - 1; i >= 0; i--)
					{
						if (!(this.windows[i] == null))
						{
							return true;
						}
					}
					return false;
				}
			}

			// Token: 0x170007EC RID: 2028
			// (get) Token: 0x06002F39 RID: 12089 RVA: 0x000FFEE8 File Offset: 0x000FE0E8
			public Window topWindow
			{
				get
				{
					for (int i = this.windows.Count - 1; i >= 0; i--)
					{
						if (!(this.windows[i] == null))
						{
							return this.windows[i];
						}
					}
					return null;
				}
			}

			// Token: 0x06002F3A RID: 12090 RVA: 0x000FFF30 File Offset: 0x000FE130
			public WindowManager(GameObject windowPrefab, GameObject faderPrefab, Transform parent)
			{
				this.windowPrefab = windowPrefab;
				this.parent = parent;
				this.windows = new List<Window>();
				this.fader = UnityEngine.Object.Instantiate<GameObject>(faderPrefab);
				this.fader.transform.SetParent(parent, false);
				this.fader.GetComponent<RectTransform>().localScale = Vector2.one;
				this.SetFaderActive(false);
			}

			// Token: 0x06002F3B RID: 12091 RVA: 0x000203A8 File Offset: 0x0001E5A8
			public Window OpenWindow(string name, int width, int height)
			{
				Window result = this.InstantiateWindow(name, width, height);
				this.UpdateFader();
				return result;
			}

			// Token: 0x06002F3C RID: 12092 RVA: 0x000203B9 File Offset: 0x0001E5B9
			public Window OpenWindow(GameObject windowPrefab, string name)
			{
				if (windowPrefab == null)
				{
					Debug.LogError("Rewired Control Mapper: Window Prefab is null!");
					return null;
				}
				Window result = this.InstantiateWindow(name, windowPrefab);
				this.UpdateFader();
				return result;
			}

			// Token: 0x06002F3D RID: 12093 RVA: 0x000FFF9C File Offset: 0x000FE19C
			public void CloseTop()
			{
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null))
					{
						this.DestroyWindow(this.windows[i]);
						this.windows.RemoveAt(i);
						break;
					}
					this.windows.RemoveAt(i);
				}
				this.UpdateFader();
			}

			// Token: 0x06002F3E RID: 12094 RVA: 0x000203DE File Offset: 0x0001E5DE
			public void CloseWindow(int windowId)
			{
				this.CloseWindow(this.GetWindow(windowId));
			}

			// Token: 0x06002F3F RID: 12095 RVA: 0x0010000C File Offset: 0x000FE20C
			public void CloseWindow(Window window)
			{
				if (window == null)
				{
					return;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (this.windows[i] == null)
					{
						this.windows.RemoveAt(i);
					}
					else if (!(this.windows[i] != window))
					{
						this.DestroyWindow(this.windows[i]);
						this.windows.RemoveAt(i);
						break;
					}
				}
				this.UpdateFader();
				this.FocusTopWindow();
			}

			// Token: 0x06002F40 RID: 12096 RVA: 0x001000A0 File Offset: 0x000FE2A0
			public void CloseAll()
			{
				this.SetFaderActive(false);
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (this.windows[i] == null)
					{
						this.windows.RemoveAt(i);
					}
					else
					{
						this.DestroyWindow(this.windows[i]);
						this.windows.RemoveAt(i);
					}
				}
				this.UpdateFader();
			}

			// Token: 0x06002F41 RID: 12097 RVA: 0x00100114 File Offset: 0x000FE314
			public void CancelAll()
			{
				if (!this.isWindowOpen)
				{
					return;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null))
					{
						this.windows[i].Cancel();
					}
				}
				this.CloseAll();
			}

			// Token: 0x06002F42 RID: 12098 RVA: 0x00100170 File Offset: 0x000FE370
			public Window GetWindow(int windowId)
			{
				if (windowId < 0)
				{
					return null;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null) && this.windows[i].id == windowId)
					{
						return this.windows[i];
					}
				}
				return null;
			}

			// Token: 0x06002F43 RID: 12099 RVA: 0x000203ED File Offset: 0x0001E5ED
			public bool IsFocused(int windowId)
			{
				return windowId >= 0 && !(this.topWindow == null) && this.topWindow.id == windowId;
			}

			// Token: 0x06002F44 RID: 12100 RVA: 0x00020413 File Offset: 0x0001E613
			public void Focus(int windowId)
			{
				this.Focus(this.GetWindow(windowId));
			}

			// Token: 0x06002F45 RID: 12101 RVA: 0x00020422 File Offset: 0x0001E622
			public void Focus(Window window)
			{
				if (window == null)
				{
					return;
				}
				window.TakeInputFocus();
				this.DefocusOtherWindows(window.id);
			}

			// Token: 0x06002F46 RID: 12102 RVA: 0x001001D4 File Offset: 0x000FE3D4
			private void DefocusOtherWindows(int focusedWindowId)
			{
				if (focusedWindowId < 0)
				{
					return;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null) && this.windows[i].id != focusedWindowId)
					{
						this.windows[i].Disable();
					}
				}
			}

			// Token: 0x06002F47 RID: 12103 RVA: 0x00100238 File Offset: 0x000FE438
			private void UpdateFader()
			{
				if (!this.isWindowOpen)
				{
					this.SetFaderActive(false);
					return;
				}
				if (this.topWindow.transform.parent == null)
				{
					return;
				}
				this.SetFaderActive(true);
				this.fader.transform.SetAsLastSibling();
				int siblingIndex = this.topWindow.transform.GetSiblingIndex();
				this.fader.transform.SetSiblingIndex(siblingIndex);
			}

			// Token: 0x06002F48 RID: 12104 RVA: 0x00020440 File Offset: 0x0001E640
			private void FocusTopWindow()
			{
				if (this.topWindow == null)
				{
					return;
				}
				this.topWindow.TakeInputFocus();
			}

			// Token: 0x06002F49 RID: 12105 RVA: 0x0002045C File Offset: 0x0001E65C
			private void SetFaderActive(bool state)
			{
				this.fader.SetActive(state);
			}

			// Token: 0x06002F4A RID: 12106 RVA: 0x001002A8 File Offset: 0x000FE4A8
			private Window InstantiateWindow(string name, int width, int height)
			{
				if (string.IsNullOrEmpty(name))
				{
					name = "Window";
				}
				GameObject gameObject = UITools.InstantiateGUIObject<Window>(this.windowPrefab, this.parent, name);
				if (gameObject == null)
				{
					return null;
				}
				Window component = gameObject.GetComponent<Window>();
				if (component != null)
				{
					component.Initialize(this.GetNewId(), new Func<int, bool>(this.IsFocused));
					this.windows.Add(component);
					component.SetSize(width, height);
				}
				return component;
			}

			// Token: 0x06002F4B RID: 12107 RVA: 0x00100320 File Offset: 0x000FE520
			private Window InstantiateWindow(string name, GameObject windowPrefab)
			{
				if (string.IsNullOrEmpty(name))
				{
					name = "Window";
				}
				if (windowPrefab == null)
				{
					return null;
				}
				GameObject gameObject = UITools.InstantiateGUIObject<Window>(windowPrefab, this.parent, name);
				if (gameObject == null)
				{
					return null;
				}
				Window component = gameObject.GetComponent<Window>();
				if (component != null)
				{
					component.Initialize(this.GetNewId(), new Func<int, bool>(this.IsFocused));
					this.windows.Add(component);
				}
				return component;
			}

			// Token: 0x06002F4C RID: 12108 RVA: 0x0002046A File Offset: 0x0001E66A
			private void DestroyWindow(Window window)
			{
				if (window == null)
				{
					return;
				}
				UnityEngine.Object.Destroy(window.gameObject);
			}

			// Token: 0x06002F4D RID: 12109 RVA: 0x00020481 File Offset: 0x0001E681
			private int GetNewId()
			{
				int result = this.idCounter;
				this.idCounter++;
				return result;
			}

			// Token: 0x06002F4E RID: 12110 RVA: 0x00020497 File Offset: 0x0001E697
			public void ClearCompletely()
			{
				this.CloseAll();
				if (this.fader != null)
				{
					UnityEngine.Object.Destroy(this.fader);
				}
			}

			// Token: 0x04002F3D RID: 12093
			private List<Window> windows;

			// Token: 0x04002F3E RID: 12094
			private GameObject windowPrefab;

			// Token: 0x04002F3F RID: 12095
			private Transform parent;

			// Token: 0x04002F40 RID: 12096
			private GameObject fader;

			// Token: 0x04002F41 RID: 12097
			private int idCounter;
		}
	}
}
