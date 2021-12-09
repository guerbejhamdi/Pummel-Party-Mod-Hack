using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000569 RID: 1385
public class UITurnCounter : MonoBehaviour
{
	// Token: 0x0600246E RID: 9326 RVA: 0x0001A2E1 File Offset: 0x000184E1
	private void Update()
	{
		this.UpdateText();
	}

	// Token: 0x0600246F RID: 9327 RVA: 0x000DAE10 File Offset: 0x000D9010
	private void UpdateText()
	{
		if (GameManager.Board != null && GameManager.Board.CurnTurnNum != this.lastTurnNumber)
		{
			this.text.text = GameManager.Board.CurnTurnNum.ToString() + "/" + ((GameManager.TurnCount == 9999) ? "∞" : GameManager.TurnCount.ToString());
			if (GameManager.Board.CurnTurnNum == GameManager.TurnCount)
			{
				this.textLocalize.SetTerm("Final Turn");
			}
			else
			{
				this.textLocalize.SetTerm("Turn");
			}
			this.lastTurnNumber = GameManager.Board.CurnTurnNum;
		}
	}

	// Token: 0x040027AD RID: 10157
	public Text text;

	// Token: 0x040027AE RID: 10158
	public Text labelText;

	// Token: 0x040027AF RID: 10159
	public Localize textLocalize;

	// Token: 0x040027B0 RID: 10160
	private int lastTurnNumber = -1;
}
