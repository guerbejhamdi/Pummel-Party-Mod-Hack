using System;

// Token: 0x020002E6 RID: 742
public class PlatformTextManager
{
	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x060014CF RID: 5327 RVA: 0x00010024 File Offset: 0x0000E224
	public static PlatformTextManager Instance
	{
		get
		{
			PlatformTextManager platformTextManager = PlatformTextManager.instance;
			return PlatformTextManager.instance;
		}
	}

	// Token: 0x060014D0 RID: 5328 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Initialize()
	{
	}

	// Token: 0x060014D1 RID: 5329 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void GetSanitizedText(string defaultText, string keyboardTitle, string keyboardDescription, SanitizeTextResult callback)
	{
	}

	// Token: 0x040015E1 RID: 5601
	private static PlatformTextManager instance;
}
