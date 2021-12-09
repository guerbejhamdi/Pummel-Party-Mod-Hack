using System;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200052E RID: 1326
public class TabBar : Selectable
{
	// Token: 0x060022E8 RID: 8936 RVA: 0x000192BB File Offset: 0x000174BB
	protected override void Start()
	{
		if (Application.isPlaying)
		{
			this.Initialize();
		}
		base.Start();
	}

	// Token: 0x060022E9 RID: 8937 RVA: 0x000192D0 File Offset: 0x000174D0
	public override void OnSelect(BaseEventData eventData)
	{
		this.selected = true;
		this.UpdateState();
		base.OnSelect(eventData);
		if (!this.tween)
		{
			return;
		}
		this.SetStates(this.scale, this.highlightedColor, this.baseTextColor);
	}

	// Token: 0x060022EA RID: 8938 RVA: 0x00019307 File Offset: 0x00017507
	public override void OnDeselect(BaseEventData eventData)
	{
		this.selected = false;
		this.UpdateState();
		base.OnDeselect(eventData);
		if (!this.tween || !this.IsInteractable())
		{
			return;
		}
		this.SetStates(this.baseScale, this.baseColor, this.baseTextColor);
	}

	// Token: 0x060022EB RID: 8939 RVA: 0x000D494C File Offset: 0x000D2B4C
	public void SetInteractable(bool state)
	{
		base.interactable = state;
		this.UpdateState();
		if (!this.tween)
		{
			return;
		}
		if (!state)
		{
			this.SetStates(this.baseScale, this.disabledColor, this.disabledTextColor);
			return;
		}
		if (this.selected)
		{
			return;
		}
		this.SetStates(this.baseScale, this.baseColor, this.baseTextColor);
	}

	// Token: 0x060022EC RID: 8940 RVA: 0x000D49AC File Offset: 0x000D2BAC
	private void SetStates(Vector3 scale, Color outlineColor, Color textColor)
	{
		LeanTween.cancel(this.outline);
		LeanTween.cancel(this.outline);
		if (this.buttonText != null)
		{
			LeanTween.cancel(this.buttonText);
		}
		LeanTween.scale(this.outline, scale, 0.1f).setEase(LeanTweenType.easeInOutSine);
		LeanTween.color(this.outline, outlineColor, 0.1f).setEase(LeanTweenType.easeInOutSine).setRecursive(false);
		if (this.buttonText != null)
		{
			LeanTween.colorText(this.buttonText, textColor, 0.1f).setEase(LeanTweenType.easeInOutSine).setRecursive(false);
		}
	}

	// Token: 0x060022ED RID: 8941 RVA: 0x000D4A50 File Offset: 0x000D2C50
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

	// Token: 0x060022EE RID: 8942 RVA: 0x000D4A98 File Offset: 0x000D2C98
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

	// Token: 0x060022EF RID: 8943 RVA: 0x00019346 File Offset: 0x00017546
	protected virtual void ControllerChanged(Player player, Controller controller)
	{
		this.lastControllerType = ((controller != null) ? controller.type : ((player.id == 0) ? ControllerType.Mouse : ControllerType.Joystick));
		this.UpdateState();
	}

	// Token: 0x060022F0 RID: 8944 RVA: 0x000D4B10 File Offset: 0x000D2D10
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

	// Token: 0x060022F1 RID: 8945 RVA: 0x0001936B File Offset: 0x0001756B
	protected override void OnDestroy()
	{
		if (this.player != null)
		{
			this.player.controllers.RemoveLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.ControllerChanged));
		}
		base.OnDestroy();
	}

	// Token: 0x040025C3 RID: 9667
	public int playerID;

	// Token: 0x040025C4 RID: 9668
	public BasicButtonBase leftButton;

	// Token: 0x040025C5 RID: 9669
	public BasicButtonBase rightButton;

	// Token: 0x040025C6 RID: 9670
	[Header("Animation")]
	public bool tween;

	// Token: 0x040025C7 RID: 9671
	public RectTransform outline;

	// Token: 0x040025C8 RID: 9672
	public RectTransform buttonText;

	// Token: 0x040025C9 RID: 9673
	public Color highlightedColor = new Color(1f, 0.694929f, 0.3088235f);

	// Token: 0x040025CA RID: 9674
	public Color highlightedTextColor = new Color(1f, 0.694929f, 0.3088235f);

	// Token: 0x040025CB RID: 9675
	public Color baseColor = new Color(0.4352941f, 0.4352941f, 0.4352941f, 0.827451f);

	// Token: 0x040025CC RID: 9676
	public Color baseTextColor = new Color(1f, 1f, 1f, 1f);

	// Token: 0x040025CD RID: 9677
	public Color disabledColor = new Color(0.5220588f, 0.5220588f, 0.5220588f, 0.566f);

	// Token: 0x040025CE RID: 9678
	public Color disabledTextColor = new Color(0.4852941f, 0.4852941f, 0.4852941f, 0.553f);

	// Token: 0x040025CF RID: 9679
	private Vector3 baseScale = new Vector3(1f, 1f, 1f);

	// Token: 0x040025D0 RID: 9680
	private Vector3 scale = new Vector3(1.05f, 1f, 1f);

	// Token: 0x040025D1 RID: 9681
	protected Player player;

	// Token: 0x040025D2 RID: 9682
	private bool selected;

	// Token: 0x040025D3 RID: 9683
	private ControllerType lastControllerType = ControllerType.Mouse;

	// Token: 0x040025D4 RID: 9684
	private bool initialized;

	// Token: 0x040025D5 RID: 9685
	private EventTrigger eventTrigger;
}
