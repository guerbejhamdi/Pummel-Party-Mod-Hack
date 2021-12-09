using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200035D RID: 861
public class Interaction : MonoBehaviour
{
	// Token: 0x17000229 RID: 553
	// (get) Token: 0x0600172D RID: 5933 RVA: 0x000114AE File Offset: 0x0000F6AE
	// (set) Token: 0x0600172E RID: 5934 RVA: 0x000114B6 File Offset: 0x0000F6B6
	public bool Finished { get; protected set; }

	// Token: 0x0600172F RID: 5935 RVA: 0x000114BF File Offset: 0x0000F6BF
	public virtual void Setup(BoardPlayer player)
	{
		this.Finished = false;
		this.player = player;
	}

	// Token: 0x06001730 RID: 5936 RVA: 0x000114CF File Offset: 0x0000F6CF
	public virtual void OnInteractionChoice(byte choice, int seed)
	{
		if (this.dialog != null)
		{
			this.dialog.window.SetState(MainMenuWindowState.Hidden);
		}
		GameManager.UIController.SetInputStatus(false);
		base.StartCoroutine(this.DoInteraction(choice));
	}

	// Token: 0x06001731 RID: 5937 RVA: 0x000053AE File Offset: 0x000035AE
	public virtual IEnumerator DoInteraction(byte choice)
	{
		return null;
	}

	// Token: 0x06001732 RID: 5938 RVA: 0x0000539F File Offset: 0x0000359F
	public virtual int GetAIChoice()
	{
		return 0;
	}

	// Token: 0x04001878 RID: 6264
	public string title = "Title";

	// Token: 0x04001879 RID: 6265
	public string description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla tincidunt bibendum lectus, id porta nulla. Vivamus molestie, dolor at tincidunt placerat, augue nunc fermentum quam, quis imperdiet ipsum enim quis purus.";

	// Token: 0x0400187A RID: 6266
	public Sprite icon;

	// Token: 0x0400187B RID: 6267
	public InteractionButtonSettings[] buttonSettings;

	// Token: 0x0400187C RID: 6268
	public InteractionDialog dialog;

	// Token: 0x0400187D RID: 6269
	protected BoardPlayer player;
}
