using System;
using System.Collections.Generic;
using System.ComponentModel;
using Rewired.Internal;
using Rewired.Utils.Interfaces;
using Rewired.Utils.Platforms.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Utils
{
	// Token: 0x02000648 RID: 1608
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class ExternalTools : IExternalTools
	{
		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002C05 RID: 11269 RVA: 0x0001DD28 File Offset: 0x0001BF28
		// (set) Token: 0x06002C06 RID: 11270 RVA: 0x0001DD2F File Offset: 0x0001BF2F
		public static Func<object> getPlatformInitializerDelegate
		{
			get
			{
				return ExternalTools._getPlatformInitializerDelegate;
			}
			set
			{
				ExternalTools._getPlatformInitializerDelegate = value;
			}
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x0000398C File Offset: 0x00001B8C
		public void Destroy()
		{
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06002C09 RID: 11273 RVA: 0x0001DD37 File Offset: 0x0001BF37
		public bool isEditorPaused
		{
			get
			{
				return this._isEditorPaused;
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06002C0A RID: 11274 RVA: 0x0001DD3F File Offset: 0x0001BF3F
		// (remove) Token: 0x06002C0B RID: 11275 RVA: 0x0001DD58 File Offset: 0x0001BF58
		public event Action<bool> EditorPausedStateChangedEvent
		{
			add
			{
				this._EditorPausedStateChangedEvent = (Action<bool>)Delegate.Combine(this._EditorPausedStateChangedEvent, value);
			}
			remove
			{
				this._EditorPausedStateChangedEvent = (Action<bool>)Delegate.Remove(this._EditorPausedStateChangedEvent, value);
			}
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x0001DD71 File Offset: 0x0001BF71
		public object GetPlatformInitializer()
		{
			return Main.GetPlatformInitializer();
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x0001DD78 File Offset: 0x0001BF78
		public string GetFocusedEditorWindowTitle()
		{
			return string.Empty;
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x0000539F File Offset: 0x0000359F
		public bool IsEditorSceneViewFocused()
		{
			return false;
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x0000539F File Offset: 0x0000359F
		public bool LinuxInput_IsJoystickPreconfigured(string name)
		{
			return false;
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06002C10 RID: 11280 RVA: 0x000F6FF8 File Offset: 0x000F51F8
		// (remove) Token: 0x06002C11 RID: 11281 RVA: 0x000F7030 File Offset: 0x000F5230
		public event Action<uint, bool> XboxOneInput_OnGamepadStateChange;

		// Token: 0x06002C12 RID: 11282 RVA: 0x0000539F File Offset: 0x0000359F
		public int XboxOneInput_GetUserIdForGamepad(uint id)
		{
			return 0;
		}

		// Token: 0x06002C13 RID: 11283 RVA: 0x0000FF7B File Offset: 0x0000E17B
		public ulong XboxOneInput_GetControllerId(uint unityJoystickId)
		{
			return 0UL;
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x0000539F File Offset: 0x0000359F
		public bool XboxOneInput_IsGamepadActive(uint unityJoystickId)
		{
			return false;
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x0001DD78 File Offset: 0x0001BF78
		public string XboxOneInput_GetControllerType(ulong xboxControllerId)
		{
			return string.Empty;
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x0000539F File Offset: 0x0000359F
		public uint XboxOneInput_GetJoystickId(ulong xboxControllerId)
		{
			return 0U;
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x0000398C File Offset: 0x00001B8C
		public void XboxOne_Gamepad_UpdatePlugin()
		{
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x0000539F File Offset: 0x0000359F
		public bool XboxOne_Gamepad_SetGamepadVibration(ulong xboxOneJoystickId, float leftMotor, float rightMotor, float leftTriggerLevel, float rightTriggerLevel)
		{
			return false;
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x0000398C File Offset: 0x00001B8C
		public void XboxOne_Gamepad_PulseVibrateMotor(ulong xboxOneJoystickId, int motorInt, float startLevel, float endLevel, ulong durationMS)
		{
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x0001DD7F File Offset: 0x0001BF7F
		public Vector3 PS4Input_GetLastAcceleration(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x0001DD7F File Offset: 0x0001BF7F
		public Vector3 PS4Input_GetLastGyro(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x0001DD86 File Offset: 0x0001BF86
		public Vector4 PS4Input_GetLastOrientation(int id)
		{
			return Vector4.zero;
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x0001DD8D File Offset: 0x0001BF8D
		public void PS4Input_GetLastTouchData(int id, out int touchNum, out int touch0x, out int touch0y, out int touch0id, out int touch1x, out int touch1y, out int touch1id)
		{
			touchNum = 0;
			touch0x = 0;
			touch0y = 0;
			touch0id = 0;
			touch1x = 0;
			touch1y = 0;
			touch1id = 0;
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x0001DDA9 File Offset: 0x0001BFA9
		public void PS4Input_GetPadControllerInformation(int id, out float touchpixelDensity, out int touchResolutionX, out int touchResolutionY, out int analogDeadZoneLeft, out int analogDeadZoneright, out int connectionType)
		{
			touchpixelDensity = 0f;
			touchResolutionX = 0;
			touchResolutionY = 0;
			analogDeadZoneLeft = 0;
			analogDeadZoneright = 0;
			connectionType = 0;
		}

		// Token: 0x06002C1F RID: 11295 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_PadSetMotionSensorState(int id, bool bEnable)
		{
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_PadSetTiltCorrectionState(int id, bool bEnable)
		{
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_PadSetAngularVelocityDeadbandState(int id, bool bEnable)
		{
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_PadSetLightBar(int id, int red, int green, int blue)
		{
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_PadResetLightBar(int id)
		{
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_PadSetVibration(int id, int largeMotor, int smallMotor)
		{
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_PadResetOrientation(int id)
		{
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x0000539F File Offset: 0x0000359F
		public bool PS4Input_PadIsConnected(int id)
		{
			return false;
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_GetUsersDetails(int slot, object loggedInUser)
		{
		}

		// Token: 0x06002C28 RID: 11304 RVA: 0x00012147 File Offset: 0x00010347
		public int PS4Input_GetDeviceClassForHandle(int handle)
		{
			return -1;
		}

		// Token: 0x06002C29 RID: 11305 RVA: 0x000053AE File Offset: 0x000035AE
		public string PS4Input_GetDeviceClassString(int intValue)
		{
			return null;
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x0000539F File Offset: 0x0000359F
		public int PS4Input_PadGetUsersHandles2(int maxControllers, int[] handles)
		{
			return 0;
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_GetSpecialControllerInformation(int id, int padIndex, object controllerInformation)
		{
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x0001DD7F File Offset: 0x0001BF7F
		public Vector3 PS4Input_SpecialGetLastAcceleration(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x0001DD7F File Offset: 0x0001BF7F
		public Vector3 PS4Input_SpecialGetLastGyro(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06002C2E RID: 11310 RVA: 0x0001DD86 File Offset: 0x0001BF86
		public Vector4 PS4Input_SpecialGetLastOrientation(int id)
		{
			return Vector4.zero;
		}

		// Token: 0x06002C2F RID: 11311 RVA: 0x0000539F File Offset: 0x0000359F
		public int PS4Input_SpecialGetUsersHandles(int maxNumberControllers, int[] handles)
		{
			return 0;
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x0000539F File Offset: 0x0000359F
		public int PS4Input_SpecialGetUsersHandles2(int maxNumberControllers, int[] handles)
		{
			return 0;
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x0000539F File Offset: 0x0000359F
		public bool PS4Input_SpecialIsConnected(int id)
		{
			return false;
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_SpecialResetLightSphere(int id)
		{
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_SpecialResetOrientation(int id)
		{
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_SpecialSetAngularVelocityDeadbandState(int id, bool bEnable)
		{
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_SpecialSetLightSphere(int id, int red, int green, int blue)
		{
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_SpecialSetMotionSensorState(int id, bool bEnable)
		{
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_SpecialSetTiltCorrectionState(int id, bool bEnable)
		{
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_SpecialSetVibration(int id, int largeMotor, int smallMotor)
		{
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x0001DD7F File Offset: 0x0001BF7F
		public Vector3 PS4Input_AimGetLastAcceleration(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x0001DD7F File Offset: 0x0001BF7F
		public Vector3 PS4Input_AimGetLastGyro(int id)
		{
			return Vector3.zero;
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x0001DD86 File Offset: 0x0001BF86
		public Vector4 PS4Input_AimGetLastOrientation(int id)
		{
			return Vector4.zero;
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x0000539F File Offset: 0x0000359F
		public int PS4Input_AimGetUsersHandles(int maxNumberControllers, int[] handles)
		{
			return 0;
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x0000539F File Offset: 0x0000359F
		public int PS4Input_AimGetUsersHandles2(int maxNumberControllers, int[] handles)
		{
			return 0;
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x0000539F File Offset: 0x0000359F
		public bool PS4Input_AimIsConnected(int id)
		{
			return false;
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_AimResetLightSphere(int id)
		{
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_AimResetOrientation(int id)
		{
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_AimSetAngularVelocityDeadbandState(int id, bool bEnable)
		{
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_AimSetLightSphere(int id, int red, int green, int blue)
		{
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_AimSetMotionSensorState(int id, bool bEnable)
		{
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_AimSetTiltCorrectionState(int id, bool bEnable)
		{
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x0000398C File Offset: 0x00001B8C
		public void PS4Input_AimSetVibration(int id, int largeMotor, int smallMotor)
		{
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x0001DD7F File Offset: 0x0001BF7F
		public Vector3 PS4Input_GetLastMoveAcceleration(int id, int index)
		{
			return Vector3.zero;
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x0001DD7F File Offset: 0x0001BF7F
		public Vector3 PS4Input_GetLastMoveGyro(int id, int index)
		{
			return Vector3.zero;
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x0000539F File Offset: 0x0000359F
		public int PS4Input_MoveGetButtons(int id, int index)
		{
			return 0;
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x0000539F File Offset: 0x0000359F
		public int PS4Input_MoveGetAnalogButton(int id, int index)
		{
			return 0;
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x0000539F File Offset: 0x0000359F
		public bool PS4Input_MoveIsConnected(int id, int index)
		{
			return false;
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x0000539F File Offset: 0x0000359F
		public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers, int[] primaryHandles, int[] secondaryHandles)
		{
			return 0;
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x0000539F File Offset: 0x0000359F
		public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers, int[] primaryHandles)
		{
			return 0;
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x0000539F File Offset: 0x0000359F
		public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers)
		{
			return 0;
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x0001DDC5 File Offset: 0x0001BFC5
		public IntPtr PS4Input_MoveGetControllerInputForTracking()
		{
			return IntPtr.Zero;
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x0000539F File Offset: 0x0000359F
		public int PS4Input_MoveSetLightSphere(int id, int index, int red, int green, int blue)
		{
			return 0;
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x0000539F File Offset: 0x0000359F
		public int PS4Input_MoveSetVibration(int id, int index, int motor)
		{
			return 0;
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x0001DDCC File Offset: 0x0001BFCC
		public void GetDeviceVIDPIDs(out List<int> vids, out List<int> pids)
		{
			vids = new List<int>();
			pids = new List<int>();
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x00012147 File Offset: 0x00010347
		public int GetAndroidAPILevel()
		{
			return -1;
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x0001DDDC File Offset: 0x0001BFDC
		public bool UnityUI_Graphic_GetRaycastTarget(object graphic)
		{
			return !(graphic as Graphic == null) && (graphic as Graphic).raycastTarget;
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x0001DDF9 File Offset: 0x0001BFF9
		public void UnityUI_Graphic_SetRaycastTarget(object graphic, bool value)
		{
			if (graphic as Graphic == null)
			{
				return;
			}
			(graphic as Graphic).raycastTarget = value;
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06002C55 RID: 11349 RVA: 0x0001DE16 File Offset: 0x0001C016
		public bool UnityInput_IsTouchPressureSupported
		{
			get
			{
				return Input.touchPressureSupported;
			}
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x0001DE1D File Offset: 0x0001C01D
		public float UnityInput_GetTouchPressure(ref Touch touch)
		{
			return touch.pressure;
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x0001DE25 File Offset: 0x0001C025
		public float UnityInput_GetTouchMaximumPossiblePressure(ref Touch touch)
		{
			return touch.maximumPossiblePressure;
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x0001DE2D File Offset: 0x0001C02D
		public IControllerTemplate CreateControllerTemplate(Guid typeGuid, object payload)
		{
			return ControllerTemplateFactory.Create(typeGuid, payload);
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x0001DE36 File Offset: 0x0001C036
		public Type[] GetControllerTemplateTypes()
		{
			return ControllerTemplateFactory.templateTypes;
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x0001DE3D File Offset: 0x0001C03D
		public Type[] GetControllerTemplateInterfaceTypes()
		{
			return ControllerTemplateFactory.templateInterfaceTypes;
		}

		// Token: 0x04002DE2 RID: 11746
		private static Func<object> _getPlatformInitializerDelegate;

		// Token: 0x04002DE3 RID: 11747
		private bool _isEditorPaused;

		// Token: 0x04002DE4 RID: 11748
		private Action<bool> _EditorPausedStateChangedEvent;
	}
}
