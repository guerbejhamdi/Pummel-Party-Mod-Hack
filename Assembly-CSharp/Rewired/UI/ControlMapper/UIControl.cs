using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200069F RID: 1695
	[AddComponentMenu("")]
	public class UIControl : MonoBehaviour
	{
		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06003109 RID: 12553 RVA: 0x000213CA File Offset: 0x0001F5CA
		public int id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x000213D2 File Offset: 0x0001F5D2
		private void Awake()
		{
			this._id = UIControl.GetNextUid();
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x0600310B RID: 12555 RVA: 0x000213DF File Offset: 0x0001F5DF
		// (set) Token: 0x0600310C RID: 12556 RVA: 0x000213E7 File Offset: 0x0001F5E7
		public bool showTitle
		{
			get
			{
				return this._showTitle;
			}
			set
			{
				if (this.title == null)
				{
					return;
				}
				this.title.gameObject.SetActive(value);
				this._showTitle = value;
			}
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void SetCancelCallback(Action cancelCallback)
		{
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x00021410 File Offset: 0x0001F610
		private static int GetNextUid()
		{
			if (UIControl._uidCounter == 2147483647)
			{
				UIControl._uidCounter = 0;
			}
			int uidCounter = UIControl._uidCounter;
			UIControl._uidCounter++;
			return uidCounter;
		}

		// Token: 0x04003036 RID: 12342
		public Text title;

		// Token: 0x04003037 RID: 12343
		private int _id;

		// Token: 0x04003038 RID: 12344
		private bool _showTitle;

		// Token: 0x04003039 RID: 12345
		private static int _uidCounter;
	}
}
