using System;
using System.Collections.Generic;
using MalbersAnimations.Events;
using UnityEngine;
using UnityEngine.Events;

namespace MalbersAnimations
{
	// Token: 0x02000729 RID: 1833
	public class MalbersInput : MonoBehaviour, IInputSource
	{
		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06003577 RID: 13687 RVA: 0x0002444F File Offset: 0x0002264F
		// (set) Token: 0x06003576 RID: 13686 RVA: 0x00024446 File Offset: 0x00022646
		public bool MoveCharacter { get; set; }

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06003578 RID: 13688 RVA: 0x00024457 File Offset: 0x00022657
		// (set) Token: 0x06003579 RID: 13689 RVA: 0x0002445F File Offset: 0x0002265F
		public bool CameraBaseInput
		{
			get
			{
				return this.cameraBaseInput;
			}
			set
			{
				this.cameraBaseInput = value;
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x0600357A RID: 13690 RVA: 0x00024468 File Offset: 0x00022668
		// (set) Token: 0x0600357B RID: 13691 RVA: 0x00024470 File Offset: 0x00022670
		public bool AlwaysForward
		{
			get
			{
				return this.alwaysForward;
			}
			set
			{
				this.alwaysForward = value;
			}
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x00114DEC File Offset: 0x00112FEC
		private void Awake()
		{
			this.Input_System = DefaultInput.GetInputSystem(this.PlayerID);
			this.Horizontal.InputSystem = (this.Vertical.InputSystem = this.Input_System);
			foreach (InputRow inputRow in this.inputs)
			{
				inputRow.InputSystem = this.Input_System;
			}
			this.List_to_Dictionary();
			this.InitializeCharacter();
			this.MoveCharacter = true;
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x00114E88 File Offset: 0x00113088
		private void InitializeCharacter()
		{
			this.mCharacter = base.GetComponent<IMCharacter>();
			this.mCharacterMove = base.GetComponent<ICharacterMove>();
			if (this.mCharacter != null)
			{
				Dictionary<string, BoolEvent> dictionary = new Dictionary<string, BoolEvent>();
				foreach (KeyValuePair<string, InputRow> keyValuePair in this.DInputs)
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value.OnInputChanged);
				}
				this.mCharacter.InitializeInputs(dictionary);
			}
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x00024479 File Offset: 0x00022679
		private void OnEnable()
		{
			this.OnInputEnabled.Invoke();
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x00024486 File Offset: 0x00022686
		public virtual void EnableMovement(bool value)
		{
			this.MoveCharacter = value;
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x0002448F File Offset: 0x0002268F
		private void OnDisable()
		{
			if (this.mCharacterMove != null)
			{
				this.mCharacterMove.Move(Vector3.zero, true);
			}
			this.OnInputDisabled.Invoke();
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x000244B5 File Offset: 0x000226B5
		private void Start()
		{
			if (Camera.main != null)
			{
				this.m_Cam = Camera.main.transform;
				return;
			}
			this.m_Cam = UnityEngine.Object.FindObjectOfType<Camera>().transform;
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x000244E5 File Offset: 0x000226E5
		private void Update()
		{
			this.SetInput();
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x00114F20 File Offset: 0x00113120
		protected virtual void SetInput()
		{
			this.h = this.Horizontal.GetAxis;
			this.v = (this.alwaysForward ? 1f : this.Vertical.GetAxis);
			this.CharacterMove();
			foreach (InputRow inputRow in this.inputs)
			{
				bool getInput = inputRow.GetInput;
			}
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x00114FA8 File Offset: 0x001131A8
		private void CharacterMove()
		{
			if (this.MoveCharacter && this.mCharacterMove != null)
			{
				if (this.cameraBaseInput)
				{
					this.mCharacterMove.Move(this.CameraInputBased(), true);
					return;
				}
				this.mCharacterMove.Move(new Vector3(this.h, 0f, this.v), false);
			}
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x00115004 File Offset: 0x00113204
		protected Vector3 CameraInputBased()
		{
			if (this.m_Cam != null)
			{
				this.m_CamForward = Vector3.Scale(this.m_Cam.forward, Vector3.one).normalized;
				this.m_Move = this.v * this.m_CamForward + this.h * this.m_Cam.right;
			}
			else
			{
				this.m_Move = this.v * Vector3.forward + this.h * Vector3.right;
			}
			return this.m_Move;
		}

		// Token: 0x06003586 RID: 13702 RVA: 0x001150A8 File Offset: 0x001132A8
		public virtual void EnableInput(string inputName, bool value)
		{
			InputRow inputRow = this.inputs.Find((InputRow item) => item.name == inputName);
			if (inputRow != null)
			{
				inputRow.active = value;
			}
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x001150E4 File Offset: 0x001132E4
		public virtual void EnableInput(string inputName)
		{
			InputRow inputRow = this.inputs.Find((InputRow item) => item.name == inputName);
			if (inputRow != null)
			{
				inputRow.active = true;
			}
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x00115120 File Offset: 0x00113320
		public virtual void DisableInput(string inputName)
		{
			InputRow inputRow = this.inputs.Find((InputRow item) => item.name == inputName);
			if (inputRow != null)
			{
				inputRow.active = false;
			}
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x0011515C File Offset: 0x0011335C
		public virtual bool IsActive(string name)
		{
			InputRow inputRow;
			return this.DInputs.TryGetValue(name, out inputRow) && inputRow.active;
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x00115184 File Offset: 0x00113384
		public virtual InputRow FindInput(string name)
		{
			InputRow inputRow = this.inputs.Find((InputRow item) => item.name.ToUpper() == name.ToUpper());
			if (inputRow != null)
			{
				return inputRow;
			}
			return null;
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x001151BC File Offset: 0x001133BC
		private void Reset()
		{
			this.inputs = new List<InputRow>
			{
				new InputRow("Jump", "Jump", KeyCode.Space, InputButton.Press, InputType.Input),
				new InputRow("Shift", "Fire3", KeyCode.LeftShift, InputButton.Press, InputType.Input),
				new InputRow("Attack1", "Fire1", KeyCode.Mouse0, InputButton.Press, InputType.Input),
				new InputRow("Attack2", "Fire2", KeyCode.Mouse1, InputButton.Press, InputType.Input),
				new InputRow(false, "SpeedDown", "SpeedDown", KeyCode.Alpha1, InputButton.Down, InputType.Key),
				new InputRow(false, "SpeedUp", "SpeedUp", KeyCode.Alpha2, InputButton.Down, InputType.Key),
				new InputRow("Speed1", "Speed1", KeyCode.Alpha1, InputButton.Down, InputType.Key),
				new InputRow("Speed2", "Speed2", KeyCode.Alpha2, InputButton.Down, InputType.Key),
				new InputRow("Speed3", "Speed3", KeyCode.Alpha3, InputButton.Down, InputType.Key),
				new InputRow("Action", "Action", KeyCode.E, InputButton.Down, InputType.Key),
				new InputRow("Fly", "Fly", KeyCode.Q, InputButton.Down, InputType.Key),
				new InputRow("Dodge", "Dodge", KeyCode.R, InputButton.Down, InputType.Key),
				new InputRow("Down", "Down", KeyCode.C, InputButton.Press, InputType.Key),
				new InputRow("Up", "Jump", KeyCode.Space, InputButton.Press, InputType.Input),
				new InputRow("Stun", "Stun", KeyCode.H, InputButton.Press, InputType.Key),
				new InputRow("Damaged", "Damaged", KeyCode.J, InputButton.Down, InputType.Key),
				new InputRow("Death", "Death", KeyCode.K, InputButton.Down, InputType.Key)
			};
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x00115388 File Offset: 0x00113588
		private void List_to_Dictionary()
		{
			this.DInputs = new Dictionary<string, InputRow>();
			foreach (InputRow inputRow in this.inputs)
			{
				this.DInputs.Add(inputRow.name, inputRow);
			}
		}

		// Token: 0x040034B8 RID: 13496
		private IMCharacter mCharacter;

		// Token: 0x040034B9 RID: 13497
		private ICharacterMove mCharacterMove;

		// Token: 0x040034BA RID: 13498
		private IInputSystem Input_System;

		// Token: 0x040034BB RID: 13499
		private Vector3 m_CamForward;

		// Token: 0x040034BC RID: 13500
		private Vector3 m_Move;

		// Token: 0x040034BD RID: 13501
		private Transform m_Cam;

		// Token: 0x040034BE RID: 13502
		public List<InputRow> inputs = new List<InputRow>();

		// Token: 0x040034BF RID: 13503
		protected Dictionary<string, InputRow> DInputs = new Dictionary<string, InputRow>();

		// Token: 0x040034C0 RID: 13504
		public InputAxis Horizontal = new InputAxis("Horizontal", true, true);

		// Token: 0x040034C1 RID: 13505
		public InputAxis Vertical = new InputAxis("Vertical", true, true);

		// Token: 0x040034C3 RID: 13507
		[SerializeField]
		private bool cameraBaseInput;

		// Token: 0x040034C4 RID: 13508
		[SerializeField]
		private bool alwaysForward;

		// Token: 0x040034C5 RID: 13509
		public bool showInputEvents;

		// Token: 0x040034C6 RID: 13510
		public UnityEvent OnInputEnabled = new UnityEvent();

		// Token: 0x040034C7 RID: 13511
		public UnityEvent OnInputDisabled = new UnityEvent();

		// Token: 0x040034C8 RID: 13512
		private float h;

		// Token: 0x040034C9 RID: 13513
		private float v;

		// Token: 0x040034CA RID: 13514
		public string PlayerID = "Player0";
	}
}
