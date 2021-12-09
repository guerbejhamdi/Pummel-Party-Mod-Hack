using System;
using System.Collections;
using System.Collections.Generic;
using Rewired.UI.ControlMapper;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000514 RID: 1300
public class MainMenuWindow : UIBehaviour
{
	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x060021E4 RID: 8676 RVA: 0x00018914 File Offset: 0x00016B14
	// (set) Token: 0x060021E5 RID: 8677 RVA: 0x0001891C File Offset: 0x00016B1C
	protected MainMenuWindowState CurState { get; set; }

	// Token: 0x060021E6 RID: 8678 RVA: 0x0000398C File Offset: 0x00001B8C
	protected override void Awake()
	{
	}

	// Token: 0x060021E7 RID: 8679 RVA: 0x00018925 File Offset: 0x00016B25
	protected override void Start()
	{
		this.Initialize();
		base.Start();
	}

	// Token: 0x060021E8 RID: 8680 RVA: 0x00018933 File Offset: 0x00016B33
	protected override void OnEnable()
	{
		this.Initialize();
		base.OnEnable();
	}

	// Token: 0x060021E9 RID: 8681 RVA: 0x000D0B9C File Offset: 0x000CED9C
	protected virtual void Initialize()
	{
		if (this.CurState == MainMenuWindowState.Uninitialized)
		{
			if (!base.transform.gameObject.activeSelf)
			{
				base.transform.gameObject.SetActive(true);
			}
			if (base.transform.parent != null)
			{
				this.parentWindow = base.transform.parent.GetComponentInParent<MainMenuWindow>();
			}
			this.canvas_group = base.GetComponent<CanvasGroup>();
			this.animator = base.GetComponent<Animator>();
			if (!this.customTransition && this.animator != null)
			{
				UnityEngine.Object.Destroy(this.animator);
			}
			if (this.firstObject != null)
			{
				this.firstObjectSelectable = this.firstObject.GetComponent<Selectable>();
			}
			this.UpdateSelectables();
			this.CurState = MainMenuWindowState.Initialized;
			if (this.InitiallyDisabled)
			{
				this.SetState(MainMenuWindowState.Hidden);
				this.canvas_group.alpha = 0f;
				return;
			}
			this.SetState(MainMenuWindowState.Visible);
		}
	}

	// Token: 0x060021EA RID: 8682 RVA: 0x00018941 File Offset: 0x00016B41
	public void UpdateSelectables()
	{
		this.ui_elements = new List<Selectable>();
		this.FindSelectables(base.transform);
	}

