using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020006C6 RID: 1734
	[AddComponentMenu("")]
	public class CustomControllerDemo : MonoBehaviour
	{
		// Token: 0x060032AF RID: 12975 RVA: 0x00022730 File Offset: 0x00020930
		private void Awake()
		{
			if (SystemInfo.deviceType == DeviceType.Handheld && Screen.orientation != ScreenOrientation.LandscapeLeft)
			{
				Screen.orientation = ScreenOrientation.LandscapeLeft;
			}
			this.Initialize();
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x00107EC4 File Offset: 0x001060C4
		private void Initialize()
		{
			ReInput.InputSourceUpdateEvent += this.OnInputSourceUpdate;
			this.joysticks = base.GetComponentsInChildren<TouchJoystickExample>();
			this.buttons = base.GetComponentsInChildren<TouchButtonExample>();
			this.axisCount = this.joysticks.Length * 2;
			this.buttonCount = this.buttons.Length;
			this.axisValues = new float[this.axisCount];
			this.buttonValues = new bool[this.buttonCount];
			Player player = ReInput.players.GetPlayer(this.playerId);
			this.controller = player.controllers.GetControllerWithTag<CustomController>(this.controllerTag);
			if (this.controller == null)
			{
				Debug.LogError("A matching controller was not found for tag \"" + this.controllerTag + "\"");
			}
			if (this.controller.buttonCount != this.buttonValues.Length || this.controller.axisCount != this.axisValues.Length)
			{
				Debug.LogError("Controller has wrong number of elements!");
			}
			if (this.useUpdateCallbacks && this.controller != null)
			{
				this.controller.SetAxisUpdateCallback(new Func<int, float>(this.GetAxisValueCallback));
				this.controller.SetButtonUpdateCallback(new Func<int, bool>(this.GetButtonValueCallback));
			}
			this.initialized = true;
		}

		// Token: 0x060032B1 RID: 12977 RVA: 0x0002274E File Offset: 0x0002094E
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (!this.initialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x00022766 File Offset: 0x00020966
		private void OnInputSourceUpdate()
		{
			this.GetSourceAxisValues();
			this.GetSourceButtonValues();
			if (!this.useUpdateCallbacks)
			{
				this.SetControllerAxisValues();
				this.SetControllerButtonValues();
			}
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x00108000 File Offset: 0x00106200
		private void GetSourceAxisValues()
		{
			for (int i = 0; i < this.axisValues.Length; i++)
			{
				if (i % 2 != 0)
				{
					this.axisValues[i] = this.joysticks[i / 2].position.y;
				}
				else
				{
					this.axisValues[i] = this.joysticks[i / 2].position.x;
				}
			}
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x00108060 File Offset: 0x00106260
		private void GetSourceButtonValues()
		{
			for (int i = 0; i < this.buttonValues.Length; i++)
			{
				this.buttonValues[i] = this.buttons[i].isPressed;
			}
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x00108098 File Offset: 0x00106298
		private void SetControllerAxisValues()
		{
			for (int i = 0; i < this.axisValues.Length; i++)
			{
				this.controller.SetAxisValue(i, this.axisValues[i]);
			}
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x001080CC File Offset: 0x001062CC
		private void SetControllerButtonValues()
		{
			for (int i = 0; i < this.buttonValues.Length; i++)
			{
				this.controller.SetButtonValue(i, this.buttonValues[i]);
			}
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x00022788 File Offset: 0x00020988
		private float GetAxisValueCallback(int index)
		{
			if (index >= this.axisValues.Length)
			{
				return 0f;
			}
			return this.axisValues[index];
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x000227A3 File Offset: 0x000209A3
		private bool GetButtonValueCallback(int index)
		{
			return index < this.buttonValues.Length && this.buttonValues[index];
		}

		// Token: 0x04003102 RID: 12546
		public int playerId;

		// Token: 0x04003103 RID: 12547
		public string controllerTag;

		// Token: 0x04003104 RID: 12548
		public bool useUpdateCallbacks;

		// Token: 0x04003105 RID: 12549
		private int buttonCount;

		// Token: 0x04003106 RID: 12550
		private int axisCount;

		// Token: 0x04003107 RID: 12551
		private float[] axisValues;

		// Token: 0x04003108 RID: 12552
		private bool[] buttonValues;

		// Token: 0x04003109 RID: 12553
		private TouchJoystickExample[] joysticks;

		// Token: 0x0400310A RID: 12554
		private TouchButtonExample[] buttons;

		// Token: 0x0400310B RID: 12555
		private CustomController controller;

		// Token: 0x0400310C RID: 12556
		[NonSerialized]
		private bool initialized;
	}
}
