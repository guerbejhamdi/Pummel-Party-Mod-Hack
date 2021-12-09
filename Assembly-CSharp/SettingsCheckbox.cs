using System;
using LlockhamIndustries.Decals;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000499 RID: 1177
public class SettingsCheckbox : SettingField
{
	// Token: 0x06001F7B RID: 8059 RVA: 0x000C7F38 File Offset: 0x000C6138
	public override void Setup()
	{
		switch (this.fieldType)
		{
		case FieldType.BloodEffects:
			this.toggle.isOn = Settings.BloodEffects;
			break;
		case FieldType.ControllerRumble:
			this.toggle.isOn = Settings.ControllerRumble;
			break;
		case FieldType.ChatEnabled:
			this.toggle.isOn = Settings.ChatEnabled;
			break;
		case FieldType.CameraShake:
			this.toggle.isOn = Settings.CameraShake;
			break;
		case FieldType.UseXInput:
			this.toggle.isOn = Settings.UseXInput;
			break;
		}
		if (this.initialized)
		{
			return;
		}
		GameObject gameObject = base.transform.Find("RowBackground").gameObject;
		GameObject gameObject2 = base.transform.Find("Toggle").gameObject;
		this.rowBackground = gameObject.GetComponent<Image>();
		this.baseColor = this.rowBackground.color;
		EventTrigger component = gameObject.GetComponent<EventTrigger>();
		EventTrigger eventTrigger = gameObject2.AddComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener(delegate(BaseEventData data)
		{
			this.OnPointerEnter();
		});
		EventTrigger.Entry entry2 = new EventTrigger.Entry();
		entry2.eventID = EventTriggerType.PointerExit;
		entry2.callback.AddListener(delegate(BaseEventData data)
		{
			this.OnPointerExit();
		});
		EventTrigger.Entry entry3 = new EventTrigger.Entry();
		entry3.eventID = EventTriggerType.Select;
		entry3.callback.AddListener(delegate(BaseEventData data)
		{
			this.OnPointerEnter();
		});
		EventTrigger.Entry entry4 = new EventTrigger.Entry();
		entry4.eventID = EventTriggerType.Deselect;
		entry4.callback.AddListener(delegate(BaseEventData data)
		{
			this.OnPointerExit();
		});
		component.triggers.Add(entry);
		component.triggers.Add(entry2);
		component.triggers.Add(entry3);
		component.triggers.Add(entry4);
		eventTrigger.triggers.Add(entry);
		eventTrigger.triggers.Add(entry2);
		eventTrigger.triggers.Add(entry3);
		this.initialized = true;
	}

	// Token: 0x06001F7C RID: 8060 RVA: 0x000C8120 File Offset: 0x000C6320
	public void OnValueChange()
	{
		switch (this.fieldType)
		{
		case FieldType.BloodEffects:
			if (!this.toggle.isOn)
			{
				ProjectionPool pool = ProjectionPool.GetPool("Orange Goo");
				while (pool.activePool.Count > 0)
				{
					pool.activePool[0].Return();
				}
			}
			Settings.BloodEffects = this.toggle.isOn;
			return;
		case FieldType.ControllerRumble:
			Settings.ControllerRumble = this.toggle.isOn;
			return;
		case FieldType.RefreshRate:
		case FieldType.Language:
			break;
		case FieldType.ChatEnabled:
			Settings.ChatEnabled = this.toggle.isOn;
			return;
		case FieldType.CameraShake:
			Settings.CameraShake = this.toggle.isOn;
			return;
		case FieldType.UseXInput:
			Settings.UseXInput = this.toggle.isOn;
			break;
		default:
			return;
		}
	}

	// Token: 0x06001F7D RID: 8061 RVA: 0x000C81E8 File Offset: 0x000C63E8
	public override void OnApply()
	{
		if (!this.initialized)
		{
			this.Setup();
		}
		switch (this.fieldType)
		{
		case FieldType.BloodEffects:
			Settings.BloodEffects = this.toggle.isOn;
			return;
		case FieldType.ControllerRumble:
			Settings.ControllerRumble = this.toggle.isOn;
			return;
		case FieldType.RefreshRate:
		case FieldType.Language:
			break;
		case FieldType.ChatEnabled:
			Settings.ChatEnabled = this.toggle.isOn;
			return;
		case FieldType.CameraShake:
			Settings.CameraShake = this.toggle.isOn;
			return;
		case FieldType.UseXInput:
			Settings.UseXInput = this.toggle.isOn;
			break;
		default:
			return;
		}
	}

	// Token: 0x06001F7E RID: 8062 RVA: 0x00017149 File Offset: 0x00015349
	public override void OnPointerEnter()
	{
		this.rowBackground.color = this.highlightColor;
		base.OnPointerEnter();
	}

	// Token: 0x06001F7F RID: 8063 RVA: 0x00017162 File Offset: 0x00015362
	public override void OnPointerExit()
	{
		this.rowBackground.color = this.baseColor;
		base.OnPointerExit();
	}

	// Token: 0x06001F80 RID: 8064 RVA: 0x0001717B File Offset: 0x0001537B
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x0400225A RID: 8794
	public Toggle toggle;

	// Token: 0x0400225B RID: 8795
	private Image rowBackground;

	// Token: 0x0400225C RID: 8796
	private Color highlightColor = new Color32(25, 25, 25, 140);

	// Token: 0x0400225D RID: 8797
	private Color baseColor = new Color32(0, 0, 0, 96);
}
