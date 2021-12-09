using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020002BA RID: 698
[RequireComponent(typeof(TextMeshProUGUI))]
public class OpenHyperlinks : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06001424 RID: 5156 RVA: 0x00097C88 File Offset: 0x00095E88
	public void OnPointerClick(PointerEventData eventData)
	{
		int num = TMP_TextUtilities.FindIntersectingLink(this.pTextMeshPro, Input.mousePosition, null);
		if (num != -1)
		{
			TMP_LinkInfo tmp_LinkInfo = this.pTextMeshPro.textInfo.linkInfo[num];
			Application.OpenURL(tmp_LinkInfo.GetLinkID());
		}
	}

	// Token: 0x06001425 RID: 5157 RVA: 0x0000398C File Offset: 0x00001B8C
	private void LateUpdate()
	{
	}

	// Token: 0x0400155F RID: 5471
	public TextMeshProUGUI pTextMeshPro;

	// Token: 0x04001560 RID: 5472
	public Color mouseOverColor;
}
