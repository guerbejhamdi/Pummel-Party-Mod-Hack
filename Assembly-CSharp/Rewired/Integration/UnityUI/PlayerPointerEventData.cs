using System;
using System.Text;
using Rewired.UI;
using UnityEngine.EventSystems;

namespace Rewired.Integration.UnityUI
{
	// Token: 0x020006AD RID: 1709
	public class PlayerPointerEventData : PointerEventData
	{
		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06003175 RID: 12661 RVA: 0x00021929 File Offset: 0x0001FB29
		// (set) Token: 0x06003176 RID: 12662 RVA: 0x00021931 File Offset: 0x0001FB31
		public int playerId { get; set; }

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06003177 RID: 12663 RVA: 0x0002193A File Offset: 0x0001FB3A
		// (set) Token: 0x06003178 RID: 12664 RVA: 0x00021942 File Offset: 0x0001FB42
		public int inputSourceIndex { get; set; }

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06003179 RID: 12665 RVA: 0x0002194B File Offset: 0x0001FB4B
		// (set) Token: 0x0600317A RID: 12666 RVA: 0x00021953 File Offset: 0x0001FB53
		public IMouseInputSource mouseSource { get; set; }

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x0600317B RID: 12667 RVA: 0x0002195C File Offset: 0x0001FB5C
		// (set) Token: 0x0600317C RID: 12668 RVA: 0x00021964 File Offset: 0x0001FB64
		public ITouchInputSource touchSource { get; set; }

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x0600317D RID: 12669 RVA: 0x0002196D File Offset: 0x0001FB6D
		// (set) Token: 0x0600317E RID: 12670 RVA: 0x00021975 File Offset: 0x0001FB75
		public PointerEventType sourceType { get; set; }

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x0600317F RID: 12671 RVA: 0x0002197E File Offset: 0x0001FB7E
		// (set) Token: 0x06003180 RID: 12672 RVA: 0x00021986 File Offset: 0x0001FB86
		public int buttonIndex { get; set; }

		// Token: 0x06003181 RID: 12673 RVA: 0x0002198F File Offset: 0x0001FB8F
		public PlayerPointerEventData(EventSystem eventSystem) : base(eventSystem)
		{
			this.playerId = -1;
			this.inputSourceIndex = -1;
			this.buttonIndex = -1;
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x001033FC File Offset: 0x001015FC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<b>Player Id</b>: " + this.playerId.ToString());
			string str = "<b>Mouse Source</b>: ";
			IMouseInputSource mouseSource = this.mouseSource;
			stringBuilder.AppendLine(str + ((mouseSource != null) ? mouseSource.ToString() : null));
			stringBuilder.AppendLine("<b>Input Source Index</b>: " + this.inputSourceIndex.ToString());
			string str2 = "<b>Touch Source/b>: ";
			ITouchInputSource touchSource = this.touchSource;
			stringBuilder.AppendLine(str2 + ((touchSource != null) ? touchSource.ToString() : null));
			stringBuilder.AppendLine("<b>Source Type</b>: " + this.sourceType.ToString());
			stringBuilder.AppendLine("<b>Button Index</b>: " + this.buttonIndex.ToString());
			stringBuilder.Append(base.ToString());
			return stringBuilder.ToString();
		}
	}
}
