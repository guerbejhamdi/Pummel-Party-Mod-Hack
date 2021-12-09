using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200054E RID: 1358
public class UIMapView : MonoBehaviour
{
	// Token: 0x060023D4 RID: 9172 RVA: 0x00019D56 File Offset: 0x00017F56
	public void SetTitle(string text)
	{
		this.map_view_title.text = text;
	}

	// Token: 0x060023D5 RID: 9173 RVA: 0x0001210A File Offset: 0x0001030A
	public void ShowWindow()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x060023D6 RID: 9174 RVA: 0x00012118 File Offset: 0x00010318
	public void HideWindow()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x040026CE RID: 9934
	public Text map_view_title;
}
