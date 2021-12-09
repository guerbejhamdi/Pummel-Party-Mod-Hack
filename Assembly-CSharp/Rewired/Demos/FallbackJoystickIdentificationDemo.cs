using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020006CE RID: 1742
	[AddComponentMenu("")]
	public class FallbackJoystickIdentificationDemo : MonoBehaviour
	{
		// Token: 0x060032E7 RID: 13031 RVA: 0x00022A08 File Offset: 0x00020C08
		private void Awake()
		{
			if (!ReInput.unityJoystickIdentificationRequired)
			{
				return;
			}
			ReInput.ControllerConnectedEvent += this.JoystickConnected;
			ReInput.ControllerDisconnectedEvent += this.JoystickDisconnected;
			this.IdentifyAllJoysticks();
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x00022A3A File Offset: 0x00020C3A
		private void JoystickConnected(ControllerStatusChangedEventArgs args)
		{
			this.IdentifyAllJoysticks();
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x00022A3A File Offset: 0x00020C3A
		private void JoystickDisconnected(ControllerStatusChangedEventArgs args)
		{
			this.IdentifyAllJoysticks();
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x00108B4C File Offset: 0x00106D4C
		public void IdentifyAllJoysticks()
		{
			this.Reset();
			if (ReInput.controllers.joystickCount == 0)
			{
				return;
			}
			Joystick[] joysticks = ReInput.controllers.GetJoysticks();
			if (joysticks == null)
			{
				return;
			}
			this.identifyRequired = true;
			this.joysticksToIdentify = new Queue<Joystick>(joysticks);
			this.SetInputDelay();
		}

		// Token: 0x060032EB RID: 13035 RVA: 0x00022A42 File Offset: 0x00020C42
		private void SetInputDelay()
		{
			this.nextInputAllowedTime = Time.time + 1f;
		}

		// Token: 0x060032EC RID: 13036 RVA: 0x00108B94 File Offset: 0x00106D94
		private void OnGUI()
		{
			if (!this.identifyRequired)
			{
				return;
			}
			if (this.joysticksToIdentify == null || this.joysticksToIdentify.Count == 0)
			{
				this.Reset();
				return;
			}
			Rect screenRect = new Rect((float)Screen.width * 0.5f - 125f, (float)Screen.height * 0.5f - 125f, 250f, 250f);
			GUILayout.Window(0, screenRect, new GUI.WindowFunction(this.DrawDialogWindow), "Joystick Identification Required", Array.Empty<GUILayoutOption>());
			GUI.FocusWindow(0);
			if (Time.time < this.nextInputAllowedTime)
			{
				return;
			}
			if (!ReInput.controllers.SetUnityJoystickIdFromAnyButtonOrAxisPress(this.joysticksToIdentify.Peek().id, 0.8f, false))
			{
				return;
			}
			this.joysticksToIdentify.Dequeue();
			this.SetInputDelay();
			if (this.joysticksToIdentify.Count == 0)
			{
				this.Reset();
			}
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x00108C78 File Offset: 0x00106E78
		private void DrawDialogWindow(int windowId)
		{
			if (!this.identifyRequired)
			{
				return;
			}
			if (this.style == null)
			{
				this.style = new GUIStyle(GUI.skin.label);
				this.style.wordWrap = true;
			}
			GUILayout.Space(15f);
			GUILayout.Label("A joystick has been attached or removed. You will need to identify each joystick by pressing a button on the controller listed below:", this.style, Array.Empty<GUILayoutOption>());
			Joystick joystick = this.joysticksToIdentify.Peek();
			GUILayout.Label("Press any button on \"" + joystick.name + "\" now.", this.style, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Skip", Array.Empty<GUILayoutOption>()))
			{
				this.joysticksToIdentify.Dequeue();
				return;
			}
		}

		// Token: 0x060032EE RID: 13038 RVA: 0x00022A55 File Offset: 0x00020C55
		private void Reset()
		{
			this.joysticksToIdentify = null;
			this.identifyRequired = false;
		}

		// Token: 0x04003133 RID: 12595
		private const float windowWidth = 250f;

		// Token: 0x04003134 RID: 12596
		private const float windowHeight = 250f;

		// Token: 0x04003135 RID: 12597
		private const float inputDelay = 1f;

		// Token: 0x04003136 RID: 12598
		private bool identifyRequired;

		// Token: 0x04003137 RID: 12599
		private Queue<Joystick> joysticksToIdentify;

		// Token: 0x04003138 RID: 12600
		private float nextInputAllowedTime;

		// Token: 0x04003139 RID: 12601
		private GUIStyle style;
	}
}
