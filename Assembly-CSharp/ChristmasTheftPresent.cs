using System;
using UnityEngine;

// Token: 0x02000197 RID: 407
public class ChristmasTheftPresent : MonoBehaviour
{
	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x0000B61B File Offset: 0x0000981B
	public ChristmasTheftPresentState State
	{
		get
		{
			return this.m_state;
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x0000B623 File Offset: 0x00009823
	public ChristmasTheftPlayer Player
	{
		get
		{
			return this.m_ownerPlayer;
		}
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x0000B62B File Offset: 0x0000982B
	// (set) Token: 0x06000BBA RID: 3002 RVA: 0x0000B633 File Offset: 0x00009833
	public byte ID { get; set; }

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06000BBB RID: 3003 RVA: 0x0000B63C File Offset: 0x0000983C
	// (set) Token: 0x06000BBC RID: 3004 RVA: 0x0000B644 File Offset: 0x00009844
	public int Slot { get; set; }

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06000BBD RID: 3005 RVA: 0x0000B64D File Offset: 0x0000984D
	// (set) Token: 0x06000BBE RID: 3006 RVA: 0x0000B655 File Offset: 0x00009855
	public float Distance { get; set; }

	// Token: 0x06000BBF RID: 3007 RVA: 0x00063AD8 File Offset: 0x00061CD8
	public void SetState(ChristmasTheftPresentState state, Vector3 position, ChristmasTheftPlayer player)
	{
		this.m_state = state;
		base.transform.position = position;
		this.m_ownerPlayer = player;
		switch (this.m_state)
		{
		case ChristmasTheftPresentState.Free:
			this.m_presentGraphic.SetActive(true);
			return;
		case ChristmasTheftPresentState.Held:
			this.m_presentGraphic.SetActive(false);
			return;
		case ChristmasTheftPresentState.Owned:
			this.m_presentGraphic.SetActive(true);
			return;
		default:
			return;
		}
	}

	// Token: 0x04000AE3 RID: 2787
	[SerializeField]
	private GameObject m_presentGraphic;

	// Token: 0x04000AE4 RID: 2788
	private ChristmasTheftPresentState m_state;

	// Token: 0x04000AE5 RID: 2789
	private ChristmasTheftPlayer m_ownerPlayer;
}
