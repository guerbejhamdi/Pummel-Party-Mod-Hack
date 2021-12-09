using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020004FF RID: 1279
public class InteractionDialog : MonoBehaviour
{
	// Token: 0x0600218C RID: 8588 RVA: 0x00018524 File Offset: 0x00016724
	public void OnInteractionChoice(int choice)
	{
		GameManager.Board.OnInteractionChoice(choice);
	}

	// Token: 0x04002446 RID: 9286
	public MainMenuWindow window;

	// Token: 0x04002447 RID: 9287
	public Text titleText;

	// Token: 0x04002448 RID: 9288
	public Text descText;

	// Token: 0x04002449 RID: 9289
	public Image icon;

	// Token: 0x0400244A RID: 9290
	public InteractionDialogButton[] buttons;

	// Token: 0x0400244B RID: 9291
	public EventSystemGroup eventSystemGroup;
}
