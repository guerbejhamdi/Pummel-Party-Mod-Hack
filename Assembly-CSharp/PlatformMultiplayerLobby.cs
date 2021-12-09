using System;

// Token: 0x020002D8 RID: 728
public class PlatformMultiplayerLobby
{
	// Token: 0x0600146F RID: 5231 RVA: 0x00098D54 File Offset: 0x00096F54
	public PlatformMultiplayerLobby(string name, string version, string mapName, string gameMode, int numPlayers, int numOpenSlots, object handle)
	{
		this.name = name;
		this.version = version;
		this.mapName = mapName;
		this.gameMode = gameMode;
		this.numPlayers = numPlayers;
		this.numOpenSlots = numOpenSlots;
		this.handle = handle;
	}

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x06001470 RID: 5232 RVA: 0x0000FEFB File Offset: 0x0000E0FB
	// (set) Token: 0x06001471 RID: 5233 RVA: 0x0000FF03 File Offset: 0x0000E103
	public string Name
	{
		get
		{
			return this.name;
		}
		set
		{
			this.name = value;
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06001472 RID: 5234 RVA: 0x0000FF0C File Offset: 0x0000E10C
	public string Version
	{
		get
		{
			return this.version;
		}
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06001473 RID: 5235 RVA: 0x0000FF14 File Offset: 0x0000E114
	public string MapName
	{
		get
		{
			return this.mapName;
		}
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06001474 RID: 5236 RVA: 0x0000FF1C File Offset: 0x0000E11C
	public string GameMode
	{
		get
		{
			return this.gameMode;
		}
	}

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x06001475 RID: 5237 RVA: 0x0000FF24 File Offset: 0x0000E124
	public int NumPlayers
	{
		get
		{
			return this.numPlayers;
		}
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x06001476 RID: 5238 RVA: 0x0000FF2C File Offset: 0x0000E12C
	public int NumOpenSlots
	{
		get
		{
			return this.numOpenSlots;
		}
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x06001477 RID: 5239 RVA: 0x0000FF34 File Offset: 0x0000E134
	public object Handle
	{
		get
		{
			return this.handle;
		}
	}

	// Token: 0x040015C6 RID: 5574
	private string name = "DEFAULT_NAME";

	// Token: 0x040015C7 RID: 5575
	private string version = "";

	// Token: 0x040015C8 RID: 5576
	private string mapName = "";

	// Token: 0x040015C9 RID: 5577
	private string gameMode = "";

	// Token: 0x040015CA RID: 5578
	private int numPlayers;

	// Token: 0x040015CB RID: 5579
	private int numOpenSlots;

	// Token: 0x040015CC RID: 5580
	private object handle;
}
