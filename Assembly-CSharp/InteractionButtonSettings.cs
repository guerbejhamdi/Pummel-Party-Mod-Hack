using System;

// Token: 0x02000500 RID: 1280
[Serializable]
public struct InteractionButtonSettings
{
	// Token: 0x0600218E RID: 8590 RVA: 0x00018531 File Offset: 0x00016731
	public InteractionButtonSettings(string choice, bool interactable, int cost)
	{
		this.choice = choice;
		this.interactable = interactable;
		this.cost = cost;
	}

	// Token: 0x0400244C RID: 9292
	public string choice;

	// Token: 0x0400244D RID: 9293
	public bool interactable;

	// Token: 0x0400244E RID: 9294
	public int cost;
}
