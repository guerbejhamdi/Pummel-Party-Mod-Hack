using System;

// Token: 0x02000093 RID: 147
public class RulesetLabelElement : RulesetElementDefinition
{
	// Token: 0x06000313 RID: 787 RVA: 0x00005937 File Offset: 0x00003B37
	public RulesetLabelElement(string label, RulesetElementStyle style, bool enabled = true)
	{
		this.text = label;
		this.elementType = RulesetElementType.Label;
		this.enabled = enabled;
		this.style = style;
	}
}
