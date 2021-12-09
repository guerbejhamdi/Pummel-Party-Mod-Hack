using System;
using System.Collections;
using I2.Loc;
using Rewired;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020004EC RID: 1260
public class BasicButtonBase : UIBehaviour
{
	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x06002130 RID: 8496 RVA: 0x000180A1 File Offset: 0x000162A1
	public BasicButtonBase.BasicButtonState CurState
	{
		get
		{
			return this.curState;
		}
	}

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x06002131 RID: 8497 RVA: 0x000180A9 File Offset: 0x000162A9
	// (set) Token: 0x06002132 RID: 8498 RVA: 0x000CDFF8 File Offset: 0x000CC1F8
	public int ActionID
	{
		get
		{
			return this.actionID;
		}
		set
		{
			this.actionID = value;
			if (this.uIGlyph != null)
			{
				this.uIGlyph.inputDetails.keyboardActionID = this.actionID;
				this.uIGlyph.inputDetails.joystickActionID = this.actionID;
				this.uIGlyph.ForceUpdateGlyph();
			}
		}
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x000CE054 File Offset: 0x000CC254
	private void OnXInputChange()
	{
		Player player = ReInput.players.GetPlayer(0);
		if (this.uIGlyph != null)
		{
			this.uIGlyph.OnXInputChanged();
		}
		this.SetPlayer(player, player == null);
	}

	// Token: 0x06002134 RID: 8500 RVA: 0x000CE094 File Offset: 0x000CC294
	protected override void Start()
	{
		GameManager.OnXInput = (GameManager.OnXInputEvent)Delegate.Combine(GameManager.OnXInput, new GameManager.OnXInputEvent(this.OnXInputChange));
		this.text = base.GetComponentInChildren<Text>();
		if (this.text != null)
		{
			this.localize = this.text.gameObject.GetComponent<Localize>();
			if (this.localize != null)
			{
				this.startTerm = this.localize.Term;
			}
		}
		if (this.uIGlyph != null && this.actionID == -1)
		{
			this.ActionID = (string.IsNullOrEmpty(this.actionName) ? -1 : ReInput.mapping.GetActionId(this.actionName));
		}
		if (ReInput.isReady)
		{
			Player player = ReInput.players.GetPlayer(0);
			this.SetPlayer(player, player == null);
		}
		if (this.button == null)
		{
			this.button = base.GetComponentInChildren<Button>();
		}
		if (this.button != null)
		{
			this.eventTrigger = this.button.gameObject.GetComponent<EventTrigger>();
			if (this.eventTrigger == null)
			{
				this.eventTrigger = this.button.gameObject.AddComponent<EventTrigger>();
			}
			if (this.eventTrigger != null)
			{
				this.AddEventTrigger(new UnityAction<BaseEventData>(this.OnClick), EventTriggerType.PointerClick);
				this.AddEventTrigger(new UnityAction(this.OnSubmit), EventTriggerType.Submit);
			}
		}
		this.mainmenuWindow = base.GetComponentInParent<MainMenuWindow>();
		base.Start();
		if (this.tween && this.button != null && this.eventTrigger != null)
		{
			this.AddEventTrigger(new UnityAction<BaseEventData>(this.Deselect), EventTriggerType.Deselect);
			this.AddEventTrigger(new UnityAction<BaseEventData>(this.Deselect), EventTriggerType.PointerExit);
			this.AddEventTrigger(new UnityAction<BaseEventData>(this.Select), EventTriggerType.Select);
			this.AddEventTrigger(new UnityAction<BaseEventData>(this.Select), EventTriggerType.PointerEnter);
		}
	}

	// Token: 0x06002135 RID: 8501 RVA: 0x000CE288 File Offset: 0x000CC488
	public virtual void Update()
	{
		if (this.hold && this.holdImage != null)
		{
			this.holdImage.fillAmount = Mathf.Clamp01((float)this.player.GetButtonTimePressed(this.actionID) / this.holdTime);
		}
		if (this.tween && this.button != null && this.button.IsInteractable() != this.lastInteractable)
		{
			this.lastInteractable = this.button.IsInteractable();
			if (this.lastInteractable)
			{
				this.OnInteractable();
			}
			else
			{
				this.OnNotInteractable();
			}
		}
		if (this.pollAction && this.curState != BasicButtonBase.BasicButtonState.Disabled && (this.mainmenuWindow == null || this.mainmenuWindow.Visible) && (!this.pollPadOnly || this.lastControllerType == ControllerType.Joystick) && (this.button == null || (this.button != null && this.button.IsInteractable())) && this.actionID != -1 && this.GetButton())
		{
			this.OnSubmit();
		}
	}

	// Token: 0x06002136 RID: 8502 RVA: 0x000180B1 File Offset: 0x000162B1
	private bool GetButton()
	{
		if (!this.hold)
		{
			return this.player.GetButtonUp(this.actionID);
		}
		return this.player.GetButtonTimedPressDown(this.actionID, this.holdTime);
	}

	// Token: 0x06002137 RID: 8503 RVA: 0x000180E4 File Offset: 0x000162E4
	private void Deselect(BaseEventData d)
	{
		this.selected = false;
		if (this.button != null && !this.button.IsInteractable())
		{
			return;
		}
		this.SetStates(this.baseScale, this.baseColor, this.baseTextColor);
	}

	// Token: 0x06002138 RID: 8504 RVA: 0x00018121 File Offset: 0x00016321
	private void Select(BaseEventData d)
	{
		if (this.button != null && !this.button.IsInteractable())
		{
			return;
		}
		this.selected = true;
		this.SetStates(this.scale, this.highlightedColor, this.highlightedColor);
	}

	// Token: 0x06002139 RID: 8505 RVA: 0x0001815E File Offset: 0x0001635E
	protected virtual void OnInteractable()
	{
		if (this.selected)
		{
			return;
		}
		this.SetStates(this.baseScale, this.baseColor, this.baseTextColor);
	}

	// Token: 0x0600213A RID: 8506 RVA: 0x00018181 File Offset: 0x00016381
	protected virtual void OnNotInteractable()
	{
		this.SetStates(this.baseScale, this.disabledColor, this.disabledTextColor);
	}

	// Token: 0x0600213B RID: 8507 RVA: 0x000CE3A4 File Offset: 0x000CC5A4
	private void SetStates(Vector3 scale, Color outlineColor, Color textColor)
	{
		this.CancelTween();
		LeanTween.scale(this.controllerTransform, scale, 0.1f).setEase(LeanTweenType.easeInOutSine);
		LeanTween.color(this.outline, outlineColor, 0.1f).setEase(LeanTweenType.easeInOutSine).setRecursive(false);
		LeanTween.colorText(this.buttonText, textColor, 0.1f).setEase(LeanTweenType.easeInOutSine).setRecursive(false);
	}

	// Token: 0x0600213C RID: 8508 RVA: 0x0001819B File Offset: 0x0001639B
	private void CancelTween()
	{
		LeanTween.cancel(this.controllerTransform);
		LeanTween.cancel(this.outline);
		LeanTween.cancel(this.buttonText);
	}

	// Token: 0x0600213D RID: 8509 RVA: 0x0000398C File Offset: 0x00001B8C
	protected override void OnDisable()
	{
	}

	// Token: 0x0600213E RID: 8510 RVA: 0x000CE410 File Offset: 0x000CC610
	protected override void OnDestroy()
	{
		if (this.player != null)
		{
			this.player.controllers.RemoveLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.ControllerChanged));
		}
		base.OnDestroy();
		GameManager.OnXInput = (GameManager.OnXInputEvent)Delegate.Remove(GameManager.OnXInput, new GameManager.OnXInputEvent(this.OnXInputChange));
	}

	// Token: 0x0600213F RID: 8511 RVA: 0x000CE468 File Offset: 0x000CC668
	public virtual void SetPlayer(Player newPlayer, bool isNull)
	{
		if (!ReInput.isReady || this.player == newPlayer)
		{
			return;
		}
		if (isNull)
		{
			this.player.controllers.RemoveLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.ControllerChanged));
		}
		this.player = newPlayer;
		if (this.uIGlyph != null)
		{
			this.uIGlyph.SetPlayer(this.player);
		}
		if (isNull)
		{
			return;
		}
		this.player.controllers.AddLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.ControllerChanged));
		this.ControllerChanged(this.player, this.player.controllers.GetLastActiveController());
	}

	// Token: 0x06002140 RID: 8512 RVA: 0x000CE50C File Offset: 0x000CC70C
	public virtual void OnClick(BaseEventData d)
	{
		if (this.curState != BasicButtonBase.BasicButtonState.Disabled && this.CompareEventSystemID(d) && this.button != null && this.button.IsInteractable() && this.EventSystem != null)
		{
			this.EventSystem.SetSelectedGameObject(null);
			this.OnSubmit();
		}
	}

	// Token: 0x06002141 RID: 8513 RVA: 0x000181BE File Offset: 0x000163BE
	public virtual void OnSubmit()
	{
		AudioSystem.PlayOneShot("ButtonPress01_SFXR", 1f, 0f);
	}

	// Token: 0x06002142 RID: 8514 RVA: 0x000181D4 File Offset: 0x000163D4
	protected override void OnEnable()
	{
		this.SetControllerType(this.lastControllerType);
		base.OnEnable();
	}

	// Token: 0x06002143 RID: 8515 RVA: 0x000CE568 File Offset: 0x000CC768
	private string GetHeirarchyString()
	{
		Transform transform = base.transform;
		string text = base.gameObject.name;
		while (transform.parent != null)
		{
			text = transform.parent.gameObject.name + "/" + text;
			transform = transform.parent;
		}
		return text;
	}

	// Token: 0x06002144 RID: 8516 RVA: 0x000CE5BC File Offset: 0x000CC7BC
	protected virtual void ControllerChanged(Player player, Controller controller)
	{
		if (this == null)
		{
			return;
		}
		ControllerType controllerType = (controller != null) ? controller.type : ((player.id == 0) ? ControllerType.Mouse : ControllerType.Joystick);
		this.SetControllerType(controllerType);
	}

	// Token: 0x06002145 RID: 8517 RVA: 0x000CE5F4 File Offset: 0x000CC7F4
	private void SetControllerType(ControllerType controllerType)
	{
		if (this.keyboard == null || this.controller == null)
		{
			if (this.animator != null && this.animator.isInitialized && this.uIGlyph != null && this.animator.isActiveAndEnabled)
			{
				this.animator.SetBool("Joystick", controllerType == ControllerType.Joystick);
			}
		}
		else
		{
			this.keyboard.SetActive(controllerType != ControllerType.Joystick);
			this.controller.SetActive(controllerType == ControllerType.Joystick);
		}
		this.lastControllerType = controllerType;
	}

	// Token: 0x06002146 RID: 8518 RVA: 0x000CE694 File Offset: 0x000CC894
	public void SetState(BasicButtonBase.BasicButtonState state)
	{
		if (this.curState == state)
		{
			return;
		}
		BasicButtonBase.BasicButtonElements[] array = this.disableElements;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetState(state);
		}
		if (this.button != null)
		{
			if (state != BasicButtonBase.BasicButtonState.Enabled)
			{
				if (state == BasicButtonBase.BasicButtonState.Disabled)
				{
					if (this.localize != null)
					{
						this.localize.Term = this.disabledTerm;
					}
					if (this.EventSystem != null && this.EventSystem.currentSelectedGameObject == this.button.gameObject)
					{
						this.EventSystem.SetSelectedGameObject(null);
					}
					this.button.interactable = false;
				}
			}
			else
			{
				this.button.interactable = true;
				if (this.localize != null)
				{
					this.localize.Term = this.startTerm;
				}
			}
		}
		if (this.uIGlyph != null)
		{
			this.uIGlyph.Enabled = (state == BasicButtonBase.BasicButtonState.Enabled);
		}
		this.curState = state;
	}

	// Token: 0x06002147 RID: 8519 RVA: 0x000181E8 File Offset: 0x000163E8
	private IEnumerator Fade(bool fadeIn)
	{
		float startTime = Time.time;
		float fadeTime = 0.5f;
		while (Time.time - startTime < fadeTime)
		{
			foreach (BasicButtonBase.BasicButtonElements basicButtonElements in this.disableElements)
			{
				float num = (Time.time - startTime) / fadeTime;
				if (fadeIn)
				{
					num = 1f - num;
				}
				Color color = Color.Lerp(basicButtonElements.disableColor, basicButtonElements.imageOriginalColor, num);
				basicButtonElements.image.color = color;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002148 RID: 8520 RVA: 0x000CE79C File Offset: 0x000CC99C
	private void AddEventTrigger(UnityAction action, EventTriggerType triggerType)
	{
		EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
		triggerEvent.AddListener(delegate(BaseEventData eventData)
		{
			action();
		});
		EventTrigger.Entry item = new EventTrigger.Entry
		{
			callback = triggerEvent,
			eventID = triggerType
		};
		this.eventTrigger.triggers.Add(item);
	}

	// Token: 0x06002149 RID: 8521 RVA: 0x000CE7F4 File Offset: 0x000CC9F4
	protected void AddEventTrigger(UnityAction<BaseEventData> action, EventTriggerType triggerType)
	{
		EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
		triggerEvent.AddListener(delegate(BaseEventData eventData)
		{
			action(eventData);
		});
		EventTrigger.Entry item = new EventTrigger.Entry
		{
			callback = triggerEvent,
			eventID = triggerType
		};
		this.eventTrigger.triggers.Add(item);
	}

	// Token: 0x040023DF RID: 9183
	[Header("Variables")]
	public string actionName = "";

	// Token: 0x040023E0 RID: 9184
	public int actionID = -1;

	// Token: 0x040023E1 RID: 9185
	public bool hold;

	// Token: 0x040023E2 RID: 9186
	public float holdTime = 2f;

	// Token: 0x040023E3 RID: 9187
	public bool pollAction;

	// Token: 0x040023E4 RID: 9188
	public bool pollPadOnly;

	// Token: 0x040023E5 RID: 9189
	public BasicButtonBase.BasicButtonElements[] disableElements;

	// Token: 0x040023E6 RID: 9190
	public string disabledTerm;

	// Token: 0x040023E7 RID: 9191
	[Header("References")]
	public UIGlyph uIGlyph;

	// Token: 0x040023E8 RID: 9192
	public Animator animator;

	// Token: 0x040023E9 RID: 9193
	public Button button;

	// Token: 0x040023EA RID: 9194
	public GameObject controller;

	// Token: 0x040023EB RID: 9195
	public GameObject keyboard;

	// Token: 0x040023EC RID: 9196
	[Header("Animation")]
	public RectTransform outline;

	// Token: 0x040023ED RID: 9197
	public RectTransform buttonText;

	// Token: 0x040023EE RID: 9198
	public RectTransform controllerTransform;

	// Token: 0x040023EF RID: 9199
	public Color highlightedColor = new Color(1f, 0.694929f, 0.3088235f);

	// Token: 0x040023F0 RID: 9200
	public Color highlightedTextColor = new Color(1f, 0.694929f, 0.3088235f);

	// Token: 0x040023F1 RID: 9201
	public Color baseColor = new Color(0.4352941f, 0.4352941f, 0.4352941f, 0.827451f);

	// Token: 0x040023F2 RID: 9202
	public Color baseTextColor = new Color(1f, 1f, 1f, 1f);

	// Token: 0x040023F3 RID: 9203
	public Color disabledColor = new Color(0.5220588f, 0.5220588f, 0.5220588f, 0.566f);

	// Token: 0x040023F4 RID: 9204
	public Color disabledTextColor = new Color(0.4852941f, 0.4852941f, 0.4852941f, 0.553f);

	// Token: 0x040023F5 RID: 9205
	public bool tween;

	// Token: 0x040023F6 RID: 9206
	public Image holdImage;

	// Token: 0x040023F7 RID: 9207
	protected Player player;

	// Token: 0x040023F8 RID: 9208
	protected MainMenuWindow mainmenuWindow;

	// Token: 0x040023F9 RID: 9209
	protected EventTrigger eventTrigger;

	// Token: 0x040023FA RID: 9210
	protected BasicButtonBase.BasicButtonState curState;

	// Token: 0x040023FB RID: 9211
	protected string startTerm;

	// Token: 0x040023FC RID: 9212
	protected Text text;

	// Token: 0x040023FD RID: 9213
	protected Localize localize;

	// Token: 0x040023FE RID: 9214
	protected ControllerType lastControllerType = ControllerType.Mouse;

	// Token: 0x040023FF RID: 9215
	private bool lastInteractable;

	// Token: 0x04002400 RID: 9216
	private bool selected;

	// Token: 0x04002401 RID: 9217
	private Vector3 scale = new Vector3(1.05f, 1f, 1f);

	// Token: 0x04002402 RID: 9218
	private Vector3 baseScale = new Vector3(1f, 1f, 1f);

	// Token: 0x020004ED RID: 1261
	public enum BasicButtonState
	{
		// Token: 0x04002404 RID: 9220
		Enabled,
		// Token: 0x04002405 RID: 9221
		Disabled
	}

	// Token: 0x020004EE RID: 1262
	[Serializable]
	public class BasicButtonElements
	{
		// Token: 0x0600214B RID: 8523 RVA: 0x000CE968 File Offset: 0x000CCB68
		public void SetState(BasicButtonBase.BasicButtonState state)
		{
			if (state != BasicButtonBase.BasicButtonState.Enabled)
			{
				if (state == BasicButtonBase.BasicButtonState.Disabled)
				{
					if (this.image != null)
					{
						if (this.curState == BasicButtonBase.BasicButtonState.Enabled)
						{
							this.imageOriginalColor = this.image.color;
						}
						this.image.color = this.disableColor;
					}
				}
			}
			else if (this.image != null)
			{
				this.image.color = this.imageOriginalColor;
			}
			this.curState = state;
		}

		// Token: 0x04002406 RID: 9222
		public Graphic image;

		// Token: 0x04002407 RID: 9223
		public Color disableColor;

		// Token: 0x04002408 RID: 9224
		public Color imageOriginalColor;

		// Token: 0x04002409 RID: 9225
		private BasicButtonBase.BasicButtonState curState;
	}
}
