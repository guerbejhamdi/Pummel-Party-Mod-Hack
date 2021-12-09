using System;

// Token: 0x02000097 RID: 151
public class RulesetListElement : RulesetElementDefinition
{
	// Token: 0x06000318 RID: 792 RVA: 0x00037E40 File Offset: 0x00036040
	public RulesetListElement(string label, int startIndex, string[] elements, object obj, RulesetListValueChanged OnValueChanged, RulesetElementStyle style, bool enabled = true, RulesetListAllowValueChange AllowValueChange = null)
	{
		this.text = label;
		this.elementType = RulesetElementType.List;
		this.enabled = enabled;
		this.curIndex = startIndex;
		this.elements = elements;
		this.style = style;
		this.OnValueChanged = OnValueChanged;
		this.AllowValueChange = AllowValueChange;
		this.obj = obj;
	}

	// Token: 0x0400032A RID: 810
	public int curIndex;

	// Token: 0x0400032B RID: 811
	public string[] elements;

	// Token: 0x0400032C RID: 812
	public RulesetListValueChanged OnValueChanged;

	// Token: 0x0400032D RID: 813
	public RulesetListAllowValueChange AllowValueChange;

	// Token: 0x0400032E RID: 814
	public object obj;
}
