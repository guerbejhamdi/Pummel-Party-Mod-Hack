using System;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000087 RID: 135
public class RulesetSelectable : Button
{
	// Token: 0x060002E8 RID: 744 RVA: 0x00005826 File Offset: 0x00003A26
	protected override void Start()
	{
		if (Application.isPlaying)
		{
			this.Initialize();
		}
		base.Start();
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x00037AF4 File Offset: 0x00035CF4
	private void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		Player player = ReInput.players.GetPlayer((this.playerID == -1) ? 0 : this.playerID);
		bool isNull = player == null;
		this.SetPlayer(player, isNull);
	}

	// Token: 0x060002EA RID: 746 RVA: 0x00037B3C File Offset: 0x00035D3C
	public virtual void SetPlayer(Player newPlayer, bool isNull)
	{
		if (newPlayer == this.player)
		{
			return;
		}
		if (isNull)
		{
			this.player.controllers.RemoveLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.ControllerChanged));
		}
		this.player = newPlayer;
		this.player.controllers.AddLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.ControllerChanged));
		this.ControllerChanged(this.player, this.player.controllers.GetLastActiveController());
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0000583B File Offset: 0x00003A3B
	protected virtual void ControllerChanged(Player player, Controller controller)
	{
		this.lastControllerType = ((controller != null) ? controller.type : ((player.id == 0) ? ControllerType.Mouse : ControllerType.Joystick));
		this.UpdateState();
	}

	// Token: 0x060002EC RID: 748 RVA: 0x00037BB4 File Offset: 0x00035DB4
	private void UpdateState()
	{
		this.Initialize();
		bool flag = base.interactable && (this.selected || this.lastControllerType == ControllerType.Mouse || this.lastControllerType == ControllerType.Keyboard);
		try
		{
			this.leftButton.SetState(flag ? BasicButtonBase.BasicButtonState.Enabled : BasicButtonBase.BasicButtonState.Disabled);
			this.rightButton.SetState(flag ? BasicButtonBase.BasicButtonState.Enabled : BasicButtonBase.BasicButtonState.Disabled);
		}
		catch (Exception ex)
		{
			string str = "Exception: ";
			Exception ex2 = ex;
			Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null));
			Debug.LogError(base.gameObject.name);
		}
	}

	// Token: 0x060002ED RID: 749 RVA: 0x00005860 File Offset: 0x00003A60
	protected override void OnDestroy()
	{
		if (this.player != null)
		{
			this.player.controllers.RemoveLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.ControllerChanged));
		}
		base.OnDestroy();
	}

	// Token: 0x060002EE RID: 750 RVA: 0x0000588D File Offset: 0x00003A8D
	public override void OnSelect(BaseEventData eventData)
	{
		this.selected = true;
		this.UpdateState();
		base.OnSelect(eventData);
	}

	// Token: 0x060002EF RID: 751 RVA: 0x000058A3 File Offset: 0x00003AA3
	public override void OnDeselect(BaseEventData eventData)
	{
		this.selected = false;
		this.UpdateState();
		base.OnDeselect(eventData);
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x000058B9 File Offset: 0x00003AB9
	public void SetInteractable(bool state)
	{
		base.interactable = state;
		this.UpdateState();
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x000058C8 File Offset: 0x00003AC8
	public void Clear()
	{
		if (EventSystem.GetSystem(this.playerID).currentSelectedGameObject != base.gameObject)
		{
			this.selected = false;
			this.UpdateState();
		}
	}

	// Token: 0x04000300 RID: 768
	public int playerID;

	// Token: 0x04000301 RID: 769
	public BasicButtonBase leftButton;

	// Token: 0x04000302 RID: 770
	public BasicButtonBase rightButton;

	// Token: 0x04000303 RID: 771
	protected Player player;

	// Token: 0x04000304 RID: 772
	private bool selected;

	// Token: 0x04000305 RID: 773
	private ControllerType lastControllerType = ControllerType.Mouse;

	// Token: 0x04000306 RID: 774
	private bool initialized;
}
