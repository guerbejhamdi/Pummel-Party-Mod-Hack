using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200068A RID: 1674
	[AddComponentMenu("")]
	public class InputRow : MonoBehaviour
	{
		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06003011 RID: 12305 RVA: 0x00020C36 File Offset: 0x0001EE36
		// (set) Token: 0x06003012 RID: 12306 RVA: 0x00020C3E File Offset: 0x0001EE3E
		public ButtonInfo[] buttons { get; private set; }

		// Token: 0x06003013 RID: 12307 RVA: 0x00020C47 File Offset: 0x0001EE47
		public void Initialize(int rowIndex, string label, Action<int, ButtonInfo> inputFieldActivatedCallback)
		{
			this.rowIndex = rowIndex;
			this.label.text = label;
			this.inputFieldActivatedCallback = inputFieldActivatedCallback;
			this.buttons = base.transform.GetComponentsInChildren<ButtonInfo>(true);
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x00020C75 File Offset: 0x0001EE75
		public void OnButtonActivated(ButtonInfo buttonInfo)
		{
			if (this.inputFieldActivatedCallback == null)
			{
				return;
			}
			this.inputFieldActivatedCallback(this.rowIndex, buttonInfo);
		}

		// Token: 0x04002FA2 RID: 12194
		public Text label;

		// Token: 0x04002FA4 RID: 12196
		private int rowIndex;

		// Token: 0x04002FA5 RID: 12197
		private Action<int, ButtonInfo> inputFieldActivatedCallback;
	}
}
