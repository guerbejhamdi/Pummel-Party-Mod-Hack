using System;
using UnityEngine;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x0200003A RID: 58
public class ConnectionSlotGroupController : MonoBehaviour
{
	// Token: 0x060000F8 RID: 248 RVA: 0x00004394 File Offset: 0x00002594
	public void OnEnable()
	{
		this.lastMaxPlayers = 0;
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00004394 File Offset: 0x00002594
	public void OnStart()
	{
		this.lastMaxPlayers = 0;
	}

	// Token: 0x060000FA RID: 250 RVA: 0x0002FC64 File Offset: 0x0002DE64
	private void Update()
	{
		int num = Mathf.Max(GameManager.LobbyMaxPlayers - 1, NetSystem.PlayerCount - 1);
		if (num != this.lastMaxPlayers && num >= 0 && num < this.m_connectionSlots.Length)
		{
			float cellSize = this.m_settings[num].cellSize;
			float spacing = this.m_settings[num].spacing;
			this.m_group.cellSize = new Vector2(cellSize, this.m_group.cellSize.y);
			this.m_group.spacing = new Vector2(spacing, this.m_group.spacing.y);
			for (int i = 0; i < this.m_connectionSlots.Length; i++)
			{
				this.m_connectionSlots[i].SetActive(i <= num);
			}
			this.lastMaxPlayers = num;
		}
	}

	// Token: 0x04000151 RID: 337
	public GridLayoutGroup m_group;

	// Token: 0x04000152 RID: 338
	public ConnectionSlotGridSettings[] m_settings;

	// Token: 0x04000153 RID: 339
	public GameObject[] m_connectionSlots;

	// Token: 0x04000154 RID: 340
	private int lastMaxPlayers;
}
