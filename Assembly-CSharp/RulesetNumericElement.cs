using System;

// Token: 0x02000096 RID: 150
public class RulesetNumericElement : RulesetElementDefinition
{
	// Token: 0x06000316 RID: 790 RVA: 0x00037DD0 File Offset: 0x00035FD0
	public RulesetNumericElement(string label, RulesetElementStyle style, float curValue, float min, float max, string format, object obj, RulesetNumericValueChanged OnValueChanged, float step = 1f, bool enabled = true, RulesetNumericAllowValueChange AllowValueChange = null)
	{
		this.text = label;
		this.elementType = RulesetElementType.Numeric;
		this.enabled = enabled;
		this.style = style;
		this.numericValue = curValue;
		this.numericMin = min;
		this.numericMax = max;
		this.numericStep = step;
		this.format = format;
		this.OnValueChanged = OnValueChanged;
		this.AllowValueChange = AllowValueChange;
		this.obj = obj;
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00005978 File Offset: 0x00003B78
	public string GetCurValue()
	{
		return this.numericValue.ToString(this.format);
	}

	// Token: 0x04000322 RID: 802
	public string format;

	// Token: 0x04000323 RID: 803
	public float numericValue;

	// Token: 0x04000324 RID: 804
	public float numericMin;

	// Token: 0x04000325 RID: 805
	public float numericMax;

	// Token: 0x04000326 RID: 806
	public float numericStep;

	// Token: 0x04000327 RID: 807
	public RulesetNumericValueChanged OnValueChanged;

	// Token: 0x04000328 RID: 808
	public RulesetNumericAllowValueChange AllowValueChange;

	// Token: 0x04000329 RID: 809
	public object obj;
}
