using System;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x0200069C RID: 1692
	[AddComponentMenu("")]
	public class ThemedElement : MonoBehaviour
	{
		// Token: 0x06003103 RID: 12547 RVA: 0x000213A5 File Offset: 0x0001F5A5
		private void Start()
		{
			ControlMapper.ApplyTheme(this._elements);
		}

		// Token: 0x04003033 RID: 12339
		[SerializeField]
		private ThemedElement.ElementInfo[] _elements;

		// Token: 0x0200069D RID: 1693
		[Serializable]
		public class ElementInfo
		{
			// Token: 0x1700089D RID: 2205
			// (get) Token: 0x06003105 RID: 12549 RVA: 0x000213B2 File Offset: 0x0001F5B2
			public string themeClass
			{
				get
				{
					return this._themeClass;
				}
			}

			// Token: 0x1700089E RID: 2206
			// (get) Token: 0x06003106 RID: 12550 RVA: 0x000213BA File Offset: 0x0001F5BA
			public Component component
			{
				get
				{
					return this._component;
				}
			}

			// Token: 0x04003034 RID: 12340
			[SerializeField]
			private string _themeClass;

			// Token: 0x04003035 RID: 12341
			[SerializeField]
			private Component _component;
		}
	}
}
