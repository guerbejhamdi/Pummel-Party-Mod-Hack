using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200052B RID: 1323
[ExecuteInEditMode]
public class SettingsButton : SettingField
{
	// Token: 0x060022C3 RID: 8899 RVA: 0x00005651 File Offset: 0x00003851
	private bool IsSettingAvailable()
	{
		return true;
	}

	// Token: 0x060022C4 RID: 8900 RVA: 0x00019131 File Offset: 0x00017331
	public void Awake()
	{
		if (!this.IsSettingAvailable())
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060022C5 RID: 8901 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x060022C6 RID: 8902 RVA: 0x000D3B7C File Offset: 0x000D1D7C
	private void GetObjects()
	{
		this.dropDown = this.controlObject.GetComponentInChildren<Dropdown>();
		this.rowOutline = this.controlObject.transform.Find("Dropdown/RowOutline").GetComponent<Image>();
		GameObject gameObject = this.controlObject.transform.Find("RowBackground").gameObject;
		this.rowBackground = gameObject.GetComponent<Image>();
		GameObject gameObject2 = this.controlObject.transform.Find("Dropdown").gameObject;
		this.rowBackground2 = gameObject2.GetComponent<Image>();
		this.trigger = gameObject.GetComponent<EventTrigger>();
		this.trigger2 = gameObject2.GetComponent<EventTrigger>();
		this.localize = this.controlObject.transform.Find("RowBackground/RowText").GetComponent<Localize>();
	}

	// Token: 0x060022C7 RID: 8903 RVA: 0x000D3C40 File Offset: 0x000D1E40
	public override void Setup()
	{
		if (!this.IsSettingAvailable())
		{
			return;
		}
		if (this.controlObject == null)
		{
			return;
		}
		if (!this.initialized)
		{
			this.initialized = true;
			if (this.isDropDown)
			{
				this.dropDown.onValueChanged.RemoveAllListeners();
				this.dropDown.onValueChanged.AddListener(delegate(int index)
				{
					this.OnDropDownChange(index);
				});
				this.baseColor = this.rowBackground.color;
				this.baseColor2 = this.rowBackground2.color;
				this.trigger.triggers.Clear();
				this.trigger2.triggers.Clear();
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.PointerEnter;
				entry.callback.RemoveAllListeners();
				entry.callback.AddListener(delegate(BaseEventData data)
				{
					this.OnPointerEnter();
				});
				EventTrigger.Entry entry2 = new EventTrigger.Entry();
				entry2.eventID = EventTriggerType.PointerExit;
				entry2.callback.RemoveAllListeners();
				entry2.callback.AddListener(delegate(BaseEventData data)
				{
					this.OnPointerExit();
				});
				EventTrigger.Entry entry3 = new EventTrigger.Entry();
				entry3.eventID = EventTriggerType.Select;
				entry3.callback.RemoveAllListeners();
				entry3.callback.AddListener(delegate(BaseEventData data)
				{
					this.OnPointerEnter();
				});
				EventTrigger.Entry entry4 = new EventTrigger.Entry();
				entry4.eventID = EventTriggerType.Deselect;
				entry4.callback.RemoveAllListeners();
				entry4.callback.AddListener(delegate(BaseEventData data)
				{
					this.OnPointerExit();
				});
				this.trigger.triggers.Add(entry);
				this.trigger2.triggers.Add(entry);
				this.trigger.triggers.Add(entry2);
				this.trigger2.triggers.Add(entry2);
				this.trigger.triggers.Add(entry3);
				this.trigger2.triggers.Add(entry3);
				this.trigger.triggers.Add(entry4);
				this.trigger2.triggers.Add(entry4);
				EventTrigger.Entry entry5 = new EventTrigger.Entry();
				entry5.eventID = EventTriggerType.Submit;
				entry5.callback.AddListener(delegate(BaseEventData data)
				{
					this.OnSbmit();
				});
				this.trigger2.triggers.Add(entry5);
			}
			this.localize.SetTerm(this.title);
		}
		switch (this.fieldType)
		{
		case FieldType.Resolution:
			this.resolutions = Screen.resolutions;
			this.settingsText = new string[this.resolutions.Length];
			this.curIndex = this.resolutions.Length - 1;
			for (int i = 0; i < this.resolutions.Length; i++)
			{
				this.settingsText[i] = this.resolutions[i].ToString();
				if (Settings.Resolution.height == this.resolutions[i].height && Settings.Resolution.width == this.resolutions[i].width && Settings.Resolution.refreshRate == this.resolutions[i].refreshRate)
				{
					this.curIndex = i;
				}
			}
			break;
		case FieldType.WindowMode:
			this.settingsText = SettingsButton.windowModeSettings;
			this.curIndex = (int)Settings.WindowMode;
			break;
		case FieldType.VSyncCount:
			this.settingsText = SettingsButton.vSyncSettings;
			for (int j = 0; j < this.vSyncCount.Length; j++)
			{
				if (Settings.VSyncCount == this.vSyncCount[j])
				{
					this.curIndex = j;
				}
			}
			break;
		case FieldType.AntiAliasing:
			this.settingsText = SettingsButton.antiAliasingText;
			this.curIndex = (int)Settings.AntiAliasing;
			break;
		case FieldType.Bloom:
			this.settingsText = SettingsButton.bloomText;
			this.curIndex = (int)Settings.Bloom;
			break;
		case FieldType.AmbientOcclusion:
			this.settingsText = SettingsButton.ambienOcclusionText;
			this.curIndex = (int)Settings.AmbientOcclusion;
			break;
		case FieldType.Shadows:
			this.settingsText = SettingsButton.shadowsText;
			this.curIndex = (int)Settings.Shadows;
			break;
		case FieldType.ShadowResolution:
			this.settingsText = SettingsButton.shadowResolutionText;
			this.curIndex = (int)Settings.ShadowResolution;
			break;
		case FieldType.Language:
		{
			int num = LocalizationManager.GetAllLanguages(true).IndexOf(Settings.Language);
			this.settingsText = LocalizationManager.GetAllLanguages(true).ToArray();
			this.curIndex = ((num == -1) ? 0 : num);
			break;
		}
		}
		if (this.curIndex == -1)
		{
			this.curIndex = 0;
		}
		this.UpdateUI();
	}

	// Token: 0x060022C8 RID: 8904 RVA: 0x000D40D4 File Offset: 0x000D22D4
	private void UpdateUI()
	{
		if (this.isDropDown)
		{
			this.dropDown.ClearOptions();
			if (this.fieldType != FieldType.Resolution && this.fieldType != FieldType.Language)
			{
				List<string> list = new List<string>();
				foreach (string text in this.settingsText)
				{
					string term = "Options_" + text;
					string item = text;
					if (LocalizationManager.TryGetTranslation(term, out item, true, 0, true, false, null, null, true))
					{
						list.Add(item);
					}
					else
					{
						list.Add(text);
					}
				}
				this.dropDown.AddOptions(list);
			}
			else
			{
				this.dropDown.AddOptions(new List<string>(this.settingsText));
			}
			this.dropDown.value = this.curIndex;
		}
		FieldType fieldType = this.fieldType;
		if (fieldType == FieldType.VSyncCount)
		{
			QualitySettings.vSyncCount = (int)this.vSyncCount[this.curIndex];
			return;
		}
		if (fieldType != FieldType.Language)
		{
			return;
		}
		LocalizationManager.CurrentLanguage = this.dropDown.options[this.curIndex].text;
	}

	// Token: 0x060022C9 RID: 8905 RVA: 0x00019147 File Offset: 0x00017347
	private void MakeDirty(int newIndex)
	{
		if (this.dirty && this.dirtyIndex == newIndex)
		{
			this.dirty = false;
			this.dirtyIndex = -1;
			return;
		}
		if (!this.dirty)
		{
			this.dirtyIndex = this.curIndex;
			this.dirty = true;
		}
	}

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x060022CA RID: 8906 RVA: 0x00019184 File Offset: 0x00017384
	public bool IsDirty
	{
		get
		{
			return this.dirty;
		}
	}

	// Token: 0x060022CB RID: 8907 RVA: 0x000D41D4 File Offset: 0x000D23D4
	public void OnDropDownChange(int index)
	{
		this.curIndex = index;
		FieldType fieldType = this.fieldType;
		if (fieldType == FieldType.VSyncCount)
		{
			QualitySettings.vSyncCount = (int)this.vSyncCount[this.curIndex];
			return;
		}
		if (fieldType != FieldType.Language)
		{
			return;
		}
		LocalizationManager.CurrentLanguage = this.dropDown.options[this.curIndex].text;
	}

	// Token: 0x060022CC RID: 8908 RVA: 0x0001918C File Offset: 0x0001738C
	public void OnLeftArrowPress()
	{
		this.MakeDirty(this.curIndex - 1);
		this.curIndex--;
		this.UpdateUI();
	}

	// Token: 0x060022CD RID: 8909 RVA: 0x000191B0 File Offset: 0x000173B0
	public void OnRightArrowPress()
	{
		this.MakeDirty(this.curIndex + 1);
		this.curIndex++;
		this.UpdateUI();
	}

	// Token: 0x060022CE RID: 8910 RVA: 0x000D422C File Offset: 0x000D242C
	public override void OnApply()
	{
		if (!this.initialized)
		{
			this.Setup();
		}
		switch (this.fieldType)
		{
		case FieldType.Resolution:
			Settings.Resolution = this.resolutions[this.curIndex];
			return;
		case FieldType.WindowMode:
			Settings.WindowMode = (WindowMode)this.curIndex;
			return;
		case FieldType.VSyncCount:
			Settings.VSyncCount = this.vSyncCount[this.curIndex];
			return;
		case FieldType.Display:
		case FieldType.MasterVolume:
		case FieldType.MusicVolume:
		case FieldType.EffectsVolume:
		case FieldType.BloodEffects:
		case FieldType.ControllerRumble:
		case FieldType.RefreshRate:
			break;
		case FieldType.AntiAliasing:
			Settings.AntiAliasing = (AntiAliasingType)this.curIndex;
			return;
		case FieldType.Bloom:
			Settings.Bloom = (BloomQuality)this.curIndex;
			return;
		case FieldType.AmbientOcclusion:
			Settings.AmbientOcclusion = (SettingsAmbientOcclusionQuality)this.curIndex;
			return;
		case FieldType.Shadows:
			Settings.Shadows = (ShadowQuality)this.curIndex;
			return;
		case FieldType.ShadowResolution:
			Settings.ShadowResolution = (ShadowResolution)this.curIndex;
			return;
		case FieldType.Language:
			Settings.Language = this.dropDown.options[this.curIndex].text;
			LocalizationManager.CurrentLanguage = this.dropDown.options[this.curIndex].text;
			break;
		default:
			return;
		}
	}

	// Token: 0x060022CF RID: 8911 RVA: 0x000191D4 File Offset: 0x000173D4
	public override void OnPointerEnter()
	{
		this.rowBackground.color = this.highlightColor;
		this.rowBackground2.color = this.highlightColor;
		this.rowOutline.enabled = true;
		base.OnPointerEnter();
	}

	// Token: 0x060022D0 RID: 8912 RVA: 0x0001920A File Offset: 0x0001740A
	public override void OnPointerExit()
	{
		this.rowBackground.color = this.baseColor;
		this.rowBackground2.color = this.baseColor2;
		this.rowOutline.enabled = false;
		base.OnPointerExit();
	}

	// Token: 0x060022D1 RID: 8913 RVA: 0x0000398C File Offset: 0x00001B8C
	public void OnSbmit()
	{
	}

	// Token: 0x040025A1 RID: 9633
	public Localize localize;

	// Token: 0x040025A2 RID: 9634
	public Texture2D[] settingPictures;

	// Token: 0x040025A3 RID: 9635
	public bool isDropDown;

	// Token: 0x040025A4 RID: 9636
	public GameObject controlObject;

	// Token: 0x040025A5 RID: 9637
	public Dropdown dropDown;

	// Token: 0x040025A6 RID: 9638
	public Image rowBackground;

	// Token: 0x040025A7 RID: 9639
	public Image rowBackground2;

	// Token: 0x040025A8 RID: 9640
	public Image rowOutline;

	// Token: 0x040025A9 RID: 9641
	public EventTrigger trigger;

	// Token: 0x040025AA RID: 9642
	public EventTrigger trigger2;

	// Token: 0x040025AB RID: 9643
	private Resolution[] resolutions;

	// Token: 0x040025AC RID: 9644
	private int[] displayNumber;

	// Token: 0x040025AD RID: 9645
	private string[] settingsText;

	// Token: 0x040025AE RID: 9646
	private string[] localizedSettingsText;

	// Token: 0x040025AF RID: 9647
	private VSyncCount[] vSyncCount = new VSyncCount[]
	{
		VSyncCount.Off,
		VSyncCount.Half,
		VSyncCount.On
	};

	// Token: 0x040025B0 RID: 9648
	private int curIndex = -1;

	// Token: 0x040025B1 RID: 9649
	private int dirtyIndex = -1;

	// Token: 0x040025B2 RID: 9650
	private bool dirty;

	// Token: 0x040025B3 RID: 9651
	private static readonly string[] windowModeSettings = new string[]
	{
		"FullScreen",
		"FullScreen Windowed",
		"Maximized Window",
		"Windowed"
	};

	// Token: 0x040025B4 RID: 9652
	private static readonly string[] vSyncSettings = new string[]
	{
		"Disabled",
		"Half",
		"Enabled"
	};

	// Token: 0x040025B5 RID: 9653
	private static readonly string[] antiAliasingText = new string[]
	{
		"Disabled",
		"FXAA",
		"SMAA",
		"TAA"
	};

	// Token: 0x040025B6 RID: 9654
	private static readonly string[] ambienOcclusionText = new string[]
	{
		"Disabled",
		"Low",
		"Medium",
		"High"
	};

	// Token: 0x040025B7 RID: 9655
	private static readonly string[] bloomText = new string[]
	{
		"Disabled",
		"Enabled"
	};

	// Token: 0x040025B8 RID: 9656
	private static readonly string[] shadowsText = new string[]
	{
		"Disabled",
		"Low",
		"High"
	};

	// Token: 0x040025B9 RID: 9657
	private static readonly string[] shadowResolutionText = new string[]
	{
		"Low",
		"Medium",
		"High",
		"Very High"
	};

	// Token: 0x040025BA RID: 9658
	private Color highlightColor = new Color32(25, 25, 25, 140);

	// Token: 0x040025BB RID: 9659
	private Color baseColor = new Color32(0, 0, 0, 96);

	// Token: 0x040025BC RID: 9660
	private Color baseColor2 = new Color32(0, 0, 0, 96);

	// Token: 0x040025BD RID: 9661
	public bool setup;
}
