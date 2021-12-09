using System;

// Token: 0x020002F0 RID: 752
internal class GameChat2Data
{
	// Token: 0x06001508 RID: 5384 RVA: 0x000100D4 File Offset: 0x0000E2D4
	static GameChat2Data()
	{
		GameChat2Data.ChatEnabled = true;
		GameChat2Data.SendChatData = true;
	}

	// Token: 0x040015FE RID: 5630
	public static bool ChatEnabled;

	// Token: 0x040015FF RID: 5631
	public static int SelectedUser = 0;

	// Token: 0x04001600 RID: 5632
	public static bool SendChatData;
}
