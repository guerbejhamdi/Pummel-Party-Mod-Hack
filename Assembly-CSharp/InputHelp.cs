using System;

// Token: 0x02000531 RID: 1329
[Serializable]
public class InputHelp
{
	// Token: 0x060022FE RID: 8958 RVA: 0x00004023 File Offset: 0x00002223
	public InputHelp()
	{
	}

	// Token: 0x060022FF RID: 8959 RVA: 0x00019465 File Offset: 0x00017665
	public InputHelp(InputDetails[] keyboard)
	{
		this.seperateControllerActions = false;
		this.keyboard = keyboard;
		this.controller = null;
	}

	// Token: 0x06002300 RID: 8960 RVA: 0x00019482 File Offset: 0x00017682
	public InputHelp(InputDetails[] keyboard, InputDetails[] controller)
	{
		this.seperateControllerActions = true;
		this.keyboard = keyboard;
		this.controller = controller;
	}

	// Token: 0x040025E8 RID: 9704
	public InputDetails[] keyboard;

	// Token: 0x040025E9 RID: 9705
	public InputDetails[] controller;

	// Token: 0x040025EA RID: 9706
	public bool seperateControllerActions;
}
