using System;

// Token: 0x02000401 RID: 1025
public class PlayerAnimationEvent
{
	// Token: 0x06001C8F RID: 7311 RVA: 0x00014FEA File Offset: 0x000131EA
	public PlayerAnimationEvent(AnimationEventType type)
	{
		this.event_type = type;
	}

	// Token: 0x06001C90 RID: 7312 RVA: 0x00014FF9 File Offset: 0x000131F9
	public PlayerAnimationEvent(AnimationEventType type, int _foot_index)
	{
		this.event_type = type;
		this.foot_index = _foot_index;
	}

	// Token: 0x04001EB2 RID: 7858
	public AnimationEventType event_type;

	// Token: 0x04001EB3 RID: 7859
	public int foot_index;
}
