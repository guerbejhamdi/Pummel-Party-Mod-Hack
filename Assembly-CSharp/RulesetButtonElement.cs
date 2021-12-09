using System;

// Token: 0x02000095 RID: 149
public class RulesetButtonElement : RulesetElementDefinition
{
	// Token: 0x06000315 RID: 789 RVA: 0x00037D80 File Offset: 0x00035F80
	public RulesetButtonElement(string label, RulesetEventDelegate OnButtonPressed, object obj, RulesetElementStyle style, bool enabled = true, int mappedActionID = -1)
	{
		this.text = label;
		this.elementType = RulesetElementType.Button;
		this.enabled = enabled;
		this.OnButtonPressed = OnButtonPressed;
		this.style = style;
		this.obj = obj;
		this.mappedActionID = mappedActionID;
	}

	// Token: 0x0400031F RID: 799
	public RulesetEventDelegate OnButtonPressed;

	// Token: 0x04000320 RID: 800
	public object obj;

	// Token: 0x04000321 RID: 801
	public int mappedActionID = -1;
}
