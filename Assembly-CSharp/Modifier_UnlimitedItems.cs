using System;
using ZP.Net;

// Token: 0x020002AA RID: 682
public class Modifier_UnlimitedItems : BoardModifier
{
	// Token: 0x060013EF RID: 5103 RVA: 0x00005743 File Offset: 0x00003943
	protected override int GetModifierID()
	{
		return 4;
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x0000539F File Offset: 0x0000359F
	public override bool OnShouldConsumeItems()
	{
		return false;
	}

	// Token: 0x060013F1 RID: 5105 RVA: 0x00097244 File Offset: 0x00095444
	public override void OnBoardReturnFromMinigame()
	{
		if (NetSystem.IsServer && !this.m_hasGivenItems && GameManager.Board != null)
		{
			foreach (BoardPlayer boardPlayer in GameManager.Board.BoardPlayers)
			{
				for (int i = 0; i < 15; i++)
				{
					if (GameManager.ItemList.IsItemActive(i))
					{
						boardPlayer.GiveItem((byte)i, false);
					}
				}
			}
			this.m_hasGivenItems = true;
		}
	}

	// Token: 0x04001517 RID: 5399
	private bool m_hasGivenItems;
}
