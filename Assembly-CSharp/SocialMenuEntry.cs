using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020004AE RID: 1198
public class SocialMenuEntry : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
	// Token: 0x170003CC RID: 972
	// (get) Token: 0x06001FF6 RID: 8182 RVA: 0x000175F5 File Offset: 0x000157F5
	// (set) Token: 0x06001FF7 RID: 8183 RVA: 0x000175FD File Offset: 0x000157FD
	public bool IsSelected { get; set; }

	// Token: 0x06001FF8 RID: 8184 RVA: 0x00017606 File Offset: 0x00015806
	public void OnSelect(BaseEventData eventData)
	{
		this.IsSelected = true;
	}

	// Token: 0x06001FF9 RID: 8185 RVA: 0x0001760F File Offset: 0x0001580F
	public void OnDeselect(BaseEventData eventData)
	{
		this.IsSelected = false;
	}

	// Token: 0x040022CD RID: 8909
	public Image profilePicture;

	// Token: 0x040022CE RID: 8910
	public Text profileName;

	// Token: 0x040022CF RID: 8911
	public Image mutedIcon;

	// Token: 0x040022D0 RID: 8912
	public Button button;
}
