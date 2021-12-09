using System;
using UnityEngine;

// Token: 0x02000106 RID: 262
[Serializable]
public class LTRect
{
	// Token: 0x06000785 RID: 1925 RVA: 0x0004B4DC File Offset: 0x000496DC
	public LTRect()
	{
		this.reset();
		this.rotateEnabled = (this.alphaEnabled = true);
		this._rect = new Rect(0f, 0f, 1f, 1f);
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x0004B560 File Offset: 0x00049760
	public LTRect(Rect rect)
	{
		this._rect = rect;
		this.reset();
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x0004B5BC File Offset: 0x000497BC
	public LTRect(float x, float y, float width, float height)
	{
		this._rect = new Rect(x, y, width, height);
		this.alpha = 1f;
		this.rotation = 0f;
		this.rotateEnabled = (this.alphaEnabled = false);
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x0004B644 File Offset: 0x00049844
	public LTRect(float x, float y, float width, float height, float alpha)
	{
		this._rect = new Rect(x, y, width, height);
		this.alpha = alpha;
		this.rotation = 0f;
		this.rotateEnabled = (this.alphaEnabled = false);
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x0004B6C8 File Offset: 0x000498C8
	public LTRect(float x, float y, float width, float height, float alpha, float rotation)
	{
		this._rect = new Rect(x, y, width, height);
		this.alpha = alpha;
		this.rotation = rotation;
		this.rotateEnabled = (this.alphaEnabled = false);
		if (rotation != 0f)
		{
			this.rotateEnabled = true;
			this.resetForRotation();
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x0600078A RID: 1930 RVA: 0x000092F2 File Offset: 0x000074F2
	public bool hasInitiliazed
	{
		get
		{
			return this._id != -1;
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x0600078B RID: 1931 RVA: 0x00009300 File Offset: 0x00007500
	public int id
	{
		get
		{
			return this._id | this.counter << 16;
		}
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00009312 File Offset: 0x00007512
	public void setId(int id, int counter)
	{
		this._id = id;
		this.counter = counter;
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x0004B760 File Offset: 0x00049960
	public void reset()
	{
		this.alpha = 1f;
		this.rotation = 0f;
		this.rotateEnabled = (this.alphaEnabled = false);
		this.margin = Vector2.zero;
		this.sizeByHeight = false;
		this.useColor = false;
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x0004B7AC File Offset: 0x000499AC
	public void resetForRotation()
	{
		Vector3 vector = new Vector3(GUI.matrix[0, 0], GUI.matrix[1, 1], GUI.matrix[2, 2]);
		if (this.pivot == Vector2.zero)
		{
			this.pivot = new Vector2((this._rect.x + this._rect.width * 0.5f) * vector.x + GUI.matrix[0, 3], (this._rect.y + this._rect.height * 0.5f) * vector.y + GUI.matrix[1, 3]);
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x0600078F RID: 1935 RVA: 0x00009322 File Offset: 0x00007522
	// (set) Token: 0x06000790 RID: 1936 RVA: 0x0000932F File Offset: 0x0000752F
	public float x
	{
		get
		{
			return this._rect.x;
		}
		set
		{
			this._rect.x = value;
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06000791 RID: 1937 RVA: 0x0000933D File Offset: 0x0000753D
	// (set) Token: 0x06000792 RID: 1938 RVA: 0x0000934A File Offset: 0x0000754A
	public float y
	{
		get
		{
			return this._rect.y;
		}
		set
		{
			this._rect.y = value;
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06000793 RID: 1939 RVA: 0x00009358 File Offset: 0x00007558
	// (set) Token: 0x06000794 RID: 1940 RVA: 0x00009365 File Offset: 0x00007565
	public float width
	{
		get
		{
			return this._rect.width;
		}
		set
		{
			this._rect.width = value;
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000795 RID: 1941 RVA: 0x00009373 File Offset: 0x00007573
	// (set) Token: 0x06000796 RID: 1942 RVA: 0x00009380 File Offset: 0x00007580
	public float height
	{
		get
		{
			return this._rect.height;
		}
		set
		{
			this._rect.height = value;
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06000797 RID: 1943 RVA: 0x0004B874 File Offset: 0x00049A74
	// (set) Token: 0x06000798 RID: 1944 RVA: 0x0000938E File Offset: 0x0000758E
	public Rect rect
	{
		get
		{
			if (LTRect.colorTouched)
			{
				LTRect.colorTouched = false;
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
			}
			if (this.rotateEnabled)
			{
				if (this.rotateFinished)
				{
					this.rotateFinished = false;
					this.rotateEnabled = false;
					this.pivot = Vector2.zero;
				}
				else
				{
					GUIUtility.RotateAroundPivot(this.rotation, this.pivot);
				}
			}
			if (this.alphaEnabled)
			{
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alpha);
				LTRect.colorTouched = true;
			}
			if (this.fontScaleToFit)
			{
				if (this.useSimpleScale)
				{
					this.style.fontSize = (int)(this._rect.height * this.relativeRect.height);
				}
				else
				{
					this.style.fontSize = (int)this._rect.height;
				}
			}
			return this._rect;
		}
		set
		{
			this._rect = value;
		}
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x00009397 File Offset: 0x00007597
	public LTRect setStyle(GUIStyle style)
	{
		this.style = style;
		return this;
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x000093A1 File Offset: 0x000075A1
	public LTRect setFontScaleToFit(bool fontScaleToFit)
	{
		this.fontScaleToFit = fontScaleToFit;
		return this;
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x000093AB File Offset: 0x000075AB
	public LTRect setColor(Color color)
	{
		this.color = color;
		this.useColor = true;
		return this;
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x000093BC File Offset: 0x000075BC
	public LTRect setAlpha(float alpha)
	{
		this.alpha = alpha;
		return this;
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x000093C6 File Offset: 0x000075C6
	public LTRect setLabel(string str)
	{
		this.labelStr = str;
		return this;
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x000093D0 File Offset: 0x000075D0
	public LTRect setUseSimpleScale(bool useSimpleScale, Rect relativeRect)
	{
		this.useSimpleScale = useSimpleScale;
		this.relativeRect = relativeRect;
		return this;
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x000093E1 File Offset: 0x000075E1
	public LTRect setUseSimpleScale(bool useSimpleScale)
	{
		this.useSimpleScale = useSimpleScale;
		this.relativeRect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		return this;
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x0000940C File Offset: 0x0000760C
	public LTRect setSizeByHeight(bool sizeByHeight)
	{
		this.sizeByHeight = sizeByHeight;
		return this;
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x0004B988 File Offset: 0x00049B88
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"x:",
			this._rect.x.ToString(),
			" y:",
			this._rect.y.ToString(),
			" width:",
			this._rect.width.ToString(),
			" height:",
			this._rect.height.ToString()
		});
	}

	// Token: 0x0400063B RID: 1595
	public Rect _rect;

	// Token: 0x0400063C RID: 1596
	public float alpha = 1f;

	// Token: 0x0400063D RID: 1597
	public float rotation;

	// Token: 0x0400063E RID: 1598
	public Vector2 pivot;

	// Token: 0x0400063F RID: 1599
	public Vector2 margin;

	// Token: 0x04000640 RID: 1600
	public Rect relativeRect = new Rect(0f, 0f, float.PositiveInfinity, float.PositiveInfinity);

	// Token: 0x04000641 RID: 1601
	public bool rotateEnabled;

	// Token: 0x04000642 RID: 1602
	[HideInInspector]
	public bool rotateFinished;

	// Token: 0x04000643 RID: 1603
	public bool alphaEnabled;

	// Token: 0x04000644 RID: 1604
	public string labelStr;

	// Token: 0x04000645 RID: 1605
	public LTGUI.Element_Type type;

	// Token: 0x04000646 RID: 1606
	public GUIStyle style;

	// Token: 0x04000647 RID: 1607
	public bool useColor;

	// Token: 0x04000648 RID: 1608
	public Color color = Color.white;

	// Token: 0x04000649 RID: 1609
	public bool fontScaleToFit;

	// Token: 0x0400064A RID: 1610
	public bool useSimpleScale;

	// Token: 0x0400064B RID: 1611
	public bool sizeByHeight;

	// Token: 0x0400064C RID: 1612
	public Texture texture;

	// Token: 0x0400064D RID: 1613
	private int _id = -1;

	// Token: 0x0400064E RID: 1614
	[HideInInspector]
	public int counter;

	// Token: 0x0400064F RID: 1615
	public static bool colorTouched;
}
