using System;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000512 RID: 1298
public class MainMenuButton : MonoBehaviour
{
	// Token: 0x060021DA RID: 8666 RVA: 0x000188A1 File Offset: 0x00016AA1
	private bool ShowOnPlatform()
	{
		return this.OnClickEvent != MainMenuButtonEventType.SelectProfile;
	}

	// Token: 0x060021DB RID: 8667 RVA: 0x000D09D0 File Offset: 0x000CEBD0
	private void Start()
	{
		if (!this.ShowOnPlatform())
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.ui_canvas = GameObject.Find("Canvas");
		if (this.ui_canvas != null)
		{
			this.ui_controller = this.ui_canvas.GetComponent<UIController>();
		}
		this.SetPlayer(this.playerID);
		if (ReInput.isReady)
		{
			this.SetPlayer(ReInput.players.GetPlayer(0));
		}
		this.mainmenuWindow = base.GetComponentInParent<MainMenuWindow>();
		if (this.button == null)
		{
			this.button = base.GetComponentInChildren<Button>();
		}
		this.actionID = ReInput.mapping.GetActionId(this.actionName);
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x0000398C File Offset: 0x00001B8C
	private void SetPlayer(Player player)
	{
	}

	// Token: 0x060021DD RID: 8669 RVA: 0x0000398C File Offset: 0x00001B8C
	private void SetPlayer(int i)
	{
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x000D0A84 File Offset: 0x000CEC84
	private void Update()
	{
		if (this.pollAction2 && (this.mainmenuWindow == null || this.mainmenuWindow.Visible) && (!this.pollPadOnly || (ReInput.players.GetPlayer(0).controllers.GetLastActiveController() != null && ReInput.players.GetPlayer(0).controllers.GetLastActiveController().type == ControllerType.Joystick)) && (this.button == null || (this.button != null && this.button.IsInteractable())) && this.actionID != -1 && ReInput.players.GetPlayer(0).GetButtonDown(this.actionID))
		{
			this.ButtonClick();
		}
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x000D0B48 File Offset: 0x000CED48
	public virtual void ButtonClick()
	{
		AudioSystem.PlayOneShot("ButtonPress01_SFXR", 1f, 0f);
		if (this.ui_controller != null)
		{
			this.ui_controller.DoButtonEvent(this.OnClickEvent);
			return;
		}
		GameManager.UIController.DoButtonEvent(this.OnClickEvent);
	}

	// Token: 0x060021E0 RID: 8672 RVA: 0x000188B0 File Offset: 0x00016AB0
	public void ButtonHover()
	{
		AudioSystem.PlayOneShot("ButtonHover01_SFXR", 1f, 0f);
		if (this.ui_controller != null)
		{
			this.ui_controller.SetHoverText(this.hover_description);
		}
	}

	// Token: 0x060021E1 RID: 8673 RVA: 0x000188E5 File Offset: 0x00016AE5
	public void ButtonLeave()
	{
		if (this.ui_controller != null)
		{
			this.ui_controller.SetHoverText("EmptyString");
		}
	}

	// Token: 0x060021E2 RID: 8674 RVA: 0x0000398C File Offset: 0x00001B8C
	public void OnPointerClick()
	{
	}

	// Token: 0x040024C8 RID: 9416
	public MainMenuButtonEventType OnClickEvent;

	// Token: 0x040024C9 RID: 9417
	public string hover_description;

	// Token: 0x040024CA RID: 9418
	public int playerID;

	// Token: 0x040024CB RID: 9419
	public Image glyphImage;

	// Token: 0x040024CC RID: 9420
	public bool pollAction2;

	// Token: 0x040024CD RID: 9421
	public bool pollPadOnly;

	// Token: 0x040024CE RID: 9422
	public string actionName;

	// Token: 0x040024CF RID: 9423
	protected MainMenuWindow mainmenuWindow;

	// Token: 0x040024D0 RID: 9424
	public Button button;

	// Token: 0x040024D1 RID: 9425
	private GameObject ui_canvas;

	// Token: 0x040024D2 RID: 9426
	protected UIController ui_controller;

	// Token: 0x040024D3 RID: 9427
	private int actionID = -1;
}
