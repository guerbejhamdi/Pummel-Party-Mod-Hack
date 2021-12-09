using System;
using UnityEngine;

// Token: 0x02000108 RID: 264
public class LTGUI
{
	// Token: 0x060007A3 RID: 1955 RVA: 0x0000942C File Offset: 0x0000762C
	public static void init()
	{
		if (LTGUI.levels == null)
		{
			LTGUI.levels = new LTRect[LTGUI.RECT_LEVELS * LTGUI.RECTS_PER_LEVEL];
			LTGUI.levelDepths = new int[LTGUI.RECT_LEVELS];
		}
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x0004BA18 File Offset: 0x00049C18
	public static void initRectCheck()
	{
		if (LTGUI.buttons == null)
		{
			LTGUI.buttons = new Rect[LTGUI.BUTTONS_MAX];
			LTGUI.buttonLevels = new int[LTGUI.BUTTONS_MAX];
			LTGUI.buttonLastFrame = new int[LTGUI.BUTTONS_MAX];
			for (int i = 0; i < LTGUI.buttonLevels.Length; i++)
			{
				LTGUI.buttonLevels[i] = -1;
			}
		}
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x0004BA74 File Offset: 0x00049C74
	public static void reset()
	{
		if (LTGUI.isGUIEnabled)
		{
			LTGUI.isGUIEnabled = false;
			for (int i = 0; i < LTGUI.levels.Length; i++)
			{
				LTGUI.levels[i] = null;
			}
			for (int j = 0; j < LTGUI.levelDepths.Length; j++)
			{
				LTGUI.levelDepths[j] = 0;
			}
		}
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x0004BAC4 File Offset: 0x00049CC4
	public static void update(int updateLevel)
	{
		if (LTGUI.isGUIEnabled)
		{
			LTGUI.init();
			if (LTGUI.levelDepths[updateLevel] > 0)
			{
				LTGUI.color = GUI.color;
				int num = updateLevel * LTGUI.RECTS_PER_LEVEL;
				int num2 = num + LTGUI.levelDepths[updateLevel];
				for (int i = num; i < num2; i++)
				{
					LTGUI.r = LTGUI.levels[i];
					if (LTGUI.r != null)
					{
						if (LTGUI.r.useColor)
						{
							GUI.color = LTGUI.r.color;
						}
						if (LTGUI.r.type == LTGUI.Element_Type.Label)
						{
							if (LTGUI.r.style != null)
							{
								GUI.skin.label = LTGUI.r.style;
							}
							if (LTGUI.r.useSimpleScale)
							{
								GUI.Label(new Rect((LTGUI.r.rect.x + LTGUI.r.margin.x + LTGUI.r.relativeRect.x) * LTGUI.r.relativeRect.width, (LTGUI.r.rect.y + LTGUI.r.margin.y + LTGUI.r.relativeRect.y) * LTGUI.r.relativeRect.height, LTGUI.r.rect.width * LTGUI.r.relativeRect.width, LTGUI.r.rect.height * LTGUI.r.relativeRect.height), LTGUI.r.labelStr);
							}
							else
							{
								GUI.Label(new Rect(LTGUI.r.rect.x + LTGUI.r.margin.x, LTGUI.r.rect.y + LTGUI.r.margin.y, LTGUI.r.rect.width, LTGUI.r.rect.height), LTGUI.r.labelStr);
							}
						}
						else if (LTGUI.r.type == LTGUI.Element_Type.Texture && LTGUI.r.texture != null)
						{
							Vector2 vector = LTGUI.r.useSimpleScale ? new Vector2(0f, LTGUI.r.rect.height * LTGUI.r.relativeRect.height) : new Vector2(LTGUI.r.rect.width, LTGUI.r.rect.height);
							if (LTGUI.r.sizeByHeight)
							{
								vector.x = (float)LTGUI.r.texture.width / (float)LTGUI.r.texture.height * vector.y;
							}
							if (LTGUI.r.useSimpleScale)
							{
								GUI.DrawTexture(new Rect((LTGUI.r.rect.x + LTGUI.r.margin.x + LTGUI.r.relativeRect.x) * LTGUI.r.relativeRect.width, (LTGUI.r.rect.y + LTGUI.r.margin.y + LTGUI.r.relativeRect.y) * LTGUI.r.relativeRect.height, vector.x, vector.y), LTGUI.r.texture);
							}
							else
							{
								GUI.DrawTexture(new Rect(LTGUI.r.rect.x + LTGUI.r.margin.x, LTGUI.r.rect.y + LTGUI.r.margin.y, vector.x, vector.y), LTGUI.r.texture);
							}
						}
					}
				}
				GUI.color = LTGUI.color;
			}
		}
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x0004BED4 File Offset: 0x0004A0D4
	public static bool checkOnScreen(Rect rect)
	{
		bool flag = rect.x + rect.width < 0f;
		bool flag2 = rect.x > (float)Screen.width;
		bool flag3 = rect.y > (float)Screen.height;
		bool flag4 = rect.y + rect.height < 0f;
		return !flag && !flag2 && !flag3 && !flag4;
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x0004BF38 File Offset: 0x0004A138
	public static void destroy(int id)
	{
		int num = id & 65535;
		int num2 = id >> 16;
		if (id >= 0 && LTGUI.levels[num] != null && LTGUI.levels[num].hasInitiliazed && LTGUI.levels[num].counter == num2)
		{
			LTGUI.levels[num] = null;
		}
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x0004BF84 File Offset: 0x0004A184
	public static void destroyAll(int depth)
	{
		int num = depth * LTGUI.RECTS_PER_LEVEL + LTGUI.RECTS_PER_LEVEL;
		int num2 = depth * LTGUI.RECTS_PER_LEVEL;
		while (LTGUI.levels != null && num2 < num)
		{
			LTGUI.levels[num2] = null;
			num2++;
		}
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x00009459 File Offset: 0x00007659
	public static LTRect label(Rect rect, string label, int depth)
	{
		return LTGUI.label(new LTRect(rect), label, depth);
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x00009468 File Offset: 0x00007668
	public static LTRect label(LTRect rect, string label, int depth)
	{
		rect.type = LTGUI.Element_Type.Label;
		rect.labelStr = label;
		return LTGUI.element(rect, depth);
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x0000947F File Offset: 0x0000767F
	public static LTRect texture(Rect rect, Texture texture, int depth)
	{
		return LTGUI.texture(new LTRect(rect), texture, depth);
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x0000948E File Offset: 0x0000768E
	public static LTRect texture(LTRect rect, Texture texture, int depth)
	{
		rect.type = LTGUI.Element_Type.Texture;
		rect.texture = texture;
		return LTGUI.element(rect, depth);
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x0004BFC0 File Offset: 0x0004A1C0
	public static LTRect element(LTRect rect, int depth)
	{
		LTGUI.isGUIEnabled = true;
		LTGUI.init();
		int num = depth * LTGUI.RECTS_PER_LEVEL + LTGUI.RECTS_PER_LEVEL;
		int num2 = 0;
		if (rect != null)
		{
			LTGUI.destroy(rect.id);
		}
		if (rect.type == LTGUI.Element_Type.Label && rect.style != null && rect.style.normal.textColor.a <= 0f)
		{
			Debug.LogWarning("Your GUI normal color has an alpha of zero, and will not be rendered.");
		}
		if (rect.relativeRect.width == float.PositiveInfinity)
		{
			rect.relativeRect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		}
		for (int i = depth * LTGUI.RECTS_PER_LEVEL; i < num; i++)
		{
			LTGUI.r = LTGUI.levels[i];
			if (LTGUI.r == null)
			{
				LTGUI.r = rect;
				LTGUI.r.rotateEnabled = true;
				LTGUI.r.alphaEnabled = true;
				LTGUI.r.setId(i, LTGUI.global_counter);
				LTGUI.levels[i] = LTGUI.r;
				if (num2 >= LTGUI.levelDepths[depth])
				{
					LTGUI.levelDepths[depth] = num2 + 1;
				}
				LTGUI.global_counter++;
				return LTGUI.r;
			}
			num2++;
		}
		Debug.LogError("You ran out of GUI Element spaces");
		return null;
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x0004C0F4 File Offset: 0x0004A2F4
	public static bool hasNoOverlap(Rect rect, int depth)
	{
		LTGUI.initRectCheck();
		bool result = true;
		bool flag = false;
		for (int i = 0; i < LTGUI.buttonLevels.Length; i++)
		{
			if (LTGUI.buttonLevels[i] >= 0)
			{
				if (LTGUI.buttonLastFrame[i] + 1 < Time.frameCount)
				{
					LTGUI.buttonLevels[i] = -1;
				}
				else if (LTGUI.buttonLevels[i] > depth && LTGUI.pressedWithinRect(LTGUI.buttons[i]))
				{
					result = false;
				}
			}
			if (!flag && LTGUI.buttonLevels[i] < 0)
			{
				flag = true;
				LTGUI.buttonLevels[i] = depth;
				LTGUI.buttons[i] = rect;
				LTGUI.buttonLastFrame[i] = Time.frameCount;
			}
		}
		return result;
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x0004C190 File Offset: 0x0004A390
	public static bool pressedWithinRect(Rect rect)
	{
		Vector2 vector = LTGUI.firstTouch();
		if (vector.x < 0f)
		{
			return false;
		}
		float num = (float)Screen.height - vector.y;
		return vector.x > rect.x && vector.x < rect.x + rect.width && num > rect.y && num < rect.y + rect.height;
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x0004C204 File Offset: 0x0004A404
	public static bool checkWithinRect(Vector2 vec2, Rect rect)
	{
		vec2.y = (float)Screen.height - vec2.y;
		return vec2.x > rect.x && vec2.x < rect.x + rect.width && vec2.y > rect.y && vec2.y < rect.y + rect.height;
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x000094A5 File Offset: 0x000076A5
	public static Vector2 firstTouch()
	{
		if (Input.touchCount > 0)
		{
			return Input.touches[0].position;
		}
		if (Input.GetMouseButton(0))
		{
			return Input.mousePosition;
		}
		return new Vector2(float.NegativeInfinity, float.NegativeInfinity);
	}

	// Token: 0x04000652 RID: 1618
	public static int RECT_LEVELS = 5;

	// Token: 0x04000653 RID: 1619
	public static int RECTS_PER_LEVEL = 10;

	// Token: 0x04000654 RID: 1620
	public static int BUTTONS_MAX = 24;

	// Token: 0x04000655 RID: 1621
	private static LTRect[] levels;

	// Token: 0x04000656 RID: 1622
	private static int[] levelDepths;

	// Token: 0x04000657 RID: 1623
	private static Rect[] buttons;

	// Token: 0x04000658 RID: 1624
	private static int[] buttonLevels;

	// Token: 0x04000659 RID: 1625
	private static int[] buttonLastFrame;

	// Token: 0x0400065A RID: 1626
	private static LTRect r;

	// Token: 0x0400065B RID: 1627
	private static Color color = Color.white;

	// Token: 0x0400065C RID: 1628
	private static bool isGUIEnabled = false;

	// Token: 0x0400065D RID: 1629
	private static int global_counter = 0;

	// Token: 0x02000109 RID: 265
	public enum Element_Type
	{
		// Token: 0x0400065F RID: 1631
		Texture,
		// Token: 0x04000660 RID: 1632
		Label
	}
}
