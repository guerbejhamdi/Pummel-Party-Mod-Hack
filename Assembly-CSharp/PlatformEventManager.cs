using System;

// Token: 0x020002CD RID: 717
public class PlatformEventManager
{
	// Token: 0x170001BA RID: 442
	// (get) Token: 0x06001467 RID: 5223 RVA: 0x0000FEEE File Offset: 0x0000E0EE
	public static PlatformEventManager Instance
	{
		get
		{
			PlatformEventManager platformEventManager = PlatformEventManager.instance;
			return PlatformEventManager.instance;
		}
	}

	// Token: 0x06001468 RID: 5224 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Initialize()
	{
	}

	// Token: 0x06001469 RID: 5225 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Update()
	{
	}

	// Token: 0x040015AC RID: 5548
	private static PlatformEventManager instance;
}
