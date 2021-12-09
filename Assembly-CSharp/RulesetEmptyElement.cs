using System;

// Token: 0x02000094 RID: 148
public class RulesetEmptyElement : RulesetElementDefinition
{
	// Token: 0x06000314 RID: 788 RVA: 0x0000595B File Offset: 0x00003B5B
	public RulesetEmptyElement(RulesetElementStyle style, bool enabled = true)
	{
		this.elementType = RulesetElementType.Empty;
		this.enabled = enabled;
		this.style = style;
	}
}
