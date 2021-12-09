using System;
using System.Collections.Generic;
using System.Text;
using Rewired.UI;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rewired.Integration.UnityUI
{
	// Token: 0x020006AF RID: 1711
	public abstract class RewiredPointerInputModule : BaseInputModule
	{
		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06003187 RID: 12679 RVA: 0x00103544 File Offset: 0x00101744
		private RewiredPointerInputModule.UnityInputSource defaultInputSource
		{
			get
			{
				if (this.__m_DefaultInputSource == null)
				{
					return this.__m_DefaultInputSource = new RewiredPointerInputModule.UnityInputSource();
				}
				return this.__m_DefaultInputSource;
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06003188 RID: 12680 RVA: 0x000219C6 File Offset: 0x0001FBC6
		private IMouseInputSource defaultMouseInputSource
		{
			get
			{
				return this.defaultInputSource;
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06003189 RID: 12681 RVA: 0x000219C6 File Offset: 0x0001FBC6
		protected ITouchInputSource defaultTouchInputSource
		{
			get
			{
				return this.defaultInputSource;
			}
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x000219CE File Offset: 0x0001FBCE
		protected bool IsDefaultMouse(IMouseInputSource mouse)
		{
			return this.defaultMouseInputSource == mouse;
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x00103570 File Offset: 0x00101770
		public IMouseInputSource GetMouseInputSource(int playerId, int mouseIndex)
		{
			if (mouseIndex < 0)
			{
				throw new ArgumentOutOfRangeException("mouseIndex");
			}
			if (this.m_MouseInputSourcesList.Count == 0 && this.IsDefaultPlayer(playerId))
			{
				return this.defaultMouseInputSource;
			}
			int count = this.m_MouseInputSourcesList.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				IMouseInputSource mouseInputSource = this.m_MouseInputSourcesList[i];
				if (!UnityTools.IsNullOrDestroyed<IMouseInputSource>(mouseInputSource) && mouseInputSource.playerId == playerId)
				{
					if (mouseIndex == num)
					{
						return mouseInputSource;
					}
					num++;
				}
			}
			return null;
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x000219D9 File Offset: 0x0001FBD9
		public void RemoveMouseInputSource(IMouseInputSource source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			this.m_MouseInputSourcesList.Remove(source);
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x000219F6 File Offset: 0x0001FBF6
		public void AddMouseInputSource(IMouseInputSource source)
		{
			if (UnityTools.IsNullOrDestroyed<IMouseInputSource>(source))
			{
				throw new ArgumentNullException("source");
			}
			this.m_MouseInputSourcesList.Add(source);
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x001035EC File Offset: 0x001017EC
		public int GetMouseInputSourceCount(int playerId)
		{
			if (this.m_MouseInputSourcesList.Count == 0 && this.IsDefaultPlayer(playerId))
			{
				return 1;
			}
			int count = this.m_MouseInputSourcesList.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				IMouseInputSource mouseInputSource = this.m_MouseInputSourcesList[i];
				if (!UnityTools.IsNullOrDestroyed<IMouseInputSource>(mouseInputSource) && mouseInputSource.playerId == playerId)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x00021A17 File Offset: 0x0001FC17
		public ITouchInputSource GetTouchInputSource(int playerId, int sourceIndex)
		{
			if (!UnityTools.IsNullOrDestroyed<ITouchInputSource>(this.m_UserDefaultTouchInputSource))
			{
				return this.m_UserDefaultTouchInputSource;
			}
			return this.defaultTouchInputSource;
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x00021A33 File Offset: 0x0001FC33
		public void RemoveTouchInputSource(ITouchInputSource source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (this.m_UserDefaultTouchInputSource == source)
			{
				this.m_UserDefaultTouchInputSource = null;
			}
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x00021A53 File Offset: 0x0001FC53
		public void AddTouchInputSource(ITouchInputSource source)
		{
			if (UnityTools.IsNullOrDestroyed<ITouchInputSource>(source))
			{
				throw new ArgumentNullException("source");
			}
			this.m_UserDefaultTouchInputSource = source;
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x00021A6F File Offset: 0x0001FC6F
		public int GetTouchInputSourceCount(int playerId)
		{
			if (!this.IsDefaultPlayer(playerId))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x00021A7D File Offset: 0x0001FC7D
		protected void ClearMouseInputSources()
		{
			this.m_MouseInputSourcesList.Clear();
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06003194 RID: 12692 RVA: 0x00103650 File Offset: 0x00101850
		protected virtual bool isMouseSupported
		{
			get
			{
				int count = this.m_MouseInputSourcesList.Count;
				if (count == 0)
				{
					return this.defaultMouseInputSource.enabled;
				}
				for (int i = 0; i < count; i++)
				{
					if (this.m_MouseInputSourcesList[i].enabled)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06003195 RID: 12693
		protected abstract bool IsDefaultPlayer(int playerId);

		// Token: 0x06003196 RID: 12694 RVA: 0x0010369C File Offset: 0x0010189C
		protected bool GetPointerData(int playerId, int pointerIndex, int pointerTypeId, out PlayerPointerEventData data, bool create, PointerEventType pointerEventType)
		{
			Dictionary<int, PlayerPointerEventData>[] array;
			if (!this.m_PlayerPointerData.TryGetValue(playerId, out array))
			{
				array = new Dictionary<int, PlayerPointerEventData>[pointerIndex + 1];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new Dictionary<int, PlayerPointerEventData>();
				}
				this.m_PlayerPointerData.Add(playerId, array);
			}
			if (pointerIndex >= array.Length)
			{
				Dictionary<int, PlayerPointerEventData>[] array2 = new Dictionary<int, PlayerPointerEventData>[pointerIndex + 1];
				for (int j = 0; j < array.Length; j++)
				{
					array2[j] = array[j];
				}
				array2[pointerIndex] = new Dictionary<int, PlayerPointerEventData>();
				array = array2;
				this.m_PlayerPointerData[playerId] = array;
			}
			Dictionary<int, PlayerPointerEventData> dictionary = array[pointerIndex];
			if (dictionary.TryGetValue(pointerTypeId, out data))
			{
				data.mouseSource = ((pointerEventType == PointerEventType.Mouse) ? this.GetMouseInputSource(playerId, pointerIndex) : null);
				data.touchSource = ((pointerEventType == PointerEventType.Touch) ? this.GetTouchInputSource(playerId, pointerIndex) : null);
				return false;
			}
			if (!create)
			{
				return false;
			}
			data = this.CreatePointerEventData(playerId, pointerIndex, pointerTypeId, pointerEventType);
			dictionary.Add(pointerTypeId, data);
			return true;
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x00103784 File Offset: 0x00101984
		private PlayerPointerEventData CreatePointerEventData(int playerId, int pointerIndex, int pointerTypeId, PointerEventType pointerEventType)
		{
			PlayerPointerEventData playerPointerEventData = new PlayerPointerEventData(base.eventSystem)
			{
				playerId = playerId,
				inputSourceIndex = pointerIndex,
				pointerId = pointerTypeId,
				sourceType = pointerEventType
			};
			if (pointerEventType == PointerEventType.Mouse)
			{
				playerPointerEventData.mouseSource = this.GetMouseInputSource(playerId, pointerIndex);
			}
			else if (pointerEventType == PointerEventType.Touch)
			{
				playerPointerEventData.touchSource = this.GetTouchInputSource(playerId, pointerIndex);
			}
			if (pointerTypeId == -1)
			{
				playerPointerEventData.buttonIndex = 0;
			}
			else if (pointerTypeId == -2)
			{
				playerPointerEventData.buttonIndex = 1;
			}
			else if (pointerTypeId == -3)
			{
				playerPointerEventData.buttonIndex = 2;
			}
			else if (pointerTypeId >= -2147483520 && pointerTypeId <= -2147483392)
			{
				playerPointerEventData.buttonIndex = pointerTypeId - -2147483520;
			}
			return playerPointerEventData;
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x00103828 File Offset: 0x00101A28
		protected void RemovePointerData(PlayerPointerEventData data)
		{
			Dictionary<int, PlayerPointerEventData>[] array;
			if (this.m_PlayerPointerData.TryGetValue(data.playerId, out array) && data.inputSourceIndex < array.Length)
			{
				array[data.inputSourceIndex].Remove(data.pointerId);
			}
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x0010386C File Offset: 0x00101A6C
		protected PlayerPointerEventData GetTouchPointerEventData(int playerId, int touchDeviceIndex, Touch input, out bool pressed, out bool released)
		{
			PlayerPointerEventData playerPointerEventData;
			bool pointerData = this.GetPointerData(playerId, touchDeviceIndex, input.fingerId, out playerPointerEventData, true, PointerEventType.Touch);
			playerPointerEventData.Reset();
			pressed = (pointerData || input.phase == TouchPhase.Began);
			released = (input.phase == TouchPhase.Canceled || input.phase == TouchPhase.Ended);
			if (pointerData)
			{
				playerPointerEventData.position = input.position;
			}
			if (pressed)
			{
				playerPointerEventData.delta = Vector2.zero;
			}
			else
			{
				playerPointerEventData.delta = input.position - playerPointerEventData.position;
			}
			playerPointerEventData.position = input.position;
			playerPointerEventData.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(playerPointerEventData, this.m_RaycastResultCache);
			RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			playerPointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
			this.m_RaycastResultCache.Clear();
			return playerPointerEventData;
		}

		// Token: 0x0600319A RID: 12698 RVA: 0x00103940 File Offset: 0x00101B40
		protected virtual RewiredPointerInputModule.MouseState GetMousePointerEventData(int playerId, int mouseIndex)
		{
			IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, mouseIndex);
			if (mouseInputSource == null)
			{
				return null;
			}
			PlayerPointerEventData playerPointerEventData;
			bool pointerData = this.GetPointerData(playerId, mouseIndex, -1, out playerPointerEventData, true, PointerEventType.Mouse);
			playerPointerEventData.Reset();
			if (pointerData)
			{
				playerPointerEventData.position = mouseInputSource.screenPosition;
			}
			Vector2 screenPosition = mouseInputSource.screenPosition;
			if (mouseInputSource.locked || !mouseInputSource.enabled)
			{
				playerPointerEventData.position = new Vector2(-1f, -1f);
				playerPointerEventData.delta = Vector2.zero;
			}
			else
			{
				playerPointerEventData.delta = screenPosition - playerPointerEventData.position;
				playerPointerEventData.position = screenPosition;
			}
			playerPointerEventData.scrollDelta = mouseInputSource.wheelDelta;
			playerPointerEventData.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(playerPointerEventData, this.m_RaycastResultCache);
			RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			playerPointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
			this.m_RaycastResultCache.Clear();
			PlayerPointerEventData playerPointerEventData2;
			this.GetPointerData(playerId, mouseIndex, -2, out playerPointerEventData2, true, PointerEventType.Mouse);
			this.CopyFromTo(playerPointerEventData, playerPointerEventData2);
			playerPointerEventData2.button = PointerEventData.InputButton.Right;
			PlayerPointerEventData playerPointerEventData3;
			this.GetPointerData(playerId, mouseIndex, -3, out playerPointerEventData3, true, PointerEventType.Mouse);
			this.CopyFromTo(playerPointerEventData, playerPointerEventData3);
			playerPointerEventData3.button = PointerEventData.InputButton.Middle;
			for (int i = 3; i < mouseInputSource.buttonCount; i++)
			{
				PlayerPointerEventData playerPointerEventData4;
				this.GetPointerData(playerId, mouseIndex, -2147483520 + i, out playerPointerEventData4, true, PointerEventType.Mouse);
				this.CopyFromTo(playerPointerEventData, playerPointerEventData4);
				playerPointerEventData4.button = (PointerEventData.InputButton)(-1);
			}
			this.m_MouseState.SetButtonState(0, this.StateForMouseButton(playerId, mouseIndex, 0), playerPointerEventData);
			this.m_MouseState.SetButtonState(1, this.StateForMouseButton(playerId, mouseIndex, 1), playerPointerEventData2);
			this.m_MouseState.SetButtonState(2, this.StateForMouseButton(playerId, mouseIndex, 2), playerPointerEventData3);
			for (int j = 3; j < mouseInputSource.buttonCount; j++)
			{
				PlayerPointerEventData data;
				this.GetPointerData(playerId, mouseIndex, -2147483520 + j, out data, false, PointerEventType.Mouse);
				this.m_MouseState.SetButtonState(j, this.StateForMouseButton(playerId, mouseIndex, j), data);
			}
			return this.m_MouseState;
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x00103B1C File Offset: 0x00101D1C
		protected PlayerPointerEventData GetLastPointerEventData(int playerId, int pointerIndex, int pointerTypeId, bool ignorePointerTypeId, PointerEventType pointerEventType)
		{
			if (!ignorePointerTypeId)
			{
				PlayerPointerEventData result;
				this.GetPointerData(playerId, pointerIndex, pointerTypeId, out result, false, pointerEventType);
				return result;
			}
			Dictionary<int, PlayerPointerEventData>[] array;
			if (!this.m_PlayerPointerData.TryGetValue(playerId, out array))
			{
				return null;
			}
			if (pointerIndex >= array.Length)
			{
				return null;
			}
			using (Dictionary<int, PlayerPointerEventData>.Enumerator enumerator = array[pointerIndex].GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<int, PlayerPointerEventData> keyValuePair = enumerator.Current;
					return keyValuePair.Value;
				}
			}
			return null;
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x00103BA4 File Offset: 0x00101DA4
		private static bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
		{
			return !useDragThreshold || (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x00103BD0 File Offset: 0x00101DD0
		protected virtual void ProcessMove(PlayerPointerEventData pointerEvent)
		{
			GameObject newEnterTarget;
			if (pointerEvent.sourceType == PointerEventType.Mouse)
			{
				IMouseInputSource mouseInputSource = this.GetMouseInputSource(pointerEvent.playerId, pointerEvent.inputSourceIndex);
				if (mouseInputSource != null)
				{
					newEnterTarget = ((!mouseInputSource.enabled || mouseInputSource.locked) ? null : pointerEvent.pointerCurrentRaycast.gameObject);
				}
				else
				{
					newEnterTarget = null;
				}
			}
			else
			{
				if (pointerEvent.sourceType != PointerEventType.Touch)
				{
					throw new NotImplementedException();
				}
				newEnterTarget = pointerEvent.pointerCurrentRaycast.gameObject;
			}
			base.HandlePointerExitAndEnter(pointerEvent, newEnterTarget);
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x00103C4C File Offset: 0x00101E4C
		protected virtual void ProcessDrag(PlayerPointerEventData pointerEvent)
		{
			if (!pointerEvent.IsPointerMoving() || pointerEvent.pointerDrag == null)
			{
				return;
			}
			if (pointerEvent.sourceType == PointerEventType.Mouse)
			{
				IMouseInputSource mouseInputSource = this.GetMouseInputSource(pointerEvent.playerId, pointerEvent.inputSourceIndex);
				if (mouseInputSource == null || mouseInputSource.locked || !mouseInputSource.enabled)
				{
					return;
				}
			}
			if (!pointerEvent.dragging && RewiredPointerInputModule.ShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, (float)base.eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
			{
				ExecuteEvents.Execute<IBeginDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
				pointerEvent.dragging = true;
			}
			if (pointerEvent.dragging)
			{
				if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
				{
					ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
					pointerEvent.eligibleForClick = false;
					pointerEvent.pointerPress = null;
					pointerEvent.rawPointerPress = null;
				}
				ExecuteEvents.Execute<IDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
			}
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x00103D3C File Offset: 0x00101F3C
		public override bool IsPointerOverGameObject(int pointerTypeId)
		{
			foreach (KeyValuePair<int, Dictionary<int, PlayerPointerEventData>[]> keyValuePair in this.m_PlayerPointerData)
			{
				Dictionary<int, PlayerPointerEventData>[] value = keyValuePair.Value;
				for (int i = 0; i < value.Length; i++)
				{
					PlayerPointerEventData playerPointerEventData;
					if (value[i].TryGetValue(pointerTypeId, out playerPointerEventData) && playerPointerEventData.pointerEnter != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x00103DC4 File Offset: 0x00101FC4
		protected void ClearSelection()
		{
			BaseEventData baseEventData = this.GetBaseEventData();
			foreach (KeyValuePair<int, Dictionary<int, PlayerPointerEventData>[]> keyValuePair in this.m_PlayerPointerData)
			{
				Dictionary<int, PlayerPointerEventData>[] value = keyValuePair.Value;
				for (int i = 0; i < value.Length; i++)
				{
					foreach (KeyValuePair<int, PlayerPointerEventData> keyValuePair2 in value[i])
					{
						base.HandlePointerExitAndEnter(keyValuePair2.Value, null);
					}
					value[i].Clear();
				}
			}
			base.eventSystem.SetSelectedGameObject(null, baseEventData);
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x00103E90 File Offset: 0x00102090
		public override string ToString()
		{
			string str = "<b>Pointer Input Module of type: </b>";
			Type type = base.GetType();
			StringBuilder stringBuilder = new StringBuilder(str + ((type != null) ? type.ToString() : null));
			stringBuilder.AppendLine();
			foreach (KeyValuePair<int, Dictionary<int, PlayerPointerEventData>[]> keyValuePair in this.m_PlayerPointerData)
			{
				stringBuilder.AppendLine("<B>Player Id:</b> " + keyValuePair.Key.ToString());
				Dictionary<int, PlayerPointerEventData>[] value = keyValuePair.Value;
				for (int i = 0; i < value.Length; i++)
				{
					stringBuilder.AppendLine("<B>Pointer Index:</b> " + i.ToString());
					foreach (KeyValuePair<int, PlayerPointerEventData> keyValuePair2 in value[i])
					{
						stringBuilder.AppendLine("<B>Button Id:</b> " + keyValuePair2.Key.ToString());
						stringBuilder.AppendLine(keyValuePair2.Value.ToString());
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x00021A8A File Offset: 0x0001FC8A
		protected void DeselectIfSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
		{
			if (ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo) != base.eventSystem.currentSelectedGameObject)
			{
				base.eventSystem.SetSelectedGameObject(null, pointerEvent);
			}
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x00021AB1 File Offset: 0x0001FCB1
		protected void CopyFromTo(PointerEventData from, PointerEventData to)
		{
			to.position = from.position;
			to.delta = from.delta;
			to.scrollDelta = from.scrollDelta;
			to.pointerCurrentRaycast = from.pointerCurrentRaycast;
			to.pointerEnter = from.pointerEnter;
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x00103FDC File Offset: 0x001021DC
		protected PointerEventData.FramePressState StateForMouseButton(int playerId, int mouseIndex, int buttonId)
		{
			IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, mouseIndex);
			if (mouseInputSource == null)
			{
				return PointerEventData.FramePressState.NotChanged;
			}
			bool buttonDown = mouseInputSource.GetButtonDown(buttonId);
			bool buttonUp = mouseInputSource.GetButtonUp(buttonId);
			if (buttonDown && buttonUp)
			{
				return PointerEventData.FramePressState.PressedAndReleased;
			}
			if (buttonDown)
			{
				return PointerEventData.FramePressState.Pressed;
			}
			if (buttonUp)
			{
				return PointerEventData.FramePressState.Released;
			}
			return PointerEventData.FramePressState.NotChanged;
		}

		// Token: 0x04003069 RID: 12393
		public const int kMouseLeftId = -1;

		// Token: 0x0400306A RID: 12394
		public const int kMouseRightId = -2;

		// Token: 0x0400306B RID: 12395
		public const int kMouseMiddleId = -3;

		// Token: 0x0400306C RID: 12396
		public const int kFakeTouchesId = -4;

		// Token: 0x0400306D RID: 12397
		private const int customButtonsStartingId = -2147483520;

		// Token: 0x0400306E RID: 12398
		private const int customButtonsMaxCount = 128;

		// Token: 0x0400306F RID: 12399
		private const int customButtonsLastId = -2147483392;

		// Token: 0x04003070 RID: 12400
		private readonly List<IMouseInputSource> m_MouseInputSourcesList = new List<IMouseInputSource>();

		// Token: 0x04003071 RID: 12401
		private Dictionary<int, Dictionary<int, PlayerPointerEventData>[]> m_PlayerPointerData = new Dictionary<int, Dictionary<int, PlayerPointerEventData>[]>();

		// Token: 0x04003072 RID: 12402
		private ITouchInputSource m_UserDefaultTouchInputSource;

		// Token: 0x04003073 RID: 12403
		private RewiredPointerInputModule.UnityInputSource __m_DefaultInputSource;

		// Token: 0x04003074 RID: 12404
		private readonly RewiredPointerInputModule.MouseState m_MouseState = new RewiredPointerInputModule.MouseState();

		// Token: 0x020006B0 RID: 1712
		protected class MouseState
		{
			// Token: 0x060031A6 RID: 12710 RVA: 0x0010401C File Offset: 0x0010221C
			public bool AnyPressesThisFrame()
			{
				for (int i = 0; i < this.m_TrackedButtons.Count; i++)
				{
					if (this.m_TrackedButtons[i].eventData.PressedThisFrame())
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060031A7 RID: 12711 RVA: 0x0010405C File Offset: 0x0010225C
			public bool AnyReleasesThisFrame()
			{
				for (int i = 0; i < this.m_TrackedButtons.Count; i++)
				{
					if (this.m_TrackedButtons[i].eventData.ReleasedThisFrame())
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060031A8 RID: 12712 RVA: 0x0010409C File Offset: 0x0010229C
			public RewiredPointerInputModule.ButtonState GetButtonState(int button)
			{
				RewiredPointerInputModule.ButtonState buttonState = null;
				for (int i = 0; i < this.m_TrackedButtons.Count; i++)
				{
					if (this.m_TrackedButtons[i].button == button)
					{
						buttonState = this.m_TrackedButtons[i];
						break;
					}
				}
				if (buttonState == null)
				{
					buttonState = new RewiredPointerInputModule.ButtonState
					{
						button = button,
						eventData = new RewiredPointerInputModule.MouseButtonEventData()
					};
					this.m_TrackedButtons.Add(buttonState);
				}
				return buttonState;
			}

			// Token: 0x060031A9 RID: 12713 RVA: 0x00021B18 File Offset: 0x0001FD18
			public void SetButtonState(int button, PointerEventData.FramePressState stateForMouseButton, PlayerPointerEventData data)
			{
				RewiredPointerInputModule.ButtonState buttonState = this.GetButtonState(button);
				buttonState.eventData.buttonState = stateForMouseButton;
				buttonState.eventData.buttonData = data;
			}

			// Token: 0x04003075 RID: 12405
			private List<RewiredPointerInputModule.ButtonState> m_TrackedButtons = new List<RewiredPointerInputModule.ButtonState>();
		}

		// Token: 0x020006B1 RID: 1713
		public class MouseButtonEventData
		{
			// Token: 0x060031AB RID: 12715 RVA: 0x00021B4B File Offset: 0x0001FD4B
			public bool PressedThisFrame()
			{
				return this.buttonState == PointerEventData.FramePressState.Pressed || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
			}

			// Token: 0x060031AC RID: 12716 RVA: 0x00021B60 File Offset: 0x0001FD60
			public bool ReleasedThisFrame()
			{
				return this.buttonState == PointerEventData.FramePressState.Released || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
			}

			// Token: 0x04003076 RID: 12406
			public PointerEventData.FramePressState buttonState;

			// Token: 0x04003077 RID: 12407
			public PlayerPointerEventData buttonData;
		}

		// Token: 0x020006B2 RID: 1714
		protected class ButtonState
		{
			// Token: 0x170008C1 RID: 2241
			// (get) Token: 0x060031AE RID: 12718 RVA: 0x00021B76 File Offset: 0x0001FD76
			// (set) Token: 0x060031AF RID: 12719 RVA: 0x00021B7E File Offset: 0x0001FD7E
			public RewiredPointerInputModule.MouseButtonEventData eventData
			{
				get
				{
					return this.m_EventData;
				}
				set
				{
					this.m_EventData = value;
				}
			}

			// Token: 0x170008C2 RID: 2242
			// (get) Token: 0x060031B0 RID: 12720 RVA: 0x00021B87 File Offset: 0x0001FD87
			// (set) Token: 0x060031B1 RID: 12721 RVA: 0x00021B8F File Offset: 0x0001FD8F
			public int button
			{
				get
				{
					return this.m_Button;
				}
				set
				{
					this.m_Button = value;
				}
			}

			// Token: 0x04003078 RID: 12408
			private int m_Button;

			// Token: 0x04003079 RID: 12409
			private RewiredPointerInputModule.MouseButtonEventData m_EventData;
		}

		// Token: 0x020006B3 RID: 1715
		private sealed class UnityInputSource : IMouseInputSource, ITouchInputSource
		{
			// Token: 0x170008C3 RID: 2243
			// (get) Token: 0x060031B3 RID: 12723 RVA: 0x00021B98 File Offset: 0x0001FD98
			int IMouseInputSource.playerId
			{
				get
				{
					this.TryUpdate();
					return 0;
				}
			}

			// Token: 0x170008C4 RID: 2244
			// (get) Token: 0x060031B4 RID: 12724 RVA: 0x00021B98 File Offset: 0x0001FD98
			int ITouchInputSource.playerId
			{
				get
				{
					this.TryUpdate();
					return 0;
				}
			}

			// Token: 0x170008C5 RID: 2245
			// (get) Token: 0x060031B5 RID: 12725 RVA: 0x00021BA1 File Offset: 0x0001FDA1
			bool IMouseInputSource.enabled
			{
				get
				{
					this.TryUpdate();
					return true;
				}
			}

			// Token: 0x170008C6 RID: 2246
			// (get) Token: 0x060031B6 RID: 12726 RVA: 0x00021BAA File Offset: 0x0001FDAA
			bool IMouseInputSource.locked
			{
				get
				{
					this.TryUpdate();
					return Cursor.lockState == CursorLockMode.Locked;
				}
			}

			// Token: 0x170008C7 RID: 2247
			// (get) Token: 0x060031B7 RID: 12727 RVA: 0x00021BBA File Offset: 0x0001FDBA
			int IMouseInputSource.buttonCount
			{
				get
				{
					this.TryUpdate();
					return 3;
				}
			}

			// Token: 0x060031B8 RID: 12728 RVA: 0x00021BC3 File Offset: 0x0001FDC3
			bool IMouseInputSource.GetButtonDown(int button)
			{
				this.TryUpdate();
				return Input.GetMouseButtonDown(button);
			}

			// Token: 0x060031B9 RID: 12729 RVA: 0x00021BD1 File Offset: 0x0001FDD1
			bool IMouseInputSource.GetButtonUp(int button)
			{
				this.TryUpdate();
				return Input.GetMouseButtonUp(button);
			}

			// Token: 0x060031BA RID: 12730 RVA: 0x00021BDF File Offset: 0x0001FDDF
			bool IMouseInputSource.GetButton(int button)
			{
				this.TryUpdate();
				return Input.GetMouseButton(button);
			}

			// Token: 0x170008C8 RID: 2248
			// (get) Token: 0x060031BB RID: 12731 RVA: 0x00021BED File Offset: 0x0001FDED
			Vector2 IMouseInputSource.screenPosition
			{
				get
				{
					this.TryUpdate();
					return Input.mousePosition;
				}
			}

			// Token: 0x170008C9 RID: 2249
			// (get) Token: 0x060031BC RID: 12732 RVA: 0x00021BFF File Offset: 0x0001FDFF
			Vector2 IMouseInputSource.screenPositionDelta
			{
				get
				{
					this.TryUpdate();
					return this.m_MousePosition - this.m_MousePositionPrev;
				}
			}

			// Token: 0x170008CA RID: 2250
			// (get) Token: 0x060031BD RID: 12733 RVA: 0x00021C18 File Offset: 0x0001FE18
			Vector2 IMouseInputSource.wheelDelta
			{
				get
				{
					this.TryUpdate();
					return Input.mouseScrollDelta;
				}
			}

			// Token: 0x170008CB RID: 2251
			// (get) Token: 0x060031BE RID: 12734 RVA: 0x00021C25 File Offset: 0x0001FE25
			bool ITouchInputSource.touchSupported
			{
				get
				{
					this.TryUpdate();
					return Input.touchSupported;
				}
			}

			// Token: 0x170008CC RID: 2252
			// (get) Token: 0x060031BF RID: 12735 RVA: 0x00021C32 File Offset: 0x0001FE32
			int ITouchInputSource.touchCount
			{
				get
				{
					this.TryUpdate();
					return Input.touchCount;
				}
			}

			// Token: 0x060031C0 RID: 12736 RVA: 0x00021C3F File Offset: 0x0001FE3F
			Touch ITouchInputSource.GetTouch(int index)
			{
				this.TryUpdate();
				return Input.GetTouch(index);
			}

			// Token: 0x060031C1 RID: 12737 RVA: 0x00021C4D File Offset: 0x0001FE4D
			private void TryUpdate()
			{
				if (Time.frameCount == this.m_LastUpdatedFrame)
				{
					return;
				}
				this.m_LastUpdatedFrame = Time.frameCount;
				this.m_MousePositionPrev = this.m_MousePosition;
				this.m_MousePosition = Input.mousePosition;
			}

			// Token: 0x0400307A RID: 12410
			private Vector2 m_MousePosition;

			// Token: 0x0400307B RID: 12411
			private Vector2 m_MousePositionPrev;

			// Token: 0x0400307C RID: 12412
			private int m_LastUpdatedFrame = -1;
		}
	}
}
