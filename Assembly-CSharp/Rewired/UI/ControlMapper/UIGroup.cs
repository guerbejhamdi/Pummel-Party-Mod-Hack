using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020006A3 RID: 1699
	[AddComponentMenu("")]
	public class UIGroup : MonoBehaviour
	{
		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x0600311C RID: 12572 RVA: 0x0002149E File Offset: 0x0001F69E
		// (set) Token: 0x0600311D RID: 12573 RVA: 0x000214BF File Offset: 0x0001F6BF
		public string labelText
		{
			get
			{
				if (!(this._label != null))
				{
					return string.Empty;
				}
				return this._label.text;
			}
			set
			{
				if (this._label == null)
				{
					return;
				}
				this._label.text = value;
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x0600311E RID: 12574 RVA: 0x000214DC File Offset: 0x0001F6DC
		public Transform content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x000214E4 File Offset: 0x0001F6E4
		public void SetLabelActive(bool state)
		{
			if (this._label == null)
			{
				return;
			}
			this._label.gameObject.SetActive(state);
		}

		// Token: 0x04003043 RID: 12355
		[SerializeField]
		private Text _label;

		// Token: 0x04003044 RID: 12356
		[SerializeField]
		private Transform _content;
	}
}