	// Token: 0x060021EB RID: 8683 RVA: 0x000D0C8C File Offset: 0x000CEE8C
	private void FindSelectables(Transform t)
	{
		Selectable component = t.GetComponent<Selectable>();
		if (component != null && component.EventSystemID == this.EventSystemID)
		{
			this.ui_elements.Add(component);
		}
		if (t.GetComponent<MainMenuWindow>() == null || t == base.transform)
		{
			for (int i = 0; i < t.childCount; i++)
			{
				this.FindSelectables(t.GetChild(i));
			}
		}
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x0001895A File Offset: 0x00016B5A
	public virtual void TransitionIn()
	{
		this.Fade(1f);
	}

	// Token: 0x060021ED RID: 8685 RVA: 0x00018967 File Offset: 0x00016B67
	public virtual void TransitionOut()
	{
		this.Fade(0f);
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x000D0D00 File Offset: 0x000CEF00
	public void Update()
	{
		if (this.customTransition)
		{
			this.animator.SetBool("Hidden", this.CurState == MainMenuWindowState.Hidden);
		}
		if (this.rememberLastSelection && this.EventSystem != null)
		{
			GameObject currentSelectedGameObject = this.EventSystem.currentSelectedGameObject;
			if (this.CurState == MainMenuWindowState.Visible && currentSelectedGameObject != null)
			{
				if (currentSelectedGameObject != this.lastTested)
				{
					Selectable component = currentSelectedGameObject.GetComponent<Selectable>();
					if (this.ui_elements.Contains(component) && component.navigation.mode != Navigation.Mode.None && component.IsInteractable() && currentSelectedGameObject.activeInHierarchy)
					{
						this.lastSelected = component;
					}
				}
				this.lastTested = currentSelectedGameObject;
			}
		}
	}

	// Token: 0x060021EF RID: 8687 RVA: 0x00018974 File Offset: 0x00016B74
	public void SetState(MainMenuWindowState newState)
	{
		this.Initialize();
		if (newState != this.CurState)
		{
			switch (newState)
			{
			case MainMenuWindowState.Visible:
				this.ShowWindow();
				return;
			case MainMenuWindowState.Hidden:
				this.HideWindow();
				return;
			case MainMenuWindowState.Disabled:
				this.FadeWindow();
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x060021F0 RID: 8688 RVA: 0x000D0DB4 File Offset: 0x000CEFB4
	private void ShowWindow()
	{
		switch (this.CurState)
		{
		case MainMenuWindowState.Initialized:
		case MainMenuWindowState.Disabled:
			if (this.CurState == MainMenuWindowState.Disabled)
			{
				this.TransitionIn();
			}
			this.canvas_group.interactable = true;
			this.canvas_group.blocksRaycasts = true;
			this.CurState = MainMenuWindowState.Visible;
			break;
		case MainMenuWindowState.Visible:
			break;
		case MainMenuWindowState.Hidden:
			if (this.showWindowDelayCoroutine != null)
			{
				base.StopCoroutine(this.showWindowDelayCoroutine);
			}
			this.showWindowDelayCoroutine = base.StartCoroutine(this.ShowWindowDelay());
			return;
		default:
			return;
		}
	}

	// Token: 0x060021F1 RID: 8689 RVA: 0x000189AE File Offset: 0x00016BAE
	private IEnumerator ShowWindowDelay()
	{
		this.CurState = MainMenuWindowState.Visible;
		this.TransitionIn();
		yield return new WaitForSeconds(0.01f);
		this.canvas_group.interactable = true;
		this.canvas_group.blocksRaycasts = true;
		yield return new WaitForSeconds(0.15f);
		if (this.EventSystem != null)
		{
			this.lastWindow = ControllerFocus.CurWindow(this.EventSystemID);
			ControllerFocus.AddWindow(this.EventSystemID, this);
		}
		yield break;
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x000D0E38 File Offset: 0x000CF038
	private void HideWindow()
	{
		if (this.CurState != MainMenuWindowState.Initialized)
		{
			if (this.showWindowDelayCoroutine != null)
			{
				base.StopCoroutine(this.showWindowDelayCoroutine);
			}
			if (this.EventSystem != null && this.EventSystem.currentSelectedGameObject != null && this.ui_elements.Contains(this.EventSystem.currentSelectedGameObject.GetComponent<Selectable>()))
			{
				this.EventSystem.SetSelectedGameObject(null);
			}
			this.TransitionOut();
			ControllerFocus.RemoveWindow(this.EventSystemID, this);
			this.canvas_group.interactable = false;
			this.canvas_group.blocksRaycasts = false;
		}
		else
		{
			base.StartCoroutine(this.DelayCanvasInteractableState());
		}
		this.CurState = MainMenuWindowState.Hidden;
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x000189BD File Offset: 0x00016BBD
	private IEnumerator DelayCanvasInteractableState()
	{
		yield return null;
		this.canvas_group.interactable = false;
		this.canvas_group.blocksRaycasts = false;
		yield break;
	}

	// Token: 0x060021F4 RID: 8692 RVA: 0x000189CC File Offset: 0x00016BCC
	protected void Fade(float alpha)
	{
		if (this.lt != null)
		{
			LeanTween.cancel(base.gameObject);
			this.lt = null;
		}
		this.lt = LeanTween.alphaCanvas(this.canvas_group, alpha, 0.1666f).setDelay(0.001f);
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x000D0EF0 File Offset: 0x000CF0F0
	private void DisableChildren()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			GameObject gameObject = base.transform.GetChild(i).gameObject;
			if (gameObject.activeSelf)
			{
				this.DisabledObjects.Add(gameObject);
				gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060021F6 RID: 8694 RVA: 0x000D0F40 File Offset: 0x000CF140
	private void EnableChildren()
	{
		for (int i = 0; i < this.DisabledObjects.Count; i++)
		{
			this.DisabledObjects[i].SetActive(true);
		}
		this.DisabledObjects.Clear();
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x00018A09 File Offset: 0x00016C09
	private IEnumerator SetPreviousWindowDelay()
	{
		yield return new WaitForSeconds(0.1f);
		ControllerFocus.RemoveWindow(this.EventSystemID, this);
		yield break;
	}

	// Token: 0x060021F8 RID: 8696 RVA: 0x00018A18 File Offset: 0x00016C18
	private void FadeWindow()
	{
		this.canvas_group.interactable = false;
		this.canvas_group.blocksRaycasts = false;
		this.CurState = MainMenuWindowState.Disabled;
	}

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x060021F9 RID: 8697 RVA: 0x00018A39 File Offset: 0x00016C39
	public bool Hidden
	{
		get
		{
			return this.CurState == MainMenuWindowState.Hidden || this.CurState == MainMenuWindowState.Uninitialized;
		}
	}

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x060021FA RID: 8698 RVA: 0x00018A4F File Offset: 0x00016C4F
	public bool Interactable
	{
		get
		{
			return this.InteractableInHierarchy();
		}
	}

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x060021FB RID: 8699 RVA: 0x00018A57 File Offset: 0x00016C57
	public bool Visible
	{
		get
		{
			return this.CurState == MainMenuWindowState.Visible && (this.parentWindow == null || this.parentWindow.Visible);
		}
	}

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x060021FC RID: 8700 RVA: 0x00018A7F File Offset: 0x00016C7F
	public bool Disabled
	{
		get
		{
			return this.CurState == MainMenuWindowState.Disabled || (this.parentWindow != null && this.parentWindow.Disabled);
		}
	}

	// Token: 0x060021FD RID: 8701 RVA: 0x000D0F80 File Offset: 0x000CF180
	private bool InteractableInHierarchy()
	{
		MainMenuWindow mainMenuWindow = null;
		if (base.transform.parent != null)
		{
			mainMenuWindow = base.transform.parent.gameObject.GetComponentInParent<MainMenuWindow>();
		}
		if (mainMenuWindow != null && mainMenuWindow != this)
		{
			return mainMenuWindow.Interactable && this.canvas_group.interactable;
		}
		return this.canvas_group.interactable;
	}

	// Token: 0x060021FE RID: 8702 RVA: 0x000D0FEC File Offset: 0x000CF1EC
	public GameObject GetSelectable()
	{
		if (this.rememberLastSelection && this.lastSelected != null && this.lastSelected.interactable)
		{
			return this.lastSelected.gameObject;
		}
		if (this.firstObject != null && this.firstObjectSelectable != null && this.firstObjectSelectable.interactable && this.firstObjectSelectable.enabled && this.firstObject.activeInHierarchy)
		{
			return this.firstObject;
		}
		if (this.dynamicUpdateSelectables)
		{
			this.UpdateSelectables();
		}
		List<Selectable> list = new List<Selectable>(this.ui_elements);
		list.RemoveAll((Selectable m) => !m.interactable || !m.gameObject.activeInHierarchy || m == null || m.navigation.mode == Navigation.Mode.None || !m.enabled);
		if (list.Count == 0)
		{
			return null;
		}
		Selectable selectable;
		if (list.Count == 1)
		{
			selectable = list[0];
		}
		else
		{
			selectable = UISelectionUtility.FindNextSelectable(null, base.transform, list, (Vector2.down + Vector2.right).normalized);
		}
		if (selectable != null)
		{
			return selectable.gameObject;
		}
		return list[0].gameObject;
	}

	// Token: 0x060021FF RID: 8703 RVA: 0x00018AA7 File Offset: 0x00016CA7
	protected override void OnDestroy()
	{
		ControllerFocus.RemoveWindow(this.EventSystemID, this);
	}

	// Token: 0x0400250B RID: 9483
	public bool InitiallyDisabled;

	// Token: 0x0400250C RID: 9484
	public GameObject firstObject;

	// Token: 0x0400250D RID: 9485
	public bool autoFindFirstSelection = true;

	// Token: 0x0400250E RID: 9486
	public bool rememberLastSelection = true;

	// Token: 0x0400250F RID: 9487
	public bool dynamicUpdateSelectables;

	// Token: 0x04002510 RID: 9488
	public bool ignoreAsCurrent;

	// Token: 0x04002511 RID: 9489
	public bool customTransition;

	// Token: 0x04002512 RID: 9490
	private Animator animator;

	// Token: 0x04002513 RID: 9491
	private List<Selectable> ui_elements;

	// Token: 0x04002514 RID: 9492
	protected CanvasGroup canvas_group;

	// Token: 0x04002515 RID: 9493
	private MainMenuWindow parentWindow;

	// Token: 0x04002516 RID: 9494
	private Selectable lastSelected;

	// Token: 0x04002517 RID: 9495
	private GameObject lastTested;

	// Token: 0x04002518 RID: 9496
	private Selectable firstObjectSelectable;

	// Token: 0x04002519 RID: 9497
	private MainMenuWindow lastWindow;

	// Token: 0x0400251B RID: 9499
	public List<GameObject> DisabledObjects = new List<GameObject>();

	// Token: 0x0400251C RID: 9500
	private Coroutine showWindowDelayCoroutine;

	// Token: 0x0400251D RID: 9501
	private LTDescr lt;
}
