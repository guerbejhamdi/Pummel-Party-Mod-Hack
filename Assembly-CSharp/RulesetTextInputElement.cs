using System;

// Token: 0x02000092 RID: 146
public class RulesetTextInputElement : RulesetElementDefinition
{
	// Token: 0x06000312 RID: 786 RVA: 0x00005903 File Offset: 0x00003B03
	public RulesetTextInputElement(string text, RulesetStringEventDelegate OnTextChanged, object obj, RulesetElementStyle style, bool enabled = true)
	{
		this.text = text;
		this.elementType = RulesetElementType.TextInput;
		this.enabled = enabled;
		this.style = style;
		this.obj = obj;
		this.OnTextChanged = OnTextChanged;
	}

	// Token: 0x0400031D RID: 797
	public RulesetStringEventDelegate OnTextChanged;

	// Token: 0x0400031E RID: 798
	public object obj;
}
