using System;
using MalbersAnimations.Events;

namespace MalbersAnimations
{
	// Token: 0x0200072F RID: 1839
	[Serializable]
	public class InputAxis
	{
		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x0600359F RID: 13727 RVA: 0x00115C14 File Offset: 0x00113E14
		public float GetAxis
		{
			get
			{
				if (this.inputSystem == null || !this.active)
				{
					return 0f;
				}
				this.currentAxisValue = (this.raw ? this.inputSystem.GetAxisRaw(this.input) : this.inputSystem.GetAxis(this.input));
				return this.currentAxisValue;
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x060035A0 RID: 13728 RVA: 0x00024554 File Offset: 0x00022754
		// (set) Token: 0x060035A1 RID: 13729 RVA: 0x0002455C File Offset: 0x0002275C
		public IInputSystem InputSystem
		{
			get
			{
				return this.inputSystem;
			}
			set
			{
				this.inputSystem = value;
			}
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x00115C70 File Offset: 0x00113E70
		public InputAxis()
		{
			this.active = true;
			this.raw = true;
			this.input = "Value";
			this.name = "NewAxis";
			this.inputSystem = new DefaultInput();
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x00115CEC File Offset: 0x00113EEC
		public InputAxis(string value)
		{
			this.active = true;
			this.raw = false;
			this.input = value;
			this.name = "NewAxis";
			this.inputSystem = new DefaultInput();
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x00115D64 File Offset: 0x00113F64
		public InputAxis(string InputValue, bool active, bool isRaw)
		{
			this.active = active;
			this.raw = isRaw;
			this.input = InputValue;
			this.name = "NewAxis";
			this.inputSystem = new DefaultInput();
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x00115DDC File Offset: 0x00113FDC
		public InputAxis(string name, string InputValue, bool active, bool raw)
		{
			this.active = active;
			this.raw = raw;
			this.input = InputValue;
			this.name = name;
			this.inputSystem = new DefaultInput();
		}

		// Token: 0x040034E4 RID: 13540
		public bool active = true;

		// Token: 0x040034E5 RID: 13541
		public string name = "NewAxis";

		// Token: 0x040034E6 RID: 13542
		public bool raw = true;

		// Token: 0x040034E7 RID: 13543
		public string input = "Value";

		// Token: 0x040034E8 RID: 13544
		private IInputSystem inputSystem = new DefaultInput();

		// Token: 0x040034E9 RID: 13545
		public FloatEvent OnAxisValueChanged = new FloatEvent();

		// Token: 0x040034EA RID: 13546
		private float currentAxisValue;
	}
}
