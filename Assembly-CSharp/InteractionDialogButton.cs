using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000501 RID: 1281
public class InteractionDialogButton : BasicButtonBase
{
	// Token: 0x0600218F RID: 8591 RVA: 0x00018548 File Offset: 0x00016748
	protected override void Start()
	{
		this.interactionDialog = base.GetComponentInParent<InteractionDialog>();
		base.Start();
	}

	// Token: 0x06002190 RID: 8592 RVA: 0x0001855C File Offset: 0x0001675C
	public override void OnSubmit()
	{
		this.interactionDialog.OnInteractionChoice(this.choiceID);
		base.OnSubmit();
	}

	// Token: 0x06002191 RID: 8593 RVA: 0x000CF560 File Offset: 0x000CD760
	public void Setup(GamePlayer player, InteractionButtonSettings settings)
	{
		this.settings = settings;
		this.player = player;
		this.hasSettings = true;
		this.SetPlayer(player.RewiredPlayer, player.RewiredPlayer == null);
		base.SetState((settings.interactable && player.IsLocalPlayer && !player.IsAI && player.BoardObject.Gold >= settings.cost) ? BasicButtonBase.BasicButtonState.Enabled : BasicButtonBase.BasicButtonState.Disabled);
		if (this.costText != null)
		{
			this.costText.text = settings.cost.ToString();
		}
		for (int i = 0; i < this.choiceTexts.Length; i++)
		{
			if (this.choiceTexts[i] != null)
			{
				this.choiceTexts[i].text = LocalizationManager.GetTranslation(settings.choice, true, 0, true, false, null, null, true);
			}
		}
	}

	// Token: 0x06002192 RID: 8594 RVA: 0x00018575 File Offset: 0x00016775
	public void Setup(GamePlayer player)
	{
		this.SetPlayer(player.RewiredPlayer, player.RewiredPlayer == null);
		base.SetState((player.IsLocalPlayer && !player.IsAI) ? BasicButtonBase.BasicButtonState.Enabled : BasicButtonBase.BasicButtonState.Disabled);
	}

	// Token: 0x06002193 RID: 8595 RVA: 0x000CF634 File Offset: 0x000CD834
	public override void Update()
	{
		if (this.hasSettings && this.testTimer.Elapsed(true))
		{
			base.SetState((this.settings.interactable && this.player.IsLocalPlayer && !this.player.IsAI && this.player.BoardObject.Gold >= this.settings.cost) ? BasicButtonBase.BasicButtonState.Enabled : BasicButtonBase.BasicButtonState.Disabled);
		}
		base.Update();
	}

	// Token: 0x0400244F RID: 9295
	[Header("Interaction Dialog Button Vars")]
	public int choiceID;

	// Token: 0x04002450 RID: 9296
	public Text[] choiceTexts;

	// Token: 0x04002451 RID: 9297
	public Text costText;

	// Token: 0x04002452 RID: 9298
	private InteractionDialog interactionDialog;

	// Token: 0x04002453 RID: 9299
	private bool hasSettings;

	// Token: 0x04002454 RID: 9300
	private InteractionButtonSettings settings;

	// Token: 0x04002455 RID: 9301
	private new GamePlayer player;

	// Token: 0x04002456 RID: 9302
	private ActionTimer testTimer = new ActionTimer(1f);
}
