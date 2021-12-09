using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200056A RID: 1386
public class UITurnOrder : MonoBehaviour
{
	// Token: 0x06002471 RID: 9329 RVA: 0x000DAECC File Offset: 0x000D90CC
	public void UpdateOrder(List<BoardPlayer> players)
	{
		for (int i = 0; i < 8; i++)
		{
			if (i >= players.Count)
			{
				this.images[i].gameObject.SetActive(false);
			}
			else
			{
				this.images[i].gameObject.SetActive(true);
				this.images[i].color = players[i].GamePlayer.Color.skinColor1;
			}
		}
	}

	// Token: 0x040027B1 RID: 10161
	public Image[] images;
}
