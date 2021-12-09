using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004BE RID: 1214
public class StatUIRow : MonoBehaviour
{
	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x0600203C RID: 8252 RVA: 0x000178A8 File Offset: 0x00015AA8
	public int CellCount
	{
		get
		{
			return this.m_cells.Count;
		}
	}

	// Token: 0x0600203D RID: 8253 RVA: 0x000CA454 File Offset: 0x000C8654
	public void Setup(int numColumns, bool isHeader, bool isAlt)
	{
		this.m_cellPfb.SetActive(false);
		for (int i = 0; i < numColumns; i++)
		{
			GameObject obj = UnityEngine.Object.Instantiate<GameObject>(this.m_cellPfb, this.m_cellParent);
			this.SetupCell(obj, isHeader, isAlt, isHeader ? i : 0);
		}
	}

	// Token: 0x0600203E RID: 8254 RVA: 0x000178B5 File Offset: 0x00015AB5
	public StatUICell GetCell(int index)
	{
		return this.m_cells[index];
	}

	// Token: 0x0600203F RID: 8255 RVA: 0x000CA49C File Offset: 0x000C869C
	private void SetupCell(GameObject obj, bool isHeader, bool isAlt, int buttonAction)
	{
		StatUICell component = obj.GetComponent<StatUICell>();
		if (component != null)
		{
			component.Setup(isHeader, isAlt, buttonAction);
			component.gameObject.SetActive(true);
			this.m_cells.Add(component);
		}
	}

	// Token: 0x0400230A RID: 8970
	[SerializeField]
	private RectTransform m_cellParent;

	// Token: 0x0400230B RID: 8971
	[Header("Prefabs")]
	[SerializeField]
	private GameObject m_cellPfb;

	// Token: 0x0400230C RID: 8972
	private List<StatUICell> m_cells = new List<StatUICell>();
}
