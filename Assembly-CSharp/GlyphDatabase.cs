using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Data.Mapping;
using UnityEngine;

// Token: 0x020004F9 RID: 1273
public class GlyphDatabase : ScriptableObject
{
	// Token: 0x06002180 RID: 8576 RVA: 0x0001846E File Offset: 0x0001666E
	public GlyphDatabase.GlyphDetails GetMouseGlyph(MouseInputElement mouseInputElement)
	{
		return this.mouseGlyphs[(int)mouseInputElement].glyphDetails;
	}

	// Token: 0x06002181 RID: 8577 RVA: 0x000CF3B0 File Offset: 0x000CD5B0
	public GlyphDatabase.GlyphDetails GetKeyboardGlyph(KeyboardKeyCode keyCode)
	{
		foreach (GlyphDatabase.KeyboardGlyph keyboardGlyph in this.keyboardGlyphs)
		{
			if (keyboardGlyph.keyboardKeyCode == keyCode)
			{
				return keyboardGlyph.glyphDetails;
			}
		}
		return this.defaultGlyph;
	}

	// Token: 0x06002182 RID: 8578 RVA: 0x000CF3EC File Offset: 0x000CD5EC
	public GlyphDatabase.GlyphDetails GetGlyph(Guid joystickGuid, int elementIdentifierId, AxisRange axisRange)
	{
		for (int i = 0; i < this.controllers.Length; i++)
		{
			if (this.controllers[i] != null && !(this.controllers[i].joystick == null) && !(this.controllers[i].joystick.Guid != joystickGuid))
			{
				return this.controllers[i].GetGlyph(elementIdentifierId, axisRange);
			}
		}
		return this.controllers[0].GetGlyph(elementIdentifierId, axisRange);
	}

	// Token: 0x04002429 RID: 9257
	[SerializeField]
	public GlyphDatabase.GlyphDetails defaultGlyph;

	// Token: 0x0400242A RID: 9258
	[SerializeField]
	public Texture mouseAtlas;

	// Token: 0x0400242B RID: 9259
	[SerializeField]
	public GlyphDatabase.MouseGlyph[] mouseGlyphs;

	// Token: 0x0400242C RID: 9260
	public bool mouseGlyphsFolded;

	// Token: 0x0400242D RID: 9261
	[SerializeField]
	public Texture keyboardAtlas;

	// Token: 0x0400242E RID: 9262
	[SerializeField]
	public GlyphDatabase.KeyboardGlyph[] keyboardGlyphs;

	// Token: 0x0400242F RID: 9263
	public bool keyboardGlyphsFolded;

	// Token: 0x04002430 RID: 9264
	[SerializeField]
	public GlyphDatabase.ControllerEntry[] controllers;

	// Token: 0x04002431 RID: 9265
	public static GlyphDatabase Instance;

	// Token: 0x04002432 RID: 9266
	public static Guid XB1ControllerGUID = new Guid("19002688-7406-4f4a-8340-8d25335406c8");

	// Token: 0x04002433 RID: 9267
	public static Guid PS4ControllerGUID = new Guid("cd9718bf-a87a-44bc-8716-60a0def28a9f");

	// Token: 0x020004FA RID: 1274
	[Serializable]
	public class ControllerEntry
	{
		// Token: 0x06002185 RID: 8581 RVA: 0x000CF464 File Offset: 0x000CD664
		public GlyphDatabase.GlyphDetails GetGlyph(int elementIdentifierId, AxisRange axisRange)
		{
			if (this.glyphs == null)
			{
				return null;
			}
			for (int i = 0; i < this.glyphs.Count; i++)
			{
				if (this.glyphs[i] != null && this.glyphs[i].elementIdentifierId == elementIdentifierId)
				{
					return this.glyphs[i].GetGlyph(axisRange);
				}
			}
			return GlyphDatabase.Instance.defaultGlyph;
		}

		// Token: 0x04002434 RID: 9268
		public bool isFolded;

		// Token: 0x04002435 RID: 9269
		public HardwareJoystickMap joystick;

		// Token: 0x04002436 RID: 9270
		public List<GlyphDatabase.GlyphEntry> glyphs = new List<GlyphDatabase.GlyphEntry>();
	}

	// Token: 0x020004FB RID: 1275
	[Serializable]
	public class GlyphEntry
	{
		// Token: 0x06002187 RID: 8583 RVA: 0x000184B0 File Offset: 0x000166B0
		public GlyphEntry(string name, int elementIdentifierId)
		{
			this.name = name;
			this.elementIdentifierId = elementIdentifierId;
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x000CF4D0 File Offset: 0x000CD6D0
		public GlyphDatabase.GlyphDetails GetGlyph(AxisRange axisRange)
		{
			switch (axisRange)
			{
			case AxisRange.Full:
				return new GlyphDatabase.GlyphDetails(this.glyph, this.color);
			case AxisRange.Positive:
				return new GlyphDatabase.GlyphDetails((this.glyphPos != null) ? this.glyphPos : this.glyph, this.color);
			case AxisRange.Negative:
				return new GlyphDatabase.GlyphDetails((this.glyphNeg != null) ? this.glyphNeg : this.glyph, this.color);
			default:
				return GlyphDatabase.Instance.defaultGlyph;
			}
		}

		// Token: 0x04002437 RID: 9271
		public bool isFolded;

		// Token: 0x04002438 RID: 9272
		public string name;

		// Token: 0x04002439 RID: 9273
		public int elementIdentifierId;

		// Token: 0x0400243A RID: 9274
		public Color color = new Color(1f, 1f, 1f, 1f);

		// Token: 0x0400243B RID: 9275
		public Sprite glyph;

		// Token: 0x0400243C RID: 9276
		public Sprite glyphPos;

		// Token: 0x0400243D RID: 9277
		public Sprite glyphNeg;
	}

	// Token: 0x020004FC RID: 1276
	[Serializable]
	public class GlyphDetails
	{
		// Token: 0x06002189 RID: 8585 RVA: 0x000184E5 File Offset: 0x000166E5
		public GlyphDetails(Sprite glyph, Color color)
		{
			this.glyph = glyph;
			this.color = color;
		}

		// Token: 0x0400243E RID: 9278
		public Sprite glyph;

		// Token: 0x0400243F RID: 9279
		public Color color = Color.white;
	}

	// Token: 0x020004FD RID: 1277
	[Serializable]
	public class KeyboardGlyph
	{
		// Token: 0x0600218A RID: 8586 RVA: 0x00018506 File Offset: 0x00016706
		public KeyboardGlyph(KeyboardKeyCode keyboardKeyCode)
		{
			this.keyboardKeyCode = keyboardKeyCode;
		}

		// Token: 0x04002440 RID: 9280
		public bool isFolded;

		// Token: 0x04002441 RID: 9281
		public KeyboardKeyCode keyboardKeyCode;

		// Token: 0x04002442 RID: 9282
		public GlyphDatabase.GlyphDetails glyphDetails;
	}

	// Token: 0x020004FE RID: 1278
	[Serializable]
	public class MouseGlyph
	{
		// Token: 0x0600218B RID: 8587 RVA: 0x00018515 File Offset: 0x00016715
		public MouseGlyph(MouseInputElement mouseInputElement)
		{
			this.mouseInputElement = mouseInputElement;
		}

		// Token: 0x04002443 RID: 9283
		public bool isFolded;

		// Token: 0x04002444 RID: 9284
		public MouseInputElement mouseInputElement;

		// Token: 0x04002445 RID: 9285
		public GlyphDatabase.GlyphDetails glyphDetails;
	}
}
