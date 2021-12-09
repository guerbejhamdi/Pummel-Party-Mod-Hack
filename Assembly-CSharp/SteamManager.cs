using System;
using System.IO;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x02000448 RID: 1096
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x17000390 RID: 912
	// (get) Token: 0x06001E2A RID: 7722 RVA: 0x000163DB File Offset: 0x000145DB
	private static SteamManager Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x06001E2B RID: 7723 RVA: 0x000163FF File Offset: 0x000145FF
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x06001E2C RID: 7724 RVA: 0x0001640B File Offset: 0x0001460B
	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x06001E2D RID: 7725 RVA: 0x000C37D0 File Offset: 0x000C19D0
	private void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (!Application.isEditor)
			{
				this.DeleteSteamAppID();
			}
			if (SteamAPI.RestartAppIfNecessary((AppId_t)880940U))
			{
				Debug.LogError("Steam Quit.");
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			string str = "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n";
			DllNotFoundException ex2 = ex;
			Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null), this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInialized = true;
	}

	// Token: 0x06001E2E RID: 7726 RVA: 0x000C38CC File Offset: 0x000C1ACC
	private void DeleteSteamAppID()
	{
		if (Application.isEditor)
		{
			return;
		}
		if (File.Exists("steam_appid.txt"))
		{
			try
			{
				File.Delete("steam_appid.txt");
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message);
			}
			if (File.Exists("steam_appid.txt"))
			{
				Debug.LogError("Quitting...");
				Application.Quit();
			}
		}
	}

	// Token: 0x06001E2F RID: 7727 RVA: 0x000C3930 File Offset: 0x000C1B30
	private void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x06001E30 RID: 7728 RVA: 0x00016413 File Offset: 0x00014613
	private void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x06001E31 RID: 7729 RVA: 0x00016437 File Offset: 0x00014637
	private void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x04002119 RID: 8473
	private static SteamManager s_instance;

	// Token: 0x0400211A RID: 8474
	private static bool s_EverInialized;

	// Token: 0x0400211B RID: 8475
	private bool m_bInitialized;

	// Token: 0x0400211C RID: 8476
	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
