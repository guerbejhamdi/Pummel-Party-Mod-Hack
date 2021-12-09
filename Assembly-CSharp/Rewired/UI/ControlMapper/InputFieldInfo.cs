using System;
using UnityEngine;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000689 RID: 1673
	[AddComponentMenu("")]
	public class InputFieldInfo : UIElementInfo
	{
		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06003006 RID: 12294 RVA: 0x00020BE1 File Offset: 0x0001EDE1
		// (set) Token: 0x06003007 RID: 12295 RVA: 0x00020BE9 File Offset: 0x0001EDE9
		public int actionId { get; set; }

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06003008 RID: 12296 RVA: 0x00020BF2 File Offset: 0x0001EDF2
		// (set) Token: 0x06003009 RID: 12297 RVA: 0x00020BFA File Offset: 0x0001EDFA
		public AxisRange axisRange { get; set; }

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x0600300A RID: 12298 RVA: 0x00020C03 File Offset: 0x0001EE03
		// (set) Token: 0x0600300B RID: 12299 RVA: 0x00020C0B File Offset: 0x0001EE0B
		public int actionElementMapId { get; set; }

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x0600300C RID: 12300 RVA: 0x00020C14 File Offset: 0x0001EE14
		// (set) Token: 0x0600300D RID: 12301 RVA: 0x00020C1C File Offset: 0x0001EE1C
		public ControllerType controllerType { get; set; }

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x0600300E RID: 12302 RVA: 0x00020C25 File Offset: 0x0001EE25
		// (set) Token: 0x0600300F RID: 12303 RVA: 0x00020C2D File Offset: 0x0001EE2D
		public int controllerId { get; set; }
	}
}
