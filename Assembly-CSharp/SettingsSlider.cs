using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200052C RID: 1324
public class SettingsSlider : SettingField
{
	// Token: 0x060022DA RID: 8922 RVA: 0x000D44E0 File Offset: 0x000D26E0
	public override void Setup()
	{
		FieldType fieldType = this.fieldType;
		switch (fieldType)
		{
		case FieldType.MasterVolume:
			this.slider.value = Settings.MasterVolume;
			break;
		case FieldType.MusicVolume:
			this.slider.value = Settings.MusicVolume;
			break;
		case FieldType.EffectsVolume:
			this.slider.value = Settings.EffectsVolume;
			break;
		default:
			if (fieldType == FieldType.TargetFrameRate)
			{
				this.slider.value = (float)Settings.TargetFrameRate;
				if (this.inputField != null)
				{
					this.inputField.text = Settings.TargetFrameRate.ToString();
				}
			}
			break;
		}
		if (this.initialized)
		{
			return;
		}
		GameObject gameObject = base.transform.Find("RowBackground").gameObject;
		GameObject gameObject2 = base.transform.Find("Slider").gameObject;
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
		eventTrigger.triggers.Add(entry4);
		this.initialized = true;
	}

	// Token: 0x060022DB RID: 8923 RVA: 0x000D46E0 File Offset: 0x000D28E0
	public void OnValueChange()
	{
		FieldType fieldType = this.fieldType;
		switch (fieldType)
		{
		case FieldType.MasterVolume:
			AudioSystem.MasterVolume = this.slider.value;
			return;
		case FieldType.MusicVolume:
			AudioSystem.MusicVolume = this.slider.value;
			return;
		case FieldType.EffectsVolume:
			AudioSystem.EffectsVolume = this.slider.value;
			return;
		default:
			if (fieldType != FieldType.TargetFrameRate)
			{
				return;
			}
			Application.targetFrameRate = (int)this.slider.value;
			this.inputField.text = Application.targetFrameRate.ToString();
			return;
		}
	}

	// Token: 0x060022DC RID: 8924 RVA: 0x000D476C File Offset: 0x000D296C
	public void OnInputFieldEndEdit()
	{
		if (this.fieldType == FieldType.TargetFrameRate)
		{
			int value = 0;
			if (int.TryParse(this.inputField.text, out value))
			{
				Application.targetFrameRate = Mathf.Clamp(value, 10, 200);
				this.inputField.text = Application.targetFrameRate.ToString();
				this.slider.value = (float)Application.targetFrameRate;
				return;
			}
			this.inputField.text = Application.targetFrameRate.ToString();
		}
	}

	// Token: 0x060022DD RID: 8925 RVA: 0x000D47F0 File Offset: 0x000D29F0
	public override void OnApply()
	{
		if (!this.initialized)
		{
			this.Setup();
		}
		FieldType fieldType = this.fieldType;
		switch (fieldType)
		{
		case FieldType.MasterVolume:
			Settings.MasterVolume = this.slider.value;
			return;
		case FieldType.MusicVolume:
			Settings.MusicVolume = this.slider.value;
			return;
		case FieldType.EffectsVolume:
			Settings.EffectsVolume = this.slider.value;
			return;
		default:
			if (fieldType != FieldType.TargetFrameRate)
			{
				return;
			}
			Settings.TargetFrameRate = (int)this.slider.value;
			return;
		}
	}

	// Token: 0x060022DE RID: 8926 RVA: 0x00019251 File Offset: 0x00017451
	public override void OnPointerEnter()
	{
		this.rowBackground.color = this.highlightColor;
		base.OnPointerEnter();
	}

	// Token: 0x060022DF RID: 8927 RVA: 0x0001926A File Offset: 0x0001746A
	public override void OnPointerExit()
	{
		this.rowBackground.color = this.baseColor;
		base.OnPointerExit();
	}

	// Token: 0x060022E0 RID: 8928 RVA: 0x0001717B File Offset: 0x0001537B
	public override void Update()
	{
		base.Update();
	}

	// Token: 0x040025BE RID: 9662
	public Slider slider;

	// Token: 0x040025BF RID: 9663
	public InputField inputField;

	// Token: 0x040025C0 RID: 9664
	private Image rowBackground;

	// Token: 0x040025C1 RID: 9665
	private Color highlightColor = new Color32(25, 25, 25, 140);

	// Token: 0x040025C2 RID: 9666
	private Color baseColor = new Color32(0, 0, 0, 96);
}
