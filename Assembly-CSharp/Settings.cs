using System;
using System.Collections.Generic;
using I2.Loc;
using Rewired;
using UnityEngine;

// Token: 0x0200040A RID: 1034
public static class Settings
{
	// Token: 0x17000364 RID: 868
	// (get) Token: 0x06001CCD RID: 7373 RVA: 0x000153F2 File Offset: 0x000135F2
	// (set) Token: 0x06001CCE RID: 7374 RVA: 0x000153F9 File Offset: 0x000135F9
	public static Resolution Resolution
	{
		get
		{
			return Settings.resolution;
		}
		set
		{
			Settings.resolution = value;
		}
	}

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x06001CCF RID: 7375 RVA: 0x00015401 File Offset: 0x00013601
	// (set) Token: 0x06001CD0 RID: 7376 RVA: 0x00015408 File Offset: 0x00013608
	public static WindowMode WindowMode
	{
		get
		{
			return Settings.windowMode;
		}
		set
		{
			Settings.windowMode = value;
		}
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x06001CD1 RID: 7377 RVA: 0x00015410 File Offset: 0x00013610
	// (set) Token: 0x06001CD2 RID: 7378 RVA: 0x00015417 File Offset: 0x00013617
	public static VSyncCount VSyncCount
	{
		get
		{
			return Settings.vSyncCount;
		}
		set
		{
			QualitySettings.vSyncCount = (int)value;
			Settings.vSyncCount = value;
		}
	}

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x06001CD3 RID: 7379 RVA: 0x00015425 File Offset: 0x00013625
	// (set) Token: 0x06001CD4 RID: 7380 RVA: 0x0001542C File Offset: 0x0001362C
	public static int TargetFrameRate
	{
		get
		{
			return Settings.targetFrameRate;
		}
		set
		{
			Application.targetFrameRate = value;
			Settings.targetFrameRate = value;
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x06001CD5 RID: 7381 RVA: 0x0001543A File Offset: 0x0001363A
	// (set) Token: 0x06001CD6 RID: 7382 RVA: 0x00015441 File Offset: 0x00013641
	public static SettingsAmbientOcclusionQuality AmbientOcclusion
	{
		get
		{
			return Settings.ambientOcclusion;
		}
		set
		{
			Settings.ambientOcclusion = value;
		}
	}

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x06001CD7 RID: 7383 RVA: 0x00015449 File Offset: 0x00013649
	// (set) Token: 0x06001CD8 RID: 7384 RVA: 0x00015450 File Offset: 0x00013650
	public static AntiAliasingType AntiAliasing
	{
		get
		{
			return Settings.antiAliasing;
		}
		set
		{
			Settings.antiAliasing = value;
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x00015458 File Offset: 0x00013658
	// (set) Token: 0x06001CDA RID: 7386 RVA: 0x0001545F File Offset: 0x0001365F
	public static BloomQuality Bloom
	{
		get
		{
			return Settings.bloom;
		}
		set
		{
			Settings.bloom = value;
		}
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x06001CDB RID: 7387 RVA: 0x00015467 File Offset: 0x00013667
	// (set) Token: 0x06001CDC RID: 7388 RVA: 0x0001546E File Offset: 0x0001366E
	public static ShadowQuality Shadows
	{
		get
		{
			return Settings.shadows;
		}
		set
		{
			Settings.shadows = value;
		}
	}

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x06001CDD RID: 7389 RVA: 0x00015476 File Offset: 0x00013676
	// (set) Token: 0x06001CDE RID: 7390 RVA: 0x0001547D File Offset: 0x0001367D
	public static ShadowResolution ShadowResolution
	{
		get
		{
			return Settings.shadowResolution;
		}
		set
		{
			Settings.shadowResolution = value;
		}
	}

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x06001CDF RID: 7391 RVA: 0x00015485 File Offset: 0x00013685
	// (set) Token: 0x06001CE0 RID: 7392 RVA: 0x0001548C File Offset: 0x0001368C
	public static float ShadowDistance
	{
		get
		{
			return Settings.shadowDistance;
		}
		set
		{
			Settings.shadowDistance = value;
		}
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x06001CE1 RID: 7393 RVA: 0x00015494 File Offset: 0x00013694
	// (set) Token: 0x06001CE2 RID: 7394 RVA: 0x0001549B File Offset: 0x0001369B
	public static int TextureRes
	{
		get
		{
			return Settings.textureRes;
		}
		set
		{
			Settings.shadowDistance = (float)value;
		}
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x06001CE3 RID: 7395 RVA: 0x000154A4 File Offset: 0x000136A4
	// (set) Token: 0x06001CE4 RID: 7396 RVA: 0x000154AB File Offset: 0x000136AB
	public static ParticleQuality ParticleQuality
	{
		get
		{
			return Settings.particleQuality;
		}
		set
		{
			Settings.particleQuality = value;
		}
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x000154B3 File Offset: 0x000136B3
	// (set) Token: 0x06001CE6 RID: 7398 RVA: 0x000154BA File Offset: 0x000136BA
	public static DetailQuality DetailQuality
	{
		get
		{
			return Settings.detailQuality;
		}
		set
		{
			Settings.detailQuality = value;
		}
	}

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x000154C2 File Offset: 0x000136C2
	// (set) Token: 0x06001CE8 RID: 7400 RVA: 0x000154C9 File Offset: 0x000136C9
	public static float MasterVolume
	{
		get
		{
			return Settings.masterVolume;
		}
		set
		{
			Settings.masterVolume = value;
		}
	}

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x000154D1 File Offset: 0x000136D1
	// (set) Token: 0x06001CEA RID: 7402 RVA: 0x000154D8 File Offset: 0x000136D8
	public static float MusicVolume
	{
		get
		{
			return Settings.musicVolume;
		}
		set
		{
			Settings.musicVolume = value;
		}
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x06001CEB RID: 7403 RVA: 0x000154E0 File Offset: 0x000136E0
	// (set) Token: 0x06001CEC RID: 7404 RVA: 0x000154E7 File Offset: 0x000136E7
	public static float EffectsVolume
	{
		get
		{
			return Settings.effectsVolume;
		}
		set
		{
			Settings.effectsVolume = value;
		}
	}

	// Token: 0x17000374 RID: 884
	// (get) Token: 0x06001CED RID: 7405 RVA: 0x000154EF File Offset: 0x000136EF
	public static List<string> Names
	{
		get
		{
			return Settings.names;
		}
	}

	// Token: 0x17000375 RID: 885
	// (get) Token: 0x06001CEE RID: 7406 RVA: 0x000154F6 File Offset: 0x000136F6
	// (set) Token: 0x06001CEF RID: 7407 RVA: 0x000154FD File Offset: 0x000136FD
	public static bool BloodEffects
	{
		get
		{
			return Settings.bloodEffects;
		}
		set
		{
			Settings.bloodEffects = value;
		}
	}

	// Token: 0x17000376 RID: 886
	// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x00015505 File Offset: 0x00013705
	// (set) Token: 0x06001CF1 RID: 7409 RVA: 0x0001550C File Offset: 0x0001370C
	public static bool ControllerRumble
	{
		get
		{
			return Settings.controllerRumble;
		}
		set
		{
			Settings.controllerRumble = value;
		}
	}

	// Token: 0x17000377 RID: 887
	// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x00015514 File Offset: 0x00013714
	// (set) Token: 0x06001CF3 RID: 7411 RVA: 0x0001551B File Offset: 0x0001371B
	public static bool ChatEnabled
	{
		get
		{
			return Settings.chatEnabled;
		}
		set
		{
			Settings.chatEnabled = value;
		}
	}

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x00015523 File Offset: 0x00013723
	// (set) Token: 0x06001CF5 RID: 7413 RVA: 0x0001552A File Offset: 0x0001372A
	public static bool CameraShake
	{
		get
		{
			return Settings.cameraShake;
		}
		set
		{
			Settings.cameraShake = value;
		}
	}

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x00015532 File Offset: 0x00013732
	// (set) Token: 0x06001CF7 RID: 7415 RVA: 0x00015539 File Offset: 0x00013739
	public static bool UseXInput
	{
		get
		{
			return Settings.useXInput;
		}
		set
		{
			ReInput.configuration.useXInput = value;
			Settings.useXInput = value;
			GameManager.OnXInput();
		}
	}

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x00015556 File Offset: 0x00013756
	// (set) Token: 0x06001CF9 RID: 7417 RVA: 0x0001555D File Offset: 0x0001375D
	public static string Language
	{
		get
		{
			return Settings.language;
		}
		set
		{
			LocalizationManager.CurrentLanguage = value;
			Settings.language = value;
		}
	}

	// Token: 0x06001CFA RID: 7418 RVA: 0x0001556B File Offset: 0x0001376B
	private static void UseLocalizationCSV(string CSVfile)
	{
		LocalizationManager.Sources[0].Import_CSV(string.Empty, CSVfile, eSpreadsheetUpdateMode.Replace, ',');
		LocalizationManager.LocalizeAll(true);
	}

	// Token: 0x06001CFB RID: 7419 RVA: 0x000BDF30 File Offset: 0x000BC130
	static Settings()
	{
		Settings.LoadSettings();
		if (!Application.isEditor && RBPrefs.GetString("LastVersion", "") != GameManager.VERSION)
		{
			RBPrefs.SetString("LastVersion", GameManager.VERSION);
			RBPrefs.SetInt("LobbySettings_1", 0);
		}
	}

	// Token: 0x06001CFC RID: 7420 RVA: 0x0001558D File Offset: 0x0001378D
	public static void OnStorageLoaded()
	{
		Settings.LoadSettings();
		Settings.ApplySettings();
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x00015599 File Offset: 0x00013799
	public static void ApplySettings()
	{
		Settings.SetGraphicsQualitySettings();
		AudioSystem.MasterVolume = Settings.masterVolume;
		AudioSystem.MusicVolume = Settings.musicVolume;
		AudioSystem.EffectsVolume = Settings.effectsVolume;
		LocalizationManager.CurrentLanguage = Settings.language;
		ReInput.configuration.useXInput = Settings.useXInput;
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x000BDF84 File Offset: 0x000BC184
	private static void SetGraphicsQualitySettings()
	{
		if (Screen.currentResolution.width != Settings.resolution.width || Screen.currentResolution.height != Settings.resolution.height || Screen.currentResolution.refreshRate != Settings.resolution.refreshRate)
		{
			Screen.SetResolution(Settings.resolution.width, Settings.resolution.height, (FullScreenMode)Settings.windowMode, Settings.resolution.refreshRate);
		}
		if (Screen.fullScreenMode != (FullScreenMode)Settings.windowMode)
		{
			Screen.fullScreenMode = (FullScreenMode)Settings.windowMode;
		}
		if (QualitySettings.vSyncCount != (int)Settings.vSyncCount)
		{
			QualitySettings.vSyncCount = (int)Settings.vSyncCount;
		}
		if (Application.targetFrameRate != Settings.targetFrameRate)
		{
			Application.targetFrameRate = Settings.targetFrameRate;
		}
		if (QualitySettings.shadows != Settings.shadows)
		{
			QualitySettings.shadows = Settings.shadows;
		}
		if (QualitySettings.shadowResolution != Settings.shadowResolution)
		{
			QualitySettings.shadowResolution = Settings.shadowResolution;
		}
		float num = QualitySettings.shadowDistance;
		float num2 = Settings.shadowDistance;
		if (QualitySettings.masterTextureLimit != Settings.textureRes)
		{
			QualitySettings.masterTextureLimit = Settings.textureRes;
		}
		if (Settings.OnEffectsChange != null)
		{
			Settings.OnEffectsChange();
		}
	}

	// Token: 0x06001CFF RID: 7423 RVA: 0x000BE0A8 File Offset: 0x000BC2A8
	private static void LoadSettings()
	{
		Settings.resolution = default(Resolution);
		Resolution[] resolutions = Screen.resolutions;
		Settings.resolution.height = RBPrefs.GetInt("ResolutionHeight", resolutions[resolutions.Length - 1].height);
		Settings.resolution.width = RBPrefs.GetInt("ResolutionWidth", resolutions[resolutions.Length - 1].width);
		Settings.resolution.refreshRate = RBPrefs.GetInt("RefreshRate", resolutions[resolutions.Length - 1].refreshRate);
		bool flag = false;
		for (int i = 0; i < resolutions.Length; i++)
		{
			if (Settings.resolution.height == resolutions[i].height && Settings.resolution.width == resolutions[i].width && Settings.resolution.refreshRate == resolutions[i].refreshRate)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Settings.resolution = resolutions[resolutions.Length - 1];
		}
		Settings.windowMode = (WindowMode)RBPrefs.GetInt("Fullscreen", 0);
		Settings.vSyncCount = (VSyncCount)RBPrefs.GetInt("VSyncCount", 1);
		Settings.targetFrameRate = RBPrefs.GetInt("TargetFrameRate", 200);
		Settings.ambientOcclusion = (SettingsAmbientOcclusionQuality)RBPrefs.GetInt("AmbientOcclusion", 3);
		Settings.antiAliasing = (AntiAliasingType)RBPrefs.GetInt("AntiAliasingType", 2);
		Settings.bloom = (BloomQuality)RBPrefs.GetInt("Bloom", 1);
		Settings.shadows = (ShadowQuality)RBPrefs.GetInt("Shadows", 2);
		Settings.shadowResolution = (ShadowResolution)RBPrefs.GetInt("ShadowResolution", 3);
		Settings.shadowDistance = RBPrefs.GetFloat("ShadowDistance", 300f);
		Settings.textureRes = RBPrefs.GetInt("TextureRes", 0);
		Settings.particleQuality = (ParticleQuality)RBPrefs.GetInt("ParticleQuality", 1);
		Settings.detailQuality = (DetailQuality)RBPrefs.GetInt("DetailQuality", 2);
		Settings.masterVolume = RBPrefs.GetFloat("MasterVolume", 0.5f);
		Settings.musicVolume = RBPrefs.GetFloat("MusicVolume", 0.5f);
		Settings.effectsVolume = RBPrefs.GetFloat("EffectsVolume", 0.5f);
		if (RBPrefs.GetString("PlayerNames", "") != "")
		{
			Settings.names = new List<string>(RBPrefs.GetString("PlayerNames", "").Split(new char[]
			{
				','
			}));
			List<string> list = new List<string>();
			for (int j = 0; j < Settings.names.Count; j++)
			{
				if (!list.Contains(Settings.names[j]))
				{
					list.Add(Settings.names[j]);
				}
			}
			Settings.names = list;
		}
		else
		{
			Settings.names = new List<string>();
		}
		Settings.bloodEffects = (RBPrefs.GetInt("BloodEffects", 1) == 1);
		Settings.controllerRumble = (RBPrefs.GetInt("ControllerRumble", 1) == 1);
		Settings.chatEnabled = (RBPrefs.GetInt("ChatEnabled", 1) == 1);
		Settings.cameraShake = (RBPrefs.GetInt("CameraShake", 1) == 1);
		Settings.useXInput = (RBPrefs.GetInt("UseXInput", 1) == 1);
		Settings.Language = RBPrefs.GetString("NewLanguage", LocalizationManager.CurrentLanguage);
	}

	// Token: 0x06001D00 RID: 7424 RVA: 0x000BE3B4 File Offset: 0x000BC5B4
	public static void SaveSettings()
	{
		RBPrefs.SetInt("ResolutionHeight", Settings.resolution.height);
		RBPrefs.SetInt("ResolutionWidth", Settings.resolution.width);
		RBPrefs.SetInt("RefreshRate", Settings.resolution.refreshRate);
		RBPrefs.SetInt("Fullscreen", (int)Settings.windowMode);
		RBPrefs.SetInt("VSyncCount", (int)Settings.vSyncCount);
		RBPrefs.SetInt("TargetFrameRate", Settings.targetFrameRate);
		RBPrefs.SetInt("AmbientOcclusion", (int)Settings.AmbientOcclusion);
		RBPrefs.SetInt("AntiAliasingType", (int)Settings.antiAliasing);
		RBPrefs.SetInt("Bloom", (int)Settings.bloom);
		RBPrefs.SetInt("Shadows", (int)Settings.shadows);
		RBPrefs.SetInt("ShadowResolution", (int)Settings.shadowResolution);
		RBPrefs.SetFloat("ShadowDistance", Settings.shadowDistance);
		RBPrefs.SetInt("TextureRes", Settings.textureRes);
		RBPrefs.SetInt("ParticleQuality", (int)Settings.particleQuality);
		RBPrefs.SetInt("DetailQuality", (int)Settings.detailQuality);
		RBPrefs.SetFloat("MasterVolume", Settings.masterVolume);
		RBPrefs.SetFloat("MusicVolume", Settings.musicVolume);
		RBPrefs.SetFloat("EffectsVolume", Settings.effectsVolume);
		RBPrefs.SetInt("BloodEffects", Settings.bloodEffects ? 1 : 0);
		RBPrefs.SetInt("ControllerRumble", Settings.controllerRumble ? 1 : 0);
		RBPrefs.SetInt("ChatEnabled", Settings.chatEnabled ? 1 : 0);
		RBPrefs.SetInt("CameraShake", Settings.cameraShake ? 1 : 0);
		RBPrefs.SetInt("UseXInput", Settings.useXInput ? 1 : 0);
		RBPrefs.SetString("NewLanguage", Settings.language);
		RBPrefs.Save();
	}

	// Token: 0x06001D01 RID: 7425 RVA: 0x000BE55C File Offset: 0x000BC75C
	public static void RestoreGraphicsDefaults()
	{
		Settings.resolution = default(Resolution);
		Settings.resolution.height = Screen.currentResolution.height;
		Settings.resolution.width = Screen.currentResolution.width;
		Settings.resolution.refreshRate = Screen.currentResolution.refreshRate;
		Settings.windowMode = WindowMode.FullScreen;
		Settings.vSyncCount = VSyncCount.On;
		Settings.targetFrameRate = 200;
		Settings.ambientOcclusion = SettingsAmbientOcclusionQuality.Ultra;
		Settings.antiAliasing = AntiAliasingType.SMAA;
		Settings.bloom = BloomQuality.Enabled;
		Settings.shadows = ShadowQuality.All;
		Settings.shadowResolution = ShadowResolution.VeryHigh;
		Settings.shadowDistance = 300f;
		Settings.textureRes = 0;
		Settings.particleQuality = ParticleQuality.High;
		Settings.detailQuality = DetailQuality.High;
	}

	// Token: 0x06001D02 RID: 7426 RVA: 0x000155D7 File Offset: 0x000137D7
	public static void RestoreSoundDefaults()
	{
		Settings.masterVolume = 0.5f;
		Settings.musicVolume = 0.5f;
		Settings.effectsVolume = 0.5f;
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x000155F7 File Offset: 0x000137F7
	public static void RestoreGeneralDefaults()
	{
		Settings.bloodEffects = true;
		Settings.controllerRumble = true;
		Settings.chatEnabled = true;
		Settings.cameraShake = true;
		Settings.UseXInput = true;
		Settings.Language = LocalizationManager.GetCurrentDeviceLanguage();
	}

	// Token: 0x06001D04 RID: 7428 RVA: 0x000BE60C File Offset: 0x000BC80C
	public static void SaveNames()
	{
		string text = "";
		for (int i = 0; i < Settings.names.Count; i++)
		{
			text += Settings.names[i];
			if (i != Settings.names.Count - 1)
			{
				text += ",";
			}
		}
		RBPrefs.SetString("PlayerNames", text);
		RBPrefs.Save();
	}

	// Token: 0x06001D05 RID: 7429 RVA: 0x000BE674 File Offset: 0x000BC874
	public static void SetNames(List<Name> newNames)
	{
		Settings.names.Clear();
		for (int i = 8; i < newNames.Count; i++)
		{
			Settings.names.Add(newNames[i].name);
		}
		Settings.SaveNames();
	}

	// Token: 0x04001F41 RID: 8001
	private static Resolution resolution;

	// Token: 0x04001F42 RID: 8002
	private static WindowMode windowMode;

	// Token: 0x04001F43 RID: 8003
	private static int displayNum;

	// Token: 0x04001F44 RID: 8004
	private static VSyncCount vSyncCount;

	// Token: 0x04001F45 RID: 8005
	private static int targetFrameRate;

	// Token: 0x04001F46 RID: 8006
	public static Settings.EffectsChange OnEffectsChange;

	// Token: 0x04001F47 RID: 8007
	private static SettingsAmbientOcclusionQuality ambientOcclusion;

	// Token: 0x04001F48 RID: 8008
	private static AntiAliasingType antiAliasing;

	// Token: 0x04001F49 RID: 8009
	private static BloomQuality bloom;

	// Token: 0x04001F4A RID: 8010
	private static ShadowQuality shadows;

	// Token: 0x04001F4B RID: 8011
	private static ShadowResolution shadowResolution;

	// Token: 0x04001F4C RID: 8012
	private static float shadowDistance;

	// Token: 0x04001F4D RID: 8013
	private static int textureRes;

	// Token: 0x04001F4E RID: 8014
	private static ParticleQuality particleQuality;

	// Token: 0x04001F4F RID: 8015
	private static DetailQuality detailQuality;

	// Token: 0x04001F50 RID: 8016
	private static float masterVolume;

	// Token: 0x04001F51 RID: 8017
	private static float musicVolume;

	// Token: 0x04001F52 RID: 8018
	private static float effectsVolume;

	// Token: 0x04001F53 RID: 8019
	private static List<string> names;

	// Token: 0x04001F54 RID: 8020
	private static bool bloodEffects;

	// Token: 0x04001F55 RID: 8021
	private static bool controllerRumble;

	// Token: 0x04001F56 RID: 8022
	private static bool chatEnabled;

	// Token: 0x04001F57 RID: 8023
	private static bool cameraShake;

	// Token: 0x04001F58 RID: 8024
	private static bool useXInput = true;

	// Token: 0x04001F59 RID: 8025
	private static string language;

	// Token: 0x0200040B RID: 1035
	// (Invoke) Token: 0x06001D07 RID: 7431
	public delegate void EffectsChange();
}
