using System;

// Token: 0x020002CB RID: 715
public class PlatformChatManager
{
	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x06001461 RID: 5217 RVA: 0x0000FEE1 File Offset: 0x0000E0E1
	public static PlatformChatManager Instance
	{
		get
		{
			PlatformChatManager platformChatManager = PlatformChatManager.instance;
			return PlatformChatManager.instance;
		}
	}

	// Token: 0x06001462 RID: 5218 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Initialize()
	{
	}

	// Token: 0x06001463 RID: 5219 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Update()
	{
	}

	// Token: 0x06001464 RID: 5220 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void ConnectToGameChat()
	{
	}

	// Token: 0x06001465 RID: 5221 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void DisconnectGameChat()
	{
	}

	// Token: 0x040015AB RID: 5547
	private static PlatformChatManager instance;
}
