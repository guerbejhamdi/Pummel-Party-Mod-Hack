using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200011D RID: 285
public class LoadSaveDialogElement : MonoBehaviour
{
	// Token: 0x06000883 RID: 2179 RVA: 0x0004F310 File Offset: 0x0004D510
	public void Setup(GameSave save, int index, bool first)
	{
		this.save = save;
		this.index = index;
		this.gameNameText.text = save.ownersName + "'s game";
		MapDetails mapDetails = GameManager.GetMap((int)save.lobbyOptions[1]);
		this.map.text = LocalizationManager.GetTranslation(mapDetails.name, true, 0, true, false, null, null, true);
		this.time.text = save.time.ToString();
		int num = GameManager.PossibleTurnCounts[(int)save.lobbyOptions[2]];
		this.turns.text = (save.turnSaves[index].curTurnNum + 1).ToString() + "/" + num.ToString();
		this.loadSaveDialog = base.GetComponentInParent<LoadSaveDialog>();
		if (first)
		{
			this.toggle.isOn = true;
		}
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x00009CD5 File Offset: 0x00007ED5
	public void OnToggle()
	{
		if (this.toggle.isOn)
		{
			this.loadSaveDialog.Selected(this);
		}
	}

	// Token: 0x040006D7 RID: 1751
	public Text gameNameText;

	// Token: 0x040006D8 RID: 1752
	public Text turns;

	// Token: 0x040006D9 RID: 1753
	public Text time;

	// Token: 0x040006DA RID: 1754
	public Text map;

	// Token: 0x040006DB RID: 1755
	public Toggle toggle;

	// Token: 0x040006DC RID: 1756
	[HideInInspector]
	public GameSave save;

	// Token: 0x040006DD RID: 1757
	[HideInInspector]
	public int index;

	// Token: 0x040006DE RID: 1758
	private LoadSaveDialog loadSaveDialog;
}
