using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020006AA RID: 1706
	[AddComponentMenu("")]
	[RequireComponent(typeof(CanvasGroup))]
	public class Window : MonoBehaviour
	{
		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x0600313D RID: 12605 RVA: 0x000215ED File Offset: 0x0001F7ED
		public bool hasFocus
		{
			get
			{
				return this._isFocusedCallback != null && this._isFocusedCallback(this._id);
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x0600313E RID: 12606 RVA: 0x0002160A File Offset: 0x0001F80A
		public int id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x0600313F RID: 12607 RVA: 0x00021612 File Offset: 0x0001F812
		public RectTransform rectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = base.gameObject.GetComponent<RectTransform>();
				}
				return this._rectTransform;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06003140 RID: 12608 RVA: 0x00021639 File Offset: 0x0001F839
		public Text titleText
		{
			get
			{
				return this._titleText;
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06003141 RID: 12609 RVA: 0x00021641 File Offset: 0x0001F841
		public List<Text> contentText
		{
			get
			{
				return this._contentText;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06003142 RID: 12610 RVA: 0x00021649 File Offset: 0x0001F849
		// (set) Token: 0x06003143 RID: 12611 RVA: 0x00021651 File Offset: 0x0001F851
		public GameObject defaultUIElement
		{
			get
			{
				return this._defaultUIElement;
			}
			set
			{
				this._defaultUIElement = value;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06003144 RID: 12612 RVA: 0x0002165A File Offset: 0x0001F85A
		// (set) Token: 0x06003145 RID: 12613 RVA: 0x00021662 File Offset: 0x0001F862
		public Action<int> updateCallback
		{
			get
			{
				return this._updateCallback;
			}
			set
			{
				this._updateCallback = value;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06003146 RID: 12614 RVA: 0x0002166B File Offset: 0x0001F86B
		public Window.Timer timer
		{
			get
			{
				return this._timer;
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06003147 RID: 12615 RVA: 0x00021673 File Offset: 0x0001F873
		// (set) Token: 0x06003148 RID: 12616 RVA: 0x00102FAC File Offset: 0x001011AC
		public int width
		{
			get
			{
				return (int)this.rectTransform.sizeDelta.x;
			}
			set
			{
				Vector2 sizeDelta = this.rectTransform.sizeDelta;
				sizeDelta.x = (float)value;
				this.rectTransform.sizeDelta = sizeDelta;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06003149 RID: 12617 RVA: 0x00021686 File Offset: 0x0001F886
		// (set) Token: 0x0600314A RID: 12618 RVA: 0x00102FDC File Offset: 0x001011DC
		public int height
		{
			get
			{
				return (int)this.rectTransform.sizeDelta.y;
			}
			set
			{
				Vector2 sizeDelta = this.rectTransform.sizeDelta;
				sizeDelta.y = (float)value;
				this.rectTransform.sizeDelta = sizeDelta;
			}
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x0600314B RID: 12619 RVA: 0x00021699 File Offset: 0x0001F899
		protected bool initialized
		{
			get
			{
				return this._initialized;
			}
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x000216A1 File Offset: 0x0001F8A1
		private void OnEnable()
		{
			base.StartCoroutine("OnEnableAsync");
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x000216AF File Offset: 0x0001F8AF
		protected virtual void Update()
		{
			if (!this._initialized)
			{
				return;
			}
			if (!this.hasFocus)
			{
				return;
			}
			this.CheckUISelection();
			if (this._updateCallback != null)
			{
				this._updateCallback(this._id);
			}
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x0010300C File Offset: 0x0010120C
		public virtual void Initialize(int id, Func<int, bool> isFocusedCallback)
		{
			if (this._initialized)
			{
				Debug.LogError("Window is already initialized!");
				return;
			}
			this._id = id;
			this._isFocusedCallback = isFocusedCallback;
			this._timer = new Window.Timer();
			this._contentText = new List<Text>();
			this._canvasGroup = base.GetComponent<CanvasGroup>();
			this._initialized = true;
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x000216E2 File Offset: 0x0001F8E2
		public void SetSize(int width, int height)
		{
			this.rectTransform.sizeDelta = new Vector2((float)width, (float)height);
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x000216F8 File Offset: 0x0001F8F8
		public void CreateTitleText(GameObject prefab, Vector2 offset)
		{
			this.CreateText(prefab, ref this._titleText, "Title Text", UIPivot.TopCenter, UIAnchor.TopHStretch, offset);
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x00021717 File Offset: 0x0001F917
		public void CreateTitleText(GameObject prefab, Vector2 offset, string text)
		{
			this.CreateTitleText(prefab, offset);
			this.SetTitleText(text);
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x00103064 File Offset: 0x00101264
		public void AddContentText(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			Text item = null;
			this.CreateText(prefab, ref item, "Content Text", pivot, anchor, offset);
			this._contentText.Add(item);
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x00021728 File Offset: 0x0001F928
		public void AddContentText(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset, string text)
		{
			this.AddContentText(prefab, pivot, anchor, offset);
			this.SetContentText(text, this._contentText.Count - 1);
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x0002174A File Offset: 0x0001F94A
		public void AddContentImage(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			this.CreateImage(prefab, "Image", pivot, anchor, offset);
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x0002175C File Offset: 0x0001F95C
		public void AddContentImage(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset, string text)
		{
			this.AddContentImage(prefab, pivot, anchor, offset);
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x00103094 File Offset: 0x00101294
		public void CreateButton(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset, string buttonText, UnityAction confirmCallback, UnityAction cancelCallback, bool setDefault)
		{
			if (prefab == null)
			{
				return;
			}
			ButtonInfo buttonInfo;
			GameObject gameObject = this.CreateButton(prefab, "Button", anchor, pivot, offset, out buttonInfo);
			if (gameObject == null)
			{
				return;
			}
			Button component = gameObject.GetComponent<Button>();
			if (confirmCallback != null)
			{
				component.onClick.AddListener(confirmCallback);
			}
			CustomButton customButton = component as CustomButton;
			if (cancelCallback != null && customButton != null)
			{
				customButton.CancelEvent += cancelCallback;
			}
			if (buttonInfo.text != null)
			{
				buttonInfo.text.text = buttonText;
			}
			if (setDefault)
			{
				this._defaultUIElement = gameObject;
			}
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x00021769 File Offset: 0x0001F969
		public string GetTitleText(string text)
		{
			if (this._titleText == null)
			{
				return string.Empty;
			}
			return this._titleText.text;
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x0002178A File Offset: 0x0001F98A
		public void SetTitleText(string text)
		{
			if (this._titleText == null)
			{
				return;
			}
			this._titleText.text = text;
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x00103124 File Offset: 0x00101324
		public string GetContentText(int index)
		{
			if (this._contentText == null || this._contentText.Count <= index || this._contentText[index] == null)
			{
				return string.Empty;
			}
			return this._contentText[index].text;
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x00103174 File Offset: 0x00101374
		public float GetContentTextHeight(int index)
		{
			if (this._contentText == null || this._contentText.Count <= index || this._contentText[index] == null)
			{
				return 0f;
			}
			return this._contentText[index].rectTransform.sizeDelta.y;
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x000217A7 File Offset: 0x0001F9A7
		public void SetContentText(string text, int index)
		{
			if (this._contentText == null || this._contentText.Count <= index || this._contentText[index] == null)
			{
				return;
			}
			this._contentText[index].text = text;
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x000217E6 File Offset: 0x0001F9E6
		public void SetUpdateCallback(Action<int> callback)
		{
			this.updateCallback = callback;
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x000217EF File Offset: 0x0001F9EF
		public virtual void TakeInputFocus()
		{
			if (EventSystem.current == null)
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(this._defaultUIElement);
			this.Enable();
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x00021815 File Offset: 0x0001FA15
		public virtual void Enable()
		{
			this._canvasGroup.interactable = true;
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x00021823 File Offset: 0x0001FA23
		public virtual void Disable()
		{
			this._canvasGroup.interactable = false;
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x00021831 File Offset: 0x0001FA31
		public virtual void Cancel()
		{
			if (!this.initialized)
			{
				return;
			}
			if (this.cancelCallback != null)
			{
				this.cancelCallback();
			}
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x001031CC File Offset: 0x001013CC
		private void CreateText(GameObject prefab, ref Text textComponent, string name, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			if (prefab == null || this.content == null)
			{
				return;
			}
			if (textComponent != null)
			{
				Debug.LogError("Window already has " + name + "!");
				return;
			}
			GameObject gameObject = UITools.InstantiateGUIObject<Text>(prefab, this.content.transform, name, pivot, anchor.min, anchor.max, offset);
			if (gameObject == null)
			{
				return;
			}
			textComponent = gameObject.GetComponent<Text>();
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x00103250 File Offset: 0x00101450
		private void CreateImage(GameObject prefab, string name, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			if (prefab == null || this.content == null)
			{
				return;
			}
			UITools.InstantiateGUIObject<Image>(prefab, this.content.transform, name, pivot, anchor.min, anchor.max, offset);
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x001032A0 File Offset: 0x001014A0
		private GameObject CreateButton(GameObject prefab, string name, UIAnchor anchor, UIPivot pivot, Vector2 offset, out ButtonInfo buttonInfo)
		{
			buttonInfo = null;
			if (prefab == null)
			{
				return null;
			}
			GameObject gameObject = UITools.InstantiateGUIObject<ButtonInfo>(prefab, this.content.transform, name, pivot, anchor.min, anchor.max, offset);
			if (gameObject == null)
			{
				return null;
			}
			buttonInfo = gameObject.GetComponent<ButtonInfo>();
			if (gameObject.GetComponent<Button>() == null)
			{
				Debug.Log("Button prefab is missing Button component!");
				return null;
			}
			if (buttonInfo == null)
			{
				Debug.Log("Button prefab is missing ButtonInfo component!");
				return null;
			}
			return gameObject;
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x0002184F File Offset: 0x0001FA4F
		private IEnumerator OnEnableAsync()
		{
			yield return 1;
			if (EventSystem.current == null)
			{
				yield break;
			}
			if (this.defaultUIElement != null)
			{
				EventSystem.current.SetSelectedGameObject(this.defaultUIElement);
			}
			else
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
			yield break;
		}

		// Token: 0x06003165 RID: 12645 RVA: 0x0010332C File Offset: 0x0010152C
		private void CheckUISelection()
		{
			if (!this.hasFocus)
			{
				return;
			}
			if (EventSystem.current == null)
			{
				return;
			}
			if (EventSystem.current.currentSelectedGameObject == null)
			{
				this.RestoreDefaultOrLastUISelection();
			}
			this.lastUISelection = EventSystem.current.currentSelectedGameObject;
		}

		// Token: 0x06003166 RID: 12646 RVA: 0x0002185E File Offset: 0x0001FA5E
		private void RestoreDefaultOrLastUISelection()
		{
			if (!this.hasFocus)
			{
				return;
			}
			if (this.lastUISelection == null || !this.lastUISelection.activeInHierarchy)
			{
				this.SetUISelection(this._defaultUIElement);
				return;
			}
			this.SetUISelection(this.lastUISelection);
		}

		// Token: 0x06003167 RID: 12647 RVA: 0x0001F3A8 File Offset: 0x0001D5A8
		private void SetUISelection(GameObject selection)
		{
			if (EventSystem.current == null)
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(selection);
		}

		// Token: 0x0400304F RID: 12367
		public Image backgroundImage;

		// Token: 0x04003050 RID: 12368
		public GameObject content;

		// Token: 0x04003051 RID: 12369
		private bool _initialized;

		// Token: 0x04003052 RID: 12370
		private int _id = -1;

		// Token: 0x04003053 RID: 12371
		private RectTransform _rectTransform;

		// Token: 0x04003054 RID: 12372
		private Text _titleText;

		// Token: 0x04003055 RID: 12373
		private List<Text> _contentText;

		// Token: 0x04003056 RID: 12374
		private GameObject _defaultUIElement;

		// Token: 0x04003057 RID: 12375
		private Action<int> _updateCallback;

		// Token: 0x04003058 RID: 12376
		private Func<int, bool> _isFocusedCallback;

		// Token: 0x04003059 RID: 12377
		private Window.Timer _timer;

		// Token: 0x0400305A RID: 12378
		private CanvasGroup _canvasGroup;

		// Token: 0x0400305B RID: 12379
		public UnityAction cancelCallback;

		// Token: 0x0400305C RID: 12380
		private GameObject lastUISelection;

		// Token: 0x020006AB RID: 1707
		public class Timer
		{
			// Token: 0x170008B1 RID: 2225
			// (get) Token: 0x06003169 RID: 12649 RVA: 0x000218AC File Offset: 0x0001FAAC
			public bool started
			{
				get
				{
					return this._started;
				}
			}

			// Token: 0x170008B2 RID: 2226
			// (get) Token: 0x0600316A RID: 12650 RVA: 0x000218B4 File Offset: 0x0001FAB4
			public bool finished
			{
				get
				{
					if (!this.started)
					{
						return false;
					}
					if (Time.realtimeSinceStartup < this.end)
					{
						return false;
					}
					this._started = false;
					return true;
				}
			}

			// Token: 0x170008B3 RID: 2227
			// (get) Token: 0x0600316B RID: 12651 RVA: 0x000218D7 File Offset: 0x0001FAD7
			public float remaining
			{
				get
				{
					if (!this._started)
					{
						return 0f;
					}
					return this.end - Time.realtimeSinceStartup;
				}
			}

			// Token: 0x0600316D RID: 12653 RVA: 0x000218F3 File Offset: 0x0001FAF3
			public void Start(float length)
			{
				this.end = Time.realtimeSinceStartup + length;
				this._started = true;
			}

			// Token: 0x0600316E RID: 12654 RVA: 0x00021909 File Offset: 0x0001FB09
			public void Stop()
			{
				this._started = false;
			}

			// Token: 0x0400305D RID: 12381
			private bool _started;

			// Token: 0x0400305E RID: 12382
			private float end;
		}
	}
}
