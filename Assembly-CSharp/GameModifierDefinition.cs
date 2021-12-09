using System;
using UnityEngine;

// Token: 0x020002B0 RID: 688
public class GameModifierDefinition : ScriptableObject
{
	// Token: 0x060013F9 RID: 5113 RVA: 0x000972DC File Offset: 0x000954DC
	public static int GetGameModifierID(GameModifierDefinition def)
	{
		int num = (new int[]
		{
			0,
			10000,
			20000
		})[(int)def.modifierType];
		switch (def.modifierType)
		{
		case GameModifierType.BoardModifier:
			return (int)(num + def.boardModifierID);
		case GameModifierType.MinigameModifier:
			return (int)(num + def.minigameModifierID);
		case GameModifierType.GlobalMinigameModifier:
			return (int)(num + def.globalMinigameModifierID);
		default:
			return 0;
		}
	}

	// Token: 0x0400152D RID: 5421
	public bool enabled = true;

	// Token: 0x0400152E RID: 5422
	public GameModifierType modifierType;

	// Token: 0x0400152F RID: 5423
	public BoardModifierID boardModifierID;

	// Token: 0x04001530 RID: 5424
	public MinigameModifierID minigameModifierID;

	// Token: 0x04001531 RID: 5425
	public GlobalMinigameModifierID globalMinigameModifierID;

	// Token: 0x04001532 RID: 5426
	public ModifierActivationState defaultActivationState;

	// Token: 0x04001533 RID: 5427
	public string scriptTypeName = "";

	// Token: 0x04001534 RID: 5428
	public string modifierName = "";

	// Token: 0x04001535 RID: 5429
	public string nameToken = "";

	// Token: 0x04001536 RID: 5430
	public string description = "";

	// Token: 0x04001537 RID: 5431
	public string descriptionToken = "";

	// Token: 0x04001538 RID: 5432
	public Sprite icon;

	// Token: 0x04001539 RID: 5433
	public Color color = Color.white;
}
