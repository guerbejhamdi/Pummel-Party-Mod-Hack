using System;
using MalbersAnimations.Events;
using UnityEngine;
using UnityEngine.Events;

namespace MalbersAnimations
{
	// Token: 0x0200072E RID: 1838
	[Serializable]
	public class InputRow
	{
		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06003596 RID: 13718 RVA: 0x00115464 File Offset: 0x00113664
		public virtual bool GetInput
		{
			get
			{
				if (!this.active)
				{
					return false;
				}
				if (this.inputSystem == null)
				{
					return false;
				}
				bool inputValue = this.InputValue;
				switch (this.GetPressed)
				{
				case InputButton.Press:
					this.InputValue = ((this.type == InputType.Input) ? this.InputSystem.GetButton(this.input) : Input.GetKey(this.key));
					if (inputValue != this.InputValue)
					{
						if (this.InputValue)
						{
							this.OnInputDown.Invoke();
						}
						else
						{
							this.OnInputUp.Invoke();
						}
						this.OnInputChanged.Invoke(this.InputValue);
					}
					if (this.InputValue)
					{
						this.OnInputPressed.Invoke();
					}
					return this.InputValue;
				case InputButton.Down:
					this.InputValue = ((this.type == InputType.Input) ? this.InputSystem.GetButtonDown(this.input) : Input.GetKeyDown(this.key));
					if (inputValue != this.InputValue)
					{
						if (this.InputValue)
						{
							this.OnInputDown.Invoke();
						}
						this.OnInputChanged.Invoke(this.InputValue);
					}
					return this.InputValue;
				case InputButton.Up:
					this.InputValue = ((this.type == InputType.Input) ? this.InputSystem.GetButtonUp(this.input) : Input.GetKeyUp(this.key));
					if (inputValue != this.InputValue)
					{
						if (this.InputValue)
						{
							this.OnInputUp.Invoke();
						}
						this.OnInputChanged.Invoke(this.InputValue);
					}
					return this.InputValue;
				case InputButton.LongPress:
					this.InputValue = ((this.type == InputType.Input) ? this.InputSystem.GetButton(this.input) : Input.GetKey(this.key));
					if (this.InputValue)
					{
						if (!this.InputCompleted)
						{
							if (!this.FirstInputPress)
							{
								this.InputCurrentTime = Time.time;
								this.FirstInputPress = true;
								this.OnInputDown.Invoke();
							}
							else
							{
								if (Time.time - this.InputCurrentTime >= this.LongPressTime)
								{
									this.OnLongPress.Invoke();
									this.OnPressedNormalized.Invoke(1f);
									this.InputCompleted = true;
									return this.InputValue = true;
								}
								this.OnPressedNormalized.Invoke((Time.time - this.InputCurrentTime) / this.LongPressTime);
							}
						}
					}
					else
					{
						if (!this.InputCompleted && this.FirstInputPress)
						{
							this.OnInputUp.Invoke();
						}
						this.FirstInputPress = (this.InputCompleted = false);
					}
					return this.InputValue = false;
				case InputButton.DoubleTap:
					this.InputValue = ((this.type == InputType.Input) ? this.InputSystem.GetButtonDown(this.input) : Input.GetKeyDown(this.key));
					if (this.InputValue)
					{
						if (this.InputCurrentTime != 0f && Time.time - this.InputCurrentTime > this.DoubleTapTime)
						{
							this.FirstInputPress = false;
						}
						if (!this.FirstInputPress)
						{
							this.OnInputDown.Invoke();
							this.InputCurrentTime = Time.time;
							this.FirstInputPress = true;
						}
						else
						{
							if (Time.time - this.InputCurrentTime <= this.DoubleTapTime)
							{
								this.FirstInputPress = false;
								this.InputCurrentTime = 0f;
								this.OnDoubleTap.Invoke();
								return this.InputValue = true;
							}
							this.FirstInputPress = false;
						}
					}
					return this.InputValue = false;
				default:
					return false;
				}
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06003597 RID: 13719 RVA: 0x00024543 File Offset: 0x00022743
		// (set) Token: 0x06003598 RID: 13720 RVA: 0x0002454B File Offset: 0x0002274B
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

		// Token: 0x06003599 RID: 13721 RVA: 0x001157CC File Offset: 0x001139CC
		public InputRow(KeyCode k)
		{
			this.active = true;
			this.type = InputType.Key;
			this.key = k;
			this.GetPressed = InputButton.Down;
			this.inputSystem = new DefaultInput();
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x0011589C File Offset: 0x00113A9C
		public InputRow(string input, KeyCode key)
		{
			this.active = true;
			this.type = InputType.Key;
			this.key = key;
			this.input = input;
			this.GetPressed = InputButton.Down;
			this.inputSystem = new DefaultInput();
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x0011589C File Offset: 0x00113A9C
		public InputRow(string unityInput, KeyCode k, InputButton pressed)
		{
			this.active = true;
			this.type = InputType.Key;
			this.key = k;
			this.input = unityInput;
			this.GetPressed = InputButton.Down;
			this.inputSystem = new DefaultInput();
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x00115970 File Offset: 0x00113B70
		public InputRow(string name, string unityInput, KeyCode k, InputButton pressed, InputType itype)
		{
			this.name = name;
			this.active = true;
			this.type = itype;
			this.key = k;
			this.input = unityInput;
			this.GetPressed = pressed;
			this.inputSystem = new DefaultInput();
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x00115A50 File Offset: 0x00113C50
		public InputRow(bool active, string name, string unityInput, KeyCode k, InputButton pressed, InputType itype)
		{
			this.name = name;
			this.active = active;
			this.type = itype;
			this.key = k;
			this.input = unityInput;
			this.GetPressed = pressed;
			this.inputSystem = new DefaultInput();
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x00115B30 File Offset: 0x00113D30
		public InputRow()
		{
			this.active = true;
			this.name = "InputName";
			this.type = InputType.Input;
			this.input = "Value";
			this.key = KeyCode.A;
			this.GetPressed = InputButton.Press;
			this.inputSystem = new DefaultInput();
		}

		// Token: 0x040034CF RID: 13519
		public bool active = true;

		// Token: 0x040034D0 RID: 13520
		public string name = "InputName";

		// Token: 0x040034D1 RID: 13521
		public InputType type;

		// Token: 0x040034D2 RID: 13522
		public string input = "Value";

		// Token: 0x040034D3 RID: 13523
		public KeyCode key = KeyCode.A;

		// Token: 0x040034D4 RID: 13524
		public InputButton GetPressed;

		// Token: 0x040034D5 RID: 13525
		public bool InputValue;

		// Token: 0x040034D6 RID: 13526
		public UnityEvent OnInputDown = new UnityEvent();

		// Token: 0x040034D7 RID: 13527
		public UnityEvent OnInputUp = new UnityEvent();

		// Token: 0x040034D8 RID: 13528
		public UnityEvent OnLongPress = new UnityEvent();

		// Token: 0x040034D9 RID: 13529
		public UnityEvent OnDoubleTap = new UnityEvent();

		// Token: 0x040034DA RID: 13530
		public BoolEvent OnInputChanged = new BoolEvent();

		// Token: 0x040034DB RID: 13531
		protected IInputSystem inputSystem = new DefaultInput();

		// Token: 0x040034DC RID: 13532
		public bool ShowEvents;

		// Token: 0x040034DD RID: 13533
		public float DoubleTapTime = 0.3f;

		// Token: 0x040034DE RID: 13534
		public float LongPressTime = 0.5f;

		// Token: 0x040034DF RID: 13535
		private bool FirstInputPress;

		// Token: 0x040034E0 RID: 13536
		private bool InputCompleted;

		// Token: 0x040034E1 RID: 13537
		private float InputCurrentTime;

		// Token: 0x040034E2 RID: 13538
		public UnityEvent OnInputPressed = new UnityEvent();

		// Token: 0x040034E3 RID: 13539
		public FloatEvent OnPressedNormalized = new FloatEvent();
	}
}
