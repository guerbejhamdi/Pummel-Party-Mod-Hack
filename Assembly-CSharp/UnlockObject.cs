using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200056C RID: 1388
public class UnlockObject : MonoBehaviour
{
	// Token: 0x0600247A RID: 9338 RVA: 0x000DB214 File Offset: 0x000D9414
	public void Setup(string nameToken, int cost, int unlockID, Rect iconRect, UnlockWindowManager manager)
	{
		this.nameText.text = LocalizationManager.GetTranslation(nameToken, true, 0, true, false, null, null, true);
		this.costText.text = cost.ToString();
		this.rawImage.uvRect = iconRect;
		this.manager = manager;
		this.unlockID = unlockID;
		this.cost = cost;
	}

	// Token: 0x0600247B RID: 9339 RVA: 0x000DB270 File Offset: 0x000D9470
	public void OnSubmit()
	{
		if (this.unlockButton.CurState == BasicButtonBase.BasicButtonState.Disabled)
		{
			return;
		}
		if (GameManager.TrophyCount >= this.cost)
		{
			GameManager.TrophyCount -= this.cost;
			GameManager.SaveUnlocks();
			this.manager.OnUnlock(this.unlockID);
		}
	}

	// Token: 0x0600247C RID: 9340 RVA: 0x0001A366 File Offset: 0x00018566
	public void UpdateButton(int currentTrophyCount)
	{
		this.unlockButton.SetState((currentTrophyCount < this.cost || GameManager.unlocked[this.unlockID]) ? BasicButtonBase.BasicButtonState.Disabled : BasicButtonBase.BasicButtonState.Enabled);
	}

	// Token: 0x040027C0 RID: 10176
	public RawImage rawImage;

	// Token: 0x040027C1 RID: 10177
	public Text nameText;

	// Token: 0x040027C2 RID: 10178
	public Text costText;

	// Token: 0x040027C3 RID: 10179
	public BasicButtonBase unlockButton;

	// Token: 0x040027C4 RID: 10180
	private UnlockWindowManager manager;

	// Token: 0x040027C5 RID: 10181
	private int unlockID;

	// Token: 0x040027C6 RID: 10182
	private int cost;
}
