using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TMPro
{
	// Token: 0x020005AC RID: 1452
	public class TMP_TextEventHandler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x060025B2 RID: 9650 RVA: 0x0001AF46 File Offset: 0x00019146
		// (set) Token: 0x060025B3 RID: 9651 RVA: 0x0001AF4E File Offset: 0x0001914E
		public TMP_TextEventHandler.CharacterSelectionEvent onCharacterSelection
		{
			get
			{
				return this.m_OnCharacterSelection;
			}
			set
			{
				this.m_OnCharacterSelection = value;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x060025B4 RID: 9652 RVA: 0x0001AF57 File Offset: 0x00019157
		// (set) Token: 0x060025B5 RID: 9653 RVA: 0x0001AF5F File Offset: 0x0001915F
		public TMP_TextEventHandler.SpriteSelectionEvent onSpriteSelection
		{
			get
			{
				return this.m_OnSpriteSelection;
			}
			set
			{
				this.m_OnSpriteSelection = value;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x060025B6 RID: 9654 RVA: 0x0001AF68 File Offset: 0x00019168
		// (set) Token: 0x060025B7 RID: 9655 RVA: 0x0001AF70 File Offset: 0x00019170
		public TMP_TextEventHandler.WordSelectionEvent onWordSelection
		{
			get
			{
				return this.m_OnWordSelection;
			}
			set
			{
				this.m_OnWordSelection = value;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x060025B8 RID: 9656 RVA: 0x0001AF79 File Offset: 0x00019179
		// (set) Token: 0x060025B9 RID: 9657 RVA: 0x0001AF81 File Offset: 0x00019181
		public TMP_TextEventHandler.LineSelectionEvent onLineSelection
		{
			get
			{
				return this.m_OnLineSelection;
			}
			set
			{
				this.m_OnLineSelection = value;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x060025BA RID: 9658 RVA: 0x0001AF8A File Offset: 0x0001918A
		// (set) Token: 0x060025BB RID: 9659 RVA: 0x0001AF92 File Offset: 0x00019192
		public TMP_TextEventHandler.LinkSelectionEvent onLinkSelection
		{
			get
			{
				return this.m_OnLinkSelection;
			}
			set
			{
				this.m_OnLinkSelection = value;
			}
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x000E3974 File Offset: 0x000E1B74
		private void Awake()
		{
			this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
			if (this.m_TextComponent.GetType() == typeof(TextMeshProUGUI))
			{
				this.m_Canvas = base.gameObject.GetComponentInParent<Canvas>();
				if (this.m_Canvas != null)
				{
					if (this.m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
					{
						this.m_Camera = null;
						return;
					}
					this.m_Camera = this.m_Canvas.worldCamera;
					return;
				}
			}
			else
			{
				this.m_Camera = Camera.main;
			}
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x000E3A00 File Offset: 0x000E1C00
		private void LateUpdate()
		{
			if (TMP_TextUtilities.IsIntersectingRectTransform(this.m_TextComponent.rectTransform, Input.mousePosition, this.m_Camera))
			{
				int num = TMP_TextUtilities.FindIntersectingCharacter(this.m_TextComponent, Input.mousePosition, this.m_Camera, true);
				if (num != -1 && num != this.m_lastCharIndex)
				{
					this.m_lastCharIndex = num;
					TMP_TextElementType elementType = this.m_TextComponent.textInfo.characterInfo[num].elementType;
					if (elementType == TMP_TextElementType.Character)
					{
						this.SendOnCharacterSelection(this.m_TextComponent.textInfo.characterInfo[num].character, num);
					}
					else if (elementType == TMP_TextElementType.Sprite)
					{
						this.SendOnSpriteSelection(this.m_TextComponent.textInfo.characterInfo[num].character, num);
					}
				}
				int num2 = TMP_TextUtilities.FindIntersectingWord(this.m_TextComponent, Input.mousePosition, this.m_Camera);
				if (num2 != -1 && num2 != this.m_lastWordIndex)
				{
					this.m_lastWordIndex = num2;
					TMP_WordInfo tmp_WordInfo = this.m_TextComponent.textInfo.wordInfo[num2];
					this.SendOnWordSelection(tmp_WordInfo.GetWord(), tmp_WordInfo.firstCharacterIndex, tmp_WordInfo.characterCount);
				}
				int num3 = TMP_TextUtilities.FindIntersectingLine(this.m_TextComponent, Input.mousePosition, this.m_Camera);
				if (num3 != -1 && num3 != this.m_lastLineIndex)
				{
					this.m_lastLineIndex = num3;
					TMP_LineInfo tmp_LineInfo = this.m_TextComponent.textInfo.lineInfo[num3];
					char[] array = new char[tmp_LineInfo.characterCount];
					int num4 = 0;
					while (num4 < tmp_LineInfo.characterCount && num4 < this.m_TextComponent.textInfo.characterInfo.Length)
					{
						array[num4] = this.m_TextComponent.textInfo.characterInfo[num4 + tmp_LineInfo.firstCharacterIndex].character;
						num4++;
					}
					string line = new string(array);
					this.SendOnLineSelection(line, tmp_LineInfo.firstCharacterIndex, tmp_LineInfo.characterCount);
				}
				int num5 = TMP_TextUtilities.FindIntersectingLink(this.m_TextComponent, Input.mousePosition, this.m_Camera);
				if (num5 != -1 && num5 != this.m_selectedLink)
				{
					this.m_selectedLink = num5;
					TMP_LinkInfo tmp_LinkInfo = this.m_TextComponent.textInfo.linkInfo[num5];
					this.SendOnLinkSelection(tmp_LinkInfo.GetLinkID(), tmp_LinkInfo.GetLinkText(), num5);
					return;
				}
			}
			else
			{
				this.m_selectedLink = -1;
				this.m_lastCharIndex = -1;
				this.m_lastWordIndex = -1;
				this.m_lastLineIndex = -1;
			}
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x0000398C File Offset: 0x00001B8C
		public void OnPointerEnter(PointerEventData eventData)
		{
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x0000398C File Offset: 0x00001B8C
		public void OnPointerExit(PointerEventData eventData)
		{
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x0001AF9B File Offset: 0x0001919B
		private void SendOnCharacterSelection(char character, int characterIndex)
		{
			if (this.onCharacterSelection != null)
			{
				this.onCharacterSelection.Invoke(character, characterIndex);
			}
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x0001AFB2 File Offset: 0x000191B2
		private void SendOnSpriteSelection(char character, int characterIndex)
		{
			if (this.onSpriteSelection != null)
			{
				this.onSpriteSelection.Invoke(character, characterIndex);
			}
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x0001AFC9 File Offset: 0x000191C9
		private void SendOnWordSelection(string word, int charIndex, int length)
		{
			if (this.onWordSelection != null)
			{
				this.onWordSelection.Invoke(word, charIndex, length);
			}
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x0001AFE1 File Offset: 0x000191E1
		private void SendOnLineSelection(string line, int charIndex, int length)
		{
			if (this.onLineSelection != null)
			{
				this.onLineSelection.Invoke(line, charIndex, length);
			}
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x0001AFF9 File Offset: 0x000191F9
		private void SendOnLinkSelection(string linkID, string linkText, int linkIndex)
		{
			if (this.onLinkSelection != null)
			{
				this.onLinkSelection.Invoke(linkID, linkText, linkIndex);
			}
		}

		// Token: 0x0400292A RID: 10538
		[SerializeField]
		private TMP_TextEventHandler.CharacterSelectionEvent m_OnCharacterSelection = new TMP_TextEventHandler.CharacterSelectionEvent();

		// Token: 0x0400292B RID: 10539
		[SerializeField]
		private TMP_TextEventHandler.SpriteSelectionEvent m_OnSpriteSelection = new TMP_TextEventHandler.SpriteSelectionEvent();

		// Token: 0x0400292C RID: 10540
		[SerializeField]
		private TMP_TextEventHandler.WordSelectionEvent m_OnWordSelection = new TMP_TextEventHandler.WordSelectionEvent();

		// Token: 0x0400292D RID: 10541
		[SerializeField]
		private TMP_TextEventHandler.LineSelectionEvent m_OnLineSelection = new TMP_TextEventHandler.LineSelectionEvent();

		// Token: 0x0400292E RID: 10542
		[SerializeField]
		private TMP_TextEventHandler.LinkSelectionEvent m_OnLinkSelection = new TMP_TextEventHandler.LinkSelectionEvent();

		// Token: 0x0400292F RID: 10543
		private TMP_Text m_TextComponent;

		// Token: 0x04002930 RID: 10544
		private Camera m_Camera;

		// Token: 0x04002931 RID: 10545
		private Canvas m_Canvas;

		// Token: 0x04002932 RID: 10546
		private int m_selectedLink = -1;

		// Token: 0x04002933 RID: 10547
		private int m_lastCharIndex = -1;

		// Token: 0x04002934 RID: 10548
		private int m_lastWordIndex = -1;

		// Token: 0x04002935 RID: 10549
		private int m_lastLineIndex = -1;

		// Token: 0x020005AD RID: 1453
		[Serializable]
		public class CharacterSelectionEvent : UnityEvent<char, int>
		{
		}

		// Token: 0x020005AE RID: 1454
		[Serializable]
		public class SpriteSelectionEvent : UnityEvent<char, int>
		{
		}

		// Token: 0x020005AF RID: 1455
		[Serializable]
		public class WordSelectionEvent : UnityEvent<string, int, int>
		{
		}

		// Token: 0x020005B0 RID: 1456
		[Serializable]
		public class LineSelectionEvent : UnityEvent<string, int, int>
		{
		}

		// Token: 0x020005B1 RID: 1457
		[Serializable]
		public class LinkSelectionEvent : UnityEvent<string, string, int>
		{
		}
	}
}
