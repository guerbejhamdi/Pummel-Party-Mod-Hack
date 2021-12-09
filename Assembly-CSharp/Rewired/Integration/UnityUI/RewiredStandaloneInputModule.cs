using System;
using System.Collections.Generic;
using Rewired.Components;
using Rewired.UI;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Rewired.Integration.UnityUI
{
	// Token: 0x020006B5 RID: 1717
	[AddComponentMenu("Rewired/Rewired Standalone Input Module")]
	public sealed class RewiredStandaloneInputModule : RewiredPointerInputModule
	{
		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x060031C3 RID: 12739 RVA: 0x00021C93 File Offset: 0x0001FE93
		// (set) Token: 0x060031C4 RID: 12740 RVA: 0x00021C9B File Offset: 0x0001FE9B
		public InputManager_Base RewiredInputManager
		{
			get
			{
				return this.rewiredInputManager;
			}
			set
			{
				this.rewiredInputManager = value;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x060031C5 RID: 12741 RVA: 0x00021CA4 File Offset: 0x0001FEA4
		// (set) Token: 0x060031C6 RID: 12742 RVA: 0x00021CAC File Offset: 0x0001FEAC
		public bool UseAllRewiredGamePlayers
		{
			get
			{
				return this.useAllRewiredGamePlayers;
			}
			set
			{
				bool flag = value != this.useAllRewiredGamePlayers;
				this.useAllRewiredGamePlayers = value;
				if (flag)
				{
					this.SetupRewiredVars();
				}
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x060031C7 RID: 12743 RVA: 0x00021CC9 File Offset: 0x0001FEC9
		// (set) Token: 0x060031C8 RID: 12744 RVA: 0x00021CD1 File Offset: 0x0001FED1
		public bool UseRewiredSystemPlayer
		{
			get
			{
				return this.useRewiredSystemPlayer;
			}
			set
			{
				bool flag = value != this.useRewiredSystemPlayer;
				this.useRewiredSystemPlayer = value;
				if (flag)
				{
					this.SetupRewiredVars();
				}
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x060031C9 RID: 12745 RVA: 0x00021CEE File Offset: 0x0001FEEE
		// (set) Token: 0x060031CA RID: 12746 RVA: 0x00021D00 File Offset: 0x0001FF00
		public int[] RewiredPlayerIds
		{
			get
			{
				return (int[])this.rewiredPlayerIds.Clone();
			}
			set
			{
				this.rewiredPlayerIds = ((value != null) ? ((int[])value.Clone()) : new int[0]);
				this.SetupRewiredVars();
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x060031CB RID: 12747 RVA: 0x00021D24 File Offset: 0x0001FF24
		// (set) Token: 0x060031CC RID: 12748 RVA: 0x00021D2C File Offset: 0x0001FF2C
		public bool UsePlayingPlayersOnly
		{
			get
			{
				return this.usePlayingPlayersOnly;
			}
			set
			{
				this.usePlayingPlayersOnly = value;
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x060031CD RID: 12749 RVA: 0x00021D35 File Offset: 0x0001FF35
		// (set) Token: 0x060031CE RID: 12750 RVA: 0x00021D42 File Offset: 0x0001FF42
		public List<PlayerMouse> PlayerMice
		{
			get
			{
				return new List<PlayerMouse>(this.playerMice);
			}
			set
			{
				if (value == null)
				{
					this.playerMice = new List<PlayerMouse>();
					this.SetupRewiredVars();
					return;
				}
				this.playerMice = new List<PlayerMouse>(value);
				this.SetupRewiredVars();
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x060031CF RID: 12751 RVA: 0x00021D6B File Offset: 0x0001FF6B
		// (set) Token: 0x060031D0 RID: 12752 RVA: 0x00021D73 File Offset: 0x0001FF73
		public bool MoveOneElementPerAxisPress
		{
			get
			{
				return this.moveOneElementPerAxisPress;
			}
			set
			{
				this.moveOneElementPerAxisPress = value;
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x060031D1 RID: 12753 RVA: 0x00021D7C File Offset: 0x0001FF7C
		// (set) Token: 0x060031D2 RID: 12754 RVA: 0x00021D84 File Offset: 0x0001FF84
		public bool allowMouseInput
		{
			get
			{
				return this.m_allowMouseInput;
			}
			set
			{
				this.m_allowMouseInput = value;
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x060031D3 RID: 12755 RVA: 0x00021D8D File Offset: 0x0001FF8D
		// (set) Token: 0x060031D4 RID: 12756 RVA: 0x00021D95 File Offset: 0x0001FF95
		public bool allowMouseInputIfTouchSupported
		{
			get
			{
				return this.m_allowMouseInputIfTouchSupported;
			}
			set
			{
				this.m_allowMouseInputIfTouchSupported = value;
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x060031D5 RID: 12757 RVA: 0x00021D9E File Offset: 0x0001FF9E
		// (set) Token: 0x060031D6 RID: 12758 RVA: 0x00021DA6 File Offset: 0x0001FFA6
		public bool allowTouchInput
		{
			get
			{
				return this.m_allowTouchInput;
			}
			set
			{
				this.m_allowTouchInput = value;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x060031D7 RID: 12759 RVA: 0x00021DAF File Offset: 0x0001FFAF
		// (set) Token: 0x060031D8 RID: 12760 RVA: 0x00021DB7 File Offset: 0x0001FFB7
		public bool deselectIfBackgroundClicked
		{
			get
			{
				return this.m_deselectIfBackgroundClicked;
			}
			set
			{
				this.m_deselectIfBackgroundClicked = value;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x060031D9 RID: 12761 RVA: 0x00021DC0 File Offset: 0x0001FFC0
		// (set) Token: 0x060031DA RID: 12762 RVA: 0x00021DC8 File Offset: 0x0001FFC8
		private bool deselectBeforeSelecting
		{
			get
			{
				return this.m_deselectBeforeSelecting;
			}
			set
			{
				this.m_deselectBeforeSelecting = value;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x060031DB RID: 12763 RVA: 0x00021DD1 File Offset: 0x0001FFD1
		// (set) Token: 0x060031DC RID: 12764 RVA: 0x00021DD9 File Offset: 0x0001FFD9
		public bool SetActionsById
		{
			get
			{
				return this.setActionsById;
			}
			set
			{
				if (this.setActionsById == value)
				{
					return;
				}
				this.setActionsById = value;
				this.SetupRewiredVars();
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x060031DD RID: 12765 RVA: 0x00021DF2 File Offset: 0x0001FFF2
		// (set) Token: 0x060031DE RID: 12766 RVA: 0x0010410C File Offset: 0x0010230C
		public int HorizontalActionId
		{
			get
			{
				return this.horizontalActionId;
			}
			set
			{
				if (value == this.horizontalActionId)
				{
					return;
				}
				this.horizontalActionId = value;
				if (ReInput.isReady)
				{
					this.m_HorizontalAxis = ((ReInput.mapping.GetAction(value) != null) ? ReInput.mapping.GetAction(value).name : string.Empty);
				}
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x060031DF RID: 12767 RVA: 0x00021DFA File Offset: 0x0001FFFA
		// (set) Token: 0x060031E0 RID: 12768 RVA: 0x0010415C File Offset: 0x0010235C
		public int VerticalActionId
		{
			get
			{
				return this.verticalActionId;
			}
			set
			{
				if (value == this.verticalActionId)
				{
					return;
				}
				this.verticalActionId = value;
				if (ReInput.isReady)
				{
					this.m_VerticalAxis = ((ReInput.mapping.GetAction(value) != null) ? ReInput.mapping.GetAction(value).name : string.Empty);
				}
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x060031E1 RID: 12769 RVA: 0x00021E02 File Offset: 0x00020002
		// (set) Token: 0x060031E2 RID: 12770 RVA: 0x001041AC File Offset: 0x001023AC
		public int SubmitActionId
		{
			get
			{
				return this.submitActionId;
			}
			set
			{
				if (value == this.submitActionId)
				{
					return;
				}
				this.submitActionId = value;
				if (ReInput.isReady)
				{
					this.m_SubmitButton = ((ReInput.mapping.GetAction(value) != null) ? ReInput.mapping.GetAction(value).name : string.Empty);
				}
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x060031E3 RID: 12771 RVA: 0x00021E0A File Offset: 0x0002000A
		// (set) Token: 0x060031E4 RID: 12772 RVA: 0x001041FC File Offset: 0x001023FC
		public int CancelActionId
		{
			get
			{
				return this.cancelActionId;
			}
			set
			{
				if (value == this.cancelActionId)
				{
					return;
				}
				this.cancelActionId = value;
				if (ReInput.isReady)
				{
					this.m_CancelButton = ((ReInput.mapping.GetAction(value) != null) ? ReInput.mapping.GetAction(value).name : string.Empty);
				}
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x060031E5 RID: 12773 RVA: 0x00021E12 File Offset: 0x00020012
		protected override bool isMouseSupported
		{
			get
			{
				return base.isMouseSupported && this.m_allowMouseInput && (!this.isTouchSupported || this.m_allowMouseInputIfTouchSupported);
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x060031E6 RID: 12774 RVA: 0x00021D9E File Offset: 0x0001FF9E
		private bool isTouchAllowed
		{
			get
			{
				return this.m_allowTouchInput;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x060031E7 RID: 12775 RVA: 0x00021E38 File Offset: 0x00020038
		// (set) Token: 0x060031E8 RID: 12776 RVA: 0x00021E40 File Offset: 0x00020040
		[Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead")]
		public bool allowActivationOnMobileDevice
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x060031E9 RID: 12777 RVA: 0x00021E38 File Offset: 0x00020038
		// (set) Token: 0x060031EA RID: 12778 RVA: 0x00021E40 File Offset: 0x00020040
		public bool forceModuleActive
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x060031EB RID: 12779 RVA: 0x00021E49 File Offset: 0x00020049
		// (set) Token: 0x060031EC RID: 12780 RVA: 0x00021E51 File Offset: 0x00020051
		public float inputActionsPerSecond
		{
			get
			{
				return this.m_InputActionsPerSecond;
			}
			set
			{
				this.m_InputActionsPerSecond = value;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x060031ED RID: 12781 RVA: 0x00021E5A File Offset: 0x0002005A
		// (set) Token: 0x060031EE RID: 12782 RVA: 0x00021E62 File Offset: 0x00020062
		public float repeatDelay
		{
			get
			{
				return this.m_RepeatDelay;
			}
			set
			{
				this.m_RepeatDelay = value;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x060031EF RID: 12783 RVA: 0x00021E6B File Offset: 0x0002006B
		// (set) Token: 0x060031F0 RID: 12784 RVA: 0x00021E73 File Offset: 0x00020073
		public string horizontalAxis
		{
			get
			{
				return this.m_HorizontalAxis;
			}
			set
			{
				if (this.m_HorizontalAxis == value)
				{
					return;
				}
				this.m_HorizontalAxis = value;
				if (ReInput.isReady)
				{
					this.horizontalActionId = ReInput.mapping.GetActionId(value);
				}
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x060031F1 RID: 12785 RVA: 0x00021EA3 File Offset: 0x000200A3
		// (set) Token: 0x060031F2 RID: 12786 RVA: 0x00021EAB File Offset: 0x000200AB
		public string verticalAxis
		{
			get
			{
				return this.m_VerticalAxis;
			}
			set
			{
				if (this.m_VerticalAxis == value)
				{
					return;
				}
				this.m_VerticalAxis = value;
				if (ReInput.isReady)
				{
					this.verticalActionId = ReInput.mapping.GetActionId(value);
				}
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x060031F3 RID: 12787 RVA: 0x00021EDB File Offset: 0x000200DB
		// (set) Token: 0x060031F4 RID: 12788 RVA: 0x00021EE3 File Offset: 0x000200E3
		public string submitButton
		{
			get
			{
				return this.m_SubmitButton;
			}
			set
			{
				if (this.m_SubmitButton == value)
				{
					return;
				}
				this.m_SubmitButton = value;
				if (ReInput.isReady)
				{
					this.submitActionId = ReInput.mapping.GetActionId(value);
				}
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x060031F5 RID: 12789 RVA: 0x00021F13 File Offset: 0x00020113
		// (set) Token: 0x060031F6 RID: 12790 RVA: 0x00021F1B File Offset: 0x0002011B
		public string cancelButton
		{
			get
			{
				return this.m_CancelButton;
			}
			set
			{
				if (this.m_CancelButton == value)
				{
					return;
				}
				this.m_CancelButton = value;
				if (ReInput.isReady)
				{
					this.cancelActionId = ReInput.mapping.GetActionId(value);
				}
			}
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x0010424C File Offset: 0x0010244C
		private RewiredStandaloneInputModule()
		{
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x00021F4B File Offset: 0x0002014B
		protected override void Awake()
		{
			base.Awake();
			this.isTouchSupported = base.defaultTouchInputSource.touchSupported;
			ReInput.InitializedEvent += this.OnRewiredInitialized;
			this.InitializeRewired();
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x00021F7B File Offset: 0x0002017B
		public override void UpdateModule()
		{
			this.CheckEditorRecompile();
			if (this.recompiling)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.m_HasFocus)
			{
				this.ShouldIgnoreEventsOnNoFocus();
				return;
			}
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x00005651 File Offset: 0x00003851
		public override bool IsModuleSupported()
		{
			return true;
		}

		// Token: 0x060031FB RID: 12795 RVA: 0x001042F4 File Offset: 0x001024F4
		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			if (this.recompiling)
			{
				return false;
			}
			if (!ReInput.isReady)
			{
				return false;
			}
			bool flag = this.m_ForceModuleActive;
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					flag |= this.GetButtonDown(player, this.submitActionId);
					flag |= this.GetButtonDown(player, this.cancelActionId);
					if (this.moveOneElementPerAxisPress)
					{
						flag |= (this.GetButtonDown(player, this.horizontalActionId) || this.GetNegativeButtonDown(player, this.horizontalActionId));
						flag |= (this.GetButtonDown(player, this.verticalActionId) || this.GetNegativeButtonDown(player, this.verticalActionId));
					}
					else
					{
						flag |= !Mathf.Approximately(this.GetAxis(player, this.horizontalActionId), 0f);
						flag |= !Mathf.Approximately(this.GetAxis(player, this.verticalActionId), 0f);
					}
				}
			}
			if (this.isMouseSupported)
			{
				flag |= this.DidAnyMouseMove();
				flag |= this.GetMouseButtonDownOnAnyMouse(0);
			}
			if (this.isTouchAllowed)
			{
				for (int j = 0; j < base.defaultTouchInputSource.touchCount; j++)
				{
					Touch touch = base.defaultTouchInputSource.GetTouch(j);
					flag |= (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary);
				}
			}
			return flag;
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x00104480 File Offset: 0x00102680
		public override void ActivateModule()
		{
			if (!this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus())
			{
				return;
			}
			base.ActivateModule();
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x00021FA4 File Offset: 0x000201A4
		public override void DeactivateModule()
		{
			base.DeactivateModule();
			base.ClearSelection();
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x001044D8 File Offset: 0x001026D8
		public override void Process()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus())
			{
				return;
			}
			if (!base.enabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			bool flag = this.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= this.SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendSubmitEventToSelectedObject();
				}
			}
			if (!this.ProcessTouchEvents() && this.isMouseSupported)
			{
				this.ProcessMouseEvents();
			}
		}

		// Token: 0x060031FF RID: 12799 RVA: 0x00104554 File Offset: 0x00102754
		private bool ProcessTouchEvents()
		{
			if (!this.isTouchAllowed)
			{
				return false;
			}
			for (int i = 0; i < base.defaultTouchInputSource.touchCount; i++)
			{
				Touch touch = base.defaultTouchInputSource.GetTouch(i);
				if (touch.type != TouchType.Indirect)
				{
					bool pressed;
					bool flag;
					PlayerPointerEventData touchPointerEventData = base.GetTouchPointerEventData(0, 0, touch, out pressed, out flag);
					this.ProcessTouchPress(touchPointerEventData, pressed, flag);
					if (!flag)
					{
						this.ProcessMove(touchPointerEventData);
						this.ProcessDrag(touchPointerEventData);
					}
					else
					{
						base.RemovePointerData(touchPointerEventData);
					}
				}
			}
			return base.defaultTouchInputSource.touchCount > 0;
		}

		// Token: 0x06003200 RID: 12800 RVA: 0x001045DC File Offset: 0x001027DC
		private void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
		{
			GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
			if (pressed)
			{
				pointerEvent.eligibleForClick = true;
				pointerEvent.delta = Vector2.zero;
				pointerEvent.dragging = false;
				pointerEvent.useDragThreshold = true;
				pointerEvent.pressPosition = pointerEvent.position;
				pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
				this.HandleMouseTouchDeselectionOnSelectionChanged(gameObject, pointerEvent);
				if (pointerEvent.pointerEnter != gameObject)
				{
					base.HandlePointerExitAndEnter(pointerEvent, gameObject);
					pointerEvent.pointerEnter = gameObject;
				}
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, pointerEvent, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				double unscaledTime = ReInput.time.unscaledTime;
				if (gameObject2 == pointerEvent.lastPress)
				{
					if (unscaledTime - (double)pointerEvent.clickTime < 0.30000001192092896)
					{
						int clickCount = pointerEvent.clickCount + 1;
						pointerEvent.clickCount = clickCount;
					}
					else
					{
						pointerEvent.clickCount = 1;
					}
					pointerEvent.clickTime = (float)unscaledTime;
				}
				else
				{
					pointerEvent.clickCount = 1;
				}
				pointerEvent.pointerPress = gameObject2;
				pointerEvent.rawPointerPress = gameObject;
				pointerEvent.clickTime = (float)unscaledTime;
				pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (released)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (pointerEvent.pointerPress == eventHandler && pointerEvent.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
				}
				else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, pointerEvent, ExecuteEvents.dropHandler);
				}
				pointerEvent.eligibleForClick = false;
				pointerEvent.pointerPress = null;
				pointerEvent.rawPointerPress = null;
				if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.dragging = false;
				pointerEvent.pointerDrag = null;
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.pointerDrag = null;
				ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
				pointerEvent.pointerEnter = null;
			}
		}

		// Token: 0x06003201 RID: 12801 RVA: 0x0010480C File Offset: 0x00102A0C
		private bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			if (this.recompiling)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					if (this.GetButtonDown(player, this.submitActionId))
					{
						ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
						break;
					}
					if (this.GetButtonDown(player, this.cancelActionId))
					{
						ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
						break;
					}
				}
			}
			return baseEventData.used;
		}

		// Token: 0x06003202 RID: 12802 RVA: 0x001048D4 File Offset: 0x00102AD4
		private Vector2 GetRawMoveVector()
		{
			if (this.recompiling)
			{
				return Vector2.zero;
			}
			Vector2 zero = Vector2.zero;
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					float num = this.GetAxis(player, this.horizontalActionId);
					float num2 = this.GetAxis(player, this.verticalActionId);
					if (Mathf.Approximately(num, 0f))
					{
						num = 0f;
					}
					if (Mathf.Approximately(num2, 0f))
					{
						num2 = 0f;
					}
					if (this.moveOneElementPerAxisPress)
					{
						if (this.GetButtonDown(player, this.horizontalActionId) && num > 0f)
						{
							zero.x += 1f;
						}
						if (this.GetNegativeButtonDown(player, this.horizontalActionId) && num < 0f)
						{
							zero.x -= 1f;
						}
						if (this.GetButtonDown(player, this.verticalActionId) && num2 > 0f)
						{
							zero.y += 1f;
						}
						if (this.GetNegativeButtonDown(player, this.verticalActionId) && num2 < 0f)
						{
							zero.y -= 1f;
						}
					}
					else
					{
						if (this.GetButton(player, this.horizontalActionId) && num > 0f)
						{
							zero.x += 1f;
						}
						if (this.GetNegativeButton(player, this.horizontalActionId) && num < 0f)
						{
							zero.x -= 1f;
						}
						if (this.GetButton(player, this.verticalActionId) && num2 > 0f)
						{
							zero.y += 1f;
						}
						if (this.GetNegativeButton(player, this.verticalActionId) && num2 < 0f)
						{
							zero.y -= 1f;
						}
					}
				}
			}
			return zero;
		}

		// Token: 0x06003203 RID: 12803 RVA: 0x00104AD4 File Offset: 0x00102CD4
		private bool SendMoveEventToSelectedObject()
		{
			if (this.recompiling)
			{
				return false;
			}
			double unscaledTime = ReInput.time.unscaledTime;
			Vector2 rawMoveVector = this.GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				this.m_ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0f;
			bool flag2;
			bool flag3;
			this.CheckButtonOrKeyMovement(out flag2, out flag3);
			AxisEventData axisEventData = null;
			bool flag4 = flag2 || flag3;
			if (flag4)
			{
				axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0f);
				MoveDirection moveDir = axisEventData.moveDir;
				flag4 = (((moveDir == MoveDirection.Up || moveDir == MoveDirection.Down) && flag3) || ((moveDir == MoveDirection.Left || moveDir == MoveDirection.Right) && flag2));
			}
			if (!flag4)
			{
				if (this.m_RepeatDelay > 0f)
				{
					if (flag && this.m_ConsecutiveMoveCount == 1)
					{
						flag4 = (unscaledTime > this.m_PrevActionTime + (double)this.m_RepeatDelay);
					}
					else
					{
						flag4 = (unscaledTime > this.m_PrevActionTime + (double)(1f / this.m_InputActionsPerSecond));
					}
				}
				else
				{
					flag4 = (unscaledTime > this.m_PrevActionTime + (double)(1f / this.m_InputActionsPerSecond));
				}
			}
			if (!flag4)
			{
				return false;
			}
			if (axisEventData == null)
			{
				axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0f);
			}
			if (axisEventData.moveDir != MoveDirection.None)
			{
				ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
				if (!flag)
				{
					this.m_ConsecutiveMoveCount = 0;
				}
				if (this.m_ConsecutiveMoveCount == 0 || (!flag2 && !flag3))
				{
					this.m_ConsecutiveMoveCount++;
				}
				this.m_PrevActionTime = unscaledTime;
				this.m_LastMoveVector = rawMoveVector;
			}
			else
			{
				this.m_ConsecutiveMoveCount = 0;
			}
			return axisEventData.used;
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x00104C8C File Offset: 0x00102E8C
		private void CheckButtonOrKeyMovement(out bool downHorizontal, out bool downVertical)
		{
			downHorizontal = false;
			downVertical = false;
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					downHorizontal |= (this.GetButtonDown(player, this.horizontalActionId) || this.GetNegativeButtonDown(player, this.horizontalActionId));
					downVertical |= (this.GetButtonDown(player, this.verticalActionId) || this.GetNegativeButtonDown(player, this.verticalActionId));
				}
			}
		}

		// Token: 0x06003205 RID: 12805 RVA: 0x00104D20 File Offset: 0x00102F20
		private void ProcessMouseEvents()
		{
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(this.playerIds[i]);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					int mouseInputSourceCount = base.GetMouseInputSourceCount(this.playerIds[i]);
					for (int j = 0; j < mouseInputSourceCount; j++)
					{
						this.ProcessMouseEvent(this.playerIds[i], j);
					}
				}
			}
		}

		// Token: 0x06003206 RID: 12806 RVA: 0x00104D90 File Offset: 0x00102F90
		private void ProcessMouseEvent(int playerId, int pointerIndex)
		{
			RewiredPointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData(playerId, pointerIndex);
			if (mousePointerEventData == null)
			{
				return;
			}
			RewiredPointerInputModule.MouseButtonEventData eventData = mousePointerEventData.GetButtonState(0).eventData;
			this.ProcessMousePress(eventData);
			this.ProcessMove(eventData.buttonData);
			this.ProcessDrag(eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(1).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(1).eventData.buttonData);
			this.ProcessMousePress(mousePointerEventData.GetButtonState(2).eventData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(2).eventData.buttonData);
			IMouseInputSource mouseInputSource = base.GetMouseInputSource(playerId, pointerIndex);
			if (mouseInputSource == null)
			{
				return;
			}
			for (int i = 3; i < mouseInputSource.buttonCount; i++)
			{
				this.ProcessMousePress(mousePointerEventData.GetButtonState(i).eventData);
				this.ProcessDrag(mousePointerEventData.GetButtonState(i).eventData.buttonData);
			}
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), eventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		// Token: 0x06003207 RID: 12807 RVA: 0x00104EBC File Offset: 0x001030BC
		private bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x00104F04 File Offset: 0x00103104
		private void ProcessMousePress(RewiredPointerInputModule.MouseButtonEventData data)
		{
			PlayerPointerEventData buttonData = data.buttonData;
			if (base.GetMouseInputSource(buttonData.playerId, buttonData.inputSourceIndex) == null)
			{
				return;
			}
			GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				this.HandleMouseTouchDeselectionOnSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				double unscaledTime = ReInput.time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					if (unscaledTime - (double)buttonData.clickTime < 0.30000001192092896)
					{
						PlayerPointerEventData playerPointerEventData = buttonData;
						int clickCount = playerPointerEventData.clickCount + 1;
						playerPointerEventData.clickCount = clickCount;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = (float)unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = (float)unscaledTime;
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (data.ReleasedThisFrame())
			{
				ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
				else if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.eligibleForClick = false;
				buttonData.pointerPress = null;
				buttonData.rawPointerPress = null;
				if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
				}
				buttonData.dragging = false;
				buttonData.pointerDrag = null;
				if (gameObject != buttonData.pointerEnter)
				{
					base.HandlePointerExitAndEnter(buttonData, null);
					base.HandlePointerExitAndEnter(buttonData, gameObject);
				}
			}
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x00105120 File Offset: 0x00103320
		private void HandleMouseTouchDeselectionOnSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
		{
			if (this.m_deselectIfBackgroundClicked && this.m_deselectBeforeSelecting)
			{
				base.DeselectIfSelectionChanged(currentOverGo, pointerEvent);
				return;
			}
			GameObject eventHandler = ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo);
			if (this.m_deselectIfBackgroundClicked)
			{
				if (eventHandler != base.eventSystem.currentSelectedGameObject && eventHandler != null)
				{
					base.eventSystem.SetSelectedGameObject(null, pointerEvent);
					return;
				}
			}
			else if (this.m_deselectBeforeSelecting && eventHandler != null && eventHandler != base.eventSystem.currentSelectedGameObject)
			{
				base.eventSystem.SetSelectedGameObject(null, pointerEvent);
			}
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x00021FB2 File Offset: 0x000201B2
		private void OnApplicationFocus(bool hasFocus)
		{
			this.m_HasFocus = hasFocus;
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x00021FBB File Offset: 0x000201BB
		private bool ShouldIgnoreEventsOnNoFocus()
		{
			return !ReInput.isReady || ReInput.configuration.ignoreInputWhenAppNotInFocus;
		}

		// Token: 0x0600320C RID: 12812 RVA: 0x00021FD0 File Offset: 0x000201D0
		protected override void OnDestroy()
		{
			base.OnDestroy();
			ReInput.InitializedEvent -= this.OnRewiredInitialized;
			ReInput.ShutDownEvent -= this.OnRewiredShutDown;
			ReInput.EditorRecompileEvent -= this.OnEditorRecompile;
		}

		// Token: 0x0600320D RID: 12813 RVA: 0x001051B0 File Offset: 0x001033B0
		protected override bool IsDefaultPlayer(int playerId)
		{
			if (this.playerIds == null)
			{
				return false;
			}
			if (!ReInput.isReady)
			{
				return false;
			}
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < this.playerIds.Length; j++)
				{
					Player player = ReInput.players.GetPlayer(this.playerIds[j]);
					if (player != null && (i >= 1 || !this.usePlayingPlayersOnly || player.isPlaying) && (i >= 2 || player.controllers.hasMouse))
					{
						return this.playerIds[j] == playerId;
					}
				}
			}
			return false;
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x00105238 File Offset: 0x00103438
		private void InitializeRewired()
		{
			if (!ReInput.isReady)
			{
				Debug.LogError("Rewired is not initialized! Are you missing a Rewired Input Manager in your scene?");
				return;
			}
			ReInput.ShutDownEvent -= this.OnRewiredShutDown;
			ReInput.ShutDownEvent += this.OnRewiredShutDown;
			ReInput.EditorRecompileEvent -= this.OnEditorRecompile;
			ReInput.EditorRecompileEvent += this.OnEditorRecompile;
			this.SetupRewiredVars();
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x001052A4 File Offset: 0x001034A4
		private void SetupRewiredVars()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.SetUpRewiredActions();
			if (this.useAllRewiredGamePlayers)
			{
				IList<Player> list = this.useRewiredSystemPlayer ? ReInput.players.AllPlayers : ReInput.players.Players;
				this.playerIds = new int[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					this.playerIds[i] = list[i].id;
				}
			}
			else
			{
				bool flag = false;
				List<int> list2 = new List<int>(this.rewiredPlayerIds.Length + 1);
				for (int j = 0; j < this.rewiredPlayerIds.Length; j++)
				{
					Player player = ReInput.players.GetPlayer(this.rewiredPlayerIds[j]);
					if (player != null && !list2.Contains(player.id))
					{
						list2.Add(player.id);
						if (player.id == 9999999)
						{
							flag = true;
						}
					}
				}
				if (this.useRewiredSystemPlayer && !flag)
				{
					list2.Insert(0, ReInput.players.GetSystemPlayer().id);
				}
				this.playerIds = list2.ToArray();
			}
			this.SetUpRewiredPlayerMice();
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x001053C4 File Offset: 0x001035C4
		private void SetUpRewiredPlayerMice()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			base.ClearMouseInputSources();
			for (int i = 0; i < this.playerMice.Count; i++)
			{
				PlayerMouse playerMouse = this.playerMice[i];
				if (!UnityTools.IsNullOrDestroyed<PlayerMouse>(playerMouse))
				{
					base.AddMouseInputSource(playerMouse);
				}
			}
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x00105414 File Offset: 0x00103614
		private void SetUpRewiredActions()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.setActionsById)
			{
				this.horizontalActionId = ReInput.mapping.GetActionId(this.m_HorizontalAxis);
				this.verticalActionId = ReInput.mapping.GetActionId(this.m_VerticalAxis);
				this.submitActionId = ReInput.mapping.GetActionId(this.m_SubmitButton);
				this.cancelActionId = ReInput.mapping.GetActionId(this.m_CancelButton);
				return;
			}
			InputAction action = ReInput.mapping.GetAction(this.horizontalActionId);
			this.m_HorizontalAxis = ((action != null) ? action.name : string.Empty);
			if (action == null)
			{
				this.horizontalActionId = -1;
			}
			action = ReInput.mapping.GetAction(this.verticalActionId);
			this.m_VerticalAxis = ((action != null) ? action.name : string.Empty);
			if (action == null)
			{
				this.verticalActionId = -1;
			}
			action = ReInput.mapping.GetAction(this.submitActionId);
			this.m_SubmitButton = ((action != null) ? action.name : string.Empty);
			if (action == null)
			{
				this.submitActionId = -1;
			}
			action = ReInput.mapping.GetAction(this.cancelActionId);
			this.m_CancelButton = ((action != null) ? action.name : string.Empty);
			if (action == null)
			{
				this.cancelActionId = -1;
			}
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x0002200B File Offset: 0x0002020B
		private bool GetButton(Player player, int actionId)
		{
			return actionId >= 0 && player.GetButton(actionId);
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x0002201A File Offset: 0x0002021A
		private bool GetButtonDown(Player player, int actionId)
		{
			return actionId >= 0 && player.GetButtonDown(actionId);
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x00022029 File Offset: 0x00020229
		private bool GetNegativeButton(Player player, int actionId)
		{
			return actionId >= 0 && player.GetNegativeButton(actionId);
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x00022038 File Offset: 0x00020238
		private bool GetNegativeButtonDown(Player player, int actionId)
		{
			return actionId >= 0 && player.GetNegativeButtonDown(actionId);
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x00022047 File Offset: 0x00020247
		private float GetAxis(Player player, int actionId)
		{
			if (actionId < 0)
			{
				return 0f;
			}
			return player.GetAxis(actionId);
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x0002205A File Offset: 0x0002025A
		private void CheckEditorRecompile()
		{
			if (!this.recompiling)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			this.recompiling = false;
			this.InitializeRewired();
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x0002207A File Offset: 0x0002027A
		private void OnEditorRecompile()
		{
			this.recompiling = true;
			this.ClearRewiredVars();
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x00022089 File Offset: 0x00020289
		private void ClearRewiredVars()
		{
			Array.Clear(this.playerIds, 0, this.playerIds.Length);
			base.ClearMouseInputSources();
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x00105550 File Offset: 0x00103750
		private bool DidAnyMouseMove()
		{
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				int playerId = this.playerIds[i];
				Player player = ReInput.players.GetPlayer(playerId);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					int mouseInputSourceCount = base.GetMouseInputSourceCount(playerId);
					for (int j = 0; j < mouseInputSourceCount; j++)
					{
						IMouseInputSource mouseInputSource = base.GetMouseInputSource(playerId, j);
						if (mouseInputSource != null && mouseInputSource.screenPositionDelta.sqrMagnitude > 0f)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x001055DC File Offset: 0x001037DC
		private bool GetMouseButtonDownOnAnyMouse(int buttonIndex)
		{
			for (int i = 0; i < this.playerIds.Length; i++)
			{
				int playerId = this.playerIds[i];
				Player player = ReInput.players.GetPlayer(playerId);
				if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
				{
					int mouseInputSourceCount = base.GetMouseInputSourceCount(playerId);
					for (int j = 0; j < mouseInputSourceCount; j++)
					{
						IMouseInputSource mouseInputSource = base.GetMouseInputSource(playerId, j);
						if (mouseInputSource != null && mouseInputSource.GetButtonDown(buttonIndex))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x000220A5 File Offset: 0x000202A5
		private void OnRewiredInitialized()
		{
			this.InitializeRewired();
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x000220AD File Offset: 0x000202AD
		private void OnRewiredShutDown()
		{
			this.ClearRewiredVars();
		}

		// Token: 0x04003080 RID: 12416
		private const string DEFAULT_ACTION_MOVE_HORIZONTAL = "UIHorizontal";

		// Token: 0x04003081 RID: 12417
		private const string DEFAULT_ACTION_MOVE_VERTICAL = "UIVertical";

		// Token: 0x04003082 RID: 12418
		private const string DEFAULT_ACTION_SUBMIT = "UISubmit";

		// Token: 0x04003083 RID: 12419
		private const string DEFAULT_ACTION_CANCEL = "UICancel";

		// Token: 0x04003084 RID: 12420
		[Tooltip("(Optional) Link the Rewired Input Manager here for easier access to Player ids, etc.")]
		[SerializeField]
		private InputManager_Base rewiredInputManager;

		// Token: 0x04003085 RID: 12421
		[SerializeField]
		[Tooltip("Use all Rewired game Players to control the UI. This does not include the System Player. If enabled, this setting overrides individual Player Ids set in Rewired Player Ids.")]
		private bool useAllRewiredGamePlayers;

		// Token: 0x04003086 RID: 12422
		[SerializeField]
		[Tooltip("Allow the Rewired System Player to control the UI.")]
		private bool useRewiredSystemPlayer;

		// Token: 0x04003087 RID: 12423
		[SerializeField]
		[Tooltip("A list of Player Ids that are allowed to control the UI. If Use All Rewired Game Players = True, this list will be ignored.")]
		private int[] rewiredPlayerIds = new int[1];

		// Token: 0x04003088 RID: 12424
		[SerializeField]
		[Tooltip("Allow only Players with Player.isPlaying = true to control the UI.")]
		private bool usePlayingPlayersOnly;

		// Token: 0x04003089 RID: 12425
		[SerializeField]
		[Tooltip("Player Mice allowed to interact with the UI. Each Player that owns a Player Mouse must also be allowed to control the UI or the Player Mouse will not function.")]
		private List<PlayerMouse> playerMice = new List<PlayerMouse>();

		// Token: 0x0400308A RID: 12426
		[SerializeField]
		[Tooltip("Makes an axis press always move only one UI selection. Enable if you do not want to allow scrolling through UI elements by holding an axis direction.")]
		private bool moveOneElementPerAxisPress;

		// Token: 0x0400308B RID: 12427
		[SerializeField]
		[Tooltip("If enabled, Action Ids will be used to set the Actions. If disabled, string names will be used to set the Actions.")]
		private bool setActionsById;

		// Token: 0x0400308C RID: 12428
		[SerializeField]
		[Tooltip("Id of the horizontal Action for movement (if axis events are used).")]
		private int horizontalActionId = -1;

		// Token: 0x0400308D RID: 12429
		[SerializeField]
		[Tooltip("Id of the vertical Action for movement (if axis events are used).")]
		private int verticalActionId = -1;

		// Token: 0x0400308E RID: 12430
		[SerializeField]
		[Tooltip("Id of the Action used to submit.")]
		private int submitActionId = -1;

		// Token: 0x0400308F RID: 12431
		[SerializeField]
		[Tooltip("Id of the Action used to cancel.")]
		private int cancelActionId = -1;

		// Token: 0x04003090 RID: 12432
		[SerializeField]
		[Tooltip("Name of the horizontal axis for movement (if axis events are used).")]
		private string m_HorizontalAxis = "UIHorizontal";

		// Token: 0x04003091 RID: 12433
		[SerializeField]
		[Tooltip("Name of the vertical axis for movement (if axis events are used).")]
		private string m_VerticalAxis = "UIVertical";

		// Token: 0x04003092 RID: 12434
		[SerializeField]
		[Tooltip("Name of the action used to submit.")]
		private string m_SubmitButton = "UISubmit";

		// Token: 0x04003093 RID: 12435
		[SerializeField]
		[Tooltip("Name of the action used to cancel.")]
		private string m_CancelButton = "UICancel";

		// Token: 0x04003094 RID: 12436
		[SerializeField]
		[Tooltip("Number of selection changes allowed per second when a movement button/axis is held in a direction.")]
		private float m_InputActionsPerSecond = 10f;

		// Token: 0x04003095 RID: 12437
		[SerializeField]
		[Tooltip("Delay in seconds before vertical/horizontal movement starts repeating continouously when a movement direction is held.")]
		private float m_RepeatDelay;

		// Token: 0x04003096 RID: 12438
		[SerializeField]
		[Tooltip("Allows the mouse to be used to select elements.")]
		private bool m_allowMouseInput = true;

		// Token: 0x04003097 RID: 12439
		[SerializeField]
		[Tooltip("Allows the mouse to be used to select elements if the device also supports touch control.")]
		private bool m_allowMouseInputIfTouchSupported = true;

		// Token: 0x04003098 RID: 12440
		[SerializeField]
		[Tooltip("Allows touch input to be used to select elements.")]
		private bool m_allowTouchInput = true;

		// Token: 0x04003099 RID: 12441
		[SerializeField]
		[Tooltip("Deselects the current selection on mouse/touch click when the pointer is not over a selectable object.")]
		private bool m_deselectIfBackgroundClicked = true;

		// Token: 0x0400309A RID: 12442
		[SerializeField]
		[Tooltip("Deselects the current selection on mouse/touch click before selecting the next object.")]
		private bool m_deselectBeforeSelecting = true;

		// Token: 0x0400309B RID: 12443
		[SerializeField]
		[FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
		[Tooltip("Forces the module to always be active.")]
		private bool m_ForceModuleActive;

		// Token: 0x0400309C RID: 12444
		[NonSerialized]
		private int[] playerIds;

		// Token: 0x0400309D RID: 12445
		private bool recompiling;

		// Token: 0x0400309E RID: 12446
		[NonSerialized]
		private bool isTouchSupported;

		// Token: 0x0400309F RID: 12447
		[NonSerialized]
		private double m_PrevActionTime;

		// Token: 0x040030A0 RID: 12448
		[NonSerialized]
		private Vector2 m_LastMoveVector;

		// Token: 0x040030A1 RID: 12449
		[NonSerialized]
		private int m_ConsecutiveMoveCount;

		// Token: 0x040030A2 RID: 12450
		[NonSerialized]
		private bool m_HasFocus = true;

		// Token: 0x020006B6 RID: 1718
		[Serializable]
		public class PlayerSetting
		{
			// Token: 0x0600321E RID: 12830 RVA: 0x000220B5 File Offset: 0x000202B5
			public PlayerSetting()
			{
			}

			// Token: 0x0600321F RID: 12831 RVA: 0x00105658 File Offset: 0x00103858
			private PlayerSetting(RewiredStandaloneInputModule.PlayerSetting other)
			{
				if (other == null)
				{
					throw new ArgumentNullException("other");
				}
				this.playerId = other.playerId;
				this.playerMice = new List<PlayerMouse>();
				if (other.playerMice != null)
				{
					foreach (PlayerMouse item in other.playerMice)
					{
						this.playerMice.Add(item);
					}
				}
			}

			// Token: 0x06003220 RID: 12832 RVA: 0x000220C8 File Offset: 0x000202C8
			public RewiredStandaloneInputModule.PlayerSetting Clone()
			{
				return new RewiredStandaloneInputModule.PlayerSetting(this);
			}

			// Token: 0x040030A3 RID: 12451
			public int playerId;

			// Token: 0x040030A4 RID: 12452
			public List<PlayerMouse> playerMice = new List<PlayerMouse>();
		}
	}
}
