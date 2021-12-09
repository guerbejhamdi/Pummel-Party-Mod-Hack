using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000150 RID: 336
public class BilliardBattleBar : MonoBehaviour
{
	// Token: 0x060009A1 RID: 2465 RVA: 0x000564E0 File Offset: 0x000546E0
	private void Update()
	{
		if (this.player.IsDead)
		{
			return;
		}
		base.transform.position = this.minigameController.minigameCameras[0].WorldToScreenPoint(this.player.transform.position + this.worldOffset) + this.screenOffset;
		this.chargeFill.fillAmount = this.player.HoldTime;
	}

	// Token: 0x04000855 RID: 2133
	public BilliardBattlePlayer player;

	// Token: 0x04000856 RID: 2134
	public Image chargeFill;

	// Token: 0x04000857 RID: 2135
	public MinigameController minigameController;

	// Token: 0x04000858 RID: 2136
	public Vector3 screenOffset = new Vector3(0f, 0f, 0f);

	// Token: 0x04000859 RID: 2137
	public Vector3 worldOffset = new Vector3(0f, 0f, 0f);
}
