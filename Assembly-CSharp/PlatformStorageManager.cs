using System;

// Token: 0x020002E2 RID: 738
public class PlatformStorageManager
{
	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x060014B5 RID: 5301 RVA: 0x0000FF9D File Offset: 0x0000E19D
	// (set) Token: 0x060014B6 RID: 5302 RVA: 0x0000FFA5 File Offset: 0x0000E1A5
	public bool Setup { get; set; }

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x060014B7 RID: 5303 RVA: 0x0000FFAE File Offset: 0x0000E1AE
	// (set) Token: 0x060014B8 RID: 5304 RVA: 0x0000FFB6 File Offset: 0x0000E1B6
	public bool Ready { get; set; }

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x060014B9 RID: 5305 RVA: 0x0000FFBF File Offset: 0x0000E1BF
	// (set) Token: 0x060014BA RID: 5306 RVA: 0x0000FFC7 File Offset: 0x0000E1C7
	public bool Suspending { get; set; }

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x060014BC RID: 5308 RVA: 0x0000FFD0 File Offset: 0x0000E1D0
	public static PlatformStorageManager Instance
	{
		get
		{
			PlatformStorageManager platformStorageManager = PlatformStorageManager.instance;
			return PlatformStorageManager.instance;
		}
	}

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x060014BD RID: 5309 RVA: 0x00098F18 File Offset: 0x00097118
	// (remove) Token: 0x060014BE RID: 5310 RVA: 0x00098F50 File Offset: 0x00097150
	public event PlatformStorageManager.StorageLoadedEvent OnStorageLoaded;

	// Token: 0x060014BF RID: 5311 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Initialize()
	{
	}

	// Token: 0x060014C0 RID: 5312 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void LoadAll()
	{
	}

	// Token: 0x060014C1 RID: 5313 RVA: 0x000053AE File Offset: 0x000035AE
	public virtual byte[] Load(string file)
	{
		return null;
	}

	// Token: 0x060014C2 RID: 5314 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Save(string file, byte[] data)
	{
	}

	// Token: 0x060014C3 RID: 5315 RVA: 0x0000FFDD File Offset: 0x0000E1DD
	public virtual bool HasLoaded()
	{
		return this.m_hasLoaded;
	}

	// Token: 0x060014C4 RID: 5316 RVA: 0x0000FFE5 File Offset: 0x0000E1E5
	protected void OnStorageLoadedInternal()
	{
		if (this.OnStorageLoaded != null)
		{
			this.OnStorageLoaded();
		}
	}

	// Token: 0x060014C5 RID: 5317 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Update()
	{
	}

	// Token: 0x040015D2 RID: 5586
	private static PlatformStorageManager instance;

	// Token: 0x040015D3 RID: 5587
	public static string PreferencesFile = "Preferences";

	// Token: 0x040015D4 RID: 5588
	public static string SavesFile = "Save";

	// Token: 0x040015D5 RID: 5589
	public static string RulesetsFile = "Rulesets";

	// Token: 0x040015D6 RID: 5590
	public static string BoardSavesFile = "BoardSaves";

	// Token: 0x040015D7 RID: 5591
	protected bool m_hasLoaded;

	// Token: 0x020002E3 RID: 739
	// (Invoke) Token: 0x060014C8 RID: 5320
	public delegate void StorageLoadedEvent();
}
