using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000683 RID: 1667
	public interface ICustomSelectable : ICancelHandler, IEventSystemHandler
	{
		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06002FDB RID: 12251
		// (set) Token: 0x06002FDC RID: 12252
		Sprite disabledHighlightedSprite { get; set; }

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06002FDD RID: 12253
		// (set) Token: 0x06002FDE RID: 12254
		Color disabledHighlightedColor { get; set; }

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06002FDF RID: 12255
		// (set) Token: 0x06002FE0 RID: 12256
		string disabledHighlightedTrigger { get; set; }

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06002FE1 RID: 12257
		// (set) Token: 0x06002FE2 RID: 12258
		bool autoNavUp { get; set; }

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06002FE3 RID: 12259
		// (set) Token: 0x06002FE4 RID: 12260
		bool autoNavDown { get; set; }

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06002FE5 RID: 12261
		// (set) Token: 0x06002FE6 RID: 12262
		bool autoNavLeft { get; set; }

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06002FE7 RID: 12263
		// (set) Token: 0x06002FE8 RID: 12264
		bool autoNavRight { get; set; }

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06002FE9 RID: 12265
		// (remove) Token: 0x06002FEA RID: 12266
		event UnityAction CancelEvent;
	}
}
