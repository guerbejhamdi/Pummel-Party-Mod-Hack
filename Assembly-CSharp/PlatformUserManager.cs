using System;
using System.Collections.Generic;
using ZP.Net;

// Token: 0x020002EC RID: 748
public class PlatformUserManager
{
	// Token: 0x170001CD RID: 461
	// (get) Token: 0x060014E9 RID: 5353 RVA: 0x00010031 File Offset: 0x0000E231
	public IPlatformUser MainUser
	{
		get
		{
			return this.m_mainUser;
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x060014EA RID: 5354 RVA: 0x00010039 File Offset: 0x0000E239
	public static PlatformUserManager Instance
	{
		get
		{
			PlatformUserManager platformUserManager = PlatformUserManager.instance;
			return PlatformUserManager.instance;
		}
	}

	// Token: 0x060014EB RID: 5355 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Initialize()
	{
	}

	// Token: 0x060014EC RID: 5356 RVA: 0x00010046 File Offset: 0x0000E246
	public void SetMainUser(IPlatformUser user)
	{
		this.m_mainUser = user;
		NetSystem.SetPlayerName((user != null) ? user.GetProfileName() : "Player");
		if (this.OnMainUserChanged != null)
		{
			this.OnMainUserChanged(user);
		}
	}

	// Token: 0x060014ED RID: 5357 RVA: 0x00098F88 File Offset: 0x00097188
	public virtual IPlatformUser GetUserForControllerID(ulong controllerID)
	{
		IPlatformUser result = null;
		this.m_controllerUserMap.TryGetValue(controllerID, out result);
		return result;
	}

	// Token: 0x060014EE RID: 5358 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void ShowAccountPicker(ulong controllerID, bool allowGuests, bool requireUnique, AccountPickerCompleteEvent callback)
	{
	}

	// Token: 0x060014EF RID: 5359 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void RefreshAfterSuspend()
	{
	}

	// Token: 0x040015E2 RID: 5602
	protected static PlatformUserManager instance;

	// Token: 0x040015E3 RID: 5603
	public UserManagerEvent OnMainUserSignOutStarted;

	// Token: 0x040015E4 RID: 5604
	public UserManagerEvent OnMainUserSignOutCompleted;

	// Token: 0x040015E5 RID: 5605
	public AccountChangedEvent OnMainUserChanged;

	// Token: 0x040015E6 RID: 5606
	public AccountChangedEvent OnAccountDetailsChanged;

	// Token: 0x040015E7 RID: 5607
	public AccountChangedEvent OnAccountSignedOut;

	// Token: 0x040015E8 RID: 5608
	public ControllerStatusChangedEvent OnControllerDisconnected;

	// Token: 0x040015E9 RID: 5609
	protected IPlatformUser m_mainUser;

	// Token: 0x040015EA RID: 5610
	protected Dictionary<ulong, IPlatformUser> m_controllerUserMap = new Dictionary<ulong, IPlatformUser>();
}
