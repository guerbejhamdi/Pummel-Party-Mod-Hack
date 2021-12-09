using System;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class EventMaterialSwapper : MonoBehaviour
{
	// Token: 0x06000357 RID: 855 RVA: 0x00038DF4 File Offset: 0x00036FF4
	public void Awake()
	{
		GameEventTheme currentEventTheme = GameManager.GetCurrentEventTheme();
		if (currentEventTheme == GameEventTheme.None)
		{
			return;
		}
		foreach (SwapMaterialInfo swapMaterialInfo in this.m_swapMaterials)
		{
			if (swapMaterialInfo.theme == currentEventTheme)
			{
				base.GetComponent<Renderer>().sharedMaterial = swapMaterialInfo.material;
				return;
			}
		}
	}

	// Token: 0x04000368 RID: 872
	[SerializeField]
	private SwapMaterialInfo[] m_swapMaterials;
}
