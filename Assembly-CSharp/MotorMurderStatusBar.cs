using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E2 RID: 482
public class MotorMurderStatusBar : MonoBehaviour
{
	// Token: 0x06000E10 RID: 3600 RVA: 0x000714E0 File Offset: 0x0006F6E0
	private void Update()
	{
		if (this.player.IsDead)
		{
			return;
		}
		base.transform.position = this.minigameController.minigameCameras[0].WorldToScreenPoint(this.player.bodyRenderer.transform.position + this.worldOffset) + this.screenOffset;
		this.healthFill.fillAmount = this.player.Health / this.player.maxhealth;
		this.heatFill.fillAmount = this.player.Heat / 255f;
	}

	// Token: 0x04000D7E RID: 3454
	public Image healthFill;

	// Token: 0x04000D7F RID: 3455
	public Image heatFill;

	// Token: 0x04000D80 RID: 3456
	public MinigameController minigameController;

	// Token: 0x04000D81 RID: 3457
	public MotorMurderPlayer player;

	// Token: 0x04000D82 RID: 3458
	public Vector3 screenOffset = new Vector3(0f, 30f, 0f);

	// Token: 0x04000D83 RID: 3459
	public Vector3 worldOffset = new Vector3(0f, 0f, 0f);
}
