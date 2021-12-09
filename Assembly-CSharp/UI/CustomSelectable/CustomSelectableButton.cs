using System;
using UnityEngine.UI;

namespace UI.CustomSelectable
{
	// Token: 0x020005A3 RID: 1443
	public class CustomSelectableButton : Button
	{
		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06002584 RID: 9604 RVA: 0x0001AD41 File Offset: 0x00018F41
		// (set) Token: 0x06002585 RID: 9605 RVA: 0x0001AD49 File Offset: 0x00018F49
		public Selectable upSelectable { get; set; }

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06002586 RID: 9606 RVA: 0x0001AD52 File Offset: 0x00018F52
		// (set) Token: 0x06002587 RID: 9607 RVA: 0x0001AD5A File Offset: 0x00018F5A
		public Selectable downSelectable { get; set; }

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06002588 RID: 9608 RVA: 0x0001AD63 File Offset: 0x00018F63
		// (set) Token: 0x06002589 RID: 9609 RVA: 0x0001AD6B File Offset: 0x00018F6B
		public Selectable leftSelectable { get; set; }

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x0600258A RID: 9610 RVA: 0x0001AD74 File Offset: 0x00018F74
		// (set) Token: 0x0600258B RID: 9611 RVA: 0x0001AD7C File Offset: 0x00018F7C
		public Selectable rightSelectable { get; set; }

		// Token: 0x0600258C RID: 9612 RVA: 0x0001AD85 File Offset: 0x00018F85
		public override Selectable FindSelectableOnUp()
		{
			if (!(this.upSelectable != null))
			{
				return base.FindSelectableOnUp();
			}
			return this.upSelectable;
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x0001ADA2 File Offset: 0x00018FA2
		public override Selectable FindSelectableOnDown()
		{
			if (!(this.downSelectable != null))
			{
				return base.FindSelectableOnDown();
			}
			return this.downSelectable;
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x0001ADBF File Offset: 0x00018FBF
		public override Selectable FindSelectableOnLeft()
		{
			if (!(this.leftSelectable != null))
			{
				return base.FindSelectableOnLeft();
			}
			return this.leftSelectable;
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x0001ADDC File Offset: 0x00018FDC
		public override Selectable FindSelectableOnRight()
		{
			if (!(this.rightSelectable != null))
			{
				return base.FindSelectableOnRight();
			}
			return this.rightSelectable;
		}
	}
}
