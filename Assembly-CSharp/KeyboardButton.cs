using System;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000508 RID: 1288
public class KeyboardButton : MonoBehaviour
{
	// Token: 0x060021B9 RID: 8633 RVA: 0x0001875A File Offset: 0x0001695A
	public void Setup(KeyboardKeyCode keyCode, UIMultiplayerLobbySlot slot)
	{
		this.keyCode = keyCode;
		this.slot = slot;
		this.glyph.sprite = GlyphDatabase.Instance.GetKeyboardGlyph(keyCode).glyph;
	}

	// Token: 0x060021BA RID: 8634 RVA: 0x00018785 File Offset: 0x00016985
	public void OnClick()
	{
		this.slot.KeyboardPress(this.keyCode);
	}

	// Token: 0x04002487 RID: 9351
	public Image glyph;

	// Token: 0x04002488 RID: 9352
	private KeyboardKeyCode keyCode;

	// Token: 0x04002489 RID: 9353
	private UIMultiplayerLobbySlot slot;
}
