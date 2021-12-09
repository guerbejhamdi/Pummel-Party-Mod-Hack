using System;
using UnityEngine;

// Token: 0x020002A5 RID: 677
public class Modifier_BoardDoubleTime : BoardModifier
{
	// Token: 0x060013DE RID: 5086 RVA: 0x000054CD File Offset: 0x000036CD
	protected override int GetModifierID()
	{
		return 3;
	}

	// Token: 0x060013DF RID: 5087 RVA: 0x0000FAF8 File Offset: 0x0000DCF8
	public override void BoardPreInitialize(GameBoardController controller)
	{
		Time.timeScale = 2f;
	}

	// Token: 0x060013E0 RID: 5088 RVA: 0x0000FB04 File Offset: 0x0000DD04
	public override void OnBoardEnterMinigame()
	{
		Time.timeScale = 1f;
	}

	// Token: 0x060013E1 RID: 5089 RVA: 0x0000FAF8 File Offset: 0x0000DCF8
	public override void OnBoardReturnFromMinigame()
	{
		Time.timeScale = 2f;
	}

	// Token: 0x060013E2 RID: 5090 RVA: 0x0000FB04 File Offset: 0x0000DD04
	public override void OnDestroy()
	{
		Time.timeScale = 1f;
	}
}
